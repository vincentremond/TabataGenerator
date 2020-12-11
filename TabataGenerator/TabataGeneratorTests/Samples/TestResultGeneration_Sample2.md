``` yaml
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
  Favorite: true
  Notes: |-
    https://example.com/test
  TemplateId: 1001
  Exercises:
  - Squat foot touch
  - Montée de genou
  - Pompes en T
  - Burpees
```

``` text
101 - Poids du corps 1 (new) ⭐
```

``` text
# Poids du corps 1 (new)

https://example.com/test

Exercises:
- Squat foot touch
- Montée de genou
- Pompes en T
- Burpees
```
