namespace System.Collections.Generic
{
    public interface IGetKey<out T>
    {
        T GetKey();
    }
}