using System.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace System.Diagnostics
{
    public static class PrintExtensions
    {
        private const int MaxLevel = 10;

        /// <summary>
        /// print class or struct's member value.
        /// <para>if use Print attribute on class or struct, only print it's member which has Print attribute.</para>
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="indent"></param>
        /// <returns></returns>
        public static string Print(this IPrintable obj, int indent = 2) => new PrintObjectWalker(obj, indent).Build();

        private interface IWalker
        {
            int Indent { get; }

            int Level { get; }

            StringBuilder Builder { get; }

            void Walk(object obj);
        }

        private static void WriteNewLine<T>(this T walker) where T : IWalker
        {
            walker.Builder.AppendLine();
            if (walker.Level > 0 && walker.Indent > 0)
            {
                walker.Builder.Append(' ', walker.Indent * walker.Level);
            }
        }

        private static void WriteTypeName<T>(this T walker, Type type) where T : IWalker
            => walker.Builder.Append(type.GetCSharpName()).Append("()");

        private struct PrintObjectWalker : IBuilder<string>
        {
            private readonly IPrintable obj;
            private readonly int indent;

            public PrintObjectWalker(IPrintable obj, int indent)
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
                    this.WriteNewLine();
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
                    this.WriteNewLine();
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

                public void Walk(object obj)
                {
                    if (obj == null)
                    {
                        this.WriteNull();
                        return;
                    }

                    if (this.Level > MaxLevel)
                    {
                        this.Builder.Append("...");
                    }

                    var str = obj as string;
                    if (str != null)
                    {
                        this.WriteString(str);
                        return;
                    }

                    var printer = obj as IPrintable;
                    if (printer != null)
                    {
                        this.WriteTypeName(printer.GetType());
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

                    var type = obj.GetType();
                    if (type.GetTypeInfo().IsEnum)
                    {
                        this.Builder.Append(type.Name);
                        this.Builder.Append('.');
                        this.Builder.Append(obj);
                        return;
                    }

                    this.Builder.Append(obj);
                }

                private void WriteEnumerable(IEnumerable array)
                {
                    this.WriteTypeName(array.GetType());

                    var items = new List<object>(10);
                    var itor = array.GetEnumerator();
                    items.AddRange(from _ in Generater.Repeat(10) where itor.MoveNext() select itor.Current);
                    var hasNext = itor.MoveNext();

                    if (items.Count == 0)
                    {
                        this.Builder.Append(" []");
                        return;
                    }

                    var childObjectWalker = new ObjectWalker(
                        this.Indent, this.Level + 2, this.Builder);

                    this.WriteNewLine();
                    this.Builder.Append("[");
                    this.WriteNewLine();
                    foreach (var o in items)
                    {
                        this.Builder.Append(' ', this.Indent);
                        childObjectWalker.Walk(o);
                        this.Builder.Append(",");
                        this.WriteNewLine();
                    }
                    this.WriteNewLine();
                    if (hasNext)
                    {
                        this.Builder.Append("... MORE");
                        this.WriteNewLine();
                    }
                    this.Builder.Append("]");
                }

                private void WritePrinter(IPrintable printable)
                {
                    var type = printable.GetType();
                    var childObjectWalker = new ObjectWalker(
                        this.Indent, this.Level + 2, this.Builder);

                    bool unAttr = type.GetTypeInfo().GetCustomAttribute<PrintAttribute>() == null;

                    this.WriteNewLine();
                    this.Builder.Append("{");

                    foreach (var f in type.GetRuntimeFields().Where(f =>
                        !f.IsStatic &&
                        (unAttr || f.GetCustomAttribute<PrintAttribute>() != null) &&
                        f.Name[0] != '<'))
                    {
                        var fieldWalker = new FieldWalker(f, childObjectWalker,
                            this.Indent, this.Level + 1, this.Builder);
                        fieldWalker.Walk(printable);
                    }

                    foreach (var p in type.GetRuntimeProperties())
                    {
                        if (p.CanRead && (unAttr || p.GetCustomAttribute<PrintAttribute>() != null))
                        {
                            var propertyWalker = new PropertyWalker(p, childObjectWalker,
                                this.Indent, this.Level + 1, this.Builder);
                            propertyWalker.Walk(printable);
                        }
                    }

                    this.WriteNewLine();
                    this.Builder.Append("}");
                }

                private void WriteString(string text)
                {
                    text = text
                        .Replace("\\", "\\\\")
                        .Replace("\"", "\\\"")
                        .Replace("\n", "\\n")
                        .Replace("\r", "\\r");

                    this.Builder.Append("\"");
                    if (text.Length > 128)
                    {
                        this.Builder.Append(text, 0, 128);
                    }
                    else
                    {
                        this.Builder.Append(text);
                    }
                    this.Builder.Append("\"");
                }

                private void WriteNull() => this.Builder.Append("null");

                private void WriteEnd() => this.Builder.Append(";");
            }
        }
    }
}