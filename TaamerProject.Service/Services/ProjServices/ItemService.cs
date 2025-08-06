using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using System.Net;
using System.Data.SqlClient;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class ItemService : IItemService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IItemRepository _ItemRepository ;



        public ItemService(TaamerProjectContext dataContext, ISystemAction systemAction, IItemRepository itemRepository) 
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
_ItemRepository = itemRepository;
     
        }
        public async Task<IEnumerable<ItemVM>> GetAllItems(string lang, int? typeId = null)
        {
            var items = await _ItemRepository.GetAllItems(lang, typeId);
            return items;
        }
        public GeneralMessage SaveItem(Item item, int UserId, int BranchId)
        {
            try
            {
                if (item.ItemId == 0)
                {
                    item.BranchId = BranchId;
                    item.AddUser = UserId;
                    item.AddDate = DateTime.Now;
                    //ItemUpdated.BranchId = item.BranchId;
                    item.Ramainder = item.Quantity;
                    _TaamerProContext.Item.Add(item);
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة عهدة جديدة";
                   _SystemAction.SaveAction("SaveItem", "ItemService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully, ReturnedParm = item.ItemId, ReturnedStr = item.ItemId.ToString() };
                }
                else
                {
                    var ItemUpdated = _ItemRepository.GetById(item.ItemId);
                    if (ItemUpdated != null)
                    {
                        ItemUpdated.NameAr = item.NameAr;
                        ItemUpdated.NameEn = item.NameEn;
                        ItemUpdated.TypeId = item.TypeId;
                        ItemUpdated.Quantity = item.Quantity;
                        ItemUpdated.Ramainder = (item.Quantity ?? 0) + (ItemUpdated.Ramainder < 0 ? 0 : ItemUpdated.Ramainder);
                        ItemUpdated.Price = item.Price;
                        ItemUpdated.SachetNo = item.SachetNo;
                        ItemUpdated.FormNo = item.FormNo;
                        ItemUpdated.Color = item.Color;
                        ItemUpdated.IssuancePlace = item.IssuancePlace;
                        ItemUpdated.IssuanceDate = item.IssuanceDate;
                        ItemUpdated.IssuanceHijriDate = item.IssuanceHijriDate;
                        ItemUpdated.IssuanceEndDate = item.IssuanceEndDate;
                        ItemUpdated.IssuanceEndHijriDate = item.IssuanceEndHijriDate;
                        ItemUpdated.SupplyDate = item.SupplyDate;
                        ItemUpdated.SupplyHijriDate = item.SupplyHijriDate;
                        ItemUpdated.PlateNo = item.PlateNo;
                        ItemUpdated.InsuranceNo = item.InsuranceNo;
                        ItemUpdated.InsuranceEndDate = item.InsuranceEndDate;
                        ItemUpdated.InsuranceEndHijriDate = item.InsuranceEndHijriDate;
                        ItemUpdated.LiscenceFileUrl = item.LiscenceFileUrl;
                        ItemUpdated.InsuranceFileUrl = item.InsuranceFileUrl;
                        //ItemUpdated.BranchId = item.BranchId;
                        //ItemUpdated.Ramainder = item.Quantity;
                        ItemUpdated.Status = item.Status;
                        ItemUpdated.UpdateUser = UserId;
                        ItemUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل عهدة رقم " + item.ItemId;
                    _SystemAction.SaveAction("SaveItem", "ItemService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully, ReturnedParm = item.ItemId, ReturnedStr = item.ItemId.ToString() };
                }

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ عهدة";
                _SystemAction.SaveAction("SaveItem", "ItemService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteItem(int ItemId, int UserId, int BranchId)
        {
            try
            {
                Item item = _ItemRepository.GetById(ItemId);
                item.IsDeleted = true;
                item.DeleteDate = DateTime.Now;
                item.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate4 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote4 = " حذف عهدة رقم " + ItemId;
                _SystemAction.SaveAction("DeleteItem", "ItemService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate4, UserId, BranchId, ActionNote4, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حذف عهدة";
                _SystemAction.SaveAction("DeleteItem", "ItemService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
        public IEnumerable<object> FillItemSelect()
        {
            return _ItemRepository.GetAllItems("rtl").Result.Select(s => new
            {
                Id = s.ItemId,
                Name = s.NameAr
            });
        }

        //public IEnumerable<ItemVM> FillSelectItem(string lang, int BranchId,int ItemId)
        //{
        //    return _ItemRepository.FillSelectItem(lang, BranchId, ItemId).Select(s => new
        //    {
        //        Id = s.ItemId,
        //        Name = s.NameAr
        //    });
        //}

        public IEnumerable<object> FillItemSelectSQL(string Con, int ItemId)
        {

            SqlConnection con = new SqlConnection(Con);
            con.Open();
            SqlCommand cmd = new SqlCommand("select itemid,NameAr from Emp_Items where ItemId=" + ItemId + " ||ItemId not in(select itemid from Emp_EmpCustody where IsDeleted =0)");
            cmd.Connection = con;

            SqlDataAdapter a = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            a.Fill(ds);
            DataTable myDataTable = new DataTable();
            myDataTable = ds.Tables[0];
            con.Close();
            return myDataTable.AsEnumerable().Select(row => new
            {
                Id = int.Parse(row[0].ToString()),
                Name = row[1].ToString()
            }
            );

        }

        public IEnumerable<object> FillItemSelectSQL(string Con)
        {
            //List<ItemVM> lmd = new List<ItemVM>();
            //object obj = new object();


            SqlConnection con = new SqlConnection(Con);
            con.Open();
            SqlCommand cmd = new SqlCommand("select itemid,NameAr+' ('+CAST(Ramainder as varchar(10)) + ') ' as NameAr from Emp_Items where Ramainder>0 and IsDeleted =0");
            cmd.Connection = con;


            SqlDataAdapter a = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            a.Fill(ds);
            DataTable myDataTable = new DataTable();
            myDataTable = ds.Tables[0];
            con.Close();

            return myDataTable.AsEnumerable().Select(row => new
            {
                Id = int.Parse(row[0].ToString()),
                Name = row[1].ToString()
            }
            );



            //foreach (System.Data.DataRow row in myDataTable.Rows)
            //{
            //    lmd.Add(new ItemVM
            //    {
            //        ItemId = int.Parse(row[0].ToString()),
            //        ItemName = row[1].ToString()
            //    }

            //    );
            //}
            // return lmd;
            //return _ItemRepository.GetAllItems("rtl").Select(s => new
            //{
            //    Id = s.ItemId,
            //    Name = s.NameAr
            //});
        }

        public IEnumerable<object> FillItemCarSelect()
        {
            return _ItemRepository.GetAllItems("rtl").Result.Where(s => s.TypeId == 1).Select(s => new
            {
                Id = s.ItemId,
                Name = s.NameAr
            });
        }
        public IEnumerable<ItemVM> SearchItems(ItemVM ItemsSearch, string lang)
        {
            var items = _ItemRepository.SearchItems(ItemsSearch, lang).Result.ToList();
            return items;
        }
        public bool IsCar(int ItemId)
        {
            var ItemTypeId = _ItemRepository.GetById(ItemId).TypeId;
            if (ItemTypeId != null)
                return _TaamerProContext.ItemType.Where(x=>x.ItemTypeId==ItemTypeId).FirstOrDefault().NameEn == "Car" ? true : false;
            else
                return false;
        }
    }
}
