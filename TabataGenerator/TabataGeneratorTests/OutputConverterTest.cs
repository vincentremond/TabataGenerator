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
            var workout = ConvertSingle(
                @"
- Id: 101
  Label: Poids du corps 1 (new)
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

            Assert.AreEqual("101 - Poids du corps 1 (new)", workout.Title);
            
            // Prepare
            Assert.AreEqual(IntervalType.Prepare, workout.Intervals.First().Type);
            Assert.AreEqual(30, workout.Intervals.First().Time);
            Assert.AreEqual(1, workout.Intervals.Count(i => i.Type == IntervalType.Prepare));

            // Work
            Assert.AreEqual(20, workout.Intervals.Count(i => i.Type == IntervalType.Work));
            Assert.AreEqual("Warmup\n[Ex. 1/4]\nSquat foot touch", workout.Intervals.First(i => i.Type == IntervalType.Work).Description);
            Assert.AreEqual("\n[Cycle 4/4 · Ex. 4/4]\nBurpees", workout.Intervals.Last(i => i.Type == IntervalType.Work).Description);

            //
            Assert.IsTrue(workout.Intervals.Where(i => i.Type == IntervalType.Work).All(i => i.Time == 15));

            SanityCheck(workout.Intervals);
        }

        [Test]
        public void TestResultGeneration_Sample2()
        {
            var workout = ConvertSingle(
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
  Label: Poids du corps 1 (new)
  Notes: https://90daylc.thibaultgeoffray.com/mes-routines/phase-1/routine-38
  TemplateId: 1001
  Exercises:
  - Squat foot touch
  - Montée de genou
  - Pompes en T
  - Burpees"
            );

            SanityCheck(workout.Intervals);
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

        private Workout ConvertSingle(string input)
        {
            var workoutDescription = new WorkoutReader()
                .Read(input)
                .Single();
            return new OutputConverter()
                .BuildResult(workoutDescription)
                .Workout;
        }
    }
}
