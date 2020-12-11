using NUnit.Framework;
using TabataGenerator;
using TabataGeneratorTests.Helpers;

namespace TabataGeneratorTests
{
    public class WorkoutReaderTests
    {
        [TestCase("Samples/SampleFullObject.md")]
        [TestCase("Samples/SampleLazyObject.md")]
        public void ReadFullObjectMd(string fileName)
        {
            var (input, expected) =
                MarkdownHelper
                    .GetCodeBlocks(fileName)
                    .AsTupple2();
            var result = new WorkoutReader().Read(input);
            var resultCsharp = ObjectDumper.Dump(result, DumpStyle.CSharp).WindowsToUnixLineBreak();
            Assert.AreEqual(expected, resultCsharp);
        }
    }
}
