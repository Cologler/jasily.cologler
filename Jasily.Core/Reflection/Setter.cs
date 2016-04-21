using JetBrains.Annotations;

namespace System.Reflection
{
    public class Setter<TObject, TMember> : ISetter
    {
        private readonly Action<TObject, TMember> setter;

        public Setter([NotNull] Action<TObject, TMember> setter)
        {
            if (setter == null) throw new ArgumentNullException(nameof(setter));
            this.setter = setter;
        }

        public void Set(TObject obj, TMember value) => this.setter(obj, value);

        public TMember this[TObject obj]
        {
            set { this.Set(obj, value); }
        }

        #region Implementation of ISetter

        public void Set(object instance, object value) => this.setter((TObject)instance, (TMember)value);

        public object this[object obj]
        {
            set { this.Set(obj, value); }
        }

        #endregion

        public static implicit operator Action<TObject, TMember>(Setter<TObject, TMember> self) => self?.setter;
    }
}