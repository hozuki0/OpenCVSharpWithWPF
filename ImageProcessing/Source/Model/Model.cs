using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.Windows.Media;
using Reactive.Bindings;

namespace ImageProcessing.Model
{
    public class Model : IModel
    {
        public Mat ImageMat { get; set; }
        public ReactiveProperty<ImageSource> Image { get; set; } = new ReactiveProperty<ImageSource>();

        public string FilePath { get; set; }

        public Model(string filePath)
        {
            FilePath = filePath;
            Reset();
            Apply();
        }

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
            ImageMat = new Mat(FilePath);
        }
    }
}
