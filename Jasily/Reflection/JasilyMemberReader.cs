using System;
using System.Collections.Generic;
using System.Threading;

namespace Jasily.Reflection
{
    public sealed class JasilyMemberReader<T>
    {
        public Dictionary<string, Func<object, object>> Mapped;

        public void Mapping()
        {
            if (this.Mapped != null) return;

            var mapping = new Dictionary<string, Func<object, object>>();

            foreach (var member in typeof(T).GetRuntimeJasilyMemberInfos())
            {
                var attr = member.GetCustomAttribute<ReadableMemberAttribute>();
                if (attr != null)
                {
                    if (!member.CanRead) throw new NotSupportedException("member not support read.");
                    if (member.IsStatic) throw new NotSupportedException("static member was not supported.");

                    var name = attr.Name.IsNullOrWhiteSpace() ? member.Name : attr.Name;

                    if (mapping.ContainsKey(name)) throw new InvalidOperationException("has two same member name.");

                    mapping.Add(name, member.GetInstanceValue);
                }
            }

            Interlocked.CompareExchange(ref this.Mapped, mapping, null);
        }

        public bool TryRead(T obj, string name, out object value)
        {
            var mapped = this.Mapped;
            if (mapped == null)
                throw new InvalidOperationException("please call Mapping() before read");

            Func<object, object> getter;
            if (mapped.TryGetValue(name, out getter))
            {
                value = getter.Invoke(obj);
                return true;
            }
            else
            {
                value = default(object);
                return false;
            }
        }
    }
}