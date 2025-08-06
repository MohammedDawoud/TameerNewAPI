using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using TaamerProject.API.Helper.QRCode.Tags;

namespace TaamerProject.API.Helper
{
    public class QRCodeEncoder
    {
        private QRCodeEncoder()
        {

        }
        public static string encode(
        Seller_Inv seller,
        TaxNumber_Inv taxNumber,
        InvoiceDate_Inv invoiceDate,
        TotalAmount_Inv invoiceTotalAmount,
        TaxAmount_Inv invoiceTaxAmount)
        {
            return toBase64(toTLV(seller, taxNumber, invoiceDate, invoiceTotalAmount, invoiceTaxAmount));
        }

        private static string toTLV(
                Seller_Inv seller,
                TaxNumber_Inv taxNumber,
                InvoiceDate_Inv invoiceDate,
                TotalAmount_Inv invoiceTotalAmount,
                TaxAmount_Inv invoiceTaxAmount)
        {
            return seller.strTLV()
            + taxNumber.strTLV()
            + invoiceDate.strTLV()
            + invoiceTotalAmount.strTLV()
            + invoiceTaxAmount.strTLV();
        }

        public static byte[] FromHex(string hex)
        {
            hex = hex.Replace("-", "");
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }
        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        public static byte[] HexStringToBytes(string hexString)
        {
            var bytes = new byte[hexString.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                string currentHex = hexString.Substring(i * 2, 2);
                bytes[i] = Convert.ToByte(currentHex, 16);
            }
            return bytes;
        }

        private static string toBase64(string tlvString)
        {
            //string lengthhex = tlvString.Substring(2,2);
            //int intValue = Convert.ToInt32(lengthhex, 16);
            //var Value = tlvString.Remove(0, 4);

            //string lengthhex2 = Value.Substring(((intValue*2)+2), 2);
            //int intValue2 = Convert.ToInt32(lengthhex2, 16);
            //Value = Value.Remove((intValue * 2), 4);

            //string lengthhex3 = Value.Substring((((intValue * 2)+(intValue2*2)) + 2), 2);
            //int intValue3 = Convert.ToInt32(lengthhex3, 16);
            //Value = Value.Remove((intValue * 2)+(intValue2 * 2), 4);


            //string lengthhex4 = Value.Substring((((intValue * 2) + (intValue2 * 2)+(intValue3 * 2)) + 2), 2);
            //int intValue4 = Convert.ToInt32(lengthhex4, 16);
            //Value = Value.Remove((intValue * 2) + (intValue2 * 2)+ (intValue3 * 2), 4);

            //string lengthhex5 = Value.Substring((((intValue * 2) + (intValue2 * 2) + (intValue3 * 2)+ (intValue4 * 2)) + 2), 2);
            //int intValue5 = Convert.ToInt32(lengthhex5, 16);
            //Value = Value.Remove((intValue * 2) + (intValue2 * 2) + (intValue3 * 2)+ (intValue4 * 2), 4);



            byte[] data = FromHex(tlvString);
            byte[] data2 = StringToByteArray(tlvString);

            //string s = Encoding.UTF8.GetString(data);
            //var plainTextBytes = Encoding.UTF8.GetBytes(s);
            string encodedText2 = Convert.ToBase64String(data2);

            string encodedText = Convert.ToBase64String(data);
            return encodedText2;
            //return Base64.getEncoder().encodeToString(tlvString.getBytes());
        }
    }
}