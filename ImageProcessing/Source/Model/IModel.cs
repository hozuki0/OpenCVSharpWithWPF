using OpenCvSharp;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ImageProcessing.Model
{
    public interface IModel
    {
        Mat ImageMat { get; set; }
        ReactiveProperty<ImageSource> Image { get; set; }

        /// <summary>
        /// Mat型をImageSoureに反映する
        /// </summary>
        void Apply();

        void Reset();
    }
}
