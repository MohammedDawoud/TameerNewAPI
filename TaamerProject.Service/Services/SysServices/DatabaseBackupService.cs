using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Ionic.Zip;
using Microsoft.IdentityModel.Protocols;
using System.Data;
using System.Data.SqlClient;
//using System.IO.Compression;
//using OfficeOpenXml.Packaging;
using System.Diagnostics;
using System.Globalization;
using System.IO.Compression;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Models.DBContext;
using TaamerProject.Models.DomainObjects;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;
using Twilio.TwiML.Messaging;
using static Google.Apis.Drive.v3.DriveService;


//using ICSharpCode.SharpZipLib.Zip;

namespace TaamerProject.Service.Services
{
    public class DatabaseBackupService :   IDatabaseBackupService
    {
        private readonly IDatabaseBackupRepository _DatabaseBackupRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IEmailSettingRepository _EmailSettingRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IBranchesRepository _BranchesRepository;
        private readonly INotificationRepository _NotificationRepository;
        private readonly IProjectRepository _projectRepository;
        
        
        public DatabaseBackupService(TaamerProjectContext dataContext
            , ISystemAction systemAction, IDatabaseBackupRepository DatabaseBackupRepository,
            IEmailSettingRepository EmailSettingRepository, IUsersRepository usersRepository, IBranchesRepository BranchesRepository
            , INotificationRepository NotificationRepository, IProjectRepository projectRepository )
        {
            _DatabaseBackupRepository = DatabaseBackupRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _EmailSettingRepository = EmailSettingRepository;
            _BranchesRepository = BranchesRepository;
            _usersRepository = usersRepository;
            _NotificationRepository = NotificationRepository;
            _projectRepository = projectRepository;

        }

       

        List<string> tableslist = new List<string>(new string[] { "Sys_UserPrivileges", "Sys_GroupPrivileges", "Sys_Branches", "Sys_UserBranches", "Emp_Employees", "Emp_Holidays_Public", "Emp_AttendaceTime" });

        public Task<IEnumerable<DatabaseBackupVM>> GetAllDBackup()
        {
            var AllDBackup = _DatabaseBackupRepository.GetAllDBackup();
            return AllDBackup;
        }

        public BackupStatistics GetBackupStatistics(string lang)
        {
            var AllDBackup = _DatabaseBackupRepository.GetBackupStatistics(lang);
            return AllDBackup;
        }

        public Task<IEnumerable<DatabaseBackupVM>> GetDBDBackupById(int backupid)
        {
            var AllDBackup = _DatabaseBackupRepository.GetDBackupByID(backupid);
            return AllDBackup;
        }

        public Task<BackupStatistics> GetDBackupByIDWithDetails(int backupid,string lang)
        {
            var AllDBackup = _DatabaseBackupRepository.GetDBackupByIDWithDetails(backupid, lang);
            return AllDBackup;
        }
        //public GeneralMessage SaveDBackup(DatabaseBackup info, int UserId, string path, int BranchId, string remote, string Con)
        //{
        //    SqlConnection con = new SqlConnection(Con);
        //    SqlCommand sqlcmd = new SqlCommand();

        //    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(Con);


        //    var local = path;
        //    //Create all the directories.
        //    string[] _bakfiles = Directory.GetFiles(path);
        //    foreach (string s in _bakfiles)
        //    {
        //        File.Delete(s);
        //    }
        //    var uploadfolder = remote + "Uploads";
        //    var filefolders = remote + "Files";
        //    var tempfiles = remote + "TempFiles";
        //    var npath = "_" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss", CultureInfo.CreateSpecificCulture("en"));
        //    var zipFile = path + info.SavedName + npath + @".zip";
        //    var uploadfiles = Directory.GetFiles(uploadfolder);
        //    var filefiles = Directory.GetFiles(filefolders);
        //    var tempfilesfiles = Directory.GetFiles(tempfiles);





        //    string dirRoot = uploadfolder;
        //    //var filefolders = remote + "Files";
        //    //var tempfiles = remote + "TempFiles";
        //    //get a list of files
        //    string[] filesToZip = Directory.GetFiles(dirRoot, "*.*", SearchOption.AllDirectories);

        //    //final archive name (I use date / time)
        //    string zipFileName = path + "Backup" + npath + @".zip";
        //    string zipFilebackName = path + "DBBackup" + npath + @".zip";
        //    string zipFileDbbackName = path + "DBBackup" + npath + @".zip";
        //    string zipuploadfolder = path + "upload" + @".zip";

        //    var nfilepath = path + "Files";
        //    Directory.CreateDirectory(path + "Files");
        //    CopyFilesRecursively(filefolders, nfilepath);
        //    var directories = Directory.GetDirectories(nfilepath + "/ProjectFiles");
        //    Directory.CreateDirectory(nfilepath + "/ArchiveProjects");
        //    var archivepath = nfilepath + "/ArchiveProjects";
        //    var message = "";
        //    try
        //    {

        //        if (directories.Length > 0)
        //        {
        //            foreach (var item in directories)
        //            {
        //                message=item.ToString();
        //                string directoryName = Path.GetFileName(item);
        //                message = directoryName;

        //                var project = _projectRepository.GetProjectByNUmber("", directoryName);

        //                if (project.Result != null)
        //                {
        //                    message = project.Result.CustomerName_W + directoryName;
        //                    if (project.Result.Status == 1)
        //                    {        //archive
        //                             //Directory.Move(item, archivepath + "\\" + directoryName + project.CustomerName_W);
        //                             //var CNwithoutspecialcharacters = Regex.Replace(project.CustomerName_W, @"[^0-9a-zA-Z]+", "");
        //                        var CNwithoutspecialcharacters = ExtensionMethod.RemoveSpecialChars(project.Result.CustomerName_W);
        //                        Directory.Move(item, archivepath + "\\" + directoryName + CNwithoutspecialcharacters);
        //                    }
        //                    else
        //                    {
        //                        var CNwithoutspecialcharacters = ExtensionMethod.RemoveSpecialChars(project.Result.CustomerName_W);
        //                        Directory.Move(item, item + "-" + CNwithoutspecialcharacters);
        //                    }



        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //-----------------------------------------------------------------------------------------------------------------
        //        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
        //        string ActionNote2 = "فشل في حفظ Backup Database";
        //         _SystemAction.SaveAction("SaveDBackup", "DatabaseBackupService", 1,Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
        //        //-----------------------------------------------------------------------------------------------------------------

        //        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReturnedStr = ex.Message.ToString() + message, ReasonPhrase = Resources.General_SavedFailed };
        //    }

        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message);

        //        //-----------------------------------------------------------------------------------------------------------------
        //        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
        //        string ActionNote2 = "فشل في حفظ Backup Database";
        //         _SystemAction.SaveAction("SaveDBackup", "DatabaseBackupService", 1,Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
        //        //-----------------------------------------------------------------------------------------------------------------

        //        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReturnedStr = ex.Message.ToString(),ReasonPhrase = Resources.General_SavedFailed };
        //    }

        //    try
        //    {

        //        if (info.BackupId == 0)
        //        {
        //            info.AddUser = UserId;
        //            info.AddDate = DateTime.Now;
        //            try
        //            {
        //                info.UserId = UserId;
        //            }
        //            catch (Exception ex)
        //            {
        //                //-----------------------------------------------------------------------------------------------------------------
        //                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
        //                string ActionNote2 = "فشل في حفظ Backup Database";
        //                 _SystemAction.SaveAction("SaveDBackup", "DatabaseBackupService", 1,Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
        //                //-----------------------------------------------------------------------------------------------------------------

        //                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReturnedStr = ex.Message.ToString(),ReasonPhrase = Resources.General_SavedFailed };
        //            }
        //            info.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));


        //            var fulpath = System.IO.Path.Combine(remote,path);
        //            info.LocalSavedPath = path;
        //            string BackUpName = "Backup" + "_" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss", CultureInfo.CreateSpecificCulture("en"));

        //            try
        //            {
        //                con.Open();
        //                string pathDB = "BACKUP DATABASE " +"["+ builder.InitialCatalog+"]" + " TO DISK='" + fulpath /*+ "\\"*/ + BackUpName + ".Bak'" + "WITH FORMAT, INIT, MEDIANAME = N'Backup',  NAME = N'TameerProDB-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";




        //                sqlcmd = new SqlCommand(pathDB, con);
        //                sqlcmd.ExecuteNonQuery();
        //                con.Close();
        //                var dbpat = path + BackUpName + @".Bak";



        //                using (ZipArchive zipArchive = System.IO.Compression.ZipFile.Open(zipFilebackName, ZipArchiveMode.Create, Encoding.UTF8))
        //                {
        //                    zipArchive.CreateEntryFromFile(dbpat, Path.GetFileName(dbpat));

        //                }

        //                var name = Path.GetFileName(dbpat);
        //                using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
        //                {

        //                    zip.Password = "T134711";
        //                    zip.AddFile(zipFilebackName, "");
        //                    zip.Save(zipFileDbbackName);
        //                }

        //                using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
        //                {
        //                    zip.UseUnicodeAsNecessary = true;

        //                    zip.AddDirectory(tempfiles, "tempFile");
        //                    zip.AddDirectory(nfilepath, "Files");
        //                    zip.AddDirectory(dirRoot, "Upload");

        //                    zip.AddFile(zipFileDbbackName, "");
        //                    zip.UseZip64WhenSaving = Zip64Option.Always;
        //                    // zip.CompressionMethod = CompressionMethod.BZip2;
        //                    zip.Save(zipFileName);
        //                }


        //                var filename = Path.GetFileName(zipFilebackName);

        //                string[] files = Directory.GetFiles(path, filename);

        //                //loop through all files and delete
        //                foreach (string s in files)
        //                {
        //                    File.Delete(s);
        //                }

        //                var file2name = Path.GetFileName(zipFileDbbackName);

        //                string[] files2 = Directory.GetFiles(path, file2name);

        //                //loop through all files and delete
        //                foreach (string s in files2)
        //                {
        //                    File.Delete(s);
        //                }
        //                Directory.Delete(nfilepath, true);


        //                string[] bakfiles = Directory.GetFiles(path, "*.Bak");
        //                foreach (string s in bakfiles)
        //                {
        //                    File.Delete(s);
        //                }

        //                FileInfo f = new FileInfo(zipFileName);
        //                long filesize = f.Length; // file size in bytes
        //                var bytes = BytesToString(filesize);
        //                info.FileSize = bytes.ToString();

        //                info.SavedName = "Backup" + npath + ".zip";

        //                _TaamerProContext.DatabaseBackup.Add(info);



        //               // var unReadNotify = _NotificationRepository.GetMatching(s => s.IsDeleted == false && s.Type == 9 && (s.IsRead == false || s.IsRead == null));
        //                var unReadNotify = _TaamerProContext.Notification.Where(s => s.IsDeleted == false && s.Type == 9 && (s.IsRead == false || s.IsRead == null));


        //                if (unReadNotify != null)
        //                {
        //                    foreach (var item in unReadNotify)
        //                    {
        //                        item.IsRead = true;
        //                        item.ReadingDate = DateTime.Now;
        //                    }
        //                }

        //                _TaamerProContext.SaveChanges();
        //                int id = (int)info.BackupId;
        //                //-----------------------------------------------------------------------------------------------------------------
        //                string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
        //                string ActionNote3 = "حفظ Backup Database";
        //                 _SystemAction.SaveAction("SaveDBackup", "DatabaseBackupService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate3, UserId, BranchId, ActionNote3, 1);
        //                //-----------------------------------------------------------------------------------------------------------------
        //                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReturnedParm = id,ReasonPhrase = Resources.MDa_BackupSuccess};

        //                }
        //            catch (Exception ex)
        //            {
        //                //-----------------------------------------------------------------------------------------------------------------
        //                string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
        //                string ActionNote4 = "فشل في حفظ Backup Database";
        //                 _SystemAction.SaveAction("SaveDBackup", "DatabaseBackupService", 1,Resources.General_SavedFailed, "", "", ActionDate4, UserId, BranchId, ActionNote4, 0);
        //                //-----------------------------------------------------------------------------------------------------------------


        //                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReturnedStr = ex.Message.ToString(),ReasonPhrase = Resources.General_SavedFailed };
        //            }


        //        }
        //        //-----------------------------------------------------------------------------------------------------------------
        //        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
        //        string ActionNote = "اضافة بند جديد";
        //         _SystemAction.SaveAction("SaveDBackup", "DatabaseBackupService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
        //        //-----------------------------------------------------------------------------------------------------------------

        //        return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.MDa_BackupSuccess };
        //        }
        //    catch (Exception ex)
        //    {
        //        //-----------------------------------------------------------------------------------------------------------------
        //        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
        //        string ActionNote = "فشل في حفظ Backup Database";
        //         _SystemAction.SaveAction("SaveDBackup", "DatabaseBackupService", 1,Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
        //        //-----------------------------------------------------------------------------------------------------------------

        //        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReturnedStr = ex.Message.ToString(),ReasonPhrase = Resources.General_SavedFailed };
        //    }
        //}
        public GeneralMessage SaveDBackup(DatabaseBackup info, int UserId, string path, int BranchId, string remote, string Con)
        {
            SqlConnection con = new SqlConnection(Con);
            SqlCommand sqlcmd = new SqlCommand();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(Con);


            var local = path;
            //Create all the directories.
            string[] _bakfiles = Directory.GetFiles(path);
            //foreach (string s in _bakfiles)
            //{
            //    File.Delete(s);
            //}
            if (Directory.Exists(local))
            {
                Directory.Delete(local, true); // true = recursive delete
            }
            var uploadfolder = remote + "Uploads";
            var filefolders = remote + "Files";
            var tempfiles = remote + "TempFiles";
            var npath = "_" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss", CultureInfo.CreateSpecificCulture("en"));
            var zipFile = path + info.SavedName + npath + @".zip";
            var uploadfiles = Directory.GetFiles(uploadfolder);
            var filefiles = Directory.GetFiles(filefolders);
            var tempfilesfiles = Directory.GetFiles(tempfiles);





            string dirRoot = uploadfolder;
            //var filefolders = remote + "Files";
            //var tempfiles = remote + "TempFiles";
            //get a list of files
            string[] filesToZip = Directory.GetFiles(dirRoot, "*.*", SearchOption.AllDirectories);

            //final archive name (I use date / time)
            string zipFileName = path + "Backup" + npath + @".zip";
            string zipFilebackName = path + "DBBackup" + npath + @".zip";
            string zipFileDbbackName = path + "DBBackup" + npath + @".zip";
            string zipuploadfolder = path + "upload" + @".zip";

            var nfilepath = path + "Files";
            Directory.CreateDirectory(path + "Files");
            CopyFilesRecursivelyNew(filefolders, nfilepath);
            var directories = Directory.GetDirectories(nfilepath + "/ProjectFiles");
            Directory.CreateDirectory(nfilepath + "/ArchiveProjects");
            var archivepath = nfilepath + "/ArchiveProjects";
            var sourcepath = nfilepath + "/ProjectFiles";
            var sourcepathForFolder = "";
            var sourcepathForFolderTrim = "";
            string ProjectNo_result = "";
            var message = "";
            try
            {

                if (directories.Length > 0)
                {
                    char separator = '\\';

                    var lastWords = directories.Select(t =>
                    {
                        if (string.IsNullOrWhiteSpace(t)) return string.Empty;
                        return t.Split(separator, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
                    }).ToList();

                    string projectsNo = string.Join(",", lastWords);
                    var project = _projectRepository.getProjectsBackup_Proc(projectsNo, "rtl", Con, BranchId).Result;



                    foreach (var item in project)
                    {
                        ProjectNo_result = (item.ProjectNo ?? "").Trim();
                        ProjectNo_result = ProjectNo_result.Replace(" ", "");
                        sourcepathForFolder = sourcepath + "\\" + item.ProjectNo;
                        sourcepathForFolderTrim = sourcepath + "\\" + ProjectNo_result;


                        if (item.Status == 1)
                        {        //archive
                            var CNwithoutspecialcharacters = ExtensionMethod.RemoveSpecialChars(item.CustomerName ?? "");
                            CNwithoutspecialcharacters = CNwithoutspecialcharacters.Trim();
                            CNwithoutspecialcharacters = CNwithoutspecialcharacters.Replace(" ", "");
                            //if (File.Exists(sourcepathForFolder + "-" + CNwithoutspecialcharacters))
                            //{
                            //    Directory.Move(sourcepathForFolder, archivepath + "\\" + item.ProjectNo + CNwithoutspecialcharacters);
                            //}
                            Directory.Move(sourcepathForFolder, archivepath + "\\" + ProjectNo_result + CNwithoutspecialcharacters);

                        }
                        else
                        {
                            var CNwithoutspecialcharacters = ExtensionMethod.RemoveSpecialChars(item.CustomerName ?? "");
                            CNwithoutspecialcharacters = CNwithoutspecialcharacters.Trim();
                            CNwithoutspecialcharacters = CNwithoutspecialcharacters.Replace(" ", "");
                            //if (File.Exists(sourcepathForFolder + "-" + CNwithoutspecialcharacters))
                            //{
                            //    Directory.Move(sourcepathForFolder, sourcepathForFolder + "-" + CNwithoutspecialcharacters);

                            //}
                            Directory.Move(sourcepathForFolder, sourcepathForFolderTrim + "-" + CNwithoutspecialcharacters);

                        }

                    }
                }

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote2 = "فشل في حفظ Backup Database";
                _SystemAction.SaveAction("SaveDBackup", "DatabaseBackupService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReturnedStr = ex.Message.ToString() + message, ReasonPhrase = Resources.General_SavedFailed };
            }

            try
            {

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote2 = "فشل في حفظ Backup Database";
                _SystemAction.SaveAction("SaveDBackup", "DatabaseBackupService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReturnedStr = ex.Message.ToString(), ReasonPhrase = Resources.General_SavedFailed };
            }

            try
            {

                if (info.BackupId == 0)
                {
                    info.AddUser = UserId;
                    info.AddDate = DateTime.Now;
                    try
                    {
                        info.UserId = UserId;
                    }
                    catch (Exception ex)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote2 = "فشل في حفظ Backup Database";
                        _SystemAction.SaveAction("SaveDBackup", "DatabaseBackupService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReturnedStr = ex.Message.ToString(), ReasonPhrase = Resources.General_SavedFailed };
                    }
                    info.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));


                    var fulpath = System.IO.Path.Combine(remote, path);
                    info.LocalSavedPath = path;
                    string BackUpName = "Backup" + "_" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss", CultureInfo.CreateSpecificCulture("en"));

                    try
                    {
                        con.Open();
                        string pathDB = "BACKUP DATABASE " + "[" + builder.InitialCatalog + "]" + " TO DISK='" + fulpath /*+ "\\"*/ + BackUpName + ".Bak'" + "WITH FORMAT, INIT, MEDIANAME = N'Backup',  NAME = N'TameerProDB-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";




                        sqlcmd = new SqlCommand(pathDB, con);
                        sqlcmd.ExecuteNonQuery();
                        con.Close();
                        var dbpat = path + BackUpName + @".Bak";



                        using (ZipArchive zipArchive = System.IO.Compression.ZipFile.Open(zipFilebackName, ZipArchiveMode.Create, Encoding.UTF8))
                        {
                            zipArchive.CreateEntryFromFile(dbpat, Path.GetFileName(dbpat));

                        }

                        var name = Path.GetFileName(dbpat);
                        using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
                        {

                            zip.Password = "T134711";
                            zip.AddFile(zipFilebackName, "");
                            zip.Save(zipFileDbbackName);
                        }

                        using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
                        {
                            zip.UseUnicodeAsNecessary = true;
                            zip.CompressionLevel = Ionic.Zlib.CompressionLevel.None; // or None
                            zip.AddDirectory(tempfiles, "tempFile");
                            zip.AddDirectory(nfilepath, "Files");
                            zip.AddDirectory(dirRoot, "Upload");

                            zip.AddFile(zipFileDbbackName, "");
                            zip.UseZip64WhenSaving = Zip64Option.Always;
                            // zip.CompressionMethod = CompressionMethod.BZip2;
                            zip.Save(zipFileName);
                        }

                        //using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
                        //{
                        //    zip.UseZip64WhenSaving = Zip64Option.Always;
                        //    int fileCount = 0;
                        //    foreach (var file in Directory.GetFiles(nfilepath, "*", SearchOption.AllDirectories))
                        //    {
                        //        zip.AddFile(file);
                        //        fileCount++;

                        //        if (fileCount % 1000 == 0) // every 1000 files save a part
                        //        {
                        //            zip.Save($@"C:\output_part{fileCount / 1000}.zip");
                        //            zip.Dispose();
                        //            zip = new Ionic.Zip.ZipFile();
                        //        }
                        //    }
                        //    zip.Save(@"C:\output_last.zip");
                        //}




                        var filename = Path.GetFileName(zipFilebackName);

                        string[] files = Directory.GetFiles(path, filename);

                        //loop through all files and delete
                        foreach (string s in files)
                        {
                            File.Delete(s);
                        }

                        var file2name = Path.GetFileName(zipFileDbbackName);

                        string[] files2 = Directory.GetFiles(path, file2name);

                        //loop through all files and delete
                        foreach (string s in files2)
                        {
                            File.Delete(s);
                        }
                        Directory.Delete(nfilepath, true);


                        string[] bakfiles = Directory.GetFiles(path, "*.Bak");
                        foreach (string s in bakfiles)
                        {
                            File.Delete(s);
                        }

                        FileInfo f = new FileInfo(zipFileName);
                        long filesize = f.Length; // file size in bytes
                        var bytes = BytesToString(filesize);
                        info.FileSize = bytes.ToString();

                        info.SavedName = "Backup" + npath + ".zip";

                        _TaamerProContext.DatabaseBackup.Add(info);



                        // var unReadNotify = _NotificationRepository.GetMatching(s => s.IsDeleted == false && s.Type == 9 && (s.IsRead == false || s.IsRead == null));
                        var unReadNotify = _TaamerProContext.Notification.Where(s => s.IsDeleted == false && s.Type == 9 && (s.IsRead == false || s.IsRead == null));


                        if (unReadNotify != null)
                        {
                            foreach (var item in unReadNotify)
                            {
                                item.IsRead = true;
                                item.ReadingDate = DateTime.Now;
                            }
                        }

                        _TaamerProContext.SaveChanges();
                        int id = (int)info.BackupId;
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote3 = "حفظ Backup Database";
                        _SystemAction.SaveAction("SaveDBackup", "DatabaseBackupService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate3, UserId, BranchId, ActionNote3, 1);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReturnedParm = id, ReasonPhrase = Resources.MDa_BackupSuccess };

                    }
                    catch (Exception ex)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote4 = "فشل في حفظ Backup Database";
                        _SystemAction.SaveAction("SaveDBackup", "DatabaseBackupService", 1, Resources.General_SavedFailed, "", "", ActionDate4, UserId, BranchId, ActionNote4, 0);
                        //-----------------------------------------------------------------------------------------------------------------


                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReturnedStr = ex.Message.ToString(), ReasonPhrase = Resources.General_SavedFailed };
                    }


                }
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "اضافة بند جديد";
                _SystemAction.SaveAction("SaveDBackup", "DatabaseBackupService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.MDa_BackupSuccess };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ Backup Database";
                _SystemAction.SaveAction("SaveDBackup", "DatabaseBackupService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReturnedStr = ex.Message.ToString(), ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage SaveDBackup_ActiveYear(DatabaseBackup info, int UserId, string path, int BranchId, string remote,int yearid, string Con)
        {
            
            SqlConnection con = new SqlConnection(Con);
            SqlCommand sqlcmd = new SqlCommand();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(Con);


            var local = path;
            //Create all the directories.
            string[] _bakfiles = Directory.GetFiles(path, "*");
            foreach (string s in _bakfiles)
            {
                File.Delete(s);
            }
            var uploadfolder = remote + "Uploads";
            var filefolders = remote + "Files";
            var tempfiles = remote + "TempFiles";
            var npath = "_" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss", CultureInfo.CreateSpecificCulture("en"));
            var zipFile = path + info.SavedName + npath + @".zip";
            var uploadfiles = Directory.GetFiles(uploadfolder);
            var filefiles = Directory.GetFiles(filefolders);
            var tempfilesfiles = Directory.GetFiles(tempfiles);





            string dirRoot = uploadfolder;
            //var filefolders = remote + "Files";
            //var tempfiles = remote + "TempFiles";
            //get a list of files
            string[] filesToZip = Directory.GetFiles(dirRoot, "*.*", SearchOption.AllDirectories);

            //final archive name (I use date / time)
            string zipFileName = path + "Backup" + npath + @".zip";
            string zipFilebackName = path + "DBBackup" + npath + @".zip";
            string zipFileDbbackName = path + "DBBackup" + npath + @".zip";
            string zipuploadfolder = path + "upload" + @".zip";

            var nfilepath = path + "Files";
            Directory.CreateDirectory(path + "Files");
            CopyFilesRecursively(filefolders, nfilepath);
            var directories = Directory.GetDirectories(nfilepath + "/ProjectFiles");
            Directory.CreateDirectory(nfilepath + "/ArchiveProjects");
            var archivepath = nfilepath + "/ArchiveProjects";

            try
            {
                if (directories.Length > 0)
                {
                    foreach (var item in directories)
                    {
                        string directoryName = Path.GetFileName(item);

                        var project = _projectRepository.GetProjectByNUmber("", directoryName);
                        if (project.Result != null)
                        {
                            DateTime prodate = DateTime.ParseExact(project.Result.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                            int proyear = prodate.Year;
                            if(proyear == yearid) { 
                            if (project.Result.Status == 1)
                            {        //archive
                                     //Directory.Move(item, archivepath + "\\" + directoryName + project.CustomerName_W);
                                     //var CNwithoutspecialcharacters = Regex.Replace(project.CustomerName_W, @"[^0-9a-zA-Z]+", "");
                                var CNwithoutspecialcharacters = ExtensionMethod.RemoveSpecialChars(project.Result.CustomerName_W);
                                Directory.Move(item, archivepath + "\\" + directoryName + CNwithoutspecialcharacters);

                            }
                            else
                            {
                                var CNwithoutspecialcharacters = ExtensionMethod.RemoveSpecialChars(project.Result.CustomerName_W);
                                Directory.Move(item, item + "-" + CNwithoutspecialcharacters);
                            }

                            }
                            else
                            {
                                Directory.Delete(item,true);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote2 = "فشل في حفظ Backup Database";
                 _SystemAction.SaveAction("SaveDBackup", "DatabaseBackupService", 1,Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReturnedStr = ex.Message.ToString(),ReasonPhrase = Resources.General_SavedFailed };
            }

            try
            {
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote2 = "فشل في حفظ Backup Database";
                 _SystemAction.SaveAction("SaveDBackup", "DatabaseBackupService", 1,Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReturnedStr = ex.Message.ToString(),ReasonPhrase = Resources.General_SavedFailed };
            }

            try
            {

                if (info.BackupId == 0)
                {
                    info.AddUser = UserId;
                    info.AddDate = DateTime.Now;
                    try
                    {
                        info.UserId = UserId;
                    }
                    catch (Exception ex)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote2 = "فشل في حفظ Backup Database";
                         _SystemAction.SaveAction("SaveDBackup", "DatabaseBackupService", 1,Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReturnedStr = ex.Message.ToString(),ReasonPhrase = Resources.General_SavedFailed };
                    }
                    info.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en"));

                    var fulpath = System.IO.Path.Combine(remote,path);
                    info.LocalSavedPath = path;
                    string BackUpName = "Backup" + "_" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss", CultureInfo.CreateSpecificCulture("en"));

                    try
                    {
                        con.Open();
                        string pathDB = "BACKUP DATABASE " + builder.InitialCatalog + " TO DISK='" + fulpath /*+ "\\"*/ + BackUpName + ".Bak'" + "WITH FORMAT, INIT, MEDIANAME = N'Backup',  NAME = N'TameerProDB-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";



                        sqlcmd = new SqlCommand(pathDB, con);
                        sqlcmd.ExecuteNonQuery();
                        con.Close();
                        var dbpat = path + BackUpName + @".Bak";



                        using (ZipArchive zipArchive = System.IO.Compression.ZipFile.Open(zipFilebackName, ZipArchiveMode.Create, Encoding.UTF8))
                        {
                            zipArchive.CreateEntryFromFile(dbpat, Path.GetFileName(dbpat));

                        }

                        var name = Path.GetFileName(dbpat);
                        using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
                        {

                            zip.Password = "T134711";
                            zip.AddFile(zipFilebackName, "");
                            zip.Save(zipFileDbbackName);
                        }

                        using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
                        {
                            zip.UseUnicodeAsNecessary = true;

                            zip.AddDirectory(tempfiles, "tempFile");
                            zip.AddDirectory(nfilepath, "Files");
                            zip.AddDirectory(dirRoot, "Upload");

                            zip.AddFile(zipFileDbbackName, "");
                            zip.UseZip64WhenSaving = Zip64Option.Always;
                            // zip.CompressionMethod = CompressionMethod.BZip2;
                            zip.Save(zipFileName);
                        }


                        var filename = Path.GetFileName(zipFilebackName);

                        string[] files = Directory.GetFiles(path, filename);

                        //loop through all files and delete
                        foreach (string s in files)
                        {
                            File.Delete(s);
                        }

                        var file2name = Path.GetFileName(zipFileDbbackName);

                        string[] files2 = Directory.GetFiles(path, file2name);

                        //loop through all files and delete
                        foreach (string s in files2)
                        {
                            File.Delete(s);
                        }
                        //var file3name = Path.GetFileName(nfilepath);

                        //string[] files3 = Directory.GetFiles(path, file3name);

                        ////loop through all files and delete
                        //foreach (string s in files3)
                        //{
                        //    File.Delete(s);
                        //}

                        Directory.Delete(nfilepath, true);


                        string[] bakfiles = Directory.GetFiles(path, "*.Bak");
                        foreach (string s in bakfiles)
                        {
                            File.Delete(s);
                        }

                        FileInfo f = new FileInfo(zipFileName);
                        long filesize = f.Length; // file size in bytes
                        var bytes = BytesToString(filesize);
                        info.FileSize = bytes.ToString();

                        info.SavedName = "Backup" + npath + ".zip";

                        _TaamerProContext.DatabaseBackup.Add(info);



                        var unReadNotify = _NotificationRepository.GetMatching(s => s.IsDeleted == false && s.Type == 9 && (s.IsRead == false || s.IsRead == null));
                        if (unReadNotify != null)
                        {
                            foreach (var item in unReadNotify)
                            {
                                item.IsRead = true;
                                item.ReadingDate = DateTime.Now;
                            }
                        }

                        _TaamerProContext.SaveChanges();
                        int id = (int)info.BackupId;
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote3 = "حفظ Backup Database";
                         _SystemAction.SaveAction("SaveDBackup", "DatabaseBackupService", 1," Resources.General_SavedSuccessfully", "", "", ActionDate3, UserId, BranchId, ActionNote3, 1);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReturnedParm = id,ReasonPhrase = Resources.MDa_BackupSuccess };

                        }
                    catch (Exception ex)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote4 = "فشل في حفظ Backup Database";
                         _SystemAction.SaveAction("SaveDBackup", "DatabaseBackupService", 1,Resources.General_SavedFailed, "", "", ActionDate4, UserId, BranchId, ActionNote4, 0);
                        //-----------------------------------------------------------------------------------------------------------------


                        return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReturnedStr = ex.Message.ToString(),ReasonPhrase = Resources.General_SavedFailed };
                    }


                }
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "اضافة بند جديد";
                 _SystemAction.SaveAction("SaveDBackup", "DatabaseBackupService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.MDa_BackupSuccess };
                }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ Backup Database";
                 _SystemAction.SaveAction("SaveDBackup", "DatabaseBackupService", 1,Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReturnedStr = ex.Message.ToString(),ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage SaveDBackup2( string path,string Con)
        {
            var npath = "_" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss", CultureInfo.CreateSpecificCulture("en"));

            string bavkFileName = path + "specifivBackup" + npath + @".txt";
            StreamWriter swExtLogFile = new StreamWriter(File.Open(bavkFileName, FileMode.Create), Encoding.UTF8);// new StreamWriter(bavkFileName ,Encoding.UTF8);

            try
            {

                swExtLogFile.Write("use [TameerProDB]");
                swExtLogFile.Write(Environment.NewLine);
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand cmd = new SqlCommand("InsertGenerator", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@tableName", "Sys_Users"));
                        cmd.Connection = con;
                        con.Open();
                        //cmd.ExecuteNonQuery();


                        SqlDataAdapter a = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];


                        swExtLogFile.Write(" SET IDENTITY_INSERT[dbo].[Sys_Users] ON");
                        swExtLogFile.Write(Environment.NewLine);
                        int i;
                        foreach (DataRow row in dt.Rows)
                        {
                            object[] array = row.ItemArray;
                            for (i = 0; i < array.Length - 1; i++)
                            {
                                swExtLogFile.Write(array[i].ToString());
                            }
                            swExtLogFile.WriteLine(array[i].ToString());
                        }

                        


                    }




                    //swExtLogFile.Close();
                    //con.Close();


                }

                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand cmd = new SqlCommand("InsertGenerator", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@tableName", "Sys_Branches"));
                        cmd.Connection = con;
                        con.Open();
                        //cmd.ExecuteNonQuery();


                        SqlDataAdapter a = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];


                        swExtLogFile.Write(" SET IDENTITY_INSERT[dbo].[Sys_Branches] ON");
                        swExtLogFile.Write(Environment.NewLine);
                        int i;
                        foreach (DataRow row in dt.Rows)
                        {
                            object[] array = row.ItemArray;
                            for (i = 0; i < array.Length - 1; i++)
                            {
                                swExtLogFile.Write(array[i].ToString());
                            }
                            swExtLogFile.WriteLine(array[i].ToString());
                        }




                    }




                    //swExtLogFile.Close();
                    //con.Close();


                }

                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand cmd = new SqlCommand("InsertGenerator", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@tableName", "Sys_UserBranches"));
                        cmd.Connection = con;
                        con.Open();
                        //cmd.ExecuteNonQuery();


                        SqlDataAdapter a = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];


                        swExtLogFile.Write(" SET IDENTITY_INSERT[dbo].[Sys_UserBranches] ON");
                        swExtLogFile.Write(Environment.NewLine);
                        int i;
                        foreach (DataRow row in dt.Rows)
                        {
                            object[] array = row.ItemArray;
                            for (i = 0; i < array.Length - 1; i++)
                            {
                                swExtLogFile.Write(array[i].ToString());
                            }
                            swExtLogFile.WriteLine(array[i].ToString());
                        }




                    }




                    swExtLogFile.Close();
                    con.Close();


                }
                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =  Resources.General_SavedSuccessfully };
                }
            catch (Exception ex)
            {
                swExtLogFile.Close();
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.FailedSave };

                }
        }


        









        public GeneralMessage DeleteBackup(int BackupId, int UserId,int BranchId,string path)
        {
            try
            {
                DatabaseBackup back = _DatabaseBackupRepository.GetBackupByID(BackupId).Result;
                
                

                string fullpath = path + back.SavedName;
                string backfile = Path.GetFileNameWithoutExtension(fullpath);

                //string sourceDir = @"d:\";

                //get types files
                string[] bakfiles = Directory.GetFiles(path, "*.Bak");
                foreach (string s in bakfiles)
                {
                    File.Delete(s);
                }


                string[] files = Directory.GetFiles(path, back.SavedName);

                //loop through all files and delete
                foreach (string s in files)
                {
                    File.Delete(s);
                }
                //DeleteDirectory(fullpath);
                //Directory.Delete(fullpath);

                back.IsDeleted = true;
                back.DeleteDate = DateTime.Now;
                back.DeleteUser = UserId;

                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف Backup Database رقم " + BackupId;
                 _SystemAction.SaveAction("DeleteBackup", "DatabaseBackupService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف Backup Database رقم " + BackupId; ;
                 _SystemAction.SaveAction("DeleteBackup", "DatabaseBackupService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_DeletedFailed };
                }
        }
       
        static String BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }

        //static String RemoveSpecialChars(string str)
        //{
        //    // Create  a string array and add the special characters you want to remove
        //    string[] chars = new string[] { ",", ".", "/", "!", "@", "#", "$", "%", "^", "&", "*", "'", "\"", ";", "_", "(", ")", ":", "|", "[", "]" };
        //    //Iterate the number of times based on the String array length.
        //    for (int i = 0; i < chars.Length; i++)
        //    {
        //        if (str.Contains(chars[i]))
        //        {
        //            str = str.Replace(chars[i], "");
        //        }
        //    }
        //    return str;
        //}

        //static String RemoveSpecialCharacters(this string str)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    foreach (char c in str)
        //    {
        //        if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
        //        {
        //            sb.Append(c);
        //        }
        //    }
        //    return sb.ToString();
        //}
        public GeneralMessage sendmailnotification(List<int> user, int UserId,int BranchId, string Dayes, string Time)
        {
           
                var ListOfPrivNotify = new List<Notification>();
                var branch = _BranchesRepository.GetById(BranchId);
            //var hours = Time.Split(':')[0];
            //var minutes = Time.Split(':')[1];
            //Double h = Convert.ToDouble(hours);
            //Double m = Convert.ToDouble(minutes);
           
            if (Dayes == "أسبوع") {
                DateTime nxtday = DateTime.Now.AddDays(7);
               // nxtday = nxtday.AddHours(h).AddMinutes(m);
            }
            else if(Dayes == "يوم")
            {
                DateTime nxtday = DateTime.Now.AddDays(1);
               // nxtday = nxtday.AddHours(h).AddMinutes(m);
            }
            else 
            {
                DateTime nxtday = DateTime.Now.AddDays(30);
                //nxtday.ToLocalTime().AddHours(h);
                //nxtday = nxtday.AddHours(h).AddMinutes(m);
            
            }



            //DateTime s = DateTime.Now;
            //TimeSpan ts = new TimeSpan(hours, minutes);
            //s = s.Date + ts;
            //_userPrivilegesRepository.GetMatching(s => s.IsDeleted == false && s.PrivilegeId == 131001).Where(w => w.Users.IsDeleted == false)
            try { 
            foreach (var userCounter in user)
                    {

                        try

                        {
                        // var userexist =_NotificationRepository.GetMatching(x => x.ReceiveUserId == userCounter && x.IsDeleted == false);
                        var userexist = _TaamerProContext.Notification.Where(x => x.ReceiveUserId == userCounter && x.IsDeleted == false);

                        if (userexist == null) { 
                            ListOfPrivNotify.Add(new Notification
                            {
                                ReceiveUserId = userCounter,
                                Name = "تذكير ",
                                Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en")),
                                HijriDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CreateSpecificCulture("ar")),
                                SendUserId = UserId,
                                Type = 9, // notification backup
                                Description = "تذكير باخذ نسخة احتياطية ",
                                AllUsers = false,
                                SendDate = DateTime.Now,
                                ProjectId = 0,
                                TaskId = 0,
                                AddUser = UserId,
                                AddDate = DateTime.Now,
                               
                                IsHidden = false
                            });
                        }
                    }
                        catch (Exception ex)
                        {
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote4 = "فشل في ارسال اشعار لاخذ نسخة احتياطية";
                             _SystemAction.SaveAction("SaveProject", "ProjectService", 1,Resources.General_SavedFailed, "", "", ActionDate4, UserId, BranchId, ActionNote4, 0);
                            //-----------------------------------------------------------------------------------------------------------------
                        }

                    }
                    _TaamerProContext.Notification.AddRange(ListOfPrivNotify);
            

              
                    foreach (var userCounter in user)
                    {
                    try
                    {
                        // var userexist = _NotificationRepository.GetMatching(x => x.ReceiveUserId == userCounter && x.IsDeleted == false);
                        var userexist = _TaamerProContext.Notification.Where(x => x.ReceiveUserId == userCounter && x.IsDeleted == false);

                        if (userexist == null)
                        {
                            var Desc = "تذكير باخذ نسخة احتياطية";

                            SendMailNoti(0, Desc, "تذكير بنسخة احتياطية", BranchId, UserId, userCounter);
                        }
                    }
                    catch (Exception ex2)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate5 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote5 = "فشل في ارسال ميل لاخذ نسخة احتياطية";
                         _SystemAction.SaveAction("SaveProject", "ProjectService", 1, Resources.General_SavedFailed, "", "", ActionDate5, UserId, BranchId, ActionNote5, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                    }

                    }
            _TaamerProContext.SaveChanges();
            }
            catch(Exception ex3) {

                string ActionDate5 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote5 = "فشل في ارسال تذكير لاخذ نسخة احتياطية";
                 _SystemAction.SaveAction("SaveProject", "ProjectService", 1, Resources.General_SavedFailed, "", "", ActionDate5, UserId, BranchId, ActionNote5, 0);

            }
            //}

            //var UserNotifPriv_Mobile = _userNotificationPrivilegesService.GetUsersByPrivilegesIds(323);
            //if (UserNotifPriv_Mobile.Count() != 0)
            //{
            //    foreach (var userCounter in UserNotifPriv_Mobile)
            //    {
            //        try
            //        {
            //            var userObj = _usersRepository.GetById(userCounter.UserId);
            //            var NotStr = branch.NameAr + " فرع " + customer.CustomerNameAr + " للعميل " + project.ProjectNo + " تم اصدار فاتورة لمشروع رقم " + userCounter.FullName + " المستخدم ";
            //            if (userObj.Mobile != null && userObj.Mobile != "")
            //            {
            //                var result = _userNotificationPrivilegesService.SendSMS(userObj.Mobile, NotStr, UserId, BranchId);
            //            }

            //        catch (Exception ex)
            //        {
            //            //-----------------------------------------------------------------------------------------------------------------
            //            string ActionDate6 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
            //            string ActionNote6 = "فشل في ارسال SMS لمن لدية صلاحية فاتورة";
            //             _SystemAction.SaveAction("SaveProject", "ProjectService", 1,Resources.General_SavedFailed, "", "", ActionDate6, UserId, BranchId, ActionNote6, 0);
            //            //-----------------------------------------------------------------------------------------------------------------
            //        }

            //    }
            //}
            return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_NotificationsSent };

        }


        private bool SendMailNoti(int ProjectID, string Desc, string Subject, int BranchId, int UserId, int ToUserID)
        {

            try
            {
                //string Email = _usersRepository.GetById(ToUserID).Email ?? "";
                string Email = _TaamerProContext.Users.Where(s => s.UserId == ToUserID)?.FirstOrDefault()?.Email ?? "";
                if (Email != "")
                {
                    //var branch = _BranchesRepository.GetById(BranchId).OrganizationId;
                    // var branch = _BranchesRepository.GetById(BranchId).OrganizationId;
                    int OrganizationId;
                    Branch? branch = _TaamerProContext.Branch.Where(s => s.BranchId == BranchId).FirstOrDefault();
                    if (branch != null)
                    {
                        OrganizationId = branch.OrganizationId;
                    }
                    string textBody = Desc;
                    var mail = new MailMessage();
                    var loginInfo = new NetworkCredential(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.Password);

                    if (_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.DisplayName != null)
                    {
                        mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.SenderEmail, _EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.DisplayName);
                    }
                    else
                    {
                        mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.SenderEmail, "لديك اشعار من نظام تعمير السحابي");
                    }


                    //mail.From = new MailAddress(_EmailSettingRepository.GetEmailSetting(branch).SenderEmail);
                    mail.To.Add(new MailAddress(Email));
                    mail.Subject = Subject;
                    mail.Body = textBody;
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    var smtpClient = new SmtpClient(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.Host);
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = loginInfo;
                    //smtpClient.Port = 587;
                    smtpClient.Port = Convert.ToInt32(_EmailSettingRepository.GetEmailSetting(branch?.OrganizationId ?? 0).Result.Port);

                    smtpClient.Send(mail);
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception wx)
            {
                return false;
            }
        }


        private void DeleteDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                //Delete all files from the Directory
                foreach (string file in Directory.GetFiles(path))
                {
                    File.Delete(file);
                }
                //Delete all child Directories
                foreach (string directory in Directory.GetDirectories(path))
                {
                    DeleteDirectory(directory);
                }
                //Delete a Directory
                Directory.Delete(path);
            }
        }

        static void CopyFilesRecursivelyNew(string sourcePath, string destinationPath)
        {
            // Create target directory if not exists
            Directory.CreateDirectory(destinationPath);

            // Copy all files
            foreach (string file in Directory.GetFiles(sourcePath))
            {
                string destFile = Path.Combine(destinationPath, Path.GetFileName(file));
                File.Copy(file, destFile, overwrite: true);
            }

            // Copy all subdirectories recursively
            foreach (string directory in Directory.GetDirectories(sourcePath))
            {
                string destDir = Path.Combine(destinationPath, Path.GetFileName(directory));
                CopyFilesRecursivelyNew(directory, destDir);
            }
        }

        private static void CopyFilesRecursively(string sourcePath, string targetPath)
        {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }

        private  void CopyFilesRecursively_active(string sourcePath, string targetPath,int yearid)
        {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                string directoryName = Path.GetFileName(dirPath);

                var project = _projectRepository.GetProjectByNUmber("", directoryName);
                if (project != null)
                {
                    DateTime prodate = DateTime.ParseExact(project.Result.ProjectDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    int proyear = prodate.Year;
                    if (proyear == yearid)
                    {
                        Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
                    }
                }
                else
                {
                    Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
                }
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                if (File.Exists(newPath))
                {
                }
                
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
                
            }
        }

        private void EncryptFile(string inputFile, string outputFile)
        {
            string password = @"yourPWhere";
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] key = CreateKey(password);

            string cryptFile = outputFile;
            FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

            RijndaelManaged RMCrypto = new RijndaelManaged();
           var IV = CreateIV(password);

            CryptoStream cs = new CryptoStream(fsCrypt,
                RMCrypto.CreateEncryptor(key, IV),
                CryptoStreamMode.Write);

            FileStream fsIn = new FileStream(inputFile, FileMode.Open);

            int data;
            while ((data = fsIn.ReadByte()) != -1)
                cs.WriteByte((byte)data);


            fsIn.Close();
            cs.Close();
            fsCrypt.Close();
        }

        private static int saltLengthLimit = 32;
        private static byte[] GetSalt(int maximumSaltLength)
        {
            var salt = new byte[maximumSaltLength];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }

            return salt;
        }
        public static byte[] CreateKey(string password)
        {
            var salt = GetSalt(10);

            int iterationCount = 20000; // Nowadays you should use at least 10.000 iterations
            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, iterationCount))
                return rfc2898DeriveBytes.GetBytes(16);
        }
        public byte[] CreateIV(string password)
        {
            var salt = GetSalt(9);

            const int Iterations = 325;
            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, Iterations))
                return rfc2898DeriveBytes.GetBytes(16);
        }



        private static void RenameFolder(string path, string source, string dest)
        {
            //Console.WriteLine("{0} {1} {2}", path, source, dest); 

            DirectoryInfo di = new DirectoryInfo(path);

            foreach (DirectoryInfo si in di.GetDirectories("*", SearchOption.TopDirectoryOnly))
            {
                RenameFolder(si.Parent.FullName + @"\" + si.Name, source, dest);

                string strFoldername = si.Name;
                if (strFoldername.Contains(source))
                {
                    strFoldername = strFoldername.Replace(source, dest);
                    string strFolderRoot = si.Parent.FullName + "\\" + strFoldername;

                    si.MoveTo(strFolderRoot);
                }
                Console.WriteLine("{0} renamed to {1}", si.Parent.FullName, si.Name);
            }
        }




        //add scope
        public static string[] Scopes = { Google.Apis.Drive.v3.DriveService.Scope.Drive };
        
        //create Drive API service.
        public static DriveService GetService(string Cspath ,string FolserPath)
        {
            //get Credentials from client_secret.json file 
             
            UserCredential credential;
            //Root Folder of project
            var CSPath = Cspath;// System.Web.Hosting.HostingEnvironment.MapPath("~/");
            using (var stream = new FileStream(Path.Combine(CSPath, "client_secret_941871846078-6le0onkm323rmsi1b49ejct46l4r1vp0.apps.googleusercontent.com (12).json"), FileMode.Open, FileAccess.Read))
            {
                String FolderPath = FolserPath;// System.Web.Hosting.HostingEnvironment.MapPath("~/"); ;
                String FilePath = Path.Combine(FolderPath, "DriveServiceCredentials.json");
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(FilePath, true)).Result;
            }
          
            //create Drive API service.
            DriveService service = new Google.Apis.Drive.v3.DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                //ApiKey= "AIzaSyDMBqumxSeJ03L5LCymoi_x6MPZ0jnUXOM",
                ApplicationName = "Tameer",
            });
            service.HttpClient.Timeout = TimeSpan.FromMinutes(100);
            
            return service;
            
        }
        public static DriveService GetServiceProject(string Cspath, string FolserPath)
        {
            //get Credentials from client_secret.json file 

            UserCredential credential;
            //Root Folder of project
            var CSPath = Cspath;// System.Web.Hosting.HostingEnvironment.MapPath("~/");
            using (var stream = new FileStream(Path.Combine(CSPath, "Credential.json"), FileMode.Open, FileAccess.Read))
            {
                String FolderPath = FolserPath;// System.Web.Hosting.HostingEnvironment.MapPath("~/"); ;
                String FilePath = Path.Combine(FolderPath, "DriveServiceCredentials.json");
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(FilePath, true)).Result;
            }

            //create Drive API service.
            DriveService service = new Google.Apis.Drive.v3.DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                //ApiKey= "AIzaSyDMBqumxSeJ03L5LCymoi_x6MPZ0jnUXOM",
                ApplicationName = "Tameer",
            });
            service.HttpClient.Timeout = TimeSpan.FromMinutes(100);

            return service;

        }


        //file Upload to the Google Drive root folder.
        public GeneralMessage UplaodFileOnDrive(string fileName , string paths,string CsPath ,string FolderPath ,string mimmapping ,string bckuppth)
        {
           // testuploadAsync(CsPath, FolderPath, bckuppth);

            try {


                DriveService service = GetService(CsPath, FolderPath);
                string path = paths;// Path.Combine(HttpContext.Current.Server.MapPath("~/GoogleDriveFiles"),
                                    //Path.GetFileName(file.FileName));
                                    // file.SaveAs(path);
                var FileMetaData = new Google.Apis.Drive.v3.Data.File();
                FileMetaData.Name = fileName;// Path.GetFileName(file.FileName);
                FileMetaData.MimeType = mimmapping;// MimeMapping.GetMimeMapping(path);
                FilesResource.CreateMediaUpload request;
                try
                {
                    using (var stream = new System.IO.FileStream(bckuppth, System.IO.FileMode.Open))
                    {
                        request = service.Files.Create(FileMetaData, stream, FileMetaData.MimeType);
                        request.Fields = "*";
                        //request.ResponseReceived += Upload_ResponseReceived;
                        //request.ProgressChanged += Upload_ProgressChanged;
                        request.UploadAsync();
                    }
                }
                catch (Exception ex)
                {
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed, ReturnedStr = ex.Message.ToString() };

                }
                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.uploadsuccfully };
            }
            catch(Exception ex)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = ex.InnerException.ToString(),ReturnedStr= ex.Message.ToString()};


            }



        }

        public GeneralMessage UplaodFileOnDriveProject(string fileName, string paths, string CsPath, string FolderPath, string mimmapping, string bckuppth)
        {
            // testuploadAsync(CsPath, FolderPath, bckuppth);

            try
            {


                DriveService service = GetServiceProject(CsPath, FolderPath);
                string path = paths;// Path.Combine(HttpContext.Current.Server.MapPath("~/GoogleDriveFiles"),
                                    //Path.GetFileName(file.FileName));
                                    // file.SaveAs(path);
                var FileMetaData = new Google.Apis.Drive.v3.Data.File();
                FileMetaData.Name = fileName;// Path.GetFileName(file.FileName);
                FileMetaData.MimeType = mimmapping;// MimeMapping.GetMimeMapping(path);
                FilesResource.CreateMediaUpload request;
                try
                {
                    using (var stream = new System.IO.FileStream(bckuppth, System.IO.FileMode.Open))
                    {
                        request = service.Files.Create(FileMetaData, stream, FileMetaData.MimeType);
                        request.Fields = "*";
                        //request.ResponseReceived += Upload_ResponseReceived;
                        //request.ProgressChanged += Upload_ProgressChanged;
                        request.UploadAsync();
                    }
                }
                catch (Exception ex)
                {
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed, ReturnedStr = ex.Message.ToString() };

                }
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.uploadsuccfully };
            }
            catch (Exception ex)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = ex.InnerException.ToString(), ReturnedStr = ex.Message.ToString() };


            }



        }

        public async Task testuploadAsync(string Cspath, string FolserPath, string bckuppth)
        {
            UserCredential credential;

            //Root Folder of project
            var CSPath = Cspath;// System.Web.Hosting.HostingEnvironment.MapPath("~/");
            using (var stream = new FileStream(Path.Combine(CSPath, "client_secret_941871846078-6le0onkm323rmsi1b49ejct46l4r1vp0.apps.googleusercontent.com (12).json"), FileMode.Open, FileAccess.Read))
            {
                String FolderPath = FolserPath;// System.Web.Hosting.HostingEnvironment.MapPath("~/"); ;
                String FilePath = Path.Combine(FolderPath, "DriveServiceCredentials.json");
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(FilePath, true)).Result;
            }
            // Create the service using the client credentials.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                //ApiKey = "AIzaSyDMBqumxSeJ03L5LCymoi_x6MPZ0jnUXOM",

                ApplicationName = "Tameer"
            });
            //xmlFile2 = Server.MapPath("~/Invoice/Invoice_638009239226849869.xml");
            using (var uploadStream = System.IO.File.OpenRead(bckuppth))
            {

                // Create the File resource to upload.
                Google.Apis.Drive.v3.Data.File driveFile = new Google.Apis.Drive.v3.Data.File
                {
                    Name = "Drive_File_Name"
                };
                // Get the media upload request object.
                FilesResource.CreateMediaUpload insertRequest = service.Files.Create(
                    driveFile, uploadStream, "image/jpg");

                // Add handlers which will be notified on progress changes and upload completion.
                // Notification of progress changed will be invoked when the upload was started,
                // on each upload chunk, and on success or failure.
                //insertRequest.ProgressChanged += Upload_ProgressChanged;
                //insertRequest.ResponseReceived += Upload_ResponseReceived;
                
                await insertRequest.UploadAsync();

            }


        }





        public GeneralMessage UploadFiletodrive2(string fileName, string paths, string CsPath, string FolderPath, string mimmapping, string bckuppth)
        {
            try
            {
                DriveService service = GetService();


                string path = paths;// Path.Combine(HttpContext.Current.Server.MapPath("~/GoogleDriveFiles"),
                                    //Path.GetFileName(file.FileName));
                                    // file.SaveAs(path);
                var FileMetaData = new Google.Apis.Drive.v3.Data.File();
                FileMetaData.Name = fileName;// Path.GetFileName(file.FileName);
                FileMetaData.MimeType = mimmapping;// MimeMapping.GetMimeMapping(path);
                //FilesResource.CreateMediaUpload request;
                try
                {
                    using (var stream = new System.IO.FileStream(bckuppth, System.IO.FileMode.Open))
                    {
                      
                        var request = service.Files.Create(FileMetaData, stream, FileMetaData.MimeType);
                        request.Fields = "id";
                        var response = request.Upload();
                        if (response.Status != Google.Apis.Upload.UploadStatus.Completed)
                            throw response.Exception;

                    }
                }
                catch (Exception ex)
                {
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed, ReturnedStr = ex.Message.ToString() };

                }
                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.uploadsuccfully };
            }
            catch (Exception ex)
            {
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = ex.InnerException.ToString(), ReturnedStr = ex.Message.ToString() };


            }


        }
        private static DriveService GetService()
        {
            var tokenResponse = new TokenResponse
            {
                AccessToken = "ya29.a0AeTM1if8m6G7769IlaRFz4l0wvrFoW5ozBQVO5PoKBQaNDNhT-GaQ3DF4_NoOACEj26jmH8r-TOcuv2lU-SPyq2SrPYYoXTAKJct-oa9XRBAllmD9dvpggMMP_dz-rblr83OQaMnRPxUnyjlQH0hF1U00a4eaCgYKAY0SARISFQHWtWOmbmJFxVLGlF1Wj_HmK3OSlA0163",
                RefreshToken = "1//04OWd0ZbKNoYhCgYIARAAGAQSNwF-L9IrjZsYP7wblP90lGk4VitEhEIXIne4R5tIwn6cIoS3iX0PPz-qb2reZnaV6rWL4v5nlSM",
            };


            var applicationName = "MVC Drive Exampe";// Use the name of the project in Google Cloud
            var username = "";// Use your email

           
            var apiCodeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = "941871846078-6le0onkm323rmsi1b49ejct46l4r1vp0.apps.googleusercontent.com",
                    ClientSecret = "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"
                },
                Scopes = new[] { Scope.Drive },
                DataStore = new FileDataStore(applicationName)
            });


            var credential = new UserCredential(apiCodeFlow, username, tokenResponse);


            var service = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName
            });
                    service.HttpClient.Timeout = TimeSpan.FromMinutes(100);

                    return service;
                }




                static void Upload_ResponseReceived(Google.Apis.Drive.v3.Data.File file) =>
            Debug.WriteLine(file.Name + " was uploaded successfully");



            static void Upload_ProgressChanged(IUploadProgress progress) =>
            Debug.WriteLine(progress.Status + " " + progress.BytesSent);

            }


   


            public static class ExtensionMethod
            {

                public static void AddDirectory(this ZipArchive zip, string targetDir, string subDir = null, CompressionLevel compressionLevel = CompressionLevel.Optimal)
                {
                    var ogrDir = targetDir.Replace("/", "\\");
                    if (!ogrDir.EndsWith("\\"))
                        ogrDir = ogrDir + "\\";

                    if (subDir == null)
                        subDir = "";
                    else
                    {
                        subDir = subDir.Replace("/", "\\");
                        if (subDir.StartsWith("\\"))
                            subDir = subDir.Remove(0, 1);

                        if (!subDir.EndsWith("\\"))
                            subDir = subDir + "\\";
                    }
                    Action<string> AddDirectoryAndSubs = null;
                    AddDirectoryAndSubs = delegate (string _targetDir)
                    {
                        string[] files = Directory.GetFiles(_targetDir);
                        foreach (string file in files)
                        {
                            var fileInfo = new FileInfo(file);
                            zip.CreateEntryFromFile(fileInfo.FullName, subDir + (fileInfo.Directory.ToString() + "\\").Replace(ogrDir, "") + fileInfo.Name, compressionLevel);
                        }

                        string[] dirs = Directory.GetDirectories(_targetDir);
                        foreach (string dir in dirs)
                            AddDirectoryAndSubs(dir);
                    };

                    AddDirectoryAndSubs(targetDir);
                }



                public static string RemoveSpecialChars(string str)
                {
                    // Create  a string array and add the special characters you want to remove
                    string[] chars = new string[] { ",", ".", "/", "!", "@", "#", "$", "%", "^", "&", "*", "'", "\\", ";", "_", "(", ")", ":", "|", "[", "]", "\t", "\n" };
            //Iterate the number of times based on the String array length.
            if (str != null)
            {
      
              
                for (int i = 0; i < chars.Length; i++)
                {
                    if (str.Contains(chars[i]))
                    {
                        str = str.Replace(chars[i], "");
                    }
                }
            }
                    return str;
                }

                public static string RemoveSpecialCharacters(this string str)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (char c in str)
                    {
                        if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                        {
                            sb.Append(c);
                        }
                    }
                    return sb.ToString();
                }





    }


}
