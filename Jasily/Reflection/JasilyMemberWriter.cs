using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Jasily.Reflection
{
    public sealed class JasilyMemberWriter<T>
    {
        public Dictionary<string, Action<object, object>> Mapped;

        public void Mapping()
        {
            if (this.Mapped != null) return;

            var mapping = new Dictionary<string, Action<object, object>>();

            foreach (var member in typeof(T).GetRuntimeJasilyMemberInfos())
            {
                var attr = member.GetCustomAttribute<WriteableMemberAttribute>();
                if (attr != null)
                {
                    if (!member.CanWrite) throw new NotSupportedException("member not support write.");
                    if (member.IsStatic) throw new NotSupportedException("static member was not supported.");

                    var name = attr.Name.IsNullOrWhiteSpace() ? member.Name : attr.Name;

                    if (mapping.ContainsKey(name)) throw new InvalidOperationException("has two same member name.");

                    mapping.Add(name, member.SetInstanceValue);
                }
            }

            Interlocked.CompareExchange(ref this.Mapped, mapping, null);
        }

        public void Write(T obj, string name, object value)
        {
            var mapped = this.Mapped;
            if (mapped == null)
                throw new InvalidOperationException("please call Mapping() before write");

            mapped.GetValueOrDefault(name)?.Invoke(obj, value);
        }
    }
}