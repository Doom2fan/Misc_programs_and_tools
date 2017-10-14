using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace picThingy {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private string searchQuery { get; set; } = string.Empty;

        public MainWindow () {
            InitializeComponent ();
            ImageDataList.Images.LoadDatabase ();
            Thumbnails.LoadThumbs ();

            ImageDataList.Images.OnChange += ImageList_OnChange;
        }

        public void ImageList_OnChange (object sender, ImageDataChangeEventArgs e) {
            if (e.ChangeType != ImageDataChangeType.Loading && e.ChangeType != ImageDataChangeType.Clear && Program.Options.SaveOnChange) // Don't save if it just loaded the database, and don't save on clear.
                ImageDataList.Images.SaveDatabase ();
            if (Program.Options.RefreshOnChange)
                this.SearchGo ();
        }

        private void menuItemSettings_Click (object sender, RoutedEventArgs e) {
            var sw = new SettingsWindow ();
            sw.ShowDialog ();
        }

        private void menuItemExit_Click (object sender, RoutedEventArgs e) {
            Close ();
        }

        private void menuItemAddImage_Click (object sender, RoutedEventArgs e) {
            var aiw = new AddImageWindow ();
            aiw.ShowDialog ();
        }

        private void menuItemSaveDB_Click (object sender, RoutedEventArgs e) {
            ImageDataList.Images.SaveDatabase ();
            Thumbnails.SaveThumbs ();
        }

        private void SearchGo () {
            statusBarLabel.Content = "Searching images";
            string [] queryItems = searchQuery.Split (',');
            foreach (string query in queryItems) {
                if (query.StartsWith ("md5:", StringComparison.CurrentCultureIgnoreCase) && queryItems.Length > 1) {
                    MessageBox.Show ("Invalid search query string", "Invalid search", MessageBoxButton.OK, MessageBoxImage.Error);
                    statusBarLabel.Content = "Ready";
                    return;
                }
            }

            this.picList.ClearImages ();
            List<ImageData> images = new List<ImageData> ();
            if (String.IsNullOrWhiteSpace (this.searchQuery.Trim ())) {
                for (int i = 0; i < ImageDataList.Images.Count; i++)
                    images.Add (ImageDataList.Images [i]);
            } else {
                for (int i = 0; i < ImageDataList.Images.Count; i++) {
                    ImageData img = ImageDataList.Images [i];
                    bool add = true;
                    foreach (string query in queryItems) {
                        if ((query.StartsWith ("source:") && !img.sources.Contains (query.Remove (0, 7).Trim (), StringComparer.OrdinalIgnoreCase)) ||
                            (!img.tags.Contains (query.Trim (), StringComparer.OrdinalIgnoreCase))) {
                            add = false;
                            break;
                        }
                    }
                    if (add)
                        images.Add (img);
                }
            }

            statusBarLabel.Content = this.picList.AddImages (images.ToArray ()).ToString ();
        }

        private void buttonSearchGo_Click (object sender, RoutedEventArgs e) {
            this.searchQuery = this.textBoxSearchQuery.Text;
            this.SearchGo ();
        }
    }
}
