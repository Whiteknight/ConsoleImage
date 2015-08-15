using System;
using System.Drawing;

namespace ConsoleImage
{
    public static class ColorExtensions
    {
        // Chop off the low bits of each byte to decrease search-space for colors
        public static Color Round(this Color c)
        {
            return Color.FromArgb(c.R & 0xFC, c.G & 0xFC, c.B & 0xFC);
        }

        // Represent the Color as an integer
        public static int GetRgbInt(this Color c)
        {
            return (c.R << 16) | (c.G << 8) | c.B;
        }

        // Calculate the distance between two colors in color-space. Smaller results mean more
        // similar colors
        public static double DistanceTo(this Color c, Color p)
        {
            return Math.Sqrt(Sqr(c.R - p.R) + Sqr(c.G - p.G) + Sqr(c.B - p.B));
        }

        private static int Sqr(int x)
        {
            return x * x;
        }

        public static byte Brightness(this Color c)
        {
            return (byte)((c.R + c.G + c.B) / 3); 
        }

        public static Color Invert(this Color c)
        {
            return Color.FromArgb(255 - c.R, 255 - c.G, 255 - c.B);
        }

        public static Color BlendTransparency(this Color fg, Color bg)
        {
            return Blend(fg, bg, (fg.A / 255.0));
        }

        public static Color Blend(this Color c1, Color c2, double percent)
        {
            int r = BlendColorComponent(c1.R, c2.R, percent);
            int g = BlendColorComponent(c1.G, c2.G, percent);
            int b = BlendColorComponent(c1.B, c2.B, percent);
            return Color.FromArgb(r, g, b);
        }

        private static byte BlendColorComponent(int comp1, int comp2, double percent)
        {
            int part1 = (int)(comp1 * percent);
            int part2 = (int)(comp2 * (1.0 - percent));
            int total = part1 + part2;
            if (total > 255)
                return 255;
            if (total < 0)
                return 0;
            return (byte)total;
        }

        public static bool IsGrayscale(this Color c)
        {
            if (c.R - c.G < 5 && c.R - c.B < 5)
                return true;
            return false;
        }
    }
}
