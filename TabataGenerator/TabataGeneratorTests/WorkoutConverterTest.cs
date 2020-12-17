using System.Linq;
using NUnit.Framework;
using TabataGenerator;
using TabataGeneratorTests.Helpers;

namespace TabataGeneratorTests
{
    public class WorkoutConverterTest
    {
        [TestCase("Samples/SampleWithSettings.md")]
        public void TestFullConversion(string fileName)
        {
            var (input, expectedFilename, expectedContent) =
                MarkdownHelper
                    .GetCodeBlocks(fileName)
                    .AsTupple3();
            var result =
                WorkoutConverter
                    .GenerateWorkoutFiles(input)
                    .First();

            Assert.AreEqual(expectedFilename, result.FileName);
            Assert.AreEqual(expectedContent, result.Contents.WindowsToUnixLineBreak());
        }
    }
}
