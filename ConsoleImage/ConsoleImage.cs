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
        // TODO: This class should go away, when the interfaces for the other objects are clean enough
        public static void Draw(Bitmap bitmap, ImageSettings settings = null)
        {
            if (settings == null)
                settings = new ImageSettings();

            settings.Validate();

            ConsoleManager manager = new ConsoleManager();
            using (IDisposable state = manager.SaveConsoleState().AsResetOnDispose())
            {
                manager.SetForGraphics();
                //manager.ResizeConsoleWindow(image.Size);

                ImageBuilder builder = new ImageBuilder(settings.Sampler, settings.Converter, settings.TransparencyColor.ToColor());
                IImage image = builder.Build(bitmap, ConsoleManager.MaxSize);
                if (settings.ImageCropStart.HasValue || settings.ImageCropSize.HasValue)
                    image = new ImageRegion(image, settings.ImageCropStart, settings.ImageCropSize);

                
                ConsoleRegion region = new ConsoleRegion(settings.ConsoleStart, settings.ImageMaxSize, settings.RenderStrategy);
                region.Draw(image.GetBuffer(0));
            }
        }

        public static void DrawAnimate(Bitmap bitmap, Func<bool> shouldStop, ImageSettings settings = null)
        {
            if (settings == null)
                settings = new ImageSettings();

            settings.Validate();

            ConsoleManager manager = new ConsoleManager();
            using (IDisposable state = manager.SaveConsoleState().AsResetOnDispose())
            {
                manager.SetForGraphics();
                manager.ResizeConsoleWindow(settings.ImageMaxSize);

                ImageBuilder builder = new ImageBuilder(settings.Sampler, settings.Converter, settings.TransparencyColor.ToColor());
                IImage image = builder.Build(bitmap, settings.ImageMaxSize);
                if (settings.ImageCropStart.HasValue || settings.ImageCropSize.HasValue)
                    image = new ImageRegion(image, settings.ImageCropStart, settings.ImageCropSize);

                new ConsoleManager().ResizeConsoleWindow(image.Size);
                ConsoleRegion region = new ConsoleRegion(settings.ConsoleStart, settings.ImageMaxSize, settings.RenderStrategy);

                Animator animator = new Animator(region, image);
                animator.Animate(shouldStop);
            }
        }
    }
}
