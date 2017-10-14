using System.IO;
using System.Windows.Forms;

namespace picThingyOLD {
    public static class Constants {
        public static readonly string ProgramPath = Path.GetDirectoryName (Application.ExecutablePath);
        public static readonly string ConfigFile = Path.Combine (ProgramPath, "config.json");
        public static readonly string DefaultDataFile = Path.Combine (ProgramPath, "data.json");
    }
}
