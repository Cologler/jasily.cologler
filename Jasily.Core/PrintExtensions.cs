using System.Attributes;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace System
{
    public static class PrintExtensions
    {
        private interface IWalker
        {
            int Indent { get; }
            int Level { get; }
            StringBuilder Builder { get; }

            void Walk(object obj);
        }

        private struct PrintObjectWalker : IBuilder<string>
        {
            private readonly IPrint obj;
            private readonly int indent;

            public PrintObjectWalker(IPrint obj, int indent)
            {
                this.obj = obj;
                this.indent = indent;
            }

            public string Build()
            {
                var builder = new StringBuilder();
                var walker = new ObjectWalker(this.indent, 0, builder);
                walker.Walk(this.obj);
                return builder.ToString();
            }

            private struct FieldWalker : IWalker
            {
                private readonly FieldInfo field;
                private readonly ObjectWalker valueWalker;

                public FieldWalker(FieldInfo field, ObjectWalker valueWalker,
                    int indent, int level, StringBuilder builder)
                {
                    this.field = field;
                    this.valueWalker = valueWalker;
                    this.Indent = indent;
                    this.Level = level;
                    this.Builder = builder;
                }

                public int Indent { get; }
                public int Level { get; }
                public StringBuilder Builder { get; }

                public void Walk(object obj)
                {
                    this.WriteIndent();
                    this.Builder.Append("[Field] ");

                    if (this.field.IsPublic)
                        this.Builder.Append("public ");
                    else if (this.field.IsFamily)
                        this.Builder.Append("protected ");
                    else if (this.field.IsFamilyAndAssembly)
                        this.Builder.Append("protected internal ");
                    else if (this.field.IsPrivate)
                        this.Builder.Append("private ");

                    this.Builder.AppendFormat("{0} {1} = ",
                        this.field.FieldType.GetCSharpName(),
                        this.field.Name);

                    this.valueWalker.Walk(this.field.GetValue(obj));
                    this.Builder.AppendLine();
                }
            }

            private struct PropertyWalker : IWalker
            {
                private readonly PropertyInfo property;
                private readonly ObjectWalker valueWalker;

                public PropertyWalker(PropertyInfo property, ObjectWalker valueWalker,
                    int indent, int level, StringBuilder builder)
                {
                    this.property = property;
                    this.valueWalker = valueWalker;
                    this.Indent = indent;
                    this.Level = level;
                    this.Builder = builder;
                }

                public int Indent { get; }
                public int Level { get; }
                public StringBuilder Builder { get; }

                public void Walk(object obj)
                {
                    this.WriteIndent();
                    this.Builder.Append("[Property] ");

                    this.Builder.AppendFormat("{0} {1} ",
                        this.property.PropertyType.GetCSharpName(),
                        this.property.Name);
                    this.Builder.Append("{ ");
                    this.Builder.Append(GetMethodAttribute(this.property.GetMethod.Attributes));
                    this.Builder.Append("get; ");
                    if (this.property.CanWrite)
                    {
                        this.Builder.Append(GetMethodAttribute(this.property.SetMethod.Attributes));
                        this.Builder.Append("set; ");
                    }
                    this.Builder.Append("} = ");

                    this.valueWalker.Walk(this.property.GetValue(obj));
                }

                private static string GetMethodAttribute(MethodAttributes attr)
                {
                    if (attr.HasFlag(MethodAttributes.Public)) return "public ";
                    else if (attr.HasFlag(MethodAttributes.Family)) return "protected ";
                    else if (attr.HasFlag(MethodAttributes.FamANDAssem)) return "protected internal ";
                    else if (attr.HasFlag(MethodAttributes.Private)) return "private ";
                    return "";
                }
            }

            private struct ObjectWalker : IWalker
            {
                public int Indent { get; }
                public int Level { get; }
                public StringBuilder Builder { get; }

                public ObjectWalker(int indent, int level, StringBuilder builder)
                {
                    this.Indent = indent;
                    this.Level = level;
                    this.Builder = builder;
                }

                //public void Walk(object obj)
                //{
                //    this.InnerWalk(obj);
                //    this.WriteEnd();
                //}

                public void Walk(object obj)
                {
                    if (obj == null)
                    {
                        this.WriteNull();
                        return;
                    }

                    var str = obj as string;
                    if (str != null)
                    {
                        this.WriteString(str);
                        return;
                    }

                    var printer = obj as IPrint;
                    if (printer != null)
                    {
                        this.Builder.AppendFormat("new {0}()", printer.GetType().GetCSharpName());
                        this.WritePrinter(printer);
                        return;
                    }

                    var buff = obj as byte[];
                    if (buff != null)
                    {
                        this.Builder.AppendLine(buff.Take(128).GetHexString() + "...");
                        return;
                    }

                    if (obj is Stream)
                    {
                        this.Builder.AppendLine("stream content was ignored.");
                        return;
                    }

                    var array = obj as IEnumerable;
                    if (array != null)
                    {
                        this.WriteEnumerable(array);
                        return;
                    }

                    this.Builder.Append(obj);
                }

                private void WriteEnumerable(IEnumerable array)
                {
                    var childObjectWalker = new ObjectWalker(
                        this.Indent, this.Level + 2, this.Builder);

                    this.WriteTypeName(array.GetType());

                    var coll = array as ICollection;
                    if (coll != null && coll.Count == 0)
                    {
                        this.Builder.Append(" []");
                        return;
                    }

                    this.WriteIndent();
                    this.Builder.Append("[");
                    this.WriteIndent();
                    foreach (var o in array)
                    {
                        this.Builder.Append(' '.Repeat(this.Indent));
                        childObjectWalker.Walk(o);
                        this.Builder.Append(",");
                        this.WriteIndent();
                    }
                    this.Builder.Append("]");
                }

                private void WritePrinter(IPrint print)
                {
                    var type = print.GetType();
                    var childObjectWalker = new ObjectWalker(
                        this.Indent, this.Level + 2, this.Builder);

                    bool unAttr = type.GetTypeInfo().GetCustomAttribute<PrintAttribute>() == null;

                    this.WriteIndent();
                    this.Builder.Append("{");

                    foreach (var f in type.GetRuntimeFields().Where(f =>
                        !f.IsStatic &&
                        (unAttr || f.GetCustomAttribute<PrintAttribute>() != null) &&
                        f.Name[0] != '<'))
                    {
                        var fieldWalker = new FieldWalker(f, childObjectWalker,
                            this.Indent, this.Level + 1, this.Builder);
                        fieldWalker.Walk(print);
                    }

                    foreach (var p in type.GetRuntimeProperties())
                    {
                        if (p.CanRead && (unAttr || p.GetCustomAttribute<PrintAttribute>() != null))
                        {
                            var propertyWalker = new PropertyWalker(p, childObjectWalker,
                                this.Indent, this.Level + 1, this.Builder);
                            propertyWalker.Walk(print);
                        }
                    }

                    this.WriteIndent();
                    this.Builder.Append("}");
                }

                private void WriteString(string text)
                    => this.Builder.Append("\"")
                           .Append(text.Replace("\r\n", "<\\r\\n>").Replace("\n", "<\\n>"))
                           .Append("\"");

                private void WriteNull() => this.Builder.Append("null");

                private void WriteEnd() => this.Builder.Append(";");
            }
        }

        private static void WriteIndent(this IWalker walker)
        {
            walker.Builder.AppendLine();
            if (walker.Level > 0 && walker.Indent > 0)
            {
                walker.Builder.Append(' '.Repeat(walker.Indent * walker.Level));
            }
        }

        private static void WriteTypeName(this IWalker walker, Type type)
            => walker.Builder.AppendFormat("new {0}()", type.GetCSharpName());

        /// <summary>
        /// print class or struct's member value.
        /// <para>if use Print attribute on class or struct, only print it's member which has Print attribute.</para>
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="indent"></param>
        /// <returns></returns>
        public static string Print(this IPrint obj, int indent = 2)
        {
            return new PrintObjectWalker(obj, indent).Build();
        }
    }
}