using System;

namespace Conformity
{
    internal class JobValidator
    {
        public void ValidateJob(Job job)
        {
            ValidateStringField(job.ConnectionString, nameof(job.ConnectionString));
            ValidateStringField(job.PrimaryProcedure, nameof(job.PrimaryProcedure));
            ValidateStringField(job.PrimaryKey, nameof(job.PrimaryKey));
            ValidateStringField(job.TemplateFile, nameof(job.TemplateFile));

            ValidateSecondaryProcedures(job);
        }

        private void ValidateSecondaryProcedures(Job job)
        {
            if (job.SecondaryProcedures == null)
            {
                throw new InvalidJobException("SecondaryProcedures cannot be null");
            }

            foreach (var item in job.SecondaryProcedures)
            {
                if (string.IsNullOrWhiteSpace(item.Key))
                {
                    throw new InvalidJobException("SecondaryProcedures must have a Field Name specified");
                }

                if (string.IsNullOrWhiteSpace(item.Value))
                {
                    throw new InvalidJobException("SecondaryProcedures must have a Stored Procedure name specified");
                }
            }
        }

        private void ValidateStringField(string field, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(field))
            {
                throw new InvalidJobException($"{fieldName} is missing in job definition");
            }
        }
    }
}
