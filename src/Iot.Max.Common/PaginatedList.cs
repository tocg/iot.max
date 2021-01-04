using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Common
{
    public class PaginatedList<T> : List<T>
    {
        public int Offset { get; private set; }
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }

        /// <summary>
        /// For manually paing.
        /// </summary>
        /// <param name="source">The result of current page</param>
        /// <param name="totalCount"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        public PaginatedList(IEnumerable<T> source, int totalCount, int pageIndex, int pageSize)
        {
            Offset = pageIndex * pageSize;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

            this.AddRange(source);
        }

        public PaginatedList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            Offset = pageIndex * pageSize;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = source.Count();
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

            this.AddRange(source.Skip(PageIndex * PageSize).Take(PageSize));
        }

        public PaginatedList(IQueryable<T> source, int pageSize, int? offset = null, int? pageIndex = null)
        {
            if (offset != null)
            {
                Offset = offset.Value;
                PageIndex = (int)Math.Ceiling((decimal)Offset / pageSize);
            }
            else if (pageIndex != null)
            {
                Offset = pageIndex.Value * pageSize;
                PageIndex = pageIndex.Value;
            }
            else
            {
                //offset = null and pageIndex = null
                throw new ArgumentNullException("offset and pageIndex");
            }
            PageSize = pageSize;
            TotalCount = source.Count();
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

            this.AddRange(source.Skip(Offset).Take(PageSize));
        }

        public bool HasPreviousPage
        {
            get
            {
                return (Offset > 0);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (Offset + PageSize < TotalCount);
            }
        }

    }
}
