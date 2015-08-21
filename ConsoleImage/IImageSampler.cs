using System.Drawing;

namespace ConsoleImage
{
    public interface IImageSampler
    {
        Color GetSampleColor(Size bufferSize, Bitmap bmp, int left, int top, Color bgColor);
    }

    public class AveragingImageSampler : IImageSampler
    {
        // TODO: The "pixels" in the console aren't usually square. Create a sampler that accounts for a 
        // rectangular pixel
        public Color GetSampleColor(Size bufferSize, Bitmap bmp, int left, int top, Color bgColor)
        {
            double x = ((double)bmp.Size.Width / bufferSize.Width) * left;
            double y = ((double)bmp.Size.Height / bufferSize.Height) * top;

            Color c = bmp.GetPixel((int)x, (int)y);
            return c.BlendTransparency(bgColor);
        }
    }
}
