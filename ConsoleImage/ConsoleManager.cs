using System;
using System.Drawing;
using System.Text;

namespace ConsoleImage
{
    // TODO: Method to create, return and manage ConsoleRegions
    // TODO: Ability to render multiple ConsoleRegions at once, even if they overlap (with z-indexing)
    public class ConsoleManager
    {
        public static Size MaxSize
        {
            get { return new Size(Console.LargestWindowWidth, Console.LargestWindowHeight); }
        }

        public static Size CurrentSize
        {
            get { return new Size(Console.WindowWidth, Console.WindowHeight); }
        }

        public static Size MaxVerticalSize
        {
            get { return new Size(Console.WindowWidth, Console.LargestWindowHeight); }
        }

        public static Point Origin
        {
            get { return new Point(0, 0); }
        }

        public ConsoleManager()
        {
        }

        public ConsoleState SaveConsoleState()
        {
            return ConsoleState.GetState();
        }

        public void SetForGraphics()
        {
            Console.OutputEncoding = Encoding.GetEncoding(1252);
            Console.CursorVisible = false;
        }

        public void ResizeConsoleWindow(Size size, bool allowShrink = false)
        {
            size = MaxSize.BestFitWithin(Origin, size);

            if (Console.WindowHeight < size.Height || (allowShrink && Console.WindowHeight > size.Height))
                Console.WindowHeight = size.Height;

            if (Console.WindowWidth < size.Width || (allowShrink && Console.WindowWidth > size.Width))
                Console.WindowWidth = size.Width;                
        }
    }
}
