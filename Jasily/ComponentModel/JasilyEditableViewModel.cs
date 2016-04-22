using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Jasily.ComponentModel
{
    internal static class JasilyEditableViewModel
    {
        public class Cache
        {
            private List<Action<object, object>> writeToActions;
            private List<Action<object, object>> readFromActions;
            private Dictionary<string, Tuple<Getter<object, object>, Setter<object, object>>> currentTypeMapped;
            private Dictionary<string, Tuple<Getter<object, object>, Setter<object, object>>> sourceTypeMapped;

            protected Type ViewModelType { get; }

            protected Type ObjectType { get; }

            public Cache(Type viewModelType, Type objectType)
            {
                this.ViewModelType = viewModelType;
                this.ObjectType = objectType;

                this.MappingWrite();
                this.MappingRead();
            }

            private void MappingType()
            {
                // mapper type
                if (this.currentTypeMapped == null)
                {
                    var mapped = new Dictionary<string, Tuple<Getter<object, object>, Setter<object, object>>>();
                    foreach (var field in this.ViewModelType.GetRuntimeFields()
                        .Where(JasilyCustomAttributeExtensions.HasCustomAttribute<EditableFieldAttribute>))
                    {
                        mapped.Add(field.Name, Tuple.Create(field.CompileGetter(), field.CompileSetter()));
                    }
                    foreach (var property in this.ViewModelType.GetRuntimeProperties()
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
                    foreach (var field in this.ObjectType.GetRuntimeFields())
                    {
                        if (this.currentTypeMapped.TryGetValue(field.Name, out tmp))
                        {
                            mapped.Add(field.Name, Tuple.Create(field.CompileGetter(), field.CompileSetter()));
                        }
                    }
                    foreach (var property in this.ObjectType.GetRuntimeProperties())
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

            public void WriteToObject(object vm, object obj)
            {
                Debug.Assert(vm != null);
                Debug.Assert(obj != null);
                Debug.Assert(vm.GetType() == this.ViewModelType);
                Debug.Assert(obj.GetType() == this.ObjectType);

                foreach (var writer in this.writeToActions) writer(vm, obj);
            }

            public void ReadFromObject(object obj, object vm)
            {
                Debug.Assert(vm != null);
                Debug.Assert(obj != null);
                Debug.Assert(vm.GetType() == this.ViewModelType);
                Debug.Assert(obj.GetType() == this.ObjectType);

                foreach (var reader in this.readFromActions) reader(obj, vm);
            }
        }

        public class Cache<T>
        {
            // ReSharper disable once StaticMemberInGenericType
            private static readonly Dictionary<Type, Cache> Cached = new Dictionary<Type, Cache>();

            public static Cache GetMapperCache(Type viewModelType)
            {
                lock (Cached)
                {
                    var ret = Cached.GetValueOrDefault(viewModelType);
                    if (ret != null) return ret;
                }

                var @new = new Cache(viewModelType, typeof(T));

                lock (Cached)
                {
                    return Cached.GetOrSetValue(viewModelType, @new);
                }
            }
        }
    }

    public abstract class JasilyEditableViewModel<T> : JasilyViewModel<T>
    {
        private JasilyEditableViewModel.Cache mapperCached;

        protected JasilyEditableViewModel(T source)
            : base(source)
        {
        }

        public virtual void WriteToObject(T obj)
        {
            if (ReferenceEquals(obj, null)) return;
            if (this.mapperCached == null)
                this.mapperCached = JasilyEditableViewModel.Cache<T>.GetMapperCache(this.GetType());
            this.mapperCached.WriteToObject(this, obj);
        }

        public virtual void ReadFromObject(T obj)
        {
            if (ReferenceEquals(obj, null)) return;
            if (this.mapperCached == null)
                this.mapperCached = JasilyEditableViewModel.Cache<T>.GetMapperCache(this.GetType());
            this.mapperCached.ReadFromObject(obj, this);
        }
    }
}