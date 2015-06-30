using System;
using System.Drawing;
using System.Net.Mime;

namespace ConsoleImage.Viewer
{
    class Program
    {
        private const string ImageFile = @"";

        static void Main(string[] args)
        {
            Bitmap original = (Bitmap)Image.FromFile(ImageFile);
            double ratio = (double)original.Size.Height / (double)original.Size.Width;
            Console.WindowHeight = (int)((Console.WindowWidth - 1) * ratio);
            Console.OutputEncoding = System.Text.Encoding.GetEncoding(1252);
            Bitmap bmp = new Bitmap(original, new Size(Console.WindowWidth, Console.WindowHeight));

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

            Console.ReadKey();
        }

        //
        private static string charsPercent = " .o*O0@";
        private static char GetCharToWrite(Color color)
        {
            int i = (int)(((double)(color.R + color.G + color.B) / (double)(256 * 3)) * charsPercent.Length);
            return charsPercent[i];
        }


    }
}