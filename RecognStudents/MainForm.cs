using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AForge.WindowsForms
{
    public partial class MainForm : Form
    {
        private Controller controller;

        private FilterInfoCollection videoDevicesList;
        private IVideoSource videoSource;

        private readonly Stopwatch sw = new Stopwatch();

        private StudentNetwork studentNet;
        private BaseNetwork accordNet;
        private BaseNetwork activeNet;

        private bool studentReady = false;
        private bool accordReady = false;
        private bool trainingNow = false;

        private readonly object _frameLock = new object();
        private Bitmap _lastFrame;

        private SamplesSet _trainSet;
        private SamplesSet _testSet;


        private readonly int[] structure = { Symbols.SensorsCount, 200, 80, Symbols.ClassesCount };

        public MainForm()
        {
            InitializeComponent();

            controller = new Controller(UpdateFormFields);

            originalImageBox.SizeMode = PictureBoxSizeMode.Zoom;
            processedImgBox.SizeMode = PictureBoxSizeMode.Zoom;

            videoDevicesList = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo device in videoDevicesList)
                cmbVideoSource.Items.Add(device.Name);

            if (cmbVideoSource.Items.Count > 0)
            {
                cmbVideoSource.SelectedIndex = 0;
                cmbVideoSource_SelectionChangeCommitted(this, EventArgs.Empty);
            }
            else
            {
                MessageBox.Show("Камера не найдена");
            }

            studentNet = new StudentNetwork(structure) { LearningRate = 0.08 };
            studentNet.TrainProgress += Net_TrainProgress;

            accordNet = TryCreateAccordNet();
            if (accordNet != null)
                accordNet.TrainProgress += Net_TrainProgress;

            cmbNetwork.Items.Clear();
            cmbNetwork.Items.Add("Студенческая (StudentNetwork)");
            cmbNetwork.Items.Add("Библиотечная (AccordNet)");
            cmbNetwork.SelectedIndex = 0;
            activeNet = studentNet;

            txtResult.Text = "Это символ: (нет)";
            btnRecognize.Enabled = false;

            if (clbClasses != null) clbClasses.Visible = false;
            if (lblAug != null) lblAug.Visible = false;
            if (numAug != null) numAug.Visible = false;
            if (lblMaxPerClass != null) lblMaxPerClass.Visible = false;
            if (numMaxPerClass != null) numMaxPerClass.Visible = false;

            RefreshDatasetInfo();
            UpdateNetworkStatusText();
        }

        private void RefreshDatasetInfo()
        {
            try
            {
                var st = DataSetUtils.ScanDataset();

                string s = $"Всего изображений: {st.Total}\r\n";
                lblDatasetInfo.Text = s;
            }
            catch (Exception ex)
            {
                lblDatasetInfo.Text = "Dataset: (ошибка)\r\n" + ex.Message;
            }
        }

        private void btnScanDataset_Click(object sender, EventArgs e)
        {
            RefreshDatasetInfo();
        }

        private async void btnTrain_Click(object sender, EventArgs e)
        {
            if (trainingNow) return;
            trainingNow = true;

            btnTrain.Enabled = false;
            btnTest.Enabled = false;
            btnRecognize.Enabled = false;

            progressTrain.Value = 0;
            lblTrainStatus.Text = "Статус обучения: подготовка...";
            await Task.Run(() =>
            {
                DataSetUtils.BuildCacheIfMissing(msg =>
                {
                    BeginInvoke(new Action(() =>
                    {
                        lblTrainStatus.Text = "Статус обучения: " + msg;
                    }));
                });
            });


            try
            {
                int epochs = (int)numEpochs.Value;
                double trainFrac = (double)numTrainSplit.Value / 100.0;
                double acceptableError = (double)numAcceptError.Value;

                bool[] useClasses = new bool[Symbols.ClassesCount];
                for (int i = 0; i < useClasses.Length; i++) useClasses[i] = true;

                await Task.Run(() =>
                {
                    DataSetUtils.LoadSamplesSplit(
                        augPerImage: 0,     
                        useClasses: useClasses,
                        trainFraction: trainFrac,
                        maxBaseImagesPerClass: 0,
                        seed: Environment.TickCount,
                        out _trainSet,
                        out _testSet
                    );
                });

                if (cmbNetwork.SelectedIndex == 0)
                {
                    studentNet = new StudentNetwork(structure) { LearningRate = 0.08 };
                    studentNet.TrainProgress += Net_TrainProgress;
                    activeNet = studentNet;

                    await Task.Run(() =>
                    {
                        studentNet.TrainOnDataSet(
                            _trainSet,
                            epochsCount: epochs,
                            acceptableError: acceptableError,
                            parallel: false
                        );
                    });

                    studentReady = true;
                }
                else
                {
                    accordNet = TryCreateAccordNet();
                    if (accordNet == null)
                        throw new Exception("AccordNet недоступна: не хватает DLL (Accord.Neuro/Accord).");

                    accordNet.TrainProgress += Net_TrainProgress;
                    activeNet = accordNet;

                    await Task.Run(() =>
                    {
                        accordNet.TrainOnDataSet(
                            _trainSet,
                            epochsCount: epochs,
                            acceptableError: acceptableError,
                            parallel: false
                        );
                    });

                    accordReady = true;
                }

                double acc = 0.0;
                if (_testSet != null && _testSet.Count > 0)
                    acc = EvaluateAccuracy(activeNet, _testSet);

                lblTrainStatus.Text = $"Статус обучения: готово Тест: {acc * 100.0:0.0}%";
                UpdateNetworkStatusText();

                btnTest.Enabled = true;
                btnRecognize.Enabled = IsActiveNetReady();
            }
            catch (Exception ex)
            {
                lblTrainStatus.Text = "Статус обучения: ошибка!!!";
                MessageBox.Show(ex.ToString(), "Ошибка обучения");
                UpdateNetworkStatusText();
            }
            finally
            {
                btnTrain.Enabled = true;
                trainingNow = false;
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (!IsActiveNetReady())
            {
                MessageBox.Show("Выбранная сеть ещё не обучена.", "Тест");
                return;
            }

            if (_testSet == null || _testSet.Count == 0)
            {
                MessageBox.Show("Нет тестовой выборки.", "Тест");
                return;
            }

            double acc = EvaluateAccuracy(activeNet, _testSet);
            lblTrainStatus.Text = $"Тест: {acc * 100.0:0.0}%";
            MessageBox.Show($"Точность на тесте: {acc * 100.0:0.0}%", "Тест");
        }

        private void Net_TrainProgress(double progress, double error, TimeSpan time)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => Net_TrainProgress(progress, error, time)));
                return;
            }

            int val = (int)System.Math.Max(0, System.Math.Min(100, progress * 100.0));
            progressTrain.Value = val;
            lblTrainStatus.Text = $"Статус обучения: {val}% | ошибка {error:0.0000} | {time.TotalSeconds:0.0} c";
        }

        private static double EvaluateAccuracy(BaseNetwork net, SamplesSet set)
        {
            if (net == null || set == null || set.Count == 0) return 0.0;

            int ok = 0;
            for (int i = 0; i < set.Count; i++)
            {
                var s = set[i];
                var outp = net.ComputeRaw(s.input);

                int best = 0;
                double bestVal = outp[0];
                for (int k = 1; k < outp.Length; k++)
                {
                    if (outp[k] > bestVal)
                    {
                        bestVal = outp[k];
                        best = k;
                    }
                }

                if (best == (int)s.actualClass) ok++;
            }

            return (double)ok / set.Count;
        }

        private async void btnRecognize_Click(object sender, EventArgs e)
        {
            if (activeNet == null)
            {
                txtResult.Text = "Сеть не выбрана/не создана";
                return;
            }

            if (!IsActiveNetReady())
            {
                txtResult.Text = "Выбранная сеть ещё не обучена";
                return;
            }

            Bitmap frameToProcess = null;
            lock (_frameLock)
            {
                if (_lastFrame != null)
                    frameToProcess = (Bitmap)_lastFrame.Clone();
            }

            if (frameToProcess == null)
            {
                txtResult.Text = "Это символ: (нет кадра — нажми Старт)";
                return;
            }

            bool ok;
            try
            {
                ok = await controller.ProcessImage(frameToProcess);
            }
            catch (Exception ex)
            {
                txtResult.Text = "Ошибка обработки кадра";
                MessageBox.Show(ex.ToString(), "Ошибка обработки");
                return;
            }

            var sensors = controller.LastSensors;
            if (!ok || sensors == null)
            {
                txtResult.Text = "Попробуйте распознать еще раз";
                return;
            }

            double[] outp;
            try
            {
                outp = activeNet.ComputeRaw(sensors);
            }
            catch (Exception ex)
            {
                txtResult.Text = "Это символ: (ошибка сети)";
                MessageBox.Show(ex.ToString(), "Ошибка сети");
                return;
            }

            int best = 0;
            double bestVal = outp[0];
            for (int i = 1; i < outp.Length; i++)
            {
                if (outp[i] > bestVal)
                {
                    bestVal = outp[i];
                    best = i;
                }
            }

            txtResult.Text = $"Это символ: {Symbols.ClassNames[best]} (уверенность {bestVal:0.00})";
        }


        private BaseNetwork TryCreateAccordNet()
        {
            try { return new AccordNet(structure); }
            catch (FileNotFoundException) { return null; }
            catch { return null; }
        }

        private bool IsActiveNetReady()
        {
            if (cmbNetwork.SelectedIndex == 0) return studentReady;
            return accordReady;
        }

        private void UpdateNetworkStatusText()
        {
            if (cmbNetwork.SelectedIndex == 0)
            {
                statusLabel.Text = studentReady ? "Статус: StudentNetwork обучена" : "Статус: выбрана StudentNetwork";
            }
            else
            {
                if (accordNet == null)
                    statusLabel.Text = "Статус: AccordNet недоступна (нет DLL)";
                else
                    statusLabel.Text = accordReady ? "Статус: AccordNet обучена" : "Статус: выбрана AccordNet";
            }
        }

        private void cmbNetwork_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbNetwork.SelectedIndex == 0)
                {
                    activeNet = studentNet;
                    btnRecognize.Enabled = studentReady;
                    UpdateNetworkStatusText();
                    return;
                }

                // AccordNet выбрали
                if (accordNet == null)
                    accordNet = TryCreateAccordNet();

                if (accordNet == null)
                {
                    MessageBox.Show("AccordNet недоступна (ошибка создания или нет DLL). Остаёмся на StudentNetwork.");
                    cmbNetwork.SelectedIndex = 0;
                    activeNet = studentNet;
                    btnRecognize.Enabled = studentReady;
                    UpdateNetworkStatusText();
                    return;
                }

                // ВАЖНО: если AccordNet не обучена — не даём включить распознавание
                activeNet = accordNet;
                btnRecognize.Enabled = accordReady;
                if (!accordReady)
                    txtResult.Text = "AccordNet не обучена";

                UpdateNetworkStatusText();
            }
            catch (Exception ex)
            {
                // НИКОГДА не падаем при переключении — возвращаемся на StudentNetwork
                MessageBox.Show(ex.ToString(), "Ошибка при переключении сети");
                cmbNetwork.SelectedIndex = 0;
                activeNet = studentNet;
                btnRecognize.Enabled = studentReady;
                UpdateNetworkStatusText();
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (videoSource == null)
            {
                var vcd = new VideoCaptureDevice(videoDevicesList[cmbVideoSource.SelectedIndex].MonikerString);

                if (resolutionsBox.SelectedIndex >= 0 && vcd.VideoCapabilities.Length > 0)
                    vcd.VideoResolution = vcd.VideoCapabilities[resolutionsBox.SelectedIndex];

                videoSource = vcd;
                videoSource.NewFrame += video_NewFrame;
                videoSource.Start();

                StartButton.Text = "Стоп";
                cmbVideoSource.Enabled = false;
                resolutionsBox.Enabled = false;
            }
            else
            {
                videoSource.SignalToStop();
                videoSource = null;

                StartButton.Text = "Старт";
                cmbVideoSource.Enabled = true;
                resolutionsBox.Enabled = true;
            }
        }

        private void cmbVideoSource_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var vcd = new VideoCaptureDevice(videoDevicesList[cmbVideoSource.SelectedIndex].MonikerString);

            resolutionsBox.Items.Clear();
            foreach (var cap in vcd.VideoCapabilities)
                resolutionsBox.Items.Add(cap.FrameSize.ToString());

            if (resolutionsBox.Items.Count > 0)
                resolutionsBox.SelectedIndex = 0;
        }

        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap frame = null;

            try
            {
                frame = (Bitmap)eventArgs.Frame.Clone();

                lock (_frameLock)
                {
                    _lastFrame?.Dispose();
                    _lastFrame = (Bitmap)frame.Clone();
                }

                sw.Restart();

                if (controller.Ready)
                {
#pragma warning disable CS4014
                    controller.ProcessImage(frame);
#pragma warning restore CS4014
                    frame = null; 
                }
            }
            catch
            {
                // ничего — главное не падать
            }
            finally
            {
                frame?.Dispose();
            }
        }


        private void UpdateFormFields()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(UpdateFormFields));
                return;
            }

            sw.Stop();
            ticksLabel.Text = "Тики: " + sw.ElapsedMilliseconds + " мс";

            var oldOrig = originalImageBox.Image;
            originalImageBox.Image = controller.GetOriginalImage();
            oldOrig?.Dispose();

            var oldProc = processedImgBox.Image;
            processedImgBox.Image = controller.GetProcessedImage();
            oldProc?.Dispose();
        }

        private void tresholdTrackBar_ValueChanged(object sender, EventArgs e)
        {
            controller.settings.threshold = (byte)tresholdTrackBar.Value;
            controller.settings.differenceLim = (float)tresholdTrackBar.Value / tresholdTrackBar.Maximum;
        }

        private void borderTrackBar_ValueChanged(object sender, EventArgs e)
        {
            controller.settings.border = borderTrackBar.Value;
        }

        private void marginTrackBar_ValueChanged(object sender, EventArgs e)
        {
            controller.settings.margin = marginTrackBar.Value;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            controller.settings.processImg = checkBox1.Checked;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (videoSource != null && videoSource.IsRunning)
                    videoSource.SignalToStop();
            }
            catch { }
        }
    }
}
