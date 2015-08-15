using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ConsoleImage
{
    public class ConsolePixelRepository
    {
        private readonly IReadOnlyList<ConsolePixel> _pixels;
        private readonly IReadOnlyList<ConsolePixel> _grayscalePixels;
        private readonly Dictionary<int, ConsolePixel> _pixelCache;

        public ConsolePixelRepository(IEnumerable<IPixelSource> sources)
        {
            Dictionary<int, ConsolePixel> pixels = new Dictionary<int, ConsolePixel>();
            Dictionary<int, ConsolePixel> gsPixels = new Dictionary<int, ConsolePixel>();
            foreach (ConsolePixel p in sources.SelectMany(s => s.GetPixels()))
            {
                if (!pixels.ContainsKey(p.AsInt))
                    pixels.Add(p.AsInt, p);
                if (p.IsGrayscale && !gsPixels.ContainsKey(p.AsInt))
                    gsPixels.Add(p.AsInt, p);
            }

            _pixels = pixels.Values.ToList();
            _grayscalePixels = gsPixels.Values.ToList();
            _pixelCache = new Dictionary<int, ConsolePixel>();
        }

        public ConsolePixel GetClosestPixel(Color c)
        {
            // TODO: make rounding configurable?
            // TODO: method to clear cache?
            c = c.Round();
            int key = c.GetRgbInt();

            if (_pixelCache.ContainsKey(key))
                return _pixelCache[key];

            IReadOnlyList<ConsolePixel> pixels = _pixels;
            if (c.IsGrayscale())
                pixels = _grayscalePixels;

            var w = pixels
                .Select(p => new {
                    Pixel = p,
                    Distance = c.DistanceTo(p.Color)
                })
                .OrderBy(x => x.Distance)
                .ToList();
            ConsolePixel pixel = w[0].Pixel;

            _pixelCache.Add(key, pixel);
            return pixel;
        }

        public ConsolePixel GetFurthestPixel(Color c)
        {
            return GetClosestPixel(c.Invert());
        }

        public IEnumerable<ConsolePixel> RelatedColors(ConsoleColor cc)
        {
            ConsoleColor c1 = cc.MakeBright();
            return _pixels.Where(p => p.BackgroundColor.MakeBright() == c1 || p.ForegroundColor.MakeBright() == c1);
        }

        public IReadOnlyList<ConsolePixel> AllPixels
        {
            get { return _pixels; }
        }
    }
}
