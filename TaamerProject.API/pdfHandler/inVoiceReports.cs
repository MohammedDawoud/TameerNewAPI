using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data;
using Newtonsoft.Json;
//using System.Web.Script.Serialization;
using System.Configuration;
using System.IO;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using TaamerProject.API.Helper.QRCode.Tags;
using TaamerProject.API.Helper.QRCode;
using iTextSharp.text.pdf.qrcode;
using TaamerProject.Models;
using TaamerProject.API.Helper;
using TaamerProject.Models.Common;
using TaamerP.Service.LocalResources;

namespace TaamerProject.API.pdfHandler
{
    public class inVoiceReports
    {
        public static CultureInfo us = new CultureInfo("en-US");
        public static string PrintDate = DateTime.Now.ToString("yyyy-MM-dd ", us);
        public static string HourMinute = DateTime.Now.ToString("HH:mm");
        //نموذج 1



        public static byte[] GenInvoiceNew(List<VoucherDetailsVM> VoucherDetailsVM, OrganizationsVM organizationsVM, InvoicesVM InvoicesVMObj, DataTable voucher, string costCenterName)
        {
            try
            {

                string JournalNumber = InvoicesVMObj.JournalNumber.ToString();
                string CustomerName = voucher != null && voucher.Rows.Count > 0 && voucher.Columns.Contains("CustomerName") ? voucher.Rows[0]["CustomerName"].ToString() : "";
                string CustomerCode = voucher != null && voucher.Rows.Count > 0 && voucher.Columns.Contains("CustomerCode") ? voucher.Rows[0]["CustomerCode"].ToString() : "";

                bool isFooter = (organizationsVM.IsFooter != null && organizationsVM.IsFooter != "" || organizationsVM.IsFooter != "0") ? true : false;//send from user
                string footerData = "" + organizationsVM.NameAr + "  " + organizationsVM.Address + "  - تليفون  : " + organizationsVM.Mobile + " - فاكس  : " + organizationsVM.Fax + " -  بريد الكترونى  : " + organizationsVM.Email;

                Document doc = new Document(PageSize.A4, 10, 10, 30, 44);
                var memoryStream = PdfBase.OpenDocument(doc, "", false, "", isFooter, footerData);//طباعه افقية

                var headerTitle = "";
                RoundRectangle rr = new RoundRectangle();

                PdfPCell cell = null;
                PdfPTable tblParent = new PdfPTable(1);
                tblParent.DefaultCell.Padding = 0;
                tblParent.WidthPercentage = 100;
                tblParent.DefaultCell.BorderWidth = 0;
                tblParent.HorizontalAlignment = Element.ALIGN_CENTER;
                tblParent.SplitLate = false;
                tblParent.HeaderRows = 1;

                #region HeaderReport
                PdfPTable headerTbl = new PdfPTable(57);
                headerTbl.DefaultCell.Padding = 10;
                headerTbl.WidthPercentage = 95;
                headerTbl.DefaultCell.BorderWidth = 1;
                headerTbl.DefaultCell.BorderColor = BaseColor.BLUE;
                headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

                #region FirstRow
                headerTitle = "";


                DateTime Date = InvoicesVMObj.AddDate ?? DateTime.Now;
                string ActionDate = Date.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.CreateSpecificCulture("en"));

                var SupplierName = (organizationsVM.NameAr ?? "").TrimStart();

                SupplierName = SupplierName.TrimEnd();

                string qrBarcodeHash = QRCodeEncoder.encode(
                    new Seller_Inv(SupplierName),
                    new TaxNumber_Inv(organizationsVM.TaxCode),
                    new InvoiceDate_Inv(ActionDate),
                    new TotalAmount_Inv(InvoicesVMObj.TotalValue.ToString()),
                    new TaxAmount_Inv(InvoicesVMObj.TaxAmount.ToString())
                );


                IDictionary<EncodeHintType, Object> hints = new Dictionary<EncodeHintType, object>();
                hints[EncodeHintType.CHARACTER_SET] = "UTF-8";

                var bc2 = new BarcodeQRCode(qrBarcodeHash, 400, 400, hints);

                iTextSharp.text.Image img1 = bc2.GetImage();
                cell = PdfBase.drawImageCell(img1, 130F, 120F, 24, 2, 2, 2, 2, 0, Element.ALIGN_RIGHT, Element.ALIGN_RIGHT);
                //cell.BorderColorLeft = BaseColor.RED;
                //cell.BorderColorRight = BaseColor.GREEN;
                //cell.BorderColorTop = BaseColor.PINK;
                //cell.BorderColorBottom = BaseColor.YELLOW;
                //cell.BorderWidthLeft = 1f;
                //cell.BorderWidthRight = 1f;
                //cell.BorderWidthTop = 1f;
                //cell.BorderWidthBottom = 1f;
                //cell.Border = PdfPCell.NO_BORDER;
                cell.CellEvent = rr;
                headerTbl.AddCell(cell);



                if (organizationsVM.LogoUrl != "")
                {
                    string imageFilePathLogo = PdfBase.getReportImagePath(organizationsVM.LogoUrl);

                    if (imageFilePathLogo != "")
                    {
                        Image headerJpg = Image.GetInstance(imageFilePathLogo); //infoReport[1];

                        cell = PdfBase.drawImageCell(headerJpg, 100F, 80F, 33, 3, 5, 1, 5, 1, Element.ALIGN_LEFT, Element.ALIGN_LEFT);
                        //cell.BorderColorLeft = BaseColor.RED;
                        //cell.BorderColorRight = BaseColor.GREEN;
                        //cell.BorderColorTop = BaseColor.PINK;
                        //cell.BorderColorBottom = BaseColor.YELLOW;
                        //cell.BorderWidthLeft = 1f;
                        //cell.BorderWidthRight = 1f;
                        //cell.BorderWidthTop = 1f;
                        //cell.BorderWidthBottom = 1f;
                        cell.Border = PdfPCell.NO_BORDER;
                        cell.CellEvent = rr;
                        cell.BorderColor = BaseColor.RED;
                        cell.BorderWidth = 2f;
                        headerTbl.AddCell(cell);
                    }
                    else
                    {
                        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 33, 3, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_LEFT);
                        //cell.BorderColorLeft = BaseColor.RED;
                        //cell.BorderColorRight = BaseColor.GREEN;
                        //cell.BorderColorTop = BaseColor.PINK;
                        //cell.BorderColorBottom = BaseColor.YELLOW;
                        //cell.BorderWidthLeft = 1f;
                        //cell.BorderWidthRight = 1f;
                        //cell.BorderWidthTop = 1f;
                        //cell.BorderWidthBottom = 1f;
                        cell.Border = PdfPCell.NO_BORDER;
                        cell.CellEvent = rr;
                        cell.BorderColor = BaseColor.RED;
                        cell.BorderWidth = 2f;
                        headerTbl.AddCell(cell);
                    }
                }
                else
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 33, 3, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_LEFT);
                    //cell.BorderColorLeft = BaseColor.RED;
                    //cell.BorderColorRight = BaseColor.GREEN;
                    //cell.BorderColorTop = BaseColor.PINK;
                    //cell.BorderColorBottom = BaseColor.YELLOW;
                    //cell.BorderWidthLeft = 1f;
                    //cell.BorderWidthRight = 1f;
                    //cell.BorderWidthTop = 1f;
                    //cell.BorderWidthBottom = 1f;
                    cell.Border = PdfPCell.NO_BORDER;
                    cell.CellEvent = rr;
                    cell.BorderColor = BaseColor.RED;
                    cell.BorderWidth = 2f;
                    headerTbl.AddCell(cell);
                }

                #endregion
                #region secondRow


                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_NORMAL.ToString()], "فاتورة ضريبية", 57, 0, 0, 0, 0, 0, 2, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
                //cell.BorderColorLeft = BaseColor.RED;
                //cell.BorderColorRight = BaseColor.GREEN;
                //cell.BorderColorTop = BaseColor.PINK;
                //cell.BorderColorBottom = BaseColor.YELLOW;
                //cell.BorderWidthLeft = 1f;
                //cell.BorderWidthRight = 1f;
                //cell.BorderWidthTop = 1f;
                //cell.BorderWidthBottom = 1f;
                cell.Border = PdfPCell.NO_BORDER;
                cell.CellEvent = rr;
                cell.BorderColor = BaseColor.RED;
                cell.BorderWidth = 2f;
                headerTbl.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_NORMAL.ToString()], "Tax Invoice", 57, 0, 0, 0, 0, 0, 2, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
                //cell.BorderColorLeft = BaseColor.RED;
                //cell.BorderColorRight = BaseColor.GREEN;
                //cell.BorderColorTop = BaseColor.PINK;
                //cell.BorderColorBottom = BaseColor.YELLOW;
                //cell.BorderWidthLeft = 1f;
                //cell.BorderWidthRight = 1f;
                //cell.BorderWidthTop = 1f;
                //cell.BorderWidthBottom = 1f;
                cell.Border = PdfPCell.NO_BORDER;
                cell.CellEvent = rr;
                cell.BorderColor = BaseColor.RED;
                cell.BorderWidth = 2f;
                headerTbl.AddCell(cell);
                #endregion

                cell = new PdfPCell(headerTbl);
                cell.Colspan = 57;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.PaddingBottom = 5;

                tblParent.AddCell(cell);
                #endregion

                #region content header

                PdfPTable invoiceData = new PdfPTable(57);
                headerTbl.DefaultCell.Padding = 10;
                headerTbl.WidthPercentage = 95;
                headerTbl.DefaultCell.BorderWidth = 0;
                headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 57, 2, 1, 1, 1, 0, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);

                //Startrow1

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "شششششششششششششش", 20, 1, 1, 1, 1, 1, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                cell.BorderColor = BaseColor.BLUE;
                cell.BorderWidth = 1f;
                invoiceData.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "رقم المبني", 7, 1, 1, 1, 1, 1, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                cell.BorderColor = BaseColor.BLUE;
                cell.BorderWidth = 1f;
                cell.BackgroundColor = BaseColor.GRAY;
                invoiceData.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 3, 1, 1, 1, 1, 1 , 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "ببببببببببببب", 20, 1, 1, 1, 1, 1, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                cell.BorderColor = BaseColor.BLUE;
                cell.BorderWidth = 1f;
                invoiceData.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "أسم الشارع", 7, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                cell.BorderColor = BaseColor.BLUE;
                cell.BorderWidth = 1f;
                cell.BackgroundColor = BaseColor.GRAY;
                invoiceData.AddCell(cell);

                //Endrow1



                //Startrow2

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "شششششششششششششش", 20, 1, 1, 1, 1, 1, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);

                //System.Drawing.Color color2 = new System.Drawing.Color(System.Drawing.ColorTranslator.FromHtml("#2AB1C3"));
                //cell.BorderColor = new System.Drawing.Color(System.Drawing.ColorTranslator.FromHtml("#2AB1C3"));
                cell.BorderColor = BaseColor.BLUE;
                cell.BorderWidth = 1f;
                invoiceData.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "رقم المبني", 7, 1, 1, 1, 1, 1, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                cell.BorderColor = BaseColor.BLUE;
                cell.BorderWidth = 1f;
                cell.BackgroundColor = BaseColor.GRAY;
                invoiceData.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 3, 1, 1, 1, 1, 1, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "ببببببببببببب", 20, 1, 1, 1, 1, 1, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                cell.BorderColor = BaseColor.BLUE;
                cell.BorderWidth = 1f;
                invoiceData.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "أسم الشارع", 7, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                cell.BorderColor = BaseColor.BLUE;
                cell.BorderWidth = 1f;
                cell.BackgroundColor = BaseColor.GRAY;
                invoiceData.AddCell(cell);

                //Endrow2







                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], costCenterName, 12, 0, 1, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "م.التكلفة:", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.InvoiceNumber.ToString(), 7, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "ر.الفاتورة:", 5, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.PayTypeName, 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "النوع:", 5, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);
                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.Date.ToString(), 11, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                //invoiceData.AddCell(cell);
                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "التاريخ:", 5, 0, 0, 1, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                //invoiceData.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.Date.ToString(), 11, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "التاريخ:", 5, 0, 0, 1, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);






                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.InvoiceReference, 12, 0, 1, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "المرجع:", 6, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "ريال سعودى", 7, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "العملة:", 5, 0, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.AddDate.Value.ToString("h:mm tt"), 6, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "الوقت:", 5, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.HijriDate.ToString(), 11, 0, 0, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "الموافق", 5, 0, 0, 1, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);








                var xx = InvoicesVMObj.CustomerType;
                //var val = "العنوان";
                string address = "", taxCodeofClient = "";
                if (InvoicesVMObj.CustomerType == "2")
                {
                    //val = "الرقم الضريبي للعميل";
                    address = "";
                    taxCodeofClient = InvoicesVMObj.Address;
                }
                else
                {
                    //val = "العنوان";
                    address = InvoicesVMObj.Address;
                    taxCodeofClient = "";
                }

                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 57, 1, 0, 1, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //invoiceData.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 57, 2, 1, 1, 0, 1, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.CustomerPhone, 11, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "الهاتف:", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], taxCodeofClient, 13, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "برقم ضريبى:", 7, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.CustomerName, 14, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "العميل ", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);

                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 4, 0, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                //invoiceData.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], JournalNumber == "" ? "\n" : JournalNumber, 11, 0, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "القيد:", 6, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.ProjectNo, 13, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "رقم المشروع:", 7, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);
                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "", 13, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //invoiceData.AddCell(cell);



                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], address, 14, 0, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], " العنوان", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);

                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], JournalNumber == "" ? "\n" : JournalNumber, 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                //invoiceData.AddCell(cell);
                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "القيد", 5, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //invoiceData.AddCell(cell);
                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "", 46, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //invoiceData.AddCell(cell);

                cell = new PdfPCell(invoiceData);
                cell.Colspan = 57;
                cell.BorderWidth = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.PaddingBottom = 5;
                tblParent.AddCell(cell);
                #endregion

                #region table
                PdfPTable invoiceTable = new PdfPTable(57);
                invoiceTable.WidthPercentage = 95;
                invoiceTable.DefaultCell.BorderWidth = 1;
                invoiceTable.DefaultCell.BorderColor = BaseColor.BLUE;
                invoiceTable.HorizontalAlignment = Element.ALIGN_CENTER;

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 57, 2, 1, 1, 1, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);

                headerTitle = "الصافي +الضريبة";
                DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 8, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

                headerTitle = "ض. قيمة مضافة";
                DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 8, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                headerTitle = "الصافى";
                DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 6, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

                headerTitle = "السعر";
                DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 6, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

                headerTitle = "الكمية";
                DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);


                headerTitle = "الوحدة";
                DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);


                headerTitle = "البيان";
                DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 13, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

                headerTitle = "ر.الخدمة";
                DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 6, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

                headerTitle = "م";
                DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 2, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

                decimal totalDetailsPrice = 0;
                decimal totalDiscount = 0;
                decimal percentage = 0;

                decimal Diff = 0;
                percentage = decimal.Parse(InvoicesVMObj.DiscountPercentage != null ? InvoicesVMObj.DiscountPercentage.ToString() : "0");

                for (int i = 0; i < VoucherDetailsVM.Count; i++)
                {
                    Diff = 0;
                    totalDetailsPrice = totalDetailsPrice + (decimal.Parse(VoucherDetailsVM[i] != null ? VoucherDetailsVM[i].Amount.ToString() : "0"));
                    Diff = Convert.ToDecimal(VoucherDetailsVM[i].TotalAmount - VoucherDetailsVM[i].TaxAmount);
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], VoucherDetailsVM[i].TotalAmount.ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? "" : ""), 8, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    invoiceTable.AddCell(cell);
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], VoucherDetailsVM[i].TaxAmount.ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? "" : ""), 8, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    invoiceTable.AddCell(cell);
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], Diff.ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? "" : ""), 6, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    invoiceTable.AddCell(cell);
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], VoucherDetailsVM[i].Amount.ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? "" : ""), 6, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    invoiceTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], VoucherDetailsVM[i].Qty.ToString(), 4, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    invoiceTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "خدمة", 4, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    invoiceTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], VoucherDetailsVM[i].ServicesPriceName + ((i == (VoucherDetailsVM.Count) - 1) ? " " : ""), 13, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    invoiceTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (i + 1).ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? " " : ""), 6, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    invoiceTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (i + 1).ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? " " : ""), 2, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    invoiceTable.AddCell(cell);
                }

                totalDiscount = (percentage * totalDetailsPrice) / 100;
                totalDiscount = decimal.Parse(InvoicesVMObj.DiscountPercentage != null && InvoicesVMObj.DiscountPercentage > 0 ? totalDiscount.ToString() : (InvoicesVMObj.DiscountValue != null && InvoicesVMObj.DiscountValue > 0 ? InvoicesVMObj.DiscountValue.ToString() : "0"));
                decimal tax = ((totalDetailsPrice - totalDiscount) * 15) / 100;
                decimal totalValueOfInvoice = (totalDetailsPrice + tax) - (totalDiscount);
                cell = new PdfPCell(invoiceTable);
                cell.Colspan = 57;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                tblParent.AddCell(cell);


                //#endregion
                PdfPTable bodyTable1 = new PdfPTable(57);
                bodyTable1.WidthPercentage = 95;
                bodyTable1.DefaultCell.BorderWidth = 0;
                bodyTable1.HorizontalAlignment = Element.ALIGN_CENTER;
                decimal _paidValud = (InvoicesVMObj.PaidValue.HasValue ? InvoicesVMObj.PaidValue.Value : 0);
                decimal _totalVal = InvoicesVMObj.TotalValue.HasValue ? InvoicesVMObj.TotalValue.Value : 0;
                string remainder = (_totalVal - _paidValud).ToString();

                decimal discountPercentage = InvoicesVMObj.DiscountPercentage.HasValue ? InvoicesVMObj.DiscountPercentage.Value : default(decimal);
                decimal discountValue = InvoicesVMObj.DiscountValue.HasValue ? InvoicesVMObj.DiscountValue.Value : default(decimal);
                if (discountPercentage == default(decimal) && discountValue != default(decimal))
                {
                    discountPercentage = discountValue * 100 / InvoicesVMObj.InvoiceValue.Value;
                }

                if (discountPercentage != default(decimal) && discountValue == default(decimal))
                {
                    discountValue = (InvoicesVMObj.InvoiceValue.Value * discountPercentage) / 100;
                }
                decimal netVal = 0;

                if (VoucherDetailsVM.FirstOrDefault().TaxType == 3)
                {
                    netVal = InvoicesVMObj.InvoiceValue.Value - InvoicesVMObj.TaxAmount.Value - discountValue;

                }
                else
                {
                    netVal = InvoicesVMObj.InvoiceValue.Value - discountValue;

                }



                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 57, 3, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM);
                bodyTable1.AddCell(cell);

                //line 06
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 45, 2, 0, 0, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 12, 2, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);


                //line 0
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], InvoicesVMObj.InvoiceValue.ToString(), 8, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "الاجمالي", 8, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 2, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                // cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], _paidValud.ToString(), 8, 1, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                // cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "المدفوع", 8, 1, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (""), 23, 0, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 7, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                //// cell.Padding = 10;
                //bodyTable1.AddCell(cell);

                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], "", 7, 0, 0, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                ////cell.Padding = 10;
                //bodyTable1.AddCell(cell);

                ////line 01
                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "00.00 ", 8, 0, 1, 1, 1, 1, 5, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                ////cell.Padding = 10;
                //bodyTable1.AddCell(cell);

                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "الاضافات", 8, 0, 1, 1, 1, 1, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                ////cell.Padding = 10;
                //bodyTable1.AddCell(cell);

                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 41, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                ////cell.Padding = 10;
                //bodyTable1.AddCell(cell);

                //line 02
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], InvoicesVMObj.InvoiceValue.ToString(), 8, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "الاجمالي الكلى", 8, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 2, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], remainder, 8, 1, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "المتبقى", 8, 1, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "", 23, 0, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "2021-02-04 ", 7, 0, 0, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                ////cell.Padding = 10;
                //bodyTable1.AddCell(cell);

                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], "الاستحقاق", 7, 0, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                ////cell.Padding = 10;
                //bodyTable1.AddCell(cell);

                //line 03
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], String.Format("{0:0.00}", totalDiscount), 5, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], String.Format("{0:0.00}", discountPercentage) + "%", 3, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "الخصم", 8, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 43, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                //line 04
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], String.Format("{0:0.00}", netVal), 8, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " الصافى", 8, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 7, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);


                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.AddUser, 27, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], "المستخدم", 7, 0, 0, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                bodyTable1.AddCell(cell);

                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "2 ", 7, 0, 0, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                ////cell.Padding = 10;
                //bodyTable1.AddCell(cell);

                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], "رقم النسخة", 7, 0, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                ////cell.Padding = 10;
                //bodyTable1.AddCell(cell);

                //line 05
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], InvoicesVMObj.TaxAmount.ToString(), 8, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "15% ", 4, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], " اجمالى ضريبة القيمة المضافة", 14, 0, 1, 1, 1, 1, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 1, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);



                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.Notes, 23, 0, 1, 1, 1, 1, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], "ملاحظات", 7, 0, 0, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                //line 06
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], _totalVal.ToString(), 8, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "الصافى شامل ض. ق. VAT ", 15, 0, 1, 1, 1, 1, 5, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], ConvertNumToString(_totalVal.ToString()), 34, 0, 1, 1, 1, 1, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                //line 06
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], organizationsVM.AccountBank, 45, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "الحساب البنكى :  ", 12, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);


                headerTitle = "المستلم                       :                             ";
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 28, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 1, 0, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                bodyTable1.AddCell(cell);

                headerTitle = "البائع                                   :                                ";
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 28, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                bodyTable1.AddCell(cell);

                cell = new PdfPCell(bodyTable1);
                cell.Colspan = (57);
                cell.BorderWidth = 1;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //cell.Padding = 10;
                tblParent.AddCell(cell);
                #endregion

                doc.Add(tblParent);
                doc.Close();
                return memoryStream.GetBuffer();
            }
            catch (Exception ex)
            {
                Document doc = new Document(PageSize.A4, 10, 10, 30, 44);
                var memoryStream = PdfBase.OpenDocument(doc, "", false, "", false, "");
                var v = ex.Message;
                return memoryStream.GetBuffer();

            }


        }
        public static byte[] GenInvoiceN(List<VoucherDetailsVM> VoucherDetailsVM, OrganizationsVM organizationsVM, InvoicesVM InvoicesVMObj, DataTable voucher, string costCenterName)
        {
            string JournalNumber = InvoicesVMObj.JournalNumber.ToString();
            string CustomerName = voucher != null && voucher.Rows.Count > 0 && voucher.Columns.Contains("CustomerName") ? voucher.Rows[0]["CustomerName"].ToString() : "";
            string CustomerCode = voucher != null && voucher.Rows.Count > 0 && voucher.Columns.Contains("CustomerCode") ? voucher.Rows[0]["CustomerCode"].ToString() : "";

            bool isFooter = (organizationsVM.IsFooter != null && organizationsVM.IsFooter != "" || organizationsVM.IsFooter != "0") ? true : false;//send from user
            string footerData = "" + organizationsVM.NameAr + "  " + organizationsVM.Address + "  - تليفون  : " + organizationsVM.Mobile + " - فاكس  : " + organizationsVM.Fax + " -  بريد الكترونى  : " + organizationsVM.Email;

            Document doc = new Document(PageSize.A4, 10, 10, 30, 44);
            var memoryStream = PdfBase.OpenDocument(doc, "", false, "", isFooter, footerData);//طباعه افقية

            var headerTitle = "";

            PdfPCell cell = null;
            PdfPTable tblParent = new PdfPTable(1);
            tblParent.DefaultCell.Padding = 0;
            tblParent.WidthPercentage = 100;
            tblParent.DefaultCell.BorderWidth = 0;
            tblParent.HorizontalAlignment = Element.ALIGN_CENTER;
            tblParent.SplitLate = false;
            tblParent.HeaderRows = 1;

            #region HeaderReport
            PdfPTable headerTbl = new PdfPTable(57);
            headerTbl.DefaultCell.Padding = 10;
            headerTbl.WidthPercentage = 95;
            headerTbl.DefaultCell.BorderWidth = 1;
            headerTbl.DefaultCell.BorderColor = BaseColor.BLUE;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            #region FirstRow
            headerTitle = "";

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " ", 57, 2, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "Tax Number", 7, 3, 0, 0, 0, 0, 5, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            headerTbl.AddCell(cell);



            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], organizationsVM.TaxCode, 10, 3, 0, 0, 0, 0, 5, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            headerTbl.AddCell(cell);



            if (organizationsVM.LogoUrl != "")
            {
                string imageFilePathLogo = PdfBase.getReportImagePath(organizationsVM.LogoUrl);

                if (imageFilePathLogo != "")
                {
                    Image headerJpg = Image.GetInstance(imageFilePathLogo); //infoReport[1];

                    cell = PdfBase.drawImageCell(headerJpg, 80F, 65F, 24, 3, 2, 2, 2, 2, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
                    headerTbl.AddCell(cell);
                }
                else
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 24, 3, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
            }
            else
            {
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 24, 3, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                headerTbl.AddCell(cell);
            }

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], organizationsVM.NameAr, 16, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], organizationsVM.PostalCode, 9, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "السجل التجارى: ", 8, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], organizationsVM.TaxCode, 10, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "رقم ضريبى:", 6, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);


            DateTime Date = InvoicesVMObj.AddDate ?? DateTime.Now;
            string ActionDate = Date.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.CreateSpecificCulture("en"));

            var SupplierName = (organizationsVM.NameAr ?? "").TrimStart();

            SupplierName = SupplierName.TrimEnd();

            string qrBarcodeHash = QRCodeEncoder.encode(
                new Seller_Inv(SupplierName),
                new TaxNumber_Inv(organizationsVM.TaxCode),
                new InvoiceDate_Inv(ActionDate),
                new TotalAmount_Inv(InvoicesVMObj.TotalValue.ToString()),
                new TaxAmount_Inv(InvoicesVMObj.TaxAmount.ToString())
            );


            IDictionary<EncodeHintType, Object> hints = new Dictionary<EncodeHintType, object>();
            hints[EncodeHintType.CHARACTER_SET] = "UTF-8";

            var bc2 = new BarcodeQRCode(qrBarcodeHash, 400, 400, hints);

            iTextSharp.text.Image img1 = bc2.GetImage();
            cell = PdfBase.drawImageCell(img1, 130F, 120F, 24, 2, 1, 0, 1, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 16, 1, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], organizationsVM.Mobile, 11, 1, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "الهاتف:", 6, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 10, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], organizationsVM.Address, 17, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "العنوان: ", 6, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);



            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "Tax Number", 9, 1, 0, 0, 0, 0, 5, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            //headerTbl.AddCell(cell);
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], organizationsVM.TaxCode, 17, 1, 0, 0, 0, 0, 5, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            //headerTbl.AddCell(cell);
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "", 5, 1, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //headerTbl.AddCell(cell);
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], organizationsVM.TaxCode, 17, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //headerTbl.AddCell(cell);
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "الــرقم الضريبى", 9, 1, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //headerTbl.AddCell(cell);

            #endregion
            #region secondRow
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_16_UNDERLINE.ToString()], organizationsVM.NameAr, 57, 2, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            //headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_16_UNDERLINE.ToString()], "فاتورة مبيعات", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            headerTbl.AddCell(cell);
            #endregion

            cell = new PdfPCell(headerTbl);
            cell.Colspan = 57;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;

            tblParent.AddCell(cell);
            #endregion

            #region content header

            PdfPTable invoiceData = new PdfPTable(57);
            headerTbl.DefaultCell.Padding = 10;
            headerTbl.WidthPercentage = 95;
            headerTbl.DefaultCell.BorderWidth = 0;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 57, 2, 1, 1, 1, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], costCenterName, 12, 0, 1, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "م.التكلفة:", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.InvoiceNumber.ToString(), 7, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "ر.الفاتورة:", 5, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.PayTypeName, 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "النوع:", 5, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.Date.ToString(), 11, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //invoiceData.AddCell(cell);
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "التاريخ:", 5, 0, 0, 1, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            //invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.Date.ToString(), 11, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "التاريخ:", 5, 0, 0, 1, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);






            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.InvoiceReference, 12, 0, 1, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "المرجع:", 6, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "ريال سعودى", 7, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "العملة:", 5, 0, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.AddDate.Value.ToString("h:mm tt"), 6, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "الوقت:", 5, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.HijriDate.ToString(), 11, 0, 0, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "الموافق", 5, 0, 0, 1, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);








            var xx = InvoicesVMObj.CustomerType;
            //var val = "العنوان";
            string address = "", taxCodeofClient = "";
            if (InvoicesVMObj.CustomerType == "2")
            {
                //val = "الرقم الضريبي للعميل";
                address = "";
                taxCodeofClient = InvoicesVMObj.Address;
            }
            else
            {
                //val = "العنوان";
                address = InvoicesVMObj.Address;
                taxCodeofClient = "";
            }

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 57, 1, 0, 1, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 57, 2, 1, 1, 0, 1, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.CustomerPhone, 11, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "الهاتف:", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], taxCodeofClient, 13, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "برقم ضريبى:", 7, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.CustomerName, 14, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "العميل ", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 4, 0, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], JournalNumber == "" ? "\n" : JournalNumber, 11, 0, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "القيد:", 6, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.ProjectNo, 13, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "رقم المشروع:", 7, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "", 13, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //invoiceData.AddCell(cell);



            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], address, 14, 0, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], " العنوان", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], JournalNumber == "" ? "\n" : JournalNumber, 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //invoiceData.AddCell(cell);
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "القيد", 5, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //invoiceData.AddCell(cell);
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "", 46, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //invoiceData.AddCell(cell);

            cell = new PdfPCell(invoiceData);
            cell.Colspan = 57;
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            tblParent.AddCell(cell);
            #endregion

            #region table
            PdfPTable invoiceTable = new PdfPTable(57);
            invoiceTable.WidthPercentage = 95;
            invoiceTable.DefaultCell.BorderWidth = 1;
            invoiceTable.DefaultCell.BorderColor = BaseColor.BLUE;
            invoiceTable.HorizontalAlignment = Element.ALIGN_CENTER;

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 57, 2, 1, 1, 1, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);

            headerTitle = "الصافي +الضريبة";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 8, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "ض. قيمة مضافة";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 8, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            headerTitle = "الصافى";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 6, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "السعر";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 6, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "الكمية";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);


            headerTitle = "الوحدة";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);


            headerTitle = "البيان";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 13, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "ر.الخدمة";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 6, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "م";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 2, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            decimal totalDetailsPrice = 0;
            decimal totalDiscount = 0;
            decimal percentage = 0;

            decimal Diff = 0;
            percentage = decimal.Parse(InvoicesVMObj.DiscountPercentage != null ? InvoicesVMObj.DiscountPercentage.ToString() : "0");

            for (int i = 0; i < VoucherDetailsVM.Count; i++)
            {
                Diff = 0;
                totalDetailsPrice = totalDetailsPrice + (decimal.Parse(VoucherDetailsVM[i] != null ? VoucherDetailsVM[i].Amount.ToString() : "0"));
                Diff = Convert.ToDecimal(VoucherDetailsVM[i].TotalAmount - VoucherDetailsVM[i].TaxAmount);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], VoucherDetailsVM[i].TotalAmount.ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? "" : ""), 8, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], VoucherDetailsVM[i].TaxAmount.ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? "" : ""), 8, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], Diff.ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? "" : ""), 6, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], VoucherDetailsVM[i].Amount.ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? "" : ""), 6, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], VoucherDetailsVM[i].Qty.ToString(), 4, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "خدمة", 4, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], VoucherDetailsVM[i].ServicesPriceName + ((i == (VoucherDetailsVM.Count) - 1) ? " " : ""), 13, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (i + 1).ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? " " : ""), 6, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (i + 1).ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? " " : ""), 2, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);
            }

            totalDiscount = (percentage * totalDetailsPrice) / 100;
            totalDiscount = decimal.Parse(InvoicesVMObj.DiscountPercentage != null && InvoicesVMObj.DiscountPercentage > 0 ? totalDiscount.ToString() : (InvoicesVMObj.DiscountValue != null && InvoicesVMObj.DiscountValue > 0 ? InvoicesVMObj.DiscountValue.ToString() : "0"));
            decimal tax = ((totalDetailsPrice - totalDiscount) * 15) / 100;
            decimal totalValueOfInvoice = (totalDetailsPrice + tax) - (totalDiscount);
            cell = new PdfPCell(invoiceTable);
            cell.Colspan = 57;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            tblParent.AddCell(cell);


            //#endregion
            PdfPTable bodyTable1 = new PdfPTable(57);
            bodyTable1.WidthPercentage = 95;
            bodyTable1.DefaultCell.BorderWidth = 0;
            bodyTable1.HorizontalAlignment = Element.ALIGN_CENTER;
            decimal _paidValud = (InvoicesVMObj.PaidValue.HasValue ? InvoicesVMObj.PaidValue.Value : 0);
            decimal _totalVal = InvoicesVMObj.TotalValue.HasValue ? InvoicesVMObj.TotalValue.Value : 0;
            string remainder = (_totalVal - _paidValud).ToString();

            decimal discountPercentage = InvoicesVMObj.DiscountPercentage.HasValue ? InvoicesVMObj.DiscountPercentage.Value : default(decimal);
            decimal discountValue = InvoicesVMObj.DiscountValue.HasValue ? InvoicesVMObj.DiscountValue.Value : default(decimal);
            if (discountPercentage == default(decimal) && discountValue != default(decimal))
            {
                discountPercentage = discountValue * 100 / InvoicesVMObj.InvoiceValue.Value;
            }

            if (discountPercentage != default(decimal) && discountValue == default(decimal))
            {
                discountValue = (InvoicesVMObj.InvoiceValue.Value * discountPercentage) / 100;
            }
            decimal netVal = 0;

            if (VoucherDetailsVM.FirstOrDefault().TaxType == 3)
            {
                netVal = InvoicesVMObj.InvoiceValue.Value - InvoicesVMObj.TaxAmount.Value - discountValue;

            }
            else
            {
                netVal = InvoicesVMObj.InvoiceValue.Value - discountValue;

            }



            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 57, 3, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM);
            bodyTable1.AddCell(cell);

            //line 06
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 45, 2, 0, 0, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 12, 2, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);


            //line 0
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], InvoicesVMObj.InvoiceValue.ToString(), 8, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "الاجمالي", 8, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 2, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            // cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], _paidValud.ToString(), 8, 1, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            // cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "المدفوع", 8, 1, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (""), 23, 0, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 7, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //// cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], "", 7, 0, 0, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            ////line 01
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "00.00 ", 8, 0, 1, 1, 1, 1, 5, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "الاضافات", 8, 0, 1, 1, 1, 1, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 41, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            //line 02
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], InvoicesVMObj.InvoiceValue.ToString(), 8, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "الاجمالي الكلى", 8, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 2, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], remainder, 8, 1, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "المتبقى", 8, 1, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "", 23, 0, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "2021-02-04 ", 7, 0, 0, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], "الاستحقاق", 7, 0, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            //line 03
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], String.Format("{0:0.00}", totalDiscount), 5, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], String.Format("{0:0.00}", discountPercentage) + "%", 3, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "الخصم", 8, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 43, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            //line 04
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], String.Format("{0:0.00}", netVal), 8, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " الصافى", 8, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 7, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.AddUser, 27, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], "المستخدم", 7, 0, 0, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "2 ", 7, 0, 0, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], "رقم النسخة", 7, 0, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            //line 05
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], InvoicesVMObj.TaxAmount.ToString(), 8, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "15% ", 4, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], " اجمالى ضريبة القيمة المضافة", 14, 0, 1, 1, 1, 1, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 1, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);



            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.Notes, 23, 0, 1, 1, 1, 1, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], "ملاحظات", 7, 0, 0, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            //line 06
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], _totalVal.ToString(), 8, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "الصافى شامل ض. ق. VAT ", 15, 0, 1, 1, 1, 1, 5, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], ConvertNumToString(_totalVal.ToString()), 34, 0, 1, 1, 1, 1, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            //line 06
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], organizationsVM.AccountBank, 45, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "الحساب البنكى :  ", 12, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);


            headerTitle = "المستلم                       :                             ";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 28, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 1, 0, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            bodyTable1.AddCell(cell);

            headerTitle = "البائع                                   :                                ";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 28, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            bodyTable1.AddCell(cell);

            cell = new PdfPCell(bodyTable1);
            cell.Colspan = (57);
            cell.BorderWidth = 1;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //cell.Padding = 10;
            tblParent.AddCell(cell);
            #endregion

            doc.Add(tblParent);
            doc.Close();
            return memoryStream.GetBuffer();

        }

        public static byte[] GenInvoiceNoti(List<VoucherDetailsVM> VoucherDetailsVM, OrganizationsVM organizationsVM, InvoicesVM InvoicesVMObj, DataTable voucher, string costCenterName)
        {
            string JournalNumber = InvoicesVMObj.JournalNumber.ToString();
            string CustomerName = voucher != null && voucher.Rows.Count > 0 && voucher.Columns.Contains("CustomerName") ? voucher.Rows[0]["CustomerName"].ToString() : "";
            string CustomerCode = voucher != null && voucher.Rows.Count > 0 && voucher.Columns.Contains("CustomerCode") ? voucher.Rows[0]["CustomerCode"].ToString() : "";

            bool isFooter = (organizationsVM.IsFooter != null && organizationsVM.IsFooter != "" || organizationsVM.IsFooter != "0") ? true : false;//send from user
            string footerData = "" + organizationsVM.NameAr + "  " + organizationsVM.Address + "  - تليفون  : " + organizationsVM.Mobile + " - فاكس  : " + organizationsVM.Fax + " -  بريد الكترونى  : " + organizationsVM.Email;

            Document doc = new Document(PageSize.A4, 10, 10, 30, 44);
            var memoryStream = PdfBase.OpenDocument(doc, "", false, "", isFooter, footerData);//طباعه افقية

            var headerTitle = "";

            PdfPCell cell = null;
            PdfPTable tblParent = new PdfPTable(1);
            tblParent.DefaultCell.Padding = 0;
            tblParent.WidthPercentage = 100;
            tblParent.DefaultCell.BorderWidth = 0;
            tblParent.HorizontalAlignment = Element.ALIGN_CENTER;
            tblParent.SplitLate = false;
            tblParent.HeaderRows = 1;

            #region HeaderReport
            PdfPTable headerTbl = new PdfPTable(57);
            headerTbl.DefaultCell.Padding = 10;
            headerTbl.WidthPercentage = 95;
            headerTbl.DefaultCell.BorderWidth = 1;
            headerTbl.DefaultCell.BorderColor = BaseColor.BLUE;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            #region FirstRow
            headerTitle = "";

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " ", 57, 2, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "Tax Number", 7, 3, 0, 0, 0, 0, 5, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            headerTbl.AddCell(cell);



            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], organizationsVM.TaxCode, 10, 3, 0, 0, 0, 0, 5, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            headerTbl.AddCell(cell);



            if (organizationsVM.LogoUrl != "")
            {
                string imageFilePathLogo = PdfBase.getReportImagePath(organizationsVM.LogoUrl);

                if (imageFilePathLogo != "")
                {
                    Image headerJpg = Image.GetInstance(imageFilePathLogo); //infoReport[1];

                    cell = PdfBase.drawImageCell(headerJpg, 80F, 65F, 24, 3, 2, 2, 2, 2, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
                    headerTbl.AddCell(cell);
                }
                else
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 24, 3, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
            }
            else
            {
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 24, 3, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                headerTbl.AddCell(cell);
            }

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], organizationsVM.NameAr, 16, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], organizationsVM.PostalCode, 9, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "السجل التجارى: ", 8, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], organizationsVM.TaxCode, 10, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "رقم ضريبى:", 6, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);


            DateTime Date = InvoicesVMObj.AddDate ?? DateTime.Now;
            string ActionDate = Date.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.CreateSpecificCulture("en"));

            var SupplierName = (organizationsVM.NameAr ?? "").TrimStart();

            SupplierName = SupplierName.TrimEnd();

            string qrBarcodeHash = QRCodeEncoder.encode(
                new Seller_Inv(SupplierName),
                new TaxNumber_Inv(organizationsVM.TaxCode),
                new InvoiceDate_Inv(ActionDate),
                new TotalAmount_Inv(InvoicesVMObj.TotalValue.ToString()),
                new TaxAmount_Inv(InvoicesVMObj.TaxAmount.ToString())
            );

            IDictionary<EncodeHintType, Object> hints = new Dictionary<EncodeHintType, object>();
            hints[EncodeHintType.CHARACTER_SET] = "UTF-8";

            var bc2 = new BarcodeQRCode(qrBarcodeHash, 400, 400, hints);

            iTextSharp.text.Image img1 = bc2.GetImage();
            cell = PdfBase.drawImageCell(img1, 130F, 120F, 24, 2, 1, 0, 1, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 16, 1, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], organizationsVM.Mobile, 11, 1, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "الهاتف:", 6, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 10, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], organizationsVM.Address, 17, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "العنوان: ", 6, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            #endregion
            #region secondRow

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_16_UNDERLINE.ToString()], "إشعار دائن لفاتورة", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            headerTbl.AddCell(cell);
            #endregion

            cell = new PdfPCell(headerTbl);
            cell.Colspan = 57;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;

            tblParent.AddCell(cell);
            #endregion

            #region content header

            PdfPTable invoiceData = new PdfPTable(57);
            headerTbl.DefaultCell.Padding = 10;
            headerTbl.WidthPercentage = 95;
            headerTbl.DefaultCell.BorderWidth = 0;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 57, 2, 1, 1, 1, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], costCenterName, 12, 0, 1, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "م.التكلفة:", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.InvoiceNumber.ToString(), 7, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "ر.الفاتورة:", 5, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.PayTypeName, 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "النوع:", 5, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.Date.ToString(), 11, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "التاريخ:", 5, 0, 0, 1, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.InvoiceReference, 12, 0, 1, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "المرجع:", 6, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "ريال سعودى", 7, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "العملة:", 5, 0, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.AddDate.Value.ToString("h:mm tt"), 6, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "الوقت:", 5, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.HijriDate.ToString(), 11, 0, 0, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "الموافق", 5, 0, 0, 1, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);


            var xx = InvoicesVMObj.CustomerType;
            //var val = "العنوان";
            string address = "", taxCodeofClient = "";
            if (InvoicesVMObj.CustomerType == "2")
            {
                //val = "الرقم الضريبي للعميل";
                address = "";
                taxCodeofClient = InvoicesVMObj.Address;
            }
            else
            {
                //val = "العنوان";
                address = InvoicesVMObj.Address;
                taxCodeofClient = "";
            }

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 57, 2, 1, 1, 0, 1, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.CustomerPhone, 11, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "الهاتف:", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], taxCodeofClient, 13, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "برقم ضريبى:", 7, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.CustomerName, 14, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "العميل ", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], JournalNumber == "" ? "\n" : JournalNumber, 11, 0, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "القيد:", 6, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.ProjectNo, 13, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "رقم المشروع:", 7, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], address, 14, 0, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], " العنوان", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);

            cell = new PdfPCell(invoiceData);
            cell.Colspan = 57;
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            tblParent.AddCell(cell);
            #endregion

            #region table
            PdfPTable invoiceTable = new PdfPTable(57);
            invoiceTable.WidthPercentage = 95;
            invoiceTable.DefaultCell.BorderWidth = 1;
            invoiceTable.DefaultCell.BorderColor = BaseColor.BLUE;
            invoiceTable.HorizontalAlignment = Element.ALIGN_CENTER;

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 57, 2, 1, 1, 1, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);

            headerTitle = "الصافي +الضريبة";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 8, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "ض. قيمة مضافة";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 8, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            headerTitle = "الصافى";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 6, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "السعر";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 6, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "الكمية";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);


            headerTitle = "الوحدة";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);


            headerTitle = "البيان";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 13, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "ر.الخدمة";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 6, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "م";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 2, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            decimal totalDetailsPrice = 0;
            decimal totalDetailTax= 0;
            decimal totalDetail = 0;
            int QtyDetail = 0;

            decimal totalDiscount = 0;
            decimal percentage = 0;

            decimal Diff = 0;
            percentage = decimal.Parse(InvoicesVMObj.DiscountPercentage != null ? InvoicesVMObj.DiscountPercentage.ToString() : "0");

            for (int i = 0; i < VoucherDetailsVM.Count; i++)
            {
                QtyDetail = (int.Parse(VoucherDetailsVM[i] != null ? VoucherDetailsVM[i].Qty.ToString() : "1"));


                Diff = 0;
                totalDetailsPrice = totalDetailsPrice + (decimal.Parse(VoucherDetailsVM[i] != null ? VoucherDetailsVM[i].Amount.ToString() : "0")* QtyDetail);
                totalDetailTax = totalDetailTax + (decimal.Parse(VoucherDetailsVM[i] != null ? VoucherDetailsVM[i].TaxAmount.ToString() : "0")* QtyDetail);
                totalDetail = totalDetail + (decimal.Parse(VoucherDetailsVM[i] != null ? VoucherDetailsVM[i].TotalAmount.ToString() : "0"));

                Diff = Convert.ToDecimal(VoucherDetailsVM[i].TotalAmount - VoucherDetailsVM[i].TaxAmount);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], VoucherDetailsVM[i].TotalAmount.ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? "" : ""), 8, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], VoucherDetailsVM[i].TaxAmount.ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? "" : ""), 8, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], Diff.ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? "" : ""), 6, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], VoucherDetailsVM[i].Amount.ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? "" : ""), 6, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], VoucherDetailsVM[i].Qty.ToString(), 4, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "خدمة", 4, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], VoucherDetailsVM[i].ServicesPriceName + ((i == (VoucherDetailsVM.Count) - 1) ? " " : ""), 13, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (i + 1).ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? " " : ""), 6, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (i + 1).ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? " " : ""), 2, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);
            }

            totalDiscount = (percentage * totalDetailsPrice) / 100;
            totalDiscount = decimal.Parse(InvoicesVMObj.DiscountPercentage != null && InvoicesVMObj.DiscountPercentage > 0 ? totalDiscount.ToString() : (InvoicesVMObj.DiscountValue != null && InvoicesVMObj.DiscountValue > 0 ? InvoicesVMObj.DiscountValue.ToString() : "0"));
            decimal tax = ((totalDetailsPrice - totalDiscount) * 15) / 100;
            decimal totalValueOfInvoice = (totalDetailsPrice + tax) - (totalDiscount);
            cell = new PdfPCell(invoiceTable);
            cell.Colspan = 57;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            tblParent.AddCell(cell);


            //#endregion
            PdfPTable bodyTable1 = new PdfPTable(57);
            bodyTable1.WidthPercentage = 95;
            bodyTable1.DefaultCell.BorderWidth = 0;
            bodyTable1.HorizontalAlignment = Element.ALIGN_CENTER;
            decimal _paidValud = (InvoicesVMObj.PaidValue.HasValue ? InvoicesVMObj.PaidValue.Value : 0);
            decimal _totalVal = InvoicesVMObj.TotalValue.HasValue ? InvoicesVMObj.TotalValue.Value : 0;
            string remainder = (_totalVal - _paidValud).ToString();

            decimal discountPercentage = InvoicesVMObj.DiscountPercentage.HasValue ? InvoicesVMObj.DiscountPercentage.Value : default(decimal);
            decimal discountValue = InvoicesVMObj.DiscountValue.HasValue ? InvoicesVMObj.DiscountValue.Value : default(decimal);
            if (discountPercentage == default(decimal) && discountValue != default(decimal))
            {
                discountPercentage = discountValue * 100 / InvoicesVMObj.InvoiceValue.Value;
            }

            if (discountPercentage != default(decimal) && discountValue == default(decimal))
            {
                discountValue = (InvoicesVMObj.InvoiceValue.Value * discountPercentage) / 100;
            }
            decimal netVal = 0;

            if (VoucherDetailsVM.FirstOrDefault().TaxType == 3)
            {
                netVal = InvoicesVMObj.InvoiceValue.Value - InvoicesVMObj.TaxAmount.Value - discountValue;

            }
            else
            {
                netVal = InvoicesVMObj.InvoiceValue.Value - discountValue;

            }



            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 57, 3, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM);
            bodyTable1.AddCell(cell);

            //line 06
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 45, 2, 0, 0, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 12, 2, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);


            ////line 0
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], totalDetailTax.ToString(), 8, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "الاجمالي", 8, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 2, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //// cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], _paidValud.ToString(), 8, 1, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //// cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "المدفوع", 8, 1, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (""), 23, 0, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);


            //line 02
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], totalDetailsPrice.ToString(), 8, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "الاجمالي الكلى", 8, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 2, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], remainder, 8, 1, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "المتبقى", 8, 1, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "", 39, 0, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);


            ////line 03
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], String.Format("{0:0.00}", totalDiscount), 5, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], String.Format("{0:0.00}", discountPercentage) + "%", 3, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "الخصم", 8, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 43, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            ////line 04
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], String.Format("{0:0.00}", netVal), 8, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " الصافى", 8, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 7, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);


            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.AddUser, 27, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], "المستخدم", 7, 0, 0, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //bodyTable1.AddCell(cell);


            //line 05
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], totalDetailTax.ToString(), 8, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "15% ", 4, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], " اجمالى ضريبة القيمة المضافة", 14, 0, 1, 1, 1, 1, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 1, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);



            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.Notes, 23, 0, 1, 1, 1, 1, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], "ملاحظات", 7, 0, 0, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            //line 06
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], totalDetail.ToString(), 8, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "الصافى شامل ض. ق. VAT ", 15, 0, 1, 1, 1, 1, 5, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], ConvertNumToString(_totalVal.ToString()), 34, 0, 1, 1, 1, 1, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            //line 06
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], organizationsVM.AccountBank, 45, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "الحساب البنكى :  ", 12, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);


            headerTitle = "المستلم                       :                             ";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 28, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 1, 0, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            bodyTable1.AddCell(cell);

            headerTitle = "البائع                                   :                                ";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 28, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            bodyTable1.AddCell(cell);

            cell = new PdfPCell(bodyTable1);
            cell.Colspan = (57);
            cell.BorderWidth = 1;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //cell.Padding = 10;
            tblParent.AddCell(cell);
            #endregion

            doc.Add(tblParent);
            doc.Close();
            return memoryStream.GetBuffer();

        }

        public static byte[] GenInvoiceNotiDepit(List<VoucherDetailsVM> VoucherDetailsVM, OrganizationsVM organizationsVM, InvoicesVM InvoicesVMObj, DataTable voucher, string costCenterName)
        {
            string JournalNumber = InvoicesVMObj.JournalNumber.ToString();
            string CustomerName = voucher != null && voucher.Rows.Count > 0 && voucher.Columns.Contains("CustomerName") ? voucher.Rows[0]["CustomerName"].ToString() : "";
            string CustomerCode = voucher != null && voucher.Rows.Count > 0 && voucher.Columns.Contains("CustomerCode") ? voucher.Rows[0]["CustomerCode"].ToString() : "";

            bool isFooter = (organizationsVM.IsFooter != null && organizationsVM.IsFooter != "" || organizationsVM.IsFooter != "0") ? true : false;//send from user
            string footerData = "" + organizationsVM.NameAr + "  " + organizationsVM.Address + "  - تليفون  : " + organizationsVM.Mobile + " - فاكس  : " + organizationsVM.Fax + " -  بريد الكترونى  : " + organizationsVM.Email;

            Document doc = new Document(PageSize.A4, 10, 10, 30, 44);
            var memoryStream = PdfBase.OpenDocument(doc, "", false, "", isFooter, footerData);//طباعه افقية

            var headerTitle = "";

            PdfPCell cell = null;
            PdfPTable tblParent = new PdfPTable(1);
            tblParent.DefaultCell.Padding = 0;
            tblParent.WidthPercentage = 100;
            tblParent.DefaultCell.BorderWidth = 0;
            tblParent.HorizontalAlignment = Element.ALIGN_CENTER;
            tblParent.SplitLate = false;
            tblParent.HeaderRows = 1;

            #region HeaderReport
            PdfPTable headerTbl = new PdfPTable(57);
            headerTbl.DefaultCell.Padding = 10;
            headerTbl.WidthPercentage = 95;
            headerTbl.DefaultCell.BorderWidth = 1;
            headerTbl.DefaultCell.BorderColor = BaseColor.BLUE;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            #region FirstRow
            headerTitle = "";

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " ", 57, 2, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "Tax Number", 7, 3, 0, 0, 0, 0, 5, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            headerTbl.AddCell(cell);



            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], organizationsVM.TaxCode, 10, 3, 0, 0, 0, 0, 5, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            headerTbl.AddCell(cell);



            if (organizationsVM.LogoUrl != "")
            {
                string imageFilePathLogo = PdfBase.getReportImagePath(organizationsVM.LogoUrl);

                if (imageFilePathLogo != "")
                {
                    Image headerJpg = Image.GetInstance(imageFilePathLogo); //infoReport[1];

                    cell = PdfBase.drawImageCell(headerJpg, 80F, 65F, 24, 3, 2, 2, 2, 2, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
                    headerTbl.AddCell(cell);
                }
                else
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 24, 3, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
            }
            else
            {
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 24, 3, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                headerTbl.AddCell(cell);
            }

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], organizationsVM.NameAr, 16, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], organizationsVM.PostalCode, 9, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "السجل التجارى: ", 8, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], organizationsVM.TaxCode, 10, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "رقم ضريبى:", 6, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);


            DateTime Date = InvoicesVMObj.AddDate ?? DateTime.Now;
            string ActionDate = Date.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.CreateSpecificCulture("en"));

            var SupplierName = (organizationsVM.NameAr ?? "").TrimStart();

            SupplierName = SupplierName.TrimEnd();

            string qrBarcodeHash = QRCodeEncoder.encode(
                new Seller_Inv(SupplierName),
                new TaxNumber_Inv(organizationsVM.TaxCode),
                new InvoiceDate_Inv(ActionDate),
                new TotalAmount_Inv(InvoicesVMObj.TotalValue.ToString()),
                new TaxAmount_Inv(InvoicesVMObj.TaxAmount.ToString())
            );

            IDictionary<EncodeHintType, Object> hints = new Dictionary<EncodeHintType, object>();
            hints[EncodeHintType.CHARACTER_SET] = "UTF-8";

            var bc2 = new BarcodeQRCode(qrBarcodeHash, 400, 400, hints);

            iTextSharp.text.Image img1 = bc2.GetImage();
            cell = PdfBase.drawImageCell(img1, 130F, 120F, 24, 2, 1, 0, 1, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 16, 1, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], organizationsVM.Mobile, 11, 1, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "الهاتف:", 6, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 10, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], organizationsVM.Address, 17, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "العنوان: ", 6, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            #endregion
            #region secondRow

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_16_UNDERLINE.ToString()], "إشعار مدين لفاتورة", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            headerTbl.AddCell(cell);
            #endregion

            cell = new PdfPCell(headerTbl);
            cell.Colspan = 57;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;

            tblParent.AddCell(cell);
            #endregion

            #region content header

            PdfPTable invoiceData = new PdfPTable(57);
            headerTbl.DefaultCell.Padding = 10;
            headerTbl.WidthPercentage = 95;
            headerTbl.DefaultCell.BorderWidth = 0;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 57, 2, 1, 1, 1, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], costCenterName, 12, 0, 1, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "م.التكلفة:", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.InvoiceNumber.ToString(), 7, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "ر.الفاتورة:", 5, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.PayTypeName, 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "النوع:", 5, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.Date.ToString(), 11, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "التاريخ:", 5, 0, 0, 1, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.InvoiceReference, 12, 0, 1, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "المرجع:", 6, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "ريال سعودى", 7, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "العملة:", 5, 0, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.AddDate.Value.ToString("h:mm tt"), 6, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "الوقت:", 5, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.HijriDate.ToString(), 11, 0, 0, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "الموافق", 5, 0, 0, 1, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);


            var xx = InvoicesVMObj.CustomerType;
            //var val = "العنوان";
            string address = "", taxCodeofClient = "";
            if (InvoicesVMObj.CustomerType == "2")
            {
                //val = "الرقم الضريبي للعميل";
                address = "";
                taxCodeofClient = InvoicesVMObj.Address;
            }
            else
            {
                //val = "العنوان";
                address = InvoicesVMObj.Address;
                taxCodeofClient = "";
            }

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 57, 2, 1, 1, 0, 1, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.CustomerPhone, 11, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "الهاتف:", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], taxCodeofClient, 13, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "برقم ضريبى:", 7, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.CustomerName, 14, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "العميل ", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], JournalNumber == "" ? "\n" : JournalNumber, 11, 0, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "القيد:", 6, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.ProjectNo, 13, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "رقم المشروع:", 7, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], address, 14, 0, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], " العنوان", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);

            cell = new PdfPCell(invoiceData);
            cell.Colspan = 57;
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            tblParent.AddCell(cell);
            #endregion

            #region table
            PdfPTable invoiceTable = new PdfPTable(57);
            invoiceTable.WidthPercentage = 95;
            invoiceTable.DefaultCell.BorderWidth = 1;
            invoiceTable.DefaultCell.BorderColor = BaseColor.BLUE;
            invoiceTable.HorizontalAlignment = Element.ALIGN_CENTER;

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 57, 2, 1, 1, 1, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);

            headerTitle = "الصافي +الضريبة";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 8, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "ض. قيمة مضافة";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 8, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            headerTitle = "الصافى";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 6, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "السعر";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 6, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "الكمية";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);


            headerTitle = "الوحدة";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);


            headerTitle = "البيان";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 13, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "ر.الخدمة";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 6, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "م";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 2, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            decimal totalDetailsPrice = 0;
            decimal totalDetailTax = 0;
            decimal totalDetail = 0;
            int QtyDetail = 0;

            decimal totalDiscount = 0;
            decimal percentage = 0;

            decimal Diff = 0;
            percentage = decimal.Parse(InvoicesVMObj.DiscountPercentage != null ? InvoicesVMObj.DiscountPercentage.ToString() : "0");

            for (int i = 0; i < VoucherDetailsVM.Count; i++)
            {
                QtyDetail = (int.Parse(VoucherDetailsVM[i] != null ? VoucherDetailsVM[i].Qty.ToString() : "1"));


                Diff = 0;
                totalDetailsPrice = totalDetailsPrice + (decimal.Parse(VoucherDetailsVM[i] != null ? VoucherDetailsVM[i].Amount.ToString() : "0")* QtyDetail);
                totalDetailTax = totalDetailTax + (decimal.Parse(VoucherDetailsVM[i] != null ? VoucherDetailsVM[i].TaxAmount.ToString() : "0")* QtyDetail);
                totalDetail = totalDetail + (decimal.Parse(VoucherDetailsVM[i] != null ? VoucherDetailsVM[i].TotalAmount.ToString() : "0"));

                Diff = Convert.ToDecimal(VoucherDetailsVM[i].TotalAmount - VoucherDetailsVM[i].TaxAmount);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], VoucherDetailsVM[i].TotalAmount.ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? "" : ""), 8, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], VoucherDetailsVM[i].TaxAmount.ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? "" : ""), 8, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], Diff.ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? "" : ""), 6, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], VoucherDetailsVM[i].Amount.ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? "" : ""), 6, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], VoucherDetailsVM[i].Qty.ToString(), 4, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "خدمة", 4, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], VoucherDetailsVM[i].ServicesPriceName + ((i == (VoucherDetailsVM.Count) - 1) ? " " : ""), 13, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (i + 1).ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? " " : ""), 6, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (i + 1).ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? " " : ""), 2, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);
            }

            totalDiscount = (percentage * totalDetailsPrice) / 100;
            totalDiscount = decimal.Parse(InvoicesVMObj.DiscountPercentage != null && InvoicesVMObj.DiscountPercentage > 0 ? totalDiscount.ToString() : (InvoicesVMObj.DiscountValue != null && InvoicesVMObj.DiscountValue > 0 ? InvoicesVMObj.DiscountValue.ToString() : "0"));
            decimal tax = ((totalDetailsPrice - totalDiscount) * 15) / 100;
            decimal totalValueOfInvoice = (totalDetailsPrice + tax) - (totalDiscount);
            cell = new PdfPCell(invoiceTable);
            cell.Colspan = 57;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            tblParent.AddCell(cell);


            //#endregion
            PdfPTable bodyTable1 = new PdfPTable(57);
            bodyTable1.WidthPercentage = 95;
            bodyTable1.DefaultCell.BorderWidth = 0;
            bodyTable1.HorizontalAlignment = Element.ALIGN_CENTER;
            decimal _paidValud = (InvoicesVMObj.PaidValue.HasValue ? InvoicesVMObj.PaidValue.Value : 0);
            decimal _totalVal = InvoicesVMObj.TotalValue.HasValue ? InvoicesVMObj.TotalValue.Value : 0;
            string remainder = (_totalVal - _paidValud).ToString();

            decimal discountPercentage = InvoicesVMObj.DiscountPercentage.HasValue ? InvoicesVMObj.DiscountPercentage.Value : default(decimal);
            decimal discountValue = InvoicesVMObj.DiscountValue.HasValue ? InvoicesVMObj.DiscountValue.Value : default(decimal);
            if (discountPercentage == default(decimal) && discountValue != default(decimal))
            {
                discountPercentage = discountValue * 100 / InvoicesVMObj.InvoiceValue.Value;
            }

            if (discountPercentage != default(decimal) && discountValue == default(decimal))
            {
                discountValue = (InvoicesVMObj.InvoiceValue.Value * discountPercentage) / 100;
            }
            decimal netVal = 0;

            if (VoucherDetailsVM.FirstOrDefault().TaxType == 3)
            {
                netVal = InvoicesVMObj.InvoiceValue.Value - InvoicesVMObj.TaxAmount.Value - discountValue;

            }
            else
            {
                netVal = InvoicesVMObj.InvoiceValue.Value - discountValue;

            }



            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 57, 3, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM);
            bodyTable1.AddCell(cell);

            //line 06
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 45, 2, 0, 0, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 12, 2, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);


            ////line 0
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], totalDetailTax.ToString(), 8, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "الاجمالي", 8, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 2, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //// cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], _paidValud.ToString(), 8, 1, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //// cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "المدفوع", 8, 1, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (""), 23, 0, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);


            //line 02
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], totalDetailsPrice.ToString(), 8, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "الاجمالي الكلى", 8, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 2, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], remainder, 8, 1, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "المتبقى", 8, 1, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "", 39, 0, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);


            ////line 03
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], String.Format("{0:0.00}", totalDiscount), 5, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], String.Format("{0:0.00}", discountPercentage) + "%", 3, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "الخصم", 8, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 43, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            ////line 04
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], String.Format("{0:0.00}", netVal), 8, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " الصافى", 8, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 7, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);


            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.AddUser, 27, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], "المستخدم", 7, 0, 0, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //bodyTable1.AddCell(cell);


            //line 05
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], totalDetailTax.ToString(), 8, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "15% ", 4, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], " اجمالى ضريبة القيمة المضافة", 14, 0, 1, 1, 1, 1, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 1, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);



            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.Notes, 23, 0, 1, 1, 1, 1, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], "ملاحظات", 7, 0, 0, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            //line 06
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], totalDetail.ToString(), 8, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "الصافى شامل ض. ق. VAT ", 15, 0, 1, 1, 1, 1, 5, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], ConvertNumToString(_totalVal.ToString()), 34, 0, 1, 1, 1, 1, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            //line 06
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], organizationsVM.AccountBank, 45, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "الحساب البنكى :  ", 12, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);


            headerTitle = "المستلم                       :                             ";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 28, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 1, 0, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            bodyTable1.AddCell(cell);

            headerTitle = "البائع                                   :                                ";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 28, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            bodyTable1.AddCell(cell);

            cell = new PdfPCell(bodyTable1);
            cell.Colspan = (57);
            cell.BorderWidth = 1;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //cell.Padding = 10;
            tblParent.AddCell(cell);
            #endregion

            doc.Add(tblParent);
            doc.Close();
            return memoryStream.GetBuffer();

        }


        public static string Encrypt(string encryptString)
        {
            string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
                    0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptString;
        }

        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
                    0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }


        public static byte[] GenInvoiceN_S(List<VoucherDetailsVM> VoucherDetailsVM, OrganizationsVM organizationsVM, InvoicesVM InvoicesVMObj, DataTable voucher, string costCenterName,string? QRcodeV)
        {


            try
            {
                string JournalNumber = InvoicesVMObj.JournalNumber.ToString();
                string CustomerName = voucher != null && voucher.Rows.Count > 0 && voucher.Columns.Contains("CustomerName") ? voucher.Rows[0]["CustomerName"].ToString() : "";
                string CustomerCode = voucher != null && voucher.Rows.Count > 0 && voucher.Columns.Contains("CustomerCode") ? voucher.Rows[0]["CustomerCode"].ToString() : "";

                bool isFooter = (organizationsVM.IsFooter != null && organizationsVM.IsFooter != "" || organizationsVM.IsFooter != "0") ? true : false;//send from user
                string footerData = "" + organizationsVM.NameAr + "  " + organizationsVM.Address + "  - تليفون  : " + organizationsVM.Mobile + " - فاكس  : " + organizationsVM.Fax + " -  بريد الكترونى  : " + organizationsVM.Email;
                Rectangle pagesize = new Rectangle(105, 148);
                Document doc = new Document(PageSize.A6, 10, 10, 10, 10);

                Document doc3 = new Document(PageSize.A4, 10, 10, 30, 44);
                Document doc2 = new Document(PageSize.A4, 10, 10, 30, 44);
                var memoryStream = PdfBase.OpenDocument(doc, "", false, "", isFooter, footerData);//طباعه افقية

                var headerTitle = "";

                PdfPCell cell = null;
                PdfPTable tblParent = new PdfPTable(1);
                tblParent.DefaultCell.Padding = 0;
                tblParent.WidthPercentage = 100;
                tblParent.DefaultCell.BorderWidth = 0;
                tblParent.HorizontalAlignment = Element.ALIGN_CENTER;
                tblParent.SplitLate = false;
                tblParent.HeaderRows = 1;

                #region HeaderReport
                PdfPTable headerTbl = new PdfPTable(57);
                headerTbl.DefaultCell.Padding = 10;
                headerTbl.WidthPercentage = 95;
                headerTbl.DefaultCell.BorderWidth = 1;
                headerTbl.DefaultCell.BorderColor = BaseColor.BLUE;
                headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

                #region FirstRow
                headerTitle = "";


                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], organizationsVM.NameAr, 57, 1, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
                headerTbl.AddCell(cell);

                DateTime Date = InvoicesVMObj.AddDate??DateTime.Now;
                string ActionDate = Date.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.CreateSpecificCulture("en"));

                var SupplierName = (organizationsVM.NameAr ?? "").TrimStart();

                SupplierName = SupplierName.TrimEnd();

                string qrBarcodeHash = QRCodeEncoder.encode(
                    new Seller_Inv(SupplierName),
                    new TaxNumber_Inv(organizationsVM.TaxCode),
                    new InvoiceDate_Inv(ActionDate),
                    new TotalAmount_Inv(InvoicesVMObj.TotalValue.ToString()),
                    new TaxAmount_Inv(InvoicesVMObj.TaxAmount.ToString())
                );
                string valueToEncode = organizationsVM.NameAr;

                if (QRcodeV != null)
                {
                    qrBarcodeHash = QRcodeV;
                }


                IDictionary<EncodeHintType, Object> hints = new Dictionary<EncodeHintType, object>();
                hints[EncodeHintType.CHARACTER_SET] = "UTF-8";

                //var bc2 = new BarcodeQRCode(InvoicesVMObj.QRCodeNum, 20, 20, null);
                var bc2 = new BarcodeQRCode(qrBarcodeHash, 350, 350, hints);

                iTextSharp.text.Image img1 = bc2.GetImage();
                cell = PdfBase.drawImageCell(img1, 80F, 70F, 16, 2, 1, 0, 1, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
                headerTbl.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_UNDERLINE.ToString()], " ", 1, 0, 0, 0, 0, 0, 2, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
                headerTbl.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_UNDERLINE.ToString()], "فاتورة ضريبية 'مبسطة' ", 40, 0, 0, 0, 0, 0, 5, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
                headerTbl.AddCell(cell);


                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], InvoicesVMObj.InvoiceNumber.ToString(), 9, 1, 0, 0, 0, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP);
                headerTbl.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "الرقم التسلسلي للفاتورة: ", 32, 1, 0, 0, 0, 0, 2, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
                headerTbl.AddCell(cell);

                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.InvoiceNumber.ToString(), 57, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
                //headerTbl.AddCell(cell);



                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], organizationsVM.Address, 20, 1, 0, 0, 0, 0, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP);
                headerTbl.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "العنوان: ", 9, 1, 0, 0, 0, 0, 3, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
                headerTbl.AddCell(cell);


                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], organizationsVM.TaxCode, 17, 1, 0, 0, 0, 0, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP);
                headerTbl.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "رقم ضريبى:", 11, 1, 0, 0, 0, 0, 3, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
                headerTbl.AddCell(cell);
                #endregion

                cell = new PdfPCell(headerTbl);
                cell.Colspan = 57;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.PaddingBottom = 5;

                tblParent.AddCell(cell);
                #endregion

                #region content header

                PdfPTable invoiceData = new PdfPTable(57);
                headerTbl.DefaultCell.Padding = 10;
                headerTbl.WidthPercentage = 95;
                headerTbl.DefaultCell.BorderWidth = 0;
                headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 57, 2, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);



                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.PayTypeName, 11, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "النوع:", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.AddDate.Value.ToString("h:mm tt"), 16, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);

                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.HijriDate.ToString(), 13, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                //invoiceData.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.Date.ToString(), 15, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "التاريخ:", 9, 0, 0, 0, 0, 0, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);


                cell = new PdfPCell(invoiceData);
                cell.Colspan = 57;
                cell.BorderWidth = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.PaddingBottom = 5;
                tblParent.AddCell(cell);
                #endregion

                #region table
                PdfPTable invoiceTable = new PdfPTable(57);
                invoiceTable.WidthPercentage = 95;
                invoiceTable.DefaultCell.BorderWidth = 1;
                invoiceTable.DefaultCell.BorderColor = BaseColor.BLUE;
                invoiceTable.HorizontalAlignment = Element.ALIGN_CENTER;

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 57, 2, 1, 1, 1, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);

                headerTitle = "الصافي+ض.ق.م";
                DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], headerTitle, 1, 8, 2, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

                headerTitle = "ض.ق المضافة";
                DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], headerTitle, 1, 8, 2, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                headerTitle = "الصافى";
                DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], headerTitle, 1, 7, 2, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

                headerTitle = "الخصم";
                DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], headerTitle, 1, 7, 2, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);


                headerTitle = "السعر";
                DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], headerTitle, 1, 8, 2, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

                headerTitle = "الكمية";
                DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], headerTitle, 1, 4, 2, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);


                //headerTitle = "الوحدة";
                //DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], headerTitle, 1, 5, 10, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);


                headerTitle = "الخدمة";
                DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], headerTitle, 1, 15, 10, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

                //headerTitle = "ر.الخدمة";
                //DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], headerTitle, 1, 6, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

                //headerTitle = "م";
                //DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], headerTitle, 1, 2, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

                decimal totalDetailsPrice = 0;
                decimal totalDiscount = 0;
                decimal percentage = 0;
                decimal Diff = 0;
                decimal AccualValue = 0;
                decimal DiscountValue_Det_Total_withqty = 0;
                decimal TotalInvWithoutDisc = 0;

                percentage = decimal.Parse(InvoicesVMObj.DiscountPercentage != null ? InvoicesVMObj.DiscountPercentage.ToString() : "0");

                for (int i = 0; i < VoucherDetailsVM.Count; i++)
                {
                    Diff = 0;
                    AccualValue = 0;
                    totalDetailsPrice = totalDetailsPrice + (decimal.Parse(VoucherDetailsVM[i] != null ? VoucherDetailsVM[i].Amount.ToString() : "0"));
                    Diff = Convert.ToDecimal(VoucherDetailsVM[i].TotalAmount - VoucherDetailsVM[i].TaxAmount);


                    if (VoucherDetailsVM[i].TaxType == 3)
                    {
                        AccualValue = (((VoucherDetailsVM[i].TotalAmount ?? 0) + (VoucherDetailsVM[i].DiscountValue_Det ?? 0)) / (VoucherDetailsVM[i].Qty ?? 1));
                    }
                    else
                    {
                        AccualValue = (((VoucherDetailsVM[i].Amount ?? 0) + (VoucherDetailsVM[i].DiscountValue_Det ?? 0)) / (VoucherDetailsVM[i].Qty ?? 1));
                    }
                    DiscountValue_Det_Total_withqty = DiscountValue_Det_Total_withqty + (VoucherDetailsVM[i].DiscountValue_Det) ?? 0;

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], VoucherDetailsVM[i].TotalAmount.ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? "" : ""), 8, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER);
                    invoiceTable.AddCell(cell);
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], VoucherDetailsVM[i].TaxAmount.ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? "" : ""), 7, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER);
                    invoiceTable.AddCell(cell);
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], Diff.ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? "" : ""), 8, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER);
                    invoiceTable.AddCell(cell);
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], VoucherDetailsVM[i].DiscountValue_Det.ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? "" : ""), 7, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER);
                    invoiceTable.AddCell(cell);
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], AccualValue.ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? "" : ""), 8, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER);
                    invoiceTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], VoucherDetailsVM[i].Qty.ToString(), 4, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER);
                    invoiceTable.AddCell(cell);

                    //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "خدمة", 5, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER);
                    //invoiceTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], VoucherDetailsVM[i].ServicesPriceName + ((i == (VoucherDetailsVM.Count) - 1) ? " " : ""), 15, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER);
                    invoiceTable.AddCell(cell);

                    //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (i + 1).ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? " " : ""), 6, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    //invoiceTable.AddCell(cell);

                    //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (i + 1).ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? " " : ""), 2, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    //invoiceTable.AddCell(cell);
                }

                totalDiscount = (percentage * totalDetailsPrice) / 100;
                totalDiscount = decimal.Parse(InvoicesVMObj.DiscountPercentage != null && InvoicesVMObj.DiscountPercentage > 0 ? totalDiscount.ToString() : (InvoicesVMObj.DiscountValue != null && InvoicesVMObj.DiscountValue > 0 ? InvoicesVMObj.DiscountValue.ToString() : "0"));
                decimal tax = ((totalDetailsPrice - totalDiscount) * 15) / 100;
                decimal totalValueOfInvoice = (totalDetailsPrice + tax) - (totalDiscount);
                cell = new PdfPCell(invoiceTable);
                cell.Colspan = 57;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                tblParent.AddCell(cell);


                //#endregion
                PdfPTable bodyTable1 = new PdfPTable(57);
                bodyTable1.WidthPercentage = 95;
                bodyTable1.DefaultCell.BorderWidth = 0;
                bodyTable1.HorizontalAlignment = Element.ALIGN_CENTER;
                decimal _paidValud = (InvoicesVMObj.PaidValue.HasValue ? InvoicesVMObj.PaidValue.Value : 0);
                decimal _totalVal = InvoicesVMObj.TotalValue.HasValue ? InvoicesVMObj.TotalValue.Value : 0;
                string remainder = (_totalVal - _paidValud).ToString();

                decimal discountPercentage = InvoicesVMObj.DiscountPercentage.HasValue ? InvoicesVMObj.DiscountPercentage.Value : default(decimal);
                decimal discountValue = InvoicesVMObj.DiscountValue.HasValue ? InvoicesVMObj.DiscountValue.Value : default(decimal);
                if (discountPercentage == default(decimal) && discountValue != default(decimal))
                {
                    discountPercentage = discountValue * 100 / InvoicesVMObj.InvoiceValue.Value;
                }

                if (discountPercentage != default(decimal) && discountValue == default(decimal))
                {
                    discountValue = (InvoicesVMObj.InvoiceValue.Value * discountPercentage) / 100;
                }
                decimal netVal = 0;

                if (VoucherDetailsVM.FirstOrDefault().TaxType == 3)
                {
                    netVal = InvoicesVMObj.TotalValue.Value - InvoicesVMObj.TaxAmount.Value;
                    TotalInvWithoutDisc = InvoicesVMObj.TotalValue ?? 0;

                }
                else
                {
                    netVal = InvoicesVMObj.InvoiceValue.Value;
                    TotalInvWithoutDisc = InvoicesVMObj.InvoiceValue ?? 0;
                }



                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 57, 3, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM);
                bodyTable1.AddCell(cell);

                //line 06
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 45, 2, 0, 0, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 12, 2, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);


                //line 0
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], (TotalInvWithoutDisc + DiscountValue_Det_Total_withqty).ToString(), 10, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "الاجمالي", 9, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 2, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                // cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], _paidValud.ToString(), 10, 1, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                // cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "المدفوع", 10, 1, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], (""), 16, 0, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], (TotalInvWithoutDisc + DiscountValue_Det_Total_withqty).ToString(), 10, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "الاجمالي الكلى", 9, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 2, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], remainder, 10, 1, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "المتبقى", 10, 1, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 16, 0, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);



                //line 03
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], String.Format("{0:0.00}", DiscountValue_Det_Total_withqty), 10, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], String.Format("{0:0.00}", ((DiscountValue_Det_Total_withqty * 100) / ((TotalInvWithoutDisc + DiscountValue_Det_Total_withqty)))) + "%", 9, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "الخصم", 12, 0, 0, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 27, 0, 0, 0, 0, 0, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                //line 04
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], String.Format("{0:0.00}", TotalInvWithoutDisc.ToString()), 19, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "إجم. بعد الخصم", 12, 0, 0, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 7, 0, 0, 0, 0, 0, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 12, 0, 0, 0, 0, 0, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 7, 0, 0, 0, 0, 0, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                bodyTable1.AddCell(cell);

                //line 04
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], String.Format("{0:0.00}", netVal), 19, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], " الصافى", 12, 0, 0, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 7, 0, 0, 0, 0, 0, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);


                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 12, 0, 0, 0, 0, 0, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 7, 0, 0, 0, 0, 0, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                bodyTable1.AddCell(cell);


                //line 05
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], InvoicesVMObj.TaxAmount.ToString(), 10, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "15% ", 9, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], " اجمالى ض.ق.م ", 12, 0, 0, 1, 1, 1, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 1, 0, 0, 0, 0, 0, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);



                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 18, 0, 0, 0, 0, 0, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 7, 0, 0, 0, 0, 0, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);


                //line 06
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], _totalVal.ToString(), 9, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "الصافى شامل ض.ق.م ", 15, 0, 0, 1, 1, 1, 5, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], ConvertNumToString(_totalVal.ToString()), 33, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], organizationsVM.AccountBank, 45, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                //bodyTable1.AddCell(cell);
                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "الحساب البنكى :  ", 12, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                //bodyTable1.AddCell(cell);


                //headerTitle = "المستلم                       :                             ";
                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], headerTitle, 28, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                //bodyTable1.AddCell(cell);

                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 1, 0, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                //bodyTable1.AddCell(cell);

                //headerTitle = "البائع                                   :                                ";
                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], headerTitle, 28, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                //bodyTable1.AddCell(cell);




                cell = new PdfPCell(bodyTable1);
                cell.Colspan = (57);
                cell.BorderWidth = 1;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //cell.Padding = 10;
                tblParent.AddCell(cell);



                PdfPTable FData = new PdfPTable(57);
                FData.DefaultCell.Padding = 10;
                FData.WidthPercentage = 95;
                FData.DefaultCell.BorderWidth = 0;
                FData.HorizontalAlignment = Element.ALIGN_CENTER;


                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
                FData.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
                FData.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_16_UNDERLINE.ToString()], "نشكر لكم زيارتكم", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
                FData.AddCell(cell);


                cell = new PdfPCell(FData);
                cell.Colspan = 57;
                cell.BorderWidth = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.PaddingBottom = 5;
                tblParent.AddCell(cell);





                #endregion

                doc.Add(tblParent);
                doc.Close();
                return memoryStream.GetBuffer();
            }
            catch (Exception ex)
            {

                var mes = ex.Message;
                bool isFooter = (organizationsVM.IsFooter != null && organizationsVM.IsFooter != "" || organizationsVM.IsFooter != "0") ? true : false;//send from user
                string footerData = "" + organizationsVM.NameAr + "  " + organizationsVM.Address + "  - تليفون  : " + organizationsVM.Mobile + " - فاكس  : " + organizationsVM.Fax + " -  بريد الكترونى  : " + organizationsVM.Email;
                Rectangle pagesize = new Rectangle(105, 148);
                Document doc = new Document(PageSize.A6, 10, 10, 10, 10);
                var memoryStream = PdfBase.OpenDocument(doc, "", false, "", isFooter, footerData);//طباعه افقية

                return memoryStream.GetBuffer();
            }



        }

        public static byte[] GenInvoiceN_SDraft(List<VoucherDetailsVM> VoucherDetailsVM, OrganizationsVM organizationsVM, InvoicesVM InvoicesVMObj, DataTable voucher, string costCenterName, string? QRcodeV)
        {


            try
            {
                string JournalNumber = InvoicesVMObj.JournalNumber.ToString();
                string CustomerName = voucher != null && voucher.Rows.Count > 0 && voucher.Columns.Contains("CustomerName") ? voucher.Rows[0]["CustomerName"].ToString() : "";
                string CustomerCode = voucher != null && voucher.Rows.Count > 0 && voucher.Columns.Contains("CustomerCode") ? voucher.Rows[0]["CustomerCode"].ToString() : "";

                bool isFooter = (organizationsVM.IsFooter != null && organizationsVM.IsFooter != "" || organizationsVM.IsFooter != "0") ? true : false;//send from user
                string footerData = "" + organizationsVM.NameAr + "  " + organizationsVM.Address + "  - تليفون  : " + organizationsVM.Mobile + " - فاكس  : " + organizationsVM.Fax + " -  بريد الكترونى  : " + organizationsVM.Email;
                Rectangle pagesize = new Rectangle(105, 148);
                Document doc = new Document(PageSize.A6, 10, 10, 10, 10);

                Document doc3 = new Document(PageSize.A4, 10, 10, 30, 44);
                Document doc2 = new Document(PageSize.A4, 10, 10, 30, 44);
                var memoryStream = PdfBase.OpenDocument(doc, "", false, "", isFooter, footerData);//طباعه افقية

                var headerTitle = "";

                PdfPCell cell = null;
                PdfPTable tblParent = new PdfPTable(1);
                tblParent.DefaultCell.Padding = 0;
                tblParent.WidthPercentage = 100;
                tblParent.DefaultCell.BorderWidth = 0;
                tblParent.HorizontalAlignment = Element.ALIGN_CENTER;
                tblParent.SplitLate = false;
                tblParent.HeaderRows = 1;

                #region HeaderReport
                PdfPTable headerTbl = new PdfPTable(57);
                headerTbl.DefaultCell.Padding = 10;
                headerTbl.WidthPercentage = 95;
                headerTbl.DefaultCell.BorderWidth = 1;
                headerTbl.DefaultCell.BorderColor = BaseColor.BLUE;
                headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

                #region FirstRow
                headerTitle = "";


                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], organizationsVM.NameAr, 57, 1, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
                headerTbl.AddCell(cell);

                DateTime Date = InvoicesVMObj.AddDate ?? DateTime.Now;
                string ActionDate = Date.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.CreateSpecificCulture("en"));

                var SupplierName = (organizationsVM.NameAr ?? "").TrimStart();

                SupplierName = SupplierName.TrimEnd();

                string qrBarcodeHash = QRCodeEncoder.encode(
                    new Seller_Inv(SupplierName),
                    new TaxNumber_Inv(organizationsVM.TaxCode),
                    new InvoiceDate_Inv(ActionDate),
                    new TotalAmount_Inv(InvoicesVMObj.TotalValue.ToString()),
                    new TaxAmount_Inv(InvoicesVMObj.TaxAmount.ToString())
                );

                if (QRcodeV != null)
                {
                    qrBarcodeHash = QRcodeV;
                }
                string valueToEncode = organizationsVM.NameAr;




                IDictionary<EncodeHintType, Object> hints = new Dictionary<EncodeHintType, object>();
                hints[EncodeHintType.CHARACTER_SET] = "UTF-8";

                //var bc2 = new BarcodeQRCode(InvoicesVMObj.QRCodeNum, 20, 20, null);
                var bc2 = new BarcodeQRCode(qrBarcodeHash, 350, 350, hints);

                iTextSharp.text.Image img1 = bc2.GetImage();
                //cell = PdfBase.drawImageCell(img1, 80F, 70F, 16, 2, 1, 0, 1, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
                //headerTbl.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_UNDERLINE.ToString()], " ", 1, 0, 0, 0, 0, 0, 2, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
                headerTbl.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_UNDERLINE.ToString()], "فاتورة ضريبية 'مبسطة' ", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
                headerTbl.AddCell(cell);


                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], InvoicesVMObj.InvoiceNumber.ToString(), 25, 1, 0, 0, 0, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP);
                headerTbl.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "الرقم التسلسلي للفاتورة: ", 32, 1, 0, 0, 0, 0, 2, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
                headerTbl.AddCell(cell);

                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.InvoiceNumber.ToString(), 57, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
                //headerTbl.AddCell(cell);



                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], organizationsVM.Address, 20, 1, 0, 0, 0, 0, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP);
                headerTbl.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "العنوان: ", 9, 1, 0, 0, 0, 0, 3, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
                headerTbl.AddCell(cell);


                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], organizationsVM.TaxCode, 17, 1, 0, 0, 0, 0, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP);
                headerTbl.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "رقم ضريبى:", 11, 1, 0, 0, 0, 0, 3, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
                headerTbl.AddCell(cell);
                #endregion

                cell = new PdfPCell(headerTbl);
                cell.Colspan = 57;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.PaddingBottom = 5;

                tblParent.AddCell(cell);
                #endregion

                #region content header

                PdfPTable invoiceData = new PdfPTable(57);
                headerTbl.DefaultCell.Padding = 10;
                headerTbl.WidthPercentage = 95;
                headerTbl.DefaultCell.BorderWidth = 0;
                headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 57, 2, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);



                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.PayTypeName, 11, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "النوع:", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.AddDate.Value.ToString("h:mm tt"), 16, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);

                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.HijriDate.ToString(), 13, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                //invoiceData.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.Date.ToString(), 15, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "التاريخ:", 9, 0, 0, 0, 0, 0, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);


                cell = new PdfPCell(invoiceData);
                cell.Colspan = 57;
                cell.BorderWidth = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.PaddingBottom = 5;
                tblParent.AddCell(cell);
                #endregion

                #region table
                PdfPTable invoiceTable = new PdfPTable(57);
                invoiceTable.WidthPercentage = 95;
                invoiceTable.DefaultCell.BorderWidth = 1;
                invoiceTable.DefaultCell.BorderColor = BaseColor.BLUE;
                invoiceTable.HorizontalAlignment = Element.ALIGN_CENTER;

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 57, 2, 1, 1, 1, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                invoiceData.AddCell(cell);

                headerTitle = "الصافي+ض.ق.م";
                DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], headerTitle, 1, 8, 2, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

                headerTitle = "ض.ق المضافة";
                DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], headerTitle, 1, 8, 2, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                headerTitle = "الصافى";
                DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], headerTitle, 1, 7, 2, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

                headerTitle = "الخصم";
                DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], headerTitle, 1, 7, 2, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);


                headerTitle = "السعر";
                DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], headerTitle, 1, 8, 2, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

                headerTitle = "الكمية";
                DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], headerTitle, 1, 4, 2, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);


                //headerTitle = "الوحدة";
                //DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], headerTitle, 1, 5, 10, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);


                headerTitle = "الخدمة";
                DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], headerTitle, 1, 15, 10, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

                //headerTitle = "ر.الخدمة";
                //DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], headerTitle, 1, 6, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

                //headerTitle = "م";
                //DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], headerTitle, 1, 2, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

                decimal totalDetailsPrice = 0;
                decimal totalDiscount = 0;
                decimal percentage = 0;
                decimal Diff = 0;
                decimal AccualValue = 0;
                decimal DiscountValue_Det_Total_withqty = 0;
                decimal TotalInvWithoutDisc = 0;

                percentage = decimal.Parse(InvoicesVMObj.DiscountPercentage != null ? InvoicesVMObj.DiscountPercentage.ToString() : "0");

                for (int i = 0; i < VoucherDetailsVM.Count; i++)
                {
                    Diff = 0;
                    AccualValue = 0;
                    totalDetailsPrice = totalDetailsPrice + (decimal.Parse(VoucherDetailsVM[i] != null ? VoucherDetailsVM[i].Amount.ToString() : "0"));
                    Diff = Convert.ToDecimal(VoucherDetailsVM[i].TotalAmount - VoucherDetailsVM[i].TaxAmount);


                    if (VoucherDetailsVM[i].TaxType == 3)
                    {
                        AccualValue = (((VoucherDetailsVM[i].TotalAmount ?? 0) + (VoucherDetailsVM[i].DiscountValue_Det ?? 0)) / (VoucherDetailsVM[i].Qty ?? 1));
                    }
                    else
                    {
                        AccualValue = (((VoucherDetailsVM[i].Amount ?? 0) + (VoucherDetailsVM[i].DiscountValue_Det ?? 0)) / (VoucherDetailsVM[i].Qty ?? 1));
                    }
                    DiscountValue_Det_Total_withqty = DiscountValue_Det_Total_withqty + (VoucherDetailsVM[i].DiscountValue_Det) ?? 0;

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], VoucherDetailsVM[i].TotalAmount.ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? "" : ""), 8, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER);
                    invoiceTable.AddCell(cell);
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], VoucherDetailsVM[i].TaxAmount.ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? "" : ""), 7, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER);
                    invoiceTable.AddCell(cell);
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], Diff.ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? "" : ""), 8, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER);
                    invoiceTable.AddCell(cell);
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], VoucherDetailsVM[i].DiscountValue_Det.ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? "" : ""), 7, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER);
                    invoiceTable.AddCell(cell);
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], AccualValue.ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? "" : ""), 8, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER);
                    invoiceTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], VoucherDetailsVM[i].Qty.ToString(), 4, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER);
                    invoiceTable.AddCell(cell);

                    //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "خدمة", 5, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER);
                    //invoiceTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], VoucherDetailsVM[i].ServicesPriceName + ((i == (VoucherDetailsVM.Count) - 1) ? " " : ""), 15, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER);
                    invoiceTable.AddCell(cell);

                    //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (i + 1).ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? " " : ""), 6, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    //invoiceTable.AddCell(cell);

                    //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (i + 1).ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? " " : ""), 2, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    //invoiceTable.AddCell(cell);
                }

                totalDiscount = (percentage * totalDetailsPrice) / 100;
                totalDiscount = decimal.Parse(InvoicesVMObj.DiscountPercentage != null && InvoicesVMObj.DiscountPercentage > 0 ? totalDiscount.ToString() : (InvoicesVMObj.DiscountValue != null && InvoicesVMObj.DiscountValue > 0 ? InvoicesVMObj.DiscountValue.ToString() : "0"));
                decimal tax = ((totalDetailsPrice - totalDiscount) * 15) / 100;
                decimal totalValueOfInvoice = (totalDetailsPrice + tax) - (totalDiscount);
                cell = new PdfPCell(invoiceTable);
                cell.Colspan = 57;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                tblParent.AddCell(cell);


                //#endregion
                PdfPTable bodyTable1 = new PdfPTable(57);
                bodyTable1.WidthPercentage = 95;
                bodyTable1.DefaultCell.BorderWidth = 0;
                bodyTable1.HorizontalAlignment = Element.ALIGN_CENTER;
                decimal _paidValud = (InvoicesVMObj.PaidValue.HasValue ? InvoicesVMObj.PaidValue.Value : 0);
                decimal _totalVal = InvoicesVMObj.TotalValue.HasValue ? InvoicesVMObj.TotalValue.Value : 0;
                string remainder = (_totalVal - _paidValud).ToString();

                decimal discountPercentage = InvoicesVMObj.DiscountPercentage.HasValue ? InvoicesVMObj.DiscountPercentage.Value : default(decimal);
                decimal discountValue = InvoicesVMObj.DiscountValue.HasValue ? InvoicesVMObj.DiscountValue.Value : default(decimal);
                if (discountPercentage == default(decimal) && discountValue != default(decimal))
                {
                    discountPercentage = discountValue * 100 / InvoicesVMObj.InvoiceValue.Value;
                }

                if (discountPercentage != default(decimal) && discountValue == default(decimal))
                {
                    discountValue = (InvoicesVMObj.InvoiceValue.Value * discountPercentage) / 100;
                }
                decimal netVal = 0;

                if (VoucherDetailsVM.FirstOrDefault().TaxType == 3)
                {
                    netVal = InvoicesVMObj.TotalValue.Value - InvoicesVMObj.TaxAmount.Value;
                    TotalInvWithoutDisc = InvoicesVMObj.TotalValue ?? 0;

                }
                else
                {
                    netVal = InvoicesVMObj.InvoiceValue.Value;
                    TotalInvWithoutDisc = InvoicesVMObj.InvoiceValue ?? 0;
                }



                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 57, 3, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM);
                bodyTable1.AddCell(cell);

                //line 06
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 45, 2, 0, 0, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 12, 2, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);


                //line 0
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], (TotalInvWithoutDisc + DiscountValue_Det_Total_withqty).ToString(), 10, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "الاجمالي", 9, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 2, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                // cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], _paidValud.ToString(), 10, 1, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                // cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "المدفوع", 10, 1, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], (""), 16, 0, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], (TotalInvWithoutDisc + DiscountValue_Det_Total_withqty).ToString(), 10, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "الاجمالي الكلى", 9, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 2, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], remainder, 10, 1, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "المتبقى", 10, 1, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 16, 0, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);



                //line 03
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], String.Format("{0:0.00}", DiscountValue_Det_Total_withqty), 10, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], String.Format("{0:0.00}", ((DiscountValue_Det_Total_withqty * 100) / ((TotalInvWithoutDisc + DiscountValue_Det_Total_withqty)))) + "%", 9, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "الخصم", 12, 0, 0, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 27, 0, 0, 0, 0, 0, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                //line 04
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], String.Format("{0:0.00}", TotalInvWithoutDisc.ToString()), 19, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "إجم. بعد الخصم", 12, 0, 0, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 7, 0, 0, 0, 0, 0, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 12, 0, 0, 0, 0, 0, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 7, 0, 0, 0, 0, 0, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                bodyTable1.AddCell(cell);

                //line 04
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], String.Format("{0:0.00}", netVal), 19, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], " الصافى", 12, 0, 0, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 7, 0, 0, 0, 0, 0, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);


                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 12, 0, 0, 0, 0, 0, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 7, 0, 0, 0, 0, 0, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                bodyTable1.AddCell(cell);


                //line 05
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], InvoicesVMObj.TaxAmount.ToString(), 10, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "15% ", 9, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], " اجمالى ض.ق.م ", 12, 0, 0, 1, 1, 1, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 1, 0, 0, 0, 0, 0, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);



                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 18, 0, 0, 0, 0, 0, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 7, 0, 0, 0, 0, 0, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);


                //line 06
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], _totalVal.ToString(), 9, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "الصافى شامل ض.ق.م ", 15, 0, 0, 1, 1, 1, 5, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], ConvertNumToString(_totalVal.ToString()), 33, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                //cell.Padding = 10;
                bodyTable1.AddCell(cell);

                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], organizationsVM.AccountBank, 45, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                //bodyTable1.AddCell(cell);
                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "الحساب البنكى :  ", 12, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                //bodyTable1.AddCell(cell);


                //headerTitle = "المستلم                       :                             ";
                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], headerTitle, 28, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                //bodyTable1.AddCell(cell);

                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 1, 0, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                //bodyTable1.AddCell(cell);

                //headerTitle = "البائع                                   :                                ";
                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], headerTitle, 28, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                //bodyTable1.AddCell(cell);




                cell = new PdfPCell(bodyTable1);
                cell.Colspan = (57);
                cell.BorderWidth = 1;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //cell.Padding = 10;
                tblParent.AddCell(cell);



                PdfPTable FData = new PdfPTable(57);
                FData.DefaultCell.Padding = 10;
                FData.WidthPercentage = 95;
                FData.DefaultCell.BorderWidth = 0;
                FData.HorizontalAlignment = Element.ALIGN_CENTER;


                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
                FData.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
                FData.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_16_UNDERLINE.ToString()], "نشكر لكم زيارتكم", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
                FData.AddCell(cell);


                cell = new PdfPCell(FData);
                cell.Colspan = 57;
                cell.BorderWidth = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.PaddingBottom = 5;
                tblParent.AddCell(cell);





                #endregion

                doc.Add(tblParent);
                doc.Close();
                return memoryStream.GetBuffer();
            }
            catch (Exception ex)
            {

                var mes = ex.Message;
                bool isFooter = (organizationsVM.IsFooter != null && organizationsVM.IsFooter != "" || organizationsVM.IsFooter != "0") ? true : false;//send from user
                string footerData = "" + organizationsVM.NameAr + "  " + organizationsVM.Address + "  - تليفون  : " + organizationsVM.Mobile + " - فاكس  : " + organizationsVM.Fax + " -  بريد الكترونى  : " + organizationsVM.Email;
                Rectangle pagesize = new Rectangle(105, 148);
                Document doc = new Document(PageSize.A6, 10, 10, 10, 10);
                var memoryStream = PdfBase.OpenDocument(doc, "", false, "", isFooter, footerData);//طباعه افقية

                return memoryStream.GetBuffer();
            }



        }

        public static byte[] GenInvoiceNRet_S(List<VoucherDetailsVM> VoucherDetailsVM, OrganizationsVM organizationsVM, InvoicesVM InvoicesVMObj, DataTable voucher, string costCenterName)
        {
            string JournalNumber = InvoicesVMObj.JournalNumber.ToString();
            string CustomerName = voucher != null && voucher.Rows.Count > 0 && voucher.Columns.Contains("CustomerName") ? voucher.Rows[0]["CustomerName"].ToString() : "";
            string CustomerCode = voucher != null && voucher.Rows.Count > 0 && voucher.Columns.Contains("CustomerCode") ? voucher.Rows[0]["CustomerCode"].ToString() : "";

            bool isFooter = (organizationsVM.IsFooter != null && organizationsVM.IsFooter != "" || organizationsVM.IsFooter != "0") ? true : false;//send from user
            string footerData = "" + organizationsVM.NameAr + "  " + organizationsVM.Address + "  - تليفون  : " + organizationsVM.Mobile + " - فاكس  : " + organizationsVM.Fax + " -  بريد الكترونى  : " + organizationsVM.Email;
            Rectangle pagesize = new Rectangle(105, 148);
            Document doc = new Document(PageSize.A6, 10, 10, 10, 10);

            Document doc3 = new Document(PageSize.A4, 10, 10, 30, 44);
            Document doc2 = new Document(PageSize.A4, 10, 10, 30, 44);
            var memoryStream = PdfBase.OpenDocument(doc, "", false, "", isFooter, footerData);//طباعه افقية

            var headerTitle = "";

            PdfPCell cell = null;
            PdfPTable tblParent = new PdfPTable(1);
            tblParent.DefaultCell.Padding = 0;
            tblParent.WidthPercentage = 100;
            tblParent.DefaultCell.BorderWidth = 0;
            tblParent.HorizontalAlignment = Element.ALIGN_CENTER;
            tblParent.SplitLate = false;
            tblParent.HeaderRows = 1;

            #region HeaderReport
            PdfPTable headerTbl = new PdfPTable(57);
            headerTbl.DefaultCell.Padding = 10;
            headerTbl.WidthPercentage = 95;
            headerTbl.DefaultCell.BorderWidth = 1;
            headerTbl.DefaultCell.BorderColor = BaseColor.BLUE;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            #region FirstRow
            headerTitle = "";


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], organizationsVM.NameAr, 57, 1, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            headerTbl.AddCell(cell);


            if (InvoicesVMObj.QRCodeNum != "")
            {
                var bc2 = new iTextSharp.text.pdf.BarcodeQRCode(InvoicesVMObj.QRCodeNum, 20, 20, null);
                iTextSharp.text.Image img1 = bc2.GetImage();
                cell = PdfBase.drawImageCell(img1, 60F, 50F, 16, 2, 1, 0, 1, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
                headerTbl.AddCell(cell);

            }
            else
            {
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], " ", 16, 2, 1, 0, 1, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
                headerTbl.AddCell(cell);
            }

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_UNDERLINE.ToString()], " ", 1, 0, 0, 0, 0, 0, 2, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
            //headerTbl.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_UNDERLINE.ToString()], "مردود فاتورة ضريبية 'مبسطة ' ", 41, 0, 0, 0, 0, 0, 5, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
            headerTbl.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], InvoicesVMObj.InvoiceNumber.ToString(), 7, 1, 0, 0, 0, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "الرقم التسلسلي للفاتورة: ", 34, 1, 0, 0, 0, 0, 2, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.InvoiceNumber.ToString(), 57, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            //headerTbl.AddCell(cell);



            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], organizationsVM.Address, 16, 1, 0, 0, 0, 0, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "العنوان: ", 10, 1, 0, 0, 0, 0, 3, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], organizationsVM.TaxCode, 17, 1, 0, 0, 0, 0, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "رقم ضريبى:", 14, 1, 0, 0, 0, 0, 3, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            #endregion

            cell = new PdfPCell(headerTbl);
            cell.Colspan = 57;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;

            tblParent.AddCell(cell);
            #endregion

            #region content header

            PdfPTable invoiceData = new PdfPTable(57);
            headerTbl.DefaultCell.Padding = 10;
            headerTbl.WidthPercentage = 95;
            headerTbl.DefaultCell.BorderWidth = 0;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 57, 2, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);



            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.PayTypeName, 11, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "النوع:", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.AddDate.Value.ToString("h:mm tt"), 16, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.HijriDate.ToString(), 13, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.Date.ToString(), 15, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "التاريخ:", 9, 0, 0, 0, 0, 0, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);


            cell = new PdfPCell(invoiceData);
            cell.Colspan = 57;
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            tblParent.AddCell(cell);
            #endregion

            #region table
            PdfPTable invoiceTable = new PdfPTable(57);
            invoiceTable.WidthPercentage = 95;
            invoiceTable.DefaultCell.BorderWidth = 1;
            invoiceTable.DefaultCell.BorderColor = BaseColor.BLUE;
            invoiceTable.HorizontalAlignment = Element.ALIGN_CENTER;

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 57, 2, 1, 1, 1, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);

            headerTitle = "الصافي+ض.ق.م";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], headerTitle, 1, 10, 2, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "ض.ق المضافة";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], headerTitle, 1, 9, 2, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            headerTitle = "الصافى";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], headerTitle, 1, 10, 2, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "السعر";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], headerTitle, 1, 8, 2, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "الكمية";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], headerTitle, 1, 5, 2, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);


            //headerTitle = "الوحدة";
            //DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], headerTitle, 1, 5, 10, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);


            headerTitle = "الخدمة";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], headerTitle, 1, 15, 10, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            //headerTitle = "ر.الخدمة";
            //DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], headerTitle, 1, 6, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            //headerTitle = "م";
            //DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], headerTitle, 1, 2, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            decimal totalDetailsPrice = 0;
            decimal totalDiscount = 0;
            decimal percentage = 0;
            decimal Diff = 0;
            percentage = decimal.Parse(InvoicesVMObj.DiscountPercentage != null ? InvoicesVMObj.DiscountPercentage.ToString() : "0");

            for (int i = 0; i < VoucherDetailsVM.Count; i++)
            {
                Diff = 0;
                totalDetailsPrice = totalDetailsPrice + (decimal.Parse(VoucherDetailsVM[i] != null ? VoucherDetailsVM[i].Amount.ToString() : "0"));
                Diff = Convert.ToDecimal(VoucherDetailsVM[i].TotalAmount - VoucherDetailsVM[i].TaxAmount);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], VoucherDetailsVM[i].TotalAmount.ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? "" : ""), 10, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], VoucherDetailsVM[i].TaxAmount.ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? "" : ""), 9, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], Diff.ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? "" : ""), 10, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], VoucherDetailsVM[i].Amount.ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? "" : ""), 8, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "1 ", 5, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);

                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "خدمة", 5, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER);
                //invoiceTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], VoucherDetailsVM[i].ServicesPriceName + ((i == (VoucherDetailsVM.Count) - 1) ? " " : ""), 15, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);

                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (i + 1).ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? " " : ""), 6, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                //invoiceTable.AddCell(cell);

                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (i + 1).ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? " " : ""), 2, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                //invoiceTable.AddCell(cell);
            }

            totalDiscount = (percentage * totalDetailsPrice) / 100;
            totalDiscount = decimal.Parse(InvoicesVMObj.DiscountPercentage != null && InvoicesVMObj.DiscountPercentage > 0 ? totalDiscount.ToString() : (InvoicesVMObj.DiscountValue != null && InvoicesVMObj.DiscountValue > 0 ? InvoicesVMObj.DiscountValue.ToString() : "0"));
            decimal tax = ((totalDetailsPrice - totalDiscount) * 15) / 100;
            decimal totalValueOfInvoice = (totalDetailsPrice + tax) - (totalDiscount);
            cell = new PdfPCell(invoiceTable);
            cell.Colspan = 57;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            tblParent.AddCell(cell);


            //#endregion
            PdfPTable bodyTable1 = new PdfPTable(57);
            bodyTable1.WidthPercentage = 95;
            bodyTable1.DefaultCell.BorderWidth = 0;
            bodyTable1.HorizontalAlignment = Element.ALIGN_CENTER;
            decimal _paidValud = (InvoicesVMObj.PaidValue.HasValue ? InvoicesVMObj.PaidValue.Value : 0);
            decimal _totalVal = InvoicesVMObj.TotalValue.HasValue ? InvoicesVMObj.TotalValue.Value : 0;
            string remainder = (_totalVal - _paidValud).ToString();

            decimal discountPercentage = InvoicesVMObj.DiscountPercentage.HasValue ? InvoicesVMObj.DiscountPercentage.Value : default(decimal);
            decimal discountValue = InvoicesVMObj.DiscountValue.HasValue ? InvoicesVMObj.DiscountValue.Value : default(decimal);
            if (discountPercentage == default(decimal) && discountValue != default(decimal))
            {
                discountPercentage = discountValue * 100 / InvoicesVMObj.InvoiceValue.Value;
            }

            if (discountPercentage != default(decimal) && discountValue == default(decimal))
            {
                discountValue = (InvoicesVMObj.InvoiceValue.Value * discountPercentage) / 100;
            }
            decimal netVal = 0;

            if (VoucherDetailsVM.FirstOrDefault().TaxType == 3)
            {
                netVal = InvoicesVMObj.InvoiceValue.Value - InvoicesVMObj.TaxAmount.Value - discountValue;

            }
            else
            {
                netVal = InvoicesVMObj.InvoiceValue.Value - discountValue;

            }



            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 57, 3, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM);
            bodyTable1.AddCell(cell);

            //line 06
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 45, 2, 0, 0, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 12, 2, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);


            //line 0
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], InvoicesVMObj.InvoiceValue.ToString(), 10, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "الاجمالي", 9, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 2, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            // cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], _paidValud.ToString(), 10, 1, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            // cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "المدفوع", 10, 1, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], (""), 16, 0, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], InvoicesVMObj.InvoiceValue.ToString(), 10, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "الاجمالي الكلى", 9, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 2, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], remainder, 10, 1, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "المتبقى", 10, 1, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 16, 0, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);



            //line 03
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], String.Format("{0:0.00}", totalDiscount), 10, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], String.Format("{0:0.00}", discountPercentage) + "%", 9, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "الخصم", 12, 0, 0, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 27, 0, 0, 0, 0, 0, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            //line 04
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], String.Format("{0:0.00}", netVal), 19, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], " الصافى", 12, 0, 0, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 7, 0, 0, 0, 0, 0, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 12, 0, 0, 0, 0, 0, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 7, 0, 0, 0, 0, 0, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            bodyTable1.AddCell(cell);


            //line 05
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], InvoicesVMObj.TaxAmount.ToString(), 10, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "15% ", 9, 0, 1, 1, 1, 1, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], " اجمالى ض.ق.م ", 12, 0, 0, 1, 1, 1, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 1, 0, 0, 0, 0, 0, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);



            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 18, 0, 0, 0, 0, 0, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 7, 0, 0, 0, 0, 0, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);


            //line 06
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], _totalVal.ToString(), 9, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "الصافى شامل ض.ق.م ", 15, 0, 0, 1, 1, 1, 5, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], ConvertNumToString(_totalVal.ToString()), 33, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], organizationsVM.AccountBank, 45, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //bodyTable1.AddCell(cell);
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "الحساب البنكى :  ", 12, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //bodyTable1.AddCell(cell);


            //headerTitle = "المستلم                       :                             ";
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], headerTitle, 28, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], "", 1, 0, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //bodyTable1.AddCell(cell);

            //headerTitle = "البائع                                   :                                ";
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], headerTitle, 28, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //bodyTable1.AddCell(cell);




            cell = new PdfPCell(bodyTable1);
            cell.Colspan = (57);
            cell.BorderWidth = 1;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //cell.Padding = 10;
            tblParent.AddCell(cell);



            PdfPTable FData = new PdfPTable(57);
            FData.DefaultCell.Padding = 10;
            FData.WidthPercentage = 95;
            FData.DefaultCell.BorderWidth = 0;
            FData.HorizontalAlignment = Element.ALIGN_CENTER;


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            FData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            FData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_16_UNDERLINE.ToString()], "نشكر لكم زيارتكم", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            FData.AddCell(cell);


            cell = new PdfPCell(FData);
            cell.Colspan = 57;
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            tblParent.AddCell(cell);





            #endregion

            doc.Add(tblParent);
            doc.Close();
            return memoryStream.GetBuffer();

        }


        public static byte[] GenInvoiceNRet(List<VoucherDetailsVM> VoucherDetailsVM, OrganizationsVM organizationsVM, InvoicesVM InvoicesVMObj, DataTable voucher, string costCenterName)
        {
            string JournalNumber = InvoicesVMObj.JournalNumber.ToString();
            string CustomerName = voucher != null && voucher.Rows.Count > 0 && voucher.Columns.Contains("CustomerName") ? voucher.Rows[0]["CustomerName"].ToString() : "";
            string CustomerCode = voucher != null && voucher.Rows.Count > 0 && voucher.Columns.Contains("CustomerCode") ? voucher.Rows[0]["CustomerCode"].ToString() : "";

            bool isFooter = (organizationsVM.IsFooter != null && organizationsVM.IsFooter != "" || organizationsVM.IsFooter != "0") ? true : false;//send from user
            string footerData = "" + organizationsVM.NameAr + "  " + organizationsVM.Address + "  - تليفون  : " + organizationsVM.Mobile + " - فاكس  : " + organizationsVM.Fax + " -  بريد الكترونى  : " + organizationsVM.Email;

            Document doc = new Document(PageSize.A4, 10, 10, 30, 44);
            var memoryStream = PdfBase.OpenDocument(doc, "", false, "", isFooter, footerData);//طباعه افقية

            var headerTitle = "";

            PdfPCell cell = null;
            PdfPTable tblParent = new PdfPTable(1);
            tblParent.DefaultCell.Padding = 0;
            tblParent.WidthPercentage = 100;
            tblParent.DefaultCell.BorderWidth = 0;
            tblParent.HorizontalAlignment = Element.ALIGN_CENTER;
            tblParent.SplitLate = false;
            tblParent.HeaderRows = 1;

            #region HeaderReport
            PdfPTable headerTbl = new PdfPTable(57);
            headerTbl.DefaultCell.Padding = 10;
            headerTbl.WidthPercentage = 95;
            headerTbl.DefaultCell.BorderWidth = 1;
            headerTbl.DefaultCell.BorderColor = BaseColor.BLUE;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            #region FirstRow
            headerTitle = "";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "Tax Number", 7, 3, 0, 0, 0, 0, 5, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            headerTbl.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], organizationsVM.TaxCode, 10, 3, 0, 0, 0, 0, 5, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            headerTbl.AddCell(cell);

            if (organizationsVM.LogoUrl != "")
            {
                string imageFilePathLogo = PdfBase.getReportImagePath(organizationsVM.LogoUrl);

                if (imageFilePathLogo != "")
                {
                    Image headerJpg = Image.GetInstance(imageFilePathLogo); //infoReport[1];

                    cell = PdfBase.drawImageCell(headerJpg, 90F, 80F, 24, 3, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
                    headerTbl.AddCell(cell);
                }
                else
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 24, 3, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
            }
            else
            {
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 24, 3, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                headerTbl.AddCell(cell);
            }

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], organizationsVM.NameAr, 16, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], organizationsVM.PostalCode, 9, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "السجل التجارى: ", 7, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], organizationsVM.TaxCode, 10, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "رقم ضريبى:", 6, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "", 40, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], organizationsVM.Mobile, 11, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "الهاتف:", 6, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "", 10, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], organizationsVM.Address, 41, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "العنوان: ", 6, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);



            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "Tax Number", 9, 1, 0, 0, 0, 0, 5, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            //headerTbl.AddCell(cell);
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], organizationsVM.TaxCode, 17, 1, 0, 0, 0, 0, 5, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            //headerTbl.AddCell(cell);
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "", 5, 1, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //headerTbl.AddCell(cell);
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], organizationsVM.TaxCode, 17, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //headerTbl.AddCell(cell);
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "الــرقم الضريبى", 9, 1, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //headerTbl.AddCell(cell);

            #endregion
            #region secondRow
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_16_UNDERLINE.ToString()], organizationsVM.NameAr, 57, 2, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            //headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_16_UNDERLINE.ToString()], "مردود فاتورة المبيعات", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            headerTbl.AddCell(cell);
            #endregion

            cell = new PdfPCell(headerTbl);
            cell.Colspan = 57;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;

            tblParent.AddCell(cell);
            #endregion

            #region content header

            PdfPTable invoiceData = new PdfPTable(57);
            headerTbl.DefaultCell.Padding = 10;
            headerTbl.WidthPercentage = 95;
            headerTbl.DefaultCell.BorderWidth = 0;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 57, 2, 1, 1, 1, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], costCenterName, 12, 0, 1, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "م.التكلفة:", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.InvoiceNumber.ToString(), 7, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "ر.الفاتورة:", 5, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.PayTypeName, 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "النوع:", 5, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.Date.ToString(), 11, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //invoiceData.AddCell(cell);
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "التاريخ:", 5, 0, 0, 1, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            //invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.Date.ToString(), 11, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "التاريخ:", 5, 0, 0, 1, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);






            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.InvoiceReference, 12, 0, 1, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "المرجع:", 6, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "ريال سعودى", 7, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "العملة:", 5, 0, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.AddDate.Value.ToString("h:mm tt"), 6, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "الوقت:", 5, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.HijriDate.ToString(), 11, 0, 0, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "الموافق", 5, 0, 0, 1, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);








            var xx = InvoicesVMObj.CustomerType;
            //var val = "العنوان";
            string address = "", taxCodeofClient = "";
            if (InvoicesVMObj.CustomerType == "2")
            {
                //val = "الرقم الضريبي للعميل";
                address = "";
                taxCodeofClient = InvoicesVMObj.Address;
            }
            else
            {
                //val = "العنوان";
                address = InvoicesVMObj.Address;
                taxCodeofClient = "";
            }

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 57, 1, 0, 1, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 57, 2, 1, 1, 0, 1, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.CustomerPhone, 11, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "الهاتف:", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], taxCodeofClient, 13, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "برقم ضريبى:", 7, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.CustomerName, 14, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "العميل ", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 4, 0, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], JournalNumber == "" ? "\n" : JournalNumber, 11, 0, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "القيد:", 6, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.ProjectNo, 13, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "رقم المشروع:", 7, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "", 13, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //invoiceData.AddCell(cell);



            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], address, 14, 0, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], " العنوان", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], JournalNumber == "" ? "\n" : JournalNumber, 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //invoiceData.AddCell(cell);
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "القيد", 5, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //invoiceData.AddCell(cell);
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "", 46, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //invoiceData.AddCell(cell);

            cell = new PdfPCell(invoiceData);
            cell.Colspan = 57;
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            tblParent.AddCell(cell);
            #endregion

            #region table
            PdfPTable invoiceTable = new PdfPTable(57);
            invoiceTable.WidthPercentage = 95;
            invoiceTable.DefaultCell.BorderWidth = 1;
            invoiceTable.DefaultCell.BorderColor = BaseColor.BLUE;
            invoiceTable.HorizontalAlignment = Element.ALIGN_CENTER;

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 57, 2, 1, 1, 1, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);

            headerTitle = "الصافي +الضريبة";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 8, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "ض. قيمة مضافة";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 8, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            headerTitle = "الصافى";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 6, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "السعر";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 6, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "الكمية";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);


            headerTitle = "الوحدة";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);


            headerTitle = "البيان";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 13, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "ر.الخدمة";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 6, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "م";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 2, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            decimal totalDetailsPrice = 0;
            decimal totalDiscount = 0;
            decimal percentage = 0;
            decimal Diff = 0;
            percentage = decimal.Parse(InvoicesVMObj.DiscountPercentage != null ? InvoicesVMObj.DiscountPercentage.ToString() : "0");

            for (int i = 0; i < VoucherDetailsVM.Count; i++)
            {
                Diff = 0;
                totalDetailsPrice = totalDetailsPrice + (decimal.Parse(VoucherDetailsVM[i] != null ? VoucherDetailsVM[i].Amount.ToString() : "0"));
                Diff = Convert.ToDecimal(VoucherDetailsVM[i].TotalAmount - VoucherDetailsVM[i].TaxAmount);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], VoucherDetailsVM[i].TotalAmount.ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? "" : ""), 8, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], VoucherDetailsVM[i].TaxAmount.ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? "" : ""), 8, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], Diff.ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? "" : ""), 6, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], VoucherDetailsVM[i].Amount.ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? "" : ""), 6, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "1 ", 4, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "خدمة", 4, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], VoucherDetailsVM[i].ServicesPriceName + ((i == (VoucherDetailsVM.Count) - 1) ? " " : ""), 13, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (i + 1).ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? " " : ""), 6, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (i + 1).ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? " " : ""), 2, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);
            }

            totalDiscount = (percentage * totalDetailsPrice) / 100;
            totalDiscount = decimal.Parse(InvoicesVMObj.DiscountPercentage != null && InvoicesVMObj.DiscountPercentage > 0 ? totalDiscount.ToString() : (InvoicesVMObj.DiscountValue != null && InvoicesVMObj.DiscountValue > 0 ? InvoicesVMObj.DiscountValue.ToString() : "0"));
            decimal tax = ((totalDetailsPrice - totalDiscount) * 15) / 100;
            decimal totalValueOfInvoice = (totalDetailsPrice + tax) - (totalDiscount);
            cell = new PdfPCell(invoiceTable);
            cell.Colspan = 57;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            tblParent.AddCell(cell);


            //#endregion
            PdfPTable bodyTable1 = new PdfPTable(57);
            bodyTable1.WidthPercentage = 95;
            bodyTable1.DefaultCell.BorderWidth = 0;
            bodyTable1.HorizontalAlignment = Element.ALIGN_CENTER;
            decimal _paidValud = (InvoicesVMObj.PaidValue.HasValue ? InvoicesVMObj.PaidValue.Value : 0);
            decimal _totalVal = InvoicesVMObj.TotalValue.HasValue ? InvoicesVMObj.TotalValue.Value : 0;
            string remainder = (_totalVal - _paidValud).ToString();

            decimal discountPercentage = InvoicesVMObj.DiscountPercentage.HasValue ? InvoicesVMObj.DiscountPercentage.Value : default(decimal);
            decimal discountValue = InvoicesVMObj.DiscountValue.HasValue ? InvoicesVMObj.DiscountValue.Value : default(decimal);
            if (discountPercentage == default(decimal) && discountValue != default(decimal))
            {
                discountPercentage = discountValue * 100 / InvoicesVMObj.InvoiceValue.Value;
            }

            if (discountPercentage != default(decimal) && discountValue == default(decimal))
            {
                discountValue = (InvoicesVMObj.InvoiceValue.Value * discountPercentage) / 100;
            }
            decimal netVal = 0;

            if (VoucherDetailsVM.FirstOrDefault().TaxType == 3)
            {
                netVal = InvoicesVMObj.InvoiceValue.Value - InvoicesVMObj.TaxAmount.Value - discountValue;

            }
            else
            {
                netVal = InvoicesVMObj.InvoiceValue.Value - discountValue;

            }



            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 57, 3, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM);
            bodyTable1.AddCell(cell);

            //line 06
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 45, 2, 0, 0, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 12, 2, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);


            //line 0
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], InvoicesVMObj.InvoiceValue.ToString(), 8, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "الاجمالي", 8, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 2, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            // cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], _paidValud.ToString(), 8, 1, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            // cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "المدفوع", 8, 1, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (""), 23, 0, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 7, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //// cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], "", 7, 0, 0, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            ////line 01
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "00.00 ", 8, 0, 1, 1, 1, 1, 5, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "الاضافات", 8, 0, 1, 1, 1, 1, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 41, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            //line 02
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], InvoicesVMObj.InvoiceValue.ToString(), 8, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "الاجمالي الكلى", 8, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 2, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], remainder, 8, 1, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "المتبقى", 8, 1, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "", 23, 0, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "2021-02-04 ", 7, 0, 0, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], "الاستحقاق", 7, 0, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            //line 03
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], String.Format("{0:0.00}", totalDiscount), 5, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], String.Format("{0:0.00}", discountPercentage) + "%", 3, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "الخصم", 8, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 43, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            //line 04
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], String.Format("{0:0.00}", netVal), 8, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " الصافى", 8, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 7, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.AddUser, 27, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], "المستخدم", 7, 0, 0, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "2 ", 7, 0, 0, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], "رقم النسخة", 7, 0, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            ////cell.Padding = 10;
            //bodyTable1.AddCell(cell);

            //line 05
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], InvoicesVMObj.TaxAmount.ToString(), 8, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "15% ", 4, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], " اجمالى ضريبة القيمة المضافة", 14, 0, 1, 1, 1, 1, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 1, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);



            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.Notes, 23, 0, 1, 1, 1, 1, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], "ملاحظات", 7, 0, 0, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            //line 06
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], _totalVal.ToString(), 8, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "الصافى شامل ض. ق. VAT ", 15, 0, 1, 1, 1, 1, 5, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], ConvertNumToString(_totalVal.ToString()), 34, 0, 1, 1, 1, 1, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);

            //line 06
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], organizationsVM.AccountBank, 45, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "الحساب البنكى :  ", 12, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            bodyTable1.AddCell(cell);


            headerTitle = "المستلم                       :                             ";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 28, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            bodyTable1.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 1, 0, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            bodyTable1.AddCell(cell);

            headerTitle = "البائع                                   :                                ";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 28, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            bodyTable1.AddCell(cell);

            cell = new PdfPCell(bodyTable1);
            cell.Colspan = (57);
            cell.BorderWidth = 1;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //cell.Padding = 10;
            tblParent.AddCell(cell);
            #endregion

            doc.Add(tblParent);
            doc.Close();
            return memoryStream.GetBuffer();

        }




        //نموذج 1
        public static byte[] GenInvoice(List<VoucherDetailsVM> VoucherDetailsVM, OrganizationsVM organizationsVM, InvoicesVM InvoicesVMObj, DataTable voucher)
        {
            string JournalNumber = InvoicesVMObj.JournalNumber.ToString();
            string CustomerName = voucher != null && voucher.Rows.Count > 0 && voucher.Columns.Contains("CustomerName") ? voucher.Rows[0]["CustomerName"].ToString() : "";
            string CustomerCode = voucher != null && voucher.Rows.Count > 0 && voucher.Columns.Contains("CustomerCode") ? voucher.Rows[0]["CustomerCode"].ToString() : "";

            bool isFooter = (organizationsVM.IsFooter != null && organizationsVM.IsFooter != "" || organizationsVM.IsFooter != "0") ? true : false;//send from user
            string footerData = "" + organizationsVM.NameAr + "  " + organizationsVM.Address + "  - تليفون  : " + organizationsVM.Mobile + " - فاكس  : " + organizationsVM.Fax + " -  بريد الكترونى  : " + organizationsVM.Email;

            Document doc = new Document(PageSize.A4, 10, 10, 30, 44);
            var memoryStream = PdfBase.OpenDocument(doc, "", false, "", isFooter, footerData);//طباعه افقية

            var headerTitle = "";

            PdfPCell cell = null;
            PdfPTable tblParent = new PdfPTable(1);
            tblParent.DefaultCell.Padding = 0;
            tblParent.WidthPercentage = 100;
            tblParent.DefaultCell.BorderWidth = 0;
            tblParent.HorizontalAlignment = Element.ALIGN_CENTER;
            tblParent.SplitLate = false;
            tblParent.HeaderRows = 1;

            #region HeaderReport
            PdfPTable headerTbl = new PdfPTable(57);
            headerTbl.DefaultCell.Padding = 10;
            headerTbl.WidthPercentage = 95;
            headerTbl.DefaultCell.BorderWidth = 1;
            headerTbl.DefaultCell.BorderColor = BaseColor.BLUE;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            #region FirstRow
            headerTitle = "";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "الــرقم الضريبى", 11, 1, 0, 0, 0, 0, 5, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            if (organizationsVM.LogoUrl != "")
            {
                string imageFilePathLogo = PdfBase.getReportImagePath(organizationsVM.LogoUrl);

                if (imageFilePathLogo != "")
                {
                    Image headerJpg = Image.GetInstance(imageFilePathLogo); //infoReport[1];

                    cell = PdfBase.drawImageCell(headerJpg, 90F, 80F, 35, 3, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
                    headerTbl.AddCell(cell);
                }
                else
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 35, 3, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
            }
            else
            {
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 35, 3, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                headerTbl.AddCell(cell);
            }

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "Invoice - فاتورة  ", 11, 3, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], organizationsVM.TaxCode, 11, 1, 0, 0, 0, 0, 5, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
            headerTbl.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "رقــم القــيــد", 11, 1, 1, 1, 1, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], JournalNumber == "" ? "\n" : JournalNumber, 11, 1, 1, 1, 0, 1, 10, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
            headerTbl.AddCell(cell);

            #endregion
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_16_UNDERLINE.ToString()], "", 57, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            #region secondRow
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_16_UNDERLINE.ToString()], organizationsVM.NameAr, 57, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            #endregion

            cell = new PdfPCell(headerTbl);
            cell.Colspan = 57;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            tblParent.AddCell(cell);
            #endregion
            #region content header

            PdfPTable invoiceData = new PdfPTable(57);
            headerTbl.DefaultCell.Padding = 10;
            headerTbl.WidthPercentage = 95;
            headerTbl.DefaultCell.BorderWidth = 0;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "رقم المشروع", 10, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            cell.PaddingBottom = 7;
            cell.PaddingTop = 5;
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 10, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.PaddingBottom = 7;
            cell.PaddingTop = 5;
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 17, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.PaddingBottom = 7;
            cell.PaddingTop = 5;
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.InvoiceNumber.ToString(), 10, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.PaddingBottom = 7;
            cell.PaddingTop = 5;
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "رقم الفاتورة", 10, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            cell.PaddingBottom = 7;
            cell.PaddingTop = 5;
            invoiceData.AddCell(cell);
            //end of line one

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.ProjectNo, 10, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            cell.PaddingBottom = 7;
            cell.PaddingTop = 5;
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "", 10, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.PaddingBottom = 7;
            cell.PaddingTop = 5;
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.HijriDate.ToString(), 17, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            cell.PaddingBottom = 7;
            cell.PaddingTop = 5;
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.Date.ToString(), 10, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.PaddingBottom = 7;
            cell.PaddingTop = 5;
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "تاريخ الاصدار", 10, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            cell.PaddingBottom = 7;
            cell.PaddingTop = 5;
            invoiceData.AddCell(cell);

            //end of line 2


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "", 10, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            cell.PaddingBottom = 7;
            cell.PaddingTop = 5;
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "", 10, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.PaddingBottom = 7;
            cell.PaddingTop = 5;
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "", 17, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.PaddingBottom = 7;
            cell.PaddingTop = 5;
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "", 10, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.PaddingBottom = 7;
            cell.PaddingTop = 5;
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "", 10, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            cell.PaddingBottom = 7;
            cell.PaddingTop = 5;
            invoiceData.AddCell(cell);
            //end of line 3
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "", 10, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            cell.PaddingBottom = 7;
            cell.PaddingTop = 5;
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "", 10, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.PaddingBottom = 7;
            cell.PaddingTop = 5;
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.CustomerName, 17, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            cell.PaddingBottom = 7;
            cell.PaddingTop = 5;
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], voucher.Rows[0]["codefrom"].ToString(), 10, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.PaddingBottom = 7;
            cell.PaddingTop = 5;
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "كود العميل", 10, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            cell.PaddingBottom = 7;
            cell.PaddingTop = 5;
            invoiceData.AddCell(cell);
            //end of line 4
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "", 10, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            cell.PaddingBottom = 7;
            cell.PaddingTop = 5;
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "", 10, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.PaddingBottom = 7;
            cell.PaddingTop = 5;
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "", 17, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.PaddingBottom = 7;
            cell.PaddingTop = 5;
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], organizationsVM.TaxCode.ToString(), 10, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.PaddingBottom = 7;
            cell.PaddingTop = 5;
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "الرقم الضريبى", 10, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            cell.PaddingBottom = 7;
            cell.PaddingTop = 5;
            invoiceData.AddCell(cell);
            //end of line 5
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "", 10, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            cell.PaddingBottom = 7;
            cell.PaddingTop = 5;
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "", 10, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.PaddingBottom = 7;
            cell.PaddingTop = 5;
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "", 17, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.PaddingBottom = 7;
            cell.PaddingTop = 5;
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], InvoicesVMObj.Address, 10, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.PaddingBottom = 7;
            cell.PaddingTop = 5;
            invoiceData.AddCell(cell);
            var xx = InvoicesVMObj.CustomerType;
            var val = "العنوان";
            if (InvoicesVMObj.CustomerType == "2")
            {
                val = "الرقم الضريبي للعميل";
            }
            else
            {
                val = "العنوان";
            }

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], val, 10, 0, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            cell.PaddingBottom = 7;
            cell.PaddingTop = 5;
            invoiceData.AddCell(cell);
            //end of line 6

            cell = new PdfPCell(invoiceData);
            cell.Colspan = 57;
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            tblParent.AddCell(cell);


            #endregion
            #region table
            PdfPTable invoiceTable = new PdfPTable(57);
            invoiceTable.WidthPercentage = 95;
            invoiceTable.DefaultCell.BorderWidth = 1;
            invoiceTable.DefaultCell.BorderColor = BaseColor.BLUE;
            invoiceTable.HorizontalAlignment = Element.ALIGN_CENTER;


            headerTitle = "الاجمالى";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 9, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "قيمة الضريبة";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 9, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "السعر";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 9, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);


            headerTitle = "اسم الخدمة";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 19, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "رقم الخدمة";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 9, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "م";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 2, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            decimal totalDetailsPrice = 0;
            decimal totalDiscount = 0;
            decimal percentage = 0;
            percentage = decimal.Parse(InvoicesVMObj.DiscountPercentage != null ? InvoicesVMObj.DiscountPercentage.ToString() : "0");

            for (int i = 0; i < VoucherDetailsVM.Count; i++)
            {

                totalDetailsPrice = totalDetailsPrice + (decimal.Parse(VoucherDetailsVM[i] != null ? VoucherDetailsVM[i].Amount.ToString() : "0"));
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], VoucherDetailsVM[i].TotalAmount.ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? "\n \n\n \n\n \n\n \n\n \n\n\n\n\n " : ""), 9, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], VoucherDetailsVM[i].TaxAmount.ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? "\n \n\n \n\n \n\n \n\n \n\n\n\n\n  " : ""), 9, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], VoucherDetailsVM[i].Amount.ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? "\n \n\n \n\n \n\n \n\n \n\n\n\n\n  " : ""), 9, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], VoucherDetailsVM[i].ServicesPriceName + ((i == (VoucherDetailsVM.Count) - 1) ? "\n \n\n \n\n \n\n \n\n \n\n\n\n\n  " : ""), 19, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], voucher.Rows[i]["codeto"] + ((i == (VoucherDetailsVM.Count) - 1) ? "\n \n\n \n\n \n\n \n\n \n\n\n\n\n  " : ""), 9, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (i + 1).ToString() + ((i == (VoucherDetailsVM.Count) - 1) ? "\n \n\n \n\n \n\n \n\n \n\n\n\n\n  " : ""), 2, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);
            }

            totalDiscount = (percentage * totalDetailsPrice) / 100;
            totalDiscount = decimal.Parse(InvoicesVMObj.DiscountPercentage != null && InvoicesVMObj.DiscountPercentage > 0 ? totalDiscount.ToString() : (InvoicesVMObj.DiscountValue != null && InvoicesVMObj.DiscountValue > 0 ? InvoicesVMObj.DiscountValue.ToString() : "0"));
            decimal tax = ((totalDetailsPrice - totalDiscount) * 15) / 100;
            decimal totalValueOfInvoice = (totalDetailsPrice + tax) - (totalDiscount);
            cell = new PdfPCell(invoiceTable);
            cell.Colspan = 57;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            tblParent.AddCell(cell);

            #endregion
            #region footer
            PdfPTable tblFooter = new PdfPTable(57);
            tblFooter.WidthPercentage = 95;
            tblFooter.DefaultCell.BorderWidth = 0;
            tblFooter.HorizontalAlignment = Element.ALIGN_CENTER;

            headerTitle = "الإجمالى بدون ضريبة القيمه المضافة :  " + InvoicesVMObj.InvoiceValue.ToString();
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 30, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            tblFooter.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 27, 5, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            tblFooter.AddCell(cell);


            headerTitle = "قيمة ضريبة القيمة المضافة :   " + InvoicesVMObj.TaxAmount.ToString();
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 30, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            tblFooter.AddCell(cell);

            //float value= InvoicesVMObj.DiscountPercentage != null && InvoicesVMObj.DiscountPercentage > 0 
            headerTitle = "الخصم :  " + totalDiscount.ToString();
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 30, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            tblFooter.AddCell(cell);

            headerTitle = "الإجمالى  : " + InvoicesVMObj.TotalValue.ToString();
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 30, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            tblFooter.AddCell(cell);

            headerTitle = "الإجمالى شامل  القيمه المضافة كتابتا  :  " + ConvertNumToString(InvoicesVMObj.TotalValue.ToString());
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 30, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            tblFooter.AddCell(cell);

            headerTitle = InvoicesVMObj.Notes;
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 47, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            tblFooter.AddCell(cell);

            headerTitle = "";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 10, 0, 0, 0, 0, 0, 15, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            tblFooter.AddCell(cell);

            if (InvoicesVMObj.printBankAccount == true)
            {
                headerTitle = organizationsVM.AccountBank;
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], headerTitle, 47, 0, 1, 1, 1, 1, 15, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                tblFooter.AddCell(cell);

                headerTitle = "  الحساب البنكى :";
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 10, 0, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                tblFooter.AddCell(cell);
            }
            headerTitle = "المستلم                       :                             Received By";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 28, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            tblFooter.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 1, 0, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            tblFooter.AddCell(cell);

            headerTitle = "توقيع                                   :                               Sign ";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 28, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            tblFooter.AddCell(cell);

            cell = new PdfPCell(tblFooter);
            cell.Colspan = 57;
            cell.BorderWidth = 0;
            cell.Padding = 10;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            tblParent.AddCell(cell);

            #endregion
            doc.Add(tblParent);
            doc.Close();
            return memoryStream.GetBuffer();

        }
        //نموذج 2
        public static byte[] GenInvoice2(List<VoucherDetailsVM> VoucherDetailsVM, OrganizationsVM organizationsVM, InvoicesVM InvoicesVMObj, DataTable voucher)
        {
            string JournalNumber = InvoicesVMObj.JournalNumber.ToString();
            string CustomerName = voucher != null && voucher.Rows.Count > 0 && voucher.Columns.Contains("CustomerName") ? voucher.Rows[0]["CustomerName"].ToString() : "";
            string CustomerCode = voucher != null && voucher.Rows.Count > 0 && voucher.Columns.Contains("CustomerCode") ? voucher.Rows[0]["CustomerCode"].ToString() : "";


            bool isFooter = (organizationsVM.IsFooter != null && organizationsVM.IsFooter != "" || organizationsVM.IsFooter != "0") ? true : false;//send from user
            string footerData = "" + organizationsVM.NameAr + "  " + organizationsVM.Address + "  - تليفون  : " + organizationsVM.Mobile + " - فاكس  : " + organizationsVM.Fax + " -  بريد الكترونى  : " + organizationsVM.Email;

            Document doc = new Document(PageSize.A4, 10, 10, 30, 44);
            var memoryStream = PdfBase.OpenDocument(doc, "", false, "", isFooter, footerData);//طباعه راسية

            var headerTitle = "";

            PdfPCell cell = null;
            PdfPTable tblParent = new PdfPTable(1);
            tblParent.DefaultCell.Padding = 0;
            tblParent.WidthPercentage = 100;
            tblParent.DefaultCell.BorderWidth = 0;
            tblParent.HorizontalAlignment = Element.ALIGN_CENTER;
            tblParent.SplitLate = false;
            tblParent.HeaderRows = 1;
            #region HeaderReport
            PdfPTable headerTbl = new PdfPTable(57);
            headerTbl.DefaultCell.Padding = 10;
            headerTbl.WidthPercentage = 95;
            headerTbl.DefaultCell.BorderWidth = 1;
            headerTbl.DefaultCell.BorderColor = BaseColor.BLUE;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            #region FirstRow
            headerTitle = "";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "الــرقم الضريبى", 11, 1, 0, 0, 0, 0, 5, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            if (organizationsVM.LogoUrl != "")
            {
                string imageFilePathLogo = PdfBase.getReportImagePath(organizationsVM.LogoUrl);

                if (imageFilePathLogo != "")
                {
                    Image headerJpg = Image.GetInstance(imageFilePathLogo); //infoReport[1];

                    cell = PdfBase.drawImageCell(headerJpg, 90F, 80F, 35, 3, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
                    headerTbl.AddCell(cell);
                }
                else
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 35, 3, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
            }
            else
            {
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 35, 3, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                headerTbl.AddCell(cell);
            }

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "Invoice - فاتورة ", 11, 1, 1, 1, 1, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            headerTbl.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], organizationsVM.TaxCode, 11, 1, 0, 0, 0, 0, 5, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], InvoicesVMObj.InvoiceNumber.ToString(), 11, 1, 1, 1, 0, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM);
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "رقــم القــيــد ", 11, 1, 1, 1, 1, 0, 5, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
            headerTbl.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_13_UNDERLINE.ToString()], "", 22, 1, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], JournalNumber == "" ? "\n" : JournalNumber, 11, 1, 1, 1, 0, 1, 10, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
            headerTbl.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_16_UNDERLINE.ToString()], "", 57, 1, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            headerTbl.AddCell(cell);

            #endregion
            #region secondRow
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_16_UNDERLINE.ToString()], organizationsVM.NameAr, 57, 1, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            headerTbl.AddCell(cell);
            #endregion

            cell = new PdfPCell(headerTbl);
            cell.Colspan = 57;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;

            tblParent.AddCell(cell);
            #endregion
            #region content header 

            PdfPTable invoiceData = new PdfPTable(57);
            invoiceData.DefaultCell.Padding = 10;
            invoiceData.WidthPercentage = 100;
            invoiceData.HorizontalAlignment = Element.ALIGN_CENTER;
            invoiceData.DefaultCell.BorderWidth = 0;
            if (InvoicesVMObj.PayType == 1 || InvoicesVMObj.PayType == 8)
                headerTitle = "نقدى                  " + (InvoicesVMObj.PayType == 1 ? "√" : "") + "                     Cash \n اجل               " + (InvoicesVMObj.PayType == 8 ? "√" : "") + "                        Credit";

            else
                headerTitle = "نقدى                                     Cash \n اجل                                       Credit";

            cell.Padding = 10;
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], headerTitle, 18, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], "رقم المشروع  -  " + InvoicesVMObj.ProjectNo + "", 14, 0, 0, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], InvoicesVMObj.CustomerName + "  -  " + voucher.Rows[0]["codefrom"].ToString(), 20, 0, 0, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], "السادة : ", 5, 0, 0, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            invoiceData.AddCell(cell);



            cell = new PdfPCell(invoiceData);
            cell.Colspan = 57;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Padding = 10;
            cell.BorderWidth = 0;
            tblParent.AddCell(cell);

            #endregion
            #region table
            PdfPTable invoiceTable = new PdfPTable(57);
            invoiceTable.WidthPercentage = 95;
            invoiceTable.DefaultCell.BorderWidth = 0;
            invoiceTable.DefaultCell.BorderColor = BaseColor.BLUE;
            invoiceTable.HorizontalAlignment = Element.ALIGN_CENTER;


            headerTitle = " كود الخدمة \n Acc Code";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 9, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "اسم الصنف Description";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 20, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "سعر الوحدة \n unit Price";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 8, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "السعر الاجمالى \n Total Price";
            DrawTableHeader(invoiceTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 20, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            decimal totalDetailsPrice = 0;
            decimal totalDiscount = 0;
            decimal percentage = 0;
            percentage = decimal.Parse(InvoicesVMObj.DiscountPercentage != null ? InvoicesVMObj.DiscountPercentage.ToString() : "0");

            for (int i = 0; i < VoucherDetailsVM.Count; i++)
            {

                totalDetailsPrice = totalDetailsPrice + (decimal.Parse(VoucherDetailsVM[i] != null ? VoucherDetailsVM[i].Amount.ToString() : "0"));
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], voucher.Rows[i]["codeto"] + ((i == (VoucherDetailsVM.Count) - 1) ? "\n \n\n \n\n \n\n \n\n \n\n\n\n\n \n\n\n\n" : ""), 9, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], VoucherDetailsVM[i].ServicesPriceName + ((i == (VoucherDetailsVM.Count) - 1) ? "\n \n\n \n\n \n\n \n\n \n\n\n\n\n \n\n\n\n " : ""), 20, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], VoucherDetailsVM[i].Amount + ((i == (VoucherDetailsVM.Count) - 1) ? "\n \n\n \n\n \n\n \n\n \n\n\n\n\n \n\n\n\n" : ""), 8, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], VoucherDetailsVM[i].TotalAmount + ((i == (VoucherDetailsVM.Count) - 1) ? "\n \n\n \n\n \n\n \n\n \n\n\n\n\n \n\n\n\n " : ""), 20, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                invoiceTable.AddCell(cell);
            }
            totalDiscount = (percentage * totalDetailsPrice) / 100;
            totalDiscount = decimal.Parse(InvoicesVMObj.DiscountPercentage != null && InvoicesVMObj.DiscountPercentage > 0 ? totalDiscount.ToString() : (InvoicesVMObj.DiscountValue != null && InvoicesVMObj.DiscountValue > 0 ? InvoicesVMObj.DiscountValue.ToString() : "0"));
            decimal tax = ((totalDetailsPrice - totalDiscount) * 15) / 100;
            decimal totalValueOfInvoice = (totalDetailsPrice + tax) - (totalDiscount);
            cell = new PdfPCell(invoiceTable);
            cell.Colspan = 57;
            cell.Padding = 10;
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            tblParent.AddCell(cell);

            #endregion
            #region footer
            PdfPTable tblFooter = new PdfPTable(57);
            tblFooter.WidthPercentage = 95;
            tblFooter.DefaultCell.BorderWidth = 0;
            tblFooter.HorizontalAlignment = Element.ALIGN_CENTER;

            headerTitle = InvoicesVMObj.Notes;
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 25, 3, 1, 1, 1, 0, 40, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            tblFooter.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 1, 1, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            tblFooter.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "الإجمالى قبل الضريبة \n Amount", 15, 1, 1, 1, 1, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            tblFooter.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 1, 1, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            tblFooter.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.InvoiceValue.ToString(), 15, 1, 1, 1, 1, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            tblFooter.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 1, 1, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            tblFooter.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "خصم \n Discount", 15, 1, 1, 1, 1, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            tblFooter.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 1, 1, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            tblFooter.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (totalDiscount.ToString()), 15, 1, 1, 1, 1, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            tblFooter.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 1, 1, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            tblFooter.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "الضريبة\n VAT  ", 15, 1, 1, 1, 1, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            tblFooter.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 1, 1, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            tblFooter.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.TaxAmount.ToString(), 15, 1, 1, 1, 1, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            tblFooter.AddCell(cell);

            headerTitle = ConvertNumToString(InvoicesVMObj.TotalValue.ToString());
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 25, 0, 1, 1, 1, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            tblFooter.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 1, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            tblFooter.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "الإجمالى شامل الضريبة\n Total", 15, 0, 1, 1, 1, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            tblFooter.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 1, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            tblFooter.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], InvoicesVMObj.TotalValue.ToString(), 15, 0, 1, 1, 1, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            tblFooter.AddCell(cell);


            if (InvoicesVMObj.printBankAccount == true)
            {
                string accountBank = InvoicesVMObj.printBankAccount == true ? organizationsVM.AccountBank : "";

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], accountBank, 41, 0, 1, 1, 1, 1, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                tblFooter.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 1, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                tblFooter.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "الحــســاب البــنــكى", 31, 0, 1, 1, 1, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                tblFooter.AddCell(cell);

            }
            else
            {
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 41, 0, 1, 1, 1, 1, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                tblFooter.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 1, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                tblFooter.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 31, 0, 1, 1, 1, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                tblFooter.AddCell(cell);
            }
            headerTitle = "المستلم                       :                             Received By";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 28, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            tblFooter.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 1, 0, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            tblFooter.AddCell(cell);

            headerTitle = "توقيع                                   :                               Sign ";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 28, 0, 1, 1, 1, 1, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            tblFooter.AddCell(cell);

            cell = new PdfPCell(tblFooter);
            cell.Colspan = 57;
            cell.BorderWidth = 0;
            cell.Padding = 10;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            tblParent.AddCell(cell);

            #endregion
            doc.Add(tblParent);
            doc.Close();
            return memoryStream.GetBuffer();

        }
        //التقرير القيود
        public static byte[] PrintDailyVoucherReport(DataTable dt, string[] infoReport)
        {

            bool isFooter = (infoReport[6] != null && infoReport[6] != "" || infoReport[6] != "0") ? true : false;//send from user
            string footerData = "" + infoReport[0] + "  " + infoReport[2] + "  - تليفون  : " + infoReport[5] + " - فاكس  : " + infoReport[4] + " -  بريد الكترونى  : " + infoReport[3];
            Document doc = new Document(PageSize.A4, 10, 10, 30, 44);
            var memoryStream = PdfBase.OpenDocument(doc, "", false, "", isFooter, footerData);

            var headerTitle = "";

            PdfPCell cell = null;
            PdfPTable tblParent = new PdfPTable(1);
            tblParent.DefaultCell.Padding = 0;
            tblParent.WidthPercentage = 95;
            tblParent.DefaultCell.BorderWidth = 0;
            tblParent.HorizontalAlignment = Element.ALIGN_CENTER;
            tblParent.SplitLate = false;
            tblParent.HeaderRows = 1;

            //#region HeaderReport
            //PdfPTable headerTbl = new PdfPTable(57);
            //headerTbl.DefaultCell.Padding = 5;
            //headerTbl.WidthPercentage = 95;
            //headerTbl.DefaultCell.BorderWidthBottom = 1;
            //headerTbl.DefaultCell.BorderColor = BaseColor.BLUE;
            //headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            //#region FirstRow
            //headerTitle = "";


            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " ", 57, 2, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            //headerTbl.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], "تاريخ السند ميلادى : " + dt.Rows[0]["InvoiceDate"].ToString() + "", 19, 1, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            //cell.Padding = 5;
            //headerTbl.AddCell(cell);

            //if (infoReport[1] != "")
            //{
            //    string imageFilePathLogo = PdfBase.getReportImagePath(infoReport[1]);

            //    if (imageFilePathLogo != "")
            //    {
            //        Image headerJpg = Image.GetInstance(imageFilePathLogo); //infoReport[1];

            //        cell = PdfBase.drawImageCell(headerJpg, 80F, 70F, 19, 3, 2, 2, 2, 2, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            //        headerTbl.AddCell(cell);
            //    }
            //    else
            //    {
            //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 3, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            //        headerTbl.AddCell(cell);
            //    }
            //}
            //else
            //{
            //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 3, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            //    headerTbl.AddCell(cell);
            //}


            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "تاريخ : " + PrintDate + "  " + HourMinute, 19, 1, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            //headerTbl.AddCell(cell);


            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], "تاريخ السند هجري  : " + dt.Rows[0]["InvoiceHijriDate"].ToString() + " ", 19, 1, 1, 1, 1, 1, 0, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            //cell.Padding = 10;
            //headerTbl.AddCell(cell);


            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "رقم السند", 19, 1, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            //cell.Padding = 5;
            //headerTbl.AddCell(cell);
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 1, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
            //cell.Padding = 5;
            //headerTbl.AddCell(cell);
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.BarCodeFont_20_NORMAL.ToString()], dt.Rows[0]["InvoiceNumber"].ToString(), 19, 1, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            //cell.Padding = 5;
            //headerTbl.AddCell(cell);
            //#endregion
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_UNDERLINE.ToString()], infoReport[0], 57, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            //cell.Padding = 5;
            //headerTbl.AddCell(cell);
            //#region secondRow
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.BarCodeFont_30_NORMAL.ToString()], "سند يومية", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            //cell.Padding = 5;
            //headerTbl.AddCell(cell);

            //#endregion

            //cell = new PdfPCell(headerTbl);
            //cell.Colspan = 57;
            //cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //cell.PaddingBottom = 5;
            //tblParent.AddCell(cell);


            //#endregion



            //#region HeaderReport
            PdfPTable headerTbl = new PdfPTable(57);
            headerTbl.DefaultCell.Padding = 5;
            headerTbl.WidthPercentage = 95;
            headerTbl.DefaultCell.BorderWidthBottom = 1;
            headerTbl.DefaultCell.BorderColor = BaseColor.BLUE;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            #region FirstRow
            headerTitle = "";

            if (infoReport[1] != "")
            {
                string imageFilePathLogo = PdfBase.getReportImagePath(infoReport[1]);

                if (imageFilePathLogo != "")
                {
                    Image headerJpg = Image.GetInstance(imageFilePathLogo); //infoReport[1];

                    cell = PdfBase.drawImageCell(headerJpg, 170F, 90F, 57, 0, 0, 0, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                    cell.Padding = 5;
                    headerTbl.AddCell(cell);
                }
                else
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 57, 0, 0, 0, 0, 0, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    cell.Padding = 5;
                    headerTbl.AddCell(cell);
                }
            }
            else
            {
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 57, 0, 0, 0, 0, 0, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                cell.Padding = 5;
                headerTbl.AddCell(cell);
            }
            #endregion



            cell = new PdfPCell(headerTbl);
            cell.Colspan = 57;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            cell.Border = Rectangle.NO_BORDER;
            tblParent.AddCell(cell);







            PdfPTable headerTb3 = new PdfPTable(30);
            headerTb3.DefaultCell.Padding = 5;
            headerTb3.WidthPercentage = 95;
            headerTb3.DefaultCell.BorderWidth = 0;
            headerTb3.HorizontalAlignment = Element.ALIGN_RIGHT;





            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " ", 10, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            cell.Padding = 2;

            headerTb3.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], PrintDate, 6, 0, 1, 1, 1, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            headerTb3.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], " التاريخ", 4, 0, 0, 1, 1, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;

            cell.BorderColor = BaseColor.BLUE;
            headerTb3.AddCell(cell);
            //end of line one
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " ", 10, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            cell.Padding = 2;
            headerTb3.AddCell(cell);





            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " ", 10, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            cell.Padding = 2;

            headerTb3.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "سند يومية", 6, 0, 1, 1, 0, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            headerTb3.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], " نوع العملية", 4, 0, 0, 1, 0, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;

            cell.BorderColor = BaseColor.BLUE;
            headerTb3.AddCell(cell);
            //end of line one

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 10, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            cell.Padding = 2;
            headerTb3.AddCell(cell);




            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " ", 10, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            cell.Padding = 2;

            headerTb3.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[0]["InvoiceNumber"] != null ? dt.Rows[0]["InvoiceNumber"].ToString() : ""), 6, 0, 1, 1, 0, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            headerTb3.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], " رقم السند", 4, 0, 0, 1, 0, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;

            cell.BorderColor = BaseColor.BLUE;
            headerTb3.AddCell(cell);
            //end of line one

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_NORMAL.ToString()], infoReport[0], 10, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            cell.Padding = 2;
            headerTb3.AddCell(cell);





            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " ", 10, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            //cell.Padding = 2;

            //headerTb3.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[0]["JoNo"] != null ? dt.Rows[0]["JoNo"].ToString() : ""), 6, 0, 1, 1, 0, 1, 15, Element.ALIGN_CENTER);
            //cell.Padding = 2;
            //cell.BorderColor = BaseColor.BLUE;
            //headerTb3.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], " رقم القيد", 4, 0, 0, 1, 0, 1, 15, Element.ALIGN_CENTER);
            //cell.Padding = 2;

            //cell.BorderColor = BaseColor.BLUE;
            //headerTb3.AddCell(cell);
            ////end of line one
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " ", 10, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            //cell.Padding = 2;
            //headerTb3.AddCell(cell);








            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " ", 10, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.Rowspan = 2;
            headerTb3.AddCell(cell);


            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "دائن", 3, 0, 1, 1, 0, 1, 15, Element.ALIGN_CENTER);
            //cell.Padding = 2;
            //cell.BorderColor = BaseColor.BLUE;
            //cell.BackgroundColor = BaseColor.PINK;
            //headerTb3.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[0]["InvoiceDate"] != null ? dt.Rows[0]["InvoiceDate"].ToString() : ""), 6, 0, 1, 1, 0, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            headerTb3.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], " تاريخ السند", 4, 0, 0, 1, 0, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.Rowspan = 2;
            cell.BorderColor = BaseColor.BLUE;
            headerTb3.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], " ", 10, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.Rowspan = 2;
            headerTb3.AddCell(cell);



            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " ", 10, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            //cell.Padding = 2;
            //cell.Rowspan = 2;
            //headerTb3.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[0]["InvoiceHijriDate"] != null ? dt.Rows[0]["InvoiceHijriDate"].ToString() : ""), 6, 0, 1, 1, 0, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;

            headerTb3.AddCell(cell);








            cell = new PdfPCell(headerTb3);
            cell.Colspan = 30;
            cell.BorderWidth = 0;

            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Padding = 5;
            //tblParent.AddCell(cell);

            tblParent.AddCell(cell);


            PdfPTable header2Table = new PdfPTable(57);
            header2Table.WidthPercentage = 95;
            header2Table.DefaultCell.BorderWidth = 0;
            header2Table.HorizontalAlignment = Element.ALIGN_CENTER;
            header2Table.DefaultCell.Padding = 3;


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (""), 20, 0, 0, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            header2Table.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (dt.Rows[0]["CostCenterName"] != null ? dt.Rows[0]["CostCenterName"].ToString() : ""), 10, 0, 0, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            header2Table.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], "مركز التكلفة ", 27, 0, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            header2Table.AddCell(cell);
            
            cell = new PdfPCell(header2Table);
            cell.Colspan = (57);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 3;
            tblParent.AddCell(cell);

            string fontspath = Path.Combine("fonts/ArialUni.ttf");


            BaseFont bf = BaseFont.CreateFont(fontspath, BaseFont.IDENTITY_H, true);

            iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 20, iTextSharp.text.Font.BOLD, BaseColor.BLUE);

            cell = PdfBase.drawCell(font, "قيد يومية", 57, 0, 0, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);

            cell.Padding = 10;
            tblParent.AddCell(cell);



            #region body
            PdfPTable bodyTable = new PdfPTable(57);
            bodyTable.WidthPercentage = 95;
            bodyTable.DefaultCell.BorderWidth = 0;
            bodyTable.DefaultCell.BorderColor = BaseColor.BLUE;
            bodyTable.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyTable.DefaultCell.Padding = 5;
            bodyTable.HeaderRows = 1;

            //headerTitle = "مركز التكلفة";
            //DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 9, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            //headerTitle = "البيان";
            //DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 9, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            //headerTitle = "دائن";
            //DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 9, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            //headerTitle = "مدين";
            //DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 9, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            //headerTitle = "اسم الحساب";
            //DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 12, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            //headerTitle = "كود الحساب";
            //DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 9, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "مركز التكلفة", 9, 0, 1, 1, 1, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "البيان", 9, 0, 0, 1, 1, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "دائن", 9, 0, 0, 1, 1, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "مدين", 9, 0, 0, 1, 1, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "اسم الحساب", 12, 0, 0, 0, 1, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "كود الحساب", 9, 0, 1, 1, 1, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            bodyTable.AddCell(cell);

            if (dt != null && dt.Rows.Count > 0)
            {
                decimal total_Credit = 0;
                decimal total_Depit = 0;
                for (int x = 0; x < dt.Rows.Count; x++)
                {
                    // result = result + (float.Parse((dt[x].Credit != null ? dt[x].Credit : 0).ToString()) - float.Parse((dt[x].Depit != null ? dt[x].Depit : 0).ToString()));
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[x]["CostCenterNameAr"] != null ? dt.Rows[x]["CostCenterNameAr"].ToString() : ""), 9, 0, 1, 1, 0, 1, 15, Element.ALIGN_CENTER);
                    cell.Padding = 2;
                    cell.BorderColor = BaseColor.BLUE;
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], dt.Rows[x]["Notes"].ToString(), 9, 0, 0, 1, 0, 1, 15, Element.ALIGN_CENTER);
                    cell.Padding = 2;
                    cell.BorderColor = BaseColor.BLUE;
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[x]["Credit"] != null ? dt.Rows[x]["Credit"].ToString() : ""), 9, 0, 0, 1, 0, 1, 15, Element.ALIGN_CENTER);
                    cell.Padding = 2;
                    cell.BorderColor = BaseColor.BLUE;
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[x]["Depit"] != null ? dt.Rows[x]["Depit"].ToString() : "0.00"), 9, 0, 0, 1, 0, 1, 15, Element.ALIGN_CENTER);
                    cell.Padding = 2;
                    cell.BorderColor = BaseColor.BLUE;
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[x]["AccountNameAr"] != null ? dt.Rows[x]["AccountNameAr"].ToString() : "0.00"), 12, 0, 0, 1, 0, 1, 15, Element.ALIGN_CENTER);
                    cell.Padding = 2;
                    cell.BorderColor = BaseColor.BLUE;
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[x]["AccountCode"] != null ? dt.Rows[x]["AccountCode"].ToString() : ""), 9, 0, 0, 1, 0, 1, 15, Element.ALIGN_CENTER);
                    cell.Padding = 2;
                    cell.BorderColor = BaseColor.BLUE;
                    bodyTable.AddCell(cell);

                    total_Credit = total_Credit + (dt.Rows[x]["Credit"] != null ? (decimal)dt.Rows[x]["Credit"] : 0);
                    total_Depit = total_Depit + (dt.Rows[x]["Depit"] != null ? (decimal)dt.Rows[x]["Depit"] : 0);

                }
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (""), 9, 0, 1, 1, 0, 1, 15, Element.ALIGN_CENTER);
                cell.Padding = 2;
                cell.BorderColor = BaseColor.BLUE;
                bodyTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (""), 9, 0, 0, 1, 0, 1, 15, Element.ALIGN_CENTER);
                cell.Padding = 2;
                cell.BorderColor = BaseColor.BLUE;
                bodyTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (total_Credit.ToString()), 9, 0, 0, 1, 0, 1, 15, Element.ALIGN_CENTER);
                cell.Padding = 2;
                cell.BorderColor = BaseColor.BLUE;
                bodyTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (total_Depit.ToString()), 9, 0, 0, 1, 0, 1, 15, Element.ALIGN_CENTER);
                cell.Padding = 2;
                cell.BorderColor = BaseColor.BLUE;
                bodyTable.AddCell(cell);

                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (""), 12, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                //bodyTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (Resources.Totalcreditordebtor), 21, 0, 0, 1, 0, 1, 15, Element.ALIGN_CENTER);
                cell.Padding = 2;
                cell.BorderColor = BaseColor.BLUE;
                bodyTable.AddCell(cell);

            }
            else
            {
                var titlenodata = "لا توجد نتائج ";
                var callNodata = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_BOLD_Red.ToString()], titlenodata, 57, 0, 1, 1, 0, 1, 15, Element.ALIGN_CENTER);
                callNodata.Padding = 2;
                callNodata.BorderColor = BaseColor.BLUE;
                bodyTable.AddCell(callNodata);
            }


            cell = new PdfPCell(bodyTable);
            cell.Colspan = (57);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            tblParent.AddCell(cell);

            #endregion
            PdfPTable footerTable = new PdfPTable(57);
            footerTable.WidthPercentage = 95;
            footerTable.DefaultCell.BorderWidth = 0;
            footerTable.HorizontalAlignment = Element.ALIGN_CENTER;
            footerTable.DefaultCell.Padding = 5;

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "......................", 12, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER);
            footerTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "المدير المالى    ", 12, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER);
            footerTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 9, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER);
            footerTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], ".......................", 12, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER);
            footerTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "المحاسب     ", 12, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER);
            footerTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "", 57, 0, 0, 0, 0, 0, 25, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            footerTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_UNDERLINE.ToString()], dt.Rows[0]["FullName"] != null ? dt.Rows[0]["FullName"].ToString() : "", 57, 0, 0, 0, 0, 0, 20, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            footerTable.AddCell(cell);

            cell = new PdfPCell(footerTable);
            cell.Colspan = (57);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            tblParent.AddCell(cell);

            doc.Add(tblParent);
            doc.Close();
            return memoryStream.GetBuffer();
        }
        public static byte[] PrintDailyVoucherReport_Custody(DataTable dt, string[] infoReport)
        {

            bool isFooter = (infoReport[6] != null && infoReport[6] != "" || infoReport[6] != "0") ? true : false;//send from user
            string footerData = "" + infoReport[0] + "  " + infoReport[2] + "  - تليفون  : " + infoReport[5] + " - فاكس  : " + infoReport[4] + " -  بريد الكترونى  : " + infoReport[3];
            Document doc = new Document(PageSize.A4, 10, 10, 30, 44);
            var memoryStream = PdfBase.OpenDocument(doc, "", false, "", isFooter, footerData);

            var headerTitle = "";

            PdfPCell cell = null;
            PdfPTable tblParent = new PdfPTable(1);
            tblParent.DefaultCell.Padding = 0;
            tblParent.WidthPercentage = 95;
            tblParent.DefaultCell.BorderWidth = 0;
            tblParent.HorizontalAlignment = Element.ALIGN_CENTER;
            tblParent.SplitLate = false;
            tblParent.HeaderRows = 1;



            //#region HeaderReport
            PdfPTable headerTbl = new PdfPTable(57);
            headerTbl.DefaultCell.Padding = 5;
            headerTbl.WidthPercentage = 95;
            headerTbl.DefaultCell.BorderWidthBottom = 1;
            headerTbl.DefaultCell.BorderColor = BaseColor.BLUE;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            #region FirstRow
            headerTitle = "";

            if (infoReport[1] != "")
            {
                string imageFilePathLogo = PdfBase.getReportImagePath(infoReport[1]);

                if (imageFilePathLogo != "")
                {
                    Image headerJpg = Image.GetInstance(imageFilePathLogo); //infoReport[1];

                    cell = PdfBase.drawImageCell(headerJpg, 170F, 90F, 57, 0, 0, 0, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                    cell.Padding = 5;
                    headerTbl.AddCell(cell);
                }
                else
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 57, 0, 0, 0, 0, 0, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    cell.Padding = 5;
                    headerTbl.AddCell(cell);
                }
            }
            else
            {
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 57, 0, 0, 0, 0, 0, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                cell.Padding = 5;
                headerTbl.AddCell(cell);
            }
            #endregion



            cell = new PdfPCell(headerTbl);
            cell.Colspan = 57;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            tblParent.AddCell(cell);







            PdfPTable headerTb3 = new PdfPTable(30);
            headerTb3.DefaultCell.Padding = 5;
            headerTb3.WidthPercentage = 95;
            headerTb3.DefaultCell.BorderWidth = 0;
            headerTb3.HorizontalAlignment = Element.ALIGN_RIGHT;





            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " ", 10, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            cell.Padding = 2;

            headerTb3.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], PrintDate, 6, 0, 1, 1, 1, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            headerTb3.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], " التاريخ", 4, 0, 0, 1, 1, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;

            cell.BorderColor = BaseColor.BLUE;
            headerTb3.AddCell(cell);
            //end of line one
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " ", 10, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            cell.Padding = 2;
            headerTb3.AddCell(cell);





            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " ", 10, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            cell.Padding = 2;

            headerTb3.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "سند يومية", 6, 0, 1, 1, 0, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            headerTb3.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], " نوع العملية", 4, 0, 0, 1, 0, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;

            cell.BorderColor = BaseColor.BLUE;
            headerTb3.AddCell(cell);
            //end of line one

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 10, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            cell.Padding = 2;
            headerTb3.AddCell(cell);




            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " ", 10, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            cell.Padding = 2;

            headerTb3.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[0]["InvoiceNumber"] != null ? dt.Rows[0]["InvoiceNumber"].ToString() : ""), 6, 0, 1, 1, 0, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            headerTb3.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], " رقم السند", 4, 0, 0, 1, 0, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;

            cell.BorderColor = BaseColor.BLUE;
            headerTb3.AddCell(cell);
            //end of line one

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_NORMAL.ToString()], infoReport[0], 10, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            cell.Padding = 2;
            headerTb3.AddCell(cell);



            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " ", 10, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.Rowspan = 2;
            headerTb3.AddCell(cell);



            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[0]["InvoiceDate"] != null ? dt.Rows[0]["InvoiceDate"].ToString() : ""), 6, 0, 1, 1, 0, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            headerTb3.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], " تاريخ السند", 4, 0, 0, 1, 0, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.Rowspan = 2;
            cell.BorderColor = BaseColor.BLUE;
            headerTb3.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], " ", 10, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.Rowspan = 2;
            headerTb3.AddCell(cell);



            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[0]["InvoiceHijriDate"] != null ? dt.Rows[0]["InvoiceHijriDate"].ToString() : ""), 6, 0, 1, 1, 0, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;

            headerTb3.AddCell(cell);








            cell = new PdfPCell(headerTb3);
            cell.Colspan = 30;
            cell.BorderWidth = 0;

            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Padding = 5;
            //tblParent.AddCell(cell);

            tblParent.AddCell(cell);


            PdfPTable header2Table = new PdfPTable(57);
            header2Table.WidthPercentage = 95;
            header2Table.DefaultCell.BorderWidth = 0;
            header2Table.HorizontalAlignment = Element.ALIGN_CENTER;
            header2Table.DefaultCell.Padding = 3;


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (""), 20, 0, 0, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            header2Table.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (dt.Rows[0]["CostCenterNameAr"] != null ? dt.Rows[0]["CostCenterNameAr"].ToString() : ""), 10, 0, 0, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            header2Table.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], "مركز التكلفة ", 27, 0, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);
            header2Table.AddCell(cell);

            cell = new PdfPCell(header2Table);
            cell.Colspan = (57);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 3;
            tblParent.AddCell(cell);

            string fontspath = Path.Combine("fonts/ArialUni.ttf");


            BaseFont bf = BaseFont.CreateFont(fontspath, BaseFont.IDENTITY_H, true);

            iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 20, iTextSharp.text.Font.BOLD, BaseColor.BLUE);

            cell = PdfBase.drawCell(font, "عهدة مالية", 57, 0, 0, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);

            cell.Padding = 10;
            tblParent.AddCell(cell);



            #region body
            PdfPTable bodyTable = new PdfPTable(57);
            bodyTable.WidthPercentage = 95;
            bodyTable.DefaultCell.BorderWidth = 0;
            bodyTable.DefaultCell.BorderColor = BaseColor.BLUE;
            bodyTable.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyTable.DefaultCell.Padding = 5;
            bodyTable.HeaderRows = 1;

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "مركز التكلفة", 9, 0, 1, 1, 1, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "البيان", 9, 0, 0, 1, 1, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "دائن", 9, 0, 0, 1, 1, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "مدين", 9, 0, 0, 1, 1, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "اسم الحساب", 12, 0, 0, 0, 1, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "كود الحساب", 9, 0, 1, 1, 1, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            bodyTable.AddCell(cell);

            if (dt != null && dt.Rows.Count > 0)
            {
                decimal total_Credit = 0;
                decimal total_Depit = 0;
                for (int x = 0; x < dt.Rows.Count; x++)
                {
                    // result = result + (float.Parse((dt[x].Credit != null ? dt[x].Credit : 0).ToString()) - float.Parse((dt[x].Depit != null ? dt[x].Depit : 0).ToString()));
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[x]["CostCenterNameAr"] != null ? dt.Rows[x]["CostCenterNameAr"].ToString() : ""), 9, 0, 1, 1, 0, 1, 15, Element.ALIGN_CENTER);
                    cell.Padding = 2;
                    cell.BorderColor = BaseColor.BLUE;
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], dt.Rows[x]["Notes"].ToString(), 9, 0, 0, 1, 0, 1, 15, Element.ALIGN_CENTER);
                    cell.Padding = 2;
                    cell.BorderColor = BaseColor.BLUE;
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[x]["Credit"] != null ? dt.Rows[x]["Credit"].ToString() : ""), 9, 0, 0, 1, 0, 1, 15, Element.ALIGN_CENTER);
                    cell.Padding = 2;
                    cell.BorderColor = BaseColor.BLUE;
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[x]["Depit"] != null ? dt.Rows[x]["Depit"].ToString() : "0.00"), 9, 0, 0, 1, 0, 1, 15, Element.ALIGN_CENTER);
                    cell.Padding = 2;
                    cell.BorderColor = BaseColor.BLUE;
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[x]["AccountNameAr"] != null ? dt.Rows[x]["AccountNameAr"].ToString() : "0.00"), 12, 0, 0, 1, 0, 1, 15, Element.ALIGN_CENTER);
                    cell.Padding = 2;
                    cell.BorderColor = BaseColor.BLUE;
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[x]["AccountCode"] != null ? dt.Rows[x]["AccountCode"].ToString() : ""), 9, 0, 0, 1, 0, 1, 15, Element.ALIGN_CENTER);
                    cell.Padding = 2;
                    cell.BorderColor = BaseColor.BLUE;
                    bodyTable.AddCell(cell);

                    total_Credit = total_Credit + (dt.Rows[x]["Credit"] != null ? (decimal)dt.Rows[x]["Credit"] : 0);
                    total_Depit = total_Depit + (dt.Rows[x]["Depit"] != null ? (decimal)dt.Rows[x]["Depit"] : 0);

                }
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (""), 9, 0, 1, 1, 0, 1, 15, Element.ALIGN_CENTER);
                cell.Padding = 2;
                cell.BorderColor = BaseColor.BLUE;
                bodyTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (""), 9, 0, 0, 1, 0, 1, 15, Element.ALIGN_CENTER);
                cell.Padding = 2;
                cell.BorderColor = BaseColor.BLUE;
                bodyTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (total_Credit.ToString()), 9, 0, 0, 1, 0, 1, 15, Element.ALIGN_CENTER);
                cell.Padding = 2;
                cell.BorderColor = BaseColor.BLUE;
                bodyTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (total_Depit.ToString()), 9, 0, 0, 1, 0, 1, 15, Element.ALIGN_CENTER);
                cell.Padding = 2;
                cell.BorderColor = BaseColor.BLUE;
                bodyTable.AddCell(cell);

                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (""), 12, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                //bodyTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (Resources.Totalcreditordebtor), 21, 0, 0, 1, 0, 1, 15, Element.ALIGN_CENTER);
                cell.Padding = 2;
                cell.BorderColor = BaseColor.BLUE;
                bodyTable.AddCell(cell);

            }
            else
            {
                var titlenodata = "لا توجد نتائج ";
                var callNodata = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_BOLD_Red.ToString()], titlenodata, 57, 0, 1, 1, 0, 1, 15, Element.ALIGN_CENTER);
                callNodata.Padding = 2;
                callNodata.BorderColor = BaseColor.BLUE;
                bodyTable.AddCell(callNodata);
            }


            cell = new PdfPCell(bodyTable);
            cell.Colspan = (57);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            tblParent.AddCell(cell);

            #endregion
            PdfPTable footerTable = new PdfPTable(57);
            footerTable.WidthPercentage = 95;
            footerTable.DefaultCell.BorderWidth = 0;
            footerTable.HorizontalAlignment = Element.ALIGN_CENTER;
            footerTable.DefaultCell.Padding = 5;

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "......................", 12, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER);
            footerTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "المدير المالى    ", 12, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER);
            footerTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 9, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER);
            footerTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], ".......................", 12, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER);
            footerTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "المحاسب     ", 12, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER);
            footerTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "", 57, 0, 0, 0, 0, 0, 25, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            footerTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_UNDERLINE.ToString()], dt.Rows[0]["AccountNameAr"] != null ? dt.Rows[0]["AccountNameAr"].ToString() : "", 57, 0, 0, 0, 0, 0, 20, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            footerTable.AddCell(cell);

            cell = new PdfPCell(footerTable);
            cell.Colspan = (57);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            tblParent.AddCell(cell);

            doc.Add(tblParent);
            doc.Close();
            return memoryStream.GetBuffer();
        }


        public static byte[] PrintClosingVoucherReport(DataTable dt, string[] infoReport)
        {

            bool isFooter = (infoReport[6] != null && infoReport[6] != "" || infoReport[6] != "0") ? true : false;//send from user
            string footerData = "" + infoReport[0] + "  " + infoReport[2] + "  - تليفون  : " + infoReport[5] + " - فاكس  : " + infoReport[4] + " -  بريد الكترونى  : " + infoReport[3];
            Document doc = new Document(PageSize.A4, 10, 10, 30, 44);
            var memoryStream = PdfBase.OpenDocument(doc, "1", false, "", isFooter, footerData);

            var headerTitle = "";

            PdfPCell cell = null;
            PdfPTable tblParent = new PdfPTable(1);
            tblParent.DefaultCell.Padding = 0;
            tblParent.WidthPercentage = 100;
            tblParent.DefaultCell.BorderWidth = 0;
            tblParent.HorizontalAlignment = Element.ALIGN_CENTER;
            tblParent.SplitLate = false;
            tblParent.HeaderRows = 1;

            //#region HeaderReport
            //PdfPTable headerTbl = new PdfPTable(57);
            //headerTbl.DefaultCell.Padding = 5;
            //headerTbl.WidthPercentage = 95;
            //headerTbl.DefaultCell.BorderWidthBottom = 1;
            //headerTbl.DefaultCell.BorderColor = BaseColor.BLUE;
            //headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            //#region FirstRow
            //headerTitle = "";
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], "تاريخ السند ميلادى : " + dt.Rows[0]["InvoiceDate"].ToString() + "", 19, 1, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            //cell.Padding = 5;
            //headerTbl.AddCell(cell);

            //if (infoReport[1] != "")
            //{
            //    string imageFilePathLogo = PdfBase.getReportImagePath(infoReport[1]);

            //    if (imageFilePathLogo != "")
            //    {
            //        Image headerJpg = Image.GetInstance(imageFilePathLogo); //infoReport[1];

            //        cell = PdfBase.drawImageCell(headerJpg, 90F, 80F, 19, 3, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            //        headerTbl.AddCell(cell);
            //    }
            //    else
            //    {
            //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 3, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            //        headerTbl.AddCell(cell);
            //    }
            //}
            //else
            //{
            //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 3, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            //    headerTbl.AddCell(cell);
            //}


            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "تاريخ : " + PrintDate + "  " + HourMinute, 19, 1, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            //cell.Padding = 10;
            //headerTbl.AddCell(cell);


            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], "تاريخ السند هجري  : " + dt.Rows[0]["InvoiceHijriDate"].ToString() + " ", 19, 1, 1, 1, 1, 1, 0, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            //cell.Padding = 10;
            //headerTbl.AddCell(cell);


            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "رقم السند", 19, 1, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            //cell.Padding = 5;
            //headerTbl.AddCell(cell);
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 1, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
            //cell.Padding = 5;
            //headerTbl.AddCell(cell);
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.BarCodeFont_20_NORMAL.ToString()], dt.Rows[0]["InvoiceNumber"].ToString(), 19, 1, 1, 1, 1, 1, 10, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            //cell.Padding = 5;
            //headerTbl.AddCell(cell);
            //#endregion
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_UNDERLINE.ToString()], infoReport[0], 57, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            //cell.Padding = 5;
            //headerTbl.AddCell(cell);
            //#region secondRow
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.BarCodeFont_30_NORMAL.ToString()], "سند اقفال", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            //cell.Padding = 5;
            //headerTbl.AddCell(cell);

            //#endregion

            //cell = new PdfPCell(headerTbl);
            //cell.Colspan = 57;
            //cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //cell.PaddingBottom = 5;
            //tblParent.AddCell(cell);


            //#endregion



            #region HeaderReport
            PdfPTable headerTbl = new PdfPTable(57);
            headerTbl.DefaultCell.Padding = 5;
            headerTbl.WidthPercentage = 95;
            headerTbl.DefaultCell.BorderWidthBottom = 0;
            headerTbl.DefaultCell.BorderColor = BaseColor.BLUE;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            #region FirstRow
            headerTitle = "";

            if (infoReport[1] != "")
            {
                string imageFilePathLogo = PdfBase.getReportImagePath(infoReport[1]);

                if (imageFilePathLogo != "")
                {
                    Image headerJpg = Image.GetInstance(imageFilePathLogo); //infoReport[1];

                    cell = PdfBase.drawImageCell(headerJpg, 170F, 90F, 57, 0, 0, 0, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                    cell.Padding = 5;
                    headerTbl.AddCell(cell);
                }
                else
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 57, 0, 0, 0, 0, 0, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    cell.Padding = 5;
                    headerTbl.AddCell(cell);
                }
            }
            else
            {
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 57, 0, 0, 0, 0, 0, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                cell.Padding = 5;
                headerTbl.AddCell(cell);
            }


            #endregion



            cell = new PdfPCell(headerTbl);
            cell.Colspan = 57;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            cell.Border = Rectangle.NO_BORDER;
            tblParent.AddCell(cell);

            #endregion



            PdfPTable headerTb3 = new PdfPTable(30);
            headerTb3.DefaultCell.Padding = 5;
            headerTb3.WidthPercentage = 95;
            headerTb3.DefaultCell.BorderWidth = 0;
            headerTb3.HorizontalAlignment = Element.ALIGN_RIGHT;







            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " ", 10, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            cell.Padding = 2;

            headerTb3.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], PrintDate, 6, 0, 1, 1, 1, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            headerTb3.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], " التاريخ", 4, 0, 0, 1, 1, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;

            cell.BorderColor = BaseColor.BLUE;
            headerTb3.AddCell(cell);
            //end of line one
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " ", 10, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            cell.Padding = 2;
            headerTb3.AddCell(cell);



            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " ", 10, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            cell.Padding = 2;
            headerTb3.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], dt.Rows[0]["InvoiceNumber"].ToString(), 6, 0, 1, 1, 0, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            headerTb3.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "رقم السند", 4, 0, 0, 1, 0, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            headerTb3.AddCell(cell);
            //end of line one
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " ", 10, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            cell.Padding = 2;
            headerTb3.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " ", 10, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            cell.Padding = 2;
            headerTb3.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], dt.Rows[0]["InvoiceDate"].ToString(), 6, 0, 1, 1, 0, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            headerTb3.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "تاريخ السند ميلادى ", 4, 0, 0, 1, 0, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            headerTb3.AddCell(cell);
            //end of line one

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_NORMAL.ToString()], infoReport[0], 10, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            headerTb3.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " ", 10, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            cell.Padding = 2;
            headerTb3.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], dt.Rows[0]["InvoiceHijriDate"].ToString(), 6, 0, 1, 1, 0, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            headerTb3.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "تاريخ السند هجري", 4, 0, 0, 1, 0, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            headerTb3.AddCell(cell);
            //end of line one
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " ", 10, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            cell.Padding = 2;
            headerTb3.AddCell(cell);



            cell = new PdfPCell(headerTb3);
            cell.Colspan = 30;
            cell.BorderWidth = 0;

            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Padding = 5;
            //tblParent.AddCell(cell);

            tblParent.AddCell(cell);



            string fontspath = Path.Combine("fonts/ArialUni.ttf");


            BaseFont bf = BaseFont.CreateFont(fontspath, BaseFont.IDENTITY_H, true);

            iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 20, iTextSharp.text.Font.BOLD, BaseColor.BLUE);

            cell = PdfBase.drawCell(font, "سند  اقفال", 57, 0, 0, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);

            cell.Padding = 10;
            tblParent.AddCell(cell);





            #region body
            PdfPTable bodyTable = new PdfPTable(57);
            bodyTable.WidthPercentage = 95;
            bodyTable.DefaultCell.BorderWidth = 0;
            bodyTable.DefaultCell.BorderColor = BaseColor.BLUE;
            bodyTable.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyTable.DefaultCell.Padding = 5;
            bodyTable.HeaderRows = 1;

            //headerTitle = "مركز التكلفة";
            //DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 9, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            //headerTitle = "البيان";
            //DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 9, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            //headerTitle = "دائن";
            //DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 9, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            //headerTitle = "مدين";
            //DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 9, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            //headerTitle = "اسم الحساب";
            //DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 12, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            //headerTitle = "كود الحساب";
            //DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 9, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);



            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "مركز التكلفة", 9, 0, 1, 1, 1, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "البيان", 9, 0, 0, 1, 1, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "دائن", 9, 0, 0, 1, 1, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "مدين", 9, 0, 0, 1, 1, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "اسم الحساب", 12, 0, 0, 0, 1, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "كود الحساب", 9, 0, 1, 1, 1, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            bodyTable.AddCell(cell);

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int x = 0; x < dt.Rows.Count; x++)
                {
                    // result = result + (float.Parse((dt[x].Credit != null ? dt[x].Credit : 0).ToString()) - float.Parse((dt[x].Depit != null ? dt[x].Depit : 0).ToString()));
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[x]["CostCenterNameAr"] != null ? dt.Rows[x]["CostCenterNameAr"].ToString() : ""), 9, 0, 1, 1, 0, 1, 5, Element.ALIGN_CENTER);
                    cell.Padding = 2;
                    cell.BorderColor = BaseColor.BLUE;
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], dt.Rows[x]["Notes"].ToString(), 9, 0, 0, 1, 0, 1, 10, Element.ALIGN_CENTER);
                    cell.Padding = 2;
                    cell.BorderColor = BaseColor.BLUE;
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[x]["Credit"] != null ? dt.Rows[x]["Credit"].ToString() : ""), 9, 0, 0, 1, 0, 1, 5, Element.ALIGN_CENTER);
                    cell.Padding = 2;
                    cell.BorderColor = BaseColor.BLUE;
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[x]["Depit"] != null ? dt.Rows[x]["Depit"].ToString() : "0.00"), 9, 0, 0, 1, 0, 1, 5, Element.ALIGN_CENTER);
                    cell.Padding = 2;
                    cell.BorderColor = BaseColor.BLUE;
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[x]["AccountNameAr"] != null ? dt.Rows[x]["AccountNameAr"].ToString() : "0.00"), 12, 0, 0, 1, 0, 1, 5, Element.ALIGN_CENTER);
                    cell.Padding = 2;
                    cell.BorderColor = BaseColor.BLUE;
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[x]["AccountCode"] != null ? dt.Rows[x]["AccountCode"].ToString() : ""), 9, 0, 0, 1, 0, 1, 5, Element.ALIGN_CENTER);
                    cell.Padding = 2;
                    cell.BorderColor = BaseColor.BLUE;
                    bodyTable.AddCell(cell);

                }

            }
            else
            {
                var titlenodata = "لا توجد نتائج ";
                var callNodata = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_BOLD_Red.ToString()], titlenodata, 57, 0, 1, 1, 0, 1, 5, Element.ALIGN_CENTER);
                callNodata.Padding = 2;
                callNodata.BorderColor = BaseColor.BLUE;
                bodyTable.AddCell(callNodata);
            }


            cell = new PdfPCell(bodyTable);
            cell.Colspan = (57);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            tblParent.AddCell(cell);

            #endregion
            PdfPTable footerTable = new PdfPTable(57);
            footerTable.WidthPercentage = 95;
            footerTable.DefaultCell.BorderWidth = 0;
            footerTable.HorizontalAlignment = Element.ALIGN_CENTER;
            footerTable.DefaultCell.Padding = 5;

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "......................", 12, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER);
            footerTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "المدير المالى    ", 12, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER);
            footerTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 9, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER);
            footerTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], ".......................", 12, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER);
            footerTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "المحاسب     ", 12, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER);
            footerTable.AddCell(cell);

            cell = new PdfPCell(footerTable);
            cell.Colspan = (57);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            tblParent.AddCell(cell);

            doc.Add(tblParent);
            doc.Close();
            return memoryStream.GetBuffer();
        }


        //التقرير الافتتاحى
        public static byte[] PrintOpeningVoucherReport(DataTable dt, string[] infoReport)
        {

            bool isFooter = (infoReport[6] != null && infoReport[6] != "" || infoReport[6] != "0") ? true : false;//send from user
            string footerData = "" + infoReport[0] + "  " + infoReport[2] + "  - تليفون  : " + infoReport[5] + " - فاكس  : " + infoReport[4] + " -  بريد الكترونى  : " + infoReport[3];
            Document doc = new Document(PageSize.A4, 10, 10, 30, 44);
            var memoryStream = PdfBase.OpenDocument(doc, "1", false, "", isFooter, footerData);

            var headerTitle = "";

            PdfPCell cell = null;
            PdfPTable tblParent = new PdfPTable(1);
            tblParent.DefaultCell.Padding = 0;
            tblParent.WidthPercentage = 100;
            tblParent.DefaultCell.BorderWidth = 0;
            tblParent.HorizontalAlignment = Element.ALIGN_CENTER;
            tblParent.SplitLate = false;
            tblParent.HeaderRows = 1;

    




            #region HeaderReport
            PdfPTable headerTbl = new PdfPTable(57);
            headerTbl.DefaultCell.Padding = 5;
            headerTbl.WidthPercentage = 95;
            headerTbl.DefaultCell.BorderWidthBottom = 0;
            headerTbl.DefaultCell.BorderColor = BaseColor.BLUE;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            #region FirstRow
            headerTitle = "";

            if (infoReport[1] != "")
            {
                string imageFilePathLogo = PdfBase.getReportImagePath(infoReport[1]);

                if (imageFilePathLogo != "")
                {
                    Image headerJpg = Image.GetInstance(imageFilePathLogo); //infoReport[1];

                    cell = PdfBase.drawImageCell(headerJpg, 170F, 90F, 57, 0, 0, 0, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
                    cell.Padding = 5;
                    headerTbl.AddCell(cell);
                }
                else
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 57, 0, 0, 0, 0, 0, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    cell.Padding = 5;
                    headerTbl.AddCell(cell);
                }
            }
            else
            {
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 57, 0, 0, 0, 0, 0, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                cell.Padding = 5;
                headerTbl.AddCell(cell);
            }


            #endregion



            cell = new PdfPCell(headerTbl);
            cell.Colspan = 57;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            cell.Border = Rectangle.NO_BORDER;
            tblParent.AddCell(cell);

            #endregion



            PdfPTable headerTb3 = new PdfPTable(30);
            headerTb3.DefaultCell.Padding = 5;
            headerTb3.WidthPercentage = 95;
            headerTb3.DefaultCell.BorderWidth = 0;
            headerTb3.HorizontalAlignment = Element.ALIGN_RIGHT;







            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " ", 10, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            cell.Padding = 2;

            headerTb3.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], PrintDate, 6, 0, 1, 1, 1, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            headerTb3.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], " التاريخ", 4, 0, 0, 1, 1, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;

            cell.BorderColor = BaseColor.BLUE;
            headerTb3.AddCell(cell);
            //end of line one
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " ", 10, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            cell.Padding = 2;
            headerTb3.AddCell(cell);



            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " ", 10, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            cell.Padding = 2;
            headerTb3.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], dt.Rows[0]["InvoiceNumber"].ToString(), 6, 0, 1, 1, 0, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            headerTb3.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "رقم السند", 4, 0, 0, 1, 0, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            headerTb3.AddCell(cell);
            //end of line one
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " ", 10, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            cell.Padding = 2;
            headerTb3.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " ", 10, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            cell.Padding = 2;
            headerTb3.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], dt.Rows[0]["InvoiceDate"].ToString(), 6, 0, 1, 1, 0, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            headerTb3.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "تاريخ السند ميلادى ", 4, 0, 0, 1, 0, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            headerTb3.AddCell(cell);
            //end of line one

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_NORMAL.ToString()], infoReport[0], 10, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            headerTb3.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " ", 10, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            cell.Padding = 2;
            headerTb3.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], dt.Rows[0]["InvoiceHijriDate"].ToString(), 6, 0, 1, 1, 0, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            headerTb3.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "تاريخ السند هجري", 4, 0, 0, 1, 0, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            headerTb3.AddCell(cell);
            //end of line one
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " ", 10, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            cell.Padding = 2;
            headerTb3.AddCell(cell);



            cell = new PdfPCell(headerTb3);
            cell.Colspan = 30;
            cell.BorderWidth = 0;

            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Padding = 5;
            //tblParent.AddCell(cell);

            tblParent.AddCell(cell);



            string fontspath = Path.Combine("fonts/ArialUni.ttf");


            BaseFont bf = BaseFont.CreateFont(fontspath, BaseFont.IDENTITY_H, true);

            iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 20, iTextSharp.text.Font.BOLD, BaseColor.BLUE);

            cell = PdfBase.drawCell(font, "سند قيد افتتاحي", 57, 0, 0, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);

            cell.Padding = 10;
            tblParent.AddCell(cell);





            #region body
            PdfPTable bodyTable = new PdfPTable(57);
            bodyTable.WidthPercentage = 95;
            bodyTable.DefaultCell.BorderWidth = 0;
            bodyTable.DefaultCell.BorderColor = BaseColor.BLUE;
            bodyTable.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyTable.DefaultCell.Padding = 5;
            bodyTable.HeaderRows = 1;


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "مركز التكلفة", 9, 0, 1, 1, 1, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], " البيان", 9, 0, 0, 1, 1, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "دائن ", 9, 0, 0, 1, 1, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "مدين", 9, 0, 0, 1, 1, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], " اسم الحساب", 12, 0, 0, 0, 1, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "كود الحساب", 9, 0, 1, 1, 1, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            bodyTable.AddCell(cell);

            decimal totalDepit = 0;
            decimal totalCredit = 0;
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int x = 0; x < dt.Rows.Count; x++)
                {
                    totalDepit = totalDepit + decimal.Parse(dt.Rows[x]["Depit"].ToString() == "" || dt.Rows[x]["Depit"] == null || !dt.Columns.Contains("Depit") ? "0" : dt.Rows[x]["Depit"].ToString());
                    totalCredit = totalCredit + decimal.Parse(!dt.Columns.Contains("Credit") || dt.Rows[x]["Credit"].ToString() == "" || dt.Rows[x]["Credit"] == null ? "0" : dt.Rows[x]["Credit"].ToString());

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[x]["CostCenterNameAr"] != null ? dt.Rows[x]["CostCenterNameAr"].ToString() : ""), 9, 0, 1, 1, 0, 1, 5, Element.ALIGN_CENTER);
                    cell.Padding = 2;
                    cell.BorderColor = BaseColor.BLUE;
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], dt.Rows[x]["Notes"].ToString(), 9, 0, 0, 1, 0, 1, 10, Element.ALIGN_CENTER);
                    cell.Padding = 2;
                    cell.BorderColor = BaseColor.BLUE;
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[x]["Credit"] != null ? dt.Rows[x]["Credit"].ToString() : ""), 9, 0, 0, 1, 0, 1, 5, Element.ALIGN_CENTER);
                    cell.Padding = 2;
                    cell.BorderColor = BaseColor.BLUE;
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[x]["Depit"] != null ? dt.Rows[x]["Depit"].ToString() : "0.00"), 9, 0, 0, 1, 0, 1, 5, Element.ALIGN_CENTER);
                    cell.Padding = 2;
                    cell.BorderColor = BaseColor.BLUE;
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[x]["AccountNameAr"] != null ? dt.Rows[x]["AccountNameAr"].ToString() : "0.00"), 12, 0, 0, 1, 0, 1, 5, Element.ALIGN_CENTER);

                    cell.Padding = 2;
                    cell.BorderColor = BaseColor.BLUE; 
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[x]["AccountCode"] != null ? dt.Rows[x]["AccountCode"].ToString() : ""), 9, 0, 0, 1, 0, 1, 5, Element.ALIGN_CENTER);
                    cell.Padding = 2;
                    cell.BorderColor = BaseColor.BLUE;
                    bodyTable.AddCell(cell);

                }
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "                                                                                        ", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER);
                cell.Padding = 2;
                cell.BorderColor = BaseColor.BLUE;
                bodyTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 18, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER);
                cell.Padding = 2;
                cell.BorderColor = BaseColor.BLUE;
                bodyTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_16_UNDERLINE.ToString()], totalCredit.ToString(), 9, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER);
                cell.Padding = 2;
                cell.BorderColor = BaseColor.BLUE;
                bodyTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_16_UNDERLINE.ToString()], totalDepit.ToString(), 9, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER);
                cell.Padding = 2;
                cell.BorderColor = BaseColor.BLUE;
                bodyTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_22_NORMAL.ToString()], "الإجـمـــالـــى", 21, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER);
                cell.Padding = 2;
                cell.BorderColor = BaseColor.BLUE;
                bodyTable.AddCell(cell);


            }
            else
            {
                var titlenodata = "لا توجد نتائج ";
                var callNodata = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_BOLD_Red.ToString()], titlenodata, 57, 0, 1, 1, 0, 1, 5, Element.ALIGN_CENTER);
                bodyTable.AddCell(callNodata);
            }


            cell = new PdfPCell(bodyTable);
            cell.Colspan = (57);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            tblParent.AddCell(cell);

            #endregion
            PdfPTable footerTable = new PdfPTable(57);
            footerTable.WidthPercentage = 95;
            footerTable.DefaultCell.BorderWidth = 0;
            footerTable.HorizontalAlignment = Element.ALIGN_CENTER;
            footerTable.DefaultCell.Padding = 5;

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "                                                                                        ", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER);
            footerTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "......................", 12, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER);
            footerTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_NORMAL.ToString()], "المدير المالى    ", 12, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER);
            footerTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 9, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER);
            footerTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], ".......................", 12, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER);
            footerTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_NORMAL.ToString()], "المحاسب     ", 12, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER);
            footerTable.AddCell(cell);

            cell = new PdfPCell(footerTable);
            cell.Colspan = (57);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            tblParent.AddCell(cell);

            doc.Add(tblParent);
            doc.Close();
            return memoryStream.GetBuffer();
        }
        public static void DrawTableHeader(PdfPTable table, PdfPCell cell, Font font, string headerTitle, int borderWidth = 0, int colspan = 0, int paddingBottom = 0, int Padding = 0, int horiAlign = 0, int vertAlign = 0)
        {
            cell = new PdfPCell(new Phrase(headerTitle, font));
            cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            cell.BorderWidth = borderWidth;
            cell.Colspan = colspan;
            cell.PaddingBottom = paddingBottom;
            cell.Padding = Padding;
            cell.HorizontalAlignment = horiAlign;
            cell.VerticalAlignment = vertAlign;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            table.AddCell(cell);
        }
        public static string ConvertNumToString(string Num)
        {
            TaamerProject.Models.Common.Utilities util = new TaamerProject.Models.Common.Utilities(Num);
            if (util.GetNumberAr() == " ")
            {
                NumberToText numberToText = new NumberToText();
                return (numberToText.EnglishNumToText(Num) + " ريال فقط ");
            }
            return (util.GetNumberAr());
        }

        //القيد المحاسبي
        public static byte[] GetAllJournalsByInvID(List<TransactionsVM> dt, OrganizationsVM organizationsVM)
        {
            bool isFooter = (organizationsVM.IsFooter != null && organizationsVM.IsFooter != "" || organizationsVM.IsFooter != "0") ? true : false;//send from user
            string footerData = "" + organizationsVM.NameAr + "  " + organizationsVM.Address + "  - تليفون  : " + organizationsVM.Mobile + " - فاكس  : " + organizationsVM.Fax + " -  بريد الكترونى  : " + organizationsVM.Email;

            Document doc = new Document(PageSize.A4, 10, 10, 30, 44);
//            var memoryStream = PdfBase.OpenDocument(doc, "", false, "", isFooter, footerData);//طباعه افقية

            var memoryStream = PdfBase.OpenDocument(doc, "1", true, "", isFooter, footerData);


            var headerTitle = "";

            PdfPCell cell = null;
            PdfPTable tblParent = new PdfPTable(1);
            tblParent.DefaultCell.Padding = 0;
            tblParent.WidthPercentage = 100;
            tblParent.DefaultCell.BorderWidth = 0;
            tblParent.HorizontalAlignment = Element.ALIGN_CENTER;
            tblParent.SplitLate = false;
            tblParent.HeaderRows = 1;

            #region HeaderReport
            PdfPTable headerTbl = new PdfPTable(57);
            headerTbl.DefaultCell.Padding = 10;
            headerTbl.WidthPercentage = 95;
            headerTbl.DefaultCell.BorderWidth = 1;
            headerTbl.DefaultCell.BorderColor = BaseColor.BLUE;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            #region FirstRow
            headerTitle = "";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 1, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            cell.Padding = 5;
            headerTbl.AddCell(cell);

            if (organizationsVM.LogoUrl != "")
            {
                string imageFilePathLogo = PdfBase.getReportImagePath(organizationsVM.LogoUrl);

                if (imageFilePathLogo != "")
                {
                    Image headerJpg = Image.GetInstance(imageFilePathLogo); //infoReport[1];

                    cell = PdfBase.drawImageCell(headerJpg, 90F, 80F, 19, 3, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
                    headerTbl.AddCell(cell);
                }
                else
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 3, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
            }
            else
            {
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 3, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                headerTbl.AddCell(cell);
            }

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], organizationsVM.NameAr, 16, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM);
            cell.Padding = 10;
            headerTbl.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], organizationsVM.PostalCode, 9, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 9, 1, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "تاريخ : " + PrintDate, 19, 1, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            cell.Padding = 10;
            headerTbl.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 1, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
            cell.Padding = 5;
            headerTbl.AddCell(cell);

            #endregion

            #region secondRow
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_NORMAL.ToString()], "", 57, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            cell.Padding = 5;
            headerTbl.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_NORMAL.ToString()], @Resources.MAcc_ViewJournalEntry, 57, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            cell.Padding = 5;
            headerTbl.AddCell(cell);
            #endregion

            cell = new PdfPCell(headerTbl);
            cell.Colspan = 57;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            tblParent.AddCell(cell);


            #endregion

            PdfPTable bodyTable = new PdfPTable(81);
            bodyTable.WidthPercentage = 95;
            bodyTable.DefaultCell.BorderWidth = 0;
            bodyTable.DefaultCell.BorderColor = BaseColor.BLUE;
            bodyTable.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyTable.DefaultCell.Padding = 5;
            bodyTable.HeaderRows = 1;

            headerTitle = @Resources.General_credit;
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 9, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = @Resources.General_debit;
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 9, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = @Resources.Acc_Statement;
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 9, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = @Resources.General_AccountName;
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 10, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = @Resources.Acc_AccountCode;
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 9, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = @Resources.Pro_ConstraintNo;
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 10, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = @Resources.Acc_Vouchertype;
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 10, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = @Resources.General_VoucherNumber;
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = @Resources.General_Date;
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 12, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);



            if (dt != null && dt.Count > 0)
            {
                decimal total_Credit = 0;
                decimal total_Depit = 0;
                for (int x = 0; x < dt.Count; x++)
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].Credit != null ? dt[x].Credit.ToString() : ""), 9, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].Depit != null ? dt[x].Depit.ToString() : ""), 9, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].Notes != null ? dt[x].Notes.ToString() : ""), 9, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].AccountName != null ? dt[x].AccountName.ToString() : ""), 10, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].AccountCode != null ? dt[x].AccountCode.ToString() : ""), 9, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].JournalNo != null ? dt[x].JournalNo.ToString() : ""), 10, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].TypeName != null ? dt[x].TypeName.ToString() : ""), 10, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].InvoiceNumber != null ? dt[x].InvoiceNumber.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].TransactionDate != null ? dt[x].TransactionDate.ToString() : ""), 12, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    total_Credit = total_Credit + (dt[x].Credit != null ? (decimal)dt[x].Credit : 0);
                    total_Depit = total_Depit + (dt[x].Depit != null ? (decimal)dt[x].Depit : 0);
                }
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (total_Credit.ToString()), 9, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                bodyTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (total_Depit.ToString()), 9, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                bodyTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (""), 9, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                bodyTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (""), 10, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                bodyTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (""), 9, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                bodyTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (""), 10, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                bodyTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (""), 10, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                bodyTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                bodyTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (Resources.Totalcreditordebtor), 12, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                bodyTable.AddCell(cell);
            }
            else
            {
                var titlenodata = "لا توجد نتائج ";
                var callNodata = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_BOLD_Red.ToString()], titlenodata, 57, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                bodyTable.AddCell(callNodata);
            }
            cell = new PdfPCell(bodyTable);
            cell.Colspan = (81);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            tblParent.AddCell(cell);

            doc.Add(tblParent);
            doc.Close();
            return memoryStream.GetBuffer();
        }


    }
}
