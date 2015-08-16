using System;
using System.Drawing;
using System.Text;

namespace ConsoleImage
{
    public class ConsoleManager
    {
        public static Size MaxSize
        {
            get { return new Size(Console.LargestWindowWidth, Console.LargestWindowHeight); }
        }

        private readonly ImageSettings _settings;

        public ConsoleManager(ImageSettings settings)
        {
            _settings = settings;
        }

        public ConsoleState SaveConsoleState()
        {
            return ConsoleState.GetState();
        }

        public void SetForGraphics()
        {
            Console.OutputEncoding = Encoding.GetEncoding(1252);
        }

        // TODO: Move other console-related logic here, like console state save/restore

        public void ResizeConsoleWindow(Size size)
        {
            Size imageSize = _settings.ImageCropSize.HasValue ? _settings.ImageCropSize.Value : size;

            int imageHeight = size.Height;
            if (imageSize.Height > 0 && imageSize.Height < size.Height)
                imageHeight = imageSize.Height;

            int imageWidth = size.Width;
            if (imageSize.Width > 0 && imageSize.Width < size.Width)
                imageWidth = imageSize.Width;

            int minConsoleHeight = imageHeight + _settings.ConsoleStart.Y;
            if (minConsoleHeight > _settings.ImageMaxSize.Height)
                minConsoleHeight = _settings.ImageMaxSize.Height;
            if (Console.WindowHeight < minConsoleHeight)
                Console.WindowHeight = minConsoleHeight;

            int minConsoleWidth = imageWidth + _settings.ConsoleStart.X;
            if (minConsoleWidth > _settings.ImageMaxSize.Width)
                minConsoleWidth = _settings.ImageMaxSize.Width;
            if (Console.WindowWidth < minConsoleWidth)
                Console.WindowWidth = minConsoleWidth;
        }
    }
}
