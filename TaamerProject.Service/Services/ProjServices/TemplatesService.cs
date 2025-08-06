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
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using TaamerProject.Service.Generic;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class TemplatesService :   ITemplatesService
    {
        private readonly ITemplatesRepository _TemplatesRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        public TemplatesService(TaamerProjectContext dataContext
            , ISystemAction systemAction, ITemplatesRepository templatesRepository)
        {
            _TemplatesRepository = templatesRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }
        public Task<IEnumerable<TemplatesVM>> GetAllTemplates(int BranchId)
        {
            var Template = _TemplatesRepository.GetAllTemplates(BranchId);
            return Template;
        }
        public  GeneralMessage SaveTemplates(Templates templates, int UserId, int BranchId)
        {
            try
            {
                if (templates.TemplateId == 0)
                {
                    templates.BranchId = BranchId;
                    templates.AddDate = DateTime.Now;
                    _TaamerProContext.Templates.Add(templates);
                     _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة نموذج جديد";
                    _SystemAction.SaveAction("SaveTemplates", "TemplateService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    //var TemplatesUpdated = _TemplatesRepository.GetById(templates.TemplateId);
                    Templates? TemplatesUpdated =   _TaamerProContext.Templates.Where(s => s.TemplateId == templates.TemplateId).FirstOrDefault();

                    if (TemplatesUpdated != null)
                    {
                        TemplatesUpdated.NameAr = templates.NameAr;
                        TemplatesUpdated.NameEn = templates.NameEn;
                        TemplatesUpdated.Notes = templates.Notes;
                        TemplatesUpdated.UpdateDate = DateTime.Now;
               
                    }
                     _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل نموذج رقم " + templates.TemplateId;
                    _SystemAction.SaveAction("SaveTemplates", "TemplateService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ النموذج";
                _SystemAction.SaveAction("SaveTemplates", "TemplateService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public  GeneralMessage DeleteTemplates( int TemplateId, int UserId, int BranchId)
        {
            try
            {
                //Templates Template = _TemplatesRepository.GetById(TemplateId);
                Templates? Template =   _TaamerProContext.Templates.Where(s => s.TemplateId == TemplateId).FirstOrDefault();
                if (Template != null)
                {
                    Template.IsDeleted = true;
                    Template.DeleteDate = DateTime.Now;
                    _TaamerProContext.SaveChanges();

                }

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف نموذج رقم " + TemplateId;
                _SystemAction.SaveAction("DeleteTemplates", "TemplateService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف نموذج رقم " + TemplateId; ;
                _SystemAction.SaveAction("DeleteTemplates", "TemplateService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
 
        
    }
}
