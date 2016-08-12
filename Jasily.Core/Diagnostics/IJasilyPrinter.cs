namespace System.Diagnostics
{
    public interface IJasilyPrinter<T>
    {
        /// <summary>
        /// print a object as mulit-line text.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        string Print(T obj);
    }
}
