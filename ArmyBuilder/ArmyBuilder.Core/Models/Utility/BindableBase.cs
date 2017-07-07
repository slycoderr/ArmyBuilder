using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Slycoder.MVVM
{
    [DataContract(Namespace = "")]
    public abstract class BindableBase : INotifyPropertyChanged
    {
       public event PropertyChangedEventHandler PropertyChanged;

       protected virtual bool SetValue<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
       {
           if (!EqualityComparer<T>.Default.Equals(storage, value))
           {
               storage = value;
               OnPropertyChanged(propertyName);
               return true;
           }

           return false;
       }

       protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
       {
           var property = (MemberExpression)propertyExpression.Body;
           OnPropertyChanged(property.Member.Name);
       }
 
       protected void OnPropertyChanged(string propertyName)
       {
           var handler = PropertyChanged;

           handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
       }
    }
}
