using System.Windows.Data;

namespace picThingy {
    public class SettingsBindingExtension : Binding {
        public SettingsBindingExtension () {
            Initialize ();
        }

        public SettingsBindingExtension (string path)
            : base (path) {
            Initialize ();
        }

        private void Initialize () {
            this.Source = Program.Options;
            this.Mode = BindingMode.TwoWay;
        }
    }
}
