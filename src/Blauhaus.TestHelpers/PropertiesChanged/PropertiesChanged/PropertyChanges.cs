using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading;
using Blauhaus.Common.Utils.Extensions;
using Blauhaus.TestHelpers.Extensions;

namespace Blauhaus.TestHelpers.PropertiesChanged.PropertiesChanged
{
    /// <summary>
    /// Subscribes to PropertyChanged event handler on any object that implements INotifyPropertyChanged
    /// </summary>
    /// <returns>
    /// A disposable that will unsubscribe from the PropertyChanged event when disposed
    /// </returns>
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
        
        /// <summary>
        /// Unsubscribes from the PropertyChanged event
        /// </summary>
        public void Dispose()
        {
            _bindableObject.PropertyChanged -= BindableObject_PropertyChanged;
        }
        /// <summary>
        ///  Subscribes to PropertyChanged event handler on any object that implements INotifyPropertyChanged and keeps track of the values
        /// of the specified property each time the value changes. 
        /// </summary>
        public static PropertyChanges<TBindableObject, TProperty>  Subscribe(TBindableObject bindableObject, Expression<Func<TBindableObject, TProperty>> propertyFunc)
        {
            return new PropertyChanges<TBindableObject, TProperty>(bindableObject, propertyFunc);
        }

        /// <summary>
        /// Wait until the specified number of property changes is received, or timeout
        /// </summary>
        /// <param name="requiredCount">Number of changes required</param>
        /// <param name="timeoutMs">Milliseconds after which to return regardless of number of property changes</param>
        public PropertyChanges<TBindableObject, TProperty> WaitForChangeCount(int requiredCount, int timeoutMs = 1000)
        {
            WaitForChanges(x => x.Count >= requiredCount, timeoutMs);
            return this;
        }

        /// <summary>
        /// Wait until the specified number of property changes is received, or timeout
        /// </summary>
        /// <param name="predicate">Matching condition to wait for on this PropertyChanges object</param>
        /// <param name="timeoutMs">Milliseconds after which to return regardless of number of property changes</param>
        public PropertyChanges<TBindableObject, TProperty> WaitForChanges(Expression<Func<PropertyChanges<TBindableObject, TProperty>, bool>> predicate, int timeoutMs = 1000)
        {
            return this.WaitFor(predicate, timeoutMs); 
        }
    }


}