﻿using System;
﻿using System.Collections.Generic;
﻿using System.Drawing;

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
            { ConsoleColor.Black, new[] { BL, BL, BL }},
            { ConsoleColor.Blue, new[] { BL, BL, L }},
            { ConsoleColor.Cyan, new[] { BL, L, L }},
            { ConsoleColor.DarkBlue, new[] { BD, BD, D }},
            { ConsoleColor.DarkCyan, new[] { BD, D, D }},
            { ConsoleColor.DarkGray, new[] { G1, G1, G1 }},
            { ConsoleColor.DarkGreen, new[] { BD, D, BD }},
            { ConsoleColor.DarkMagenta, new[] { D, BD, D }},
            { ConsoleColor.DarkRed, new[] { D, BD, BD }},
            { ConsoleColor.DarkYellow, new[] {D, D, BD }},
            { ConsoleColor.Gray, new[] { G2, G2, G2 }},
            { ConsoleColor.Green, new[] { BL, L, BL }},
            { ConsoleColor.Magenta, new[] { L, BL, L }},
            { ConsoleColor.Red, new[] { L, BL, BL }},
            { ConsoleColor.White, new[] { L, L, L }},
            { ConsoleColor.Yellow, new[] { L, L, BL }}
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

            int r = CalculateColor(color1[0], color2[0], _percent);
            int g = CalculateColor(color1[1], color2[1], _percent);
            int b = CalculateColor(color1[2], color2[2], _percent);
            Color = Color.FromArgb(r, g, b);
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
            return new ConsolePixel(BackgroundColor.Invert(), ForegroundColor.Invert(), PrintableCharacter);
        }

        public Color Color { get; private set; }

        public int AsInt
        {
            get { return Color.GetRgbInt(); }
        }

        public bool IsGrayscale
        {
            get { return BackgroundColor.IsGrayscale() && (_percent == 0 || ForegroundColor.IsGrayscale()); }
        }
    }
} 