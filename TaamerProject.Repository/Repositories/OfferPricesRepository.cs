using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace TaamerProject.Repository.Repositories
{
    public class OfferPricesRepository : IOffersPricesRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;


        public OfferPricesRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }


        public async Task<IEnumerable<OffersPricesVM>> GetAllOffers(int BranchId)
        {
            var Offers = _TaamerProContext.OffersPrices.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select(x => new OffersPricesVM
            {
                OffersPricesId=x.OffersPricesId,
                CustomerId =x.CustomerId??0,
                //CustomerName = x.CustomerName==null?x.Customer!=null?x.Customer.CustomerNameAr:"":x.CustomerName,
                CustomerName = x.CustomerName == null ? x.Customer != null ? x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameEn + "(**)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "" : x.CustomerName,
                CustomerName2 = x.CustomerName,
                OfferDate=x.OfferDate,
                OfferNo=x.OfferNo,
                Department=x.Department,
                BranchId=x.BranchId,
                CustomerStatus=x.CustomerStatus??0,
                OfferStatus=x.OfferStatus??0,
                OfferValue=x.OfferValue,
                OfferValueTxt=x.OfferValueTxt,
                ServiceId=x.ServiceId,
                UserId=x.UserId,
                OfferHijriDate =x.OfferHijriDate,
                Presenter =x.Users!=null?x.Users.FullNameAr==null?x.Users.FullName:x.Users.FullNameAr:"",
                ServQty=x.ServQty??1,
                RememberDate = x.RememberDate ?? "",
                OfferAlarmCheck = x.OfferAlarmCheck ?? false,
                ISsent = x.ISsent ?? 0,
                OfferNoType=x.OfferNoType??0,
                projectno=x.Project!.ProjectNo??"",
                projecttime= (x.Project.NoOfDays < 30) ? x.Project.NoOfDays + " يوم " : (x.Project.NoOfDays == 30) ? x.Project.NoOfDays / 30 + " شهر " : (x.Project.NoOfDays > 30) ? ((x.Project.NoOfDays / 30) + " شهر " + (x.Project.NoOfDays % 30) + " يوم  ") : "",
               IsContainLogo =x.IsContainLogo ?? false,
                IsContainSign =x.IsContainSign ?? false,
                printBankAccount=x.printBankAccount??false,
                CustomerEmail=x.CustomerEmail ?? "",
                Customerphone=x.Customerphone ?? "",
                CUstomerName_EN=x.CUstomerName_EN ??"",
                IsEnglish=x.IsEnglish??0,
                Description=x.Description??"",
                Introduction=x.Introduction??"",
                NickName=x.NickName??"",
                setIntroduction=x.setIntroduction??0,
                NotDisCustPrint = x.NotDisCustPrint ?? 0,
                CustomerMailCode = x.CustomerMailCode ?? 0,
                ProjectId=x.ProjectId ?? 0,
                IsCertified=x.IsCertified??false,
                ImplementationDuration=x.ImplementationDuration??0,
                OfferValidity=x.OfferValidity??0,
                ProjectName=x.ProjectName??"",

            }).ToList();
     
            return Offers;


        }


        public async Task<IEnumerable<OffersPricesVM>> Getofferconestintroduction(int BranchId)
        {
            var Offers = _TaamerProContext.OffersPrices.Where(s => s.IsDeleted == false && s.setIntroduction==1).Select(x => new OffersPricesVM
            {
                OffersPricesId = x.OffersPricesId,
               setIntroduction=x.setIntroduction,
               Introduction=x.Introduction??"",

            }).ToList();

            return Offers;


        }

        public async Task<IEnumerable<OffersPricesVM>> GetOfferByid(int offerid)
        {
            var Offers = _TaamerProContext.OffersPrices.Where(s => s.IsDeleted == false && s.OffersPricesId== offerid).Select(x => new OffersPricesVM
            {
                OffersPricesId = x.OffersPricesId,
                CustomerId = x.CustomerId ?? 0,
                //CustomerName = x.CustomerName == null ? x.Customer != null ? x.Customer.CustomerNameAr : "" : x.CustomerName,
                CustomerName = x.CustomerName == null ? x.Customer != null ? x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameEn + "(**)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "" : x.CustomerName,

                CustomerName2 = x.CustomerName??"",
                OfferDate = x.OfferDate,
                OfferNo = x.OfferNo,
                Department = x.Department,
                BranchId = x.BranchId,
                CustomerStatus = x.CustomerStatus ?? 0,
                OfferStatus = x.OfferStatus ?? 0,
                OfferValue = x.OfferValue,
                OfferValueTxt = x.OfferValueTxt,
                ServiceId = x.ServiceId,
                UserId = x.UserId,
                OfferHijriDate = x.OfferHijriDate,
                Presenter = x.Users != null ? x.Users.FullNameAr == null ? x.Users.FullName : x.Users.FullNameAr : "",
                PresenterEN= x.Users != null ? x.Users.FullName == null ? x.Users.FullNameAr : x.Users.FullName : "",
                ServQty = x.ServQty ?? 1,
                RememberDate = x.RememberDate ?? "",
                OfferAlarmCheck = x.OfferAlarmCheck ?? false,
                ISsent = x.ISsent ?? 0,
                OfferNoType = x.OfferNoType ?? 0,
                projectno = x.Project!.ProjectNo ?? "",
                projecttime = (x.Project.NoOfDays < 30) ? x.Project.NoOfDays + " يوم " : (x.Project.NoOfDays == 30) ? x.Project.NoOfDays / 30 + " شهر " : (x.Project.NoOfDays > 30) ? ((x.Project.NoOfDays / 30) + " شهر " + (x.Project.NoOfDays % 30) + " يوم  ") : "",

                IsContainLogo = x.IsContainLogo ?? false,
                IsContainSign = x.IsContainSign ?? false,
                printBankAccount = x.printBankAccount ?? false,
                CustomerEmail = x.CustomerEmail ?? "",
                Customerphone = x.Customerphone ?? "",
                CUstomerName_EN = x.CUstomerName_EN ?? "",
                IsEnglish = x.IsEnglish ?? 0,
                Description = x.Description ?? "",
                Introduction = x.Introduction ?? "",
                NickName = x.NickName ?? "",
                setIntroduction = x.setIntroduction ?? 0,
                NotDisCustPrint = x.NotDisCustPrint ?? 0,
                CustomerMailCode = x.CustomerMailCode ?? 0,
                ProjectId = x.ProjectId ?? 0,
                IsCertified = x.IsCertified ?? false,
                ImplementationDuration = x.ImplementationDuration ?? 0,
                OfferValidity = x.OfferValidity ?? 0,
                ProjectName = x.ProjectName ?? "",

            }).ToList();
            return Offers;


        }

        public async Task<IEnumerable<OffersPricesVM>> GetOfferByCustomerData(int offerid, string NationalId, int ActivationCode)
        {
            var Offers = _TaamerProContext.OffersPrices.Where(s => s.IsDeleted == false && s.OffersPricesId == offerid && s.CustomerMailCode== ActivationCode && ((s.Customer!.CustomerNationalId??"")== NationalId || s.Customer==null)).Select(x => new OffersPricesVM
            {
                OffersPricesId = x.OffersPricesId,
                //CustomerId = x.CustomerId ?? 0,
                ////CustomerName = x.CustomerName == null ? x.Customer != null ? x.Customer.CustomerNameAr : "" : x.CustomerName,
                //CustomerName = x.CustomerName == null ? x.Customer != null ? x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameEn + "(**)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "" : x.CustomerName,

                //CustomerName2 = x.CustomerName ?? "",
                //OfferDate = x.OfferDate,
                //OfferNo = x.OfferNo,
                //Department = x.Department,
                //BranchId = x.BranchId,
                //CustomerStatus = x.CustomerStatus ?? 0,
                //OfferStatus = x.OfferStatus ?? 0,
                //OfferValue = x.OfferValue,
                //OfferValueTxt = x.OfferValueTxt,
                //ServiceId = x.ServiceId,
                //UserId = x.UserId,
                //OfferHijriDate = x.OfferHijriDate,
                //Presenter = x.Users != null ? x.Users.FullNameAr == null ? x.Users.FullName : x.Users.FullNameAr : "",
                //PresenterEN = x.Users != null ? x.Users.FullName == null ? x.Users.FullNameAr : x.Users.FullName : "",
                //ServQty = x.ServQty ?? 1,
                //RememberDate = x.RememberDate ?? "",
                //OfferAlarmCheck = x.OfferAlarmCheck ?? false,
                //ISsent = x.ISsent ?? 0,
                //OfferNoType = x.OfferNoType ?? 0,
                //projectno = x.Project.ProjectNo ?? "",
                //projecttime = (x.Project.NoOfDays < 30) ? x.Project.NoOfDays + " يوم " : (x.Project.NoOfDays == 30) ? x.Project.NoOfDays / 30 + " شهر " : (x.Project.NoOfDays > 30) ? ((x.Project.NoOfDays / 30) + " شهر " + (x.Project.NoOfDays % 30) + " يوم  ") : "",

                //IsContainLogo = x.IsContainLogo ?? false,
                //IsContainSign = x.IsContainSign ?? false,
                //printBankAccount = x.printBankAccount ?? false,
                //CustomerEmail = x.CustomerEmail ?? "",
                //Customerphone = x.Customerphone ?? "",
                //CUstomerName_EN = x.CUstomerName_EN ?? "",
                //IsEnglish = x.IsEnglish ?? 0,
                //Description = x.Description ?? "",
                //Introduction = x.Introduction ?? "",
                //NickName = x.NickName ?? "",
                //setIntroduction = x.setIntroduction ?? 0,
                //NotDisCustPrint = x.NotDisCustPrint ?? 0,
                CustomerMailCode = x.CustomerMailCode ?? 0,


            }).ToList();
            return Offers;


        }

        public async Task<IEnumerable<OffersPricesVM>> GetOfferByid2(int offerid)
        {
            var Offers = _TaamerProContext.OffersPrices.Where(s => s.IsDeleted == false && s.OffersPricesId == offerid).Select(x => new OffersPricesVM
            {
                OffersPricesId = x.OffersPricesId,
                CustomerId = x.CustomerId ?? 0,
                //CustomerName = x.CustomerName == null ? x.Customer != null ? x.Customer.CustomerNameAr : "" : x.CustomerName,
                CustomerName = x.CustomerName == null ? x.Customer != null ? x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameEn + "(**)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "" : x.CustomerName,

                CustomerName2 = x.CustomerName ?? "",
                OfferDate = x.OfferDate,
                OfferNo = x.OfferNo,
                Department = x.Department,
                BranchId = x.BranchId,
                CustomerStatus = x.CustomerStatus ?? 0,
                OfferStatus = x.OfferStatus ?? 0,
                OfferValue = x.OfferValue,
                OfferValueTxt = x.OfferValueTxt,
                ServiceId = x.ServiceId,
                UserId = x.UserId,
                OfferHijriDate = x.OfferHijriDate,
                Presenter = x.Users != null ? x.Users.FullNameAr == null ? x.Users.FullName : x.Users.FullNameAr : "",
                PresenterEN = x.Users != null ? x.Users.FullName == null ? x.Users.FullNameAr : x.Users.FullName : "",
                ServQty = x.ServQty ?? 1,
                RememberDate = x.RememberDate ?? "",
                OfferAlarmCheck = x.OfferAlarmCheck ?? false,
                ISsent = x.ISsent ?? 0,
                OfferNoType = x.OfferNoType ?? 0,
                projectno = x.Project!.ProjectNo ?? "",
                projecttime = (x.Project.NoOfDays < 30) ? x.Project.NoOfDays + " يوم " : (x.Project.NoOfDays == 30) ? x.Project.NoOfDays / 30 + " شهر " : (x.Project.NoOfDays > 30) ? ((x.Project.NoOfDays / 30) + " شهر " + (x.Project.NoOfDays % 30) + " يوم  ") : "",

                IsContainLogo = x.IsContainLogo ?? false,
                IsContainSign = x.IsContainSign ?? false,
                printBankAccount = x.printBankAccount ?? false,
                CustomerEmail = x.CustomerEmail ?? "",
                Customerphone = x.Customerphone ?? "",
                CUstomerName_EN = x.CUstomerName_EN ?? "",
                IsEnglish = x.IsEnglish ?? 0,
                Description = x.Description ?? "المحترمين",
                Introduction = x.Introduction ?? "نتشرف بتقديم عرض السعر  ، آملين أن ينال رضاكم واستحسانكم ، وفي حال وجود أي استفسار حول العرض لا تتردو في التواصل معنا",
                NickName = x.NickName ?? "السادة",
                setIntroduction = x.setIntroduction ?? 0,
                NotDisCustPrint = x.NotDisCustPrint ?? 0,
                CustomerMailCode = x.CustomerMailCode ?? 0,
                ProjectId = x.ProjectId ?? 0,
                IsCertified = x.IsCertified ?? false,
                ImplementationDuration = x.ImplementationDuration ?? 0,
                OfferValidity = x.OfferValidity ?? 0,
                ProjectName = x.ProjectName ?? "",

            }).ToList();
            return Offers;


        }
        public async Task<IEnumerable<OffersPricesVM>> GetOfferByCustomerId(int Customerid)
        {
            var Offers = _TaamerProContext.OffersPrices.Where(s => s.IsDeleted == false && s.CustomerId == Customerid).Select(x => new OffersPricesVM
            {
                OffersPricesId = x.OffersPricesId,
                CustomerId = x.CustomerId ?? 0,
                //CustomerName = x.CustomerName == null ? x.Customer != null ? x.Customer.CustomerNameAr : "" : x.CustomerName,
                CustomerName = x.CustomerName == null ? x.Customer != null ? x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameEn + "(**)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "" : x.CustomerName,

                OfferDate = x.OfferDate,
                OfferNo = x.OfferNo,
                Department = x.Department,
                BranchId = x.BranchId,
                CustomerStatus = x.CustomerStatus ?? 0,
                OfferStatus = x.OfferStatus ?? 0,
                OfferValue = x.OfferValue,
                OfferValueTxt = x.OfferValueTxt,
                ServiceId = x.ServiceId,
                UserId = x.UserId,
                OfferHijriDate = x.OfferHijriDate,
                Presenter = x.Users != null ? x.Users.FullNameAr == null ? x.Users.FullName : x.Users.FullNameAr : "",
                ServQty = x.ServQty ?? 1,
                RememberDate = x.RememberDate ?? "",
                OfferAlarmCheck = x.OfferAlarmCheck ?? false,
                ISsent = x.ISsent ?? 0,
                OfferNoType = x.OfferNoType ?? 0,
                projectno = x.Project!.ProjectNo ?? "",
                projecttime = (x.Project.NoOfDays < 30) ? x.Project.NoOfDays + " يوم " : (x.Project.NoOfDays == 30) ? x.Project.NoOfDays / 30 + " شهر " : (x.Project.NoOfDays > 30) ? ((x.Project.NoOfDays / 30) + " شهر " + (x.Project.NoOfDays % 30) + " يوم  ") : "",
                IsContainLogo = x.IsContainLogo ?? false,
                IsContainSign = x.IsContainSign ?? false,
                printBankAccount = x.printBankAccount ?? false,
                CustomerEmail = x.CustomerEmail ?? "",
                Customerphone = x.Customerphone ?? "",
                CUstomerName_EN = x.CUstomerName_EN ?? "",
                IsEnglish = x.IsEnglish ?? 0,
                Description = x.Description ?? "",
                Introduction = x.Introduction ?? "",
                NickName = x.NickName ?? "",
                setIntroduction = x.setIntroduction ?? 0,
                NotDisCustPrint = x.NotDisCustPrint ?? 0,
                CustomerMailCode = x.CustomerMailCode ?? 0,
                ProjectId = x.ProjectId ?? 0,
                IsCertified = x.IsCertified ?? false,
                ImplementationDuration = x.ImplementationDuration ?? 0,
                OfferValidity = x.OfferValidity ?? 0,
                ProjectName = x.ProjectName ?? "",

            }).ToList();
            return Offers;


        }
        public async Task<IEnumerable<OffersPricesVM>> GetAllOffersByProjectId(int ProjectId)
        {
            var Offers = _TaamerProContext.OffersPrices.Where(s => s.IsDeleted == false && s.ProjectId == ProjectId).Select(x => new OffersPricesVM
            {
                OffersPricesId = x.OffersPricesId,
                CustomerId = x.CustomerId ?? 0,
                //CustomerName = x.CustomerName == null ? x.Customer != null ? x.Customer.CustomerNameAr : "" : x.CustomerName,
                CustomerName = x.CustomerName == null ? x.Customer != null ? x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameEn + "(**)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "" : x.CustomerName,

                OfferDate = x.OfferDate,
                OfferNo = x.OfferNo,
                Department = x.Department,
                BranchId = x.BranchId,
                CustomerStatus = x.CustomerStatus ?? 0,
                OfferStatus = x.OfferStatus ?? 0,
                OfferValue = x.OfferValue,
                OfferValueTxt = x.OfferValueTxt,
                ServiceId = x.ServiceId,
                UserId = x.UserId,
                OfferHijriDate = x.OfferHijriDate,
                Presenter = x.Users != null ? x.Users.FullNameAr == null ? x.Users.FullName : x.Users.FullNameAr : "",
                ServQty = x.ServQty ?? 1,
                RememberDate = x.RememberDate ?? "",
                OfferAlarmCheck = x.OfferAlarmCheck ?? false,
                ISsent = x.ISsent ?? 0,
                OfferNoType = x.OfferNoType ?? 0,
                projectno = x.Project!.ProjectNo ?? "",
                projecttime = (x.Project.NoOfDays < 30) ? x.Project.NoOfDays + " يوم " : (x.Project.NoOfDays == 30) ? x.Project.NoOfDays / 30 + " شهر " : (x.Project.NoOfDays > 30) ? ((x.Project.NoOfDays / 30) + " شهر " + (x.Project.NoOfDays % 30) + " يوم  ") : "",
                IsContainLogo = x.IsContainLogo ?? false,
                IsContainSign = x.IsContainSign ?? false,
                printBankAccount = x.printBankAccount ?? false,
                CustomerEmail = x.CustomerEmail ?? "",
                Customerphone = x.Customerphone ?? "",
                CUstomerName_EN = x.CUstomerName_EN ?? "",
                IsEnglish = x.IsEnglish ?? 0,
                Description = x.Description ?? "",
                Introduction = x.Introduction ?? "",
                NickName = x.NickName ?? "",
                setIntroduction = x.setIntroduction ?? 0,
                NotDisCustPrint = x.NotDisCustPrint ?? 0,
                CustomerMailCode = x.CustomerMailCode ?? 0,
                ProjectId = x.ProjectId ?? 0,
                IsCertified = x.IsCertified ?? false,
                ImplementationDuration = x.ImplementationDuration ?? 0,
                OfferValidity = x.OfferValidity ?? 0,
                ProjectName = x.ProjectName ?? "",

            }).ToList();
            return Offers;


        }

        public async Task< IEnumerable<OffersPricesVM>> GetOfferPrice_Search(string offerno, string date, string customername, string presenter, decimal? Amount, int BranchId)
        {
            var result = _TaamerProContext.OffersPrices.Where(x => x.IsDeleted == false && x.BranchId==BranchId &&
            ((!string.IsNullOrEmpty(offerno) && x.OfferNo.Contains(offerno)) || (string.IsNullOrEmpty(offerno) && true)) &&
            ((!string.IsNullOrEmpty(date) && x.OfferDate.Contains(date)) || (string.IsNullOrEmpty(date) && true)) &&
            ((!string.IsNullOrEmpty(customername) && x.CustomerName.Contains(customername) )|| (string.IsNullOrEmpty(customername) && true)) &&
            
            ((Amount.HasValue && Amount.Value == x.OfferValue) || (!Amount.HasValue && true))).Select(x => new OffersPricesVM
            {
                OffersPricesId = x.OffersPricesId,
                CustomerId = x.CustomerId??0,
                //CustomerName = x.CustomerName == null ? x.Customer != null ? x.Customer.CustomerNameAr : "" : x.CustomerName,
                CustomerName = x.CustomerName == null ? x.Customer != null ? x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 0 ? x.Customer.CustomerNameAr : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 1 ? x.Customer.CustomerNameAr : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 2 ? x.Customer.CustomerNameAr + "(*)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 3 ? x.Customer.CustomerNameEn + "(**)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() == 4 ? x.Customer.CustomerNameAr + "(***)" : x.Customer.Projects.Where(p => p.IsDeleted == false).Count() >= 5 ? x.Customer.CustomerNameAr + "(VIP)" : x.Customer.CustomerNameAr : "" : x.CustomerName,

                OfferDate = x.OfferDate,
                OfferNo = x.OfferNo,
                Department = x.Department,
                BranchId = x.BranchId,
                CustomerStatus = x.CustomerStatus ?? 0,
                OfferStatus = x.OfferStatus ?? 0,
                OfferValue = x.OfferValue,
                OfferValueTxt = x.OfferValueTxt,
                ServiceId = x.ServiceId,
                UserId = x.UserId,
                OfferHijriDate = x.OfferHijriDate,
                Presenter = x.Users != null ? x.Users.FullNameAr == null ? x.Users.FullName : x.Users.FullNameAr : "",
                ServQty = x.ServQty ?? 1,
                RememberDate = x.RememberDate ?? "",
                OfferAlarmCheck = x.OfferAlarmCheck ?? false,
                ISsent = x.ISsent ?? 0,
                OfferNoType = x.OfferNoType ?? 0,
                projectno = x.Project!.ProjectNo ?? "",
                projecttime = (x.Project.NoOfDays < 30) ? x.Project.NoOfDays + " يوم " : (x.Project.NoOfDays == 30) ? x.Project.NoOfDays / 30 + " شهر " : (x.Project.NoOfDays > 30) ? ((x.Project.NoOfDays / 30) + " شهر " + (x.Project.NoOfDays % 30) + " يوم  ") : "",
                IsContainLogo = x.IsContainLogo ?? false,
                IsContainSign = x.IsContainSign ?? false,
                printBankAccount = x.printBankAccount ?? false,
                CustomerEmail = x.CustomerEmail ?? "",
                Customerphone = x.Customerphone ?? "",
                CUstomerName_EN = x.CUstomerName_EN ?? "",
                IsEnglish = x.IsEnglish ?? 0,
                Description = x.Description ?? "",
                Introduction = x.Introduction ?? "",
                NickName = x.NickName ?? "",
                setIntroduction = x.setIntroduction ?? 0,
                NotDisCustPrint = x.NotDisCustPrint ?? 0,
                CustomerMailCode = x.CustomerMailCode ?? 0,
                ProjectId = x.ProjectId ?? 0,
                IsCertified = x.IsCertified ?? false,
                ImplementationDuration = x.ImplementationDuration ?? 0,
                OfferValidity = x.OfferValidity ?? 0,
                ProjectName = x.ProjectName ?? "",


            }).OrderByDescending(x => x.OffersPricesId).ToList();

            return result;
        }


        //public async Task<int> GenerateNextOfferNumber(int BranchId)
        //{
        //    var offer = _TaamerProContext.OffersPrices.Where(s => s.IsDeleted == false && s.OfferNoType==1);
        //    if (offer != null)
        //    {
        //        var lastRow = offer.OrderByDescending(u => u.OffersPricesId).Take(1).FirstOrDefault();
        //        if (lastRow != null)
        //        {
        //            var last =  int.Parse(lastRow.OfferNo);
        //            return int.Parse(lastRow.OfferNo) + 1;
        //        }
        //        else
        //        {
        //            return 1;
        //        }
        //    }
        //    else
        //    {
        //        return 1;
        //    }
        //}

        public async Task<int> GenerateNextOfferNumber(int BranchId, string codePrefix)
        {
            if (_TaamerProContext.OffersPrices != null)
            {
                var lastRow = _TaamerProContext.OffersPrices.Where(s => s.IsDeleted == false && s.OfferNoType == 1 && s.OfferNo!.Contains(codePrefix)).OrderByDescending(u => u.OffersPricesId).Take(1).FirstOrDefault();
                if (lastRow != null)
                {
                    try
                    {

                        var OfferNumber = 0;

                        if (codePrefix == "")
                        {
                            OfferNumber = int.Parse(lastRow!.OfferNo!) + 1;
                        }
                        else
                        {
                            OfferNumber = int.Parse(lastRow!.OfferNo!.Replace(codePrefix, "").Trim()) + 1;
                        }

                        return OfferNumber;
                    }
                    catch (Exception)
                    {
                        return 1;
                    }
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 1;
            }
        }


        public virtual OffersPrices GetById(int Id)
        {
            return _TaamerProContext.OffersPrices.Where(x=>x.OffersPricesId== Id).FirstOrDefault();
        }

        public virtual IEnumerable<OffersPrices> GetAll()
        {
            return _TaamerProContext.OffersPrices.ToList<OffersPrices>();
        }

        public virtual IEnumerable<OffersPrices> GetMatching(Func<OffersPrices, bool> where)
        {
            return _TaamerProContext.OffersPrices.Where(where).ToList<OffersPrices>();
        }
    }
}
