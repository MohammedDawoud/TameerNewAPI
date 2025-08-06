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
using System.Runtime.Intrinsics.Arm;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class GuranteesService :  IGuranteesService
    {
        private readonly IGuranteesRepository _guranteesRepository;
        private readonly IInvoicesRepository _InvoicesRepository;
        private readonly ITransactionsRepository _TransactionsRepository;
        private readonly IBranchesRepository _branchesRepository;
        private readonly IAccountsRepository _AccountsRepository;
        private readonly IFiscalyearsRepository _fiscalyearsRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;

        public GuranteesService(IGuranteesRepository guranteesRepository, IInvoicesRepository InvoicesRepository,
            ITransactionsRepository TransactionsRepository, IBranchesRepository branchesRepository,
            IAccountsRepository AccountsRepository, IFiscalyearsRepository fiscalyearsRepository,
            TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _guranteesRepository = guranteesRepository;
            _InvoicesRepository = InvoicesRepository;
            _TransactionsRepository = TransactionsRepository;
            _branchesRepository = branchesRepository;
            _AccountsRepository = AccountsRepository;
            _fiscalyearsRepository = fiscalyearsRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;

        }
        public Task<IEnumerable<GuaranteesVM>> GetAllGurantees(int BranchId)
        {
            var gurantees = _guranteesRepository.GetAllGurantees(BranchId);
            return gurantees;
        }
        public GeneralMessage SaveGurantee(Guarantees guarantees,int UserId, int BranchId, int? yearid)
        {
            try
            {
               // var codeExist = _guranteesRepository.GetMatching(s => s.IsDeleted == false && s.GuaranteeId != guarantees.GuaranteeId && s.Number == guarantees.Number).FirstOrDefault();

                var codeExist = _TaamerProContext.Guarantees.Where(s => s.IsDeleted == false && s.GuaranteeId != guarantees.GuaranteeId && s.Number == guarantees.Number).FirstOrDefault();

                if (codeExist != null)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate1 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote1 = "فشل في حفظ الضمان";
                    _SystemAction.SaveAction("SaveGurantee", "GuranteesService", 1, "رقم الضمان موجود من قبل", "", "", ActionDate1, UserId, BranchId, ActionNote1, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =Resources.NumberAlreadyExists };
                }
                //var year = _fiscalyearsRepository.GetCurrentYear();
                if (yearid == null)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حفظ الضمان";
                    _SystemAction.SaveAction("SaveGurantee", "GuranteesService", 1, "فشل في الحفظ ,تأكد من اختيار السنة الماليه أولا", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =Resources.General_SavedFailed};
                }
                if (guarantees.GuaranteeId == 0)
                {
                    DateTimeFormatInfo usDtfi = new CultureInfo("en-US", false).DateTimeFormat;
                    guarantees.StartDate = Convert.ToDateTime(guarantees.StartDateStr, usDtfi);
                    guarantees.EndDate = guarantees.StartDate.Value.AddDays(guarantees.Period);
                    guarantees.AddUser = UserId;
                    guarantees.BranchId = BranchId;
                    guarantees.AddDate = DateTime.Now;
                    _TaamerProContext.Guarantees.Add(guarantees);
                    // add gurantee Invoice
                    //var newInvoiceObj = new Invoices();
                    //newInvoiceObj.InvoiceNumber = _InvoicesRepository.GenerateNextInvoiceNumber(15, yearid, BranchId);
                    //newInvoiceObj.Type = 15;
                    //newInvoiceObj.Date = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    //newInvoiceObj.HijriDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar"));
                    //newInvoiceObj.Notes = guarantees.Number + "  " + "ضمان بنكي برقم";
                    //newInvoiceObj.Rad = false;
                    //newInvoiceObj.InvoiceValue = guarantees.Value;
                    //newInvoiceObj.TotalValue = guarantees.Value - (newInvoiceObj.DiscountValue ?? 0);
                    //newInvoiceObj.IsPost = true;
                    //newInvoiceObj.PostDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    //newInvoiceObj.PostHijriDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar"));
                    //newInvoiceObj.YearId = yearid;
                    //newInvoiceObj.InvoiceReference = guarantees.Number;
                    //newInvoiceObj.IsTax = false;
                    //newInvoiceObj.BranchId = BranchId;
                    //newInvoiceObj.printBankAccount = false;
                    //newInvoiceObj.DunCalc = false;

                    //_InvoicesRepository.Add(newInvoiceObj);
                    //// add gurantee transaction
                    //var depitTrans = new Transactions();
                    //try
                    //{
                    //    depitTrans.AccountId = _branchesRepository.GetById(BranchId).GuaranteeAccId;
                    //    depitTrans.AccountType = _AccountsRepository.GetById(depitTrans.AccountId).Type;
                    //}
                    //catch (Exception)
                    //{
                    //    //-----------------------------------------------------------------------------------------------------------------
                    //    string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    //    string ActionNote2 = "فشل في حفظ الضمان";
                    //    _SystemAction.SaveAction("SaveGurantee", "GuranteesService", 1, "فشل في الحفظ ,تأكد من ربط الفرع بحساب للضمانات", "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                    //    //-----------------------------------------------------------------------------------------------------------------
                    //    return new GeneralMessage {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase ="فشل في الحفظ ,تأكد من ربط الفرع بحساب للضمانات" };
                    //}
                    //newInvoiceObj.TransactionDetails = new List<Transactions>();
                    ////depit 
                    //newInvoiceObj.TransactionDetails.Add(new Transactions
                    //{
                    //    TransactionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en")),
                    //    TransactionHijriDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar")),
                    //    AccountId = depitTrans.AccountId,
                    //    AccountType = depitTrans.AccountType,
                    //    Type = 15,
                    //    LineNumber = 1,
                    //    Depit = guarantees.Value,Credit = 0,
                    //    YearId = yearid,
                    //    Notes = guarantees.Number + "  " + "ضمان بنكي برقم",
                    //    Details = guarantees.Number + "  " + "ضمان بنكي برقم",
                    //    InvoiceReference = guarantees.Number,
                    //    IsPost = true,
                    //    BranchId = BranchId,
                    //    AddDate = DateTime.Now,
                    //    AddUser = UserId,
                    //    IsDeleted = false,
                    //});
                    ////credit 
                    //newInvoiceObj.TransactionDetails.Add(new Transactions
                    //{
                    //    TransactionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en")),
                    //    TransactionHijriDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar")),
                    //    AccountId = guarantees.GuarantorAccId,
                    //    AccountType = _AccountsRepository.GetById(guarantees.GuarantorAccId).Type,
                    //    Type = 15,
                    //    LineNumber = 2,
                    //    Credit = guarantees.Value, Depit = 0,
                    //    YearId = yearid,
                    //    Notes = guarantees.Number + "  " + "ضمان بنكي برقم",
                    //    Details = guarantees.Number + "  " + "ضمان بنكي برقم",
                    //    InvoiceReference = guarantees.Number,
                    //    IsPost = true,
                    //    BranchId = BranchId,
                    //    AddDate = DateTime.Now,
                    //    AddUser = UserId,
                    //   IsDeleted = false,
                    //});
                    //_TransactionsRepository.AddRange(newInvoiceObj.TransactionDetails);
                     _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة ضمان جديد";
                    _SystemAction.SaveAction("SaveGurantee", "GuranteesService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =Resources.General_SavedSuccessfully };
                }
                else
                {
                    // var guranteeUpdated = _guranteesRepository.GetById(guarantees.GuaranteeId);
                    Guarantees? guranteeUpdated = _TaamerProContext.Guarantees.Where(s => s.GuaranteeId == guarantees.GuaranteeId).FirstOrDefault();
                    if (guranteeUpdated != null)
                    {
                        DateTimeFormatInfo usDtfi = new CultureInfo("en-US", false).DateTimeFormat;
                        guranteeUpdated.StartDate = Convert.ToDateTime(guarantees.StartDateStr, usDtfi);
                        //guranteeUpdated.EndDate = guarantees.StartDate.Value.AddDays(guarantees.Period);
                        guranteeUpdated.EndDate = Convert.ToDateTime(guarantees.StartDateStr, usDtfi).AddDays(guarantees.Period);

                        guranteeUpdated.Number = guarantees.Number;
                        guranteeUpdated.Period = guarantees.Period;
                        guranteeUpdated.ProjectName = guarantees.ProjectName;
                        guranteeUpdated.Value = guarantees.Value;
                        guranteeUpdated.GuarantorAccId = guarantees.GuarantorAccId;
                        guranteeUpdated.BankName = guarantees.BankName;
                        guranteeUpdated.Type = guarantees.Type;
                        guranteeUpdated.CustomerName = guarantees.CustomerName;
                        guranteeUpdated.UpdateUser = UserId;
                        guranteeUpdated.UpdateDate = DateTime.Now;
                    }
                     _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل ضمان رقم " + guarantees.GuaranteeId;
                    _SystemAction.SaveAction("SaveGurantee", "GuranteesService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =Resources.General_EditedSuccessfully };
                }

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الضمان";
                _SystemAction.SaveAction("SaveGurantee", "GuranteesService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteGurantee(int GuaranteeId,int UserId,int BranchId)
        {
            try
            {
                //Guarantees guarantee = _guranteesRepository.GetById(GuaranteeId);
                Guarantees? guarantee = _TaamerProContext.Guarantees.Where(s => s.GuaranteeId == GuaranteeId).FirstOrDefault();
                if (guarantee != null)
                {
                    guarantee.IsDeleted = true;
                    guarantee.DeleteDate = DateTime.Now;
                    guarantee.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف ضمان رقم " + GuaranteeId;
                    _SystemAction.SaveAction("DeleteGurantee", "GuranteesService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }


                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف ضمان رقم " + GuaranteeId; ;
                _SystemAction.SaveAction("DeleteGurantee", "GuranteesService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =Resources.General_DeletedFailed };
            }
        }
       

    }
}
