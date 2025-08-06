using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{

    public class NewsVM
    {

        public int NewsId { get; set; }

        public string? NewsTitleAr { get; set; }
        public string? NewsBodyAr { get; set; }
        public string? NewsTitleEn { get; set; }
        public string? NewsBodyEn { get; set; }
        public string? NewsImg { get; set; }

        public string? NewsTitle { get; set; }
        public string? NewsBody { get; set; }

    }
}
