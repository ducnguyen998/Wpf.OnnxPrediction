using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wpf.BindableBase;
using Wpf.BindableBase.Commands;
using Wpf.OnnxPrediction.Navigation;

namespace Wpf.OnnxPrediction.Views
{
    internal class ViewHomeViewmodel : ViewmodelBase
    {
        private readonly IServiceProvider serviceProvider;

        public ICommand UploadImageCommand { get; }

        public ViewHomeViewmodel(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.UploadImageCommand = new SimpleCommand(uploadImage);
        }

        private void uploadImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files : *.jpg, *.png, *.bmp | *.jpg; *.png; *.bmp";
            openFileDialog.Title = "Choose an image";
            if (openFileDialog.ShowDialog() == true)
            {
                this.serviceProvider.CreateSingleViewNavigationService(EView.Prediction).Navigate();
                this.serviceProvider.GetRequiredService<ViewPredictionViewmodel>().StartPrediction(openFileDialog.FileName);
            }
        }
    }
}
