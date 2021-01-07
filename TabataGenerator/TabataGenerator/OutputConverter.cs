using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using TabataGenerator.Helpers;
using TabataGenerator.Input;
using TabataGenerator.OutputFormat;

namespace TabataGenerator
{
    public class OutputConverter
    {
        public Workout BuildWorkout(WorkoutDescription workoutDescription)
        {
            var intervals = GetIntervals(workoutDescription).ToArray();
            return new Workout(
                id: workoutDescription.Id,
                title: GetFormattedTitle(workoutDescription),
                intervals: intervals,
                coolDown: workoutDescription.CoolDown,
                work: workoutDescription.Work,
                recovery: workoutDescription.Recovery,
                rest: workoutDescription.Warmup,
                warmup: workoutDescription.Warmup,
                settings: workoutDescription.Settings,
                notes: GetFormattedNotes(workoutDescription)
            );
        }

        private static string GetFormattedTitle(WorkoutDescription workoutDescription)
        {
            return workoutDescription.Favorite
                ? $"{workoutDescription.Id} - {workoutDescription.Label} ⭐"
                : $"{workoutDescription.Id} - {workoutDescription.Label}";
        }

        private static string GetFormattedNotes(WorkoutDescription workoutDescription)
        {
            static IEnumerable<string> FormattedNotes(WorkoutDescription d)
            {
                yield return $"# {d.Label}";

                if (!string.IsNullOrEmpty(d.Notes))
                {
                    yield return string.Empty;
                    yield return d.Notes;
                }

                if (d.Exercises.Any())
                {
                    yield return string.Empty;
                    yield return "Exercises:";
                    foreach (var e in d.Exercises)
                    {
                        yield return $"- {e}";
                    }
                }
            }

            return string.Join("\n", FormattedNotes(workoutDescription));
        }

        private Interval[] GetIntervals(WorkoutDescription workout)
        {
            var result = new List<Interval>();
            AddIfNotEmpty(result, workout.Warmup, IntervalType.Prepare, null);
            AddCyclesAndExercises(
                workout,
                result,
                "Warmup",
                workout.WarmupCycles,
                skipLastRecovery: false,
                additionalCyclesCount: workout.Cycles
            );
            AddCyclesAndExercises(
                workout,
                result,
                string.Empty,
                workout.Cycles,
                skipLastRecovery: true,
                additionalCyclesCount: null
            );
            AddIfNotEmpty(result, workout.CoolDown, IntervalType.CoolDown, null);
            return result.ToArray();
        }

        private void AddCyclesAndExercises(WorkoutDescription workout, List<Interval> result, string prefix, int cyclesCount, bool skipLastRecovery, int? additionalCyclesCount) =>
            LinqHelper.ForEach(
                cyclesCount,
                (indexCycle, _, lastCycle) =>
                {
                    var isLastRecoveryAndShouldBeSkipped = skipLastRecovery && lastCycle;

                    LinqHelper.ForEach(
                        workout.Exercises,
                        (indexExercise, exercise, _, lastExercise) =>
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
                                    workout.Exercises.Length,
                                    additionalCyclesCount
                                )
                            );

                            var shouldReplaceRecovery = workout.Recovery.IsEmpty() && !isLastRecoveryAndShouldBeSkipped;
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

        private static string GetLabel(string prefix, int cyclesCount, int indexCycle, int indexExercise, string exercise, int exercisesCount, int? additionalCyclesCount)
        {
            IEnumerable<string> GetParts()
            {
                if (prefix != null)
                {
                    yield return prefix;
                }

                yield return "\n";

                var showExercises = exercisesCount > 1;
                var showCycles = cyclesCount > 1 || additionalCyclesCount.HasValue;

                if (showExercises || showCycles)
                {
                    yield return "[";
                    if (showExercises)
                    {
                        yield return $"Ex. {indexExercise + 1}/{exercisesCount}";
                    }

                    if (showExercises && showCycles)
                    {
                        yield return " • ";
                    }

                    if (showCycles)
                    {
                        yield return $"Cycle {indexCycle + 1}/{cyclesCount}";
                        if (additionalCyclesCount.HasValue)
                        {
                            yield return $"+{additionalCyclesCount.Value}";
                        }
                    }

                    yield return "]";
                    yield return "\n";
                }

                yield return exercise;
            }

            var concat = GetParts().Concat();
            return concat;
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
            if (duration.IsEmpty())
            {
                return;
            }

            result.Add(new Interval(duration, intervalType, description));
        }
    }
}
