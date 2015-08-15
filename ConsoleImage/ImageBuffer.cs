using System;
using System.Drawing;

namespace ConsoleImage
{
    public class ImageBuffer
    {
        private readonly ConsolePixel[,] _mBuffer;
        public Size Size { get; private set; }

        public ImageBuffer(Size size)
        {
            if (size.Height <= 0 || size.Width <= 0)
                throw new Exception("Width and height must be strictly positive");

            Size = size;

            _mBuffer = new ConsolePixel[size.Height, size.Width];
        }

        public void SetPixel(int left, int top, ConsolePixel pixel)
        {
            //if (left > _mBuffer.GetLength(0) || left < 0 || top > _mBuffer.GetLength(1) || top < 0)
            //    throw new IndexOutOfRangeException();

            _mBuffer[top, left] = pixel;
        }

        public ConsolePixel GetPixel(int left, int top)
        {
            //if (left > _mBuffer.GetLength(0) || left < 0 || top > _mBuffer.GetLength(1) || top < 0)
            //    throw new IndexOutOfRangeException();

            return _mBuffer[top, left];
        }
    }
}
