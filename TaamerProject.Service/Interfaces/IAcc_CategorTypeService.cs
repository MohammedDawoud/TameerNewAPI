using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TaamerProject.Models.Common;

namespace TaamerProject.Service.Interfaces
{
    public interface IAcc_CategorTypeService
    {

        Task<IEnumerable<Acc_CategorTypeVM>> GetAllCategorytype(string SearchText);
        GeneralMessage SaveCategory(Acc_CategorType CategoryT, int UserId, int BranchId);
        GeneralMessage DeleteCategory(int Categryid, int UserId, int BranchId);
    }
}
