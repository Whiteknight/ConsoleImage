﻿using System;
﻿using System.Collections.Generic;

namespace ConsoleImage
{
    public class ConsolePixel
    {
        private const int L = 255;
        private const int D = 128;
        private const int BL = 0;
        private const int BD = 64;
        private const int G1 = 96;
        private const int G2 = 194;

        private static readonly Dictionary<ConsoleColor, int[]> _colorLookup = new Dictionary<ConsoleColor, int[]> {
            { ConsoleColor.Black, new int[] { BL, BL, BL }},
            { ConsoleColor.Blue, new int[] { BL, BL, L }},
            { ConsoleColor.Cyan, new int[] { BL, L, L }},
            { ConsoleColor.DarkBlue, new int[] { BD, BD, D }},
            { ConsoleColor.DarkCyan, new int[] { BD, D, D }},
            { ConsoleColor.DarkGray, new int[] { G1, G1, G1 }},
            { ConsoleColor.DarkGreen, new int[] { BD, D, BD }},
            { ConsoleColor.DarkMagenta, new int[] { D, BD, D }},
            { ConsoleColor.DarkRed, new int[] { D, BD, BD }},
            { ConsoleColor.DarkYellow, new int[] {D, D, BD }},
            { ConsoleColor.Gray, new int[] { G2, G2, G2 }},
            { ConsoleColor.Green, new int[] { BL, L, BL }},
            { ConsoleColor.Magenta, new int[] { L, BL, L }},
            { ConsoleColor.Red, new int[] { L, BL, BL }},
            { ConsoleColor.White, new int[] { L, L, L }},
            { ConsoleColor.Yellow, new int[] { L, L, BL }}
        };

        // percentage of the background color which shows with each glyph
        private static readonly Dictionary<char, byte> _percents = new Dictionary<char, byte> {
            { ' ', 100 },
            { '\xB0', 75 },
            { '\xB1', 50 },
            { '\xB2', 25 }
        };

        private readonly byte _percent;

        public ConsolePixel(ConsoleColor backgroundColor, ConsoleColor foregroundColor, char printableCharacter)
        {
            BackgroundColor = backgroundColor;
            ForegroundColor = foregroundColor;
            PrintableCharacter = printableCharacter;

            _percent = _percents.ContainsKey(printableCharacter) ? _percents[printableCharacter] : (byte)0;

            int[] color1 = _colorLookup[backgroundColor];
            int[] color2 = _colorLookup[foregroundColor];

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

        public ConsolePixel Invert()
        {
            return new ConsolePixel(InvertColor(BackgroundColor), InvertColor(ForegroundColor), PrintableCharacter);
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

        public int AsInt
        {
            get { return (((byte) PrintableCharacter) << 8) | ((byte) BackgroundColor << 4) | ((byte) ForegroundColor); }
        }

        public bool IsGrayscale
        {
            get { return IsColorGrayscale(BackgroundColor) && (_percent == 0 || IsColorGrayscale(ForegroundColor)); }
        }

        private static bool IsColorGrayscale(ConsoleColor cc)
        {
            return cc == ConsoleColor.Black || cc == ConsoleColor.Gray || cc == ConsoleColor.DarkGray || cc == ConsoleColor.White;
        }

        private static ConsoleColor InvertColor(ConsoleColor cc)
        {
            return (ConsoleColor) (~((byte) cc) & 0x0F);
        }
    }
} 