using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ConsoleImage.Scales
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.GetEncoding(1252);

            for (int i = 0; i < 13; i++)
            {
                ConsolePixel p = GetGreyscalePixel(i);
                p.Print();
            }
            Console.WriteLine();

            ConsoleColor[] colors = new ConsoleColor[] {
                ConsoleColor.Magenta, 
                ConsoleColor.Red, 
                ConsoleColor.Yellow, 
                ConsoleColor.Green, 
                ConsoleColor.Cyan, 
                ConsoleColor.Blue
            };

            foreach (ConsoleColor color in colors)
            {

                for (int i = 0; i < 13; i++)
                {
                    ConsolePixel p = GetPureColorPixel(color, i);
                    p.Print();
                }
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            for (int i = 0; i < colors.Length; i++)
            {
                PrintColorBlend(colors[i], colors[(i + 1) % colors.Length]);
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            for (int i = 0; i < colors.Length; i++)
            {
                PrintColorBlend(MakeDark(colors[i]), MakeDark(colors[(i + 1) % colors.Length]));
            }


            for (int i = 0; i < 13; i++)
            {
                for (int j = 0; j <= colors.Length; j++)
                {
                    Console.SetCursorPosition(j * 4, i + 8);
                    ConsolePixel p = GetPureColorPixel(colors[j % colors.Length], 12 - i);
                    p.Print();
                }
            }

            Console.WriteLine();

            for (int i = 0; i < 13; i++)
            {
                ConsolePixel p = GetPureColorPixel(ConsoleColor.Red, i);
                p.Print();
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine(" ({0},{1},{2})", p.Red, p.Green, p.Blue);
            }

            Console.WriteLine();

            ConsolePixel pixel = new ConsolePixelRepository().GetClosestPixel(Color.FromArgb(75, 102, 21));
            pixel.Print();
            pixel.Print();
            pixel.Print();
            Console.WriteLine();
            pixel.Print();
            pixel.Print();
            pixel.Print();



            Console.ReadKey();
        }
        
        static void PrintColorBlend(ConsoleColor c1, ConsoleColor c2)
        {
            for (int i = 0; i < 4; i++)
            {
                ConsolePixel p = GetBlendColorPixel(c1, c2, i);
                p.Print();
            }
            //Console.WriteLine();
        }

        static ConsolePixel GetGreyscalePixel(int i)
        {
            if (i > 12)
                i = 12;
            if (i < 0)
                i = 0;

            ConsoleColor[] greyscales = new ConsoleColor[] { ConsoleColor.Black, ConsoleColor.DarkGray, ConsoleColor.Gray, ConsoleColor.White, ConsoleColor.White, ConsoleColor.White };
            const string blocks = " \xB0\xB1\xB2 \xB0\xB1\xB2 \xB0\xB1\xB2 ";
            int gidx = i / 4;
            ConsoleColor bg = greyscales[gidx];
            ConsoleColor fg = greyscales[gidx + 1];
            char c = blocks[i];
            return new ConsolePixel(bg, fg, c);
        }

        static ConsolePixel GetPureColorPixel(ConsoleColor baseColor, int i)
        {
            if (i > 13)
                i = 13;
            if (i < 0)
                i = 0;
            const string blocks = " \xB0\xB1\xB2 \xB0\xB1\xB2 \xB0\xB1\xB2 ";
            ConsoleColor bright = MakeBright(baseColor);
            ConsoleColor dark = MakeDark(baseColor);
            ConsoleColor[] scales = new[] { ConsoleColor.Black, dark, bright, ConsoleColor.White, ConsoleColor.White };
            int gidx = i / 4;
            ConsoleColor bg = scales[gidx];
            ConsoleColor fg = scales[gidx + 1];
            char c = blocks[i];
            return new ConsolePixel(bg, fg, c);
        }

        static ConsolePixel GetBlendColorPixel(ConsoleColor c1, ConsoleColor c2, int i)
        {
            if (i > 4)
                i = 4;
            if (i < 0)
                i = 0;
            const string blocks = " \xB0\xB1\xB2";
            if (i == 4)
                return new ConsolePixel(c2, c1, ' ');
            return new ConsolePixel(c1, c2, blocks[i]);
        }

        static ConsoleColor MakeDark(ConsoleColor c)
        {
            return (ConsoleColor)(((int)c) & 0x07);
        }

        static ConsoleColor MakeBright(ConsoleColor c)
        {
            return (ConsoleColor)(((int)c) | 0x08);
        }
    }
}
