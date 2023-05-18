using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.BindableBase;
using Wpf.OnnxPrediction.Views.Layouts;

namespace Wpf.OnnxPrediction.Navigation
{
    internal class LayoutNavigationService<T> : INavigationService where T : ViewmodelBase
    {
        private readonly NavigationStore navigationStore;

        private readonly Func<T> createBarController;

        private readonly Func<T> createContent;

        public LayoutNavigationService(NavigationStore navigationStore, Func<T> createBarController, Func<T> createContent)
        {
            this.navigationStore = navigationStore;
            this.createBarController = createBarController;
            this.createContent = createContent;
        }

        public void Navigate()
        {
            this.navigationStore.CurrentViewmodel = new LayoutViewmodel(createBarController, createContent);
        }
    }
}
