using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Wpf.OnnxPrediction.Process
{
    internal class ImagePredictedEventArgs : EventArgs
    {
        public ImagePredictedEventArgs(BitmapImage outputImage)
        {
            OutputImage = outputImage;
        }

        public BitmapImage OutputImage { get; set; }
    }
}
