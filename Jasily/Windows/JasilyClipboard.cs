using System;
using System.Diagnostics;

namespace Jasily.Windows
{
    public class JasilyClipboard<T>
    {
        public static JasilyClipboard<T> Default { get; } = new JasilyClipboard<T>();

        private object SyncRoot = new object();
        private bool HasValue;
        private T CurrentItem;
        private ClipMode Mode;

        public event EventHandler ContentChanged;

        private void SetValue(ClipMode mode, T item)
        {
            lock (this.SyncRoot)
            {
                this.HasValue = true;
                this.Mode = mode;
                this.CurrentItem = item;
            }

            this.ContentChanged.Fire(typeof(JasilyClipboard<T>));
        }

        public void Copy(T item)
        {
            Debug.Assert(!ReferenceEquals(item, null));

            this.SetValue(ClipMode.Copy, item);
        }

        public void Cut(T item)
        {
            Debug.Assert(!ReferenceEquals(item, null));

            this.SetValue(ClipMode.Cut, item);
        }

        public bool IsExist()
        {
            return this.HasValue;
        }

        public T Paste()
        {
            if (!this.HasValue)
                throw new InvalidOperationException();

            lock (this.SyncRoot)
            {
                if (!this.HasValue)
                    throw new InvalidOperationException();

                try
                {
                    return this.CurrentItem;
                }
                finally
                {
                    if (this.Mode == ClipMode.Cut)
                    {
                        this.HasValue = false;
                        this.CurrentItem = default(T);

                        this.ContentChanged.BeginFire(typeof(JasilyClipboard<T>));
                    }
                }
            }
        }

        private enum ClipMode
        {
            Copy,

            Cut
        }
    }
}
