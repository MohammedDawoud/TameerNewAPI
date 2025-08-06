using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TaamerProject.Models.Common;

namespace TaamerProject.Service.Interfaces
{
    public interface IAcc_InvoicesRequestsService
    {
        Task<Acc_InvoicesRequestsVM> GetInvoiceReqByReqId(int InvoiceReqId);
        Task<Acc_InvoicesRequestsVM> GetInvoiceReq(int InvoiceId);
        Task<IEnumerable<Acc_InvoicesRequestsVM>> GetAllInvoiceRequests(int BranchId);
        Task<IEnumerable<Acc_InvoicesRequestsVM>> GetAllInvoiceRequests(int InvoiceId, int BranchId);


        GeneralMessage SaveInvoicesRequest(int InvoiceReqId, int InvoiceId,int Type, string InvoiceHash, string SingedXML, string EncodedInvoice
             , string ZatcaUUID, string QRCode, string PIH, string SingedXMLFileName, int InvoiceNoRequest
             , bool IsSent, int? StatusCode, string? SendingStatus, string? warningmessage, string? ClearedInvoice, string? errormessage,int BranchId);

    }
}
