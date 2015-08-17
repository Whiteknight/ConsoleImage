using System;
using System.Drawing;
using System.Text;

namespace ConsoleImage.Viewer
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: Parse settings from commandline args
            // TODO: Delay parameter so the image doesn't render faster than the screen can handle.

            ImageSettings settings = new ImageSettings {
                //ImageLeft = 50,
                //ImageTop = 60,
                //ImageMaxSize = new Size {
                //    Height = 50,
                //    Width = 50
                //},

                //ImageCropStart = new Point {
                //    X = 10,
                //    Y = 10
                //},

                ImageCropSize = new Size
                {
                    Height = 500,
                    Width = 400
                },

                //ConsoleStart = new Point
                //{
                //    X = 10,
                //    Y = 5
                //},

                //Converter = new GreyscalePixelConverterB()
                TransparencyColor = ConsoleColor.White,
                RenderStrategy = new ProgressiveRenderStrategy()
            };

            Bitmap bitmap = (Bitmap)System.Drawing.Image.FromFile(args[0]);

            //ConsoleImage.DrawAnimate(original, () => Console.KeyAvailable, settings);

            settings.Validate();

            ConsoleManager manager = new ConsoleManager(settings);
            using (IDisposable state = manager.SaveConsoleState().AsDisposable())
            {
                manager.SetForGraphics();
                //manager.ResizeConsoleWindow(image.Size);

                ImageBuilder builder = new ImageBuilder(settings.Sampler, settings.Converter, settings.TransparencyColor.ToColor());
                IImage image = builder.Build(bitmap, ConsoleManager.MaxSize);
                if (settings.ImageCropStart.HasValue || settings.ImageCropSize.HasValue)
                    image = new ImageRegion(image, settings.ImageCropStart, settings.ImageCropSize);


                ConsoleRegion region = new ConsoleRegion(ConsoleManager.Origin, new Size(36, 36), new ProgressiveRenderStrategy());
                region.Draw(image.GetBuffer(0));

                Console.ReadKey();
                region.MoveTo(new Point(36, 36));
            }

            Console.ReadKey();
        }

    }
}