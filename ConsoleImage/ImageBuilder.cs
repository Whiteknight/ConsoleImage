using System.Drawing;
using System.Drawing.Imaging;

namespace ConsoleImage
{
    public class ImageBuilder
    {
        private readonly IImageSampler _sampler;
        private readonly IPixelConverter _converter;
        private readonly ImageSettings _settings;

        public ImageBuilder(IImageSampler sampler, IPixelConverter converter, ImageSettings settings)
        {
            _sampler = sampler;
            _converter = converter;
            _settings = settings;
        }

        public Image Build(Bitmap bmp)
        {
            FrameDimension dimension = new FrameDimension(bmp.FrameDimensionsList[0]);
            int frameCount = bmp.GetFrameCount(dimension);

            Size size = CalculateImageSize(bmp, _settings.ConsoleMaxSize, _settings.ConsoleLeft, _settings.ConsoleTop);

            Image image = new Image(size);
            image.SetNumberOfBuffers(frameCount);

            for (int i = 0; i < frameCount; i++)
            {
                bmp.SelectActiveFrame(dimension, i);
                RenderFrame(size, image.GetBuffer(i), bmp);
            }

            image.SetCurrentBuffer(0);
            return image;
        }

        private void RenderFrame(Size targetSize, ImageBuffer buffer, Bitmap bmp)
        {
            for (int i = 0; i < targetSize.Height; i++)
            {
                for (int j = 0; j < targetSize.Width; j++)
                {
                    Color c = _sampler.GetSampleColor(targetSize, bmp, j, i);
                    ConsolePixel pixel = _converter.CreatePixel(c);
                    buffer.SetPixel(j, i, pixel);
                }
            }
        }

        public Size CalculateImageSize(Bitmap original, Size maxSize, int consoleLeft, int consoleTop)
        {
            double ratio = (double)original.Size.Height / (double)original.Size.Width;
            int width = maxSize.Width - consoleLeft;
            int height = (int)(width * ratio);

            if (height + consoleTop > maxSize.Height)
            {
                height = maxSize.Height - consoleTop;
                width = (int)(height / ratio);
            }

            //if (settings.ImageHeight <= 0)
            //    settings.ImageHeight = height - settings.ImageTop;
            //if (settings.ImageWidth <= 0)
            //    settings.ImageWidth = width - settings.ImageLeft;

            return new Size(width, height);
        }
    }
}
