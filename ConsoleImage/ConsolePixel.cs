﻿using System;
﻿using System.Collections.Generic;

namespace ConsoleImage
{
    public class ConsolePixel
    {
        private static readonly Dictionary<ConsoleColor, int[]> s_colorLookup = new Dictionary<ConsoleColor, int[]> {
            { ConsoleColor.Black, new int[] { 0, 0, 0 }},
            { ConsoleColor.Blue, new int[] { 0, 0, 255 }},
            { ConsoleColor.Cyan, new int[] { 0, 255, 255 }},
            { ConsoleColor.DarkBlue, new int[] { 0, 0, 128 }},
            { ConsoleColor.DarkCyan, new int[] { 0, 128, 128 }},
            { ConsoleColor.DarkGray, new int[] { 96, 96, 96 }},
            { ConsoleColor.DarkGreen, new int[] { 0, 128, 0 }},
            { ConsoleColor.DarkMagenta, new int[] { 128, 0, 128 }},
            { ConsoleColor.DarkRed, new int[] { 128, 0, 0 }},
            { ConsoleColor.DarkYellow, new int[] {128, 128, 0 }},
            { ConsoleColor.Gray, new int[] { 194, 194, 194 }},
            { ConsoleColor.Green, new int[] { 0, 255, 0 }},
            { ConsoleColor.Magenta, new int[] { 255, 0, 255 }},
            { ConsoleColor.Red, new int[] { 255, 0, 0 }},
            { ConsoleColor.White, new int[] { 255, 255, 255 }},
            { ConsoleColor.Yellow, new int[] { 255, 255, 0 }}
        };

        private static readonly Dictionary<char, int> _percents = new Dictionary<char, int> {
            { ' ', 100 },
            { '\xB0', 75 },
            { '\xB1', 50 },
            { '\xB2', 25 }
        };

        private int _percent;

        public ConsolePixel(ConsoleColor backgroundColor, ConsoleColor foregroundColor, char printableCharacter)
        {
            BackgroundColor = backgroundColor;
            ForegroundColor = foregroundColor;
            PrintableCharacter = printableCharacter;

            _percent = _percents.ContainsKey(printableCharacter) ? _percents[printableCharacter] : 0;

            int[] color1 = s_colorLookup[backgroundColor];
            int[] color2 = s_colorLookup[foregroundColor];

            Red = CalculateColor(color1[0], color2[0], _percent);
            Green = CalculateColor(color1[1], color2[1], _percent);
            Blue = CalculateColor(color1[2], color2[2], _percent);
        }

        private static byte CalculateColor(int color1, int color2, int percent)
        {
            double p = (double)percent / 100.0;
            int part1 = (int)(color1 * p);
            int part2 = (int)(color2 * (1.0 - p));
            int total = part1 + part2;
            if (total > 255)
                return 255;
            if (total < 0)
                return 0;
            return (byte)total;
        }

        public ConsoleColor BackgroundColor { get; private set; }
        public ConsoleColor ForegroundColor { get; private set; }
        public char PrintableCharacter { get; private set; }

        public void Print()
        {
            Console.ForegroundColor = ForegroundColor;
            Console.BackgroundColor = BackgroundColor;
            Console.Write(PrintableCharacter);
        }

        public byte Brightness
        {
            get { return (byte)(((int) Red + (int) Blue + (int) Green)/3); } 
        }
        public byte Red { get; private set; }
        public byte Blue { get; private set; }
        public byte Green { get; private set; }

        public int Rgb
        {
            get { return (Red << 8) | (Green << 4) | Blue; }
        }

        public bool IsGrayscale
        {
            get { return IsColorGrayscale(BackgroundColor) && (_percent == 0 || IsColorGrayscale(ForegroundColor)); }
        }

        private static bool IsColorGrayscale(ConsoleColor cc)
        {
            return cc == ConsoleColor.Black || cc == ConsoleColor.Gray || cc == ConsoleColor.DarkGray || cc == ConsoleColor.White;
        }
    }
} 