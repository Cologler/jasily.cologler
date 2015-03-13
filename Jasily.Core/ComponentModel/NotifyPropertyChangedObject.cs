using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace System.ComponentModel
{
    public class NotifyPropertyChangedObject : INotifyPropertyChanged
    {
        object SyncRootForEndRefresh = new object();
        List<string> RegisteredPropertyForEndRefresh;

        public NotifyPropertyChangedObject()
        {
            RegisteredPropertyForEndRefresh = new List<string>();
        }

        /// <summary>
        /// please always use for background thread.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertySelector"></param>
        public void RegisterForEndRefresh<T>(Expression<Func<T, object>> propertySelector)
        {
            if (typeof(T).FullName != this.GetType().FullName)
                throw new NotSupportedException("type of source in propertySelector must be current type.");

            lock (SyncRootForEndRefresh)
                RegisteredPropertyForEndRefresh.Add(propertySelector.ParsePathFromPropertySelector());
        }

        /// <summary>
        /// run on UI thread
        /// </summary>
        public void EndRefresh()
        {
            if (this.RegisteredPropertyForEndRefresh.Count > 0)
            {
                lock (SyncRootForEndRefresh)
                {
                    if (this.RegisteredPropertyForEndRefresh.Count > 0)
                    {
                        this.NotifyPropertyChanged(this.RegisteredPropertyForEndRefresh);
                        this.RegisteredPropertyForEndRefresh.Clear();
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
