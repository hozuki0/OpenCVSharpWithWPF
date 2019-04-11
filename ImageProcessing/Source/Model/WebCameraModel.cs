using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OpenCvSharp;
using Reactive.Bindings;

namespace ImageProcessing.Model
{
    public class WebCameraModel : IModel
    {
        public Mat ImageMat { get; set; }
        public ReactiveProperty<ImageSource> Image { get; set; } = new ReactiveProperty<ImageSource>();

        public void Apply()
        {
            var image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(ImageMat);
            int dpi = 96;
            WriteableBitmap bitmap = new WriteableBitmap(image.Width, image.Height, dpi, dpi, PixelFormats.Pbgra32, null);
            using (var stream = ImageMat.ToMemoryStream())
            {
                Image.Value = BitmapFrame.Create(stream, BitmapCreateOptions.IgnoreColorProfile, BitmapCacheOption.OnLoad);
            }
        }

        public void Reset()
        {
            
        }
    }
}
