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
using System.Text.RegularExpressions;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class GroupsService :  IGroupsService
    {
        private readonly IGroupsRepository _GroupsRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;

        public GroupsService(IGroupsRepository GroupsRepository,
            TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _GroupsRepository = GroupsRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;

        }
        public Task<IEnumerable<GroupsVM>> GetAllGroups()
        {
            var Groups = _GroupsRepository.GetAllGroups();
            return Groups;
        }
        public Task<IEnumerable<GroupsVM>> GetAllGroups_S(string SearchText)
        {
            var Groups = _GroupsRepository.GetAllGroups_S(SearchText);
            return Groups;
        }
        public GeneralMessage SaveGroups(Groups groups, int UserId, int BranchId)
        {
            try
            {
               // var codeExist = _GroupsRepository.GetMatching(s => s.IsDeleted == false && s.GroupId != groups.GroupId && s.NameAr == groups.NameAr).FirstOrDefault();
                var codeExist = _TaamerProContext.Groups.Where(s => s.IsDeleted == false && s.GroupId != groups.GroupId && s.NameAr == groups.NameAr).FirstOrDefault();

                if (codeExist != null)
                {
              
                }
                if (groups.GroupId == 0)
                {
                    groups.BranchId = BranchId;
                    groups.AddUser = UserId;
                    groups.AddDate = DateTime.Now;
                    _TaamerProContext.Groups.Add(groups);
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة مجموعة جديد";
                    _SystemAction.SaveAction("SaveGroups", "GroupsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =Resources.General_SavedSuccessfully };
                }
                else
                {
                    // var GroupsUpdated = _GroupsRepository.GetById(groups.GroupId);
                    Groups? GroupsUpdated = _TaamerProContext.Groups.Where(s => s.GroupId == groups.GroupId).FirstOrDefault();
                    if (GroupsUpdated != null)
                    {
                        if (GroupsUpdated != null)
                        {
                            GroupsUpdated.NameAr = groups.NameAr;
                            GroupsUpdated.NameEn = groups.NameEn;
                            GroupsUpdated.Notes = groups.Notes;
                            //GroupsUpdated.BranchId = groups.BranchId;
                            GroupsUpdated.UpdateUser = UserId;
                            GroupsUpdated.UpdateDate = DateTime.Now;
                        }
                        _TaamerProContext.SaveChanges();

                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = " تعديل مجموعة رقم " + groups.GroupId;
                        _SystemAction.SaveAction("SaveGroups", "GroupsService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------

                    }


                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =Resources.General_EditedSuccessfully };
                }

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ المجموعة";
                _SystemAction.SaveAction("SaveGroups", "GroupsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteGroups(int GroupId, int UserId,int BranchId)
        {
            try
            {
                //Groups groups = _GroupsRepository.GetById(GroupId);
                Groups? groups = _TaamerProContext.Groups.Where(s => s.GroupId == GroupId).FirstOrDefault();
                if (groups != null)
                {
                    groups.IsDeleted = true;
                    groups.DeleteDate = DateTime.Now;
                    groups.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف مجموعة رقم " + GroupId;
                    _SystemAction.SaveAction("DeleteGroups", "GroupsService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }
            
               
                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف مجموعة رقم " + GroupId; ;
                _SystemAction.SaveAction("DeleteGroups", "GroupsService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =Resources.General_DeletedFailed };
            }
        }

        public GroupsVM GetGroupById(int GroupId)
        {
           // var group = _GroupsRepository.GetById(GroupId);
            Groups? group = _TaamerProContext.Groups.Where(s => s.GroupId == GroupId).FirstOrDefault();
            return new GroupsVM { GroupId = group.GroupId, NameAr = group.NameAr, NameEn = group.NameEn };
        }
      

    }
}
