using System.Linq;
using System.Reflection;

namespace System.ComponentModel.Editable
{
    public abstract class JasilyEditableViewModel<T> : JasilyViewModel<T>
    {
        protected Type ThisType { get; private set; }

        protected Type SourceType { get; private set; }

        public JasilyEditableViewModel(T source)
            : base(source)
        {
            this.ThisType = this.GetType();
            this.SourceType = typeof(T);
        }

        public virtual void WriteToObject(T obj)
        {
            if (ReferenceEquals(obj, null)) return;

            foreach (var field in this.SourceType.GetRuntimeFields()
                .Where(f => f.GetCustomAttribute<EditableFieldAttribute>() != null))
            {
                var getter = this.ThisType.GetGetter(field.Name);
                if (getter == null) continue;
                var setter = this.SourceType.GetSetter(field.Name);
                if (setter == null) continue;
                setter(obj, getter(this));
            }

            foreach (var property in this.SourceType.GetRuntimeProperties()
                .Where(f => f.GetCustomAttribute<EditableFieldAttribute>() != null))
            {
                var getter = this.ThisType.GetGetter(property.Name);
                if (getter == null) continue;
                var setter = this.SourceType.GetSetter(property.Name);
                if (setter == null) continue;
                setter(obj, getter(this));
            }
        }

        public virtual void ReadFromObject(T obj)
        {
            if (ReferenceEquals(obj, null)) return;

            foreach (var field in this.SourceType.GetRuntimeFields()
                .Where(f => f.GetCustomAttribute<EditableFieldAttribute>() != null))
            {
                var getter = this.SourceType.GetGetter(field.Name);
                if (getter == null) continue;
                var setter = this.ThisType.GetSetter(field.Name);
                if (setter == null) continue;
                setter(this, getter(obj));
            }

            foreach (var property in this.SourceType.GetRuntimeProperties()
                .Where(f => f.GetCustomAttribute<EditableFieldAttribute>() != null))
            {
                var getter = this.SourceType.GetGetter(property.Name);
                if (getter == null) continue;
                var setter = this.ThisType.GetSetter(property.Name);
                if (setter == null) continue;
                setter(this, getter(obj));
            }
        }
    }
}