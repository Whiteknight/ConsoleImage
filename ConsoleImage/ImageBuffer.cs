using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ConsoleImage
{
    public interface IImageBuffer
    {
        Size Size { get; }
        ConsolePixel GetPixel(int left, int top);
        IEnumerable<ConsolePixel> GetRow(int top);
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
