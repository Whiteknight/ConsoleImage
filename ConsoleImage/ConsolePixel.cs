﻿using System;
﻿using System.Collections.Generic;
﻿using System.Drawing;

namespace ConsoleImage
{
    public class ConsolePixel
    {
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

            Color color1 = backgroundColor.ToColor();
            Color color2 = foregroundColor.ToColor(); ;

            Color = color1.Blend(color2, (double)_percent);
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