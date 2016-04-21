using System.Linq;

namespace System.Reflection
{
    public static class JasilyReflectionExtensions
    {
        #region get

        public static ParameterInfo GetParameter(this MethodBase method, string name)
        {
            return method.GetParameters().FirstOrDefault(z => z.Name == name);
        }

        #endregion
    }
}
