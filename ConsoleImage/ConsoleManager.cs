using System;

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
            int imageHeight = image.Size.Height;
            if (_settings.ImageCropSize.Height > 0 && _settings.ImageCropSize.Height < image.Size.Height)
                imageHeight = _settings.ImageCropSize.Height;

            int imageWidth = image.Size.Width;
            if (_settings.ImageCropSize.Width > 0 && _settings.ImageCropSize.Width < image.Size.Width)
                imageWidth = _settings.ImageCropSize.Width;

            int minConsoleHeight = imageHeight + _settings.ConsoleTop;
            if (minConsoleHeight > _settings.ConsoleMaxSize.Height)
                minConsoleHeight = _settings.ConsoleMaxSize.Height;
            if (Console.WindowHeight < minConsoleHeight)
                Console.WindowHeight = minConsoleHeight;

            int minConsoleWidth = imageWidth + _settings.ConsoleLeft;
            if (minConsoleWidth > _settings.ConsoleMaxSize.Width)
                minConsoleWidth = _settings.ConsoleMaxSize.Width;
            if (Console.WindowWidth < minConsoleWidth)
                Console.WindowWidth = minConsoleWidth;
        }
    }
}
