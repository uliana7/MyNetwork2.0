using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace AForge.WindowsForms
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += ResolveMissingAssembly;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        private static Assembly ResolveMissingAssembly(object sender, ResolveEventArgs args)
        {
            try
            {
                string asmName = new AssemblyName(args.Name).Name + ".dll";
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;

                string[] probes = new[]
                {
                    baseDir,
                    Path.GetFullPath(Path.Combine(baseDir, "..")),
                    Path.GetFullPath(Path.Combine(baseDir, "..", "..")),
                    Path.Combine(baseDir, "AForge"),
                    Path.Combine(Path.GetFullPath(Path.Combine(baseDir, "..")), "AForge"),
                };

                foreach (var dir in probes)
                {
                    if (string.IsNullOrWhiteSpace(dir) || !Directory.Exists(dir)) continue;
                    string cand = Path.Combine(dir, asmName);
                    if (File.Exists(cand))
                        return Assembly.LoadFrom(cand);
                }
            }
            catch { }
            return null;
        }
    }
}
