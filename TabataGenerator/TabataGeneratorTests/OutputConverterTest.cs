using System.Linq;
using NUnit.Framework;
using TabataGenerator;
using TabataGenerator.OutputFormat;
using TabataGeneratorTests.Helpers;

namespace TabataGeneratorTests
{
    [TestFixture]
    public class OutputConverterTest
    {
        [Test]
        public void TestResultGeneration_Sample1()
        {
            var intervals = ConvertSingle(
                @"
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
  - Burpees"
            );

            // Prepare
            Assert.AreEqual(IntervalType.Prepare, intervals.First().Type);
            Assert.AreEqual(30, intervals.First().Time);
            Assert.AreEqual(1, intervals.Count(i => i.Type == IntervalType.Prepare));

            // Work
            Assert.AreEqual(20, intervals.Count(i => i.Type == IntervalType.Work));
            Assert.AreEqual("Warmup\n[Ex. 1/4]\nSquat foot touch", intervals.First(i => i.Type == IntervalType.Work).Description);
            Assert.AreEqual("\n[Ex. 4/4 · Cycle 4/4]\nBurpees", intervals.Last(i => i.Type == IntervalType.Work).Description);

            //
            Assert.IsTrue(intervals.Where(i => i.Type == IntervalType.Work).All(i => i.Time == 15));

            SanityCheck(intervals);
        }

        [Test]
        public void TestResultGeneration_Sample2()
        {
            var intervals = ConvertSingle(
                @"
- Id: 1001
  Template: true
  Label: HIIT
  Warmup: 20s
  WarmupCycles: 2
  Cycles: 4
  Work: 15s
  Rest: 45s
  CoolDown: 5m

- Id: 101
  Label: 101 - Poids du corps 1 (new)
  Notes: https://90daylc.thibaultgeoffray.com/mes-routines/phase-1/routine-38
  TemplateId: 1001
  Exercises:
  - Squat foot touch
  - Montée de genou
  - Pompes en T
  - Burpees"
            );

            SanityCheck(intervals);
        }

        private static void SanityCheck(Interval[] intervals)
        {
            // Two exercises should not be together
            var sameTypeAndNeighbourgs = intervals
                .SelectTwoConsecutives()
                .Where(couple => couple.Item1.Type == couple.Item2.Type)
                .ToList();
            Assert.IsEmpty(sameTypeAndNeighbourgs);
            // Two exercises should not be together
            var restShouldNotBeNextToRecovery = intervals
                .SelectTwoConsecutives()
                .Where(ConsecutiveRests)
                .ToList();
            Assert.IsEmpty(restShouldNotBeNextToRecovery, "There should not be a two consecutive rests");
        }

        private static bool ConsecutiveRests((Interval, Interval) couple)
        {
            bool IsRest(IntervalType t) => t != IntervalType.Work;
            return IsRest(couple.Item1.Type) && IsRest(couple.Item2.Type);
        }

        private Interval[] ConvertSingle(string input)
        {
            var workoutDescription = new WorkoutReader()
                .Read(input)
                .Single();
            return new OutputConverter()
                .BuildResult(workoutDescription)
                .Workout
                .Intervals;
        }
    }
}
