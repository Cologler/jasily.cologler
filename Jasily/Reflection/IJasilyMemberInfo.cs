using System;
using System.Collections.Generic;
using System.Reflection;

namespace Jasily.Reflection
{
    public interface IJasilyMemberInfo
    {
        #region meta

        MemberInfo Member { get; }

        bool CanRead { get; }

        bool CanWrite { get; }

        bool IsStatic { get; }

        Type ValueType { get; }

        JasilyMemberType MemberType { get; }

        bool IsCompilerGenerated { get; }

        #endregion

        #region attribute

        T GetCustomAttribute<T>() where T : Attribute;

        IEnumerable<Attribute> GetCustomAttributes();

        IEnumerable<T> GetCustomAttributes<T>() where T : Attribute;

        bool HasCustomAttribute<T>() where T : Attribute;

        #endregion

        #region method

        object GetInstanceValue(object instance);

        T GetInstanceValue<T>(object instance);

        void SetInstanceValue(object instance, object value);

        #endregion
    }
}