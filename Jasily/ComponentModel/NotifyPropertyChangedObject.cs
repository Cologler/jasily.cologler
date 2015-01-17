using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.ComponentModel
{
    public class NotifyPropertyChangedObject : INotifyPropertyChanged
    {
        public bool SetPropertyRef<T>(ref T property, T newValue, string propertyName)
        {
            if ((property == null && newValue == null) || (property != null && property.Equals(newValue)))
                return false;

            property = newValue;
            PropertyChanged.Fire(this, propertyName);

            return true;
        }
        public bool SetPropertyRef<T>(ref T property, T newValue, params string[] propertyNames)
        {
            if ((property == null && newValue == null) || (property != null && property.Equals(newValue)))
                return false;
            
            property = newValue;
            foreach (var propertyName in propertyNames)
                PropertyChanged.Fire(this, propertyName);

            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public static class JasilyINotifyPropertyChangedEM
    {
        public static void Fire(this PropertyChangedEventHandler e, object sender, string propertyName)
        {
            if (e != null)
                e(sender, new PropertyChangedEventArgs(propertyName));
        }
    }
}
