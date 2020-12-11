using System;
using Newtonsoft.Json;
using TabataGenerator.Input;

namespace TabataGenerator.OutputFormat
{
    [Serializable]
    public class Workout
    {
        public Workout(int id,
            string title,
            Interval[] intervals,
            Duration coolDown,
            Duration work,
            Duration recovery,
            Duration rest,
            Duration warmup,
            string notes)
        {
            Id = id;
            Title = title;
            Intervals = intervals;
            Notes = notes;

            CoolDown = coolDown.TotalSeconds;
            Work = work.TotalSeconds;
            Rest = rest.TotalSeconds;
            RestBetweenTabatas = recovery.TotalSeconds;
            Prepare = warmup.TotalSeconds;
        }

        public int ColorId => 2;
        public int CoolDown { get; }
        public int Cycles => 1;
        public bool DoNotRepeatFirstPrepareAndLastCoolDown => false;
        public int Id { get; }
        public Interval[] Intervals { get; }
        public int IntervalsSetsCount => 1;
        public bool IsFavorite => false;
        public bool IsRestRepsMode => false;
        public bool IsWorkRepsMode => false;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Notes { get; }

        public int Prepare { get; }
        public int Rest { get; }
        public int RestBetweenTabatas { get; }
        public int RestBetweenWorkoutsReps => 0;
        public bool RestBetweenWorkoutsRepsMode => false;
        public int RestBetweenWorkoutsTime => 0;
        public int RestBpm => 0;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RestDescription => null;

        public int RestReps => 0;
        public bool SkipLastRestInterval => true;
        public bool SkipPrepareAndCoolDownBetweenWorkouts => false;
        public int TabatasCount => 1;
        public string Title { get; }
        public int Work { get; }
        public int WorkBpm => 0;
        public int WorkReps => 0;
    }
}
