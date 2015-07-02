using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ConsoleImage
{
    public class ConsolePixelRepository
    {
        private readonly IReadOnlyList<ConsolePixel> _pixels; 
        public ConsolePixelRepository(IEnumerable<IPixelSource> sources)
        {
            Dictionary<int, ConsolePixel> pixels = new Dictionary<int, ConsolePixel>();
            foreach (ConsolePixel p in sources.SelectMany(s => s.GetPixels()))
            {
                if (!pixels.ContainsKey(p.AsInt))
                    pixels.Add(p.AsInt, p);
            }

            _pixels = pixels.Values.ToList();
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
            ConsoleColor c1 = ConsoleColorUtility.MakeBright(cc);
            return _pixels.Where(p => ConsoleColorUtility.MakeBright(p.BackgroundColor) == c1 || ConsoleColorUtility.MakeBright(p.ForegroundColor) == c1);
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
