using System;
using System.Drawing;

namespace ConsoleImage
{
    public interface IRenderStrategy
    {
        void Render(ConsoleRegion region, IImageBuffer imageBuffer);
    }

    public abstract class RenderStrategyBase : IRenderStrategy
    {
        #region Implementation of IRenderStrategy

        public void Render(ConsoleRegion region, IImageBuffer imageBuffer)
        {
            RenderInternal(region, imageBuffer);
        }

        #endregion

        protected abstract void RenderInternal(ConsoleRegion region, IImageBuffer imageBuffer);
    }

    public class ProgressiveRenderStrategy : RenderStrategyBase
    {
        #region Implementation of IRenderStrategy

        protected override void RenderInternal(ConsoleRegion region, IImageBuffer imageBuffer)
        {
            for (int y = 0; y < imageBuffer.Size.Height; y++)
            {
                region.DrawRow(y, imageBuffer.GetRow(y));
            }
        }

        #endregion
    }

    //public class InterlacedRenderStrategy : RenderStrategyBase
    //{
    //    #region Implementation of IRenderStrategy

    //    protected override void RenderInternal(Point consoleStart, IImageBuffer imageBuffer)
    //    {
    //        for (int y = 0; y < imageBuffer.Size.Height; y += 2)
    //        {

    //            RenderRow(consoleStart, imageBuffer, y);
    //        }
    //        for (int y = 1; y < imageBuffer.Size.Height; y += 2)
    //        {
    //            RenderRow(consoleStart, imageBuffer, y);
    //        }
    //    }

    //    #endregion
    //}

    //public class Interlaced2RenderStrategy : RenderStrategyBase
    //{
    //    #region Implementation of IRenderStrategy

    //    protected override void RenderInternal(Point consoleStart, IImageBuffer imageBuffer)
    //    {
    //        for (int y = 0; y < imageBuffer.Size.Height; y += 4)
    //        {
    //            RenderRow(consoleStart, imageBuffer, y);
    //        }
    //        for (int y = 2; y < imageBuffer.Size.Height; y += 4)
    //        {
    //            RenderRow(consoleStart, imageBuffer, y);
    //        }
    //        for (int y = 1; y < imageBuffer.Size.Height; y += 4)
    //        {
    //            RenderRow(consoleStart, imageBuffer, y);
    //        }
    //        for (int y = 3; y < imageBuffer.Size.Height; y += 4)
    //        {
    //            RenderRow(consoleStart, imageBuffer, y);
    //        }
    //    }

    //    #endregion
    //}
}
