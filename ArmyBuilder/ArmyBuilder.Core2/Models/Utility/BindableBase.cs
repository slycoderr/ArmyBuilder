using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace ArmyBuilder.Core.Models.Utility
{
    [XmlRoot]
    public abstract class BindableBase : INotifyPropertyChanged
    {
       public event PropertyChangedEventHandler PropertyChanged;

       protected bool SetValue<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
       {
           if (!EqualityComparer<T>.Default.Equals(storage, value))
           {
               storage = value;
               OnPropertyChanged(propertyName);
               return true;
           }

           return false;
       }

       protected void OnPropertyChanged(string propertyName)
       {
           var handler = PropertyChanged;

           handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
       }
    }
}
