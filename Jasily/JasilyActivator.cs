using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Jasily
{
    public static class JasilyActivator
    {
        private static readonly Type ActivatorType = typeof(Activator<>);

        public static object CreateInstance(Type type) => ActivatorType.MakeGenericType(type).CreateInstance<Activator>().Create();

        public static T CreateInstance<T>() => (T)new Activator<T>().Create();

        private interface Activator
        {
            object Create();
        }

        public class Activator<T> : Activator
        {
            private static readonly Func<object> ActivatorFunc;

            static Activator()
            {
                var type = typeof(T);
                var typeInfo = type.GetTypeInfo();

                ConstructorInfo entryCtor = null;
                object[] entryParams = null;

                var ctors = typeInfo.DeclaredConstructors.ToArray();
                var entryCtors = ctors.Where(z => z.HasCustomAttribute<EntryAttribute>()).ToArray();
                if (entryCtors.Length > 1)
                {
                    throw new InvalidOperationException("too many entry constructor.");
                }
                else if (entryCtors.Length == 1)
                {
                    entryCtor = entryCtors[0];
                    var parameters = entryCtor.GetParameters();
                    if (parameters.Length == 0)
                    {
                        entryParams = Empty<object>.Array;
                    }
                    else
                    {
                        var entryParamsList = new List<object>();
                        foreach (var parameter in parameters)
                        {
                            var dv = parameter.GetCustomAttribute<DefaultValueAttribute>();
                            if (dv != null)
                            {
                                entryParamsList.Add(dv.Value);
                            }
                            else if (parameter.HasDefaultValue)
                            {
                                entryParamsList.Add(parameter.DefaultValue);
                            }
                            else
                            {
                                entryParamsList.Add(parameter.ParameterType.GetDefaultValue());
                            }
                        }
                        entryParams = entryParamsList.ToArray();
                    }
                }
                else
                {
                    foreach (var ctor in ctors)
                    {
                        var parameters = ctor.GetParameters();
                        if (parameters.Length == 0)
                        {
                            entryCtor = ctor;
                            entryParams = Empty<object>.Array;
                        }
                        else
                        {
                            if (parameters.All(z => z.HasDefaultValue))
                            {
                                if (entryCtor == null)
                                {
                                    entryCtor = ctor;
                                    entryParams = parameters.Select(z => z.DefaultValue).ToArray();
                                }
                            }
                        }
                    }
                }

                Debug.Assert((entryCtor != null && entryParams != null) || (entryCtor == null && entryParams == null));

                if (entryCtor != null)
                {
                    NewExpression @new;
                    if (entryParams.Length == 0)
                    {
                        @new = Expression.New(entryCtor);
                    }
                    else
                    {
                        @new = Expression.New(entryCtor, entryParams.Select(z => Expression.Constant(z)));
                    }

                    ActivatorFunc = Expression.Lambda<Func<object>>(@new).Compile();
                }
            }

            public Activator()
            {

            }

            #region Implementation of Activator

            public object Create()
            {
                if (ActivatorFunc == null) throw new NotSupportedException("cannot create instance.");
                return ActivatorFunc();
            }

            #endregion
        }

        [AttributeUsage(AttributeTargets.Constructor)]
        public class EntryAttribute : Attribute
        {

        }

        [AttributeUsage(AttributeTargets.Parameter)]
        public class DefaultValueAttribute : Attribute
        {
            public DefaultValueAttribute(object value)
            {
                this.Value = value;
            }

            public object Value { get; }
        }
    }
}