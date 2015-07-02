using System;
using System.Collections.Generic;
using System.Drawing;

namespace ConsoleImage
{
    public interface IColorPixelConverter
    {
        ConsolePixel CreatePixel(Color c);
    }

    public class SimpleColorPixelConverterA : IColorPixelConverter
    {
        public ConsolePixel CreatePixel(Color c)
        {
            int rBright = c.R / 43;
            int gBright = c.G / 43;
            int bBright = c.B / 43;

            int rBgComp = (rBright < 1 ? 0 : 4) | (rBright >= 4 ? 8 : 0);
            int gBgComp = (gBright < 1 ? 0 : 2) | (gBright >= 4 ? 8 : 0);
            int bBgComp = (bBright < 1 ? 0 : 1) | (bBright >= 4 ? 8 : 0);

            ConsoleColor bgColor = (ConsoleColor)(rBgComp | gBgComp | bBgComp);

            int rFgComp = (rBright < 2 ? 0 : 4) | (rBright >= 5 ? 8 : 0);
            int gFgComp = (gBright < 2 ? 0 : 2) | (gBright >= 5 ? 8 : 0);
            int bFgComp = (bBright < 2 ? 0 : 1) | (bBright >= 5 ? 8 : 0);

            ConsoleColor fgColor = (ConsoleColor)(rFgComp | gFgComp | gFgComp);
            char ch = ' ';

            return new ConsolePixel(bgColor, fgColor, ch);
        }
    }

    public class SearchColorPixelConverter : IColorPixelConverter
    {
        private readonly ConsolePixelRepository _repository;

        public SearchColorPixelConverter()
        {
            _repository = new ConsolePixelRepository(new List<IPixelSource> {
                new GrayscalePixelSource(),
                new ColorsPixelSource()
            });
        }

        public ConsolePixel CreatePixel(Color c)
        {
            return _repository.GetClosestPixel(c);
        }
    }

    public class GreyscaleColorPixelConverterB : IColorPixelConverter
    {
        private readonly ConsolePixelRepository _repository;

        public GreyscaleColorPixelConverterB()
        {
            _repository = new ConsolePixelRepository(new List<IPixelSource> {
                new GrayscalePixelSource(),
                new ColorsPixelSource()
            });
        }

        public ConsolePixel CreatePixel(Color c)
        {
            return _repository.GetClosestPixel(c);
        }
    }

    public class InvertBlockColorPixelConverterB : IColorPixelConverter
    {
        private readonly ConsolePixelRepository _repository;

        public InvertBlockColorPixelConverterB()
        {
            _repository = new ConsolePixelRepository(new List<IPixelSource> {
                new GrayscalePixelSource(),
                new ColorsPixelSource()
            });
        }

        public ConsolePixel CreatePixel(Color c)
        {
            return _repository.GetClosestPixel(c).Invert();
        }
    }
}