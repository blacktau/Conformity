using System;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("Conformity.Tests")]

namespace Conformity
{
    internal class Program
    {
        /// <summary>
        /// A command line application to convert an html template to a pdf files filling in the {namedVariables} from a stored procedures results
        /// </summary>
        /// <param name="jobFile">The path to the job definition file</param>
        /// <param name="outputFolder">The path to output the pdfiles to.</param>
        public static async Task<int> Main(string jobFile, string outputFolder)
        {
            var jobFileParser = new JobFileParser();
            var job = jobFileParser.LoadJobFile(jobFile);

            var dataSource = new DataSource(job.ConnectionString);

            var dataset = await dataSource.GetDataAsync(job);

            Console.WriteLine($"Processing {dataset.Count} rows");

            var template = new Template(job.TemplateFile);


            foreach (var row in dataset)
            {
                Console.Out.WriteLine($"Processing {row[job.PrimaryKey]}");

                string source = await template.ApplyAsync(row);
                var tempFile = GetTempFile(job.TemplateFileLocation);
                File.WriteAllText(tempFile, source);

                var fileName = row[job.PrimaryKey];
                var outputFile = Path.Combine(outputFolder, $"{fileName}.pdf");

                Converter.GeneratePdf(tempFile, outputFile);
                File.Delete(tempFile);
            }

            return 0;
        }

        private static string GetTempFile(DirectoryInfo templateFileLocation)
        {
            return Path.Combine(templateFileLocation.ToString(), $"{Guid.NewGuid().ToString()}.html");
        }
    }
}
