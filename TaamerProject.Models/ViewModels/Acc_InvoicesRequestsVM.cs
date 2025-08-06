using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class Acc_InvoicesRequestsVM
    {
        public int InvoiceReqId { get; set; }
        public int? InvoiceId { get; set; }
        public long? InvoiceNoRequest { get; set; }
        public bool? IsSent { get; set; }
        public int? StatusCode { get; set; }
        public string? SendingStatus { get; set; }
        public string? warningmessage { get; set; }
        public string? ClearedInvoice { get; set; }
        public string? errormessage { get; set; }
        public string? InvoiceHash { get; set; }
        public string? SingedXML { get; set; }
        public string? EncodedInvoice { get; set; }
        public string? ZatcaUUID { get; set; }
        public string? QRCode { get; set; }
        public string? PIH { get; set; }
        public string? SingedXMLFileName { get; set; }
        public int? BranchId { get; set; }
        public string? InvoiceNo { get; set; }
        public string? CustomerName { get; set; }
        public string? Date { get; set; }
        public decimal? InvoiceValue { get; set; }
        public decimal? TotalValue { get; set; }
        public int? Type { get; set; }
        public string? InvoiceRetId { get; set; }
        public bool? Rad { get; set; }


    }
}
