using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface ISocialMediaLinksService  
    {

        Task<SocialMediaLinksVM> GetAllSocialMediaLinks();
        GeneralMessage SaveSocialMediaLinks(SocialMediaLinks Links, int UserId, int BranchId);


    }
}
