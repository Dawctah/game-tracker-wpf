using System.Security.Principal;

namespace Core.Data
{
    public static class FileNames
    {
#pragma warning disable CA1416 // Validate platform compatibility. This is for me on windows, it's fine.
        public static string Directory => "C:\\Users\\" + WindowsIdentity.GetCurrent().Name.Split('\\')[1] + "\\AppData\\Roaming\\GameTracker\\";
#pragma warning restore CA1416 // Validate platform compatibility


        public static string NewFileName => $"games{NewExtension}";
        public static string NewFullPath => Directory + NewFileName;
        public static string TestFileName => $"testing{NewExtension}";
        public static string TestFullPath => Directory + TestFileName;
        public static string OldFileName => "games.play";

        private static string NewExtension => ".ninetales";
    }
}
