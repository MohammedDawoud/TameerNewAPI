using System.ComponentModel;

namespace TaamerProject.API
{
    public struct PaymentMethodEnums
    {
        public const string Incash = "10";
        public const string Credit = "30";
        public const string Payment_to_bank_account = "42";
        public const string Bank_card = "48";
    }
    public struct InvoiceTypeEnums
    {
        public const int Simplified_Invoice = 388;
        public const int Standared_Invoice = 388;
        public const int Simplified_DebitNote = 383;
        public const int Standared_DebitNote = 383;
        public const int Simplified_CreditNote = 381;
        public const int Standard_CreditNote = 381;
    }
    public struct InvoiceTypeNameEnums
    {
        public const string Simplified_Invoice = "0200000";
        public const string Standared_Invoice = "0100000";
        public const string Simplified_DebitNote = "0200000";
        public const string Standared_DebitNote = "0100000";
        public const string Simplified_CreditNote = "0200000";
        public const string Standard_CreditNote = "0100000";

    }
}
