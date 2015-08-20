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

        public static IJasilyMemberInfo AsJasilyMemberInfo(this FieldInfo field)
        {
            return (JasilyFieldInfo)field;
        }

        public static IJasilyMemberInfo AsJasilyMemberInfo(this PropertyInfo property)
        {
            return (JasilyPropertyInfo)property;
        }

        private struct JasilyFieldInfo : IJasilyMemberInfo
        {
            #region field

            private readonly FieldInfo member;

            public JasilyFieldInfo(FieldInfo field)
            {
                this.member = field;
                this.IsCompilerGenerated = field.HasCustomAttribute<CompilerGeneratedAttribute>();
            }

            public static implicit operator JasilyFieldInfo(FieldInfo value)
            {
                return new JasilyFieldInfo(value);
            }

            #endregion

            public T GetCustomAttribute<T>() where T : Attribute
                => this.member.GetCustomAttribute<T>();

            public IEnumerable<Attribute> GetCustomAttributes()
                => this.member.GetCustomAttributes();

            public IEnumerable<T> GetCustomAttributes<T>() where T : Attribute
                => this.member.GetCustomAttributes<T>();

            public bool HasCustomAttribute<T>() where T : Attribute
                => this.member.HasCustomAttribute<T>();

            public string Name => this.member.Name;

            public bool CanRead => true;

            public bool CanWrite => !this.member.IsLiteral && !this.member.IsInitOnly;

            public bool IsStatic => this.member.IsStatic;

            public Type ValueType => this.member.FieldType;

            public Type DeclaringType => this.member.DeclaringType;

            public JasilyMemberType MemberType => JasilyMemberType.Field;

            public object GetInstanceValue(object instance)
                => this.member.GetValue(instance);

            public T GetInstanceValue<T>(object instance)
                => (T)this.member.GetValue(instance);

            public void SetInstanceValue(object instance, object value)
                => this.member.SetValue(instance, value);

            public bool IsCompilerGenerated { get; }
        }

        private struct JasilyPropertyInfo : IJasilyMemberInfo
        {
            #region property

            private readonly PropertyInfo member;

            public JasilyPropertyInfo(PropertyInfo property)
            {
                this.member = property;
                this.IsStatic = (property.CanRead ? property.GetMethod : property.SetMethod).IsStatic;
                this.IsCompilerGenerated = property.HasCustomAttribute<CompilerGeneratedAttribute>();
            }

            public static implicit operator JasilyPropertyInfo(PropertyInfo value)
            {
                return new JasilyPropertyInfo(value);
            }

            #endregion

            public T GetCustomAttribute<T>() where T : Attribute
                => this.member.GetCustomAttribute<T>();

            public IEnumerable<Attribute> GetCustomAttributes()
                => this.member.GetCustomAttributes();

            public IEnumerable<T> GetCustomAttributes<T>() where T : Attribute
                => this.member.GetCustomAttributes<T>();

            public bool HasCustomAttribute<T>() where T : Attribute
                => this.member.HasCustomAttribute<T>();

            public string Name => this.member.Name;

            public bool CanRead => this.member.CanRead;

            public bool CanWrite => this.member.CanWrite;
            
            public bool IsStatic { get; }

            public Type ValueType => this.member.PropertyType;

            public Type DeclaringType => this.member.DeclaringType;

            public JasilyMemberType MemberType => JasilyMemberType.Property;

            public object GetInstanceValue(object instance)
                => this.member.GetValue(instance);

            public T GetInstanceValue<T>(object instance)
                => (T)this.member.GetValue(instance);

            public void SetInstanceValue(object instance, object value)
                => this.member.SetValue(instance, value);

            public bool IsCompilerGenerated { get; }
        }
    }
}