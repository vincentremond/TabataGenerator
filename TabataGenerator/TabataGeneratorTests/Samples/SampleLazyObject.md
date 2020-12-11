``` yaml
- Label: Poids du corps 1 (new)
  Work: 30s
  Rest: 30s
```

``` C#
var workoutDescriptionArray = new WorkoutDescription[]
{
  new WorkoutDescription
  {
    Id = 0,
    Template = false,
    TemplateId = 0,
    Favorite = false,
    Label = "Poids du corps 1 (new)",
    Notes = null,
    Warmup = new Duration
    {
      TotalSeconds = 0
    },
    WarmupCycles = 0,
    Cycles = 1,
    Work = new Duration
    {
      TotalSeconds = 30
    },
    Exercises = new string[]
    {
      "(no label)"
    }
  }
};
```
