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
            this._registeredPropertyForEndRefresh = new List<string>();
        }

        private string ParseProperty<T>(Expression<Func<T, object>> propertySelector)
        {
            if (typeof(T).FullName != this.GetType().FullName)
                throw new NotSupportedException("type of source in propertySelector must be current type.");

            return propertySelector.PropertySelector();
        }

        /// <summary>
        /// please always call on background thread.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertySelector"></param>
        public void RegisterForEndRefresh<T>(Expression<Func<T, object>> propertySelector)
        {
            lock (this._syncRootForEndRefresh)
                this._registeredPropertyForEndRefresh.Add(this.ParseProperty(propertySelector));
        }

        /// <summary>
        /// please always call on UI thread. the method will call PropertyChanged for each Registered property.
        /// </summary>
        public void EndRefresh()
        {
            if (this._registeredPropertyForEndRefresh.Count > 0)
            {
                lock (this._syncRootForEndRefresh)
                {
                    if (this._registeredPropertyForEndRefresh.Count > 0)
                    {
                        this.NotifyPropertyChanged(this._registeredPropertyForEndRefresh);
                        this._registeredPropertyForEndRefresh.Clear();
                    }
                }
            }
        }

        protected bool SetPropertyRef<T>(ref T property, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (property.NormalEquals(newValue))
            {
                return false;
            }
            else
            {
                property = newValue;
                this.NotifyPropertyChanged(propertyName);
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
                this.NotifyPropertyChanged(propertyNames);
                return true;
            }
        }

        protected virtual void NotifyPropertyChanged<T>(Expression<Func<T, object>> propertySelector)
        {
            this.NotifyPropertyChanged(this.ParseProperty(propertySelector));
        }
        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged.Fire(this, propertyName);
        }
        protected virtual void NotifyPropertyChanged(params string[] propertyNames)
        {
            this.PropertyChanged.Fire(this, propertyNames);
        }
        protected virtual void NotifyPropertyChanged(IEnumerable<string> propertyNames)
        {
            this.PropertyChanged.Fire(this, propertyNames);
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
