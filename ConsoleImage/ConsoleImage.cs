using System;
using System.Drawing;
using System.Text;

namespace ConsoleImage
{
    public class ConsoleImage
    {
        public static void Draw(Bitmap image, ImageSettings settings = null)
        {
            if (settings == null)
                settings = new ImageSettings();

            settings.Validate();

            ConsoleState state = ConsoleState.GetState();
            Console.OutputEncoding = Encoding.GetEncoding(1252);

            Bitmap bmp = new ImageResizer().Resize(image, settings);
            IColorPixelConverter converter = settings.ColorConverter ?? new BlockColorPixelConverterB();
            ImageBuffer buffer = new ImageBuilder().Build(converter, bmp);
            new ImageRenderer().Draw(settings, buffer);

            state.ResetConsole();
        }
    }
}
