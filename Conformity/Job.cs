using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace Conformity
{
    [DataContract]
    internal class Job
    {
        [DataMember]
        public string ConnectionString { get; set; }

        [DataMember]
        public string PrimaryProcedure { get; set; }

        [DataMember]
        public Dictionary<string, string> SecondaryProcedures { get; set; }

        [DataMember]
        public string TemplateFile { get; set; }

        [DataMember]
        public string PrimaryKey { get; set; }

        public DirectoryInfo TemplateFileLocation { get; set; }
    }
}
