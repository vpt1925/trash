using System;
using System.Collections.Generic;

namespace QLCHTH_65133373.Models.ViewModels
{
    public class PagedList<T>
    {
        public List<T> Items { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public PagedList(List<T> items, int pageIndex, int pageSize, int totalCount)
        {
            Items = items;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        public int StartPage
        {
            get
            {
                int start = PageIndex - 2;
                if (start < 1) start = 1;
                if (TotalPages > 5 && start > TotalPages - 4)
                    start = TotalPages - 4;
                return start;
            }
        }

        public int EndPage
        {
            get
            {
                int end = StartPage + 4;
                if (end > TotalPages) end = TotalPages;
                return end;
            }
        }
    }
}
