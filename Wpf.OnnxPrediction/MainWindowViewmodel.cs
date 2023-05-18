using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.BindableBase;
using Wpf.OnnxPrediction.Navigation;
using Wpf.OnnxPrediction.Views;
using Wpf.OnnxPrediction.Views.Bars;
using Wpf.OnnxPrediction.Views.Layouts;

namespace Wpf.OnnxPrediction
{
    internal class MainWindowViewmodel : ViewmodelBase
    {
        private readonly NavigationStore navigationStore;

        public MainWindowViewmodel(NavigationStore navigationStore)
        {
            this.navigationStore = navigationStore;
            this.navigationStore.CurrentViewmodelChanged += () => OnPropertyChanged(nameof(CurrentViewmodel));
        }

        public ViewmodelBase CurrentViewmodel => navigationStore.CurrentViewmodel;

    }
}
