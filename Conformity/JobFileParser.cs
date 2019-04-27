using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;

namespace Conformity
{
    internal class JobFileParser
    {
        public Job LoadJobFile(string jobFilePath)
        {
            if (!File.Exists(jobFilePath))
            {
                throw new FileNotFoundException("Could not find Jobfile", jobFilePath);
            }

            using(var fileStream = File.OpenRead(jobFilePath))
            {
                var settings =  new DataContractJsonSerializerSettings
                {
                    UseSimpleDictionaryFormat = true
                };

                var serializer = new DataContractJsonSerializer(typeof(Job), settings);
                var job = (Job)serializer.ReadObject(fileStream);


                job.TemplateFile = GetFullTemplatePath(job.TemplateFile, jobFilePath);
                job.TemplateFileLocation = new DirectoryInfo(Path.GetDirectoryName(job.TemplateFile));

                job.SecondaryProcedures = job.SecondaryProcedures ?? new Dictionary<string, string>();

                return job;
            }
        }

        private string GetFullTemplatePath(string templateFile, string jobFilePath)
        {
            if (IsFullPath(templateFile))
            {
                return templateFile;
            }

            var jobFileLocation = Path.GetDirectoryName(jobFilePath);

            return Path.Combine(jobFileLocation, templateFile);
        }

        public static bool IsFullPath(string path)
        {
            return !string.IsNullOrWhiteSpace(path)
                && path.IndexOfAny(Path.GetInvalidPathChars()) == -1
                && Path.IsPathRooted(path)
                && !Path.GetPathRoot(path).Equals(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal);
        }
    }
}
