using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace picThingyOLD {
    public class ImagesInternal {
        private List<ImageData> images;
        #region Events
        public event EventHandler OnChange = delegate { };
        #endregion

        public ImagesInternal () {
            images = new List<ImageData> ();
        }

        #region Properties
        public int Count { get { return images.Count; } }
        #endregion

        #region Enumeration, Collection functions, etc.
        public ImageData this [int index] {
            get { return images [index]; }
            set { images [index] = value; OnChange (this, null); }
        }
        #endregion  

        #region Functions
        public void Add (ImageData img) { images.Add (img); OnChange (this, null); }
        public void AddRange (IEnumerable<ImageData> imgs) { images.AddRange (imgs); OnChange (this, null); }
        public bool Remove (ImageData img) { bool ret = images.Remove (img); OnChange (this, null); return ret; }
        public void RemoveAt (int index) { images.RemoveAt (index); }
        public int  RemoveAll (Predicate<ImageData> match) { int ret = images.RemoveAll (match); OnChange (this, null); return ret; }
        public void RemoveRange (int index, int count) { images.RemoveRange (index, count); OnChange (this, null); }
        public void Clear () { images.Clear (); OnChange (this, null); }
        public ImageData [] ToArray () { return images.ToArray (); }

        public void SaveDatabase (string path = null) {
            if (String.IsNullOrWhiteSpace (path))
                path = Program.Options.DataFile;
            ImageDataList imagesList = new ImageDataList ();
            imagesList.imageList = ImageDataList.Images.ToArray ();
            JsonSerializer serializer = new JsonSerializer ();

            var writer = new StreamWriter (path);
            serializer.Serialize (writer, imagesList, typeof (ImageDataList));
            writer.Flush ();
            writer.Close ();
        }
        public void LoadDatabase (string path = null) {
            if (String.IsNullOrWhiteSpace (path))
                path = Program.Options.DataFile;

            if (!File.Exists (Program.Options.DataFile)) {
                var writer = File.CreateText (path); // Create the file
                writer.Write ("{\n}"); writer.Flush (); // Write to the stream and flush it
                writer.Close (); writer.Dispose (); // Close and dispose of the stream
                writer = null; // Null the stream
            }

            var reader = new StreamReader (path, Encoding.UTF8);
            JsonReader jsonReader = new JsonTextReader (reader);
            // Try to deserialize the stream
            JsonSerializer serializer = new JsonSerializer ();
            ImageDataList imgList = serializer.Deserialize<ImageDataList> (jsonReader);
            ImageDataList.Images = new ImagesInternal ();
            if (imgList != null && imgList.imageList != null)
                ImageDataList.Images.AddRange (imgList.imageList);

            imgList = null; // Null the temporary image list
            jsonReader.Close (); // Close the jsonReader
            reader.Close (); reader.Dispose (); // Close and dispose of the stream
            serializer = null; jsonReader = null; reader = null; // Null the serializer, the reader and the stream
        }
        #endregion
    }
    public class ImageDataList {
        public static ImagesInternal Images = new ImagesInternal ();
        public ImageData [] imageList;
    }

    public class ImageData {
        public string path, sha1Hash, description;
        public long size;
        public int width, height;
        public string [] sources;
        public string [] tags;
    }
}
