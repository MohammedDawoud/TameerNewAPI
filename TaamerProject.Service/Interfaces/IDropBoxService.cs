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
    public interface IDropBoxService
    {
        GeneralMessage DownloadFile(DropboxClient dbx, string folder, string file);
        GeneralMessage UploadFile(DropboxClient dbx, string folder, string file, string content, byte[]? bytes);
        GeneralMessage DeleteFile(DropboxClient dbx, string folder, string file);

    }
}
