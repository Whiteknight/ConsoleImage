using System;
using System.Drawing;

namespace ConsoleImage.Viewer
{
    class Program
    {
        private const string ImageFile = @"";

        static void Main(string[] args)
        {
            // TODO: Need support for transparency, default background color

            ImageSettings settings = new ImageSettings {
                //ImageLeft = 50,
                //ImageTop = 60,
                //ImageWidth = 30,
                //ImageHeight = 20
                //ConsoleLeft = 5,
                //ConsoleTop = 3,
                //ConsoleMaxHeight = 25,
                //ConsoleMaxWidth = 40
                ColorConverter = new GreyscaleColorPixelConverterB()
            };
            Bitmap original = (Bitmap)Image.FromFile(ImageFile);

            ConsoleImage.Draw(original, settings);

            Console.ReadKey();
        }

    }
}