using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TOSSM.Util
{
    public static class Matrix
    {
        public static double[,] Mean3x3
        {
            get
            {
                return new double[,]  
                { { 1, 1, 1, }, 
                  { 1, 1, 1, }, 
                  { 1, 1, 1, }, };
            }
        }

        public static double[,] Mean5x5
        {
            get
            {
                return new double[,]  
                { { 1, 1, 1, 1, 1}, 
                  { 1, 1, 1, 1, 1},
                  { 1, 1, 1, 1, 1},
                  { 1, 1, 1, 1, 1},
                  { 1, 1, 1, 1, 1}, };
            }
        }

        public static double[,] Mean7x7
        {
            get
            {
                return new double[,]  
                { { 1, 1, 1, 1, 1, 1, 1}, 
                  { 1, 1, 1, 1, 1, 1, 1},
                  { 1, 1, 1, 1, 1, 1, 1},
                  { 1, 1, 1, 1, 1, 1, 1},
                  { 1, 1, 1, 1, 1, 1, 1},
                  { 1, 1, 1, 1, 1, 1, 1},
                  { 1, 1, 1, 1, 1, 1, 1}, };
            }
        }

        public static double[,] Mean9x9
        {
            get
            {
                return new double[,]  
                { { 1, 1, 1, 1, 1, 1, 1, 1, 1}, 
                  { 1, 1, 1, 1, 1, 1, 1, 1, 1},
                  { 1, 1, 1, 1, 1, 1, 1, 1, 1},
                  { 1, 1, 1, 1, 1, 1, 1, 1, 1},
                  { 1, 1, 1, 1, 1, 1, 1, 1, 1},
                  { 1, 1, 1, 1, 1, 1, 1, 1, 1},
                  { 1, 1, 1, 1, 1, 1, 1, 1, 1},
                  { 1, 1, 1, 1, 1, 1, 1, 1, 1},
                  { 1, 1, 1, 1, 1, 1, 1, 1, 1}, };
            }
        }

        public static double[,] GaussianBlur3x3
        {
            get
            {
                return new double[,]  
                { { 1, 2, 1, }, 
                  { 2, 4, 2, }, 
                  { 1, 2, 1, }, };
            }
        }

        public static double[,] GaussianBlur5x5
        {
            get
            {
                return new double[,]  
                { { 2, 04, 05, 04, 2 }, 
                  { 4, 09, 12, 09, 4 }, 
                  { 5, 12, 15, 12, 5 },
                  { 4, 09, 12, 09, 4 },
                  { 2, 04, 05, 04, 2 }, };
            }
        }

        public static double[,] MotionBlur5x5
        {
            get
            {
                return new double[,]  
                { { 1, 0, 0, 0, 1}, 
                  { 0, 1, 0, 1, 0}, 
                  { 0, 0, 1, 0, 0},
                  { 0, 1, 0, 1, 0},
                  { 1, 0, 0, 0, 1}, };
            }
        }

        public static double[,] MotionBlur5x5At45Degrees
        {
            get
            {
                return new double[,]  
                { { 0, 0, 0, 0, 1}, 
                  { 0, 0, 0, 1, 0}, 
                  { 0, 0, 1, 0, 0},
                  { 0, 1, 0, 0, 0},
                  { 1, 0, 0, 0, 0}, };
            }
        }

        public static double[,] MotionBlur5x5At135Degrees
        {
            get
            {
                return new double[,]  
                { { 1, 0, 0, 0, 0}, 
                  { 0, 1, 0, 0, 0}, 
                  { 0, 0, 1, 0, 0},
                  { 0, 0, 0, 1, 0},
                  { 0, 0, 0, 0, 1}, };
            }
        }

        public static double[,] MotionBlur7x7
        {
            get
            {
                return new double[,]  
                { { 1, 0, 0, 0, 0, 0, 1}, 
                  { 0, 1, 0, 0, 0, 1, 0}, 
                  { 0, 0, 1, 0, 1, 0, 0},
                  { 0, 0, 0, 1, 0, 0, 0},
                  { 0, 0, 1, 0, 1, 0, 0},
                  { 0, 1, 0, 0, 0, 1, 0},
                  { 1, 0, 0, 0, 0, 0, 1}, };
            }
        }

        public static double[,] MotionBlur7x7At45Degrees
        {
            get
            {
                return new double[,]  
                { { 0, 0, 0, 0, 0, 0, 1}, 
                  { 0, 0, 0, 0, 0, 1, 0}, 
                  { 0, 0, 0, 0, 1, 0, 0},
                  { 0, 0, 0, 1, 0, 0, 0},
                  { 0, 0, 1, 0, 0, 0, 0},
                  { 0, 1, 0, 0, 0, 0, 0},
                  { 1, 0, 0, 0, 0, 0, 0}, };
            }
        }

        public static double[,] MotionBlur7x7At135Degrees
        {
            get
            {
                return new double[,]  
                { { 1, 0, 0, 0, 0, 0, 0}, 
                  { 0, 1, 0, 0, 0, 0, 0}, 
                  { 0, 0, 1, 0, 0, 0, 0},
                  { 0, 0, 0, 1, 0, 0, 0},
                  { 0, 0, 0, 0, 1, 0, 0},
                  { 0, 0, 0, 0, 0, 1, 0},
                  { 0, 0, 0, 0, 0, 0, 1}, };
            }
        }

        public static double[,] MotionBlur9x9
        {
            get
            {
                return new double[,]  
                { {1, 0, 0, 0, 0, 0, 0, 0, 1,},
                  {0, 1, 0, 0, 0, 0, 0, 1, 0,},
                  {0, 0, 1, 0, 0, 0, 1, 0, 0,},
                  {0, 0, 0, 1, 0, 1, 0, 0, 0,},
                  {0, 0, 0, 0, 1, 0, 0, 0, 0,},
                  {0, 0, 0, 1, 0, 1, 0, 0, 0,},
                  {0, 0, 1, 0, 0, 0, 1, 0, 0,},
                  {0, 1, 0, 0, 0, 0, 0, 1, 0,},
                  {1, 0, 0, 0, 0, 0, 0, 0, 1,}, };
            }
        }

        public static double[,] MotionBlur9x9At45Degrees
        {
            get
            {
                return new double[,]  
                { {0, 0, 0, 0, 0, 0, 0, 0, 1,},
                  {0, 0, 0, 0, 0, 0, 0, 1, 0,},
                  {0, 0, 0, 0, 0, 0, 1, 0, 0,},
                  {0, 0, 0, 0, 0, 1, 0, 0, 0,},
                  {0, 0, 0, 0, 1, 0, 0, 0, 0,},
                  {0, 0, 0, 1, 0, 0, 0, 0, 0,},
                  {0, 0, 1, 0, 0, 0, 0, 0, 0,},
                  {0, 1, 0, 0, 0, 0, 0, 0, 0,},
                  {1, 0, 0, 0, 0, 0, 0, 0, 0,}, };
            }
        }

        public static double[,] MotionBlur9x9At135Degrees
        {
            get
            {
                return new double[,]  
                { {1, 0, 0, 0, 0, 0, 0, 0, 0,},
                  {0, 1, 0, 0, 0, 0, 0, 0, 0,},
                  {0, 0, 1, 0, 0, 0, 0, 0, 0,},
                  {0, 0, 0, 1, 0, 0, 0, 0, 0,},
                  {0, 0, 0, 0, 1, 0, 0, 0, 0,},
                  {0, 0, 0, 0, 0, 1, 0, 0, 0,},
                  {0, 0, 0, 0, 0, 0, 1, 0, 0,},
                  {0, 0, 0, 0, 0, 0, 0, 1, 0,},
                  {0, 0, 0, 0, 0, 0, 0, 0, 1,}, };
            }
        }
    }

    public static class ExtBitmap
    {
        public static Bitmap CopyToSquareCanvas(this Bitmap sourceBitmap, int canvasWidthLenght)
        {
            float ratio = 1.0f;
            int maxSide = sourceBitmap.Width > sourceBitmap.Height ?
                          sourceBitmap.Width : sourceBitmap.Height;

            ratio = (float)maxSide / (float)canvasWidthLenght;

            Bitmap bitmapResult = (sourceBitmap.Width > sourceBitmap.Height ?
                                    new Bitmap(canvasWidthLenght, (int)(sourceBitmap.Height / ratio))
                                    : new Bitmap((int)(sourceBitmap.Width / ratio), canvasWidthLenght));

            using (Graphics graphicsResult = Graphics.FromImage(bitmapResult))
            {
                graphicsResult.CompositingQuality = CompositingQuality.HighQuality;
                graphicsResult.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphicsResult.PixelOffsetMode = PixelOffsetMode.HighQuality;

                graphicsResult.DrawImage(sourceBitmap,
                                        new Rectangle(0, 0,
                                            bitmapResult.Width, bitmapResult.Height),
                                        new Rectangle(0, 0,
                                            sourceBitmap.Width, sourceBitmap.Height),
                                            GraphicsUnit.Pixel);
                graphicsResult.Flush();
            }

            return bitmapResult;
        }

        public static Bitmap ImageBlurFilter(this Bitmap sourceBitmap,
                                                    BlurType blurType)
        {
            Bitmap resultBitmap = null;

            switch (blurType)
            {
                case BlurType.Mean3x3:
                    {
                        resultBitmap = sourceBitmap.ConvolutionFilter(
                                       Matrix.Mean3x3, 1.0 / 9.0, 0);
                    } break;
                case BlurType.Mean5x5:
                    {
                        resultBitmap = sourceBitmap.ConvolutionFilter(
                                       Matrix.Mean5x5, 1.0 / 25.0, 0);
                    } break;
                case BlurType.Mean7x7:
                    {
                        resultBitmap = sourceBitmap.ConvolutionFilter(
                                       Matrix.Mean7x7, 1.0 / 49.0, 0);
                    } break;
                case BlurType.Mean9x9:
                    {
                        resultBitmap = sourceBitmap.ConvolutionFilter(
                                       Matrix.Mean9x9, 1.0 / 81.0, 0);
                    } break;
                case BlurType.GaussianBlur3x3:
                    {
                        resultBitmap = sourceBitmap.ConvolutionFilter(
                                Matrix.GaussianBlur3x3, 1.0 / 16.0, 0);
                    } break;
                case BlurType.GaussianBlur5x5:
                    {
                        resultBitmap = sourceBitmap.ConvolutionFilter(
                               Matrix.GaussianBlur5x5, 1.0 / 159.0, 0);
                    } break;
                case BlurType.MotionBlur5x5:
                    {
                        resultBitmap = sourceBitmap.ConvolutionFilter(
                                   Matrix.MotionBlur5x5, 1.0 / 10.0, 0);
                    } break;
                case BlurType.MotionBlur5x5At45Degrees:
                    {
                        resultBitmap = sourceBitmap.ConvolutionFilter(
                        Matrix.MotionBlur5x5At45Degrees, 1.0 / 5.0, 0);
                    } break;
                case BlurType.MotionBlur5x5At135Degrees:
                    {
                        resultBitmap = sourceBitmap.ConvolutionFilter(
                        Matrix.MotionBlur5x5At135Degrees, 1.0 / 5.0, 0);
                    } break;
                case BlurType.MotionBlur7x7:
                    {
                        resultBitmap = sourceBitmap.ConvolutionFilter(
                        Matrix.MotionBlur7x7, 1.0 / 14.0, 0);
                    } break;
                case BlurType.MotionBlur7x7At45Degrees:
                    {
                        resultBitmap = sourceBitmap.ConvolutionFilter(
                        Matrix.MotionBlur7x7At45Degrees, 1.0 / 7.0, 0);
                    } break;
                case BlurType.MotionBlur7x7At135Degrees:
                    {
                        resultBitmap = sourceBitmap.ConvolutionFilter(
                        Matrix.MotionBlur7x7At135Degrees, 1.0 / 7.0, 0);
                    } break;
                case BlurType.MotionBlur9x9:
                    {
                        resultBitmap = sourceBitmap.ConvolutionFilter(
                        Matrix.MotionBlur9x9, 1.0 / 18.0, 0);
                    } break;
                case BlurType.MotionBlur9x9At45Degrees:
                    {
                        resultBitmap = sourceBitmap.ConvolutionFilter(
                        Matrix.MotionBlur9x9At45Degrees, 1.0 / 9.0, 0);
                    } break;
                case BlurType.MotionBlur9x9At135Degrees:
                    {
                        resultBitmap = sourceBitmap.ConvolutionFilter(
                        Matrix.MotionBlur9x9At135Degrees, 1.0 / 9.0, 0);
                    } break;
                case BlurType.Median3x3:
                    {
                        resultBitmap = sourceBitmap.MedianFilter(3);
                    } break;
                case BlurType.Median5x5:
                    {
                        resultBitmap = sourceBitmap.MedianFilter(5);
                    } break;
                case BlurType.Median7x7:
                    {
                        resultBitmap = sourceBitmap.MedianFilter(7);
                    } break;
                case BlurType.Median9x9:
                    {
                        resultBitmap = sourceBitmap.MedianFilter(9);
                    } break;
                case BlurType.Median11x11:
                    {
                        resultBitmap = sourceBitmap.MedianFilter(11);
                    } break;
            }

            return resultBitmap;
        }

        private static Bitmap ConvolutionFilter(this Bitmap sourceBitmap,
                                                  double[,] filterMatrix,
                                                       double factor = 1,
                                                            int bias = 0)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                     sourceBitmap.Width, sourceBitmap.Height),
                                                       ImageLockMode.ReadOnly,
                                                 PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);

            double alpha = 0.0;
            double blue = 0.0;
            double green = 0.0;
            double red = 0.0;

            int filterWidth = filterMatrix.GetLength(1);
            int filterHeight = filterMatrix.GetLength(0);

            int filterOffset = (filterWidth - 1) / 2;
            int calcOffset = 0;

            int byteOffset = 0;

            for (int offsetY = filterOffset; offsetY <
                sourceBitmap.Height - filterOffset; offsetY++)
            {
                for (int offsetX = filterOffset; offsetX <
                    sourceBitmap.Width - filterOffset; offsetX++)
                {
                    blue = 0;
                    green = 0;
                    red = 0;
                    alpha = 0;

                    byteOffset = offsetY *
                                 sourceData.Stride +
                                 offsetX * 4;

                    for (int filterY = -filterOffset; filterY <= filterOffset; filterY++)
                    {
                        for (int filterX = -filterOffset;
                            filterX <= filterOffset; filterX++)
                        {
                            calcOffset = byteOffset +
                                         (filterX * 4) +
                                         (filterY * sourceData.Stride);

                            blue += (double)(pixelBuffer[calcOffset]) *
                                    filterMatrix[filterY + filterOffset,
                                                        filterX + filterOffset];

                            green += (double)(pixelBuffer[calcOffset + 1]) *
                                     filterMatrix[filterY + filterOffset,
                                                        filterX + filterOffset];

                            red += (double)(pixelBuffer[calcOffset + 2]) *
                                   filterMatrix[filterY + filterOffset,
                                                      filterX + filterOffset];

                            alpha += (double)(pixelBuffer[calcOffset+3]) *
                                   filterMatrix[filterY + filterOffset,
                                                       filterX + filterOffset];
                        }
                    }

                    blue = factor * blue + bias;
                    green = factor * green + bias;
                    red = factor * red + bias;
                    alpha = factor * alpha + bias;

                    blue = (blue > 255 ? 255 :
                           (blue < 0 ? 0 :
                            blue));

                    green = (green > 255 ? 255 :
                            (green < 0 ? 0 :
                             green));

                    red = (red > 255 ? 255 :
                          (red < 0 ? 0 :
                           red));

                    alpha = (alpha > 255 ? 255 :
                          (alpha < 0 ? 0 :
                           alpha));

                    resultBuffer[byteOffset] = (byte)(blue);
                    resultBuffer[byteOffset + 1] = (byte)(green);
                    resultBuffer[byteOffset + 2] = (byte)(red);
                    resultBuffer[byteOffset + 3] = (byte)alpha;
                }
            }

            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);

            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                     resultBitmap.Width, resultBitmap.Height),
                                                      ImageLockMode.WriteOnly,
                                                 PixelFormat.Format32bppArgb);

            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);

            return resultBitmap;
        }

        public enum BlurType
        {
            Mean3x3,
            Mean5x5,
            Mean7x7,
            Mean9x9,
            GaussianBlur3x3,
            GaussianBlur5x5,
            MotionBlur5x5,
            MotionBlur5x5At45Degrees,
            MotionBlur5x5At135Degrees,
            MotionBlur7x7,
            MotionBlur7x7At45Degrees,
            MotionBlur7x7At135Degrees,
            MotionBlur9x9,
            MotionBlur9x9At45Degrees,
            MotionBlur9x9At135Degrees,
            Median3x3,
            Median5x5,
            Median7x7,
            Median9x9,
            Median11x11
        }

        public static Bitmap MedianFilter(this Bitmap sourceBitmap,
                                                    int matrixSize)
        {
            BitmapData sourceData =
                       sourceBitmap.LockBits(new Rectangle(0, 0,
                       sourceBitmap.Width, sourceBitmap.Height),
                       ImageLockMode.ReadOnly,
                       PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceData.Stride *
                                          sourceData.Height];

            byte[] resultBuffer = new byte[sourceData.Stride *
                                           sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0,
                                       pixelBuffer.Length);

            sourceBitmap.UnlockBits(sourceData);

            int filterOffset = (matrixSize - 1) / 2;
            int calcOffset = 0;

            int byteOffset = 0;

            List<int> neighbourPixels = new List<int>();
            byte[] middlePixel;

            for (int offsetY = filterOffset; offsetY <
                sourceBitmap.Height - filterOffset; offsetY++)
            {
                for (int offsetX = filterOffset; offsetX <
                    sourceBitmap.Width - filterOffset; offsetX++)
                {
                    byteOffset = offsetY *
                                 sourceData.Stride +
                                 offsetX * 4;

                    neighbourPixels.Clear();

                    for (int filterY = -filterOffset;
                        filterY <= filterOffset; filterY++)
                    {
                        for (int filterX = -filterOffset;
                            filterX <= filterOffset; filterX++)
                        {

                            calcOffset = byteOffset +
                                         (filterX * 4) +
                                         (filterY * sourceData.Stride);

                            neighbourPixels.Add(BitConverter.ToInt32(
                                             pixelBuffer, calcOffset));
                        }
                    }

                    neighbourPixels.Sort();

                    middlePixel = BitConverter.GetBytes(
                                       neighbourPixels[filterOffset]);

                    resultBuffer[byteOffset] = middlePixel[0];
                    resultBuffer[byteOffset + 1] = middlePixel[1];
                    resultBuffer[byteOffset + 2] = middlePixel[2];
                    resultBuffer[byteOffset + 3] = middlePixel[3];
                }
            }

            var resultBitmap = new Bitmap(sourceBitmap.Width,
                                             sourceBitmap.Height);

            BitmapData resultData =
                       resultBitmap.LockBits(new Rectangle(0, 0,
                       resultBitmap.Width, resultBitmap.Height),
                       ImageLockMode.WriteOnly,
                       PixelFormat.Format32bppArgb);

            Marshal.Copy(resultBuffer, 0, resultData.Scan0,
                                       resultBuffer.Length);

            resultBitmap.UnlockBits(resultData);

            return resultBitmap;
        }
    }  

    class GdiUtil
    {
     

        public static Bitmap ToShadowBitmap(Bitmap sourceBitmap, Color shadowColor)
        {
            var w = sourceBitmap.Width;
            var h = sourceBitmap.Height;
           
            var shadowBitmap = new Bitmap(w, h, PixelFormat.Format32bppArgb);

            var sourceData = sourceBitmap.LockBits(new Rectangle(0, 0, w, h), 
                ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            var shadowData = shadowBitmap.LockBits(new Rectangle(0, 0, w, h), 
                ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            var bytesPerRow = sourceData.Stride;

            for (var y = 0; y < h; y++)
            {
                var sourceRowBytes = new byte[bytesPerRow];
                var shadowRowBytes = new byte[bytesPerRow];

                Marshal.Copy(sourceData.Scan0 + bytesPerRow * y, sourceRowBytes, 0, bytesPerRow);

                for (var i = 0; i < w; i++)
                {
                    var aByte=sourceRowBytes[i*4+3];
                    if (aByte>0)
                    {
                        shadowRowBytes[i * 4 + 3] = (byte) (shadowColor.A * aByte/255);
                        shadowRowBytes[i * 4 + 2] = shadowColor.R;
                        shadowRowBytes[i * 4 + 1] = shadowColor.G;
                        shadowRowBytes[i * 4 + 0] = shadowColor.B;
                    }
                }

                Marshal.Copy(shadowRowBytes,0, (IntPtr)((int)shadowData.Scan0 + bytesPerRow * y), bytesPerRow);
            }

            sourceBitmap.UnlockBits(sourceData);
            shadowBitmap.UnlockBits(shadowData);
            return shadowBitmap;
        }

        public static void DrawWithShadow(Action<Graphics> drawAction, Graphics graphics, 
            int x, int y, int graphicWidth, int graphicHeight,
            int shadowX, int shadowY,
            Color shadowColor)
        {
            const int marginForBlur = 10;
            var shadowBitmap = new Bitmap(graphicWidth + marginForBlur * 2, graphicHeight + marginForBlur*2, PixelFormat.Format32bppArgb);
            using (var tempGraphics = Graphics.FromImage(shadowBitmap))
            {
                tempGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                tempGraphics.SmoothingMode = SmoothingMode.HighQuality;
                tempGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                tempGraphics.Transform = new System.Drawing.Drawing2D.Matrix(1, 0, 0, 1, marginForBlur, marginForBlur);
                drawAction(tempGraphics);
            }
            var blurredShadowBitmap = shadowBitmap.ImageBlurFilter(ExtBitmap.BlurType.Mean5x5);
            var monoColorShadowBitmap = ToShadowBitmap(blurredShadowBitmap, shadowColor);
         
            

            var normalBitmap = new Bitmap(graphicWidth, graphicHeight);
            using (var tempGraphics = Graphics.FromImage(normalBitmap))
            {
                tempGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                tempGraphics.SmoothingMode = SmoothingMode.HighQuality;
                tempGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                drawAction(tempGraphics);
            }

            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;

            graphics.DrawImage(monoColorShadowBitmap, new Rectangle(x + shadowX - marginForBlur,
                y + shadowY - marginForBlur,
                monoColorShadowBitmap.Width,
                monoColorShadowBitmap.Height),
                0, 0, monoColorShadowBitmap.Width, monoColorShadowBitmap.Height, GraphicsUnit.Pixel);

            graphics.DrawImage(normalBitmap, new Rectangle(x, y, normalBitmap.Width, normalBitmap.Height),
               0, 0, normalBitmap.Width, normalBitmap.Height, GraphicsUnit.Pixel);


            blurredShadowBitmap.Dispose();
            shadowBitmap.Dispose();
            monoColorShadowBitmap.Dispose();
            normalBitmap.Dispose();
        }

        //public static void RectangleDropShadow(Graphics tg, Rectangle rc, Color shadowColor, int depth, int maxOpacity)
        //{

        //    //calculate the opacities

        //    Color darkShadow = Color.FromArgb(maxOpacity, shadowColor);

        //    Color lightShadow = Color.FromArgb(0, shadowColor);

        //    //Create a brush that will create a softshadow circle

        //    GraphicsPath gp = new GraphicsPath();

        //    gp.AddEllipse(0, 0, 2*depth, 2*depth);

        //    PathGradientBrush pgb = new PathGradientBrush(gp);

        //    pgb.CenterColor = darkShadow;

        //    pgb.SurroundColors = new Color[] {lightShadow};

        //    //generate a softshadow pattern that can be used to paint the shadow

        //    Bitmap patternbm = new Bitmap(2*depth, 2*depth);

        //    Graphics g = Graphics.FromImage(patternbm);

        //    g.FillEllipse(pgb, 0, 0, 2*depth, 2*depth);

        //    g.Dispose();

        //    pgb.Dispose();

        //    SolidBrush sb = new SolidBrush(Color.FromArgb(maxOpacity, shadowColor));

        //    tg.FillRectangle(sb, rc.Left + depth, rc.Top + depth, rc.Width - (2*depth), rc.Height -
        //                                                                                (2*depth));

        //    sb.Dispose();

        //    //top left corner

        //    tg.DrawImage(patternbm, new Rectangle(rc.Left, rc.Top, depth, depth), 0, 0, depth, depth, GraphicsUnit.Pixel);

        //    //top side

        //    tg.DrawImage(patternbm, new Rectangle(rc.Left + depth, rc.Top, rc.Width - (2*depth), depth), depth, 0, 1,
        //                 depth, GraphicsUnit.Pixel);

        //    //top right corner

        //    tg.DrawImage(patternbm, new Rectangle(rc.Right - depth, rc.Top, depth, depth), depth, 0, depth, depth,
        //                 GraphicsUnit.Pixel);

        //    //right side

        //    tg.DrawImage(patternbm, new Rectangle(rc.Right - depth, rc.Top + depth, depth, rc.Height - (2*depth)), depth,
        //                 depth
        //                 , depth, 1, GraphicsUnit.Pixel);

        //    //bottom left corner

        //    tg.DrawImage(patternbm, new Rectangle(rc.Right - depth, rc.Bottom - depth, depth, depth), depth, depth,
        //                 depth, depth, GraphicsUnit.Pixel);

        //    //bottom side

        //    tg.DrawImage(patternbm, new Rectangle(rc.Left + depth, rc.Bottom - depth, rc.Width - (2*depth), depth),
        //                 depth, depth, 1, depth, GraphicsUnit.Pixel);

        //    //bottom left corner

        //    tg.DrawImage(patternbm, new Rectangle(rc.Left, rc.Bottom - depth, depth, depth), 0, depth, depth, depth,
        //                 GraphicsUnit.Pixel);

        //    //left side

        //    tg.DrawImage(patternbm, new Rectangle(rc.Left, rc.Top + depth, depth, rc.Height - (2*depth)), 0, depth,
        //                 depth, 1, GraphicsUnit.Pixel);

        //    patternbm.Dispose();

        //}

    }
}
