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

        protected void DrawRow(Point imageStart, Size regionSize, ImageBuffer imageBuffer, int rowIdx)
        {
            
        }

        #region Implementation of IRenderStrategy

        public abstract void Render(Point consoleStart, Point imageStart, Size regionSize, ImageBuffer imageBuffer);

        #endregion
    }

    //public class InterlacedRenderStrategy : RenderStrategyBase
    //{
    //    #region Implementation of IRenderStrategy

    //    public override void Render(int left, int top, Size size, ImageBuffer imageBuffer)
    //    {
    //        for (int i = 0; i < size.Height; i += 2)
    //        {
    //            Console.SetCursorPosition(left, top + i);
    //            DrawRow(size, imageBuffer, i);
    //        }
    //        for (int i = 1; i < size.Height; i += 2)
    //        {
    //            Console.SetCursorPosition(left, top + i);
    //            DrawRow(size, imageBuffer, i);
    //        }
    //    }

    //    #endregion
    //}

    public class ProgressiveRenderStrategy : RenderStrategyBase
    {
        #region Implementation of IRenderStrategy

        public override void Render(Point consoleStart, Point imageStart, Size regionSize, ImageBuffer imageBuffer)
        {
            int maxY = regionSize.Height;
            if (maxY >= imageBuffer.Size.Height - imageStart.Y)
                maxY = imageBuffer.Size.Height - imageStart.Y;

            int maxX = regionSize.Width;
            if (maxX >= imageBuffer.Size.Width - imageStart.X)
                maxX = imageBuffer.Size.Width - imageStart.X;

            for (int y = 0; y < maxY; y++)
            {
                Console.SetCursorPosition(consoleStart.X, consoleStart.Y + y);

                for (int x = 0; x < maxX; x++)
                {
                    ConsolePixel p = imageBuffer.GetPixel(x + imageStart.X, y + imageStart.Y);
                    p.Print();
                }
            }
        }

        #endregion
    }

    //public class Interlaced2RenderStrategy : RenderStrategyBase
    //{
    //    #region Implementation of IRenderStrategy

    //    public override void Render(int left, int top, Size size, ImageBuffer imageBuffer)
    //    {
    //        for (int i = 0; i < size.Height; i += 4)
    //        {
    //            Console.SetCursorPosition(left, top + i);
    //            DrawRow(size, imageBuffer, i);
    //        }
    //        for (int i = 2; i < size.Height; i += 4)
    //        {
    //            Console.SetCursorPosition(left, top + i);
    //            DrawRow(size, imageBuffer, i);
    //        }
    //        for (int i = 1; i < size.Height; i += 2)
    //        {
    //            Console.SetCursorPosition(left, top + i);
    //            DrawRow(size, imageBuffer, i);
    //        }
    //        for (int i = 3; i < size.Height; i += 2)
    //        {
    //            Console.SetCursorPosition(left, top + i);
    //            DrawRow(size, imageBuffer, i);
    //        }
    //    }

    //    #endregion
    //}
}
