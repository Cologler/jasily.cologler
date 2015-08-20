using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.ComponentModel
{
    public class NotifyPropertyChangedObject : INotifyPropertyChanged
    {
        readonly object syncRootForEndRefresh = new object();
        readonly List<string> registeredPropertyForEndRefresh = new List<string>();

        private string ParseProperty<T>(Expression<Func<T, object>> propertySelector)
        {
            if (this.GetType() != typeof(T) &&
                !this.GetType().GetTypeInfo().IsSubclassOf(typeof(T)))
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
            var property = this.ParseProperty(propertySelector);
            lock (this.syncRootForEndRefresh)
                this.registeredPropertyForEndRefresh.Add(property);
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
            var list = (
                from property in this.GetType().GetRuntimeProperties()
                let attr = property.GetCustomAttribute<NotifyPropertyChangedAttribute>()
                where attr != null
                select new Tuple<NotifyPropertyChangedAttribute, PropertyInfo>(attr, property)
            ).ToList();

            this.NotifyPropertyChanged(list
                .OrderBy(z => z.Item1.Order)
                .Select(z => z.Item2.Name)
                .ToArray());
        }

        protected void NotifyPropertyChanged<T>(Expression<Func<T, object>> propertySelector)
        {
            var propertyName = this.ParseProperty(propertySelector);
            this.PropertyChanged.Fire(this, propertyName);
        }
        protected void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged.Fire(this, propertyName);
        }
        protected void NotifyPropertyChanged(params string[] propertyNames)
        {
            this.PropertyChanged.Fire(this, propertyNames);
        }
        protected void NotifyPropertyChanged(IEnumerable<string> propertyNames)
        {
            this.PropertyChanged.Fire(this, propertyNames);
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        public void ClearPropertyChangedInvocationList()
        {
            this.PropertyChanged = null;
        }
    }
}
