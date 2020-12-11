using System;
using System.Globalization;
using NUnit.Framework;
using TabataGenerator.Input;

namespace TabataGeneratorTests.HelpersTests
{
    public class TimespanHelperTests
    {
        [Test]
        [TestCase("30s", "00:00:30")]
        [TestCase("3m", "00:03:00")]
        [TestCase("1m15s", "00:01:15")]
        [TestCase(null, null)]
        public void ToTimeSpan(string input, string expectedAsString)
        {
            var expected = GetDuration(expectedAsString);
            var actual = (Duration)input;
            Assert.AreEqual(expected, actual);
        }

        private Duration GetDuration(string input)
        {
            if (input == null)
            {
                return Duration.Empty;
            }

            return TimeSpan.ParseExact(input, @"hh\:mm\:ss", CultureInfo.InvariantCulture);
        }
    }
}
