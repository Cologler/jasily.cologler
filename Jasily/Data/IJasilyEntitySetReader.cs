using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jasily.Data
{
    public interface IJasilyEntitySetReader<TEntity, TKey>
        where TEntity : class, IJasilyEntity<TKey>
    {
        /// <summary>
        /// return null if not found.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> FindAsync(TKey id);

        /// <summary>
        /// return a entities dictionary where match id.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IDictionary<TKey, TEntity>> FindAsync(IEnumerable<TKey> ids);

        /// <summary>
        /// list some item from all entities
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> ListAsync(int skip, int take);

        Task CursorAsync(Action<TEntity> callback);
    }
}