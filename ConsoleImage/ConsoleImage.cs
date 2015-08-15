﻿using System;
using System.Drawing;
using System.Text;

namespace ConsoleImage
{
    public class ConsoleImage
    {
        public static void Draw(Bitmap bitmap, ImageSettings settings = null)
        {
            if (settings == null)
                settings = new ImageSettings();

            settings.Validate();

            // TODO: using(IDisposable state = ConsoleState.GetRestoreOnDisposeState()) { ... }
            ConsoleState state = ConsoleState.GetState();
            Console.OutputEncoding = Encoding.GetEncoding(1252);

            Image image = new Image(bitmap, settings);
            ImageRenderer renderer = new ImageRenderer(settings);
            renderer.ResizeConsoleWindow(image);
            renderer.Draw(image);

            state.ResetConsole();
        }

        public static void DrawAnimate(Bitmap bitmap, Func<bool> shouldStop, ImageSettings settings = null)
        {
            if (settings == null)
                settings = new ImageSettings();

            settings.Validate();

            ConsoleState state = ConsoleState.GetState();
            Console.OutputEncoding = Encoding.GetEncoding(1252);

            Image image = new Image(bitmap, settings);

            ImageRenderer renderer = new ImageRenderer(settings);
            renderer.ResizeConsoleWindow(image);

            bool shouldBreak = false;
            while (!shouldBreak)
            {
                foreach (ImageBuffer buffer in image.Buffers)
                {
                    if (shouldStop())
                    {
                        shouldBreak = true;
                        break;
                    }
                    renderer.Draw(image.Size, buffer);
                }
            }

            state.ResetConsole();
        }
    }
}
