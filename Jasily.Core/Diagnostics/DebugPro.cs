namespace System.Diagnostics
{
    public static class DebugPro
    {
        [Conditional("DEBUG")]
        public static void AssertType<T>(this object obj)
        {
            Debug.Assert(obj != null, "obj should not be null.");
            Debug.Assert(obj is T, String.Format("assert object type false. {0} not {1}", obj.GetType().FullName, typeof(T).FullName));
        }

        [Conditional("DEBUG")]
        public static void AssertType<T>(this Type type)
        {
            Debug.Assert(type == typeof(T), String.Format("assert object type false. {0} not {1}", type.FullName, typeof(T).FullName));
        }
    }
}
