using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models.Common
{
    public class PagedLists<T>
    {
        public PagedLists(MetaData metaData, List<T> values)
        {
            MetaData = metaData;
            Items = values;
        }
        public MetaData MetaData { get; set; }
        public List<T> Items { get; set; }
    }
}
