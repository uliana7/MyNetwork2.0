using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace AForge.WindowsForms
{
    internal class Settings
    {
        public int width = 640;
        public int height = 480;

        public Size orignalDesiredSize = new Size(500, 500);
        public Size processedDesiredSize = new Size(500, 500);

        public bool processImg = false;

        public byte threshold = 120;
        public float differenceLim = 0.15f;

        public int border = 10;
        public int margin = 10;
    }

    internal class MagicEye
    {
        public Bitmap processed;
        public Bitmap original;

        public Settings settings = new Settings();

        public double[] lastSensors;

        public bool ProcessImage(Bitmap bitmap)
        {
            lastSensors = null;
            if (bitmap == null) return false;

            Bitmap resized = ResizeKeepAspect(bitmap, settings.orignalDesiredSize.Width, settings.orignalDesiredSize.Height);
            original = (Bitmap)resized.Clone();

            var u = AForge.Imaging.UnmanagedImage.FromManagedImage(resized);

            var gray = new AForge.Imaging.Filters.Grayscale(0.2125, 0.7154, 0.0721);
            u = gray.Apply(u);

            var bradley = new AForge.Imaging.Filters.BradleyLocalThresholding();
            bradley.PixelBrightnessDifferenceLimit = settings.differenceLim;
            bradley.ApplyInPlace(u);

            var inv = new AForge.Imaging.Filters.Invert();
            inv.ApplyInPlace(u);

            Rectangle symbolRect;
            if (!TryFindSymbolRect(u, out symbolRect))
            {
                processed = UpscaleForView(u.ToManagedImage(), settings.processedDesiredSize);
                resized.Dispose();
                return false;
            }

            var crop = new AForge.Imaging.Filters.Crop(symbolRect);
            u = crop.Apply(u);

            var scale = new AForge.Imaging.Filters.ResizeBilinear(Symbols.ImageW, Symbols.ImageH);
            u = scale.Apply(u);

            Bitmap bin200 = u.ToManagedImage();

            if (!IsValidInkAmount(bin200))
            {
                processed = UpscaleForView(bin200, settings.processedDesiredSize);
                bin200.Dispose();
                resized.Dispose();
                return false;
            }

            lastSensors = BuildFeatures400(bin200);

            processed = UpscaleForView(bin200, settings.processedDesiredSize);

            bin200.Dispose();
            resized.Dispose();

            return lastSensors != null;
        }

        private double[] BuildFeatures400(Bitmap bin200)
        {
            if (bin200 == null) return null;

            int w = bin200.Width;
            int h = bin200.Height;

            int N = 200;
            if (w != N || h != N)
                return null;

            double[] input = new double[400];

            var rect = new Rectangle(0, 0, w, h);
            var data = bin200.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            try
            {
                int stride = data.Stride;
                int bytes = stride * h;
                byte[] buf = new byte[bytes];
                System.Runtime.InteropServices.Marshal.Copy(data.Scan0, buf, 0, bytes);

                for (int y = 0; y < N; y++)
                {
                    int row = y * stride;
                    int rowSum = 0;

                    for (int x = 0; x < N; x++)
                    {
                        byte r = buf[row + x * 3 + 2];
                        if (r > 128)
                        {
                            rowSum++;
                            input[200 + x] += 1.0;
                        }
                    }

                    input[y] = rowSum;
                }
            }
            finally
            {
                bin200.UnlockBits(data);
            }

            double norm = 200.0;
            for (int i = 0; i < 400; i++)
                input[i] /= norm;

            return input;
        }

        private bool IsValidInkAmount(Bitmap bin200)
        {
            if (bin200 == null) return false;

            int w = bin200.Width;
            int h = bin200.Height;
            int total = w * h;
            int white = 0;

            var rect = new Rectangle(0, 0, w, h);
            var data = bin200.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            try
            {
                int stride = data.Stride;
                int bytes = stride * h;
                byte[] buf = new byte[bytes];
                System.Runtime.InteropServices.Marshal.Copy(data.Scan0, buf, 0, bytes);

                for (int y = 0; y < h; y++)
                {
                    int row = y * stride;
                    for (int x = 0; x < w; x++)
                    {
                        byte r = buf[row + x * 3 + 2];
                        if (r > 128) white++;
                    }
                }
            }
            finally
            {
                bin200.UnlockBits(data);
            }

            double ratio = white / (double)total;
            return ratio >= 0.005 && ratio <= 0.90;
        }

        private bool TryFindSymbolRect(AForge.Imaging.UnmanagedImage u, out Rectangle rect)
        {
            rect = Rectangle.Empty;

            var bc = new AForge.Imaging.BlobCounter
            {
                FilterBlobs = true,
                ObjectsOrder = AForge.Imaging.ObjectsOrder.Area
            };

            bc.MinWidth = 8;
            bc.MinHeight = 8;

            bc.ProcessImage(u);

            Rectangle[] rects = bc.GetObjectsRectangles();
            if (rects == null || rects.Length == 0)
                return false;

            for (int idx = 0; idx < rects.Length; idx++)
            {
                Rectangle r = rects[idx];

                double rw = r.Width / (double)u.Width;
                double rh = r.Height / (double)u.Height;

                if (rw > 0.92 && rh < 0.25) continue;
                if (rh > 0.92 && rw < 0.25) continue;

                if (rw > 0.90 || rh > 0.90) continue;
                if (rw < 0.05 && rh < 0.05) continue;

                double aspect = r.Width / (double)System.Math.Max(1, r.Height);
                if (aspect > 4.5 || aspect < 1.0 / 4.5) continue;

                int pad = System.Math.Max(2, (int)(System.Math.Min(r.Width, r.Height) * 0.08));
                int x = System.Math.Max(0, r.X - pad);
                int y = System.Math.Max(0, r.Y - pad);
                int w = System.Math.Min(u.Width - x, r.Width + 2 * pad);
                int h = System.Math.Min(u.Height - y, r.Height + 2 * pad);

                rect = new Rectangle(x, y, w, h);
                return true;
            }

            return false;
        }

        private static Bitmap ResizeKeepAspect(Bitmap src, int targetW, int targetH)
        {
            if (src.Width <= 0 || src.Height <= 0) return new Bitmap(targetW, targetH);

            float scale = System.Math.Min(targetW / (float)src.Width, targetH / (float)src.Height);
            int w = System.Math.Max(1, (int)(src.Width * scale));
            int h = System.Math.Max(1, (int)(src.Height * scale));

            Bitmap bmp = new Bitmap(targetW, targetH);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;

                int x = (targetW - w) / 2;
                int y = (targetH - h) / 2;

                g.DrawImage(src, x, y, w, h);
            }
            return bmp;
        }

        private static Bitmap UpscaleForView(Bitmap src, Size viewSize)
        {
            if (src == null) return null;

            Bitmap big = new Bitmap(viewSize.Width, viewSize.Height);
            using (Graphics g = Graphics.FromImage(big))
            {
                g.Clear(Color.Black);
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                g.DrawImage(src, 0, 0, viewSize.Width, viewSize.Height);
            }
            return big;
        }
    }

    public static class Symbols
    {
        public const int ImageW = 200;
        public const int ImageH = 200;

        public const int SensorsCount = 400;
        public const int ClassesCount = 6;

        public static readonly string[] FolderNames =
        {
            "0_NoWash",
            "1_Wash",
            "2_Circle",
            "3_CircleCross",
            "4_Triangle",
            "5_TriangleCross"
        };

        public static readonly string[] ClassNames =
        {
            "Не стирать",
            "Стирка",
            "Сушка",
            "Сушка запрещена",
            "Отбеливание",
            "Отбеливание запрещено"
        };
    }

    public static class DataSetUtils
    {
        private const string TrainFolderName = "DataWash";
        private const string CacheFolderName = "DataWashCache";
        private const string CacheFileName = "features.bin";

        private const int CACHE_VERSION = 1;
        private const int CACHE_MAGIC = unchecked((int)0x57415348);

        private static string GetTrainingRoot()
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory;

            for (int i = 0; i < 8; i++)
            {
                string cand = Path.Combine(dir, TrainFolderName);
                if (Directory.Exists(cand)) return cand;
                dir = Directory.GetParent(dir)?.FullName ?? dir;
            }

            return Path.Combine(Environment.CurrentDirectory, TrainFolderName);
        }

        private static string GetCacheRoot()
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory;

            for (int i = 0; i < 8; i++)
            {
                string cand = Path.Combine(dir, CacheFolderName);
                if (Directory.Exists(cand)) return cand;
                dir = Directory.GetParent(dir)?.FullName ?? dir;
            }

            return Path.Combine(Environment.CurrentDirectory, CacheFolderName);
        }

        private static bool IsImageFile(string f)
        {
            string ext = Path.GetExtension(f).ToLowerInvariant();
            return ext == ".png" || ext == ".jpg" || ext == ".jpeg" || ext == ".bmp";
        }

        public class DatasetStats
        {
            public string Root;
            public int[] Counts = new int[Symbols.ClassesCount];
            public int Total
            {
                get
                {
                    int s = 0;
                    for (int i = 0; i < Counts.Length; i++) s += Counts[i];
                    return s;
                }
            }
        }

        public static DatasetStats ScanDataset()
        {
            var st = new DatasetStats();
            st.Root = GetTrainingRoot();

            for (int cls = 0; cls < Symbols.ClassesCount; cls++)
            {
                string folder = Path.Combine(st.Root, Symbols.FolderNames[cls]);
                if (!Directory.Exists(folder))
                {
                    st.Counts[cls] = 0;
                    continue;
                }

                st.Counts[cls] = Directory.GetFiles(folder).Count(IsImageFile);
            }

            return st;
        }

        private static string GetCacheFilePath()
        {
            return Path.Combine(GetCacheRoot(), CacheFileName);
        }

        public static bool CacheExists()
        {
            return File.Exists(GetCacheFilePath());
        }

        public static void BuildCacheIfMissing(Action<string> report = null)
        {
            string trainRoot = GetTrainingRoot();
            if (!Directory.Exists(trainRoot))
                throw new DirectoryNotFoundException($"Не найдена папка {TrainFolderName}: {trainRoot}");

            string cacheRoot = GetCacheRoot();
            Directory.CreateDirectory(cacheRoot);

            string cacheFile = GetCacheFilePath();

            if (File.Exists(cacheFile))
            {
                try
                {
                    using (var fs = new FileStream(cacheFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                    using (var br = new BinaryReader(fs))
                    {
                        int magic = br.ReadInt32();
                        int ver = br.ReadInt32();
                        int sensors = br.ReadInt32();
                        int classes = br.ReadInt32();
                        int total = br.ReadInt32();

                        if (magic != CACHE_MAGIC) throw new InvalidDataException("bad magic");
                        if (ver != CACHE_VERSION) throw new InvalidDataException("bad version");
                        if (sensors != Symbols.SensorsCount) throw new InvalidDataException("bad sensors");
                        if (classes != Symbols.ClassesCount) throw new InvalidDataException("bad classes");
                        if (total <= 0) throw new InvalidDataException("bad count");

                        long expectedMin = 4 + 4 + 4 + 4 + 4 + (long)total * (1 + 4L * Symbols.SensorsCount);
                        if (fs.Length < expectedMin) throw new EndOfStreamException("cache truncated");
                    }

                    return;
                }
                catch
                {
                    try { File.Delete(cacheFile); } catch { }
                }
            }

            var all = new System.Collections.Generic.List<(string file, int cls)>();
            for (int cls = 0; cls < Symbols.ClassesCount; cls++)
            {
                string folder = Path.Combine(trainRoot, Symbols.FolderNames[cls]);
                if (!Directory.Exists(folder)) continue;

                foreach (var f in Directory.GetFiles(folder))
                    if (IsImageFile(f))
                        all.Add((f, cls));
            }

            report?.Invoke($"Подготовка (кэш): найдено файлов {all.Count}...");

            var eye = new MagicEye();
            eye.settings.processImg = false;

            string tmp = cacheFile + ".tmp";
            if (File.Exists(tmp))
            {
                try { File.Delete(tmp); } catch { }
            }

            int written = 0;

            using (var fs = new FileStream(tmp, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            using (var bw = new BinaryWriter(fs))
            {
                bw.Write(CACHE_MAGIC);
                bw.Write(CACHE_VERSION);
                bw.Write(Symbols.SensorsCount);
                bw.Write(Symbols.ClassesCount);

                long countPos = fs.Position;
                bw.Write(0); 

                for (int i = 0; i < all.Count; i++)
                {
                    var item = all[i];

                    try
                    {
                        using (var bmp = (Bitmap)Image.FromFile(item.file))
                        {
                            bool ok = eye.ProcessImage(bmp);
                            var s = eye.lastSensors;

                            if (!ok || s == null || s.Length != Symbols.SensorsCount)
                                continue;

                            bw.Write((byte)item.cls);
                            for (int k = 0; k < Symbols.SensorsCount; k++)
                                bw.Write((float)s[k]);

                            written++;
                        }
                    }
                    catch
                    {
                        // пропускаем плохие
                    }

                    if (i % 50 == 0)
                        report?.Invoke($"Подготовка (кэш): {i}/{all.Count}...");
                }

                long endPos = fs.Position;
                fs.Position = countPos;
                bw.Write(written);
                fs.Position = endPos;

                bw.Flush();
                fs.Flush(true);
            }

            if (File.Exists(cacheFile))
            {
                try { File.Delete(cacheFile); } catch { }
            }

            File.Move(tmp, cacheFile);

            report?.Invoke($"Подготовка (кэш): готово. Записано {written} примеров.");
        }

        private struct CachedSample
        {
            public int ClassId;
            public float[] X;
        }

        private static System.Collections.Generic.List<CachedSample> ReadCacheAllSafe()
        {
            string cacheFile = GetCacheFilePath();
            if (!File.Exists(cacheFile))
                throw new FileNotFoundException("Кэш не найден", cacheFile);

            var list = new System.Collections.Generic.List<CachedSample>();

            using (var fs = new FileStream(cacheFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var br = new BinaryReader(fs))
            {
                int magic = br.ReadInt32();
                int ver = br.ReadInt32();
                int sensors = br.ReadInt32();
                int classes = br.ReadInt32();
                int total = br.ReadInt32();

                if (magic != CACHE_MAGIC) throw new InvalidDataException("Неверный формат кэша (magic)");
                if (ver != CACHE_VERSION) throw new InvalidDataException("Неподдерживаемая версия кэша");
                if (sensors != Symbols.SensorsCount) throw new InvalidDataException("Кэш не совпадает по SensorsCount");
                if (classes != Symbols.ClassesCount) throw new InvalidDataException("Кэш не совпадает по ClassesCount");
                if (total <= 0) throw new InvalidDataException("Кэш пустой или повреждён (count)");

                for (int i = 0; i < total; i++)
                {
                    int cls = br.ReadByte();
                    var x = new float[Symbols.SensorsCount];
                    for (int k = 0; k < Symbols.SensorsCount; k++)
                        x[k] = br.ReadSingle();

                    list.Add(new CachedSample { ClassId = cls, X = x });
                }
            }

            return list;
        }

        private static void Shuffle<T>(System.Collections.Generic.IList<T> a, Random rnd)
        {
            for (int i = a.Count - 1; i > 0; i--)
            {
                int j = rnd.Next(i + 1);
                T tmp = a[i];
                a[i] = a[j];
                a[j] = tmp;
            }
        }

        public static void LoadSamplesSplit(
            int augPerImage,
            bool[] useClasses,
            double trainFraction,
            int maxBaseImagesPerClass,
            int seed,
            out SamplesSet trainSet,
            out SamplesSet testSet)
        {
            trainSet = new SamplesSet();
            testSet = new SamplesSet();

            if (useClasses == null || useClasses.Length != Symbols.ClassesCount)
            {
                useClasses = new bool[Symbols.ClassesCount];
                for (int i = 0; i < useClasses.Length; i++) useClasses[i] = true;
            }

            if (trainFraction <= 0) trainFraction = 0.8;
            if (trainFraction >= 1) trainFraction = 0.99;

            try
            {
                if (!CacheExists())
                    BuildCacheIfMissing();
            }
            catch
            {
                try { File.Delete(GetCacheFilePath()); } catch { }
                BuildCacheIfMissing();
            }

            System.Collections.Generic.List<CachedSample> all;
            try
            {
                all = ReadCacheAllSafe();
            }
            catch
            {
                // кэш плохой -> пересобираем и читаем заново
                try { File.Delete(GetCacheFilePath()); } catch { }
                BuildCacheIfMissing();
                all = ReadCacheAllSafe();
            }

            var filtered = new System.Collections.Generic.List<CachedSample>(all.Count);
            for (int i = 0; i < all.Count; i++)
            {
                int cls = all[i].ClassId;
                if (cls >= 0 && cls < Symbols.ClassesCount && useClasses[cls])
                    filtered.Add(all[i]);
            }

            var rnd = new Random(seed);

            for (int cls = 0; cls < Symbols.ClassesCount; cls++)
            {
                if (!useClasses[cls]) continue;

                var perClass = new System.Collections.Generic.List<CachedSample>();
                for (int i = 0; i < filtered.Count; i++)
                    if (filtered[i].ClassId == cls)
                        perClass.Add(filtered[i]);

                if (perClass.Count == 0) continue;

                Shuffle(perClass, rnd);

                if (maxBaseImagesPerClass > 0 && perClass.Count > maxBaseImagesPerClass)
                    perClass = perClass.GetRange(0, maxBaseImagesPerClass);

                int splitIndex = (int)System.Math.Round(perClass.Count * trainFraction);
                if (splitIndex < 1) splitIndex = 1;
                if (splitIndex > perClass.Count - 1) splitIndex = System.Math.Max(1, perClass.Count - 1);

                for (int i = 0; i < perClass.Count; i++)
                {
                    var cs = perClass[i];

                    double[] x = new double[Symbols.SensorsCount];
                    for (int k = 0; k < Symbols.SensorsCount; k++)
                        x[k] = cs.X[k];

                    var sample = new Sample(x, (FigureType)cls);

                    if (i < splitIndex) trainSet.Add(sample);
                    else testSet.Add(sample);
                }
            }
        }
    }
}
