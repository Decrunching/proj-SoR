using System.IO;

namespace SoR
{
    public static class Globals
    {
        public static string GetPath(string name) => Path.Combine("../../../", name);
    }
}
