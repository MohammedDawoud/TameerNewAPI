using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;

namespace TaamerProject.Models.Pagination
{
    public class GeneratePagination<T> : List<T>
    {
        public MetaData MetaData { get; set; }
        public GeneratePagination(List<T> items, int count, int pageNumber, int pageSize)
        {
            MetaData = new MetaData
            {
                TotalCount = count,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };
            AddRange(items);
        }
     

        public static GeneratePagination<T> ToPagedList(IEnumerable<T> source, int pageNumber, int pageSize)
        {


            var count = source.Count();
            var items = source
            .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize).ToList();
            return new GeneratePagination<T>(items, count, pageNumber, pageSize);
        }



    }

}
