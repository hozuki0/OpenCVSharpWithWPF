using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using OpenCvSharp;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using Reactive.Bindings;
using System.Threading;

namespace ImageProcessing.ViewModel
{
    public class WebCameraViewModel
    {
        private Model.IModel model;

        public ReactiveProperty<float> Threshold { get; set; } = new ReactiveProperty<float>();
        public ReactiveProperty<ImageSource> Image => model.Image;

        public Command.ClickCommand DefaultCommand { get; } = new Command.ClickCommand();
        public Command.ClickCommand BinarizationCommand { get; } = new Command.ClickCommand();
        public Command.ClickCommand GrayScaleCommand { get; } = new Command.ClickCommand();

        public Mode Current { get; private set; }

        public WebCameraViewModel()
        {
            model = new Model.WebCameraModel();

            DefaultCommand.OnClick.Subscribe(_ =>
            {
                model.Reset();
                model.Apply();
                Current = Mode.Default;
            });
            BinarizationCommand.OnClick.Subscribe(_ =>
            {
                model.Reset();
                Utility.ImageProcessingUtility.Binarization(model.ImageMat, Threshold.Value);
                model.Apply();
                Current = Mode.Binarization;
            });
            GrayScaleCommand.OnClick.Subscribe(_ =>
            {
                model.Reset();
                Utility.ImageProcessingUtility.GrayScale(model.ImageMat);
                model.Apply();
                Current = Mode.GrayScale;
            });

            var camera = new VideoCapture(0)
            {
                FrameWidth = 500,
                FrameHeight = 450,
            };

            Task.Run(() =>
            {
                using (var img = new Mat())
                using (camera)
                {
                    while (true)
                    {
                        camera.Read(img);
                        switch (Current)
                        {
                            case Mode.Default:
                                break;
                            case Mode.Binarization:
                                Utility.ImageProcessingUtility.Binarization(img, Threshold.Value);
                                break;
                            case Mode.GrayScale:
                                Utility.ImageProcessingUtility.GrayScale(img);
                                break;
                            default:
                                break;
                        }
                        model.ImageMat = img.Flip(FlipMode.Y);
                        model.Apply();
                    }
                }
            });
        }
    }
}
