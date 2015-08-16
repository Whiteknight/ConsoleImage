using System;
using System.Drawing;

namespace ConsoleImage
{
    public class ImageRenderer
    {
        // TODO: Option to draw border
        // TODO: Option to draw caption in various positions
        // TODO: Keep a buffer of existing pixels, and only render a pixel if it is different
        // from what is already visible.
        // TODO: Create a ConsoleRegion class which represents the drawable region of the console
        // and offload drawing/positioning logic to that. ImageRenderer takes an image and a
        // ConsoleRegion to render the image to.

        private readonly ImageSettings _settings;

        public ImageRenderer(ImageSettings settings)
        {
            _settings = settings;
        }

        public void Draw(IImage image)
        {
            Draw(image.CurrentBuffer);
        }

        public void Draw(IImage image, int bufferIdx)
        {
            Draw(image.GetBuffer(bufferIdx));
        }

        public void Draw(IImageBuffer imageBuffer)
        {
            _settings.RenderStrategy.Render(_settings.ConsoleStart, imageBuffer);
        }
    }

    public interface IRenderStrategy
    {
        void Render(Point consoleStart, IImageBuffer imageBuffer);
    }

    public abstract class RenderStrategyBase : IRenderStrategy
    {
        #region Implementation of IRenderStrategy

        public void Render(Point consoleStart, IImageBuffer imageBuffer)
        {
            RenderInternal(consoleStart, imageBuffer);
        }

        #endregion

        protected abstract void RenderInternal(Point consoleStart, IImageBuffer imageBuffer);

        protected void RenderRow(Point consoleStart, IImageBuffer imageBuffer, int y)
        {
            Console.SetCursorPosition(consoleStart.X, consoleStart.Y + y);

            for (int x = 0; x < imageBuffer.Size.Width; x++)
                imageBuffer.GetPixel(x, y).Print();
        }
    }

    public class InterlacedRenderStrategy : RenderStrategyBase
    {
        #region Implementation of IRenderStrategy

        protected override void RenderInternal(Point consoleStart, IImageBuffer imageBuffer)
        {
            for (int y = 0; y < imageBuffer.Size.Height; y += 2)
            {
                RenderRow(consoleStart, imageBuffer, y);
            }
            for (int y = 1; y < imageBuffer.Size.Height; y += 2)
            {
                RenderRow(consoleStart, imageBuffer, y);
            }
        }

        #endregion
    }

    public class ProgressiveRenderStrategy : RenderStrategyBase
    {
        #region Implementation of IRenderStrategy

        protected override void RenderInternal(Point consoleStart, IImageBuffer imageBuffer)
        {
            for (int y = 0; y < imageBuffer.Size.Height; y++)
            {
                RenderRow(consoleStart, imageBuffer, y);
            }
        }

        #endregion
    }

    public class Interlaced2RenderStrategy : RenderStrategyBase
    {
        #region Implementation of IRenderStrategy

        protected override void RenderInternal(Point consoleStart, IImageBuffer imageBuffer)
        {
            for (int y = 0; y < imageBuffer.Size.Height; y += 4)
            {
                RenderRow(consoleStart, imageBuffer, y);
            }
            for (int y = 2; y < imageBuffer.Size.Height; y += 4)
            {
                RenderRow(consoleStart, imageBuffer, y);
            }
            for (int y = 1; y < imageBuffer.Size.Height; y += 4)
            {
                RenderRow(consoleStart, imageBuffer, y);
            }
            for (int y = 3; y < imageBuffer.Size.Height; y += 4)
            {
                RenderRow(consoleStart, imageBuffer, y);
            }
        }

        #endregion
    }
}
