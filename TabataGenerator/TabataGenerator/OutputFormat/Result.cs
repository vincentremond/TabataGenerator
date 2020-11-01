using System;

namespace TabataGenerator.OutputFormat
{
    [Serializable]
    public class Result
    {
        public Result(Workout workout)
        {
            this.Workout = workout;
        }
        
        public Workout Workout { get; }
        
        public int FileVersion => 1;
        public string PackageName => "com.evgeniysharafan.tabatatimer";
        public int Platform => 1;
        public int Type => 1;
        public int VersionCode => 502002;
        public string VersionName => "5.2.2";
    }
}
