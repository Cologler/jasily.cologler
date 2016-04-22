﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Jasily.ComponentModel
{
    internal static class JasilyEditableViewModel
    {
        public class Writer
        {
            public Writer(string name, EditableFieldAttribute attribute)
            {
                Debug.Assert(name != null);
                Debug.Assert(attribute != null);
                this.Name = name;
                this.Attribute = attribute;
            }

            public string Name { get; }

            public EditableFieldAttribute Attribute { get; }

            public Getter<object, object> ViewModelGetter { get; set; }

            public Setter<object, object> ViewModelSetter { get; set; }

            public Getter<object, object> ObjectGetter { get; set; }

            public Setter<object, object> ObjectSetter { get; set; }

            [Conditional("DEBUG")]
            public void Verify()
            {
                Debug.Assert(this.ViewModelGetter != null);
                Debug.Assert(this.ViewModelSetter != null);
                Debug.Assert(this.ObjectGetter != null);
                Debug.Assert(this.ObjectSetter != null);
            }

            public void WriteToObject(object vm, object obj) => this.ObjectSetter.Set(obj, this.ViewModelGetter.Get(vm));

            public void ReadFromObject(object obj, object vm) => this.ViewModelSetter.Set(obj, this.ObjectGetter.Get(vm));
        }

        public class Cache
        {
            private Dictionary<string, Writer> writers = new Dictionary<string, Writer>();

            protected Type ViewModelType { get; }

            protected Type ObjectType { get; }

            public Cache(Type viewModelType, Type objectType)
            {
                this.ViewModelType = viewModelType;
                this.ObjectType = objectType;

                this.MappingType();
            }

            private void MappingType()
            {
                if (this.writers == null)
                {
                    var mapped = new Dictionary<string, Writer>();

                    // view model
                    foreach (var field in this.ViewModelType.GetRuntimeFields())
                    {
                        var attr = field.GetCustomAttribute<EditableFieldAttribute>();
                        if (attr != null)
                        {
                            var writer = new Writer(field.Name, attr);
                            writer.ViewModelGetter = field.CompileGetter();
                            writer.ViewModelSetter = field.CompileSetter();
                            mapped.Add(field.Name, writer);
                        }
                    }
                    foreach (var property in this.ViewModelType.GetRuntimeProperties())
                    {
                        var attr = property.GetCustomAttribute<EditableFieldAttribute>();
                        if (attr != null)
                        {
                            var writer = new Writer(property.Name, attr);
                            writer.ViewModelGetter = property.CompileGetter();
                            writer.ViewModelSetter = property.CompileSetter();
                            mapped.Add(property.Name, writer);
                        }
                    }

                    // object
                    foreach (var field in this.ObjectType.GetRuntimeFields())
                    {
                        Writer writer;
                        if (mapped.TryGetValue(field.Name, out writer))
                        {
                            writer.ObjectGetter = field.CompileGetter();
                            writer.ObjectSetter = field.CompileSetter();
                        }
                    }
                    foreach (var property in this.ObjectType.GetRuntimeProperties())
                    {
                        Writer writer;
                        if (mapped.TryGetValue(property.Name, out writer))
                        {
                            writer.ObjectGetter = property.CompileGetter();
                            writer.ObjectSetter = property.CompileSetter();
                        }
                    }

#if DEBUG
                    mapped.Values.ForEach(z => z.Verify());
#endif

                    this.writers = mapped;
                }
            }

            public void WriteToObject(object vm, object obj)
            {
                Debug.Assert(vm != null);
                Debug.Assert(obj != null);
                Debug.Assert(vm.GetType() == this.ViewModelType);
                Debug.Assert(obj.GetType() == this.ObjectType);

                foreach (var writer in this.writers.Values)
                {
                    writer.WriteToObject(vm, obj);
                }
            }

            public void ReadFromObject(object obj, object vm)
            {
                Debug.Assert(vm != null);
                Debug.Assert(obj != null);
                Debug.Assert(vm.GetType() == this.ViewModelType);
                Debug.Assert(obj.GetType() == this.ObjectType);

                foreach (var writer in this.writers.Values)
                {
                    writer.ReadFromObject(vm, obj);
                }
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