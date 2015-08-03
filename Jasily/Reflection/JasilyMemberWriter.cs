using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Jasily.Reflection
{
    public class JasilyMemberWriter<T>
    {
        public Dictionary<string, object> Mapped;

        public JasilyMemberWriter()
        {
            
        }

        private void Mapping()
        {
            foreach (var member in typeof(T).GetRuntimeJasilyMemberInfos())
            {
                var attr = member.GetCustomAttribute<WriteableMemberAttribute>();
                if (attr != null)
                {
                    var name = attr.Name.IsNullOrWhiteSpace() ? member.Name : attr.Name;
                }
            }
        }

        public void Write(string name, object value)
        {
            var mapped = this.Mapped;
            if (mapped == null)
                throw new InvalidOperationException("please call Mapping() before write");
        }
    }
}