using System;
using System.Drawing;

namespace ConsoleImage
{
    public class ImageSettings
    {
        public ImageSettings()
        {
            ConsoleTop = 0;
            ConsoleLeft = 0;
            ConsoleMaxSize = new Size
            {
                Width = Console.WindowWidth,
                Height = Console.LargestWindowHeight
            };
            
            ImageTop = 0;
            ImageLeft = 0;

            TransparencyColor = Console.BackgroundColor;
        }

        public int ImageTop { get; set; }
        public int ImageLeft { get; set; }

        // The size of the area of the image to use. This is cropped from the larger image, and will default to the
        // total size of the image unless a cropped region is specified
        public Size ImageCropSize { get; set; }

        // The distance from the top of the console to start drawing
        public int ConsoleTop { get; set; }

        // The distance from the left of the console to start drawing
        public int ConsoleLeft { get; set;  }

        // The maximum size of the console or console region to use for rendering
        public Size ConsoleMaxSize { get; set; }

        public ConsoleColor TransparencyColor { get; set; }
        public string SaveResizedImageAs { get; set; }

        private IPixelConverter _converter;
        public IPixelConverter Converter {
            get { return _converter ?? (_converter = new SearchPixelConverter()); }
            set { _converter = value; }
        }

        private IImageSampler _sampler;
        public IImageSampler Sampler {
            get { return _sampler ?? (_sampler = new AveragingImageSampler()); }
            set { _sampler = value; }
        }
        //bmp.Save(@"C:\Users\awhitworth\Pictures\Cute-Cats-063-resize.jpg");

        public void Validate()
        {
            if (ConsoleLeft + ConsoleMaxSize.Width > Console.LargestWindowWidth)
                throw new Exception("Window is too small");
            if (ConsoleTop + ConsoleMaxSize.Height > Console.LargestWindowHeight)
                throw new Exception("Window is too short");
        }

        public void SetMaxImageSize()
        {
            ConsoleMaxSize = new Size
            {
                Width = Console.LargestWindowHeight,
                Height = Console.LargestWindowHeight
            };
        }
    }
}
