using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.BindableBase;

namespace Wpf.OnnxPrediction.Views.Layouts
{
    internal class LayoutViewmodel : ViewmodelBase
    {
        public LayoutViewmodel(Func<ViewmodelBase> createBarController, Func<ViewmodelBase> createContent)
        {
            this.BarControllerViewmodel = createBarController();
            this.CurrentViewmodel = createContent();
        }
        public ViewmodelBase BarControllerViewmodel { get; }
        public ViewmodelBase CurrentViewmodel { get; }
    }
}
