using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Wpf.OnnxPrediction.Process
{
    internal class OnnxPredictionProvider
    {
        private readonly string modelPath = "C:\\Users\\Ryutaros\\Downloads\\u2net.quant.onnx";

        /// <summary>
        /// Input tensor
        /// </summary>
        private Tensor<float> inputTensor;

        /// <summary>
        /// Output tensr
        /// </summary>
        private Tensor<float> outputTensor;

        /// <summary>
        /// Session to prediction
        /// </summary>
        private InferenceSession session;

        /// <summary>
        /// The original size of image
        /// </summary>
        private Size originalSize;

        #region Events

        public event EventHandler<ImageLoadedEventArgs>? ImageLoaded;

        public event EventHandler<ImagePredictedEventArgs>? ImagePredicted;

        private void onImageLoaded(BitmapImage bitmapImage)
        {
            this.ImageLoaded?.Invoke(this, new ImageLoadedEventArgs(bitmapImage));
        }

        private void onImagePredicted(BitmapImage bitmapImage)
        {
            this.ImagePredicted?.Invoke(this, new ImagePredictedEventArgs(bitmapImage));
        }

        #endregion

        public void Predict(string imagePath)
        {
            this.doPreProcessing(imagePath);

            this.doPredict();

            this.doPostProcessing();
        }

        private void doPreProcessing(string imagePath)
        {
            var originalImage = Image.Load<Rgb24>(imagePath);

            this.originalSize = originalImage.Size; 

            var resizedImage = originalImage.Resize(new Size(320, 320));

            inputTensor = resizedImage.ToTensor();

            onImageLoaded(originalImage.ToBitmapImage());
        }

        private void doPredict()
        {
            this.session = new InferenceSession(modelPath);

            var results = this.session.Run(new NamedOnnxValue[] { NamedOnnxValue.CreateFromTensor("input.1", inputTensor) });
            
            if (results.FirstOrDefault()?.Value is not Tensor<float> output) 
            {
                throw new ArgumentNullException(nameof(output));
            }

            outputTensor = output;
        }

        private void doPostProcessing()
        {
            var outputImage = outputTensor.ToImageRgba(inputTensor).Resize(originalSize).ToBitmapImage();

            onImagePredicted(outputImage);
        }
    }
}
