
using System.Globalization;
using System.Net;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class FollowProjService : IFollowProjService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IFollowProjRepository _FollowProjRepository;



        public FollowProjService(TaamerProjectContext dataContext, ISystemAction systemAction, IFollowProjRepository followProjRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _FollowProjRepository = followProjRepository;
        }

        public async Task<IEnumerable<FollowProjVM>> GetAllFollowProj()
        {
            var FollowProj =await _FollowProjRepository.GetAllFollowProj();
            return FollowProj;
        }
        public async Task<IEnumerable<FollowProjVM>> GetAllFollowProjById(int FollowProjId)
        {
            var FollowProj = await _FollowProjRepository.GetAllFollowProjById(FollowProjId);
            return FollowProj;
        }
        public async Task<IEnumerable<FollowProjVM>> GetAllFollowProjByProId(int ProjectId)
        {
            var FollowProj = await _FollowProjRepository.GetAllFollowProjByProId(ProjectId);
            return FollowProj;
        }
        public GeneralMessage SaveFollowProj(List<FollowProj> followProj, int UserId, int BranchId)
        {
            try
            {
                var followProjBefore = _FollowProjRepository.GetMatching(s => s.ProjectId == followProj[0].ProjectId).ToList();
                if (followProjBefore.Count() != 0)
                {
                    _TaamerProContext.FollowProj.RemoveRange(followProjBefore.ToList());
                }
                foreach (FollowProj foProj in followProj)
                {
                    if (foProj.FollowProjId == 0)
                    {
                        foProj.ProjectId = foProj.ProjectId;
                        foProj.EmpId = foProj.EmpId;
                        foProj.TimeNo = foProj.TimeNo;
                        foProj.TimeType = foProj.TimeType;
                        foProj.EmpRate = foProj.EmpRate;
                        foProj.Amount = foProj.Amount;
                        foProj.ExpectedCost = foProj.ExpectedCost;
                        foProj.ConfirmRate = foProj.ConfirmRate;
                        foProj.AddUser = UserId;
                        foProj.AddDate = DateTime.Now;
                        _TaamerProContext.FollowProj.Add(foProj);
                    }
                    else
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote2 = "فشل في حفظ توزيع نسب المشروع";
                        _SystemAction.SaveAction("SaveFollowProj", "FollowProjService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                    }

                    _TaamerProContext.SaveChanges();
                }
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "اضافة توزيع نسب المشروع";
                _SystemAction.SaveAction("SaveFollowProj", "FollowProjService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ توزيع نسب المشروع";
                _SystemAction.SaveAction("SaveFollowProj", "FollowProjService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteFollowProj(int FollowProjId, int UserId, int BranchId)
        {
            try
            {
                FollowProj followProj = _FollowProjRepository.GetById(FollowProjId);
                followProj.IsDeleted = true;
                followProj.DeleteDate = DateTime.Now;
                followProj.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }


        public GeneralMessage ConfirmRate(int FollowProjId, bool Status, int UserId, int BranchId)
        {
            try
            {
                FollowProj followProj = _FollowProjRepository.GetById(FollowProjId);
                followProj.ConfirmRate = Status;
                _TaamerProContext.SaveChanges();
                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.Approval_completed_successfully };
            }
            catch (Exception)
            {
                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.Approval_completed_faild };
            }
        }


    }
}
