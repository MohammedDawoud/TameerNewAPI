using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace TaamerProject.API.pdfHandler
{
    public enum fontsenum
    {
        nationalTextFont_12_NORMAL,
        nationalTextFont_11_UNDERLINE,
        nationalTextFont_11_NORMAL,
        nationalTextFont_13_NORMAL,
        nationalTextFont_9_UNDERLINE,
        nationalTextFont_7_BOLD,
        nationalTextFont_8_NORMAL,
        nationalTextFont_9_BOLD_Red,
        nationalTextFont_7_NORMAL,
        nationalTextFont_14_NORMAL,
        nationalTextFont_16_UNDERLINE,
        nationalTextFont_14_BOLD,
        nationalTextFont_14_BOLD_Red,
        nationalTextFont_10_BOLD,
        nationalTextFont_13_BOLD,
        nationalTextFont_11_BOLD,
        nationalTextFont_9_NORMAL,
        nationalTextFont_8_BOLD,
        nationalTextFont_15_BOLD_RED,
        nationalTextFont_10_NORMAL,
        nationalTextFont_10_White,
        nationalTextFont_13_UNDERLINE,
        nationalTextFont_14_UNDERLINE,
        nationalTextFont_18_UNDERLINE,
        nationalTextFont_18_UNDERLINE_BLUE,
        nationalTextFont_22_NORMAL,
        nationalTextFont_12_BOLD,
        nationalTextFont_15_BOLD,
        nationalTextFont_9_BOLD,
        nationalTextFont_10_BOLD_RED,
        BarCodeFont_20_NORMAL,
        nationalTextFont_20_UNDERLINE,
        nationalTextFont_20_UNDERLINE_BlUE,
        nationalTextFont_8_UNDERLINE_BLUE,
        nationalTextFont_13_NORMAL_GREEN,
        nationalTextFont_13_NORMAL_WHITE,
        nationalTextFont_14_BOLD_RED,
        BarCodeFont_30_NORMAL,
        nationalTextFont_12_BOLD_WHITE,
        nationalTextFont_11_NORMAL_BLUE,
        nationalTextFont_13_NORMAL_GREY,
        nationalTextFont_13_BOLD_GREY,
        nationalTextFont_12_NORMAL_RED,
        nationalTextFont_18_NORMAL,
        nationalTextFont_14_NORMAL_RED,
        arialunicodepath_24_NORMAL_RED,
                    nationalTextFont_7_BOLD_new
    }
    public class PdfBase
    {
        private static Dictionary<string, Font> fontDic;

        public static Dictionary<string, Font> Fonts
        {
            get
            {
                if (fontDic == null)
                {
                    fontDic = PdfBase.DicOfFonts();
                }
                return fontDic;
            }
        }


        public static MemoryStream OpenDocument(Document doc, string show_report_type, bool Rotate = true, string Show_border_type = "", bool isFooter = false, string footerData = "")
        {
            if (Rotate)
                doc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            pdfPage page = new pdfPage();
            System.IO.MemoryStream memory_stream = new MemoryStream();
            PdfWriter pdfwriter = PdfWriter.GetInstance(doc, memory_stream);
            pdfwriter.PageEvent = page;
            if (isFooter)
                page.report_by = reportBy(footerData);
            page.Show_report_type = show_report_type;
            page.Show_border_type = Show_border_type;
            doc.Open();


            return memory_stream;
        }
        public static MemoryStream OpenDocumentProjectReport(ref PdfWriter PdfWriter ,Document doc, string show_report_type, bool Rotate = true, string Show_border_type = "", bool isFooter = false, string footerData = "")
        {
            if (Rotate)
                doc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            pdfPage page = new pdfPage();
            System.IO.MemoryStream memory_stream = new MemoryStream();

            PdfWriter = PdfWriter.GetInstance(doc, memory_stream);
            PdfWriter.PageEvent = page;
            if (isFooter)
                page.report_by = reportBy(footerData);
            page.Show_report_type = show_report_type;
            page.Show_border_type = Show_border_type;
            doc.Open();


            return memory_stream;
        }


        public static string reportBy(string footerData = "")
        {
            string r = string.Empty;
            r += " \n \n ";
            r += footerData;
            return r;
        }

        
        public static BaseFont GetBaseFont()
        {
            BaseFont nationalBase = null;
            string regex_match_arabic_hebrew = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\arial.ttf";
           // string regex_match_arabic_hebrew = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\mohammad bold art 1.ttf";
            if (File.Exists(regex_match_arabic_hebrew))
                nationalBase = BaseFont.CreateFont(regex_match_arabic_hebrew, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            return nationalBase;
        }

        public static BaseFont GetBaseFont2()
        {
            BaseFont nationalBase2 = null;
            string regex_match_arabic_hebrew = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\cairo.ttf";
            // string regex_match_arabic_hebrew = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\mohammad bold art 1.ttf";
            if (File.Exists(regex_match_arabic_hebrew))
                nationalBase2 = BaseFont.CreateFont(regex_match_arabic_hebrew, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            return nationalBase2;
        }


        /// <summary>
        /// return Dictionary of fonts.
        /// </summary>
        /// <returns></returns>
        /// notoSansarabic-regular !important
        private static Dictionary<string, Font> DicOfFonts()
        {
            BaseFont nationalBase = GetBaseFont();
            BaseFont nationalBase2 = GetBaseFont2();
            //add more styles for font
            Dictionary<string, Font> fonts = new Dictionary<string, Font>
            {
                {fontsenum.nationalTextFont_7_BOLD_new.ToString(),new Font(nationalBase2, 7f, Font.BOLD)},
                {fontsenum.nationalTextFont_7_BOLD.ToString(),new Font(nationalBase, 7f, Font.BOLD)},
                {fontsenum.nationalTextFont_7_NORMAL.ToString(),new Font(nationalBase, 7f, Font.NORMAL, new BaseColor(0, 0, 255))},
                {fontsenum.nationalTextFont_8_NORMAL.ToString(),new Font(nationalBase, 8f, Font.NORMAL)},
                {fontsenum.nationalTextFont_8_BOLD.ToString(),new Font(nationalBase, 8f, Font.BOLD)},
                {fontsenum.nationalTextFont_9_NORMAL.ToString(),new Font(nationalBase, 9f, Font.NORMAL)},
                {fontsenum.nationalTextFont_9_BOLD.ToString(),new Font(nationalBase, 9f, Font.BOLD)},
                {fontsenum.nationalTextFont_9_BOLD_Red.ToString(),new Font(nationalBase, 9f, Font.BOLD, new BaseColor(255, 0, 0))},
                {fontsenum.nationalTextFont_9_UNDERLINE.ToString(),new Font(nationalBase, 9f, Font.UNDERLINE)},
                {fontsenum.nationalTextFont_10_BOLD.ToString(),new Font(nationalBase, 10f, Font.BOLD, BaseColor.BLACK)},
                {fontsenum.nationalTextFont_10_BOLD_RED.ToString(),new Font(nationalBase, 10f, Font.BOLD, BaseColor.RED)},
                {fontsenum.nationalTextFont_10_NORMAL.ToString(),new Font(nationalBase, 10f, Font.NORMAL, BaseColor.BLACK)},
                 {fontsenum.nationalTextFont_10_White.ToString(),new Font(nationalBase, 10f, Font.NORMAL, BaseColor.WHITE)},
                {fontsenum.nationalTextFont_11_UNDERLINE.ToString(), new Font(nationalBase, 11f, Font.UNDERLINE) },
                {fontsenum.nationalTextFont_11_NORMAL.ToString(),new Font(nationalBase, 11f, Font.NORMAL)},
                {fontsenum.nationalTextFont_11_BOLD.ToString(),new Font(nationalBase, 11f, Font.BOLD, BaseColor.BLACK)},
                {fontsenum.nationalTextFont_12_NORMAL.ToString(),  new Font(nationalBase, 12f, Font.NORMAL)},
                {fontsenum.nationalTextFont_12_BOLD.ToString(),  new Font(nationalBase, 12f, Font.BOLD)},
                {fontsenum.nationalTextFont_13_NORMAL.ToString(), new Font(nationalBase, 13f, Font.NORMAL)},
                {fontsenum.nationalTextFont_13_BOLD.ToString(), new Font(nationalBase, 13f, Font.BOLD)},
                {fontsenum.nationalTextFont_13_UNDERLINE.ToString(), new Font(nationalBase, 13f, Font.UNDERLINE)},
                {fontsenum.nationalTextFont_14_BOLD.ToString(), new Font(nationalBase, 14f, Font.BOLD)},
                {fontsenum.nationalTextFont_14_BOLD_Red.ToString(),new Font(nationalBase, 14f, Font.BOLD, BaseColor.RED)},
                {fontsenum.nationalTextFont_14_UNDERLINE.ToString(),new Font(nationalBase, 14f, Font.UNDERLINE)},
                {fontsenum.nationalTextFont_15_BOLD.ToString(),new Font(nationalBase, 15f, Font.BOLD)},
                {fontsenum.nationalTextFont_15_BOLD_RED.ToString(),new Font(nationalBase, 15f, Font.BOLD, new BaseColor(255, 0, 0))},
                {fontsenum.nationalTextFont_16_UNDERLINE.ToString(),new Font(nationalBase, 16f, Font.UNDERLINE)},
                {fontsenum.nationalTextFont_18_UNDERLINE.ToString(), new Font(nationalBase, 18f, Font.UNDERLINE)},
                {fontsenum.nationalTextFont_18_UNDERLINE_BLUE.ToString(), new Font(nationalBase, 18f, Font.UNDERLINE,new BaseColor(0, 138, 204))},
                {fontsenum.nationalTextFont_22_NORMAL.ToString(), new Font(nationalBase, 22f, Font.NORMAL)},
                {fontsenum.BarCodeFont_20_NORMAL.ToString(), new Font(nationalBase, 20f, Font.NORMAL)},
                {fontsenum.nationalTextFont_20_UNDERLINE.ToString(), new Font(nationalBase, 20f, Font.UNDERLINE)},
                 {fontsenum.nationalTextFont_20_UNDERLINE_BlUE.ToString(), new Font(nationalBase, 20f, Font.UNDERLINE, new BaseColor(0, 138, 204))},
                {fontsenum.nationalTextFont_8_UNDERLINE_BLUE.ToString(), new Font(nationalBase, 8f, Font.UNDERLINE, new BaseColor(39, 113, 179))},
                {fontsenum.nationalTextFont_13_NORMAL_GREEN.ToString(), new Font(nationalBase, 13f, Font.NORMAL, new BaseColor(0, 132, 0))},
                {fontsenum.nationalTextFont_13_NORMAL_WHITE.ToString(), new Font(nationalBase, 13f, Font.NORMAL, new BaseColor(255, 255, 255))},
                {fontsenum.nationalTextFont_14_BOLD_RED.ToString(), new Font(nationalBase, 14f, Font.BOLD, BaseColor.RED)},
                {fontsenum.BarCodeFont_30_NORMAL.ToString(), new Font(nationalBase, 30f, Font.NORMAL)},
                {fontsenum.nationalTextFont_12_BOLD_WHITE.ToString(), new Font(nationalBase, 12f, Font.BOLD, BaseColor.WHITE)},
                {fontsenum.nationalTextFont_11_NORMAL_BLUE.ToString(), new Font(nationalBase, 11f, Font.ITALIC, new BaseColor(0, 138, 204))},
                {fontsenum.nationalTextFont_13_NORMAL_GREY.ToString(), new Font(nationalBase, 13f, Font.NORMAL, new BaseColor(123, 123, 123))},
                {fontsenum.nationalTextFont_13_BOLD_GREY.ToString(), new Font(nationalBase, 13f, Font.BOLD, new BaseColor(123, 123, 123))},
                {fontsenum.nationalTextFont_12_NORMAL_RED.ToString(), new Font(nationalBase, 12f, Font.NORMAL, BaseColor.RED)},
                {fontsenum.nationalTextFont_18_NORMAL.ToString(), new Font(nationalBase, 18f, Font.NORMAL)},
                {fontsenum.nationalTextFont_14_NORMAL.ToString(), new Font(nationalBase, 14f, Font.NORMAL)},
                {fontsenum.nationalTextFont_14_NORMAL_RED.ToString(), new Font(nationalBase, 14f, Font.NORMAL, BaseColor.RED)},
                {fontsenum.arialunicodepath_24_NORMAL_RED.ToString(), new Font(nationalBase, 24f, Font.NORMAL, BaseColor.RED)},

        };

            return fonts;
        }

        /// <summary>
        /// draw image and return imag cell
        /// </summary>
        /// <param name="img">iTextSharp.text.Image</param>
        /// <param name="imgWidth">float, img Width</param>
        /// <param name="imgHigth">float ,img Higth</param>
        /// <param name="Colspan">int Colspan</param>
        /// <param name="Rowspan">int Rowspan</param>
        /// <param name="paddingLeft">int paddingLeft</param>
        /// <param name="paddingTop">int paddingTop</param>
        /// <param name="paddingRigth">int paddingRigth</param>
        /// <param name="paddingBottom">int paddingBottom</param>
        /// <param name="HoriAlign">int HorizontalAlignment(Element.ALIGN_LEFT ,...etc)</param>
        /// <param name="VertAlign">int VerticalAlignment(Element.ALIGN_MIDDLE,..etc)</param>
        /// <returns></returns>
        public static PdfPCell drawImageCell(iTextSharp.text.Image img, float imgWidth, float imgHigth, int Colspan = 0, int Rowspan = 0, int paddingLeft = 0, int paddingTop = 0, int paddingRigth = 0, int paddingBottom = 0, int HoriAlign = 0, int VertAlign = 0)
        {
            img.BorderWidth = 0f;
            img.ScaleAbsolute(imgWidth, imgHigth);

            PdfPCell cell = new PdfPCell(img);
            cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            cell.BorderWidth = 0;
            cell.Colspan = Colspan;
            if (Rowspan != 0) cell.Rowspan = Rowspan;
            cell.HorizontalAlignment = (HoriAlign == 0) ? (Element.ALIGN_LEFT) : (HoriAlign);
            cell.VerticalAlignment = (VertAlign == 0) ? (Element.ALIGN_MIDDLE) : (VertAlign);
            cell.PaddingLeft = paddingLeft;
            cell.PaddingRight = paddingRigth;
            cell.PaddingTop = paddingTop;
            cell.PaddingBottom = paddingBottom;
            cell.PaddingBottom = 5;

            return cell;
        }

        public static string getReportImagePath(string path)
        {
            //return HttpContext.Current.Server.MapPath("~/Uploads/Organizations/pictures/yy.jpg");
            var isExist = Path.Combine(path);

            if (File.Exists(isExist))
            {
                return isExist;
            }
            return "";
        }

        public static PdfPCell drawLogoImageCell(float imgWidth = 80, float imgHigth = 70, int Colspan = 0, int Rowspan = 0, int paddingLeft = 0, int paddingTop = 0, int paddingRigth = 0, int paddingBottom = 0, int HoriAlign = 0, int VertAlign = 0, string path = "")
        {
            string imageFilePath1 = getReportImagePath(path);
            Image _jpg = Image.GetInstance(imageFilePath1);
            return drawImageCell(_jpg, imgWidth, imgHigth, Colspan, Rowspan, paddingLeft, paddingTop, paddingRigth, paddingBottom, HoriAlign, VertAlign);
        }

        /// <summary>
        /// draw datatable if colspan and rowpan is the same in each column.
        /// </summary>
        /// <param name="Db">DataTable</param>
        /// <param name="cell">cell Instance</param>
        /// <param name="pdf_Table">pdf Table to add each cell to </param>
        /// <param name="font">Font</param>
        /// <param name="horiAlign">int, HorizontalAlignment(Element.ALIGN_LEFT ,...etc)</param>
        /// <param name="vertAlign">int ,VerticalAlignment(Element.ALIGN_MIDDLE,..etc)</param>
        /// <param name="borderWidth">int, borderWidth</param>
        /// <param name="colspan">int ,colspan</param>
        /// <param name="paddingBottom">int, paddingBottom</param>
        /// <param name="directionTrl">int, PdfWriter.RUN_DIRECTION_RTL</param>
        public static PdfPTable DarwTable(DataTable Db, PdfPCell cell, PdfPTable pdf_Table, Font font, int horiAlign = 0, int vertAlign = 0, int borderWidth = 0, int colspan = 0, int paddingBottom = 0, int directionTrl = 0)
        {
            foreach (DataRow dtRow in Db.Rows)
            {
                foreach (DataColumn dc in Db.Columns)
                {
                    var cellValue = dtRow[dc].ToString();
                    cell = new PdfPCell(new Phrase(cellValue, font));
                    cell.RunDirection = directionTrl;
                    cell.BorderWidth = borderWidth;
                    cell.Colspan = colspan;
                    cell.PaddingBottom = paddingBottom;
                    cell.HorizontalAlignment = horiAlign;
                    cell.VerticalAlignment = vertAlign;
                    pdf_Table.AddCell(cell);
                }
            }
            return pdf_Table;
        }
        /// <summary>
        /// create, and draw new PdfPCell Instance and return it .
        /// </summary>
        /// <param name="font">Font</param>
        /// <param name="text">text show in cell</param>
        /// <param name="Colspan">int , Colspan</param>
        /// <param name="Rowspan">int ,Rowspan</param>
        /// <param name="BorderWidthLeft">int ,BorderWidthLeft</param>
        /// <param name="BorderWidthRight">int ,BorderWidthRight</param>
        /// <param name="BorderWidthTop">int ,BorderWidthTop</param>
        /// <param name="BorderWidthBottom">int ,BorderWidthBottom</param>
        /// <param name="Padding">int ,Padding</param>
        /// <param name="HoriAlign">int, HorizontalAlignment(Element.ALIGN_LEFT ,...etc)</param>
        /// <param name="VertAlign">int ,VerticalAlignment(Element.ALIGN_MIDDLE,..etc)</param>
        /// <param name="BgColor">(BaseColor.WHITE,...etc)</param>
        /// <returns></returns>
        public static PdfPCell drawCell(Font font, string text = "", int Colspan = 0, int Rowspan = 0, int BorderWidthLeft = 0, int BorderWidthRight = 0, int BorderWidthTop = 0, int BorderWidthBottom = 0, int Padding = 0, int HoriAlign = 0, int VertAlign = 0, BaseColor BgColor = null)
        {
            Phrase ph = new Phrase(text, font);
            //ph.Leading = 50;
            PdfPCell cell = new PdfPCell(ph);
            cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            cell.BorderWidth = 0;
            cell.Colspan = Colspan;
            if (Rowspan != 0) cell.Rowspan = Rowspan;
            cell.HorizontalAlignment = (HoriAlign == 0) ? (Element.ALIGN_LEFT) : (HoriAlign);
            cell.VerticalAlignment = (VertAlign == 0) ? (Element.ALIGN_MIDDLE) : (VertAlign);
            cell.BackgroundColor = (BgColor == null) ? (BaseColor.WHITE) : (BgColor);
            cell.BorderWidthLeft = BorderWidthLeft;
            cell.BorderWidthRight = BorderWidthRight;
            cell.BorderWidthTop = BorderWidthTop;
            cell.BorderWidthBottom = BorderWidthBottom;
            cell.PaddingBottom = 5;
            cell.Padding = Padding;
            return cell;
        }

        public static PdfPCell drawCell(string text, Font font,
            int ColSpan, int RowSpan = 1,
            float borderWidthTop = 1f, float rightWidth = 1f, float borderWidthBottom = 1f, float borderWidthLeft = 1f,
            float paddingTop = 0f, float borderWidthRight = 0f, float paddingBottom = 0f, float paddingLeft = 0f,
            int HoriAlign = 0, int VertAlign = 0, BaseColor BgColor = null)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font));

            cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;

            cell.Colspan = ColSpan;
            cell.Rowspan = RowSpan;

            cell.HorizontalAlignment = HoriAlign;
            cell.VerticalAlignment = VertAlign;

            cell.BackgroundColor = BgColor == null ? BaseColor.WHITE : BgColor;

            cell.BorderWidthLeft = borderWidthLeft;
            cell.BorderWidthRight = rightWidth;
            cell.BorderWidthTop = borderWidthTop;
            cell.BorderWidthBottom = borderWidthBottom;

            cell.PaddingTop = paddingTop;
            cell.PaddingRight = borderWidthRight;
            cell.PaddingBottom = paddingBottom;
            cell.PaddingLeft = paddingLeft;

            return cell;
        }

        public static PdfPCell EditCell(PdfPCell cell, int Colspan = 0, int Rowspan = 0, Font font = null, string text = "", int BorderWidth = 0, int Padding = 0, int HoriAlign = 1, int VertAlign = 1, BaseColor BgColor = null)
        {
            if (font != null)
                cell = new PdfPCell(new Phrase(text, font));
            cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            cell.BorderWidth = 0;
            cell.Colspan = Colspan;
            if (Rowspan != 0) cell.Rowspan = Rowspan;
            cell.HorizontalAlignment = HoriAlign;
            cell.VerticalAlignment = VertAlign;
            cell.BackgroundColor = (BgColor == null) ? (BaseColor.WHITE) : (BgColor);
            cell.BorderWidth = BorderWidth;
            cell.PaddingBottom = 5;
            cell.Padding = Padding;

            return cell;
        }

        public static PdfPTable NoResultCell(PdfPCell cell, PdfPTable mainTable, int Colspan = 30, string Message = "")
        {
            cell = new PdfPCell(new Phrase(Message ?? "لا يوجد نتائج متاحة", PdfBase.Fonts[fontsenum.nationalTextFont_14_BOLD_Red.ToString()]));
            cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            cell.BorderWidthBottom = 1;
            cell.Colspan = Colspan;
            cell.BorderWidthBottom = 1;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            mainTable.AddCell(cell);
            return mainTable;
        }
        private static void drawTableCell(string text, int colSpan, PdfPTable first_dataTbl, int hAlign, int vAlign, int borderWidth = 0, int rowSpan = 0, int paddingBottom = 0, int paddingRight = 0, int borderWidthLeft = 0, int borderWidthRight = 0, int borderWidthBottom = 0)
        {
            var cell = drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], text, colSpan,
                       rowSpan, borderWidth, borderWidth, borderWidth, borderWidth, 0, hAlign, vAlign, null);
            cell.PaddingBottom = paddingBottom;
            cell.PaddingRight = paddingRight;
            if (borderWidthLeft != 0) cell.BorderWidthLeft = borderWidthLeft;
            if (borderWidthRight != 0) cell.BorderWidthRight = borderWidthRight;
            if (borderWidthBottom != 0) cell.BorderWidthBottom = borderWidthBottom;
            cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            first_dataTbl.AddCell(cell);
        }
        public static string en2ar_numbers(string en_no)
        {
            char[] i_no = en_no.ToCharArray();
            string ar_no = "";
            char digitBase = '\u0660';
            int digitDelta = digitBase - '\u0030';
            for (int i = 0; i < i_no.Length; i++)
            {
                char ch = i_no[i];
                if (ch <= '\u0039' && ch >= '\u0030')
                {
                    i_no[i] += (char)digitDelta;
                    ar_no += i_no[i].ToString();
                }
                else
                {
                    ar_no += i_no[i].ToString();
                }
            }
            return ar_no;
        }
    }
}