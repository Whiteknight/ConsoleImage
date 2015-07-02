using System;
using System.Drawing;

namespace ConsoleImage
{
    // TODO: Bounds checking, etc
    public class ImageBuffer
    {
        private readonly int m_width;
        private readonly int m_height;
        private readonly ConsolePixel[,] m_buffer;

        public ImageBuffer(Size size)
        {
            m_width = size.Width;
            m_height = size.Height;
            m_buffer = new ConsolePixel[m_height, m_width];
        }

        public ImageBuffer(int width, int height)
        {
            m_width = width;
            m_height = height;
            m_buffer = new ConsolePixel[height, width];
        }

        public void SetPixel(int left, int top, ConsolePixel pixel)
        {
            m_buffer[top, left] = pixel;
        }

        public ConsolePixel GetPixel(int left, int top)
        {
            return m_buffer[top, left];
        }

    }
}
