using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using Paragraph = Spire.Doc.Documents.Paragraph;
using System.Drawing;
using TaamerProject.Service.Interfaces;
using TaamerProject.Service.Services;
using System.Net;
using TaamerProject.API.Helper;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using System.Text.RegularExpressions;
using Spire.Doc;
using System.Globalization;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TaamerP.Service.LocalResources;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Drawing;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class DraftController : ControllerBase
    {

            private IDraftService _DrafService;
            private IDraftDetailsService _DraftDetailsService;

            private readonly IContractService _contractservice;
        private IContractServicesService _contractService;

        private ICustomerService _customerservice;
            private readonly IBranchesService _branchservice;

            public IOrganizationsService _organizationservice { get; }

            private readonly IContractDetailsService _contractDetailsService;
            private IEmployeesService _employeeService;
            private IServicesPriceService _servicesPriceService;
            private IDraftService _draftService;
            private IProjectService _projectService;
            private ICustomerPaymentsService _customerPaymentservice;
        private readonly IServicesPriceOfferService _servicesPriceOfferService;
        private readonly IDrafts_TemplatesService _drafts_TemplatesService;


        private IConfiguration Configuration;
        public GlobalShared _globalshared;
        string Con;
        private readonly IWebHostEnvironment _hostingEnvironment;


        public DraftController(IDraftService draftService, IDraftDetailsService draftDetailsService, IBranchesService  branchesService, IOrganizationsService organizationsService,
                IContractDetailsService contractDetailsService  , ICustomerService customerService, IEmployeesService employeesService, IServicesPriceService servicesPriceService,
                  IServicesPriceOfferService servicesPriceOfferService, IDrafts_TemplatesService drafts_TemplatesService, IContractServicesService contractServicesService, IDraftService draftService1, IProjectService projectService, IContractService contractService, ICustomerPaymentsService customerPaymentsService, IWebHostEnvironment webHostEnvironment)
            {
            _contractService = contractServicesService;

            this._DrafService = draftService;
                this._DraftDetailsService = draftDetailsService;
                _employeeService = employeesService;
                _servicesPriceService = servicesPriceService;
                _draftService = draftService1;
                _projectService = projectService;
                _contractservice = contractService;
                this._customerservice = customerService;
                this._branchservice = branchesService;
                this._organizationservice = organizationsService;
                _contractDetailsService = contractDetailsService;
                this._customerPaymentservice = customerPaymentsService;
            _servicesPriceOfferService = servicesPriceOfferService;
            _drafts_TemplatesService = drafts_TemplatesService;


            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            _hostingEnvironment = webHostEnvironment;

        }
        [HttpPost("SaveDraft")]
        public IActionResult SaveDraft(Draft draft,IFormFile UploadedFile,  int ContractId)
            {
            GeneralMessage result = null;

            try
            { 


            if (!UploadedFile.FileName.Contains(".doc") && !UploadedFile.FileName.Contains(".pdf"))
            {
                result = new GeneralMessage() { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = " يرجى أن يكون الملف بإحدى الصيغ : '.doc', '.docx', '.pdf' " };
            }
            if (UploadedFile != null)
            {
                System.Net.Http.HttpResponseMessage response = new System.Net.Http.HttpResponseMessage();


                string path = System.IO.Path.Combine("Uploads/", "Drafts/");
                string pathW = System.IO.Path.Combine("/Uploads/", "Drafts/");

                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                List<string> uploadedFiles = new List<string>();
                string pathes = "";
                //foreach (IFormFile postedFile in postedFiles)
                //{
                string fileName = System.IO.Path.GetFileName(Guid.NewGuid() + UploadedFile.FileName);
                //string fileName = System.IO.Path.GetFileName(postedFiles.FileName);

                var path2 = System.IO.Path.Combine(path, fileName);
                if (System.IO.File.Exists(path2))
                {
                    System.IO.File.Delete(path2);
                }
                using (System.IO.FileStream stream = new System.IO.FileStream(System.IO.Path.Combine(path, fileName), System.IO.FileMode.Create))
                {


                    UploadedFile.CopyTo(stream);
                    uploadedFiles.Add(fileName);
                    // string returnpath = host + path + fileName;
                    //pathes.Add(pathW + fileName);
                    pathes = pathW + fileName;
                }


                if (pathes != null)
                {
                    draft.DraftUrl = pathes;
                    draft.Name = fileName;
                }

                {
                    var UrlS = _hostingEnvironment.WebRootPath.Replace("Draft/SaveDraft", "");
                    draft.DraftUrl = UrlS + "Uploads/Drafts/" + fileName;
                    result = _draftService.SaveDraft(draft, _globalshared.UserId_G, _globalshared.BranchId_G);
                }
            }
        }
                        catch (Exception ex)
                        {
                            var massege = "";
                            if (_globalshared.Lang_G == "rtl")
                            {
                                massege = "فشل في رفع المسودة";
                            }
                            else if (_globalshared.Lang_G == "ltr")
                            {
                                massege = "Uploaded Failed";
                            }
                      return Ok(new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = massege });
                        }
                   
                
                  return Ok(result);
            //string fileLocation = "";
            //    GeneralMessage result = null;
            //    if (!Request.Files["UploadedFile"].FileName.Contains(".doc") && !Request.Files["UploadedFile"].FileName.Contains(".pdf"))
            //    {
            //        result = new GeneralMessage() { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = " يرجى أن يكون الملف بإحدى الصيغ : '.doc', '.docx', '.pdf' " };
            //    }
            //    HttpPostedFileBase file = Request.Files["UploadedFile"];
            //    if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
            //    {
            //        if (Request.Files["UploadedFile"].ContentLength > 0)
            //        {
            //            string fileName = System.IO.Path.GetFileName(GenerateRandomNo() + Request.Files["UploadedFile"].FileName);

            //            fileLocation = Server.MapPath("~/Uploads/Drafts/") + fileName;
            //            try
            //            {
            //                Request.Files["UploadedFile"].SaveAs(fileLocation);
            //                draft.DraftUrl = "/Uploads/Drafts/" + fileName;
            //                draft.Name = fileName;

            //                //Not exists 
            //                //if (draft.Name.Contains(".doc") && (_draftService.GetAllDrafts().Where(x => x.DraftName == draft.Name).FirstOrDefault() == null))
            //                //    result = appendFile_Draft(ContractId, fileLocation);
            //                //else //If exists before
            //                {
            //                    var UrlS = _hostingEnvironment.WebRootPath.Replace("Draft/SaveDraft", "");
            //                    draft.DraftUrl = UrlS + "Uploads/Drafts/" + fileName;
            //                    result = _draftService.SaveDraft(draft, _globalshared.UserId_G, _globalshared.BranchId_G);
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                var massege = "";
            //                if (_globalshared.Lang_G == "rtl")
            //                {
            //                    massege = "فشل في رفع المسودة";
            //                }
            //                else if (_globalshared.Lang_G == "ltr")
            //                {
            //                    massege = "Uploaded Failed";
            //                }
            //                return Ok(new GeneralMessage {  StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = massege });
            //            }
            //        }
            //    }
            //    else
            //        result = new GeneralMessage() {  StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يوجد ملف أو الملف غير صالح" };

            //    return Ok(result);
            }


        [HttpPost("ConnectProjectTypeId")]
        public IActionResult ConnectProjectTypeId(Draft draft)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _draftService.SaveDraft(draft, _globalshared.UserId_G, _globalshared.BranchId_G);
                return Ok(result);
            }
        [HttpPost("appendFile_Draft")]
        public  GeneralMessage appendFile_Draft(int ContractId, string uploadedFile)/*IFormFile file*//*ContractId*/
            {
                Spire.Doc.Document doc = new Spire.Doc.Document();
                Spire.Doc.Document doc_Uploaded = new Spire.Doc.Document();

                List<string> fileUrls = new List<string>();
                //foreach (var item in contracts)
                //{
                var pathh = "";
                string path = System.IO.Path.Combine("Reports/Contract/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fileName = "DraftTemplate.docx";
                pathh = path + fileName;

                // var pathh = "";
                int? custid = 0;
                var massage = "";
                string fileLocation = "";
                string fileDir = "";
                string fileNamed;
                string filDown;
                String URII;
                try
                {
                    #region Initilizations

                    EmployeesVM Emp = null;
                    AccServicesPricesVM Service = null;
                    string serviceDetails = "";

                var CustContract =  _contractservice.GetAllContracts().Result.ToList().Where(w => w.ContractId == ContractId).FirstOrDefault();
                    custid = CustContract.CustomerId;
                    var customerData = _customerservice.GetCustomersByCustomerId(custid, _globalshared.Lang_G).Result;
                    var branchObj = _branchservice.GetBranchById(CustContract.BranchId??0).Result;
                    int branchdata = branchObj.OrganizationId;
                    var OrgData = _organizationservice.GetBranchOrganizationData(branchdata).Result;
                    string BranchId= Convert.ToString(branchObj.BranchId);
                    string CustomerId = Convert.ToString(CustContract.CustomerId);
                    string ProjectId = Convert.ToString(CustContract.ProjectId);
                    string contanme = "Contract_ProjDraft" + CustContract.ProjectNo;
                    string IsContains = CustContract.TaxType == 3 ? "شامل" : "غير شامل ";

                    if (CustContract.OrgEmpId != null)
                    {
                        Emp = _employeeService.GetEmployeeById(CustContract.OrgEmpId.Value, _globalshared.Lang_G).Result;
                    }

                    if (CustContract.ServiceId != null)
                    {
                        Service = _servicesPriceService.GetAllServicesPrice().Result.Where(x => x.ServicesId == CustContract.ServiceId).FirstOrDefault();

                        var ServiceDetObjs = _servicesPriceService.GetServicesPriceByParentId(Service.ServicesId).Result;
                        foreach (var item in ServiceDetObjs)
                        {
                            serviceDetails = serviceDetails + (item.ServicesName + " - " + item.AccountName) + "\r\n";
                            ;
                        }
                    }
                    #endregion
                    fileLocation = System.IO.Path.Combine("Uploads/Drafts/") + contanme + ".docx";

                    fileDir = System.IO.Path.Combine("Uploads/Drafts/");
                    filDown = "Uploads/Drafts/";

                    #region conditions


                    if (System.IO.Directory.Exists(fileDir))
                    {
                        if (System.IO.File.Exists(fileLocation))
                        {
                            System.IO.File.Delete(fileLocation);
                        }
                    }
                    else
                    {
                        System.IO.Directory.CreateDirectory(fileDir);
                        if (System.IO.File.Exists(fileLocation))
                        {
                            System.IO.File.Delete(fileLocation);
                        }
                    }
                    #endregion


                    fileNamed = contanme + ".docx";
                    string sourcePath = path;
                    string targetPath = fileDir;

                    // Use Path class to manipulate file and directory paths.
                    string sourceFile = System.IO.Path.Combine(sourcePath, fileName);

                    var ContractDetails = _contractDetailsService.GetAllEmpConDetailsByContractId(ContractId).Result;
                    string ContractDetailsStr = "";
                    if (ContractDetails.Count() > 0)
                    {
                        string[] strings = { "الأول", "الثاني", "الثالث", "الرابع", "الخامس", "السادس", "السابع", "الثامن", "التاسع", "العاشر" };
                        ContractDetailsStr = "بنود العقد: " + "\r\n";
                        int x = 0;
                        foreach (var item in ContractDetails)
                        {
                            ContractDetailsStr = ContractDetailsStr + string.Format("البند {0}: {1}", strings[x], item.Clause) + "\r\n";
                            x++;
                        }
                    }

                    WebClient webClient = new WebClient();
                    MemoryStream ms_Uploaded = new MemoryStream(webClient.DownloadData(uploadedFile));
                    doc_Uploaded.LoadFromStream(ms_Uploaded, FileFormat.Docx);


                    using (MemoryStream ms = new MemoryStream(webClient.DownloadData(sourceFile)))
                    {

                        doc.LoadFromStream(ms, FileFormat.Docx);
                        string dayname = GetDayName((DateTime.ParseExact(CustContract.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        doc.Replace("#ContNo", CustContract.ContractNo, false, true);
                        doc.Replace("#Hijri", CustContract.HijriDate, false, true);
                        doc.Replace("#Miladi", CustContract.Date, false, true);
                        doc.Replace("#Day", dayname, false, true);
                        doc.Replace("#CustName", customerData.CustomerName, false, true);
                        doc.Replace("#CompName", OrgData.NameAr, false, true);
                        string CustomerNationalData = "";
                        if (customerData.CustomerTypeId == 1)
                        {
                            CustomerNationalData = string.Format("بطاقة الهوية {0}", customerData.CustomerNationalId);
                        }
                        else if (customerData.CustomerTypeId == 2)
                        {
                            CustomerNationalData = string.Format("سجل تجاري: {0}", customerData.CommercialRegister);
                        }

                        doc.Replace("#CustNationalData", CustomerNationalData, false, true);
                        doc.Replace("#TotalTxt", CustContract.ValueText, false, true);
                        //New
                        doc.Replace("#TaxValue", CustContract.TaxesValue.HasValue ? CustContract.TaxesValue.Value.ToString() : "", false, true);
                        if (Service != null)
                        {
                            doc.Replace("#ServiceName", Service.Name, false, true);
                            doc.Replace("#ServiceDetails", serviceDetails, false, true);
                        }
                        else
                        {
                            doc.Replace("#ServiceName", "", false, true);
                            doc.Replace("#ServiceDetails", "", false, true);
                        }
                        doc.Replace("#ContractValue", CustContract.TotalValue.ToString(), false, true);
                        doc.Replace("#IsContain", IsContains, false, true);
                        if (Emp != null)
                        {
                            doc.Replace("#OrgEmpName", Emp.EmployeeNameAr, false, true);
                            doc.Replace("#OrgEmpJob", Emp.JobName, false, true);
                            doc.Replace("#EmpNationalId", Emp.NationalId, false, true);
                        }
                        else
                        {
                            doc.Replace("#OrgEmpName", "", false, true);
                            doc.Replace("#OrgEmpJob", "", false, true);
                            doc.Replace("#EmpNationalId", "", false, true);
                        }

                        doc.Replace("#ContractDetails", ContractDetailsStr, false, true);

                        doc.Replace("#ProjectNo", CustContract.ProjectNo, false, true);

                        /////////////////////
                        Section section2 = doc.Sections[0];

                        TextSelection selection = doc.FindString("#Dof3at", true, true);

                        TextRange range = selection.GetAsOneRange();

                        Spire.Doc.Documents.Paragraph paragraph = range.OwnerParagraph;

                        Spire.Doc.Body body = paragraph.OwnerTextBody;

                        int index = body.ChildObjects.IndexOf(paragraph);

                        var payment = _customerPaymentservice.GetAllCustomerPayments(Convert.ToInt32(ContractId)).Result;

                        if (payment.Count() > 0)
                        {
                            Spire.Doc.Table table2 = section2.AddTable(true);
                            var countp = payment.Count();
                            table2.ResetCells(1, 2);
                            // table.AddRow(false, 2);
                            //table2.Rows[0].Cells[0].Width = table2.Rows[0].Cells[1].Width = 60F;

                            table2.TableFormat.Bidi = true;
                            table2[0, 0].AddParagraph().AppendText("المبلغ");
                            string text = "تاريخ الدفعة";
                            table2[0, 1].AddParagraph().AppendText(text);

                            int x = 1;
                            foreach (var p in payment)
                            {

                                table2.AddRow(true, 2);
                                table2[x, 0].AddParagraph().AppendText(p.TotalAmount.ToString());
                                table2[x, 1].AddParagraph().AppendText(p.PaymentDate.ToString());
                                x++;
                            }

                            body.ChildObjects.Remove(paragraph);

                            body.ChildObjects.Insert(index, table2);

                        }
                        else
                        {
                            body.ChildObjects.Remove(paragraph);

                            selection = doc.FindString("جدول الدفعات:", true, true);

                            range = selection.GetAsOneRange();

                            paragraph = range.OwnerParagraph;

                            body.ChildObjects.Remove(paragraph);
                        }
                    }
                    int i = 0;
                    foreach (Section sec in doc.Sections)
                    {
                        foreach (DocumentObject obj in sec.Body.ChildObjects)
                        {
                            doc_Uploaded.Sections[0].Body.ChildObjects.Insert(i, obj.Clone());
                            i++;
                        }
                    }

                    //Save Word
                    #region Save tables

                    Section section = doc_Uploaded.AddSection();
                    Spire.Doc.Table table = section.AddTable(true);

                    table.ResetCells(3, 2);

                    table.TableFormat.Borders.Color = System.Drawing.Color.Transparent;

                    table[0, 0].AddParagraph().AppendText(string.Format("الطرف الأول"));
                    table[0, 0].Paragraphs[0].Format.IsBidi = true;
                    table.Rows[0].Cells[0].CellFormat.Borders.Color = System.Drawing.Color.Transparent;

                    table[0, 1].AddParagraph().AppendText(string.Format("الطرف الثاني"));
                    table[0, 1].Paragraphs[0].Format.IsBidi = true;
                    table.Rows[0].Cells[1].CellFormat.Borders.Color = System.Drawing.Color.Transparent;
                    //table.AddRow(true, 2);

                    table[1, 0].AddParagraph().AppendText(string.Format("الاسم: {0}", OrgData.NameAr));
                    table[1, 0].Paragraphs[0].Format.IsBidi = true;
                    table.Rows[1].Cells[0].CellFormat.Borders.Color = System.Drawing.Color.Transparent;

                    table[1, 1].AddParagraph().AppendText(string.Format("الاسم: {0}", customerData.CustomerName));
                    table[1, 1].Paragraphs[0].Format.IsBidi = true;
                    table.Rows[1].Cells[1].CellFormat.Borders.Color = System.Drawing.Color.Transparent;
                    //table.AddRow(true, 2);

                    table[2, 0].AddParagraph().AppendText(string.Format("التوقيع: .................................."));
                    table[2, 0].Paragraphs[0].Format.IsBidi = true;
                    table.Rows[2].Cells[0].CellFormat.Borders.Color = System.Drawing.Color.Transparent;

                    table[2, 1].AddParagraph().AppendText(string.Format("التوقيع: .................................."));
                    table[2, 1].Paragraphs[0].Format.IsBidi = true;
                    table.Rows[2].Cells[1].CellFormat.Borders.Color = System.Drawing.Color.Transparent;
                    //table.AddRow(true, 2);

                    table.TableFormat.Bidi = true;

                    #endregion

                    doc_Uploaded.SaveToFile(uploadedFile, FileFormat.Docx);
                    fileUrls.Add(uploadedFile);
                   // TempData["Linkurl"] = uploadedFile;

                    ////////////////////////////////////////////

                    RemoveRedLine(uploadedFile);
                    ////////////////////////////////////////////

                    var UrlS = _hostingEnvironment.WebRootPath;

                    StringBuilder sb = new StringBuilder(UrlS);

                    sb.Replace("Contract", filDown);
                    sb.Replace("UploadFile", fileNamed);
                    URII = sb.ToString();
                    //TempData["Linkurl"] = URII;
                    string[] fileNamedSplit = uploadedFile.Split('\\');
                    string uploadedFileName = fileNamedSplit[(fileNamedSplit.Count() - 1)];
                    URII = URII.Replace("Draft/SaveDraft", filDown) + uploadedFileName;
                    //Save as a draft
                    int projectType = _projectService.GetTypeOfProjct(CustContract.ProjectId.Value);
                    Draft draft = new Draft() { DraftUrl = URII, Name = uploadedFileName, ProjectTypeId = projectType };
                    _draftService.SaveDraft(draft, _globalshared.UserId_G, this._globalshared.BranchId_G);
                }

                catch (Exception ex)
                {

                    if (_globalshared.Lang_G == "rtl")
                    {
                        massage = "فشل في تحميل العقد";
                    }
                    else
                    {
                        massage = "Failed To Download Contract Files";
                    }
                    return new GeneralMessage {  StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = massage };
                }


                return new GeneralMessage {  StatusCode = HttpStatusCode.OK, ReasonPhrase = "" };
            }
        [HttpPost("Connect_appendFile_Draft")]
        public IActionResult Connect_appendFile_Draft(int ProjectId, string uploadedFile)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var project = _projectService.GetProjectById("rtl", ProjectId).Result;
                int ContractId = project.ContractId.Value;


                string[] fileNamedSplit = uploadedFile.Split('/');
                string uploadedFileName = fileNamedSplit[(fileNamedSplit.Count() - 1)];
            string result_uploadedFile = System.IO.Path.Combine(uploadedFile.Remove(0, 1));
            //if (System.IO.File.Exists(result_uploadedFile))
            //{
            //    return Ok(new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully, ReturnedStr = uploadedFile });
            //}

            //if (uploadedFileName.Contains("Contract_" + project.ProjectNo))
            //    {
            //        return Ok(new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully, ReturnedStr = uploadedFile });
            //    }
            var EmptyFile = "Reports/Contract/EmptyFile.docx";
            string fileName = "DraftTemplate.docx";

            uploadedFile = System.IO.Path.Combine("Uploads/Drafts/") + uploadedFileName;

            var drft = _drafts_TemplatesService.GetDraft_templateByProjectId(project.ProjectTypeId ?? 0).Result;
            if (drft != null)
            {
                string result = drft.DraftUrl.Remove(0, 1);
                uploadedFile = result;
                fileName = "DraftTemplate.docx";
            }
            else
            {
                uploadedFile = EmptyFile;
                fileName = "Contract1_Draft.docx";
            }


            Spire.Doc.Document doc = new Spire.Doc.Document();
                Spire.Doc.Document doc_Uploaded = new Spire.Doc.Document();

                List<string> fileUrls = new List<string>();
                //foreach (var item in contracts)
                //{
                var pathh = "";
                string path = System.IO.Path.Combine("Reports/Contract/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                pathh = path + fileName;

                // var pathh = "";
                int? custid = 0;
                var massage = "";
                string fileLocation = "";
                string fileDir = "";
                string fileNamed;
                string filDown;
                String URII;
                try
                {
                    #region Initilizations

                    EmployeesVM Emp = null;
                    AccServicesPricesVM Service = null;
                    string serviceDetails = "";

                    var CustContract = _contractservice.GetAllContracts().Result.Where(w => w.ContractId == ContractId).FirstOrDefault();
                    custid = CustContract.CustomerId;
                    var customerData = _customerservice.GetCustomersByCustomerId(custid, _globalshared.Lang_G).Result;
                    var branchObj = _branchservice.GetBranchById(CustContract.BranchId??0).Result;
                    int branchdata = branchObj.OrganizationId;
                    var OrgData = _organizationservice.GetBranchOrganizationData(branchdata).Result;
                    string BranchId = Convert.ToString(branchObj.BranchId);
                    string CustomerId = Convert.ToString(CustContract.CustomerId);
                    //string ProjectId = Convert.ToString(CustContract.ProjectId);
                    string contanme = "Contract_ProjDraft" + CustContract.ProjectNo;
                    string IsContains = CustContract.TaxType == 3 ? "شامل" : "غير شامل ";

                    if (CustContract.OrgEmpId != null)
                    {
                        Emp = _employeeService.GetEmployeeById(CustContract.OrgEmpId.Value, _globalshared.Lang_G).Result;
                    }

                    if (CustContract.ServiceId != null)
                    {
                        //Service = _servicesPriceService.GetAllServicesPrice().Result.Where(x => x.ServicesId == CustContract.ServiceId).FirstOrDefault();

                        //var ServiceDetObjs = _servicesPriceService.GetServicesPriceByParentId(Service.ServicesId).Result;
                    var ServiceDetObjs = _servicesPriceOfferService.GetServicesPriceByParentIdAndContractId(CustContract.ServiceId, ContractId).Result;
                    ServiceDetObjs = ServiceDetObjs.OrderBy(q => q.LineNumber).ToList();
                    foreach (var item in ServiceDetObjs)
                        {
                            serviceDetails = serviceDetails + (item.ServicesName) + "\r\n";
                            ;
                        }
                    }
                    #endregion
                    fileLocation = System.IO.Path.Combine("Uploads/Drafts/") + contanme + ".docx";

                    fileDir = System.IO.Path.Combine("Uploads/Drafts/");
                    filDown = "Uploads/Drafts/";

                    #region conditions


                    if (System.IO.Directory.Exists(fileDir))
                    {
                        if (System.IO.File.Exists(fileLocation))
                        {
                            System.IO.File.Delete(fileLocation);
                        }
                    }
                    else
                    {
                        System.IO.Directory.CreateDirectory(fileDir);
                        if (System.IO.File.Exists(fileLocation))
                        {
                            System.IO.File.Delete(fileLocation);
                        }
                    }
                    #endregion


                    fileNamed = contanme + ".docx";
                    string sourcePath = path;
                    string targetPath = fileDir;

                    // Use Path class to manipulate file and directory paths.
                    string sourceFile = System.IO.Path.Combine(sourcePath, fileName);

                    var ContractDetails = _contractDetailsService.GetAllEmpConDetailsByContractId(ContractId).Result;
                    string ContractDetailsStr = "";
                    if (ContractDetails.Count() > 0)
                    {
                        string[] strings = { "الأول", "الثاني", "الثالث", "الرابع", "الخامس", "السادس", "السابع", "الثامن", "التاسع", "العاشر" };
                        ContractDetailsStr = "بنود العقد: " + "\r\n";
                        int x = 0;
                        foreach (var item in ContractDetails)
                        {
                            ContractDetailsStr = ContractDetailsStr + string.Format("البند {0}: {1}", strings[x], item.Clause) + "\r\n";
                            x++;
                        }
                    }

                    WebClient webClient = new WebClient();
                
                //uploadedFile
                MemoryStream ms_Uploaded = new MemoryStream(webClient.DownloadData(uploadedFile));
                doc_Uploaded.LoadFromStream(ms_Uploaded, FileFormat.Auto);


                using (MemoryStream ms = new MemoryStream(webClient.DownloadData(sourceFile)))
                    {

                        doc.LoadFromStream(ms, FileFormat.Docx);

                        string dayname = GetDayName((DateTime.ParseExact(CustContract.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        doc.Replace("#ContNo", CustContract.ContractNo??"", false, true);
                        doc.Replace("#Hijri", CustContract.HijriDate??"", false, true);
                        doc.Replace("#Miladi", CustContract.Date??"", false, true);
                        doc.Replace("#Day", dayname, false, true);
                        doc.Replace("#CustName", customerData.CustomerName??"", false, true);
                        doc.Replace("#CompName", OrgData.NameAr??"", false, true);
                        string CustomerNationalData = "";
                        if (customerData.CustomerTypeId == 1)
                        {
                            CustomerNationalData = string.Format("بطاقة الهوية {0}", (customerData.CustomerNationalId??""));
                        }
                        else if (customerData.CustomerTypeId == 2)
                        {
                            CustomerNationalData = string.Format("سجل تجاري: {0}", (customerData.CommercialRegister ?? ""));
                        }

                        doc.Replace("#CustNationalData", CustomerNationalData, false, true);
                        doc.Replace("#TotalTxt", CustContract.ValueText??"", false, true);
                        //New
                        doc.Replace("#TaxValue", CustContract.TaxesValue.HasValue ? CustContract.TaxesValue.Value.ToString() : "", false, true);
                        if (Service != null)
                        {
                            doc.Replace("#ServiceName", Service.Name, false, true);
                            doc.Replace("#ServiceDetails", serviceDetails, false, true);
                        }
                        else
                        {
                            doc.Replace("#ServiceName", "", false, true);
                            doc.Replace("#ServiceDetails", "", false, true);
                        }
                        doc.Replace("#ContractValue", CustContract.TotalValue.ToString(), false, true);
                        doc.Replace("#IsContain", IsContains, false, true);
                        if (Emp != null)
                        {
                            doc.Replace("#OrgEmpName", Emp.EmployeeNameAr, false, true);
                            doc.Replace("#OrgEmpJob", Emp.JobName, false, true);
                            doc.Replace("#EmpNationalId", Emp.NationalId, false, true);
                        }
                        else
                        {
                            doc.Replace("#OrgEmpName", "", false, true);
                            doc.Replace("#OrgEmpJob", "", false, true);
                            doc.Replace("#EmpNationalId", "", false, true);
                        }

                        doc.Replace("#ContractDetails", ContractDetailsStr, false, true);

                        doc.Replace("#ProjectNo", CustContract.ProjectNo, false, true);




                    Section section3 = doc.Sections[0];

                    TextSelection selection1 = doc.FindString("#khadmat", true, true);

                    TextRange range1 = selection1.GetAsOneRange();
                    Spire.Doc.Documents.Paragraph paragraph1 = range1.OwnerParagraph;
                    paragraph1.Format.HorizontalAlignment=HorizontalAlignment.Right;
                    //section3.TextDirection= TextDirection.RightToLeft;
                    Spire.Doc.Body body1 = paragraph1.OwnerTextBody;

                    int index1 = body1.ChildObjects.IndexOf(paragraph1);

                    var conservices = _contractService.GetContractservicenByid(ContractId).Result;
                    if (conservices.Count() > 0)
                    {

                        Spire.Doc.Table table2 = section3.AddTable(true);
                        var countS = conservices.Count();
                        table2.ResetCells(1, 5);

                        // table.AddRow(false, 2);
                        //table2.Rows[0].Cells[0].Width = table2.Rows[0].Cells[1].Width = 60F;


                        //Spire.Doc.TableCell cell = table2.Rows[0].Cells[0];

                        //cell.CellFormat.TextDirection = TextDirection.RightToLeft;

                        table2.TableFormat.Bidi = true;
                        Paragraph paragraphTemp;
                        paragraphTemp = table2[0, 0].AddParagraph();
                        paragraphTemp.Format.HorizontalAlignment = HorizontalAlignment.Right;
                        paragraphTemp.AppendText("رقم المشروع");
                        //table2[0, 0].AddParagraph().Format.IsBidi = true;
                        string text = "الخدمة";
                        paragraphTemp = table2[0, 1].AddParagraph();
                        paragraphTemp.Format.HorizontalAlignment = HorizontalAlignment.Right;

                        paragraphTemp.AppendText(text);
                        paragraphTemp = table2[0, 2].AddParagraph();
                        paragraphTemp.Format.HorizontalAlignment = HorizontalAlignment.Right;

                        paragraphTemp.AppendText(" الكمية");
                        paragraphTemp = table2[0, 3].AddParagraph();
                        paragraphTemp.Format.HorizontalAlignment = HorizontalAlignment.Right;

                        paragraphTemp.AppendText("مبلغ العقد");
                        paragraphTemp = table2[0, 4].AddParagraph();
                        paragraphTemp.Format.HorizontalAlignment = HorizontalAlignment.Right;

                        paragraphTemp.AppendText("مبلغ ضريبة");
                        int ii = 1;
                        foreach (var S in conservices)
                        {

                            string serviceDetails1 = "";
                            //var ServiceDetObjs12 = _servicesPriceService.GetServicesPriceByParentId(S.ServiceId).Result;
                            var ServiceDetObjs1 = _servicesPriceOfferService.GetServicesPriceByParentIdAndContractId(S.ServiceId, ContractId).Result;
                            ServiceDetObjs1 = ServiceDetObjs1.OrderBy(q => q.LineNumber).ToList();

                            var TaxAmount = "";
                            if (CustContract.TaxType == 3)
                            {
                                TaxAmount = (Convert.ToDecimal(S.Serviceamountval - (S.Serviceamountval / Convert.ToDecimal(1.15)))).ToString("0.00");
                            }
                            else
                            {
                                TaxAmount = (Convert.ToDecimal((S.Serviceamountval * Convert.ToDecimal(0.15)))).ToString("0.00");

                            }
                            foreach (var item in ServiceDetObjs1)
                            {
                                serviceDetails1 = serviceDetails1 + (item.ServicesName) + "\r\n";
                            }


                            table2.AddRow(true, 5);
                            paragraphTemp = table2[ii, 0].AddParagraph();
                            paragraphTemp.Format.HorizontalAlignment = HorizontalAlignment.Right;
                            paragraphTemp.AppendText((CustContract.ProjectNo ?? "").ToString());

                            paragraphTemp = table2[ii, 1].AddParagraph();
                            paragraphTemp.Format.HorizontalAlignment = HorizontalAlignment.Right;
                            paragraphTemp.AppendText((S.servicename ?? "").ToString());
                            if(serviceDetails1!="")
                            {
                                table2[ii, 1].AddParagraph().AppendText(("━━━━━━━━━━━━").ToString());
                            }
                            paragraphTemp = table2[ii, 1].AddParagraph();
                            paragraphTemp.Format.HorizontalAlignment = HorizontalAlignment.Right;
                            paragraphTemp.AppendText((serviceDetails1 ?? "").ToString());

                            paragraphTemp = table2[ii, 2].AddParagraph();
                            paragraphTemp.Format.HorizontalAlignment = HorizontalAlignment.Right;
                            paragraphTemp.AppendText((S.ServiceQty ?? 1).ToString());

                            paragraphTemp = table2[ii, 3].AddParagraph();
                            paragraphTemp.Format.HorizontalAlignment = HorizontalAlignment.Right;
                            paragraphTemp.AppendText((S.Serviceamountval ?? 0).ToString());
                            paragraphTemp = table2[ii, 4].AddParagraph();
                            paragraphTemp.Format.HorizontalAlignment = HorizontalAlignment.Right;
                            paragraphTemp.AppendText((TaxAmount ?? "").ToString());
                            ii++;
                        }

                        body1.ChildObjects.Remove(paragraph1);

                        body1.ChildObjects.Insert(index1, table2);
                    }


                    /////////////////////
                    Section section2 = doc.Sections[0];

                        TextSelection selection = doc.FindString("#Dof3at", true, true);

                        TextRange range = selection.GetAsOneRange();

                        Spire.Doc.Documents.Paragraph paragraph = range.OwnerParagraph;

                        Spire.Doc.Body body = paragraph.OwnerTextBody;

                        int index = body.ChildObjects.IndexOf(paragraph);

                    var payment = _customerPaymentservice.GetAllCustomerPayments(Convert.ToInt32(ContractId)).Result;

                    //var payment = _customerPaymentservice.GetAllCustomerPaymentsIsNotCanceled(Convert.ToInt32(ContractId)).Result;

                        if (payment.Count() > 0)
                        {
                            Spire.Doc.Table table2 = section2.AddTable(true);
                            var countp = payment.Count();
                            table2.ResetCells(1, 2);
                            // table.AddRow(false, 2);
                            //table2.Rows[0].Cells[0].Width = table2.Rows[0].Cells[1].Width = 60F;

                            table2.TableFormat.Bidi = true;
                            Paragraph paragraphTemp;

                            paragraphTemp = table2[0, 0].AddParagraph();
                            paragraphTemp.Format.HorizontalAlignment = HorizontalAlignment.Right;
                        paragraphTemp.AppendText("المبلغ");
                            string text = "تاريخ الدفعة";
                        paragraphTemp = table2[0, 1].AddParagraph();
                        paragraphTemp.Format.HorizontalAlignment = HorizontalAlignment.Right;
                        paragraphTemp.AppendText(text);

                            int x = 1;
                            foreach (var p in payment)
                            {

                                table2.AddRow(true, 2);
                            paragraphTemp = table2[x, 0].AddParagraph();
                            paragraphTemp.Format.HorizontalAlignment = HorizontalAlignment.Right;
                            paragraphTemp.AppendText(p.TotalAmount.ToString());
                            paragraphTemp = table2[x, 1].AddParagraph();
                            paragraphTemp.Format.HorizontalAlignment = HorizontalAlignment.Right;
                            paragraphTemp.AppendText(p.PaymentDate.ToString());
                                x++;
                            }

                            body.ChildObjects.Remove(paragraph);

                            body.ChildObjects.Insert(index, table2);

                        }
                        else
                        {
                            body.ChildObjects.Remove(paragraph);

                            selection = doc.FindString("جدول الدفعات:", true, true);

                            range = selection.GetAsOneRange();

                            paragraph = range.OwnerParagraph;

                            body.ChildObjects.Remove(paragraph);
                        }
                    }
                    int i = 0;
                    foreach (Section sec in doc.Sections)
                    {
                        foreach (DocumentObject obj in sec.Body.ChildObjects)
                        {
                            doc_Uploaded.Sections[0].Body.ChildObjects.Insert(i, obj.Clone());
                            i++;
                        }
                    }

                    //Save Word
                    #region Save tables

                    Section section = doc_Uploaded.AddSection();
                    Spire.Doc.Table table = section.AddTable(true);

                    table.ResetCells(3, 2);

                    table.TableFormat.Borders.Color = System.Drawing.Color.Transparent;

                    table[0, 0].AddParagraph().AppendText(string.Format("الطرف الأول"));
                    table[0, 0].Paragraphs[0].Format.IsBidi = true;
                    table.Rows[0].Cells[0].CellFormat.Borders.Color = System.Drawing.Color.Transparent;

                    table[0, 1].AddParagraph().AppendText(string.Format("الطرف الثاني"));
                    table[0, 1].Paragraphs[0].Format.IsBidi = true;
                    table.Rows[0].Cells[1].CellFormat.Borders.Color = System.Drawing.Color.Transparent;
                    //table.AddRow(true, 2);

                    table[1, 0].AddParagraph().AppendText(string.Format("الاسم: {0}", OrgData.NameAr));
                    table[1, 0].Paragraphs[0].Format.IsBidi = true;
                    table.Rows[1].Cells[0].CellFormat.Borders.Color = System.Drawing.Color.Transparent;

                    table[1, 1].AddParagraph().AppendText(string.Format("الاسم: {0}", customerData.CustomerName));
                    table[1, 1].Paragraphs[0].Format.IsBidi = true;
                    table.Rows[1].Cells[1].CellFormat.Borders.Color = System.Drawing.Color.Transparent;
                    //table.AddRow(true, 2);

                    table[2, 0].AddParagraph().AppendText(string.Format("التوقيع: .................................."));
                    table[2, 0].Paragraphs[0].Format.IsBidi = true;
                    table.Rows[2].Cells[0].CellFormat.Borders.Color = System.Drawing.Color.Transparent;

                    table[2, 1].AddParagraph().AppendText(string.Format("التوقيع: .................................."));
                    table[2, 1].Paragraphs[0].Format.IsBidi = true;
                    table.Rows[2].Cells[1].CellFormat.Borders.Color = System.Drawing.Color.Transparent;
                    //table.AddRow(true, 2);

                    table.TableFormat.Bidi = true;

                    #endregion

                    #region Haeader And Footer

                    //Add Header
                    Section Headersection = doc_Uploaded.Sections[0];

                    Spire.Doc.HeaderFooter header = Headersection.HeadersFooters.Header;
                    Spire.Doc.Table Htable = header.AddTable(true);

                    Htable.ResetCells(1, 3);

                    Htable.TableFormat.Borders.Bottom.Color = System.Drawing.Color.Gray;


                    Htable.Rows[0].Cells[0].AddParagraph();
                    string resultLogo = OrgData.LogoUrl.Remove(0, 1);
                    if((branchObj.headerPrintcontract??false)==true)
                    {
                        resultLogo = branchObj.BranchLogoUrl.Remove(0, 1);
                    }
                    DocPicture picture = Htable.Rows[0].Cells[0].Paragraphs[0].AppendPicture(Image.FromFile(System.IO.Path.Combine(resultLogo)));
                    picture.Width = 90;
                    picture.Height = 80;

                    Htable[0, 0].Paragraphs[0].Format.HorizontalAlignment = HorizontalAlignment.Center;
                    Htable.Rows[0].Cells[0].CellFormat.Borders.Color = System.Drawing.Color.Transparent;
                    Htable.Rows[0].Cells[0].CellFormat.Borders.Bottom.Color = System.Drawing.Color.Gray;


                TextRange FText = Htable[0, 1].AddParagraph().AppendText(string.Format(OrgData!.NameAr??""));

                FText.CharacterFormat.FontName = "Calibri";
                FText.CharacterFormat.FontSize = 13;
                FText.CharacterFormat.TextColor = System.Drawing.Color.Black;

                Htable[0, 1].Paragraphs[0].Format.IsBidi = true;
                Htable.Rows[0].Cells[1].CellFormat.Borders.Color = System.Drawing.Color.Transparent;
                Htable.Rows[0].Cells[1].CellFormat.Borders.Bottom.Color = System.Drawing.Color.Gray;

                //Htable.AddRow(true, 2);

                //FText = Htable[0, 2].AddParagraph().AppendText(string.Format("التاريخ {0}:", CustContract.Date));
                FText = Htable[0, 2].AddParagraph().AppendText("");

                FText.CharacterFormat.FontName = "Calibri";
                    FText.CharacterFormat.FontSize = 13;
                    FText.CharacterFormat.TextColor = System.Drawing.Color.Black;

                    Htable[0, 2].Paragraphs[0].Format.HorizontalAlignment = HorizontalAlignment.Center;
                    Htable[0, 2].Paragraphs[0].Format.IsBidi = true;

                    Htable.Rows[0].Cells[2].CellFormat.Borders.Color = System.Drawing.Color.Transparent;
                    Htable.Rows[0].Cells[2].CellFormat.Borders.Bottom.Color = System.Drawing.Color.Gray;

                    Htable.TableFormat.Bidi = true;


                //Add Footer
                    Spire.Doc.HeaderFooter footer = Headersection.HeadersFooters.Footer;
                    Paragraph FParagraph = footer.AddParagraph();
                    string footerStr = string.Format("{0} - {1} - تليفون ,{2}: فاكس ,{3}: بريد الكترونى {4}: ", OrgData.NameAr, OrgData.Address, OrgData.Mobile, OrgData.Fax, OrgData.Email);
                    FText = FParagraph.AppendText(footerStr);

                    //Set Footer Text Format
                    FText.CharacterFormat.FontName = "Calibri";
                    FText.CharacterFormat.FontSize = 8;
                    FText.CharacterFormat.TextColor = System.Drawing.Color.Black;

                    //Set Footer Paragrah Format
                    FParagraph.Format.HorizontalAlignment = HorizontalAlignment.Left;
                    FParagraph.Format.IsBidi = true;
                    FParagraph.Format.Borders.Top.BorderType = BorderStyle.ThinThinSmallGap;
                    FParagraph.Format.Borders.Top.Space = 0.15f;
                    FParagraph.Format.Borders.Color = System.Drawing.Color.DarkGray;
                    #endregion

                    uploadedFile = fileLocation = System.IO.Path.Combine("Uploads/Drafts/") + string.Format("{0}{1}", CustContract.ProjectNo, uploadedFileName);
                    doc_Uploaded.SaveToFile(uploadedFile, FileFormat.Docx);
                    fileUrls.Add(uploadedFile);
                    //TempData["Linkurl"] = uploadedFile;

                    ////////////////////////////////////////////

                    RemoveRedLine(uploadedFile);
                    ////////////////////////////////////////////

                    var UrlS = _hostingEnvironment.WebRootPath;
                    //UrlS = UrlS.Replace("Draft/Connect_appendFile_Draft", "") + "/Uploads//Drafts//" + string.Format("{0}{1}", CustContract.ProjectNo, uploadedFileName);
                var pathfile = "/"+uploadedFile;
                return Ok(new GeneralMessage {  StatusCode = HttpStatusCode.OK, ReasonPhrase = "تم التحميل", ReturnedStr = pathfile });
                }

                catch (Exception ex)
                {

                    if (_globalshared.Lang_G == "rtl")
                    {
                        massage = "فشل في تحميل العقد";
                    }
                    else
                    {
                        massage = "Failed To Download Contract Files";
                    }
                    return Ok(new GeneralMessage {  StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = massage });
                }


                //return new GeneralMessage {  StatusCode = HttpStatusCode.OK, ReasonPhrase = "" };
            }


        [HttpGet("GetAllDrafts")]
        public IActionResult GetAllDrafts()
            {
                return Ok(_DrafService.GetAllDrafts());
            }
        [HttpGet("GetAllDraftsbyProjectsType")]
        public IActionResult GetAllDraftsbyProjectsType(int? ProjectTypeId)
            {
                return Ok(_DrafService.GetAllDraftsbyProjectsType(ProjectTypeId));
            }

        [HttpGet("GetAllDraftsbyProjectsType_2")]
        public IActionResult GetAllDraftsbyProjectsType_2(int? ProjectTypeId)
            {
                return Ok(_DrafService.GetAllDraftsbyProjectsType_2(ProjectTypeId));
            }
        [HttpPost("DeleteDraft")]
        public IActionResult DeleteDraft(int DraftId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _DrafService.DeleteDraft(DraftId, _globalshared.UserId_G, _globalshared.BranchId_G);
                return Ok(result);
            }
        [HttpGet("GetAllDraftsByDraftName_Union")]
        public IActionResult GetAllDraftsByDraftName_Union(string DraftName)
            {
                var result = _draftService.GetAllDraftsByDraftName_Union(DraftName);
                return Ok(result);
            }

        //DraftDetails
        [HttpPost("SaveDraftDetails")]
        public IActionResult SaveDraftDetails(DraftDetails draftDetails)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            try
            {
                    var result = _DraftDetailsService.SaveDraftDetails(draftDetails, _globalshared.UserId_G, _globalshared.BranchId_G);

                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return Ok(new { result = false, ReasonPhrases= "فشل في الحفظ" });
                }
            }

        [HttpGet("GetAllDraftDetailsByDraftId")]
        public IActionResult GetAllDraftDetailsByDraftId(int? DraftId)
            {
                return Ok(_DraftDetailsService.GetAllDraftDetailsByDraftId(DraftId));
            }

        [HttpGet("GetAllDraftsDetailsbyProjectId")]
        public IActionResult GetAllDraftsDetailsbyProjectId(int? ProjectId)
            {
                return Ok(_DraftDetailsService.GetAllDraftsDetailsbyProjectId(ProjectId));
            }

        [HttpGet("GetAllDraftDetailsByDraftId_Union")]
        public IActionResult GetAllDraftDetailsByDraftId_Union(int draftId)
            {
                return Ok(_DraftDetailsService.GetAllDraftDetailsByDraftId_Union(draftId));
            }

        [HttpPost("DeleteDraftDetails")]
        public IActionResult DeleteDraftDetails(int DraftDetailId)
            {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _DraftDetailsService.DeleteDraftDetails(DraftDetailId, _globalshared.UserId_G, _globalshared.BranchId_G);
                return Ok(result);
            }

        [HttpGet("FillAllDraft")]
        public IActionResult FillAllDraft(int param)
            {
                var someProject = _DrafService.GetAllDraftsbyProjectsType(param).Result.Select(s => new {
                    Id = s.DraftId,
                    Name = s.DraftName
                });
                return Ok(someProject);
            }
        [HttpGet("RemoveRedLine")]
        public bool RemoveRedLine(string filePath)
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
        [HttpPost("GetDayName")]
        private string GetDayName(DateTime dt)
            {
                string day = "";
                day = dt.DayOfWeek.ToString();
                StringBuilder sb = new StringBuilder(day);


                string dayAr =
                    sb
                      .Replace("Saturday", "السبت")
                      .Replace("Sunday", "الأحد")
                      .Replace("Monday", "الإثنين")
                       .Replace("Tuesday", "الثلاثاء")
                        .Replace("Wednesday", "الأربعاء")
                         .Replace("Thursday", "الخميس")
                          .Replace("Friday", "الجمعة")
                      .ToString();
                return dayAr;
            }
        [HttpPost("GenerateRandomNo")]
        public int GenerateRandomNo()
            {
                int _min = 1000;
                int _max = 9999;
                Random _rdm = new Random();
                return _rdm.Next(_min, _max);
            }
        }
    
}
