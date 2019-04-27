using Stubble.Core.Builders;

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Conformity
{
    internal class Template
    {
        private readonly string filePath;
        private string template;

        public Template(string filePath)
        {
            this.filePath = filePath;
        }

        internal async Task<string> ApplyAsync(Dictionary<string, object> row)
        {
            if (string.IsNullOrWhiteSpace(template))
            {
                template = ReadTemplate();
            }

            var stubble = new StubbleBuilder().Build();
            return await stubble.RenderAsync(template, row);
        }

        private string ReadTemplate()
        {
            return File.ReadAllText(filePath);
        }
    }
}
