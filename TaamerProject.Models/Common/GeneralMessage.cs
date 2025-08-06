
using System.Net;
namespace TaamerProject.Models.Common
{
    public class GeneralMessage
    {
        public HttpStatusCode StatusCode { get; set; }
        public string? ReasonPhrase { get; set; }
        public int? ReturnedParm { get; set; }
        public string? ReturnedStr { get; set; }
        public string? ReturnedStrExtra { get; set; }
        public string? ReturnedStrExtra2 { get; set; }
        public string? ReturnedStrNeeded { get; set; }
        public bool? InvoiceIsDeleted { get; set; }
        public List<int>? voucherDetObj { get; set; }
        public List<ObjRet>? ObjRetDet { get; set; }
        public StringContent? Content { get; set; }

        
    }
    public class ObjRet
    {
        public int VoucherDetailsId { get; set; }
        public int? InvoiceId { get; set; }
        public int? Type { get; set; }
        public decimal? Qty { get; set; }
        public string? ServicesPriceName { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? Amount { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? DiscountValue_Det { get; set; }
        public decimal? DiscountPercentage_Det { get; set; }

    }
}
