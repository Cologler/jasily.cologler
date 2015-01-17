using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.ComponentModel
{
    public class NotifyPropertyChangedObject : INotifyPropertyChanged
    {
        public void SetPropertyRef<T>(ref T property, T newValue, string propertyName)
        {
            if ((property == null && newValue == null) || (property != null && property.Equals(newValue)))
                return;

            property = newValue;
            PropertyChanged.Fire(this, propertyName);
        }
        public void SetPropertyRef<T>(ref T property, T newValue, params string[] propertyNames)
        {
            if ((property == null && newValue == null) || (property != null && property.Equals(newValue)))
                return;
            
            property = newValue;
            foreach (var propertyName in propertyNames)
                PropertyChanged.Fire(this, propertyName);
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
