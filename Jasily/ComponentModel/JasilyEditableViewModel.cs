using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Jasily.ComponentModel
{
    public abstract class JasilyEditableViewModel<T> : JasilyViewModel<T>
    {
        private List<Action<object, object>> writeToActions;
        private List<Action<object, object>> readFromActions;
        private Dictionary<string, Tuple<Getter<object, object>, Setter<object, object>>> currentTypeMapped;
        private Dictionary<string, Tuple<Getter<object, object>, Setter<object, object>>> sourceTypeMapped;

        protected Type CurrentType { get; }

        protected Type SourceType { get; }

        public JasilyEditableViewModel(T source)
            : base(source)
        {
            this.CurrentType = this.GetType();
            this.SourceType = typeof(T);
        }

        private void MappingType()
        {
            if (this.currentTypeMapped == null)
            {
                var mapped = new Dictionary<string, Tuple<Getter<object, object>, Setter<object, object>>>();
                foreach (var field in this.CurrentType.GetRuntimeFields()
                    .Where(JasilyCustomAttributeExtensions.HasCustomAttribute<EditableFieldAttribute>))
                {
                    mapped.Add(field.Name, Tuple.Create(field.CompileGetter(), field.CompileSetter()));
                }
                foreach (var property in this.CurrentType.GetRuntimeProperties()
                    .Where(JasilyCustomAttributeExtensions.HasCustomAttribute<EditableFieldAttribute>))
                {
                    mapped.Add(property.Name, Tuple.Create(property.CompileGetter(), property.CompileSetter()));
                }

                this.currentTypeMapped = mapped;
            }

            if (this.sourceTypeMapped == null)
            {
                Tuple<Getter<object, object>, Setter<object, object>> tmp;
                var mapped = new Dictionary<string, Tuple<Getter<object, object>, Setter<object, object>>>();
                foreach (var field in this.SourceType.GetRuntimeFields())
                {
                    if (this.currentTypeMapped.TryGetValue(field.Name, out tmp))
                    {
                        mapped.Add(field.Name, Tuple.Create(field.CompileGetter(), field.CompileSetter()));
                    }
                }
                foreach (var property in this.SourceType.GetRuntimeProperties())
                {
                    if (this.currentTypeMapped.TryGetValue(property.Name, out tmp))
                    {
                        mapped.Add(property.Name, Tuple.Create(property.CompileGetter(), property.CompileSetter()));
                    }
                }

                this.sourceTypeMapped = mapped;
            }
        }

        private void MappingWrite()
        {
            if (this.writeToActions == null)
            {
                this.MappingType();
                var mapping = new List<Action<object, object>>();
                foreach (var kvp in this.currentTypeMapped)
                {
                    var getter = kvp.Value.Item1;
                    var setter = this.sourceTypeMapped[kvp.Key].Item2;
                    mapping.Add((source, dest) => setter.Set(dest, getter.Get(source)));
                }
                this.writeToActions = mapping;
            }
        }

        private void MappingRead()
        {
            if (this.readFromActions == null)
            {
                this.MappingType();
                var mapping = new List<Action<object, object>>();
                foreach (var kvp in this.currentTypeMapped)
                {
                    var getter = this.sourceTypeMapped[kvp.Key].Item1;
                    var setter = kvp.Value.Item2;
                    mapping.Add((source, dest) => setter.Set(dest, getter.Get(source)));
                }
                this.readFromActions = mapping;
            }
        }

        public virtual void WriteToObject(T obj)
        {
            if (ReferenceEquals(obj, null)) return;

            this.MappingWrite();

            foreach (var m in this.writeToActions)
            {
                m(this, obj);
            }
        }

        public virtual void ReadFromObject(T obj)
        {
            if (ReferenceEquals(obj, null)) return;

            this.MappingRead();

            foreach (var m in this.readFromActions)
            {
                m(obj, this);
            }
        }
    }
}