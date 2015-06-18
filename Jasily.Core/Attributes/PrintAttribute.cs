namespace System.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct,
        Inherited = false, AllowMultiple = false)]
    public class PrintAttribute : Attribute
    {
         
    }
}