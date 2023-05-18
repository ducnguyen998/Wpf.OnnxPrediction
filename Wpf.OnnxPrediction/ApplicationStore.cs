using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Wpf.BindableBase;
using Wpf.OnnxPrediction.Navigation;
using Wpf.OnnxPrediction.Views;
using Wpf.OnnxPrediction.Views.Bars;

namespace Wpf.OnnxPrediction
{
    internal class ApplicationStore
    {
        private readonly IServiceProvider serviceProvider;

        public ApplicationStore()
        {
            var services = new ServiceCollection();

            #region Services
            services.AddSingleton(s => s.CreateServiceProvider());
            #endregion

            #region Bars
            services.AddSingleton<BarControllerViewmodel>();
            #endregion

            #region Stores
            services.AddSingleton<NavigationStore>();
            #endregion

            #region Views
            services.AddSingleton<ViewHomeViewmodel>();
            services.AddSingleton<ViewPredictionViewmodel>();
            #endregion

            #region Initial Service
            services.AddSingleton(s => s.CreateSingleViewNavigationService(EView.Home));
            #endregion

            #region Windows
            services.AddSingleton<MainWindowViewmodel>();
            services.AddSingleton(s => s.CreateMainWindow());
            #endregion

            this.serviceProvider = services.BuildServiceProvider();
        }

        public void DoStartup()
        {
            serviceProvider.GetRequiredService<INavigationService>().Navigate();
            serviceProvider.GetRequiredService<MainWindow>().Show();
        }
    }

    internal static class ApplicationStoreExtension
    {
        public static IServiceProvider CreateServiceProvider(this IServiceProvider serviceProvider)
        {
            return serviceProvider;
        }

        public static MainWindow CreateMainWindow(this IServiceProvider serviceProvider)
        {
            return new MainWindow() { DataContext = serviceProvider.GetRequiredService<MainWindowViewmodel>() };
        }

        public static INavigationService CreateSingleViewNavigationService(this IServiceProvider serviceProvider, EView eView)
        {
            ViewmodelBase contentViewmodel;

            switch (eView)
            {
                case EView.Home:
                    contentViewmodel = serviceProvider.GetRequiredService<ViewHomeViewmodel>();
                    break;
                case EView.Prediction:
                    contentViewmodel = serviceProvider.GetRequiredService<ViewPredictionViewmodel>();
                    break;
                default:
                    contentViewmodel = serviceProvider.GetRequiredService<ViewHomeViewmodel>();
                    break;
            }

            return new LayoutNavigationService<ViewmodelBase>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.CreateBarControllerViewmodel(),
                () => contentViewmodel);
        }

        public static BarControllerViewmodel CreateBarControllerViewmodel(this IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<BarControllerViewmodel>();
        }
    }
}
