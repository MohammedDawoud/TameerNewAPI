using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaamerP.Service.LocalResources;

namespace TaamerProject.API.Helper
{
    public static class Privileges
    {
        public static List<Privilege> PrivilegesList = new List<Privilege>
        {


                    ///////////// home ////////////////////////////////////////////////////////////////////////////////////////////////
            new Privilege { Id = 15 , ParentId = null , Name = Resources.General_Home, Description = Resources.General_Home },
                   new Privilege { Id = 15010 , ParentId = 15 , Name =  Resources.General_Administration , Description = Resources.General_Administration},
                     new Privilege { Id = 1510 , ParentId = 15010 , Name =  "متابعة التشغيل والمهام -Daily Operational Tasks", Description = "متابعة التشغيل والمهام -Daily Operational Tasks"},
                            new Privilege { Id = 151011 , ParentId = 1510 , Name =  "Project timeline", Description = "Project timeline" },
                            new Privilege { Id = 151012 , ParentId = 1510 , Name =  "Timeline of tasks", Description = "Timeline of tasks" },
                            new Privilege { Id = 151013 , ParentId = 1510 , Name =  "Manegment tasks", Description = "Manegment tasks" },
                             
                   new Privilege { Id = 1511 , ParentId = 15010 , Name =  "Project Flow-Up", Description = "Project Flow-Up" },
                        new Privilege { Id = 151111 , ParentId = 1511 , Name =  "Pending", Description = "Pending" },
                        new Privilege { Id = 151112 , ParentId = 1511 , Name =  "Natural", Description = "Natural" },
                        new Privilege { Id = 151113 , ParentId = 1511 , Name =  "Stopped", Description = "Stopped" },
                        new Privilege { Id = 151114 , ParentId = 1511 , Name =  "Late", Description = "Late" },
                        new Privilege { Id = 151115 , ParentId = 1511 , Name =  "Without Contracts", Description = "Without Contracts" },
                        new Privilege { Id = 151116 , ParentId = 1511 , Name =  "Uplouded", Description = "Uplouded" },
                        new Privilege { Id = 151117 , ParentId = 1511 , Name =  "staff projects", Description = "staff projects" },
                        new Privilege { Id = 151118 , ParentId = 1511 , Name =  "Project revenues and expenses", Description = "Project revenues and expenses" },
                        new Privilege { Id = 151119 , ParentId = 1511 , Name =  "Oversight flights", Description = "Oversight flights" },
                        new Privilege { Id = 151120 , ParentId = 1511 , Name =  "Project contracts", Description = "Project contracts" },
                        new Privilege { Id = 151121 , ParentId = 1511 , Name =  "Tasks by project", Description = "Tasks by project" },
                        new Privilege { Id = 151122 , ParentId = 1511 , Name =  "tasks for employees", Description = "tasks for employees" },

                  new Privilege { Id = 1512 , ParentId = 15010 , Name =  "Financial Flow-Up", Description = "Financial Flow-Up" },
                       new Privilege { Id = 151211 , ParentId = 1512 , Name =  "TheIncome", Description = "TheIncome" },
                       new Privilege { Id = 151212 , ParentId = 1512 , Name =  "TheExpenses", Description = "TheExpenses" },
                       new Privilege { Id = 151213 , ParentId = 1512 , Name =  "TheBox", Description = "TheBox" },
                       new Privilege { Id = 151214 , ParentId = 1512 , Name =  "TheBank", Description = "TheBank" },
                       new Privilege { Id = 151215 , ParentId = 1512 , Name =  "Sale invoices", Description = "Sale invoices" },
                       new Privilege { Id = 151216 , ParentId = 1512 , Name =  "Unposted sales invoices", Description = "Unposted sales invoices" },
                       new Privilege { Id = 151217 , ParentId = 1512 , Name =  "Bills without a project", Description = "Bills without a project" },
                       new Privilege { Id = 151218 , ParentId = 1512 , Name =  "Purchase invoices", Description = "Purchase invoices" },
                       new Privilege { Id = 151219 , ParentId = 1512 , Name =  "Unposted purchase invoices", Description = "Unposted purchase invoices" },
                       new Privilege { Id = 151220 , ParentId = 1512 , Name =  "Non-post receipt vouchers", Description = "Non-post receipt vouchers" },
                       new Privilege { Id = 151221 , ParentId = 1512 , Name =  "Non-post bonds", Description = "Non-post bonds" },


                    new Privilege { Id = 1513 , ParentId = 15010 , Name =  "Administrative Flow", Description = "Administrative Flow" },
                        new Privilege { Id = 151311 , ParentId = 1513 , Name =  "Attendence", Description = "Attendence" },
                        new Privilege { Id = 151312 , ParentId = 1513 , Name =  "TheAbsence", Description = "TheAbsence" },
                        new Privilege { Id = 151313 , ParentId = 1513 , Name =  "Latecomers", Description = "Latecomers" },
                        new Privilege { Id = 151314 , ParentId = 1513 , Name =  "Those who ask for permission", Description = "Those who ask for permission" },
                        new Privilege { Id = 151315 , ParentId = 1513 , Name =  "Holiday requests", Description = "Holiday requests" },
                        new Privilege { Id = 151316 , ParentId = 1513 , Name =  "Loan requests", Description = "Loan requests" },
                        new Privilege { Id = 151317 , ParentId = 1513 , Name =  "Identities are about to expire", Description = "Identities are about to expire" },
                        new Privilege { Id = 151318 , ParentId = 1513 , Name =  "Expired identities", Description = "Expired identities" },
                        new Privilege { Id = 151319 , ParentId = 1513 , Name =  "Documents nearing completion", Description = "Documents nearing completion" },
                        new Privilege { Id = 151320 , ParentId = 1513 , Name =  "expired documents", Description = "expired documents" },
                        new Privilege { Id = 151321 , ParentId = 1513 , Name =  "Employee contracts are about to expire", Description = "Employee contracts are about to expire" },
                        new Privilege { Id = 151322 , ParentId = 1513 , Name =  "Expired employee contracts", Description = "Expired employee contracts" },
                        new Privilege { Id = 151323 , ParentId = 1513 , Name =  "Employees without contracts", Description = "Employees without contracts" },
                        new Privilege { Id = 151324 , ParentId = 1513 , Name =  "الاعتماد والموافقة علي الاذن  ", Description = "الاعتماد والموافقة علي الاذن  " },


                         //new Privilege { Id = 171113 , ParentId = 1511 , Name =  Resources.General_AdjustTheProceedings, Description = Resources.General_AdjustTheProceedings },


                            //new Privilege { Id = 151011 , ParentId = 1510 , Name =  Resources.General_projectsNumber, Description = Resources.General_projectsNumber },
                            // new Privilege { Id = 151010 , ParentId = 1510 , Name =  Resources.General_CustomersNumbers , Description = Resources.General_CustomersNumbers},
                            //  new Privilege { Id = 151021 , ParentId = 1510 , Name = Resources.Total_Revenue , Description = Resources.Total_Revenue},
                            //    new Privilege { Id = 151022 , ParentId = 1510 , Name = Resources.Total_Cost , Description = Resources.Total_Cost},
                            //     //new Privilege { Id = 151023 , ParentId = 1510 , Name = Resources.Total_Cost , Description = Resources.Total_Cost},
                            //      new Privilege { Id = 151014 , ParentId = 1510 , Name = "مخطط زمني للمشاريع" , Description = "مخطط زمني للمشاريع"},
                            //        new Privilege { Id = 151018 , ParentId = 1510 , Name =  "مخطط زمني للمهام", Description = "مخطط زمني للمهام" },
                            //        new Privilege { Id = 151023 , ParentId = 1510 , Name = Resources.Sys_NotifationSystem , Description = Resources.Sys_NotifationSystem},
  //new Privilege { Id = 151024 , ParentId = 151023 , Name = Resources.General_Staysarealmostover , Description = Resources.General_Staysarealmostover},
  //  new Privilege { Id = 151025 , ParentId = 151023 , Name = Resources.Gerneral_StaysOver , Description = Resources.Gerneral_StaysOver},
  //    new Privilege { Id = 151026 , ParentId = 151023 , Name = Resources.Gerneral_Documentsarenearingcompletion , Description = Resources.Gerneral_Documentsarenearingcompletion},
  //      new Privilege { Id = 151027 , ParentId = 151023 , Name = Resources.Gerneral_Documentsarecompletion , Description = Resources.Gerneral_Documentsarecompletion},
  //        new Privilege { Id = 151028 , ParentId = 151023 , Name = Resources.Gerneral_EmployeeContract , Description = Resources.Gerneral_EmployeeContract},
  //          new Privilege { Id = 151029 , ParentId = 151023 , Name = Resources.Gerneral_Vacations , Description = Resources.Gerneral_Vacations},
  //            new Privilege { Id = 151030 , ParentId = 151023 , Name = Resources.Gerneral_Loans , Description = Resources.Gerneral_Loans},
  //              new Privilege { Id = 151031 , ParentId = 151023 , Name = Resources.General_InvoicesAndServices , Description = Resources.General_InvoicesAndServices},

  //                                                  new Privilege { Id = 151032 , ParentId = 1510 , Name = "نموذج طلب اجازه" , Description =  "نموذج طلب اجازه"},
  //                                                  new Privilege { Id = 151033 , ParentId = 1510 , Name = "نموذج طلب سلفة" , Description = "نموذج طلب سلفة"},

  //               new Privilege { Id = 15011 , ParentId = 15 , Name =  Resources.General_User , Description = Resources.General_User},
  //                new Privilege { Id = 15012 , ParentId = 15011 , Name =  Resources.General_HomeDashBoard_U , Description = Resources.General_HomeDashBoard_U},

  //               new Privilege { Id = 15013 , ParentId = 15012 , Name =  Resources.General_Notifications , Description = Resources.General_Notifications},
  //               new Privilege { Id = 15014 , ParentId = 15012 , Name =  Resources.General_Alerts , Description = Resources.General_Alerts},
  //               new Privilege { Id = 15015 , ParentId = 15012 , Name =  Resources.General_Mytasks , Description = Resources.General_Mytasks},
  //               new Privilege { Id = 15016 , ParentId = 15012 , Name =  Resources.General_MyProjects , Description = Resources.General_MyProjects},
  //               new Privilege { Id = 15017 , ParentId = 15012 , Name =  Resources.General_Mymessages , Description = Resources.General_Mymessages},
  //               new Privilege { Id = 15018 , ParentId = 15012 , Name =  Resources.Myvacationbalance , Description = Resources.Myvacationbalance},

  //               new Privilege { Id = 15019 , ParentId = 15012 , Name =  Resources.General_Newtasks , Description = Resources.General_Newtasks},
  //               new Privilege { Id = 15020 , ParentId = 15012 , Name =  Resources.General_Latetasks , Description = Resources.General_Latetasks},
  //               new Privilege { Id = 15021 , ParentId = 15012 , Name =  Resources.Pro_LateWorkOrder , Description = Resources.Pro_LateWorkOrder},

  //               new Privilege { Id = 15022 , ParentId = 15012 , Name =  Resources.User_MyReports , Description = Resources.User_MyReports},
  //               new Privilege { Id = 15023 , ParentId = 15012 , Name =  Resources.User_OverallRatio , Description = Resources.User_OverallRatio},
                
                 //------------------------------------------
                 
                 
                 // new Privilege { Id = 15024 , ParentId = 15012 , Name =  Resources.General_Latetasks , Description = Resources.General_Latetasks},
            



                            //new Privilege { Id = 151012 , ParentId = 1510 , Name =  Resources.General_usersnumber , Description = Resources.General_usersnumber},
                          //  new Privilege { Id = 151013 , ParentId = 1510 , Name =  Resources.General_VouchersNumber , Description = Resources.General_VouchersNumber},
                           
                            //new Privilege { Id = 151015 , ParentId = 1510 , Name = Resources.General_CustomerStatistics , Description = Resources.General_CustomerStatistics},
                          //  new Privilege { Id = 151016 , ParentId = 1510 , Name =  Resources.General_SystemStatistics, Description = Resources.General_SystemStatistics},
                          //  new Privilege { Id = 151017 , ParentId = 1510 , Name =  Resources.General_RevenuesAndExpenses, Description = Resources.General_RevenuesAndExpenses},
                          
                          //  new Privilege { Id = 151019 , ParentId = 1510 , Name =  Resources.General_Taskextensionrequests , Description = Resources.General_Taskextensionrequests},
                                       //   new Privilege { Id = 15101910, ParentId = 151019 , Name =  Resources.General_pending , Description = Resources.General_pending},
                                       //   new Privilege { Id = 15101911, ParentId = 151019 , Name = Resources.General_Approved , Description = Resources.General_Approved},
                                       //   new Privilege { Id = 15101912, ParentId = 151019 , Name =  Resources.General_Reject , Description = Resources.General_Reject},
                          //  new Privilege { Id = 151020 , ParentId = 1510 , Name = Resources.Acc_Acceptrejectextensionrequests , Description = Resources.Acc_Acceptrejectextensionrequests},
               //   new Privilege { Id = 1511 , ParentId = 15 , Name =  Resources.General_Listofprojects , Description = Resources.General_Listofprojects},
                    
                       /////////////// customers////////////////////////////////////////////////////////////////
             new Privilege { Id = 12 , ParentId = null , Name = Resources.General_Secrtary, Description = Resources.General_Secrtary },

                     new Privilege { Id = 1210 , ParentId = 12 , Name =  Resources.Pro_Customers, Description = Resources.Pro_Customers },
                       new Privilege { Id = 121001 , ParentId = 1210 , Name =  Resources.Pro_AddNewCustomer , Description = Resources.Pro_AddNewCustomer},
                                     new Privilege { Id = 121002 , ParentId = 1210 , Name =  @Resources.General_Delete , Description = Resources.General_deleteCustomer },
                                     new Privilege { Id = 121003 , ParentId = 1210 , Name =  @Resources.General_Edit  , Description =Resources.General_EditCustomer},
                                      new Privilege { Id = 121004 , ParentId = 1210 , Name =  Resources.General_Search  , Description =Resources.General_SearchCustomer},
                                       new Privilege { Id = 121005 , ParentId = 1210 , Name =  Resources.Pro_Printcustomerdata , Description = Resources.Pro_Printcustomerdata},
                                        new Privilege { Id = 121006 , ParentId = 1210 , Name =  Resources.General_SendEmail1 , Description = Resources.General_SendEmail1},
                                        new Privilege { Id = 121007, ParentId = 1210 , Name =  Resources.General_Sms , Description = Resources.General_Sms},
                                        new Privilege { Id = 121008 , ParentId = 1210 , Name =  Resources.General_Show , Description = Resources.General_Show},
                                        new Privilege { Id = 121009 , ParentId = 1210 , Name =  Resources.ExportToExcel , Description = Resources.ExportToExcel},



                       new Privilege { Id = 1211 , ParentId = 12 , Name =  Resources.General_Operationscustomers1, Description = Resources.General_Operationscustomers1 },
                         new Privilege { Id = 121101, ParentId = 1211 , Name =  Resources.Pro_EmailMessages , Description = Resources.Pro_EmailMessages},
                                        new Privilege { Id = 121102 , ParentId = 1211 , Name =  Resources.General_Delete , Description = Resources.General_Delete},


                         //new Privilege { Id = 1212 , ParentId = 12 , Name =  Resources.General_ArchiveProjects, Description = Resources.General_ArchiveProjects },
                         //                         new Privilege { Id = 1212101 , ParentId = 1212 , Name =  Resources.Transferringtheprojecttoongoing, Description = Resources.Transferringtheprojecttoongoing },

                          new Privilege { Id = 1213 , ParentId = 12 , Name =  Resources.General_CustomerStatistics1, Description = Resources.General_CustomerStatistics1 },
                             new Privilege { Id = 121301 , ParentId = 1213 , Name =  Resources.General_AllCustomer , Description =  Resources.General_AllCustomer},
                            new Privilege { Id = 121302 , ParentId = 1213 , Name =  Resources.General_CustomersDuringPeriod , Description =  Resources.General_CustomersDuringPeriod},
                             new Privilege { Id = 121303 , ParentId = 1213 , Name =  Resources.Pro_Citizens , Description =  Resources.Pro_Citizens},
                             new Privilege { Id = 121304 , ParentId = 1213 , Name =  Resources.General_Institutionsandcompanies , Description =  Resources.General_Institutionsandcompanies},
                             new Privilege { Id = 121305 , ParentId = 1213 , Name =  Resources.General_Governmentalentities , Description =  Resources.General_Governmentalentities},
                             new Privilege { Id = 121306 , ParentId = 1213 , Name =  "كشف حساب عميل" , Description =  "كشف حساب عميل"},
                             new Privilege { Id = 121307 , ParentId = 1213 , Name =  "متابعة التحصيل" , Description =  "متابعة التحصيل"},

                             //new Privilege { Id = 121306 , ParentId = 1213 , Name =  Resources.Pro_PrivateCustomer , Description =  Resources.Pro_PrivateCustomer},



            //                           new Privilege { Id = 12101010 , ParentId = 121010 , Name =  Resources.Pro_AddNewCustomer , Description = Resources.Pro_AddNewCustomer},
            //                         new Privilege { Id = 12101011 , ParentId = 121010 , Name =  @Resources.General_Delete , Description = Resources.General_deleteCustomer },
            //                         new Privilege { Id = 12101012 , ParentId = 121010 , Name =  @Resources.General_Edit  , Description =Resources.General_EditCustomer},
            //              new Privilege { Id = 121010 , ParentId = 1210 , Name =  Resources.Pro_Customers , Description = Resources.Pro_Customers},
                                   
            //                         new Privilege { Id = 12101013 , ParentId = 121010 , Name =  Resources.Pro_Printcustomerdata , Description = Resources.Pro_Printcustomerdata},
            //                         new Privilege { Id = 12101014 , ParentId = 121010 , Name =  Resources.Pro_Sendemail, Description = Resources.Pro_Sendemail },
            //                         new Privilege { Id = 12101015 , ParentId = 121010 , Name =  Resources.Pro_PrivateCustomer , Description = Resources.Pro_PrivateCustomer},
            //                         new Privilege { Id = 12101016 , ParentId = 121010 , Name =  Resources.General_Search  , Description =Resources.General_SearchCustomer},
            //                         new Privilege { Id = 12101017 , ParentId = 121010 , Name =  Resources.Pro_SMS , Description = Resources.General_SMSCustomer},
            //              new Privilege { Id = 121011 , ParentId = 1210 , Name =  Resources.Pro_Messages , Description = Resources.Pro_Messages},
            //                         new Privilege { Id = 12101110 , ParentId = 121011 , Name =  Resources.Pro_EmailMessages, Description = Resources.Pro_EmailMessages},
            //                         new Privilege { Id = 12101111 , ParentId = 121011 , Name =  Resources.Pro_SMS, Description = Resources.Pro_SMS },
            //              new Privilege { Id = 121012 , ParentId = 1210 , Name =  Resources.Pro_Projects , Description = Resources.Pro_Projects},
            //                         new Privilege { Id = 12101210 , ParentId = 121012 , Name =  Resources.Pro_CurrentProjects , Description = Resources.Pro_CurrentProjects},
            //                         new Privilege { Id = 12101211 , ParentId = 121012 , Name =  Resources.Pro_ArchivedProjects , Description = Resources.Pro_ArchivedProjects},
            //              new Privilege { Id = 121013 , ParentId = 1210 , Name =  Resources.Pro_CutomerContract , Description = Resources.Pro_CutomerContract},
            //                         new Privilege { Id = 12101310 , ParentId = 121013 , Name =   Resources.Pro_CutomerContract , Description = Resources.Pro_CutomerContract},
            //                         new Privilege { Id = 12101311 , ParentId = 121013 , Name =  Resources.Pro_CustomerFiles , Description = Resources.Pro_CustomerFiles},
            //              new Privilege { Id = 121014 , ParentId = 1210 , Name =  Resources.Pro_FinancialDetailsOfCustomers , Description = Resources.Pro_FinancialDetailsOfCustomers},

            //new Privilege { Id = 1211 , ParentId = 12 , Name =  @Resources.General_ArchiveProject, Description = Resources.General_ArchiveProject},
                  

                  /////////// Communications /////////////////////////////////////////////////////////////////////////
            new Privilege { Id = 10 , ParentId = null , Name =  Resources.General_AdministrativeCommunications , Description = Resources.General_AdministrativeCommunications},
                   new Privilege { Id = 1010 , ParentId = 10 , Name =  @Resources.General_Outbox, Description = Resources.General_Outbox },
                           new Privilege { Id = 101010 , ParentId = 1010 , Name =   @Resources.General_Add , Description = Resources.Contac_AddOutBox},
                           new Privilege { Id = 101011 , ParentId = 1010 , Name =  @Resources.General_EditOutbox  , Description = Resources.General_EditOutbox},
                           new Privilege { Id = 101012 , ParentId = 1010 , Name =  @Resources.General_DeleteOutbox  , Description = Resources.General_DeleteOutbox},
                            new Privilege { Id = 101013 , ParentId = 1010 , Name =  @Resources.General_Show  , Description = Resources.General_Show},


                   new Privilege { Id = 1011 , ParentId = 10 , Name =  Resources.Contac_Inbox, Description = Resources.Contac_Inbox },
                           new Privilege { Id = 101110 , ParentId = 1011 , Name =   @Resources.General_Add , Description = Resources.Contac_AddInbox },
                           new Privilege { Id = 101111 , ParentId = 1011 , Name =  @Resources.General_EditInbox , Description = Resources.General_EditInbox },
                           new Privilege { Id = 101112 , ParentId = 1011 , Name =  @Resources.General_DeleteInbox , Description = Resources.General_DeleteInbox },
                           new Privilege { Id = 101113 , ParentId = 1011 , Name =  @Resources.General_Show  , Description = Resources.General_Show},



                            new Privilege { Id = 1012 , ParentId = 10 , Name =  Resources.General_Search, Description = Resources.General_Search },
                              new Privilege { Id = 101201 , ParentId = 1012 , Name =  Resources.General_OutInboxSearch, Description = Resources.General_OutInboxSearch },
                              new Privilege { Id = 10120101 , ParentId = 101201 , Name =  Resources.General_Show, Description = Resources.General_Show },
                               new Privilege { Id = 101201002 , ParentId = 101201 , Name =  Resources.General_Edit, Description = Resources.General_Edit },

                              new Privilege { Id = 101202 , ParentId = 1012 , Name =  Resources.General_FilesSearch, Description = Resources.General_FilesSearch },
                               new Privilege { Id = 10120201 , ParentId = 101202 , Name =  Resources.General_Show, Description = Resources.General_Show },
                                //new Privilege { Id = 10120202 , ParentId = 101202 , Name =  Resources.General_FilesSearch, Description = Resources.General_FilesSearch },


                   //new Privilege { Id = 1012 , ParentId = 10 , Name =  Resources.Contac_OutInboxFiles, Description = Resources.Contac_OutInboxFiles },
                   //  new Privilege { Id = 1013 , ParentId = 10 , Name =  Resources.Contac_OutInboxFiles, Description = Resources.Contac_OutInboxFiles },
                 

                           ///////////////////////////////////////////////////////////////////////////////////////
                         //new Privilege { Id = 18 , ParentId = null , Name = Resources.General_FollowUpDepartment , Description = Resources.General_FollowUpDepartment},
                         //   new Privilege { Id =  1810 , ParentId = 18 , Name =  Resources.General_ReceivingAndSending, Description = Resources.General_ReceivingAndSending },
                         //   new Privilege { Id =  1811 , ParentId = 18 , Name =  Resources.General_Search , Description = Resources.General_Search},
             ///////////////////////////// projects /////////////////////////////////////////////////////
            new Privilege { Id = 11 , ParentId = null , Name =  Resources.Pro_Projectmanagement, Description = Resources.Pro_Projectmanagement },
                    new Privilege { Id = 1110 , ParentId = 11 , Name =  Resources.Pro_MainScreen , Description = Resources.Pro_MainScreen},
                           new Privilege { Id = 111010 , ParentId = 1110 , Name = "إضافة مشروع جديد" , Description = "إضافة مشروع جديد"},
                           //new Privilege { Id = 111011 , ParentId = 1110 , Name = @Resources.General_Addagovernmentproject , Description = Resources.General_Addagovernmentproject},
                           //new Privilege { Id = 111012 , ParentId = 1110 , Name =  Resources.Pro_WorkOrderProject , Description = Resources.Pro_WorkOrderProject},
                           //new Privilege { Id = 111013 , ParentId = 1110 , Name =  Resources.Pro_Addrandomproject, Description = Resources.Pro_Addrandomproject },
                           new Privilege { Id = 111014 , ParentId = 1110 , Name =  Resources.General_Edit , Description = Resources.General_Edit},
                           new Privilege { Id = 111022 , ParentId = 1110 , Name =  Resources.General_FilesSearch , Description = Resources.General_FilesSearch},
                           new Privilege { Id = 111023 , ParentId = 1110 , Name =  Resources.Pro_DownloadFiles , Description = Resources.Pro_DownloadFiles},
                            new Privilege { Id = 111027 , ParentId = 1110 , Name =  "رفع ملف على مشروع" , Description =   "رفع ملف على مشروع"},

                           //new Privilege { Id = 111024 , ParentId = 1110 , Name =  Resources.Pro_Conversiontofollowing, Description = Resources.Pro_Conversiontofollowing },
                                            new Privilege { Id = 111025 , ParentId = 1110 , Name =  Resources.General_Show , Description = Resources.General_Show},

                           new Privilege { Id = 111026 , ParentId = 1110 , Name =  "انهاء مشروع" , Description =   "انهاء مشروع"},
                           new Privilege { Id = 111028 , ParentId = 1110 , Name =  "المتابع الذكي ", Description =   "المتابع الذكي"},


                     new Privilege { Id = 1111 , ParentId = 11 , Name =  Resources.General_EditProjectProgress , Description = Resources.General_EditProjectProgress},
                           new Privilege { Id = 111110 , ParentId = 1111 , Name =  Resources.General_MainPhases , Description = Resources.Contac_OutInboxFiles},
                                      new Privilege { Id = 11111010 , ParentId = 111110 , Name =   @Resources.General_Add , Description = Resources.General_AddMainPhases },
                                      new Privilege { Id = 11111011 , ParentId = 111110 , Name =  @Resources.General_Delete  , Description = Resources.General_DeleteMainPhases},
                                      new Privilege { Id = 11111012 , ParentId = 111110 , Name =  @Resources.General_Edit  , Description = Resources.General_EditMainPhases},
                           new Privilege { Id = 111111 , ParentId = 1111 , Name = Resources.General_SubPhases, Description = Resources.General_SubPhases },
                                      new Privilege { Id = 11111110 , ParentId = 111111 , Name =  @Resources.General_Add  , Description = Resources.General_AddSubPhases},
                                      new Privilege { Id = 11111111 , ParentId = 111111 , Name = @Resources.General_Delete  , Description = Resources.General_DeleteSubPhases},
                                      new Privilege { Id = 11111112 , ParentId = 111111 , Name =  @Resources.General_Edit  , Description = Resources.General_EditSubPhases},
                           new Privilege { Id = 111112 , ParentId = 1111 , Name = @Resources.General_Tasks , Description = Resources.General_Tasks},
                                      new Privilege { Id = 11111210 , ParentId = 111112 , Name =  @Resources.General_Add  , Description = Resources.General_AddTask},
                                      new Privilege { Id = 11111211 , ParentId = 111112 , Name =  @Resources.General_Delete  , Description = Resources.General_DeleteTask},
                                      new Privilege { Id = 11111212 , ParentId = 111112 , Name = @Resources.General_Edit , Description = Resources.General_EditTask },
                                      new Privilege { Id = 11111213 , ParentId = 111112 , Name = @Resources.Merge , Description = Resources.Merge },

                     new Privilege { Id = 1112 , ParentId = 11 , Name =  "بحث في مهام المشاريع" , Description =  "بحث في مهام المشاريع"},
                                      new Privilege { Id = 111210 , ParentId = 1112 , Name =  "تقرير جميع المهام", Description = "تقرير جميع المهام" },
                     new Privilege { Id = 1113 , ParentId = 11 , Name = Resources.General_TasksControlling , Description = Resources.General_TasksControlling},
                                      new Privilege { Id = 111310 , ParentId = 1113 , Name = Resources.General_AddTasks, Description = Resources.General_AddTasks},
                                      new Privilege { Id = 111311 , ParentId = 1113 , Name = Resources.PlusTimetask, Description = Resources.PlusTimetask},
                                      new Privilege { Id = 111312 , ParentId = 1113 , Name = Resources.Pro_PlayTaskAsk, Description = Resources.Pro_PlayTaskAsk},
                                      new Privilege { Id = 111313 , ParentId = 1113 , Name = Resources.General_DeleteTask, Description = Resources.General_DeleteTask},
                                      new Privilege { Id = 111314 , ParentId = 1113 , Name = Resources.exchangeTask, Description = Resources.exchangeTask},
                                      new Privilege { Id = 111315 , ParentId = 1113 , Name = "إنهاء المهمة", Description = "إنهاء المهمة"},
                                      new Privilege { Id = 111316 , ParentId = 1113 , Name = "ظهور المستخدم في فرع أخر لإسناد المهام إليه" , Description = "ظهور المستخدم في فرع أخر لإسناد المهام إليه"},

            new Privilege { Id = 1114 , ParentId = 11 , Name = Resources.Pro_Workorders1 , Description = Resources.Pro_Workorders1},
                                      new Privilege { Id = 111410 , ParentId = 1114 , Name = Resources.General_Show, Description = Resources.General_Show},
                                      new Privilege { Id = 111411 , ParentId = 1114 , Name = Resources.General_Add, Description = Resources.General_Add},
                                      new Privilege { Id = 111412 , ParentId = 1114 , Name = Resources.General_Edit, Description = Resources.General_Edit},
                                      new Privilege { Id = 111413 , ParentId = 1114 , Name = Resources.General_Delete, Description = Resources.General_Delete},
                     new Privilege { Id = 1115 , ParentId = 11 , Name =  Resources.General_AdjustTheProceedings , Description = Resources.General_AdjustTheProceedings},
                                      new Privilege { Id = 111510 , ParentId = 1115 , Name = Resources.General_Add, Description = Resources.General_Add  },

                                      new Privilege { Id = 111511 , ParentId = 1115 , Name =  Resources.General_Edit , Description = Resources.General_Edit},

                                      new Privilege { Id = 111512 , ParentId = 1115 , Name =  @Resources.General_Delete , Description = Resources.General_Delete},

                                      //new Privilege { Id = 111513 , ParentId = 1115 , Name = Resources.Acc_projectType , Description = Resources.Acc_projectType},

                                      //new Privilege { Id = 111514 , ParentId = 1115 , Name =  Resources.Acc_SubprojectType , Description = Resources.Acc_SubprojectType},

                           new Privilege { Id = 1116 , ParentId = 11 , Name = Resources.Pro_Models , Description = Resources.Pro_Models},
                                      new Privilege { Id = 111610 , ParentId = 1116 , Name =   @Resources.General_Add , Description = Resources.Pro_AddNewModel },
                                      new Privilege { Id = 111611 , ParentId = 1116 , Name =  @Resources.General_Delete  , Description = Resources.General_DeleteModel},
                                      new Privilege { Id = 111612 , ParentId = 1116 , Name =  @Resources.General_Edit , Description = Resources.General_EditModel },
                           //new Privilege { Id = 1116 , ParentId = 11 , Name =  Resources.General_SupervisionCard, Description = Resources.General_SupervisionCard },
                           //           new Privilege { Id = 111610 , ParentId = 1116 , Name =  @Resources.General_Delete , Description = "تعديل بطاقة اشراف"},
                          


                        new Privilege { Id = 1119 , ParentId = 11 , Name = "متابعة طلعات الاشراف", Description = "متابعة طلعات الاشراف"},
                                      //new Privilege { Id = 111910 , ParentId = 1119 , Name = Resources.General_Show, Description = Resources.General_Show},
                                      new Privilege { Id = 111911 , ParentId = 1119 , Name = Resources.General_Add, Description = Resources.General_Add},
                                      new Privilege { Id = 111912 , ParentId = 1119 , Name = Resources.General_Edit, Description = Resources.General_Edit},
                                      new Privilege { Id = 111913 , ParentId = 1119 , Name = Resources.General_Delete, Description = Resources.General_Delete},
                                       new Privilege { Id = 111914 , ParentId = 1119 , Name = "اتاحة طلعة", Description = "اتاحة طلعة"},


                        //new Privilege { Id = 1120 , ParentId = 11 , Name = Resources.Pro_ProjectStatus, Description = Resources.Pro_ProjectStatus},
                          new Privilege { Id = 1121 , ParentId = 11 , Name = "متابعة إيرادات ومصروفات المشاريع", Description = "متابعة إيرادات ومصروفات المشاريع"},


                           new Privilege { Id = 1117 , ParentId = 11 , Name = "التحكم في ايقاف وتشيغل المشروع" , Description = "التحكم في ايقاف وتشيغل المشروع"},
                           new Privilege { Id = 1118 , ParentId = 11 , Name = " admin  مشاريع" , Description = " admin  مشاريع"},
                           new Privilege { Id = 1125 , ParentId = 11 , Name = "بحث بجميع الفروع في التقارير" , Description = "بحث بجميع الفروع في التقارير"},

                           //new Privilege { Id = 1122 , ParentId = 11 , Name =  "إلغاء المهام وحذف المشروع" , Description = "إلغاء المهام وحذف المشروع"},
                           new Privilege { Id = 1123 , ParentId = 11 , Name =  "التحكم في بداية المشروع" , Description = "التحكم في بداية المشروع"},
                            new Privilege { Id = 1124 , ParentId = 11 , Name =  " استمارة المشروع " , Description = "استمارة المشروع"},

                           ///أرشيف المشاريع المحول من السكرتارية
                           new Privilege { Id = 1212 , ParentId = 11 , Name =  Resources.General_ArchiveProjects, Description = Resources.General_ArchiveProjects },
                               new Privilege { Id = 1212101 , ParentId = 1212 , Name =  Resources.Transferringtheprojecttoongoing, Description = Resources.Transferringtheprojecttoongoing },



                                  new Privilege { Id = 12131 , ParentId = 11 , Name =  "عرض سعر" , Description =  "عرض سعر"},
                       new Privilege { Id = 1213101 , ParentId = 12131 , Name =  Resources.General_Add , Description =  Resources.General_Add},
                        new Privilege { Id = 1213102 , ParentId = 12131 , Name =  Resources.General_Edit , Description =  Resources.General_Edit},
                         new Privilege { Id = 1213103 , ParentId = 12131 , Name =  Resources.General_Delete , Description =  Resources.General_Delete},
                         new Privilege { Id = 1213104 , ParentId = 12131 , Name =  "صلاحية اعتماد عرض السعر" , Description =  "صلاحية اعتماد عرض السعر" },
                          new Privilege { Id = 12132 , ParentId = 11 , Name = Resources.General_SearchFileProject , Description =  Resources.General_SearchFileProject},

                          //Project Reports
                           new Privilege { Id = 121321 , ParentId = 11 , Name = Resources.Project_reports , Description =  Resources.Project_reports},
                           new Privilege { Id = 1213211 , ParentId = 121321 , Name = "تقرير الاداء الشامل" , Description =   "تقرير الاداء الشامل"},
                           new Privilege { Id = 1213212 , ParentId = 121321 , Name = "مشاريع المستخدم" , Description =  "مشاريع المستخدم"},
                           new Privilege { Id = 1213213 , ParentId = 121321 , Name = "مهام المستخدم" , Description =  "مهام المستخدم"},
                           new Privilege { Id = 1213214 , ParentId = 121321 , Name = "مهام حسب المشروع" , Description =  "مهام حسب المشروع"},
                           new Privilege { Id = 1213215 , ParentId = 121321 , Name = "تكلفة المشروع" , Description =  "تكلفة المشروع"},
                           new Privilege { Id = 1213216 , ParentId = 121321 , Name = " تكلفة المهام" , Description =  " تكلفة المهام"},

                     new Privilege { Id = 121322 , ParentId = 11 , Name = "مخطط زمني المشاريع" , Description =  "مخطط زمني للمشروع"},
                            new Privilege { Id = 1213221 , ParentId = 121322 , Name = Resources.General_Add , Description =  Resources.General_Add},
                            new Privilege { Id = 1213222 , ParentId = 121322 , Name = Resources.General_Edit , Description =  Resources.General_Edit},
                            new Privilege { Id = 1213223 , ParentId = 121322 , Name = Resources.General_Delete , Description =  Resources.General_Delete},
                            new Privilege { Id = 1213224 , ParentId = 121322 , Name = "نسخ مخطط" , Description =  "نسخ مخطط"},
                            new Privilege { Id = 1213225 , ParentId = 121322 , Name = "تعديل علي مهام المخطط" , Description =  "تعديل علي مهام المخطط"},

                ///////////// employee ////////////////////////////////////////////////////////////////////////////////////////////////
            new Privilege { Id = 14 , ParentId = null , Name =  Resources.General_HumanResources , Description =  Resources.General_HumanResources},
                     new Privilege { Id = 1410 , ParentId = 14 , Name =  Resources.MP_StaffSearch , Description =  Resources.MP_StaffSearch},
                       new Privilege { Id = 141001 , ParentId = 1410 , Name =  Resources.General_Add , Description =  Resources.General_Add},
                        new Privilege { Id = 141002 , ParentId = 1410 , Name =  Resources.General_Edit , Description =  Resources.General_Edit},
                         new Privilege { Id = 141003 , ParentId = 1410 , Name =  Resources.General_Delete , Description =  Resources.General_Delete},
                           new Privilege { Id = 141004 , ParentId = 1410 , Name =  Resources.General_Print , Description =  Resources.General_Print},
                           new Privilege { Id = 141005 , ParentId = 1410 , Name =  "رصيد اجازات سابق" , Description =  "رصيد اجازات سابق"},
                            new Privilege { Id = 141006 , ParentId = 1410 , Name =  "تغيير الرقم الوظيفي" , Description =  "تغيير الرقم الوظيفي"},

                //#MP_Attendance_Departure########### 
                    new Privilege { Id = 1418 , ParentId = 14 , Name =  Resources.MP_Attendance_Departure , Description =  Resources.MP_Attendance_Departure},
                        new Privilege { Id = 141801 , ParentId = 1418 , Name =  Resources.Emp_Attendance , Description =  Resources.Emp_Attendance},
                        //new Privilege { Id = 14180101 , ParentId = 141801 , Name =  Resources.Pullfromthefingerprintdevice , Description =  Resources.Pullfromthefingerprintdevice},
                        //new Privilege { Id = 14180102 , ParentId = 141801 , Name =  Resources.General_Delete , Description =  Resources.General_Delete},

                        new Privilege { Id = 141803 , ParentId = 1418 , Name =  Resources.Maniual_Attend , Description =  Resources.Maniual_Attend},
                          new Privilege { Id = 14180303 , ParentId = 141803 , Name =  Resources.General_Show , Description = Resources.General_Show},
                          new Privilege { Id = 14180301 , ParentId = 141803 , Name =  Resources.General_Add , Description =  Resources.General_Add},
                          new Privilege { Id = 14180302 , ParentId = 141803 , Name =  Resources.General_Edit , Description =  Resources.General_Edit},
                          new Privilege { Id = 14180304 , ParentId = 141803 , Name =  Resources.Att_EditAttendence , Description =  Resources.Att_EditAttendence},


                      new Privilege { Id = 1411 , ParentId = 14 , Name =  Resources.MP_Vacations , Description =  Resources.MP_Vacations},
                        new Privilege { Id = 141101 , ParentId = 1411 , Name =  Resources.MH_StaffHolidays , Description =  Resources.MH_StaffHolidays},
                           new Privilege { Id = 14110101 , ParentId = 141101 , Name =  Resources.General_Accept , Description =  Resources.General_Accept},
                         new Privilege { Id = 14110102 , ParentId = 141101 , Name =  Resources.General_Reject , Description =  Resources.General_Reject},
                           new Privilege { Id = 14110103 , ParentId = 141101 , Name =  Resources.General_Delete , Description =  Resources.General_Delete},


                          new Privilege { Id = 141102 , ParentId = 1411 , Name =  Resources.MH_OfficialHolidays , Description =  Resources.MH_OfficialHolidays},
                            new Privilege { Id = 14110204 , ParentId = 141102 , Name =  Resources.General_Show , Description = Resources.General_Show},
                            new Privilege { Id = 14110201 , ParentId = 141102 , Name =  Resources.General_Add , Description =  Resources.General_Add},
                            new Privilege { Id = 14110202 , ParentId = 141102 , Name =  Resources.General_Edit , Description =  Resources.General_Edit},
                            new Privilege { Id = 14110203 , ParentId = 141102 , Name =  Resources.General_Delete , Description =  Resources.General_Delete},

                              new Privilege { Id = 141103 , ParentId = 1411 , Name =  "طلب اجازة بعذر" , Description =   "طلب اجازة بعذر" },



                          new Privilege { Id = 1421 , ParentId = 1411 , Name =  "طلب إذن" , Description =  "طلب إذن"},
                            new Privilege { Id = 142101 , ParentId = 1421 , Name =  Resources.General_Show , Description = Resources.General_Show},
                            new Privilege { Id = 142102 , ParentId = 1421 , Name =  Resources.General_Add , Description =  Resources.General_Add},
                            new Privilege { Id = 142104 , ParentId = 1421 , Name =  Resources.General_Delete , Description =  Resources.General_Delete},



                        new Privilege { Id = 1412 , ParentId = 14 , Name =  Resources.MP_Imprest , Description =  Resources.MP_Imprest},
                           new Privilege { Id = 141204 , ParentId = 1412 , Name =  Resources.General_Show , Description = Resources.General_Show},
                         new Privilege { Id = 141201 , ParentId = 1412 , Name =  Resources.General_Accept , Description =  Resources.General_Accept},
                         new Privilege { Id = 141202 , ParentId = 1412 , Name =  Resources.General_Reject , Description =  Resources.General_Reject},
                           new Privilege { Id = 141203 , ParentId = 1412 , Name =  Resources.General_Delete , Description =  Resources.General_Delete},





                          new Privilege { Id = 1413 , ParentId = 14 , Name =  Resources.MP_PayrollMarches , Description =  Resources.MP_PayrollMarches},
                            new Privilege { Id = 141302 , ParentId = 1413 , Name =  Resources.General_Show , Description = Resources.General_Show},
                             new Privilege { Id = 141301 , ParentId = 1413 , Name =  Resources.General_Print , Description =  Resources.General_Print},

                           //############################################################MP_AdvancedOfEmp##########################################################
                            new Privilege { Id = 1414 , ParentId = 14 , Name =  Resources.MP_AdvancedOfEmp , Description =  Resources.MP_AdvancedOfEmp},
                                     new Privilege { Id = 141401 , ParentId = 1414 , Name =  Resources.General_Add , Description =  Resources.General_Add},
                                      new Privilege { Id = 141402 , ParentId = 1414 , Name =  Resources.MC_Free , Description =  Resources.MC_Free},
                                       new Privilege { Id = 141403 , ParentId = 1414 , Name =  Resources.General_Edit , Description =  Resources.General_Edit},
                                        new Privilege { Id = 141404 , ParentId = 1414 , Name =  Resources.General_Delete , Description =  Resources.General_Delete},
                           //############################################################MP_AdvancedOfEmp########################################################## 

                           //############################################################MP_EmployeeEvaluation########################################################## 
                                //new Privilege { Id = 1415 , ParentId = 14 , Name =  Resources.MP_EmployeeEvaluation , Description =  Resources.MP_EmployeeEvaluation},
                                //new Privilege { Id = 141501 , ParentId = 1415 , Name =  Resources.General_Add , Description =  Resources.General_Add},
                                //new Privilege { Id = 141502 , ParentId = 1415 , Name =  Resources.General_Edit , Description =  Resources.General_Edit},
                                //new Privilege { Id = 141503 , ParentId = 1415 , Name =  Resources.General_Delete , Description =  Resources.General_Delete},
                          //############################################################MP_EmployeeEvaluation########################################################## 

 //############################################################General_CarMovement########################################################## 
                                new Privilege { Id = 1416 , ParentId = 14 , Name =  Resources.General_CarMovement , Description =  Resources.General_CarMovement},
                                new Privilege { Id = 141601 , ParentId = 1416 , Name =  Resources.General_Add , Description =  Resources.General_Add},
                                new Privilege { Id = 141602 , ParentId = 1416 , Name =  Resources.General_Edit , Description =  Resources.General_Edit},
                                new Privilege { Id = 141603 , ParentId = 1416 , Name =  Resources.General_Delete , Description =  Resources.General_Delete},

                                 new Privilege { Id = 141604 , ParentId = 1416 , Name =  Resources.General_ReportMovements , Description =  Resources.General_ReportMovements},
                                  new Privilege { Id = 141605 , ParentId = 1416 , Name =  Resources.ReportCollection , Description =  Resources.ReportCollection},


 //############################################################General_CarMovement########################################################## 
 //############################################################AttenceDevice########################################################## 
                                  //new Privilege { Id = 1417 , ParentId = 17 , Name =  Resources.AttenceDevice , Description =  Resources.AttenceDevice},
                                  //new Privilege { Id = 141701 , ParentId = 1417 , Name =  Resources.General_Add , Description =  Resources.General_Add},
                                  // new Privilege { Id = 141702 , ParentId = 1417 , Name =  Resources.Connect , Description =  Resources.Connect},
                                  //     new Privilege { Id = 141703 , ParentId = 1417 , Name =  Resources.General_Edit , Description =  Resources.General_Edit},
                                  //      new Privilege { Id = 141704 , ParentId = 1417 , Name =  Resources.General_Delete , Description =  Resources.General_Delete},

//############################################################AttenceDevice########################################################## 

//############################################################MP_Attendance_Departure########################################################## 
                                    //new Privilege { Id = 141803 , ParentId = 1418 , Name =  Resources.Emp_Attendance , Description =  Resources.Emp_Attendance},
                                  new Privilege { Id = 141802 , ParentId = 1418 , Name =  Resources.Statisticalreports , Description =  Resources.Statisticalreports},
                        new Privilege { Id = 14180201 , ParentId = 141802 , Name =  Resources.MF_AbsenteeStaff , Description =  Resources.MF_AbsenteeStaff},
                          new Privilege { Id = 14180202 , ParentId = 141802 , Name =  Resources.MF_AbsenteeStaffToday , Description =  Resources.MF_AbsenteeStaffToday},
                            new Privilege { Id = 14180203 , ParentId = 141802 , Name =  Resources.MF_LateStaff , Description =  Resources.MF_LateStaff},
                              new Privilege { Id = 14180204 , ParentId = 141802 , Name =  Resources.Latetoday , Description =  Resources.Latetoday},
                                new Privilege { Id = 14180205 , ParentId = 141802 , Name =  Resources.Earlyexit , Description =  Resources.Earlyexit},
                                  new Privilege { Id = 14180206 , ParentId = 141802 , Name =  Resources.Notloggedout , Description =  Resources.Notloggedout},
                                   new Privilege { Id = 14180207 , ParentId = 141802 , Name =  Resources.Attendance , Description =  Resources.Attendance},

                                    new Privilege { Id = 14180208 , ParentId = 141802 , Name =  "متابعة الحضور والانصراف من خلال التطبيق" , Description =   "متابعة الحضور والانصراف من خلال التطبيق"},
                                //############################################################MH_Staffcontracts##########################################################
                                     new Privilege { Id = 1419 , ParentId = 14 , Name =  Resources.MH_Staffcontracts , Description =  Resources.MH_Staffcontracts},
                                      new Privilege { Id = 141905 , ParentId = 1419 , Name =  Resources.General_Show , Description = Resources.General_Show},
                                      new Privilege { Id = 141901 , ParentId = 1419 , Name =  Resources.General_Add , Description =  Resources.General_Add},
                                      new Privilege { Id = 141902 , ParentId = 1419 , Name =  Resources.General_Print , Description =  Resources.General_Print},
                                       new Privilege { Id = 141903 , ParentId = 1419 , Name =  Resources.General_Edit , Description =  Resources.General_Edit},
                                        new Privilege { Id = 141904 , ParentId = 1419 , Name =  Resources.General_Delete , Description =  Resources.General_Delete},
                                        new Privilege { Id = 141906 , ParentId = 1419 , Name =  Resources.Con_StartWork , Description =  Resources.Con_StartWork},
                                        new Privilege { Id = 141907 , ParentId = 1419 , Name =  Resources.Emp_EndWork , Description =  Resources.Emp_EndWork},
                                        new Privilege { Id = 141908 , ParentId = 1419 , Name =  Resources.Con_EditStartWorkDate , Description =  Resources.Con_EditStartWorkDate},
                               //########################################################MH_Staffcontracts##############################################################
                        //########################################################Establish Payroll Marches##############################################################

                            
                          new Privilege { Id = 1420 , ParentId = 14 , Name = "إنشاء مسيرات الرواتب" , Description =  "إنشاء مسيرات الرواتب"},
                             new Privilege { Id = 142001 , ParentId = 1420 , Name =  Resources.General_Show , Description = Resources.General_Show},
                             new Privilege { Id = 142002 , ParentId = 1420 , Name =  Resources.General_Print , Description =  Resources.General_Print},
                             new Privilege { Id = 142003 , ParentId = 1420 , Name =  Resources.General_Edit , Description =  Resources.General_Edit},
                             new Privilege { Id = 142004 , ParentId = 1420 , Name =  Resources.payroll_Post , Description =  Resources.payroll_Post},

                             //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                             
                              new Privilege { Id = 170103 , ParentId = 14 , Name =  Resources.General_Alerts , Description = Resources.General_Alerts},
                              
                                      new Privilege { Id = 17010301 , ParentId = 170103 , Name =  Resources.General_Add , Description = Resources.General_Add},
                                       new Privilege { Id = 17010302 , ParentId = 170103 , Name =  Resources.ShowAlert , Description = Resources.ShowAlert},

                               new Privilege { Id = 142005 , ParentId = 14 , Name =  "ارشيف الموظفين" , Description = "ارشيف الموظفين" },
                               new Privilege { Id = 142006 , ParentId = 14 , Name =  "Admin HR" , Description = "Admin HR" },

              ///////////////// Accounting /////////////////////////////////////    
             new Privilege { Id = 13 , ParentId = null , Name =  Resources.General_Accounts , Description = Resources.General_Accounts},
              new Privilege { Id = 1310 , ParentId = 13 , Name =  Resources.MNAcc_Accountingbonds, Description = Resources.MNAcc_Accountingbonds },
                              new Privilege { Id = 131006 , ParentId = 1310 , Name =  "المسودات", Description = "المسودات" },

            new Privilege { Id = 131001 , ParentId = 1310 , Name =  Resources.MNAcc_Invoice, Description = Resources.MNAcc_Invoice },

                new Privilege { Id = 13100101 , ParentId = 131001 , Name =  Resources.General_Add , Description =  Resources.General_Add},
                                       new Privilege { Id = 13100102 , ParentId = 131001 , Name =  Resources.General_Edit , Description =  Resources.General_Edit},
                                        //new Privilege { Id = 13100103 , ParentId = 131001 , Name =  Resources.General_Delete , Description =  Resources.General_Delete},
                                         new Privilege { Id = 13100104 , ParentId = 131001 , Name =  Resources.General_Print , Description =  Resources.General_Print},
                                           new Privilege { Id = 13100105 , ParentId = 131001 , Name =  Resources.MNAcc_PostInvoice , Description =  Resources.MNAcc_PostInvoice},
                                              new Privilege { Id = 13100106 , ParentId = 131001 , Name =  Resources.PostBack , Description =  Resources.PostBack},
                                               new Privilege { Id = 13100107 , ParentId = 131001 , Name =  "حفظ وترحيل" , Description =  "حفظ وترحيل"},
                                                 new Privilege { Id = 13100108 , ParentId = 131001 , Name =  "إصدار فاتورة من مشروع" , Description =  "إصدار فاتورة من مشروع"},



                new Privilege { Id = 131002 , ParentId = 1310 , Name =  Resources.MNAcc_ReceiptVoucher, Description = Resources.MNAcc_ReceiptVoucher },
                  new Privilege { Id = 13100201 , ParentId = 131002 , Name =  Resources.General_Add , Description =  Resources.General_Add},
                                       new Privilege { Id = 13100202 , ParentId = 131002 , Name =  Resources.General_Edit , Description =  Resources.General_Edit},
                                        new Privilege { Id = 13100203 , ParentId = 131002 , Name =  Resources.General_Delete , Description =  Resources.General_Delete},
                                         new Privilege { Id = 13100204 , ParentId = 131002 , Name =  Resources.General_Print , Description =  Resources.General_Print},
                                           new Privilege { Id = 13100205 , ParentId = 131002 , Name =  "ترحيل السند" , Description =  "ترحيل السند"},
                                            new Privilege { Id = 13100206 , ParentId = 131002 , Name =  Resources.PostBack , Description =  Resources.PostBack},
                                                new Privilege { Id = 13100207 , ParentId = 131002 , Name =  "حفظ وترحيل" , Description =  "حفظ وترحيل"},


                 new Privilege { Id = 131003 , ParentId = 1310 , Name =  Resources.MNAcc_PaymentVoucher, Description = Resources.MNAcc_PaymentVoucher },
                   new Privilege { Id = 13100301 , ParentId = 131003 , Name =  Resources.General_Add , Description =  Resources.General_Add},
                                       new Privilege { Id = 13100302 , ParentId = 131003 , Name =  Resources.General_Edit , Description =  Resources.General_Edit},
                                        new Privilege { Id = 13100303 , ParentId = 131003 , Name =  Resources.General_Delete , Description =  Resources.General_Delete},
                                         new Privilege { Id = 13100304 , ParentId = 131003 , Name =  Resources.General_Print , Description =  Resources.General_Print},
                                           new Privilege { Id = 13100305 , ParentId = 131003 , Name =  "ترحيل السند" , Description =  "ترحيل السند"},
                                            new Privilege { Id = 13100306 , ParentId = 131003 , Name =  Resources.PostBack , Description =  Resources.PostBack},
                                              new Privilege { Id = 13100307 , ParentId = 131003 , Name =  "حفظ وترحيل" , Description =  "حفظ وترحيل"},


                 new Privilege { Id = 131004 , ParentId = 1310 , Name = "فاتورة مشتريات", Description = "فاتورة مشتريات" },
                   new Privilege { Id = 13100401 , ParentId = 131004 , Name =  Resources.General_Add , Description =  Resources.General_Add},
                                       new Privilege { Id = 13100402 , ParentId = 131004 , Name =  Resources.General_Edit , Description =  Resources.General_Edit},
                                        new Privilege { Id = 13100403 , ParentId = 131004 , Name =  Resources.General_Delete , Description =  Resources.General_Delete},
                                         new Privilege { Id = 13100404 , ParentId = 131004 , Name =  Resources.General_Print , Description =  Resources.General_Print},
                                           new Privilege { Id = 13100405 , ParentId = 131004 , Name =  "ترحيل السند" , Description =  "ترحيل السند"},
                                            new Privilege { Id = 13100406 , ParentId = 131004 , Name =  Resources.PostBack , Description =  Resources.PostBack},
                                              new Privilege { Id = 13100407 , ParentId = 131004 , Name =  "حفظ وترحيل" , Description =  "حفظ وترحيل"},

  new Privilege { Id = 131004001 , ParentId = 1310 , Name = "فاتورة مشتريات", Description = "فاتورة مشتريات" },
                   new Privilege { Id = 1310040011 , ParentId = 131004 , Name =  Resources.General_Add , Description =  Resources.General_Add},
                                       new Privilege { Id = 1310040012 , ParentId = 131004001 , Name =  Resources.General_Edit , Description =  Resources.General_Edit},
                                        new Privilege { Id = 1310040013 , ParentId = 131004001 , Name =  Resources.General_Delete , Description =  Resources.General_Delete},
                                         new Privilege { Id = 1310040014 , ParentId = 131004001 , Name =  Resources.General_Print , Description =  Resources.General_Print},
                                              new Privilege { Id = 1310040015 , ParentId = 131004001 , Name =  "حفظ" , Description =  "حفظ"},
                                              new Privilege { Id = 1310040016 , ParentId = 131004001 , Name =  "تحويل إلي الفاتورة" , Description =  "تحويل إلي فاتورة"},

                 new Privilege { Id = 131005 , ParentId = 1310 , Name = " العهد المالية للموظفين", Description = "العهد المالية للموظفين" },
                                    new Privilege { Id = 13100501 , ParentId = 131005 , Name =  "اضافة قيد" , Description =  "اضافة قيد"},

               new Privilege { Id = 1311 , ParentId = 13 , Name =  "القيود", Description = "القيود" },
                new Privilege { Id = 131101 , ParentId = 1311 , Name =  Resources.MNAcc_OpeningEntry, Description = Resources.MNAcc_OpeningEntry },
                  new Privilege { Id = 13110101 , ParentId = 131101 , Name =  Resources.General_Add , Description =  Resources.General_Add},
                                       new Privilege { Id = 13110102 , ParentId = 131101 , Name =  Resources.General_Edit , Description =  Resources.General_Edit},
                                        new Privilege { Id = 13110103 , ParentId = 131101 , Name =  Resources.General_Delete , Description =  Resources.General_Delete},
                                         new Privilege { Id = 13110104 , ParentId = 131101 , Name =  Resources.General_Print , Description =  Resources.General_Print},
                                           new Privilege { Id = 13110105 , ParentId = 131101 , Name =  "ترحيل قيد افتتاحي" , Description =  "ترحيل قيد افتتاحي"},
                                             new Privilege { Id = 13110106 , ParentId = 131101 , Name =  Resources.PostBack , Description =  Resources.PostBack},
                                                    new Privilege { Id = 13110107 , ParentId = 131101 , Name =  "حفظ وترحيل" , Description =  "حفظ وترحيل"},



                 new Privilege { Id = 131102 , ParentId = 1311 , Name =  Resources.MNAcc_JournalVoucher, Description = Resources.MNAcc_JournalVoucher },
                   new Privilege { Id = 13110201 , ParentId = 131102 , Name =  Resources.General_Add , Description =  Resources.General_Add},
                                       new Privilege { Id = 13110202 , ParentId = 131102 , Name =  Resources.General_Edit , Description =  Resources.General_Edit},
                                        new Privilege { Id = 13110203 , ParentId = 131102 , Name =  Resources.General_Delete , Description =  Resources.General_Delete},
                                         new Privilege { Id = 13110204 , ParentId = 131102 , Name =  Resources.General_Print , Description =  Resources.General_Print},
                                           new Privilege { Id = 13110205 , ParentId = 131102 , Name =  "ترحيل قيد يومية" , Description =  "ترحيل قيد يومية"},
                                             new Privilege { Id = 13110206 , ParentId = 131102 , Name =  Resources.PostBack , Description =  Resources.PostBack},
                                                   new Privilege { Id = 13110207 , ParentId = 131102 , Name =  "حفظ وترحيل" , Description =  "حفظ وترحيل"},


                 new Privilege { Id = 131103 , ParentId = 1311 , Name =  "قيد اقفال", Description = "قيد اقفال" },
                                   new Privilege { Id = 13110301 , ParentId = 131103 , Name =  Resources.General_Add , Description =  Resources.General_Add},
                                                       new Privilege { Id = 13110302 , ParentId = 131103 , Name =  Resources.General_Edit , Description =  Resources.General_Edit},
                                                        new Privilege { Id = 13110303 , ParentId = 131103 , Name =  Resources.General_Delete , Description =  Resources.General_Delete},
                                                         new Privilege { Id = 13110304 , ParentId = 131103 , Name =  Resources.General_Print , Description =  Resources.General_Print},
                                                           new Privilege { Id = 13110305 , ParentId = 131103 , Name =  "ترحيل قيد اقفال" , Description =  "ترحيل قيد اقفال"},
                                                             new Privilege { Id = 13110306 , ParentId = 131103 , Name =  Resources.PostBack , Description =  Resources.PostBack},


                new Privilege { Id = 1312 , ParentId = 13 , Name =  Resources.General_Officialdocuments, Description = Resources.General_Officialdocuments },
                 new Privilege { Id = 131201 , ParentId = 1312 , Name =  Resources.General_Add , Description =  Resources.General_Add},
                                       new Privilege { Id = 131202 , ParentId = 1312 , Name =  Resources.General_Edit , Description =  Resources.General_Edit},
                                        new Privilege { Id = 131203 , ParentId = 1312 , Name =  Resources.General_Delete , Description =  Resources.General_Delete},
                                         //new Privilege { Id = 131204 , ParentId = 1312 , Name =  Resources.General_Print , Description =  Resources.General_Print},
                                         


                 new Privilege { Id = 1313 , ParentId = 13 , Name =  Resources.General_InvoicesAndServices, Description = Resources.General_InvoicesAndServices },
                  new Privilege { Id = 131301 , ParentId = 1313 , Name =  Resources.General_Add , Description =  Resources.General_Add},
                                       new Privilege { Id = 131302 , ParentId = 1313 , Name =  Resources.General_Edit , Description =  Resources.General_Edit},
                                        new Privilege { Id = 131303 , ParentId = 1313 , Name =  Resources.General_Delete , Description =  Resources.General_Delete},
                                         //new Privilege { Id = 131304 , ParentId = 1313 , Name =  Resources.General_Print , Description =  Resources.General_Print},



                  new Privilege { Id = 1314 , ParentId = 13 , Name =  Resources.Acc_Checks, Description = Resources.Acc_Checks },
                   new Privilege { Id = 131401 , ParentId = 1314 , Name =  Resources.Acc_ExportsTabClickes, Description = Resources.Acc_ExportsTabClickes },
                    new Privilege { Id = 13140101 , ParentId = 131401 , Name =  Resources.General_Add , Description =  Resources.General_Add},
                                       new Privilege { Id = 13140102 , ParentId = 131401 , Name =  Resources.General_Edit , Description =  Resources.General_Edit},
                                        new Privilege { Id = 13140103 , ParentId = 131401 , Name =  Resources.General_Delete , Description =  Resources.General_Delete},
                                         //new Privilege { Id = 13140104 , ParentId = 131401 , Name =  Resources.General_Print , Description =  Resources.General_Print},

                    new Privilege { Id = 131402 , ParentId = 1314 , Name =  Resources.Acc_UndercollectionTabClickes, Description = Resources.Acc_UndercollectionTabClickes },

                     new Privilege { Id = 13140201 , ParentId = 131402 , Name =  Resources.General_Add , Description =  Resources.General_Add},
                                       new Privilege { Id = 13140202 , ParentId = 131402 , Name =  Resources.General_Edit , Description =  Resources.General_Edit},
                                        new Privilege { Id = 13140203 , ParentId = 131402 , Name =  Resources.General_Delete , Description =  Resources.General_Delete},
                                         //new Privilege { Id = 13140204 , ParentId = 131402 , Name =  Resources.General_Print , Description =  Resources.General_Print},




                   new Privilege { Id = 1315 , ParentId = 13 , Name =  Resources.Pro_CustomerContracts, Description = Resources.Pro_CustomerContracts },
                    new Privilege { Id = 131501 , ParentId = 1315 , Name =  Resources.General_Add , Description =  Resources.General_Add},

                                       new Privilege { Id = 131502 , ParentId = 1315 , Name =  Resources.General_Edit , Description =  Resources.General_Edit},
                                        new Privilege { Id = 131503 , ParentId = 1315 , Name =  Resources.General_Delete , Description =  Resources.General_Delete},
                                          new Privilege { Id = 131504 , ParentId = 1315 , Name =  Resources.General_ContractCancel , Description =  Resources.General_ContractCancel},
                                            new Privilege { Id = 131505 , ParentId = 1315 , Name =  Resources.MAcc_UploadContract , Description =  Resources.MAcc_UploadContract},
                                             new Privilege { Id = 131506 , ParentId = 1315 , Name =  Resources.General_Print , Description =  Resources.General_Print},
                                                new Privilege { Id = 131507 , ParentId = 1315 , Name =  "تعديل القيمة" , Description =  "تعديل القيمة"},
                                                  new Privilege { Id = 131508 , ParentId = 1315 , Name =  "ارفاق ملف" , Description =  "ارفاق ملف"},
                                                    new Privilege { Id = 131509 , ParentId = 1315 , Name =  "تحرير و تعديل بيانات دفعات العقد" , Description =  "تحرير و تعديل بيانات دفعات العقد"},



                    new Privilege { Id = 1316 , ParentId = 13 , Name =  Resources.Powersoffiscalyears, Description = Resources.Powersoffiscalyears },
                     new Privilege { Id = 1317 , ParentId = 13 , Name =  Resources.General_Gurantees, Description = Resources.General_Gurantees },

                      new Privilege { Id = 1318 , ParentId = 13 , Name =  Resources.General_AccountsReports, Description = Resources.General_AccountsReports },
                      new Privilege { Id = 131801 , ParentId = 1318 , Name =  Resources.General_AccountStatement, Description = Resources.General_AccountStatement },
                      new Privilege { Id = 131802 , ParentId = 1318 , Name =  Resources.General_DailyJournal, Description = Resources.General_DailyJournal },
                      new Privilege { Id = 131803 , ParentId = 1318 , Name =  Resources.General_TrialBalance, Description = Resources.General_TrialBalance },
                      new Privilege { Id = 131804 , ParentId = 1318 , Name =  Resources.General_IncomeState, Description = Resources.General_IncomeState },
                       new Privilege { Id = 1318041 , ParentId = 131804 , Name =  "طباعة قائمة الدخل" , Description = "طباعة قائمة الدخل"},

                      new Privilege { Id = 131805 , ParentId = 1318 , Name =  Resources.General_Publicbudget, Description = Resources.General_Publicbudget },
                      //new Privilege { Id = 131806 , ParentId = 1318 , Name =  Resources.MAcc_FinalReport, Description = Resources.MAcc_FinalReport },
                      new Privilege { Id = 131807 , ParentId = 1318 , Name =  Resources.General_ValueAdded, Description = Resources.General_ValueAdded },
                      new Privilege { Id = 131809 , ParentId = 1318 , Name =  "فرق الداين والمدين", Description = "فرق الداين والمدين" },

                      new Privilege { Id = 131808 , ParentId = 1318 , Name =  Resources.MAcc_OtherReport, Description = Resources.MAcc_OtherReport },


                       new Privilege { Id = 13180801 , ParentId = 131808 , Name =  Resources.Pro_FinancialDetailsOfCustomers, Description = Resources.Pro_FinancialDetailsOfCustomers },
                        new Privilege { Id = 13180802 , ParentId = 131808 , Name =  Resources.General_Centermovementscost, Description = Resources.General_Centermovementscost },
                         new Privilege { Id = 13180803 , ParentId = 131808 , Name =  Resources.General_CollectiveBondDisclosure, Description = Resources.General_CollectiveBondDisclosure },
                          new Privilege { Id = 13180804 , ParentId = 131808 , Name =  Resources.Projectmanagersrevenue, Description = Resources.Projectmanagersrevenue },
                           new Privilege { Id = 13180805 , ParentId = 131808 , Name =  "متابعة إيرادات العملاء", Description = "متابعة إيرادات العملاء" },
                          new Privilege { Id = 13180806 , ParentId = 131808 , Name =  "ايراد العميل (تفصيلي)", Description = "ايراد العميل (تفصيلي)" },
                          new Privilege { Id = 13180807 , ParentId = 131808 , Name =  "متابعة المصروفات", Description = "متابعة المصروفات" },
                          new Privilege { Id = 13180808 , ParentId = 131808 , Name =  "متابعة مراكز التكلفة", Description = "متابعة مراكز التكلفة" },
                          new Privilege { Id = 13180809 , ParentId = 131808 , Name =  "اشعارات الدائن والمدين", Description = "اشعارات الدائن والمدين" },
                          new Privilege { Id = 13180810 , ParentId = 131808 , Name =  "جدول أعمار الديون", Description = "جدول أعمار الديون" },
                           new Privilege { Id = 13180811 , ParentId = 131808 , Name =  "متابعة المندوبين", Description = "متابعة المندوبين" },
                            new Privilege { Id = 13180812 , ParentId = 131808 , Name =  "متابعة فواتير الهيئة", Description = "متابعة فواتير الهيئة" },



                       new Privilege { Id = 1319 , ParentId = 13 , Name =  Resources.General_Accounting1, Description = Resources.General_Accounting1 },

                        new Privilege { Id = 131901 , ParentId = 1319 , Name =  Resources.General_AccountInfo, Description = Resources.General_AccountInfo },
                         new Privilege { Id = 131902 , ParentId = 1319 , Name =  Resources.General_CostCenters, Description = Resources.General_CostCenters },
                          new Privilege { Id = 131903 , ParentId = 1319 , Name =  Resources.Service_Prices, Description = Resources.Service_Prices },
                           new Privilege { Id = 13190301 , ParentId = 131903 , Name =  Resources.General_Add , Description =  Resources.General_Add},
                                       new Privilege { Id = 13190302 , ParentId = 131903 , Name =  Resources.General_Edit , Description =  Resources.General_Edit},
                                        new Privilege { Id = 13190303 , ParentId = 131903 , Name =  Resources.General_Delete , Description =  Resources.General_Delete},


                           new Privilege { Id = 131904 , ParentId = 1319 , Name =  Resources.General_Fiscalyears, Description = Resources.General_Fiscalyears },
                           new Privilege { Id = 131905 , ParentId = 1319 , Name =  "بحث بجميع السنوات", Description = "بحث بجميع السنوات" },

                        new Privilege { Id = 1320 , ParentId = 13 , Name =  Resources.ConfirmSalary, Description = Resources.ConfirmSalary },
                           // new Privilege { Id = 132001 , ParentId = 1320 , Name =  Resources., Description = Resources.General_AccountInfo },
                      


                       //new Privilege { Id = 1310 , ParentId = 13 , Name =  Resources.General_Vouchers, Description = Resources.General_Vouchers },
                       //                 new Privilege { Id = 131011 , ParentId = 1310 , Name =  Resources.General_receiptVouchers , Description = Resources.General_receiptVouchers},
                       //                           new Privilege { Id = 13101110 , ParentId = 131011 , Name =  Resources.General_Add , Description = Resources.General_AddReceiptVoucher },
                       //                           new Privilege { Id = 13101111 , ParentId = 131011 , Name =  @Resources.General_Delete  , Description =Resources.General_DeletereceiptVouchers},
                       //                           new Privilege { Id = 13101112 , ParentId = 131011 , Name =  @Resources.General_Edit , Description = Resources.General_EditreceiptVouchers },
                       //                           new Privilege { Id = 13101113 , ParentId = 131011 , Name =  Resources.General_Post , Description =Resources.General_receiptVouchersPost },
                       //                  new Privilege { Id = 131012 , ParentId = 1310 , Name =  Resources.General_exchangeVoucher, Description = Resources.General_exchangeVoucher },
                       //                           new Privilege { Id = 13101210 , ParentId = 131012 , Name =   @Resources.General_Add , Description = Resources.General_NewexchangeVoucher},
                       //                           new Privilege { Id = 13101211 , ParentId = 131012 , Name =  @Resources.General_Delete, Description =Resources.General_DeleteexchangeVoucher },
                       //                           new Privilege { Id = 13101212 , ParentId = 131012 , Name = @Resources.General_Edit, Description = @Resources.General_EditexchangeVoucher },
                       //                           new Privilege { Id = 13101213 , ParentId = 131012 , Name =  Resources.General_Post , Description = Resources.General_exchangeVoucherPost },
                       //                  new Privilege { Id = 131013 , ParentId = 1310 , Name =  @Resources.General_dailyVoucher , Description = Resources.General_dailyVoucher},
                       //                           new Privilege { Id = 13101310 , ParentId = 131013 , Name =   @Resources.General_Add , Description = Resources.General_AdddailyVoucher },
                       //                           new Privilege { Id = 13101311 , ParentId = 131013 , Name =  @Resources.General_Delete , Description = Resources.General_DeletedailyVoucher },
                       //                           new Privilege { Id = 13101312 , ParentId = 131013 , Name = @Resources.General_Edit, Description = Resources.General_EditdailyVoucher  },
                       //                           new Privilege { Id = 13101313 , ParentId = 131013 , Name =  Resources.General_Post , Description = Resources.General_dailyVoucherPost },
                       //                  new Privilege { Id = 131014 , ParentId = 1310 , Name = Resources.General_openingVoucher , Description =  Resources.General_openingVoucher },
                       //                           new Privilege { Id = 13101410 , ParentId = 131014 , Name =   @Resources.General_Add , Description =Resources.General_AddopeningVoucher },
                       //                           new Privilege { Id = 13101411 , ParentId = 131014 , Name = @Resources.General_Delete , Description = Resources.General_DeleteopeningVoucher },
                       //                           new Privilege { Id = 13101412 , ParentId = 131014 , Name =  @Resources.General_Edit , Description =Resources.General_EditopeningVoucher },
                       //                           new Privilege { Id = 13101413 , ParentId = 131014 , Name = Resources.General_Post , Description = Resources.General_openingVoucherPost },
                       //                  new Privilege { Id = 131015 , ParentId = 1310 , Name =  Resources.General_Openingbalance , Description =  Resources.General_Openingbalance },
                       //                           new Privilege { Id = 13101510 , ParentId = 131015 , Name = Resources.General_AddOpeningbalance , Description = Resources.General_AddOpeningbalance },
                       //                           new Privilege { Id = 13101511 , ParentId = 131015 , Name = @Resources.General_DeleteOpeningbalance , Description = Resources.General_DeleteOpeningbalance },
                       //                           new Privilege { Id = 13101512 , ParentId = 131015 , Name =  @Resources.General_EditOpeningbalance , Description =Resources.General_EditOpeningbalance },
                       //                           new Privilege { Id = 13101513 , ParentId = 131015 , Name = Resources.General_OpeningbalancePost , Description = Resources.General_OpeningbalancePost },
                       //                   new Privilege { Id = 131016 , ParentId = 1310 , Name = Resources.General_branchConvertVoucher , Description =  Resources.General_branchConvertVoucher},

                       //new Privilege { Id = 1311 , ParentId = 13 , Name = Resources.Acc_Checks , Description =  Resources.Acc_Checks},
                       //         new Privilege { Id = 131110 , ParentId = 1311 , Name =   @Resources.General_Add , Description = Resources.Acc_AddUndercollectionChecks},
                       //                        new Privilege { Id = 13111010 , ParentId = 131110 , Name =  @Resources.General_Delete , Description = Resources.Acc_DeleteUndercollectionChecks},
                       //                        new Privilege { Id = 13111011 , ParentId = 131110 , Name =  @Resources.General_Edit , Description = Resources.Acc_EditUndercollectionChecks},
                       //         new Privilege { Id = 131111 , ParentId = 1311 , Name = Resources.Acc_ExportsTabClickes, Description =  Resources.Acc_ExportsTabClickes },
                       //                       new Privilege { Id = 13111110 , ParentId = 131111 , Name =   @Resources.General_Add , Description = Resources.Acc_AddExportsChecks},
                       //                       new Privilege { Id = 13111111 , ParentId = 131111 , Name =  @Resources.General_Delete , Description = Resources.Acc_DeleteExportsChecks},
                       //                       new Privilege { Id = 13111112 , ParentId = 131111 , Name = @Resources.General_Edit , Description = Resources.Acc_EditExportsChecks},


                       // new Privilege { Id = 1312 , ParentId = 13 , Name =  Resources.General_RevenuesAndExpenses , Description =  Resources.General_RevenuesAndExpenses},
                       //               new Privilege { Id = 131210 , ParentId = 1312 , Name =   @Resources.General_Add, Description = Resources.Addrevenuesandexpenses },
                       //               new Privilege { Id = 131211 , ParentId = 1312 , Name =  @Resources.General_Delete , Description = Resources.General__DeleteRevenuesAndExpenses},
                       //               new Privilege { Id = 131212 , ParentId = 1312 , Name = @Resources.General_Edit , Description = Resources.General__DeleteRevenuesAndExpenses},
                       // new Privilege { Id = 1313 , ParentId = 13 , Name =  Resources.General_Currencies, Description =  Resources.General_Currencies },
                       //               new Privilege { Id = 131310 , ParentId = 1313 , Name =   @Resources.General_Add, Description = Resources.Acc_Currency},
                       //               new Privilege { Id = 131311 , ParentId = 1313 , Name = @Resources.General_Delete , Description = Resources.General_EditCurrency},
                       //               new Privilege { Id = 131312 , ParentId = 1313 , Name =  @Resources.General_Edit , Description =Resources.General_EditCurrency},
                       // new Privilege { Id = 1314 , ParentId = 13 , Name =  "تجهيز السندات" , Description =  "تجهيز السندات"},
                       // new Privilege { Id = 1315 , ParentId = 13 , Name =  Resources.General_Officialdocuments, Description =  Resources.General_Officialdocuments },
                       //               new Privilege { Id = 131510 , ParentId = 1315 , Name =   @Resources.General_Add, Description = Resources.General_AddOfficialdocuments },
                       //               new Privilege { Id = 131511 , ParentId = 1315 , Name = @Resources.General_Delete , Description = Resources.General_DeleteOfficialdocuments},
                       //               new Privilege { Id = 131512 , ParentId = 1315 , Name =  @Resources.General_Edit , Description = Resources.General_EditOfficialdocuments},
                       // new Privilege { Id = 1316 , ParentId = 13 , Name =Resources.General_InvoicesAndServices  , Description =  Resources.General_InvoicesAndServices},
                       //               new Privilege { Id = 131610 , ParentId = 1316 , Name =   @Resources.General_Add , Description = Resources.General_AddServices},
                       //               new Privilege { Id = 131611 , ParentId = 1316 , Name =  @Resources.General_Delete, Description =Resources.General_DeleteServices},
                       //               new Privilege { Id = 131612 , ParentId = 1316 , Name =  @Resources.General_Edit , Description = Resources.General_EditServices},
                       // new Privilege { Id = 1317 , ParentId = 13 , Name =  Resources.General_AccountingTree , Description =   Resources.General_AccountingTree},
                       //               new Privilege { Id = 131710 , ParentId = 1317 , Name =   @Resources.General_Add, Description = "اضافة حساب" },
                       //               new Privilege { Id = 131711 , ParentId = 1317 , Name =  @Resources.General_Delete, Description = "حذف حساب"},
                       //               new Privilege { Id = 131712 , ParentId = 1317 , Name =  @Resources.General_Edit, Description = "تعديل حساب" },
                       // new Privilege { Id = 1318 , ParentId = 13 , Name =  Resources.General_CostCenters , Description =  Resources.General_CostCenters},
                       //               new Privilege { Id = 131810 , ParentId = 1318 , Name =   @Resources.General_Add , Description =  "اضافة مركز تكلفة"},
                       //               new Privilege { Id = 131811 , ParentId = 1318 , Name =  @Resources.General_Delete, Description =  Resources.General_DeleteCostCenters },
                       //               new Privilege { Id = 131812 , ParentId = 1318 , Name =  @Resources.General_Edit , Description =  Resources.General_EditCostCenters },
                       // new Privilege { Id = 1319 , ParentId = 13 , Name =  Resources.General_Accountingreports, Description =  Resources.General_Accountingreports },
                       //               new Privilege { Id = 131910 , ParentId = 1319 , Name =  Resources.General_AccountStatement, Description =  Resources.General_AccountStatement  },
                       //               new Privilege { Id = 131911 , ParentId = 1319 , Name = Resources.General_GeneralLedger , Description =  Resources.General_GeneralLedger },
                       //               new Privilege { Id = 131912 , ParentId = 1319 , Name = Resources.General_AssistantProfessor, Description =  Resources.General_AssistantProfessor  },
                       //               new Privilege { Id = 131913 , ParentId = 1319 , Name = Resources.General_Trading , Description =  Resources.General_Trading },
                       //               new Privilege { Id = 131914 , ParentId = 1319 , Name = Resources.General_profitsandlosses , Description =   Resources.General_profitsandlosses },
                       //               new Privilege { Id = 131915 , ParentId = 1319 , Name =  "اليومية العامة" , Description =  "اليومية العامة" },
                       // new Privilege { Id = 1320 , ParentId = 13 , Name =  Resources.General_EstimatedBudget , Description =  Resources.General_EstimatedBudget },
                       // new Privilege { Id = 1321 , ParentId = 13 , Name =  Resources.General_Gurantees , Description =  Resources.General_Gurantees },
                       //               new Privilege { Id = 132110 , ParentId = 1321 , Name =   @Resources.General_Add , Description = Resources.Acc_NewGurantee},
                       //               new Privilege { Id = 132111 , ParentId = 1321 , Name =  @Resources.General_Delete , Description = Resources.General_DeleteGurantees},
                       //               new Privilege { Id = 132112 , ParentId = 1321 , Name =  @Resources.General_Edit , Description = Resources.General_EditGurantees},
                       // new Privilege { Id = 1322 , ParentId = 13 , Name = Resources.General_CustomerExpensesAndRevenues , Description =  Resources.General_CustomerExpensesAndRevenues},
                          
                      
       




                     //                     new Privilege { Id = 141010 , ParentId = 1410 , Name =  Resources.Emp_Employees, Description =  Resources.Emp_Employees },
                     //                                  new Privilege { Id = 14101010,ParentId = 141010 , Name =   @Resources.General_Add , Description = Resources.Emp_NewEmployee},
                     //                                  new Privilege { Id = 14101011 , ParentId = 141010 , Name =  Resources.General_Print , Description = Resources.General_PrintEmployeeData},
                     //                                  new Privilege { Id = 14101012 , ParentId = 141010 , Name =  @Resources.General_Delete , Description =Resources.General_DeleteEmployee},
                     //                                  new Privilege { Id = 14101013 , ParentId = 141010 , Name =  @Resources.General_Edit , Description = Resources.General_EditEmployee},
                     //                                  new Privilege { Id = 14101014 , ParentId = 141010 , Name =  @Resources.General_Search, Description = Resources.General_SearchEmployee},
                     //                     new Privilege { Id = 141011 , ParentId = 1410 , Name =  Resources.Emp_VacationSearch , Description =  Resources.Emp_VacationSearch},
                     //                                  new Privilege { Id = 14101110 , ParentId = 141011 , Name =  @Resources.General_Delete, Description = Resources.General_DeleteVacation },
                     //                     new Privilege { Id = 141012 , ParentId = 1410 , Name = Resources.Emp_AllowanceSearch , Description =  Resources.Emp_AllowanceSearch},
                     //                                   new Privilege { Id = 14101210 , ParentId = 141012 , Name =  @Resources.General_Delete , Description = Resources.General_DeleteAllowance},
                     //                                   new Privilege { Id = 14101211 , ParentId = 141012 , Name =  @Resources.General_Search , Description = Resources.General_SearchAllowance},
                     //                     new Privilege { Id = 141013 , ParentId = 1410 , Name =   Resources.Emp_Custody , Description =  Resources.Emp_Custody},
                     //                                  new Privilege { Id = 14101310 , ParentId = 141013 , Name =  @Resources.General_Delete, Description = Resources.Emp_DeleteCustody},
                     //                                  new Privilege { Id = 14101311 , ParentId = 141013 , Name =  @Resources.General_Edit , Description = Resources.Emp_EditCustody},
                     //                     new Privilege { Id = 141014 , ParentId = 1410 , Name =  Resources.Emp_Attendance , Description =  Resources.Emp_Attendance},
                     //                                   new Privilege { Id = 14101410 , ParentId = 141014 , Name =  @Resources.General_Delete, Description =Resources.General_DeleteDay },
                     //                                   new Privilege { Id = 14101411 , ParentId = 141014 , Name =  @Resources.General_Edit, Description =Resources.General_EditDay },
                     //                     new Privilege { Id = 141015 , ParentId = 1410 , Name =  Resources.Emp_SalariesOfficer , Description =  Resources.Emp_SalariesOfficer},
                     //                     new Privilege { Id = 141016 , ParentId = 1410 , Name =  Resources.Emp_AdministrativeStructure , Description =  Resources.Emp_AdministrativeStructure},      
                     //new Privilege { Id = 1411 , ParentId = 14 , Name =  Resources.Emp_CarMovements , Description =  Resources.Emp_CarMovements},
                     //                     new Privilege { Id = 141110 , ParentId = 1411 , Name = @Resources.General_Delete, Description = Resources.General_DeleteCarMovement},
                     //                     new Privilege { Id = 141111 , ParentId = 1411 , Name = @Resources.General_Edit , Description = Resources.General_EditCarMovement},
                     //new Privilege { Id = 1412 , ParentId = 14 , Name = Resources.Emp_Employeesappraisal , Description = Resources.Emp_Employeesappraisal},
                     //                     new Privilege { Id = 141210 , ParentId = 1412 , Name =  @Resources.General_Delete, Description = Resources.General_DeleteDegree},
                     //                     new Privilege { Id = 141211 , ParentId = 1412 , Name = @Resources.General_Edit , Description = Resources.General_EditDegree},
                     //new Privilege { Id = 1413 , ParentId = 14 , Name = Resources.General_Items , Description = Resources.General_Items},
                     //                     new Privilege { Id = 141310 , ParentId = 1413 , Name =  @Resources.General_Delete, Description = Resources.General_DeleteItem},
                     //                     new Privilege { Id = 141311 , ParentId = 1413 , Name = @Resources.General_Edit , Description = Resources.General_EditItem},
                                
               ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
               
                      //new Privilege { Id = 16 , ParentId = null , Name =  Resources.General_AccountsReports , Description = Resources.General_AccountsReports },
                      //      new Privilege { Id = 1610 , ParentId = 16 , Name =  Resources.General_AccountStatement, Description = Resources.General_AccountStatement },
                      //                      new Privilege { Id = 161010 , ParentId = 1610 , Name = Resources.Pro_Show , Description = Resources.Pro_Show},
                      //      new Privilege { Id = 1611 , ParentId = 16 , Name =  Resources.Sys_generalmanager , Description = Resources.Sys_generalmanager},
                      //                      new Privilege { Id = 161110 , ParentId = 1611 , Name = Resources.Sys_generalmanager , Description = Resources.Sys_generalmanager},
                      //      new Privilege { Id = 1612 , ParentId = 16 , Name = Resources.Sys_incomelist , Description = Resources.Sys_incomelist},
                      //                      new Privilege { Id = 161210 , ParentId = 1612 , Name =  Resources.Sys_incomelist, Description = Resources.Sys_incomelist },
                      //      new Privilege { Id = 1613 , ParentId = 16 , Name =Resources.General_ProjectTaskesReport, Description = Resources.General_ProjectTaskesReport},
                               
                                                ////////////////////////////////////////////////////////
                                         ////////////////////////////////////////////// System Settings
                       new Privilege { Id = 17 , ParentId = null , Name =  Resources.General_ControlPanel , Description = Resources.General_ControlPanel},

                         new Privilege { Id = 1701 , ParentId = 17 , Name =  Resources.General_Administration1 , Description = Resources.General_Administration1},

                             new Privilege { Id = 170101 , ParentId = 1701 , Name =  Resources.General_BuildingData , Description = Resources.General_BuildingData},

                               new Privilege { Id = 17010101 , ParentId = 170101 , Name =  Resources.General_Save , Description =  Resources.General_Save},
                                 new Privilege { Id = 17010102 , ParentId = 170101 , Name =  Resources.General_MailSettings , Description =  Resources.General_MailSettings},
                                 new Privilege { Id = 1701010201 , ParentId = 17010102 , Name =  Resources.General_Email , Description =  Resources.General_Email},
                                  new Privilege { Id = 1701010202 , ParentId = 17010102 , Name =  Resources.General_Sms , Description =  Resources.General_Sms},








                                 new Privilege { Id = 170102 , ParentId = 1701 , Name =  Resources.MS_VAT_Sett , Description = Resources.MS_VAT_Sett},
                                     //new Privilege { Id = 170103 , ParentId = 1701 , Name =  Resources.General_Alerts , Description = Resources.General_Alerts},

                                     // new Privilege { Id = 17010301 , ParentId = 170103 , Name =  Resources.General_Add , Description = Resources.General_Add},
                                     //  new Privilege { Id = 17010302 , ParentId = 170103 , Name =  Resources.ShowAlert , Description = Resources.ShowAlert},



                                       new Privilege { Id = 170104 , ParentId = 1701 , Name =  Resources.SystemOptions , Description = Resources.SystemOptions},
                                       new Privilege { Id = 17010401 , ParentId = 170104 , Name =  "سجل احداث النظام" , Description = "سجل احداث النظام"},


                                          new Privilege { Id = 170105 , ParentId = 1701 , Name =  Resources.MD_Create_restore_backup , Description = Resources.MD_Create_restore_backup},
                                          new Privilege { Id = 17010501 , ParentId = 170105 , Name =  Resources.MB_CreateBackup , Description = Resources.MB_CreateBackup},
                                        new Privilege { Id = 17010502 , ParentId = 170105 , Name =  Resources.MB_Restore , Description = Resources.MB_Restore},
                                         new Privilege { Id = 17010503 , ParentId = 170105 , Name =  Resources.MB_Download , Description = Resources.MB_Download},
                                         new Privilege { Id = 17010504 , ParentId = 170105 , Name =  Resources.MB_delete , Description = Resources.MB_delete},





                           new Privilege { Id = 1702 , ParentId = 17 , Name =  Resources.General_SystemPermissions , Description = Resources.General_SystemPermissions},

                            new Privilege { Id = 170201 , ParentId = 1702 , Name =  Resources.General_Users , Description = Resources.General_Users},

                             new Privilege { Id = 17020101 , ParentId = 170201 , Name ="المستخدمين" , Description = "المستخدمين"},
                              new Privilege { Id = 1702010101 , ParentId = 17020101 , Name =  Resources.General_Add , Description =  Resources.General_Add},
                              new Privilege { Id = 1702010102 , ParentId = 17020101 , Name =  Resources.General_Edit , Description =  Resources.General_Edit},
                              new Privilege { Id = 1702010103 , ParentId = 17020101 , Name =  Resources.General_Delete , Description =  Resources.General_Delete},

                              new Privilege { Id = 17020102 , ParentId = 170201 , Name =  Resources.General_QueryAboutUser , Description = Resources.General_QueryAboutUser},
                              new Privilege { Id = 17020103 , ParentId = 170201 , Name =  Resources.General_InquiryAboutValidity , Description = Resources.General_InquiryAboutValidity},
                              new Privilege { Id = 17020104 , ParentId = 170201 , Name =  Resources.General_Usersstatement , Description = Resources.General_Usersstatement},
                              new Privilege { Id = 17020105 , ParentId = 170201 , Name =  Resources.General_OnlineUsers , Description = Resources.General_OnlineUsers},
                              new Privilege { Id = 17020106 , ParentId = 170201 , Name =  "حالة المستخدم" , Description ="حالة المستخدم" },











                             new Privilege { Id = 170202 , ParentId = 1702 , Name =  Resources.General_Groups , Description = Resources.General_Groups},
                               new Privilege { Id = 17020201 , ParentId = 170202 , Name =  Resources.General_Add , Description =  Resources.General_Add},
                              new Privilege { Id = 17020202 , ParentId = 170202 , Name =  Resources.General_Save , Description =  Resources.General_Save},





                               new Privilege { Id = 1703 , ParentId = 1701 , Name =  Resources.Sys_Branches , Description = Resources.Sys_Branches},
                                  new Privilege { Id = 170301 , ParentId = 1703 , Name =  Resources.General_Add , Description =  Resources.General_Add},
                                       new Privilege { Id = 170302 , ParentId = 1703 , Name =  Resources.General_Edit , Description =  Resources.General_Edit},
                                        new Privilege { Id = 170303 , ParentId = 1703 , Name =  Resources.General_Delete , Description =  Resources.General_Delete},
                                         new Privilege { Id = 170304 , ParentId = 1703 , Name =  Resources.Sys_Branchaccounts , Description =  Resources.Sys_Branchaccounts},









                                new Privilege { Id = 1704 , ParentId = 1701 , Name =  Resources.General_Attendees , Description = Resources.General_Attendees},
                                  new Privilege { Id = 170401 , ParentId = 1704 , Name =  Resources.General_Add , Description =  Resources.General_Add},
                                       new Privilege { Id = 170402 , ParentId = 1704 , Name =  Resources.General_Edit , Description =  Resources.General_Edit},
                                        new Privilege { Id = 170403 , ParentId = 1704 , Name =  Resources.General_Delete , Description =  Resources.General_Delete},
                                         new Privilege { Id = 170404 , ParentId = 1704 , Name =  Resources.Emp_AttTimeDetails , Description =  Resources.Emp_AttTimeDetails},




                            //new Privilege { Id = 1710 , ParentId = 17 , Name =  Resources.General_Accounting , Description = Resources.General_Accounting},
                            //               new Privilege { Id = 171010 , ParentId = 1710 , Name = Resources.Acc_AccountInformation , Description = Resources.Acc_AccountInformation},
                            //               new Privilege { Id = 171011 , ParentId = 1710 , Name = Resources.General_CostCenters , Description = Resources.General_CostCenters},
                            //               new Privilege { Id = 171012 , ParentId = 1710 , Name =  Resources.General_Currencies , Description = Resources.General_Currencies},
                            //               new Privilege { Id = 171013 , ParentId = 1710 , Name =  Resources.General_Fiscalyears, Description = Resources.General_Fiscalyears},
                            
            new Privilege { Id = 1711 , ParentId = 17 , Name =Resources. General_Administration , Description = Resources.General_Administration},
                                           new Privilege { Id = 171110 , ParentId = 1711 , Name =  @Resources.Sys_Branches , Description = Resources.Sys_Branches},
                                                            new Privilege { Id = 17111010 , ParentId = 171110 , Name =   @Resources.General_Add , Description = Resources.Sys_NewBranch},
                                                            new Privilege { Id = 17111011 , ParentId = 171110 , Name =  @Resources.General_Delete , Description = Resources.General_DeleteBranch},
                                                            new Privilege { Id = 17111012 , ParentId = 171110 , Name = @Resources.General_Edit , Description = Resources.General_EditBranch},
                                           new Privilege { Id = 171111 , ParentId = 1711 , Name =  Resources.Sys_OrganizationData, Description = Resources.Sys_OrganizationData },
                                           new Privilege { Id = 171112 , ParentId = 1711 , Name =  Resources.General_Attendees, Description = Resources.General_Attendees },
                                           //new Privilege { Id = 171113 , ParentId = 1711 , Name =  Resources.General_AdjustTheProceedings, Description = Resources.General_AdjustTheProceedings },
                                           //new Privilege { Id = 171114 , ParentId = 1711 , Name =  Resources.General_Alerts, Description = Resources.General_Alerts },
                                           //new Privilege { Id = 171115 , ParentId = 1711 , Name =  Resources.General_Alertssettings , Description = Resources.General_Alertssettings},

                             new Privilege { Id = 1712 , ParentId = 17 , Name =  Resources.General_SystemPermissions, Description = Resources.General_SystemPermissions },
                                   new Privilege { Id = 171210 , ParentId = 1712 , Name =  Resources.General_Users, Description = Resources.General_Users },
                                        new Privilege { Id = 17121010 , ParentId = 171210 , Name =  Resources.General_AllUsers, Description = Resources.General_AllUsers },
                                        new Privilege { Id = 17121011 , ParentId = 171210 , Name =  Resources.General_NewUser, Description = Resources.General_NewUser },
                                        new Privilege { Id = 17121012 , ParentId = 171210 , Name =  @Resources.General_Delete , Description = Resources.General_DeleteUser},
                                        new Privilege { Id = 17121013 , ParentId = 171210 , Name =  @Resources.General_Edit, Description =Resources.General_EditUser},
                                   new Privilege { Id = 171211 , ParentId = 1712 , Name =  Resources.General_Groups, Description = Resources.General_Groups},
                                         new Privilege { Id = 17121110 , ParentId = 171211 , Name =  Resources.Acc_AddNewGroup , Description = Resources.Acc_AddNewGroup},



                                           new Privilege { Id = 17123, ParentId = 17 , Name =  "اعدادات نطاق العمل", Description = "اعدادات نطاق العمل" },
                                        new Privilege { Id = 1712310 , ParentId = 17123 , Name =  Resources.General_Add, Description = Resources.General_Add },
                                        new Privilege { Id = 1712311 , ParentId = 17123 , Name =  Resources.General_Edit, Description = Resources.General_Edit },
                                        new Privilege { Id = 1712312 , ParentId = 17123 , Name =  @Resources.General_Delete , Description = Resources.General_Delete},



                                           new Privilege { Id = 17124, ParentId = 17 , Name =  "اعدادات  إدارة الاشعارات", Description =  "اعدادات  إدارة الاشعارات" },
                                        new Privilege { Id = 171241 , ParentId = 17124 , Name =  @Resources.General_Show , Description = Resources.General_Show},
                                        new Privilege { Id = 171242 , ParentId = 17124 , Name =  Resources.General_Edit, Description = Resources.General_Edit },
                                       
             
             
            //////////////////////////////////////////////////////////////////
            new Privilege { Id = 1714 , ParentId = 17 , Name =  "تنبيه لجميع المستخدمين" },
            new Privilege { Id = 1715 , ParentId = 17 , Name =  "ارسال اشعار عام" },
            new Privilege { Id = 1716 , ParentId = 17 , Name =  "ارسال اشعار خاص" },
            new Privilege { Id = 1717 , ParentId = 17 , Name =  "تقرير بيانات المستخدمين" },
            new Privilege { Id = 1718 , ParentId = 17 , Name =  "تقرير صلاحيات المستخدمين" },
            //new Privilege { Id = 1719 , ParentId = 17 , Name =  "طلب  صلاحية" },  
            ////////////////////////تقارير الحسابات////////////////////////////////
//new Privilege { Id = 19 , ParentId = null , Name =  @Resources.General_AccountsReports , Description =  @Resources.General_AccountsReports },
//  new Privilege { Id = 1910 , ParentId = 19 , Name = @Resources.General_AccountStatement , Description =  @Resources.General_AccountStatement },
//  new Privilege { Id = 1911 , ParentId = 19 , Name =  @Resources.General_GeneralLedger , Description =  @Resources.General_GeneralLedger},
//  new Privilege { Id = 1912 , ParentId = 19 , Name =  @Resources.General_AssistantProfessor , Description =  @Resources.General_AssistantProfessor },
//  new Privilege { Id = 1913 , ParentId = 19 , Name =  @Resources.General_DailyJournal , Description =  @Resources.General_DailyJournal },
//  new Privilege { Id = 1914 , ParentId = 19 , Name =  @Resources.General_TrialBalance , Description =  @Resources.General_TrialBalance },
//  new Privilege { Id = 1915 , ParentId = 19 , Name =  @Resources.General_IncomeState , Description =  @Resources.General_IncomeState },
//  new Privilege { Id = 1916 , ParentId = 19 , Name =  @Resources.Sys_Trading, Description =  @Resources.Sys_Trading },
//  new Privilege { Id = 1917 , ParentId = 19 , Name =   @Resources.General_profitsandlosses , Description =  @Resources.General_profitsandlosses },
//  new Privilege { Id = 1919 , ParentId = 19 , Name =  @Resources.General_Centermovementscost , Description = @Resources.General_Centermovementscost },
//  new Privilege { Id = 1920 , ParentId = 19 , Name =  @Resources.General_CostCenters , Description =  @Resources.General_CostCenters },
//  new Privilege { Id = 1921 , ParentId = 19 , Name =  @Resources.General_Publicbudget , Description =  @Resources.General_Publicbudget },
//  new Privilege { Id = 1922 , ParentId = 19 , Name =  @Resources.General_CollectiveBondDisclosure , Description =  @Resources.General_CollectiveBondDisclosure },
//  new Privilege { Id = 1923 , ParentId = 19 , Name =  @Resources.General_ValueAdded , Description =  @Resources.General_ValueAdded },




        };
    }


    public class Privilege
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}