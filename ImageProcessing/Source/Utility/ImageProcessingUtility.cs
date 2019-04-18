using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing.Utility
{
    public static class ImageProcessingUtility
    {
        public static void GrayScale(Mat mat)
        {
            var mat3 = new MatOfByte3(mat);
            var indexer = mat3.GetIndexer();

            for (int y = 0; y < mat.Height; y++)
            {
                for (int x = 0; x < mat.Width; x++)
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

        public static void Binarization(Mat mat, float threshold)
        {
            var mat3 = new MatOfByte3(mat);
            var indexer = mat3.GetIndexer();

            var bthreshold = threshold * byte.MaxValue;
            for (int y = 0; y < mat.Height; y++)
            {
                for (int x = 0; x < mat.Width; x++)
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
