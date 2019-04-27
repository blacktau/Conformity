using System;
using System.IO;
using System.Reflection;
using Xunit;

namespace Conformity.Tests
{
    public class JobFileParserTests
    {
        private const string ValidJobFileName = "ValidJobFile.json";
        private const string ValidJobFileNoSecondaryProcedures = "ValidJobFileNoSecondaryProcedures.json";

        [Fact]
        public void Constructor_Constructs()
        {
            var jobfileParser = new JobFileParser();
            Assert.NotNull(jobfileParser);
        }

        [Fact]
        public void LoadJobFile_GivenValidJobFilePath_ReturnsJob()
        {
            var parser = new JobFileParser();
            var testFile = GetTestJobFilePath(ValidJobFileName);

            var job = parser.LoadJobFile(testFile);

            Assert.NotNull(job);
        }

        [Fact]
        public void LoadJobFile_GivenValidJobFile_ReturnedJobHasConnectionString()
        {
            var parser = new JobFileParser();
            var testFile = GetTestJobFilePath(ValidJobFileName);

            var job = parser.LoadJobFile(testFile);

            Assert.False(string.IsNullOrWhiteSpace(job.ConnectionString));
        }

        [Fact]
        public void LoadJobFile_GivenValidJobFile_ReturnedJobHasPrimaryProcedure()
        {
            var parser = new JobFileParser();
            var testFile = GetTestJobFilePath(ValidJobFileName);

            var job = parser.LoadJobFile(testFile);

            Assert.False(string.IsNullOrWhiteSpace(job.PrimaryProcedure));
        }

        [Fact]
        public void LoadJobFile_GivenValidJobFile_ReturnedJobHasTemplateFile()
        {
            var parser = new JobFileParser();
            var testFile = GetTestJobFilePath(ValidJobFileName);

            var job = parser.LoadJobFile(testFile);

            Assert.False(string.IsNullOrWhiteSpace(job.TemplateFile));
        }

        [Fact]
        public void LoadJobFile_GivenValidJobFile_ReturnedJobHasPrimaryKey()
        {
            var parser = new JobFileParser();
            var testFile = GetTestJobFilePath(ValidJobFileName);

            var job = parser.LoadJobFile(testFile);

            Assert.False(string.IsNullOrWhiteSpace(job.PrimaryKey));
        }

        [Fact]
        public void LoadJobFile_GivenValidJobFile_ReturnedJobHasTemplateFileLocation()
        {
            var parser = new JobFileParser();
            var testFile = GetTestJobFilePath(ValidJobFileName);
            var testFileFolder = GetTestFileFolder();

            var job = parser.LoadJobFile(testFile);

            Assert.NotNull(job.TemplateFileLocation);
        }

        [Fact]
        public void LoadJobFile_GivenValidJobFileWithRelativeTemplateFile_ReturnedJobHasAbsoluteTemplateFileLocation()
        {
            var parser = new JobFileParser();
            var testFile = GetTestJobFilePath(ValidJobFileName);
            var testFileFolder = GetTestFileFolder();

            var job = parser.LoadJobFile(testFile);

            Assert.NotNull(job.TemplateFileLocation);
        }


        [Fact]
        public void LoadJobFile_GivenValidJobFile_ReturnedJobHasCorrectTemplateFileLocation()
        {
            var parser = new JobFileParser();
            var testFile = GetTestJobFilePath(ValidJobFileName);
            var testFileFolder = Path.Combine(GetTestFileFolder(), "templates");

            var job = parser.LoadJobFile(testFile);

            Assert.Equal(testFileFolder, job.TemplateFileLocation.FullName);
        }

        [Fact]
        public void LoadJobFile_GivenValidJobFileWithSecondaryProcedures_ReturnedJobHasSecondaryProcedures()
        {
            var parser = new JobFileParser();
            var testFile = GetTestJobFilePath(ValidJobFileName);

            var job = parser.LoadJobFile(testFile);

            Assert.NotEmpty(job.SecondaryProcedures);
        }

        [Fact]
        public void LoadJobFile_GivenValidJobFileWithSecondaryProcedures_SecondaryProceduresAreCorrect()
        {
            var parser = new JobFileParser();
            var testFile = GetTestJobFilePath(ValidJobFileName);

            var job = parser.LoadJobFile(testFile);

            Assert.Collection(job.SecondaryProcedures, item => {
                Assert.Equal("SupplierItems", item.Key);
                Assert.Equal("pr_GetSupplierItems", item.Value);
            });
        }

        [Fact]
        public void LoadJobFile_GivenValidJobFileWithNoSecondaryProcedures_SecondaryProceduresIsEmptyDictionary()
        {
            var parser = new JobFileParser();
            var testFile = GetTestJobFilePath(ValidJobFileNoSecondaryProcedures);

            var job = parser.LoadJobFile(testFile);

            Assert.NotNull(job.SecondaryProcedures);
            Assert.Empty(job.SecondaryProcedures);
        }

        private string GetTestJobFilePath(string jobFileName)
        {
            return Path.Combine(GetTestFileFolder(), jobFileName);
        }

        private string GetTestFileFolder()
        {
            return Path.Combine(Path.GetDirectoryName(this.GetType().Assembly.Location), "jobs");
        }
    }
}

