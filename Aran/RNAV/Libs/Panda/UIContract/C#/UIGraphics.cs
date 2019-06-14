using System;
using ARAN.Common;
using ARAN.Contracts.Registry;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace ARAN.Contracts.UI
{
	public class IconInfo : IDisposable
	{
		private ICONINFO ii;

		public IconInfo(IntPtr hCursor)
		{
			if (!GetIconInfo(hCursor, out ii))
			{
				throw new Exception("Bad hCursor");
			}
		}

		public System.Drawing.Bitmap ColorBitmap
		{
			get
			{
				if (ii.hbmColor == IntPtr.Zero) return null;
				return System.Drawing.Image.FromHbitmap(ii.hbmColor);
			}
		}

		public System.Drawing.Bitmap MaskBitmap
		{
			get
			{
				if (ii.hbmMask == IntPtr.Zero) return null;
				return System.Drawing.Image.FromHbitmap(ii.hbmMask);
			}
		}

		public System.Drawing.Point HotPoint
		{
			get
			{
				return new System.Drawing.Point(ii.xHotspot, ii.yHotspot);
			}
		}

		void IDisposable.Dispose()
		{
			if (ii.hbmColor != IntPtr.Zero) DeleteObject(ii.hbmColor);
			if (ii.hbmMask != IntPtr.Zero) DeleteObject(ii.hbmMask);
		}

		[DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
		static extern IntPtr DeleteObject(IntPtr hDc);

		[DllImport("user32.dll")]
		static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO piconinfo);

		[StructLayout(LayoutKind.Sequential)]
		struct ICONINFO
		{
			public bool fIcon;
			public Int32 xHotspot;
			public Int32 yHotspot;
			public IntPtr hbmMask;
			public IntPtr hbmColor;
		}
	}

	public static class UIGraphic
	{
		public static System.Drawing.Bitmap CheckPixelFormat(System.Drawing.Bitmap input)
		{
			if (input.PixelFormat != PixelFormat.Format32bppRgb)
			{
				//Convert the image into Format32bppRgb since our unmanaged code
				//can walk that image type
				System.Drawing.Bitmap b2 = new System.Drawing.Bitmap(input.Size.Width, input.Size.Height, PixelFormat.Format32bppRgb);

				System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(b2);
				g.DrawImage(input, new System.Drawing.Point(0, 0));
				g.Dispose();
				return b2;
			}
			return input;
		}
	}

	public class UIBitmap : PandaItem
	{
		private Int32 FWidth = 0, FHeight = 0;
		private UInt32[] FBits = { };

		public UInt32[] GetBits()
		{
			return FBits;
		}

		public UIBitmap()
			: base()
		{
			//FBits = new Int32[0];
			//FWidth = 0;
			//FHeight = 0;
		}

		public unsafe UIBitmap(System.Drawing.Bitmap input)
			: base()
		{
			UInt32[] pd;
			UInt32* ps;
			int x, y, i;

			SetSize(input.Width, input.Height);
			pd = FBits;

			input = UIGraphic.CheckPixelFormat(input);
			BitmapData bitmapData = input.LockBits(new System.Drawing.Rectangle(0, 0, input.Width, input.Height),
				ImageLockMode.ReadOnly, input.PixelFormat);

			ps = (UInt32*)bitmapData.Scan0.ToPointer();

			for (y = 0, i = 0; y < input.Height; y++)
			{
				for (x = 0; x < input.Width; x++)
				{
					pd[i] = *ps & 0xFFFFFF;
					i++;
					ps++;
				}
			}
			input.UnlockBits(bitmapData);
		}

		~UIBitmap()
		{
			SetSize(0, 0);
		}

		public void SetSize(Int32 NewWidth, Int32 NewHeight)
		{
			FWidth = NewWidth;
			FHeight = NewHeight;
			Int32 size = FWidth * FHeight;

			System.Array.Resize(ref FBits, size);
		}

		public void Clear()
		{
			SetSize(0, 0);
		}

		public void Pack(Int32 handle)
		{
			Registry_Contract.PutInt32(handle, FWidth);
			Registry_Contract.PutInt32(handle, FHeight);
			for (Int32 i = 0; i < FWidth * FHeight; i++)
				Registry_Contract.PutInt32(handle, (Int32)FBits[i]);
		}

		public void UnPack(Int32 handle)
		{
			Int32 w = Registry_Contract.GetInt32(handle);
			Int32 h = Registry_Contract.GetInt32(handle);

			SetSize(w, h);
			for (Int32 i = 0; i < w * h; i++)
				FBits[i] = (UInt32)Registry_Contract.GetInt32(handle);
		}

		public void AssignTo(UIBitmap dest)
		{
			dest.Assign(this);
		}

		public void Assign(PandaItem src)
		{
			SetSize(((UIBitmap)src).FWidth, ((UIBitmap)src).FHeight);
			for (Int32 i = 0; i < FWidth * FHeight; i++)
				FBits[i] = ((UIBitmap)src).FBits[i];
		}

		public Object Clone()
		{
			UIBitmap result = new UIBitmap();
			result.Assign(this);
			return result;
		}

		public Int32 Width
		{
			get { return FWidth; }
			set { FWidth = value; }
		}

		public Int32 Height
		{
			get { return FHeight; }
			set { FHeight = value; }
		}

		public UInt32[] Bits
		{
			get { return FBits; }
			//set { FBits = value; }
		}
	}

	public class UICursor : PandaItem
	{
		private Int32	FX = 0, FY = 0,
						FWidth = 0, FHeight = 0;
		private UInt32[] FAndMask = { };
		private UInt32[] FXorMask = { };
		private bool	FHaveXorMask;

		public UICursor()
			: base()
		{
			FHaveXorMask = false;
		}

		public UICursor(bool isColored)
			: base()
		{
			FHaveXorMask = isColored;
		}

		public unsafe UICursor(System.Windows.Forms.Cursor input)
		{
			UInt32[] pd;
			UInt32* ps;
			int cx, cy, x, y, i;
			System.Drawing.Bitmap cursorPicture;

			IconInfo cursorInfo = new IconInfo(input.Handle);

			cursorPicture = UIGraphic.CheckPixelFormat(cursorInfo.MaskBitmap);

			cx = cursorPicture.Width;
			cy = cursorPicture.Height;
			FHaveXorMask = cursorInfo.ColorBitmap != null;

			SetSize(cx, cy);
			SetSpot(cursorInfo.HotPoint.X, cursorInfo.HotPoint.Y);

			BitmapData bitmapData = cursorPicture.LockBits(new System.Drawing.Rectangle(0, 0, cx, cy),
				ImageLockMode.ReadOnly, cursorPicture.PixelFormat);

			pd = AndMask;
			ps = (UInt32*)bitmapData.Scan0.ToPointer();

			for (y = 0, i = 0; y < cy; y++)
			{
				for (x = 0; x < cx; x++)
				{
					pd[i] = *ps & 0xFFFFFF;
					i++;
					ps++;
				}
			}
			cursorPicture.UnlockBits(bitmapData);
			cursorPicture.Dispose();

			if (Colored)
			{
				cursorPicture = UIGraphic.CheckPixelFormat(cursorInfo.ColorBitmap);

				bitmapData = cursorPicture.LockBits(new System.Drawing.Rectangle(0, 0, cx, cy),
					ImageLockMode.ReadOnly, cursorPicture.PixelFormat);

				pd = XorMask;
				ps = (UInt32*)bitmapData.Scan0.ToPointer();

				for (y = 0, i = 0; y < cy; y++)
				{
					for (x = 0; x < cx; x++)
					{
						pd[i] = *ps & 0xFFFFFF;
						i++;
						ps++;
					}
				}
				cursorPicture.UnlockBits(bitmapData);
				cursorPicture.Dispose();
			}
		}

		~UICursor()
		{
			SetSize(0, 0);
		}

		public void SetSpot(Int32 SpotX, Int32 SpotY)
		{
			FX = SpotX;
			FY = SpotX;
		}

		public void SetSize(Int32 NewWidth, Int32 NewHeight)
		{
			FWidth = NewWidth;
			FHeight = NewHeight;
			Int32 size = FWidth * FHeight;

			if (size >= 0)
			{
				System.Array.Resize(ref FAndMask, size);
				if (FHaveXorMask)
					System.Array.Resize(ref FXorMask, size);
			}
		}

		public void Clear()
		{
			SetSize(0, 0);
			FX = 0;
			FY = 0;
		}

		public void Pack(Int32 handle)
		{
			Registry_Contract.PutInt32(handle, FWidth);
			Registry_Contract.PutInt32(handle, FHeight);
			Registry_Contract.PutInt32(handle, FX);
			Registry_Contract.PutInt32(handle, FY);
			Registry_Contract.PutBool(handle, FHaveXorMask);

			Int32 i, Size = FWidth * FHeight;

			for (i = 0; i < Size; i++)
				Registry_Contract.PutInt32(handle, (Int32)FAndMask[i]);

			if (FHaveXorMask)
				for (i = 0; i < Size; i++)
					Registry_Contract.PutInt32(handle, (Int32)FXorMask[i]);
		}

		public void UnPack(Int32 handle)
		{
			FWidth = Registry_Contract.GetInt32(handle);
			FHeight = Registry_Contract.GetInt32(handle);
			FX = Registry_Contract.GetInt32(handle);
			FY = Registry_Contract.GetInt32(handle);
			FHaveXorMask = Registry_Contract.GetBool(handle);

			SetSize(FWidth, FHeight);

			Int32 i, Size = FWidth * FHeight;

			for (i = 0; i < Size; i++)
				FAndMask[i] = (UInt32)Registry_Contract.GetInt32(handle);

			if (FHaveXorMask) 
				for (i = 0; i < Size; i++)
					FXorMask[i] = (UInt32)Registry_Contract.GetInt32(handle);
		}

		public void AssignTo(UICursor dest)
		{
			dest.Assign(this);
		}

		public void Assign(PandaItem src)
		{
			FHaveXorMask = ((UICursor)src).FHaveXorMask;
			SetSize(((UICursor)src).FWidth, ((UICursor)src).FHeight);
			FX = ((UICursor)src).FX;
			FY = ((UICursor)src).FY;

			Int32 Size = FWidth * FHeight;
			for (Int32 i = 0; i < Size; i++)
			{
				FAndMask[i] = ((UICursor)src).FAndMask[i];
				if (FHaveXorMask)
					FXorMask[i] = ((UICursor)src).FXorMask[i];
			}
		}

		public Object Clone()
		{
			UICursor result = new UICursor();
			result.Assign(this);
			return result;
		}

		public Int32 Width
		{
			get { return FWidth; }
			set { SetSize(value, FHeight);}
		}

		public Int32 Height
		{
			get { return FHeight; }
			set { SetSize(FWidth, value);}
		}

		public Int32 SpotX
		{
			get { return FX; }
			set { FX = value; }
		}

		public Int32 SpotY
		{
			get { return FY; }
			set { FY = value; }
		}
		/*
public Int32[] GetAndMask()
{
	return FAndMask;
}

public Int32[] GetXorMask()
{
	return FXorMask;
}
*/

		public UInt32[] AndMask
		{
			get { return FAndMask; }
		}

		public UInt32[] XorMask
		{
			get
			{
				if (FHaveXorMask)
					return FXorMask;
				else
					return null;
			}
		}

		public bool Colored
		{
			get { return FHaveXorMask; }
		}
	}
}