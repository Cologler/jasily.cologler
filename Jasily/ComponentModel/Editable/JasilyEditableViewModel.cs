using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace Jasily.ComponentModel.Editable
{
    internal static class JasilyEditableViewModel
    {
        internal interface IEditableViewModel
        {
            void WriteToObject(object obj);

            void ReadFromObject(object obj);
        }

        private abstract class Executor : IGetKey<string>
        {
            protected Executor(string name, EditableFieldAttribute attribute)
            {
                Debug.Assert(name != null);
                Debug.Assert(attribute != null);
                this.Name = string.IsNullOrWhiteSpace(attribute.Name) ? name : attribute.Name;
                this.Attribute = attribute;
            }

            public string Name { get; }

            public EditableFieldAttribute Attribute { get; }

            public Getter<object, object> ViewModelGetter { get; set; }

            public virtual void Verify()
            {
                Debug.Assert(this.ViewModelGetter != null);
            }

            [Pure]
            public abstract void WriteToObject(object vm, object obj);

            [Pure]
            public abstract void ReadFromObject(object obj, object vm);

            #region Implementation of IGetKey<out string>

            public string GetKey() => this.Name;

            #endregion
        }

        private class SubViewModelCaller : Executor
        {
            [Pure]
            public override void WriteToObject(object vm, object obj)
            {
                Debug.Assert(vm != null && obj != null);
                var value = this.ViewModelGetter.Get(vm);
                if (value == null) return;
                Debug.Assert(value is IEditableViewModel);
                ((IEditableViewModel)value).WriteToObject(obj);
            }

            [Pure]
            public override void ReadFromObject(object obj, object vm)
            {
                Debug.Assert(vm != null && obj != null);
                var value = this.ViewModelGetter.Get(vm);
                if (value == null) return;
                Debug.Assert(value is IEditableViewModel);
                ((IEditableViewModel)value).ReadFromObject(obj);
            }

            public SubViewModelCaller(string name, EditableFieldAttribute attribute)
                : base(name, attribute)
            {
            }
        }

        private class FieldWriter : Executor
        {
            public FieldWriter(string name, EditableFieldAttribute attribute)
                : base(name, attribute)
            {
            }

            public Setter<object, object> ViewModelSetter { get; set; }

            public Getter<object, object> ObjectGetter { get; set; }

            public Setter<object, object> ObjectSetter { get; set; }

            public List<WriteToObjectConditionAttribute> WriteConditions { get; }
                = new List<WriteToObjectConditionAttribute>();

            public bool IsPropertyContainer { get; set; }

            public override void Verify()
            {
                base.Verify();

                if (this.ObjectGetter == null || this.ObjectSetter == null)
                    throw new InvalidOperationException($"can not find property or field call [{this.Name}] on object.");

                if (this.IsPropertyContainer)
                    Debug.Assert(this.ViewModelSetter == null);
                else
                    Debug.Assert(this.ViewModelSetter != null);

                if (this.Attribute.Converter != null)
                {
                    if (!typeof(IConverter).GetTypeInfo().IsAssignableFrom(this.Attribute.Converter.GetTypeInfo()))
                        throw new InvalidCastException($"can not cast {this.Attribute.Converter} to {typeof(IConverter)}");

                    if (this.Attribute.Converter.GetTypeInfo()
                        .DeclaredConstructors
                        .FirstOrDefault(z => z.GetParameters().Length == 0) == null)
                        throw new InvalidOperationException($"{this.Attribute.Converter} has no none args constructor.");
                }
            }

            [Pure]
            public override void WriteToObject(object vm, object obj)
            {
                Debug.Assert(vm != null && obj != null);

                var value = this.ViewModelGetter.Get(vm);

                // unwrap IPropertyContainer
                if (this.IsPropertyContainer)
                {
                    value = (value as IPropertyContainer)?.Value;
                }

                // convert
                if (this.Attribute.Converter != null)
                {
                    var converter = Activator.CreateInstance(this.Attribute.Converter) as IConverter;
                    Debug.Assert(converter != null);
                    if (!converter.CanConvertBack(value)) return;
                    value = converter.ConvertBack(value);
                }

                // check for write
                if (this.WriteConditions.Count > 0)
                {
                    if (this.WriteConditions.Any(z => !z.IsMatch(value))) return;
                }

                // set
                this.ObjectSetter.Set(obj, value);
            }

            [Pure]
            public override void ReadFromObject(object obj, object vm)
            {
                Debug.Assert(vm != null && obj != null);

                var value = this.ObjectGetter.Get(obj);

                // convert
                if (this.Attribute.Converter != null)
                {
                    var converter = Activator.CreateInstance(this.Attribute.Converter) as IConverter;
                    Debug.Assert(converter != null);
                    if (!converter.CanConvert(value)) return;
                    value = converter.Convert(value);
                }

                if (this.IsPropertyContainer)
                {
                    // wrap IPropertyContainer
                    var container = this.ViewModelGetter.Get(vm) as IPropertyContainer;
                    if (container != null) container.Value = value;
                }
                else
                {
                    this.ViewModelSetter.Set(vm, value);
                }
            }
        }

        public class Cache
        {
            private Dictionary<string, Executor> executors;

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
                if (this.executors == null)
                {
                    var mapped = new Dictionary<string, Executor>();

                    // view model
                    foreach (var field in this.ViewModelType.GetRuntimeFields())
                    {
                        var attr = field.GetCustomAttribute<EditableFieldAttribute>();
                        if (attr != null)
                        {
                            if (attr.IsSubEditableViewModel)
                            {
                                if (!typeof(IEditableViewModel).GetTypeInfo().IsAssignableFrom(field.FieldType.GetTypeInfo()))
                                    throw new InvalidCastException();
                                var executor = new SubViewModelCaller(field.Name, attr)
                                {
                                    ViewModelGetter = field.CompileGetter()
                                };
                                mapped.Add(executor);
                            }
                            else
                            {
                                var executor = new FieldWriter(field.Name, attr)
                                {
                                    ViewModelGetter = field.CompileGetter()
                                };
                                executor.WriteConditions.AddRange(field.GetCustomAttributes<WriteToObjectConditionAttribute>());
                                executor.IsPropertyContainer = typeof(IPropertyContainer)
                                    .GetTypeInfo()
                                    .IsAssignableFrom(field.FieldType.GetTypeInfo());
                                if (!executor.IsPropertyContainer) executor.ViewModelSetter = field.CompileSetter();
                                mapped.Add(executor);
                            }
                        }
                    }
                    foreach (var property in this.ViewModelType.GetRuntimeProperties())
                    {
                        var attr = property.GetCustomAttribute<EditableFieldAttribute>();
                        if (attr != null)
                        {
                            if (attr.IsSubEditableViewModel)
                            {
                                if (!typeof(IEditableViewModel).GetTypeInfo().IsAssignableFrom(property.PropertyType.GetTypeInfo()))
                                    throw new InvalidCastException();
                                var executor = new SubViewModelCaller(property.Name, attr)
                                {
                                    ViewModelGetter = property.CompileGetter()
                                };
                                mapped.Add(executor);
                            }
                            else
                            {
                                var executor = new FieldWriter(property.Name, attr)
                                {
                                    ViewModelGetter = property.CompileGetter()
                                };
                                executor.WriteConditions.AddRange(property.GetCustomAttributes<WriteToObjectConditionAttribute>());
                                executor.IsPropertyContainer = typeof(IPropertyContainer)
                                    .GetTypeInfo()
                                    .IsAssignableFrom(property.PropertyType.GetTypeInfo());
                                if (!executor.IsPropertyContainer) executor.ViewModelSetter = property.CompileSetter();
                                mapped.Add(executor);
                            }
                        }
                    }

                    // object
                    foreach (var field in this.ObjectType.GetRuntimeFields())
                    {
                        Executor executor;
                        if (mapped.TryGetValue(field.Name, out executor))
                        {
                            var writer = executor as FieldWriter;
                            if (writer != null)
                            {
                                writer.ObjectGetter = field.CompileGetter();
                                writer.ObjectSetter = field.CompileSetter();
                            }
                        }
                    }
                    foreach (var property in this.ObjectType.GetRuntimeProperties())
                    {
                        Executor executor;
                        if (mapped.TryGetValue(property.Name, out executor))
                        {
                            var writer = executor as FieldWriter;
                            if (writer != null)
                            {
                                writer.ObjectGetter = property.CompileGetter();
                                writer.ObjectSetter = property.CompileSetter();
                            }
                        }
                    }

                    mapped.Values.ForEach(z => z.Verify());
                    this.executors = mapped;
                }
            }

            [Pure]
            public void WriteToObject(object vm, object obj)
            {
                Debug.Assert(vm != null);
                Debug.Assert(obj != null);
                Debug.Assert(vm.GetType() == this.ViewModelType);
                Debug.Assert(obj.GetType() == this.ObjectType);

                foreach (var executor in this.executors.Values)
                {
                    executor.WriteToObject(vm, obj);
                }
            }

            [Pure]
            public void ReadFromObject(object obj, object vm)
            {
                Debug.Assert(vm != null);
                Debug.Assert(obj != null);
                Debug.Assert(vm.GetType() == this.ViewModelType);
                Debug.Assert(obj.GetType() == this.ObjectType);

                foreach (var executor in this.executors.Values)
                {
                    executor.ReadFromObject(obj, vm);
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

    public abstract class JasilyEditableViewModel<T> : JasilyViewModel,
        JasilyEditableViewModel.IEditableViewModel
    {
        private JasilyEditableViewModel.Cache mapperCached;

        [NotifyPropertyChanged(Order = -1)]
        public T ReadCached { get; private set; }

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
            this.ReadCached = obj;
            this.mapperCached.ReadFromObject(obj, this);
        }

        #region Implementation of IEditableViewModel

        void JasilyEditableViewModel.IEditableViewModel.WriteToObject(object obj) => this.WriteToObject((T)obj);

        void JasilyEditableViewModel.IEditableViewModel.ReadFromObject(object obj) => this.ReadFromObject((T)obj);

        #endregion
    }
}