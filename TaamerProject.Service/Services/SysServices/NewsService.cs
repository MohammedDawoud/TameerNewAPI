using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using System.Net;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class NewsService : INewsService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly INewsRepository _newsRepository;



        public NewsService(TaamerProjectContext dataContext, ISystemAction systemAction, INewsRepository newsRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _newsRepository = newsRepository;

        }
        public async Task< IEnumerable<NewsVM>> GetAllNews(string Lang)
        {
            var news = await _newsRepository.GetAllNews(Lang);


            return news;

        }



        public GeneralMessage SaveNews(News news, int UserId, int BranchId)
        {
            try
            {
                if (news.NewsId == 0)
                {
                    news.AddUser = UserId;
                    news.AddDate = DateTime.Now;
                    _TaamerProContext.News.Add(news);

                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة اخبار جديده";
                   _SystemAction.SaveAction("SaveNews", "NewsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    var NewsUpdate = _TaamerProContext.News.Where(x=>x.NewsId==news.NewsId).FirstOrDefault();
                    if (NewsUpdate != null)
                    {
                        NewsUpdate.NewsBodyAr = news.NewsBodyAr;
                        NewsUpdate.NewsBodyEn = news.NewsBodyEn;
                        NewsUpdate.NewsTitleAr = news.NewsTitleAr;
                        NewsUpdate.NewsTitleEn = news.NewsTitleEn;
                        if (news.NewsImg != null)
                        {
                            NewsUpdate.NewsImg = news.NewsImg;
                        }

                        NewsUpdate.UpdateUser = UserId;
                        NewsUpdate.UpdateDate = DateTime.Now;

                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل  اخبار ";
                    _SystemAction.SaveAction("SaveNews", "NewsService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
            }
            catch (Exception)
            { //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الاخبار";
                _SystemAction.SaveAction("SaveNews", "NewsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }



        public GeneralMessage DeleteNews(int NewsId, int UserId, int BranchId)
        {
            try
            {

                News newa = _TaamerProContext.News.Where(x=>x.NewsId==NewsId).FirstOrDefault();
                newa.IsDeleted = true;
                newa.DeleteDate = DateTime.Now;
                newa.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };

            }
            catch (Exception)
            {
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
    }
}
