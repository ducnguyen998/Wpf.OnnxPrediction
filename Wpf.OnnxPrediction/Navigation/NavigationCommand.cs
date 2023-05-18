using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.BindableBase;

namespace Wpf.OnnxPrediction.Navigation
{
    internal class NavigationCommand : CommandBase
    {
        private readonly INavigationService navigationService;

        public NavigationCommand(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public override void Execute(object parameter)
        {
            navigationService.Navigate();
        }
    }
}
