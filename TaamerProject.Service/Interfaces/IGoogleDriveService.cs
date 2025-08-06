using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TaamerProject.Models.Common;
using Dropbox.Api;

namespace TaamerProject.Service.Interfaces
{
    public interface IGoogleDriveService
    {
        GeneralMessage UploadFile(int FileId, int type, string folder, string file, string content, byte[]? bytes);
        GeneralMessage DeleteFile(string FileId);
        GeneralMessage DownloadFile(string FileId, string UploadName);

        GeneralMessage UploadFileNew(Stream file, string fileName, string fileMime, string folder, string fileDescription);
        GeneralMessage  CreateFolder_Func(string folderName);
        GeneralMessage CreateFolder_Func2(string folderName);

    }
}
