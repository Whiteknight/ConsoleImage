using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace ConsoleImage
{
    public interface IImageBufferBuilder : IDisposable
    {
        int NumberOfBuffers { get; }
        IImageBuffer Build(int bufferIdx);
    }

    public class ImageBuilder
    {
        private readonly IImageSampler _sampler;
        private readonly IPixelConverter _converter;
        private readonly Color _transparencyColor;

        public ImageBuilder(IImageSampler sampler, IPixelConverter converter, Color transparencyColor)
        {
            _sampler = sampler;
            _converter = converter;
            _transparencyColor = transparencyColor;
        }

        public Image Build(Bitmap bmp, Size targetSize)
        {
            if (targetSize.Height <= 0 || targetSize.Width <= 0)
                throw new Exception("Width and height must be strictly positive");

            IImageBufferBuilder imageBufferBuilder = new ImageBufferBuilder(bmp, targetSize, _sampler, _converter, _transparencyColor);
            return new Image(imageBufferBuilder, targetSize);
        }

        private class ImageBufferBuilder : IImageBufferBuilder
        {
            private Bitmap _bmp;
            private readonly Size _size;
            private IImageSampler _sampler;
            private IPixelConverter _converter;
            private readonly Color _transparencyColor;
            private FrameDimension _frameDimension;
            private readonly int _frameCount;

            public ImageBufferBuilder(Bitmap bmp, Size size, IImageSampler sampler, IPixelConverter converter, Color transparencyColor)
            {
                _bmp = bmp;
                _size = size;
                _sampler = sampler;
                _converter = converter;
                _transparencyColor = transparencyColor;

                _frameDimension = new FrameDimension(bmp.FrameDimensionsList[0]);
                _frameCount = bmp.GetFrameCount(_frameDimension);
            }

            #region Implementation of IDisposable

            public void Dispose()
            {
                if (_bmp != null)
                {
                    _bmp.Dispose();
                    _bmp = null;
                    _frameDimension = null;
                    _sampler = null;
                    _converter = null;
                }
            }

            #endregion

            #region Implementation of IImageBufferBuilder

            public int NumberOfBuffers { get { return _frameCount; } }
            public IImageBuffer Build(int bufferIdx)
            {
                return BuildBuffer(_size, _bmp, _frameDimension, bufferIdx);
            }

            #endregion

            private IImageBuffer BuildBuffer(Size size, Bitmap bmp, FrameDimension frameDimension, int idx)
            {
                ConsolePixel[,] buffer = new ConsolePixel[size.Height, size.Width];
                bmp.SelectActiveFrame(frameDimension, idx);

                for (int i = 0; i < size.Height; i++)
                {
                    for (int j = 0; j < size.Width; j++)
                    {
                        Color c = _sampler.GetSampleColor(size, bmp, j, i, _transparencyColor);
                        ConsolePixel pixel = _converter.CreatePixel(c);
                        buffer[i, j] = pixel;
                    }
                }

                return new ImageBuffer(size, buffer);
            }
        }

        private class ImageBuffer : IImageBuffer
        {
            private readonly ConsolePixel[,] _buffer;
            public Size Size { get; private set; }

            public ImageBuffer(Size size, ConsolePixel[,] buffer)
            {
                _buffer = buffer;
                Size = size;
            }

            public ConsolePixel GetPixel(int left, int top)
            {
                // TODO: Bounds checking
                //if (left > _buffer.GetLength(0) || left < 0 || top > _buffer.GetLength(1) || top < 0)
                //    throw new IndexOutOfRangeException();

                return _buffer[top, left];
            }

            public IEnumerable<ConsolePixel> GetRow(int top)
            {
                for (int i = 0; i < Size.Width; i++)
                    yield return _buffer[top, i];
            }
        }
    }
}
