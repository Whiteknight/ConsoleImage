using System;
using System.Drawing;

namespace ConsoleImage
{
    public class ImageRenderer
    {
        // TODO: Option to draw border
        // TODO: Option to draw caption in various positions
        // TODO: Move console resizing logic out of this class. Assume the console is the correct size here, or follow
        // existing console bounds only.

        private readonly ImageSettings _settings;

        public ImageRenderer(ImageSettings settings)
        {
            _settings = settings;
        }

        public void Draw(Image image)
        {
            Draw(image.Size, image.CurrentBuffer);
        }

        public void Draw(Image image, int bufferIdx)
        {
            Draw(image.Size, image.GetBuffer(bufferIdx));
        }

        public void Draw(Size size, ImageBuffer imageBuffer)
        {
            for (int i = 0; i < size.Height; i++)
            {
                Console.SetCursorPosition(_settings.ConsoleLeft, _settings.ConsoleTop + i - _settings.ImageTop);
                DrawRow(size, imageBuffer, i);
            }
        }

        public void DrawRow(Size size, Image image, int bufferIdx, int rowIdx)
        {
            DrawRow(size, image.GetBuffer(bufferIdx), rowIdx);
        }

        public void DrawRow(Size size, ImageBuffer imageBuffer, int rowIdx)
        {
            for (int j = 0; j < size.Width; j++)
            {
                ConsolePixel p = imageBuffer.GetPixel(j, rowIdx);
                p.Print();
            }
        }

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
