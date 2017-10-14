using System.IO;
using System.Reflection;

namespace picThingy {
    public static class Constants {
        public static readonly string ProgramPath = Path.GetDirectoryName (Assembly.GetExecutingAssembly ().Location);
        public static readonly string ConfigFile = Path.Combine (ProgramPath, "config.json");
        public const string DefaultDataFileName = "data.json";
        public static readonly string DefaultDataFile = Path.Combine (ProgramPath, DefaultDataFileName);
        public const string ThumbsFileName = "thumbs.json";
        public static readonly string DefaultThumbsPath = Path.Combine (ProgramPath, @"thumbs\");
    }
}
