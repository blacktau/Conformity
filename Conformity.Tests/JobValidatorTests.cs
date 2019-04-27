using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Conformity.Tests
{
    public class JobValidatorTests
    {
        [Fact]
        public void Constructor_Constructor()
        {
            var validator = new JobValidator();
            Assert.NotNull(validator);
        }

        [Fact]
        public void ValidateJob_GivenJobMissingConnectionString_ThrowsInvalidJobException()
        {
            var validator = new JobValidator();
            var job = CreateTestJob();
            job.ConnectionString = null;

            Assert.Throws<InvalidJobException>(() => validator.ValidateJob(job));
        }

        [Fact]
        public void ValidateJob_GivenJobMissingPrimaryProcedure_ThrowsInvalidJobException()
        {
            var validator = new JobValidator();
            var job = CreateTestJob();
            job.PrimaryProcedure = null;

            Assert.Throws<InvalidJobException>(() => validator.ValidateJob(job));
        }

        [Fact]
        public void ValidateJob_GivenJobMissingTemplateFile_ThrowsInvalidJobException()
        {
            var validator = new JobValidator();
            var job = CreateTestJob();
            job.TemplateFile = null;

            Assert.Throws<InvalidJobException>(() => validator.ValidateJob(job));
        }

        [Fact]
        public void ValidateJob_GivenJobMissingPrimaryKey_ThrowsInvalidJobException()
        {
            var validator = new JobValidator();
            var job = CreateTestJob();
            job.PrimaryKey = null;

            Assert.Throws<InvalidJobException>(() => validator.ValidateJob(job));
        }

        [Fact]
        public void ValidateJob_GivenJobWithNullSecondaryProcedures_ThrowsInvalidJobException()
        {
            var validator = new JobValidator();
            var job = CreateTestJob();
            job.SecondaryProcedures = null;

            Assert.Throws<InvalidJobException>(() => validator.ValidateJob(job));
        }

        [Fact]
        public void ValidateJob_GivenJobWithSecondaryProceduresHavingEmptyKey_ThrowsInvalidJobException()
        {
            var validator = new JobValidator();
            var job = CreateTestJob();
            job.SecondaryProcedures = new Dictionary<string, string>
            {
                {  string.Empty, Guid.NewGuid().ToString() }
            };

            Assert.Throws<InvalidJobException>(() => validator.ValidateJob(job));
        }

        [Fact]
        public void ValidateJob_GivenJobWithSecondaryProceduresHavingNullValue_ThrowsInvalidJobException()
        {
            var validator = new JobValidator();
            var job = CreateTestJob();
            job.SecondaryProcedures = new Dictionary<string, string>
            {
                {  Guid.NewGuid().ToString(), string.Empty }
            };

            Assert.Throws<InvalidJobException>(() => validator.ValidateJob(job));
        }


        private Job CreateTestJob()
        {
            return new Job {
                ConnectionString = Guid.NewGuid().ToString(),
                PrimaryProcedure = Guid.NewGuid().ToString(),
                TemplateFile = Guid.NewGuid().ToString(),
                PrimaryKey = Guid.NewGuid().ToString(),
                SecondaryProcedures = new Dictionary<string, string>(),
                TemplateFileLocation = new DirectoryInfo(".")
            };
        }
    }
}
