using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.ServiceModel.Web;
using Aran.Temporality.Common.Util;

namespace Aran.Temporality.Internal.Remote.Service
{
    internal class ExternalSystemService : IExternalSystemService
    {
        public string Dummy()
        {
            return "It is working!";
        }

        public Stream Test()
        {
            Bitmap bitmap = new Bitmap(100, 100);
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    bitmap.SetPixel(i, j, Math.Abs(i - j) < 2 ? Color.Blue : Color.Yellow);
                }
            }
            var ms = MemoryUtil.GetMemoryStream();
            bitmap.Save(ms, ImageFormat.Jpeg);
            ms.Position = 0;
            if (WebOperationContext.Current != null)
            {
                WebOperationContext.Current.OutgoingResponse.ContentType = "image/jpeg";
            }
            return ms;
        }
    }
}
