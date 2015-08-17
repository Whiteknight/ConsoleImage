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
        int NumberOfBuffers { get; }
        IImageBuffer GetBuffer(int bufferIdx);
    }

    // Effectively a read-only wrapper around ImageBufferSet
    public sealed class Image : IImage, IDisposable
    {
        private readonly ImageBufferSet _imageBuffers;
        private readonly Size _size;
        private readonly int _numberOfBuffers;

        public Image(IImageBufferBuilder builder, Size size)
        {
            _imageBuffers = new ImageBufferSet(builder);
            _size = size;
            _numberOfBuffers = builder.NumberOfBuffers;
        }

        public Size Size
        {
            get { return _size; }
        }

        public IEnumerable<IImageBuffer> Buffers
        {
            get { return _imageBuffers; }
        }

        public int NumberOfBuffers
        {
            get { return _numberOfBuffers; }
        }

        public IImageBuffer GetBuffer(int bufferIdx)
        {
            return _imageBuffers.GetBuffer(bufferIdx);
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            _imageBuffers.Dispose();
        }

        #endregion

        private class ImageBufferSet : IEnumerable<IImageBuffer>, IDisposable
        {
            private IImageBufferBuilder _builder;
            private readonly List<IImageBuffer> _buffers;
            private readonly int _numberOfBuffers;
            private int _numberOfGeneratedBuffers;

            public ImageBufferSet(IImageBufferBuilder builder)
            {
                _builder = builder;
                _numberOfBuffers = builder.NumberOfBuffers;
                _numberOfGeneratedBuffers = 0;
                _buffers = Enumerable.Range(0, _numberOfBuffers).Select<int, IImageBuffer>(x => null).ToList();
            }

            public IEnumerator<IImageBuffer> GetEnumerator()
            {
                for (int i = 0; i < _numberOfBuffers; i++)
                    yield return GetBuffer(i);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public IImageBuffer GetBuffer(int bufferIdx)
            {
                if (bufferIdx < 0 || bufferIdx >= _numberOfBuffers)
                    throw new IndexOutOfRangeException("The buffer index is out of bounds");

                IImageBuffer buffer = (bufferIdx < _buffers.Count) ? _buffers[bufferIdx] : null;
                if (buffer == null)
                {
                    buffer = _builder.Build(bufferIdx);
                    _buffers.Insert(bufferIdx, buffer);
                    _numberOfGeneratedBuffers++;
                    if (_numberOfGeneratedBuffers >= _numberOfBuffers && _buffers.All(ib => ib != null))
                    {
                        _builder.Dispose();
                        _builder = null;
                    }
                }
                return buffer;
            }

            #region Implementation of IDisposable

            public void Dispose()
            {
                if (_builder != null)
                {
                    _builder.Dispose();
                    _builder = null;
                }
            }

            #endregion
        }
    }
}