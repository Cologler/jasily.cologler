using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Jasily.Reflection
{
    public sealed class JasilyMemberWriter<T>
    {
        Dictionary<string, Action<object, object>> mapped;

        public void Mapping()
        {
            if (this.mapped != null) return;

            var mapping = new Dictionary<string, Action<object, object>>();

            foreach (var member in typeof(T).GetRuntimeJasilyMemberInfos())
            {
                var attr = member.GetCustomAttribute<WriteableMemberAttribute>();
                if (attr != null)
                {
                    if (!member.CanWrite) throw new NotSupportedException("member not support write.");
                    if (member.IsStatic) throw new NotSupportedException("static member was not supported.");

                    var name = attr.Name.IsNullOrWhiteSpace() ? member.Member.Name : attr.Name;

                    if (mapping.ContainsKey(name)) throw new InvalidOperationException("has two same member name.");

                    mapping.Add(name, member.SetInstanceValue);
                }
            }

            Interlocked.CompareExchange(ref this.mapped, mapping, null);
        }

        public bool TryWrite(T obj, string name, object value)
        {
            var mapped = this.mapped;
            if (mapped == null)
                throw new InvalidOperationException("please call Mapping() before write");

            var setter = mapped.GetValueOrDefault(name);
            if (setter == null) return false;
            setter(obj, value);
            return true;
        }
    }
}