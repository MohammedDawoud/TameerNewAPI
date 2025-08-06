using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models.Enums
{
    public enum Beneficiary_type
    {
       
            اختر = 0,
            مستخدمين = 1,
            مدير_المشروع = 2,
            مدير_المشروع_و_المحاسب = 3,
            مدير_المشروع_و_المشاركين = 4,
            مدير_المشروع_و_المشاركين_و_كل_من_لديه_مهمة = 5,
            مشاركين = 6,


            المدير_المباشر = 7,
            المدير_المباشر_و_الموظف = 8,
            الموظف_و_المدير_المباشر_و_المحاسب = 9,
            الموظف =10,

            المستخدم_المسؤول_عن_عرض_السعر = 11,
            مقدم_الطلب = 12,
        
        }

    
}
