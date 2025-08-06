using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace TaamerProject.API.pdfHandler
{
    public class pdfPage : iTextSharp.text.pdf.PdfPageEventHelper
    {
        #region Variables

        public string Show_report_type;
        public string report_by; // footer text at end of each page //اعداد ... بتاريخ ...ا
        public string Show_border_type;
        protected PdfTemplate total;
        public int pagesCount;

        #endregion

        #region Properties

        protected iTextSharp.text.Font footer
        {
            get
            {
                iTextSharp.text.BaseColor grey = new BaseColor(128, 128, 128);
                iTextSharp.text.Font font = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL, grey);
                return font;
            }
        }

        #endregion

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            total = writer.DirectContent.CreateTemplate(100, 100);
            total.BoundingBox = new Rectangle(-20, -20, 100, 100);
            //BaseFont helv = BaseFont.CreateFont(arialunicodepath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        }

        public override void OnStartPage(PdfWriter writer, Document doc)
        {

        }

        public override void OnEndPage(PdfWriter writer, Document doc)
        {
            if (!string.IsNullOrEmpty(Show_report_type))
            {
                page_no_footer(writer, doc);
            }
            if (!string.IsNullOrEmpty(report_by))
            {
                report_By_footer(writer, doc);
            }
            if (!string.IsNullOrEmpty(Show_border_type))
            {
                set_border_type(writer, doc);
            }
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            BaseFont nationalBase = PdfBase.GetBaseFont();

            Font small_nationalTextFont = new Font(nationalBase, 17f, Font.BOLD);
            // helv = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.NOT_EMBEDDED);
            total.BeginText();
            total.SetFontAndSize(nationalBase, 10);
            total.SetTextMatrix(0, 0);
            int pageNumber = writer.PageNumber;
            pagesCount = pageNumber;
            total.ShowText(PdfBase.en2ar_numbers(Convert.ToString(pageNumber)) + " /  ");
            total.EndText();
        }
        void page_no_footer(PdfWriter writer, Document doc)
        {
            switch (Show_report_type.Trim())
            {
                case "0":
                    get_page_no(writer, doc, 55, 140, false);
                    break;
                case "1":
                    get_page_no(writer, doc, 30, 92, false);
                    break;
                case "2":
                    get_page_no(writer, doc, 55, 100, false);
                    break;
                case "3":
                    get_page_no(writer, doc, 30, 160, false);
                    break;
            }


            BaseFont nationalBase = PdfBase.GetBaseFont();

            Font small_nationalTextFont = new Font(nationalBase, 9f, Font.BOLD);
            Font max_nationalTextFont = new Font(nationalBase, 16f, Font.BOLD);

            PdfPTable footerTbl = new PdfPTable(2);
            footerTbl.TotalWidth = doc.PageSize.Width;
            footerTbl.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell PdfPCell1 = null;

            Paragraph para;
            para = new Paragraph("   ", footer);
            para.Add(Environment.NewLine);
            PdfPCell1 = new PdfPCell(para);
            PdfPCell1.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            PdfPCell1.Border = 0;
            PdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            PdfPCell1.PaddingLeft = 10;
            footerTbl.AddCell(PdfPCell1);

            para = new Paragraph("    ", footer);
            para.Add(Environment.NewLine);
            PdfPCell1 = new PdfPCell(para);
            PdfPCell1.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            PdfPCell1.Border = 0;
            PdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            PdfPCell1.PaddingLeft = 10;
            footerTbl.AddCell(PdfPCell1);

            PdfPCell1 = new PdfPCell(new Phrase(" صفحة رقم  " + PdfBase.en2ar_numbers(doc.PageNumber.ToString()), small_nationalTextFont));
            PdfPCell1.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            PdfPCell1.Border = 0;
            PdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            PdfPCell1.PaddingLeft = 30;
            footerTbl.AddCell(PdfPCell1);
            PdfPCell1 = new PdfPCell(new Phrase("     ", small_nationalTextFont));
            PdfPCell1.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            PdfPCell1.Border = 0;
            PdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            PdfPCell1.PaddingLeft = 10;
            footerTbl.AddCell(PdfPCell1);

            footerTbl.WriteSelectedRows(0, -1, 0, (doc.BottomMargin + 10), writer.DirectContent);
        }

        void get_page_no(PdfWriter writer, Document doc, int top, int left, bool isTop)
        {
            PdfContentByte cb = writer.DirectContent;
            cb.SaveState();
            float textBase = ((isTop == true) ? (doc.Top) : (0)) + top;
            float textSize = ((isTop == true) ? (left - 20) : (left));
            cb.AddTemplate(total, doc.Left + textSize, textBase);
            cb.RestoreState();
        }

        public static byte[] addWaterMarkToPdf(byte[] pdfBytes, string waterMarkStr)
        {
            PdfReader reader = new PdfReader(pdfBytes);
            int n = reader.NumberOfPages;
            Document document = new Document(reader.GetPageSizeWithRotation(1));
            MemoryStream ms = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, ms);
            document.Open();

            PdfContentByte cb = writer.DirectContent;
            PdfImportedPage page;

            string waterMark = string.Empty;
            int rotation = 0, i = 0;

            while (i < n)
            {
                i++;
                document.SetPageSize(reader.GetPageSizeWithRotation(i));
                document.NewPage();
                page = writer.GetImportedPage(reader, i);
                rotation = reader.GetPageRotation(i);

                if (rotation == 90 || rotation == 270)
                    cb.AddTemplate(page, 0, -1f, 1f, 0, 0, reader.GetPageSizeWithRotation(i).Height);
                else
                    cb.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);

                Font pdffont = new Font(Font.FontFamily.HELVETICA, 40, Font.BOLD, new GrayColor(0.75f));
                do
                {
                    waterMark += waterMarkStr + " ";
                } while (waterMark.Length < 45);
                ColumnText.ShowTextAligned(writer.DirectContentUnder, Element.ALIGN_LEFT, new Phrase(waterMark.Substring(0, 25), pdffont), 55, 400, 56, PdfWriter.RUN_DIRECTION_LTR, ColumnText.AR_LIG);
                ColumnText.ShowTextAligned(writer.DirectContentUnder, Element.ALIGN_LEFT, new Phrase(waterMark.Substring(0, 41), pdffont), 62, 55, 56, PdfWriter.RUN_DIRECTION_LTR, ColumnText.AR_LIG);
                ColumnText.ShowTextAligned(writer.DirectContentUnder, Element.ALIGN_LEFT, new Phrase(waterMark.Substring(0, 25), pdffont), 300, 50, 56, PdfWriter.RUN_DIRECTION_LTR, ColumnText.AR_LIG);
            }
            document.Close();

            return ms.GetBuffer();
        }

        void report_By_footer(PdfWriter writer, Document doc)
        {
            BaseFont nationalBase = PdfBase.GetBaseFont();

            Font small_nnationalTextFont = new Font(nationalBase, 9f, Font.NORMAL);
            PdfPTable footerTbl = new PdfPTable(1);
            footerTbl.TotalWidth = doc.PageSize.Width;
            footerTbl.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell PdfPCell1 = null;
            PdfPCell1 = new PdfPCell(new Phrase(report_by, small_nnationalTextFont));
            PdfPCell1.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            PdfPCell1.Border = 0;
            PdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT;
            PdfPCell1.PaddingRight = 30;
            PdfPCell1.PaddingLeft = 10;
            footerTbl.AddCell(PdfPCell1);

            footerTbl.WriteSelectedRows(0, -1, 0, (doc.BottomMargin), writer.DirectContent);

        }

        void set_border_type(PdfWriter writer, Document doc)
        {
            if ((new[] { "0", "1" }).Contains(Show_border_type.Trim()))
            {
                PdfContentByte cb = writer.DirectContent;
                switch (Show_border_type.Trim())
                {
                    case "0": // Horizontal Border
                        cb.RoundRectangle(12f, 12f, 817f, 575f, 20f);
                        break;
                    case "1": // Vertical Border 
                        cb.RoundRectangle(12f, 20f, 570f, 800f, 20f);
                        break;
                }
                cb.Stroke();
            }
        }
    }
}