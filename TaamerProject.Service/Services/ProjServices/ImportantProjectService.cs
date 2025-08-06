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
    public class ImportantProjectService : IImportantProjectService
    {

        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IImportantProjectRepository _importantProjectRepository;



        public ImportantProjectService(TaamerProjectContext dataContext, ISystemAction systemAction, IImportantProjectRepository importantProjectRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _importantProjectRepository = importantProjectRepository;
        }
        public async Task< IEnumerable<ImportantProjectVM>> GetImportantProjects(int projectid, int userid)
        {
            var important =await _importantProjectRepository.GetImportantProjects(projectid, userid);
            return important;
        }


        public GeneralMessage ChangeFlag(ImportantProject import, int UserId, int BranchId)
        {
            try
            {
                if (import.ImportantProId == 0)
                {
                    import.AddDate = DateTime.Now;
                    import.AddUser = UserId;
                    import.UserId = UserId;
                    _TaamerProContext.ImportantProject.Add(import);
                }
                else
                {
                    var impupdate = _TaamerProContext.ImportantProject.Where(s => s.IsDeleted == false && s.ImportantProId == import.ImportantProId && s.ProjectId == import.ProjectId && s.UserId == UserId).FirstOrDefault();
                    if(impupdate != null)
                    {
                        impupdate.Flag = import.Flag;
                        impupdate.UpdateDate = DateTime.Now;
                        impupdate.UpdateUser = UserId;
                    }
                }



                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "تغيير اهمية المشروع ";
               _SystemAction.SaveAction("UpdateImportant", "ProjectService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase =Resources.relevance_has_been_changed_successfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تغيير اهمية المشروع";
                _SystemAction.SaveAction("UpdateImportant", "ProjectService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Failed_change_importance_project };
            }
        }



        public GeneralMessage ChangeImportant(ImportantProject import, int UserId, int BranchId)
        {
            try
            {
                if (import.ImportantProId == 0)
                {
                    import.AddDate = DateTime.Now;
                    import.AddUser = UserId;
                    import.UserId = UserId;
                    _TaamerProContext.ImportantProject.Add(import);
                }
                else
                {
                    var impupdate = _TaamerProContext.ImportantProject.Where(s => s.IsDeleted == false && s.ImportantProId == import.ImportantProId && s.ProjectId == import.ProjectId && s.UserId == UserId).FirstOrDefault();
                    if (impupdate != null)
                    {
                        impupdate.IsImportant = import.IsImportant;
                        impupdate.UpdateDate = DateTime.Now;
                        impupdate.UpdateUser = UserId;
                    }
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = Resources.relevance_has_been_changed_successfully;
                _SystemAction.SaveAction("UpdateImportant", "ProjectService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.relevance_has_been_changed_successfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تغيير اهمية المشروع";
                _SystemAction.SaveAction("UpdateImportant", "ProjectService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Failed_change_importance_project };
            }
        }


    }
}
