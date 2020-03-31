using System.ComponentModel;
using System.Linq;
using Blauhaus.TestHelpers.PropertiesChanged.NotifyPropertyChanged;

namespace Blauhaus.TestHelpers.PropertiesChanged
{
    public static class PropertyChangesExtensions
    {
        public static bool IsSequenceEqual<TNotifyPropertyChanged, T>(this PropertyChanges<TNotifyPropertyChanged, T> propertyChanges, params T[] expectedSequence) where TNotifyPropertyChanged : INotifyPropertyChanged
        {
            return expectedSequence.SequenceEqual(propertyChanges);
        }

    }
}