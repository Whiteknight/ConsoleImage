using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace ConsoleImage
{
    // TODO: create a subclass that represents a projection of a cropped region in an image.
    public interface IImage
    {
        Size Size { get; }
        IEnumerable<IImageBuffer> Buffers { get; }
        IImageBuffer CurrentBuffer { get; }
        int NumberOfBuffers { get; }
        void SetCurrentBuffer(int bufferIdx);
        IImageBuffer GetBuffer(int bufferIdx);
    }

    public class Image : IImage
    {
        private readonly ImageBufferSet _buffers;
        private int _currentBuffer;

        public Image(Bitmap bmp, ImageSettings settings)
        {
            _currentBuffer = 0;
            _buffers = new ImageBufferSet(bmp, settings);
        }

        public Size Size
        {
            get { return _buffers.Size; }
        }

        public IEnumerable<IImageBuffer> Buffers
        {
            get { return _buffers; }
        }

        public IImageBuffer CurrentBuffer
        {
            get { return _buffers.GetBuffer(_currentBuffer); }
        }

        public int NumberOfBuffers
        {
            get { return _buffers.Count; }
        }

        public void SetCurrentBuffer(int bufferIdx)
        {
            if (bufferIdx < 0 || bufferIdx >= _buffers.Count)
                throw new IndexOutOfRangeException("The buffer index is out of bounds");
            
            _currentBuffer = bufferIdx;
        }

        public IImageBuffer GetBuffer(int bufferIdx)
        {
            return _buffers.GetBuffer(bufferIdx);
        }

        public class ImageBufferSet : IEnumerable<IImageBuffer>
        {
            private readonly Size _size;
            private readonly Bitmap _bmp;
            private readonly ImageSettings _settings;
            private readonly List<IImageBuffer> _buffers;
            private readonly FrameDimension _frameDimension;
            private readonly int _frameCount;

            public ImageBufferSet(Bitmap bmp, ImageSettings settings)
            {
                _settings = settings;
                _size = CalculateImageSize(bmp, _settings.ImageMaxSize, _settings.ConsoleStart.X, _settings.ConsoleStart.Y);

                _frameDimension = new FrameDimension(bmp.FrameDimensionsList[0]);
                _frameCount = bmp.GetFrameCount(_frameDimension);
                _buffers = new List<IImageBuffer>(_frameCount);
                _bmp = bmp;
            }

            public Size Size
            {
                get { return _size; }
            }

            public IEnumerator<IImageBuffer> GetEnumerator()
            {
                for (int i = 0; i < _frameCount; i++)
                    yield return GetBuffer(i);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public IImageBuffer GetBuffer(int bufferIdx)
            {
                if (bufferIdx < 0 || bufferIdx >= _frameCount)
                    throw new IndexOutOfRangeException("The buffer index is out of bounds");

                IImageBuffer buffer = (bufferIdx < _buffers.Count) ? _buffers[bufferIdx] : null;
                if (buffer == null)
                {
                    buffer = new ImageBuffer(_size);
                    _buffers.Insert(bufferIdx, buffer);
                    _bmp.SelectActiveFrame(_frameDimension, bufferIdx);

                    Color bgColor = _settings.TransparencyColor.GetColor();

                    for (int i = 0; i < _size.Height; i++)
                    {
                        for (int j = 0; j < _size.Width; j++)
                        {
                            Color c = _settings.Sampler.GetSampleColor(_size, _bmp, j, i, bgColor);
                            ConsolePixel pixel = _settings.Converter.CreatePixel(c);
                            buffer.SetPixel(j, i, pixel);
                        }
                    }
                }
                return buffer;
            }

            public int Count
            {
                get { return _frameCount; }
            }

            // TODO: I don't think this belongs here. Find a more appropriate home
            public Size CalculateImageSize(Bitmap original, Size maxSize, int consoleLeft, int consoleTop)
            {
                double ratio = (double)original.Size.Height / (double)original.Size.Width;
                int width = maxSize.Width - consoleLeft;
                int height = (int)(width * ratio);

                if (height + consoleTop > maxSize.Height)
                {
                    height = maxSize.Height - consoleTop;
                    width = (int)(height / ratio);
                }

                //if (settings.ImageHeight <= 0)
                //    settings.ImageHeight = height - settings.ImageTop;
                //if (settings.ImageWidth <= 0)
                //    settings.ImageWidth = width - settings.ImageLeft;

                return new Size(width, height);
            }
        }
    }

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
            {
                int width = size.Value.Width;
                if (width + _start.X > image.Size.Width)
                    width = image.Size.Width - _start.X;

                int height = size.Value.Height;
                if (height + _start.Y > image.Size.Height)
                    height = image.Size.Height - _start.Y;

                if (width <= 0 || height <= 0)
                    throw new ArgumentException("size is invalid", "size");

                _size = new Size(width, height);
            }
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
        public IImageBuffer CurrentBuffer { get { return WrapBuffer(_image.CurrentBuffer); } }
        public int NumberOfBuffers { get { return _image.NumberOfBuffers; } }
        public void SetCurrentBuffer(int bufferIdx)
        {
            _image.SetCurrentBuffer(bufferIdx);
        }

        public IImageBuffer GetBuffer(int bufferIdx)
        {
            return WrapBuffer(_image.GetBuffer(bufferIdx));
        }

        #endregion
    }
}