namespace TabataGenerator.JsonModels;

public class ResultListFile
{
    public ResultListFile(Workout[] workouts)
    {
        Workouts = workouts;
    }

    public object Settings => new();
    public object Statistics => new();
    public Workout[] Workouts { get; }
    public int FileVersion => 1;
    public string PackageName => "com.evgeniysharafan.tabatatimer";
    public int Platform = 1;
    public int Type => 3;
    public int VersionCode => 502002;
    public string VersionName => "5.2.2";
}
