﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Jasily.Diagnostics
{
    public static class JasilyDebug
    {
        #region assert

        [Conditional("DEBUG")]
        public static void Assert(Func<bool> condition)
        {
            Debug.Assert(condition());
        }
        [Conditional("DEBUG")]
        public static void Assert(Func<bool> condition, string message)
        {
            Debug.Assert(condition(), message);
        }
        [Conditional("DEBUG")]
        public static void Assert(Func<bool> condition, Func<string> message)
        {
            Debug.Assert(condition(), message());
        }

        [Conditional("DEBUG")]
        public static void AssertIsType<T>(object obj)
        {
            Debug.Assert(obj != null);
            AssertIsType<T>(obj.GetType());
        }
        [Conditional("DEBUG")]
        public static void AssertIsType<T>(Type type)
        {
            Debug.Assert(type == typeof(T), $"assert type failed. {type.FullName} not {typeof(T).FullName}");
        }

        [Conditional("DEBUG")]
        public static void AssertNotNull(object obj) => Debug.Assert(obj != null);

        [Conditional("DEBUG")]
        public static void AssertForEach<T>(IEnumerable<T> items, Func<T, bool> tester)
        {
            foreach (var item in items)
            {
                Debug.Assert(tester(item));
            }
        }

        #endregion

        #region write line

        [Conditional("DEBUG")]
        public static void WriteLine(Func<string> message) => Debug.WriteLine(message());

        [Conditional("DEBUG")]
        public static void WriteLine(Func<object> value) => Debug.WriteLine(value());

        [Conditional("DEBUG")]
        public static void WriteLineIf(Func<bool> condition, string message) => Debug.WriteLineIf(condition(), message);

        [Conditional("DEBUG")]
        public static void WriteLineIf(Func<bool> condition, Func<string> message) => Debug.WriteLineIf(condition(), message());

        #endregion

        [Conditional("DEBUG")]
        public static void NotImplemented(string message = null, bool @continue = false)
        {
            if (Debugger.IsAttached) Debugger.Break();
            if (@continue) return;
            Debug.Assert(false, message);
            throw new NotImplementedException(message);
        }

        #region pointer

        [Conditional("DEBUG")]
        public static void Pointer(string message = null,
            [CallerFilePath] string path = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
        {
            var msg = message == null
                ? $"[POINTER] {path} ({line}) {member}"
                : $"[POINTER] [{message}] {path} ({line}) {member}";
            Debug.WriteLine(msg);
            OnPointer?.Invoke(null, msg);
        }

        public static event EventHandler<string> OnPointer;

        #endregion
    }
}