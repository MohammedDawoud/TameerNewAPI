using System.Globalization;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using System.Net;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;
using System.Data;
using System.Data.SqlClient;

namespace TaamerProject.Service.Services
{
    public class OutInBoxService : IOutInBoxService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IOutInBoxRepository _OutInBoxRepository;
        private readonly IOutInImagesToRepository _OutInImagesToRepository;
        private readonly IOutInBoxSerialRepository _OutInBoxSerialRepository;


        public OutInBoxService(TaamerProjectContext dataContext, ISystemAction systemAction, IOutInBoxRepository outInBoxRepository, IOutInImagesToRepository outInImagesToRepository,
            IOutInBoxSerialRepository outInBoxSerialRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _OutInBoxRepository = outInBoxRepository;
            _OutInImagesToRepository = outInImagesToRepository;
            _OutInBoxSerialRepository = outInBoxSerialRepository;
        }

        public async Task<IEnumerable<OutInBoxVM>> GetAllOutInbox(int Type, int BranchId)
        {
            var OutInBoxes = await _OutInBoxRepository.GetAllOutInbox(Type, BranchId);
            return OutInBoxes;
        }
        public async Task<IEnumerable<OutInBoxVM>> GetAllOutInboxSearch(int BranchId)
        {
            var OutInBoxes = await _OutInBoxRepository.GetAllOutInboxSearch(BranchId);
            return OutInBoxes;
        }
        public Task<OutInBoxVM> GetOutInboxById(int OutInBoxId)
        {
            var OutInBoxes = _OutInBoxRepository.GetOutInboxById(OutInBoxId);
            return OutInBoxes;
        }
        public async Task<IEnumerable<OutInBoxVM>> GetOutInboxFiles(int? Type, int? OutInType, int? ArchiveFileId, int BranchId)
        {
            var OutInBoxes =await _OutInBoxRepository.GetOutInboxFiles(Type, OutInType, ArchiveFileId, BranchId);
            return OutInBoxes;
        }
        public async Task<IEnumerable<OutInBoxVM>> GetOutInboxProjectFiles(int Type, int? ProjectId, int BranchId)
        {
            var OutInBoxes =await _OutInBoxRepository.GetOutInboxProjectFiles(Type, ProjectId, BranchId);
            return OutInBoxes;
        }
        public GeneralMessage SaveOutInbox(OutInBox OutInbox, int UserId, int BranchId)
        {
            try
            {
                var Message = "";
                if (OutInbox.OutInBoxId == 0)
                {
                    OutInbox.AddUser = UserId;
                    OutInbox.BranchId = BranchId;
                    OutInbox.AddDate = DateTime.Now;
                    _TaamerProContext.OutInBox.Add(OutInbox);
                    var OutInBoxSerial = _TaamerProContext.OutInBoxSerial.Where(x=>x.OutInSerialId==OutInbox.NumberType).FirstOrDefault();
                    if (OutInBoxSerial != null)
                    {
                        OutInBoxSerial.LastNumber += 1;
                    }
                    _TaamerProContext.SaveChanges();

                    if (OutInbox.OutInImagesIds != null)
                    {
                        foreach (var item in OutInbox.OutInImagesIds.ToList())
                        {
                            var OutInImagesTo = new OutInImagesTo();
                            OutInImagesTo.DepartmentId = item;
                            OutInImagesTo.OutInboxId = OutInbox.OutInBoxId; //_OutInBoxRepository.GetMaxId() + 1;
                            OutInImagesTo.BranchId = BranchId;
                            OutInImagesTo.IsDeleted = false;
                            OutInImagesTo.AddUser = UserId;
                            OutInImagesTo.AddDate = DateTime.Now;
                            _TaamerProContext.OutInImagesTo.Add(OutInImagesTo);
                        }
                    }
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة صادر ";
                    _SystemAction.SaveAction("SaveOutInbox", "OutInBoxService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    Message = OutInbox.Type == 1 ? ("تم اضافة صادر برقم : " + OutInbox.Number) : ("تم اضافة وارد برقم : " + OutInbox.Number);
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Message, ReturnedParm = OutInbox.OutInBoxId };
                }
                else
                {
                    var outinbox = _TaamerProContext.OutInBox.Where(x=>x.OutInBoxId==OutInbox.OutInBoxId).FirstOrDefault();
                    outinbox.ArchiveFileId = OutInbox.ArchiveFileId;
                    outinbox.Date = OutInbox.Date;
                    outinbox.HijriDate = OutInbox.HijriDate;
                    outinbox.InnerId = OutInbox.InnerId;
                    outinbox.OutInType = OutInbox.OutInType;
                    outinbox.Priority = OutInbox.Priority;
                    outinbox.ProjectId = OutInbox.ProjectId;
                    outinbox.RelatedToId = OutInbox.RelatedToId;
                    outinbox.SideFromId = OutInbox.SideFromId;
                    outinbox.SideToId = OutInbox.SideToId;
                    outinbox.Topic = OutInbox.Topic;
                    outinbox.TypeId = OutInbox.TypeId;
                    outinbox.UpdateUser = UserId;
                    outinbox.UpdateDate = DateTime.Now;
                    outinbox.Number = OutInbox.Number;
                    outinbox.Code = OutInbox.Code;

                    if (OutInbox.OutInImagesIds != null)
                    {
                        var existImage = _TaamerProContext.OutInImagesTo.Where(s => s.IsDeleted == false && s.OutInboxId == OutInbox.OutInBoxId);
                        _TaamerProContext.OutInImagesTo.RemoveRange(existImage);
                        foreach (var item in OutInbox.OutInImagesIds.ToList())
                        {
                            var OutInImagesTo = new OutInImagesTo();
                            OutInImagesTo.DepartmentId = item;
                            OutInImagesTo.OutInboxId = outinbox.OutInBoxId; //_OutInBoxRepository.GetMaxId() + 1;
                            OutInImagesTo.BranchId = BranchId;
                            OutInImagesTo.AddUser = UserId;
                            OutInImagesTo.AddDate = DateTime.Now;
                            _TaamerProContext.OutInImagesTo.Add(OutInImagesTo);
                        }
                    }
                    Message = outinbox.Type == 1 ? ("تم تعدبل صادر رقم : " + outinbox.Number) : ("تم تعدبل وارد رقم : " + outinbox.Number);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote2 = " تعديل صادر رقم " + OutInbox;
                    _SystemAction.SaveAction("SaveOutInbox", "OutInBoxService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate2, UserId, BranchId, ActionNote2, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Message, ReturnedParm = OutInbox.OutInBoxId };
                }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الصادر";
                _SystemAction.SaveAction("SaveOutInbox", "OutInBoxService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage SaveOutInboxattach(int OutInboxid, string attachurl,string con)
        {
            try
            {
                OutInBox outin = _TaamerProContext.OutInBox.Where(x => x.OutInBoxId == OutInboxid).FirstOrDefault();
                if (outin != null)
                {
                    //outin.AttachmentUrl = attachurl;
                    //_TaamerProContext.SaveChanges();
                    //return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

                    string connectionString = con;
                    string query = "UPDATE Contac_OutInBox SET AttachmentUrl = @AttachUrl WHERE OutInBoxId = @Id";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            // Add parameters to avoid SQL injection
                            command.Parameters.Add(new SqlParameter("@AttachUrl", SqlDbType.NVarChar)
                            {
                                Value = attachurl
                            });

                            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)
                            {
                                Value = OutInboxid
                            });

                            connection.Open();
                            int rowsAffected = command.ExecuteNonQuery();
                            connection.Close();
                            Console.WriteLine($"Rows affected: {rowsAffected}");
                        }
                    }
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

                }

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
            catch (Exception ex) 
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = ex.Message };


            }
        }
        public GeneralMessage DeleteOutInBox(int OutInBoxId, int UserId, int BranchId)
        {
            try
            {
                var OutInBoxes = _OutInBoxRepository.GetOutInboxByRelatedToId(OutInBoxId).Result;
                if (OutInBoxes != null && OutInBoxes.RelatedToId != null)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " فشل في حذف الصادر رقم " + OutInBoxId; ;
                    _SystemAction.SaveAction("DeleteOutInBox", "OutInBoxService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.IsLinkedToAnotherProcess };
                }
                else
                {
                    OutInBox OutInBox = _TaamerProContext.OutInBox.Where(x => x.OutInBoxId == OutInBoxId).FirstOrDefault();
                    OutInBox.IsDeleted = true;
                    OutInBox.DeleteDate = DateTime.Now;
                    OutInBox.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف الصادر رقم " + OutInBoxId;
                    _SystemAction.SaveAction("DeleteOutInBox", "OutInBoxService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
                };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف الصادر/وارد رقم " + OutInBoxId; ;
                _SystemAction.SaveAction("DeleteOutInBox", "OutInBoxService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public IEnumerable<object> FillOutboxTypeSelect(int? param, int BranchId)
        {
            return _OutInBoxRepository.GetAllOutInboxList(1, BranchId).Result.Where(t => t.OutInType == param || param == null).Select(s => new {
                Id = s.OutInBoxId,
                Name = s.Number
            });
        }
        public IEnumerable<object> FillInboxTypeSelect(int? param, int BranchId)
        {
            return _OutInBoxRepository.GetAllOutInboxList(2, BranchId).Result.Where(t => t.OutInType == param || param == null).Select(s => new {
                Id = s.OutInBoxId,
                Name = s.Number
            });
        }
        public async Task<IEnumerable<OutInBoxVM>> SearchOutbox(OutInBoxVM OutInBoxesSearch, string DateFrom, string DateTo, int BranchId)
        {
            var OutInBoxes =await _OutInBoxRepository.SearchOutbox(OutInBoxesSearch, DateFrom, DateTo, BranchId);
            return OutInBoxes;
        }
        public async Task<IEnumerable<OutInBoxVM>> SearchOutInbox(OutInBoxVM OutInBoxesSearch, string DateFrom, string DateTo, int BranchId)
        {
            var OutInBoxes = await _OutInBoxRepository.SearchOutInbox(OutInBoxesSearch, DateFrom, DateTo, BranchId);
            return OutInBoxes;
        }
        public async Task<IEnumerable<OutInBoxVM>> GetAllOutboxByDateSearch(string DateFrom, string DateTo, int BranchId, int Type)
        {
            var OutInBoxes = await _OutInBoxRepository.GetAllOutboxByDateSearch(DateFrom, DateTo, BranchId, Type);
            return OutInBoxes;
        }
        public async Task<IEnumerable<OutInBoxVM>> GetAllOutInboxByDateSearch(string DateFrom, string DateTo, int BranchId)
        {
            var OutInBoxes =await _OutInBoxRepository.GetAllOutInboxByDateSearch(DateFrom, DateTo, BranchId);
            return OutInBoxes;
        }
        public List<string> GetAllDeptsByOutInBoxId(int OutInboxId, int Type)
        {
            var Ids = _TaamerProContext.OutInImagesTo.Where(s => s.IsDeleted == false && s.OutInboxId == OutInboxId && s.Department.Type == Type).Select(s => s.DepartmentId.ToString()).ToList();
            return Ids;
        }
    }
}
