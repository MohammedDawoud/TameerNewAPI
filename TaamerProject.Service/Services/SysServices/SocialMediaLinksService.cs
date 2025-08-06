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
    public class SocialMediaLinksService :   ISocialMediaLinksService
    {
        private readonly ISocialMediaLinksRepository _socialMediaLinksRepository;
        private readonly ISys_SystemActionsRepository _Sys_SystemActionsRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;

        public SocialMediaLinksService(ISocialMediaLinksRepository socialMediaLinksRepository,
           ISys_SystemActionsRepository sys_SystemActionsRepository,
            TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _socialMediaLinksRepository = socialMediaLinksRepository;
            _Sys_SystemActionsRepository = sys_SystemActionsRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }

        public Task<SocialMediaLinksVM> GetAllSocialMediaLinks()
        {
            var Links = _socialMediaLinksRepository.GetAllSocialMediaLinks();

            return Links;

        }


        public GeneralMessage SaveSocialMediaLinks(SocialMediaLinks Links, int UserId, int BranchId)
        {
            try
            {
                if(Links != null) { 
                if (Links.LinksId == 0)
                {
                    Links.AddUser = UserId;
                    Links.AddDate = DateTime.Now;
                    _TaamerProContext.SocialMediaLinks.Add(Links);

                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة لينكات جديده";
                    _SystemAction.SaveAction("SaveSocialMediaLinks", "SocialMediaLinksService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                        //var LinksUpdate = _socialMediaLinksRepository.GetById(Links.LinksId);
                        SocialMediaLinks? LinksUpdate =   _TaamerProContext.SocialMediaLinks.Where(s => s.LinksId == Links.LinksId).FirstOrDefault();
                        if (LinksUpdate != null)
                        {
                        LinksUpdate.FaceBookLink = Links.FaceBookLink;
                        LinksUpdate.TwitterLink = Links.TwitterLink;
                        LinksUpdate.LinkedInLink = Links.LinkedInLink;
                        LinksUpdate.InstagramLink = Links.InstagramLink;
                        LinksUpdate.SnapchatLink = Links.SnapchatLink;
                        LinksUpdate.GooglePlusLink = Links.GooglePlusLink;
                        LinksUpdate.UpdateUser = UserId;
                        LinksUpdate.UpdateDate = DateTime.Now;

                        }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل  لينكات سوشيال ميديا ";
                    _SystemAction.SaveAction("SaveSocialMediaLinks", "SocialMediaLinksService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
            }
                return new GeneralMessage {StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.NoDataFound };

            }
            catch (Exception)
            { //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ اللينكات";
                _SystemAction.SaveAction("SaveSocialMediaLinks", "SocialMediaLinksService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }
        }



    

    }
}
