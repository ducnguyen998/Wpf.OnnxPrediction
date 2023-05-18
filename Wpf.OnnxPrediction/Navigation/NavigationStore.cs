using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.BindableBase;

namespace Wpf.OnnxPrediction.Navigation
{
    internal class NavigationStore
    {
        public event Action? CurrentViewmodelChanged;

        public ViewmodelBase CurrentViewmodel
        {
            get => currentViewmodel;
            set
            {
                currentViewmodel = value;
                OnCurrentViewmodelChanged();
            }
        }

        private void OnCurrentViewmodelChanged()
        {
            this.CurrentViewmodelChanged?.Invoke();
        }

        private ViewmodelBase? currentViewmodel;
    }
}
