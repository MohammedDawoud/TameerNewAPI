using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Repository.Repositories;
using System.Net;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class CarMovementsService : ICarMovementsService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly ICarMovementsRepository _CarMovementsRepository;
        private readonly IUsersRepository _usersRepository;


        public CarMovementsService(TaamerProjectContext dataContext, ISystemAction systemAction, ICarMovementsRepository carMovementsRepository, IUsersRepository usersRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            this._usersRepository = usersRepository;
            _CarMovementsRepository = carMovementsRepository;
        }

        public async Task<IEnumerable<CarMovementsVM>> GetAllCarMovements()
        {
            var requirements = await _CarMovementsRepository.GetAllCarMovements();
            return requirements;
        }

        public async Task<IEnumerable<CarMovementsVM>> GetAllCarMovementsSearch(CarMovementsVM Search, int BranchId)
        {
            if ((bool)Search.IsSearch)
            {
                return await _CarMovementsRepository.GetAllCarMovementsSearchObject(Search, BranchId);
            }
            else
            {
                return await _CarMovementsRepository.GetAllCarMovements();
            }
        }


        public GeneralMessage SaveCarMovement(CarMovements carMovement, int UserId, int BranchId, int? YearId)
        {
            try
            {

                var Branch = _TaamerProContext.Branch.Where(x=>x.BranchId==BranchId).FirstOrDefault();
                //if (Branch == null || Branch.SaleCashAccId == null)
                //{
                //    //-----------------------------------------------------------------------------------------------------------------
                //    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                //    string ActionNote = "فشل في حفظ الحركة";
                //    SaveAction("SaveCarMovement", "VoucherService", 1, "تأكد من حساب السيارات في حسابات الفرع", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //    //-----------------------------------------------------------------------------------------------------------------

                //    return new GeneralMessage { Result = false, Message = "تأكد من حساب السيارات في حسابات الفرع" };

                //}
                //if (Branch == null || Branch.BoxAccId == null)
                //{
                //    //-----------------------------------------------------------------------------------------------------------------
                //    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                //    string ActionNote = "فشل في حفظ الحركة";
                //    SaveAction("SaveCarMovement", "VoucherService", 1, "تأكد من حساب الصندوق في حسابات الفرع", "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //    //-----------------------------------------------------------------------------------------------------------------

                //    return new GeneralMessage { Result = false, Message = "تأكد من حساب الصندوق في حسابات الفرع" };

                //}

                if (carMovement.MovementId == 0)
                {
                    carMovement.AddUser = UserId;
                    carMovement.BranchId = BranchId;
                    carMovement.AddDate = DateTime.Now;
                    _TaamerProContext.CarMovements.Add(carMovement);
                    _TaamerProContext.SaveChanges();


                    var CarTypeTrans = 28;
                    var EmpValue = carMovement.EmpAmount ?? 0;
                    var OwnerValue = carMovement.OwnerAmount ?? 0;
                    var BoxValue = EmpValue + OwnerValue;
                    string vouchertypename = "حركة سيارة";


                    //if(BoxValue>0)
                    //{

                    //    Invoices voucher = new Invoices();
                    //    voucher.IsPost = true;
                    //    voucher.Rad = false;
                    //    voucher.IsTax = false;
                    //    voucher.YearId = YearId;
                    //    voucher.BranchId = BranchId;
                    //    voucher.AddUser = UserId;
                    //    voucher.AddDate = DateTime.Now;
                    //    voucher.printBankAccount = false;
                    //    voucher.DunCalc = false;
                    //    voucher.JournalNumber = null;
                    //    voucher.MovementId = carMovement.MovementId;
                    //    voucher.InvoiceValue = BoxValue;
                    //    voucher.InvoiceValueText = ConvertToWord_NEW(BoxValue.ToString());
                    //    voucher.TaxAmount = 0;
                    //    voucher.TotalValue = BoxValue;
                    //    voucher.Type = CarTypeTrans;

                    //    voucher.Date = carMovement.Date;
                    //    voucher.HijriDate = carMovement.HijriDate;
                    //    voucher.PostDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    //    voucher.PostHijriDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar"));


                    //    var NextInv = _InvoicesRepository.GenerateNextInvoiceNumber(CarTypeTrans, YearId, BranchId);
                    //    voucher.InvoiceNumber = NextInv;

                    //    voucher.TransactionDetails = new List<Transactions>();

                    //    int? CostId;

                    //    try
                    //    { CostId = _CostCenterRepository.GetMatching(w => w.BranchId == BranchId && w.IsDeleted == false && w.ProjId == 0).Select(s => s.CostCenterId).FirstOrDefault(); }
                    //    catch
                    //    { CostId = null; }

                    //    if (carMovement.EmpId == null)
                    //    {
                    //        return new GeneralMessage { Result = false, Message = "تأكد من اختيار الموظف" };

                    //    }
                    //    var Emp = _EmployeesRepository.GetById(carMovement.EmpId);
                    //    if (Emp.AccountIDs == null)
                    //    {
                    //        return new GeneralMessage { Result = false, Message = "تأكد من حساب السلف للموظف في حسابات الفرع" };
                    //    }

                    //    _InvoicesRepository.Add(voucher);

                    //    if (EmpValue > 0)
                    //    {
                    //        voucher.TransactionDetails.Add(new Transactions
                    //        {
                    //            AccTransactionDate = carMovement.Date,
                    //            AccTransactionHijriDate = carMovement.HijriDate,
                    //            TransactionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en")),
                    //            TransactionHijriDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar")),
                    //            AccountId = Emp.AccountIDs,
                    //            CostCenterId = CostId,
                    //            AccountType = _AccountsRepository.GetById(Emp.AccountIDs).Type,
                    //            Type = CarTypeTrans,
                    //            LineNumber = 1,
                    //            Depit = EmpValue,
                    //            Credit = 0,
                    //            YearId = YearId,
                    //            Notes = "" + vouchertypename + " " + Resources.number + " " + voucher.InvoiceNumber + "",
                    //            Details = "" + vouchertypename + " " + Resources.number + " " + voucher.InvoiceNumber + "",
                    //            InvoiceReference = "",
                    //            InvoiceId = voucher.InvoiceId,
                    //            IsPost = true,
                    //            BranchId = BranchId,
                    //            AddDate = DateTime.Now,
                    //            AddUser = UserId,
                    //            IsDeleted = false,
                    //        });

                    //    }

                    //    if (OwnerValue > 0)
                    //    {
                    //        voucher.TransactionDetails.Add(new Transactions
                    //        {
                    //            AccTransactionDate = carMovement.Date,
                    //            AccTransactionHijriDate = carMovement.HijriDate,
                    //            TransactionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en")),
                    //            TransactionHijriDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar")),
                    //            AccountId = carMovement.AccountId,
                    //            CostCenterId = CostId,
                    //            AccountType = _AccountsRepository.GetById(carMovement.AccountId).Type,
                    //            Type = CarTypeTrans,
                    //            LineNumber = 2,
                    //            Credit = 0,
                    //            Depit = OwnerValue,
                    //            YearId = YearId,
                    //            Notes = "" + vouchertypename + " " + Resources.number + " " + voucher.InvoiceNumber + "",
                    //            Details = "" + vouchertypename + " " + Resources.number + " " + voucher.InvoiceNumber + "",
                    //            InvoiceReference = "",
                    //            InvoiceId = voucher.InvoiceId,
                    //            IsPost = true,
                    //            BranchId = BranchId,
                    //            AddDate = DateTime.Now,
                    //            AddUser = UserId,
                    //            IsDeleted = false,
                    //        });

                    //    }
                    //    voucher.TransactionDetails.Add(new Transactions
                    //    {
                    //        AccTransactionDate = carMovement.Date,
                    //        AccTransactionHijriDate = carMovement.HijriDate,
                    //        TransactionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en")),
                    //        TransactionHijriDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar")),
                    //        AccountId = Branch.BoxAccId,
                    //        CostCenterId = CostId,
                    //        AccountType = _AccountsRepository.GetById(Branch.BoxAccId).Type,
                    //        Type = CarTypeTrans,
                    //        LineNumber = 3,
                    //        Credit = BoxValue,
                    //        Depit = 0,
                    //        YearId = YearId,
                    //        Notes = "" + vouchertypename + " " + Resources.number + " " + voucher.InvoiceNumber + "",
                    //        Details = "" + vouchertypename + " " + Resources.number + " " + voucher.InvoiceNumber + "",
                    //        InvoiceReference = "",
                    //        InvoiceId = voucher.InvoiceId,
                    //        IsPost = true,
                    //        BranchId = BranchId,
                    //        AddDate = DateTime.Now,
                    //        AddUser = UserId,
                    //        IsDeleted = false,
                    //    });

                    //    _TransactionsRepository.AddRange(voucher.TransactionDetails);

                    //    _uow.SaveChanges();

                    //    voucher.IsPost = true;
                    //    voucher.PostDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    //    voucher.PostHijriDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar"));

                    //    var newJournal = new Journals();
                    //    var JNo = _JournalsRepository.GenerateNextJournalNumber(YearId ?? default(int), BranchId);
                    //    if (voucher.Type == 10)
                    //    {
                    //        JNo = 1;
                    //    }
                    //    else
                    //    {
                    //        JNo = (newJournal.VoucherType == 10 && JNo == 1) ? JNo : (newJournal.VoucherType != 10 && JNo == 1) ? JNo + 1 : JNo;
                    //    }


                    //    newJournal.JournalNo = JNo;
                    //    newJournal.YearMalia = YearId ?? default(int);
                    //    newJournal.VoucherId = voucher.InvoiceId;
                    //    newJournal.VoucherType = voucher.Type;
                    //    newJournal.BranchId = voucher.BranchId ?? 0;
                    //    newJournal.AddDate = DateTime.Now;
                    //    newJournal.AddUser = newJournal.UserId = UserId;
                    //    _JournalsRepository.Add(newJournal);
                    //    foreach (var trans in voucher.TransactionDetails.ToList())
                    //    {
                    //        trans.IsPost = true;
                    //        trans.JournalNo = newJournal.JournalNo;
                    //    }
                    //    voucher.JournalNumber = newJournal.JournalNo;

                    //}
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة حركة سيارة جديد";
                   _SystemAction.SaveAction("SaveCarMovement", "CarMovementsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };
                }
                else
                {
                    var CarMovementsUpdated = _TaamerProContext.CarMovements.Where(x=>x.MovementId==carMovement.MovementId).FirstOrDefault();
                    if (CarMovementsUpdated != null)
                    {
                        CarMovementsUpdated.ItemId = carMovement.ItemId;
                        CarMovementsUpdated.Type = carMovement.Type;
                        CarMovementsUpdated.EmpId = carMovement.EmpId;
                        CarMovementsUpdated.Date = carMovement.Date;
                        CarMovementsUpdated.HijriDate = carMovement.HijriDate;
                        CarMovementsUpdated.Notes = carMovement.Notes;
                        CarMovementsUpdated.EmpAmount = carMovement.EmpAmount;
                        CarMovementsUpdated.OwnerAmount = carMovement.OwnerAmount;
                        CarMovementsUpdated.UpdateUser = UserId;
                        CarMovementsUpdated.UpdateDate = DateTime.Now;
                    }
                    var CarTypeTrans = 28;
                    var EmpValue = carMovement.EmpAmount ?? 0;
                    var OwnerValue = carMovement.OwnerAmount ?? 0;
                    var BoxValue = EmpValue + OwnerValue;
                    string vouchertypename = "حركة سيارة";

                    //if(BoxValue>0)
                    //{

                    //    int? CostId;
                    //    try
                    //    { CostId = _CostCenterRepository.GetMatching(w => w.BranchId == BranchId && w.IsDeleted == false && w.ProjId == 0).Select(s => s.CostCenterId).FirstOrDefault(); }
                    //    catch
                    //    { CostId = null; }

                    //    if (carMovement.EmpId == null)
                    //    {
                    //        return new GeneralMessage { Result = false, Message = "تأكد من اختيار الموظف" };

                    //    }
                    //    var Emp = _EmployeesRepository.GetById(carMovement.EmpId);
                    //    if (Emp.AccountIDs == null)
                    //    {
                    //        return new GeneralMessage { Result = false, Message = "تأكد من حساب السلف للموظف في حسابات الفرع" };
                    //    }

                    //    var VoucherUpdated = _InvoicesRepository.GetMatching(s => s.IsDeleted==false && s.MovementId == carMovement.MovementId).FirstOrDefault();
                    //    if (VoucherUpdated != null)
                    //    {
                    //        VoucherUpdated.InvoiceValue = BoxValue;
                    //        VoucherUpdated.InvoiceValueText = ConvertToWord_NEW(BoxValue.ToString());
                    //        VoucherUpdated.TaxAmount = 0;
                    //        VoucherUpdated.TotalValue = BoxValue;

                    //        VoucherUpdated.Date = carMovement.Date;
                    //        VoucherUpdated.HijriDate = carMovement.HijriDate;
                    //        VoucherUpdated.PostDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    //        VoucherUpdated.PostHijriDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar"));

                    //        _TransactionsRepository.RemoveRange(VoucherUpdated.TransactionDetails.ToList());

                    //        VoucherUpdated.TransactionDetails = new List<Transactions>();


                    //        if (EmpValue > 0)
                    //        {
                    //            VoucherUpdated.TransactionDetails.Add(new Transactions
                    //            {
                    //                AccTransactionDate = carMovement.Date,
                    //                AccTransactionHijriDate = carMovement.HijriDate,
                    //                TransactionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en")),
                    //                TransactionHijriDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar")),
                    //                AccountId = Emp.AccountIDs,
                    //                CostCenterId = CostId,
                    //                AccountType = _AccountsRepository.GetById(Emp.AccountIDs).Type,
                    //                Type = CarTypeTrans,
                    //                LineNumber = 1,
                    //                Depit = EmpValue,
                    //                Credit = 0,
                    //                YearId = YearId,
                    //                Notes = "" + vouchertypename + " " + Resources.number + " " + VoucherUpdated.InvoiceNumber + "",
                    //                Details = "" + vouchertypename + " " + Resources.number + " " + VoucherUpdated.InvoiceNumber + "",
                    //                InvoiceReference = "",
                    //                InvoiceId = VoucherUpdated.InvoiceId,
                    //                IsPost = true,
                    //                BranchId = BranchId,
                    //                AddDate = DateTime.Now,
                    //                AddUser = UserId,
                    //                IsDeleted = false,
                    //                JournalNo= VoucherUpdated.JournalNumber,
                    //            });

                    //        }
                    //        if (OwnerValue > 0)
                    //        {
                    //            VoucherUpdated.TransactionDetails.Add(new Transactions
                    //            {
                    //                AccTransactionDate = carMovement.Date,
                    //                AccTransactionHijriDate = carMovement.HijriDate,
                    //                TransactionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en")),
                    //                TransactionHijriDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar")),
                    //                AccountId = carMovement.AccountId,
                    //                CostCenterId = CostId,
                    //                AccountType = _AccountsRepository.GetById(carMovement.AccountId).Type,
                    //                Type = CarTypeTrans,
                    //                LineNumber = 2,
                    //                Credit = 0,
                    //                Depit = OwnerValue,
                    //                YearId = YearId,
                    //                Notes = "" + vouchertypename + " " + Resources.number + " " + VoucherUpdated.InvoiceNumber + "",
                    //                Details = "" + vouchertypename + " " + Resources.number + " " + VoucherUpdated.InvoiceNumber + "",
                    //                InvoiceReference = "",
                    //                InvoiceId = VoucherUpdated.InvoiceId,
                    //                IsPost = true,
                    //                BranchId = BranchId,
                    //                AddDate = DateTime.Now,
                    //                AddUser = UserId,
                    //                IsDeleted = false,
                    //                JournalNo = VoucherUpdated.JournalNumber,
                    //            });

                    //        }
                    //        VoucherUpdated.TransactionDetails.Add(new Transactions
                    //        {
                    //            AccTransactionDate = carMovement.Date,
                    //            AccTransactionHijriDate = carMovement.HijriDate,
                    //            TransactionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en")),
                    //            TransactionHijriDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar")),
                    //            AccountId = Branch.BoxAccId,
                    //            CostCenterId = CostId,
                    //            AccountType = _AccountsRepository.GetById(Branch.BoxAccId).Type,
                    //            Type = CarTypeTrans,
                    //            LineNumber = 3,
                    //            Credit = BoxValue,
                    //            Depit = 0,
                    //            YearId = YearId,
                    //            Notes = "" + vouchertypename + " " + Resources.number + " " + VoucherUpdated.InvoiceNumber + "",
                    //            Details = "" + vouchertypename + " " + Resources.number + " " + VoucherUpdated.InvoiceNumber + "",
                    //            InvoiceReference = "",
                    //            InvoiceId = VoucherUpdated.InvoiceId,
                    //            IsPost = true,
                    //            BranchId = BranchId,
                    //            AddDate = DateTime.Now,
                    //            AddUser = UserId,
                    //            IsDeleted = false,
                    //            JournalNo = VoucherUpdated.JournalNumber,
                    //        });
                    //        _TransactionsRepository.AddRange(VoucherUpdated.TransactionDetails);

                    //    }
                    //    else
                    //    {

                    //        Invoices voucher = new Invoices();
                    //        voucher.IsPost = true;
                    //        voucher.Rad = false;
                    //        voucher.IsTax = false;
                    //        voucher.YearId = YearId;
                    //        voucher.BranchId = BranchId;
                    //        voucher.AddUser = UserId;
                    //        voucher.AddDate = DateTime.Now;
                    //        voucher.printBankAccount = false;
                    //        voucher.DunCalc = false;
                    //        voucher.JournalNumber = null;
                    //        voucher.MovementId = carMovement.MovementId;
                    //        voucher.InvoiceValue = BoxValue;
                    //        voucher.InvoiceValueText = ConvertToWord_NEW(BoxValue.ToString());
                    //        voucher.TaxAmount = 0;
                    //        voucher.TotalValue = BoxValue;
                    //        voucher.Type = CarTypeTrans;

                    //        voucher.Date = carMovement.Date;
                    //        voucher.HijriDate = carMovement.HijriDate;
                    //        voucher.PostDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    //        voucher.PostHijriDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar"));

                    //        voucher.TransactionDetails = new List<Transactions>();





                    //        var NextInv = _InvoicesRepository.GenerateNextInvoiceNumber(CarTypeTrans, YearId, BranchId);
                    //        voucher.InvoiceNumber = NextInv;

                    //        _InvoicesRepository.Add(voucher);


                    //        if (EmpValue > 0)
                    //        {
                    //            voucher.TransactionDetails.Add(new Transactions
                    //            {
                    //                AccTransactionDate = carMovement.Date,
                    //                AccTransactionHijriDate = carMovement.HijriDate,
                    //                TransactionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en")),
                    //                TransactionHijriDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar")),
                    //                AccountId = Emp.AccountIDs,
                    //                CostCenterId = CostId,
                    //                AccountType = _AccountsRepository.GetById(Emp.AccountIDs).Type,
                    //                Type = CarTypeTrans,
                    //                LineNumber = 1,
                    //                Depit = EmpValue,
                    //                Credit = 0,
                    //                YearId = YearId,
                    //                Notes = "" + vouchertypename + " " + Resources.number + " " + voucher.InvoiceNumber + "",
                    //                Details = "" + vouchertypename + " " + Resources.number + " " + voucher.InvoiceNumber + "",
                    //                InvoiceReference = "",
                    //                InvoiceId = voucher.InvoiceId,
                    //                IsPost = true,
                    //                BranchId = BranchId,
                    //                AddDate = DateTime.Now,
                    //                AddUser = UserId,
                    //                IsDeleted = false,
                    //            });

                    //        }
                    //        if (OwnerValue > 0)
                    //        {
                    //            voucher.TransactionDetails.Add(new Transactions
                    //            {
                    //                AccTransactionDate = carMovement.Date,
                    //                AccTransactionHijriDate = carMovement.HijriDate,
                    //                TransactionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en")),
                    //                TransactionHijriDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar")),
                    //                AccountId = carMovement.AccountId,
                    //                CostCenterId = CostId,
                    //                AccountType = _AccountsRepository.GetById(carMovement.AccountId).Type,
                    //                Type = CarTypeTrans,
                    //                LineNumber = 2,
                    //                Credit = 0,
                    //                Depit = OwnerValue,
                    //                YearId = YearId,
                    //                Notes = "" + vouchertypename + " " + Resources.number + " " + voucher.InvoiceNumber + "",
                    //                Details = "" + vouchertypename + " " + Resources.number + " " + voucher.InvoiceNumber + "",
                    //                InvoiceReference = "",
                    //                InvoiceId = voucher.InvoiceId,
                    //                IsPost = true,
                    //                BranchId = BranchId,
                    //                AddDate = DateTime.Now,
                    //                AddUser = UserId,
                    //                IsDeleted = false,
                    //            });

                    //        }
                    //        voucher.TransactionDetails.Add(new Transactions
                    //        {
                    //            AccTransactionDate = carMovement.Date,
                    //            AccTransactionHijriDate = carMovement.HijriDate,
                    //            TransactionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en")),
                    //            TransactionHijriDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar")),
                    //            AccountId = Branch.BoxAccId,
                    //            CostCenterId = CostId,
                    //            AccountType = _AccountsRepository.GetById(Branch.BoxAccId).Type,
                    //            Type = CarTypeTrans,
                    //            LineNumber = 3,
                    //            Credit = BoxValue,
                    //            Depit = 0,
                    //            YearId = YearId,
                    //            Notes = "" + vouchertypename + " " + Resources.number + " " + voucher.InvoiceNumber + "",
                    //            Details = "" + vouchertypename + " " + Resources.number + " " + voucher.InvoiceNumber + "",
                    //            InvoiceReference = "",
                    //            InvoiceId = voucher.InvoiceId,
                    //            IsPost = true,
                    //            BranchId = BranchId,
                    //            AddDate = DateTime.Now,
                    //            AddUser = UserId,
                    //            IsDeleted = false,
                    //        });

                    //        _TransactionsRepository.AddRange(voucher.TransactionDetails);
                    //        _uow.SaveChanges();

                    //        voucher.IsPost = true;
                    //        voucher.PostDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    //        voucher.PostHijriDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("ar"));

                    //        var newJournal = new Journals();
                    //        var JNo = _JournalsRepository.GenerateNextJournalNumber(YearId ?? default(int), BranchId);
                    //        if (voucher.Type == 10)
                    //        {
                    //            JNo = 1;
                    //        }
                    //        else
                    //        {
                    //            JNo = (newJournal.VoucherType == 10 && JNo == 1) ? JNo : (newJournal.VoucherType != 10 && JNo == 1) ? JNo + 1 : JNo;
                    //        }


                    //        newJournal.JournalNo = JNo;
                    //        newJournal.YearMalia = YearId ?? default(int);
                    //        newJournal.VoucherId = voucher.InvoiceId;
                    //        newJournal.VoucherType = voucher.Type;
                    //        newJournal.BranchId = voucher.BranchId ?? 0;
                    //        newJournal.AddDate = DateTime.Now;
                    //        newJournal.AddUser = newJournal.UserId = UserId;
                    //        _JournalsRepository.Add(newJournal);
                    //        foreach (var trans in voucher.TransactionDetails.ToList())
                    //        {
                    //            trans.IsPost = true;
                    //            trans.JournalNo = newJournal.JournalNo;
                    //        }
                    //        voucher.JournalNumber = newJournal.JournalNo;
                    //    }

                    //}

                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل حركة سيارة رقم " + carMovement.MovementId;
                    _SystemAction.SaveAction("SaveCarMovement", "CarMovementsService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully };
                }

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ حركة السيارة";
                _SystemAction.SaveAction("SaveCarMovement", "CarMovementsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteCarMovement(int MovementId, int UserId, int BranchId)
        {
            try
            {
                CarMovements carMovement = _TaamerProContext.CarMovements.Where(x => x.MovementId == MovementId).FirstOrDefault();
                carMovement.IsDeleted = true;
                carMovement.DeleteDate = DateTime.Now;
                carMovement.DeleteUser = UserId;


                var VoucherUpdated = _TaamerProContext.Invoices.Where(s => s.IsDeleted == false && s.MovementId == carMovement.MovementId).FirstOrDefault();
                if (VoucherUpdated != null)
                {
                    VoucherUpdated.IsDeleted = true;
                    VoucherUpdated.DeleteDate = DateTime.Now;
                    VoucherUpdated.DeleteUser = UserId;
                    _TaamerProContext.Transactions.RemoveRange(VoucherUpdated.TransactionDetails.ToList());
                }


                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف حركة سيارة رقم " + MovementId;
                _SystemAction.SaveAction("DeleteCarMovement", "CarMovementsService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف حركة سيارة رقم " + MovementId; ;
                _SystemAction.SaveAction("DeleteCarMovement", "CarMovementsService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public async Task<IEnumerable<CarMovementsVM>> SearchCarMovements(CarMovementsVM CarMovementsSearch, int BranchId)
        {
            var CarMovements = _CarMovementsRepository.SearchCarMovements(CarMovementsSearch, BranchId).Result.ToList();
            return CarMovements;
        }
        public async Task<IEnumerable<CarMovementsVM>> GetAllCarMovementsByDateSearch(string FromDate, string ToDate)
        {
            var CarMovements = await _CarMovementsRepository.GetAllCarMovementsByDateSearch(FromDate, ToDate);
            return CarMovements;
        }
        //heba
        public async Task<DataTable> GetAllCarMovementsByDateSearch(string CarType, string CarId, string StartDate, string EndDate, string EmpId, string Con)
        {
            DataTable dt = await _CarMovementsRepository.GetAllCarMovementsByDateSearch(CarType, CarId, StartDate, EndDate, EmpId, Con);
            return dt;
        }
        public async Task<DataTable> GetAllCarMovementsByDate(string fromDate, string toDate, string Con)
        {
            DataTable dt =await _CarMovementsRepository.GetAllCarMovementsByDate(fromDate, toDate, Con);
            return dt;
        }
        public static string ConvertToWord_NEW(string Num)
        {
            CurrencyInfo _currencyInfo = new CurrencyInfo(CurrencyInfo.Currencies.SaudiArabia);
            ToWord toWord = new ToWord(Convert.ToDecimal(Num), _currencyInfo);
            return toWord.ConvertToArabic();
        }
    }
}
