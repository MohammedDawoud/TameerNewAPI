using System.Globalization;
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
    public class AttachmentService : IAttachmentService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IAttachmentRepository _AttachmentRepository;

        public AttachmentService(TaamerProjectContext dataContext, ISystemAction systemAction, IAttachmentRepository attachmentRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _AttachmentRepository = attachmentRepository;

        }

        public async Task<IEnumerable<AttachmentVM>> GetAllAttachments(int? EmpId, string SearchText)
        {
            var Attachments =await _AttachmentRepository.GetAllAttachments(EmpId, SearchText);
            return Attachments;
        }
        public GeneralMessage SaveAttachment(Attachment attachment, int UserId, int BranchId)
        {
            try
            {
                if (attachment.AttachmentId == 0)
                {
                    attachment.AddUser = UserId;
                    attachment.AddDate = DateTime.Now;
                    _TaamerProContext.Attachment.Add(attachment);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة ملف جديد";
                    _SystemAction.SaveAction("SaveAttachment", "AttachmentService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

                }
                else
                {
                    var AttachmentUpdated = _TaamerProContext.Attachment.Where(x=>x.AttachmentId==attachment.AttachmentId).FirstOrDefault();
                    if (AttachmentUpdated != null)
                    {
                        AttachmentUpdated.AttachmentName = attachment.AttachmentName;
                        AttachmentUpdated.Date = attachment.Date;
                        AttachmentUpdated.HijriDate = attachment.HijriDate;
                        AttachmentUpdated.AttachmentUrl = attachment.AttachmentUrl;
                        AttachmentUpdated.Notes = attachment.Notes;
                        AttachmentUpdated.EmployeeId = attachment.EmployeeId;
                        AttachmentUpdated.UpdateUser = UserId;
                        AttachmentUpdated.UpdateDate = DateTime.Now;
                    }

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل ملف رقم " + attachment.AttachmentId;
                    _SystemAction.SaveAction("SaveAttachment", "AttachmentService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully,ReturnedParm = attachment.EmployeeId };

                }

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الملف";
                _SystemAction.SaveAction("SaveAttachment", "AttachmentService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };

            }
        }
        public GeneralMessage DeleteAttachment(int AttachmentId, int UserId, int BranchId)
        {
            try
            {
                Attachment attachment = _TaamerProContext.Attachment.Where(x => x.AttachmentId == AttachmentId).FirstOrDefault();
                attachment.IsDeleted = true;
                attachment.DeleteDate = DateTime.Now;
                attachment.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف ملف رقم " + AttachmentId;
                _SystemAction.SaveAction("DeleteAttachment", "AttachmentService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف ملف رقم " + AttachmentId; ;
                _SystemAction.SaveAction("DeleteAttachment", "AttachmentService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };

            }
        }
    }
}
