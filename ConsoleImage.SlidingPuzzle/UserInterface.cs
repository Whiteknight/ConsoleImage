using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleImage.SlidingPuzzle
{
    public class UserInterface
    {
        private readonly int _blockSize;
        private readonly Size _gameSize;
        private readonly ConsoleRegion[,] _regions;
        private readonly ConsoleRegion _msgRegion;

        public UserInterface(int blockSize, Size gameSize)
        {
            _blockSize = blockSize;
            _gameSize = gameSize;
            _regions = new ConsoleRegion[4,4];
            _msgRegion = new ConsoleRegion(new Point(_gameSize.Width + 5, 5), new Size(20, 10));
        }

        public void RenderInitialBoard(Bitmap bmp)
        {
            ImageBuilder builder = new ImageBuilder(new AveragingImageSampler(), new SearchPixelConverter(), Color.Black);
            IImage image = builder.Build(bmp, _gameSize);

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (i == 3 && j == 3)
                        continue;

                    Point p = new Point(i * _blockSize, j * _blockSize);
                    _regions[i, j] = new ConsoleRegion(p, new Size(_blockSize, _blockSize));
                    _regions[i, j].Draw(image.GetBuffer(0), p);
                }
            }
            _regions[3, 3] = null;
        }

        public void Message(string msg)
        {
            _msgRegion.Clear();
            _msgRegion.Write(msg);
        }

        public string GetUserInput()
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Q || key.Key == ConsoleKey.Escape)
                return "EXIT";
            if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.RightArrow)
                return key.Key.ToString();
            return "";
        }

        public void MoveTile(Point tile, Point dest)
        {
            _regions[tile.X, tile.Y].MoveTo(new Point(dest.X * _blockSize, dest.Y * _blockSize));
        }

        public void SwapTiles(Point tile1, Point tile2, Point emptyTile)
        {
            MoveTile(tile1, emptyTile);
            MoveTile(tile2, tile1);
            MoveTile(emptyTile, tile1);
        }
    }
}
