namespace Jasily.Data
{
    public interface IJasilyEntitySetProvider<TEntity, TKey> : IJasilyEntitySetReader<TEntity, TKey>, IJasilyEntitySetWriter<TEntity, TKey>
        where TEntity : class, IJasilyEntity<TKey>
    {

    }
}