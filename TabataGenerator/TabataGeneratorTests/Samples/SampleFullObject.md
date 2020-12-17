
``` yaml
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
  - Burpees
```

``` C#
var workoutDescriptionArray = new WorkoutDescription[]
{
  new WorkoutDescription
  {
    Id = 101,
    Template = false,
    TemplateId = 0,
    Favorite = false,
    Label = "Poids du corps 1 (new)",
    Notes = null,
    Warmup = new Duration
    {
      TotalSeconds = 30
    },
    WarmupCycles = 1,
    Cycles = 4,
    Work = new Duration
    {
      TotalSeconds = 15
    },
    Rest = new Duration
    {
      TotalSeconds = 5
    },
    Recovery = new Duration
    {
      TotalSeconds = 45
    },
    CoolDown = new Duration
    {
      TotalSeconds = 450
    },
    Settings = null,
    Exercises = new string[]
    {
      "Squat foot touch",
      "Montée de genou",
      "Pompes en T",
      "Burpees"
    }
  }
};
```
