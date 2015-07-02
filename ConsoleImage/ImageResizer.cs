using System.Drawing;

namespace ConsoleImage
{
    public class ImageResizer
    {
        public Size CalculateImageSize(Bitmap original, ImageSettings settings)
        {
            double ratio = (double)original.Size.Height / (double)original.Size.Width;
            int width = settings.ConsoleMaxWidth - settings.ConsoleLeft;
            int height = (int)(width * ratio);

            if (height + settings.ConsoleTop > settings.ConsoleMaxHeight)
            {
                height = settings.ConsoleMaxHeight - settings.ConsoleTop;
                width = (int)(height / ratio);
            }

            if (settings.ImageHeight <= 0)
                settings.ImageHeight = height - settings.ImageTop;
            if (settings.ImageWidth <= 0)
                settings.ImageWidth = width - settings.ImageLeft;

            return new Size(width, height);
        }

        public Bitmap Resize(Bitmap original, ImageSettings settings)
        {
            Size size = CalculateImageSize(original, settings);

            return new Bitmap(original, size);
        }
    }
}
