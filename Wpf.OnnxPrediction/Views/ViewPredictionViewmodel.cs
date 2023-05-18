using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Wpf.BindableBase;
using Wpf.BindableBase.Commands;
using Wpf.OnnxPrediction.Navigation;
using Wpf.OnnxPrediction.Process;

namespace Wpf.OnnxPrediction.Views
{
    internal class ViewPredictionViewmodel : ViewmodelBase
    {
        private readonly IServiceProvider serviceProvider;

        private OnnxPredictionProvider predictionProvider;

        public ICommand ClosePredictionCommand { get; }

        public ICommand UploadImageCommand { get; }

        public ViewPredictionViewmodel(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.ClosePredictionCommand = new SimpleCommand(closePrediction);
            this.UploadImageCommand = new SimpleCommand(uploadImage);
            this.predictionProvider = new OnnxPredictionProvider();
            this.predictionProvider.ImageLoaded += PredictionProvider_ImageLoaded;
            this.predictionProvider.ImagePredicted += PredictionProvider_ImagePredicted;
        }

        private void PredictionProvider_ImageLoaded(object? sender, ImageLoadedEventArgs e)
        {
            this.ImageSource = e.InputImage;
        }

        private void PredictionProvider_ImagePredicted(object? sender, ImagePredictedEventArgs e)
        {
            this.ImageOutput = e.OutputImage;
            this.SelectedTabIndex = 1;
        }

        public BitmapImage ImageSource { get => imageSource; protected set { imageSource = value; OnPropertyChanged(); } }
        public BitmapImage ImageOutput { get => imageOutput; protected set { imageOutput = value; OnPropertyChanged(); } }

        public int SelectedTabIndex { get => selectedTabIndex; set { selectedTabIndex = value; OnPropertyChanged(); } }

        public void StartPrediction(string imagePath)
        {
            this.SelectedTabIndex = 0;

            Task.Factory.StartNew(() => {

                this.predictionProvider.Predict(imagePath);

            });
        }

        private void closePrediction()
        {
            this.serviceProvider.CreateSingleViewNavigationService(EView.Home).Navigate();
        }
        private void uploadImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files : *.jpg, *.png, *.bmp | *.jpg; *.png; *.bmp";
            openFileDialog.Title = "Choose an image";
            if (openFileDialog.ShowDialog() == true)
            {
                this.StartPrediction(openFileDialog.FileName);
            }
        }

        private BitmapImage imageSource;

        private BitmapImage imageOutput;

        private int selectedTabIndex = 0;
    }
}
