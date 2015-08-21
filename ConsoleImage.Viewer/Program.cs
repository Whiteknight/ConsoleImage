using System;
using System.Drawing;
using System.Text;

namespace ConsoleImage.Viewer
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: Parse settings from commandline args
            // TODO: Delay parameter so the image doesn't render faster than the screen can handle.

            ImageSettings settings = new ImageSettings {
                //ImageLeft = 50,
                //ImageTop = 60,
                ImageMaxSize = new Size
                {
                    Height = 30,
                    Width = 30
                },

                //ImageCropStart = new Point {
                //    X = 10,
                //    Y = 10
                //},

                ImageCropSize = new Size
                {
                    Height = 500,
                    Width = 400
                },

                //ConsoleStart = new Point
                //{
                //    X = 10,
                //    Y = 5
                //},

                //Converter = new GreyscalePixelConverterB()
                TransparencyColor = ConsoleColor.White,
                RenderStrategy = new ProgressiveRenderStrategy()
            };

            Bitmap bitmap = (Bitmap)System.Drawing.Image.FromFile(args[0]);

            ConsoleImage.DrawAnimate(bitmap, () => Console.KeyAvailable, settings);

            Console.ReadKey();
        }

    }
}