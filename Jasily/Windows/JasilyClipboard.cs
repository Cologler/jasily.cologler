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
        private Action<T> PasteCallback;
        private ClipMode Mode;

        public event EventHandler ContentChanged;

        private void SetValue(ClipMode mode, T item, Action<T> pasteCallback)
        {
            lock (this.SyncRoot)
            {
                this.HasValue = true;
                this.Mode = mode;
                this.CurrentItem = item;
                this.PasteCallback = pasteCallback;
            }

            this.ContentChanged.Fire(typeof(JasilyClipboard<T>));
        }

        public void Copy(T item, Action<T> pasteCallback = null)
        {
            Debug.Assert(!ReferenceEquals(item, null));

            this.SetValue(ClipMode.Copy, item, pasteCallback);
        }

        public void Cut(T item, Action<T> pasteCallback = null)
        {
            Debug.Assert(!ReferenceEquals(item, null));

            this.SetValue(ClipMode.Cut, item, pasteCallback);
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
                    this.PasteCallback.BeginFire(this.CurrentItem);

                    if (this.Mode == ClipMode.Cut)
                    {
                        this.HasValue = false;
                        this.CurrentItem = default(T);
                        this.PasteCallback = null;

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
