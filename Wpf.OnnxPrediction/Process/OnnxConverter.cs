using Microsoft.ML.OnnxRuntime.Tensors;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Wpf.OnnxPrediction.Process
{
    internal static class OnnxConverter
    {
        public static Image<Rgb24> Resize(this Image<Rgb24> image, Size size)
        {
            var resizedImage = image.Clone(ctx =>
            {
                ctx.Resize(new ResizeOptions
                {
                    Size = size,
                    Mode = ResizeMode.Stretch
                });
            });

            return resizedImage;
        }

        public static Image Resize(this Image image, Size size)
        {
            var resizedImage = image.Clone(ctx =>
            {
                ctx.Resize(new ResizeOptions
                {
                    Size = size,
                    Mode = ResizeMode.Stretch
                });
            });

            return resizedImage;
        }

        public static BitmapImage ToBitmapImage(this Image image)
        {
            using (var memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, new PngEncoder());
                memoryStream.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }

        public static Tensor<float> ToTensor(this Image<Rgb24> image)
        {
            var tensor = new DenseTensor<float>(new int[] { 1, 3, image.Height, image.Width });

            image.ProcessPixelRows(pixelAccessor => {
                for (int y = 0; y < pixelAccessor.Height; y++)
                {
                    Span<Rgb24> pixelRowSpan = pixelAccessor.GetRowSpan(y);

                    for (int x = 0; x < pixelRowSpan.Length; x++)
                    {
                        Rgb24 pixel = pixelRowSpan[x];
                        tensor[0, 0, y, x] = pixel.R / 255.0f;
                        tensor[0, 1, y, x] = pixel.G / 255.0f;
                        tensor[0, 2, y, x] = pixel.B / 255.0f;
                    }
                }
            });

            return tensor;
        }

        public static Image ToImageRgb24(this Tensor<float> tensor)
        {
            int height = tensor.Dimensions[2];

            int width  = tensor.Dimensions[3];

            var image = new Image<Rgb24>(width, height);

            // Iterate over the pixels and set the corresponding Rgb24 color in the image
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Get the RGB values from the tensor
                    float r = tensor[0, 0, y, x];
                    float g = tensor[0, 1, y, x];
                    float b = tensor[0, 2, y, x];

                    // Convert the RGB values to byte values (0-255)
                    byte byteR = (byte)(r * 255);
                    byte byteG = (byte)(g * 255);
                    byte byteB = (byte)(b * 255);

                    // Create the Rgb24 color
                    var color = new Rgb24(byteR, byteG, byteB);

                    // Set the color in the image
                    image[x, y] = color;
                }
            }

            return image;
        }

        public static Image ToImageL8(this Tensor<float> tensor)
        {

            int height = tensor.Dimensions[2];

            int width  = tensor.Dimensions[3];

            var image = new Image<L8>(width, height);

            // Iterate over the pixels and set the corresponding grayscale value in the image
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Get the grayscale value from the tensor
                    float value = tensor[0, 0, y, x];

                    // Convert the grayscale value to a byte value (0-255)
                    byte byteValue = (byte)(value * 255);

                    // Create the grayscale color
                    var color = new L8(byteValue);

                    // Set the color in the image
                    image[x, y] = color;
                }
            }

            return image;
        }

        public static Image ToImageRgba(this Tensor<float> maskTensor, Tensor<float> inputTensor) 
        {

            // Get the width and height of the tensors
            int width  = maskTensor.Dimensions[3];
            int height = maskTensor.Dimensions[2];

            // Create a new RGBA32 image with the same width and height as the tensors
            var outputImage = new Image<Rgba32>(width, height);

            // Iterate over the pixels and combine the mask and image data
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Get the mask value for the current pixel
                    float maskValue = maskTensor[0, 0, y, x];

                    // Get the image values (RGB) for the current pixel
                    float r = inputTensor[0, 0, y, x];
                    float g = inputTensor[0, 1, y, x];
                    float b = inputTensor[0, 2, y, x];

                    // Convert the float values to byte values (0-255)
                    byte byteR = (byte)(r * 255);
                    byte byteG = (byte)(g * 255);
                    byte byteB = (byte)(b * 255);

                    // Create the RGBA pixel by combining the image RGB values with the mask value
                    var pixel = new Rgba32(byteR, byteG, byteB, (byte)(maskValue * 255));

                    // Set the pixel in the output image
                    outputImage[x, y] = pixel;
                }
            }

            return outputImage;
        }
    }
}
