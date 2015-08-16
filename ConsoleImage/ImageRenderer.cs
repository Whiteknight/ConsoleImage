//using System;
//using System.Drawing;

//namespace ConsoleImage
//{
//    public class ImageRenderer
//    {
//        // TODO: Option to draw border
//        // TODO: Option to draw caption in various positions
//        // TODO: Keep a buffer of existing pixels, and only render a pixel if it is different
//        // from what is already visible.
//        // TODO: Create a ConsoleRegion class which represents the drawable region of the console
//        // and offload drawing/positioning logic to that. ImageRenderer takes an image and a
//        // ConsoleRegion to render the image to.

//        private readonly ImageSettings _settings;

//        public ImageRenderer(ImageSettings settings)
//        {
//            _settings = settings;
//        }

//        public void Draw(IImage image)
//        {
//            Draw(image.CurrentBuffer);
//        }

//        public void Draw(IImage image, int bufferIdx)
//        {
//            Draw(image.GetBuffer(bufferIdx));
//        }

//        public void Draw(IImageBuffer imageBuffer)
//        {
//            //_settings.RenderStrategy.Render(_settings.ConsoleStart, imageBuffer);
//        }
//    }


//}
