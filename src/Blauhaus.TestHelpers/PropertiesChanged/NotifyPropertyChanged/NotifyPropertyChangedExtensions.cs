using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Blauhaus.Common.TestHelpers.PropertiesChanged.NotifyPropertyChanged
{
    public static class NotifyPropertyChangedExtensions
    {
        public static PropertyChanges<TBindableObject, TProperty>  SubscribeToPropertyChanged<TBindableObject, TProperty>(this TBindableObject bindableObject, 
            Expression<Func<TBindableObject, TProperty>> propertyFunc) where TBindableObject : INotifyPropertyChanged
        {
            return new PropertyChanges<TBindableObject, TProperty>(bindableObject, propertyFunc);
        }
    }
}