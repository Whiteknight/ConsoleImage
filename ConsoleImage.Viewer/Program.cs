using System;
using System.Drawing;

namespace ConsoleImage.Viewer
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: Need support for transparency, default background color
            // TODO: Fix/Test resizing and cropping

            ImageSettings settings = new ImageSettings {
                //ImageLeft = 50,
                //ImageTop = 60,
                //ImageWidth = 30,
                //ImageHeight = 20
                //ConsoleLeft = 5,
                //ConsoleTop = 3,
                //ConsoleMaxHeight = 25,
                //ConsoleMaxWidth = 40
                //Converter = new GreyscalePixelConverterB()
            };
            Bitmap original = (Bitmap)System.Drawing.Image.FromFile(args[0]);

            ConsoleImage.DrawAnimate(original, () => Console.KeyAvailable, settings);

            Console.ReadKey();
        }

    }
}