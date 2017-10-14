using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace e621_Pool_Downloader {
    public partial class mainForm : Form {
        public mainForm () {
            InitializeComponent ();
            textBoxNameMask.Text = "$i. $f.$e";
            client.DownloadDataCompleted += Client_DownloadDataCompleted;
            client.DownloadProgressChanged += Client_DownloadProgressChanged;
            picClient.DownloadFileCompleted += PicClient_DownloadFileCompleted;
            picClient.DownloadProgressChanged += PicClient_DownloadProgressChanged;
        }

        /// <summary>
        /// Shows a message box with the exception's info.
        /// Works mostly like assert, only displaying the exception if the first parameter is false
        /// </summary>
        /// <param name="expRes">Obvious</param>
        /// <param name="ex">The exception to display</param>
        private void ShowException (bool expRes, Exception ex) {
            if (!expRes)
                ShowException (ex);
        }
        /// <summary>
        /// Shows a message box with an exception's info.
        /// </summary>
        /// <param name="ex">The exception to display</param>
        private void ShowException (Exception ex) {
            MessageBox.Show (ex.Message + "\nException: " + ex.GetType ().ToString () + "\nInnerException: " + ex.InnerException + "\nSource: " + ex.Source + "\nTargetSite: " + ex.TargetSite, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        struct RegexThing {
            public Regex exp;
            public bool lit;
            public string var;
            public RegexThing (string expStr, string varName, bool notVar = false) {
                exp = new Regex (expStr, RegexOptions.ECMAScript);
                lit = notVar;
                var = varName;
            }
        }
        class VariableHolder {
            private object obj;
            private Type type;
            public string name;

            public dynamic ToVar () { return Convert.ChangeType (obj, type); }

            public VariableHolder (object newObj, Type newType, string newName) {
                this.obj = newObj; this.type = newType; this.name = newName;
            }
            public static VariableHolder MakeVariableHolder<T>(T variable, string name) {
                return new VariableHolder ((object) variable, typeof (T), name);
            }
            public static List<VariableHolder> MakeHolderList (params Tuple<Type, object, string> [] vars) {
                List<VariableHolder> ret = new List<VariableHolder> ();
                foreach (var top in vars) {
                    var variable = Convert.ChangeType (top.Item2, top.Item1);
                    ret.Add (MakeVariableHolder<object> (variable, top.Item3));
                }
                return ret;
            }
        }
        RegexThing [] regexes = {
            new RegexThing ("(?<!\\$)\\$f", "filename"),
            new RegexThing ("(?<!\\$)\\$e", "extension"),
            new RegexThing ("(?<!\\$)\\$i", "index"),
            new RegexThing ("(?<!\\$)\\$[^fei\\$]", " ", true),
            new RegexThing ("(?<!\\$)\\$\\$", "$", true),
        };
        private string ParseFilenameMask (string mask, string filename, string extension, int index = -1) {
            List<VariableHolder> vars = new List<VariableHolder> {
                VariableHolder.MakeVariableHolder<string> (filename,  "filename"),
                VariableHolder.MakeVariableHolder<string> (extension, "extension"),
                VariableHolder.MakeVariableHolder<int>    (index,     "index"),
            };
            if (index == -1)
                index = 0;
            string ret = mask;
            foreach (RegexThing regexp in regexes) {
                try {
                    var variable = regexp.lit ? regexp.var : vars.First (v => String.Compare (v.name, regexp.var) == 0).ToVar ();

                    if ((variable.GetType () == typeof (string) && String.IsNullOrEmpty (variable)) || variable == null)
                        variable = "";
                    var exp = regexp.exp;
                    ret = exp.Replace (ret, variable.ToString ());
                } catch (InvalidOperationException e) {
                    ShowException (vars.Count > 0, e);
                }
            }
            return ret;
        }

        private void textBoxNameMask_TextChanged (object sender, EventArgs e) {
            string str = ParseFilenameMask (textBoxNameMask.Text, "1c8f24072648aeecd484083fd3f08030", "jpg", 0);
            labelPreview.Text = String.Join ("", "Preview: ", str);
        }

        WebClient client = new WebClient ();
        WebClient picClient = new WebClient ();
        string clientProgText = "";
        int imageCount = -1;
        int curCount = 0;
        IJEnumerable<JToken> posts;
        JObject json = null;
        string poolName = "";
        string outPath = "";
        string [] headersStr = {
            "User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64; rv:25.8) Gecko/20151126 Firefox/31.9",
            "Accept: text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8",
            "Accept-Language: en-gb,en-us;q=0.7,en;q=0.3",
            "Accept-Encoding: identity",
        };
        public void SetHeaders (WebHeaderCollection hdrs) {
            foreach (string header in headersStr)
                hdrs.Add (header);
        }
        private void buttonGo_Click (object sender, EventArgs e) {
            json = null; poolName = null; outPath = null; curCount = 0; imageCount = -1;
            if (String.IsNullOrWhiteSpace (textBoxPID.Text)) {
                toolStripStatusLabel.Text = "You must specify a pool ID";
                return;
            } else if (String.IsNullOrWhiteSpace (textBoxPath.Text)) {
                toolStripStatusLabel.Text = "You must specify a path to download the pool in";
                return;
            } else if (String.IsNullOrWhiteSpace (textBoxNameMask.Text)) {
                toolStripStatusLabel.Text = "You must specify a filename mask";
                return;
            }
            string tmpPath = Path.GetFullPath (textBoxPath.Text);
            string path = Path.Combine (tmpPath, ParseFilenameMask (textBoxNameMask.Text, "1c8f24072648aeecd484083fd3f08030", "jpg", 0));
            try {
                Path.GetFullPath (tmpPath);
                Path.GetFullPath (path);
            } catch (Exception) {
                toolStripStatusLabel.Text = "Invalid path";
            }

            panel.Enabled = false;
            toolStripProgressBar.Style = ProgressBarStyle.Continuous;
            toolStripStatusLabel.Text = clientProgText = "Downloading pool data";
            client.QueryString.Clear ();
            client.QueryString.Add ("id", textBoxPID.Text);
            SetHeaders (client.Headers);
            client.DownloadDataAsync (new Uri ("https://e621.net/pool/show.json"));
            toolStripStatusLabel.Text = "Downloading pool data";
        }

        private void Client_DownloadProgressChanged (object sender, DownloadProgressChangedEventArgs e) {
            toolStripStatusLabel.Text = clientProgText + " Progress: " + e.BytesReceived + "/" + e.TotalBytesToReceive;
            toolStripProgressBar.Value = e.ProgressPercentage;
        }

        private void Client_DownloadDataCompleted (object sender, DownloadDataCompletedEventArgs e) {
            toolStripStatusLabel.Text = "Building image list";
            byte [] buffer = e.Result;
            File.WriteAllBytes ("derp.json", buffer);
            using (JsonReader reader = new JsonTextReader (new StreamReader (new MemoryStream (buffer)))) {
                JsonSerializer serializer = new JsonSerializer ();
                json = (JObject) serializer.Deserialize (reader);
                JToken t = null; json.TryGetValue ("success", out t);

                if (t != null && t.Value<bool> () == false) {
                    toolStripProgressBar.Style = ProgressBarStyle.Continuous; toolStripProgressBar.Value = 0; toolStripStatusLabel.Text = clientProgText = "Invalid pool"; panel.Enabled = true;
                    buffer = null; return;
                }

                int pages = (int) (Math.Ceiling (((float) json ["post_count"]) / 24.0f));
                posts = json ["posts"].AsJEnumerable ();

                if (pages > 1) {
                    for (int i = 2; i < pages + 1; i++) {
                        using (WebClient c = new WebClient ()) {
                            c.QueryString.Clear ();
                            c.QueryString.Add ("id", textBoxPID.Text);
                            c.QueryString.Add ("page", i.ToString ());
                            SetHeaders (c.Headers);
                            byte [] b = c.DownloadData (new Uri ("https://e621.net/pool/show.json"));
                            using (JsonReader r = new JsonTextReader (new StreamReader (new MemoryStream (b)))) {
                                JObject j = (JObject) serializer.Deserialize (r);
                                j.TryGetValue ("success", out t);
                                if (t != null && t.Value<bool> () == false) {
                                    toolStripProgressBar.Style = ProgressBarStyle.Continuous; toolStripProgressBar.Value = 0; panel.Enabled = true;
                                    toolStripStatusLabel.Text = clientProgText = "Invalid pool";
                                    b = null;
                                    return;
                                }
                                posts = posts.Concat (j ["posts"].AsJEnumerable ()).AsJEnumerable ();
                            }
                        }
                    }
                }

                if (checkBoxDownToSubfolder.Checked) {
                    poolName = json ["name"].ToString ().Replace ('_', ' ');
                    while (String.IsNullOrWhiteSpace (poolName) || Path.GetInvalidFileNameChars ().Any (invChar => poolName.Contains (invChar))) { // If the filename contains any invalid characters, ask the user to input a new name.
                        poolName = InputBox.ShowInputBox ("Invalid pool name", "The pool name is empty or whitespace or contains illegal characters.\nPlease input a name for the pool folder", poolName);
                        if (poolName == null) {
                            toolStripProgressBar.Style = ProgressBarStyle.Continuous; toolStripProgressBar.Value = 0; panel.Enabled = true;
                            toolStripStatusLabel.Text = "Cancelled downloading pool " + textBoxPID.Text + " (" + (json ["name"].ToString ().Replace ('_', ' ')) + ")";
                            json = null; posts = null;
                            return;
                        }
                    }
                    outPath = Path.Combine (textBoxPath.Text, poolName);
                } else
                    outPath = textBoxPath.Text;

                imageCount = posts.AsJEnumerable ().Count ();
                curCount = 0;
                toolStripProgressBar.Value = 0;
                toolStripStatusLabel.Text = "Downloading images 1/" + imageCount.ToString ();

                if (!Directory.Exists (outPath))
                    Directory.CreateDirectory (outPath);

                var post = (posts.ToArray () [0]);
                StartPicDownload ();
                reader.Close ();
            }
            buffer = null;
        }

        public void StartPicDownload () {
            int cntTMP = curCount++;
            var post = posts.ToArray () [cntTMP];
            string filePath = Path.Combine (outPath, ParseFilenameMask (textBoxNameMask.Text, Path.GetFileNameWithoutExtension (post ["file_url"].ToString ()), post ["file_ext"].ToString (), cntTMP + (checkBoxStartOne.Checked ? 1 : 0)));
            if (File.Exists (filePath))
                using (var str = File.OpenRead (filePath)) {
                    StringComparer comparer = StringComparer.OrdinalIgnoreCase;
                    if (comparer.Compare (GetMD5HashFromStream (str), post ["md5"].ToString ()) == 0) {
                        StartPicDownload ();
                        return;
                    }
                }
            SetHeaders (picClient.Headers);
            picClient.DownloadFileAsync (new Uri (post ["file_url"].ToString ()), Path.Combine (outPath, ParseFilenameMask (textBoxNameMask.Text, Path.GetFileNameWithoutExtension (post ["file_url"].ToString ()), post ["file_ext"].ToString (), cntTMP + (checkBoxStartOne.Checked ? 1 : 0))));
        }

        string GetMD5HashFromStream (Stream stream) {
            byte [] retVal;
            using (MD5 md5 = MD5.Create ())
                retVal = md5.ComputeHash (stream);

            return string.Concat (retVal.Select (x => x.ToString ("x2")));
        }

        private void PicClient_DownloadProgressChanged (object sender, DownloadProgressChangedEventArgs e) {
            int p = (int) 100 * ((curCount - 1) / imageCount + (1 / imageCount * e.ProgressPercentage));
            p = Math.Max (100, Math.Min (0, p));
            toolStripProgressBar.Value = p;
        }

        private void PicClient_DownloadFileCompleted (object sender, AsyncCompletedEventArgs e) {
            if (e.Error != null) {
                ShowException (e.Error);
                return;
            }
            if (curCount < imageCount) {
                if (!Directory.Exists (outPath))
                    Directory.CreateDirectory (outPath);
                toolStripProgressBar.Value = (100 / imageCount) * (curCount);
                StartPicDownload ();
                toolStripStatusLabel.Text = "Downloading images " + curCount.ToString () + "/" + imageCount.ToString ();
            } else
                DownloadFinishedCallback ();
        }

        private void DownloadFinishedCallback () {
            imageCount = -1;
            curCount = 0;
            panel.Enabled = true;
            toolStripProgressBar.Style = ProgressBarStyle.Continuous;
            toolStripProgressBar.Value = 0;
            toolStripStatusLabel.Text = "Finished downloading pool " + textBoxPID.Text + " (" + poolName + ")";
            json = null;
            posts = null;
        }

        private void buttonBrowsePath_Click (object sender, EventArgs e) {
            if (folderBrowserDialog.ShowDialog () == DialogResult.OK)
                textBoxPath.Text = folderBrowserDialog.SelectedPath;
        }
    }
}
