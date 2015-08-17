using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ConsoleImage
{
    public class ImageRegion : IImage
    {
        private readonly IImage _image;
        private readonly Point _start;
        private readonly Size _size;

        public ImageRegion(IImage image, Point? start, Size? size)
        {
            _image = image;
            _start = start.HasValue ? start.Value : new Point(0, 0);
            if (!size.HasValue)
                _size = new Size(image.Size.Width - _start.X, image.Size.Height - _start.Y);
            else
                _size = image.Size.BestFitWithin(_start, size.Value);
            // TODO: Check that _start and _size are valid for the image.
        }

        private IImageBuffer WrapBuffer(IImageBuffer buffer)
        {
            if (buffer == null)
                return null;
            return new ImageBufferRegion(buffer, _start, _size);
        }

        #region Implementation of IImage

        public Size Size { get { return _size; } }
        public IEnumerable<IImageBuffer> Buffers { get { return _image.Buffers.Select(WrapBuffer); } }
        public int NumberOfBuffers { get { return _image.NumberOfBuffers; } }

        public IImageBuffer GetBuffer(int bufferIdx)
        {
            return WrapBuffer(_image.GetBuffer(bufferIdx));
        }

        #endregion
    }
}