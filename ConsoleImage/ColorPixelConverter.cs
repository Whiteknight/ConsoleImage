using System;
using System.Drawing;

namespace ConsoleImage
{
    public interface IColorPixelConverter
    {
        ConsolePixel CreatePixel(Color c);
    }

    public class BlockColorPixelConverterA : IColorPixelConverter
    {
        private static string charsPercent = " \xB0\xB1\xB2\xB2\xB1\xB0 ";

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
            char ch = '\xB1';

            return new ConsolePixel(bgColor, fgColor, ch);
        }
    }

    public class BlockColorPixelConverterB : IColorPixelConverter
    {
        private readonly ConsolePixelRepository _repository;

        public BlockColorPixelConverterB()
        {
            _repository = new ConsolePixelRepository();
        }

        public ConsolePixel CreatePixel(Color c)
        {
            return _repository.GetClosestPixel(c);
        }
    }
}