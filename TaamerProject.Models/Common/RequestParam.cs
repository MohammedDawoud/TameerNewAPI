using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models.Common
{
    public class RequestParam<T>
    {
        public int pagesize { get; set; }
        //public int pagecount { get; set; }
        public int pagenumber { get; set; }
        public T Filters { get; set; }
    }

    public class PagingPayload<T>
    {
        public PagingDTO PagingDTO { get; set; }
        public T Filters { get; set; }
    }
    public class SearchFilter
    {
        public string SearchText { get; set; } = "";
    }

    public class PagingDTO
    {
        public PagingDTO()
        {
            PageCount = 10;
            CurrentPageNumber = 0;
        }

        public int TotalCount { get; set; }
        public int PageCount { get; set; }
        public int CurrentPageNumber { get; set; }
        /// <summary>
        /// True in case a filter is updated so the Total Count in the paging control need to be filled to be updated as well
        /// </summary>
        public bool IsFilterUpdated { get; set; }
        public int Skip
        {
            get
            {
                if (CurrentPageNumber == 0)
                {
                    return 0;
                }
                return (PageCount * (CurrentPageNumber - 1)) + AddedItemsCount;
            }
        }
        public int Take
        {
            get { return PageCount; }
        }

        public bool DevMode { get; set; }
        public int AddedItemsCount { get; set; }
    }
}
