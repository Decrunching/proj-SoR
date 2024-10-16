using System;
using System.IO;

namespace SoR
{
    public static class Globals
    {
        public static string GetResourcePath(string name) => Path.Combine("../../../", name);
        public static string GetSavePath(string name) => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), name);
    }
}