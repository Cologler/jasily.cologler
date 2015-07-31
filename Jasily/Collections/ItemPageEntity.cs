using System;
using System.Threading.Tasks;

namespace Jasily.Collections
{
    public struct ItemPageEntity<T> : IItemPage<T>
        where T : class
    {
        public ItemPageEntity(int pageSize, Func<Task<bool>> hasLast, Func<Task<bool>> hasNext, Func<Task<T>> getNext)
            : this()
        {
            this.PageSize = pageSize;
            this.HasLastFunc = hasLast;
            this.HasNextFunc = hasNext;
            this.GetNextFunc = getNext;
        }
        public ItemPageEntity(int pageSize, bool hasLast, bool hasNext, Func<Task<T>> getNext)
             : this()
        {
            this.PageSize = pageSize;
            this.HasLast = hasLast;
            this.HasNext = hasNext;
            this.GetNextFunc = getNext;
        }
        public ItemPageEntity(int pageSize, bool hasLast, bool hasNext, T nextPage)
             : this()
        {
            this.PageSize = pageSize;
            this.HasLast = hasLast;
            this.HasNext = hasNext;
            this.NextPage = nextPage;
        }

        public int PageSize { get; set; }

        public bool? HasLast { get; set; }

        public Func<Task<bool>> HasLastFunc { get; set; }

        public bool? HasNext { get; set; }

        public Func<Task<bool>> HasNextFunc { get; set; }

        public T NextPage { get; set; }

        public Func<Task<T>> GetNextFunc { get; set; }

        public async Task<bool> HasLastAsync()
        {
            if (this.HasLast.HasValue)
                return this.HasLast.Value;

            if (this.HasLastFunc == null)
                throw new NotSupportedException();

            return await this.HasLastFunc();
        }

        public async Task<bool> HasNextAsync()
        {
            if (this.HasNext.HasValue)
                return this.HasNext.Value;

            if (this.HasNextFunc == null)
                throw new NotSupportedException();

            return await this.HasNextFunc();
        }

        public async Task<T> GetNextPageAsync()
        {
            if (this.NextPage != null)
                return this.NextPage;

            if (this.GetNextFunc == null)
                throw new NotSupportedException();

            return await this.GetNextFunc();
        }
    }
}
