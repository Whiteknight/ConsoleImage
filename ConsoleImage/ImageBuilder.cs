using System.Drawing;

namespace ConsoleImage
{
    public class ImageBuilder
    {
        public ImageBuffer Build(ImageSettings settings, Bitmap bmp)
        {
            IColorPixelConverter converter = settings.ColorConverter ?? new BlockColorPixelConverterB();
            ImageBuffer buffer = new ImageBuffer(bmp.Size);
            for (int i = 0; i < bmp.Size.Height; i++)
            {
                for (int j = 0; j < bmp.Size.Width; j++)
                {
                    Color c = bmp.GetPixel(j, i);
                    ConsolePixel pixel = converter.CreatePixel(c);
                    buffer.SetPixel(j, i, pixel);
                }
            }
            return buffer;
        }
    }
}
