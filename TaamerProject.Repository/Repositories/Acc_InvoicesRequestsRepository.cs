using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;

namespace TaamerProject.Repository.Repositories
{
    public class Acc_InvoicesRequestsRepository: IAcc_InvoicesRequestsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public Acc_InvoicesRequestsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task<Acc_InvoicesRequestsVM> GetInvoiceReqByReqId(int InvoiceReqId)
        {
            Acc_InvoicesRequestsVM req = new Acc_InvoicesRequestsVM();

            try
            {
                var InvRequest = _TaamerProContext.Acc_InvoicesRequests.Where(s => s.InvoiceReqId == InvoiceReqId).Select(x => new Acc_InvoicesRequestsVM
                {
                    InvoiceReqId = x.InvoiceReqId,
                    InvoiceId = x.InvoiceId,
                    InvoiceNoRequest = x.InvoiceNoRequest,
                    IsSent = x.IsSent,
                    StatusCode = x.StatusCode,
                    SendingStatus = x.SendingStatus,
                    warningmessage = x.warningmessage,
                    ClearedInvoice = x.ClearedInvoice,
                    errormessage = x.errormessage,
                    InvoiceHash = x.InvoiceHash,
                    SingedXML = x.SingedXML,
                    EncodedInvoice = x.EncodedInvoice,
                    ZatcaUUID = x.ZatcaUUID,
                    QRCode = x.QRCode,
                    PIH = x.PIH,
                    SingedXMLFileName = x.SingedXMLFileName,
                    BranchId = x.BranchId,
                    InvoiceNo = x.Invoice != null ? x.Invoice.InvoiceNumber ?? null : null,
                    InvoiceValue = x.Invoice != null ? x.Invoice.InvoiceValue ?? null : null,
                    TotalValue = x.Invoice != null ? x.Invoice.TotalValue ?? null : null,
                    Type = x.Type??0,
                    InvoiceRetId = x.Invoice != null ? x.Invoice.InvoiceRetId : null,
                    Rad = x.Invoice != null ? x.Invoice.Rad : false,
                    Date = x.Invoice != null ? x.Invoice.Date ?? null : null,
                    CustomerName = x.Invoice != null ? x.Invoice.Customer != null ? x.Invoice.Customer.CustomerNameAr ?? null : null : null,

                }).FirstOrDefault();
                return InvRequest;
            }
            catch (Exception ex)
            {
                return req;

            }
        }

        public async Task<Acc_InvoicesRequestsVM> GetInvoiceReq(int InvoiceId)
        {
            Acc_InvoicesRequestsVM req = new Acc_InvoicesRequestsVM();

            try
            {
                var InvRequest = _TaamerProContext.Acc_InvoicesRequests.Where(s => s.InvoiceId == InvoiceId && s.Type==2).Select(x => new Acc_InvoicesRequestsVM
                {
                    InvoiceReqId = x.InvoiceReqId,
                    InvoiceId = x.InvoiceId,
                    InvoiceNoRequest = x.InvoiceNoRequest,
                    IsSent = x.IsSent,
                    StatusCode = x.StatusCode,
                    SendingStatus = x.SendingStatus,
                    warningmessage = x.warningmessage,
                    ClearedInvoice = x.ClearedInvoice,
                    errormessage = x.errormessage,
                    InvoiceHash = x.InvoiceHash,
                    SingedXML = x.SingedXML,
                    //EncodedInvoice = x.EncodedInvoice,
                    ZatcaUUID = x.ZatcaUUID,
                    QRCode = x.QRCode,
                    PIH = x.PIH,
                    SingedXMLFileName = x.SingedXMLFileName,
                    BranchId = x.BranchId,
                    InvoiceNo = x.Invoice != null ? x.Invoice.InvoiceNumber ?? null : null,
                    InvoiceValue = x.Invoice != null ? x.Invoice.InvoiceValue ?? null : null,
                    TotalValue = x.Invoice != null ? x.Invoice.TotalValue ?? null : null,
                    Type = x.Type ?? 0,
                    InvoiceRetId = x.Invoice != null ? x.Invoice.InvoiceRetId : null,
                    Rad = x.Invoice != null ? x.Invoice.Rad : false,
                    Date = x.Invoice != null ? x.Invoice.Date ?? null : null,
                    CustomerName = x.Invoice != null ? x.Invoice.Customer != null ? x.Invoice.Customer.CustomerNameAr ?? null : null : null,

                }).FirstOrDefault();
                return InvRequest;
            }
            catch (Exception ex)
            {
                return req;

            }
        }
        public async Task<IEnumerable<Acc_InvoicesRequestsVM>> GetAllInvoiceRequests(int BranchId)
        {

            try
            {
                var InvRequest = _TaamerProContext.Acc_InvoicesRequests.Where(s => s.BranchId == BranchId).Select(x => new Acc_InvoicesRequestsVM
                {
                    InvoiceReqId = x.InvoiceReqId,
                    InvoiceId = x.InvoiceId,
                    InvoiceNoRequest = x.InvoiceNoRequest,
                    IsSent = x.IsSent,
                    StatusCode = x.StatusCode,
                    SendingStatus = x.SendingStatus,
                    warningmessage = x.warningmessage,
                    ClearedInvoice = x.ClearedInvoice,
                    errormessage = x.errormessage,
                    InvoiceHash = x.InvoiceHash,
                    SingedXML = x.SingedXML,
                    //EncodedInvoice = x.EncodedInvoice,
                    ZatcaUUID = x.ZatcaUUID,
                    QRCode = x.QRCode,
                    PIH = x.PIH,
                    SingedXMLFileName = x.SingedXMLFileName,
                    BranchId = x.BranchId,
                    InvoiceNo = x.Invoice != null ? x.Invoice.InvoiceNumber ?? null : null,
                    InvoiceValue = x.Invoice != null ? x.Invoice.InvoiceValue ?? null : null,
                    TotalValue = x.Invoice != null ? x.Invoice.TotalValue ?? null : null,
                    Type = x.Type ?? 0,
                    InvoiceRetId = x.Invoice != null ? x.Invoice.InvoiceRetId : null,
                    Rad = x.Invoice != null ? x.Invoice.Rad : false,
                    Date = x.Invoice != null ? x.Invoice.Date ?? null : null,
                    CustomerName = x.Invoice != null ? x.Invoice.Customer != null ? x.Invoice.Customer.CustomerNameAr ?? null : null:null,
                }).ToList();
                return InvRequest;
            }
            catch (Exception ex)
            {
                List<Acc_InvoicesRequestsVM> req = new List<Acc_InvoicesRequestsVM>();
                return req;

            }
        }
        public async Task<IEnumerable<Acc_InvoicesRequestsVM>> GetAllInvoiceRequests(int InvoiceId, int BranchId)
        {

            try
            {
                var InvRequest = _TaamerProContext.Acc_InvoicesRequests.Where(s => s.InvoiceId == InvoiceId && s.BranchId == BranchId).Select(x => new Acc_InvoicesRequestsVM
                {
                    InvoiceReqId = x.InvoiceReqId,
                    InvoiceId = x.InvoiceId,
                    InvoiceNoRequest = x.InvoiceNoRequest,
                    IsSent = x.IsSent,
                    StatusCode = x.StatusCode,
                    SendingStatus = x.SendingStatus,
                    warningmessage = x.warningmessage,
                    ClearedInvoice = x.ClearedInvoice,
                    errormessage = x.errormessage,
                    InvoiceHash = x.InvoiceHash,
                    SingedXML = x.SingedXML,
                    //EncodedInvoice = x.EncodedInvoice,
                    ZatcaUUID = x.ZatcaUUID,
                    QRCode = x.QRCode,
                    PIH = x.PIH,
                    SingedXMLFileName = x.SingedXMLFileName,
                    BranchId = x.BranchId,
                    InvoiceNo = x.Invoice != null ? x.Invoice.InvoiceNumber ?? null : null,
                    InvoiceValue = x.Invoice != null ? x.Invoice.InvoiceValue ?? null : null,
                    TotalValue = x.Invoice != null ? x.Invoice.TotalValue ?? null : null,
                    Type = x.Type ?? 0,
                    InvoiceRetId = x.Invoice != null ? x.Invoice.InvoiceRetId : null,
                    Rad = x.Invoice != null ? x.Invoice.Rad : false,
                    Date = x.Invoice != null ? x.Invoice.Date ?? null : null,
                    CustomerName = x.Invoice != null ? x.Invoice.Customer != null ? x.Invoice.Customer.CustomerNameAr ?? null : null : null,

                }).ToList();
                return InvRequest;
            }
            catch (Exception ex)
            {
                List<Acc_InvoicesRequestsVM> req = new List<Acc_InvoicesRequestsVM>();
                return req;

            }
        }

    }
}
