using System;
using System.Drawing;

namespace ConsoleImage
{
    public class ConsoleManager
    {
        private readonly ImageSettings _settings;

        public ConsoleManager(ImageSettings settings)
        {
            _settings = settings;
        }

        // TODO: Move other console-related logic here, like console state save/restore

        public void ResizeConsoleWindow(Image image)
        {
            Size imageSize = _settings.ImageCropSize.HasValue ? _settings.ImageCropSize.Value : image.Size;

            int imageHeight = image.Size.Height;
            if (imageSize.Height > 0 && imageSize.Height < image.Size.Height)
                imageHeight = imageSize.Height;

            int imageWidth = image.Size.Width;
            if (imageSize.Width > 0 && imageSize.Width < image.Size.Width)
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
