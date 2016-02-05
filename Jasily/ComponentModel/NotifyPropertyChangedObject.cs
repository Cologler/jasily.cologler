using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Jasily.ComponentModel
{
    public class NotifyPropertyChangedObject : INotifyPropertyChanged
    {
        readonly object syncRootForEndRefresh = new object();
        readonly List<string> registeredPropertyForEndRefresh = new List<string>();

        /// <summary>
        /// please always call on background thread.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertySelector"></param>
        public void RegisterForEndRefresh<T>(Expression<Func<T, object>> propertySelector)
        {
            var propertyName = PropertySelector<T>.Start(propertySelector);
            lock (this.syncRootForEndRefresh)
                this.registeredPropertyForEndRefresh.Add(propertyName);
        }

        /// <summary>
        /// please always call this action on UI thread. the method will call PropertyChanged for each Registered property.
        /// </summary>
        public void EndRefresh()
        {
            if (this.registeredPropertyForEndRefresh.Count > 0)
            {
                lock (this.syncRootForEndRefresh)
                {
                    if (this.registeredPropertyForEndRefresh.Count > 0)
                    {
                        this.NotifyPropertyChanged(this.registeredPropertyForEndRefresh);
                        this.registeredPropertyForEndRefresh.Clear();
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
        protected bool SetPropertyRef<T>(ref T property, T newValue, params string[] propertyNames)
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

        /// <summary>
        /// the method will call PropertyChanged for each property which has [NotifyPropertyChanged]
        /// </summary>
        public virtual void RefreshProperties()
        {
            var properties = (
                from property in this.GetType().GetRuntimeProperties()
                let attr = property.GetCustomAttribute<NotifyPropertyChangedAttribute>()
                where attr != null
                orderby attr.Order
                select property.Name
            ).ToArray();

            this.NotifyPropertyChanged(properties);

            this.PropertiesRefreshed?.Invoke(this);
        }

        protected void NotifyPropertyChanged<T>(Expression<Func<T, object>> propertySelector)
        {
            var propertyName = PropertySelector<T>.Start(propertySelector);
            this.PropertyChanged.Fire(this, propertyName);
        }
        protected void NotifyPropertyChanged(string propertyName) => this.PropertyChanged.Fire(this, propertyName);
        protected void NotifyPropertyChanged(params string[] propertyNames) => this.PropertyChanged.Fire(this, propertyNames);
        protected void NotifyPropertyChanged(IEnumerable<string> propertyNames) => this.PropertyChanged.Fire(this, propertyNames);

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler PropertiesRefreshed;

        public void ClearPropertyChangedInvocationList()
        {
            this.PropertyChanged = null;
        }
    }
}
