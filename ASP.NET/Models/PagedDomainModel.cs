using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortfolioWebApi.Models
{
    public class Paged<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public List<T> PagedItems { get; set; }

        public Paged(List<T> data, int page, int pageSize, int totalCount)
        {
            PageIndex = page;
            PageSize = pageSize;
            PagedItems = data;

            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
        }

        public bool HasPreviousPage => PageIndex > 0;
        public bool HasNextPage => PageIndex + 1 < TotalPages;
    }
}