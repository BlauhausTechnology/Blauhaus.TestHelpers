using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using Blauhaus.Common.Utils.Extensions;

namespace Blauhaus.Common.TestHelpers.PropertiesChanged.NotifyPropertyChanged
{
    public class PropertyChanges<TBindableObject, TProperty> : List<TProperty>, IDisposable where TBindableObject : INotifyPropertyChanged
    {
        private readonly INotifyPropertyChanged _bindableObject;
        private readonly Func<TBindableObject, TProperty> _propertyFunc;
        private readonly string _propertyName;

        public PropertyChanges(TBindableObject bindableObject, Expression<Func<TBindableObject, TProperty>> propertyFunc)
        {
            _bindableObject = bindableObject;
            _propertyFunc = propertyFunc.Compile();
            _propertyName = propertyFunc.ToPropertyName();
            _bindableObject.PropertyChanged += BindableObject_PropertyChanged;
        }

        private void BindableObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == _propertyName)
            {
                Add(_propertyFunc.Invoke((TBindableObject)sender));
            }
        }
        
        public void Dispose()
        {
            _bindableObject.PropertyChanged -= BindableObject_PropertyChanged;
        }

        public static PropertyChanges<TBindableObject, TProperty>  Subscribe(TBindableObject bindableObject, Expression<Func<TBindableObject, TProperty>> propertyFunc)
        {
            return new PropertyChanges<TBindableObject, TProperty>(bindableObject, propertyFunc);
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