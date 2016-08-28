using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Jasily.ComponentModel
{
    public class NotifyPropertyChangedObject : INotifyPropertyChanged
    {
        private readonly object syncRootForEndRefresh = new object();
        private readonly List<string> registeredPropertyForEndRefresh = new List<string>();

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

        [NotifyPropertyChangedInvocator]
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
        [NotifyPropertyChangedInvocator]
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
            var properties = this.PropertiesMapper?.GetProperties(this) ??
                MapNotifyPropertyChangedAttribute(this.GetType());

            this.NotifyPropertyChanged(properties);

            this.PropertiesRefreshed?.Invoke(this);
        }

        public RefreshPropertiesMapper PropertiesMapper { get; set; }

        [NotifyPropertyChangedInvocator]
        protected void NotifyPropertyChanged<T>([NotNull] Expression<Func<T, object>> propertySelector)
        {
            if (propertySelector == null) throw new ArgumentNullException(nameof(propertySelector));
            var propertyName = PropertySelector<T>.Start(propertySelector);
            this.PropertyChanged.Fire(this, propertyName);
        }

        [NotifyPropertyChangedInvocator]
        protected void NotifyPropertyChanged([NotNull] string propertyName)
        {
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));
            this.PropertyChanged.Fire(this, propertyName);
        }

        [NotifyPropertyChangedInvocator]
        protected void NotifyPropertyChanged([NotNull] params string[] propertyNames)
        {
            if (propertyNames == null) throw new ArgumentNullException(nameof(propertyNames));
            this.PropertyChanged.Fire(this, propertyNames);
        }

        [NotifyPropertyChangedInvocator]
        protected void NotifyPropertyChanged([NotNull] IEnumerable<string> propertyNames)
        {
            if (propertyNames == null) throw new ArgumentNullException(nameof(propertyNames));
            this.PropertyChanged.Fire(this, propertyNames);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler PropertiesRefreshed;

        public void ClearPropertyChangedInvocationList()
        {
            this.PropertyChanged = null;
        }

        public sealed class RefreshPropertiesMapper
        {
            private readonly Type type;
            private readonly string[] properties;

            public RefreshPropertiesMapper([NotNull] Type type)
            {
                if (type == null) throw new ArgumentNullException(nameof(type));
                this.type = type;
                this.properties = MapNotifyPropertyChangedAttribute(type);
            }

            internal string[] GetProperties(NotifyPropertyChangedObject obj)
            {
                if (obj.GetType() != this.type) throw new InvalidOperationException();

                return this.properties;
            }
        }

        private static string[] MapNotifyPropertyChangedAttribute(Type type)
        {
            return (
                from property in type.GetRuntimeProperties()
                let attr = property.GetCustomAttribute<NotifyPropertyChangedAttribute>()
                where attr != null
                orderby attr.AsOrderable().GetOrderCode()
                select property.Name
            ).ToArray();
        }
    }
}
