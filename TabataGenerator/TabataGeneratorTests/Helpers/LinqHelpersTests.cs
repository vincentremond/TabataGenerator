using System.Linq;
using NUnit.Framework;
using TabataGenerator.Helpers;

namespace TabataGeneratorTests.Helpers
{
    public class LinqHelpersTests
    {
        [Test]
        public void When_Passing_Multiple_Elements_Then_Lettuce_Should_Be_Sandwiched()
        {
            var input = new[]
            {
                1, 2, 3,
            };
            var lettuce = 0;
            var expected = new[]
            {
                1, lettuce, 2, lettuce, 3,
            };

            var result = input.Sandwich(lettuce);


            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void When_Passing_Single_Value_Then_No_Lettuce_Should_Be_Added()
        {
            var input = new[]
            {
                1,
            };
            var lettuce = 0;
            var expected = new[]
            {
                1,
            };

            var result = input.Sandwich(lettuce);

            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void When_Passing_Empty_Then_No_Lettuce_Should_Be_Added()
        {
            var input = Enumerable.Empty<int>();
            var lettuce = 0;
            var expected = Enumerable.Empty<int>();

            var result = input.Sandwich(lettuce);

            CollectionAssert.AreEqual(expected, result);
        }
    }
}
