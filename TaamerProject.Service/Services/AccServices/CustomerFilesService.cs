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
    public class CustomerFilesService : ICustomerFilesService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly ICustomerFilesRepository _CustomerFilesRepository;



        public CustomerFilesService(TaamerProjectContext dataContext, ISystemAction systemAction, ICustomerFilesRepository customerFilesRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _CustomerFilesRepository = customerFilesRepository;
        }
        public async Task<IEnumerable<CustomerFilesVM>> GetAllCustomerFiles(int? CustomerId)
        {
            var files = await _CustomerFilesRepository.GetAllCustomerFiles(CustomerId);
            return files;
        }
        public async Task<IEnumerable<CustomerFilesVM>> GetAllCustomerFilesUploaded(int CustomerId, string SearchText)
        {
            var files =await _CustomerFilesRepository.GetAllCustomerFilesUploaded(CustomerId, SearchText);
            return files;
        }
        public GeneralMessage UploadCustomerFiles(CustomerFiles customerFiles, int UserId, int BranchId)
        {
            try
            {
                customerFiles.UploadDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                customerFiles.UserId = UserId;
                customerFiles.IsDeleted = true;
                _TaamerProContext.CustomerFiles.Add(customerFiles);
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "رفع ملف العميل";
               _SystemAction.SaveAction("UploadCustomerFiles", "CustomerFilesService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في رفع ملف العميل";
                _SystemAction.SaveAction("UploadCustomerFiles", "CustomerFilesService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteUpoladCustomerFiles(int FileId, int UserId, int BranchId)
        {
            try
            {
                //CustomerFiles Customer = _CustomerFilesRepository.GetById(FileId);
                CustomerFiles Customer = _TaamerProContext.CustomerFiles.Where(s=>s.FileId== FileId).FirstOrDefault()!;
                _TaamerProContext.CustomerFiles.Remove(Customer);
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف ملف عميل رقم " + FileId;
                _SystemAction.SaveAction("DeleteUpoladCustomerFiles", "CustomerFilesService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف ملف عميل رقم " + FileId; ;
                _SystemAction.SaveAction("DeleteUpoladCustomerFiles", "CustomerFilesService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public GeneralMessage SaveCustomerFiles(CustomerFiles customerFiles, int UserId, int BranchId)
        {
            try
            {
                var customersfiles = _CustomerFilesRepository.GetMatching(s => s.IsDeleted == true && s.CustomerId == customerFiles.CustomerId);
                foreach (var item in customersfiles)
                {
                    item.IsDeleted = false;
                    item.AddDate = DateTime.Now;
                    item.AddUser = item.UserId = UserId;
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "اضافة ملف عميل جديد";
                _SystemAction.SaveAction("SaveCustomerFiles", "CustomerFilesService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ ملف عميل جديد";
                _SystemAction.SaveAction("SaveCustomerFiles", "CustomerFilesService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //----
                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage DeleteCustomerFiles(int FileId, int UserId, int BranchId)
        {
            try
            {
                CustomerFiles Customer = _CustomerFilesRepository.GetById(FileId);
                _TaamerProContext.CustomerFiles.Remove(Customer);
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف ملف عميل رقم " + FileId;
                _SystemAction.SaveAction("DeleteCustomerFiles", "CustomerFilesService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف ملف عميل رقم " + FileId; ;
                _SystemAction.SaveAction("DeleteCustomerFiles", "CustomerFilesService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
    }
}
