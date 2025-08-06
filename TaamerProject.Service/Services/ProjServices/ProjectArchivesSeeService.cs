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
using TaamerProject.Service.Generic;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class ProjectArchivesSeeService :  IProjectArchivesSeeService
    {
        private readonly IProjectArchivesSeeRepository _ProjectArchivesSeeRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;

        public ProjectArchivesSeeService(IProjectArchivesSeeRepository ProjectArchivesSeeRepository,TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _ProjectArchivesSeeRepository = ProjectArchivesSeeRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;

        }
        public Task<IEnumerable<ProjectArchivesSeeVM>> GetProjectArchSee()
        {
            var projects = _ProjectArchivesSeeRepository.GetProjectArchSee();
            return projects;
        }
        public Task<IEnumerable<ProjectArchivesSeeVM>> GetProjectArchSee_Phases()
        {
            var projects = _ProjectArchivesSeeRepository.GetProjectArchSee_Phases();
            return projects;
        }
        public Task<IEnumerable<ProjectArchivesSeeVM>> GetProjectArchSeeParm(int ProArchReID, int UserId)
        {
            var projects = _ProjectArchivesSeeRepository.GetProjectArchSeeParm(ProArchReID, UserId);
            return projects;
        }
        public Task<IEnumerable<ProjectArchivesSeeVM>> GetProjectArchSeeParm_Phases(int ProArchReID, int UserId)
        {
            var projects = _ProjectArchivesSeeRepository.GetProjectArchSeeParm_Phases(ProArchReID, UserId);
            return projects;
        }

        public GeneralMessage InsertProjectArchSee(int ProArchReID, int UserId, int BranchId)
        {
            try
            {
                ProjectArchivesSee ProSee = new ProjectArchivesSee();
                ProSee.ProArchReID = ProArchReID;
                ProSee.UserId = UserId;
                ProSee.Status = true;
                ProSee.See_TypeID = 1;

                _TaamerProContext.ProjectArchivesSee.Add(ProSee);
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "ارشفة المشروع رقم  " + ProArchReID;
                _SystemAction.SaveAction("InsertProjectArchSee", "ProjectArchivesSeeService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                
                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =  Resources.General_SavedSuccessfully };

                }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ ارشفة مشروع";
                _SystemAction.SaveAction("InsertProjectArchSee", "ProjectArchivesSeeService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
                }
        }

        public GeneralMessage InsertProjectArchSee_Phases(int ProArchReID, int UserId, int BranchId)
        {
            try
            {
                ProjectArchivesSee ProSee = new ProjectArchivesSee();
                ProSee.ProArchReID = ProArchReID;
                ProSee.UserId = UserId;
                ProSee.Status = true;
                ProSee.See_TypeID = 2;
                _TaamerProContext.ProjectArchivesSee.Add(ProSee);
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "مرحلة ارشفة مشروع ";
                _SystemAction.SaveAction("InsertProjectArchSee_Phases", "ProjectArchivesSeeService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };

                }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ مرحلةارشفة مشروع";
                _SystemAction.SaveAction("InsertProjectArchSee_Phases", "ProjectArchivesSeeService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
                }
        }
      

    }
}
