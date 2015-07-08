using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace ConsoleImage
{
    public class Image
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

        public IEnumerable<ImageBuffer> Buffers
        {
            get { return _buffers; }
        }

        public ImageBuffer CurrentBuffer
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

        public ImageBuffer GetBuffer(int bufferIdx)
        {
            return _buffers.GetBuffer(bufferIdx);
        }

        public void SetNumberOfBuffers(int numberOfBuffers)
        {
            _buffers.SetNumberOfBuffers(numberOfBuffers);
        }

        public class ImageBufferSet : IEnumerable<ImageBuffer>
        {
            private readonly Size _size;
            private readonly Bitmap _bmp;
            private readonly ImageSettings _settings;
            private readonly List<ImageBuffer> _buffers;
            private readonly FrameDimension _frameDimension;
            private readonly int _frameCount;

            public ImageBufferSet(Bitmap bmp, ImageSettings settings)
            {
                _settings = settings;
                _size = CalculateImageSize(bmp, _settings.ConsoleMaxSize, _settings.ConsoleLeft, _settings.ConsoleTop);

                _frameDimension = new FrameDimension(bmp.FrameDimensionsList[0]);
                _frameCount = bmp.GetFrameCount(_frameDimension);
                _buffers = new List<ImageBuffer>(_frameCount);
                _bmp = bmp;
            }

            public Size Size
            {
                get { return _size; }
            }

            public IEnumerator<ImageBuffer> GetEnumerator()
            {
                for (int i = 0; i < _frameCount; i++)
                    yield return GetBuffer(i);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public ImageBuffer GetBuffer(int bufferIdx)
            {
                if (bufferIdx < 0 || bufferIdx >= _frameCount)
                    throw new IndexOutOfRangeException("The buffer index is out of bounds");

                ImageBuffer buffer = (bufferIdx < _buffers.Count) ? _buffers[bufferIdx] : null;
                if (buffer == null)
                {
                    buffer = new ImageBuffer(_size);
                    _buffers.Insert(bufferIdx, buffer);
                    _bmp.SelectActiveFrame(_frameDimension, bufferIdx);

                    for (int i = 0; i < _size.Height; i++)
                    {
                        for (int j = 0; j < _size.Width; j++)
                        {
                            Color c = _settings.Sampler.GetSampleColor(_size, _bmp, j, i);
                            ConsolePixel pixel = _settings.Converter.CreatePixel(c);
                            buffer.SetPixel(j, i, pixel);
                        }
                    }
                }
                return buffer;
            }

            public int Count
            {
                get { return _buffers.Count; }
            }

            public void SetNumberOfBuffers(int numberOfBuffers)
            {
                if (numberOfBuffers < 1)
                    throw new IndexOutOfRangeException("The number of buffers must be at least 1");

                if (numberOfBuffers < _buffers.Count)
                {
                    _buffers.RemoveRange(numberOfBuffers, _buffers.Count - numberOfBuffers);
                }

                if (numberOfBuffers > _buffers.Count)
                {
                    for (int i = _buffers.Count; i < numberOfBuffers; i++)
                    {
                        _buffers.Add(null);
                    }
                }
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
}