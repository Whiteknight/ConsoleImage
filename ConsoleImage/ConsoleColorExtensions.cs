using System;
using System.Collections.Generic;
using System.Drawing;

namespace ConsoleImage
{
    public static class ConsoleColorExtensions
    {
        public static ConsoleColor MakeDark(this ConsoleColor c)
        {
            return (ConsoleColor)(((int)c) & 0x07);
        }

        public static ConsoleColor MakeBright(this ConsoleColor c)
        {
            return (ConsoleColor)(((int)c) | 0x08);
        }

        public static bool IsGrayscale(this ConsoleColor cc)
        {
            return cc == ConsoleColor.Black || cc == ConsoleColor.Gray || cc == ConsoleColor.DarkGray || cc == ConsoleColor.White;
        }

        public static ConsoleColor Invert(this ConsoleColor cc)
        {
            return (ConsoleColor)(~((byte)cc) & 0x0F);
        }

        private const int L = 255;
        private const int D = 128;
        private const int BL = 0;
        private const int BD = 64;
        private const int G1 = 96;
        private const int G2 = 194;

        private static readonly Dictionary<ConsoleColor, Color> _colorLookup = new Dictionary<ConsoleColor, Color> {
            { ConsoleColor.Black, Color.FromArgb(BL, BL, BL )},
            { ConsoleColor.Blue, Color.FromArgb(BL, BL, L )},
            { ConsoleColor.Cyan, Color.FromArgb(BL, L, L )},
            { ConsoleColor.DarkBlue, Color.FromArgb(BD, BD, D )},
            { ConsoleColor.DarkCyan, Color.FromArgb(BD, D, D )},
            { ConsoleColor.DarkGray, Color.FromArgb(G1, G1, G1 )},
            { ConsoleColor.DarkGreen, Color.FromArgb(BD, D, BD )},
            { ConsoleColor.DarkMagenta, Color.FromArgb(D, BD, D )},
            { ConsoleColor.DarkRed, Color.FromArgb(D, BD, BD )},
            { ConsoleColor.DarkYellow, Color.FromArgb(D, D, BD )},
            { ConsoleColor.Gray, Color.FromArgb(G2, G2, G2 )},
            { ConsoleColor.Green, Color.FromArgb(BL, L, BL )},
            { ConsoleColor.Magenta, Color.FromArgb(L, BL, L )},
            { ConsoleColor.Red, Color.FromArgb(L, BL, BL )},
            { ConsoleColor.White, Color.FromArgb(L, L, L )},
            { ConsoleColor.Yellow, Color.FromArgb(L, L, BL )}
        };

        //private static readonly Dictionary<ConsoleColor, Color> _colorLookup = new Dictionary<ConsoleColor, Color> {
        //    { ConsoleColor.Black, Color.Black},
        //    { ConsoleColor.Blue, Color.Blue},
        //    { ConsoleColor.Cyan, Color.Cyan},
        //    { ConsoleColor.DarkBlue, Color.DarkBlue},
        //    { ConsoleColor.DarkCyan, Color.DarkCyan},
        //    { ConsoleColor.DarkGray, Color.Gray},
        //    { ConsoleColor.DarkGreen, Color.DarkGreen},
        //    { ConsoleColor.DarkMagenta, Color.DarkMagenta},
        //    { ConsoleColor.DarkRed, Color.DarkRed},
        //    { ConsoleColor.DarkYellow, Color.DarkGoldenrod},
        //    { ConsoleColor.Gray, Color.DarkGray},
        //    { ConsoleColor.Green, Color.Green},
        //    { ConsoleColor.Magenta, Color.Magenta},
        //    { ConsoleColor.Red, Color.Red},
        //    { ConsoleColor.White, Color.White},
        //    { ConsoleColor.Yellow, Color.Yellow}
        //};

        public static Color ToColor(this ConsoleColor cc)
        {
            return _colorLookup[cc];
        }
    }
}
