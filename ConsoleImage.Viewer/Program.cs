using System;
using System.Drawing;

namespace ConsoleImage.Viewer
{
    class Program
    {
        private const string ImageFile = @"C:\Users\awhitworth\Pictures\lena_secret.bmp";

        static void Main(string[] args)
        {
            // TODO: Need proper resizing, auto-sizing, specified sizing
            // TODO: Need ability to print image in a specified X,Y range
            // TODO: Need support for transparency support, default background color

            Bitmap original = (Bitmap)Image.FromFile(ImageFile);
            double ratio = (double)original.Size.Height / (double)original.Size.Width;
            Console.WindowHeight = (int)((Console.WindowWidth - 1) * ratio);
            Console.OutputEncoding = System.Text.Encoding.GetEncoding(1252);
            Bitmap bmp = new Bitmap(original, new Size(Console.WindowWidth, Console.WindowHeight));
            //bmp.Save(@"C:\Users\awhitworth\Pictures\Cute-Cats-063-resize.jpg");

            IColorPixelConverter converter = new BlockColorPixelConverterA();

            for (int i = 0; i < Console.WindowHeight; i++)
            {
                for (int j = 0; j < Console.WindowWidth; j++)
                {

                    Color c = bmp.GetPixel(j, i);
                    ConsolePixel pixel = converter.CreatePixel(c);
                    pixel.Print();
                }
            }
            Console.WriteLine();

            converter = new BlockColorPixelConverterB();

            for (int i = 0; i < Console.WindowHeight; i++)
            {
                for (int j = 0; j < Console.WindowWidth; j++)
                {

                    Color c = bmp.GetPixel(j, i);
                    ConsolePixel pixel = converter.CreatePixel(c);
                    pixel.Print();
                }
            }
            Console.WriteLine();

            converter = new GreyscaleColorPixelConverterB();

            for (int i = 0; i < Console.WindowHeight; i++)
            {
                for (int j = 0; j < Console.WindowWidth; j++)
                {

                    Color c = bmp.GetPixel(j, i);
                    ConsolePixel pixel = converter.CreatePixel(c);
                    pixel.Print();
                }
            }

            Console.ReadKey();
        }

    }
}