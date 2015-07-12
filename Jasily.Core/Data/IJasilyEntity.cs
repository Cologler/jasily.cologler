using System.Diagnostics.Contracts;

namespace System.Data
{
    public interface IJasilyEntity<TKey> : IPrint
    {
        /// <summary>
        /// key of entity
        /// </summary>
        TKey Id { get; set; }
    }
}