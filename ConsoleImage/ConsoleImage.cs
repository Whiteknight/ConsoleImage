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
            ImageBuffer buffer = new ImageBuilder().Build(settings, bmp);
            buffer.Draw(settings);

            state.ResetConsole();
        }
    }
}
