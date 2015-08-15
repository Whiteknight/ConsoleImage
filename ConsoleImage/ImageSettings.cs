using System;
using System.Drawing;

namespace ConsoleImage
{
    // TODO: Change this to an ImageContext class, and put the image inside it.
    public class ImageSettings
    {
        public ImageSettings()
        {
            ConsoleStart = new Point {
                X = 0,
                Y = 0
            };

            ImageMaxSize = new Size
            {
                Width = Console.WindowWidth,
                Height = Console.LargestWindowHeight
            };

            ImageCropStart = new Point {
                X = 0,
                Y = 0
            };

            TransparencyColor = Console.BackgroundColor;
        }

        // Starting position within the image of the cropped region to take. Defaults to (0, 0) 
        public Point ImageCropStart { get; set; }

        // The size of the area of the image to use. This is cropped from the larger image, and will default to the
        // total size of the image unless a cropped region is specified. If Null, use the whole image
        public Size? ImageCropSize { get; set; }

        // The upper-left pixel in the console to start rendering. Defaults to (0, 0)
        public Point ConsoleStart { get; set; }

        // The largest size of the image for rendering. Defaults to the maximum size of the console window
        public Size ImageMaxSize { get; set; }

        // The color to use when the pixel is transparent
        public ConsoleColor TransparencyColor { get; set; }

        //public string SaveResizedImageAs { get; set; }

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

        private IRenderStrategy _renderStrategy;
        public IRenderStrategy RenderStrategy
        {
            get { return _renderStrategy ?? (_renderStrategy = new ProgressiveRenderStrategy()); }
            set { _renderStrategy = value; }
        }
        //bmp.Save(@"C:\Users\awhitworth\Pictures\Cute-Cats-063-resize.jpg");

        public void Validate()
        {
            if (ConsoleStart.X + ImageMaxSize.Width > Console.LargestWindowWidth)
                throw new Exception("Window is too small");
            if (ConsoleStart.Y + ImageMaxSize.Height > Console.LargestWindowHeight)
                throw new Exception("Window is too short");
        }

        public void SetMaxImageSize()
        {
            ImageMaxSize = new Size
            {
                Width = Console.LargestWindowHeight,
                Height = Console.LargestWindowHeight
            };
        }
    }
}
