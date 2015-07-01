using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ConsoleImage
{
    public class ConsolePixelRepository
    {
        private readonly IReadOnlyList<ConsolePixel> _pixels; 
        public ConsolePixelRepository(bool isGreyscale = false)
        {
            Dictionary<int, ConsolePixel> pixels = new Dictionary<int, ConsolePixel>();

            // Get Grayscale colors
            for (int i = 0; i < 13; i++)
            {
                ConsolePixel p = GetGreyscalePixel(i);
                pixels.Add(p.AsInt, p);
            }

            if (isGreyscale)
            {
                _pixels = pixels.Values.ToList();
                return;
            }

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
                    if (!pixels.ContainsKey(p.AsInt))
                        pixels.Add(p.AsInt, p);
                }

                // Get direct pure-color blends
                for (int j = 0; j < 4; j++)
                {
                    ConsolePixel p = GetBlendColorPixel(colors[i], colors[(i + 1) % colors.Length], j);
                    if (!pixels.ContainsKey(p.AsInt))
                        pixels.Add(p.AsInt, p);

                    p = GetBlendColorPixel(MakeDark(colors[i]), MakeDark(colors[(i + 1) % colors.Length]), j);
                    if (!pixels.ContainsKey(p.AsInt))
                        pixels.Add(p.AsInt, p);
                }

                // Get diagonal bright-dark color blends
                for (int j = 0; j < 4; j++)
                {
                    ConsolePixel p = GetBlendColorPixel(colors[i], MakeDark(colors[(i + 1) % colors.Length]), j);
                    if (!pixels.ContainsKey(p.AsInt))
                        pixels.Add(p.AsInt, p);

                    p = GetBlendColorPixel(colors[i], MakeDark(colors[(i + colors.Length - 1) % colors.Length]), j);
                    if (!pixels.ContainsKey(p.AsInt))
                        pixels.Add(p.AsInt, p);
                }

                // Get Gray-Blends
                for (int j = 0; j < 4; j++)
                {
                    ConsolePixel p = GetBlendColorPixel(colors[i], ConsoleColor.Gray, j);
                    if (!pixels.ContainsKey(p.AsInt))
                        pixels.Add(p.AsInt, p);

                    p = GetBlendColorPixel(MakeDark(colors[i]), ConsoleColor.DarkGray, j);
                    if (!pixels.ContainsKey(p.AsInt))
                        pixels.Add(p.AsInt, p);
                }
            }

            // Add in some special non-adjacent blends where appropriate
            for (int j = 0; j < 4; j++)
            {
                ConsolePixel p = GetBlendColorPixel(ConsoleColor.Green, ConsoleColor.Blue, j);
                if (!pixels.ContainsKey(p.AsInt))
                    pixels.Add(p.AsInt, p);

                p = GetBlendColorPixel(ConsoleColor.DarkGreen, ConsoleColor.DarkBlue, j);
                if (!pixels.ContainsKey(p.AsInt))
                    pixels.Add(p.AsInt, p);
            }

            // Add some "brown" shades
            ConsolePixel brown = GetBlendColorPixel(ConsoleColor.Red, ConsoleColor.Green, 2);
            pixels.Add(brown.AsInt, brown);
            brown = GetBlendColorPixel(ConsoleColor.DarkRed, ConsoleColor.DarkGreen, 2);
            pixels.Add(brown.AsInt, brown);

            _pixels = pixels.Values.ToList();
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

        static ConsolePixel GetPureColorPixel(ConsoleColor baseColor, int i)
        {
            if (i > 13)
                i = 13;
            if (i < 0)
                i = 0;
            const string blocks = " \xB0\xB1\xB2 \xB0\xB1\xB2 \xB0\xB1\xB2 ";
            ConsoleColor bright = MakeBright(baseColor);
            ConsoleColor dark = MakeDark(baseColor);
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

        static ConsoleColor MakeDark(ConsoleColor c)
        {
            return (ConsoleColor)(((int)c) & 0x07);
        }

        static ConsoleColor MakeBright(ConsoleColor c)
        {
            return (ConsoleColor)(((int)c) | 0x08);
        }

        public ConsolePixel GetClosestPixel(Color c)
        {
            var pixels = _pixels.Select(p=> new
            {
                Pixel = p,
                Distance = GetDistance(c, p)
            }).OrderBy(x => x.Distance).Take(10).ToList();
            ConsolePixel pixel = pixels.First().Pixel;
            if (pixel.BackgroundColor == ConsoleColor.DarkRed)
            {
                
            }
            return pixel;
        }

        public ConsolePixel GetFurthestPixel(Color c)
        {
            return _pixels.OrderByDescending(p => GetDistance(c, p)).First();
        }

        public IEnumerable<ConsolePixel> RelatedColors(ConsoleColor cc)
        {
            ConsoleColor c1 = MakeBright(cc);
            return _pixels.Where(p => MakeBright(p.BackgroundColor) == c1 || MakeBright(p.ForegroundColor) == c1);
        }

        public IReadOnlyList<ConsolePixel> AllPixels
        {
            get { return _pixels; }
        }

        private static double GetDistance(Color c, ConsolePixel p)
        {
            double distance = Math.Sqrt(Sqr(c.R - p.Red) + Sqr(c.G - p.Green) + Sqr(c.B - p.Blue));
            return distance;
        }

        private static int Sqr(int x)
        {
            int y = x * x;
            if (y < 50)
                return 0;
            return y;
        }
    }
}
