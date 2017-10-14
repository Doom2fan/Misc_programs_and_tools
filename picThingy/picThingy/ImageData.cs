using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace picThingy {
    public enum ImageDataChangeType {
        Loading = 1,
        Addition,
        Removal,
        Modification,
        Clear,
    }
    public sealed class ImageDataChangeEventArgs : EventArgs {
        public ImageDataChangeType ChangeType { get; private set; }

        public ImageDataChangeEventArgs (ImageDataChangeType changeType) {
            this.ChangeType = changeType;
        }
    }

    public sealed class ImagesInternal {
        private List<ImageData> images;
        #region Events
        public event EventHandler<ImageDataChangeEventArgs> OnChange;
        #endregion

        public ImagesInternal () {
            images = new List<ImageData> ();
        }

        private void SendOnChange (ImageDataChangeType changeType) {
            if ((this.OnChange != null))
                this.OnChange (this, new ImageDataChangeEventArgs (changeType));
        }

        #region Properties
        public int Count { get { return images.Count; } }
        #endregion

        #region Enumeration, Collection functions, etc.
        public ImageData this [int index] {
            get { return images [index]; }
            set {
                images [index] = value;
                SendOnChange (ImageDataChangeType.Modification);
            }
        }
        #endregion

        #region Functions
        #region Search
        public bool Contains (ImageData img) { return images.Contains (img); }
        public ImageData FromPath (string path) { return images.Find (i => String.Equals (i.path, path, StringComparison.CurrentCultureIgnoreCase)); }
        public ImageData FromSHA1 (string shaHash) { return images.Find (i => String.Equals (i.sha1Hash, shaHash, StringComparison.CurrentCultureIgnoreCase)); }
        public ImageData Find (Predicate<ImageData> match) { return images.Find (match); }
        public List<ImageData> FindAll (Predicate<ImageData> match) { return images.FindAll (match); }
        #endregion

        #region Addition
        public void Add (ImageData img) {
            images.Add (img);
            SendOnChange (ImageDataChangeType.Addition);
        }
        public void AddRange (IEnumerable<ImageData> imgs) {
            images.AddRange (imgs);
            SendOnChange (ImageDataChangeType.Addition);
        }
        #endregion

        #region Removal
        public bool Remove (ImageData img) {
            bool ret = images.Remove (img);
            SendOnChange (ImageDataChangeType.Addition);
            return ret;
        }
        public void RemoveAt (int index) {
            images.RemoveAt (index);
            SendOnChange (ImageDataChangeType.Removal);
        }
        public int RemoveAll (Predicate<ImageData> match) {
            int ret = images.RemoveAll (match);
            SendOnChange (ImageDataChangeType.Removal);
            return ret;
        }
        public void RemoveRange (int index, int count) {
            images.RemoveRange (index, count);
            SendOnChange (ImageDataChangeType.Removal);
        }
        #endregion

        public void Clear () {
            images.Clear ();
            SendOnChange (ImageDataChangeType.Clear);
        }

        #region Conversion
        public ImageData [] ToArray () { return images.ToArray (); }
        #endregion

        #region Saving/Loading
        public void SaveDatabase (string path = null) {
            if (String.IsNullOrWhiteSpace (path))
                path = Program.Options.DataFile;

            ImageDataList imagesList = new ImageDataList ();
            imagesList.imageList = ImageDataList.Images.ToArray ();
            JsonSerializer serializer = new JsonSerializer ();

            using (var writer = new StreamWriter (path)) {
                serializer.Serialize (writer, imagesList, typeof (ImageDataList));
                writer.Flush ();
            }
        }
        public void LoadDatabase (string path = null) {
            if (String.IsNullOrWhiteSpace (path))
                path = Program.Options.DataFile;

            if (!File.Exists (Program.Options.DataFile)) {
                using (var writer = File.CreateText (path)) { // Create the file
                    writer.Write ("{\n}"); // Write to the stream and flush it
                    writer.Flush ();
                }
            }

            using (var reader = new StreamReader (path, Encoding.UTF8)) {
                JsonReader jsonReader = new JsonTextReader (reader);
                // Try to deserialize the stream
                JsonSerializer serializer = new JsonSerializer ();
                ImageDataList imgList = serializer.Deserialize<ImageDataList> (jsonReader);
                ImageDataList.Images.Clear ();
                if (imgList != null && imgList.imageList != null)
                    ImageDataList.Images.AddRange (imgList.imageList);

                imgList = null; // Null the temporary image list
                jsonReader.Close (); // Close the jsonReader
            }

            SendOnChange (ImageDataChangeType.Loading);
        }
        #endregion
        #endregion
    }
    public class ImageDataList {
        public static ImagesInternal Images { get; set; } = new ImagesInternal ();
        public ImageData [] imageList { get; set; }
    }

    public class ImageData {
        public string path { get; set; }
        public string sha1Hash { get; set; }
        public string description { get; set; }
        public string thumbnailMD5 { get; set; }
        public long size { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string [] sources { get; set; }
        public string [] tags { get; set; }
    }
}
