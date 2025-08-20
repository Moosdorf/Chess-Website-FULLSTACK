using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.HelperMethods
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalItems { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalItems = count;
            this.AddRange(items);
        }

        public bool HasPreviousPage => PageIndex > 0;

        public bool HasNextPage => PageIndex+1 < TotalPages;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int totalItem, int pageIndex, int pageSize)
        {
            var items = await source.ToListAsync();
            return new PaginatedList<T>(items, totalItem, pageIndex, pageSize);
        }
    }

}
