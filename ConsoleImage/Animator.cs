using System;
using System.Diagnostics;
using System.Threading;

namespace ConsoleImage
{
    public class Animator
    {
        private readonly ConsoleRegion _region;
        private readonly IImage _image;
        private int _frame;
        private bool _alreadyDrawn;

        public Animator(ConsoleRegion region, IImage image)
        {
            _region = region;
            _image = image;
        }

        public long Step()
        {
            if (_alreadyDrawn && _image.NumberOfBuffers == 1)
                return -1;

            IImageBuffer buffer = _image.GetBuffer(_frame);
            _region.Draw(buffer);

            _frame = (_frame + 1) % _image.NumberOfBuffers;
            _alreadyDrawn = true;
            
            object delayObj = buffer.GetProperty(ImagePropertyConstants.GifFrameTimeMs);
            if (delayObj == null)
                return -1;
            return (long)delayObj;
        }

        public void Animate(Func<bool> shouldStop)
        {
            Stopwatch stopwatch = new Stopwatch();
            while (!shouldStop())
            {
                stopwatch.Start();
                long frameMs = Step();
                stopwatch.Stop();
                long elapsed = stopwatch.ElapsedMilliseconds;
                stopwatch.Reset();
                if (elapsed < frameMs)
                    Thread.Sleep((int)(frameMs - elapsed));
            }
        }

        // TODO: Async/threaded methods that will continue to run in the background
    }
}
