using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaamerProject.Models
{
    public class News : Auditable
    {

        public int NewsId { get; set; }

        public string? NewsTitleAr { get; set; }
        public string? NewsBodyAr { get; set; }
        public string? NewsTitleEn { get; set; }
        public string? NewsBodyEn { get; set; }
        public string? NewsImg { get; set; }



    }
}
