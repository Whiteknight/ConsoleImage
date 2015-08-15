using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ConsoleImage
{
    public class ConsolePixelRepository
    {
        private readonly IReadOnlyList<ConsolePixel> _pixels;
        private readonly Dictionary<int, ConsolePixel> _pixelCache;

        public ConsolePixelRepository(IEnumerable<IPixelSource> sources)
        {
            Dictionary<int, ConsolePixel> pixels = new Dictionary<int, ConsolePixel>();
            foreach (ConsolePixel p in sources.SelectMany(s => s.GetPixels()))
            {
                if (!pixels.ContainsKey(p.AsInt))
                    pixels.Add(p.AsInt, p);
            }

            _pixels = pixels.Values.ToList();
            _pixelCache = new Dictionary<int, ConsolePixel>();
        }

        // TODO: Move Color manip routines to separate class
        private Color RoundColor(Color c)
        {
            return Color.FromArgb(c.R & 0xFC, c.G & 0xFC, c.B & 0xFC);
        }

        private int GetColorKey(Color c)
        {
            return (c.R << 16) | (c.G << 8) | c.B;
        }

        public ConsolePixel GetClosestPixel(Color c)
        {
            // TODO: make rounding configurable?
            // TODO: method to clear cache?
            c = RoundColor(c);
            int key = GetColorKey(c);

            if (_pixelCache.ContainsKey(key))
                return _pixelCache[key];

            var pixels = _pixels.Select(p=> new
            {
                Pixel = p,
                Distance = GetDistance(c, p)
            }).OrderBy(x => x.Distance).Take(10).ToList();
            ConsolePixel pixel = pixels.First().Pixel;

            _pixelCache.Add(key, pixel);
            return pixel;
        }

        public ConsolePixel GetFurthestPixel(Color c)
        {
            // TODO: Invert c and call GetClosestPixel
            return _pixels.OrderByDescending(p => GetDistance(c, p)).First();
        }

        public IEnumerable<ConsolePixel> RelatedColors(ConsoleColor cc)
        {
            ConsoleColor c1 = ConsoleColorUtility.MakeBright(cc);
            return _pixels.Where(p => ConsoleColorUtility.MakeBright(p.BackgroundColor) == c1 || ConsoleColorUtility.MakeBright(p.ForegroundColor) == c1);
        }

        public IReadOnlyList<ConsolePixel> AllPixels
        {
            get { return _pixels; }
        }

        private static double GetDistance(Color c, ConsolePixel p)
        {
            double distance = Math.Sqrt(Sqr(c.R - p.RedRounded) + Sqr(c.G - p.GreenRounded) + Sqr(c.B - p.BlueRounded));
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
