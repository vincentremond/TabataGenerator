namespace TabataGeneratorTests.Helpers
{
    public static class StringHelper
    {
        public static string WindowsToUnixLineBreak(this string input) => input.Replace("\r\n", "\n");
    }
}
