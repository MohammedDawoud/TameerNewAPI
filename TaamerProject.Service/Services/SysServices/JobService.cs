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
    public class JobService : IJobService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IJobRepository _JobRepository;



        public JobService(TaamerProjectContext dataContext, ISystemAction systemAction, IJobRepository jobRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _JobRepository = jobRepository;
        }
        public async Task<IEnumerable<JobVM>> GetAllJobs(string SearchText)
        {
            var jobs =await _JobRepository.GetAllJobs(SearchText);
            return jobs;
        }
        public GeneralMessage SaveJob(Job job, int UserId, int BranchId)
        {
            try
            {
                var codeExist = _JobRepository.GetMatching(s => s.IsDeleted == false && s.JobId != job.JobId && s.JobCode == job.JobCode).FirstOrDefault();
                if (codeExist != null)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ الوظيفة" + job.JobNameAr; ;
                   _SystemAction.SaveAction("SaveJob", "JobService", 1, Resources.TheCodeAlreadyExists, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.TheCodeAlreadyExists };
                }
                if (job.JobId == 0)
                {
                    job.AddUser = UserId;
                    job.AddDate = DateTime.Now;
                    job.JobCode = (_TaamerProContext.Job.AsQueryable().Count() + 1).ToString();
                    _TaamerProContext.Job.Add(job);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة وظيفة جديد" +job.JobNameAr;
                    _SystemAction.SaveAction("SaveJob", "JobService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

                }
                else
                {
                    var JobUpdated = _JobRepository.GetById(job.JobId);
                    if (JobUpdated != null)
                    {
                        //JobUpdated.JobCode = job.JobCode;
                        JobUpdated.JobNameAr = job.JobNameAr;
                        JobUpdated.JobNameEn = job.JobNameEn;
                        JobUpdated.Notes = job.Notes;
                        JobUpdated.UpdateUser = UserId;
                        JobUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل وظيفة رقم " + JobUpdated.JobNameAr;
                    _SystemAction.SaveAction("SaveJob", "JobService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully };

                }
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الوظيفة" +job.JobNameAr;
                _SystemAction.SaveAction("SaveJob", "JobService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteJob(int JobId, int UserId, int BranchId)
        {
            try
            {
                Job job = _JobRepository.GetById(JobId);
                job.IsDeleted = true;
                job.DeleteDate = DateTime.Now;
                job.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف وظيفة  " + job.JobNameAr;
                _SystemAction.SaveAction("DeleteClause", "Acc_ClausesService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الوظيفة" +JobId;
                _SystemAction.SaveAction("SaveJob", "JobService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public IEnumerable<object> FillJobSelect(string SearchText = "")
        {
            return _JobRepository.GetAllJobs(SearchText).Result.Select(s => new
            {
                Id = s.JobId,
                Name = s.JobNameAr
            });
        }
    }
}
