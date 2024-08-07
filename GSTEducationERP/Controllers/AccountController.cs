using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GSTEducationERPLibrary.Account;


namespace GSTEducationERP.Controllers
{
    public class AccountController : Controller
    {
        private readonly BALAccount objbal;


        //My name is Don
        /// <summary>
        /// //////
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        /// </summary>
        public AccountController()
        {
            objbal = new BALAccount();
        }


        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        /// <summary>
        /// This view is used to show the login page
        /// </summary>
        /// <param name="obj"></param>
        /// <returns> Login </returns>
        [HttpPost]
        public async Task<ActionResult> Login(Account obj)
        {
            
            SqlDataReader dr;
            dr = await objbal.Login(obj);
            if (dr.Read())
            {
                string Email = dr["OfficialEmailId"].ToString();
                string password = dr["Password"].ToString();
                int PositionId = Convert.ToInt32(dr["StaffPositionId"].ToString());
                string StaffCode = dr["StaffCode"].ToString();
                string BranchCode = dr["BranchCode"].ToString();
                string Coursecode = dr["CourseCode"].ToString();/*------------------------------------*/
                string StaffName = dr["StaffName"].ToString();
                string Photograph = dr["Photograph"].ToString();
                if (Email == obj.EmailId && password == obj.Password)
                {
                    if (PositionId == 1)
                    {
                        Session["StaffPositionId"] = PositionId;
                        Session["StaffCode"] = StaffCode;
                        Session["StaffName"] = StaffName;
                        Session["Photograph"] = Photograph;
                        return RedirectToAction("Dashboard", "Counsellor");
                    }
                    else if (PositionId == 2)
                    {
                        Session["StaffPositionId"] = PositionId;
                        Session["StaffCode"] = StaffCode;
                        Session["BranchCode"] = BranchCode;  /*------------------------------------*/
                        Session["StaffName"] = StaffName;
                        Session["Photograph"] = Photograph;
                        return RedirectToAction("CoordinatorDashboardPRAsync", "Coordinator");
                    }
                    else if (PositionId == 3)
                    {

                        BALAccount objTr = new BALAccount(); // Instantiate BALAccount
                                                             //obj.TrainerCode = StaffCode;

                        DataSet ds = await objTr.GetScheduledBatchCountAsyncST(StaffCode); // Use objTr consistently
                        Account count = new Account();

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            count.Count = ds.Tables[0].Rows[i]["TotalCount"].ToString();
                        }

                        //ViewBag.BatchCount = count.Count;
                        Session["BatchCount"] = count.Count;
                        Session["StaffPositionId"] = PositionId;
                        Session["StaffCode"] = StaffCode;
                        Session["BranchCode"] = BranchCode;
                        Session["CourseCode"] = Coursecode;
                        Session["StaffName"] = StaffName;
                        Session["Photograph"] = Photograph;
                        return RedirectToAction("TrainerDashboardAsyncTS", "Trainer");
                    }
                    else if (PositionId == 4)
                    {
                        Session["StaffPositionId"] = PositionId;
                        Session["StaffCode"] = StaffCode;
                        Session["StaffName"] = StaffName;
                        Session["Photograph"] = Photograph;
                        return RedirectToAction("Index", "Admin");
                    }

                    else if (PositionId == 5)
                    {
                        Session["StaffPositionId"] = PositionId;
                        Session["StaffCode"] = StaffCode;
                        Session["BranchCode"] = BranchCode;
                        Session["StaffName"] = StaffName;
                        Session["Photograph"] = Photograph;
                        return RedirectToAction("PlcementDashboardPCAsync", "Placement");
                    }
                    else if (PositionId == 6)
                    {
                        Session["StaffPositionId"] = PositionId;
                        Session["StaffCode"] = StaffCode;
                        Session["StaffName"] = StaffName;
                        Session["Photograph"] = Photograph;
                        return RedirectToAction("Index", "HR");
                    }
                    else if (PositionId == 7)
                    {
                        Session["StaffPositionId"] = PositionId;
                        Session["StaffCode"] = StaffCode;
                        Session["StaffName"] = StaffName;
                        Session["BranchCode"] = BranchCode;
                        Session["Photograph"] = Photograph;
                        return RedirectToAction("AccountantDashboardAsyncSGS", "Accountant");
                    }

                }
            }
            else
            {
                TempData["ErrorMessage"] = "Email or password is invalid.";
            }


            return await Task.Run(() => View());
        }

        //[HttpPost]
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Login");
        }

    }
}
