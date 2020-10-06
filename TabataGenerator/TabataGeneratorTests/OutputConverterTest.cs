using System.Linq;
using NUnit.Framework;
using TabataGenerator;
using TabataGenerator.OutputFormat;

namespace TabataGeneratorTests
{
    [TestFixture]
    public class OutputConverterTest
    {
        [Test]
        public void METHOD()
        {
            var intervals = ConvertSingle(@"
- Id: 101
  Label: 101 - Poids du corps 1 (new)
  Warmup: 30s
  WarmupCycles: 1
  Cycles: 4
  Work: 15s
  Rest: 5s
  Recovery: 45s
  CoolDown: 7m30s
  Exercises:
  - Squat foot touch
  - Montée de genou
  - Pompes en T
  - Burpees");

            // Prepare
            Assert.AreEqual(IntervalType.Prepare, intervals.First().type);
            Assert.AreEqual(30, intervals.First().time);
            Assert.AreEqual(1, intervals.Count(i => i.type == IntervalType.Prepare));
            
            // Work
            Assert.AreEqual(20, intervals.Count(i => i.type == IntervalType.Work));
            Assert.AreEqual("Warmup [1/1·1/4]\nSquat foot touch", intervals.First(i => i.type == IntervalType.Work).description);
            Assert.AreEqual("[4/4·4/4]\nBurpees", intervals.Last(i => i.type == IntervalType.Work).description);
            Assert.IsTrue(intervals.Where(i => i.type == IntervalType.Work).All(i => i.time == 15));
        }

        private Interval[] ConvertSingle(string input)
        {
            var workoutDescription = new WorkoutReader()
                .Read(input)
                .Single();
            return new OutputConverter()
                .BuildResult(workoutDescription)
                .workout
                .intervals;
        }
    }
}
