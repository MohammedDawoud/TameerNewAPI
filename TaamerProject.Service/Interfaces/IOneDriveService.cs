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
    public interface IOneDriveService
    {
        GeneralMessage UploadFile(int FileId, int type, string folder, string file, string content, byte[]? bytes);
        GeneralMessage DeleteFile(string FileId);
        GeneralMessage DownloadFile(string filename);

    }
}
