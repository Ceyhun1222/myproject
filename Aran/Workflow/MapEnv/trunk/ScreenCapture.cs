using Aran.AranEnvironment;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Windows.Interop;

namespace MapEnv
{
    class ScreenCapture : IScreenCapture
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

        [StructLayout(LayoutKind.Sequential)]
        private struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        private static Dictionary<string, ScreenCapture> _screenCaptures;
        private static string mainDir;

        static ScreenCapture()
        {
            mainDir = Path.Combine(Globals.AppDir, "Captures");
            if (!Directory.Exists(mainDir))
                Directory.CreateDirectory(mainDir);
            _screenCaptures = new Dictionary<string, ScreenCapture>();
        }

        public static ScreenCapture Get(string module)
        {
            if (!_screenCaptures.ContainsKey(module))
                _screenCaptures.Add(module, new ScreenCapture(module));

            return _screenCaptures[module];
        }


        private string moduleName;
        private string moduleDir;
        private string tmpDir;
        private Captures captures;
        private List<String> tempCaptures;

        private string metaInfoPath
        {
            get
            {
                return moduleDir + "\\images.xml";
            }
        }
        private ScreenCapture(string module)
        {
            tempCaptures = new List<string>();
            moduleName = module;
            moduleDir = Path.Combine(mainDir, moduleName);
            tmpDir = Path.Combine(moduleDir, "tmp");
            checkDir();
            deserializeMetaInfo();
        }


        public List<Capture> Get(Guid uuid)
        {
            List<Capture> result = new List<Capture>();
            for (int i = 0; i < captures.CaptureList.Count; i++)
            {
                if (captures.CaptureList[i].uuid == uuid.ToString())
                {
                    var cp = captures.CaptureList[i];
                    Capture capture = new Capture { uuid = cp.uuid, date = cp.date };
                    capture.Images = new List<string>();
                    for (int j = 0; j < cp.Images.Count; j++)
                    {
                        capture.Images.Add(createPath(cp.Images[j]));
                    }
                    result.Add(capture);
                }
            }
            return result;
        }


        public void Save(Guid uuid, IntPtr hWnd)
        {
            checkTmpDir();
            save(uuid, hWnd);
        }

		public void Save ( Guid uuid, System.Windows.Window window )
		{
			checkTmpDir ( );
			save ( uuid, window );
		}

		public void Save(Guid uuid, Form form)
        {
            checkTmpDir();
            save(uuid, form);
        }

        public void Save(Guid uuid)
        {
            checkTmpDir();
            var foregroundWindowsHandle = GetForegroundWindow();
            save(uuid, foregroundWindowsHandle);
        }


        public void Save(IntPtr hWnd)
        {
            Save(Guid.NewGuid(), hWnd);
        }

        public void Save(Form form)
        {
            Save(Guid.NewGuid(), form);
        }

		public void Save ( System.Windows.Window window )
		{
			Save ( Guid.NewGuid ( ), window );
		}

		public void Save()
        {
            Save(Guid.NewGuid());
        }


        private void save(Guid uuid, Form form)
        {
            using (Bitmap bitmap = new Bitmap(form.Width, form.Height))
            {
                form.DrawToBitmap(bitmap, new Rectangle(0, 0, form.Width, form.Height));
                string fileName = createTmpPath(uuid.ToString());
                bitmap.Save(fileName, ImageFormat.Jpeg);
                tempCaptures.Add(uuid.ToString());
            }
        }

        private void save(Guid uuid, IntPtr foregroundWindowsHandle)
        {
            Rectangle bounds;
            var rect = new Rect();
            GetWindowRect(foregroundWindowsHandle, ref rect);
            bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);

            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                }
                string fileName = createTmpPath(uuid.ToString());
                bitmap.Save(fileName, ImageFormat.Jpeg);
                tempCaptures.Add(uuid.ToString());
            }
        }

		private void save(Guid uuid, System.Windows.Window window)
        {
			WindowInteropHelper helper = new WindowInteropHelper ( window );
			IntPtr ptr = helper.Handle;
			Save (uuid, ptr);
		}

        public void Delete(Guid uuid)
        {
            string fileName = createTmpPath(uuid.ToString());
            File.Delete(fileName);
            tempCaptures.Remove(uuid.ToString());
        }

        public void Delete()
        {
            if (tempCaptures.Count > 0)
            {
                string fileName = createTmpPath(tempCaptures[tempCaptures.Count - 1]);
                File.Delete(fileName);
                tempCaptures.RemoveAt(tempCaptures.Count - 1);
            }
        }

        public byte[] Commit(Guid uuid)
        {
            byte[] result = getZip();
            moveImages();
            moveTempList(uuid.ToString());
            serializeInfo();
            emptyTempDir();
            return result;
        }

        

        public void Rollback()
        {
            cleanTempList();
            emptyTempDir();
        }

        private string createTmpPath(string uuid)
        {
            return tmpDir + "\\" + uuid + ".jpeg";
        }

        private string createPath(string uuid)
        {
            return moduleDir + "\\" + uuid + ".jpeg";
        }

        private void deserializeMetaInfo()
        {
            if (File.Exists(metaInfoPath))
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(Captures));
                StreamReader reader = new StreamReader(metaInfoPath);
                captures = (Captures)x.Deserialize(reader);
                reader.Close();
            }
            else captures = new Captures();

        }

        private void serializeInfo()
        {
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(captures.GetType());
            FileStream file = System.IO.File.Create(metaInfoPath);
            x.Serialize(file, captures);
            file.Close();
        }

        private byte[] getZip()
        {
            byte[] result = new byte[0];
            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    for (int i = 0; i < tempCaptures.Count; i++)
                    {
                        var path = createTmpPath(tempCaptures[i]);
                        archive.CreateEntryFromFile(path, Path.GetFileName(path));
                    }
                }
                result = memoryStream.ToArray();
            }

            return result;
        }

        private void cleanTempList()
        {
            tempCaptures.Clear();
        }

        private void moveTempList(string uuid)
        {
            Capture capture = new Capture();
            capture.uuid = uuid;
            capture.date = DateTime.Now;
            for (int i = 0; i < tempCaptures.Count; i++)
            {
                capture.Images.Add(tempCaptures[i]);
            }
            captures.CaptureList.Add(capture);
            tempCaptures.Clear();
        }


        private void moveImages()
        {
            int last = -1;
            try
            {
                for (int i = 0; i < tempCaptures.Count; i++)
                {
                    File.Copy(createTmpPath(tempCaptures[i]), createPath(tempCaptures[i]));
                    last = i;
                }
            }
            catch (Exception e)
            {
                if (last > -1)
                    for (int i = 0; i < last + 1; i++)
                    {
                        File.Delete(createPath(tempCaptures[i]));
                    }
                throw e;
            }
        }

        private void emptyTempDir()
        {
            if (Directory.Exists(tmpDir))
            {
                DirectoryInfo di = new DirectoryInfo(tmpDir);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
                di.Delete();
            }
        }

        private void checkDir()
        {
            if (!Directory.Exists(moduleDir))
                Directory.CreateDirectory(moduleDir);
        }

        private void checkTmpDir()
        {
            if (!Directory.Exists(tmpDir))
                Directory.CreateDirectory(tmpDir);
        }


    }

}
