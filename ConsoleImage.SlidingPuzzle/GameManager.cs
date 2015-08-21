using System;
using System.Drawing;
using System.Threading;

namespace ConsoleImage.SlidingPuzzle
{
    // TODO: Make the size of the board configurable?
    public class GameManager : IDisposable
    {
        const int BlockSize = 13;
        private readonly Size _gameSize;
        private readonly ConsoleManager _consoleManager;
        private readonly ConsoleState _consoleState;
        private UserInterface _userInterface;
        private Point[,] _state;
        private Point _emptySpace;

        public GameManager()
        {
             _gameSize = new Size(BlockSize * 4, BlockSize * 4);
            _consoleManager = new ConsoleManager();
            _consoleState = _consoleManager.SaveConsoleState();

            _consoleManager.SetForGraphics();
            _consoleManager.ResizeConsoleWindow(_gameSize);
        }

        public bool Run(string imageFile)
        {
            Bitmap bitmap = (Bitmap)System.Drawing.Image.FromFile(imageFile);
            _userInterface = new UserInterface(BlockSize, _gameSize);
            _userInterface.RenderInitialBoard(bitmap);
            _state = new Point[4, 4];
            _emptySpace = new Point(3, 3);

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    _state[i, j] = new Point(i, j);
                }
            }

            _userInterface.Message("Randomizing...");

            Randomize();

            _userInterface.Message("Ready");

            while (true)
            {
                string input = _userInterface.GetUserInput();
                if (string.IsNullOrEmpty(input))
                    continue;
                if (input == "EXIT")
                    break;

                MoveTile(input);
                bool win = CheckForWinCondition();
                if (win)
                {
                    _userInterface.Message("You win!");
                    return true;
                }       
            }
            return false;
        }

        private void Randomize()
        {
            string[] directions = new string[] {
                "UpArrow",
                "DownArrow",
                "LeftArrow",
                "RightArrow"
            };


            // TODO: Make the level of randomness configurable
            Random r = new Random();
            for (int i = 0; i < 20; )
            {
                bool ok = MoveTile(directions[r.Next(4)]);
                if (!ok)
                    continue;
                i++;
                Thread.Sleep(50);
            }
        }

        private bool CheckForWinCondition()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (_state[i, j].X != i || _state[i, j].Y != j)
                        return false;
                }
            }
            return true;
        }

        private bool MoveTile(string cmd)
        {
            Point target;
            switch (cmd)
            {
                case "UpArrow":
                    target = new Point(_emptySpace.X, _emptySpace.Y + 1);
                    break;
                case "DownArrow":
                    target = new Point(_emptySpace.X, _emptySpace.Y - 1);
                    break;
                case "LeftArrow":
                    target = new Point(_emptySpace.X + 1, _emptySpace.Y);
                    break;
                case "RightArrow":
                    target = new Point(_emptySpace.X - 1, _emptySpace.Y);
                    break;

                default:
                    return false;
            }

            if (target.X < 0 || target.X > 3 || target.Y < 0 || target.Y > 3)
                return false;

            _userInterface.MoveTile(_state[target.X, target.Y], _emptySpace);

            SwapStateTiles(_emptySpace, target);

            _emptySpace = target;
            return true;
        }

        private void SwapStateTiles(Point a, Point b)
        {
            Point temp = _state[a.X, a.Y];
            _state[a.X, a.Y] = _state[b.X, b.Y];
            _state[b.X, b.Y] = temp;
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _consoleState.ResetConsole();
        }

        #endregion
    }
}
