using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaamerP.Service.LocalResources;

namespace TaamerProject.API.Helper
{
    public static class NotificationPrivilages
    {
        public static List<NotificationPrivilage> NotificationPrivilage = new List<NotificationPrivilage>
    {
        #region Employess
        new NotificationPrivilage(){Id=1, ParentId= null, Name=Resources.General_Employees, Description= Resources.General_Employees },
        //مباشرة موظف جديد
             new NotificationPrivilage(){Id=11, ParentId= 1, Name=Resources.Con_StartWork, Description= Resources.Con_StartWork },
                new NotificationPrivilage(){Id=111, ParentId= 11, Name= Resources.General_SendMail, Description= Resources.General_SendMail },
                new NotificationPrivilage(){Id=112, ParentId= 11, Name= Resources.General_Notifaction, Description= Resources.General_Notifaction },
                new NotificationPrivilage(){Id=113, ParentId= 11, Name= Resources.General_SendMassage, Description= Resources.General_SendMassage },
        //إنهاء خدمات موظف
             new NotificationPrivilage(){Id=12, ParentId= 1, Name=Resources.Emp_EndWork, Description= Resources.Emp_EndWork },
                new NotificationPrivilage(){Id=121, ParentId= 12, Name= Resources.General_SendMail, Description= Resources.General_SendMail },
                new NotificationPrivilage(){Id=122, ParentId= 12, Name= Resources.General_Notifaction, Description= Resources.General_Notifaction },
                new NotificationPrivilage(){Id=123, ParentId= 12, Name= Resources.General_SendMassage, Description= Resources.General_SendMassage },
       // تقديم طلب إجازة
             new NotificationPrivilage(){Id=18, ParentId= 1, Name = "تقديم طلب إجازة", Description= "تقديم طلب إجازة" },
                new NotificationPrivilage(){Id=181, ParentId= 18, Name= Resources.General_SendMail, Description= Resources.General_SendMail },
                new NotificationPrivilage(){Id=182, ParentId= 18, Name= Resources.General_Notifaction, Description= Resources.General_Notifaction },
                new NotificationPrivilage(){Id=183, ParentId= 18, Name= Resources.General_SendMassage, Description= Resources.General_SendMassage },
        //تقديم طلب سلفة
             new NotificationPrivilage(){Id=19, ParentId= 1, Name = "تقديم طلب سلفة", Description= "تقديم طلب سلفة" },
                new NotificationPrivilage(){Id=191, ParentId= 19, Name= Resources.General_SendMail, Description= Resources.General_SendMail },
                new NotificationPrivilage(){Id=192, ParentId= 19, Name= Resources.General_Notifaction, Description= Resources.General_Notifaction },
                new NotificationPrivilage(){Id=193, ParentId= 19, Name= Resources.General_SendMassage, Description= Resources.General_SendMassage },
        //تعديل إجازة موظف (الرد على طلب الإجازة)
             new NotificationPrivilage(){Id=13, ParentId= 1, Name=Resources.General_EditVacation, Description= Resources.General_EditVacation },
                new NotificationPrivilage(){Id=131, ParentId= 13, Name= Resources.General_SendMail, Description= Resources.General_SendMail },
                new NotificationPrivilage(){Id=132, ParentId= 13, Name= Resources.General_Notifaction, Description= Resources.General_Notifaction },
                new NotificationPrivilage(){Id=133, ParentId= 13, Name= Resources.General_SendMassage, Description= Resources.General_SendMassage },
        //تعديل سلفة موظف (الرد على طلب السلفة)
             new NotificationPrivilage(){Id=14, ParentId= 1, Name=  "تعديل سلفة", Description= "تعديل سلفة" },
                new NotificationPrivilage(){Id=141, ParentId= 14, Name= Resources.General_SendMail, Description= Resources.General_SendMail },
                new NotificationPrivilage(){Id=142, ParentId= 14, Name= Resources.General_Notifaction, Description= Resources.General_Notifaction },
                new NotificationPrivilage(){Id=143, ParentId= 14, Name= Resources.General_SendMassage, Description= Resources.General_SendMassage },
        //استلام العهدة
             new NotificationPrivilage(){Id=15, ParentId= 1, Name=Resources.Emp_ReceiveCustody, Description= Resources.Emp_ReceiveCustody },
                new NotificationPrivilage(){Id=151, ParentId= 15, Name= Resources.General_SendMail, Description= Resources.General_SendMail },
                new NotificationPrivilage(){Id=152, ParentId= 15, Name= Resources.General_Notifaction, Description= Resources.General_Notifaction },
                new NotificationPrivilage(){Id=153, ParentId= 15, Name= Resources.General_SendMassage, Description= Resources.General_SendMassage },
        //فك العهدة
             new NotificationPrivilage(){Id=16, ParentId= 1, Name=Resources.MC_Free, Description= Resources.MC_Free },
                new NotificationPrivilage(){Id=161, ParentId= 16, Name= Resources.General_SendMail, Description= Resources.General_SendMail },
                new NotificationPrivilage(){Id=162, ParentId= 16, Name= Resources.General_Notifaction, Description= Resources.General_Notifaction },
                new NotificationPrivilage(){Id=163, ParentId= 16, Name= Resources.General_SendMassage, Description= Resources.General_SendMassage },
        //تجاوز الغياب الحد القانوني
             new NotificationPrivilage(){Id=17, ParentId= 1, Name = "تجاوز الغياب الحد القانوني", Description= "تجاوز الغياب الحد القانوني" },
                new NotificationPrivilage(){Id=171, ParentId= 17, Name= Resources.General_SendMail, Description= Resources.General_SendMail },
                new NotificationPrivilage(){Id=172, ParentId= 17, Name= Resources.General_Notifaction, Description= Resources.General_Notifaction },
                new NotificationPrivilage(){Id=173, ParentId= 17, Name= Resources.General_SendMassage, Description= Resources.General_SendMassage },
         // آخرحاجة 19 لكن موجودة في بداية الليستة
         // 
          new NotificationPrivilage(){Id=20, ParentId= 1, Name = "تذكير بالاقامات قاربت علي الانتهاء", Description= "تذكير بالاقامات قاربت علي الانتهاء" },
           new NotificationPrivilage(){Id=201, ParentId= 1, Name = "تذكير بالعقود قاربت علي الانتهاء", Description= "تذكير بالعقود قاربت علي الانتهاء" },

        #endregion

        #region Customers
        new NotificationPrivilage(){Id=2, ParentId= null, Name=Resources.General_Secrtary1, Description= Resources.General_Secrtary1 },
        //عميل جديد
             new NotificationPrivilage(){Id=21, ParentId= 2, Name=Resources.Pro_AddNewCustomer, Description= Resources.Pro_AddNewCustomer },
                new NotificationPrivilage(){Id=211, ParentId= 21, Name= Resources.General_SendMail, Description= Resources.General_SendMail },
                new NotificationPrivilage(){Id=212, ParentId= 21, Name= Resources.General_Notifaction, Description= Resources.General_Notifaction },
                new NotificationPrivilage(){Id=213, ParentId= 21, Name= Resources.General_SendMassage, Description= Resources.General_SendMassage },
        #endregion 
        #region Projects
             new NotificationPrivilage(){Id=3, ParentId= null, Name=Resources.Pro_Projectmanagement, Description= Resources.Pro_Projectmanagement },


        //إيقاف مشروع
             new NotificationPrivilage(){Id=33, ParentId= 3, Name="إيقاف مشروع", Description= "إيقاف مشروع" },
                new NotificationPrivilage(){Id=331, ParentId= 33, Name= Resources.General_SendMail, Description= Resources.General_SendMail },
                new NotificationPrivilage(){Id=332, ParentId= 33, Name= Resources.General_Notifaction, Description= Resources.General_Notifaction },
                new NotificationPrivilage(){Id=333, ParentId= 33, Name= Resources.General_SendMassage, Description= Resources.General_SendMassage },
        //تشغيل مشروع كان متوقف
             new NotificationPrivilage(){Id=34, ParentId= 3, Name="تشغيل مشروع كان متوقف", Description= "تشغيل مشروع كان متوقف" },
                new NotificationPrivilage(){Id=341, ParentId= 34, Name= Resources.General_SendMail, Description= Resources.General_SendMail },
                new NotificationPrivilage(){Id=342, ParentId= 34, Name= Resources.General_Notifaction, Description= Resources.General_Notifaction },
                new NotificationPrivilage(){Id=343, ParentId= 34, Name= Resources.General_SendMassage, Description= Resources.General_SendMassage },
        //إسناد مهمة جديدة لموظف
             new NotificationPrivilage(){Id=35, ParentId= 3, Name=Resources.General_AddTasks, Description= Resources.General_AddTasks },
                new NotificationPrivilage(){Id=351, ParentId= 35, Name= Resources.General_SendMail, Description= Resources.General_SendMail },
                new NotificationPrivilage(){Id=352, ParentId= 35, Name= Resources.General_Notifaction, Description= Resources.General_Notifaction },
                new NotificationPrivilage(){Id=353, ParentId= 35, Name= Resources.General_SendMassage, Description= Resources.General_SendMassage },
        //تحويل مهمة إلى لموظف آخر
             new NotificationPrivilage(){Id=36, ParentId= 3, Name=Resources.exchangeTask, Description= Resources.exchangeTask },
                new NotificationPrivilage(){Id=361, ParentId= 36, Name= Resources.General_SendMail, Description= Resources.General_SendMail },
                new NotificationPrivilage(){Id=362, ParentId= 36, Name= Resources.General_Notifaction, Description= Resources.General_Notifaction },
                new NotificationPrivilage(){Id=363, ParentId= 36, Name= Resources.General_SendMassage, Description= Resources.General_SendMassage },
        //اشعار بالعاملين علي مشروع
             //new NotificationPrivilage(){Id=37, ParentId= 3, Name="اشعار بالعاملين علي مشروع", Description= "اشعار بالعاملين علي مشروع" },
             //   new NotificationPrivilage(){Id=371, ParentId= 37, Name= Resources.General_SendMail, Description= Resources.General_SendMail },
             //   new NotificationPrivilage(){Id=372, ParentId= 37, Name= Resources.General_Notifaction, Description= Resources.General_Notifaction },
             //   new NotificationPrivilage(){Id=373, ParentId= 37, Name= Resources.General_SendMassage, Description= Resources.General_SendMassage },
        //تغيير مدة المهمة 
             new NotificationPrivilage(){Id=38, ParentId= 3, Name="تغيير مدة المهمة ", Description= "تغيير مدة المهمة" },
                new NotificationPrivilage(){Id=381, ParentId= 38, Name= Resources.General_SendMail, Description= Resources.General_SendMail },
                new NotificationPrivilage(){Id=382, ParentId= 38, Name= Resources.General_Notifaction, Description= Resources.General_Notifaction },
                new NotificationPrivilage(){Id=383, ParentId= 38, Name= Resources.General_SendMassage, Description= Resources.General_SendMassage },
        //منح المستخدم صلاحية علي مشروع 
             new NotificationPrivilage(){Id=39, ParentId= 3, Name="منح المستخدم صلاحية علي مشروع ", Description= "منح المستخدم صلاحية علي مشروع" },
                new NotificationPrivilage(){Id=391, ParentId= 39, Name= Resources.General_SendMail, Description= Resources.General_SendMail },
                new NotificationPrivilage(){Id=392, ParentId= 39, Name= Resources.General_Notifaction, Description= Resources.General_Notifaction },
                new NotificationPrivilage(){Id=393, ParentId= 39, Name= Resources.General_SendMassage, Description= Resources.General_SendMassage },
        //انتهاء مرحلة رئيسية من مشروع
             //new NotificationPrivilage(){Id=310, ParentId= 3, Name="انتهاء مرحلة رئيسية من مشروع ", Description= "انتهاء مرحلة رئيسية من مشروع" },
             //   new NotificationPrivilage(){Id=3101, ParentId= 310, Name= Resources.General_SendMail, Description= Resources.General_SendMail },
             //   new NotificationPrivilage(){Id=3102, ParentId= 310, Name= Resources.General_Notifaction, Description= Resources.General_Notifaction },
             //   new NotificationPrivilage(){Id=3103, ParentId= 310, Name= Resources.General_SendMassage, Description= Resources.General_SendMassage },
        //انتهاء مرحلة فرعية من مشروع
             //new NotificationPrivilage(){Id=311, ParentId= 3, Name="انتهاء مرحلة فرعية من مشروع ", Description= "انتهاء مرحلة فرعية من مشروع" },
             //   new NotificationPrivilage(){Id=3111, ParentId= 311, Name= Resources.General_SendMail, Description= Resources.General_SendMail },
             //   new NotificationPrivilage(){Id=3112, ParentId= 311, Name= Resources.General_Notifaction, Description= Resources.General_Notifaction },
             //   new NotificationPrivilage(){Id=3113, ParentId= 311, Name= Resources.General_SendMassage, Description= Resources.General_SendMassage },
        //المهمة قيد التشغيل
             new NotificationPrivilage(){Id=312, ParentId= 3, Name="المهمة قيد التشغيل", Description= "المهمة قيد التشغيل" },
                new NotificationPrivilage(){Id=3121, ParentId= 312, Name= Resources.General_SendMail, Description= Resources.General_SendMail },
                new NotificationPrivilage(){Id=3122, ParentId= 312, Name= Resources.General_Notifaction, Description= Resources.General_Notifaction },
                //new NotificationPrivilage(){Id=3123, ParentId= 312, Name= Resources.General_SendMassage, Description= Resources.General_SendMassage },
        //تحويل المشروع من الأرشيف الي جاري
            new NotificationPrivilage(){Id=313, ParentId= 3, Name="تحويل المشروع من الأرشيف الي جاري", Description= "تحويل المشروع من الأرشيف الي جاري" },
                new NotificationPrivilage(){Id=3131, ParentId= 313, Name= Resources.General_SendMail, Description= Resources.General_SendMail },
                new NotificationPrivilage(){Id=3132, ParentId= 313, Name= Resources.General_Notifaction, Description= Resources.General_Notifaction },
                new NotificationPrivilage(){Id=3133, ParentId= 313, Name= Resources.General_SendMassage, Description= Resources.General_SendMassage },
        //بدأ مرحلة مشروع
            new NotificationPrivilage(){Id=314, ParentId= 3, Name="بدأ مرحلة مشروع", Description= "بدأ مرحلة مشروع" },
                new NotificationPrivilage(){Id=3141, ParentId= 314, Name= Resources.General_SendMail, Description= Resources.General_SendMail },
                new NotificationPrivilage(){Id=3142, ParentId= 314, Name= Resources.General_Notifaction, Description= Resources.General_Notifaction },
                //new NotificationPrivilage(){Id=3143, ParentId= 314, Name= Resources.General_SendMassage, Description= Resources.General_SendMassage },
        //انتهاء مرحلة مشروع 
            new NotificationPrivilage(){Id=315, ParentId= 3, Name="انتهاء مرحلة مشروع", Description= "انتهاء مرحلة مشروع" },
                new NotificationPrivilage(){Id=3151, ParentId= 315, Name= Resources.General_SendMail, Description= Resources.General_SendMail },
                new NotificationPrivilage(){Id=3152, ParentId= 315, Name= Resources.General_Notifaction, Description= Resources.General_Notifaction },
                new NotificationPrivilage(){Id=3153, ParentId= 315, Name= Resources.General_SendMassage, Description= Resources.General_SendMassage },
        //التكليف بمرحلة إشراف
            //new NotificationPrivilage(){Id=316, ParentId= 3, Name="التكليف بمرحلة إشراف", Description= "التكليف بمرحلة إشراف" },
            //    new NotificationPrivilage(){Id=3161, ParentId= 316, Name= Resources.General_SendMail, Description= Resources.General_SendMail },
            //    new NotificationPrivilage(){Id=3162, ParentId= 316, Name= Resources.General_Notifaction, Description= Resources.General_Notifaction },
            //    new NotificationPrivilage(){Id=3163, ParentId= 316, Name= Resources.General_SendMassage, Description= Resources.General_SendMassage },
        //إجراء عملية تحقق من الوثائق ناجحة
            new NotificationPrivilage(){Id=317, ParentId= 3, Name="إجراء عملية تحقق من الوثائق ناجحة", Description= "إجراء عملية تحقق من الوثائق ناجحة" },
                new NotificationPrivilage(){Id=3171, ParentId= 317, Name= Resources.General_SendMail, Description= Resources.General_SendMail },
                new NotificationPrivilage(){Id=3172, ParentId= 317, Name= Resources.General_Notifaction, Description= Resources.General_Notifaction },
                new NotificationPrivilage(){Id=3173, ParentId= 317, Name= Resources.General_SendMassage, Description= Resources.General_SendMassage },
        //التكليف بطلعة إشراف
            new NotificationPrivilage(){Id=318, ParentId= 3, Name="التكليف بطلعة إشراف", Description= "التكليف بطلعة إشرافة" },
                new NotificationPrivilage(){Id=3181, ParentId= 318, Name= Resources.General_SendMail, Description= Resources.General_SendMail },
                new NotificationPrivilage(){Id=3182, ParentId= 318, Name= Resources.General_Notifaction, Description= Resources.General_Notifaction },
                new NotificationPrivilage(){Id=3183, ParentId= 318, Name= Resources.General_SendMassage, Description= Resources.General_SendMassage },

        //طلب تمديد مهمة
            new NotificationPrivilage(){Id=319, ParentId= 3, Name="طلب تمديد مهمة", Description= "طلب تمديد مهمة" },
                new NotificationPrivilage(){Id=3191, ParentId= 319, Name= Resources.General_SendMail, Description= Resources.General_SendMail },
                new NotificationPrivilage(){Id=3192, ParentId= 319, Name= Resources.General_Notifaction, Description= Resources.General_Notifaction },
                new NotificationPrivilage(){Id=3193, ParentId= 319, Name= Resources.General_SendMassage, Description= Resources.General_SendMassage },

        //طلب تحويل مهمة
            new NotificationPrivilage(){Id=320, ParentId= 3, Name="طلب تحويل مهمة", Description= "طلب تحويل مهمة" },
                new NotificationPrivilage(){Id=3201, ParentId= 320, Name= Resources.General_SendMail, Description= Resources.General_SendMail },
                new NotificationPrivilage(){Id=3202, ParentId= 320, Name= Resources.General_Notifaction, Description= Resources.General_Notifaction },
                new NotificationPrivilage(){Id=3203, ParentId= 320, Name= Resources.General_SendMassage, Description= Resources.General_SendMassage },

         // تحويل مدير مشروع
            new NotificationPrivilage(){Id=321, ParentId= 3, Name=" تحويل مدير مشروع", Description= " تحويل مدير مشروع" },
                new NotificationPrivilage(){Id=3211, ParentId= 321, Name= Resources.General_SendMail, Description= Resources.General_SendMail },
                new NotificationPrivilage(){Id=3212, ParentId= 321, Name= Resources.General_Notifaction, Description= Resources.General_Notifaction },
                new NotificationPrivilage(){Id=3213, ParentId= 321, Name= Resources.General_SendMassage, Description= Resources.General_SendMassage },

            // تحويل مهمة سير
            new NotificationPrivilage(){Id=322, ParentId= 3, Name="تحويل مهمة سير", Description= "تحويل مهمة سير" },
                new NotificationPrivilage(){Id=3221, ParentId= 322, Name= Resources.General_SendMail, Description= Resources.General_SendMail },
                new NotificationPrivilage(){Id=3222, ParentId= 322, Name= Resources.General_Notifaction, Description= Resources.General_Notifaction },
                new NotificationPrivilage(){Id=3223, ParentId= 322, Name= Resources.General_SendMassage, Description= Resources.General_SendMassage },
               //انهاء مشروع
             new NotificationPrivilage(){Id=323, ParentId= 3, Name="إنهاء مشروع", Description= "إنهاء مشروع" },
                new NotificationPrivilage(){Id=3231, ParentId= 323, Name= Resources.General_SendMail, Description= Resources.General_SendMail },
                new NotificationPrivilage(){Id=3232, ParentId= 323, Name= Resources.General_Notifaction, Description= Resources.General_Notifaction },
                new NotificationPrivilage(){Id=3233, ParentId= 323, Name= Resources.General_SendMassage, Description= Resources.General_SendMassage },
                //اقتراب انتهاء المشروع
                 new NotificationPrivilage(){Id=324, ParentId= 3, Name="عند إقتراب انتهاء مدة المشروع", Description= "عند إقتراب انتهاء مدة المشروع" },
                new NotificationPrivilage(){Id=3241, ParentId= 324, Name= Resources.General_SendMail, Description= Resources.General_SendMail },
                new NotificationPrivilage(){Id=3242, ParentId= 324, Name= Resources.General_Notifaction, Description= Resources.General_Notifaction },
                new NotificationPrivilage(){Id=3243, ParentId= 324, Name= Resources.General_SendMassage, Description= Resources.General_SendMassage },
                //               //اقتراب انتهاء المهمة
                // new NotificationPrivilage(){Id=329, ParentId= 3, Name="عند إقتراب انتهاء مدة المهمة", Description= "عند إقتراب انتهاء مدة المهمة" },
                //new NotificationPrivilage(){Id=3291, ParentId= 329, Name= Resources.General_SendMail, Description= Resources.General_SendMail },
                //new NotificationPrivilage(){Id=3292, ParentId= 329, Name= Resources.General_Notifaction, Description= Resources.General_Notifaction },
                //new NotificationPrivilage(){Id=3293, ParentId= 329, Name= Resources.General_SendMassage, Description= Resources.General_SendMassage },
               

                        //إصدار فاتورة من زر(إنشاء مشروع)
             new NotificationPrivilage(){Id=325, ParentId= 3, Name="إصدار فاتورة من زر(إنشاء مشروع)", Description= "إصدار فاتورة من زر(إنشاء مشروع)" },
                new NotificationPrivilage(){Id=3251, ParentId= 325, Name= Resources.General_SendMail, Description= Resources.General_SendMail },
                new NotificationPrivilage(){Id=3252, ParentId= 325, Name= Resources.General_Notifaction, Description= Resources.General_Notifaction },
                new NotificationPrivilage(){Id=3253, ParentId= 325, Name= Resources.General_SendMassage, Description= Resources.General_SendMassage },


            new NotificationPrivilage(){Id=326, ParentId= 3, Name="تذكير عرض سعر", Description= "تذكير عرض سعر" },
                new NotificationPrivilage(){Id=3261, ParentId= 326, Name= Resources.General_SendMail, Description= Resources.General_SendMail },
                new NotificationPrivilage(){Id=3262, ParentId= 326, Name= Resources.General_Notifaction, Description= Resources.General_Notifaction },
                new NotificationPrivilage(){Id=3263, ParentId= 326, Name= Resources.General_SendMassage, Description= Resources.General_SendMassage },




             new NotificationPrivilage(){Id=327, ParentId= 3, Name="اشعار عند انتهاء مهمة", Description= "اشعار عند انتهاء مهمة" },
                new NotificationPrivilage(){Id=3271, ParentId= 327, Name= Resources.General_SendMail, Description= Resources.General_SendMail },
                new NotificationPrivilage(){Id=3272, ParentId= 327, Name= Resources.General_Notifaction, Description= Resources.General_Notifaction },



                    new NotificationPrivilage(){Id=328, ParentId= 3, Name="عند اكتمال هدف استراتيجي للمشروع", Description= "عند اكتمال هدف استراتيجي للمشروع" },
                new NotificationPrivilage(){Id=3281, ParentId= 328, Name= Resources.General_SendMail, Description= Resources.General_SendMail },
                new NotificationPrivilage(){Id=3282, ParentId= 328, Name= Resources.General_Notifaction, Description= Resources.General_Notifaction },
                    new NotificationPrivilage(){Id=3283, ParentId= 328, Name= Resources.General_SendMassage, Description= Resources.General_SendMassage },
            #endregion           
        #region Accounts
           new NotificationPrivilage(){Id=4, ParentId= null, Name=Resources.General_Accounting, Description= Resources.General_Accounting },
                new NotificationPrivilage(){Id=42, ParentId= 4, Name="تذكير استحقاق فاتورة", Description= "تذكير استحقاق فاتورة" },
                new NotificationPrivilage(){Id=421, ParentId= 42, Name= Resources.General_SendMail, Description= Resources.General_SendMail },
                new NotificationPrivilage(){Id=422, ParentId= 42, Name= Resources.General_Notifaction, Description= Resources.General_Notifaction },
                //new NotificationPrivilage(){Id=423, ParentId= 42, Name= Resources.General_SendMassage, Description= Resources.General_SendMassage },
        #endregion

        };
    }
    public class NotificationPrivilage
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}