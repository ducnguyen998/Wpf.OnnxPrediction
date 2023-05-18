using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Wpf.OnnxPrediction
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IServiceProvider serviceProvider;

        public App()
        {
            var services = new ServiceCollection();

            services.AddSingleton(s => s.CreateServiceProvider());
            services.AddSingleton<ApplicationStore>();

            serviceProvider = services.BuildServiceProvider();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            serviceProvider.GetRequiredService<ApplicationStore>().DoStartup();
        }
    }
}
