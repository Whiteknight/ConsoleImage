using System.Drawing;
using System.Drawing.Imaging;

namespace ConsoleImage
{
    public class ImageBuilder
    {
        private readonly IImageSampler _sampler;
        private readonly IPixelConverter _converter;
        private readonly ImageSettings _settings;

        public ImageBuilder(ImageSettings settings)
        {
            _sampler = settings.Sampler;
            _converter = settings.Converter;
            _settings = settings;
        }

        public void Build(Bitmap bmp, FrameDimension frameDimension, Size targetSize, ImageBuffer buffer, int frameIdx)
        {
            
        }
    }
}
