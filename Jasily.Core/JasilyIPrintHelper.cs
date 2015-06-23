using System.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System
{
    public static class JasilyIPrintHelper
    {
        /// <summary>
        /// print class or struct's member value.
        /// <para>if use Print attribute on class or struct, only print it's member which has Print attribute.</para>
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="indent"></param>
        /// <returns></returns>
        public static string Print(this IPrint obj, int indent = 2)
        {
            if (obj == null) return "[null]";

            return Print(obj, indent, 0);
        }

        private static string Print(this object obj, int indent, int level)
        {
            if (obj == null) return "[null]";

            var print = obj as IPrint;

            if (print == null) return obj.ToString();

            var type = obj.GetType();

            bool dontNeedAttr = type.GetTypeInfo().GetCustomAttribute<PrintAttribute>() == null;

            var sb = new List<string>();

            if (level != 0) sb.Add("");

            sb.AddRange(from f in type.GetRuntimeFields()
                .Where(z => !z.IsStatic &&
                            (dontNeedAttr || CustomAttributeExtensions.GetCustomAttribute<PrintAttribute>((MemberInfo) z) != null))
                let value = f.GetValue(obj)
                select String.Format("{0}[{1}] {2}", ' '.Repeat(indent * level), f.Name, Print(value, indent, level + 1)));

            sb.AddRange(from p in type.GetRuntimeProperties()
                .Where(z => z.CanRead &&
                            (dontNeedAttr || z.GetCustomAttribute<PrintAttribute>() != null))
                let value = p.GetValue(obj)
                select String.Format("{0}[{1}] {2}", ' '.Repeat(indent * level), p.Name, Print(value, indent, level + 1)));

            return sb.AsLines();
        }
    }
}