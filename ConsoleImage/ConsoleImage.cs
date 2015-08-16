using System;
using System.Drawing;
using System.Text;
using System.Threading;

namespace ConsoleImage
{
    public class ConsoleImage
    {
        // TODO: Simultaneously draw several images, in regions that possibly overlap (with ordering)
        // TODO: Ability to animate multiple images at once. 
        public static void Draw(Bitmap bitmap, ImageSettings settings = null)
        {
            if (settings == null)
                settings = new ImageSettings();

            settings.Validate();

            // TODO: using(IDisposable state = ConsoleState.GetRestoreOnDisposeState()) { ... }
            ConsoleState state = ConsoleState.GetState();
            Console.OutputEncoding = Encoding.GetEncoding(1252);

            IImage image = new Image(bitmap, settings);
            if (settings.ImageCropStart.HasValue || settings.ImageCropSize.HasValue)
                image = new ImageRegion(image, settings.ImageCropStart, settings.ImageCropSize);

            new ConsoleManager(settings).ResizeConsoleWindow(image.Size);
            ConsoleRegion region = new ConsoleRegion(settings.ConsoleStart, settings.ImageMaxSize, settings.RenderStrategy);
            region.Draw(image);

            state.ResetConsole();
        }

        public static void DrawAnimate(Bitmap bitmap, Func<bool> shouldStop, ImageSettings settings = null)
        {
            if (settings == null)
                settings = new ImageSettings();

            settings.Validate();

            ConsoleState state = ConsoleState.GetState();
            Console.OutputEncoding = Encoding.GetEncoding(1252);

            IImage image = new Image(bitmap, settings);
            if (settings.ImageCropStart.HasValue || settings.ImageCropSize.HasValue)
                image = new ImageRegion(image, settings.ImageCropStart, settings.ImageCropSize);

            new ConsoleManager(settings).ResizeConsoleWindow(image.Size);
            ConsoleRegion region = new ConsoleRegion(settings.ConsoleStart, settings.ImageMaxSize, settings.RenderStrategy);

            bool shouldBreak = false;
            while (!shouldBreak)
            {
                if (image.NumberOfBuffers == 1)
                {
                    region.Draw(image.CurrentBuffer);
                    while (!shouldStop())
                        Thread.Sleep(200);
                    shouldBreak = true;
                }
                else
                {
                    // TODO: Timing delay for GIFs when things are moving too fast.
                    foreach (ImageBuffer buffer in image.Buffers)
                    {
                        if (shouldStop())
                        {
                            shouldBreak = true;
                            break;
                        }
                        region.Draw(buffer);
                    }
                }
            }

            state.ResetConsole();
        }
    }
}
