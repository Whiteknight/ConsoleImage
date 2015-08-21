using System;

namespace ConsoleImage
{
    public class ColorScheme
    {
        private readonly ConsoleColor _background;
        private readonly ConsoleColor _fg;

        public ColorScheme(ConsoleColor background, ConsoleColor fg)
        {
            _background = background;
            _fg = fg;
        }

        public ConsoleColor Background
        {
            get { return _background; }
        }

        public ConsoleColor Foreground
        {
            get { return _fg; }
        }

        public static ColorScheme Default
        {
            get { return new ColorScheme(ConsoleColor.Black, ConsoleColor.Gray); }
        }

        public static ColorScheme Get()
        {
            return new ColorScheme(Console.BackgroundColor, Console.ForegroundColor);
        }

        public void Set()
        {
            Console.BackgroundColor = Background;
            Console.ForegroundColor = Foreground;
        }
    }
}
