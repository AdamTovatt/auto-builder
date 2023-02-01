namespace AutoBuilder.Helpers
{
    public static class WorkingDirectory
    {
        public static string Default { get { return EnvironmentHelper.GetEnvironmentVariable("DEFAULT_WORKING_DIRECTORY"); } }
    }
}
