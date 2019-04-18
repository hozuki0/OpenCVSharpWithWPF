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

namespace ImageProcessing.ViewModel
{
    public class ViewModel
    {
        private Model.IModel model;

        public ReactiveProperty<float> Threshold { get; set; } = new ReactiveProperty<float>();
        public ReactiveProperty<ImageSource> Image => model.Image;

        public Command.ClickCommand DefaultCommand { get; } = new Command.ClickCommand();
        public Command.ClickCommand BinarizationCommand { get; } = new Command.ClickCommand();
        public Command.ClickCommand GrayScaleCommand { get; } = new Command.ClickCommand();
        public Mode Current { get; private set; }

        public ViewModel()
        {
            model = new Model.Model("../../../TestImage/evil_monkey.jpg");

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

            Threshold.Subscribe(n =>
            {
                if (Current == Mode.Binarization)
                {
                    model.Reset();

                    Utility.ImageProcessingUtility.Binarization(model.ImageMat, Threshold.Value);

                    model.Apply();
                }
            });
        }
    }
}
