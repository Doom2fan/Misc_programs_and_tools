/*using Newtonsoft.Json;
using System;
using System.IO;

namespace picThingyOLD {
    public class OptionsClass {
        #region Constants
        public static readonly OptionsClass Default = new OptionsClass ();
        private static readonly Type TypeConst = typeof (OptionsClass);
        #endregion
        #region Variables and properties
        // Variables
        private JsonSerializer serializer;
        // Properties
        public bool SaveOnChange = false;
        public bool RefreshOnChange = false;
        #endregion

        public OptionsClass () {
            serializer = new JsonSerializer ();
        }

        public void Save () {
            serializer.Serialize (new StreamWriter (Constants.ConfigFile), this, TypeConst);
        }

        public static OptionsClass Load () {
            var serializer = new JsonSerializer ();
            if (!File.Exists (Constants.ConfigFile)) {
                serializer.Serialize (new StreamWriter (Constants.ConfigFile), Default, TypeConst);
                return Default;
            }
            var stream = new StreamReader (Constants.ConfigFile);
            var reader = new JsonTextReader (stream);
            return serializer.Deserialize<OptionsClass> (reader);
            
        }
    }
}
*/