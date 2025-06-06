using System.Collections.Generic;

namespace TabataGenerator.JsonModels;

public class Workout
{
    public Workout(int id, Interval[] intervals, string notes, IDictionary<string, string> settings, string title)
    {
        Id = id;
        Intervals = intervals;
        Notes = notes;
        Settings = settings;
        Title = title;
    }

    public int ColorId => 2;
    public int CoolDown => 0;
    public int Cycles => 1;
    public bool DoNotRepeatFirstPrepareAndLastCoolDown => false;
    public int Id { get; }
    public Interval[] Intervals { get; }
    public int IntervalsSetsCount => 1;
    public bool IsFavorite => false;
    public bool IsRestRepsMode => false;
    public bool IsWorkRepsMode => false;
    public string Notes { get; }
    public int Prepare => 10;
    public int Rest => 10;
    public int RestBetweenTabatas => 0;
    public int RestBetweenWorkoutsReps => 0;
    public bool RestBetweenWorkoutsRepsMode => false;
    public int RestBetweenWorkoutsTime => 0;
    public int RestBpm => 0;
    public string? RestDescription => null;
    public int RestReps => 0;
    public IDictionary<string, string> Settings { get; }
    public bool SkipLastRestInterval => true;
    public bool SkipPrepareAndCoolDownBetweenWorkouts => false;
    public int TabatasCount => 1;
    public string Title { get; }
    public int Work => 10;
    public int WorkBpm => 0;
    public int WorkReps => 0;
}