using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ConsoleImage
{
    public interface IImageBuffer
    {
        Size Size { get; }
        void SetPixel(int left, int top, ConsolePixel pixel);
        ConsolePixel GetPixel(int left, int top);
        IEnumerable<ConsolePixel> GetRow(int top);
    }

    // TODO: Rearrange this class to be immutable. Remove the SetPixel method and have the constructor
    // take the raw buffer array instead. Use ImageBuilder.
    public class ImageBuffer : IImageBuffer
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

        public IEnumerable<ConsolePixel> GetRow(int top)
        {
            for (int i = 0; i < Size.Width; i++)
                yield return _mBuffer[top, i];
        }
    }

    public class ImageBufferRegion : IImageBuffer
    {
        private readonly IImageBuffer _buffer;
        private readonly Point _start;
        private readonly Size _size;

        public ImageBufferRegion(IImageBuffer buffer, Point start, Size size)
        {
            _buffer = buffer;
            _start = start;
            _size = size;

            // TODO: Validate _start and _size
        }

        #region Implementation of IImageBuffer

        public Size Size { get { return _size; } }
        public void SetPixel(int left, int top, ConsolePixel pixel)
        {
            throw new NotImplementedException();
        }

        public ConsolePixel GetPixel(int left, int top)
        {
            if (left >= _size.Width || left < 0)
                throw new ArgumentOutOfRangeException("left");
            if (top >= _size.Height || top < 0)
                throw new ArgumentOutOfRangeException("top");

            left = left + _start.X;
            top = top + _start.Y;
            return _buffer.GetPixel(left, top);
        }

        public IEnumerable<ConsolePixel> GetRow(int top)
        {
            return _buffer.GetRow(top + _start.Y).Skip(_start.X).Take(_size.Width);
        }

        #endregion
    }
}
