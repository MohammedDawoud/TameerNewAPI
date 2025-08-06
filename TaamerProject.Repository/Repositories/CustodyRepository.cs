using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;

namespace TaamerProject.Repository.Repositories
{
    public class CustodyRepository :  ICustodyRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public CustodyRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }
        public async Task<IEnumerable<CustodyVM>> GetAllCustody(int BranchId)
        {
            // s.Status == false && //--> to get all custodies (stell related to emps or not)
            var custody = _TaamerProContext.Custody.Where(s => s.IsDeleted == false && s.Status == false && s.BranchId == BranchId && (!s.EmployeeId.HasValue || (s.EmployeeId.HasValue && !s.Employee.IsDeleted &&
            !string.IsNullOrEmpty(s.Employee.WorkStartDate) && string.IsNullOrEmpty(s.Employee.EndWorkDate)))).Select(x => new CustodyVM
            {
                CustodyId = x.CustodyId,
                EmployeeId = x.EmployeeId,
                ItemId = x.ItemId,
                Date = x.Date,
                HijriDate = x.HijriDate,
                Quantity = x.Quantity ?? 0,
                Type = x.Type,
                BranchId = x.BranchId,
                ItemName = x.Type == 2 ? "عهدة مالية" : x.Item.NameAr,
                ItemPrice = x.Type == 2 ? x.CustodyValue : x.Item.Price,
                EmployeeName = x.Employee.EmployeeNameAr,
                Status = x.Status,
                ConvertStatus=x.ConvertStatus??false,
                InvoiceId=x.InvoiceId,
                InvoiceNumber = x.Invoices != null ? x.Invoices.InvoiceNumber.ToString() : "",

            }).ToList();
                return custody;
        }
        public async Task<IEnumerable<CustodyVM>> GetDistinctCustody(int BranchId)
        {
            var custody = _TaamerProContext.Custody.Where(s => s.IsDeleted == false && s.Status == false &&  s.BranchId == BranchId && !s.Employee.IsDeleted &&
            !string.IsNullOrEmpty(s.Employee.WorkStartDate) && string.IsNullOrEmpty(s.Employee.EndWorkDate)).Select(x => new CustodyVM
            {
                CustodyId = x.CustodyId,
                EmployeeId = x.EmployeeId,
                ItemId = x.ItemId,
                Date = x.Date,
                HijriDate = x.HijriDate,
                Quantity = x.Quantity ?? 0,
                Type = x.Type,
                BranchId = x.BranchId,
                ItemName = x.Type == 2 ? "عهدة مالية" : x.Item.NameAr,
                ItemPrice = x.Type == 2 ? x.CustodyValue : x.Item.Price,
                EmployeeName = x.Employee.EmployeeNameAr,
                Status = x.Status,
                ConvertStatus = x.ConvertStatus ?? false,
                InvoiceId = x.InvoiceId,
                InvoiceNumber = x.Invoices != null ? x.Invoices.InvoiceNumber.ToString() : "",

            }).Distinct().ToList();
            return custody;
        }
        public async Task<IEnumerable<CustodyVM>> SearchCustody(CustodyVM CustodySearch, string lang, int BranchId)
        {
            var custody = _TaamerProContext.Custody.Where(s => s.IsDeleted == false && s.Status == false && s.BranchId == BranchId && (!s.EmployeeId.HasValue || (s.EmployeeId.HasValue && !s.Employee.IsDeleted &&
            !string.IsNullOrEmpty(s.Employee.WorkStartDate) && string.IsNullOrEmpty(s.Employee.EndWorkDate)))).Select(x => new CustodyVM
            {
                CustodyId = x.CustodyId,
                EmployeeId = x.EmployeeId,
                ItemId = x.ItemId,
                Date = x.Date,
                HijriDate = x.HijriDate,
                Quantity = x.Quantity ?? 0,
                Type = x.Type,
                BranchId = x.BranchId,
                ItemName = x.Type == 2 ? "عهدة مالية" : x.Item.NameAr,
                ItemPrice = x.Type == 2 ? x.CustodyValue : x.Item.Price,
                EmployeeName = x.Employee.EmployeeNameAr,
                Status = x.Status,
                ConvertStatus = x.ConvertStatus ?? false,
                InvoiceId = x.InvoiceId,
                InvoiceNumber = x.Invoices != null ? x.Invoices.InvoiceNumber.ToString() : "",

            }).ToList();
           

            if (!String.IsNullOrWhiteSpace(Convert.ToString( CustodySearch.EmployeeId))) {
                custody = custody.Where(w => w.EmployeeId == CustodySearch.EmployeeId).ToList();
            }
            if (!String.IsNullOrWhiteSpace(Convert.ToString(CustodySearch.ItemId)))
            {
                custody = custody.Where(w => w.ItemId == CustodySearch.ItemId).ToList();
            }
          
            return custody;
        }

        public async Task<IEnumerable<CustodyVM>> SearchCustodyVoucher(CustodyVM CustodySearch, string lang, int BranchId)
        {
            var custody = _TaamerProContext.Custody.Where(s => s.IsDeleted == false && s.Status == false && s.ConvertStatus==true && s.Type == 2 && (!s.EmployeeId.HasValue || (s.EmployeeId.HasValue && !s.Employee.IsDeleted &&
            !string.IsNullOrEmpty(s.Employee.WorkStartDate) && string.IsNullOrEmpty(s.Employee.EndWorkDate)))).Select(x => new CustodyVM
            {
                CustodyId = x.CustodyId,
                EmployeeId = x.EmployeeId,
                ItemId = x.ItemId,
                Date = x.Date,
                HijriDate = x.HijriDate,
                Quantity = x.Quantity ?? 0,
                Type = x.Type,
                BranchId = x.BranchId,
                ItemName = x.Type == 2 ? "عهدة مالية" : x.Item.NameAr,
                ItemPrice = x.Type == 2 ? x.CustodyValue : x.Item.Price,
                EmployeeName = x.Employee.EmployeeNameAr,
                Status = x.Status,
                ConvertStatus = x.ConvertStatus ?? false,
                InvoiceId = x.InvoiceId,
                InvoiceNumber = x.Invoices != null ? x.Invoices.InvoiceNumber.ToString() : "",
                IsPost = x.Invoices != null ? x.Invoices.IsPost : false,

            }).ToList();


            if (!String.IsNullOrWhiteSpace(Convert.ToString(CustodySearch.EmployeeId)))
            {
                custody = custody.Where(w => w.EmployeeId == CustodySearch.EmployeeId).ToList();
            }
            if (!String.IsNullOrWhiteSpace(Convert.ToString(CustodySearch.ItemId)))
            {
                custody = custody.Where(w => w.ItemId == CustodySearch.ItemId).ToList();
            }

            return custody;
        }

        public async Task<IEnumerable<CustodyVM>> GetSomeCustody(int BranchId,bool Status)
        {
            //&& s.Status==false
            var custody = _TaamerProContext.Custody.Where(s => s.IsDeleted == false && s.Status==false && s.BranchId == BranchId  && ( !s.EmployeeId.HasValue || (s.EmployeeId.HasValue && !s.Employee.IsDeleted &&
            !string.IsNullOrEmpty(s.Employee.WorkStartDate) && string.IsNullOrEmpty(s.Employee.EndWorkDate)))).Select(x => new CustodyVM
            {
                CustodyId = x.CustodyId,
                EmployeeId = x.EmployeeId,
                ItemId = x.ItemId,
                Date = x.Date,
                HijriDate = x.HijriDate,
                Quantity = x.Quantity??0,
                Type = x.Type,
                BranchId = x.BranchId,
                ItemName = x.Type == 2 ? "عهدة مالية" : x.Item.NameAr,
                ItemPrice = x.Type == 2 ? x.CustodyValue : x.Item.Price,
                EmployeeName = x.Employee.EmployeeNameAr,
                Status = x.Status,
                ConvertStatus = x.ConvertStatus ?? false,
                InvoiceId = x.InvoiceId,
                InvoiceNumber = x.Invoices != null ? x.Invoices.InvoiceNumber.ToString() : "",

            }).ToList();
            return custody;
        }

        public async Task<IEnumerable<CustodyVM>> GetSomeCustodyVoucher(int BranchId, bool Status)
        {
            var custody = _TaamerProContext.Custody.Where(s => s.IsDeleted == false && s.Status == false  && s.ConvertStatus == true && s.Type==2 &&  (!s.EmployeeId.HasValue || (s.EmployeeId.HasValue && !s.Employee.IsDeleted &&
            !string.IsNullOrEmpty(s.Employee.WorkStartDate) && string.IsNullOrEmpty(s.Employee.EndWorkDate)))).Select(x => new CustodyVM
            {
                CustodyId = x.CustodyId,
                EmployeeId = x.EmployeeId,
                ItemId = x.ItemId,
                Date = x.Date,
                HijriDate = x.HijriDate,
                Quantity = x.Quantity ?? 0,
                Type = x.Type,
                BranchId = x.BranchId,
                ItemName = x.Type == 2 ? "عهدة مالية" : x.Item.NameAr,
                ItemPrice = x.Type == 2 ? x.CustodyValue : x.Item.Price,
                EmployeeName = x.Employee.EmployeeNameAr,
                Status = x.Status,
                ConvertStatus = x.ConvertStatus ?? false,
                InvoiceId = x.InvoiceId,
                InvoiceNumber = x.Invoices != null ? x.Invoices.InvoiceNumber.ToString() : "",
                IsPost = x.Invoices != null ? x.Invoices.IsPost : false,
            }).ToList();
            return custody;
        }


        public async Task<EmployeesVM> GetEmployeeByItemId (int Item,int BranchId)
        {
            var custody = _TaamerProContext.Custody.Where(s => s.IsDeleted == false && s.ItemId == Item && s.BranchId == BranchId).FirstOrDefault();
            if (custody != null)
            {
                return new EmployeesVM
                {
                    EmployeeId = custody.EmployeeId ?? 0,
                };
            }
            else
            {
                return new EmployeesVM();
            }
        }


        public async Task<IEnumerable<CustodyVM>> GetSomeCustodyByEmployeeId(int EmployeeId, bool Status)
        {
            var custody = _TaamerProContext.Custody.Where(s => s.IsDeleted == false && s.Status == false && s.EmployeeId== EmployeeId).Select(x => new CustodyVM
            {
                CustodyId = x.CustodyId,
                EmployeeId = x.EmployeeId,
                ItemId = x.ItemId,
                Date = x.Date,
                HijriDate = x.HijriDate,
                Quantity = x.Quantity ?? 0,
                Type = x.Type,
                BranchId = x.BranchId,
                ItemName = x.Type == 2 ? "عهدة مالية" : x.Item.NameAr,
                ItemPrice = x.Type == 2 ? x.CustodyValue : x.Item.Price,
                EmployeeName = x.Employee.EmployeeNameAr,
                Status = x.Status,
                ConvertStatus = x.ConvertStatus ?? false,
                InvoiceId = x.InvoiceId,
                InvoiceNumber = x.Invoices != null ? x.Invoices.InvoiceNumber.ToString() : "",
                IsPost = x.Invoices != null ? x.Invoices.IsPost : false,
                CustodyValue=x.CustodyValue,
            }).ToList();
            return custody;
        }


        public async Task<IEnumerable<CustodyVM>> GetCustodiesByEmployeeId(int EmployeeId)
        {
            var custody = _TaamerProContext.Custody.Where(s => s.IsDeleted == false && s.Status == false && s.EmployeeId == EmployeeId
                && s.Type==2).Select(x => new CustodyVM
            {
                CustodyId = x.CustodyId,
                EmployeeId = x.EmployeeId,
                ItemId = x.ItemId,
                Date = x.Date,
                HijriDate = x.HijriDate,
                Quantity = x.Quantity ?? 0,
                Type = x.Type,
                BranchId = x.BranchId,
                ItemName = x.Type == 2 ? "عهدة مالية" : x.Item.NameAr,
                ItemPrice = x.Type == 2 ? x.CustodyValue : x.Item.Price,
                EmployeeName = x.Employee.EmployeeNameAr,
                Status = x.Status,
                ConvertStatus = x.ConvertStatus ?? false,
                InvoiceId = x.InvoiceId,
                InvoiceNumber = x.Invoices != null ? x.Invoices.InvoiceNumber.ToString() : "",
                IsPost = x.Invoices != null ? x.Invoices.IsPost : false,
                CustodyValue = x.CustodyValue,
            }).ToList();
            return custody;
        }

        public Custody Add(Custody entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Custody> AddRange(IEnumerable<Custody> entities)
        {
            throw new NotImplementedException();
        }

        public bool Exists(int Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Custody> GetAll()
        {
            throw new NotImplementedException();
        }

        public Custody GetById(int Id)
        {
            return _TaamerProContext.Custody.Where(x => x.CustodyId == Id).FirstOrDefault();
        }

        public IEnumerable<Custody> GetMatching(Func<Custody, bool> where)
        {
            return _TaamerProContext.Custody.Where(where).ToList<Custody>();

        }

        public void Remove(Custody entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(int Id)
        {
            throw new NotImplementedException();
        }

        public void RemoveMatching(Func<Custody, bool> where)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<Custody> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(Custody entityToUpdate)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Custody> Queryable()
        {
            throw new NotImplementedException();
        }
    }
}


