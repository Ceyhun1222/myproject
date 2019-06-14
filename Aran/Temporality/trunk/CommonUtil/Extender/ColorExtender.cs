﻿using System;
using System.Diagnostics;
using System.Windows.Media;

namespace Aran.Temporality.CommonUtil.Extender
{
    public static class ColorExtender
    {
        public static Color GetPastelShade(this Color source)
        {
            return (GenerateColor(source, true, new Hsb { H = 0, S = 0.2d, B = 255 }, new Hsb { H = 360, S = 0.5d, B = 255 }));
        }

        public static Color GetRandom(this Color source)
        {
            return (GenerateColor(source, false, new Hsb { H = 0, S = 0, B = 0 }, new Hsb { H = 360, S = 1, B = 255 }));
        }

        public static Color GetRandom(this Color source, double minBrightness, double maxBrightness)
        {
            if (minBrightness >= 0 && maxBrightness <= 1)
            {
                return (GenerateColor(source, false, new Hsb { H = 0, S = 1 * minBrightness, B = 255 }, new Hsb { H = 360, S = 1 * maxBrightness, B = 255 }));
            }
            
            throw new ArgumentOutOfRangeException();
        }

        public static Color GetRandomShade(this Color source)
        {
            return (GenerateColor(source, true, new Hsb { H = 0, S = 1, B = 0 }, new Hsb { H = 360, S = 1, B = 255 }));
        }

        public static Color GetRandomShade(this Color source, double minBrightness, double maxBrightness)
        {
            if (minBrightness >= 0 && maxBrightness <= 1)
            {
                return (GenerateColor(source, true, new Hsb { H = 0, S = 1 * minBrightness, B = 255 }, new Hsb { H = 360, S = 1 * maxBrightness, B = 255 }));
            }

            throw new ArgumentOutOfRangeException();
        }

        private static Color GenerateColor(Color source, bool isaShadeOfSource, Hsb min, Hsb max)
        {
            var hsbValues = ConvertToHsb(new Rgb { R = source.R, G = source.G, B = source.B });
            var hDouble = Randomizer.NextDouble();
            var sDouble = Randomizer.NextDouble();
            var bDouble = Randomizer.NextDouble();
            if (max.B - min.B == 0) bDouble = 0; //do not change Brightness
            if (isaShadeOfSource)
            {
                min.H = hsbValues.H;
                max.H = hsbValues.H;
                hDouble = 0;
            }
            hsbValues = new Hsb
            {
                H = Convert.ToDouble(Randomizer.Next(Convert.ToInt32(min.H), Convert.ToInt32(max.H))) + hDouble,
                S = Convert.ToDouble((Randomizer.Next(Convert.ToInt32(min.S * 100), Convert.ToInt32(max.S * 100))) / 100d),
                B = Convert.ToDouble(Randomizer.Next(Convert.ToInt32(min.B), Convert.ToInt32(max.B))) + bDouble
            };
            Debug.WriteLine("H:{0} | S:{1} | B:{2} [Min_S:{3} | Max_S{4}]", hsbValues.H, hsbValues.S, hsbValues.B, min.S, max.S);
            var rgbvalues = ConvertToRgb(hsbValues);
            return new Color { A = source.A, R = (byte)rgbvalues.R, G = (byte)rgbvalues.G, B = (byte)rgbvalues.B };
        }

        static readonly Random Randomizer = new Random();
        public static Color GetContrast(this Color source, bool preserveOpacity)
        {
            var inputColor = source;
            //if RGB values are close to each other by a diff less than 10%, then if RGB values are lighter side, decrease the blue by 50% (eventually it will increase in conversion below), if RBB values are on darker side, decrease yellow by about 50% (it will increase in conversion)
            var avgColorValue = (byte)((source.R + source.G + source.B) / 3);
            var diffR = Math.Abs(source.R - avgColorValue);
            var diffG = Math.Abs(source.G - avgColorValue);
            var diffB = Math.Abs(source.B - avgColorValue);
            if (diffR < 20 && diffG < 20 && diffB < 20) //The color is a shade of gray
            {
                if (avgColorValue < 123) //color is dark
                {
                    inputColor.B = 220;
                    inputColor.G = 230;
                    inputColor.R = 50;
                }
                else
                {
                    inputColor.R = 255;
                    inputColor.G = 255;
                    inputColor.B = 50;
                }
            }
            var sourceAlphaValue = source.A;
            if (!preserveOpacity)
            {
                sourceAlphaValue = Math.Max(source.A, (byte)127); //We don't want contrast color to be more than 50% transparent ever.
            }
            var rgb = new Rgb { R = inputColor.R, G = inputColor.G, B = inputColor.B };
            var hsb = ConvertToHsb(rgb);
            hsb.H = hsb.H < 180 ? hsb.H + 180 : hsb.H - 180;
            //_hsb.B = _isColorDark ? 240 : 50; //Added to create dark on light, and light on dark
            rgb = ConvertToRgb(hsb);
            return new Color { A = sourceAlphaValue, R = (byte)rgb.R, G = (byte)rgb.G, B = (byte)rgb.B };
        }
        internal static Rgb ConvertToRgb(Hsb hsb)
        {
            // By: <a href="http://blogs.msdn.com/b/codefx/archive/2012/02/09/create-a-color-picker-for-windows-phone.aspx" title="MSDN" target="_blank">Yi-Lun Luo</a>
            var chroma = hsb.S * hsb.B;
            var hue2 = hsb.H / 60;
            var x = chroma * (1 - Math.Abs(hue2 % 2 - 1));
            var r1 = 0d;
            var g1 = 0d;
            var b1 = 0d;
            if (hue2 >= 0 && hue2 < 1)
            {
                r1 = chroma;
                g1 = x;
            }
            else if (hue2 >= 1 && hue2 < 2)
            {
                r1 = x;
                g1 = chroma;
            }
            else if (hue2 >= 2 && hue2 < 3)
            {
                g1 = chroma;
                b1 = x;
            }
            else if (hue2 >= 3 && hue2 < 4)
            {
                g1 = x;
                b1 = chroma;
            }
            else if (hue2 >= 4 && hue2 < 5)
            {
                r1 = x;
                b1 = chroma;
            }
            else if (hue2 >= 5 && hue2 <= 6)
            {
                r1 = chroma;
                b1 = x;
            }
            var m = hsb.B - chroma;
            return new Rgb
                       {
                R = r1 + m,
                G = g1 + m,
                B = b1 + m
            };
        }
        internal static Hsb ConvertToHsb(Rgb rgb)
        {
            var r = rgb.R;
            var g = rgb.G;
            var b = rgb.B;

            var max = Max(r, g, b);
            var min = Min(r, g, b);
            var chroma = max - min;
            var hue2 = 0d;
            if (chroma != 0)
            {
                if (max == r)
                {
                    hue2 = (g - b) / chroma;
                }
                else if (max == g)
                {
                    hue2 = (b - r) / chroma + 2;
                }
                else
                {
                    hue2 = (r - g) / chroma + 4;
                }
            }
            var hue = hue2 * 60;
            if (hue < 0)
            {
                hue += 360;
            }
            var brightness = max;
            double saturation = 0;
            if (chroma != 0)
            {
                saturation = chroma / brightness;
            }
            return new Hsb
                       {
                H = hue,
                S = saturation,
                B = brightness
            };
        }
        private static double Max(double d1, double d2, double d3)
        {
            return Math.Max(d1 > d2 ? d1 : d2, d3);
        }

        private static double Min(double d1, double d2, double d3)
        {
            return Math.Min(d1 < d2 ? d1 : d2, d3);
        }

        internal struct Rgb
        {
            internal double R;
            internal double G;
            internal double B;
        }
        internal struct Hsb
        {
            internal double H;
            internal double S;
            internal double B;
        }
    }
}
