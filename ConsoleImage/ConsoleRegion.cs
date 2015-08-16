using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ConsoleImage
{
    // An area on the console to render to.
    public class ConsoleRegion
    {
        private readonly IRenderStrategy _renderStrategy;
        private readonly Point _start;
        private readonly Size _size;

        public static ConsoleRegion WholeWindow(IRenderStrategy renderStrategy = null)
        {
            return new ConsoleRegion(new Point(0, 0), ConsoleManager.MaxSize, renderStrategy);
        }

        public ConsoleRegion(Point? start, Size? size, IRenderStrategy renderStrategy)
        {
            _renderStrategy = renderStrategy ?? new ProgressiveRenderStrategy();
            _start = start.HasValue ? start.Value : new Point(0, 0);

            _size = ConsoleManager.MaxSize;
            if (size.HasValue)
                _size = _size.BestFitWithin(_start, size.Value);
        }

        static IEnumerable<string> SplitStringToChunks(string str, int chunkSize)
        {
            int numChunks = str.Length / chunkSize;

            IEnumerable<string> wholeChunks = Enumerable.Range(0, numChunks)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
            foreach (string chunk in wholeChunks)
            {
                yield return chunk;
            }

            int rem = str.Length % chunkSize;
            if (rem > 0)
                yield return str.Substring(numChunks  * chunkSize);
        }

        private void SetCursorPosition(Point p)
        {
            // TODO: Validate?
            Console.SetCursorPosition(p.X + _start.X, p.Y + _start.Y);
        }

        private void SetCursorPosition(int x, int y)
        {
            // TODO: Validate?
            Console.SetCursorPosition(x + _start.X, y + _start.Y);
        }

        public void Write(string fmt, params object[] args)
        {
            string s = string.Format(fmt, args);
            SetCursorPosition(new Point(0, 0));
            List<string> chunks = SplitStringToChunks(s, _size.Width)
                .Take(_size.Height)
                .ToList();
            for (int i = 0; i < chunks.Count; i++)
            {
                SetCursorPosition(0, i);
                Console.Write(chunks[i]);
            }
        }

        public void DrawRow(int top, IEnumerable<ConsolePixel> pixels)
        {
            if (top >= _size.Height)
                return;
            SetCursorPosition(0, top);
            foreach (ConsolePixel pixel in pixels.Take(_size.Width))
                pixel.Print();
        }

        public void Draw(IImage image)
        {
            Draw(image.CurrentBuffer);
        }

        public void Draw(IImageBuffer imageBuffer)
        {
            imageBuffer = new ImageBufferRegion(imageBuffer, new Point(0, 0), _size);
            _renderStrategy.Render(this, imageBuffer);
        }
    }
}
