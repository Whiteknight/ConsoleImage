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

        private readonly ImageSettings _settings;

        public ImageRenderer(ImageSettings settings)
        {
            _settings = settings;
        }

        public void Draw(Image image)
        {
            Draw(image.CurrentBuffer);
        }

        public void Draw(Image image, int bufferIdx)
        {
            Draw(image.GetBuffer(bufferIdx));
        }

        public void Draw(ImageBuffer imageBuffer)
        {
            Size imageSize = _settings.ImageCropSize.HasValue ? _settings.ImageCropSize.Value : imageBuffer.Size;
            _settings.RenderStrategy.Render(_settings.ConsoleStart, _settings.ImageCropStart, imageSize, imageBuffer);
        }
    }

    public interface IRenderStrategy
    {
        void Render(Point consoleStart, Point imageStart, Size regionSize, ImageBuffer imageBuffer);
    }

    public abstract class RenderStrategyBase : IRenderStrategy
    {
        #region Implementation of IRenderStrategy

        public void Render(Point consoleStart, Point imageStart, Size regionSize, ImageBuffer imageBuffer)
        {
            int maxY = regionSize.Height;
            if (maxY >= imageBuffer.Size.Height - imageStart.Y)
                maxY = imageBuffer.Size.Height - imageStart.Y;

            int maxX = regionSize.Width;
            if (maxX >= imageBuffer.Size.Width - imageStart.X)
                maxX = imageBuffer.Size.Width - imageStart.X;

            Size maxSize = new Size {
                Height = maxY,
                Width = maxX
            };

            RenderInternal(consoleStart, imageStart, maxSize, imageBuffer);
        }

        #endregion

        protected abstract void RenderInternal(Point consoleStart, Point imageStart, Size regionSize, ImageBuffer imageBuffer);

        protected void RenderRow(Point consoleStart, Point imageStart, Size regionSize, ImageBuffer imageBuffer, int y)
        {
            Console.SetCursorPosition(consoleStart.X, consoleStart.Y + y);

            for (int x = 0; x < regionSize.Width; x++)
                imageBuffer.GetPixel(x + imageStart.X, y + imageStart.Y).Print();
        }
    }

    public class InterlacedRenderStrategy : RenderStrategyBase
    {
        #region Implementation of IRenderStrategy

        protected override void RenderInternal(Point consoleStart, Point imageStart, Size regionSize, ImageBuffer imageBuffer)
        {
            for (int y = 0; y < regionSize.Height; y += 2)
            {
                RenderRow(consoleStart, imageStart, regionSize, imageBuffer, y);
            }
            for (int y = 1; y < regionSize.Height; y += 2)
            {
                RenderRow(consoleStart, imageStart, regionSize, imageBuffer, y);
            }
        }

        #endregion
    }

    public class ProgressiveRenderStrategy : RenderStrategyBase
    {
        #region Implementation of IRenderStrategy

        protected override void RenderInternal(Point consoleStart, Point imageStart, Size regionSize, ImageBuffer imageBuffer)
        {
            for (int y = 0; y < regionSize.Height; y++)
            {
                RenderRow(consoleStart, imageStart, regionSize, imageBuffer, y);
            }
        }

        #endregion
    }

    public class Interlaced2RenderStrategy : RenderStrategyBase
    {
        #region Implementation of IRenderStrategy

        protected override void RenderInternal(Point consoleStart, Point imageStart, Size regionSize, ImageBuffer imageBuffer)
        {
            for (int y = 0; y < regionSize.Height; y += 4)
            {
                RenderRow(consoleStart, imageStart, regionSize, imageBuffer, y);
            }
            for (int y = 2; y < regionSize.Height; y += 4)
            {
                RenderRow(consoleStart, imageStart, regionSize, imageBuffer, y);
            }
            for (int y = 1; y < regionSize.Height; y += 4)
            {
                RenderRow(consoleStart, imageStart, regionSize, imageBuffer, y);
            }
            for (int y = 3; y < regionSize.Height; y += 4)
            {
                RenderRow(consoleStart, imageStart, regionSize, imageBuffer, y);
            }
        }

        #endregion
    }
}
