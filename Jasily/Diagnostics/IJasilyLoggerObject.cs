namespace Jasily.Diagnostics
{
    public interface IJasilyLoggerObject
    {
        JasilyLogger GetLogger();
    }

    public interface IJasilyLoggerObject<T> : IJasilyLoggerObject
    {
    }
}