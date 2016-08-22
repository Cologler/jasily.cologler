using System.Diagnostics;

namespace Jasily.Data
{
    public interface IJasilyEntity<TKey> : IPrintable
    {
        /// <summary>
        /// key of entity
        /// </summary>
        TKey Id { get; set; }
    }
}