using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ConsoleImage
{
    public class Image
    {
        private readonly Size _size;
        private readonly List<ImageBuffer> _buffers;
        private int _currentBuffer;

        public Image(Size size)
        {
            _size = size;
            _currentBuffer = 0;
            _buffers = new List<ImageBuffer>
            {
                new ImageBuffer(size)
            };
        }

        public Size Size
        {
            get { return _size; }
        }

        public IEnumerable<ImageBuffer> Buffers
        {
            get { return _buffers.AsEnumerable(); }
        }

        public ImageBuffer CurrentBuffer
        {
            get { return _buffers[_currentBuffer]; }
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
            if (bufferIdx < 0 || bufferIdx >= _buffers.Count)
                throw new IndexOutOfRangeException("The buffer index is out of bounds");

            return _buffers[bufferIdx];
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
                    _buffers.Add(new ImageBuffer(_size));
                }
            }
        }
    }
}