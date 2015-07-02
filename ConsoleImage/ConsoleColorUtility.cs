using System;

namespace ConsoleImage
{
    public static class ConsoleColorUtility
    {
        public static ConsoleColor MakeDark(ConsoleColor c)
        {
            return (ConsoleColor)(((int)c) & 0x07);
        }

        public static ConsoleColor MakeBright(ConsoleColor c)
        {
            return (ConsoleColor)(((int)c) | 0x08);
        }
    }
}
