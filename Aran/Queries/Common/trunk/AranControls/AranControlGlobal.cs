using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Features;
using System.Windows.Forms;
using Aran.Aim;
using System.Drawing;
using System.Drawing.Drawing2D;
using Aran.Queries.Common;
using System.Security.Cryptography;

namespace Aran.Controls
{
    internal class Global
    {
        public static Image GetImage (int h)
        {
            if (_image == null)
            {
                Bitmap bmp = new Bitmap (20, h);
                Graphics gr = Graphics.FromImage (bmp);

                Color color1 = Color.White;
                Color color2 = SystemColors.Control;

                LinearGradientBrush lgb = new LinearGradientBrush (
                    new Point (0, -20), new Point (0, h), color1, color2);


                gr.FillRectangle (lgb, new Rectangle (0, 0, 20, h));

                gr.Dispose ();

                _image = bmp;
            }

            return _image;
        }

        public static Image GetActiveImage (int h)
        {
            if (_activeImage == null)
            {
                Bitmap bmp = new Bitmap (20, h);
                Graphics gr = Graphics.FromImage (bmp);

                Color color1 = Color.White;
                Color color2 = ControlPaint.Dark (ControlPaint.Dark (SystemColors.Control));

                LinearGradientBrush lgb = new LinearGradientBrush (
                    new Point (0, 0), new Point (0, h + 20), color1, color2);


                gr.FillRectangle (lgb, new Rectangle (0, 0, 20, h));

                gr.Dispose ();

                _activeImage = bmp;
            }

            return _activeImage;
        }

		//public static string GetMd5Hash (string input)
		//{
		//    MD5 md5Hash = MD5.Create ();
		//    // Convert the input string to a byte array and compute the hash. 
		//    byte [] data = md5Hash.ComputeHash (Encoding.UTF8.GetBytes (input));

		//    // Create a new Stringbuilder to collect the bytes 
		//    // and create a string.
		//    StringBuilder sBuilder = new StringBuilder ();

		//    // Loop through each byte of the hashed data  
		//    // and format each one as a hexadecimal string. 
		//    for (int i = 0; i < data.Length; i++)
		//    {
		//        sBuilder.Append (data [i].ToString ("x2"));
		//    }

		//    // Return the hexadecimal string. 
		//    return sBuilder.ToString ();
		//}

        private static Image _image;
        private static Image _activeImage;
    }

    public delegate string FeatureDescriptionEventHandler (object sender, FeatureEventArgs e);
}
