using System;
using System.Diagnostics;
using System.EventArgses;
using System.Threading;

namespace Jasily.Windows
{
    public class JasilyClipboard<T>
    {
        public static JasilyClipboard<T> Default { get; } = new JasilyClipboard<T>();

        private readonly object syncRoot = new object();

        private ClipboardValueContainer container;

        public event EventHandler<ChangingEventArgs<T>> ContentChanged;

        private void SetValue(ClipMode mode, T item)
        {
            var @new = new ClipboardValueContainer(item, mode);
            var old = Interlocked.Exchange(ref this.container, @new);
            this.ContentChanged.BeginFire(this, new ChangingEventArgs<T>(old != null ? old.Value : default(T), item));
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

        public bool HasValue => this.container != null;

        /// <summary>
        /// read value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryReadValue(out T value)
        {
            var container = this.container;
            if (container != null)
            {
                value = container.Value;
                return true;
            }
            else
            {
                value = default(T);
                return false;
            }
        }

        public T Paste()
        {
            bool isCutMode;
            T value;

            lock (this.syncRoot)
            {
                if (this.container == null) return default(T);
                value = this.container.Value;
                isCutMode = this.container.Mode == ClipMode.Cut;
                if (isCutMode) this.container = null;
            }

            if (isCutMode)
                this.ContentChanged.BeginFire(this, new ChangingEventArgs<T>(value, default(T)));

            return value;
        }

        private enum ClipMode
        {
            Copy,
            Cut
        }

        private class ClipboardValueContainer
        {
            internal ClipMode Mode { get; }

            internal T Value { get; }

            internal ClipboardValueContainer(T value, ClipMode mode)
            {
                this.Value = value;
                this.Mode = mode;
            }
        }
    }
}
