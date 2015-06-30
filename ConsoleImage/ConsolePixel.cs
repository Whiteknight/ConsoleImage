﻿using System;
﻿using System.Collections.Generic;

namespace ConsoleImage
{
    public class ConsolePixel
    {
        private static readonly Dictionary<ConsoleColor, int[]> s_colorLookup = new Dictionary<ConsoleColor, int[]> {
            { ConsoleColor.Black, new int[] { 0, 0, 0 }},
            { ConsoleColor.Blue, new int[] { 0, 0, 255 }},
            { ConsoleColor.Cyan, new int[] { 0, 192, 192 }},
            { ConsoleColor.DarkBlue, new int[] { 0, 0, 128 }},
            { ConsoleColor.DarkCyan, new int[] { 0, 96, 96 }},
            { ConsoleColor.DarkGray, new int[] { 96, 96, 96 }},
            { ConsoleColor.DarkGreen, new int[] { 0, 128, 0 }},
            { ConsoleColor.DarkMagenta, new int[] { 96, 0, 96 }},
            { ConsoleColor.DarkRed, new int[] { 128, 0, 0 }},
            { ConsoleColor.DarkYellow, new int[] { 96, 96, 0 }},
            { ConsoleColor.Gray, new int[] { 194, 194, 194 }},
            { ConsoleColor.Green, new int[] { 0, 255, 0 }},
            { ConsoleColor.Magenta, new int[] { 192, 0, 192 }},
            { ConsoleColor.Red, new int[] { 255, 0, 0 }},
            { ConsoleColor.White, new int[] { 255, 255, 255 }},
            { ConsoleColor.Yellow, new int[] { 192, 192, 0 }}
        };

        private static readonly Dictionary<char, int> s_percents = new Dictionary<char, int> {
            { ' ', 100 },
            { '\xB0', 75 },
            { '\xB1', 50 },
            { '\xB2', 25 }
        };

        public ConsolePixel(ConsoleColor backgroundColor, ConsoleColor foregroundColor, char printableCharacter)
        {
            BackgroundColor = backgroundColor;
            ForegroundColor = foregroundColor;
            PrintableCharacter = printableCharacter;

            int percent = s_percents.ContainsKey(printableCharacter) ? s_percents[printableCharacter] : 0;

            int[] color1 = s_colorLookup[backgroundColor];
            int[] color2 = s_colorLookup[foregroundColor];

            Red = CalculateColor(color1[0], color2[0], percent);
            Green = CalculateColor(color1[1], color2[1], percent);
            Blue = CalculateColor(color1[2], color2[2], percent);
        }

        private static int CalculateColor(int color1, int color2, int percent)
        {
            double p = (double)percent / 100.0;
            int part1 = (int)(color1 * p);
            int part2 = (int)(color2 * (1.0 - p));
            return part1 + part2;
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

        public int Red { get; private set; }
        public int Blue { get; private set; }
        public int Green { get; private set; }
    }
} 