using System.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System
{
    public static class PrintExtensions
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

            var sb = new List<string>();

            var print = obj as IPrint;

            if (print == null)
            {
                var array = obj as Array;

                if (array != null)
                {
                    sb.Add(String.Format("{0}[{1}]", ' '.Repeat(indent * level), "Array"));
                    sb.AddRange(array.OfType<object>()
                        .Select(z => String.Format("{0}{1}", ' '.Repeat(indent * (level + 1)), Print(z, indent, level + 2))));
                }
                else
                {
                    return obj.ToString();
                }
            }

            var type = obj.GetType();

            bool dontNeedAttr = type.GetTypeInfo().GetCustomAttribute<PrintAttribute>() == null;

            if (level != 0) sb.Add("");

            sb.AddRange(from f in type.GetRuntimeFields()
                where !f.IsStatic && (dontNeedAttr || f.GetCustomAttribute<PrintAttribute>() != null) && f.Name[0] != '<'
                let value = f.GetValue(obj)
                select String.Format("{0}[{1}] {2}", ' '.Repeat(indent * level), f.Name, Print(value, indent, level + 1)));

            sb.AddRange(from p in type.GetRuntimeProperties()
                where p.CanRead && (dontNeedAttr || p.GetCustomAttribute<PrintAttribute>() != null)
                let value = p.GetValue(obj)
                select String.Format("{0}[{1}] {2}", ' '.Repeat(indent * level), p.Name, Print(value, indent, level + 1)));

            return sb.AsLines();
        }
    }
}