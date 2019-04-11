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
                Binarization(model.ImageMat, Threshold.Value);
                model.Apply();
                Current = Mode.Binarization;
            });
            GrayScaleCommand.OnClick.Subscribe(_ =>
            {
                model.Reset();
                GrayScale(model.ImageMat);
                model.Apply();
                Current = Mode.GrayScale;
            });

            Threshold.Subscribe(n =>
            {
                if (Current == Mode.Binarization)
                {
                    model.Reset();

                    Binarization(model.ImageMat, Threshold.Value);

                    model.Apply();
                }
            });
        }
        private void GrayScale(Mat mat)
        {
            var mat3 = new MatOfByte3(model.ImageMat);
            var indexer = mat3.GetIndexer();

            for (int y = 0; y < model.ImageMat.Height; y++)
            {
                for (int x = 0; x < model.ImageMat.Width; x++)
                {
                    Vec3b color = indexer[y, x];
                    var sum = color.Item0 + color.Item1 + color.Item2;
                    color.Item0 = (byte)(sum / 3);
                    color.Item1 = (byte)(sum / 3);
                    color.Item2 = (byte)(sum / 3);
                    indexer[y, x] = color;
                }
            }
        }

        private void Binarization(Mat mat, float threshold)
        {
            var mat3 = new MatOfByte3(model.ImageMat);
            var indexer = mat3.GetIndexer();

            var bthreshold = threshold * byte.MaxValue;
            for (int y = 0; y < model.ImageMat.Height; y++)
            {
                for (int x = 0; x < model.ImageMat.Width; x++)
                {
                    Vec3b color = indexer[y, x];
                    var sum = color.Item0 + color.Item1 + color.Item2;
                    color.Item0 = (byte)(sum / 3) <= bthreshold ? byte.MinValue : byte.MaxValue;
                    color.Item1 = (byte)(sum / 3) <= bthreshold ? byte.MinValue : byte.MaxValue;
                    color.Item2 = (byte)(sum / 3) <= bthreshold ? byte.MinValue : byte.MaxValue;
                    indexer[y, x] = color;
                }
            }
        }
    }
}
