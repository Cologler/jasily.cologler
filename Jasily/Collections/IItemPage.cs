using System.Threading.Tasks;

namespace Jasily.Collections
{
    public interface IItemPage : IDataPage
    {
        Task<bool> HasLastAsync();

        Task<bool> HasNextAsync();
    }

    public interface IItemPage<T> : IItemPage
    {
        Task<T> GetNextPageAsync();
    }
}
