using System.Windows.Input;

namespace Blauhaus.Common.TestHelpers.PropertiesChanged.CanExecuteChanged
{
    public static class CommandExtensions
    {
        public static CanExecuteChanges<TCommand>  SubscribeToCanExecuteChanged<TCommand>(this TCommand bindableObject) where TCommand : ICommand
        {
            return new CanExecuteChanges<TCommand>(bindableObject);
        }
        
    }
}