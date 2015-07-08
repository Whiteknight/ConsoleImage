using System.Drawing;

namespace ConsoleImage
{
    public interface IImageSampler
    {
        Color GetSampleColor(Size bufferSize, Bitmap bmp, int left, int top);
    }

    public class AveragingImageSampler : IImageSampler
    {
        public Color GetSampleColor(Size bufferSize, Bitmap bmp, int left, int top)
        {
            Size sampleSize = new Size
            {
                Height = bmp.Size.Height / bufferSize.Height,
                Width = bmp.Size.Width / bufferSize.Width
            };

            return bmp.GetPixel(left * sampleSize.Width, top * sampleSize.Height);

            //int totalRed = 0;
            //int totalGreen = 0;
            //int totalBlue = 0;

            //int numPixels = 0;

            //for (int i = top * sampleSize.Height; i < (top + 1) * sampleSize.Height; i++)
            //{
            //    for (int j = left * sampleSize.Width; j < (left + 1) * sampleSize.Width; j++)
            //    {
            //        Color c = bmp.GetPixel(j, i);
            //        totalRed += c.R;
            //        totalGreen += c.G;
            //        totalBlue += c.B;
            //        numPixels++;
            //    }
            //}

            //return Color.FromArgb(totalRed / numPixels, totalGreen / numPixels, totalBlue / numPixels);
        }
    }
}
