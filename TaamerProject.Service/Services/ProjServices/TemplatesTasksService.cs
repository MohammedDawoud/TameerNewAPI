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
    public class TemplatesTasksService :   ITemplatesTasksService
    {
        private readonly ITemplatesTasksRepository _TemplatesTasksRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;


        public TemplatesTasksService(TaamerProjectContext dataContext
            , ISystemAction systemAction, ITemplatesTasksRepository templatesTasksRepository)
        {
            _TemplatesTasksRepository = templatesTasksRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;

        }
        public Task<IEnumerable<TemplatesTasksVM>> GetAllTemplatesTasks( int BranchId)
        {
            var TemplatesTasks = _TemplatesTasksRepository.GetAllTemplatesTasks( BranchId);
            return TemplatesTasks;
        }
        public Task<IEnumerable<TemplatesTasksVM>> GetAllTemplatesTasksByTemplateId(int TemplateId, int BranchId)
        {
            var TemplatesTasks = _TemplatesTasksRepository.GetAllTemplatesTasksByTemplateId(TemplateId, BranchId);
            return TemplatesTasks;
        }
        public  GeneralMessage SaveTemplatesTasks(TemplatesTasks templatesTasks, int UserId, int BranchId)
        {
            try
            {
                if (templatesTasks.TemplateTaskId == 0)
                {
                    templatesTasks.BranchId = BranchId;
                    templatesTasks.AddDate = DateTime.Now;
                    _TaamerProContext.TemplatesTasks.Add(templatesTasks);
                     _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة نموذج مهمة جديد";
                    _SystemAction.SaveAction("SaveTemplatesTasks", "TemplatesTasksService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    //var templatesTasksUpdated = _TemplatesTasksRepository.GetById(templatesTasks.TemplateTaskId);
                    TemplatesTasks? templatesTasksUpdated =  _TaamerProContext.TemplatesTasks.Where(s => s.TemplateTaskId == templatesTasks.TemplateTaskId).FirstOrDefault();

                    if (templatesTasksUpdated != null)
                    {
                        templatesTasksUpdated.DescriptionAr = templatesTasks.DescriptionAr;
                        templatesTasksUpdated.DescriptionEn = templatesTasks.DescriptionEn;
                        templatesTasksUpdated.UserId = templatesTasks.UserId;
                        templatesTasksUpdated.TimeMinutes = templatesTasks.Remaining = templatesTasks.TimeMinutes;
                        templatesTasksUpdated.Cost = templatesTasks.Cost;
                        templatesTasksUpdated.TimeType = templatesTasks.TimeType;
                        templatesTasksUpdated.IsUrgent = templatesTasks.IsUrgent;
                        templatesTasksUpdated.TaskType = templatesTasks.TaskType;
                        templatesTasksUpdated.UpdateDate = DateTime.Now;

                    }

                     _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل نوذج مهمة رقم " + templatesTasks.TemplateTaskId;
                    _SystemAction.SaveAction("SaveTemplatesTasks", "TemplatesTasksService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ نوذج المهمة";
                _SystemAction.SaveAction("SaveTemplatesTasks", "TemplatesTasksService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage DeleteTemplatestasks(int TemplateTaskId, int UserId, int BranchId)
        {
            try
            {
                // TemplatesTasks TemplatesTasks = _TemplatesTasksRepository.GetById(TemplateTaskId);
                TemplatesTasks? TemplatesTasks = _TaamerProContext.TemplatesTasks.Where(s => s.TemplateTaskId == TemplateTaskId).FirstOrDefault();
                if (TemplatesTasks !=null)
                {
                    TemplatesTasks.IsDeleted = true;
                    TemplatesTasks.DeleteDate = DateTime.Now;
                    _TaamerProContext.SaveChanges();

                   
                }
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف نموذج مهمة رقم " + TemplateTaskId;
                _SystemAction.SaveAction("DeleteTemplatestasks", "TemplatesTasksService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف نموذج مهمة رقم " + TemplateTaskId; ;
                _SystemAction.SaveAction("DeleteTemplatestasks", "TemplatesTasksService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }

       

    }
}