using System;

namespace ConsoleImage
{
    public class ImageRenderer
    {
        // TODO: Option to draw border
        // TODO: Option to draw caption in various positions

        public void Draw(ImageSettings settings, ImageBuffer buffer)
        {
            ResizeConsoleWindow(settings);

            for (int i = settings.ImageTop; i < settings.ImageHeight + settings.ImageTop; i++)
            {
                Console.SetCursorPosition(settings.ConsoleLeft, settings.ConsoleTop + i - settings.ImageTop);
                DrawRow(settings, buffer, i);
            }
        }

        public void DrawRow(ImageSettings settings, ImageBuffer buffer, int i)
        {
            for (int j = settings.ImageLeft; j < settings.ImageWidth + settings.ImageLeft; j++)
            {
                ConsolePixel p = buffer.GetPixel(j, i);
                p.Print();
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
