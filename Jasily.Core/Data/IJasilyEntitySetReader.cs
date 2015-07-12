using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Data
{
    public interface IJasilyEntitySetReader<TEntity, in TKey>
        where TEntity : class, IJasilyEntity<TKey>
    {
        /// <summary>
        /// return null if not found.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> FindAsync(TKey id);

        /// <summary>
        /// list some item from all entities
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> ListAsync(int skip, int take);
    }
}