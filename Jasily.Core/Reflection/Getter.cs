using JetBrains.Annotations;

namespace System.Reflection
{
    public class Getter<TObject, TMember> : IGetter
    {
        private readonly Func<TObject, TMember> getter;

        public Getter([NotNull] Func<TObject, TMember> getter)
        {
            if (getter == null) throw new ArgumentNullException(nameof(getter));
            this.getter = getter;
        }

        public TMember Get(TObject obj) => this.getter(obj);

        public TMember this[TObject obj] => this.Get(obj);

        #region Implementation of IGetter

        public object Get(object instance) => this.getter((TObject)instance);

        public object this[object instance] => this.Get(instance);

        #endregion

        public static implicit operator Func<TObject, TMember>(Getter<TObject, TMember> self) => self?.getter;
    }
}