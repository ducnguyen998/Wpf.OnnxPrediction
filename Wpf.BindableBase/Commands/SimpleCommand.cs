using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.BindableBase.Commands
{
    public class SimpleCommand : CommandBase
    {
        private readonly Action _execute;

        public SimpleCommand(Action execute)
        {
            this._execute = execute;
        }

        public override void Execute(object parameter)
        {
            base.Execute(parameter);

            if (_execute != null)
            {
                _execute();
            }
        }
    }
}
