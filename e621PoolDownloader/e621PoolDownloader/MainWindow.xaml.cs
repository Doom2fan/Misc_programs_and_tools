using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace e621PoolDownloader {
    struct e621ImageData {
        public int Number { get; set; }
        public string URL { get; set; }
        public string Filename { get; set; }
        public string Extension { get; set; }
    }

    class DownloaderThreadToken {
        /// <summary>
        /// A list of image links. (int number, string link, string extension)
        /// </summary>
        public List<e621ImageData> ImageLinks { get; }
        public DownloadDataToken DownloadDataToken { get; set; }

        public DownloaderThreadToken (DownloadDataToken token) {
            DownloadDataToken = token;
            ImageLinks = new List<e621ImageData> ();
        }
    }

    class DownloadDataToken {
        public string FilenameMask { get; set; }
        public string PoolID { get; set; }
        public string DownloadPath { get; set; }
        public string PoolName { get; set; }
        public int DownloadCount { get; set; }
        public int ImageCount { get; set; }

        public string OperationText { get; set; }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        #region Constants

        /// <summary>
        /// The number of worker threads to use.
        /// </summary>
        const int ThreadCount = 8;

        /// <summary>
        /// The URL for the pool API.
        /// </summary>
        const string PoolAPIURL = "https://e621.net/pool/show.json";

        /// <summary>
        /// Required headers for sending API requests and downloading images.
        /// </summary>
        static readonly string [] headersStr = {
            "User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64; rv:25.8) Gecko/20151126 Firefox/31.9",
            "Accept: text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8",
            "Accept-Language: en-gb,en-us;q=0.7,en;q=0.3",
            "Accept-Encoding: identity",
        };

        #endregion

        #region Variables

        WebClient client = new WebClient ();
        DownloadDataToken token;

        #endregion

        #region Constructors

        public MainWindow () {
            InitializeComponent ();

            var filenameMaskBinding = new Binding ();
            filenameMaskBinding.Source = Properties.Settings.Default;
            filenameMaskBinding.Path = new PropertyPath ("FilenameMask");
            filenameMaskBinding.Mode = BindingMode.TwoWay;
            filenameMaskBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            BindingOperations.SetBinding (textBoxFilenameMask, TextBox.TextProperty, filenameMaskBinding);

            client.DownloadStringCompleted += DataClient_DownloadStringCompleted;
            client.DownloadProgressChanged += DataClient_DownloadProgressChanged;
        }

        #endregion

        #region Methods

        #region Control callbacks

        private void Window_Closing (object sender, System.ComponentModel.CancelEventArgs e) {
            Properties.Settings.Default.Save ();
        }

        private void TextBoxFilenameMask_TextChanged (object sender, TextChangedEventArgs e) {
            if (!CheckAccess ())
                return;

            string str = Utils.ParseFilenameMask (textBoxFilenameMask.Text, "1c8f24072648aeecd484083fd3f08030", "jpg", 0);
            SetFilenamePreviewLabel (String.Format ("Preview: {0}", str));
        }

        protected void SetFilenamePreviewLabel (string text) {
            if (!Dispatcher.CheckAccess ()) {
                Dispatcher.Invoke (new Action<string> (SetFilenamePreviewLabel));
                return;
            }

            labelPoolFilenamePreview.Content = text;
        }

        private void ButtonDownload_Click (object sender, RoutedEventArgs e) {
            if (String.IsNullOrWhiteSpace (textBoxPoolID.Text)) {
                labelStatus.Text = "You must specify a pool ID";
                return;
            } else if (String.IsNullOrWhiteSpace (textBoxDownloadPath.Text)) {
                labelStatus.Text = "You must specify a path to download the pool in";
                return;
            } else if (String.IsNullOrWhiteSpace (textBoxFilenameMask.Text)) {
                labelStatus.Text = "You must specify a filename mask";
                return;
            }

            string downPath;
            try {
                downPath = Path.GetFullPath (textBoxDownloadPath.Text);
                string tmpPath = Path.Combine (downPath, Utils.ParseFilenameMask (textBoxFilenameMask.Text, "1c8f24072648aeecd484083fd3f08030", "jpg", 0));

                Path.GetFullPath (downPath);
                Path.GetFullPath (tmpPath);
            } catch (Exception) {
                labelStatus.Text = "Invalid path";
                return;
            }

            DownloadPool (textBoxPoolID.Text, downPath);
        }

        private void ButtonBrowse_Click (object sender, RoutedEventArgs e) {
            var folderBrowser = new WPFFolderBrowser.WPFFolderBrowserDialog ("Select a folder to save the pool's images in");
            if (Directory.Exists (textBoxDownloadPath.Text))
                folderBrowser.InitialDirectory = textBoxDownloadPath.Text;
            else
                folderBrowser.InitialDirectory = Path.GetDirectoryName (System.Reflection.Assembly.GetEntryAssembly ().Location);

            if (folderBrowser.ShowDialog () == true)
                Properties.Settings.Default.DownloadPath = folderBrowser.FileName;
        }

        #endregion

        #region Download code

        /// <summary>
        /// Sets the required headers for a WebClient.
        /// </summary>
        /// <param name="hdrs">The WebHeaderCollection instance to insert the headers into.</param>
        protected void SetHeaders (WebHeaderCollection hdrs) {
            foreach (string header in headersStr)
                hdrs.Add (header);
        }

        /// <summary>
        /// Downloads the specified pool to the specified folder.
        /// </summary>
        /// <param name="poolID">The ID of the pool to download.</param>
        /// <param name="downloadPath">The folder to download the pool to.</param>
        protected void DownloadPool (string poolID, string downloadPath) {
            token = new DownloadDataToken ();
            token.PoolID = poolID;
            token.DownloadPath = downloadPath;
            token.FilenameMask = textBoxFilenameMask.Text;

            gridControls.IsEnabled = false;
            progressBarStatus.IsIndeterminate = false;
            labelStatus.Text = token.OperationText = "Downloading pool data";

            client.QueryString.Clear ();
            client.QueryString.Add ("id", poolID);
            SetHeaders (client.Headers);

            client.DownloadStringAsync (new Uri (PoolAPIURL), token);
        }

        /// <summary>
        /// Builds the pool data and starts the image-downloading threads
        /// </summary>
        private void DataClient_DownloadStringCompleted (object sender, DownloadStringCompletedEventArgs e) {
            var dataToken = (DownloadDataToken) e.UserState;

            labelStatus.Text = "Building image list";

            using (JsonReader reader = new JsonTextReader (new StringReader (e.Result))) {
                JsonSerializer serializer = new JsonSerializer ();
                var json = (JObject) serializer.Deserialize (reader);
                JToken t = null;

                if (json.TryGetValue ("success", out t) || t != null && t.Value<bool> () == false) {
                    progressBarStatus.Value = 0;
                    labelStatus.Text = "Invalid pool";
                    gridControls.IsEnabled = true;
                    return;
                }

                int pages = (int) (Math.Ceiling (((float) json ["post_count"]) / 24.0f));
                List<JToken> posts = new List<JToken> (json ["posts"].AsJEnumerable ());

                if (pages > 1) {
                    for (int i = 2; i < pages + 1; i++) {
                        using (WebClient c = new WebClient ()) {
                            c.QueryString.Clear ();
                            c.QueryString.Add ("id", dataToken.PoolID);
                            c.QueryString.Add ("page", i.ToString ());
                            SetHeaders (c.Headers);

                            var str = c.DownloadString (new Uri (PoolAPIURL));

                            using (JsonReader r = new JsonTextReader (new StringReader (str))) {
                                JObject j = (JObject) serializer.Deserialize (r);

                                if (j.TryGetValue ("success", out t) || t != null && t.Value<bool> () == false) {
                                    progressBarStatus.Value = 0;
                                    labelStatus.Text = "Invalid pool";
                                    gridControls.IsEnabled = true;
                                    return;
                                }

                                posts.AddRange (j ["posts"].AsJEnumerable ());
                            }
                        }
                    }
                }

                if (Properties.Settings.Default.DownloadInSubfolder) {
                    string poolName = json ["name"].ToString ().Replace ('_', ' ');

                    while (String.IsNullOrWhiteSpace (poolName) || Path.GetInvalidFileNameChars ().Any (invChar => poolName.Contains (invChar))) { // If the filename contains any invalid characters, ask the user to input a new name.
                        poolName = InputBox.ShowInputBox ("Invalid pool name", "The pool name is empty or whitespace or contains illegal characters.\nPlease input a name for the pool folder", poolName);

                        if (poolName == null) {
                            progressBarStatus.Value = 0;
                            labelStatus.Text = String.Format ("Cancelled downloading pool {0} ({1})", dataToken.PoolID, json ["name"].ToString ().Replace ('_', ' '));
                            gridControls.IsEnabled = true;
                            return;
                        }
                    }
                    dataToken.PoolName = poolName;
                    dataToken.DownloadPath = Path.Combine (dataToken.DownloadPath, poolName);
                }

                dataToken.ImageCount = posts.AsJEnumerable ().Count ();
                dataToken.DownloadCount = 0;
                progressBarStatus.Value = 0;
                labelStatus.Text = String.Format ("Downloading images - 1/{0}", dataToken.ImageCount);

                if (!Directory.Exists (dataToken.DownloadPath))
                    Directory.CreateDirectory (dataToken.DownloadPath);

                var threadTokens = new DownloaderThreadToken [ThreadCount];
                var tasks = new Task [ThreadCount];
                for (int i = 0; i < ThreadCount; i++) {
                    threadTokens [i] = new DownloaderThreadToken (token);
                    tasks [i] = new Task (new Action<object> (DoDownloadImages), threadTokens [i]);
                }

                for (int i = 0, threadNum = 0; i < posts.Count (); i++, threadNum++) {
                    var post = posts [i];

                    if (threadNum >= threadTokens.Count ())
                        threadNum = 0;

                    var img = new e621ImageData ();
                    img.Number = i;
                    if (Properties.Settings.Default.StartAtOne)
                        img.Number += 1;
                    img.URL = post ["file_url"].ToString ();
                    img.Filename = post ["md5"].ToString ();
                    img.Extension = post ["file_ext"].ToString ();

                    threadTokens [threadNum].ImageLinks.Add (img);
                }


                foreach (var task in tasks)
                    task.Start ();
            }
        }

        /// <summary>
        /// Called when the pool data download progresses.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataClient_DownloadProgressChanged (object sender, DownloadProgressChangedEventArgs e) {
            var token = (e.UserState as DownloadDataToken);

            if (token != null)
                labelStatus.Text = String.Format ("{0} - Progress: {1}/{2}", token.OperationText, e.BytesReceived, e.TotalBytesToReceive);
            progressBarStatus.Value = e.ProgressPercentage * 100;
        }

        /// <summary>
        /// Downloads the images. Worker thread function.
        /// </summary>
        /// <param name="state">The DownloaderThreadToken for this thread.</param>
        private void DoDownloadImages (object state) {
            var token = (DownloaderThreadToken) state;
            var dataToken = token.DownloadDataToken;

            using (var client = new WebClient ()) {
                // Set the required headers on the client.
                SetHeaders (client.Headers);

                foreach (var image in token.ImageLinks) {
                    client.DownloadFile (image.URL, Path.Combine (dataToken.DownloadPath, Utils.ParseFilenameMask (dataToken.FilenameMask, image.Filename, image.Extension, image.Number)));
                    ImageDownloaded ();
                }
            }
        }

        /// <summary>
        /// Called after an image finishes downloading.
        /// </summary>
        protected void ImageDownloaded () {
            if (!Dispatcher.CheckAccess ()) {
                Dispatcher.Invoke (new Action (ImageDownloaded));
                return;
            }

            if (token is null)
                return;

            token.DownloadCount++;
            if (token.DownloadCount < token.ImageCount) {
                labelStatus.Text = String.Format ("Downloading images - {0}/{1}", token.DownloadCount, token.ImageCount);
                progressBarStatus.Value = (double) token.DownloadCount / token.ImageCount * progressBarStatus.Maximum;
            } else {
                labelStatus.Text = String.Format ("Finished downloading pool {0} ({1})", token.PoolID, token.PoolName);
                progressBarStatus.Value = 0;
                gridControls.IsEnabled = true;
            }
        }

        #endregion

        #endregion
    }
}
