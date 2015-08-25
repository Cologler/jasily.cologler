using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Jasily.Reflection
{
    public static class IJasilyMemberInfoExtensions
    {
        public static IEnumerable<IJasilyMemberInfo> GetRuntimeJasilyMemberInfos(this Type type,
            Func<FieldInfo, bool> fieldFilter = null,
            Func<PropertyInfo, bool> propertyFilter = null)
        {
            foreach (var member in fieldFilter == null ? type.GetRuntimeFields() : type.GetRuntimeFields().Where(fieldFilter))
                yield return member.AsJasilyMemberInfo();

            foreach (var member in propertyFilter == null ? type.GetRuntimeProperties() : type.GetRuntimeProperties().Where(propertyFilter))
                yield return member.AsJasilyMemberInfo();
        }

        public static IJasilyMemberInfo AsJasilyMemberInfo(this FieldInfo field) => (JasilyFieldInfo)field;

        public static IJasilyMemberInfo AsJasilyMemberInfo(this PropertyInfo property) => (JasilyPropertyInfo)property;

        private abstract class JasilyMemberInfo : IJasilyMemberInfo 
        {
            protected JasilyMemberInfo(MemberInfo memberInfo)
            {
                this.IsCompilerGenerated = memberInfo.HasCustomAttribute<CompilerGeneratedAttribute>();
            }

            public abstract MemberInfo Member { get; }

            public abstract bool CanRead { get; }

            public abstract bool CanWrite { get; }

            public abstract bool IsStatic { get; }

            public abstract Type ValueType { get; }
            
            public abstract JasilyMemberType MemberType { get; }

            public bool IsCompilerGenerated { get; }

            public T GetCustomAttribute<T>() where T : Attribute
                => this.Member.GetCustomAttribute<T>();

            public IEnumerable<Attribute> GetCustomAttributes()
                => this.Member.GetCustomAttributes();

            public IEnumerable<T> GetCustomAttributes<T>() where T : Attribute
                => this.Member.GetCustomAttributes<T>();

            public bool HasCustomAttribute<T>() where T : Attribute
                => this.Member.HasCustomAttribute<T>();

            public abstract object GetInstanceValue(object instance);

            public abstract T GetInstanceValue<T>(object instance);

            public abstract void SetInstanceValue(object instance, object value);
        }

        private class JasilyFieldInfo : JasilyMemberInfo
        {
            #region field

            private readonly FieldInfo member;

            private JasilyFieldInfo(FieldInfo field)
                : base(field)
            {
                this.member = field;
            }

            public static implicit operator JasilyFieldInfo(FieldInfo value) => new JasilyFieldInfo(value);

            #endregion

            public override MemberInfo Member => this.member;

            public override bool CanRead => !this.member.IsLiteral && !this.member.IsInitOnly;

            public override bool CanWrite => !this.member.IsLiteral && !this.member.IsInitOnly;

            public override bool IsStatic => this.member.IsStatic;

            public override Type ValueType => this.member.FieldType;

            public override JasilyMemberType MemberType => JasilyMemberType.Field;

            public override object GetInstanceValue(object instance)
                => this.member.GetValue(instance);

            public override T GetInstanceValue<T>(object instance)
                => (T)this.member.GetValue(instance);

            public override void SetInstanceValue(object instance, object value)
                => this.member.SetValue(instance, value);
        }

        private class JasilyPropertyInfo : JasilyMemberInfo
        {
            #region property

            private readonly PropertyInfo member;

            private JasilyPropertyInfo(PropertyInfo property)
                : base(property)
            {
                this.member = property;
                this.IsStatic = (property.CanRead ? property.GetMethod : property.SetMethod).IsStatic;
            }

            public static implicit operator JasilyPropertyInfo(PropertyInfo value) => new JasilyPropertyInfo(value);

            #endregion

            public override MemberInfo Member => this.member;

            public override bool CanRead => this.member.CanRead;

            public override bool CanWrite => this.member.CanWrite;
            
            public override bool IsStatic { get; }

            public override Type ValueType => this.member.PropertyType;

            public override JasilyMemberType MemberType => JasilyMemberType.Property;

            public override object GetInstanceValue(object instance)
                => this.member.GetValue(instance);

            public override T GetInstanceValue<T>(object instance)
                => (T)this.member.GetValue(instance);

            public override void SetInstanceValue(object instance, object value)
                => this.member.SetValue(instance, value);
        }
    }
}