using System.Attributes;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace System
{
    public static class PrintExtensions
    {
        private class PrintObjectWalker : IBuilder<string>
        {
            private readonly IPrint obj;
            private readonly int indent;
            private int level;
            private StringBuilder builder;

            public PrintObjectWalker(IPrint obj, int indent)
            {
                this.obj = obj;
                this.indent = indent;
            }

            public string Build()
            {
                this.level = 0;
                this.builder = new StringBuilder();
                if (this.obj == null)
                    this.WriteNull();
                else
                    this.Walk(this.obj);
                return this.builder.ToString();
            }

            private void WriteNull()
            {
                this.builder.Append("null");
                this.builder.AppendLine();
            }

            private void WriteIndent(int level = 0)
            {
                level = this.level + level;
                if (level > 0 && this.indent > 0)
                {
                    this.builder.Append(' '.Repeat(this.indent * level));
                }
            }

            private void Walk(IPrint print)
            {
                var type = print.GetType();

                bool unAttr = type.GetTypeInfo().GetCustomAttribute<PrintAttribute>() == null;

                foreach (var f in type.GetRuntimeFields())
                {
                    if (!f.IsStatic &&
                        (unAttr || f.GetCustomAttribute<PrintAttribute>() != null) &&
                        f.Name[0] != '<')
                    {
                        this.WriteIndent();
                        this.builder.Append("field ");

                        if (f.IsPublic) this.builder.Append("public ");
                        else if (f.IsFamily) this.builder.Append("protected ");
                        else if (f.IsFamilyAndAssembly) this.builder.Append("protected internal ");
                        else if (f.IsPrivate) this.builder.Append("private ");

                        this.builder.AppendFormat("{0} {1} = ", f.FieldType.GetCSharpName(), f.Name);

                        this.Walk(f.GetValue(print));
                    }
                }

                foreach (var p in type.GetRuntimeProperties())
                {
                    if (p.CanRead && (unAttr || p.GetCustomAttribute<PrintAttribute>() != null))
                    {
                        this.WriteIndent();
                        this.builder.Append("property ");

                        this.builder.AppendFormat("{0} {1} ", p.PropertyType.GetCSharpName(), p.Name);
                        this.builder.Append("{ ");
                        this.builder.Append(GetMethodAttribute(p.GetMethod.Attributes));
                        this.builder.Append("get; ");
                        if (p.CanWrite)
                        {
                            this.builder.Append(GetMethodAttribute(p.SetMethod.Attributes));
                            this.builder.Append("set; ");
                        }
                        this.builder.Append("} = ");

                        this.Walk(p.GetValue(print));
                    }
                }
            }

            private static string GetMethodAttribute(MethodAttributes attr)
            {
                if (attr.HasFlag(MethodAttributes.Public)) return "public ";
                else if (attr.HasFlag(MethodAttributes.Family)) return "protected ";
                else if (attr.HasFlag(MethodAttributes.FamANDAssem)) return "protected internal ";
                else if (attr.HasFlag(MethodAttributes.Private)) return "private ";
                return "";
            }

            private void Walk(object obj)
            {
                this.level++;
                this.InnerWalk(obj);
                this.level--;
            }

            private void InnerWalk(object obj)
            {
                if (obj == null)
                {
                    this.WriteNull();
                    return;
                }

                var str = obj as string;

                if (str != null)
                {
                    this.builder.Append("\"");
                    this.builder.Append(str.Replace("\r\n", "<\\r\\n>").Replace("\n", "<\\n>"));
                    this.builder.Append("\"");
                    this.builder.AppendLine();
                    return;
                }

                var print = obj as IPrint;

                if (print != null)
                {
                    this.builder.AppendFormat("CLASS<  {0}  >", print.GetType().GetCSharpName());
                    this.builder.AppendLine();
                    this.Walk(print);
                    return;
                }

                var buff = obj as byte[];

                if (buff != null)
                {
                    this.builder.AppendLine(buff.Take(128).GetHexString() + "...");
                    return;
                }

                if (obj is Stream)
                {
                    this.builder.AppendLine("stream content was ignored.");
                    return;
                }

                var array = obj as IEnumerable;

                if (array != null)
                {
                    this.builder.AppendFormat("CLASS<  {0}  >", array.GetType().GetCSharpName());
                    this.builder.AppendLine();
                    this.WriteIndent();
                    this.builder.Append("[");
                    foreach (var o in array)
                    {
                        this.builder.Append("{");
                        this.builder.AppendLine();
                        this.WriteIndent(1);
                        this.Walk(o);
                        this.WriteIndent();
                        this.builder.Append("},");
                    }
                    this.builder.AppendLine("]");
                    return;
                }
                
                this.builder.Append(obj);
                this.builder.AppendLine();
            }
        }

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