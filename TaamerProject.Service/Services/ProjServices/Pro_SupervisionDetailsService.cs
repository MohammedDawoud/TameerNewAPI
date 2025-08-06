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
using System.ComponentModel.Design;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class Pro_SupervisionDetailsService :  IPro_SupervisionDetailsService
    {
        private readonly IPro_SupervisionDetailsRepository _Pro_SupervisionDetailsRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;

        public Pro_SupervisionDetailsService(IPro_SupervisionDetailsRepository Pro_SupervisionDetailsRepository,TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _Pro_SupervisionDetailsRepository = Pro_SupervisionDetailsRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }

        public Task<IEnumerable<Pro_SupervisionDetailsVM>> GetAllSupervisionDetails(string SearchText)
        {
            var SupervisionDetails = _Pro_SupervisionDetailsRepository.GetAllSupervisionDetails(SearchText);
            return SupervisionDetails;
        }
        public Task<IEnumerable<Pro_SupervisionDetailsVM>> GetAllSupervisionDetailsBySuperId(int? SupervisionId)
        {
            var SupervisionDetails = _Pro_SupervisionDetailsRepository.GetAllSupervisionDetailsBySuperId(SupervisionId);
            return SupervisionDetails;
        }
        public GeneralMessage SaveSupervisionDetails(Pro_SupervisionDetails SupervisionDetails, int UserId, int BranchId)
        {
            try
            {

                if (SupervisionDetails.SuperDetId == 0)
                {

                    SupervisionDetails.BranchId = BranchId;
                    SupervisionDetails.IsRead = 0;
                    SupervisionDetails.AddUser = UserId;
                    SupervisionDetails.AddDate = DateTime.Now;
                    _TaamerProContext.Pro_SupervisionDetails.Add(SupervisionDetails);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة تفاصيل مرحلة جديدة";
                    _SystemAction.SaveAction("SaveSupervisionDetails", "Pro_SupervisionDetailsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =  Resources.General_SavedSuccessfully };
                }
                else
                {
                    //var DetailsUpdated = _Pro_SupervisionDetailsRepository.GetById(SupervisionDetails.SuperDetId);
                    Pro_SupervisionDetails? DetailsUpdated = _TaamerProContext.Pro_SupervisionDetails.Where(s => s.SuperDetId == SupervisionDetails.SuperDetId).FirstOrDefault();
                    if (DetailsUpdated != null)
                    {


                        DetailsUpdated.NameAr = SupervisionDetails.NameAr;
                        DetailsUpdated.NameEn = SupervisionDetails.NameEn;
                        DetailsUpdated.UpdateUser = UserId;
                        DetailsUpdated.UpdateDate = DateTime.Now;

                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل تفاصيل مرحلة رقم " + SupervisionDetails.SuperDetId;
                    _SystemAction.SaveAction("SaveSupervisionDetails", "Pro_SupervisionDetailsService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =  Resources.General_EditedSuccessfully };
                }

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ تفاصيل المرحلة";
                _SystemAction.SaveAction("SaveSupervisionDetails", "Pro_SupervisionDetailsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =  Resources.General_SavedFailed };
            }
        }
        public GeneralMessage SaveSuperDet(List<Pro_SupervisionDetails> Det, int UserId, int BranchId)
        {
            try
            {
               // var PhaseDetBefore = _Pro_SupervisionDetailsRepository.GetMatching(s => s.SupervisionId == Det[0].SupervisionId).ToList();
                var PhaseDetBefore = _TaamerProContext.Pro_SupervisionDetails.Where(s => s.SupervisionId == Det[0].SupervisionId).ToList();

                if(PhaseDetBefore.Count()>0)
                {
                    _TaamerProContext.Pro_SupervisionDetails.RemoveRange(PhaseDetBefore.ToList());
                    foreach (Pro_SupervisionDetails SuperDet in Det)
                    {
                        if (SuperDet.SuperDetId == 0)
                        {
                            SuperDet.BranchId = BranchId;
                            SuperDet.SupervisionId = SuperDet.SupervisionId;
                            SuperDet.NameAr = SuperDet.NameAr;
                            SuperDet.Note = SuperDet.Note;
                            SuperDet.IsRead = SuperDet.IsRead;
                            SuperDet.ImageUrl = SuperDet.ImageUrl;
                            SuperDet.AddUser = UserId;
                            SuperDet.AddDate = DateTime.Now;
                            _TaamerProContext.Pro_SupervisionDetails.Add(SuperDet);
                        }
                        else
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote2 = "فشل في حفظ مجموعة تفاصيل مرحلة اشراف";
                            _SystemAction.SaveAction("SaveSuperDet", "Pro_SupervisionDetailsService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                        }
                    }
                }
                else
                {
                    foreach (Pro_SupervisionDetails SuperDet in Det)
                    {
                        if (SuperDet.SuperDetId == 0)
                        {
                            SuperDet.BranchId = BranchId;
                            SuperDet.IsRead = 0;
                            SuperDet.SupervisionId = SuperDet.SupervisionId;
                            SuperDet.NameAr = SuperDet.NameAr;
                            SuperDet.Note = SuperDet.Note;
                            SuperDet.AddUser = UserId;
                            SuperDet.AddDate = DateTime.Now;
                            _TaamerProContext.Pro_SupervisionDetails.Add(SuperDet);
                        }
                        else
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote2 = "فشل في حفظ مجموعة تفاصيل مرحلة اشراف";
                            _SystemAction.SaveAction("SaveSuperDet", "Pro_SupervisionDetailsService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                        }

                    }
                }

                _TaamerProContext.SaveChanges();

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "اضافة تفاصيل مرحلة اشراف";
                _SystemAction.SaveAction("SaveSuperDet", "Pro_SupervisionDetailsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ مجموعة تفاصيل مرحلة اشراف";
                _SystemAction.SaveAction("SaveSuperPhaseDet", "Pro_SupervisionDetailsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
            }

        }


        public GeneralMessage DeleteSupervisionDetails(int SupervisionDetId, int UserId, int BranchId)
        {
            try
            {

                //Pro_SupervisionDetails Clause = _Pro_SupervisionDetailsRepository.GetById(SupervisionDetId);
                Pro_SupervisionDetails? Clause = _TaamerProContext.Pro_SupervisionDetails.Where(s => s.SuperDetId == SupervisionDetId).FirstOrDefault();
                if (Clause !=null)
                {
                    Clause.IsDeleted = true;
                    Clause.DeleteDate = DateTime.Now;
                    Clause.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف تفاصيل مرحلة رقم " + SupervisionDetId;
                    _SystemAction.SaveAction("DeleteSupervisionDetails", "Pro_SupervisionDetailsService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                }


                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =  Resources.General_DeletedSuccessfully };

            }
            catch (Exception)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف تفاصيل مرحلة رقم " + SupervisionDetId; ;
                _SystemAction.SaveAction("DeleteSupervisionDetails", "Pro_SupervisionDetailsService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =  Resources.General_DeletedFailed };
            }
        }


        public GeneralMessage ReciveDetails(int DetailId, int UserId, int BranchId)
        {
            try
            {
                //Pro_SupervisionDetails supervisionsDet = _Pro_SupervisionDetailsRepository.GetById(DetailId);
                Pro_SupervisionDetails? supervisionsDet = _TaamerProContext.Pro_SupervisionDetails.Where(s => s.SuperDetId == DetailId).FirstOrDefault();
                if (supervisionsDet != null)
                {
                    supervisionsDet.IsRead = 1;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "استلام مرحلة رقم " + supervisionsDet.SuperDetId;
                    _SystemAction.SaveAction("ReciveDetails", "SupervisionsService", 2, Resources.Received_successfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                }

                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =  Resources.Received_successfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = Resources.Failed_receive_stage;
                _SystemAction.SaveAction("ReciveDetails", "SupervisionsService", 2, Resources.Failed_receive_stage, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =  Resources.Failed_receive_stage };
            }
        }

        public GeneralMessage NotReciveDetails(int DetailId,string Note, int UserId, int BranchId)
        {
            try
            {
                //Pro_SupervisionDetails supervisionsDet = _Pro_SupervisionDetailsRepository.GetById(DetailId);
                Pro_SupervisionDetails? supervisionsDet = _TaamerProContext.Pro_SupervisionDetails.Where(s => s.SuperDetId == DetailId).FirstOrDefault();
                if (supervisionsDet != null)
                {
                    supervisionsDet.Note = Note;
                    supervisionsDet.IsRead = 0;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "تأجيل استلام مرحلة رقم " + supervisionsDet.SuperDetId;
                    _SystemAction.SaveAction("NotReciveDetails", "SupervisionsService", 2, "تم تأجيل استلام بنجاح", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }

                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =  "تم التأجيل بنجاح" };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تأجيل مرحلة";
                _SystemAction.SaveAction("NotReciveDetails", "SupervisionsService", 2, "فشل في تأجيل استلام مرحلة", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =  "فشل في تأجيل استلام مرحلة" };
            }
        }
        public GeneralMessage TheNumberSuperDet(int DetailId, string Note, int UserId, int BranchId)
        {
            try
            {
                // Pro_SupervisionDetails supervisionsDet = _Pro_SupervisionDetailsRepository.GetById(DetailId);
                Pro_SupervisionDetails? supervisionsDet = _TaamerProContext.Pro_SupervisionDetails.Where(s => s.SuperDetId == DetailId).FirstOrDefault();
                if (supervisionsDet != null)
                {
                    supervisionsDet.TheNumber = Note;
                    _TaamerProContext.SaveChanges();
                }
                  

                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =  "تم اضافة العدد بنجاح" };
            }
            catch (Exception)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =  "فشل في اضافة العدد" };
            }
        }

        public GeneralMessage TheLocationSuperDet(int DetailId, string Note, int UserId, int BranchId)
        {
            try
            {
                // Pro_SupervisionDetails supervisionsDet = _Pro_SupervisionDetailsRepository.GetById(DetailId);
                Pro_SupervisionDetails? supervisionsDet = _TaamerProContext.Pro_SupervisionDetails.Where(s => s.SuperDetId == DetailId).FirstOrDefault();
                if (supervisionsDet != null)
                {
                    supervisionsDet.TheLocation = Note;
                    _TaamerProContext.SaveChanges();
                }
                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =  "تم اضافة الموقع بنجاح" };
            }
            catch (Exception)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =  "فشل في اضافة الموقع" };
            }
        }

        public GeneralMessage NotFoundDetails(int DetailId, string Note, int UserId, int BranchId)
        {
            try
            {
                //Pro_SupervisionDetails supervisionsDet = _Pro_SupervisionDetailsRepository.GetById(DetailId);
                Pro_SupervisionDetails? supervisionsDet = _TaamerProContext.Pro_SupervisionDetails.Where(s => s.SuperDetId == DetailId).FirstOrDefault();
                if (supervisionsDet != null)
                {
                    supervisionsDet.Note = Note;
                    supervisionsDet.IsRead = 2;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "مرحلة غير متوفرة رقم " + supervisionsDet.SuperDetId;
                    _SystemAction.SaveAction("NotFoundDetails", "SupervisionsService", 2, "تم تأجيل استلام مرحلة غير متوفرة بنجاح", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                }
                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =  "تم تأجيل استلام مرحلة غير متوفرة بنجاح" };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تأجيل استلام مرحلة غير متوفرة";
                _SystemAction.SaveAction("NotFoundDetails", "SupervisionsService", 2, "فشل في تأجيل استلام مرحلة غير متوفرة", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =  "فشل في تأجيل استلام مرحلة غير متوفرة" };
            }
        }
        public GeneralMessage AddNumberSuperDet(int DetailId, string Note,int Type, int UserId, int BranchId)
        {
            try
            {
                // Pro_SupervisionDetails supervisionsDet = _Pro_SupervisionDetailsRepository.GetById(DetailId);
                Pro_SupervisionDetails? supervisionsDet = _TaamerProContext.Pro_SupervisionDetails.Where(s => s.SuperDetId == DetailId).FirstOrDefault();
                if (supervisionsDet != null)
                {
                    if (Type == 1)
                    {
                        supervisionsDet.NameAr = " السمك الإجمالي للحائط " + Note + " سم ";
                    }
                    else if (Type == 2)
                    {
                        supervisionsDet.NameAr = " سمك الالواح المستخدمة " + Note + " سم "; ;
                    }
                    else if (Type == 3)
                    {
                        supervisionsDet.NameAr = " مقاومة الحائط " + Note + " دقيقة "; ;
                    }

                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "تم اضافة رقم ";
                    _SystemAction.SaveAction("AddNumberSuperDet", "SupervisionsService", 2, "تم الأضافة", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                }
                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =  "تم الأضافة" };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في الأضافة";
                _SystemAction.SaveAction("AddNumberSuperDet", "SupervisionsService", 2, "فشل في الأضافة", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =  "فشل في الأضافة" };
            }
        }


        public GeneralMessage UploadImageSuperDet(int DetailId, string ImageUrl, int UserId, int BranchId)
        {
            try
            {
                // Pro_SupervisionDetails supervisionsDet = _Pro_SupervisionDetailsRepository.GetById(DetailId);
                Pro_SupervisionDetails? supervisionsDet = _TaamerProContext.Pro_SupervisionDetails.Where(s => s.SuperDetId == DetailId).FirstOrDefault();
                if (supervisionsDet != null)
                {
                    supervisionsDet.ImageUrl = ImageUrl;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "حفظ صورة رقم " + supervisionsDet.SuperDetId;
                    _SystemAction.SaveAction("ReadSupervision", "SupervisionsService", 2, "تم حفظ صورة بنجاح", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                }
                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =  "تم حفظ صورة بنجاح" };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ صورة";
                _SystemAction.SaveAction("ReadSupervision", "SupervisionsService", 2, "فشل في حفظ صورة", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =  "فشل في حفظ صورة" };
            }
        }


      
    }
}
