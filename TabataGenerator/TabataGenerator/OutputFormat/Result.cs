using System;

namespace TabataGenerator.OutputFormat
{
    [Serializable]
    public class Result
    {
        public Result(Workout workout)
        {
            this.workout = workout;
        }
        
        public Workout workout { get; }
        
        public int fileVersion => 1;
        public string packageName => "com.evgeniysharafan.tabatatimer";
        public int platform => 1;
        public int type => 1;
        public int versionCode => 502002;
        public string versionName => "5.2.2";
    }
}
