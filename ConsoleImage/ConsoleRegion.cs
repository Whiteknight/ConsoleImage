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
        private Point _start;
        // TODO: Separate out the total _size of the region from the effective usable _size.
        // This way we can create decorator/wrapper types to add things like titles and borders.
        private readonly Size _size;

        public static ConsoleRegion WholeWindow(IRenderStrategy renderStrategy = null)
        {
            return new ConsoleRegion(new Point(0, 0), ConsoleManager.MaxSize, renderStrategy);
        }

        public ConsoleRegion(Point? start, Size? size, IRenderStrategy renderStrategy = null)
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

        // TODO: Move string write/print methods to a wrapper type that does text only
        // TODO: Make text scrollable, by adding lines of text to the bottom and scrolling
        // everything else up.
        public void Write(string fmt, params object[] args)
        {
            Write(ColorScheme.Default, fmt, args);
        }

        public void Write(ColorScheme scheme, string fmt, params object[] args)
        {
            ColorScheme saved = ColorScheme.Get();
            scheme.Set();

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

            saved.Set();
        }

        // TODO: Move drawing methods to a wrapper type that handles graphics only
        public void DrawRow(int top, IEnumerable<ConsolePixel> pixels)
        {
            if (top >= _size.Height)
                return;
            SetCursorPosition(0, top);
            foreach (ConsolePixel pixel in pixels.Take(_size.Width))
                pixel.Print();
        }

        public void MoveTo(Point p)
        {
            Console.MoveBufferArea(_start.X, _start.Y, _size.Width, _size.Height, p.X, p.Y);
            _start = p;
        }

        public void Draw(IImageBuffer imageBuffer, Point? start = null)
        {
            Point p = start.HasValue ? start.Value : new Point(0, 0);
            Size size = imageBuffer.Size.BestFitWithin(p, _size);
            imageBuffer = new ImageBufferRegion(imageBuffer, p, size);
            _renderStrategy.Render(this, imageBuffer);
        }

        public void Clear()
        {
            Clear(ColorScheme.Default);
        }

        public void Clear(ColorScheme scheme)
        {
            ColorScheme saved = ColorScheme.Get();
            scheme.Set();

            string s = new string(' ', _size.Width);
            for (int i = 0; i < _size.Height; i++)
            {
                SetCursorPosition(0, i);
                Console.Write(s);
            }

            saved.Set();
        }
    }
}
