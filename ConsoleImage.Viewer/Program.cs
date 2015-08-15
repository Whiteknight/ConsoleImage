using System;
using System.Drawing;

namespace ConsoleImage.Viewer
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: Fix/Test resizing and cropping

            ImageSettings settings = new ImageSettings {
                //ImageLeft = 50,
                //ImageTop = 60,
                ImageMaxSize = new Size {
                    Height = 50,
                    Width = 50
                },
                // TODO: Fix these

                ImageRegionStart = new Point {
                    X = 20,
                    Y = 20
                },

                ConsoleStart = new Point {
                    X = 10,
                    Y = 5
                },

                //Converter = new GreyscalePixelConverterB()
                TransparencyColor = ConsoleColor.White
            };
            Bitmap original = (Bitmap)System.Drawing.Image.FromFile(args[0]);

            ConsoleImage.DrawAnimate(original, () => Console.KeyAvailable, settings);

            Console.ReadKey();
        }

    }
}