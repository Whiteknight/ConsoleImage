using System;
using System.Drawing;

namespace ConsoleImage
{
    public static class SizeExtensions
    {
        public static Size BestFitWithin(this Size outerSize, Point start, Size innerSize)
        {
            int width = innerSize.Width;
            if (width + start.X > outerSize.Width)
                width = outerSize.Width - start.X;

            int height = innerSize.Height;
            if (height + start.Y > outerSize.Height)
                height = outerSize.Height - start.Y;

            if (width <= 0 || height <= 0)
                throw new ArgumentException("innerSize is invalid", "innerSize");

            return new Size(width, height);
        }
    }
}
