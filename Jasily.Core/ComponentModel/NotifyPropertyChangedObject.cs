using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace System.ComponentModel
{
    public class NotifyPropertyChangedObject : INotifyPropertyChanged
    {
        public bool SetPropertyRef<T>(ref T property, T newValue, [CallerMemberName] string propertyName = null)
        {
            if ((property == null && newValue == null) || (property != null && property.Equals(newValue)))
            {
                return false;
            }
            else
            {
                property = newValue;
                NotifyPropertyChanged(propertyName);
                return true;
            }
        }
        public bool SetPropertyRef<T>(ref T property, T newValue, params string[] propertyNames)
        {
            if ((property == null && newValue == null) || (property != null && property.Equals(newValue)))
            {
                return false;
            }
            else
            {
                property = newValue;
                NotifyPropertyChanged(propertyNames);
                return true;
            }
        }

        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged.Fire(this, propertyName);
        }
        protected virtual void NotifyPropertyChanged(params string[] propertyNames)
        {
            PropertyChanged.Fire(this, propertyNames);
        }
        protected virtual void NotifyPropertyChanged(IEnumerable<string> propertyNames)
        {
            PropertyChanged.Fire(this, propertyNames);
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
