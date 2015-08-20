using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Jasily.ComponentModel
{
    public abstract class JasilyEditableViewModel<T> : JasilyViewModel<T>
    {
        private List<Action<object, object>> WriteToObjectMapped;
        private List<Action<object, object>> ReadFromObjectMapped;
        private Dictionary<string, Tuple<Func<object, object>, Action<object, object>>> ThisMapped;
        private Dictionary<string, Tuple<Func<object, object>, Action<object, object>>> SourceMapped;

        protected Type ThisType { get; private set; }

        protected Type SourceType { get; private set; }

        public JasilyEditableViewModel(T source)
            : base(source)
        {
            this.ThisType = this.GetType();
            this.SourceType = typeof(T);
        }

        private void MappingThis()
        {
            if (this.ThisMapped == null)
            {
                var mapped = new Dictionary<string, Tuple<Func<object, object>, Action<object, object>>>();
                foreach (var field in this.GetType().GetRuntimeFields().Where(JasilyCustomAttributeExtensions.HasCustomAttribute<EditableFieldAttribute>))
                {
                    mapped.Add(field.Name, new Tuple<Func<object, object>, Action<object, object>>(field.GetValue, field.SetValue));
                }
                foreach (var property in this.GetType().GetRuntimeProperties().Where(JasilyCustomAttributeExtensions.HasCustomAttribute<EditableFieldAttribute>))
                {
                    mapped.Add(property.Name, new Tuple<Func<object, object>, Action<object, object>>(property.GetValue, property.SetValue));
                }

                this.ThisMapped = mapped;
            }
        }

        private void MappingSource()
        {
            Debug.Assert(this.ThisMapped != null);

            if (this.SourceMapped == null)
            {
                Tuple<Func<object, object>, Action<object, object>> tmp;
                var mapped = new Dictionary<string, Tuple<Func<object, object>, Action<object, object>>>();
                foreach (var field in typeof(T).GetRuntimeFields())
                {
                    if (this.ThisMapped.TryGetValue(field.Name, out tmp))
                    {
                        mapped.Add(field.Name, new Tuple<Func<object, object>, Action<object, object>>(field.GetValue, field.SetValue));
                    }
                }
                foreach (var property in typeof(T).GetRuntimeProperties())
                {
                    if (this.ThisMapped.TryGetValue(property.Name, out tmp))
                    {
                        mapped.Add(property.Name, new Tuple<Func<object, object>, Action<object, object>>(property.GetValue, property.SetValue));
                    }
                }

                this.SourceMapped = mapped;
            }
        }

        private void MappingWrite()
        {
            if (this.WriteToObjectMapped == null)
            {
                this.MappingThis();
                this.MappingSource();
                var mapping = new List<Action<object, object>>();
                foreach (var kvp in this.ThisMapped)
                {
                    var getter = kvp.Value.Item1;
                    var setter = this.SourceMapped[kvp.Key].Item2;
                    mapping.Add((source, dest) => setter(dest, getter(source)));
                }
                this.WriteToObjectMapped = mapping;
            }
        }

        private void MappingRead()
        {
            if (this.ReadFromObjectMapped == null)
            {
                this.MappingThis();
                this.MappingSource();
                var mapping = new List<Action<object, object>>();
                foreach (var kvp in this.ThisMapped)
                {
                    var getter = this.SourceMapped[kvp.Key].Item1;
                    var setter = kvp.Value.Item2;
                    mapping.Add((source, dest) => setter(dest, getter(source)));
                }
                this.ReadFromObjectMapped = mapping;
            }
        }

        public virtual void WriteToObject(T obj)
        {
            if (ReferenceEquals(obj, null)) return;

            this.MappingWrite();

            foreach (var m in this.WriteToObjectMapped)
            {
                m(this, obj);
            }
        }

        public virtual void ReadFromObject(T obj)
        {
            if (ReferenceEquals(obj, null)) return;

            this.MappingRead();

            foreach (var m in this.ReadFromObjectMapped)
            {
                m(obj, this);
            }
        }
    }
}