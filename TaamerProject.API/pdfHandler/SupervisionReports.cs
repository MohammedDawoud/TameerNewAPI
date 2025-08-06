
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Spire.Doc;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using System.Text.RegularExpressions;

//using OfficeConverter;
using System.Drawing;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using Table = Spire.Doc.Table;
using Paragraph = Spire.Doc.Documents.Paragraph;
using System.Net;
using Microsoft.Office.Interop.Word;
using System.Globalization;

using Section = Spire.Doc.Section;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.API.pdfHandler
{
    public class SupervisionReports
    {
        public SupervisionReports() { }
        public GeneralMessage PrintReport(string sourceFile, string destFile, string pdfFile, Pro_Super_PhasesVM Phase, List<Pro_SupervisionDetailsVM> phaseDetails, string Image1Path, string Image1Path2, string StampUrl, OrganizationsVM objOrganization)
        {
            Spire.Doc.Document doc = new Spire.Doc.Document();
            string fileNameOnly = DateTime.Now.Ticks.ToString();
            try
            {
                // To copy a file to another location and
                // overwrite the destination file if it already exists.
                if (System.IO.File.Exists(destFile))
                {
                    System.IO.File.Delete(destFile);
                }
                System.IO.File.Copy(sourceFile, destFile, true);

                WebClient webClient = new WebClient();
                using (MemoryStream ms = new MemoryStream(webClient.DownloadData(destFile)))
                {
                    doc.LoadFromStream(ms, FileFormat.Docx);

                    ParagraphStyle style = new ParagraphStyle(doc);
                    style.Name = "MyStyle";
                    //font name
                    style.CharacterFormat.FontName = "Arial";
                    //font size
                    //style.CharacterFormat.FontSize = 14;
                    doc.Styles.Add(style);

                    if (Phase != null)
                    {
                        doc.Replace("#title2", "تقرير " + Phase.NameAr, false, true);
                    }

                    if (phaseDetails.FirstOrDefault() != null)
                    {
                        doc.Replace("#title1", Phase.Note != "" ? ("تقرير " + Phase.Note) : "", false, true);
                        doc.Replace("#text1", phaseDetails.FirstOrDefault().MunicipalName, false, true);
                        doc.Replace("#text2", phaseDetails.FirstOrDefault().SubMunicipalityName, false, true);
                        doc.Replace("#text3", phaseDetails.FirstOrDefault().DistrictName, false, true);
                        doc.Replace("#ProjectName", phaseDetails.FirstOrDefault().ProjectDiscName, false, true);
                        doc.Replace("#OwnerName", phaseDetails.FirstOrDefault().CustomerName, false, true);
                        doc.Replace("#BuildingLic", phaseDetails.FirstOrDefault().LicenseNo, false, true);
                        doc.Replace("#LandNumber", phaseDetails.FirstOrDefault().PieceNo, false, true);
                        doc.Replace("#PlansNumber", phaseDetails.FirstOrDefault().OutlineNo, false, true);
                        doc.Replace("#BuildingType", phaseDetails.FirstOrDefault().BuildTypeName, false, true);
                        doc.Replace("#BuildingDescription", phaseDetails.FirstOrDefault().ProBuildingDisc, false, true);
                        doc.Replace("#NumberOfFloor", phaseDetails.FirstOrDefault().AdwARid.Value.ToString(), false, true);
                        doc.Replace("#CertOffice", phaseDetails.FirstOrDefault().OfficeName, false, true);
                        doc.Replace("#SuperOffice", objOrganization.NameAr ?? "", false, true);
                        doc.Replace("#CostructionConstructor", phaseDetails.FirstOrDefault().ContractorName, false, true);
                        doc.Replace("#SuperEngineer", phaseDetails.FirstOrDefault().ReceivedUserName, false, true);
                        //doc.Replace("#ReportDate", DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), false, true);
                        doc.Replace("#ReportDate", phaseDetails.FirstOrDefault().SuperDateConfirm, false, true);


                        if (phaseDetails.FirstOrDefault().SuperStatus == 2)
                        {
                            doc.Replace("#1", "#", false, true);
                            doc.Replace("#2", " ", false, true);
                            doc.Replace("#3", " ", false, true);
                        }
                        else if (phaseDetails.FirstOrDefault().SuperStatus == 3)
                        {
                            doc.Replace("#1", " ", false, true);
                            doc.Replace("#2", "#", false, true);
                            doc.Replace("#3", " ", false, true);
                        }
                        else
                        {
                            doc.Replace("#1", " ", false, true);
                            doc.Replace("#2", " ", false, true);
                            doc.Replace("#3", "#", false, true);
                        }


                        //string changesToApproved = " - 1" + phaseDetails.FirstOrDefault().HeadOutlineChangetxt1 + "\r\n" +
                        //    " - 2" + phaseDetails.FirstOrDefault().HeadOutlineChangetxt2 +  "\r\n" +
                        //    " - 3" + phaseDetails.FirstOrDefault().HeadOutlineChangetxt3 +  "\r\n";
                        //string ItemsNeedingCorrection = " - 1" + phaseDetails.FirstOrDefault().HeadPointsNotWrittentxt1 + "\r\n" +
                        //      " - 2" + phaseDetails.FirstOrDefault().HeadPointsNotWrittentxt2 +"\r\n" +
                        //     " - 3" + phaseDetails.FirstOrDefault().HeadPointsNotWrittentxt3 + "\r\n";

                        doc.Replace("#Outline1", phaseDetails.FirstOrDefault().HeadOutlineChangetxt1, false, true);
                        doc.Replace("#Outline2", phaseDetails.FirstOrDefault().HeadOutlineChangetxt2, false, true);
                        doc.Replace("#Outline3", phaseDetails.FirstOrDefault().HeadOutlineChangetxt3, false, true);
                        doc.Replace("#changesToApproved", "", false, true);


                        doc.Replace("#printing1", phaseDetails.FirstOrDefault().HeadPointsNotWrittentxt1, false, true);
                        doc.Replace("#printing2", phaseDetails.FirstOrDefault().HeadPointsNotWrittentxt2, false, true);
                        doc.Replace("#printing3", phaseDetails.FirstOrDefault().HeadPointsNotWrittentxt3, false, true);
                        doc.Replace("#ItemsNeedingCorrection", "", false, true);

                        doc.Replace("#FullName", phaseDetails.FirstOrDefault().ReceivedUserName, false, true);
                        //doc.Replace("#Signature", "", false, true);
                        doc.Replace("#CerNo", phaseDetails.FirstOrDefault().SupEngineerCert, false, true);
                        doc.Replace("#Stamp", objOrganization.NameAr ?? "", false, true);
                    }


                    //Image

                    if (Image1Path != "")
                    {
                        Image image = Image.FromFile(Image1Path);

                        TextSelection selection = doc.FindString("#photo1", false, true);
                        int index = 0;
                        TextRange range = null;

                        DocPicture pic = new DocPicture(doc);

                        pic.LoadImage(image);
                        pic.Width = 200f;
                        pic.Height = 150f;



                        range = selection.GetAsOneRange();
                        //range.OwnerParagraph.Format.BackColor = Color.Red;
                        index = range.OwnerParagraph.ChildObjects.IndexOf(range);
                        range.OwnerParagraph.ChildObjects.Insert(index, pic);
                        range.OwnerParagraph.ChildObjects.Remove(range);
                    }

                    if (Image1Path2 != "")
                    {
                        Image image2 = Image.FromFile(Image1Path2);

                        TextSelection selection2 = doc.FindString("#photo2", false, true);
                        int index2 = 0;
                        TextRange range2 = null;


                        DocPicture pic2 = new DocPicture(doc);
                        pic2.LoadImage(image2);
                        pic2.Width = 200f;
                        pic2.Height = 150f;

                        range2 = selection2.GetAsOneRange();
                        index2 = range2.OwnerParagraph.ChildObjects.IndexOf(range2);
                        range2.OwnerParagraph.ChildObjects.Insert(index2, pic2);
                        range2.OwnerParagraph.ChildObjects.Remove(range2);
                    }

                    if (StampUrl != "")
                    {
                        Image image = Image.FromFile(StampUrl);

                        TextSelection selection = doc.FindString("#Signature", false, true);
                        int index3 = 0;
                        TextRange range3 = null;

                        DocPicture pic = new DocPicture(doc);

                        pic.LoadImage(image);
                        pic.Width = 130f;
                        pic.Height = 90f;

                        range3 = selection.GetAsOneRange();
                        //range.OwnerParagraph.Format.BackColor = Color.Red;
                        index3 = range3.OwnerParagraph.ChildObjects.IndexOf(range3);
                        range3.OwnerParagraph.ChildObjects.Insert(index3, pic);
                        range3.OwnerParagraph.ChildObjects.Remove(range3);
                    }
                    else
                    {
                        doc.Replace("#Signature", "", false, true);
                    }
                    try
                    {
                        //Tables
                        BuildTablesAccordingToReportNo(doc,Phase.PhaseId, phaseDetails);
                    }
                    catch (Exception ex)
                    {

                    }

                    Section s = doc.Sections[0];
                    int i = s.Paragraphs.Count;

                    for (int j = 0; j < i; j++)
                    {
                        Spire.Doc.Documents.Paragraph p = s.Paragraphs[j];

                        style.CharacterFormat.FontName = "Arial";
                        //style.CharacterFormat.FontSize = 20;
                        doc.Styles.Add(style);
                        p.ApplyStyle(style.Name);
                    }
                }

                doc.SaveToFile(destFile, FileFormat.Docx);

                RemoveRedLine(destFile);

                //using Interop
                Convert(destFile, pdfFile, WdSaveFormat.wdFormatPDF);

                //using OfficeConverter
                //using (var converter = new Converter())
                //{
                //    converter.Convert(destFile, pdfFile);
                //}

                return new GeneralMessage() { Result = true, Message = "تم عمل التقرير بنجاح" };
            }
            catch (Exception ex)
            {
                return new GeneralMessage() { Result = false, Message = ex.Message };
            }
        }

        private void BuildTablesAccordingToReportNo(Spire.Doc.Document doc, int phaseId, List<Pro_SupervisionDetailsVM> phaseDetails)
        {
            switch (phaseId)
            {
                case 1: {
                        int tableCounts = doc.Sections[0].Tables.Count;
                        if (tableCounts == 0)
                            break;

                        //First Table
                        Table table = doc.Sections[0].Tables[2] as Spire.Doc.Table;
                        doc.Replace("#table2", "أولاً: الإعداد والتأكد من الوثائق", false, true);
                        BuildATable(table, phaseDetails, 0, 4);

                        //Second Table
                        table = doc.Sections[0].Tables[3] as Spire.Doc.Table;
                        doc.Replace("#table3", "ثانياً: مراجعة وتدقيق المخططات وفق نماذج مراجعة المخططات", false, true);
                        BuildATable(table, phaseDetails, 4, 8);

                        //Third Table
                        table = doc.Sections[0].Tables[4] as Spire.Doc.Table;
                        doc.Replace("#table4", "ثالثاً: متطلبات أعمال الوقاية والحماية من الحرائق** ", false, true);
                        BuildATable(table, phaseDetails, 12 , 9);

                        //from 4th to 12th
                        for (int j = 12; j > 4; j--)
                        {
                            //Fourth Table (Delete It)
                            table = doc.Sections[0].Tables[j] as Spire.Doc.Table;
                            //doc.Replace("#table5", "ثانياً: مراجعة وتدقيق المخططات وفق نماذج مراجعة المخططات", false, true);
                            //BuildATable(doc, table);
                            doc.Sections[0].Body.ChildObjects.Remove(table);
                        }
                        break;
                    }
                case 2:
                    {
                        int tableCounts = doc.Sections[0].Tables.Count;
                        if (tableCounts == 0)
                            break;

                        //First Table
                        Table table = doc.Sections[0].Tables[2] as Spire.Doc.Table;
                        doc.Replace("#table2", "أولاً: الإعداد والتجهيز", false, true);
                        BuildATable(table, phaseDetails, 0, 8);

                        //Second Table
                        table = doc.Sections[0].Tables[3] as Spire.Doc.Table;
                        doc.Replace("#table3", "ثانياً: أعمال الحفر", false, true);
                        BuildATable(table, phaseDetails, 8, 4);

                        //Third Table
                        table = doc.Sections[0].Tables[4] as Spire.Doc.Table;
                        doc.Replace("#table4", "ثالثاً: أعمال إزالة المياه من الموقع** ", false, true);
                        BuildATable(table, phaseDetails, 12, 4);

                        //4th Table
                        table = doc.Sections[0].Tables[5] as Spire.Doc.Table;
                        doc.Replace("#table5", "رابعاً: تجهزيات أعمال صب الخرسانة", false, true);
                        BuildATable(table, phaseDetails, 16, 9);

                        //5th Table
                        table = doc.Sections[0].Tables[6] as Spire.Doc.Table;
                        doc.Replace("#table6", "خامساً: تجهيزات أعمال التمديدات الصحية وفقاً لكود البناء(SBC (701, 702)), ومنها:", false, true);
                        BuildATable(table, phaseDetails, 25, 5);

                        //6th Table
                        table = doc.Sections[0].Tables[7] as Spire.Doc.Table;
                        doc.Replace("#table7", "سادساً: الأعمال الكهربائية والطاقة وفقاً لكود البناء(SBC (401, 601, 602)), ومنها:", false, true);
                        BuildATable(table, phaseDetails, 30, 5);

                        //7th Table
                        table = doc.Sections[0].Tables[8] as Spire.Doc.Table;
                        doc.Replace("#table8", "سابعاً: أعمال الوقاية والحماية من الحرائق**", false, true);
                        BuildATable(table, phaseDetails, 34, 2);

                        //from 4th to 12th
                        for (int j = 12; j > 8; j--)
                        {
                            //Fourth Table (Delete It)
                            table = doc.Sections[0].Tables[j] as Spire.Doc.Table;
                            //doc.Replace("#table5", "ثانياً: مراجعة وتدقيق المخططات وفق نماذج مراجعة المخططات", false, true);
                            //BuildATable(doc, table);
                            doc.Sections[0].Body.ChildObjects.Remove(table);
                        }
                        break;
                    }
                case 3:
                    {
                        int tableCounts = doc.Sections[0].Tables.Count;
                        if (tableCounts == 0)
                            break;

                        //First Table
                        Table table = doc.Sections[0].Tables[2] as Spire.Doc.Table;
                        doc.Replace("#table2", "", false, true);
                        BuildATable(table, phaseDetails, 0, 5);

                        //from 4th to 12th
                        for (int j = 12; j > 2; j--)
                        {
                            //Fourth Table (Delete It)
                            table = doc.Sections[0].Tables[j] as Spire.Doc.Table;
                            //doc.Replace("#table5", "ثانياً: مراجعة وتدقيق المخططات وفق نماذج مراجعة المخططات", false, true);
                            //BuildATable(doc, table);
                            doc.Sections[0].Body.ChildObjects.Remove(table);
                        }
                        break;
                    }
                case 4:
                    {
                        int tableCounts = doc.Sections[0].Tables.Count;
                        if (tableCounts == 0)
                            break;

                        //First Table
                        Table table = doc.Sections[0].Tables[2] as Spire.Doc.Table;
                        doc.Replace("#table2", "أولاً: الأعمال الإنشائية", false, true);
                        BuildATable(table, phaseDetails, 0, 10);
                        //2
                        table = doc.Sections[0].Tables[3] as Spire.Doc.Table;
                        doc.Replace("#table3", "ثانياً: أعمال الصرف الصحي", false, true);
                        BuildATable(table, phaseDetails, 10, 15);
                        //3
                        table = doc.Sections[0].Tables[4] as Spire.Doc.Table;
                        doc.Replace("#table4", "ثالثاً: الأعمال الكهربائية", false, true);
                        BuildATable(table, phaseDetails, 25, 1);
                        //4
                        table = doc.Sections[0].Tables[5] as Spire.Doc.Table;
                        doc.Replace("#table5", "ثالثاً: الأعمال الكهربائية", false, true);
                        BuildATable(table, phaseDetails, 26, 4);

                        //from 4th to 12th
                        for (int j = 12; j > 5; j--)
                        {
                            //Fourth Table (Delete It)
                            table = doc.Sections[0].Tables[j] as Spire.Doc.Table;
                            //doc.Replace("#table5", "ثانياً: مراجعة وتدقيق المخططات وفق نماذج مراجعة المخططات", false, true);
                            //BuildATable(doc, table);
                            doc.Sections[0].Body.ChildObjects.Remove(table);
                        }
                        break;
                    }
                case 5:
                    {

                        int tableCounts = doc.Sections[0].Tables.Count;
                        if (tableCounts == 0)
                            break;

                        //First Table
                        Table table = doc.Sections[0].Tables[2] as Spire.Doc.Table;
                        doc.Replace("#table2", "أولاً", false, true);
                        BuildATable(table, phaseDetails, 0, 5);

                        //from 4th to 12th
                        for (int j = 12; j > 2; j--)
                        {
                            //Fourth Table (Delete It)
                            table = doc.Sections[0].Tables[j] as Spire.Doc.Table;
                            //doc.Replace("#table5", "ثانياً: مراجعة وتدقيق المخططات وفق نماذج مراجعة المخططات", false, true);
                            //BuildATable(doc, table);
                            doc.Sections[0].Body.ChildObjects.Remove(table);
                        }
                        break;
                    }
                case 6:
                    {

                        int tableCounts = doc.Sections[0].Tables.Count;
                        if (tableCounts == 0)
                            break;

                        //First Table
                        Table table = doc.Sections[0].Tables[2] as Spire.Doc.Table;
                        doc.Replace("#table2", "قبل صب الأعمدة", false, true);
                        BuildATable(table, phaseDetails, 0, 13);

                        //from 4th to 12th
                        for (int j = 12; j > 2; j--)
                        {
                            //Fourth Table (Delete It)
                            table = doc.Sections[0].Tables[j] as Spire.Doc.Table;
                            //doc.Replace("#table5", "ثانياً: مراجعة وتدقيق المخططات وفق نماذج مراجعة المخططات", false, true);
                            //BuildATable(doc, table);
                            doc.Sections[0].Body.ChildObjects.Remove(table);
                        }
                        break;
                    }
                case 7:
                    {
                        int tableCounts = doc.Sections[0].Tables.Count;
                        if (tableCounts == 0)
                            break;

                        //First Table
                        Table table = doc.Sections[0].Tables[2] as Spire.Doc.Table;
                        doc.Replace("#table2", "أولاً: متطلبات تنفيذ الخرسانة وفقاً لكود البناء (SBC 201 الفصل التاسع عشر)", false, true);
                        BuildATable(table, phaseDetails, 0, 12);
                        //2
                        table = doc.Sections[0].Tables[3] as Spire.Doc.Table;
                        doc.Replace("#table3", "ثانياً: بعد صب الأعمدة", false, true);
                        BuildATable(table, phaseDetails, 12, 5);
                        //3
                        table = doc.Sections[0].Tables[4] as Spire.Doc.Table;
                        doc.Replace("#table4", "ثالثاً: البلاطة الخرسانية", false, true);
                        BuildATable(table, phaseDetails, 17, 6);
                        //4
                        table = doc.Sections[0].Tables[5] as Spire.Doc.Table;
                        doc.Replace("#table5", "تابع رابعاً: البلاطة الخرسانية", false, true);

                        Table nestedTable = table[1, 1].Tables[0] as Spire.Doc.Table;
                        Paragraph p;
                        int index = 23;
                        nestedTable.TableFormat.Bidi = true;
                        //Insert a new row as the Second row
                        for (int i = 1; i < 16; i++)
                        {
                            nestedTable.AddRow(true, 6);
                            p = nestedTable[i, 0].AddParagraph();
                            p.Format.IsBidi = true;
                            //p.Format.HorizontalAlignment = HorizontalAlignment.Right;
                            p.AppendText((i + 6).ToString());

                            p = nestedTable[i, 1].AddParagraph();
                            p.Format.IsBidi = true;
                            //p.Format.HorizontalAlignment = HorizontalAlignment.Right;
                            p.AppendText(phaseDetails[index].NameAr);

                            p = nestedTable[i, 2].AddParagraph();
                            p.Format.IsBidi = true;
                            //p.Format.HorizontalAlignment = HorizontalAlignment.Right;
                            p.AppendText("-");

                            p = nestedTable[i, 3].AddParagraph();
                            p.Format.IsBidi = true;
                            //p.Format.HorizontalAlignment = HorizontalAlignment.Right;
                            p.AppendText("-");

                            p = nestedTable[i, 4].AddParagraph();
                            p.Format.IsBidi = true;
                            //p.Format.HorizontalAlignment = HorizontalAlignment.Right;
                            p.AppendText("-");

                            p = nestedTable[i, 5].AddParagraph();
                            p.Format.IsBidi = true;
                            //p.Format.HorizontalAlignment = HorizontalAlignment.Right;
                            p.AppendText("-");

                            index++;
                        }
                        //Remove The empty row (first)
                        nestedTable.Rows.RemoveAt(0);

                        //5
                        table = doc.Sections[0].Tables[6] as Spire.Doc.Table;
                        doc.Replace("#table6", "خامساً: تجهيزات أعمال التمديدات الصحية وفقا لكود البناء (SBC 701, 702) ومنها:", false, true);
                        BuildATable(table, phaseDetails, 38, 2);
                        //6
                        table = doc.Sections[0].Tables[7] as Spire.Doc.Table;
                        doc.Replace("#table7", "سادساً: الأعمال الكهربائية و الطاقة وفقاً لأكواد البناء (SBC 401- 601- 602) ومنها:", false, true);
                        BuildATable(table, phaseDetails, 40, 9);
                        //7
                        table = doc.Sections[0].Tables[8] as Spire.Doc.Table;
                        doc.Replace("#table8", "سابعاً: متطلبات الأمن والسلامة بالموقع", false, true);
                        BuildATable(table, phaseDetails, 49, 2);

                        //8
                        table = doc.Sections[0].Tables[9] as Spire.Doc.Table;
                        doc.Replace("#table9", "ثامناً: تجهيزات أعمال التمديدات الصحية وفقا لكود البناء (SBC 701, 702) ومنها: ", false, true);
                        BuildATable(table, phaseDetails, 51, 2);

                        //9
                        table = doc.Sections[0].Tables[10] as Spire.Doc.Table;
                        doc.Replace("#table10", "تاسعاً: أعمال الحماية والوقاية من الحريق**", false, true);
                        BuildATable(table, phaseDetails, 53, 16);

                        InsertARowTitle(table, "أنظمة الاطفاء", 0);
                        InsertARowTitle(table, "نظام الانذار", 5);
                        InsertARowTitle(table, "نظام التحكم في الدخان", 10);
                        InsertARowTitle(table, "أنظمة أخرى (....................)", 15);

                        //from 4th to 12th
                        for (int j = 12; j > 10; j--)
                        {
                            //Fourth Table (Delete It)
                            table = doc.Sections[0].Tables[j] as Spire.Doc.Table;
                            //doc.Replace("#table5", "ثانياً: مراجعة وتدقيق المخططات وفق نماذج مراجعة المخططات", false, true);
                            //BuildATable(doc, table);
                            doc.Sections[0].Body.ChildObjects.Remove(table);
                        }
                        break;
                    }
                case 8:
                    {
                        int tableCounts = doc.Sections[0].Tables.Count;
                        if (tableCounts == 0)
                            break;

                        //First Table
                        Table table = doc.Sections[0].Tables[2] as Spire.Doc.Table;
                        doc.Replace("#table2", "بعد الصب البلاطة الخرسانية", false, true);
                        BuildATable(table, phaseDetails, 0, 3);

                        //from 4th to 12th
                        for (int j = 12; j > 2; j--)
                        {
                            //Fourth Table (Delete It)
                            table = doc.Sections[0].Tables[j] as Spire.Doc.Table;
                            //doc.Replace("#table5", "ثانياً: مراجعة وتدقيق المخططات وفق نماذج مراجعة المخططات", false, true);
                            //BuildATable(doc, table);
                            doc.Sections[0].Body.ChildObjects.Remove(table);
                        }
                        break;
                    }
                case 9:
                    {
                        int tableCounts = doc.Sections[0].Tables.Count;
                        if (tableCounts == 0)
                            break;

                        //First Table
                        Table table = doc.Sections[0].Tables[2] as Spire.Doc.Table;
                        doc.Replace("#table2", "أولاً: أعمال المباني", false, true);
                        BuildATable(table, phaseDetails, 0, 12);

                        //2 Table
                        table = doc.Sections[0].Tables[3] as Spire.Doc.Table;
                        doc.Replace("#table3", "ثانياً :أعمال الكهرباء", false, true);
                        BuildATable(table, phaseDetails, 12, 8);

                        //3 Table
                        table = doc.Sections[0].Tables[4] as Spire.Doc.Table;
                        doc.Replace("#table4", "ثالثاً:أعمال الغاز", false, true);
                        BuildATable(table, phaseDetails, 20, 3);

                        //4 Table
                        table = doc.Sections[0].Tables[5] as Spire.Doc.Table;
                        doc.Replace("#table5", "رابعاً: أعمال العزل الحراري والمائي", false, true);
                        BuildATable(table, phaseDetails, 23, 5);

                        //5 Table
                        table = doc.Sections[0].Tables[6] as Spire.Doc.Table;
                        doc.Replace("#table6", "خامساً:أنظمة التدفئة والتهوية والتكييف", false, true);
                        BuildATable(table, phaseDetails, 28, 4);

                        //6 Table
                        table = doc.Sections[0].Tables[7] as Spire.Doc.Table;
                        doc.Replace("#table7", "سادساً: تركيب أنظمة تسخين المياه", false, true);
                        BuildATable(table, phaseDetails, 32, 3);
                        //
                        InsertARowTitle(table, @"التأكد من أن المواسير والصمامات والتجهيزات المسبقة الأخرى تتماشى مع النظام العام لتسخين المياه)وثائق التخطيط(وتم عزل أنابيب المياه الساخنة وفق متطلبات قسم )SBC 602(السعودي البناء كود(7.3.1.5(", 1);

                        //7 Table
                        table = doc.Sections[0].Tables[8] as Spire.Doc.Table;
                        doc.Replace("#table8", "سابعاً: أعمال الحماية من الصواعق/التأريض", false, true);
                        BuildATable(table, phaseDetails, 35, 2);

                        //8Table
                        table = doc.Sections[0].Tables[9] as Spire.Doc.Table;
                        doc.Replace("#table9", "ثامناً: الحماية من الحرائق**", false, true);
                        BuildATable(table, phaseDetails, 37, 13);

                        //from 4th to 12th
                        for (int j = 12; j > 9; j--)
                        {
                            //Fourth Table (Delete It)
                            table = doc.Sections[0].Tables[j] as Spire.Doc.Table;
                            //doc.Replace("#table5", "ثانياً: مراجعة وتدقيق المخططات وفق نماذج مراجعة المخططات", false, true);
                            //BuildATable(doc, table);
                            doc.Sections[0].Body.ChildObjects.Remove(table);
                        }
                        break;
                    }
                case 10:
                    {
                        int tableCounts = doc.Sections[0].Tables.Count;
                        if (tableCounts == 0)
                            break;

                        //First Table
                        Table table = doc.Sections[0].Tables[2] as Spire.Doc.Table;
                        doc.Replace("#table2", "أولاً: تمديدات الخاصة بالمياه", false, true);
                        BuildATable(table, phaseDetails, 0, 11);

                        //2 Table
                        table = doc.Sections[0].Tables[3] as Spire.Doc.Table;
                        doc.Replace("#table3", "ثانياً :عزل دورات المياه والأماكن المبللة", false, true);
                        BuildATable(table, phaseDetails, 11, 5);

                        //3 Table
                        table = doc.Sections[0].Tables[4] as Spire.Doc.Table;
                        doc.Replace("#table4", "ثالثاً: أعمال تركيبات الصرف الصحي", false, true);
                        BuildATable(table, phaseDetails, 16, 18);

                        //4 Table
                        table = doc.Sections[0].Tables[5] as Spire.Doc.Table;
                        doc.Replace("#table5", "رابعاً: تمديدات الحماية من الحرائق**", false, true);
                        BuildATable(table, phaseDetails, 34, 10);

                        //5 Table
                        table = doc.Sections[0].Tables[6] as Spire.Doc.Table;
                        doc.Replace("#table6", "خامساً: تمديدات أعمال المرافق الخارجية", false, true);
                        BuildATable(table, phaseDetails, 44, 4);

                        //6 Table
                        table = doc.Sections[0].Tables[7] as Spire.Doc.Table;
                        doc.Replace("#table7", "سادساً: التمديدات الكهربائية", false, true);
                        BuildATable(table, phaseDetails, 48, 13);
                       

                        //from 4th to 12th
                        for (int j = 12; j > 7; j--)
                        {
                            //Fourth Table (Delete It)
                            table = doc.Sections[0].Tables[j] as Spire.Doc.Table;
                            //doc.Replace("#table5", "ثانياً: مراجعة وتدقيق المخططات وفق نماذج مراجعة المخططات", false, true);
                            //BuildATable(doc, table);
                            doc.Sections[0].Body.ChildObjects.Remove(table);
                        }
                        break;
                    }
                case 11:
                    {
                        int tableCounts = doc.Sections[0].Tables.Count;
                        if (tableCounts == 0)
                            break;

                        //First Table
                        Table table = doc.Sections[0].Tables[2] as Spire.Doc.Table;
                        doc.Replace("#table2", "أولاً: أعمال العزل", false, true);
                        BuildATable(table, phaseDetails, 0, 16);

                        //2 Table
                        table = doc.Sections[0].Tables[3] as Spire.Doc.Table;
                        doc.Replace("#table3", "ثانياً : أعمال الحماية من الحرائق**", false, true);
                        BuildATable(table, phaseDetails, 16, 16);

                        //3 Table
                        table = doc.Sections[0].Tables[4] as Spire.Doc.Table;
                        doc.Replace("#table4", "ثالثاً: :متطلبات الأمن والسلامة بالموقع", false, true);
                        BuildATable(table, phaseDetails, 32, 2);

                        //from 4th to 12th
                        for (int j = 12; j > 4; j--)
                        {
                            //Fourth Table (Delete It)
                            table = doc.Sections[0].Tables[j] as Spire.Doc.Table;
                            //doc.Replace("#table5", "ثانياً: مراجعة وتدقيق المخططات وفق نماذج مراجعة المخططات", false, true);
                            //BuildATable(doc, table);
                            doc.Sections[0].Body.ChildObjects.Remove(table);
                        }
                        break;
                    }
                case 12:
                    {
                        int tableCounts = doc.Sections[0].Tables.Count;
                        if (tableCounts == 0)
                            break;

                        //First Table
                        Table table = doc.Sections[0].Tables[2] as Spire.Doc.Table;
                        doc.Replace("#table2", "أولاً: أعمال البلاط", false, true);
                        BuildATable(table, phaseDetails, 0, 7);

                        //2 Table
                        table = doc.Sections[0].Tables[3] as Spire.Doc.Table;
                        doc.Replace("#table3", "ثانياً :أعمال النوافذ", false, true);
                        BuildATable(table, phaseDetails, 7, 8);

                        //3 Table
                        table = doc.Sections[0].Tables[4] as Spire.Doc.Table;
                        doc.Replace("#table4", "ثالثاً: أعمال الأبواب", false, true);
                        BuildATable(table, phaseDetails, 15, 17);
                       
                        InsertARowTitle(table, @"العزل المستخدم للنوافذ والقباب مطابقة لمتطلبات كود البناء السعودي (601 SBC) والمذكورة في الجدول 1.5 ومتوافقة مع تقرير مراجعة التصميم", 16);

                        InsertARowTitle(table, @"العزل المستخدم للنوافذ والقباب مطابقة لمتطلبات كود البناء السعودي (602 SBC) والمذكورة في الجدول (5.2) ومتوافقة مع تقرير مراجعة التصميم", 17);

                        InsertARowTitle(table, @"أبواب الشحن وأبواب منصات التحميل مجهزة بمغاليق محكمه ضد الظروف الجوية seals weather للحد من التسرب عند وقوف المركبات في المداخل (SBC 601) (5.4.3.3) ", 18, "16");

                        table = doc.Sections[0].Tables[5] as Spire.Doc.Table;
                        doc.Replace("#table5", "رابعاً: أعمال الحماية من الحريق**", false, true);
                        BuildATable(table, phaseDetails, 32, 6);

                        InsertARowTitle(table, "التشطيبات الداخلية", 6);

                        InsertARowTitle(table, "تقارير مواد تغطية الأسقف", 7, "1");
                        InsertARowTitle(table, "تقارير تشطيبات الجدران", 8, "2");
                        InsertARowTitle(table, "تقارير الديكورات", 9, "3");
                        InsertARowTitle(table, "تم استلام التشطيبات الداخلية من المقاول", 10, "4");
                        InsertARowTitle(table, "التأكد من أي متطلبات أخرى للوقاية والحماية من الحريق أثناء الانشاء", 11, "5");

                        //from 4th to 12th
                        for (int j = 12; j > 5; j--)
                        {
                            //Fourth Table (Delete It)
                            table = doc.Sections[0].Tables[j] as Spire.Doc.Table;
                            //doc.Replace("#table5", "ثانياً: مراجعة وتدقيق المخططات وفق نماذج مراجعة المخططات", false, true);
                            //BuildATable(doc, table);
                            doc.Sections[0].Body.ChildObjects.Remove(table);
                        }
                        break;
                    }
                case 13:
                    {
                        int tableCounts = doc.Sections[0].Tables.Count;
                        if (tableCounts == 0)
                            break;

                        //First Table
                        Table table = doc.Sections[0].Tables[2] as Spire.Doc.Table;
                        doc.Replace("#table2", "أولاً:  الأعمال الإنشائية", false, true);
                        BuildATable(table, phaseDetails, 0, 3);

                        //2 Table
                        table = doc.Sections[0].Tables[3] as Spire.Doc.Table;
                        doc.Replace("#table3", "ثانياً :أعمال التركيبات الكهربائية", false, true);
                        BuildATable(table, phaseDetails, 3, 9);

                        //3 Table
                        table = doc.Sections[0].Tables[4] as Spire.Doc.Table;
                        doc.Replace("#table4", "ثالثاً: أعمال الحماية من الحرائق**", false, true);
                        BuildATable(table, phaseDetails, 12, 14);

                        InsertARowTitle(table, "التأكد من التجهيزات الخاصة", 14);
                        InsertARowTitle(table, "متطلبات مولدات الطاقة الكهربائية ومواصفاتها", 15,"1");
                        InsertARowTitle(table, "الأفران الصناعية OVENS INDUSTRIAL ومتطلبات حمايتها", 16, "2");
                        InsertARowTitle(table, "غرف الرنين المغناطيسي MRB ومتطلبات حمايته", 17, "3");
                        InsertARowTitle(table, " أجهزة العالج الهايبربارك HPERBARIC ومتطلبات حمايتها", 18, "4");
                        InsertARowTitle(table, " مداخن المطابخ HOODS KITCHEN ومتطلبات حمايتها من التجهيزات الخاصة", 19, "5");
                        InsertARowTitle(table, "خزانات الوقود ومتطلبات حمايتها ", 20,"6");
                        InsertARowTitle(table, "الغاليات ومتطلبات حمايتها ", 21, "7");
                        InsertARowTitle(table, "التأكد من أنظمة التهوية وسحب الدخان والتحكم به", 22, "8");
                        InsertARowTitle(table, "التأكد من ممرات رجال الإطفاء وانه لا توجد عوائق في جميع انحاء المبنى تعيق اعمال الإنقاذ ", 23, "9");
                        InsertARowTitle(table, "التأكد من مسارات سيارات الإطفاء وانه لا توجد عوائق أمام سيارات الإطفاء ", 24, "10");
                        InsertARowTitle(table, "التأكد من أعمال التخزين ومطابقتها للكود SBC801 ", 25, "11");
                        InsertARowTitle(table, "تذكر أية أعمال أخرى تم فحصها", 26, "12");

                        //4 Table
                        table = doc.Sections[0].Tables[5] as Spire.Doc.Table;
                        doc.Replace("#table5", "رابعا :'المسابح وحمامات البخار", false, true);
                        BuildATable(table, phaseDetails, 38, 4);

                        //5 Table
                        table = doc.Sections[0].Tables[6] as Spire.Doc.Table;
                        doc.Replace("#table6", "خامسا : أعمال النوافذ والتثبيت", false, true);
                        BuildATable(table, phaseDetails, 42, 6);

                        //6 Table
                        table = doc.Sections[0].Tables[7] as Spire.Doc.Table;
                        doc.Replace("#table7", "سادسا: أنظمة تسخين المياه", false, true);
                        BuildATable(table, phaseDetails, 48, 2);

                        //7 Table
                        table = doc.Sections[0].Tables[8] as Spire.Doc.Table;
                        doc.Replace("#table8", "سابعا: أعمال التدفئة والتهوية والتكييف", false, true);
                        BuildATable(table, phaseDetails, 50, 18);

                        //8 Table
                        table = doc.Sections[0].Tables[9] as Spire.Doc.Table;
                        doc.Replace("#table9", "ثامنا : الأبواب", false, true);
                        BuildATable(table, phaseDetails, 68, 4);

                        //from 4th to 12th
                        for (int j = 12; j > 9; j--)
                        {
                            //Fourth Table (Delete It)
                            table = doc.Sections[0].Tables[j] as Spire.Doc.Table;
                            //doc.Replace("#table5", "ثانياً: مراجعة وتدقيق المخططات وفق نماذج مراجعة المخططات", false, true);
                            //BuildATable(doc, table);
                            doc.Sections[0].Body.ChildObjects.Remove(table);
                        }
                        break;
                    }
                default:
                    break;
            }
        }

        private static void BuildATable(Table table, List<Pro_SupervisionDetailsVM> phaseDetails, int startIndex, int length)
        {

            Table nestedTable = table[1, 1].Tables[0] as Spire.Doc.Table;
            try
            {
                Paragraph p;

                int index = 0;
                nestedTable.TableFormat.Bidi = true;
                //Insert a new row as the Second row
                for (int i = 1; i < length + 1; i++)
                {
                    index = startIndex + (i - 1);
                    nestedTable.AddRow(true, 6);
                    p = nestedTable[i, 0].AddParagraph();
                    p.Format.IsBidi = true;
                    p.ApplyStyle("MyStyle");
                    p.AppendText(i.ToString());

                    p = nestedTable[i, 1].AddParagraph();
                    p.Format.IsBidi = true;
                    p.ApplyStyle("MyStyle");
                    p.AppendText(phaseDetails[index].NameAr);

                    p = nestedTable[i, 2].AddParagraph();
                    p.Format.IsBidi = true;
                    p.ApplyStyle("MyStyle");
                    if (phaseDetails[index].IsRead==1)
                    {
                        p.AppendText("مطابق");
                    }
                    else
                    {
                        p.AppendText(" ");
                    }
                    

                    p = nestedTable[i, 3].AddParagraph();
                    p.Format.IsBidi = true;
                    p.ApplyStyle("MyStyle");
                    if (phaseDetails[index].IsRead == 0)
                    {
                        p.AppendText("غ.مطابق");                       
                    }
                    else
                    {
                        p.AppendText(" ");
                    }


                    p = nestedTable[i, 4].AddParagraph();
                    p.Format.IsBidi = true;
                    p.ApplyStyle("MyStyle");
                    if (phaseDetails[index].IsRead == 2)
                    {
                        p.AppendText("غ.متوفر");
                    }
                    else
                    {
                        p.AppendText(" ");
                    }

                    p = nestedTable[i, 5].AddParagraph();
                    p.Format.IsBidi = true;
                    p.ApplyStyle("MyStyle");
                    if (phaseDetails[index].Note == null || phaseDetails[index].Note=="" || phaseDetails[index].Note == "null" || phaseDetails[index].Note == "NULL")
                    {
                        p.AppendText(" ");
                    }
                    else
                    {
                        p.AppendText(phaseDetails[index].Note??"");
                    }
                       

                }
                //Remove The empty row (first)
                nestedTable.Rows.RemoveAt(0);
            }
            catch
            {
                if (nestedTable.Rows.Count > 0)
                {
                    nestedTable.Rows.RemoveAt(0);
                }
            }
        }
        private static void InsertARowTitle(Table table, string Title, int index, string indexString = "")
        {
            try
            {
                Table nestedTable = table[1, 1].Tables[0] as Spire.Doc.Table;
                Paragraph p;
                nestedTable.TableFormat.Bidi = true;
                //Insert a new row as the Second row
                TableRow row = nestedTable.AddRow();
                nestedTable.Rows.Insert(index, row);

                p = nestedTable[index, 0].AddParagraph();
                p.Format.IsBidi = true;
                p.ApplyStyle("MyStyle");
                p.AppendText(indexString);

                p = nestedTable[index, 1].AddParagraph();
                p.Format.IsBidi = true;
                p.ApplyStyle("MyStyle");
                p.AppendText(Title);

                p = nestedTable[index, 2].AddParagraph();
                p.Format.IsBidi = true;
                p.ApplyStyle("MyStyle");
                p.AppendText("");

                p = nestedTable[index, 3].AddParagraph();
                p.Format.IsBidi = true;
                p.ApplyStyle("MyStyle");
                p.AppendText("");

                p = nestedTable[index, 4].AddParagraph();
                p.Format.IsBidi = true;
                p.ApplyStyle("MyStyle");
                p.AppendText("");

                p = nestedTable[index, 5].AddParagraph();
                p.Format.IsBidi = true;
                p.ApplyStyle("MyStyle");
                p.AppendText("");
            }
            catch
            {
            }
        }
        private bool RemoveRedLine(string filePath)
        {
            try
            {
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, true))
                {
                    string docText = null;
                    using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                    {
                        docText = sr.ReadToEnd();
                    }

                    Regex regexText = new Regex("Evaluation Warning: The document was created with Spire.Doc for .NET.");
                    docText = regexText.Replace(docText, "");

                    using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                    {
                        sw.Write(docText);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        /// <summary>
        /// Using Microsoft.Office.Interop
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <param name="format"></param>
        public static void Convert(string input, string output, WdSaveFormat format)
        {
            // Create an instance of Word.exe
            Microsoft.Office.Interop.Word._Application oWord = new Microsoft.Office.Interop.Word.Application();

            // Make this instance of word invisible (Can still see it in the taskmgr).
            oWord.Visible = false;

            // Interop requires objects.
            object oMissing = System.Reflection.Missing.Value;
            object isVisible = true;
            object readOnly = false;
            object oInput = input;
            object oOutput = output;
            object oFormat = format;
            object SaveChanges = false;
            object openAndRepair = false;
            object DocumentDirection = WdDocumentDirection.wdRightToLeft;

            // Load a document into our instance of word.exe
            Microsoft.Office.Interop.Word._Document oDoc = oWord.Documents.Open(ref oInput, ref oMissing, ref readOnly, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref isVisible, ref openAndRepair, ref oMissing, ref oMissing, ref oMissing);

            // Make this document the active document.
            oDoc.Activate();
            oDoc.ExportAsFixedFormat(oOutput.ToString(), WdExportFormat.wdExportFormatPDF, false, WdExportOptimizeFor.wdExportOptimizeForOnScreen,
                                WdExportRange.wdExportAllDocument, 1, 1, WdExportItem.wdExportDocumentContent, true, true,
                                WdExportCreateBookmarks.wdExportCreateHeadingBookmarks, true, true, false, ref oMissing);            // Save this document in Word 2003 format.
            //oDoc.SaveAs(ref oOutput, ref oFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

            // Always close Word.exe.
            oWord.Quit(ref SaveChanges, ref oMissing, ref oMissing);
        }

    }
}