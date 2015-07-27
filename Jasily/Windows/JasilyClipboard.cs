using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows
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
            lock (SyncRoot)
            {
                HasValue = true;
                Mode = mode;
                CurrentItem = item;
            }

            ContentChanged.Fire(typeof(JasilyClipboard<T>));
        }

        public void Copy(T item)
        {
            Debug.Assert(!ReferenceEquals(item, null));

            SetValue(ClipMode.Copy, item);
        }

        public void Cut(T item)
        {
            Debug.Assert(!ReferenceEquals(item, null));

            SetValue(ClipMode.Cut, item);
        }

        public bool IsExist()
        {
            return HasValue;
        }

        public T Paste()
        {
            if (!HasValue)
                throw new InvalidOperationException();

            lock (SyncRoot)
            {
                if (!HasValue)
                    throw new InvalidOperationException();

                try
                {
                    return CurrentItem;
                }
                finally
                {
                    if (Mode == ClipMode.Cut)
                    {
                        HasValue = false;
                        CurrentItem = default(T);

                        ContentChanged.BeginFire(typeof(JasilyClipboard<T>));
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
