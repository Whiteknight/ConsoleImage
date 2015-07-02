using System;
using System.Drawing;

namespace ConsoleImage
{
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

        public void Draw(ImageSettings settings)
        {
            ResizeConsoleWindow(settings);

            for (int i = settings.ImageTop; i < settings.ImageHeight + settings.ImageTop; i++)
            {
                Console.SetCursorPosition(settings.ConsoleLeft, settings.ConsoleTop + i - settings.ImageTop);
                for (int j = settings.ImageLeft; j < settings.ImageWidth + settings.ImageLeft; j++)
                {
                    ConsolePixel p = m_buffer[i, j];
                    p.Print();
                }
            }
        }

        private static void ResizeConsoleWindow(ImageSettings settings)
        {
            int minConsoleHeight = settings.ImageHeight + settings.ConsoleTop;
            if (minConsoleHeight > settings.ConsoleMaxHeight)
                minConsoleHeight = settings.ConsoleMaxHeight;
            if (Console.WindowHeight < minConsoleHeight)
                Console.WindowHeight = minConsoleHeight;

            int minConsoleWidth = settings.ImageWidth + settings.ConsoleLeft;
            if (minConsoleWidth > settings.ConsoleMaxWidth)
                minConsoleWidth = settings.ConsoleMaxWidth;
            if (Console.WindowWidth < minConsoleWidth)
                Console.WindowWidth = minConsoleWidth;
        }
    }
}
