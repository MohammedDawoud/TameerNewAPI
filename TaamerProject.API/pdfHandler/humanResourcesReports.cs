using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using TaamerProject.API.Controllers;
using TaamerProject.Models;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.API.pdfHandler

{
    public class humanResourcesReports
    {
        public static CultureInfo us = new CultureInfo("en-US");
        public static string PrintDate = DateTime.Now.ToString("yyyy-MM-dd ", us);
        internal static byte[] PrintEmployeeReport(List<EmployeesVM> dt, string[] infoReport)
        {
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
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_NORMAL.ToString()], "تقرير الموظفين", 57, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
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

            PdfPTable bodyTable = new PdfPTable(57);
            bodyTable.WidthPercentage = 95;
            bodyTable.DefaultCell.BorderWidth = 0;
            bodyTable.DefaultCell.BorderColor = BaseColor.BLUE;
            bodyTable.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyTable.DefaultCell.Padding = 5;
            bodyTable.HeaderRows = 1;

            headerTitle = "رقم الهوية";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], headerTitle, 1, 6, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "الفرع ";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], headerTitle, 1, 6, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "الجوال";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], headerTitle, 1, 6, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "البريد الإلكترونى";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], headerTitle, 1, 7, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);


            headerTitle = " المسمي الوظيفي";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], headerTitle, 1, 6, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            headerTitle = " الراتب";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            headerTitle = " الجنسية";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], headerTitle, 1, 6, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "اسم الموظف";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], headerTitle, 1, 10, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "الرقم الوظيفى";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            if (dt != null && dt.Count > 0)
            {
                for (int x = 0; x < dt.Count; x++)
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], (dt[x].NationalId != null ? dt[x].NationalId.ToString() : ""), 6, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], (dt[x].BranchName != null ? dt[x].BranchName.ToString() : ""), 6, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], (dt[x].Mobile != null ? dt[x].Mobile.ToString() : ""), 6, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], (dt[x].Email != null ? dt[x].Email.ToString() : ""), 7, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);


                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], (dt[x].JobName != null ? dt[x].JobName.ToString() : ""), 6, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], (dt[x].Salary != null ? dt[x].Salary.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], (dt[x].NationalityName != null ? dt[x].NationalityName.ToString() : ""), 6, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);


                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], (dt[x].EmployeeNameAr != null ? dt[x].EmployeeNameAr.ToString() : ""), 10, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_8_NORMAL.ToString()], (dt[x].EmployeeNo != null ? dt[x].EmployeeNo.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
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
            cell.PaddingBottom = 5;
            tblParent.AddCell(cell);

            doc.Add(tblParent);
            doc.Close();
            return memoryStream.GetBuffer();
        }

        internal static byte[] PrintEmpContractReport(DataTable dt, string[] infoReport, IAllowanceService allowanceService, IAllowanceTypeService allowanceType)
        {
            bool isFooter = (infoReport[6] != null && infoReport[6] != "" || infoReport[6] != "0") ? true : false;//send from user
            string footerData = "" + infoReport[0] + " - " + infoReport[2] + "  - تليفون  : " + infoReport[5] + " - فاكس  : " + infoReport[4] + " -  بريد الكترونى  : " + infoReport[3];
            Document doc = new Document(PageSize.A4, 0, 0, 0, 44);
            var memoryStream = PdfBase.OpenDocument(doc, "1", false, "", isFooter, footerData);

            EmpSalaryPartsVM salaryPart = (new AllowanceController(allowanceService, allowanceType).GetSalaryPartObject(((int)dt.Rows[0]["EmpId"])));

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
            headerTbl.DefaultCell.Padding = 1;
            headerTbl.WidthPercentage = 100;
            headerTbl.DefaultCell.BorderWidthBottom = 1;
            headerTbl.DefaultCell.BorderColor = BaseColor.BLUE;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            #region FirstRow
            headerTitle = "";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 1, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
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


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], infoReport[0], 19, 3, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            headerTbl.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "تاريخ : " + PrintDate, 19, 1, 0, 0, 0, 0, 5, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
            headerTbl.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 1, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
            headerTbl.AddCell(cell);

            #endregion

            #region secondRow
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_NORMAL.ToString()], " عقد عمل رقم " + dt.Rows[0]["ContractCode"] + "", 57, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            headerTbl.AddCell(cell);
            #endregion

            #endregion
            cell = new PdfPCell(headerTbl);
            cell.Colspan = 57;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            tblParent.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_NORMAL.ToString()], " \n", 57, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            tblParent.AddCell(cell);

            PdfPTable bodyTable = new PdfPTable(57);
            bodyTable.WidthPercentage = 100;
            bodyTable.DefaultCell.BorderWidth = 0;
            bodyTable.DefaultCell.BorderColor = BaseColor.BLUE;
            bodyTable.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyTable.DefaultCell.Padding = 1;
            bodyTable.HeaderRows = 1;

            string arabDay = getArabNameOfday(dt.Rows[0]["ContDayname"].ToString());
            string dayDate = (new GeneralController()).ConvertDateCalendar((DateTime)dt.Rows[0]["AddDate"], "Gregorian", "en-US");

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (string.Format("إنه في يوم  {0} {1} تم الأتفاق بين كل من", dayDate, arabDay)), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], ("-: إنه في يوم " + arabDay + " " + dayDate + "  تم الأتفاق بين كل من"), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], ("الطرف الأول: " + " " + dt.Rows[0]["Org_NameAr"].ToString() + " - سجل تجاري رقم " + dt.Rows[0]["TaxCode"].ToString() + " - وعنوانها " + dt.Rows[0]["OR_Address"].ToString()), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (" ص.ب" + " " + dt.Rows[0]["Mailbox"].ToString() + " البريد الالكترونى" + " " + dt.Rows[0]["OR_Email"].ToString() + " - هاتف " + dt.Rows[0]["OR_Mobile"].ToString() + " فاكس " + " " + dt.Rows[0]["Fax"].ToString()), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (" البريد الالكترونى" + " " + dt.Rows[0]["OR_Email"].ToString() + " الرمز البريدى " + "---------------------" + " - هاتف " + dt.Rows[0]["OR_Mobile"].ToString() + " فاكس " + " " + dt.Rows[0]["Fax"].ToString()), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            //bodyTable.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (string.Format("ويمثلها في هذا العقد السيد/ {0} بصفته  {1} ويشار إليه في ما بعد بالطرف الأول", dt.Rows[0]["RepEmpName"].ToString(), dt.Rows[0]["PerSe"].ToString())), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            DataRow row = dt.Rows[0];
            string line = string.Format("الطرف الثاني : {0} الجنسية {1}، بطاقة الهوية {2}، عنوانه {3}",
                row["Emp_NameAr"].ToString(),
                row["EmpNationality"].ToString(),
                row["NationalId"].ToString(),
                row["Address"].ToString()
                );
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], line, 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            string line2 = string.Format(", جواز سفر رقم {0} صادر من {1} بتاريخ {2}، هاتف {3}، البريد الالكترونى {4}  ",
                row["PassportNo"].ToString(),
                row["PassportSource"].ToString(),
                row["PassportNoDate"].ToString(),
                row["Emp_Mobile"].ToString(),
                row["Email"].ToString());

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], line2, 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], "ويشار إليه فيما بعد بالطرف الثاني", 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], ("\n"), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (" بعد أن أقر الطرفـان بأهليتهمـا المعتبرة شرعا. ونظاما لإبرام مثل هذا العقـد. فقد اتفقا على ما يلي : "), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], ("البند الأول موضوع العقد:-"), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (" اتفق الطرفان على أن يعمل الطرف الثاني لدى الطرف الأول تحت إدارته، أو إشرافه بوظيفة (" + dt.Rows[0]["EmpJob"].ToString() + " ). ومباشرة الأعمال التي يكلف بها بما "), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (" يتناسب مع قدراته العملية والفنية وفقا لاحتياجات العمل  وبما لا يتعارض مع الضوابط "), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], ("المنصوص عليها في المواد من نظام العمل (الثامنة والخمسون ، والتاسعة والخمسون ، والستون)"), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            if (dt.Rows[0]["ContTypeId"].ToString() == "2")
            {
                headerTitle = " اتفق الطرفان على أن يكون هذا العقد غير محدد المدة، على أن يبدأ في تاريخ   " + dt.Rows[0]["StartDateTxt"].ToString() + ".";

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], headerTitle, 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
                bodyTable.AddCell(cell);
            }
            else
            {
                headerTitle = " اتفق الطرفان على أن يكون  مدة هذا العقد " + dt.Rows[0]["ContDuration"].ToString() + " وتبدأ من تاريخ مباشرة الطرف الثانى العمل  " + dt.Rows[0]["StartDateTxt"].ToString() + " وتنتهى فى " + dt.Rows[0]["EndDateTxt"].ToString() + ".";
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], headerTitle, 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
                bodyTable.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], " وتتجدد لمدة او لمدد مماثلة.ما لم يشعر احد الطرفين الاخر خطيا بعدم رغبته فى التجديد قبل ثلاثين يوما من تاريخ انتهاء العمل  ", 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
                bodyTable.AddCell(cell);
            }
            if (dt.Rows[0]["ProbationTypeId"].ToString() == "1")
            {
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (" يخضع الطرف الثانى لفترة تجربه مدتها " + dt.Rows[0]["ProbationDuration"].ToString() + " يوما تبدأ من تاريخ مباشرته للعمل ولا يدخل في حسابها إجازة عيدي الفطر والأضحى والإجازة المرضية  "), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
                bodyTable.AddCell(cell);

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], ("ويكون للطرفين / الطرف الأول الحق فى إنهاء العقد خلال هذه الفترة"), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
                bodyTable.AddCell(cell);
            }
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], ("البند الثاني: أيام ،و ساعات العمل :-\n"), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (" تحدد أيام العمل العادية بـ  " + dt.Rows[0]["Workingdaysperweek"].ToString() + " أيام في الأسبوع، وتحدد ساعات العمل بـ (" + dt.Rows[0]["Dailyworkinghours"].ToString() + " ساعات يومياً " + " و بــ " + dt.Rows[0]["Workinghoursperweek"].ToString() + " ساعة عمل أسبوعياً) ويلتـزم الطـرف الأول "), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], ("بـأن يدفـع الطـرف الثاني أجـراً إضافيـاً عـن ساعـات العمـل الإضافيـة يوازي أجر الساعة، مضافا إليه (50)%  من أجره الأساسي."), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], ("البند الثالث : التزامات الطرف الأول :-\n"), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], ("يدفع الطرف الأول للطرف الثاني ما يلي : -"), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], ("الراتب وبدل السكن"), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            TaamerProject.Models.Common.Utilities util = new TaamerProject.Models.Common.Utilities(dt.Rows[0]["FreelanceAmount"].ToString());
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], string.Format("1.	الراتب الأساسي           {0} ريال سعودي ({1}) يستحق في نهاية كل شهر ميلادي", dt.Rows[0]["FreelanceAmount"].ToString(), util.GetNumberAr()), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            util = new TaamerProject.Models.Common.Utilities(salaryPart.HousingAllowance.AllowanceAmount.ToString());
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], string.Format("2.	بدل السكن                  {0} ريال سعودي ({1})  يستحق في نهاية كل شهر ميلادي", salaryPart.HousingAllowance.AllowanceAmount.ToString(), util.GetNumberAr()), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            //util = new Bayanateck.TameerPro.DataModel.Utilities(salaryPart.Transportation.AllowanceAmount.ToString());
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], string.Format("3.	بدل المواصلات            {0} ريال سعودي ({1}) يستحق في نهاية كل شهر ميلادي", salaryPart.Transportation.AllowanceAmount.ToString(), util.GetNumberAr()), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            //bodyTable.AddCell(cell);

            //util = new Bayanateck.TameerPro.DataModel.Utilities(salaryPart.Communication.AllowanceAmount.ToString());
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], string.Format("4.	بدل اتصال                  {0} ريال سعودي ({1}) يستحق في نهاية كل شهر ميلادي", salaryPart.Communication.AllowanceAmount.ToString(), util.GetNumberAr()), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            //bodyTable.AddCell(cell);

            //util = new Bayanateck.TameerPro.DataModel.Utilities(salaryPart.Profession.AllowanceAmount.ToString());
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], string.Format("5.	بدل مهنة                    {0} ريال سعودي ({1}) يستحق في نهاية كل شهر ميلادي", salaryPart.Profession.AllowanceAmount.ToString(), util.GetNumberAr()), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            //bodyTable.AddCell(cell);
            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (" يدفع الطرف الاول للطرف الثاني أجرا أساسيا قدره ( " + dt.Rows[0]["FreelanceAmount"].ToString() + " )فقط ر سعودي، يستحق في نهاية كل شهر . "), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            //bodyTable.AddCell(cell);

            if (!string.IsNullOrEmpty(dt.Rows[0]["Clause"].ToString()))
            {

                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (" كما يلتزم الطرف الأول للطرف الثانى بالاتى "), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
                bodyTable.AddCell(cell);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], ((i + 1).ToString() + ". " + dt.Rows[i]["Clause"].ToString() + "\n"), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
                    bodyTable.AddCell(cell);
                }
            }

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (" يستحق الطرف الثاني عن كل عام، إجازة سنوية مدتها (" + dt.Rows[0]["Durationofannualleave"].ToString() + ") يوما مدفوعة الأجر، ويحدد الطرف الأول تاريخها خلال سنة الاستحقاق؛ وفقا لظروف"), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], ("  العمل، على أن يتم دفع أجر الإجازة مقدما عند استحقاقها : وللطرف الأول تأجيل الإجازة بعد نهاية سنة استحقاقها لمدة لا تزيد عن (90) يوماً، كما له"), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], ("  بموافقة الطرف الثاني كتابة، تأجيلها إلى نهاية السنة التالية لسنة الاستحقاق، وذلك حسب مقتضيات ظروف العمل ."), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], ("\n\n "), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            //bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (" يلتزم الطرف الأول بتوفير الرعاية الطبية للطرف الثاني بالتأمين الصحي، وفقا لحكم نظام الضمان الصحي التعاوني. "), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (" يلتزم الطرف الأول بتسجيل الطرف الثاني لدى المؤسسة العامة للتأمينات الأجتماعية، وسداد الاشتراكات حسب أنظمتها."), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], ("البند الرابع: التزامات الطرف الثاني: -  \n"), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (" أن ينجز العمل الموكل إليه: وفقا لأصول المهنة، ووفق تعليمات الطرف الأول، إذا لم يكن في هذه التعليمات ما يخالف العقد، أو النظام، أو الآداب. "), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (" العامة، ولم يكن في تنفيذها ما يعرضه للخطر."), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (" وأن يعتني عناية كافية بالأدوات، والمهمات المسندة إليه، والخامات المملوكة للطرف الأول؛ الموضوعة تحت تصرفه، أو التي تكون في عهدته، وأن "), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (" يعيد إلى الطرف الأول المواد غير المستهلكة."), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (" وأن يقدم كل عون، ومساعدة دون أن يشترط لذلك أجرا إضافيا في حالة الخطر التي تهدد سلامة مكان العمل، أو الأشخاص العاملين فيه "), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (" أن يخضع وفقا لطلب الطرف الأول للفحوص الطبية التي يرغب في إجرائها عليه قبل الالتحاق بالعمل، أو أثناء التحقق في خلوه من الأمراض"), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (" المهنية، أو السارية. "), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (" يلتزم الطرف الثاني بحسن السلوك، والأخلاق أثناء العمل، وفي جميع الأوقات يلتزم بالأنظمة، والعرف ، والعادات، والآداب المرعية في المملكة "), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], ("  العربية السعودية، وكذلك بالقواعد، واللوائح، والتعليمات المعمول بها لدى الطرف الأول، ويتحمل كافة الغرامات المالية الناتجة عن مخالفته لتلك "), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);




            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (" الأنظمة."), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], ("البند الخامس : انتهاء العقد أو إنهاؤه : - \n"), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (" يحق للطرف الأول فسخ العقد دون مكافأة، أو إشعار للطرف الثاني، أو تعويضه، شريطة إتاحة الفرصة للطرف الثاني في إبداء أسباب معارضته للفسخ، وذلك طبقا للحالات الواردة في المادة (الثمانون) من نظام العمل . "), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (" يحق للطرف الثاني ترك العمل، وإنهاء العقد دون إشعار الطرف الأول مع احتفاظه بحقه في الحصول على كافة مستحقاته. طبقا للحالة الواردة في المادة (الحادية والثمانون) من نظام العمل . "), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (" في حالة إنهاء العقد من قبل أحد الطرفين قبل انقضاء مدته دون سبب مشروع، يحق للطرف الآخر مقابل هذا الانهاء تعويضا قدره  (" + dt.Rows[0]["CompensationBothParty"].ToString() + ") ريال سعودي."), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);

            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], ("البند السادس : مكافأة نهاية الخدمة : -\n"), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (" يستحق الطرف الثاني عند إنهاء العلاقة التعاقدية من قبل الطرف الأول، أو باتفاق الطرفين، أو بانتهاء مدة العقد، أو نتيجة لقوة قاهرة، "), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], ("مكافأة قدرها أجر خمسة عشر يوما عن كل سنة من السنوات الخمس الأولى، وأجر شهر عن كل سنة من السنوات التالية، "), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], ("ويستحق العامل مكافأة عن أجزاء السنة بنسبة ما قضاه منها في العمل، وتحسب المكافأة على أساس الأجر . "), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (" إذا كان انتهاء علاقة العمل بسبب استقالة الطرف الثاني، يستحق في هذه الحالة ثلث المكافأة، بعد خدمة لا تقل مدتها عن سنتين متتاليتين،. "), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], ("  ولا تزيد على خمس سنوات، ويستحق ثلثيها، إذا زادت مدة خدمته على خمس سنوات متتالية، ولم تبلغ عشر سنوات، ويستحق المكافأة كاملة، إذا بلغت مدة خدمته عشر سنوات فأكثر. "), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], ("البند السابع : النظام الواجب التطبيق، والاختصاص القضائي : -\n"), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (" يخضع هذا العقد لنظام العمل، ولائحته التنفيذية، والقرارات الصادرة تنفيذا له؛ في كل ما لم يرد به نص في هذا العقد، ويحل هذا العقد محل كافة الاتفاقيات، والعقود السابقة الشفهية منها، أو الكتابية إن وجدت. "), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (" في حالة نشوء خلاف بين الطرفين حول هذا العقد، فإن الاختصاص القضائي ينعقد للجهة المختصة بنظر القضايا العمالية في المملكة العربية السعودية. "), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], ("البند الثامن : الإخطارات ، والإشعارات، ونسخ العقد  : -\n"), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (" تتم الإخطارات ، والإشعارات بين الطرفين كتابة على العنوان الموضح بصدر هذا العقد عن طريق البريد المسجل،. "), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], ("  أو البريد الممتاز، أو البريد الإلكتروني لكل من الطرفين، ويلتزم كل طرف بإشعار الطرف الأخر خطيا في حال تغييره للعنوان الخاص به، أو تغيير البريد الإلكتروني، وإلا اعتبر العنوان، أو البريد الإلكتروني المدونان أعلاه، هما المعمول بهما نظاما. "), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (" حرر هذا العقد من نسختين أصليتين، وقد تسلم كل طرف نسخة منه للعمل بموجبها .  \n"), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], ("والله الموفق ،،،،، "), 75, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (""), 10, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (" الطرف الثانى "), 10, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (""), 15, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (" الطرف الأول "), 10, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (""), 12, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (""), 7, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], ("الاسم  : " + dt.Rows[0]["Emp_NameAr"].ToString() + " "), 20, 4, 0, 0, 0, 0, 5, Element.ALIGN_CENTER);
            bodyTable.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (""), 7, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], ("الاسم : " + dt.Rows[0]["Org_NameAr"].ToString() + " "), 20, 4, 0, 0, 0, 0, 5, Element.ALIGN_CENTER);
            bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (""), 10, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (""), 7, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], ("التوقيع : ...................."), 20, 4, 0, 0, 0, 0, 5, Element.ALIGN_CENTER);
            bodyTable.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (""), 7, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], ("التوقيع : ...................."), 20, 4, 0, 0, 0, 0, 5, Element.ALIGN_CENTER);
            bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (""), 10, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (""), 7, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], ("الختم"), 50, 4, 0, 0, 0, 0, 5, Element.ALIGN_CENTER);
            bodyTable.AddCell(cell);

            cell = new PdfPCell(bodyTable);
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

        internal static byte[] printEmpsEarlyDeparture(List<LateVM> dt, string from, string to, string[] infoReport)
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
            if (from != "" && to != "")
            {
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_NORMAL.ToString()], " الخروج المبكر   \n", 57, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
                header1.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_NORMAL.ToString()], " فى الفترة من " + from + "الى " + to + "", 57, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
                header1.AddCell(cell);
            }
            else
            {
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.BarCodeFont_30_NORMAL.ToString()], " الخروج المبكر", 57, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
                header1.AddCell(cell);
            }

            cell = new PdfPCell(header1);
            cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            cell.BorderWidth = 0;
            cell.Colspan = 57;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 0;
            tblParent.AddCell(cell);
            #endregion


            PdfPTable bodyTable = new PdfPTable(57);
            bodyTable.WidthPercentage = 95;
            bodyTable.DefaultCell.BorderWidth = 0;
            bodyTable.DefaultCell.BorderColor = BaseColor.BLUE;
            bodyTable.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyTable.DefaultCell.Padding = 5;
            bodyTable.HeaderRows = 1;


            headerTitle = "إنصراف مبكر";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 8, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "إنصراف فترة ثانية";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 8, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "إنصراف مبكر";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 8, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "إنصراف فترة اولى";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 8, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "التاريخ";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 8, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "الرقم الوظيفى";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 8, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "اسم الموظف";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 9, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            #region body

            if (dt != null && dt.Count > 0)
            {
                for (int x = 0; x < dt.Count; x++)
                {

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].MoveTimeStringLeave2 != null ? dt[x].MoveTimeStringLeave2.ToString() : ""), 8, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].TimeLeave2 != null ? dt[x].TimeLeave2.ToString() : ""), 8, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].MoveTimeStringLeave1 != null ? dt[x].MoveTimeStringLeave1.ToString() : ""), 8, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].TimeLeave1 != null ? dt[x].TimeLeave1.ToString() : ""), 8, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].DateDay != null ? dt[x].DateDay.ToString() : ""), 8, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].EmpNo != null ? dt[x].EmpNo.ToString() : ""), 8, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].FullName != null ? dt[x].FullName.ToString() : ""), 9, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
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
            tblParent.AddCell(cell);
            #endregion

            doc.Add(tblParent);
            doc.Close();
            return memoryStream.GetBuffer();
        }

        internal static byte[] printEmpsNotLoggedOut(List<NotLoggedOutVM> dt, string fromDate, string toDate, string[] infoReport)
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
            if (fromDate != "" && toDate != "")
            {
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_NORMAL.ToString()], " الموظفون اللذين لم يسجلوا خروج", 57, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
                header1.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_NORMAL.ToString()], "فى الفترة من " + fromDate + " الى  " + toDate + "\n", 57, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
                header1.AddCell(cell);

            }
            else
            {
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_NORMAL.ToString()], "الموظفون اللذين لم يسجلوا خروج ", 57, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
                header1.AddCell(cell);
            }

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
            bodyTable.HeaderRows = 1;



            headerTitle = "الفرع";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 10, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "التاريخ";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 10, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "الرقم الوظيفى";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 10, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "اسم الموظف";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 27, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            #region body

            if (dt != null && dt.Count > 0)
            {
                for (int x = 0; x < dt.Count; x++)
                {

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].BranchName != null ? dt[x].BranchName.ToString() : ""), 10, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].CheckTime != null ? dt[x].CheckTime.ToString() : ""), 10, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].EmpNo != null ? dt[x].EmpNo.ToString() : ""), 10, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].FullName != null ? dt[x].FullName.ToString() : ""), 27, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);
                }
            }

            else
            {
                var titlenodata = "لا توجد نتائج ";
                var callNodata = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_BOLD_Red.ToString()], titlenodata, 57, 1, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                bodyTable.AddCell(callNodata);
            }
            cell = new PdfPCell(bodyTable);
            cell.Colspan = (57);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 0;
            tblParent.AddCell(cell);
            #endregion

            doc.Add(tblParent);
            doc.Close();
            return memoryStream.GetBuffer();
        }
        internal static byte[] printAttendanceData(List<LateVM> dt, string fromDate, string toDate, string[] infoReport)
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
            if (fromDate != "" && toDate != "")
            {
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_NORMAL.ToString()], "حضور وانصراف الموظفين", 57, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
                header1.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_NORMAL.ToString()], "فى الفترة من " + fromDate + " الى  " + toDate + "\n", 57, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
                header1.AddCell(cell);
            }
            else
            {
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_22_NORMAL.ToString()], "حضور وانصراف الموظفين ", 57, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
                header1.AddCell(cell);
            }

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
            bodyTable.HeaderRows = 1;

            headerTitle = "إنصراف فترة ثانية";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 8, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "حضور فترة ثانية";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 8, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "انصراف فترة اولى";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 8, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "حضور فترة اولى";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 8, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "التاريخ";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 8, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "الرقم الوظيفى";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 8, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "اسم الموظف";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 9, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            #region body

            if (dt != null && dt.Count > 0)
            {
                for (int x = 0; x < dt.Count; x++)
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].TimeLeave2 != null ? dt[x].TimeLeave2.ToString() : ""), 8, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].TimeJoin2 != null ? dt[x].TimeJoin2.ToString() : ""), 8, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].TimeLeave1 != null ? dt[x].TimeLeave1.ToString() : ""), 8, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].TimeJoin1 != null ? dt[x].TimeJoin1.ToString() : ""), 8, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].DateDay != null ? dt[x].DateDay.ToString() : ""), 8, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].EmpNo != null ? dt[x].EmpNo.ToString() : ""), 8, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].FullName != null ? dt[x].FullName.ToString() : ""), 9, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);
                }
            }

            else
            {
                var titlenodata = "لا توجد نتائج ";
                var callNodata = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_BOLD_Red.ToString()], titlenodata, 57, 1, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                bodyTable.AddCell(callNodata);
            }
            cell = new PdfPCell(bodyTable);
            cell.Colspan = (57);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 0;
            tblParent.AddCell(cell);
            #endregion

            doc.Add(tblParent);
            doc.Close();
            return memoryStream.GetBuffer();
        }
        internal static byte[] printEmpsLateToday(List<LateVM> dt, string[] infoReport)
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

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_22_NORMAL.ToString()], "الموظفون المتأخرون اليوم ", 57, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);

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


            PdfPTable bodyTable = new PdfPTable(57);
            bodyTable.WidthPercentage = 95;
            bodyTable.DefaultCell.BorderWidth = 0;
            bodyTable.DefaultCell.BorderColor = BaseColor.BLUE;
            bodyTable.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyTable.DefaultCell.Padding = 5;
            bodyTable.HeaderRows = 1;


            headerTitle = "زمن التأخير";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 8, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "حضور فترة ثانية";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 8, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "زمن التأخير";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 8, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "حضور فترة  اولى";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 8, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "التاريخ";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 8, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "الرقم الوظيفى";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 8, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "اسم الموظف";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 9, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            #region body

            if (dt != null && dt.Count > 0)
            {
                for (int x = 0; x < dt.Count; x++)
                {

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].MoveTimeStringJoin2 != null ? dt[x].MoveTimeStringJoin2.ToString() : ""), 8, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].TimeJoin2 != null ? dt[x].TimeJoin2.ToString() : ""), 8, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].MoveTimeStringJoin1 != null ? dt[x].MoveTimeStringJoin1.ToString() : ""), 8, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].TimeJoin1 != null ? dt[x].TimeJoin1.ToString() : ""), 8, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].DateDay != null ? dt[x].DateDay.ToString() : ""), 8, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].EmpNo != null ? dt[x].EmpNo.ToString() : ""), 8, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].FullName != null ? dt[x].FullName.ToString() : ""), 9, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
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
            tblParent.AddCell(cell);
            #endregion

            doc.Add(tblParent);
            doc.Close();
            return memoryStream.GetBuffer();
        }

        internal static byte[] printAbsenceEmpsToDay(List<AbsenceVM> dt, string[] infoReport)
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

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_22_NORMAL.ToString()], "الموظفون الغائبون اليوم", 57, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);

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
            bodyTable.HeaderRows = 1;



            headerTitle = "الفرع";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 10, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "اليوم";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 10, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "التاريخ";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 10, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "الرقم الوظيفى";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 10, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "اسم الموظف";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 17, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            #region body

            if (dt != null && dt.Count > 0)
            {
                for (int x = 0; x < dt.Count; x++)
                {

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].E_BranchId != null ? dt[x].E_BranchId.ToString() : ""), 10, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].DayNOfWeek != null ? getArabNameOfday(int.Parse(dt[x].DayNOfWeek.ToString())) : ""), 10, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].Mdate != null ? dt[x].Mdate.ToString() : ""), 10, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].EmpNo != null ? dt[x].EmpNo.ToString() : ""), 10, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].E_FullName != null ? dt[x].E_FullName.ToString() : ""), 17, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
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
            tblParent.AddCell(cell);
            #endregion

            doc.Add(tblParent);
            doc.Close();
            return memoryStream.GetBuffer();
        }

        internal static byte[] PrintCarMovement(DataTable dt, string fromDate, string toDate, string[] infoReport)
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
            if (fromDate != "" && toDate != "")
            {
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_22_NORMAL.ToString()], "حركة السيارات", 57, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
                header1.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_22_NORMAL.ToString()], " فى الفترة من " + fromDate + " الى  " + toDate + "\n", 57, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
                header1.AddCell(cell);
            }
            else
            {
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.BarCodeFont_30_NORMAL.ToString()], "حركة السيارات ", 57, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
                header1.AddCell(cell);
            }

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
            bodyTable.HeaderRows = 1;


            headerTitle = "اسم السائق";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 10, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "على صاحب العمل";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 10, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "المبلغ المستحق على السائق";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 10, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "نوع الحركة";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 10, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "السيارة";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 10, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "التاريخ";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 7, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            object totalOwnerAmount = dt.Compute("Sum(OwnerAmount)", string.Empty);
            object totalEmpAmount = dt.Compute("Sum(EmpAmount)", string.Empty);
            DataRow row = dt.NewRow();
            row["NameAr"] = "";
            row["OwnerAmount"] = totalOwnerAmount == null ? "": totalOwnerAmount.ToString();
            row["EmpAmount"] = totalEmpAmount == null ? "": totalEmpAmount.ToString();
            row["CType"] = "";
            row["Expr1"] = "";
            row["Date"] = "المجموع";
            dt.Rows.Add(row);
            #region body
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int x = 0; x < dt.Rows.Count; x++)
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[x]["NameAr"] != null ? dt.Rows[x]["NameAr"].ToString() : ""), 10, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[x]["OwnerAmount"] != null ? dt.Rows[x]["OwnerAmount"].ToString() : ""), 10, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[x]["EmpAmount"] != null ? dt.Rows[x]["EmpAmount"].ToString() : ""), 10, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[x]["CType"] != null ? dt.Rows[x]["CType"].ToString() : ""), 10, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[x]["Expr1"] != null ? dt.Rows[x]["Expr1"].ToString() : ""), 10, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[x]["Date"] != null ? dt.Rows[x]["Date"].ToString() : ""), 7, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);
                }

                #region Total
                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "المجموع", 10, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                //bodyTable.AddCell(cell);

                //object totalOwnerAmount = dt.Compute("Sum(OwnerAmount)", string.Empty);
                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (totalOwnerAmount != null ? totalOwnerAmount.ToString() : ""), 10, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                //bodyTable.AddCell(cell);

                //object totalEmpAmount = dt.Compute("Sum(EmpAmount)", string.Empty);
                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (totalEmpAmount != null ? totalEmpAmount.ToString() : ""), 10, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                //bodyTable.AddCell(cell);

                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 10, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                //bodyTable.AddCell(cell);

                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 7, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                //bodyTable.AddCell(cell);

                //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], "", 7, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                //bodyTable.AddCell(cell);

                #endregion
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
            tblParent.AddCell(cell);
            #endregion

            doc.Add(tblParent);
            doc.Close();
            return memoryStream.GetBuffer();
        }

        internal static byte[] printLateMonthEmps(List<LateVM> dt, string fromDate, string toDate, string[] infoReport)
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
            if (fromDate != "" && toDate != "")
            {
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_NORMAL.ToString()], "الموظفون المتأخرون  ", 57, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
                header1.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_NORMAL.ToString()], " فى الفترة من " + fromDate + " الى  " + toDate + "", 57, 0, 0, 0, 0, 0,10, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
                header1.AddCell(cell);
            }
            else
            {
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.BarCodeFont_30_NORMAL.ToString()], "الموظفون المتأخرون ", 57, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
                header1.AddCell(cell);
            }

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
            bodyTable.HeaderRows = 1;


            headerTitle = "زمن التأخير";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 8, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "حضور فترة ثانية";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 8, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "زمن التأخير";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 8, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "حضور فترة  اولى";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 8, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "التاريخ";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 8, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "الرقم الوظيفى";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 8, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "اسم الموظف";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 9, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            #region body

            if (dt != null && dt.Count > 0)
            {
                for (int x = 0; x < dt.Count; x++)
                {

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].MoveTimeStringJoin2 != null ? dt[x].MoveTimeStringJoin2.ToString() : ""), 8, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].TimeJoin2 != null ? dt[x].TimeJoin2.ToString() : ""), 8, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].MoveTimeStringJoin1 != null ? dt[x].MoveTimeStringJoin1.ToString() : ""), 8, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].TimeJoin1 != null ? dt[x].TimeJoin1.ToString() : ""), 8, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].DateDay != null ? dt[x].DateDay.ToString() : ""), 8, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].EmpNo != null ? dt[x].EmpNo.ToString() : ""), 8, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].FullName != null ? dt[x].FullName.ToString() : ""), 9, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
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
            tblParent.AddCell(cell);
            #endregion

            doc.Add(tblParent);
            doc.Close();
            return memoryStream.GetBuffer();
        }

        internal static byte[] printAbsenceMonthEmps(List<AbsenceVM> dt, string fromDate, string toDate, string[] infoReport)
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
            if (fromDate != "" && toDate != "")
            {
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_NORMAL.ToString()], "الموظفون الغائبون", 57, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
                header1.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_NORMAL.ToString()], " فى الفترة من " + fromDate + " الى  " + toDate + "\n", 57, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
                header1.AddCell(cell);
            }
            else
            {
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.BarCodeFont_30_NORMAL.ToString()], "الموظفون الغائبون ", 57, 0, 0, 0, 0, 0,10, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
                header1.AddCell(cell);
            }

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
            bodyTable.HeaderRows = 1;



            headerTitle = "الفرع";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 10, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "اليوم";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 10, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "التاريخ";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 10, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "الرقم الوظيفى";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 10, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "اسم الموظف";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 17, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            #region body

            if (dt != null && dt.Count > 0)
            {
                for (int x = 0; x < dt.Count; x++)
                {

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].E_BranchId != null ? dt[x].E_BranchId.ToString() : ""), 10, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].DayNOfWeek != null ? getArabNameOfday(int.Parse(dt[x].DayNOfWeek.ToString())) : ""), 10, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].Mdate != null ? dt[x].Mdate.ToString() : ""), 10, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].EmpNo != null ? dt[x].EmpNo.ToString() : ""), 10, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].E_FullName != null ? dt[x].E_FullName.ToString() : ""), 17, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
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
            tblParent.AddCell(cell);
            #endregion

            doc.Add(tblParent);
            doc.Close();
            return memoryStream.GetBuffer();
        }
        internal static byte[] PrintCarMovementAll(DataTable dt, string fromDate, string toDate, string[] infoReport)
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
            if (fromDate != "" && toDate != "")
            {
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_NORMAL.ToString()], "التقرير المجمع للسيارات", 57, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
                header1.AddCell(cell);
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_NORMAL.ToString()], "  فى الفترة من " + fromDate + " الى  " + toDate + " \n", 57, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
                header1.AddCell(cell);

            }

            else
            {
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_22_NORMAL.ToString()], "التقرير المجمع للسيارات ", 57, 0, 0, 0, 0, 0, 10, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
                header1.AddCell(cell);
            }

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
            bodyTable.HeaderRows = 1;

            headerTitle = "إجمالى";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 8, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "أخري";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 7, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "تأمين";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 7, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "مخالفات";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 7, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "صيانة";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 7, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "تغيير زيت";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 7, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "تعبئة وقود";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 7, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "السيارة";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 8, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);


            #region body

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int x = 0; x < dt.Rows.Count; x++)
                {
                    decimal sum = 0;

                    sum = decimal.Parse(dt.Rows[x]["Oil"].ToString()) + decimal.Parse(dt.Rows[x]["Zait"].ToString()) + decimal.Parse(dt.Rows[x]["Repairr"].ToString()) + decimal.Parse(dt.Rows[x]["Mokhalfa"].ToString()) + decimal.Parse(dt.Rows[x]["Taamen"].ToString()) + decimal.Parse(dt.Rows[x]["Others"].ToString());
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (sum.ToString()), 8, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[x]["Others"] != null ? dt.Rows[x]["Others"].ToString() : ""), 7, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[x]["Taamen"] != null ? dt.Rows[x]["Taamen"].ToString() : ""), 7, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[x]["Mokhalfa"] != null ? dt.Rows[x]["Mokhalfa"].ToString() : ""), 7, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[x]["Repairr"] != null ? dt.Rows[x]["Repairr"].ToString() : ""), 7, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[x]["Zait"] != null ? dt.Rows[x]["Zait"].ToString() : ""), 7, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[x]["Oil"] != null ? dt.Rows[x]["Oil"].ToString() : ""), 7, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt.Rows[x]["NameAr"] != null ? dt.Rows[x]["NameAr"].ToString() : ""), 8, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
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
            tblParent.AddCell(cell);
            #endregion

            doc.Add(tblParent);
            doc.Close();
            return memoryStream.GetBuffer();
        }

        internal static byte[] PrintEmployeesSalaryReports(List<EmployeesSalaryRptVM> dt, string[] infoReport, string branchName, string LoanMonthName)
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
            headerTitle = "مسير مرتبات الموظفين " + (LoanMonthName == "" ? "" : "لشهر : " + LoanMonthName + "") + " ";

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_NORMAL.ToString()], headerTitle, 57, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            cell.Padding = 5;
            headerTbl.AddCell(cell);
            #endregion
            if (branchName != "") headerTitle = "فرع : " + branchName;
            else headerTitle = "جميع الفروع";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_NORMAL.ToString()], headerTitle, 57, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            cell.Padding = 5;
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

            headerTitle = "الصافى";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            //headerTitle = "مخالفات ";
            //DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "غياب";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "خصومات";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "السلف";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "مكافئات";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "علاوات";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "البدلات الإضافية";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "البدلات الشهرية";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = Resources.HousingAllowance;
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            headerTitle = Resources.Transportation;
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            headerTitle = Resources.Profession;
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
            headerTitle = Resources.communication;
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "الراتب";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = "الموظف";
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 7, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            if (dt != null && dt.Count > 0)
            {
                EmployeesSalaryRptVM totalRow = new EmployeesSalaryRptVM()
                {
                    TotalySalaries = dt.Sum(t => decimal.Parse(t.TotalySalaries)).ToString("N2"),
                    TotalRewards = dt.Sum(t => decimal.Parse(t.TotalRewards)).ToString("N2"),
                    TotalyDays = dt.Sum(t => decimal.Parse(t.TotalyDays)).ToString("N2"),
                    TotalDiscounts = dt.Sum(t => decimal.Parse(t.TotalDiscounts)).ToString("N2"),
                    Bonus = dt.Sum(t => decimal.Parse(t.Bonus)).ToString("N2"),
                    TotalLoans = dt.Sum(t => decimal.Parse(t.TotalLoans)).ToString("N2"),
                    AddAllowances = dt.Sum(t => decimal.Parse(t.AddAllowances)).ToString("N2"),
                    MonthlyAllowances = dt.Sum(t => decimal.Parse(t.MonthlyAllowances)).ToString("N2"),
                    HousingAllowance = dt.Sum(t => decimal.Parse(t.HousingAllowance)).ToString("N2"),
                    TransportationAllawance = dt.Sum(t => decimal.Parse(t.TransportationAllawance)).ToString("N2"),
                    ProfessionAllawance = dt.Sum(t => decimal.Parse(t.ProfessionAllawance)).ToString("N2"),
                    CommunicationAllawance = dt.Sum(t => decimal.Parse(t.CommunicationAllawance)).ToString("N2"),
                    Salary = dt.Sum(t => decimal.Parse(t.Salary)).ToString("N2"),
                    EmployeeNameAr = Resources.Pro_Total
                };
                dt.Add(totalRow);
                for (int x = 0; x < dt.Count; x++)
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].TotalySalaries != null ? dt[x].TotalySalaries.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].TotalViolations != null ? dt[x].TotalViolations.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    //bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].TotalyDays != null ? dt[x].TotalyDays.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].TotalDiscounts != null ? dt[x].TotalDiscounts.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].TotalLoans != null ? dt[x].TotalLoans.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].TotalRewards != null ? dt[x].TotalRewards.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].Bonus != null ? dt[x].Bonus.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].AddAllowances != null ? dt[x].AddAllowances.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].MonthlyAllowances != null ? dt[x].MonthlyAllowances.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].HousingAllowance != null ? dt[x].HousingAllowance.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].TransportationAllawance != null ? dt[x].TransportationAllawance.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].ProfessionAllawance != null ? dt[x].ProfessionAllawance.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].CommunicationAllawance != null ? dt[x].CommunicationAllawance.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].Salary != null ? dt[x].Salary.ToString() : ""), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (dt[x].EmployeeNameAr != null ? dt[x].EmployeeNameAr.ToString() : ""), 7, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);
                }
                //المجموع
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
            cell.PaddingBottom = 5;
            tblParent.AddCell(cell);

            doc.Add(tblParent);
            doc.Close();
            return memoryStream.GetBuffer();
        }















        //internal static byte[] PrintEmployeeIdentityReports(EmployeesVM emp, List<VacationVM> vacations, List<AllowanceVM> allowances, List<Loan> loans,
        //    List<DiscountRewardVM> discounts, List<DiscountRewardVM> rewards, List<CityVM> cityList, List<CityPassVM> cityPassList, DataSource dataSource, string Con, string Lang, string[] infoReport)
        //{

        //    bool isFooter = (infoReport[6] != null && infoReport[6] != "" || infoReport[6] != "0") ? true : false;//send from user
        //    string footerData = "" + infoReport[0] + " - " + infoReport[2] + "  - تليفون  : " + infoReport[5] + " - فاكس  : " + infoReport[4] + " -  بريد الكترونى  : " + infoReport[3];
        //    Document doc = new Document(PageSize.A4, 10, 10, 30, 44);
        //    var memoryStream = PdfBase.OpenDocument(doc, "1", true, "", isFooter, footerData);

        //    var headerTitle = "";

        //    PdfPCell cell = null;
        //    PdfPTable tblParent = new PdfPTable(1);
        //    tblParent.DefaultCell.Padding = 1;
        //    tblParent.WidthPercentage = 100;
        //    tblParent.DefaultCell.BorderWidth = 0;
        //    tblParent.HorizontalAlignment = Element.ALIGN_CENTER;
        //    tblParent.SplitLate = false;
        //    tblParent.HeaderRows = 1;

        //    #region HeaderReport
        //    PdfPTable headerTbl = new PdfPTable(57);
        //    headerTbl.DefaultCell.Padding = 1;
        //    headerTbl.WidthPercentage = 100;
        //    headerTbl.DefaultCell.BorderWidthBottom = 1;
        //    headerTbl.DefaultCell.BorderColor = BaseColor.BLUE;
        //    headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

        //    #region FirstRow
        //    headerTitle = "";
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 1, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
        //    cell.Padding = 5;
        //    headerTbl.AddCell(cell);

        //    if (infoReport[1] != "")
        //    {
        //        string imageFilePathLogo = PdfBase.getReportImagePath(infoReport[1]);

        //        if (imageFilePathLogo != "")
        //        {
        //            Image headerJpg = Image.GetInstance(imageFilePathLogo); //infoReport[1];

        //            cell = PdfBase.drawImageCell(headerJpg, 90F, 80F, 19, 3, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
        //            headerTbl.AddCell(cell);
        //        }
        //        else
        //        {
        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 3, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
        //            headerTbl.AddCell(cell);
        //        }
        //    }
        //    else
        //    {
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 3, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
        //        headerTbl.AddCell(cell);
        //    }


        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], infoReport[0], 19, 3, 0, 0, 0, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
        //    cell.Padding = 10;
        //    headerTbl.AddCell(cell);


        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "تاريخ : " + PrintDate, 19, 1, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
        //    cell.Padding = 10;
        //    headerTbl.AddCell(cell);


        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 1, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
        //    cell.Padding = 5;
        //    headerTbl.AddCell(cell);

        //    #endregion

        //    #region secondRow
        //    headerTitle = "بطاقة موظف";

        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_NORMAL.ToString()], headerTitle, 57, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
        //    cell.Padding = 5;
        //    headerTbl.AddCell(cell);
        //    #endregion

        //    cell = new PdfPCell(headerTbl);
        //    cell.Colspan = 57;
        //    cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    cell.PaddingBottom = 5;
        //    tblParent.AddCell(cell);

        //    #endregion

        //    PdfPTable bodyTable = new PdfPTable(2);
        //    bodyTable.WidthPercentage = 100;
        //    bodyTable.DefaultCell.BorderWidth = 0;
        //    bodyTable.DefaultCell.BorderColor = BaseColor.BLUE;
        //    bodyTable.HorizontalAlignment = Element.ALIGN_CENTER;
        //    bodyTable.DefaultCell.Padding = 5;
        //    bodyTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //    //bodyTable.HeaderRows = 1;

        //    #region Employee personal Data

        //    PdfPTable table = new PdfPTable(4);
        //    table.WidthPercentage = 100;

        //    if (emp.PhotoUrl != "")
        //    {
        //        string imageFilePathLogo = PdfBase.getReportImagePath(emp.PhotoUrl);
        //        if (imageFilePathLogo != "")
        //        {
        //            Image headerJpg = Image.GetInstance(imageFilePathLogo); //infoReport[1];

        //            cell = PdfBase.drawImageCell(headerJpg, 90F, 80F, 5, 3, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_CENTER);
        //        }
        //        else
        //        {
        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 5, 3, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
        //        }
        //    }
        //    else
        //    {
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 5, 3, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_TOP);
        //    }
        //    cell.Colspan = 5;
        //    cell.HorizontalAlignment = 1;
        //    table.AddCell(cell);

        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], emp.BankName, 3, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //    table.AddCell(cell);
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "البنك", 2, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //    table.AddCell(cell);
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], emp.BankCardNo, 3, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //    table.AddCell(cell);
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "رقم الحساب", 2, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //    table.AddCell(cell);
        //    bodyTable.AddCell(table);

        //    table = new PdfPTable(4);
        //    table.WidthPercentage = 100;
        //    bodyTable.HorizontalAlignment = Element.ALIGN_CENTER;
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], emp.EmployeeNameAr, 3, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //    table.AddCell(cell);
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "الموظف", 2, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //    table.AddCell(cell);

        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], emp.DepartmentName, 3, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //    table.AddCell(cell);
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "إدارة/ قسم", 2, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //    table.AddCell(cell);

        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], emp.NationalityName, 3, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //    table.AddCell(cell);
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "الجنسية", 2, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //    table.AddCell(cell);

        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], emp.JobName, 3, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //    table.AddCell(cell);
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "المهنة", 2, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //    table.AddCell(cell);

        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], string.IsNullOrEmpty(emp.EducationalQualification) ? "--" : emp.EducationalQualification, 3, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //    table.AddCell(cell);
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "المؤهل", 2, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //    table.AddCell(cell);

        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], string.IsNullOrEmpty(emp.WorkStartDate) ? "" : emp.WorkStartDate, 3, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //    table.AddCell(cell);
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "تاريخ الالتحاق", 2, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //    table.AddCell(cell);


        //    var LastBaskVaction = vacations.Where(x => !string.IsNullOrEmpty(x.BackToWorkDate)).OrderByDescending(x => DateTime.ParseExact(x.BackToWorkDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)).FirstOrDefault();
        //    if (LastBaskVaction != null)
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], LastBaskVaction.BackToWorkDate, 3, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //    else
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "--", 3, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //    table.AddCell(cell);
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "تاريخ آخر مباشرة عمل", 2, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //    table.AddCell(cell);

        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], emp.VacationEndCount.HasValue ? emp.VacationEndCount.ToString() : "--", 3, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //    table.AddCell(cell);
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "رصيد الإجازات بالسنة", 2, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //    table.AddCell(cell);

        //    bodyTable.AddCell(table);

        //    #endregion

        //    cell = new PdfPCell(bodyTable);
        //    cell.Colspan = (72);
        //    cell.BorderWidth = 0;
        //    cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    cell.PaddingBottom = 10;
        //    tblParent.AddCell(cell);

        //    #region الراتب و البدلات


        //    bodyTable = new PdfPTable(35);
        //    bodyTable.WidthPercentage = 100;
        //    bodyTable.DefaultCell.BorderWidth = 0;
        //    bodyTable.DefaultCell.BorderColor = BaseColor.BLUE;
        //    bodyTable.HorizontalAlignment = Element.ALIGN_CENTER;
        //    bodyTable.DefaultCell.Padding = 1;
        //    bodyTable.HeaderRows = 1;
        //    //doc.Add(table);

        //    headerTitle = "";
        //    DrawTableHeader_RestClear(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 25, 10, 5, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);

        //    headerTitle = "الرواتب و البدلات  بدل السكن";
        //    DrawTableTitle(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 10, 10, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);

        //    headerTitle = "يؤثر في التأخير";
        //    DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //    headerTitle = "يؤثر في الإضافي";
        //    DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //    headerTitle = "يؤثر في الإجازة";
        //    DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //    headerTitle = "يؤثر في الغياب";
        //    DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //    headerTitle = "يؤثر في التصفية";
        //    DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //    headerTitle = "القيمة";
        //    DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //    headerTitle = "البدل";
        //    DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
        //    decimal Total = 0;

        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "نعم", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //    bodyTable.AddCell(cell);
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "نعم", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //    bodyTable.AddCell(cell);
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "نعم", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //    bodyTable.AddCell(cell);
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "نعم", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //    bodyTable.AddCell(cell);
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "نعم", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //    bodyTable.AddCell(cell);
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], emp.Salary.Value.ToString("N2"), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //    bodyTable.AddCell(cell);
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "الراتب", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //    bodyTable.AddCell(cell);
        //    Total += emp.Salary.Value;

        //    foreach (var allownace in allowances)
        //    {
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "نعم", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        bodyTable.AddCell(cell);
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "نعم", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        bodyTable.AddCell(cell);
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "نعم", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        bodyTable.AddCell(cell);
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "نعم", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        bodyTable.AddCell(cell);
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "نعم", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        bodyTable.AddCell(cell);
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], allownace.AllowanceAmount.HasValue ? allownace.AllowanceAmount.Value.ToString("N2") : "--", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        bodyTable.AddCell(cell);
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], allownace.AllowanceTypeName, 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        bodyTable.AddCell(cell);
        //        Total += allownace.AllowanceAmount.HasValue ? allownace.AllowanceAmount.Value : 0;
        //    }
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //    bodyTable.AddCell(cell);
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //    bodyTable.AddCell(cell);
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //    bodyTable.AddCell(cell);
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //    bodyTable.AddCell(cell);
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //    bodyTable.AddCell(cell);
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], Total.ToString("N2"), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //    bodyTable.AddCell(cell);
        //    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "المجموع", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //    bodyTable.AddCell(cell);

        //    #endregion


        //    cell = new PdfPCell(bodyTable);
        //    cell.Colspan = (72);
        //    cell.BorderWidth = 0;
        //    cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    cell.PaddingBottom = 10;
        //    tblParent.AddCell(cell);

        //    #region الحسومات و السلف
        //    bodyTable = new PdfPTable(40);
        //    bodyTable.WidthPercentage = 100;
        //    bodyTable.DefaultCell.BorderWidth = 0;
        //    bodyTable.DefaultCell.BorderColor = BaseColor.BLUE;
        //    bodyTable.HorizontalAlignment = Element.ALIGN_CENTER;
        //    bodyTable.DefaultCell.Padding = 1;
        //    bodyTable.HeaderRows = 1;


        //    if (discounts.Count() > 0 && loans.Count() > 0)
        //    {
        //        decimal valTotal = 0;
        //        decimal MonthlyTotal = 0;
        //        decimal paidTotal = 0;
        //        decimal remainTotal = 0;

        //        headerTitle = "";
        //        DrawTableHeader_RestClear(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 30, 10, 5, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);

        //        headerTitle = "الحسميات و السلف";
        //        DrawTableTitle(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 10, 10, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);

        //        headerTitle = "طريقة الدفع";
        //        DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //        headerTitle = "المتبقي";
        //        DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //        headerTitle = "المدفوع";
        //        DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //        headerTitle = "عدد الدفعات";
        //        DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //        headerTitle = "الدفعة الشهرية";
        //        DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //        headerTitle = "القيمة";
        //        DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //        headerTitle = "تاريخ البداية";
        //        DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //        headerTitle = "النوع";
        //        DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //        foreach (var discount in discounts)
        //        {
        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "قيمة ثابتة", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);
        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "0", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);
        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (discount.Amount.HasValue ? discount.Amount.Value.ToString("N2") : "--"), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);
        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "1", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);
        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (discount.Amount.HasValue ? discount.Amount.Value.ToString("N2") : "--"), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);
        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (discount.Amount.HasValue ? discount.Amount.Value.ToString("N2") : "--"), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);
        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (discount.StartDate.HasValue ?
        //                        discount.StartDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : "--"), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);
        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], discount.DiscountRewardName, 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);
        //            valTotal = valTotal + (discount.Amount.HasValue ? discount.Amount.Value : 0);
        //            MonthlyTotal = MonthlyTotal + (discount.Amount.HasValue ? discount.Amount.Value : 0);
        //            paidTotal = paidTotal + (discount.Amount.HasValue ? discount.Amount.Value : 0);
        //            remainTotal = 0;
        //        }
        //        foreach (var loan in loans)
        //        {
        //            var loanDetails = loan.LoanDetails;
        //            if (loanDetails.Count() > 0)
        //            {
        //                var paidLoans = loanDetails.Where(x => x.Date.Value.Month <= DateTime.Now.Month && x.Date.Value.Year <= DateTime.Now.Year).ToList();
        //                var paidValue = paidLoans.Count() * paidLoans[0].Amount.Value;

        //                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "قيمة ثابتة", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //                bodyTable.AddCell(cell);
        //                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (loan.Amount.Value - paidValue).ToString("N2"), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //                bodyTable.AddCell(cell);
        //                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], paidValue.ToString("N2"), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //                bodyTable.AddCell(cell);

        //                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], loan.MonthNo.Value.ToString(), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //                bodyTable.AddCell(cell);
        //                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (loanDetails[0].Amount.HasValue ? loanDetails[0].Amount.Value.ToString("N2") : "--"), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //                bodyTable.AddCell(cell);
        //                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (loan.Amount.HasValue ? loan.Amount.Value.ToString("N2") : "--"), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //                bodyTable.AddCell(cell);
        //                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], loan.StartDate , 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //                bodyTable.AddCell(cell);
        //                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "سلفة", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //                bodyTable.AddCell(cell);

        //                valTotal = valTotal + (loan.Amount.HasValue ? loan.Amount.Value : 0);
        //                MonthlyTotal = MonthlyTotal + (loanDetails[0].Amount.HasValue ? loanDetails[0].Amount.Value : 0);
        //                paidTotal = paidTotal + paidValue;
        //                remainTotal = remainTotal + (loan.Amount.Value - paidValue);
        //            }
        //            else
        //                continue;
        //        }

        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        bodyTable.AddCell(cell);
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], remainTotal.ToString(), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        bodyTable.AddCell(cell);
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], paidTotal.ToString(), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        bodyTable.AddCell(cell);
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        bodyTable.AddCell(cell);
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], MonthlyTotal.ToString(), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        bodyTable.AddCell(cell);
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], valTotal.ToString(), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        bodyTable.AddCell(cell);
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        bodyTable.AddCell(cell);
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "المجموع", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        bodyTable.AddCell(cell);


        //        cell = new PdfPCell(bodyTable);
        //        cell.Colspan = (72);
        //        cell.BorderWidth = 0;
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        cell.PaddingBottom = 10;
        //        tblParent.AddCell(cell);
        //    }
        //    #endregion


        //    #region المكافآت

        //    if (rewards.Count() > 0)
        //    {
        //        bodyTable = new PdfPTable(20);
        //        bodyTable.WidthPercentage = 100;
        //        bodyTable.DefaultCell.BorderWidth = 0;
        //        bodyTable.DefaultCell.BorderColor = BaseColor.BLUE;
        //        bodyTable.HorizontalAlignment = Element.ALIGN_CENTER;
        //        bodyTable.DefaultCell.Padding = 1;
        //        bodyTable.HeaderRows = 1;
        //        //doc.Add(table);

        //        headerTitle = "";
        //        DrawTableHeader_RestClear(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 15, 10, 5, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);

        //        headerTitle = "المكافآت";
        //        DrawTableTitle(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);

        //        headerTitle = "النوع";
        //        DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //        headerTitle = "تاريخ النهاية";
        //        DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //        headerTitle = "تاريخ البداية";
        //        DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //        headerTitle = "القيمة";
        //        DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
        //        Total = 0;
        //        foreach (var reward in rewards)
        //        {
        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (reward.Amount.HasValue ? reward.Amount.Value.ToString("N2") : "--"), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);
        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (reward.EndDate.HasValue ? reward.EndDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : "--"), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);
        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (reward.StartDate.HasValue ?
        //                        reward.StartDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : "--"), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);
        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], reward.DiscountRewardName, 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);

        //            Total = Total + (reward.Amount.HasValue ? reward.Amount.Value : 0);
        //        }

        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], Total.ToString(), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        bodyTable.AddCell(cell);
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        bodyTable.AddCell(cell);
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        bodyTable.AddCell(cell);
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "المجموع", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        bodyTable.AddCell(cell);


        //        cell = new PdfPCell(bodyTable);
        //        cell.Colspan = (40);
        //        cell.BorderWidth = 0;
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        cell.PaddingBottom = 10;
        //        tblParent.AddCell(cell);
        //    }
        //    #endregion



        //    #region الإجازات

        //    if (vacations.Count() > 0)
        //    {
        //        string StatusName = "";
        //        bodyTable = new PdfPTable(50);
        //        bodyTable.WidthPercentage = 100;
        //        bodyTable.DefaultCell.BorderWidth = 0;
        //        bodyTable.DefaultCell.BorderColor = BaseColor.BLUE;
        //        bodyTable.HorizontalAlignment = Element.ALIGN_CENTER;
        //        bodyTable.DefaultCell.Padding = 1;
        //        bodyTable.HeaderRows = 1;
        //        //doc.Add(table);
        //        headerTitle = "";
        //        DrawTableHeader_RestClear(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 40, 10, 5, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);

        //        headerTitle = "الإجازات";
        //        DrawTableTitle(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 10, 10, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);

        //        headerTitle = "قرار الإدارة";
        //        DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //        headerTitle = "ت.العودة الفعلي";
        //        DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //        headerTitle = "الأيام الفعلية";
        //        DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //        headerTitle = "الأيام الرسمية";
        //        DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //        headerTitle = "ت.العودة المتوقع";
        //        DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //        headerTitle = "تاريخ البداية";
        //        DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //        headerTitle = "مبلغ الخصم";
        //        DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //        headerTitle = "حالة الخصم";
        //        DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //        headerTitle = "تاريخ الطلب";
        //        DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //        headerTitle = "نوع الإجازة";
        //        DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //        Total = 0;
        //        VacationService _vacationService = new .VacationService(dataSource);
        //        int excpecTotalDays = 0;
        //        int actuallTotalDays = 0;
        //        foreach (var vacation in vacations)
        //        {
        //            StatusName = vacation.VacationStatus == 1 ? "تقديم طلب" : vacation.VacationStatus == 2 ? "موافقة" :
        //                       vacation.VacationStatus == 3 ? "مرفوضة " : vacation.VacationStatus == 4 ? "تحت المراجعة" : vacation.VacationStatus == 5 ? "تم تأجيلها" : "";

        //            int expectedNetDays = _vacationService.GetVacationDays_WithoutHolidays(vacation.StartDate, vacation.EndDate, emp.EmployeeId, Lang, Con, (int)vacation.VacationTypeId).Count();
        //            int actualNetDays = _vacationService.GetVacationDays_WithoutHolidays(vacation.StartDate,

        //           string.IsNullOrEmpty(vacation.BackToWorkDate) ? DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : vacation.BackToWorkDate, emp.EmployeeId, Lang, Con, (int)vacation.VacationTypeId).Count();

        //            excpecTotalDays += expectedNetDays;
        //            actuallTotalDays += actualNetDays;

        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], StatusName, 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);

        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], vacation.BackToWorkDate, 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);

        //            //actualNetDays.ToString()
        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], actualNetDays.ToString(), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);

        //            //expectedNetDays.ToString()
        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], expectedNetDays.ToString(), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);

        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], vacation.EndDate, 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);

        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], vacation.StartDate, 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);

        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], (vacation.DiscountAmount.HasValue ? vacation.DiscountAmount.Value.ToString("N2") : "--"), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);

        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], vacation.IsDiscount.Value ? "نعم" : "لا", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);

        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], vacation.Date, 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);

        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], vacation.VacationTypeName, 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);
        //            Total += vacation.DiscountAmount.HasValue ? vacation.DiscountAmount.Value : 0;
        //        }

        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        bodyTable.AddCell(cell);
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        bodyTable.AddCell(cell); 
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], actuallTotalDays.ToString(), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        bodyTable.AddCell(cell);
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], excpecTotalDays.ToString(), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        bodyTable.AddCell(cell);
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        bodyTable.AddCell(cell);
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        bodyTable.AddCell(cell);
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], Total.ToString(), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        bodyTable.AddCell(cell);
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        bodyTable.AddCell(cell);
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        bodyTable.AddCell(cell);
        //        cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "المجموع", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //        bodyTable.AddCell(cell);


        //        cell = new PdfPCell(bodyTable);
        //        cell.Colspan = (40);
        //        cell.BorderWidth = 0;
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        cell.PaddingBottom = 10;
        //        tblParent.AddCell(cell);
        //    }
        //    #endregion


        //    #region الوثائق

        //    if (!string.IsNullOrEmpty(emp.NationalId) || !string.IsNullOrEmpty(emp.MedicalNo) || !string.IsNullOrEmpty(emp.PassportNo))
        //    {
        //        bodyTable = new PdfPTable(25);
        //        bodyTable.WidthPercentage = 100;
        //        bodyTable.DefaultCell.BorderWidth = 0;
        //        bodyTable.DefaultCell.BorderColor = BaseColor.BLUE;
        //        bodyTable.HorizontalAlignment = Element.ALIGN_CENTER;
        //        bodyTable.DefaultCell.Padding = 1;
        //        bodyTable.HeaderRows = 1;
        //        //doc.Add(table);

        //        headerTitle = "";
        //        DrawTableHeader_RestClear(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 20, 10, 5, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE);

        //        headerTitle = "الوثائق";
        //        DrawTableTitle(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);

        //        headerTitle = "تاريخ الإنتهاء";
        //        DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //        headerTitle = "تاريخ الإصدار";
        //        DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //        headerTitle = "مكان الإصدار";
        //        DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //        headerTitle = "رقم الوثيقة";
        //        DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //        headerTitle = "رقم الوثيقة";
        //        DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_12_NORMAL.ToString()], headerTitle, 1, 5, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

        //        if (!string.IsNullOrEmpty(emp.NationalId))
        //        {
        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], emp.NationalIdEndDate, 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);

        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], emp.NationalIdDate, 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);

        //            //actualNetDays.ToString()
        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], emp.NationalIdSource.HasValue ? cityList.Where(x => x.CityId == emp.NationalIdSource).FirstOrDefault().NameAr:"", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);

        //            //expectedNetDays.ToString()
        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], emp.NationalId.ToString(), 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);
        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "رقم الهوية", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);
        //        }
        //        if (!string.IsNullOrEmpty(emp.MedicalNo))
        //        {
        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], emp.MedicalEndDate, 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);

        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], emp.MedicalStartDate, 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);

        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], emp.MedicalSource , 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);

        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], emp.MedicalNo, 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);

        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "التأمين الصحي", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);
        //        }
        //        if (!string.IsNullOrEmpty(emp.PassportNo))
        //        {
        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], emp.PassportEndDate, 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);

        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], emp.PassportNoDate, 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);

        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], string.IsNullOrEmpty(emp.PassportSource)? "" : cityPassList.Where(x => x.CityId == int.Parse(emp.PassportSource)).FirstOrDefault().NameAr, 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);

        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], emp.PassportNo, 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);

        //            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_9_NORMAL.ToString()], "جواز السفر", 5, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
        //            bodyTable.AddCell(cell);
        //        }

        //        cell = new PdfPCell(bodyTable);
        //        cell.Colspan = (40);
        //        cell.BorderWidth = 0;
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        cell.PaddingBottom = 10;
        //        tblParent.AddCell(cell);
        //    }


        
        //    #endregion

        //    doc.Add(tblParent);
        //    doc.Close();
        //    return memoryStream.GetBuffer();
        //}
        public static string getArabNameOfday(string enName)
        {
            if (enName == "Saturday") return "السبت";
            if (enName == "Sunday") return "الأحد";
            if (enName == "Monday") return "الإثنين";
            if (enName == "Tuesday") return "الثلاثاء";
            if (enName == "Wednesday") return "الأربعاء";
            if (enName == "Thursday	") return "الخميس";
            if (enName == "Friday") return "الجمعة";
            else return "";
        }
        public static string getArabNameOfday(int Name)
        {
            string[] days = { "السبت", "الأحد", "الإثنين", "الثلاثاء", "الأربعاء", "الخميس", "الجمعة" };
            return days[Name - 1];
        }
        public static string getEnglishNameOfday(int Name)
        {
            string[] days = { "Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
            return days[Name - 1];
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
        public static void DrawTableTitle(PdfPTable table, PdfPCell cell, Font font, string headerTitle, int borderWidth = 0, int colspan = 0, int paddingBottom = 0, int Padding = 0, int horiAlign = 0, int vertAlign = 0)
        {
            cell = new PdfPCell(new Phrase(headerTitle, font));
            cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            cell.BorderWidth = borderWidth;
            cell.Colspan = colspan;
            cell.PaddingBottom = paddingBottom;
            cell.Padding = Padding;
            cell.HorizontalAlignment = horiAlign;
            cell.VerticalAlignment = vertAlign;
            cell.BackgroundColor = new BaseColor(153, 204, 255);
            table.AddCell(cell);
        }
        public static void DrawTableHeader_RestClear(PdfPTable table, PdfPCell cell, Font font, string headerTitle, int borderWidth = 0, int colspan = 0, int paddingBottom = 0, int Padding = 0, int horiAlign = 0, int vertAlign = 0)
        {
            cell = new PdfPCell(new Phrase(headerTitle, font));
            cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            cell.BorderWidth = 0;
            cell.Colspan = colspan;
            cell.PaddingBottom = paddingBottom;
            cell.Padding = Padding;
            cell.HorizontalAlignment = horiAlign;
            cell.VerticalAlignment = vertAlign;
            cell.BackgroundColor = BaseColor.WHITE;
            table.AddCell(cell);
        }

        internal static byte[] PrintCustomersReport(List<CustomerVM> dt, string[] infoReport, string title, string fromdate, string todate)
        {
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
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_NORMAL.ToString()], title, 57, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            cell.Padding = 5;
            headerTbl.AddCell(cell);
            #endregion

            if (fromdate == "" || todate == "" || fromdate == null || todate == null)
            {

            }
            else
            {
                cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], " \n من تاريخ " + fromdate + "   الى تاريخ  " + todate + "", 57, 0, 0, 0, 0, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
                cell.PaddingBottom = 5;
                cell.PaddingTop = 5;
                headerTbl.AddCell(cell);
            }


            cell = new PdfPCell(headerTbl);
            cell.Colspan = 57;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            tblParent.AddCell(cell);


            #endregion

            PdfPTable bodyTable = new PdfPTable(63);
            bodyTable.WidthPercentage = 95;
            bodyTable.DefaultCell.BorderWidth = 0;
            bodyTable.DefaultCell.BorderColor = BaseColor.BLUE;
            bodyTable.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyTable.DefaultCell.Padding = 5;
            bodyTable.HeaderRows = 1;

            headerTitle = @Resources.Acc_AcountNumber;
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 9, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);


            headerTitle = @Resources.General_MobileNumber;
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 9, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);


            headerTitle = @Resources.General_PhoneNumber;
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 10, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);

            headerTitle = @Resources.General_Email;
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 10, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);


            headerTitle = @Resources.Emp_NationalNumber;
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 9, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);


            headerTitle = @Resources.Pro_CustomerType;
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 10, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);


            headerTitle = @Resources.Pro_CustomerName;
            DrawTableHeader(bodyTable, cell, PdfBase.Fonts[fontsenum.nationalTextFont_13_NORMAL.ToString()], headerTitle, 1, 9, 10, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);



            if (dt != null && dt.Count > 0)
            {
                for (int x = 0; x < dt.Count; x++)
                {
                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].AccountCodee != null ? dt[x].AccountCodee.ToString() : ""), 9, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);


                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].CustomerMobile != null ? dt[x].CustomerMobile.ToString() : ""), 9, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].CustomerPhone != null ? dt[x].CustomerPhone.ToString() : ""), 10, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);


                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].CustomerEmail != null ? dt[x].CustomerEmail.ToString() : ""), 10, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);

                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].CustomerNationalId != null ? dt[x].CustomerNationalId.ToString() : ""), 9, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);


                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].CustomerTypeName != null ? dt[x].CustomerTypeName.ToString() : ""), 10, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
                    bodyTable.AddCell(cell);


                    cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_10_NORMAL.ToString()], (dt[x].CustomerNameAr != null ? dt[x].CustomerNameAr.ToString() : ""), 9, 4, 1, 1, 1, 1, 5, Element.ALIGN_CENTER);
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
            cell.Colspan = (63);
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            tblParent.AddCell(cell);

            doc.Add(tblParent);
            doc.Close();
            return memoryStream.GetBuffer();
        }




        internal static byte[] PrintEmpEndworkrpt(DataTable dt, rptEmpEndWorkVM rptEmp, string[] infoReport)
        {
            bool isFooter = (infoReport[6] != null && infoReport[6] != "" || infoReport[6] != "0") ? true : false;//send from user
            string footerData = "" + infoReport[0] + " - " + infoReport[2] + "  - تليفون  : " + infoReport[5] + " - فاكس  : " + infoReport[4] + " -  بريد الكترونى  : " + infoReport[3];
            Document doc = new Document(PageSize.A4, 0, 0, 0, 44);
            var memoryStream = PdfBase.OpenDocument(doc, "1", false, "", isFooter, footerData);

            //EmpSalaryPartsVM salaryPart = (new AllowanceController().GetSalaryPartObject(((int)dt.Rows[0]["EmpId"])));

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
            headerTbl.DefaultCell.Padding = 1;
            headerTbl.WidthPercentage = 100;
            headerTbl.DefaultCell.BorderWidthBottom = 1;
            headerTbl.DefaultCell.BorderColor = BaseColor.BLUE;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            #region FirstRow
            headerTitle = "";
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 1, 0, 0, 0, 0, 10, Element.ALIGN_RIGHT, Element.ALIGN_TOP);
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


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], infoReport[0], 19, 3, 0, 0, 0, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE);
            headerTbl.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "تاريخ : " + PrintDate, 19, 1, 0, 0, 0, 0, 5, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
            headerTbl.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_NORMAL.ToString()], "", 19, 1, 0, 0, 0, 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM);
            headerTbl.AddCell(cell);

            #endregion

            #region secondRow
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_NORMAL.ToString()], " عقد انهاء خدمة ", 57, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            headerTbl.AddCell(cell);
            #endregion

            #endregion
            cell = new PdfPCell(headerTbl);
            cell.Colspan = 57;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.PaddingBottom = 5;
            tblParent.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_18_NORMAL.ToString()], " \n", 57, 0, 0, 0, 0, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM);
            tblParent.AddCell(cell);

            PdfPTable bodyTable = new PdfPTable(57);
            bodyTable.WidthPercentage = 100;
            bodyTable.DefaultCell.BorderWidth = 0;
            bodyTable.DefaultCell.BorderColor = BaseColor.BLUE;
            bodyTable.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyTable.DefaultCell.Padding = 1;
            bodyTable.HeaderRows = 1;

            string arabDay = getArabNameOfday(dt.Rows[0]["ContDayname"].ToString());
            string dayDate = (new GeneralController()).ConvertDateCalendar((DateTime)dt.Rows[0]["AddDate"], "Gregorian", "en-US");

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (string.Format("إنه في يوم  {0} {1} تم الأتفاق بين كل من", dayDate, arabDay)), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], ("-: إنه في يوم " + arabDay + " " + dayDate + "  تم الأتفاق بين كل من"), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], ("الطرف الأول: " + " " + dt.Rows[0]["Org_NameAr"].ToString() + " - سجل تجاري رقم " + dt.Rows[0]["TaxCode"].ToString() + " - وعنوانها " + dt.Rows[0]["OR_Address"].ToString()), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (" ص.ب" + " " + dt.Rows[0]["Mailbox"].ToString() + " البريد الالكترونى" + " " + dt.Rows[0]["OR_Email"].ToString() + " - هاتف " + dt.Rows[0]["OR_Mobile"].ToString() + " فاكس " + " " + dt.Rows[0]["Fax"].ToString()), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            //cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (" البريد الالكترونى" + " " + dt.Rows[0]["OR_Email"].ToString() + " الرمز البريدى " + "---------------------" + " - هاتف " + dt.Rows[0]["OR_Mobile"].ToString() + " فاكس " + " " + dt.Rows[0]["Fax"].ToString()), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            //bodyTable.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], (string.Format("ويمثلها في هذه الوثيقة العقد السيد/ {0} بصفته  {1} ويشار إليه في ما بعد بالطرف الأول", dt.Rows[0]["RepEmpName"].ToString(), dt.Rows[0]["PerSe"].ToString())), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            DataRow row = dt.Rows[0];
            string line = string.Format("الطرف الثاني : {0} الجنسية {1}، بطاقة الهوية {2}، عنوانه {3}",
                row["Emp_NameAr"].ToString(),
                row["EmpNationality"].ToString(),
                row["NationalId"].ToString(),
                row["Address"].ToString()
                );
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], line, 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            string line2 = string.Format(", جواز سفر رقم {0} صادر من {1} بتاريخ {2}، هاتف {3}، البريد الالكترونى {4}  ",
                row["PassportNo"].ToString(),
                row["PassportSource"].ToString(),
                row["PassportNoDate"].ToString(),
                row["Emp_Mobile"].ToString(),
                row["Email"].ToString());

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], line2, 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], "ويشار إليه فيما بعد بالطرف الثاني", 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], ("\n"), 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_14_UNDERLINE.ToString()], "بيانات الموظف", 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], "  اسم الموظف   :    " + rptEmp.EmpName, 20, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], "   الرقم الوظيفي   :   " + rptEmp.EmpJobNo, 20, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], "    الفرع   :   " + rptEmp.Empbranch, 17, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], "  الوظيفة   :   " + rptEmp.EmpJob, 20, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], "   تاريخ التعيين    :   " + rptEmp.EmpStartWork, 20, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], "  العهد   :    " + rptEmp.EmpCustoday, 17, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], "  السلف    :   " + rptEmp.EmpLoan, 20, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], "   رواتب متاخرة   :    " + rptEmp.EmpLateSalary, 20, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], "   مدة الخدمة   :   " + rptEmp.EmpTotalServe, 17, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);
            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], "  سبب انهاء الخدمة   :   " + rptEmp.reson, 20, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], "  تاريخ انتهاء الخدمة   :   " + rptEmp.Enddate, 20, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], "    الراتب الاساسي   :   " + rptEmp.EmpNetSalary, 17, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);


            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], "     البدلات   :   " + rptEmp.EmpEndallowance, 20, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], "     تاريخ انتهاء العقد   :   " + rptEmp.EndContractDate, 20, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);

            cell = PdfBase.drawCell(PdfBase.Fonts[fontsenum.nationalTextFont_11_NORMAL.ToString()], "   نص القرار    :    " + rptEmp.reasontxt, 57, 4, 0, 0, 0, 0, 5, Element.ALIGN_LEFT);
            bodyTable.AddCell(cell);







            cell = new PdfPCell(bodyTable);
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









    }
}