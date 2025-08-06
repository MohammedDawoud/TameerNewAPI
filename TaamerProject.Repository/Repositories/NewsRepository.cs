using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Repository.Repositories
{
    public class NewsRepository : INewsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public NewsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }

        public async Task< IEnumerable<NewsVM>> GetAllNews(string Lang)
        {
            var news = _TaamerProContext.News.Where(s => s.IsDeleted == false).Select(x => new NewsVM
            {
                NewsId=x.NewsId,
                NewsBodyAr = x.NewsBodyAr,
                NewsBodyEn=x.NewsBodyEn,
                NewsTitleAr=x.NewsTitleAr,
                NewsTitleEn=x.NewsTitleEn,
                NewsImg=x.NewsImg,
                NewsTitle= Lang == "ltr" ? x.NewsTitleEn : x.NewsTitleAr,
                NewsBody= Lang =="ltr" ? x.NewsBodyEn : x.NewsBodyAr,


            }).ToList();
            return news;
        }
    }
}
