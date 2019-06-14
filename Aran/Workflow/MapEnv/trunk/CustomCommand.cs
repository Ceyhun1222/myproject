using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.SystemUI;
using System.Drawing;

namespace MapEnv
{
    public class CustomCommand : ICommand
    {
        private Bitmap _image;
        private string _caption;
        public event EventHandler Clicked;

        public CustomCommand()
        {
            Tooltip = "";
            Message = "";
            Category = "AranCategory";
            HelpContextID = 0;
            Name = "";
            Enabled = true;
        }

        public int Bitmap { get; set; }

        public string Caption
        {
            get { return _caption; }
            set
            {
                _caption = value;
                if (string.IsNullOrWhiteSpace(Tooltip))
                    Tooltip = value;
            }
        }

        public string Category { get; set; }

        public bool Checked { get; set; }

        public bool Enabled { get; set; }

        public int HelpContextID { get; set; }

        public string HelpFile { get; set; }

        public string Message { get; set; }

        public string Name { get; private set; }

        public void OnClick()
        {
            if (Clicked != null)
                Clicked(this, null);
        }

        public void OnCreate(object Hook)
        {
        }

        public string Tooltip { get; set; }

        public Bitmap Image
        {
            get { return _image; }
            set
            {
                _image = value;
                Bitmap = _image.GetHbitmap(Color.Black).ToInt32();
            }
        }
    }
}
