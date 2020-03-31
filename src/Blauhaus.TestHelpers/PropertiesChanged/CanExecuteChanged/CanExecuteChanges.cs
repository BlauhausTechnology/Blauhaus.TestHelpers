using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Blauhaus.Common.TestHelpers.PropertiesChanged.CanExecuteChanged
{
    public class CanExecuteChanges<TCommand> : List<bool>, IDisposable where TCommand : ICommand
    {
        private readonly ICommand _command;

        public CanExecuteChanges(TCommand bindableObject)
        {
            _command = bindableObject;
            _command.CanExecuteChanged += Command_CanExecuteChanged; ;
        }

        private void Command_CanExecuteChanged(object sender, EventArgs e)
        {
            var command = (ICommand)sender;
            Add(command.CanExecute(null));
        }


        public void Dispose()
        {
            _command.CanExecuteChanged -= Command_CanExecuteChanged;
        }

        public void WaitForChangeCount(int requiredCount)
        {
            while (Count < requiredCount)
            {
                //Wait...
            }
        }
    }


    

   
}