using System;
using System.ComponentModel.DataAnnotations;
namespace TaamerProject.Models
{
    public class TransactionTypes : Auditable
    {
        public int TransactionTypeId { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
    }
}
