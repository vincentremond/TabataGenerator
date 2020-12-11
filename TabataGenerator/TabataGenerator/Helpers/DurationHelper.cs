using TabataGenerator.Input;

namespace TabataGenerator.Helpers
{
    public static class DurationHelper
    {
        public static bool IsEmpty(this Duration duration)
        {
            return duration.TotalSeconds == 0;
        }
    }
}
