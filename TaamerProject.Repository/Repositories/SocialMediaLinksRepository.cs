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
    public class SocialMediaLinksRepository : ISocialMediaLinksRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public SocialMediaLinksRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task< SocialMediaLinksVM> GetAllSocialMediaLinks()
        {
            var Links = _TaamerProContext.SocialMediaLinks.Where(s => s.IsDeleted == false).Select(x => new SocialMediaLinksVM
            {
               LinksId=x.LinksId,
               FaceBookLink =x.FaceBookLink ??"#",
               GooglePlusLink =x.GooglePlusLink ??"#",
               InstagramLink =x.InstagramLink??"#",
               LinkedInLink =x.LinkedInLink??"#",
               SnapchatLink =x.SnapchatLink??"#",
               TwitterLink =x.TwitterLink??"#",
            }).FirstOrDefault();
            return Links;
        }

    }
}
