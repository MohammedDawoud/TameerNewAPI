
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using TaamerProject.Models;

namespace TaamerProject.API.pdfHandler
{
    public class ProjectsReports
    {
        public static CultureInfo us = new CultureInfo("en-US");
        public static string PrintDate = DateTime.Now.ToString("yyyy-MM-dd ", us);

        public static byte[] PrintUserProjects(List<ProjectVM> dt, string UserName, string[] infoReport, string DateFrom, string DateTo)
        {
            bool isFooter = (infoReport[6] != null && infoReport[6] != "" || infoReport[6] != "0") ? true : false;//send from user
            string footerData = "" + infoReport[0] + " - " + infoReport[2] + "  - تليفون  : " + infoReport[5] + " - فاكس  : " + infoReport[4] + " -  بريد الكترونى  : " + infoReport[3];
            Document doc = new Document(PageSize.A4, 0, 0, 0, 44);
            PdfWriter PdfWriter = null;
            var memoryStream = PdfBase.OpenDocumentProjectReport(ref PdfWriter, doc, "1", false, "", isFooter, footerData);

            PdfContentByte cb = PdfWriter.DirectContent;
            // Bottom left coordinates x & y, followed by width, height and radius of corners.
            cb.RoundRectangle(0f, 0f, 0f, 0f, 0f);
            cb.Stroke();

            PdfPCell cell = null;
            PdfPTable tblParent = new PdfPTable(57);
            tblParent.DefaultCell.Padding = 0;
            tblParent.WidthPercentage = 100;
            tblParent.DefaultCell.BorderWidth = 0;
            tblParent.HorizontalAlignment = Element.ALIGN_CENTER;
            tblParent.SplitLate = false;
            //  tblParent.HeaderRows = 6;

            PdfPTable header1 = new PdfPTable(57);
            header1.WidthPercentage = 100;
            header1.DefaultCell.BorderWidth = 0;
            header1.DefaultCell.BorderColor = BaseColor.BLUE;
            header1.HorizontalAlignment = Element.ALIGN_CENTER;
            header1.DefaultCell.Padding = 0;
            //  header1.HeaderRows = 6;
            #region HeaderReport


            #region FirstRow
            string headerTitle = "";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 1, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            header1.AddCell(cell);

            if (infoReport[1] != "")
            {
                string imageFilePathLogo = PdfBase.getReportImagePath(infoReport[1]);

                if (imageFilePathLogo != "")
                {
                    iTextSharp.text.Image headerJpg = iTextSharp.text.Image.GetInstance(imageFilePathLogo); //infoReport[1];

                    cell = PdfBase.drawImageCell(headerJpg, 90F, 80F, 19, 3, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
                    header1.AddCell(cell);
                }
                else
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 3, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    header1.AddCell(cell);
                }
            }
            else
            {
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 3, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                header1.AddCell(cell);
            }
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], infoReport[0], 19, 3, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            header1.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "تاريخ : " + PrintDate, 19, 1, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
            header1.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 1, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
            header1.AddCell(cell);
            #endregion
            #region secondRow
            if (UserName == "")
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_UNDERLINE.ToString()], " مشاريع المستخدمين \n", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            else
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_UNDERLINE.ToString()], " مشاريع المستخدم  (" + UserName + ")\n", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);

            header1.AddCell(cell);

            #endregion

            #region secondRow
           
            if (DateFrom != "" && DateTo != "")
            {
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " \n من تاريخ: " + DateFrom +"  -  "+ "   الى تاريخ:  " + DateTo + "", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                cell.PaddingBottom = 5;
                cell.PaddingTop = 5;
                header1.AddCell(cell);
            }
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "\n\n", 57, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            header1.AddCell(cell);
            #endregion
            #endregion

            //end of line two
            PdfPTable headerbodyTable = new PdfPTable(57);
            headerbodyTable.WidthPercentage = 100;
            headerbodyTable.DefaultCell.BorderWidth = 0;
            headerbodyTable.HorizontalAlignment = Element.ALIGN_CENTER;
            headerbodyTable.DefaultCell.Padding = 0;


            headerTitle = "وصف المشروع";
            DrawTableHeader(headerbodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 8, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "نهاية المشروع";
            DrawTableHeader(headerbodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 7, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "بداية المشروع";
            DrawTableHeader(headerbodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 7, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "مدة المشروع";
            DrawTableHeader(headerbodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 7, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "نسبة الإنجاز";
            DrawTableHeader(headerbodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 7, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "نوع المشروع الفرعي";
            DrawTableHeader(headerbodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 7, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "نوع المشروع";
            DrawTableHeader(headerbodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 7, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "رقم المشروع";
            DrawTableHeader(headerbodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 7, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

           

            #region body
            PdfPTable bodyTable = new PdfPTable(57);
            List<chartDataClass> chartData = new List<chartDataClass>();

            byte[] ChartBytes = new byte[0];
            int Row_No = 0;
            if (dt != null && dt.Count > 0)
            {
                float Percent = 0;
                for (int x = 0; x < dt.Count; x++)
                {

                    if (Row_No < 23)
                    {

                        int count = int.Parse(dt[x].TaskExecPercentage_Count.ToString());
                        if (count == 0)
                        {
                            Percent = 0;
                        }
                        else
                        {
                            Percent = float.Parse(((float)(dt[x].TaskExecPercentage_Sum) / (float)((dt[x].TaskExecPercentage_Count * 100)) * 100).ToString());
                        }
                        chartData.Add(new chartDataClass()
                        {
                            Percentage = Percent,
                            ProjectNo = dt[x].ProjectNo
                        });

                        Row_No++;

                         cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].ProjectDescription != null ? dt[x].ProjectDescription.ToString() : ""), 8, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                        bodyTable.AddCell(cell);
                        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].ProjectNo != null ? dt[x].ProjectExpireDate.ToString() : ""), 7, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                        bodyTable.AddCell(cell);

                        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].ProjectNo != null ? dt[x].ProjectDate.ToString() : ""), 7, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                        bodyTable.AddCell(cell);

                        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].TimeStr != null ? dt[x].TimeStr.ToString() : ""), 7, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                        bodyTable.AddCell(cell);

                        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (Percent.ToString() + " %"), 7, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                        bodyTable.AddCell(cell);

                        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].ProjectSubTypeName != null ? dt[x].ProjectSubTypeName.ToString() : ""), 7, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                        bodyTable.AddCell(cell);

                        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].ProjectTypesName != null ? dt[x].ProjectTypesName.ToString() : ""), 7, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                        bodyTable.AddCell(cell);

                        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].ProjectNo != null ? dt[x].ProjectNo.ToString() : ""), 7, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                        bodyTable.AddCell(cell);


                    }
                    if (Row_No >= 23 || x == dt.Count - 1)
                    {
                        Row_No = 0;
                        if (Row_No == 0)
                        {
                            cell = new PdfPCell(header1);
                            cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                            cell.BorderWidth = 0;
                            cell.Colspan = 57;
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cell.PaddingBottom = 0;
                            tblParent.AddCell(cell);

                            ChartBytes = reportHelper.PopulateChart(chartData);
                            Image img = Image.GetInstance(ChartBytes);
                            cell = PdfBase.drawImageCell(img, 600F, 150F, 57, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
                            tblParent.AddCell(cell);

                            cell = new PdfPCell(headerbodyTable);
                            cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                            cell.BorderWidth = 0;
                            cell.Colspan = 57;
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cell.PaddingBottom = 0;
                            tblParent.AddCell(cell);

                            cell = new PdfPCell(bodyTable);
                            cell.Colspan = (57);
                            cell.BorderWidth = 0;
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cell.PaddingBottom = 0;
                            tblParent.AddCell(cell);

                            if (x < dt.Count - 1)
                            {
                                doc.Add(tblParent);

                                doc.NewPage();
                                cb.RoundRectangle(0f, 0f, 0f, 0f, 0f);
                                cb.Stroke();

                                tblParent = new PdfPTable(57);
                                tblParent.DefaultCell.Padding = 0;
                                tblParent.WidthPercentage = 100;
                                tblParent.DefaultCell.BorderWidth = 0;
                                tblParent.HorizontalAlignment = Element.ALIGN_CENTER;
                                tblParent.SplitLate = false;
                                //     tblParent.HeaderRows = 6;

                                bodyTable = new PdfPTable(57);
                                bodyTable.DefaultCell.Padding = 5;
                                bodyTable.WidthPercentage = 100;
                                bodyTable.DefaultCell.BorderWidth = 0;
                                bodyTable.HorizontalAlignment = Element.ALIGN_CENTER;
                                bodyTable.SplitLate = false;

                                chartData = new List<chartDataClass>();
                            }

                        }
                    }

                }

            }
            else
            {
                cell = new PdfPCell(header1);
                cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                cell.BorderWidth = 0;
                cell.Colspan = 57;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.PaddingBottom = 0;
                tblParent.AddCell(cell);

                cell = new PdfPCell(headerbodyTable);
                cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                cell.BorderWidth = 0;
                cell.Colspan = 57;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.PaddingBottom = 0;
                tblParent.AddCell(cell);

                var titlenodata = "لا توجد نتائج ";
                var callNodata = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_BOLD_Red.ToString()], titlenodata, 57, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                bodyTable.AddCell(callNodata);

                cell = new PdfPCell(bodyTable);
                cell.Colspan = (57);
                cell.BorderWidth = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.PaddingBottom = 0;
                tblParent.AddCell(cell);

            }

            #endregion

            doc.Add(tblParent);
            doc.Close();
            return memoryStream.GetBuffer();
        }

        public static byte[] PrintProjDist(List<FollowProjVM> dt, decimal ConValue, decimal CostE, decimal CostS,string ProjectNo,string TimeStr, string[] infoReport)
        {
            bool isFooter = (infoReport[6] != null && infoReport[6] != "" || infoReport[6] != "0") ? true : false;//send from user
            string footerData = "" + infoReport[0] + " - " + infoReport[2] + "  - تليفون  : " + infoReport[5] + " - فاكس  : " + infoReport[4] + " -  بريد الكترونى  : " + infoReport[3];
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

            PdfPTable headerTbl = new PdfPTable(57);
            headerTbl.DefaultCell.Padding = 5;
            headerTbl.WidthPercentage = 95;
            headerTbl.DefaultCell.BorderWidthBottom = 1;
            headerTbl.DefaultCell.BorderColor = BaseColor.BLUE;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            headerTitle = "";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 1, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            cell.Padding = 5;
            headerTbl.AddCell(cell);

            if (infoReport[1] != "")
            {
                string imageFilePathLogo = PdfBase.getReportImagePath(infoReport[1]);

                if (imageFilePathLogo != "")
                {
                    Image headerJpg = Image.GetInstance(imageFilePathLogo); //infoReport[1];

                    cell = PdfBase.drawImageCell(headerJpg, 90F, 80F, 19, 3, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
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


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], infoReport[0], 19, 3, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            cell.Padding = 10;
            headerTbl.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "تاريخ : " + PrintDate, 19, 1, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
            cell.Padding = 10;
            headerTbl.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 1, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
            cell.Padding = 5;
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_UNDERLINE.ToString()], "نسب المشروع", 57, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            cell.Padding = 5;
            headerTbl.AddCell(cell);


            cell = new PdfPCell(headerTbl);
            cell.Colspan = 57;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            tblParent.AddCell(cell);



            PdfPTable headerChart = new PdfPTable(57);
            headerChart.DefaultCell.Padding = 5;
            headerChart.WidthPercentage = 95;
            headerChart.DefaultCell.BorderWidthBottom = 1;
            headerChart.DefaultCell.BorderColor = BaseColor.BLUE;
            headerChart.HorizontalAlignment = Element.ALIGN_CENTER;

            headerTitle = "";
            
            byte[] ChartBytes = new byte[0];
            List<chartDataClass> chartData = new List<chartDataClass>();

            var CostEPer = float.Parse(((CostE / ConValue) * 100).ToString());
            var CostSPer = float.Parse(((CostS / ConValue) * 100).ToString());

            chartData.Add(new chartDataClass()
            {
                Percentage = CostEPer,
                ProjectNo = "ايرادات"
            });
            chartData.Add(new chartDataClass()
            {
                Percentage = CostSPer,
                ProjectNo = "مصروفات"
            });

            ChartBytes = reportHelper.PopulateChart(chartData);
            Image img = Image.GetInstance(ChartBytes);
            cell = PdfBase.drawImageCell(img, 600F, 150F, 57, 0, 0, 0, 5, 5, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            headerChart.AddCell(cell);


            cell = new PdfPCell(headerChart);
            cell.Colspan = 57;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            tblParent.AddCell(cell);

            PdfPTable bodyTable = new PdfPTable(57);
            bodyTable.WidthPercentage = 95;
            bodyTable.DefaultCell.BorderWidth = 0;
            bodyTable.DefaultCell.BorderColor = BaseColor.BLUE;
            bodyTable.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyTable.DefaultCell.Padding = 5;
            bodyTable.HeaderRows = 1;


            headerTitle = "التكلفة المتوقعة";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 11, 5, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "راتب الموظف";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 11, 5, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "نسبة الموظف";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 11, 5, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "المدة الزمنية";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 11, 5, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "اسم الموظف";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 13, 5, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            
            if (dt != null && dt.Count > 0)
            {
                decimal Result = 0;
                for (int x = 0; x < dt.Count; x++)
                {
                    Result = Result + Convert.ToDecimal(dt[x].ExpectedCost.ToString());
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].ExpectedCost != null ? dt[x].ExpectedCost.ToString() : "0.00"), 11, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].Amount != null ? dt[x].Amount.ToString() : "0.00"), 11, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].EmpRate.ToString() + " % "), 11, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].TimeStr != null ? dt[x].TimeStr.ToString() : ""), 11, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].EmployeeName != null ? dt[x].EmployeeName.ToString() : ""), 13, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                }
                var Status = "";
                if(Result > ConValue)
                {
                    Status = "المشروع خاسر";
                }
                else
                {
                    Status = "المشروع ناجح";
                }

                PdfPTable bodyTable2 = new PdfPTable(57);
                bodyTable2.WidthPercentage = 95;
                bodyTable2.DefaultCell.BorderWidth = 0;
                bodyTable2.DefaultCell.BorderColor = BaseColor.BLUE;
                bodyTable2.HorizontalAlignment = Element.ALIGN_CENTER;
                bodyTable2.DefaultCell.Padding = 5;
                bodyTable2.HeaderRows = 1;



                headerTitle = " ";
                DrawTableHeader(bodyTable2, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 57, 10, 20, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);


                headerTitle = "حالة المشروع";
                DrawTableHeader(bodyTable2, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 14, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

                headerTitle = "مصاريف التشغيل";
                DrawTableHeader(bodyTable2, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 14, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

                headerTitle = "قيمة المشروع";
                DrawTableHeader(bodyTable2, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 14, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

                headerTitle = "مدة المشروع";
                DrawTableHeader(bodyTable2, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 15, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);


                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (Status), 14, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                bodyTable2.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (Result.ToString()), 14, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                bodyTable2.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (ConValue.ToString()), 14, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                bodyTable2.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (TimeStr), 15, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                bodyTable2.AddCell(cell);


                cell = new PdfPCell(bodyTable);
                cell.Colspan = (57);
                cell.BorderWidth = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.PaddingBottom = 5;
                tblParent.AddCell(cell);

                cell = new PdfPCell(bodyTable2);
                cell.Colspan = (57);
                cell.BorderWidth = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.PaddingBottom = 5;
                tblParent.AddCell(cell);

            }
            else
            {
                var titlenodata = "لا توجد نتائج ";
                var callNodata = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_BOLD_Red.ToString()], titlenodata, 57, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                bodyTable.AddCell(callNodata);
            }

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

        public static byte[] PrintProjectsTasksReport(List<ProjectPhasesTasksVM> dt, string UserName, string[] infoReport)
        {
            bool isFooter = (infoReport[6] != null && infoReport[6] != "" || infoReport[6] != "0") ? true : false;//send from user
            string footerData = "" + infoReport[0] + " - " + infoReport[2] + "  - تليفون  : " + infoReport[5] + " - فاكس  : " + infoReport[4] + " -  بريد الكترونى  : " + infoReport[3];
            Document doc = new Document(PageSize.A4, 0, 0, 0, 44);
            var memoryStream = PdfBase.OpenDocument(doc, "1", false, "", isFooter, footerData);

            PdfPCell cell = null;
            PdfPTable tblParent = new PdfPTable(57);
            tblParent.DefaultCell.Padding = 0;
            tblParent.WidthPercentage = 100;
            tblParent.DefaultCell.BorderWidth = 0;
            tblParent.HorizontalAlignment = Element.ALIGN_CENTER;
            tblParent.SplitLate = false;
            tblParent.HeaderRows = 1;
            //  tblParent.HeaderRows = 6;

            PdfPTable header1 = new PdfPTable(57);
            header1.WidthPercentage = 100;
            header1.DefaultCell.BorderWidth = 0;
            header1.DefaultCell.BorderColor = BaseColor.BLUE;
            header1.HorizontalAlignment = Element.ALIGN_CENTER;
            header1.DefaultCell.Padding = 0;
            //  header1.HeaderRows = 6;
            #region HeaderReport


            #region FirstRow
            string headerTitle = "";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 1, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            header1.AddCell(cell);

            if (infoReport[1] != "")
            {
                string imageFilePathLogo = PdfBase.getReportImagePath(infoReport[1]);

                if (imageFilePathLogo != "")
                {
                    iTextSharp.text.Image headerJpg = iTextSharp.text.Image.GetInstance(imageFilePathLogo); //infoReport[1];

                    cell = PdfBase.drawImageCell(headerJpg, 90F, 80F, 19, 3, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
                    header1.AddCell(cell);
                }
                else
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 3, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    header1.AddCell(cell);
                }
            }
            else
            {
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 3, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                header1.AddCell(cell);
            }
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], infoReport[0], 19, 3, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            header1.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "تاريخ : " + PrintDate, 19, 1, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
            header1.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 1, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
            header1.AddCell(cell);
            #endregion
            #region secondRow
            if (UserName == "")
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.BarCodeFont_30_NORMAL.ToString()], "مهام المشاريع \n\n", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            else
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.BarCodeFont_30_NORMAL.ToString()], " مهام المشروع  (" + UserName + ")\n\n", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);

            header1.AddCell(cell);

            cell = new PdfPCell(header1);
            cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            cell.BorderWidth = 0;
            cell.Colspan = 57;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 0;
            tblParent.AddCell(cell);
            #endregion
            #endregion

            PdfPTable bodyTable = new PdfPTable(57);
            bodyTable.WidthPercentage = 95;
            bodyTable.DefaultCell.BorderWidth = 0;
            bodyTable.DefaultCell.BorderColor = BaseColor.BLUE;
            bodyTable.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyTable.DefaultCell.Padding = 5;
            bodyTable.HeaderRows = 6;

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "نهاية المهمة", 10, 2, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "بداية المهمة", 10, 2, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "القائم يالمهمة", 6, 2, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "اسم المهمة", 5, 2, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "المشروع", 18, 1, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "العميل", 6, 2, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            bodyTable.AddCell(cell);

            headerTitle = " رقمه";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 6, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "نوعه";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 6, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "نوعه الفرعى";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 6, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);


            #region body

            if (dt != null && dt.Count > 0)
            {
                string startDate = "";
                string endDate = "";
                for (int x = 0; x < dt.Count; x++)
                {
                    var TaskTimeFrom_Merge = "";
                    var TaskTimeTo_Merge = "";

                    if (dt[x].TaskTimeFrom == "")
                    {
                        TaskTimeFrom_Merge = dt[x].TaskStart;
                    }
                    else
                    {
                        TaskTimeFrom_Merge = dt[x].TaskStart + " - " + dt[x].TaskTimeFrom;
                    }
                    if (dt[x].TaskTimeTo == "")
                    {
                        TaskTimeTo_Merge = dt[x].EndDateCalc;
                    }
                    else
                    {
                        TaskTimeTo_Merge = dt[x].EndDateCalc + " - " + dt[x].TaskTimeTo;
                    }

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (TaskTimeTo_Merge), 11, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    cell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (TaskTimeFrom_Merge), 11, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    cell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].UserName != null ? dt[x].UserName.ToString() : ""), 6, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].DescriptionAr != null ? dt[x].DescriptionAr.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].ProjectNumber != null ? dt[x].ProjectNumber.ToString() : ""), 6, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].ProjectTypeName != null ? dt[x].ProjectTypeName.ToString() : ""), 6, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].ProjectSubTypeName != null ? dt[x].ProjectSubTypeName.ToString() : ""), 6, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].ClientName_W != null ? dt[x].ClientName_W.ToString() : ""), 6, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                }

            }

            else
            {
                var titlenodata = "لا توجد نتائج ";
                var callNodata = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_BOLD_Red.ToString()], titlenodata, 57, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                bodyTable.AddCell(callNodata);
            }


            cell = new PdfPCell(bodyTable);
            cell.Colspan = (57);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 0;
            //cell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;

            tblParent.AddCell(cell);
            #endregion

            doc.Add(tblParent);
            doc.Close();
            return memoryStream.GetBuffer();
        }


        public static byte[] PrintTaskPDF(ProjectPhasesTasksVM TaskObj, string UserName, string[] infoReport)
        {


            string FONT = "c:/windows/fonts/WINGDING.TTF";
            BaseFont bf2 = BaseFont.CreateFont(FONT, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font f = new Font(bf2, 12);

            bool isFooter = (infoReport[6] != null && infoReport[6] != "" || infoReport[6] != "0") ? true : false;//send from user
            string footerData = "" + infoReport[0] + " - " + infoReport[2] + "  - تليفون  : " + infoReport[5] + " - فاكس  : " + infoReport[4] + " -  بريد الكترونى  : " + infoReport[3];

            Document doc = new Document(PageSize.A4, 10, 10, 30, 44);
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
            headerTbl.DefaultCell.Padding = 5;
            headerTbl.WidthPercentage = 95;
            headerTbl.DefaultCell.BorderWidthBottom = 0;
            headerTbl.DefaultCell.BorderColor = BaseColor.BLUE;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            #region FirstRow
            headerTitle = "";

            if (infoReport[1] != "")
            {
                string imageFilePathLogo2 = PdfBase.getReportImagePath(infoReport[1]);
                //string startupPath2 = System.IO.Directory.GetCurrentDirectory();
                string startupPath = Environment.CurrentDirectory;
                string imageFilePathLogo = Path.GetFullPath(startupPath + infoReport[1]);
                string imageFilePathLogo3 = System.IO.Path.Combine(startupPath,infoReport[1]);


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

            cell = new PdfPCell(headerTbl);
            cell.Colspan = 57;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            cell.Border = Rectangle.NO_BORDER;
            tblParent.AddCell(cell);


            #endregion


            #endregion




            PdfPTable headerTb3 = new PdfPTable(30);
            headerTb3.DefaultCell.Padding = 5;
            headerTb3.WidthPercentage = 95;
            headerTb3.DefaultCell.BorderWidth = 0;
            headerTb3.HorizontalAlignment = Element.ALIGN_RIGHT;

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " ", 10, 0, 0, 0, 0, 0, 2, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            cell.Padding = 2;
            headerTb3.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], PrintDate, 6, 0, 1, 1, 1, 1, 2, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            headerTb3.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], " التاريخ", 4, 0, 0, 1, 1, 1, 2, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            headerTb3.AddCell(cell);

            //end of line one
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " ", 10, 0, 0, 0, 0, 0, 2, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            cell.Padding = 2;
            headerTb3.AddCell(cell);



            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " ", 10, 0, 0, 0, 0, 0, 2, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
            cell.Padding = 2;
            headerTb3.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "مهمة جديدة", 6, 0, 1, 1, 0, 1, 2, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            headerTb3.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "النوع", 4, 0, 0, 1, 0, 1, 15, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            headerTb3.AddCell(cell);
            //end of line one
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], infoReport[0], 10, 0, 0, 0, 0, 0, 15, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
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

            iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 12, iTextSharp.text.Font.BOLD, BaseColor.BLUE);

            cell = PdfBase.drawCell(font, "عزيزي المستخدم : "+ (TaskObj.UserName != null ? TaskObj.UserName.ToString() : ""), 57, 0, 0, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            cell.Padding = 8;
            tblParent.AddCell(cell);

            cell = PdfBase.drawCell(font, "السلام عليكم ورحمة الله وبركاتة", 57, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.Padding = 8;
            tblParent.AddCell(cell);

            cell = PdfBase.drawCell(font, "لديك مهمة بدأت بالتفاصيل الاتية", 57, 0, 0, 0, 0, 0, 10, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            cell.Padding = 8;
            tblParent.AddCell(cell);

            #region body2
            PdfPTable bodyTable2 = new PdfPTable(57);
            bodyTable2.WidthPercentage = 95;
            bodyTable2.DefaultCell.BorderWidth = 0;
            bodyTable2.DefaultCell.BorderColor = BaseColor.BLUE;
            bodyTable2.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyTable2.DefaultCell.Padding = 5;
            bodyTable2.HeaderRows = 1;

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], "بواسطة", 6, 0, 1, 1, 1, 1, 4, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            bodyTable2.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], "تاريخ النهاية", 8, 0, 1, 1, 1, 1, 4, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            bodyTable2.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], "تاريخ البداية", 8, 0, 1, 1, 1, 1, 4, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            bodyTable2.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], "المدة", 4, 0, 1, 1, 1, 1, 4, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            bodyTable2.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], "اسم العميل", 13, 0, 1, 1, 1, 1, 4, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            bodyTable2.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], "رقم المشروع", 8, 0, 1, 1, 1, 1, 4, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            bodyTable2.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], "وصف المهمة", 10, 0, 1, 1, 1, 1, 4, Element.ALIGN_CENTER);
            cell.Padding = 2;
            cell.BorderColor = BaseColor.BLUE;
            bodyTable2.AddCell(cell);

            var timeType = "يوم";
            var timeStr = "";
            if (TaskObj.TimeType == 1)
            {
                timeType = "ساعه";
            }
            timeStr = TaskObj.TimeMinutes + " " + timeType;


            var TaskTimeFrom_Merge = "";
            var TaskTimeTo_Merge = "";

            if (TaskObj.TaskTimeFrom == "")
            {
                TaskTimeFrom_Merge = TaskObj.ExcpectedStartDate;
            }
            else
            {
                TaskTimeFrom_Merge = TaskObj.ExcpectedStartDate + " - " + TaskObj.TaskTimeFrom;
            }

            if (TaskObj.TaskTimeTo == "")
            {
                TaskTimeTo_Merge = TaskObj.ExcpectedEndDate;
            }
            else
            {
                TaskTimeTo_Merge = TaskObj.ExcpectedEndDate + " - " + TaskObj.TaskTimeTo;
            }


            if (TaskObj != null)
            {
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (TaskObj.AddedTaskName != null ? TaskObj.AddedTaskName.ToString() : ""), 6, 0, 1, 1, 0, 1, 4, Element.ALIGN_CENTER);
                cell.Padding = 2;
                cell.BorderColor = BaseColor.BLUE;
                bodyTable2.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (TaskTimeTo_Merge != null ? TaskTimeTo_Merge.ToString() : ""), 8, 0, 1, 1, 0, 1, 4, Element.ALIGN_CENTER);
                cell.Padding = 2;
                cell.BorderColor = BaseColor.BLUE;
                bodyTable2.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (TaskTimeFrom_Merge != null ? TaskTimeFrom_Merge.ToString() : ""), 8, 0, 1, 1, 0, 1, 4, Element.ALIGN_CENTER);
                cell.Padding = 2;
                cell.BorderColor = BaseColor.BLUE;
                bodyTable2.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (TaskObj.TimeStr != null ? TaskObj.TimeStr.ToString() : ""), 4, 0, 1, 1, 0, 1, 4, Element.ALIGN_CENTER);
                cell.Padding = 2;
                cell.BorderColor = BaseColor.BLUE;
                bodyTable2.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (TaskObj.ClientName != null ? TaskObj.ClientName.ToString() : ""), 13, 0, 1, 1, 0, 1, 4, Element.ALIGN_CENTER);
                cell.Padding = 2;
                cell.BorderColor = BaseColor.BLUE;
                bodyTable2.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (TaskObj.ProjectNumber != null ? TaskObj.ProjectNumber.ToString() : ""), 8, 0, 1, 1, 0, 1, 4, Element.ALIGN_CENTER);
                cell.Padding = 2;
                cell.BorderColor = BaseColor.BLUE;
                bodyTable2.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (TaskObj.DescriptionAr != null ? TaskObj.DescriptionAr.ToString() : ""), 10, 0, 1, 1, 0, 1, 4, Element.ALIGN_CENTER);
                cell.Padding = 2;
                cell.BorderColor = BaseColor.BLUE;
                bodyTable2.AddCell(cell);

            }
            else
            {
                var titlenodata = "لا توجد نتائج ";
                var callNodata = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_BOLD_Red.ToString()], titlenodata, 57, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                callNodata.Padding = 2;
                callNodata.BorderColor = BaseColor.BLUE;
                bodyTable2.AddCell(callNodata);
            }

            #endregion
            cell = new PdfPCell(bodyTable2);
            cell.Colspan = 57;
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            tblParent.AddCell(cell);


            cell = PdfBase.drawCell(font, "مع تحيات قسم ادارة المشاريع", 57, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            cell.Padding = 10;
            tblParent.AddCell(cell);

            doc.Add(tblParent);


            doc.Close();
            return memoryStream.GetBuffer();
        }



        internal static byte[] PrintProjectsTasksReport2(List<ProjectPhasesTasksVM> dt, string UserName, string[] infoReport, string DateFrom, string DateTo)
        {

            bool isFooter = (infoReport[6] != null && infoReport[6] != "" || infoReport[6] != "0") ? true : false;//send from user
            string footerData = "" + infoReport[0] + " - " + infoReport[2] + "  - تليفون  : " + infoReport[5] + " - فاكس  : " + infoReport[4] + " -  بريد الكترونى  : " + infoReport[3];
            Document doc = new Document(PageSize.A4, 10, 10, 30, 44);
            var memoryStream = PdfBase.OpenDocument(doc, "1", true, "", isFooter, footerData);

            var headerTitle = "";

            PdfPCell cell = null;
            PdfPTable tblParent = new PdfPTable(1);
            tblParent.DefaultCell.Padding = 1;
            tblParent.WidthPercentage = 100;
            tblParent.DefaultCell.BorderWidth = 0;
            tblParent.HorizontalAlignment = Element.ALIGN_CENTER;
            tblParent.SplitLate = false;
            tblParent.HeaderRows = 1;

            #region HeaderReport
            PdfPTable headerTbl = new PdfPTable(57);
            headerTbl.DefaultCell.Padding = 1;
            headerTbl.WidthPercentage = 100;
            headerTbl.DefaultCell.BorderWidthBottom = 1;
            headerTbl.DefaultCell.BorderColor = BaseColor.BLUE;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            #region FirstRow
            headerTitle = "";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 1, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            cell.Padding = 5;
            headerTbl.AddCell(cell);

            if (infoReport[1] != "")
            {
                string imageFilePathLogo = PdfBase.getReportImagePath(infoReport[1]);

                if (imageFilePathLogo != "")
                {
                    Image headerJpg = Image.GetInstance(imageFilePathLogo); //infoReport[1];

                    cell = PdfBase.drawImageCell(headerJpg, 90F, 80F, 19, 3, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
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


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], infoReport[0], 19, 3, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            cell.Padding = 10;
            headerTbl.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "تاريخ : " + PrintDate, 19, 1, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
            cell.Padding = 10;
            headerTbl.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 1, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
            cell.Padding = 5;
            headerTbl.AddCell(cell);

            #endregion
            #region secondRow
            if (UserName == "")
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.BarCodeFont_30_NORMAL.ToString()], " مهام المشاريع  \n\n", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            else
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.BarCodeFont_30_NORMAL.ToString()], "  مهام المشروع (" + UserName + ")", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);

            headerTbl.AddCell(cell);
            #endregion
            #region ThirdRow

            if (DateFrom != "" && DateTo != "")
            {
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " \n من تاريخ: " + DateFrom + "  -  " + "   الى تاريخ:  " + DateTo + "", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                cell.PaddingBottom = 5;
                cell.PaddingTop = 5;
                headerTbl.AddCell(cell);
            }

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "\n\n", 57, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            headerTbl.AddCell(cell);
            #endregion




            cell = new PdfPCell(headerTbl);
            cell.Colspan = 57;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            tblParent.AddCell(cell);


            #endregion

            PdfPTable bodyTable = new PdfPTable(72);
            bodyTable.WidthPercentage = 100;
            bodyTable.DefaultCell.BorderWidth = 0;
            bodyTable.DefaultCell.BorderColor = BaseColor.BLUE;
            bodyTable.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyTable.DefaultCell.Padding = 1;
            bodyTable.HeaderRows = 1;


            headerTitle = "نهاية المهمة";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 11, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "بداية المهمة";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 11, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "اسم المستخدم";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 7, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "المهمة";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 9, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);


            headerTitle = "نوع المشروع الفرعي";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 8, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);


            headerTitle = "نوع المشروع";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 8, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "رقم المشروع";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 9, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "العميل";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 9, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);



            #region body

            if (dt != null && dt.Count > 0)
            {
                for (int x = 0; x < dt.Count; x++)
                {

                    CultureInfo culture = new CultureInfo("en-US");

                    var TaskTimeFrom_Merge = "";
                    var TaskTimeTo_Merge = "";

                    if (dt[x].TaskTimeFrom == "")
                    {
                        TaskTimeFrom_Merge = dt[x].TaskStart;
                    }
                    else
                    {
                        TaskTimeFrom_Merge = dt[x].TaskStart + " - " + dt[x].TaskTimeFrom;
                    }

                    if (dt[x].TaskTimeTo == "")
                    {
                        TaskTimeTo_Merge = dt[x].EndDateCalc;
                    }
                    else
                    {
                        TaskTimeTo_Merge = dt[x].EndDateCalc + " - " + dt[x].TaskTimeTo;
                    }



                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (TaskTimeTo_Merge), 11, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    cell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (TaskTimeFrom_Merge), 11, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    cell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].UserName != null ? dt[x].UserName.ToString() : ""), 7, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].DescriptionAr != null ? dt[x].DescriptionAr.ToString() : ""), 9, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].ProjectSubTypeName != null ? dt[x].ProjectSubTypeName.ToString() : ""), 8, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].ProjectTypeName != null ? dt[x].ProjectTypeName.ToString() : ""), 8, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].ProjectNumber != null ? dt[x].ProjectNumber.ToString() : ""), 9, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].ClientName != null ? dt[x].ClientName.ToString() : ""), 9, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                }


            }

            else
            {
                var titlenodata = "لا توجد نتائج ";
                var callNodata = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_BOLD_Red.ToString()], titlenodata, 57, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                bodyTable.AddCell(callNodata);
            }






            cell = new PdfPCell(bodyTable);
            cell.Colspan = (72);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 0;
            //cell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;

            tblParent.AddCell(cell);

            #endregion
            doc.Add(tblParent);
            doc.Close();
            return memoryStream.GetBuffer();
        }



        //public static byte[] PrintAllEmpTasksRpt(List<ProjectPhasesTasksVM> dt, string UserName,int? status, string[] infoReport, string DateFrom, string DateTo)
        //{



        //    bool isFooter = (infoReport[6] != null && infoReport[6] != "" || infoReport[6] != "0") ? true : false;//send from user
        //    string footerData = "" + infoReport[0] + " - " + infoReport[2] + "  - تليفون  : " + infoReport[5] + " - فاكس  : " + infoReport[4] + " -  بريد الكترونى  : " + infoReport[3];
        //    Document doc = new Document(PageSize.A4, 10, 10, 30, 44);
        //    var memoryStream = PdfBase.OpenDocument(doc, "1", false, "", isFooter, footerData);

        //    PdfPCell cell = null;
        //    PdfPTable tblParent = new PdfPTable(1);
        //    tblParent.DefaultCell.Padding = 1;
        //    tblParent.WidthPercentage = 100;
        //    tblParent.DefaultCell.BorderWidth = 0;
        //    tblParent.HorizontalAlignment = Element.ALIGN_CENTER;
        //    tblParent.SplitLate = false;
        //    tblParent.HeaderRows = 1;
        //    //  tblParent.HeaderRows = 6;

        //    PdfPTable header1 = new PdfPTable(57);
        //    header1.WidthPercentage = 100;
        //    header1.DefaultCell.BorderWidth = 0;
        //    header1.DefaultCell.BorderColor = BaseColor.BLUE;
        //    header1.HorizontalAlignment = Element.ALIGN_CENTER;
        //    header1.DefaultCell.Padding = 0;
        //    //  header1.HeaderRows = 6;
        //    #region HeaderReport


        //    #region FirstRow
        //    string headerTitle = "";
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 1, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
        //    header1.AddCell(cell);

        //    if (infoReport[1] != "")
        //    {
        //        string imageFilePathLogo = PdfBase.getReportImagePath(infoReport[1]);

        //        if (imageFilePathLogo != "")
        //        {
        //            iTextSharp.text.Image headerJpg = iTextSharp.text.Image.GetInstance(imageFilePathLogo); //infoReport[1];

        //            cell = PdfBase.drawImageCell(headerJpg, 90F, 80F, 19, 3, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
        //            header1.AddCell(cell);
        //        }
        //        else
        //        {
        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 3, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
        //            header1.AddCell(cell);
        //        }
        //    }
        //    else
        //    {
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 3, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
        //        header1.AddCell(cell);
        //    }
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], infoReport[0], 19, 3, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
        //    header1.AddCell(cell);
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "تاريخ : " + PrintDate, 19, 1, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
        //    header1.AddCell(cell);
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 1, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
        //    header1.AddCell(cell);
        //    #endregion
        //    #region secondRow
        //    if (UserName == "")
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.BarCodeFont_30_NORMAL.ToString()], " مهام المستخدمين \n\n", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
        //    else
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.BarCodeFont_30_NORMAL.ToString()], "  مهام المستخدم (" + UserName + ")", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);

        //    header1.AddCell(cell);
        //    #endregion
        //    #region ThirdRow

        //    if (DateFrom != "" && DateTo != "")
        //    {
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " \n من تاريخ: " + DateFrom + "  -  " + "   الى تاريخ:  " + DateTo + "", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
        //        cell.PaddingBottom = 5;
        //        cell.PaddingTop = 5;
        //        header1.AddCell(cell);
        //    }
        //    if(status !=null && status !=0) {
        //        var statuname =GetStatusName(status);
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " \n حالة المهمة: " + statuname + "", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
        //        cell.PaddingBottom = 5;
        //        cell.PaddingTop = 5;
        //        header1.AddCell(cell);
        //    }
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "\n\n", 57, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
        //    header1.AddCell(cell);
        //    #endregion
        //    cell = new PdfPCell(header1);
        //    cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
        //    cell.BorderWidth = 0;
        //    cell.Colspan = 57;
        //    cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    cell.PaddingBottom = 0;
        //    tblParent.AddCell(cell);



        //    #endregion

        //    PdfPTable bodyTable = new PdfPTable(72);
        //    bodyTable.WidthPercentage = 100;
        //    bodyTable.DefaultCell.BorderWidth = 0;
        //    bodyTable.DefaultCell.BorderColor = BaseColor.BLUE;
        //    bodyTable.HorizontalAlignment = Element.ALIGN_CENTER;
        //    bodyTable.DefaultCell.Padding = 1;
        //    bodyTable.HeaderRows = 1;



        //    headerTitle = "وقت المهمة";
        //    DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 9, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //    headerTitle = "الحالة";
        //    DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 9, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //    headerTitle = "نهاية المهمة";
        //    DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 9, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //    headerTitle = "بداية المهمة";
        //    DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 9, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //    headerTitle = "نوع المشروع";
        //    DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 9, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //    headerTitle = "رقم المشروع";
        //    DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 9, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //    headerTitle = "المهمة";
        //    DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 9, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //    headerTitle = "الاسم";
        //    DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 9, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);



        //    #region body

        //    if (dt != null && dt.Count > 0)
        //    {
        //        for (int x = 0; x < dt.Count; x++)
        //        {

        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].TimeStr != null ? dt[x].TimeStr.ToString() : ""), 9, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);

        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].StatusName.ToString()), 9, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);

        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].TaskEnd != null ? dt[x].TaskEnd.ToString() : ""), 9, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);

        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].TaskStart != null ? dt[x].TaskStart.ToString() : ""), 9, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);

        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].ProjectTypeName != null ? dt[x].ProjectTypeName.ToString() : ""), 9, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);

        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].ProjectNumber != null ? dt[x].ProjectNumber.ToString() : ""), 9, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);

        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].DescriptionAr != null ? dt[x].DescriptionAr.ToString() : ""), 9, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);

        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].UserName != null ? dt[x].UserName.ToString() : ""), 9, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);

        //        }


        //    }

        //    else
        //    {
        //        var titlenodata = "لا توجد نتائج ";
        //        var callNodata = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_BOLD_Red.ToString()], titlenodata, 57, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        bodyTable.AddCell(callNodata);
        //    }
        //    cell = new PdfPCell(bodyTable);
        //    cell.Colspan = (72);
        //    cell.BorderWidth = 0;
        //    cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    cell.PaddingBottom = 0;
        //    tblParent.AddCell(cell);
        //    #endregion

        //    doc.Add(tblParent);
        //    doc.Close();
        //    return memoryStream.GetBuffer();
        //}






        internal static byte[] PrintAllEmpTasksRpt(List<ProjectPhasesTasksVM> dt, string UserName, int? status, string[] infoReport, string DateFrom, string DateTo)
        {

            bool isFooter = (infoReport[6] != null && infoReport[6] != "" || infoReport[6] != "0") ? true : false;//send from user
            string footerData = "" + infoReport[0] + " - " + infoReport[2] + "  - تليفون  : " + infoReport[5] + " - فاكس  : " + infoReport[4] + " -  بريد الكترونى  : " + infoReport[3];
            Document doc = new Document(PageSize.A4, 10, 10, 30, 44);
            var memoryStream = PdfBase.OpenDocument(doc, "1", true, "", isFooter, footerData);

            var headerTitle = "";

            PdfPCell cell = null;
            PdfPTable tblParent = new PdfPTable(1);
            tblParent.DefaultCell.Padding = 1;
            tblParent.WidthPercentage = 100;
            tblParent.DefaultCell.BorderWidth = 0;
            tblParent.HorizontalAlignment = Element.ALIGN_CENTER;
            tblParent.SplitLate = false;
            tblParent.HeaderRows = 1;

            #region HeaderReport
            PdfPTable headerTbl = new PdfPTable(57);
            headerTbl.DefaultCell.Padding = 1;
            headerTbl.WidthPercentage = 100;
            headerTbl.DefaultCell.BorderWidthBottom = 1;
            headerTbl.DefaultCell.BorderColor = BaseColor.BLUE;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            #region FirstRow
            headerTitle = "";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 1, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            cell.Padding = 5;
            headerTbl.AddCell(cell);

            if (infoReport[1] != "")
            {
                string imageFilePathLogo = PdfBase.getReportImagePath(infoReport[1]);

                if (imageFilePathLogo != "")
                {
                    Image headerJpg = Image.GetInstance(imageFilePathLogo); //infoReport[1];

                    cell = PdfBase.drawImageCell(headerJpg, 90F, 80F, 19, 3, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
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


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], infoReport[0], 19, 3, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            cell.Padding = 10;
            headerTbl.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "تاريخ : " + PrintDate, 19, 1, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
            cell.Padding = 10;
            headerTbl.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 1, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
            cell.Padding = 5;
            headerTbl.AddCell(cell);

            #endregion
            #region secondRow
            if (UserName == "")
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.BarCodeFont_30_NORMAL.ToString()], " مهام المستخدمين \n\n", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            else
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.BarCodeFont_30_NORMAL.ToString()], "  مهام المستخدم (" + UserName + ")", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);

            headerTbl.AddCell(cell);
            #endregion
            #region ThirdRow

            if (DateFrom != "" && DateTo != "")
            {
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " \n من تاريخ: " + DateFrom + "  -  " + "   الى تاريخ:  " + DateTo + "", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                cell.PaddingBottom = 5;
                cell.PaddingTop = 5;
                headerTbl.AddCell(cell);
            }
            if (status != null && status != 0)
            {
                var statuname = GetStatusName(status);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " \n حالة المهمة: " + statuname + "", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                cell.PaddingBottom = 5;
                cell.PaddingTop = 5;
                headerTbl.AddCell(cell);
            }
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "\n\n", 57, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            headerTbl.AddCell(cell);
            #endregion




            cell = new PdfPCell(headerTbl);
            cell.Colspan = 57;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            tblParent.AddCell(cell);


            #endregion

            PdfPTable bodyTable = new PdfPTable(72);
            bodyTable.WidthPercentage = 100;
            bodyTable.DefaultCell.BorderWidth = 0;
            bodyTable.DefaultCell.BorderColor = BaseColor.BLUE;
            bodyTable.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyTable.DefaultCell.Padding = 1;
            bodyTable.HeaderRows = 1;

            headerTitle = "وقت المهمة";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 7, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "الحالة";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 7, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "نهاية المهمة";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 11, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "بداية المهمة";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 11, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "نوع المشروع";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 9, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "رقم المشروع";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 9, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "المهمة";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 9, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "الاسم";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 9, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);



            #region body

            if (dt != null && dt.Count > 0)
            {
                for (int x = 0; x < dt.Count; x++)
                {

                    CultureInfo culture = new CultureInfo("en-US");

                    var TaskTimeFrom_Merge = "";
                    var TaskTimeTo_Merge = "";


                    //var date1temp = dt[x].ExcpectedStartDate;
                    //var date2temp = dt[x].ExcpectedEndDate;
                    //DateTime date1 = Convert.ToDateTime(date1temp, culture);
                    //DateTime date2 = Convert.ToDateTime(date2temp, culture);
                    //var Difference_In_Days = (date2 - date1).TotalDays;
                    //DateTime date = Convert.ToDateTime(dt[x].TaskEnd, culture);
                    //var next_date = date.AddDays(Difference_In_Days+1);
                    //string Actual_EndDate = next_date.ToString("yyyy-MM-dd", culture);


                    if (dt[x].TaskTimeFrom == "")
                    {
                        TaskTimeFrom_Merge = dt[x].TaskStart;
                    }
                    else
                    {
                        TaskTimeFrom_Merge = dt[x].TaskStart + " - " + dt[x].TaskTimeFrom;
                    }

                    if (dt[x].TaskTimeTo == "")
                    {
                        TaskTimeTo_Merge = dt[x].EndDateCalc;
                    }
                    else
                    {
                        TaskTimeTo_Merge = dt[x].EndDateCalc + " - " + dt[x].TaskTimeTo;
                    }


                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].TimeStr != null ? dt[x].TimeStr.ToString() : ""), 7, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].StatusName.ToString()), 7, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (TaskTimeTo_Merge),11, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    cell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (TaskTimeFrom_Merge), 11, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    cell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].ProjectTypeName != null ? dt[x].ProjectTypeName.ToString() : ""), 9, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].ProjectNumber != null ? dt[x].ProjectNumber.ToString() : ""), 9, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].DescriptionAr != null ? dt[x].DescriptionAr.ToString() : ""), 9, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].ClientName != null ? dt[x].ClientName.ToString() : ""), 9, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                }


            }

            else
            {
                var titlenodata = "لا توجد نتائج ";
                var callNodata = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_BOLD_Red.ToString()], titlenodata, 57, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                bodyTable.AddCell(callNodata);
            }






            cell = new PdfPCell(bodyTable);
            cell.Colspan = (72);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 0;
            //cell.RunDirection = PdfWriter.RUN_DIRECTION_LTR;

            tblParent.AddCell(cell);

            #endregion
            doc.Add(tblParent);
            doc.Close();
            return memoryStream.GetBuffer();
        }

        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="UserName"></param>
        /// <param name="infoReport"></param>
        /// <returns></returns>
        /// 

        internal static byte[] PrintAllEmpPerformancrRpt(List<RptAllEmpPerformance> dt, string[] infoReport, string DateFrom, string DateTo, string Username, int datetype,int allemp)
        {

            bool isFooter = (infoReport[6] != null && infoReport[6] != "" || infoReport[6] != "0") ? true : false;//send from user
            string footerData = "" + infoReport[0] + " - " + infoReport[2] + "  - تليفون  : " + infoReport[5] + " - فاكس  : " + infoReport[4] + " -  بريد الكترونى  : " + infoReport[3];
            Document doc = new Document(PageSize.A4, 10, 10, 30, 44);
            var memoryStream = PdfBase.OpenDocument(doc, "1", true, "", isFooter, footerData);

            var headerTitle = "";

            PdfPCell cell = null;
            PdfPTable tblParent = new PdfPTable(1);
            tblParent.DefaultCell.Padding = 1;
            tblParent.WidthPercentage = 100;
            tblParent.DefaultCell.BorderWidth = 0;
            tblParent.HorizontalAlignment = Element.ALIGN_CENTER;
            tblParent.SplitLate = false;
            tblParent.HeaderRows = 1;

            #region HeaderReport
            PdfPTable headerTbl = new PdfPTable(57);
            headerTbl.DefaultCell.Padding = 1;
            headerTbl.WidthPercentage = 100;
            headerTbl.DefaultCell.BorderWidthBottom = 1;
            headerTbl.DefaultCell.BorderColor = BaseColor.BLUE;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            #region FirstRow
            headerTitle = "";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 1, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            cell.Padding = 5;
            headerTbl.AddCell(cell);

            if (infoReport[1] != "")
            {
                string imageFilePathLogo = PdfBase.getReportImagePath(infoReport[1]);

                if (imageFilePathLogo != "")
                {
                    Image headerJpg = Image.GetInstance(imageFilePathLogo); //infoReport[1];

                    cell = PdfBase.drawImageCell(headerJpg, 90F, 80F, 19, 3, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
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


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], infoReport[0], 19, 3, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            cell.Padding = 10;
            headerTbl.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "تاريخ : " + PrintDate, 19, 1, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
            cell.Padding = 10;
            headerTbl.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 1, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
            cell.Padding = 5;
            headerTbl.AddCell(cell);

            #endregion

            #region secondRow
            //headerTitle = "مسير مرتبات الموظفين " + (Username == "" ? "" : "لشهر : " + Username + "") + " ";

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_NORMAL.ToString()], headerTitle, 57, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            //cell.Padding = 5;
            //headerTbl.AddCell(cell);

            if (Username == "") { 
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.BarCodeFont_30_NORMAL.ToString()], " تقرير الاداء الشامل للموظفين \n\n", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            cell.Padding = 5;
            }
            else {
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.BarCodeFont_30_NORMAL.ToString()], "  تقرير الاداء الشامل للموظف (" + Username + ")", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
                cell.Padding = 5;
            }
            headerTbl.AddCell(cell);
           


            #endregion
            //if (Username != "") headerTitle = "فرع : " + Username;
            //else headerTitle = "جميع الفروع";
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_NORMAL.ToString()], headerTitle, 57, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            //cell.Padding = 5;
            //headerTbl.AddCell(cell);

            if (DateFrom != "" && DateTo != "" && DateFrom != null && DateTo != null)
            {
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " \n من تاريخ: " + DateFrom + "  -  " + "   الى تاريخ:  " + DateTo + "", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                cell.PaddingBottom = 5;
                cell.PaddingTop = 5;
                headerTbl.AddCell(cell);
            }
            if (datetype != null && datetype != 0)
            {
                var statuname = GetDateType(datetype);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " \n خلال فترة زمنية : " + statuname + "", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                cell.PaddingBottom = 5;
                cell.PaddingTop = 5;
                headerTbl.AddCell(cell);
            }
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "\n\n", 57, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            headerTbl.AddCell(cell);



            cell = new PdfPCell(headerTbl);
            cell.Colspan = 57;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            tblParent.AddCell(cell);


            #endregion

            PdfPTable bodyTable = new PdfPTable(72);
            bodyTable.WidthPercentage = 100;
            bodyTable.DefaultCell.BorderWidth = 0;
            bodyTable.DefaultCell.BorderColor = BaseColor.BLUE;
            bodyTable.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyTable.DefaultCell.Padding = 1;
            bodyTable.HeaderRows = 1;

            headerTitle = "قياس اداء الموظف في انجاز المشاريع";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 25, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "قياس اداء الموظف في انجاز المهام";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 40, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "الإسم";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 7, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);


            headerTitle = "بدون حركة";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "قاربت التأخير";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "المتوقفة";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "قيد التنفيذ";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "المتأخرة";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);



            headerTitle = "نسبة الانجاز";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "نسبة التأخير";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "المتأخرة";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "قاربت التأخير";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            headerTitle = "المنجزة";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            headerTitle = "الرجيعة";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            headerTitle = "قيد التشغل";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "لم تبدأ";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 7, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            if (dt != null && dt.Count > 0)
            {
            if(allemp == 1) { 
                for (int x = 0; x < dt.Count; x++)
                {

                    
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].ProjectWithout != null ? dt[x].ProjectWithout.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].TotalViolations != null ? dt[x].TotalViolations.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    //bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].ProjectAlmostfinish != null ? dt[x].ProjectAlmostfinish.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].ProjectStoped != null ? dt[x].ProjectStoped.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].ProjectInProgress != null ? dt[x].ProjectInProgress.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].ProjectLate != null ? dt[x].ProjectLate.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].CompletePercentage != "0" ? dt[x].CompletePercentage.ToString() + " % " : "0"), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].LatePercentage != "0" ? dt[x].LatePercentage.ToString() + " % " : "0"), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].Latetask != null ? dt[x].Latetask.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].AlmostLate != null ? dt[x].AlmostLate.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].Completed != null ? dt[x].Completed.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].Retrived != null ? dt[x].Retrived.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].Inprogress != null ? dt[x].Inprogress.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].Notstarted != null ? dt[x].Notstarted.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].UserName != null ? dt[x].UserName.ToString() : ""), 7, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);
                }
                }
                else
                {
                    for (int x = 0; x < dt.Count; x++)
                    {
                        if (dt[x].Notstarted == "0" && dt[x].Inprogress == "0" && dt[x].Retrived == "0" && dt[x].Completed == "0" && dt[x].AlmostLate == "0" && dt[x].Latetask == "0" && dt[x].LatePercentage == "0" && dt[x].CompletePercentage == "0" && dt[x].ProjectLate == "0" &&
                       dt[x].ProjectInProgress == "0" && dt[x].ProjectStoped == "0" && dt[x].ProjectAlmostfinish == "0" && dt[x].ProjectWithout == "0")
                        {
                        }
                        else {

                        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].ProjectWithout != null ? dt[x].ProjectWithout.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                        bodyTable.AddCell(cell);

                        //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].TotalViolations != null ? dt[x].TotalViolations.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                        //bodyTable.AddCell(cell);

                        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].ProjectAlmostfinish != null ? dt[x].ProjectAlmostfinish.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                        bodyTable.AddCell(cell);

                        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].ProjectStoped != null ? dt[x].ProjectStoped.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                        bodyTable.AddCell(cell);

                        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].ProjectInProgress != null ? dt[x].ProjectInProgress.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                        bodyTable.AddCell(cell);

                        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].ProjectLate != null ? dt[x].ProjectLate.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                        bodyTable.AddCell(cell);

                        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].CompletePercentage != "0" ? dt[x].CompletePercentage.ToString() + " % " : "0"), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                        bodyTable.AddCell(cell);

                        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].LatePercentage != "0" ? dt[x].LatePercentage.ToString() + " % " : "0"), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                        bodyTable.AddCell(cell);

                        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].Latetask != null ? dt[x].Latetask.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                        bodyTable.AddCell(cell);

                        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].AlmostLate != null ? dt[x].AlmostLate.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                        bodyTable.AddCell(cell);
                        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].Completed != null ? dt[x].Completed.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                        bodyTable.AddCell(cell);
                        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].Retrived != null ? dt[x].Retrived.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                        bodyTable.AddCell(cell);
                        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].Inprogress != null ? dt[x].Inprogress.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                        bodyTable.AddCell(cell);

                        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].Notstarted != null ? dt[x].Notstarted.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                        bodyTable.AddCell(cell);

                        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].UserName != null ? dt[x].UserName.ToString() : ""), 7, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                        bodyTable.AddCell(cell);
                        }
                    }
                }



                //المجموع
            }
            else
            {
                var titlenodata = "لا توجد نتائج ";
                var callNodata = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_BOLD_Red.ToString()], titlenodata, 57, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                bodyTable.AddCell(callNodata);
            }



            int TLatetask = 0, TInprogress = 0, TNotstarted = 0, TCompleted = 0, TRetrived = 0, Tcompercentage = 0, Tlateperc = 0;
            int TAlmostLate = 0, TProjectLate = 0, TProjectInProgress = 0, TProjectStoped = 0, TProjectWithout = 0, TProjectAlmostfinish = 0;
            int Counter = 0;
            string Tcompercentage_F = "0", Tlateperc_F = "0";
            if (dt != null && dt.Count > 0)
            {
                for (int x = 0; x < dt.Count; x++)
                {
                    if (dt[x].Notstarted == "0" && dt[x].Inprogress == "0" && dt[x].Retrived == "0" && dt[x].Completed == "0" && dt[x].AlmostLate == "0" && dt[x].Latetask == "0" && dt[x].LatePercentage == "0" && dt[x].CompletePercentage == "0" && dt[x].ProjectLate == "0" &&
                    dt[x].ProjectInProgress == "0" && dt[x].ProjectStoped == "0" && dt[x].ProjectAlmostfinish == "0" && dt[x].ProjectWithout == "0")
                    {
                    }
                    else
                    {
                        Counter = Counter + 1;
                    }

                    TLatetask += int.Parse(dt[x].Latetask);
                    TInprogress += int.Parse(dt[x].Inprogress);
                    TNotstarted += int.Parse(dt[x].Notstarted);
                    TCompleted += int.Parse(dt[x].Completed);
                    TRetrived += int.Parse(dt[x].Retrived);
                    Tcompercentage += int.Parse(dt[x].CompletePercentage);
                    Tlateperc += int.Parse(dt[x].LatePercentage);
                    TAlmostLate += int.Parse(dt[x].AlmostLate);
                    TProjectLate += int.Parse(dt[x].ProjectLate);
                    TProjectInProgress += int.Parse(dt[x].ProjectInProgress);
                    TProjectStoped += int.Parse(dt[x].ProjectStoped);
                    TProjectAlmostfinish += int.Parse(dt[x].ProjectAlmostfinish);
                    TProjectWithout += int.Parse(dt[x].ProjectWithout);


                }

                if (Tcompercentage > 0 && Counter > 0)
                {
                    Tcompercentage_F = Convert.ToDecimal((double)((double)Tcompercentage / ((double)Counter * 100)) * 100).ToString("0.##");
                    Tlateperc_F = Convert.ToDecimal((double)((double)Tlateperc / ((double)Counter * 100)) * 100).ToString("0.##");
                }
                else
                {
                    Tcompercentage_F = "0";
                    Tlateperc_F = "0";
                }



                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (TProjectWithout.ToString()), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                bodyTable.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (TProjectAlmostfinish.ToString()), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                bodyTable.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (TProjectStoped.ToString()), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                bodyTable.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (TProjectInProgress.ToString()), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                bodyTable.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (TProjectLate.ToString()), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                bodyTable.AddCell(cell);




                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (Tcompercentage_F.ToString() + " % "), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                bodyTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (Tlateperc_F.ToString() + " % "), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                bodyTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (TLatetask.ToString()), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                bodyTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (TAlmostLate.ToString()), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                bodyTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (TCompleted.ToString()), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                bodyTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (TRetrived.ToString()), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                bodyTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (TInprogress.ToString()), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                bodyTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (TNotstarted.ToString()), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                bodyTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], ("الاجمالـــي"), 7, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                bodyTable.AddCell(cell);


            }






            cell = new PdfPCell(bodyTable);
            cell.Colspan = (72);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 0;
            tblParent.AddCell(cell);

            doc.Add(tblParent);
            doc.Close();
            return memoryStream.GetBuffer();
        }


        //public static byte[] PrintAllEmpPerformancrRpt(List<RptAllEmpPerformance> dt, string[] infoReport, string DateFrom, string DateTo, string Username,int datetype)
        //{

        //    bool isFooter = (infoReport[6] != null && infoReport[6] != "" || infoReport[6] != "0") ? true : false;//send from user
        //    string footerData = "" + infoReport[0] + " - " + infoReport[2] + "  - تليفون  : " + infoReport[5] + " - فاكس  : " + infoReport[4] + " -  بريد الكترونى  : " + infoReport[3];
        //    Document doc = new Document(PageSize.A4, 0, 0, 0, 44);
        //    var memoryStream = PdfBase.OpenDocument(doc, "1", false, "", isFooter, footerData);

        //    PdfPCell cell = null;
        //    PdfPTable tblParent = new PdfPTable(57);
        //    tblParent.DefaultCell.Padding = 0;
        //    tblParent.WidthPercentage = 100;
        //    tblParent.DefaultCell.BorderWidth = 0;
        //    tblParent.HorizontalAlignment = Element.ALIGN_CENTER;
        //    tblParent.SplitLate = false;
        //    tblParent.HeaderRows = 1;
        //    //  tblParent.HeaderRows = 6;

        //    PdfPTable header1 = new PdfPTable(57);
        //    header1.WidthPercentage = 100;
        //    header1.DefaultCell.BorderWidth = 0;
        //    header1.DefaultCell.BorderColor = BaseColor.BLUE;
        //    header1.HorizontalAlignment = Element.ALIGN_CENTER;
        //    header1.DefaultCell.Padding = 0;
        //    //  header1.HeaderRows = 6;
        //    #region HeaderReport


        //    #region FirstRow
        //    string headerTitle = "";
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 1, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
        //    header1.AddCell(cell);

        //    if (infoReport[1] != "")
        //    {
        //        string imageFilePathLogo = PdfBase.getReportImagePath(infoReport[1]);

        //        if (imageFilePathLogo != "")
        //        {
        //            iTextSharp.text.Image headerJpg = iTextSharp.text.Image.GetInstance(imageFilePathLogo); //infoReport[1];

        //            cell = PdfBase.drawImageCell(headerJpg, 90F, 80F, 19, 3, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
        //            header1.AddCell(cell);
        //        }
        //        else
        //        {
        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 3, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
        //            header1.AddCell(cell);
        //        }
        //    }
        //    else
        //    {
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 3, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
        //        header1.AddCell(cell);
        //    }
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], infoReport[0], 19, 3, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
        //    header1.AddCell(cell);
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "تاريخ : " + PrintDate, 19, 1, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
        //    header1.AddCell(cell);
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 1, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
        //    header1.AddCell(cell);
        //    #endregion
        //    #region secondRow
        //    if (Username == "")
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.BarCodeFont_30_NORMAL.ToString()], " تقرير الاداء الشامل للموظفين \n\n", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
        //    else
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.BarCodeFont_30_NORMAL.ToString()], "  تقرير الاداء الشامل للموظف (" + Username + ")", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);

        //    header1.AddCell(cell);
        //    #endregion
        //    #region ThirdRow

        //    if (DateFrom != "" && DateTo != "" && DateFrom != null && DateTo != null)
        //    {
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " \n من تاريخ: " + DateFrom + "  -  " + "   الى تاريخ:  " + DateTo + "", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
        //        cell.PaddingBottom = 5;
        //        cell.PaddingTop = 5;
        //        header1.AddCell(cell);
        //    }
        //    if (datetype != null && datetype != 0)
        //    {
        //        var statuname = GetDateType(datetype);
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " \n خلال فترة زمنية : " + statuname + "", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
        //        cell.PaddingBottom = 5;
        //        cell.PaddingTop = 5;
        //        header1.AddCell(cell);
        //    }
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "\n\n", 57, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
        //    header1.AddCell(cell);
        //    #endregion
        //    cell = new PdfPCell(header1);
        //    cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
        //    cell.BorderWidth = 0;
        //    cell.Colspan = 57;
        //    cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    cell.PaddingBottom = 0;
        //    tblParent.AddCell(cell);



        //    #endregion

        //    PdfPTable bodyTable = new PdfPTable(57);
        //    bodyTable.WidthPercentage = 95;
        //    bodyTable.DefaultCell.BorderWidth = 0;
        //    bodyTable.DefaultCell.BorderColor = BaseColor.BLUE;
        //    bodyTable.HorizontalAlignment = Element.ALIGN_CENTER;
        //    bodyTable.DefaultCell.Padding = 5;
        //    bodyTable.HeaderRows = 6;







        //    //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "قياس اداء الموظف في انجاز المشاريع", 20, 1, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //    //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
        //    //bodyTable.AddCell(cell);

        //    //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "قياس اداء الموظف في انجاز المهام", 32, 2, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //    //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
        //    //bodyTable.AddCell(cell);

        //    //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], "الاسم", 5, 2, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //    //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
        //    //bodyTable.AddCell(cell);
        //    headerTitle = "قياس اداء الموظف في انجاز المشاريع ";
        //    DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_18_NORMAL.ToString()], headerTitle, 1, 20, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //    headerTitle = "قياس اداء الموظف في انجاز المهام";
        //    DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_18_NORMAL.ToString()], headerTitle, 1, 32, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //    headerTitle = "الاسم";
        //    DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);





        //    headerTitle = "بدون حركة";
        //    DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_7_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //    headerTitle = "قاربت علي التاخير";
        //    DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_7_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //    headerTitle = "المتوقفة";
        //    DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_7_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //    headerTitle = "قيد التنفيذ ";
        //    DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_7_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //    headerTitle = "المتاخرة";
        //    DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_7_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);



        //    headerTitle = "نسبة الانجاز";
        //    DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_7_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //    headerTitle = "نسبة التاخير";
        //    DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_7_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //    headerTitle = "المتاخرة";
        //    DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_7_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //    headerTitle = "قاربت التاخير";
        //    DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_7_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //    headerTitle = "المنجزة";
        //    DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_7_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //    headerTitle = "الرجيعة";
        //    DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_7_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //    headerTitle = "قيد التشغيل";
        //    DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_7_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //    headerTitle = "لم تبدا";
        //    DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_7_NORMAL.ToString()], headerTitle, 1, 4, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //    headerTitle = "";
        //    DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_7_NORMAL.ToString()], headerTitle, 1, 5, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //    #region body

        //    if (dt != null && dt.Count > 0)
        //    {
        //        for (int x = 0; x < dt.Count; x++)
        //        {





        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].ProjectWithout != null ? dt[x].ProjectWithout.ToString() : ""), 4, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);
        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].ProjectAlmostfinish != null ? dt[x].ProjectAlmostfinish.ToString() : ""), 4, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);
        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].ProjectStoped != null ? dt[x].ProjectStoped.ToString() : ""), 4, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);
        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].ProjectInProgress != null ? dt[x].ProjectInProgress.ToString() : ""), 4, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);
        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].ProjectLate != null ? dt[x].ProjectLate.ToString() : ""), 4, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);




        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].CompletePercentage != 0 ?  dt[x].CompletePercentage.ToString() + " % ": "0"), 4, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);

        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].LatePercentage != 0 ?  dt[x].LatePercentage.ToString() + " % ": "0"), 4, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);

        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].Latetask.ToString()), 4, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);

        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].AlmostLate != null ? dt[x].AlmostLate.ToString() : ""), 4, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);

        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].Completed != null ? dt[x].Completed.ToString() : ""), 4, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);

        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].Retrived != null ? dt[x].Retrived.ToString() : ""), 4, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);

        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].Inprogress != null ? dt[x].Inprogress.ToString() : ""), 4, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);

        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].Notstarted != null ? dt[x].Notstarted.ToString() : ""), 4, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);

        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].UserName != null ? dt[x].UserName.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);

        //        }


        //    }

        //    else
        //    {
        //        var titlenodata = "لا توجد نتائج ";
        //        var callNodata = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_BOLD_Red.ToString()], titlenodata, 57, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        bodyTable.AddCell(callNodata);
        //    }
        //    int TLatetask = 0, TInprogress = 0, TNotstarted = 0, TCompleted = 0, TRetrived = 0, Tcompercentage = 0, Tlateperc = 0;
        //    int TAlmostLate = 0, TProjectLate = 0, TProjectInProgress = 0, TProjectStoped = 0, TProjectWithout = 0, TProjectAlmostfinish = 0;

        //    if (dt != null && dt.Count > 0)
        //    {
        //        for (int x = 0; x < dt.Count; x++)
        //        {
        //            TLatetask += int.Parse(dt[x].Latetask);
        //            TInprogress += int.Parse(dt[x].Inprogress);
        //            TNotstarted += int.Parse(dt[x].Notstarted);
        //            TCompleted += int.Parse(dt[x].Completed);
        //            TRetrived += int.Parse(dt[x].Retrived);
        //            Tcompercentage += dt[x].CompletePercentage;
        //            Tlateperc += dt[x].LatePercentage;
        //            TAlmostLate += int.Parse(dt[x].AlmostLate);
        //            TProjectLate += int.Parse(dt[x].ProjectLate);
        //            TProjectInProgress += int.Parse(dt[x].ProjectInProgress);
        //            TProjectStoped += int.Parse(dt[x].ProjectStoped);
        //            TProjectAlmostfinish += int.Parse(dt[x].ProjectAlmostfinish);
        //            TProjectWithout += int.Parse(dt[x].ProjectWithout);


        //        }





        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (TProjectWithout.ToString() ), 4, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
        //        bodyTable.AddCell(cell);
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (TProjectAlmostfinish.ToString()), 4, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
        //        bodyTable.AddCell(cell);
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (TProjectStoped.ToString()), 4, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
        //        bodyTable.AddCell(cell);
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (TProjectInProgress.ToString()), 4, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
        //        bodyTable.AddCell(cell);
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (TProjectLate.ToString()), 4, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
        //        bodyTable.AddCell(cell);




        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (Tcompercentage.ToString() + " % "), 4, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
        //        bodyTable.AddCell(cell);

        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (Tlateperc.ToString() + " % "), 4, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
        //        bodyTable.AddCell(cell);

        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (TLatetask.ToString()), 4, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
        //        bodyTable.AddCell(cell);

        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (TAlmostLate.ToString() ), 4, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
        //        bodyTable.AddCell(cell);

        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (TCompleted.ToString()), 4, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
        //        bodyTable.AddCell(cell);

        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (TRetrived.ToString()), 4, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
        //        bodyTable.AddCell(cell);

        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (TInprogress.ToString() ), 4, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
        //        bodyTable.AddCell(cell);

        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (TNotstarted.ToString()), 4, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
        //        bodyTable.AddCell(cell);

        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], ("الاجمالـــي"), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
        //        bodyTable.AddCell(cell);


        //    }




        //    cell = new PdfPCell(bodyTable);
        //    cell.Colspan = (57);
        //    cell.BorderWidth = 0;
        //    cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    cell.PaddingBottom = 0;
        //    tblParent.AddCell(cell);
        //    #endregion

        //    doc.Add(tblParent);
        //    doc.Close();
        //    return memoryStream.GetBuffer();
        //}







        public static byte[] PrintCostProjects(List<ProjectVM> dt, string UserName, string[] infoReport)
        {
            bool isFooter = (infoReport[6] != null && infoReport[6] != "" || infoReport[6] != "0") ? true : false;//send from user
            string footerData = "" + infoReport[0] + " - " + infoReport[2] + "  - تليفون  : " + infoReport[5] + " - فاكس  : " + infoReport[4] + " -  بريد الكترونى  : " + infoReport[3];
            Document doc = new Document(PageSize.A4, 0, 0, 0, 44);
            PdfWriter PdfWriter = null;
            var memoryStream = PdfBase.OpenDocumentProjectReport(ref PdfWriter, doc, "1", false, "", isFooter, footerData);

            PdfContentByte cb = PdfWriter.DirectContent;
            // Bottom left coordinates x & y, followed by width, height and radius of corners.
            cb.RoundRectangle(0f, 0f, 0f, 0f, 0f);
            cb.Stroke();

            PdfPCell cell = null;
            PdfPTable tblParent = new PdfPTable(57);
            tblParent.DefaultCell.Padding = 0;
            tblParent.WidthPercentage = 100;
            tblParent.DefaultCell.BorderWidth = 0;
            tblParent.HorizontalAlignment = Element.ALIGN_CENTER;
            tblParent.SplitLate = false;
            //  tblParent.HeaderRows = 6;

            PdfPTable header1 = new PdfPTable(57);
            header1.WidthPercentage = 100;
            header1.DefaultCell.BorderWidth = 0;
            header1.DefaultCell.BorderColor = BaseColor.BLUE;
            header1.HorizontalAlignment = Element.ALIGN_CENTER;
            header1.DefaultCell.Padding = 0;
            //  header1.HeaderRows = 6;
            #region HeaderReport


            #region FirstRow
            string headerTitle = "";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 1, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            header1.AddCell(cell);

            if (infoReport[1] != "")
            {
                string imageFilePathLogo = PdfBase.getReportImagePath(infoReport[1]);

                if (imageFilePathLogo != "")
                {
                    iTextSharp.text.Image headerJpg = iTextSharp.text.Image.GetInstance(imageFilePathLogo); //infoReport[1];

                    cell = PdfBase.drawImageCell(headerJpg, 90F, 80F, 19, 3, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
                    header1.AddCell(cell);
                }
                else
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 3, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                    header1.AddCell(cell);
                }
            }
            else
            {
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 3, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
                header1.AddCell(cell);
            }
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], infoReport[0], 19, 3, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            header1.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "تاريخ : " + PrintDate, 19, 1, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
            header1.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 1, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
            header1.AddCell(cell);
            #endregion
            #region secondRow
            if (UserName == "")
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_UNDERLINE.ToString()], " تكلفة المشاريع \n", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            else
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_UNDERLINE.ToString()], " تكلفة المشروع  (" + UserName + ")\n", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);

            header1.AddCell(cell);

            #endregion
            #endregion

            //end of line two
            PdfPTable headerbodyTable = new PdfPTable(57);
            headerbodyTable.WidthPercentage = 100;
            headerbodyTable.DefaultCell.BorderWidth = 0;
            headerbodyTable.HorizontalAlignment = Element.ALIGN_CENTER;
            headerbodyTable.DefaultCell.Padding = 0;


            headerTitle = "التكلفة";
            DrawTableHeader(headerbodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 8, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "نوع المشروع الفرعي";
            DrawTableHeader(headerbodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 13, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "نوع المشروع";
            DrawTableHeader(headerbodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 13, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "اسم العميل";
            DrawTableHeader(headerbodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 14, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "رقم المشروع";
            DrawTableHeader(headerbodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 9, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            #region body
            PdfPTable bodyTable = new PdfPTable(57);
            List<chartDataClass> chartData = new List<chartDataClass>();

            byte[] ChartBytes = new byte[0];
            int Row_No = 0;
            if (dt != null && dt.Count > 0)
            {
                for (int x = 0; x < dt.Count; x++)
                {
                    var valCostE = (dt[x].CostE + dt[x].CostE_Depit) - dt[x].CostE_Credit;

                    var valCostS = dt[x].CostS  + dt[x].Oper_expeValue;
                    var diff = valCostE - valCostS;
                    if (Row_No < 23)
                    {
                        chartData.Add(new chartDataClass()
                        {
                            Percentage = (diff != null ? float.Parse(diff.ToString()) : 0),
                            ProjectNo = dt[x].ProjectNo
                        });

                        Row_No++;

                        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (diff != null ? diff.ToString() : ""), 8, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                        bodyTable.AddCell(cell);

                        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].ProjectSubTypeName != null ? dt[x].ProjectSubTypeName.ToString() : ""), 13, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                        bodyTable.AddCell(cell);

                        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].ProjectTypesName != null ? dt[x].ProjectTypesName.ToString() : ""), 13, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                        bodyTable.AddCell(cell);

                        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].CustomerName_W != null ? dt[x].CustomerName_W.ToString() : ""), 14, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                        bodyTable.AddCell(cell);

                        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].ProjectNo != null ? dt[x].ProjectNo.ToString() : ""), 9, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                        bodyTable.AddCell(cell);

                    }
                    if (Row_No >= 23 || x == dt.Count - 1)
                    {
                        Row_No = 0;
                        if (Row_No == 0)
                        {
                            cell = new PdfPCell(header1);
                            cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                            cell.BorderWidth = 0;
                            cell.Colspan = 57;
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cell.PaddingBottom = 0;
                            tblParent.AddCell(cell);

                            ChartBytes = reportHelper.PopulateChartForCost(chartData);
                            Image img = Image.GetInstance(ChartBytes);
                            cell = PdfBase.drawImageCell(img, 600F, 150F, 57, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
                            tblParent.AddCell(cell);

                            cell = new PdfPCell(headerbodyTable);
                            cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                            cell.BorderWidth = 0;
                            cell.Colspan = 57;
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cell.PaddingBottom = 0;
                            tblParent.AddCell(cell);

                            cell = new PdfPCell(bodyTable);
                            cell.Colspan = (57);
                            cell.BorderWidth = 0;
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cell.PaddingBottom = 0;
                            tblParent.AddCell(cell);

                            if (x < dt.Count - 1)
                            {
                                doc.Add(tblParent);

                                doc.NewPage();
                                cb.RoundRectangle(0f, 0f, 0f, 0f, 0f);
                                cb.Stroke();

                                tblParent = new PdfPTable(57);
                                tblParent.DefaultCell.Padding = 0;
                                tblParent.WidthPercentage = 100;
                                tblParent.DefaultCell.BorderWidth = 0;
                                tblParent.HorizontalAlignment = Element.ALIGN_CENTER;
                                tblParent.SplitLate = false;
                                //     tblParent.HeaderRows = 6;

                                bodyTable = new PdfPTable(57);
                                bodyTable.DefaultCell.Padding = 5;
                                bodyTable.WidthPercentage = 100;
                                bodyTable.DefaultCell.BorderWidth = 0;
                                bodyTable.HorizontalAlignment = Element.ALIGN_CENTER;
                                bodyTable.SplitLate = false;

                                chartData = new List<chartDataClass>();
                            }

                        }
                    }

                }

            }
            else
            {
                cell = new PdfPCell(header1);
                cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                cell.BorderWidth = 0;
                cell.Colspan = 57;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.PaddingBottom = 0;
                tblParent.AddCell(cell);

                cell = new PdfPCell(headerbodyTable);
                cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                cell.BorderWidth = 0;
                cell.Colspan = 57;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.PaddingBottom = 0;
                tblParent.AddCell(cell);

                var titlenodata = "لا توجد نتائج ";
                var callNodata = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_BOLD_Red.ToString()], titlenodata, 57, 0, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                bodyTable.AddCell(callNodata);

                cell = new PdfPCell(bodyTable);
                cell.Colspan = (57);
                cell.BorderWidth = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.PaddingBottom = 0;
                tblParent.AddCell(cell);

            }

            #endregion

            doc.Add(tblParent);
            doc.Close();
            return memoryStream.GetBuffer();
        }



        public static byte[] SupervisionRep(List<Pro_SupervisionDetailsVM> dt, string fromdate, string todate, string[] infoReport)
        {
            bool isFooter = (infoReport[6] != null && infoReport[6] != "" || infoReport[6] != "0") ? true : false;//send from user
            string footerData = "" + infoReport[0] + " - " + infoReport[2] + "  - تليفون  : " + infoReport[5] + " - فاكس  : " + infoReport[4] + " -  بريد الكترونى  : " + infoReport[3];//get from data base ----Not fixed



            Document doc = new Document(PageSize.A4, 10, 10, 30, 44);
            var memoryStream = PdfBase.OpenDocument(doc, "1", false, "", isFooter, footerData);//طباعه افقية

            var headerTitle = "";

            PdfPCell cell = null;
            PdfPTable tblParent = new PdfPTable(1);
            tblParent.DefaultCell.Padding = 0;
            tblParent.WidthPercentage = 100;
            tblParent.DefaultCell.BorderWidth = 0;
            tblParent.HorizontalAlignment = Element.ALIGN_CENTER;
            tblParent.SplitLate = false;
            tblParent.HeaderRows = 1;

            #region FirstRow
            PdfPTable headerTbl = new PdfPTable(57);
            headerTbl.DefaultCell.Padding = 10;
            headerTbl.WidthPercentage = 95;
            headerTbl.DefaultCell.BorderWidth = 0;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            headerTitle = "";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 1, 0, 0, 0, 0, 15, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
            cell.Padding = 10;
            headerTbl.AddCell(cell);

            if (infoReport[1] != "")
            {
                string imageFilePathLogo = PdfBase.getReportImagePath(infoReport[1]);

                if (imageFilePathLogo != "")
                {
                    Image headerJpg = Image.GetInstance(imageFilePathLogo); //infoReport[1];

                    cell = PdfBase.drawImageCell(headerJpg, 90F, 80F, 19, 3, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
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



            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], infoReport[0], 19, 3, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            cell.Padding = 10;
            headerTbl.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "تاريخ " + PrintDate + "", 19, 1, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
            cell.Padding = 10;
            headerTbl.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 1, 0, 0, 0, 0, 15, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
            cell.Padding = 10;
            headerTbl.AddCell(cell);

            #endregion




            #region secondRow


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], " اسم المرحلة : " + dt[0].PhaseName + "", 18, 1, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            cell.Padding = 5;
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], " رقم المشروع : " + dt[0].ProjectNo + "", 13, 1, 0, 0, 0, 0, 0, Element.ALIGN_MIDDLE, Element.ALIGN_MIDDLE);
            cell.Padding = 5;
            headerTbl.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 13, 1, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_RIGHT);
            //cell.Padding = 5;
            //headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], " العميل : " + dt[0].CustomerName + "", 26, 1, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            cell.Padding = 5;
            headerTbl.AddCell(cell);


            #endregion

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 57, 2, 1, 1, 1, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            headerTbl.AddCell(cell);

            #region thirdRow




            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], " المكلف بها : " + dt[0].ReceivedUserName + "", 18, 1, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            cell.Padding = 5;
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], " المقاول : " + dt[0].ContractorName + "", 13, 1, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            cell.Padding = 5;
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], " تاريخه : " + dt[0].Date + "", 13, 1, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            cell.Padding = 5;
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], " رقم الاشراف : " + dt[0].SupervisionNumber + "", 13, 1, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            cell.Padding = 5;
            headerTbl.AddCell(cell);

            #endregion

            #region fourthRow

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], " مكتب المتعاون : " + dt[0].OfficeName + "", 18, 1, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            cell.Padding = 5;
            headerTbl.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], " رقم القطعة : " + dt[0].PieceNo + "", 13, 1, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            cell.Padding = 5;
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], " رقم المخطط : " + dt[0].OutlineNo + "", 13, 1, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            cell.Padding = 5;
            headerTbl.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], " رقم الرخصة : " + dt[0].LicenseNo + "", 13, 1, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            cell.Padding = 5;
            headerTbl.AddCell(cell);
            #endregion

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 57, 2, 1, 1, 1, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            headerTbl.AddCell(cell);

            #region LastRow
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_16_UNDERLINE.ToString()], " طلعة اشراف ", 57, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            cell.Padding = 10;
            headerTbl.AddCell(cell);

            //if (fromdate != "" && todate != "")
            //{
            //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " من تاريخ " + fromdate + " الى تاريخ  " + todate + "", 57, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            //    cell.PaddingBottom = 7;
            //    cell.PaddingTop = 5;
            //    headerTbl.AddCell(cell);
            //}
            #endregion

            cell = new PdfPCell(headerTbl);
            cell.Colspan = 57;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            tblParent.AddCell(cell);

            #region body
            PdfPTable bodyTable = new PdfPTable(57);
            bodyTable.WidthPercentage = 100;
            bodyTable.DefaultCell.BorderWidth = 0;
            bodyTable.DefaultCell.BorderColor = BaseColor.BLUE;
            bodyTable.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyTable.DefaultCell.Padding = 10;
            bodyTable.HeaderRows = 1;


            headerTitle = "الحالة";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 10, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "ملاحظات";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 17, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "اسم مرحلة الاستلام";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 30, 10, 10, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            var Message = "";
            if (dt != null && dt.Count > 0)
            {
                for (int x = 0; x < dt.Count; x++)
                {

                    if(dt[x].IsRead==1)
                    {
                        Message = "تم الاستلام";
                        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL_GREEN.ToString()], (Message), 10, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                        bodyTable.AddCell(cell);
                    }
                    else if (dt[x].IsRead == 2)
                    {
                        Message = "غير متوفر";
                        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL_GREEN.ToString()], (Message), 10, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                        bodyTable.AddCell(cell);
                    }
                    else
                    {
                        Message="لم يتم الاستلام";
                        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL_RED.ToString()], (Message), 10, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                        bodyTable.AddCell(cell);
                    }


                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].Note != null ? dt[x].Note.ToString() : ""), 17, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].NameAr != null ? dt[x].NameAr.ToString() : ""), 30, 4, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                }
            }
            else
            {
                var titlenodata = "لا توجد نتائج ";
                var callNodata = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_BOLD_Red.ToString()], titlenodata, 57, 0, 1, 1, 1, 1, 10, Element.ALIGN_CENTER);
                bodyTable.AddCell(callNodata);
            }


            cell = new PdfPCell(bodyTable);
            cell.Colspan = (57);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Padding = 10;
            tblParent.AddCell(cell);

            #endregion


            doc.Add(tblParent);
            doc.Close();
            return memoryStream.GetBuffer();
        }


        public static string GetStatusName(int? status)
        {
            string statusname = "";
            if (status == 1)
            {
                statusname = "لم تبدأ";
            }
            else if (status == 2)
            {
                statusname = "قيد التشغيل";
            }
            else if (status == 3)
            {
                statusname = "متوقفة";
            }
            else if (status == 4)
            {
                statusname = "منتهية";
            }
            else if (status == 7)
            {
                statusname = "رجيعة";
            }
            else if (status == 8)
            {
                statusname = "متاخرة";
            }
            return statusname;
        }



        public static string GetDateType(int? status)
        {
            string statusname = "";
            if (status == 1)
            {
                statusname = "خلال الاسبوع الماضي";
            }
            else if (status == 2)
            {
                statusname = "خلال الاسبوعين الماضيين";
            }
            else if (status == 3)
            {
                statusname = "خلال الثلاثة اسابيع الماضية";
            }
            else if (status == 4)
            {
                statusname = "خلال الشهر الماضي";
            }
            return statusname;
        }
    }
    public class chartDataClass
    {
        public float Percentage { get; set; }
        public string ProjectNo { get; set; }
    }
}
