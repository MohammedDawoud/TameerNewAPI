using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface INewsService
    {
        Task<IEnumerable<NewsVM>> GetAllNews(string Lang);

        GeneralMessage SaveNews(News news, int UserId, int BranchId);
        GeneralMessage DeleteNews(int NewsId, int UserId, int BranchId);
    }
}
