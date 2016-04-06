using System;

namespace Jasily.Data
{
    public interface IJasilyEntity<TKey> : IPrint
    {
        /// <summary>
        /// key of entity
        /// </summary>
        TKey Id { get; set; }
    }
}