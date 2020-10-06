using System.Collections.Generic;
using System.Linq;
using TabataGenerator.Helpers;
using TabataGenerator.Input;
using TabataGenerator.OutputFormat;

namespace TabataGenerator
{
    public class OutputConverter
    {
        public Result BuildResult(WorkoutDescription workoutDescription)
        {
            var intervals = GetIntervals(workoutDescription).ToArray();
            var result = new Result(
                new Workout(workoutDescription.Id
                    , workoutDescription.Label
                    , workoutDescription.Cycles
                    , intervals
                    , workoutDescription.CoolDown
                    , workoutDescription.Work
                        , workoutDescription.Exercises.Length
                    , workoutDescription.Recovery
                    , workoutDescription.Rest
                        , workoutDescription.Warmup
                )
            );
            return result;
        }

        private Interval[] GetIntervals(WorkoutDescription workout)
        {
            var result = new List<Interval>();
            AddIfNotEmpty(result, workout.Warmup, IntervalType.Prepare, null);
            AddCyclesAndExercises(workout, result, "Warmup ", workout.WarmupCycles, skipLastRecovery: false);
            AddCyclesAndExercises(workout, result, string.Empty, workout.Cycles, skipLastRecovery: true);
            AddIfNotEmpty(result, workout.CoolDown, IntervalType.CoolDown, null);
            return result.ToArray();
        }

        private void AddCyclesAndExercises(WorkoutDescription workout, List<Interval> result, string prefix, int cyclesCount, bool skipLastRecovery) =>
            LinqHelper.ForEach(cyclesCount, (indexCycle, firstCycle, lastCycle) =>
            {
                LinqHelper.ForEach(workout.Exercises, (indexExercise, exercise, firstExercise, lastExercise) =>
                {
                    AddIfNotEmpty(enabled: !firstExercise, result, workout.Rest, IntervalType.Rest,
                        $"{prefix}[{indexCycle + 1}/{cyclesCount}·{indexExercise + 1}/{workout.Exercises.Length}] (next)\n{exercise}");
                    AddExercise(result, workout.Work,
                        $"{prefix}[{indexCycle + 1}/{cyclesCount}·{indexExercise + 1}/{workout.Exercises.Length}]\n{exercise}");
                });
                var shouldLastRecoveryBeSkipped = skipLastRecovery && lastCycle;
                AddIfNotEmpty(enabled: !shouldLastRecoveryBeSkipped, result, workout.Recovery, IntervalType.RestBetweenSets, null);
            });

        private void AddExercise(List<Interval> result, Duration duration, string description)
        {
            result.Add(new Interval(duration, IntervalType.Work, description));
        }

        private void AddIfNotEmpty(bool enabled, List<Interval> result, Duration duration, IntervalType intervalType, string description)
        {
            if (enabled)
            {
                AddIfNotEmpty(result, duration, intervalType, description);
            }
        }

        private void AddIfNotEmpty(List<Interval> result, Duration duration, IntervalType intervalType, string description)
        {
            if (duration.IsEmpty)
            {
                return;
            }

            result.Add(new Interval(duration, intervalType, description));
        }
    }
}
