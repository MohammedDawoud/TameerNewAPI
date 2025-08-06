using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data;
using Newtonsoft.Json;
//using System.Web.Script.Serialization;
using System.Globalization;

namespace TaamerProject.API.pdfHandler
{
    public class pdfClass
    {
        //1 - DencesAbouutToExpireReport
        public static byte[] DencesAbouutToExpireReport(string[] infoReport, string[] columnHeader, DataTable listData)
        {
            bool isFooter = true;//send from user
            string printType = "2";//send from user 0=> طباعه افقية 
            //1=>طباعه راسية
            string footerData = "مؤسسة بياناتك لتقنية المعلومات العنوان : المملكة العربية السعودية - المدينة المنورة - تليفون : 0503326610 ";//get from data base ----Not fixed
            Document doc = new Document(PageSize.A4, 10, 10, 30, 44);
            var memoryStream = PdfBase.OpenDocument(doc, "1", true, "", isFooter, footerData);//طباعه افقية

            doc.Open();
            //Document doc = new Document(PageSize.A4, 5, 5, 30, 44);
            //var memoryStream = PdfBase.OpenDocument(doc, "1");
            var headerTitle = "";
            #region data

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds.Tables.Add(dt);
            dt.Clear();

            for (int i = 0; i <= columnHeader.Length - 1; i++)
            {
                dt.Columns.Add(columnHeader[i]);
            }

            for (int j = 0; j <= listData.Rows.Count - 1; j++)
            {
                DataRow _ravi = dt.NewRow();

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    _ravi[i] = listData.Rows[j][i];
                }
                dt.Rows.Add(_ravi);
            }

            #endregion

            PdfPCell cell = null;
            PdfPTable parent = new PdfPTable(29);
            parent.DefaultCell.Padding = 10;
            parent.WidthPercentage = 98;
            parent.DefaultCell.BorderWidth = 0;
            parent.HorizontalAlignment = Element.ALIGN_CENTER;
            parent.HeaderRows = 1;

            #region HeaderReport
            PdfPTable headerTbl = new PdfPTable(29);
            headerTbl.DefaultCell.Padding = 10;
            headerTbl.WidthPercentage = 98;
            headerTbl.DefaultCell.BorderWidth = 0;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            #region FirstRow

            if (infoReport[2] != "")
            {
                string imageFilePathLogo = PdfBase.getReportImagePath(infoReport[2]);
                if (imageFilePathLogo != "")
                {
                    Image headerJpg = Image.GetInstance(imageFilePathLogo); //infoReport[1];

                    cell = PdfBase.drawImageCell(headerJpg, 90F, 80F, 20, 2, 230, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
                else
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 20, 2, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
            }

            headerTitle = infoReport[0];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 15, 1, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            headerTitle = infoReport[3];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 14, 1, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            #endregion

            #region secondRow

            DateTime thisDay = DateTime.Today;
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], thisDay.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), 3, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            headerTitle = "التاريخ  :   ";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 3, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            string title = infoReport[1];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_16_UNDERLINE.ToString()], title, 17, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            #endregion


            cell = new PdfPCell(headerTbl);
            cell.Colspan = (29);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            parent.AddCell(cell);

            #endregion

            #region HeaderTable
            PdfPTable dataTbl = new PdfPTable(29);
            dataTbl.DefaultCell.Padding = 10;
            dataTbl.WidthPercentage = 98;
            dataTbl.DefaultCell.BorderWidth = 0;
            dataTbl.HorizontalAlignment = Element.ALIGN_CENTER;
            dataTbl.HeaderRows = 1;
            dataTbl.SplitLate = false;

            headerTitle = "الفرع";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "القسم";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "تاريخ التنبيه";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "تاريخ الانتهاء";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "الجنسية";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "اسم الموظف";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "رقم الهوية ";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            #endregion

            #region body

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["الفرع"].ToString()), 4, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["القسم"].ToString()), 4, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], ds.Tables[0].Rows[x]["تاريخ التنبيه"].ToString(), 4, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], ds.Tables[0].Rows[x]["تاريخ الانتهاء"].ToString(), 4, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    var MND_START_DATE = ds.Tables[0].Rows[x]["الجنسية"].ToString();
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], MND_START_DATE, 4, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    var MND_END_DATE = ds.Tables[0].Rows[x]["اسم الموظف"].ToString();
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], MND_END_DATE, 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["رقم الهوية"].ToString()), 4, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);
                }
            }
            else
            {
                var titlenodata = "لا توجد نتائج ";
                var callNodata = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_BOLD_Red.ToString()], titlenodata, 29, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                dataTbl.AddCell(callNodata);
            }

            cell = new PdfPCell(dataTbl);
            cell.Colspan = (29);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            parent.AddCell(cell);
            #endregion

            doc.Add(parent);
            doc.Close();
            return memoryStream.GetBuffer();
        }

        //2 - ResDencesExpiredReport
        public static byte[] ResDencesExpiredReport(string[] infoReport, string[] columnHeader, DataTable listData)
        {
            bool isFooter = true;//send from user
            string printType = "2";//send from user 0=> طباعه افقية 
            //1=>طباعه راسية
            string footerData = "مؤسسة بياناتك لتقنية المعلومات العنوان : المملكة العربية السعودية - المدينة المنورة- تليفون : 0503326610 ";//get from data base ----Not fixed
            Document doc = new Document(PageSize.A4, 10, 10, 30, 44);
            var memoryStream = PdfBase.OpenDocument(doc, "1", true, "", isFooter, footerData);//طباعه افقية
            doc.Open();
            var headerTitle = "";
            #region data

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds.Tables.Add(dt);
            dt.Clear();

            for (int i = 0; i <= columnHeader.Length - 1; i++)
            {
                dt.Columns.Add(columnHeader[i]);
            }

            for (int j = 0; j <= listData.Rows.Count - 1; j++)
            {
                DataRow _ravi = dt.NewRow();

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    _ravi[i] = listData.Rows[j][i];
                }
                dt.Rows.Add(_ravi);
            }

            #endregion

            PdfPCell cell = null;
            PdfPTable parent = new PdfPTable(29);
            parent.DefaultCell.Padding = 10;
            parent.WidthPercentage = 98;
            parent.DefaultCell.BorderWidth = 0;
            parent.HorizontalAlignment = Element.ALIGN_CENTER;
            parent.HeaderRows = 1;

            #region HeaderReport
            PdfPTable headerTbl = new PdfPTable(29);
            headerTbl.DefaultCell.Padding = 10;
            headerTbl.WidthPercentage = 98;
            headerTbl.DefaultCell.BorderWidth = 0;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            #region FirstRow


            if (infoReport[2] != "")
            {
                string imageFilePathLogo = PdfBase.getReportImagePath(infoReport[2]);
                if (imageFilePathLogo != "")
                {
                    Image headerJpg = Image.GetInstance(imageFilePathLogo); //infoReport[1];

                    cell = PdfBase.drawImageCell(headerJpg, 90F, 80F, 20, 2, 230, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
                else
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 20, 2, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
            }

            headerTitle = infoReport[0];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 15, 1, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            headerTitle = infoReport[3];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 14, 1, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            #endregion

            #region secondRow

            DateTime thisDay = DateTime.Today;
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], thisDay.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), 3, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            headerTitle = "التاريخ  :   ";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 3, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            string title = infoReport[1];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_16_UNDERLINE.ToString()], title, 17, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            #endregion


            cell = new PdfPCell(headerTbl);
            cell.Colspan = (29);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            parent.AddCell(cell);

            #endregion

            #region HeaderTable
            PdfPTable dataTbl = new PdfPTable(29);
            dataTbl.DefaultCell.Padding = 10;
            dataTbl.WidthPercentage = 98;
            dataTbl.DefaultCell.BorderWidth = 0;
            dataTbl.HorizontalAlignment = Element.ALIGN_CENTER;
            dataTbl.HeaderRows = 1;
            dataTbl.SplitLate = false;


            headerTitle = "الفرع";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "القسم";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "تاريخ الانتهاء";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "الجنسية";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "اسم الموظف";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "رقم الهوية ";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            #endregion

            #region body

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["الفرع"].ToString()), 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["القسم"].ToString()), 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], ds.Tables[0].Rows[x]["تاريخ الانتهاء"].ToString(), 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    var MND_START_DATE = ds.Tables[0].Rows[x]["الجنسية"].ToString();
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], MND_START_DATE, 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    var MND_END_DATE = ds.Tables[0].Rows[x]["اسم الموظف"].ToString();
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], MND_END_DATE, 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["رقم الهوية"].ToString()), 4, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);
                }
            }
            else
            {
                var titlenodata = "لا توجد نتائج ";
                var callNodata = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_BOLD_Red.ToString()], titlenodata, 29, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                dataTbl.AddCell(callNodata);
            }

            cell = new PdfPCell(dataTbl);
            cell.Colspan = (29);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            parent.AddCell(cell);
            #endregion

            doc.Add(parent);
            doc.Close();
            return memoryStream.GetBuffer();
        }

        //3 - OfficialDocsAboutToExpireReport
        public static byte[] OfficialDocsAboutToExpireReport(string[] infoReport, string[] columnHeader, DataTable listData)
        {
            bool isFooter = true;//send from user
            string printType = "2";//send from user 0=> طباعه افقية 
            //1=>طباعه راسية
            string footerData = "مؤسسة بياناتك لتقنية المعلومات العنوان : المملكة العربية السعودية - المدينة المنورة- تليفون : 0503326610  ";//get from data base ----Not fixed
            Document doc = new Document(PageSize.A4, 10, 10, 30, 44);
            var memoryStream = PdfBase.OpenDocument(doc, "1", true, "", isFooter, footerData);//طباعه افقية
            doc.Open();
            var headerTitle = "";
            #region data

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds.Tables.Add(dt);
            dt.Clear();

            for (int i = 0; i <= columnHeader.Length - 1; i++)
            {
                dt.Columns.Add(columnHeader[i]);
            }

            for (int j = 0; j <= listData.Rows.Count - 1; j++)
            {
                DataRow _ravi = dt.NewRow();

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    _ravi[i] = listData.Rows[j][i];
                }
                dt.Rows.Add(_ravi);
            }

            #endregion

            PdfPCell cell = null;
            PdfPTable parent = new PdfPTable(29);
            parent.DefaultCell.Padding = 10;
            parent.WidthPercentage = 98;
            parent.DefaultCell.BorderWidth = 0;
            parent.HorizontalAlignment = Element.ALIGN_CENTER;
            parent.HeaderRows = 1;

            #region HeaderReport
            PdfPTable headerTbl = new PdfPTable(29);
            headerTbl.DefaultCell.Padding = 10;
            headerTbl.WidthPercentage = 98;
            headerTbl.DefaultCell.BorderWidth = 0;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            #region FirstRow

            if (infoReport[2] != "")
            {
                string imageFilePathLogo = PdfBase.getReportImagePath(infoReport[2]);

                if (imageFilePathLogo != "")
                {
                    Image headerJpg = Image.GetInstance(imageFilePathLogo); //infoReport[1];

                    cell = PdfBase.drawImageCell(headerJpg, 90F, 80F, 20, 2, 230, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
                else
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 20, 2, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
            }

            headerTitle = infoReport[0];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 15, 1, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            headerTitle = infoReport[3];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 14, 1, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            #endregion

            #region secondRow

            DateTime thisDay = DateTime.Today;
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], thisDay.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), 3, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            headerTitle = "التاريخ  :   ";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 3, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            string title = infoReport[1];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_16_UNDERLINE.ToString()], title, 17, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            #endregion


            cell = new PdfPCell(headerTbl);
            cell.Colspan = (29);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            parent.AddCell(cell);

            #endregion

            #region HeaderTable
            PdfPTable dataTbl = new PdfPTable(29);
            dataTbl.DefaultCell.Padding = 10;
            dataTbl.WidthPercentage = 98;
            dataTbl.DefaultCell.BorderWidth = 0;
            dataTbl.HorizontalAlignment = Element.ALIGN_CENTER;
            dataTbl.HeaderRows = 1;
            dataTbl.SplitLate = false;


            headerTitle = "الفرع";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "تاريخ التنبيه";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "تاريخ الانتهاء";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "جهة الاصدار";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "رقم الوثيقة";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "إسم الوثيقة";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            #endregion

            #region body

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["الفرع"].ToString()), 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["تاريخ التنبيه"].ToString()), 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], ds.Tables[0].Rows[x]["تاريخ الانتهاء"].ToString(), 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    var MND_START_DATE = ds.Tables[0].Rows[x]["جهة الاصدار"].ToString();
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], MND_START_DATE, 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    var MND_END_DATE = ds.Tables[0].Rows[x]["رقم الوثيقة"].ToString();
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], MND_END_DATE, 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["إسم الوثيقة"].ToString()), 4, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);
                }
            }
            else
            {
                var titlenodata = "لا توجد نتائج ";
                var callNodata = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_BOLD_Red.ToString()], titlenodata, 29, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                dataTbl.AddCell(callNodata);
            }

            cell = new PdfPCell(dataTbl);
            cell.Colspan = (29);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            parent.AddCell(cell);
            #endregion

            doc.Add(parent);
            doc.Close();
            return memoryStream.GetBuffer();
        }
        //4 - DataOfficialDocsExpiredReport
        public static byte[] DataOfficialDocsExpiredReport(string[] infoReport, string[] columnHeader, DataTable listData)
        {
            bool isFooter = true;//send from user
            string printType = "2";//send from user 0=> طباعه افقية 
            //1=>طباعه راسية
            string footerData = "مؤسسة بياناتك لتقنية المعلومات العنوان : المملكة العربية السعودية - المدينة المنورة- تليفون : 0503326610 ";//get from data base ----Not fixed
            Document doc = new Document(PageSize.A4, 10, 10, 30, 44);
            var memoryStream = PdfBase.OpenDocument(doc, "1", true, "", isFooter, footerData);//طباعه افقية
            var headerTitle = "";
            #region data
            doc.Open();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds.Tables.Add(dt);
            dt.Clear();

            for (int i = 0; i <= columnHeader.Length - 1; i++)
            {
                dt.Columns.Add(columnHeader[i]);
            }

            for (int j = 0; j <= listData.Rows.Count - 1; j++)
            {
                DataRow _ravi = dt.NewRow();

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    _ravi[i] = listData.Rows[j][i];
                }
                dt.Rows.Add(_ravi);
            }

            #endregion

            PdfPCell cell = null;
            PdfPTable parent = new PdfPTable(29);
            parent.DefaultCell.Padding = 10;
            parent.WidthPercentage = 98;
            parent.DefaultCell.BorderWidth = 0;
            parent.HorizontalAlignment = Element.ALIGN_CENTER;
            parent.HeaderRows = 1;

            #region HeaderReport
            PdfPTable headerTbl = new PdfPTable(29);
            headerTbl.DefaultCell.Padding = 10;
            headerTbl.WidthPercentage = 98;
            headerTbl.DefaultCell.BorderWidth = 0;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            #region FirstRow

            if (infoReport[2] != "")
            {
                string imageFilePathLogo = PdfBase.getReportImagePath(infoReport[2]);

                if (imageFilePathLogo != "")
                {
                    Image headerJpg = Image.GetInstance(imageFilePathLogo); //infoReport[1];

                    cell = PdfBase.drawImageCell(headerJpg, 90F, 80F, 20, 2, 230, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
                else
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 20, 2, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
            }

            headerTitle = infoReport[0];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 15, 1, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            headerTitle = infoReport[3];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 14, 1, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            #endregion

            #region secondRow

            DateTime thisDay = DateTime.Today;
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], thisDay.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), 3, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            headerTitle = "التاريخ  :   ";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 3, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            string title = infoReport[1];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_16_UNDERLINE.ToString()], title, 17, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            #endregion


            cell = new PdfPCell(headerTbl);
            cell.Colspan = (29);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            parent.AddCell(cell);

            #endregion

            #region HeaderTable
            PdfPTable dataTbl = new PdfPTable(29);
            dataTbl.DefaultCell.Padding = 10;
            dataTbl.WidthPercentage = 98;
            dataTbl.DefaultCell.BorderWidth = 0;
            dataTbl.HorizontalAlignment = Element.ALIGN_CENTER;
            dataTbl.HeaderRows = 1;
            dataTbl.SplitLate = false;


            headerTitle = "الفرع";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 6, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "تاريخ الانتهاء";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 6, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "جهة الاصدار";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 6, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "رقم الوثيقة";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 6, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "إسم الوثيقة";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            #endregion

            #region body

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["الفرع"].ToString()), 6, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], ds.Tables[0].Rows[x]["تاريخ الانتهاء"].ToString(), 6, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    var MND_START_DATE = ds.Tables[0].Rows[x]["جهة الاصدار"].ToString();
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], MND_START_DATE, 6, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    var MND_END_DATE = ds.Tables[0].Rows[x]["رقم الوثيقة"].ToString();
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], MND_END_DATE, 6, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["إسم الوثيقة"].ToString()), 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);
                }
            }
            else
            {
                var titlenodata = "لا توجد نتائج ";
                var callNodata = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_BOLD_Red.ToString()], titlenodata, 29, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                dataTbl.AddCell(callNodata);
            }

            cell = new PdfPCell(dataTbl);
            cell.Colspan = (29);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            parent.AddCell(cell);
            #endregion

            doc.Add(parent);
            doc.Close();
            return memoryStream.GetBuffer();
        }
        //5 - DataEmpContractsAboutToExpire
        public static byte[] DataEmpContractsAboutToExpire(string[] infoReport, string[] columnHeader, DataTable listData)
        {
            bool isFooter = true;//send from user
            string printType = "2";//send from user 0=> طباعه افقية 
            //1=>طباعه راسية
            string footerData = "مؤسسة بياناتك لتقنية المعلومات العنوان : المملكة العربية السعودية - المدينة المنورة- تليفون : 0503326610  ";//get from data base ----Not fixed
            Document doc = new Document(PageSize.A4, 10, 10, 30, 44);
            var memoryStream = PdfBase.OpenDocument(doc, "1", true, "", isFooter, footerData);//طباعه افقية
            var headerTitle = "";
            #region data
            doc.Open();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds.Tables.Add(dt);
            dt.Clear();

            for (int i = 0; i <= columnHeader.Length - 1; i++)
            {
                dt.Columns.Add(columnHeader[i]);
            }

            for (int j = 0; j <= listData.Rows.Count - 1; j++)
            {
                DataRow _ravi = dt.NewRow();

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    _ravi[i] = listData.Rows[j][i];
                }
                dt.Rows.Add(_ravi);
            }

            #endregion

            PdfPCell cell = null;
            PdfPTable parent = new PdfPTable(29);
            parent.DefaultCell.Padding = 10;
            parent.WidthPercentage = 98;
            parent.DefaultCell.BorderWidth = 0;
            parent.HorizontalAlignment = Element.ALIGN_CENTER;
            parent.HeaderRows = 1;

            #region HeaderReport
            PdfPTable headerTbl = new PdfPTable(29);
            headerTbl.DefaultCell.Padding = 10;
            headerTbl.WidthPercentage = 98;
            headerTbl.DefaultCell.BorderWidth = 0;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;
            #region FirstRow

            if (infoReport[2] != "")
            {
                string imageFilePathLogo = PdfBase.getReportImagePath(infoReport[2]);
                if (imageFilePathLogo != "")
                {
                    Image headerJpg = Image.GetInstance(imageFilePathLogo); //infoReport[1];

                    cell = PdfBase.drawImageCell(headerJpg, 90F, 80F, 19, 2, 230, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
                else
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 20, 2, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
            }

            headerTitle = infoReport[0];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 15, 1, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            headerTitle = infoReport[3];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 14, 1, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            #endregion

            #region secondRow

            DateTime thisDay = DateTime.Today;
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], thisDay.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), 3, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            headerTitle = "التاريخ  :   ";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 3, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            string title = infoReport[1];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_16_UNDERLINE.ToString()], title, 17, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            #endregion


            cell = new PdfPCell(headerTbl);
            cell.Colspan = (29);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            parent.AddCell(cell);

            #endregion

            #region HeaderTable
            PdfPTable dataTbl = new PdfPTable(29);
            dataTbl.DefaultCell.Padding = 10;
            dataTbl.WidthPercentage = 98;
            dataTbl.DefaultCell.BorderWidth = 0;
            dataTbl.HorizontalAlignment = Element.ALIGN_CENTER;
            dataTbl.HeaderRows = 1;
            dataTbl.SplitLate = false;


            headerTitle = "تاريخ الانتهاء";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "الفرع";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "القسم";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "الجنسية";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "إسم الموظف";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "رقم العقد";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            #endregion

            #region body

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["تاريخ الانتهاء"].ToString()), 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["الفرع"].ToString()), 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], ds.Tables[0].Rows[x]["القسم"].ToString(), 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    var MND_START_DATE = ds.Tables[0].Rows[x]["الجنسية"].ToString();
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], MND_START_DATE, 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    var MND_END_DATE = ds.Tables[0].Rows[x]["إسم الموظف"].ToString();
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], MND_END_DATE, 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["رقم العقد"].ToString()), 4, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);
                }
            }
            else
            {
                var titlenodata = "لا توجد نتائج ";
                var callNodata = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_BOLD_Red.ToString()], titlenodata, 29, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                dataTbl.AddCell(callNodata);
            }

            cell = new PdfPCell(dataTbl);
            cell.Colspan = (29);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            parent.AddCell(cell);
            #endregion

            doc.Add(parent);
            doc.Close();
            return memoryStream.GetBuffer();
        }
        //8 - DataDeservedServices
        public static byte[] DataDeservedServices(string[] infoReport, string[] columnHeader, DataTable listData)
        {
            bool isFooter = true;//send from user
            string printType = "2";//send from user 0=> طباعه افقية 
            //1=>طباعه راسية
            string footerData = "مؤسسة بياناتك لتقنية المعلومات العنوان : المملكة العربية السعودية - المدينة المنورة- تليفون : 0503326610  ";//get from data base ----Not fixed
            Document doc = new Document(PageSize.A4, 10, 10, 30, 44);
            var memoryStream = PdfBase.OpenDocument(doc, "1", true, "", isFooter, footerData);//طباعه افقية
            var headerTitle = "";
            #region data
            doc.Open();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds.Tables.Add(dt);
            dt.Clear();

            for (int i = 0; i <= columnHeader.Length - 1; i++)
            {
                dt.Columns.Add(columnHeader[i]);
            }

            for (int j = 0; j <= listData.Rows.Count - 1; j++)
            {
                DataRow _ravi = dt.NewRow();

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    _ravi[i] = listData.Rows[j][i];
                }
                dt.Rows.Add(_ravi);
            }

            #endregion

            PdfPCell cell = null;
            PdfPTable parent = new PdfPTable(29);
            parent.DefaultCell.Padding = 10;
            parent.WidthPercentage = 98;
            parent.DefaultCell.BorderWidth = 0;
            parent.HorizontalAlignment = Element.ALIGN_CENTER;
            parent.HeaderRows = 1;

            #region HeaderReport
            PdfPTable headerTbl = new PdfPTable(29);
            headerTbl.DefaultCell.Padding = 10;
            headerTbl.WidthPercentage = 98;
            headerTbl.DefaultCell.BorderWidth = 0;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            #region FirstRow

            if (infoReport[2] != "")
            {
                string imageFilePathLogo = PdfBase.getReportImagePath(infoReport[2]);

                if (imageFilePathLogo != "")
                {
                    Image headerJpg = Image.GetInstance(imageFilePathLogo); //infoReport[1];

                    cell = PdfBase.drawImageCell(headerJpg, 90F, 80F, 20, 2, 230, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
                else
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 20, 2, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
            }

            headerTitle = infoReport[0];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 15, 1, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            headerTitle = infoReport[3];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 14, 1, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            #endregion

            #region secondRow

            DateTime thisDay = DateTime.Today;
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], thisDay.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), 3, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            headerTitle = "التاريخ  :   ";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 3, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            string title = infoReport[1];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_16_UNDERLINE.ToString()], title, 17, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            #endregion


            cell = new PdfPCell(headerTbl);
            cell.Colspan = (29);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            parent.AddCell(cell);

            #endregion

            #region HeaderTable
            PdfPTable dataTbl = new PdfPTable(29);
            dataTbl.DefaultCell.Padding = 10;
            dataTbl.WidthPercentage = 98;
            dataTbl.DefaultCell.BorderWidth = 0;
            dataTbl.HorizontalAlignment = Element.ALIGN_CENTER;
            dataTbl.HeaderRows = 1;
            dataTbl.SplitLate = false;

            headerTitle = "الفرع";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 6, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "تاريخ الاستحقاق";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 6, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "جهة الاصدار";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 6, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "رقم الحساب";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 6, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "رقم الخدمة";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            #endregion

            #region body

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["الفرع"].ToString()), 6, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], ds.Tables[0].Rows[x]["تاريخ الاستحقاق"].ToString(), 6, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    var MND_START_DATE = ds.Tables[0].Rows[x]["جهة الاصدار"].ToString();
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], MND_START_DATE, 6, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    var MND_END_DATE = ds.Tables[0].Rows[x]["رقم الحساب"].ToString();
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], MND_END_DATE, 6, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["رقم الخدمة"].ToString()), 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);
                }
            }
            else
            {
                var titlenodata = "لا توجد نتائج ";
                var callNodata = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_BOLD_Red.ToString()], titlenodata, 29, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                dataTbl.AddCell(callNodata);
            }

            cell = new PdfPCell(dataTbl);
            cell.Colspan = (29);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            parent.AddCell(cell);
            #endregion

            doc.Add(parent);
            doc.Close();
            return memoryStream.GetBuffer();
        }


        //8 - DataDoneTask
        public static byte[] DataDoneTask(string[] infoReport, string[] columnHeader, DataTable listData)
        {
            bool isFooter = true;//send from user
            string printType = "2";//send from user 0=> طباعه افقية 
            //1=>طباعه راسية
            string footerData = "مؤسسة بياناتك لتقنية المعلومات العنوان : المملكة العربية السعودية - المدينة المنورة- تليفون : 0503326610  ";//get from data base ----Not fixed
            Document doc = new Document(PageSize.A4, 10, 10, 30, 44);
            var memoryStream = PdfBase.OpenDocument(doc, "1", true, "", isFooter, footerData);//طباعه افقية
            var headerTitle = "";
            #region data
            doc.Open();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds.Tables.Add(dt);
            dt.Clear();

            for (int i = 0; i <= columnHeader.Length - 1; i++)
            {
                dt.Columns.Add(columnHeader[i]);
            }

            for (int j = 0; j <= listData.Rows.Count - 1; j++)
            {
                DataRow _ravi = dt.NewRow();

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    _ravi[i] = listData.Rows[j][i];
                }
                dt.Rows.Add(_ravi);
            }

            #endregion

            PdfPCell cell = null;
            PdfPTable parent = new PdfPTable(29);
            parent.DefaultCell.Padding = 10;
            parent.WidthPercentage = 98;
            parent.DefaultCell.BorderWidth = 0;
            parent.HorizontalAlignment = Element.ALIGN_CENTER;
            parent.HeaderRows = 1;

            #region HeaderReport
            PdfPTable headerTbl = new PdfPTable(29);
            headerTbl.DefaultCell.Padding = 10;
            headerTbl.WidthPercentage = 98;
            headerTbl.DefaultCell.BorderWidth = 0;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            #region FirstRow

            if (infoReport[2] != "")
            {
                string imageFilePathLogo = PdfBase.getReportImagePath(infoReport[2]);

                if (imageFilePathLogo != "")
                {
                    Image headerJpg = Image.GetInstance(imageFilePathLogo); //infoReport[1];

                    cell = PdfBase.drawImageCell(headerJpg, 90F, 80F, 20, 2, 230, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
                else
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 20, 2, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
            }

            headerTitle = infoReport[0];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 15, 1, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            headerTitle = infoReport[3];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 14, 1, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            #endregion

            #region secondRow

            DateTime thisDay = DateTime.Now.Date;
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], thisDay.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), 3, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            headerTitle = "التاريخ  :   ";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 3, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            string title = infoReport[1];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_16_UNDERLINE.ToString()], title, 17, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            #endregion


            cell = new PdfPCell(headerTbl);
            cell.Colspan = (29);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            parent.AddCell(cell);

            #endregion

            #region HeaderTable
            PdfPTable dataTbl = new PdfPTable(29);
            dataTbl.DefaultCell.Padding = 10;
            dataTbl.WidthPercentage = 98;
            dataTbl.DefaultCell.BorderWidth = 0;
            dataTbl.HorizontalAlignment = Element.ALIGN_CENTER;
            dataTbl.HeaderRows = 1;
            dataTbl.SplitLate = false;

            headerTitle = "التكلفة";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "إلى تاريخ";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "العميل";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "رقم المشروع";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "المدة";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "المهمة";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            #endregion

            #region body

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["التكلفة"].ToString()), 4, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], ds.Tables[0].Rows[x]["إلى تاريخ"].ToString(), 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    var MND_START_DATE = ds.Tables[0].Rows[x]["العميل"].ToString();
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], MND_START_DATE, 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    var MND_END_DATE = ds.Tables[0].Rows[x]["رقم المشروع"].ToString();
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], MND_END_DATE, 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["المدة"].ToString()), 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["المهمة"].ToString()), 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);
                }
            }
            else
            {
                var titlenodata = "لا توجد نتائج ";
                var callNodata = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_BOLD_Red.ToString()], titlenodata, 29, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                dataTbl.AddCell(callNodata);
            }

            cell = new PdfPCell(dataTbl);
            cell.Colspan = (29);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            parent.AddCell(cell);
            #endregion

            doc.Add(parent);
            doc.Close();
            return memoryStream.GetBuffer();
        }

        //8 - DataDoneTask
        public static byte[] DataDoneWorkOrder(string[] infoReport, string[] columnHeader, DataTable listData)
        {
            bool isFooter = true;//send from user
            string printType = "2";//send from user 0=> طباعه افقية 
            //1=>طباعه راسية
            string footerData = "مؤسسة بياناتك لتقنية المعلومات العنوان : المملكة العربية السعودية - المدينة المنورة- تليفون : 0503326610  ";//get from data base ----Not fixed
            Document doc = new Document(PageSize.A4, 10, 10, 30, 44);
            var memoryStream = PdfBase.OpenDocument(doc, "1", true, "", isFooter, footerData);//طباعه افقية
            var headerTitle = "";
            #region data
            doc.Open();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds.Tables.Add(dt);
            dt.Clear();

            for (int i = 0; i <= columnHeader.Length - 1; i++)
            {
                dt.Columns.Add(columnHeader[i]);
            }

            for (int j = 0; j <= listData.Rows.Count - 1; j++)
            {
                DataRow _ravi = dt.NewRow();

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    _ravi[i] = listData.Rows[j][i];
                }
                dt.Rows.Add(_ravi);
            }

            #endregion

            PdfPCell cell = null;
            PdfPTable parent = new PdfPTable(29);
            parent.DefaultCell.Padding = 10;
            parent.WidthPercentage = 98;
            parent.DefaultCell.BorderWidth = 0;
            parent.HorizontalAlignment = Element.ALIGN_CENTER;
            parent.HeaderRows = 1;

            #region HeaderReport
            PdfPTable headerTbl = new PdfPTable(29);
            headerTbl.DefaultCell.Padding = 10;
            headerTbl.WidthPercentage = 98;
            headerTbl.DefaultCell.BorderWidth = 0;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            #region FirstRow

            if (infoReport[2] != "")
            {
                string imageFilePathLogo = PdfBase.getReportImagePath(infoReport[2]);

                if (imageFilePathLogo != "")
                {
                    Image headerJpg = Image.GetInstance(imageFilePathLogo); //infoReport[1];

                    cell = PdfBase.drawImageCell(headerJpg, 90F, 80F, 20, 2, 230, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
                else
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 20, 2, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
            }

            headerTitle = infoReport[0];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 15, 1, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            headerTitle = infoReport[3];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 14, 1, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            #endregion

            #region secondRow

            DateTime thisDay = DateTime.Now.Date;
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], thisDay.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), 3, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            headerTitle = "التاريخ  :   ";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 3, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            string title = infoReport[1];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_16_UNDERLINE.ToString()], title, 17, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            #endregion


            cell = new PdfPCell(headerTbl);
            cell.Colspan = (29);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            parent.AddCell(cell);

            #endregion

            #region HeaderTable
            PdfPTable dataTbl = new PdfPTable(29);
            dataTbl.DefaultCell.Padding = 10;
            dataTbl.WidthPercentage = 98;
            dataTbl.DefaultCell.BorderWidth = 0;
            dataTbl.HorizontalAlignment = Element.ALIGN_CENTER;
            dataTbl.HeaderRows = 1;
            dataTbl.SplitLate = false;

            headerTitle = "تاريخ الانجاز";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "المدة";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "العميل";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "الأمر بواسطة";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "المطلوب";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "رقم الطلب";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            #endregion

            #region body

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["تاريخ الانجاز"].ToString()), 4, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], ds.Tables[0].Rows[x]["المدة"].ToString(), 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    var MND_START_DATE = ds.Tables[0].Rows[x]["العميل"].ToString();
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], MND_START_DATE, 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    var MND_END_DATE = ds.Tables[0].Rows[x]["الأمر بواسطة"].ToString();
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], MND_END_DATE, 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["المطلوب"].ToString()), 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["رقم الطلب"].ToString()), 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);
                }
            }
            else
            {
                var titlenodata = "لا توجد نتائج ";
                var callNodata = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_BOLD_Red.ToString()], titlenodata, 29, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                dataTbl.AddCell(callNodata);
            }

            cell = new PdfPCell(dataTbl);
            cell.Colspan = (29);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            parent.AddCell(cell);
            #endregion

            doc.Add(parent);
            doc.Close();
            return memoryStream.GetBuffer();
        }
        //8 - DataDoneTask
        public static byte[] DataRunningTasks(string[] infoReport, string[] columnHeader, DataTable listData)
        {
            bool isFooter = true;//send from user
            string printType = "2";//send from user 0=> طباعه افقية 
            //1=>طباعه راسية
            string footerData = "مؤسسة بياناتك لتقنية المعلومات العنوان : المملكة العربية السعودية - المدينة المنورة- تليفون : 0503326610  ";//get from data base ----Not fixed
            Document doc = new Document(PageSize.A4, 10, 10, 30, 44);
            var memoryStream = PdfBase.OpenDocument(doc, "1", true, "", isFooter, footerData);//طباعه افقية
            var headerTitle = "";
            #region data
            doc.Open();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds.Tables.Add(dt);
            dt.Clear();

            for (int i = 0; i <= columnHeader.Length - 1; i++)
            {
                dt.Columns.Add(columnHeader[i]);
            }

            for (int j = 0; j <= listData.Rows.Count - 1; j++)
            {
                DataRow _ravi = dt.NewRow();

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    _ravi[i] = listData.Rows[j][i];
                }
                dt.Rows.Add(_ravi);
            }

            #endregion

            PdfPCell cell = null;
            PdfPTable parent = new PdfPTable(29);
            parent.DefaultCell.Padding = 10;
            parent.WidthPercentage = 98;
            parent.DefaultCell.BorderWidth = 0;
            parent.HorizontalAlignment = Element.ALIGN_CENTER;
            parent.HeaderRows = 1;

            #region HeaderReport
            PdfPTable headerTbl = new PdfPTable(29);
            headerTbl.DefaultCell.Padding = 10;
            headerTbl.WidthPercentage = 98;
            headerTbl.DefaultCell.BorderWidth = 0;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            #region FirstRow

            if (infoReport[2] != "")
            {
                string imageFilePathLogo = PdfBase.getReportImagePath(infoReport[2]);

                if (imageFilePathLogo != "")
                {
                    Image headerJpg = Image.GetInstance(imageFilePathLogo); //infoReport[1];

                    cell = PdfBase.drawImageCell(headerJpg, 90F, 80F, 20, 2, 230, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
                else
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 20, 2, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
            }

            headerTitle = infoReport[0];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 15, 1, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            headerTitle = infoReport[3];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 14, 1, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            #endregion

            #region secondRow

            DateTime thisDay = DateTime.Now.Date;
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], thisDay.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), 3, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            headerTitle = "التاريخ  :   ";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 3, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            string title = infoReport[1];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_16_UNDERLINE.ToString()], title, 17, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            #endregion


            cell = new PdfPCell(headerTbl);
            cell.Colspan = (29);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            parent.AddCell(cell);

            #endregion

            #region HeaderTable
            PdfPTable dataTbl = new PdfPTable(29);
            dataTbl.DefaultCell.Padding = 10;
            dataTbl.WidthPercentage = 98;
            dataTbl.DefaultCell.BorderWidth = 0;
            dataTbl.HorizontalAlignment = Element.ALIGN_CENTER;
            dataTbl.HeaderRows = 1;
            dataTbl.SplitLate = false;

            headerTitle = "الأهمية";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "من تاريخ";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "العميل";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "رقم المشروع";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "المدة";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "المهمة";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            #endregion

            #region body

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["المهمة"].ToString()), 4, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], ds.Tables[0].Rows[x]["من تاريخ"].ToString(), 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    var MND_START_DATE = ds.Tables[0].Rows[x]["العميل"].ToString();
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], MND_START_DATE, 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    var MND_END_DATE = ds.Tables[0].Rows[x]["رقم المشروع"].ToString();
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], MND_END_DATE, 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["المدة"].ToString()), 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["المهمة"].ToString()), 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);
                }
            }
            else
            {
                var titlenodata = "لا توجد نتائج ";
                var callNodata = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_BOLD_Red.ToString()], titlenodata, 29, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                dataTbl.AddCell(callNodata);
            }

            cell = new PdfPCell(dataTbl);
            cell.Colspan = (29);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            parent.AddCell(cell);
            #endregion

            doc.Add(parent);
            doc.Close();
            return memoryStream.GetBuffer();
        }
        //8 - DataDoneTask
        public static byte[] DataLateTasks(string[] infoReport, string[] columnHeader, DataTable listData)
        {
            bool isFooter = true;//send from user
            string printType = "2";//send from user 0=> طباعه افقية 
            //1=>طباعه راسية
            string footerData = "مؤسسة بياناتك لتقنية المعلومات العنوان : المملكة العربية السعودية - المدينة المنورة- تليفون : 0503326610  ";//get from data base ----Not fixed
            Document doc = new Document(PageSize.A4, 10, 10, 30, 44);
            var memoryStream = PdfBase.OpenDocument(doc, "1", true, "", isFooter, footerData);//طباعه افقية
            var headerTitle = "";
            #region data
            doc.Open();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds.Tables.Add(dt);
            dt.Clear();

            for (int i = 0; i <= columnHeader.Length - 1; i++)
            {
                dt.Columns.Add(columnHeader[i]);
            }

            for (int j = 0; j <= listData.Rows.Count - 1; j++)
            {
                DataRow _ravi = dt.NewRow();

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    _ravi[i] = listData.Rows[j][i];
                }
                dt.Rows.Add(_ravi);
            }

            #endregion

            PdfPCell cell = null;
            PdfPTable parent = new PdfPTable(29);
            parent.DefaultCell.Padding = 10;
            parent.WidthPercentage = 98;
            parent.DefaultCell.BorderWidth = 0;
            parent.HorizontalAlignment = Element.ALIGN_CENTER;
            parent.HeaderRows = 1;

            #region HeaderReport
            PdfPTable headerTbl = new PdfPTable(29);
            headerTbl.DefaultCell.Padding = 10;
            headerTbl.WidthPercentage = 98;
            headerTbl.DefaultCell.BorderWidth = 0;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            #region FirstRow

            if (infoReport[2] != "")
            {
                string imageFilePathLogo = PdfBase.getReportImagePath(infoReport[2]);

                if (imageFilePathLogo != "")
                {
                    Image headerJpg = Image.GetInstance(imageFilePathLogo); //infoReport[1];

                    cell = PdfBase.drawImageCell(headerJpg, 90F, 80F, 20, 2, 230, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
                else
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 20, 2, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
            }

            headerTitle = infoReport[0];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 15, 1, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            headerTitle = infoReport[3];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 14, 1, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            #endregion

            #region secondRow

            DateTime thisDay = DateTime.Now.Date;
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], thisDay.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), 3, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            headerTitle = "التاريخ  :   ";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 3, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            string title = infoReport[1];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_16_UNDERLINE.ToString()], title, 17, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            #endregion


            cell = new PdfPCell(headerTbl);
            cell.Colspan = (29);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            parent.AddCell(cell);

            #endregion

            #region HeaderTable
            PdfPTable dataTbl = new PdfPTable(29);
            dataTbl.DefaultCell.Padding = 10;
            dataTbl.WidthPercentage = 98;
            dataTbl.DefaultCell.BorderWidth = 0;
            dataTbl.HorizontalAlignment = Element.ALIGN_CENTER;
            dataTbl.HeaderRows = 1;
            dataTbl.SplitLate = false;

            headerTitle = "مدة التأخير";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "من تاريخ";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "العميل";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "رقم المشروع";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "المدة";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "المهمة";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            #endregion

            #region body

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["مدة التأخير"].ToString()), 4, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], ds.Tables[0].Rows[x]["من تاريخ"].ToString(), 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    var MND_START_DATE = ds.Tables[0].Rows[x]["العميل"].ToString();
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], MND_START_DATE, 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    var MND_END_DATE = ds.Tables[0].Rows[x]["رقم المشروع"].ToString();
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], MND_END_DATE, 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["المدة"].ToString()), 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["المهمة"].ToString()), 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);
                }
            }
            else
            {
                var titlenodata = "لا توجد نتائج ";
                var callNodata = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_BOLD_Red.ToString()], titlenodata, 29, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                dataTbl.AddCell(callNodata);
            }

            cell = new PdfPCell(dataTbl);
            cell.Colspan = (29);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            parent.AddCell(cell);
            #endregion

            doc.Add(parent);
            doc.Close();
            return memoryStream.GetBuffer();
        }
        //8 - DataDoneTask
        public static byte[] DatarunningWorkOrder(string[] infoReport, string[] columnHeader, DataTable listData)
        {
            bool isFooter = true;//send from user
            string printType = "2";//send from user 0=> طباعه افقية 
            //1=>طباعه راسية
            string footerData = "مؤسسة بياناتك لتقنية المعلومات العنوان : المملكة العربية السعودية - المدينة المنورة- تليفون : 0503326610  ";//get from data base ----Not fixed
            Document doc = new Document(PageSize.A4, 10, 10, 30, 44);
            var memoryStream = PdfBase.OpenDocument(doc, "1", true, "", isFooter, footerData);//طباعه افقية
            var headerTitle = "";
            #region data
            doc.Open();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds.Tables.Add(dt);
            dt.Clear();

            for (int i = 0; i <= columnHeader.Length - 1; i++)
            {
                dt.Columns.Add(columnHeader[i]);
            }

            for (int j = 0; j <= listData.Rows.Count - 1; j++)
            {
                DataRow _ravi = dt.NewRow();

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    _ravi[i] = listData.Rows[j][i];
                }
                dt.Rows.Add(_ravi);
            }

            #endregion

            PdfPCell cell = null;
            PdfPTable parent = new PdfPTable(29);
            parent.DefaultCell.Padding = 10;
            parent.WidthPercentage = 98;
            parent.DefaultCell.BorderWidth = 0;
            parent.HorizontalAlignment = Element.ALIGN_CENTER;
            parent.HeaderRows = 1;

            #region HeaderReport
            PdfPTable headerTbl = new PdfPTable(29);
            headerTbl.DefaultCell.Padding = 10;
            headerTbl.WidthPercentage = 98;
            headerTbl.DefaultCell.BorderWidth = 0;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            #region FirstRow

            if (infoReport[2] != "")
            {
                string imageFilePathLogo = PdfBase.getReportImagePath(infoReport[2]);

                if (imageFilePathLogo != "")
                {
                    Image headerJpg = Image.GetInstance(imageFilePathLogo); //infoReport[1];

                    cell = PdfBase.drawImageCell(headerJpg, 90F, 80F, 20, 2, 230, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
                else
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 20, 2, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
            }

            headerTitle = infoReport[0];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 15, 1, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            headerTitle = infoReport[3];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 14, 1, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            #endregion

            #region secondRow

            DateTime thisDay = DateTime.Now.Date;
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], thisDay.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), 3, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            headerTitle = "التاريخ  :   ";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 3, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            string title = infoReport[1];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_16_UNDERLINE.ToString()], title, 17, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            #endregion


            cell = new PdfPCell(headerTbl);
            cell.Colspan = (29);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            parent.AddCell(cell);

            #endregion

            #region HeaderTable
            PdfPTable dataTbl = new PdfPTable(29);
            dataTbl.DefaultCell.Padding = 10;
            dataTbl.WidthPercentage = 98;
            dataTbl.DefaultCell.BorderWidth = 0;
            dataTbl.HorizontalAlignment = Element.ALIGN_CENTER;
            dataTbl.HeaderRows = 1;
            dataTbl.SplitLate = false;

            headerTitle = "المدة المتبقية";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "المدة";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "العميل";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "الأمر بواسطة";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "المطلوب";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "رقم الطلب";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            #endregion

            #region body

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["المدة المتبقية"].ToString()), 4, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], ds.Tables[0].Rows[x]["المدة"].ToString(), 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    var MND_START_DATE = ds.Tables[0].Rows[x]["العميل"].ToString();
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], MND_START_DATE, 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    var MND_END_DATE = ds.Tables[0].Rows[x]["الأمر بواسطة"].ToString();
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], MND_END_DATE, 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["المطلوب"].ToString()), 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["رقم الطلب"].ToString()), 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);
                }
            }
            else
            {
                var titlenodata = "لا توجد نتائج ";
                var callNodata = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_BOLD_Red.ToString()], titlenodata, 29, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                dataTbl.AddCell(callNodata);
            }

            cell = new PdfPCell(dataTbl);
            cell.Colspan = (29);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            parent.AddCell(cell);
            #endregion

            doc.Add(parent);
            doc.Close();
            return memoryStream.GetBuffer();
        }


        public static byte[] DataArchive(string[] infoReport, string[] columnHeader, DataTable listData, string[] columnHeaders)
        {
            bool isFooter = true;//send from user
            string printType = "2";//send from user 0=> طباعه افقية 
            //1=>طباعه راسية
            string footerData = "مؤسسة بياناتك لتقنية المعلومات العنوان : المملكة العربية السعودية - المدينة المنورة- تليفون : 0503326610  ";//get from data base ----Not fixed
            Document doc = new Document(PageSize.A4, 10, 10, 30, 44);
            var memoryStream = PdfBase.OpenDocument(doc, "1", true, "", isFooter, footerData);//طباعه افقية
            var headerTitle = "";
            #region data
            doc.Open();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds.Tables.Add(dt);
            dt.Clear();

            for (int i = 0; i <= columnHeader.Length - 1; i++)
            {
                dt.Columns.Add(columnHeader[i]);
            }

            for (int j = 0; j <= listData.Rows.Count - 1; j++)
            {
                DataRow _ravi = dt.NewRow();

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    _ravi[i] = listData.Rows[j][columnHeaders[i]];
                }
                dt.Rows.Add(_ravi);
            }

            #endregion

            PdfPCell cell = null;
            PdfPTable parent = new PdfPTable(29);
            parent.DefaultCell.Padding = 10;
            parent.WidthPercentage = 98;
            parent.DefaultCell.BorderWidth = 0;
            parent.HorizontalAlignment = Element.ALIGN_CENTER;
            parent.HeaderRows = 1;

            #region HeaderReport
            PdfPTable headerTbl = new PdfPTable(29);
            headerTbl.DefaultCell.Padding = 10;
            headerTbl.WidthPercentage = 98;
            headerTbl.DefaultCell.BorderWidth = 0;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            #region FirstRow

            if (infoReport[2] != "")
            {
                string imageFilePathLogo = PdfBase.getReportImagePath(infoReport[2]);

                if (imageFilePathLogo != "")
                {
                    Image headerJpg = Image.GetInstance(imageFilePathLogo); //infoReport[1];

                    cell = PdfBase.drawImageCell(headerJpg, 90F, 80F, 20, 2, 230, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
                else
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 20, 2, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
            }

            headerTitle = infoReport[0];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 15, 1, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            headerTitle = infoReport[3];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 14, 1, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            #endregion

            #region secondRow

            DateTime thisDay = DateTime.Now.Date;
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], thisDay.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), 3, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            headerTitle = "التاريخ  :   ";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 3, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            string title = infoReport[1];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_16_UNDERLINE.ToString()], title, 17, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            #endregion


            cell = new PdfPCell(headerTbl);
            cell.Colspan = (29);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            parent.AddCell(cell);

            #endregion

            #region HeaderTable
            PdfPTable dataTbl = new PdfPTable(30);
            dataTbl.DefaultCell.Padding = 10;
            dataTbl.WidthPercentage = 98;
            dataTbl.DefaultCell.BorderWidth = 0;
            dataTbl.HorizontalAlignment = Element.ALIGN_CENTER;
            dataTbl.HeaderRows = 1;
            dataTbl.SplitLate = false;


            headerTitle = "المستخدم";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 3, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "مده المشروع";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 3, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "مدير المشروع";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 3, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "المنطقه";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 3, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "رقم العقد";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 3, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "نوع المشروع";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 3, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "العميل";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 3, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "رقم المشروع";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 3, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "الحاله";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 3, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "تاريخ الارشفه";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 3, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            #endregion

            #region body

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["المستخدم"].ToString()), 3, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], ds.Tables[0].Rows[x]["مده المشروع"].ToString(), 3, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["مدير المشروع"].ToString()), 3, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], ds.Tables[0].Rows[x]["المنطقه"].ToString(), 3, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["رقم العقد"].ToString()), 3, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], ds.Tables[0].Rows[x]["نوع المشروع"].ToString(), 3, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    var MND_START_DATE = ds.Tables[0].Rows[x]["العميل"].ToString();
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], MND_START_DATE, 3, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    var MND_END_DATE = ds.Tables[0].Rows[x]["رقم المشروع"].ToString();
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], MND_END_DATE, 3, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["الحاله"].ToString()), 3, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["تاريخ الارشفه"].ToString()), 3, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);
                }
            }
            else
            {
                var titlenodata = "لا توجد نتائج ";
                var callNodata = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_BOLD_Red.ToString()], titlenodata, 29, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                dataTbl.AddCell(callNodata);
            }

            cell = new PdfPCell(dataTbl);
            cell.Colspan = (29);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            parent.AddCell(cell);
            #endregion

            doc.Add(parent);
            doc.Close();
            return memoryStream.GetBuffer();
        }

        public static byte[] DataCustomer(string[] infoReport, string[] columnHeader, DataTable listData, string[] columnHeaders)
        {
            bool isFooter = true;//send from user
            string printType = "2";//send from user 0=> طباعه افقية 
            //1=>طباعه راسية
            string footerData = "مؤسسة بياناتك لتقنية المعلومات العنوان : المملكة العربية السعودية - المدينة المنورة- تليفون : 0503326610  ";//get from data base ----Not fixed
            Document doc = new Document(PageSize.A4, 10, 10, 30, 44);
            var memoryStream = PdfBase.OpenDocument(doc, "1", true, "", isFooter, footerData);//طباعه افقية
            var headerTitle = "";
            #region data
            doc.Open();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds.Tables.Add(dt);
            dt.Clear();

            for (int i = 0; i <= columnHeader.Length - 1; i++)
            {
                dt.Columns.Add(columnHeader[i]);
            }

            for (int j = 0; j <= listData.Rows.Count - 1; j++)
            {
                DataRow _ravi = dt.NewRow();

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    _ravi[i] = listData.Rows[j][columnHeaders[i]];
                }
                dt.Rows.Add(_ravi);
            }

            #endregion

            PdfPCell cell = null;
            PdfPTable parent = new PdfPTable(29);
            parent.DefaultCell.Padding = 10;
            parent.WidthPercentage = 98;
            parent.DefaultCell.BorderWidth = 0;
            parent.HorizontalAlignment = Element.ALIGN_CENTER;
            parent.HeaderRows = 1;

            #region HeaderReport
            PdfPTable headerTbl = new PdfPTable(29);
            headerTbl.DefaultCell.Padding = 10;
            headerTbl.WidthPercentage = 98;
            headerTbl.DefaultCell.BorderWidth = 0;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            #region FirstRow

            if (infoReport[2] != "")
            {
                string imageFilePathLogo = PdfBase.getReportImagePath(infoReport[2]);

                if (imageFilePathLogo != "")
                {
                    Image headerJpg = Image.GetInstance(imageFilePathLogo); //infoReport[1];

                    cell = PdfBase.drawImageCell(headerJpg, 90F, 80F, 20, 2, 230, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
                else
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 20, 2, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
            }

            headerTitle = infoReport[0];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 15, 1, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            headerTitle = infoReport[3];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 14, 1, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            #endregion

            #region secondRow

            DateTime thisDay = DateTime.Now.Date;
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], thisDay.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), 3, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            headerTitle = "التاريخ  :   ";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 3, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            string title = infoReport[1];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_16_UNDERLINE.ToString()], title, 17, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            #endregion


            cell = new PdfPCell(headerTbl);
            cell.Colspan = (29);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            parent.AddCell(cell);

            #endregion

            #region HeaderTable
            PdfPTable dataTbl = new PdfPTable(29);
            dataTbl.DefaultCell.Padding = 10;
            dataTbl.WidthPercentage = 98;
            dataTbl.DefaultCell.BorderWidth = 0;
            dataTbl.HorizontalAlignment = Element.ALIGN_CENTER;
            dataTbl.HeaderRows = 1;
            dataTbl.SplitLate = false;


            headerTitle = "رقم الحساب";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "رقم الجوال";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "رقم الهاتف";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "البريد الالكتروني";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "رقم الهوية";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "النوع";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "أسم العميل";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            #endregion

            #region body

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], ds.Tables[0].Rows[x]["رقم الحساب"].ToString(), 4, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["رقم الجوال"].ToString()), 4, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], ds.Tables[0].Rows[x]["رقم الهاتف"].ToString(), 4, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    var MND_START_DATE = ds.Tables[0].Rows[x]["البريد الالكتروني"].ToString();
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], MND_START_DATE, 4, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    var MND_END_DATE = ds.Tables[0].Rows[x]["رقم الهوية"].ToString();
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], MND_END_DATE, 4, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["النوع"].ToString()), 4, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["أسم العميل"].ToString()), 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);
                }
            }
            else
            {
                var titlenodata = "لا توجد نتائج ";
                var callNodata = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_BOLD_Red.ToString()], titlenodata, 29, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                dataTbl.AddCell(callNodata);
            }

            cell = new PdfPCell(dataTbl);
            cell.Colspan = (29);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            parent.AddCell(cell);
            #endregion

            doc.Add(parent);
            doc.Close();
            return memoryStream.GetBuffer();
        }

        public static byte[] DataProjectCustomer(string[] infoReport, string[] columnHeader, DataTable listData, string[] columnHeaders)
        {
            bool isFooter = true;//send from user
            string printType = "2";//send from user 0=> طباعه افقية 
            //1=>طباعه راسية
            string footerData = "مؤسسة بياناتك لتقنية المعلومات العنوان : المملكة العربية السعودية - المدينة المنورة- تليفون : 0503326610  ";//get from data base ----Not fixed
            Document doc = new Document(PageSize.A4, 10, 10, 30, 44);
            var memoryStream = PdfBase.OpenDocument(doc, "1", true, "", isFooter, footerData);//طباعه افقية
            var headerTitle = "";
            #region data
            doc.Open();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds.Tables.Add(dt);
            dt.Clear();

            for (int i = 0; i <= columnHeader.Length - 1; i++)
            {
                dt.Columns.Add(columnHeader[i]);
            }

            for (int j = 0; j <= listData.Rows.Count - 1; j++)
            {
                DataRow _ravi = dt.NewRow();

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    _ravi[i] = listData.Rows[j][columnHeaders[i]];
                }
                dt.Rows.Add(_ravi);
            }

            #endregion

            PdfPCell cell = null;
            PdfPTable parent = new PdfPTable(29);
            parent.DefaultCell.Padding = 10;
            parent.WidthPercentage = 98;
            parent.DefaultCell.BorderWidth = 0;
            parent.HorizontalAlignment = Element.ALIGN_CENTER;
            parent.HeaderRows = 1;

            #region HeaderReport
            PdfPTable headerTbl = new PdfPTable(29);
            headerTbl.DefaultCell.Padding = 10;
            headerTbl.WidthPercentage = 98;
            headerTbl.DefaultCell.BorderWidth = 0;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            #region FirstRow

            if (infoReport[2] != "")
            {
                string imageFilePathLogo = PdfBase.getReportImagePath(infoReport[2]);

                if (imageFilePathLogo != "")
                {
                    Image headerJpg = Image.GetInstance(imageFilePathLogo); //infoReport[1];

                    cell = PdfBase.drawImageCell(headerJpg, 90F, 80F, 20, 2, 230, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
                else
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 20, 2, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
            }

            headerTitle = infoReport[0];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 15, 1, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            headerTitle = infoReport[3];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 14, 1, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            #endregion

            #region secondRow

            DateTime thisDay = DateTime.Now.Date;
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], thisDay.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), 3, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            headerTitle = "التاريخ  :   ";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 3, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            string title = infoReport[1];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_16_UNDERLINE.ToString()], title, 17, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            #endregion


            cell = new PdfPCell(headerTbl);
            cell.Colspan = (29);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            parent.AddCell(cell);

            #endregion

            #region HeaderTable
            PdfPTable dataTbl = new PdfPTable(29);
            dataTbl.DefaultCell.Padding = 10;
            dataTbl.WidthPercentage = 98;
            dataTbl.DefaultCell.BorderWidth = 0;
            dataTbl.HorizontalAlignment = Element.ALIGN_CENTER;
            dataTbl.HeaderRows = 1;
            dataTbl.SplitLate = false;


            headerTitle = "وصف المشروع";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "مدة المشروع بالأيام";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "نسبة الانجاز";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "نوع المشروع الفرعى";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "نوع المشروع";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "رقم المشروع";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            #endregion

            #region body

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["وصف المشروع"].ToString()), 4, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], ds.Tables[0].Rows[x]["مدة المشروع بالأيام"].ToString(), 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    var MND_START_DATE = ds.Tables[0].Rows[x]["نسبة الانجاز"].ToString();
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], MND_START_DATE, 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    var MND_END_DATE = ds.Tables[0].Rows[x]["نوع المشروع الفرعى"].ToString();
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], MND_END_DATE, 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["نوع المشروع"].ToString()), 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["رقم المشروع"].ToString()), 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);
                }
            }
            else
            {
                var titlenodata = "لا توجد نتائج ";
                var callNodata = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_BOLD_Red.ToString()], titlenodata, 29, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                dataTbl.AddCell(callNodata);
            }

            cell = new PdfPCell(dataTbl);
            cell.Colspan = (29);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            parent.AddCell(cell);
            #endregion

            doc.Add(parent);
            doc.Close();
            return memoryStream.GetBuffer();
        }



        //1 - employeeAbsence
        public static byte[] DencesEmployeeAbsensce(string data, string[] infoReport, string[] columnHeader, DataTable listData)
        {
            bool isFooter = true;//send from user
            string printType = "2";//send from user 0=> طباعه افقية 
            //1=>طباعه راسية
            string footerData = "مؤسسة بياناتك لتقنية المعلومات العنوان : المملكة العربية السعودية - المدينة المنورة- تليفون : 0503326610  ";//get from data base ----Not fixed
            Document doc = new Document(PageSize.A4, 10, 10, 30, 44);
            var memoryStream = PdfBase.OpenDocument(doc, "1", true, "", isFooter, footerData);//طباعه افقية

            doc.Open();
            //Document doc = new Document(PageSize.A4, 5, 5, 30, 44);
            //var memoryStream = PdfBase.OpenDocument(doc, "1");
            var headerTitle = "";
            #region data

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds.Tables.Add(dt);
            dt.Clear();

            for (int i = 0; i <= columnHeader.Length - 1; i++)
            {
                dt.Columns.Add(columnHeader[i]);
            }

            for (int j = 0; j <= listData.Rows.Count - 1; j++)
            {
                DataRow _ravi = dt.NewRow();

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    _ravi[i] = listData.Rows[j][i];
                }
                dt.Rows.Add(_ravi);
            }

            #endregion

            PdfPCell cell = null;
            PdfPTable parent = new PdfPTable(29);
            parent.DefaultCell.Padding = 10;
            parent.WidthPercentage = 98;
            parent.DefaultCell.BorderWidth = 0;
            parent.HorizontalAlignment = Element.ALIGN_CENTER;
            parent.HeaderRows = 1;

            #region HeaderReport
            PdfPTable headerTbl = new PdfPTable(29);
            headerTbl.DefaultCell.Padding = 10;
            headerTbl.WidthPercentage = 98;
            headerTbl.DefaultCell.BorderWidth = 0;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            #region FirstRow

            if (infoReport[2] != "")
            {
                string imageFilePathLogo = PdfBase.getReportImagePath(infoReport[2]);
                if (imageFilePathLogo != "")
                {
                    Image headerJpg = Image.GetInstance(imageFilePathLogo); //infoReport[1];

                    cell = PdfBase.drawImageCell(headerJpg, 90F, 80F, 20, 2, 230, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
                else
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 20, 2, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
            }

            headerTitle = infoReport[0];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 15, 1, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            headerTitle = infoReport[3];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 14, 1, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            #endregion

            #region secondRow

            DateTime thisDay = DateTime.Today;
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], data, 3, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            headerTitle = "التاريخ  :   ";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 3, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            string title = infoReport[1];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_16_UNDERLINE.ToString()], title, 17, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            #endregion


            cell = new PdfPCell(headerTbl);
            cell.Colspan = (29);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            parent.AddCell(cell);

            #endregion

            #region HeaderTable
            PdfPTable dataTbl = new PdfPTable(29);
            dataTbl.DefaultCell.Padding = 10;
            dataTbl.WidthPercentage = 98;
            dataTbl.DefaultCell.BorderWidth = 0;
            dataTbl.HorizontalAlignment = Element.ALIGN_CENTER;
            dataTbl.HeaderRows = 1;
            dataTbl.SplitLate = false;

            headerTitle = "الفرع";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "اليوم";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 6, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "التاريخ";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 6, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "رقم البصمة";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 6, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "إسم الموظف ";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 6, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            #endregion

            #region body

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["الفرع"].ToString()), 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], ds.Tables[0].Rows[x]["اليوم"].ToString(), 6, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], ds.Tables[0].Rows[x]["التاريخ"].ToString(), 6, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    var MND_START_DATE = ds.Tables[0].Rows[x]["رقم البصمة"].ToString();
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], MND_START_DATE, 6, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    var MND_END_DATE = ds.Tables[0].Rows[x]["اسم الموظف"].ToString();
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], MND_END_DATE, 6, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                }
            }
            else
            {
                var titlenodata = "لا توجد نتائج ";
                var callNodata = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_BOLD_Red.ToString()], titlenodata, 29, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                dataTbl.AddCell(callNodata);
            }

            cell = new PdfPCell(dataTbl);
            cell.Colspan = (29);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            parent.AddCell(cell);
            #endregion

            doc.Add(parent);
            doc.Close();
            return memoryStream.GetBuffer();
        }

        //1 - employeelate
        public static byte[] DencesEmployeeLate(string data, string[] infoReport, string[] columnHeader, DataTable listData)
        {
            bool isFooter = true;//send from user
            string printType = "2";//send from user 0=> طباعه افقية 
            //1=>طباعه راسية
            string footerData = "مؤسسة بياناتك لتقنية المعلومات العنوان : المملكة العربية السعودية - المدينة المنورة- تليفون : 0503326610  ";//get from data base ----Not fixed
            Document doc = new Document(PageSize.A4, 10, 10, 30, 44);
            var memoryStream = PdfBase.OpenDocument(doc, "1", true, "", isFooter, footerData);//طباعه افقية

            doc.Open();
            //Document doc = new Document(PageSize.A4, 5, 5, 30, 44);
            //var memoryStream = PdfBase.OpenDocument(doc, "1");
            var headerTitle = "";
            #region data

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds.Tables.Add(dt);
            dt.Clear();

            for (int i = 0; i <= columnHeader.Length - 1; i++)
            {
                dt.Columns.Add(columnHeader[i]);
            }

            for (int j = 0; j <= listData.Rows.Count - 1; j++)
            {
                DataRow _ravi = dt.NewRow();

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    _ravi[i] = listData.Rows[j][i];
                }
                dt.Rows.Add(_ravi);
            }

            #endregion

            PdfPCell cell = null;
            PdfPTable parent = new PdfPTable(29);
            parent.DefaultCell.Padding = 10;
            parent.WidthPercentage = 98;
            parent.DefaultCell.BorderWidth = 0;
            parent.HorizontalAlignment = Element.ALIGN_CENTER;
            parent.HeaderRows = 1;

            #region HeaderReport
            PdfPTable headerTbl = new PdfPTable(29);
            headerTbl.DefaultCell.Padding = 10;
            headerTbl.WidthPercentage = 98;
            headerTbl.DefaultCell.BorderWidth = 0;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            #region FirstRow

            if (infoReport[2] != "")
            {
                string imageFilePathLogo = PdfBase.getReportImagePath(infoReport[2]);
                if (imageFilePathLogo != "")
                {
                    Image headerJpg = Image.GetInstance(imageFilePathLogo); //infoReport[1];

                    cell = PdfBase.drawImageCell(headerJpg, 90F, 80F, 20, 2, 230, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
                else
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 20, 2, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    headerTbl.AddCell(cell);
                }
            }

            headerTitle = infoReport[0];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 15, 1, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            headerTitle = infoReport[3];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 14, 1, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            #endregion

            #region secondRow

            DateTime thisDay = DateTime.Today;
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], data, 3, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            headerTitle = "التاريخ  :   ";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], headerTitle, 3, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            string title = infoReport[1];
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_16_UNDERLINE.ToString()], title, 17, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 6, 0, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            headerTbl.AddCell(cell);
            #endregion


            cell = new PdfPCell(headerTbl);
            cell.Colspan = (29);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            parent.AddCell(cell);

            #endregion

            #region HeaderTable
            PdfPTable dataTbl = new PdfPTable(29);
            dataTbl.DefaultCell.Padding = 10;
            dataTbl.WidthPercentage = 98;
            dataTbl.DefaultCell.BorderWidth = 0;
            dataTbl.HorizontalAlignment = Element.ALIGN_CENTER;
            dataTbl.HeaderRows = 1;
            dataTbl.SplitLate = false;

            headerTitle = " زمن التأخير";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "حضور فترة ثانية";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "زمن التأخير";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "حضور فترة اولى";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "التاريخ";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "رقم البصمة";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "إسم الموظف ";
            DrawTableHeader(dataTbl, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            #endregion

            #region body

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["زمن التأخير "].ToString()), 4, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["حضور فترة ثانية"].ToString()), 4, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], (ds.Tables[0].Rows[x]["زمن التأخير"].ToString()), 4, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], ds.Tables[0].Rows[x]["حضور فترة اولى"].ToString(), 4, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], ds.Tables[0].Rows[x]["التاريخ"].ToString(), 4, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    var MND_START_DATE = ds.Tables[0].Rows[x]["رقم البصمة"].ToString();
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], MND_START_DATE, 4, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                    var MND_END_DATE = ds.Tables[0].Rows[x]["اسم الموظف"].ToString();
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], MND_END_DATE, 5, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    dataTbl.AddCell(cell);

                }
            }
            else
            {
                var titlenodata = "لا توجد نتائج ";
                var callNodata = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_BOLD_Red.ToString()], titlenodata, 29, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                dataTbl.AddCell(callNodata);
            }

            cell = new PdfPCell(dataTbl);
            cell.Colspan = (29);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            parent.AddCell(cell);
            #endregion

            doc.Add(parent);
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
    }
}