using System;
using System.Drawing;

namespace ConsoleImage
{
    public class ImageRenderer
    {
        // TODO: Option to draw border
        // TODO: Option to draw caption in various positions
        // TODO: Move console resizing logic out of this class. Assume the console is the correct size here, or follow
        // existing console bounds only.

        private readonly ImageSettings _settings;

        public ImageRenderer(ImageSettings settings)
        {
            _settings = settings;
        }

        public void Draw(Image image)
        {
            Draw(image.Size, image.CurrentBuffer);
        }

        public void Draw(Image image, int bufferIdx)
        {
            Draw(image.Size, image.GetBuffer(bufferIdx));
        }

        public void Draw(Size size, ImageBuffer imageBuffer)
        {
            _settings.RenderStrategy.Render(_settings.ConsoleLeft, _settings.ConsoleTop - _settings.ImageTop, size, imageBuffer);
        }

    }

    public interface IRenderStrategy
    {
        void Render(int left, int top, Size size, ImageBuffer imageBuffer);
    }

    public abstract class RenderStrategyBase : IRenderStrategy
    {
        protected void DrawRow(Size size, ImageBuffer imageBuffer, int rowIdx)
        {
            for (int j = 0; j < size.Width; j++)
            {
                ConsolePixel p = imageBuffer.GetPixel(j, rowIdx);
                p.Print();
            }
        }

        #region Implementation of IRenderStrategy

        public abstract void Render(int left, int top, Size size, ImageBuffer imageBuffer);

        #endregion
    }

    public class InterlacedRenderStrategy : RenderStrategyBase
    {
        #region Implementation of IRenderStrategy

        public override void Render(int left, int top, Size size, ImageBuffer imageBuffer)
        {
            for (int i = 0; i < size.Height; i += 2)
            {
                Console.SetCursorPosition(left, top + i);
                DrawRow(size, imageBuffer, i);
            }
            for (int i = 1; i < size.Height; i += 2)
            {
                Console.SetCursorPosition(left, top + i);
                DrawRow(size, imageBuffer, i);
            }
        }

        #endregion
    }

    public class ProgressiveRenderStrategy : RenderStrategyBase
    {
        #region Implementation of IRenderStrategy

        public override void Render(int left, int top, Size size, ImageBuffer imageBuffer)
        {
            for (int i = 0; i < size.Height; i++)
            {
                Console.SetCursorPosition(left, top + i);
                DrawRow(size, imageBuffer, i);
            }
        }

        #endregion
    }

    public class Interlaced2RenderStrategy : RenderStrategyBase
    {
        #region Implementation of IRenderStrategy

        public override void Render(int left, int top, Size size, ImageBuffer imageBuffer)
        {
            for (int i = 0; i < size.Height; i += 4)
            {
                Console.SetCursorPosition(left, top + i);
                DrawRow(size, imageBuffer, i);
            }
            for (int i = 2; i < size.Height; i += 4)
            {
                Console.SetCursorPosition(left, top + i);
                DrawRow(size, imageBuffer, i);
            }
            for (int i = 1; i < size.Height; i += 2)
            {
                Console.SetCursorPosition(left, top + i);
                DrawRow(size, imageBuffer, i);
            }
            for (int i = 3; i < size.Height; i += 2)
            {
                Console.SetCursorPosition(left, top + i);
                DrawRow(size, imageBuffer, i);
            }
        }

        #endregion
    }
}
