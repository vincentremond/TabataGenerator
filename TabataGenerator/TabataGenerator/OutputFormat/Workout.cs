using Newtonsoft.Json;
using TabataGenerator.Input;

namespace TabataGenerator.OutputFormat
{
    public class Workout
    {
        public Workout(int id,
            string title,
            int intervalsSetsCount,
            Interval[] intervals,
            Duration coolDown,
            Duration work,
            int exercisesCount,
            Duration recovery,
            Duration rest,
            Duration warmup
        )
        {
            this.id = id;
            this.title = title;
            this.intervals = intervals;
            this.intervalsSetsCount = intervalsSetsCount;
            this.coolDown = coolDown.TotalSeconds;
            this.work = work.TotalSeconds;
            this.rest = rest.TotalSeconds;
            tabatasCount = exercisesCount;
            restBetweenTabatas = recovery.TotalSeconds;
            prepare = warmup.TotalSeconds;
        }

        public int colorId => 2;
        public int coolDown { get; }
        public int cycles => 1;
        public bool doNotRepeatFirstPrepareAndLastCoolDown => false;
        public int id { get; }
        public Interval[] intervals { get; }
        public int intervalsSetsCount { get; }
        public bool isFavorite => false;
        public bool isRestRepsMode => false;
        public bool isWorkRepsMode => false;
        public int prepare { get; }
        public int rest { get; }
        public int restBetweenTabatas { get; }
        public int restBetweenWorkoutsReps => 0;
        public bool restBetweenWorkoutsRepsMode => false;
        public int restBetweenWorkoutsTime => 0;
        public int restBpm => 0;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string restDescription => null;

        public int restReps => 0;
        public bool skipLastRestInterval => true;
        public bool skipPrepareAndCoolDownBetweenWorkouts => false;
        public int tabatasCount { get; }
        public string title { get; }
        public int work { get; }
        public int workBpm => 0;
        public int workReps => 0;
    }
}
