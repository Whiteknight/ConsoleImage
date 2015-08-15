using System;
using System.Collections.Generic;

namespace ConsoleImage
{
    public interface IPixelSource
    {
        IEnumerable<ConsolePixel> GetPixels();
    }

    public class GrayscalePixelSource : IPixelSource
    {
        #region Implementation of IPixelSource

        public IEnumerable<ConsolePixel> GetPixels()
        {
            for (int i = 0; i < 13; i++)
            {
                ConsolePixel p = GetGreyscalePixel(i);
                yield return p;
            }
        }

        static ConsolePixel GetGreyscalePixel(int i)
        {
            if (i > 12)
                i = 12;
            if (i < 0)
                i = 0;

            ConsoleColor[] greyscales = new ConsoleColor[] { ConsoleColor.Black, ConsoleColor.DarkGray, ConsoleColor.Gray, ConsoleColor.White, ConsoleColor.White, ConsoleColor.White };
            const string blocks = " \xB0\xB1\xB2 \xB0\xB1\xB2 \xB0\xB1\xB2 ";
            int gidx = i / 4;
            ConsoleColor bg = greyscales[gidx];
            ConsoleColor fg = greyscales[gidx + 1];
            char c = blocks[i];
            return new ConsolePixel(bg, fg, c);
        }

        #endregion
    }

    public class ColorsPixelSource : IPixelSource
    {
        #region Implementation of IPixelSource

        public IEnumerable<ConsolePixel> GetPixels()
        {
            ConsoleColor[] colors = {
                ConsoleColor.Magenta, 
                ConsoleColor.Red, 
                ConsoleColor.Yellow, 
                ConsoleColor.Green, 
                ConsoleColor.Cyan, 
                ConsoleColor.Blue
            };

            for (int i = 0; i < colors.Length; i++)
            {
                // Get the pure colors
                for (int j = 1; j < 12; j++)
                {
                    ConsolePixel p = GetPureColorPixel(colors[i], j);
                    yield return p;
                }

                // Get direct pure-color blends
                for (int j = 0; j < 4; j++)
                {
                    ConsolePixel p = GetBlendColorPixel(colors[i], colors[(i + 1) % colors.Length], j);
                    yield return p;

                    p = GetBlendColorPixel(ConsoleColorExtensions.MakeDark(colors[i]), ConsoleColorExtensions.MakeDark(colors[(i + 1) % colors.Length]), j);
                    yield return p;
                }

                // Get diagonal bright-dark color blends
                for (int j = 0; j < 4; j++)
                {
                    ConsolePixel p = GetBlendColorPixel(colors[i], ConsoleColorExtensions.MakeDark(colors[(i + 1) % colors.Length]), j);
                    yield return p;

                    p = GetBlendColorPixel(colors[i], ConsoleColorExtensions.MakeDark(colors[(i + colors.Length - 1) % colors.Length]), j);
                    yield return p;
                }

                // Get Gray-Blends
                for (int j = 0; j < 4; j++)
                {
                    ConsolePixel p = GetBlendColorPixel(colors[i], ConsoleColor.Gray, j);
                    yield return p;

                    p = GetBlendColorPixel(ConsoleColorExtensions.MakeDark(colors[i]), ConsoleColor.DarkGray, j);
                    yield return p;
                }
            }

            // Add in some special non-adjacent blends where appropriate
            for (int j = 0; j < 4; j++)
            {
                ConsolePixel p = GetBlendColorPixel(ConsoleColor.Green, ConsoleColor.Blue, j);
                yield return p;

                p = GetBlendColorPixel(ConsoleColor.DarkGreen, ConsoleColor.DarkBlue, j);
                yield return p;
            }

            // Add some "brown" shades
            ConsolePixel brown = GetBlendColorPixel(ConsoleColor.Red, ConsoleColor.Green, 2);
            yield return brown;
            brown = GetBlendColorPixel(ConsoleColor.DarkRed, ConsoleColor.DarkGreen, 2);
            yield return brown;
        }

        static ConsolePixel GetPureColorPixel(ConsoleColor baseColor, int i)
        {
            if (i > 13)
                i = 13;
            if (i < 0)
                i = 0;
            const string blocks = " \xB0\xB1\xB2 \xB0\xB1\xB2 \xB0\xB1\xB2 ";
            ConsoleColor bright = ConsoleColorExtensions.MakeBright(baseColor);
            ConsoleColor dark = ConsoleColorExtensions.MakeDark(baseColor);
            ConsoleColor[] scales = new[] { ConsoleColor.Black, dark, bright, ConsoleColor.White, ConsoleColor.White };
            int gidx = i / 4;
            ConsoleColor bg = scales[gidx];
            ConsoleColor fg = scales[gidx + 1];
            char c = blocks[i];
            return new ConsolePixel(bg, fg, c);
        }

        static ConsolePixel GetBlendColorPixel(ConsoleColor c1, ConsoleColor c2, int i)
        {
            if (i > 4)
                i = 4;
            if (i < 0)
                i = 0;
            const string blocks = " \xB0\xB1\xB2";
            if (i == 4)
                return new ConsolePixel(c2, c1, ' ');
            return new ConsolePixel(c1, c2, blocks[i]);
        }

        #endregion
    }
}