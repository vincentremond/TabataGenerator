using DeepEqual.Syntax;
using NUnit.Framework;
using TabataGenerator;
using TabataGenerator.Input;

namespace TabataGeneratorTests
{
    public class WorkoutReaderTests
    {
        [Test]
        public void ReadFullObject()
        {
            Test(@"
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
  - Burpees", new[]
            {
                new WorkoutDescription(101,
                    "101 - Poids du corps 1 (new)",
                    warmup: new Duration(0, 30),
                    1,
                    cycles: 4,
                    work: new Duration(0, 15),
                    rest: new Duration(0, 5),
                    recovery: new Duration(0, 45),
                    coolDown: new Duration(7, 30),
                    new[]
                    {
                        "Squat foot touch",
                        "Montée de genou",
                        "Pompes en T",
                        "Burpees",
                    }
                ),
            });
        }

        [Test]
        public void ReadLazyObject()
        {
            Test(@"
- Label: 101 - Poids du corps 1 (new)
", new[]
            {
                new WorkoutDescription(0,
                    "101 - Poids du corps 1 (new)",
                    warmup: Duration.Empty,
                    0,
                    cycles: 1,
                    work: Duration.FromSeconds(30),
                    rest: Duration.FromSeconds(30),
                    recovery: Duration.Empty,
                    coolDown: Duration.Empty,
                    new[]
                    {
                        "(no label)",
                    }
                ),
            });
        }

        private static void Test(string content, WorkoutDescription[] expected)
        {
            var result = new WorkoutReader().Read(content);

            result.ShouldDeepEqual(expected);
        }
    }
}
