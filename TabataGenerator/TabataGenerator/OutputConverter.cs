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
                new Workout(
                    id: workoutDescription.Id,
                    title: workoutDescription.Label,
                    intervals: intervals,
                    coolDown: workoutDescription.CoolDown,
                    work: workoutDescription.Work,
                    recovery: workoutDescription.Recovery,
                    rest: workoutDescription.Warmup,
                    warmup: workoutDescription.Warmup,
                    notes: workoutDescription.Notes
                )
            );
            return result;
        }

        private Interval[] GetIntervals(WorkoutDescription workout)
        {
            var result = new List<Interval>();
            AddIfNotEmpty(result, workout.Warmup, IntervalType.Prepare, null);
            AddCyclesAndExercises(workout, result, "Warmup", workout.WarmupCycles, skipLastRecovery: false);
            AddCyclesAndExercises(workout, result, string.Empty, workout.Cycles, skipLastRecovery: true);
            AddIfNotEmpty(result, workout.CoolDown, IntervalType.CoolDown, null);
            return result.ToArray();
        }

        private void AddCyclesAndExercises(WorkoutDescription workout, List<Interval> result, string prefix, int cyclesCount, bool skipLastRecovery) =>
            LinqHelper.ForEach(
                cyclesCount,
                (indexCycle, firstCycle, lastCycle) =>
                {
                    var isLastRecoveryAndShouldBeSkipped = skipLastRecovery && lastCycle;

                    LinqHelper.ForEach(
                        workout.Exercises,
                        (indexExercise, exercise, firstExercise, lastExercise) =>
                        {
                            AddExercise(
                                result,
                                workout.Work,
                                GetLabel(
                                    prefix,
                                    cyclesCount,
                                    indexCycle,
                                    indexExercise,
                                    exercise,
                                    workout.Exercises.Length
                                )
                            );

                            var shouldReplaceRecovery = workout.Recovery.IsEmpty && !isLastRecoveryAndShouldBeSkipped;
                            var shouldRest = !lastExercise || shouldReplaceRecovery;

                            AddIfNotEmpty(
                                enabled: shouldRest,
                                result,
                                workout.Rest,
                                IntervalType.Rest,
                                description: null
                            );
                        }
                    );

                    AddIfNotEmpty(enabled: !isLastRecoveryAndShouldBeSkipped, result, workout.Recovery, IntervalType.Recovery, null);
                }
            );

        private static string GetLabel(string prefix, int cyclesCount, int indexCycle, int indexExercise, string exercise, int exercisesCount)
        {
            IEnumerable<string> GetParts()
            {
                if (prefix != null)
                {
                    yield return prefix;
                }

                yield return "\n";
                yield return "[";

                yield return $"Ex. {indexExercise + 1}/{exercisesCount}";

                if (cyclesCount > 1)
                {
                    yield return $" · Cycle {indexCycle + 1}/{cyclesCount}";
                }

                yield return "]";

                yield return "\n";
                yield return exercise;
            }

            return GetParts().Concat();
        }

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
