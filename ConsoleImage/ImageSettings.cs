using System;

namespace ConsoleImage
{
    public class ImageSettings
    {
        public ImageSettings()
        {
            ConsoleTop = 0;
            ConsoleLeft = 0;
            ConsoleMaxWidth = Console.WindowWidth;
            ConsoleMaxHeight = Console.LargestWindowHeight;

            ImageTop = 0;
            ImageLeft = 0;

            TransparencyColor = Console.BackgroundColor;
        }

        public int ImageTop { get; set; }
        public int ImageLeft { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }

        // The distance from the top of the console to start drawing
        public int ConsoleTop { get; set; }

        // The distance from the left of the console to start drawing
        public int ConsoleLeft { get; set;  }

        // The maximum width of the console window
        public int ConsoleMaxWidth { get; set; }

        // The maximum height of the console window
        public int ConsoleMaxHeight { get; set;  }

        public ConsoleColor TransparencyColor { get; set; }
        public string SaveResizedImageAs { get; set; }
        public IColorPixelConverter ColorConverter { get; set; }
        //bmp.Save(@"C:\Users\awhitworth\Pictures\Cute-Cats-063-resize.jpg");

        public void Validate()
        {
            if (ConsoleLeft + ConsoleMaxWidth > Console.LargestWindowWidth)
                throw new Exception("Window is too small");
            if (ConsoleTop + ConsoleMaxHeight > Console.LargestWindowHeight)
                throw new Exception("Window is too short");
        }

        public void SetMaxImageSize()
        {
            ConsoleMaxHeight = Console.LargestWindowHeight;
            ConsoleMaxWidth = Console.LargestWindowWidth;
        }
    }
}
