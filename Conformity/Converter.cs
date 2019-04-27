using System;
using System.Diagnostics;
using System.IO;

namespace Conformity
{
    internal static class Converter
    {
        private static string GetProcessName()
        {
            string subfolder = Environment.Is64BitOperatingSystem
                ? "x64"
                : "x86";

            return Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location), "wkhtml2pdf", subfolder, "wkhtmltopdf.exe");
        }

        public static void GeneratePdf(string sourceFile, string outFile)
        {
            var executablePath = GetProcessName();
            ExecuteCommand(executablePath, $@"""{sourceFile}"" ""{outFile}""");
        }

        private static string ExecuteCommand(string pathToExe, string args)
        {
            try
            {
                using (var proc = new Process())
                {
                    var procStartInfo = new ProcessStartInfo(pathToExe, args)
                    {
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                    };

                    proc.StartInfo = procStartInfo;

                    proc.Start();

                    proc.WaitForExit();
                    Console.Out.WriteLine(proc.StandardOutput.ReadToEnd());
                    Console.Error.WriteLine(proc.StandardError.ReadToEnd());
                }
            }
            catch(Exception e)
            {
                Console.Error.WriteLine($"Error converting: command {pathToExe} {args}");
                Console.Error.WriteLine(e);
            }

            return null;
        }

    }

}
