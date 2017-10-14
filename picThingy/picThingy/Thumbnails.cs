using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace picThingy {
    public class ThumbnailsList {
        private Dictionary<string, string> entries = new Dictionary<string, string> ();

        public string this [string md5] {
            get {
                if (!entries.ContainsKey (md5))
                    throw new KeyNotFoundException ("MD5 key not found", null);
                return entries [md5];
            }
        }

        public Dictionary<string, string> Entries {
            get { return entries; }
        }

        public string FolderPath { get; set; }

        public bool HasMD5 (string md5) { return entries.ContainsKey (md5); }
        public bool HasPath (string path) { return entries.ContainsValue (path); }

        public string KeyFromPath (string path) { return entries.First (kvp => String.Equals (kvp.Value, path)).Key; }

        public bool RemoveEntry (string md5) { return entries.Remove (md5); }
        public void SetEntry (string md5, string origPath) {
            if (entries.ContainsKey (md5))
                entries [md5] = origPath;
            else
                entries.Add (md5, origPath);
        }
        public void SetEntries (Dictionary<string, string> newEntries) {
            entries = newEntries;
        }
    }
    public static class Thumbnails {
        private static ThumbnailsList thumbs;

        public static void LoadThumbs () {
            if (String.IsNullOrWhiteSpace (Program.Options.ThumbsPath))
                Program.Options.ThumbsPath = Constants.DefaultThumbsPath;

            if (!Directory.Exists (Program.Options.ThumbsPath))
                Directory.CreateDirectory (Program.Options.ThumbsPath);

            thumbs = new ThumbnailsList ();
            thumbs.FolderPath = Program.Options.ThumbsPath;
            string filePath = Path.Combine (Program.Options.ThumbsPath, Constants.ThumbsFileName);

            if (!File.Exists (filePath)) {
                using (var writer = File.CreateText (filePath)) { // Create the file
                    writer.Write ("{\n}"); // Write to the stream and flush it
                    writer.Flush ();
                }
            }

            using (var reader = new StreamReader (filePath, Encoding.UTF8)) {
                JsonReader jsonReader = new JsonTextReader (reader);
                // Try to deserialize the stream
                JsonSerializer serializer = new JsonSerializer ();
                thumbs = serializer.Deserialize<ThumbnailsList> (jsonReader);

                jsonReader.Close (); // Close the jsonReader
                jsonReader = null; // Null the jsonReader
            }
        }

        public static void SaveThumbs () {
            if (String.IsNullOrWhiteSpace (Program.Options.ThumbsPath))
                Program.Options.ThumbsPath = Constants.DefaultThumbsPath;

            if (!Directory.Exists (Program.Options.ThumbsPath))
                Directory.CreateDirectory (Program.Options.ThumbsPath);

            string filePath = Path.Combine (Program.Options.ThumbsPath, Constants.ThumbsFileName);

            using (var writer = new StreamWriter (filePath, false, Encoding.UTF8)) {
                JsonWriter jsonWriter = new JsonTextWriter (writer);
                // Try to deserialize the entries list
                JsonSerializer serializer = new JsonSerializer ();
                serializer.Serialize (jsonWriter, thumbs);

                jsonWriter.Close (); // Close the jsonWriter
                jsonWriter = null; // Null the jsonWriter
            }
        }

        public static string PathFromMD5 (string md5) {
            if (!thumbs.HasMD5 (md5))
                return null;
            else
                return thumbs [md5];
        }

        public static string KeyFromPath (string origPath) {
            if (!thumbs.HasPath (origPath))
                return null;
            else
                return thumbs.KeyFromPath (origPath);
        }
        public const int ThumbnailWidth = 512;
        public const int ThumbnailHeight = 512;


        public static string Add (string path) {
            string ret;

            using (var stream = new FileStream (path, FileMode.Open))
                ret = Add (stream, path);

            return ret;
        }

        /// <summary>
        /// Adds a thumbnail
        /// </summary>
        /// <param name="img">The image to create the thumbnail from</param>
        /// <returns>The MD5 of the thumbnail</returns>
        public static string Add (Stream img, string origPath) {
            img.Seek (0, SeekOrigin.Begin);

            string md5Hash;
            using (var origIMG = Image.FromStream (img)) {
                double ratio = Math.Min ((double) ThumbnailWidth / origIMG.Width, (double) ThumbnailHeight / origIMG.Height);

                using (var newBMP = new Bitmap ((int) Math.Ceiling (origIMG.Width * ratio), (int) Math.Ceiling (origIMG.Height * ratio))) {
                    using (var graph = Graphics.FromImage (newBMP)) {
                        graph.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                        graph.DrawImage (origIMG, 0, 0, (float) Math.Ceiling (origIMG.Width * ratio), (float) Math.Ceiling (origIMG.Height * ratio));
                        graph.Flush (System.Drawing.Drawing2D.FlushIntention.Sync);

                        using (var stream = new MemoryStream (1024 * 1024)) {
                            newBMP.Save (stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                            stream.Flush ();
                            byte [] bytes = stream.ToArray ();

                            using (var hasher = new System.Security.Cryptography.MD5Cng ())
                                md5Hash = BitConverter.ToString (hasher.ComputeHash (bytes)).Replace ("-", "");
                            
                            File.WriteAllBytes (Path.Combine (Program.Options.ThumbsPath, md5Hash), bytes);
                            thumbs.SetEntry (md5Hash, origPath);

                            bytes = null;
                        }
                    }
                }
            }

            return md5Hash;
        }

        public static Stream GetThumbnail (string md5) { return new FileStream (Path.Combine (Program.Options.ThumbsPath, md5), FileMode.Open); }
        public static string GetThumbnailPath (string md5) { return Path.Combine (Program.Options.ThumbsPath, md5); }

        public static void RemoveFromMd5 (string md5) {
            thumbs.RemoveEntry (md5);
        }
        public static void RemoveFromPath (string path) {
            thumbs.RemoveEntry (thumbs.KeyFromPath (path));
        }
    }
}
