using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace System.ComponentModel
{
    public class NotifyPropertyChangedObject : INotifyPropertyChanged
    {
        readonly object _syncRootForEndRefresh = new object();
        readonly List<string> _registeredPropertyForEndRefresh;

        public NotifyPropertyChangedObject()
        {
            _registeredPropertyForEndRefresh = new List<string>();
        }

        /// <summary>
        /// please always call on background thread.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertySelector"></param>
        public void RegisterForEndRefresh<T>(Expression<Func<T, object>> propertySelector)
        {
            if (typeof(T).FullName != this.GetType().FullName)
                throw new NotSupportedException("type of source in propertySelector must be current type.");

            lock (_syncRootForEndRefresh)
                _registeredPropertyForEndRefresh.Add(propertySelector.PropertySelector());
        }

        /// <summary>
        /// please always call on UI thread. the method will call PropertyChanged for each Registered property.
        /// </summary>
        public void EndRefresh()
        {
            if (this._registeredPropertyForEndRefresh.Count > 0)
            {
                lock (_syncRootForEndRefresh)
                {
                    if (this._registeredPropertyForEndRefresh.Count > 0)
                    {
                        this.NotifyPropertyChanged(this._registeredPropertyForEndRefresh);
                        this._registeredPropertyForEndRefresh.Clear();
                    }
                }
            }
        }

        public bool SetPropertyRef<T>(ref T property, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (property.NormalEquals(newValue))
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
            if (property.NormalEquals(newValue))
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
