using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleImage.SlidingPuzzle
{
    class Program
    {
        const int BlockSize = 13;

        static void Main(string[] args)
        {
            ImageSettings settings = new ImageSettings
            {
                //ImageLeft = 50,
                //ImageTop = 60,
                //ImageMaxSize = new Size {
                //    Height = 50,
                //    Width = 50
                //},

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

            settings.Validate();

            

            Size gameSize = new Size(BlockSize * 4, BlockSize * 4);

            ConsoleManager manager = new ConsoleManager(settings);
            using (IDisposable state = manager.SaveConsoleState().AsDisposable())
            {
                manager.SetForGraphics();
                manager.ResizeConsoleWindow(gameSize);

                ImageBuilder builder = new ImageBuilder(settings.Sampler, settings.Converter, settings.TransparencyColor.ToColor());
                IImage image = builder.Build(bitmap, gameSize);

                ConsoleRegion[,] regions = new ConsoleRegion[4,4];
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (i == 3 && j == 3)
                            continue;

                        Point p = new Point(i * BlockSize, j * BlockSize);

                        regions[i, j] = new ConsoleRegion(p, new Size(BlockSize, BlockSize));
                        regions[i, j].Draw(image.GetBuffer(0), p);
                    }
                }

                regions[3, 3] = null;

                Point emptySpace = new Point(3, 3);

                while (true)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Q || key.Key == ConsoleKey.Escape)
                        break;
                    if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.RightArrow)
                        emptySpace = MoveRegion(regions, emptySpace, key.Key);
                }
            }



            Console.ReadKey();
        }

        private static Point MoveRegion(ConsoleRegion[,] regions, Point emptySpace, ConsoleKey key)
        {
            Point target;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    target = new Point(emptySpace.X, emptySpace.Y + 1);
                    break;
                case ConsoleKey.DownArrow:
                    target = new Point(emptySpace.X, emptySpace.Y - 1);
                    break;
                case ConsoleKey.LeftArrow:
                    target = new Point(emptySpace.X + 1, emptySpace.Y);
                    break;
                case ConsoleKey.RightArrow:
                    target = new Point(emptySpace.X - 1, emptySpace.Y);
                    break;

                default:
                    return emptySpace;
            }

            if (target.X < 0 || target.X > 3 || target.Y < 0 || target.Y > 3)
                return emptySpace;

            regions[emptySpace.X, emptySpace.Y] = regions[target.X, target.Y];
            regions[target.X, target.Y] = null;
            regions[emptySpace.X, emptySpace.Y].MoveTo(new Point(emptySpace.X * BlockSize, emptySpace.Y * BlockSize));
            return target;
        }
    }
}
