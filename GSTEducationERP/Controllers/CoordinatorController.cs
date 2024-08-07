using GSTEducationERPLibrary.Coordinator;
using GSTEducationERPLibrary.Trainer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using static GSTEducationERPLibrary.Coordinator.Coordinator;

namespace GSTEducationERP.Controllers
{
    public class CoordinatorController : Controller
    {

        private readonly BALCoordinator objbal;

        public CoordinatorController()
        {
            objbal = new BALCoordinator();
        }

        public class BreadcrumbItem
        {
            public string Name { get; set; }
            public string Url { get; set; }
        }
        public async Task AllCourseBind()
        {
            Coordinator obj = new Coordinator();
            obj.BranchCode = Session["BranchCode"].ToString();
            DataSet ds1 = new DataSet();
            ds1 = await objbal.AllCourseBind(obj);
            List<SelectListItem> AllCourseBind = new List<SelectListItem>();
            if (ds1.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds1.Tables[0].Rows)
                {
                    AllCourseBind.Add(new SelectListItem
                    {
                        Text = dr["CourseName"].ToString(),
                        Value = dr["Coursecode"].ToString()
                    });
                }
            }
            ViewBag.AllCourseBind = AllCourseBind;

        }
        //-----------------Pratiksha Dashboard Start -----------------------------------------------------------------------//
        /// <summary>
        /// This is CoordinatorDashboard method for dashboard counts.
        /// </summary>
        /// <returns>Returns the counts.</returns>
        public async Task<ActionResult> CoordinatorDashboardPRAsync()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Coordinator objCoordinator = new Coordinator();

                objCoordinator.BranchCode = Session["BranchCode"].ToString();

                // Count Total Student.
                SqlDataReader dr;
                dr = await objbal.CountTotalStudentPRAsync(objCoordinator);
                dr.Read();
                objCoordinator.StudentId = Convert.ToInt32(dr["Total Students"].ToString());
                dr.Close();
                ViewBag.StudentId = objCoordinator.StudentId;

                // Count Total Batches.
                dr = await objbal.CountTotalBatchesPRAsync(objCoordinator);
                dr.Read();
                objCoordinator.BatchId = Convert.ToInt32(dr["Total Batches"].ToString());
                dr.Close();
                ViewBag.BatchId = objCoordinator.BatchId;

                // Count Active Batches.
                dr = await objbal.CountActiveBatchesPRAsync(objCoordinator);
                dr.Read();
                objCoordinator.ActiveBatches = Convert.ToInt32(dr["ActiveBatches"].ToString());
                dr.Close();
                ViewBag.ActiveBatches = objCoordinator.ActiveBatches;

                // Count Released Batches.
                SqlDataReader dr1;
                dr1 = await objbal.CountReleasedBatchesPRAsync(objCoordinator);
                dr1.Read();
                objCoordinator.ReleasedBatches = Convert.ToInt32(dr1["ReleasedBatches"].ToString());
                dr1.Close();
                ViewBag.ReleasedBatches = objCoordinator.ReleasedBatches;

                // Count Total Courses.
                dr = await objbal.CountTotalCoursesPRAsync(objCoordinator);
                dr.Read();
                objCoordinator.CourseId = Convert.ToInt32(dr["Total Courses"].ToString());
                dr.Close();
                ViewBag.CourseId = objCoordinator.CourseId;

                // Count Total Lab.
                dr = await objbal.CountTotalLabPRAsync(objCoordinator);
                dr.Read();
                objCoordinator.LabId = Convert.ToInt32(dr["Total Lab"].ToString());
                dr.Close();
                ViewBag.LabId = objCoordinator.LabId;

                // Count Total Event.
                dr = await objbal.CountEventsPRAsync(objCoordinator);
                dr.Read();
                objCoordinator.EventId = Convert.ToInt32(dr["Events"].ToString());
                dr.Close();
                ViewBag.EventId = objCoordinator.EventId;

                // Active Batch Graph.
                List<float> activeBatchList = new List<float>();
                List<string> xAxisCategories = new List<string>();

                DataSet Dsa = new DataSet();
                Dsa = await objbal.GraphActiveBatchPRAsync(objCoordinator);

                for (int i = 0; i < Dsa.Tables[0].Rows.Count; i++)
                {
                    float studentCount = float.Parse(Dsa.Tables[0].Rows[i]["student_count"].ToString());
                    activeBatchList.Add(studentCount);

                    // Assuming "batch_name" is the column in your database table containing batch names
                    string BatchName = Dsa.Tables[0].Rows[i]["BatchName"].ToString();
                    xAxisCategories.Add(BatchName);
                }

                ViewBag.ActiveBatch = activeBatchList.ToArray();
                ViewBag.XAxisCategories = xAxisCategories.ToArray();

                // Paid, Unpaid, and Partial fees of students counts graph.
                int paidCount = 0;
                int unpaidCount = 0;
                int partialCount = 0;
                using (SqlDataReader paidReader = await objbal.CountFeesPRAsync(objCoordinator))
                {
                    if (paidReader.HasRows)
                    {
                        while (await paidReader.ReadAsync())
                        {
                            paidCount = paidReader.GetInt32(paidReader.GetOrdinal("Paid Count"));
                            unpaidCount = paidReader.GetInt32(paidReader.GetOrdinal("Unpaid Count"));
                            partialCount = paidReader.GetInt32(paidReader.GetOrdinal("Partial Count"));
                        }
                        ViewBag.PaidCount = paidCount;
                        ViewBag.UnpaidCount = unpaidCount;
                        ViewBag.PartialCount = partialCount;
                    }
                    else
                    {
                        ViewBag.Message = "No data found for counts.";
                    }
                }

                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
        {
            new BreadcrumbItem { Name = "CoordinatorDashboard", Url = Url.Action("CoordinatorDashboardPRAsync", "Coordinator") },
        };

                ViewBag.Breadcrumbs = breadcrumbs;
                return View();
            }
        }
        //-----------------Pratiksha Dashboard End -----------------------------------------------------------------------//
        //-----------------Vedant Lab Management Start -------------------------------------------------------------------//

        /// <summary>
        /// This View is used to View All  Lab List. 
        /// </summary>
        /// <returns> Lab List. </returns>
        [HttpGet]
        public async Task<ActionResult> ListLabAsyncVJ()
        {


            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await CenterName_Bind();
                Coordinator objcoord = new Coordinator();
                objcoord.BranchCode = Session["BranchCode"].ToString();
                DataSet ds = new DataSet();
                ds = await objbal.ViewLabList(objcoord);
                List<Coordinator> Lablist1 = new List<Coordinator>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Coordinator objcoord1 = new Coordinator();
                    objcoord1.LabId = Convert.ToInt32(ds.Tables[0].Rows[i]["LabId"].ToString());
                    objcoord1.LabCode = ds.Tables[0].Rows[i]["LabCode"].ToString();
                    objcoord1.LabName = ds.Tables[0].Rows[i]["LabName"].ToString();
                    objcoord1.BranchCode = ds.Tables[0].Rows[i]["BranchCode"].ToString();
                    objcoord1.LabCapacity = Convert.ToInt32(ds.Tables[0].Rows[i]["LabCapacity"].ToString());
                    objcoord1.AvailableSystem = Convert.ToInt32(ds.Tables[0].Rows[i]["AvailableSystem"].ToString());
                    objcoord1.LabCreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["LabCreatedDate"].ToString());
                    Lablist1.Add(objcoord1);
                }
                objcoord.ListLabAsyncVJ = Lablist1;
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                    new BreadcrumbItem { Name = "DashBoard ", Url ="DashBoard/Coordinator" },
                     new BreadcrumbItem { Name = "DetailsLabManagementAsyncVJ ", Url ="DetailsLabManagementAsyncVJ/Coordinator" },
                     new BreadcrumbItem { Name = "ListLabAsyncVJ ", Url ="ListLabAsyncVJ/Coordinator" },
                };

                ViewBag.Breadcrumbs = breadcrumbs;
                return PartialView("ListLabAsyncVJ", objcoord);
            }
        }
        /// <summary>
        /// This View is used to Add New Lab.
        /// </summary>
        /// <returns> Add New Lab. </returns>
        [HttpGet]
        public async Task<ActionResult> RegisterNewLabAsyncVJ()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await CenterName_Bind();
                Coordinator objC = new Coordinator();
                objC.BranchCode = Session["BranchCode"].ToString();
                objC.LabCreatedDate = DateTime.Today;
                return PartialView("RegisterNewLabAsyncVJ", objC);
            }
        }
        /// <summary>
        /// This View is used to Add New Lab.
        /// </summary>
        /// <param name="objC"> This Object is used to Create New Lab.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> RegisterNewLabAsyncVJ(Coordinator objC)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await Task.Run(() => objbal.CreateNewLab(objC));
                return RedirectToAction("DetailsLabManagementAsyncVJ");
            }
        }
        /// <summary>
        /// This View is used to Fetch Center Name.
        /// </summary>
        /// <returns> Add New Lab. </returns>
        public async Task CenterName_Bind()
        {
            DataSet ds = await objbal.FetchCenterName();
            List<SelectListItem> centerList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                centerList.Add(new SelectListItem { Text = dr["BranchName"].ToString(), Value = dr["BranchCode"].ToString() });
            }

            ViewBag.CenterName = centerList;

        }
        /// <summary>
        /// This View is used to Edit Lab Details.
        /// </summary>
        /// <returns> Edit Lab Details. </returns>
        [HttpGet]
        public async Task<ActionResult> UpdateLabAsyncVJ(int id)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Coordinator objC = new Coordinator();
                objC.BranchCode = Session["BranchCode"].ToString();
                objC.LabId = id;
                SqlDataReader dr;
                dr = await objbal.LabDetails(objC);
                while (dr.Read())
                {
                    objC.LabId = Convert.ToInt32(dr["LabId"].ToString());
                    objC.LabCode = (dr["LabCode"].ToString());
                    objC.LabName = (dr["LabName"].ToString());
                    objC.BranchCode = (dr["BranchCode"].ToString());
                    objC.LabCapacity = Convert.ToInt32(dr["LabCapacity"].ToString());
                    objC.AvailableSystem = Convert.ToInt32(dr["AvailableSystem"].ToString());
                    objC.LabCreatedDate = Convert.ToDateTime(dr["LabCreatedDate"].ToString());
                }
                return PartialView("UpdateLabAsyncVJ", objC);
            }
        }
        /// <summary>
        /// This View is used to Edit Lab Details.
        /// </summary>
        /// <param name="objC"> This Object is used to Edit Lab.</param>
        /// <returns> Lab Details.</returns>
        [HttpPost]
        public async Task<ActionResult> UpdateLabAsyncVJ(Coordinator objC)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {

                if (ModelState.IsValid)
                {

                    await objbal.UpdateLab(objC);
                    TempData["AlertMessage"] = "Lab Updated Successfully";
                    return RedirectToAction("DetailsLabManagementAsyncVJ", objC);
                }
                else
                {
                    return View(objC);
                }
            }
        }
        /// <summary>
        /// This View is used to View Lab Details.
        /// </summary>
        /// <param name="id"> This Object is used to View Lab Details. </param>
        /// <returns> Lab Details.</returns>
        [HttpGet]
        public async Task<ActionResult> DetailsLabAsyncVJ(int LabId)
        {

            Coordinator objdetials = new Coordinator();
            objdetials.BranchCode = Session["BranchCode"].ToString();
            objdetials.LabId = LabId;

            SqlDataReader dr;
            dr = await objbal.LabDetails(objdetials);
            while (dr.Read())
            {
                objdetials.LabId = Convert.ToInt32(dr["LabId"].ToString());
                objdetials.LabCode = (dr["LabCode"].ToString());
                objdetials.LabName = (dr["LabName"].ToString());
                objdetials.BranchCode = (dr["BranchCode"].ToString());
                objdetials.LabCapacity = Convert.ToInt32(dr["LabCapacity"].ToString());
                objdetials.AvailableSystem = Convert.ToInt32(dr["AvailableSystem"].ToString());
                objdetials.LabCreatedDate = Convert.ToDateTime(dr["LabCreatedDate"].ToString());

            }
            return PartialView(objdetials);
        }
        /// <summary>
        /// This View is used to Delete Lab.
        /// </summary>
        /// <param name="id"> This Object is used to Delete Lab.</param>
        /// <returns> Delete Lab. </returns>
        [HttpGet]
        public async Task<ActionResult> DeleteLabAsyncVJ(int id)
        {

            Coordinator objU = new Coordinator();
            objU.BranchCode = Session["BranchCode"].ToString();
            objU.LabId = id;
            await objbal.DeleteLab(objU);
            TempData["AlertMessage"] = "Employee Delete Successfully";
            return RedirectToAction("DetailsLabManagementAsyncVJ", objU);
        }
        /// <summary>
        /// This View is used to View Active Lab List.
        /// </summary>
        /// <returns> Active Lab List. </returns>
        [HttpGet]
        public async Task<ActionResult> ListAcitveLabAsyncVJ()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Coordinator objcoord3 = new Coordinator();
                objcoord3.BranchCode = Session["BranchCode"].ToString();
                DataSet ds = new DataSet();
                ds = await objbal.ViewActiveLabList(objcoord3);
                List<Coordinator> Lablist2 = new List<Coordinator>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Coordinator objcoord2 = new Coordinator();
                    try
                    {
                        objcoord2.LabCode = ds.Tables[0].Rows[i]["LabCode"].ToString();
                        objcoord2.LabName = ds.Tables[0].Rows[i]["LabName"].ToString();
                        Lablist2.Add(objcoord2);
                    }
                    catch (FormatException ex)
                    {

                    }
                }
                objcoord3.ListAcitveLabAsyncVJ = Lablist2;
                return PartialView("ListAcitveLabAsyncVJ", objcoord3);
            }
        }
        /// <summary>
        /// This View is used to View Lab Schedule Details.
        /// </summary>
        /// <returns> Lab Shcedule Details. </returns>
        [HttpGet]
        public async Task<ActionResult> DetailsActiveLabAsyncVJ(string LabCode)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Coordinator objC = new Coordinator();
                objC.BranchCode = Session["BranchCode"].ToString();
                objC.LabCode = LabCode;
                List<Coordinator> Lablist3 = new List<Coordinator>();
                DataSet ds = new DataSet();
                ds = await objbal.ScheduleLab(objC.LabCode, objC.BranchCode);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Coordinator objcoord3 = new Coordinator();
                    objcoord3.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objcoord3.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    objcoord3.NoOfStudent = Convert.ToInt32(ds.Tables[0].Rows[i]["NoOfStudent"].ToString());
                    objcoord3.StartDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"].ToString());
                    if (ds.Tables[0].Rows[i]["ReleseDate"] != DBNull.Value)
                    {
                        objcoord3.ReleseDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["ReleseDate"]);
                    }
                    else
                    {
                        objcoord3.ReleseDate = null;
                    }
                    Lablist3.Add(objcoord3);
                }
                objC.DetailsActiveLabAsyncVJ = Lablist3;
                return PartialView("DetailsActiveLabAsyncVJ", objC);
            }
        }
        /// <summary>
        /// This View is used to Get Partial View of Create New Lab, Active Lab, Lab Schedule.
        /// </summary>
        /// <returns> Partial View.</returns>
        public ActionResult DetailsLabManagementAsyncVJ()
        {
            List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                    new BreadcrumbItem { Name = "CoordinatorDashboard", Url =Url.Action("CoordinatorDashboardPRAsync","Coordinator")},
                     new BreadcrumbItem { Name = "DetailsLabManagementAsyncVJ ", Url ="DetailsLabManagementAsyncVJ/Coordinator" },
                };

            ViewBag.Breadcrumbs = breadcrumbs;



            return View();
        }
        [HttpGet]
        public async Task<ActionResult> LabScheduleAsyncVJ()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Coordinator obj = new Coordinator();
                obj.BranchCode = Session["BranchCode"].ToString();
                DataSet ds = await objbal.LabscheduleAsyncVP(obj);
                Coordinator objdetail = new Coordinator();
                List<Coordinator> lstlabseduleGridvp = new List<Coordinator>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Coordinator objurser = new Coordinator();
                    objurser.LabName = ds.Tables[0].Rows[i]["LabName"].ToString();
                    objurser.LabScheduledeatils = ds.Tables[0].Rows[i]["LabSchedule"].ToString();
                    lstlabseduleGridvp.Add(objurser);
                }
                objdetail.lstlabseduleGridvp = lstlabseduleGridvp;
                return PartialView("LabScheduleAsyncVJ", objdetail);
            }
        }
        [HttpPost]
        public async Task<JsonResult> IsLabAvilableAsyncVJ(string LabName)
        {
            bool isAvailable = await objbal.IsLabAvilableAsyncVJ(LabName);
            return Json(new { isAvailable });      // Return a JsonResult indicating whether the exam is available
        }
        //-----------------Vedant Lab Management End ----------------------------------------------------------------------//
        //-----------------Sayali batch Schedule Start -------------------------------------------------------------------//
        //-----------------Sayali batch Schedule Start -------------------------------------------------------------------//
        /// <summary>
        /// This JsonResult get all Cource data  on list.
        /// </summary>
        /// <returns> Cource list</returns>
        public async Task<JsonResult> GetCourceAsyncST()
        {
            Coordinator ObjCo = new Coordinator();
            DataSet ds = new DataSet();
            ObjCo.BranchCode = Session["BranchCode"].ToString();
            ds = await objbal.GetCourceAsyncST(ObjCo);
            List<SelectListItem> CourceList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                CourceList.Add(new SelectListItem
                {
                    Text = dr["CourseName"].ToString(),
                    Value = dr["Coursecode"].ToString()
                });

            }
            ViewBag.Course = CourceList;
            return Json(CourceList, JsonRequestBehavior.AllowGet);
        }
        //public async Task AllCourseBind()
        //{
        //    Coordinator obj = new Coordinator();
        //    obj.BranchCode = Session["BranchCode"].ToString();
        //    DataSet ds1 = new DataSet();
        //    ds1 = await objbal.GetCourceAsyncST(obj);
        //    List<SelectListItem> AllCourseBind = new List<SelectListItem>();
        //    if (ds1.Tables[0].Rows.Count > 0)
        //    {
        //        foreach (DataRow dr in ds1.Tables[0].Rows)
        //        {
        //            AllCourseBind.Add(new SelectListItem
        //            {
        //                Text = dr["CourseName"].ToString(),
        //                Value = dr["Coursecode"].ToString()
        //            });
        //        }
        //    }
        //    ViewBag.AllCourseBind = AllCourseBind;

        //}
        [HttpGet]
        public async Task<ActionResult> BatchScheduleMainViewAsyncST()
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                await AllCourseBind();
                //await GetCourceAsyncST();
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
             {
              new BreadcrumbItem { Name = "CoordinatorDashboard", Url = Url.Action("CoordinatorDashboardPRAsync","Coordinator")},
              new BreadcrumbItem { Name = "BatchSchedule", Url = Url.Action("BatchScheduleMainViewAsyncST")}
              };
                ViewBag.Breadcrumbs = breadcrumbs;
                return await Task.Run(() => View());
            }
        }



        /// <summary>
        /// This view get all Scheduled batches data  on list.
        /// </summary>
        /// <returns> Batch List</returns>
        [HttpGet]
        public async Task<ActionResult> ListScheduledBatchAsyncST(string CourseCode)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Coordinator ObjCo = new Coordinator();
                ObjCo.BranchCode = Session["BranchCode"].ToString();
                ObjCo.CourseCode = CourseCode;
                Session["CourseCode"] = CourseCode;
                DataSet ds = new DataSet();
                ds = await objbal.ListScheduledBatchAsyncST(ObjCo);
                List<Coordinator> lstBatchData1 = new List<Coordinator>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Coordinator coObj = new Coordinator();
                    coObj.ScheduleId = Convert.ToInt32(ds.Tables[0].Rows[i]["ScheduleId"].ToString());
                    coObj.BatchCode = ds.Tables[0].Rows[i]["BatchCode"].ToString();
                    coObj.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    coObj.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    coObj.StaffName = ds.Tables[0].Rows[i]["StaffName"].ToString();
                    coObj.LabName = ds.Tables[0].Rows[i]["LabName"].ToString();
                    coObj.NoOfStudent = Convert.ToInt32(ds.Tables[0].Rows[i]["NoOfStudent"].ToString());
                    coObj.BatchScheduleDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["BatchScheduleDate"].ToString());
                    coObj.StartDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"].ToString());
                    coObj.StartTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartTime"].ToString());
                    coObj.EndTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["EndTime"].ToString());

                    coObj.Status = ds.Tables[0].Rows[i]["Status"].ToString();

                    lstBatchData1.Add(coObj);
                }
                ObjCo.lstBatchData = lstBatchData1;
                return PartialView("ListScheduledBatchAsyncST", ObjCo);
            }
        }

        /// <summary>
        ///  This view use to save new created batch Schedule.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> BatchScheduleAsyncST()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await GetBatchAsyncST();
                //return View();
                return PartialView("BatchScheduleAsyncST");
            }
        }

        [HttpPost]
        public async Task<ActionResult> BatchScheduleAsyncST(Coordinator ObjCo)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                ObjCo.BatchScheduleDate = DateTime.Now;
                ObjCo.StaffCode = Session["StaffCode"].ToString();
                ObjCo.EndTime = Convert.ToDateTime(ObjCo.EndTime).AddMinutes(-15);
                await objbal.BatchScheduleAsyncST(ObjCo);
                return RedirectToAction("BatchScheduleMainViewAsyncST");
            }
        }

        /// <summary>
        /// This JsonResult get New Created Batch.
        /// </summary>
        /// <returns> Batch list</returns>
        public async Task<JsonResult> GetBatchAsyncST()
        {
            DataSet ds = new DataSet();
            ds = await objbal.GetBatchAsyncST();
            List<SelectListItem> BatchList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                BatchList.Add(new SelectListItem
                {
                    Text = dr["BatchName"].ToString(),
                    Value = dr["BatchCode"].ToString()
                });
            }
            ViewBag.Batch = BatchList;
            return Json(BatchList, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// This method get CourseDuration On Creted new batch Schedule.
        /// </summary>
        /// <returns> Batch list</returns>
        [HttpGet]
        public async Task<JsonResult> GetDurationST(string batchcode)
        {
            Coordinator ObjCo = new Coordinator();
            ObjCo.BatchCode = batchcode;
            ObjCo.BranchCode = Session["BranchCode"].ToString();
            DataSet ds = new DataSet();
            ds = await objbal.GetCourceDurationST(ObjCo);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Coordinator coObj = new Coordinator();
                coObj.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                coObj.CourseCode = ds.Tables[0].Rows[i]["Coursecode"].ToString();
                coObj.Duration = ds.Tables[0].Rows[i]["Duration"].ToString();
                coObj.CourseDuration = ds.Tables[0].Rows[i]["CourseDuration"].ToString();
                ObjCo = coObj;
            }
            return Json(ObjCo, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This view get all Pendding batches data  on list.
        /// </summary>
        /// <returns> Batch List</returns>
        [HttpGet]
        public async Task<ActionResult> ListPenddingBatchAsyncST(string CourseCode)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Coordinator ObjCo = new Coordinator();
                ObjCo.BranchCode = Session["BranchCode"].ToString();
                ObjCo.CourseCode = CourseCode;
                DataSet ds = new DataSet();
                ds = await objbal.ListPenddingBatchAsyncST(ObjCo);
                List<Coordinator> lstBatchData1 = new List<Coordinator>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Coordinator coObj = new Coordinator();
                    coObj.ScheduleId = Convert.ToInt32(ds.Tables[0].Rows[i]["ScheduleId"].ToString());
                    coObj.BatchCode = ds.Tables[0].Rows[i]["BatchCode"].ToString();
                    coObj.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    coObj.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    coObj.StaffName = ds.Tables[0].Rows[i]["StaffName"].ToString();
                    coObj.LabName = ds.Tables[0].Rows[i]["LabName"].ToString();
                    coObj.NoOfStudent = Convert.ToInt32(ds.Tables[0].Rows[i]["NoOfStudent"].ToString());
                    coObj.BatchScheduleDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["BatchScheduleDate"].ToString());
                    coObj.StartDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"].ToString());
                    coObj.StartTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartTime"].ToString());
                    coObj.EndTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["EndTime"].ToString());
                    coObj.Status = ds.Tables[0].Rows[i]["Status"].ToString();

                    lstBatchData1.Add(coObj);
                }
                ObjCo.lstBatchData = lstBatchData1;
                return PartialView("ListPenddingBatchAsyncST", ObjCo);
            }
        }

        /// <summary>
        /// This view get all Assign batches data  on list.
        /// </summary>
        /// <returns> Batch List</returns>
        [HttpGet]
        public async Task<ActionResult> ListAssignBatchAsyncST(string CourseCode)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Coordinator ObjCo = new Coordinator();
                ObjCo.BranchCode = Session["BranchCode"].ToString();
                ObjCo.CourseCode = CourseCode;
                DataSet ds = new DataSet();
                ds = await objbal.ListAssignBatchAsyncST(ObjCo);
                List<Coordinator> lstBatchData1 = new List<Coordinator>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Coordinator coObj = new Coordinator();
                    coObj.ScheduleId = Convert.ToInt32(ds.Tables[0].Rows[i]["ScheduleId"].ToString());
                    coObj.BatchCode = ds.Tables[0].Rows[i]["BatchCode"].ToString();
                    coObj.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    coObj.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    coObj.StaffName = ds.Tables[0].Rows[i]["StaffName"].ToString();
                    coObj.LabName = ds.Tables[0].Rows[i]["LabName"].ToString();
                    coObj.NoOfStudent = Convert.ToInt32(ds.Tables[0].Rows[i]["NoOfStudent"].ToString());
                    coObj.BatchScheduleDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["BatchScheduleDate"].ToString());
                    coObj.StartDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"].ToString());
                    coObj.StartTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartTime"].ToString());
                    coObj.EndTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["EndTime"].ToString());
                    coObj.Status = ds.Tables[0].Rows[i]["Status"].ToString();

                    lstBatchData1.Add(coObj);
                }
                ObjCo.lstBatchData = lstBatchData1;
                return PartialView("ListAssignBatchAsyncST", ObjCo);
            }
        }

        /// <summary>
        ///This view use to get batch Schedule data for View Only.
        /// </summary>
        /// <returns> Batch Schedule Data</returns>
        [HttpGet]
        public async Task<ActionResult> DetailsBatchScheduleAsyncST(int ScheduleId)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Coordinator ObjCo = new Coordinator();
                ObjCo.ScheduleId = ScheduleId;
                ObjCo.BranchCode = Session["BranchCode"].ToString();
                SqlDataReader dr;
                dr = await objbal.DetailsBatchScheduleAsyncST(ObjCo);
                while (dr.Read())
                {
                    ObjCo.BatchCode = dr["BatchCode"].ToString();
                    ObjCo.BatchName = dr["BatchName"].ToString();
                    ObjCo.CourseName = dr["CourseName"].ToString();
                    ObjCo.StaffName = dr["StaffName"].ToString();
                    ObjCo.LabName = dr["LabName"].ToString();
                    ObjCo.NoOfStudent = Convert.ToInt32(dr["NoOfStudent"].ToString());
                    ObjCo.BatchScheduleDate = Convert.ToDateTime(dr["BatchScheduleDate"].ToString());
                    ObjCo.StartDate = Convert.ToDateTime(dr["StartDate"].ToString());
                    ObjCo.EndDate = Convert.ToDateTime(dr["EndDate"].ToString());
                    ObjCo.StartTime = Convert.ToDateTime(dr["StartTime"].ToString());
                    ObjCo.EndTime = Convert.ToDateTime(dr["EndTime"].ToString());
                    ObjCo.Status = dr["Status"].ToString();
                }
                dr.Close();
                return PartialView("DetailsBatchScheduleAsyncST", ObjCo);
            }
        }
        /// <summary>
        ///This method use to get batch Schedule data for update.
        /// </summary>
        /// <returns> string BatchCode</returns>
        [HttpGet]
        public async Task<ActionResult> UpdateBatchScheduleAsyncST(int ScheduleId)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await GetStatusAsyncST();
                Coordinator ObjCo = new Coordinator();
                ObjCo.ScheduleId = ScheduleId;
                ObjCo.BranchCode = Session["BranchCode"].ToString();
                SqlDataReader dr;
                dr = await objbal.GetBatchScheduleDetailsUpdateAsyncST(ObjCo);
                while (dr.Read())
                {
                    ObjCo.ScheduleId = Convert.ToInt32(dr["ScheduleId"].ToString());
                    ObjCo.LabScheduleId = Convert.ToInt32(dr["LabScheduleId"].ToString());
                    ObjCo.BatchCode = dr["BatchCode"].ToString();
                    ObjCo.BatchName = dr["BatchName"].ToString();
                    ObjCo.CourseName = dr["CourseName"].ToString();
                    ObjCo.StaffCode = dr["StaffCode"].ToString();
                    ObjCo.StaffName = dr["StaffName"].ToString();
                    ObjCo.LabName = dr["LabCode"].ToString();
                    ObjCo.LabCode = dr["LabName"].ToString();
                    ObjCo.BatchScheduleDate = Convert.ToDateTime(dr["BatchScheduleDate"].ToString());
                    ObjCo.StartDate = Convert.ToDateTime(dr["StartDate"].ToString());
                    ObjCo.EndDate = Convert.ToDateTime(dr["EndDate"].ToString());
                    ObjCo.StartTime1 = Convert.ToDateTime(dr["StartTime"].ToString()).ToString("h:mm tt");
                    DateTime originalEndTime = Convert.ToDateTime(dr["EndTime"].ToString());
                    DateTime updatedEndTime = originalEndTime.AddMinutes(15);
                    ObjCo.EndTime1 = updatedEndTime.ToString("h:mm tt");
                    ObjCo.StatusId = Convert.ToInt32(dr["StatusId"].ToString());
                }
                dr.Close();
                ViewBag.StartTime = ObjCo.StartTime1;
                ViewBag.EndTime = ObjCo.EndTime1;

                ViewBag.LabName = ObjCo.LabName;
                ViewBag.LabCode = ObjCo.LabCode;
                Session["LabName"] = ObjCo.LabName;

                ViewBag.StaffCode = ObjCo.StaffCode;
                ViewBag.StaffName = ObjCo.StaffName;

                return PartialView("UpdateBatchScheduleAsyncST", ObjCo);
            }
        }
        /// <summary>
        /// This view use to save Updated Batch Schedule Data.
        /// </summary>
        /// <returns> </returns>
        [HttpPost]
        public async Task<ActionResult> UpdateBatchScheduleAsyncST(Coordinator ObjCo)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                if (ObjCo.LabName == null)
                {
                    ObjCo.LabName = Session["LabName"].ToString();
                }
                else
                {
                    ObjCo.LabName = ObjCo.LabName;
                }
                ObjCo.AddedStaffCode = Session["StaffCode"].ToString();
                ObjCo.EndTime = Convert.ToDateTime(ObjCo.EndTime).AddMinutes(-15);
                await objbal.UpdateBatchScheduleAsyncST(ObjCo);
                return RedirectToAction("BatchScheduleMainViewAsyncST");
            }

        }

        /// <summary>
        /// This JsonResult get Status.
        /// </summary>
        /// <returns> Status list</returns>
        public async Task<JsonResult> GetStatusAsyncST()
        {
            DataSet ds = new DataSet();
            ds = await objbal.GetStatusAsyncST();
            List<SelectListItem> StatusList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                StatusList.Add(new SelectListItem
                {
                    Text = dr["Status"].ToString(),
                    Value = dr["StatusId"].ToString()
                });
            }
            ViewBag.Status = StatusList;
            return Json(StatusList, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// This method Read Available Labs List.
        /// </summary>
        /// <returns> Status list</returns>
        [HttpPost]
        public async Task<JsonResult> ReadAvailableLabsBatch_BindST(Coordinator objC)
        {
            objC.BranchCode = Session["BranchCode"].ToString();
            DataTable dt = new DataTable();
            DataSet ds1 = new DataSet();
            ds1 = await objbal.ReadAvailableLabsST(objC);
            dt = ds1.Tables[0];
            var Jsondata = JsonConvert.SerializeObject(dt);
            return Json(Jsondata);

        }
        /// <summary>
        /// This method get Lab List.
        /// </summary>
        /// <returns> Lab list</returns>
        [HttpGet]
        public async Task<JsonResult> GetLabAsyncST(string BatchTime)
        {
            Coordinator ObjCo = new Coordinator();
            ObjCo.BatchTime = BatchTime;
            DataSet ds = new DataSet();
            ds = await objbal.GetLabAsyncST(ObjCo);
            List<SelectListItem> LabList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                LabList.Add(new SelectListItem
                {
                    Text = dr["LabName"].ToString(),
                    Value = dr["LabCode"].ToString()
                });
            }
            return Json(LabList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This method get Trainer List.
        /// </summary>
        /// <returns> Trainer list</returns>
        [HttpGet]
        public async Task<JsonResult> GetTrainerAsyncST(string CourseCode)
        {
            Coordinator ObjCo = new Coordinator();
            ObjCo.CourseCode = CourseCode;
            ObjCo.BranchCode = Session["BranchCode"].ToString();
            DataSet ds = new DataSet();
            ds = await objbal.GetTrainerAsyncST(ObjCo);
            List<SelectListItem> TrainerList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TrainerList.Add(new SelectListItem
                {
                    Text = dr["StaffName"].ToString(),
                    Value = dr["StaffCode"].ToString()

                });
            }
            ViewBag.Trainer = TrainerList;
            return Json(TrainerList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///This method use to get batch Schedule data for Reschedule batch.
        /// </summary>
        /// <returns> string BatchCode</returns>
        [HttpGet]
        public async Task<ActionResult> BatchRescheduleAsyncST(int ScheduleId)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await GetStatusAsyncST();
                Coordinator ObjCo = new Coordinator();
                ObjCo.ScheduleId = ScheduleId;
                ObjCo.BranchCode = Session["BranchCode"].ToString();
                SqlDataReader dr;
                dr = await objbal.GetBatchScheduleDetailsUpdateAsyncST(ObjCo);
                while (dr.Read())
                {
                    ObjCo.ScheduleId = Convert.ToInt32(dr["ScheduleId"].ToString());
                    ObjCo.LabScheduleId = Convert.ToInt32(dr["LabScheduleId"].ToString());
                    ObjCo.BatchCode = dr["BatchCode"].ToString();
                    ObjCo.BatchName = dr["BatchName"].ToString();
                    ObjCo.CourseName = dr["CourseName"].ToString();
                    ObjCo.StaffCode = dr["StaffCode"].ToString();
                    ObjCo.LabName = dr["LabCode"].ToString();
                    ObjCo.LabCode = dr["LabName"].ToString();
                    ObjCo.BatchScheduleDate = Convert.ToDateTime(dr["BatchScheduleDate"].ToString());
                    ObjCo.StartDate = Convert.ToDateTime(dr["StartDate"].ToString());
                    ObjCo.EndDate = Convert.ToDateTime(dr["EndDate"].ToString());
                    ObjCo.StartTime1 = Convert.ToDateTime(dr["StartTime"].ToString()).ToString("h:mm tt");
                    DateTime originalEndTime = Convert.ToDateTime(dr["EndTime"].ToString());
                    DateTime updatedEndTime = originalEndTime.AddMinutes(15);
                    ObjCo.EndTime1 = updatedEndTime.ToString("h:mm tt");
                    ObjCo.StatusId = Convert.ToInt32(dr["StatusId"].ToString());
                    ObjCo.RescheduleDate = DateTime.Now;
                }
                dr.Close();
                ViewBag.StartTime = ObjCo.StartTime1;
                ViewBag.EndTime = ObjCo.EndTime1;
                ViewBag.LabCode = ObjCo.LabName;
                ViewBag.LabName = ObjCo.LabCode;
                Session["LabName"] = ObjCo.LabName;
                return PartialView("BatchRescheduleAsyncST", ObjCo);
            }
        }
        /// <summary>
        /// This view use to save Updated Batch  Reschedule Data.
        /// </summary>
        /// <returns> </returns>
        [HttpPost]
        public async Task<ActionResult> BatchRescheduleAsyncST(Coordinator ObjCo)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                if (ObjCo.LabName == null)
                {
                    ObjCo.LabName = Session["LabName"].ToString();
                }
                else
                {
                    ObjCo.LabName = ObjCo.LabName;
                }
                ObjCo.StaffCode = Session["StaffCode"].ToString();
                ObjCo.StatusId = 35;

                await objbal.BatchRescheduleAsyncST(ObjCo);
                return RedirectToAction("BatchScheduleMainViewAsyncST");
            }
        }


        //-----------------Sayali batch Schedule End ---------------------------------------------------------------------//
        //-----------------Kirti Attendanse follow up Start --------------------------------------------------------------//
        /// <summary>
        /// In this method bind the all courses list.
        /// </summary>
        /// <returns>Returns the all courses list through viewbag.</returns>
        public async Task<ActionResult> CourseBindKKAsync()
        {
            Coordinator objc = new Coordinator();
            objc.BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await objbal.ShowCourseKKAsync(objc);
            List<SelectListItem> courselist = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                courselist.Add(new SelectListItem { Text = dr["CourseName"].ToString(), Value = dr["CourseCode"].ToString() });
            }
            ViewBag.Course = courselist;
            return Json(courselist, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// This method is for gets the values from database.
        /// </summary>
        /// <returns>returns values of all columns which i get.</returns>
        [HttpGet]
        public async Task<ActionResult> ListShowAttendanceFollowUpKKAsync()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await CourseBindKKAsync();
                Coordinator objdate = new Coordinator();
                objdate.BranchCode = Session["BranchCode"].ToString();
                //objdate.StartDate = new DateTime(2022,1,1);
                //objdate.EndDate =DateTime.Now ;
                DataSet ds = await objbal.GetAttendanceFollowupKKAsync(objdate);
                Coordinator objlst = new Coordinator();
                List<Coordinator> lstf = new List<Coordinator>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Coordinator objc = new Coordinator();
                    objc.StudentCode = ds.Tables[0].Rows[i]["CandidateCode"].ToString();
                    objc.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    //objc.BatchCode = ds.Tables[0].Rows[i]["BatchCode"].ToString();
                    objc.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    objc.TrainerName = ds.Tables[0].Rows[i]["StaffName"].ToString();
                    objc.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                    objc.ContactNumber = ds.Tables[0].Rows[i]["ContactNumber"].ToString();
                    objc.Emailid = ds.Tables[0].Rows[i]["EmailId"].ToString();
                    objc.AbsentDays = Convert.ToInt32(ds.Tables[0].Rows[i]["AbsentDays"].ToString());
                    lstf.Add(objc);
                }
                objlst.lstFollowup = lstf;
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                    new BreadcrumbItem { Name = "CoordinatorDashboard", Url =Url.Action("CoordinatorDashboardPRAsync","Coordinator")},
                   new BreadcrumbItem { Name = "ListShowAttendanceFollowUp", Url = Url.Action("ListShowAttendanceFollowUpKKAsync","Coordinator")}
                };

                ViewBag.Breadcrumbs = breadcrumbs;
                return await Task.Run(() => View(objlst));
            }
        }
        /// <summary>
        /// This is method for filtering the data from datewise.
        /// </summary>
        /// <param name="startDate">This parameter showing the default start date before 1 year.</param>
        /// <param name="endDate">This parameter showing present date.</param>
        /// <returns>It returns the list.</returns>
        [HttpGet]
        public async Task<ActionResult> ListShowAttendanceFollowUpAsync(string startDate, string endDate)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await CourseBindKKAsync();
                Coordinator objdate = new Coordinator();
                objdate.BranchCode = Session["BranchCode"].ToString();
                objdate.StartDate = DateTime.Parse(startDate);
                objdate.EndDate = DateTime.Parse(endDate);
                DataSet ds = await objbal.GetAttendanceFollowupKKAsync(objdate);
                Coordinator objlst = new Coordinator();
                List<Coordinator> lstf = new List<Coordinator>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Coordinator objc = new Coordinator();
                    objc.StudentCode = ds.Tables[0].Rows[i]["CandidateCode"].ToString();
                    objc.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objc.BatchCode = ds.Tables[0].Rows[i]["BatchCode"].ToString();
                    objc.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    objc.TrainerName = ds.Tables[0].Rows[i]["StaffName"].ToString();
                    objc.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                    objc.ContactNumber = ds.Tables[0].Rows[i]["ContactNumber"].ToString();
                    objc.Emailid = ds.Tables[0].Rows[i]["EmailId"].ToString();
                    lstf.Add(objc);
                }
                objlst.lstFollowup = lstf;
                return PartialView("ListAttendanceFollowupDateFilter", objlst);
            }
        }

        /// <summary>
        /// This method is for adding the new followup of absent student.
        /// </summary>
        /// <param name="id">The id is use for the getting the selected student data like his studcode,email etc.</param>
        /// <returns>Returns the detail of the student data.</returns>
        [HttpGet]
        public async Task<ActionResult> AddFollowupKKAsync(string CandidateCode)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Coordinator objadd = new Coordinator();
                objadd.StudentCode = CandidateCode;
                objadd.BranchCode = Session["BranchCode"].ToString();
                DataSet ds;
                ds = await objbal.AddAttendanceFollowupKKAsync(objadd);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    objadd.StudentCode = ds.Tables[0].Rows[i]["CandidateCode"].ToString();
                    objadd.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                    objadd.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objadd.AlternateNumber = ds.Tables[0].Rows[i]["AlternateContact"].ToString();
                    objadd.DateofJoin = DateTime.Today;
                    objadd.NextFollowUpDate = DateTime.Today;
                }
                await StatusBindKKAsync();
                await StaffBindKKAsync();
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                   new BreadcrumbItem { Name = "CoordinatorDashboard", Url = Url.Action("CoordinatorDashboardPRAsync","Coordinator")},
                   new BreadcrumbItem { Name = "ListShowAttendanceFollowUp", Url = Url.Action("ListShowAttendanceFollowUpKKAsync")},
                   new BreadcrumbItem { Name = "AddFollowup", Url = Url.Action("AddFollowupKKAsync")}
                };

                ViewBag.Breadcrumbs = breadcrumbs;
                return View(objadd);
            }

        }
        /// <summary>
        /// This method is for bind the statusnames.
        /// </summary>
        public async Task StatusBindKKAsync()
        {
            DataSet ds = await objbal.ShowStatusKKAsync();
            List<SelectListItem> Statuslist = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Statuslist.Add(new SelectListItem { Text = dr["Status"].ToString(), Value = dr["StatusId"].ToString() });
            }
            ViewBag.Status = Statuslist;
        }
        /// <summary>
        /// This method is for Bind the Staffnames.
        /// </summary>
        public async Task StaffBindKKAsync()
        {
            Coordinator objst = new Coordinator();
            objst.BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await objbal.ShowStaffKKAsync(objst);
            List<SelectListItem> Stafflist = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Stafflist.Add(new SelectListItem { Text = dr["StaffName"].ToString(), Value = dr["StaffCode"].ToString() });
            }
            ViewBag.Staff = Stafflist;
        }
        /// <summary>
        /// This method is for savings the new followup data of student.
        /// </summary>
        /// <param name="objc">The object is use for the access property from property class.</param>
        /// <returns>Saving all the followup details.</returns>
        [HttpPost]
        public async Task<ActionResult> AddFollowupKKAsync(Coordinator objc)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await objbal.SaveFollowUpDataKKAsync(objc);
                return RedirectToAction("ListViewFollowupKKAsync");
            }

        }
        /// <summary>
        /// This Method is showing all the pending and completed followuplist.
        /// </summary>
        /// <returns>returns the list</returns>
        [HttpGet]
        public async Task<ActionResult> ListViewFollowupKKAsync()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Coordinator objf = new Coordinator();
                objf.BranchCode = Session["BranchCode"].ToString();
                DataSet ds = await objbal.ShowAllFollowupKKAsync(objf);
                Coordinator objlist = new Coordinator();
                List<Coordinator> lstallf = new List<Coordinator>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Coordinator objfollowup = new Coordinator();
                    objfollowup.FollowUpId = Convert.ToInt32(ds.Tables[0].Rows[i]["FollowUpId"].ToString());
                    objfollowup.StudentCode = ds.Tables[0].Rows[i]["CandidateCode"].ToString();
                    objfollowup.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objfollowup.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    objfollowup.TrainerName = ds.Tables[0].Rows[i]["TrainerName"].ToString();
                    objfollowup.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                    objfollowup.Emailid = ds.Tables[0].Rows[i]["EmailId"].ToString();
                    DateTime Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["FollowUpTakenDate"].ToString());
                    objfollowup.FTakenDate = Date.ToString("dd-MM-yyyy");
                    DateTime NextFollowupdate = Convert.ToDateTime(ds.Tables[0].Rows[i]["NextFollowUpDate"].ToString());
                    objfollowup.NextFoDate = NextFollowupdate.ToString("dd-MM-yyyy");
                    objfollowup.FollowUpNote = ds.Tables[0].Rows[i]["FollowUpNote"].ToString();
                    objfollowup.StatusName = ds.Tables[0].Rows[i]["Status"].ToString();
                    if (ds.Tables[0].Rows[i]["Date_join_installment"].ToString() != null && ds.Tables[0].Rows[i]["Date_join_installment"].ToString() != "")
                    {
                        DateTime DateofJoin = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date_join_installment"].ToString());
                        objfollowup.DofJoin = DateofJoin.ToString("dd-MM-yyyy");
                    }
                    //DateTime DateofJoin = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date_join_installment"].ToString());
                    //objfollowup.DofJoin = DateofJoin.ToString("dd-MM-yyyy");
                    objfollowup.FollowUpTakenBy = ds.Tables[0].Rows[i]["StaffName"].ToString();
                    lstallf.Add(objfollowup);
                }
                objlist.lstAllFollowup = lstallf;
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                   new BreadcrumbItem { Name = "CoordinatorDashboard", Url = Url.Action("CoordinatorDashboardPRAsync","Coordinator")},
                   new BreadcrumbItem { Name = "ListShowAttendanceFollowUp", Url = Url.Action("ListShowAttendanceFollowUpKKAsync","Coordinator")},
                   new BreadcrumbItem { Name = "ViewFollowup", Url = Url.Action("ListViewFollowupKKAsync","Coordinator")}
                };

                ViewBag.Breadcrumbs = breadcrumbs;
                return View("ListViewFollowupKKAsync", objlist);
            }
        }
        /// <summary>
        /// This method is showing all only Not reachable followup students list.
        /// </summary>
        /// <returns>returns the list</returns>
        [HttpGet]
        public async Task<ActionResult> ListPendingFollowupKKAsync()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Coordinator objfb = new Coordinator();
                objfb.BranchCode = Session["BranchCode"].ToString();
                DataSet ds = await objbal.ShowPendingFollowupKKAsync(objfb);
                Coordinator objpenlist = new Coordinator();
                List<Coordinator> lstallp = new List<Coordinator>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Coordinator objp = new Coordinator();
                    objp.FollowUpId = Convert.ToInt32(ds.Tables[0].Rows[i]["FollowUpId"].ToString());
                    objp.StudentCode = ds.Tables[0].Rows[i]["CandidateCode"].ToString();
                    objp.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objp.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    objp.TrainerName = ds.Tables[0].Rows[i]["TrainerName"].ToString();
                    objp.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                    DateTime Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["FollowUpTakenDate"].ToString());
                    objp.FTakenDate = Date.ToString("dd-MM-yyyy");
                    DateTime NextFollowUpDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["NextFollowUpDate"].ToString());
                    objp.NextFoDate = NextFollowUpDate.ToString("dd-MM-yyyy");
                    objp.FollowUpNote = ds.Tables[0].Rows[i]["FollowUpNote"].ToString();
                    objp.StatusName = ds.Tables[0].Rows[i]["Status"].ToString();
                    if (ds.Tables[0].Rows[i]["Date_join_installment"].ToString() != null && ds.Tables[0].Rows[i]["Date_join_installment"].ToString() != "")
                    {
                        DateTime DateofJoin = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date_join_installment"].ToString());
                        objp.DofJoin = DateofJoin.ToString("dd-MM-yyyy");
                    }
                    objp.FollowUpTakenBy = ds.Tables[0].Rows[i]["StaffName"].ToString();
                    lstallp.Add(objp);
                }
                objpenlist.lstPending = lstallp;
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                  new BreadcrumbItem { Name = "CoordinatorDashboard", Url = Url.Action("CoordinatorDashboardPRAsync","Coordinator")},
                  new BreadcrumbItem { Name = "ListShowAttendanceFollowUp", Url = Url.Action("ListShowAttendanceFollowUpKKAsync")},
                  new BreadcrumbItem { Name = "AddFollowup", Url = Url.Action("AddFollowupKKAsync")},
                  new BreadcrumbItem { Name = "ViewFollowup", Url = Url.Action("ListViewFollowupKKAsync")},
                  new BreadcrumbItem { Name = "PendingFollowup", Url = Url.Action("ListPendingFollowupKKAsync")}
                };
                ViewBag.Breadcrumbs = breadcrumbs;
                return View(objpenlist);
            }
        }
        /// <summary>
        /// This method is fetching all the details from addfollowup form for update data.
        /// </summary>
        /// <param name="id">Id is use for showing selected students data.</param>
        /// <returns>Returns the all fetching details.</returns>
        [HttpGet]
        public async Task<ActionResult> UpdateFollowupKKAsync(int FollowUpId)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await StatusBindKKAsync();
                await StaffBindKKAsync();
                Coordinator objadd = new Coordinator();
                objadd.FollowUpId = FollowUpId;
                DataSet ds;
                ds = await objbal.AddViewAttendanceFollowupKKAsync(objadd);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    objadd.FollowUpId = Convert.ToInt32(ds.Tables[0].Rows[i]["FollowUpId"].ToString());
                    objadd.StudentCode = ds.Tables[0].Rows[i]["CandidateCode"].ToString();
                    objadd.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                    objadd.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objadd.AlternateNumber = ds.Tables[0].Rows[i]["AlternateContact"].ToString();
                    objadd.FollowUpNote = ds.Tables[0].Rows[i]["FollowUpNote"].ToString();
                    objadd.FollowUpTakenDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["FollowUpTakenDate"].ToString());
                    objadd.NextFollowUpDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["NextFollowUpDate"].ToString());
                    objadd.StatusName = ds.Tables[0].Rows[i]["Statusid"].ToString();
                    objadd.DateofJoin = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date_join_installment"].ToString());
                    objadd.FollowUpTaken = ds.Tables[0].Rows[i]["FollowUpTakenBy_StaffCode"].ToString();
                }
                return PartialView("UpdateFollowupKKAsync", objadd);
            }
        }
        /// <summary>
        /// Updating the followup data.
        /// </summary>
        /// <param name="objadd">This object is use for access the property from the property class.</param>
        /// <returns>Returns the update data which we updated.</returns>
        [HttpPost]
        public async Task<ActionResult> UpdateFollowupKKAsync(Coordinator objadd)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await objbal.UpdateFollowupKKAsync(objadd);
                return RedirectToAction("ListViewFollowupKKAsync");
            }
        }

        /// <summary>
        /// This method is for showing the single student total followup.
        /// </summary>
        /// <param name="StudCode">The studcode use for fetching the data of single student.</param>
        /// <returns>It Returns the list of history of followup.</returns>
        [HttpGet]
        public async Task<ActionResult> ListFollowupHistoryKKAsync(string StudCode)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Coordinator objh = new Coordinator();
                objh.StudentCode = StudCode;
                objh.BranchCode = Session["BranchCode"].ToString();
                List<Coordinator> lsth = new List<Coordinator>();
                DataSet ds;
                ds = await objbal.ViewHistoryDetailsKKAsync(objh);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Coordinator objCo = new Coordinator();
                    objh.StudentCode = ds.Tables[0].Rows[i]["CandidateCode"].ToString();
                    objh.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objh.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    objh.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                    objh.ContactNumber = ds.Tables[0].Rows[i]["ContactNumber"].ToString();
                    objCo.FollowUpTakenDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["FollowUpTakenDate"].ToString());
                    objCo.NextFollowUpDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["NextFollowUpDate"].ToString());
                    objCo.FollowUpNote = ds.Tables[0].Rows[i]["FollowUpNote"].ToString();
                    objCo.FollowUpId = Convert.ToInt32(ds.Tables[0].Rows[i]["FollowUpId"].ToString());
                    objh.FollowupCount = Convert.ToInt32(ds.Tables[0].Rows[i]["FollowUpCount"].ToString());
                    objCo.FollowUpTakenBy = ds.Tables[0].Rows[i]["StaffName"].ToString();
                    lsth.Add(objCo);
                }
                objh.lstHistory = lsth;
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                 new BreadcrumbItem { Name = "CoordinatorDashboard", Url = Url.Action("CoordinatorDashboardPRAsync","Coordinator")},
                  new BreadcrumbItem { Name = "ListShowAttendanceFollowUp", Url = Url.Action("ListShowAttendanceFollowUpKKAsync")},
                  new BreadcrumbItem { Name = "Add Followup", Url = Url.Action("AddFollowupKKAsync")},
                  new BreadcrumbItem { Name = "View Followup", Url = Url.Action("ListViewFollowupKKAsync")},
                  new BreadcrumbItem { Name = "Followup History", Url = Url.Action("ListFollowupHistoryKKAsync")}
                };
                ViewBag.Breadcrumbs = breadcrumbs;
                return View("ListFollowupHistoryKKAsync", objh);
            }
        }


        //-----------------Kirti Attendanse follow up  End ---------------------------------------------------------------//
        //-----------------Vaibhav Test Management Start -----------------------------------------------------------------//
        /// <summary>
        /// This method is used for to show grid view of section list to trainer in batch sedule.
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public async Task<ActionResult> DetailsTestManagementAsyncVP()
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {

                await AllCourseBind();

                //Coordinator obj = new Coordinator();
                //obj.StartDate = new DateTime(2022, 1, 1);
                //obj.EndDate = DateTime.Now;
                //ViewBag.StartDate = obj.StartDate;
                //ViewBag.EndDate = obj.EndDate;
                Coordinator ojj = new Coordinator();
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                    new BreadcrumbItem { Name = "CoordinatorDashboard", Url =Url.Action("CoordinatorDashboardPRAsync","Coordinator")},
                    new BreadcrumbItem { Name = "Test Management", Url =Url.Action("DetailsTestManagementAsyncVP","Coordinator") }
                };

                ViewBag.Breadcrumbs = breadcrumbs;

                return await Task.Run(() => View("DetailsTestManagementAsyncVP"));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> DetailsArrangeTestListAsyncVP(string CourseCode, DateTime startDate, DateTime endDate)
        {
            if (Session["BranchCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                Coordinator obj = new Coordinator();
                obj.CourseCode = CourseCode;
                obj.StartDate = startDate;
                obj.EndDate = endDate;
                obj.BranchCode = Session["BranchCode"].ToString();
                DataSet ds = await objbal.ArrangeTestList(obj);
                Coordinator objdetail = new Coordinator();
                List<Coordinator> lstArrangeTestGrid = new List<Coordinator>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Coordinator objurser = new Coordinator();
                    objurser.AssignTestId = Convert.ToInt32(ds.Tables[0].Rows[i]["AssignTestId"].ToString());
                    objurser.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objurser.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    objurser.TestName = ds.Tables[0].Rows[i]["TestName"].ToString();
                    objurser.TestDateVP = ds.Tables[0].Rows[i]["TestDate"].ToString();
                    objurser.TestTimeMAnage = ds.Tables[0].Rows[i]["TestTime"].ToString();
                    objurser.CompleteTillDate = ds.Tables[0].Rows[i]["CompleteTillDate"].ToString();
                    objurser.Duration = ds.Tables[0].Rows[i]["Duration"].ToString();
                    objurser.TotalMarks = Convert.ToInt32(ds.Tables[0].Rows[i]["TotalMarks"].ToString());
                    objurser.StaffName = ds.Tables[0].Rows[i]["StaffName"].ToString();
                    objurser.LabName = ds.Tables[0].Rows[i]["LabName"].ToString();
                    lstArrangeTestGrid.Add(objurser);
                }
                objdetail.lstArrangeTestGrid = lstArrangeTestGrid;
                return await Task.Run(() => PartialView("DetailsArrangeTestListAsyncVP", objdetail));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> DetailsAssignTestListAsyncVP(string CourseCode, DateTime startDate, DateTime endDate)
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                Coordinator obj = new Coordinator();
                obj.CourseCode = CourseCode;
                obj.StartDate = startDate;
                obj.EndDate = endDate;
                obj.BranchCode = Session["BranchCode"].ToString();
                DataSet ds = await objbal.AssignTestList(obj);
                Coordinator objdetail = new Coordinator();
                List<Coordinator> lstAssignTestGrid = new List<Coordinator>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Coordinator objurser = new Coordinator();
                    objurser.AssignTestId = Convert.ToInt32(ds.Tables[0].Rows[i]["AssignTestId"].ToString());
                    objurser.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objurser.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    objurser.TestName = ds.Tables[0].Rows[i]["TestName"].ToString();
                    objurser.CompleteTillDate = ds.Tables[0].Rows[i]["CompleteTillDate"].ToString();
                    objurser.Duration = ds.Tables[0].Rows[i]["Duration"].ToString();
                    objurser.TotalMarks = Convert.ToInt32(ds.Tables[0].Rows[i]["TotalMarks"].ToString());
                    //objurser.StaffName = ds.Tables[0].Rows[i]["StaffName"].ToString();
                    //objurser.LabName = ds.Tables[0].Rows[i]["LabName"].ToString();
                    lstAssignTestGrid.Add(objurser);
                }
                objdetail.lstAssignTestGrid = lstAssignTestGrid;
                return await Task.Run(() => PartialView(objdetail));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> DetailsPendingTestListAsyncVP(string CourseCode, DateTime startDate, DateTime endDate)
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                Coordinator obj = new Coordinator();
                obj.CourseCode = CourseCode;
                obj.StartDate = startDate;
                obj.EndDate = endDate;
                obj.BranchCode = Session["BranchCode"].ToString();
                DataSet ds = await objbal.PendingTestList(obj);
                Coordinator objdetail = new Coordinator();
                List<Coordinator> lstPendingTestGrid = new List<Coordinator>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Coordinator objurser = new Coordinator();
                    objurser.AssignTestId = Convert.ToInt32(ds.Tables[0].Rows[i]["AssignTestId"].ToString());
                    objurser.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objurser.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    objurser.TestName = ds.Tables[0].Rows[i]["TestName"].ToString();
                    objurser.CompleteTillDate = ds.Tables[0].Rows[i]["CompleteTillDate"].ToString();
                    objurser.Duration = ds.Tables[0].Rows[i]["Duration"].ToString();
                    objurser.TotalMarks = Convert.ToInt32(ds.Tables[0].Rows[i]["TotalMarks"].ToString());
                    //objurser.StaffName = ds.Tables[0].Rows[i]["StaffName"].ToString();
                    //objurser.LabName = ds.Tables[0].Rows[i]["LabName"].ToString();
                    lstPendingTestGrid.Add(objurser);
                }
                objdetail.lstPendingTestGrid = lstPendingTestGrid;
                return await Task.Run(() => PartialView(objdetail));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> DetailsConductedTestListAsyncVP(string CourseCode, DateTime startDate, DateTime endDate)
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                Coordinator obj = new Coordinator();
                obj.CourseCode = CourseCode;
                obj.StartDate = startDate;
                obj.EndDate = endDate;
                obj.BranchCode = Session["BranchCode"].ToString();
                DataSet ds = await objbal.ConductedTestList(obj);
                Coordinator objdetail = new Coordinator();
                List<Coordinator> lstCouductedTestGrid = new List<Coordinator>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Coordinator objurser = new Coordinator();
                    objurser.AssignTestId = Convert.ToInt32(ds.Tables[0].Rows[i]["AssignTestId"].ToString());
                    objurser.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objurser.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    objurser.TestName = ds.Tables[0].Rows[i]["TestName"].ToString();
                    objurser.TestDateVP = ds.Tables[0].Rows[i]["TestDate"].ToString();
                    objurser.TestTimeMAnage = ds.Tables[0].Rows[i]["TestTime"].ToString();
                    objurser.CompleteTillDate = ds.Tables[0].Rows[i]["CompleteTillDate"].ToString();
                    objurser.Duration = ds.Tables[0].Rows[i]["Duration"].ToString();
                    objurser.TotalMarks = Convert.ToInt32(ds.Tables[0].Rows[i]["TotalMarks"].ToString());
                    objurser.StaffName = ds.Tables[0].Rows[i]["StaffName"].ToString();
                    objurser.LabName = ds.Tables[0].Rows[i]["LabName"].ToString();
                    lstCouductedTestGrid.Add(objurser);
                }
                objdetail.lstCouductedTestGrid = lstCouductedTestGrid;
                return await Task.Run(() => PartialView("DetailsConductedTestListAsyncVP", objdetail));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AssignTestId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> RegisterArrangeTestAsyncVP(int AssignTestId)
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                await ReadsupervisersBatch_Bind();
                Coordinator objedit = new Coordinator();
                objedit.AssignTestId = AssignTestId;
                SqlDataReader dr;
                dr = await objbal.ReadArrageTestVp(objedit);
                while (dr.Read())
                {
                    objedit.AssignTestId = Convert.ToInt32(dr["AssignTestId"].ToString());
                    objedit.CourseName = dr["CourseName"].ToString();
                    objedit.BatchName = dr["BatchName"].ToString();
                    objedit.BatchCode = dr["BatchCode"].ToString();
                    objedit.TestName = dr["TestName"].ToString();
                    objedit.Duration = dr["Duration"].ToString();
                    objedit.StaffName = dr["StaffName"].ToString();
                    objedit.TotalMarks = Convert.ToInt32(dr["TotalMarks"].ToString());
                }
                dr.Close();
                return PartialView("RegisterArrangeTestAsyncVP", objedit);
            }
        }
        /// <summary>
        /// this method is used to saved deatil of session of batch sedule 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> RegisterArrangeTestAsyncVP(Coordinator obj)
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                obj.EndTime = obj.EndTime.AddMinutes(-15);
                obj.ArrangedDate = DateTime.Now;
                obj.StaffCode = Session["StaffCode"].ToString();
                await objbal.SaveArrangeTest(obj);
                return Json(new { success = true, message = "Test Arrange  successfully" });
                // return RedirectToAction("DetailsTestManagementAsyncVP");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task ReadsupervisersBatch_Bind()
        {
            Coordinator obj = new Coordinator();
            obj.BranchCode = Session["BranchCode"].ToString();
            DataSet ds1 = new DataSet();
            ds1 = await objbal.Readsupervisers(obj);
            List<SelectListItem> Readsupervisers = new List<SelectListItem>();
            if (ds1.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds1.Tables[0].Rows)
                {
                    Readsupervisers.Add(new SelectListItem
                    {
                        Text = dr["StaffName"].ToString(),
                        Value = dr["StaffCode"].ToString()
                    });
                }
            }
            ViewBag.Readsupervisers = Readsupervisers;
        }
        public async Task AllTestStausVpStatus_Bind()
        {
            Coordinator obj = new Coordinator();
            DataSet ds1 = new DataSet();
            ds1 = await objbal.AllTestStausVp(obj);
            List<SelectListItem> AllTestStausVp = new List<SelectListItem>();
            if (ds1.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds1.Tables[0].Rows)
                {
                    AllTestStausVp.Add(new SelectListItem
                    {
                        Text = dr["Status"].ToString(),
                        Value = dr["StatusId"].ToString()
                    });
                }
            }
            ViewBag.AllTestStausVp = AllTestStausVp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ReadAvailableLabBatch_Bind(string Batchcode, DateTime startDate, DateTime StartTime, DateTime EndTime)
        {
            Coordinator obj = new Coordinator();
            obj.BranchCode = Session["BranchCode"].ToString();
            obj.BatchCode = Batchcode;
            obj.StartDate = startDate;
            obj.StartTime = StartTime;
            //obj.EndTime = EndTime;
            obj.EndTime = EndTime.AddMinutes(-15);
            DataTable dt = new DataTable();
            DataSet ds1 = new DataSet();
            ds1 = await objbal.ReadAvailableLabs(obj);
            dt = ds1.Tables[0];
            var Jsondata = JsonConvert.SerializeObject(dt);
            return Json(Jsondata);

        }
        [HttpPost]
        public async Task<JsonResult> ReadAvailableLabsBatch_Bind(string Batchcode, string startDate, string StartTime, string EndTime)
        {
            Coordinator obj = new Coordinator();
            obj.BranchCode = Session["BranchCode"].ToString();
            obj.BatchCode = Batchcode;
            obj.StartDate = Convert.ToDateTime(startDate);
            obj.StartTime = Convert.ToDateTime(StartTime);
            obj.EndTime = Convert.ToDateTime(EndTime).AddMinutes(-15);

            DataTable dt = new DataTable();
            DataSet ds1 = new DataSet();
            ds1 = await objbal.ReadAvailableLabs(obj);
            dt = ds1.Tables[0];
            var Jsondata = JsonConvert.SerializeObject(dt);
            return Json(Jsondata);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AssignTestId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> UpdateArrangeTestAsyncVP(int AssignTestId)
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                await ReadsupervisersBatch_Bind();
                await AllTestStausVpStatus_Bind();
                Coordinator objedit = new Coordinator();
                objedit.AssignTestId = AssignTestId;
                SqlDataReader dr;
                dr = await objbal.ReadArrangeTestDetails(objedit);
                while (dr.Read())
                {
                    objedit.AssignTestId = Convert.ToInt32(dr["AssignTestId"].ToString());
                    objedit.StatusId = Convert.ToInt32(dr["StatusId"].ToString());
                    objedit.TestDate = dr["TestDate"].ToString().AsDateTime();
                    objedit.TestTime = dr["TestTime"].ToString().AsDateTime();
                    objedit.BatchCode = dr["BatchCode"].ToString();
                    objedit.Duration = dr["Duration"].ToString();
                    objedit.SupervisorCode = dr["TrainerCodeSupervisorCode"].ToString();
                    //objedit.LabCode = dr["LabCode"].ToString();

                }
                dr.Close();
                ViewBag.TestDate = objedit.TestDate.ToString("yyyy-MM-dd");
                ViewBag.TestTime = objedit.TestTime.ToString("HH:mm");

                return PartialView("UpdateArrangeTestAsyncVP", objedit);
            }
        }
        /// <summary>
        /// this method is used to saved deatil of session of batch sedule 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> UpdateArrangeTestAsyncVP(Coordinator obj)
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                if (obj.LabCode != null && obj.LabCode != "")
                {
                    obj.EndTime = obj.EndTime.AddMinutes(-15);
                    await objbal.ArrageTestupdate(obj);
                }
                else
                {
                    await objbal.ArrageTestStatusupdate(obj);
                }

                //return Json(new { success = true, message = "Test Arrange Updated  successfully" });
                return RedirectToAction("DetailsTestManagementAsyncVP");
            }
        }

        //-----------------Vaibhav Test Management End -------------------------------------------------------------------//
        //-----------------Kirti Demo  Start -------------------------------------------------------------------------------//
        /// <summary>
        /// This method showing the lists of demo.
        /// </summary>
        /// <returns>It returns the all arranged demo list.</returns>
        [HttpGet]
        public async Task<ActionResult> ListDemoKKAsync()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                    new BreadcrumbItem { Name = "CoordinatorDashboard", Url =Url.Action("CoordinatorDashboardPRAsync","Coordinator")},
                   new BreadcrumbItem { Name = "All Demo List", Url = Url.Action("ListDemoKKAsync")}
                };

                ViewBag.Breadcrumbs = breadcrumbs;
                return await Task.Run(() => View());
            }
        }
        [HttpGet]
        /// <summary>
        /// This method is for the showing all recents demo list.
        /// </summary>
        /// <returns>Returns the list.</returns>
        public async Task<ActionResult> ListAllDemoKKAsync(int id = 0)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                List<Coordinator> model = await DemoList(id);
                Coordinator objlist = new Coordinator();
                objlist.lstDemo = model;
                return PartialView("ListAllDemoKKAsync", objlist);
            }
        }
        /// <summary>
        /// This method is showing the demo list.
        /// </summary>
        /// <param name="statusId">This parameter is use for showing selected status demo list.</param>
        /// <returns>It returns the list.</returns>
        [HttpGet]
        private async Task<List<Coordinator>> DemoList(int statusid)
        {
            //await DemoNotificationsKKAsync();
            Coordinator objde = new Coordinator();
            objde.StatusId = statusid;
            objde.BranchCode = Session["BranchCode"].ToString();
            objde.ScheduledAddedByStaffCode = Session["StaffCode"].ToString();
            DataSet ds = await objbal.ViewDemoListKKAsync(objde);
            List<Coordinator> lstd = new List<Coordinator>();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Coordinator objc = new Coordinator();
                    objc.BatchCode = ds.Tables[0].Rows[i]["BatchCode"].ToString();
                    objc.ScheduleId = Convert.ToInt32(ds.Tables[0].Rows[i]["ScheduleId"].ToString());
                    objc.LabScheduleId = Convert.ToInt32(ds.Tables[0].Rows[i]["LabScheduleId"].ToString());
                    objc.DemoName = ds.Tables[0].Rows[i]["DemoName"].ToString();
                    objc.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objc.TrainerName = ds.Tables[0].Rows[i]["TrainerName"].ToString();
                    objc.DemoArrangedBy = ds.Tables[0].Rows[i]["DemoArrangedBy"].ToString();
                    objc.LabName = ds.Tables[0].Rows[i]["LabName"].ToString();
                    objc.NoOfStudent = Convert.ToInt32(ds.Tables[0].Rows[i]["NoOfStudent"].ToString());
                    DateTime StartDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString());
                    objc.strtDate = StartDate.ToString("dd-MM-yyyy");
                    objc.StatusName = ds.Tables[0].Rows[i]["Status"].ToString();
                    objc.StatusId = Convert.ToInt32(ds.Tables[0].Rows[i]["Statusid"].ToString());
                    objc.StartTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartTime"].ToString());
                    objc.EndTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["EndTime"].ToString());
                    lstd.Add(objc);
                }
            }
            return lstd;
        }
        [HttpGet]
        public async Task<ActionResult> AddDemoKKAsync()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await CourseBindKKAsync();
                //await TrainerBindKKAsync();
                await StaffBindKKAsync();
                await DemoStatusBindKKAsync();
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                   new BreadcrumbItem { Name = "CoordinatorDashboard", Url =Url.Action("CoordinatorDashboardPRAsync","Coordinator")},
                   new BreadcrumbItem { Name = "All Demo List", Url = Url.Action("ListDemoKKAsync")},
                   new BreadcrumbItem { Name = "New Demo Form", Url = Url.Action("AddDemoKKAsync")}
                };

                ViewBag.Breadcrumbs = breadcrumbs;
                return View();
            }
        }
        /// <summary>
        /// In this method bind all the trainer list.
        /// </summary>
        /// <returns>Returns the trainer list.</returns>
        public async Task TrainerBindKKAsync()
        {
            Coordinator objs = new Coordinator();
            objs.BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await objbal.ShowTrainerStaffKKAsync(objs);
            List<SelectListItem> trainerlist = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                trainerlist.Add(new SelectListItem { Text = dr["StaffName"].ToString(), Value = dr["StaffCode"].ToString() });
            }
            ViewBag.Trainer = trainerlist;
        }
        /// <summary>
        /// In this method bind all the demo status which is required for demo creation.
        /// </summary>
        /// <returns>Returns the status list.</returns>
        public async Task DemoStatusBindKKAsync()
        {
            DataSet ds = await objbal.ShowDemoStatusKKAsync();
            List<SelectListItem> demostatuslist = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                demostatuslist.Add(new SelectListItem { Text = dr["Status"].ToString(), Value = dr["StatusId"].ToString() });
            }
            ViewBag.DemoStatus = demostatuslist;
        }
        /// <summary>
        /// This method is for the lab capacity.
        /// </summary>
        /// <param name="Labcode">The labcode use for the showing capacity of particular lab.</param>
        /// <returns>It returns the lab capacity.</returns>
        public async Task<JsonResult> LabCapacityBindAsync(string Labcode)
        {
            Coordinator objc = new Coordinator();
            objc.LabCode = Labcode;
            objc.BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await objbal.AllLabCapacityKKAsync(objc);
            List<SelectListItem> capacity = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                capacity.Add(new SelectListItem { Text = dr["LabCapacity"].ToString(), Value = dr["LabName"].ToString() });
            }
            return Json(capacity, JsonRequestBehavior.AllowGet);
        } /// <summary>
          /// This methos is for showing the student list who wants demo related to their interested course.
          /// </summary>
          /// <param name="Courseid">Using courseid showing the student list.</param>
          /// <returns>Return the list.</returns>
        public async Task<JsonResult> DemoWantStudentsAsync(string CourseCode)
        {
            Coordinator objdw = new Coordinator();
            objdw.CourseCode = CourseCode;
            objdw.BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await objbal.DemoStudentsListKKAsync(objdw);
            List<SelectListItem> demolist = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                demolist.Add(new SelectListItem { Text = dr["FullName"].ToString(), Value = dr["CandidateCode"].ToString() });
            }
            return Json(demolist, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// This method is showing the lab lists on start time and end time.
        /// </summary>
        /// <param name="NoOfStudent">This parameter is access for the labname.</param>
        /// <param name="ExpectedDate">This parameter is access for the labname.</param>
        /// <param name="StartTime">This parameter is access for the labname.</param>
        /// <param name="EndTime">This parameter is access for the labname.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> LabNameListAsync(string NoOfStudent, DateTime ExpectedDate, string StartTime, string EndTime)
        {
            Coordinator OBJ = new Coordinator();
            OBJ.NoOfStudent = Convert.ToInt32(NoOfStudent);
            OBJ.BranchCode = Session["BranchCode"].ToString();
            OBJ.StartDate = ExpectedDate;
            OBJ.EndDate = ExpectedDate;
            OBJ.StartTime = Convert.ToDateTime(StartTime);
            OBJ.EndTime = Convert.ToDateTime(EndTime).AddMinutes(-15);
            DataTable dt = new DataTable();
            DataSet ds1 = new DataSet();
            BALCoordinator objbal = new BALCoordinator();
            ds1 = await objbal.AllLabListKKAsync(OBJ);
            dt = ds1.Tables[0];
            var Jsondata = JsonConvert.SerializeObject(dt);
            return Json(Jsondata);

        }
        /// <summary>
        /// This method is for saving the new demo.
        /// </summary>
        /// <param name="objDemo">This parameter use for the access properties.</param>
        /// <returns>It returns the data od demo.</returns>
        [HttpPost]
        public async Task<ActionResult> AddDemoKKAsync(string CourseName, string BatchName, string TrainerName, string SelectedStudentCodes, string NoOfStudent, string startTime, string endTime, string ExpectedDate, string LabCode, string StaffCode)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Coordinator objDemo = new Coordinator();
                objDemo.CourseName = CourseName;
                objDemo.BatchName = BatchName;
                objDemo.TrainerName = TrainerName;
                objDemo.SelectedStudentCodes = SelectedStudentCodes;
                objDemo.NoOfStudent = Convert.ToInt32(NoOfStudent.ToString());
                objDemo.StartTime = Convert.ToDateTime(startTime.ToString());
                objDemo.EndTime = Convert.ToDateTime(endTime.ToString());
                objDemo.ExpectedDate = Convert.ToDateTime(ExpectedDate.ToString());
                objDemo.EndDate = objDemo.ExpectedDate;
                objDemo.LabCode = LabCode;
                objDemo.StaffCode = StaffCode;
                await objbal.SaveDemoDataKKAsync(objDemo);
                await objbal.SaveDemoScheduleKKAsync(objDemo);
                return RedirectToAction("ListDemoKKAsync");
            }

        }
        /// <summary>
        /// This method is for validation of demoname.
        /// </summary>
        /// <param name="BatchName">Access the properties from property class.</param>
        /// <returns>It returns the validation msg for demo name.</returns>
        [HttpPost]
        public async Task<JsonResult> IsBatchAvilableAsyncST(string BatchName)
        {
            bool isAvailable = await objbal.IsBatchAvilableAsyncST(BatchName);
            return Json(new { isAvailable });
        }
        /// <summary>
        /// This method is for the the showing the details of demo.
        /// </summary>
        /// <param name="ScheduleId">This parameter is showing the detials of demo which is conducted.</param>
        /// <returns>It returns the demo details data.</returns>
        public async Task<ActionResult> DetailsDemoKKAsync(int ScheduleId)
        {
            Coordinator detail = new Coordinator();
            detail.ScheduleId = ScheduleId;
            detail.BranchCode = Session["BranchCode"].ToString();
            DataSet ds;
            ds = await objbal.DemoDetailsKKAsync(detail);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                detail.ScheduleId = Convert.ToInt32(ds.Tables[0].Rows[i]["ScheduleId"].ToString());
                detail.BatchCode = ds.Tables[0].Rows[i]["BatchCode"].ToString();
                detail.DemoName = ds.Tables[0].Rows[i]["DemoName"].ToString();
                detail.TrainerName = ds.Tables[0].Rows[i]["TrainerName"].ToString();
                detail.DemoArrangedBy = ds.Tables[0].Rows[i]["DemoArrangedBy"].ToString();
                detail.LabName = ds.Tables[0].Rows[i]["LabName"].ToString();
                detail.NoOfStudent = Convert.ToInt32(ds.Tables[0].Rows[i]["NoOfStudent"].ToString());
                DateTime ExpectedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString());
                detail.ExpecDate = ExpectedDate.ToString("dd-MM-yyyy");
                detail.StatusName = ds.Tables[0].Rows[i]["Status"].ToString();
            }
            return PartialView("DetailsDemoKKAsync", detail);
        } /// <summary>
          /// This method is shows the noofstudent list.
          /// </summary>
          /// <param name="batchcode">Using this parameter shows the details of single student.</param>
          /// <returns>It returns the list of the student.</returns>
        [HttpGet]
        public async Task<ActionResult> ListNoOfStudentKKAsync(string batchcode)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Coordinator studs = new Coordinator();
                studs.BatchCode = batchcode;
                studs.BranchCode = Session["BranchCode"].ToString();
                DataSet ds;
                ds = await objbal.NoOfStudentListKKAsync(studs);
                Coordinator nooflist = new Coordinator();
                nooflist.BatchCode = batchcode;
                List<Coordinator> lstnoofstud = new List<Coordinator>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Coordinator objnoof = new Coordinator();
                    objnoof.StudentCode = ds.Tables[0].Rows[i]["value"].ToString();
                    objnoof.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                    objnoof.ContactNumber = ds.Tables[0].Rows[i]["ContactNumber"].ToString();
                    objnoof.Emailid = ds.Tables[0].Rows[i]["EmailId"].ToString();
                    lstnoofstud.Add(objnoof);
                }
                nooflist.lstNoofstudent = lstnoofstud;
                return PartialView("ListNoOfStudentKKAsync", nooflist);
            }
        }   /// <summary>
            /// This method for edit the reschedule data.
            /// </summary>
            /// <param name="scheduleid">schedule id use for the showing selected data.</param>
            /// <returns>It returns the list.</returns>
        [HttpGet]
        public async Task<ActionResult> UpdateResheduleDemoKKAsync(int scheduleid)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await StaffBindKKAsync();
                Coordinator objde = new Coordinator();
                objde.BranchCode = Session["BranchCode"].ToString();
                objde.ScheduleId = scheduleid;
                DataSet ds;
                ds = await objbal.RescheduleDemoKKAsync(objde);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    objde.LabScheduleId = Convert.ToInt32(ds.Tables[0].Rows[i]["LabScheduleId"].ToString());
                    objde.BatchCode = ds.Tables[0].Rows[i]["BatchCode"].ToString();
                    objde.BatchName = ds.Tables[0].Rows[i]["DemoName"].ToString();
                    objde.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objde.CourseCode = ds.Tables[0].Rows[i]["Coursecode"].ToString();
                    objde.StartDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"].ToString());
                    objde.TrainerName = ds.Tables[0].Rows[i]["TrainerCodeStaffCode"].ToString();
                    DateTime originalStartTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartTime"].ToString());
                    objde.StartTime1 = originalStartTime.ToString("h:mm tt");
                    DateTime originalEndTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["EndTime"].ToString());
                    DateTime updatedEndTime = originalEndTime.AddMinutes(15);
                    objde.EndTime1 = updatedEndTime.ToString("h:mm tt");
                    objde.LabName = ds.Tables[0].Rows[i]["LabName"].ToString();
                    objde.LabCode = ds.Tables[0].Rows[i]["LabCode"].ToString();
                    objde.ScheduledAddedByStaffCode = ds.Tables[0].Rows[i]["ScheduledAddedByStaffCode"].ToString();
                    objde.StatusName = ds.Tables[0].Rows[i]["Status"].ToString();
                    objde.RescheduleDate = DateTime.Now;
                }
                ViewBag.StartT = objde.StartDate;
                ViewBag.StartTime = objde.StartTime1;
                ViewBag.EndTime = objde.EndTime1;
                ViewBag.LabName = objde.LabName;
                ViewBag.LabCode = objde.LabCode;
                return PartialView("UpdateResheduleDemoKKAsync", objde);
            }
        }
        /// <summary>
        /// This is post method for update the reschedule demo data.
        /// </summary>
        /// <param name="objupdate">By using this property access the properties.</param>
        /// <returns>It returns the updated reschedule demo data.</returns>
        [HttpPost]
        public async Task<ActionResult> UpdateResheduleDemoKKAsync(string ScheduleId, string LabScheduleId, string TrainerName, string LabName, string StartDate, string StartTime, string EndTime, string ScheduledAddedByStaffCode, string RescheduleDate)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Coordinator objupdate = new Coordinator();
                objupdate.ScheduleId = Convert.ToInt32(ScheduleId.ToString());
                objupdate.LabScheduleId = Convert.ToInt32(LabScheduleId.ToString());
                objupdate.TrainerName = TrainerName;
                objupdate.LabCode = LabName;
                objupdate.StartDate = Convert.ToDateTime(StartDate.ToString());
                objupdate.EndDate = Convert.ToDateTime(StartDate.ToString());
                objupdate.StartTime = Convert.ToDateTime(StartTime.ToString());
                objupdate.EndTime = Convert.ToDateTime(EndTime.ToString());
                objupdate.ScheduledAddedByStaffCode = ScheduledAddedByStaffCode;
                objupdate.RescheduleDate = Convert.ToDateTime(RescheduleDate.ToString());
                objupdate.StaffCode = Session["StaffCode"].ToString();
                await objbal.UpdateDemoScheduleKKAsync(objupdate);
                return RedirectToAction("ListDemoKKAsync");
            }
        }
        /// <summary>
        /// This method is for the edit the demo which we arrange.
        /// </summary>
        /// <param name="scheduleid">Using this parameter showing the data.</param>
        /// <returns>It returns the data of whatever scheduleid is selected.</returns>
        [HttpGet]
        public async Task<ActionResult> UpdateDemoKKAsync(int scheduleid)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await StaffBindKKAsync();
                await DemoUpdateStatusBindKKAsync();
                Coordinator objedit = new Coordinator();
                objedit.BranchCode = Session["BranchCode"].ToString();
                objedit.ScheduleId = scheduleid;
                DataSet ds;
                ds = await objbal.DemoEditKKAsync(objedit);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    objedit.BatchId = Convert.ToInt32(ds.Tables[0].Rows[i]["BatchId"].ToString());
                    objedit.LabScheduleId = Convert.ToInt32(ds.Tables[0].Rows[i]["LabScheduleId"].ToString());
                    objedit.BatchCode = ds.Tables[0].Rows[i]["BatchCode"].ToString();
                    objedit.BatchName = ds.Tables[0].Rows[i]["DemoName"].ToString();
                    objedit.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objedit.CourseCode = ds.Tables[0].Rows[i]["Coursecode"].ToString();
                    objedit.NoOfStudent = Convert.ToInt32(ds.Tables[0].Rows[i]["NoOfStudent"].ToString());
                    objedit.StartDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"].ToString());
                    objedit.TrainerName = ds.Tables[0].Rows[i]["TrainerCodeStaffCode"].ToString();
                    DateTime originalStartTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartTime"].ToString());
                    objedit.StartTime1 = originalStartTime.ToString("h:mm tt");
                    DateTime originalEndTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["EndTime"].ToString());
                    DateTime updatedEndTime = originalEndTime.AddMinutes(15);
                    objedit.EndTime1 = updatedEndTime.ToString("h:mm tt");
                    objedit.LabName = ds.Tables[0].Rows[i]["LabName"].ToString();
                    objedit.LabCode = ds.Tables[0].Rows[i]["LabCode"].ToString();
                    objedit.ScheduledAddedByStaffCode = ds.Tables[0].Rows[i]["ScheduledAddedByStaffCode"].ToString();
                    objedit.StatusId = Convert.ToInt32(ds.Tables[0].Rows[i]["Statusid"].ToString());

                }
                ViewBag.StartT = objedit.StartDate;
                ViewBag.StartTime = objedit.StartTime1;
                ViewBag.EndTime = objedit.EndTime1;
                ViewBag.LabName = objedit.LabName;
                ViewBag.LabCode = objedit.LabCode;
                return PartialView("UpdateDemoKKAsync", objedit);
            }
        }
        /// <summary>
        /// This method for the selected status showing with the status list.
        /// </summary>
        /// <returns>It returns the status lists.</returns>
        public async Task DemoUpdateStatusBindKKAsync()
        {
            DataSet ds = await objbal.GetDemoStatusKKAsync();
            List<SelectListItem> demostatuslist1 = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                demostatuslist1.Add(new SelectListItem { Text = dr["Status"].ToString(), Value = dr["StatusId"].ToString() });
            }
            ViewBag.DemoStatus1 = demostatuslist1;
        }
        /// <summary>
        /// This is post method of edit the demo.
        /// </summary>
        /// <param name="demoupdate">Used this parameter for pass the staff session.</param>
        /// <returns>It retuns the update data.</returns>
        [HttpPost]
        public async Task<ActionResult> UpdateDemoKKAsync(string BatchId, string ScheduleId, string LabScheduleId, string StudentCode, string NoOfStudent, string BatchCode, string TrainerName, string LabName, string StartDate, string StartTime, string EndTime, string StatusId, string ScheduledAddedByStaffCode)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Coordinator demoupdate = new Coordinator();
                demoupdate.BatchId = Convert.ToInt32(BatchId.ToString());
                demoupdate.ScheduleId = Convert.ToInt32(ScheduleId.ToString());
                demoupdate.LabScheduleId = Convert.ToInt32(LabScheduleId.ToString());
                demoupdate.StudentCode = StudentCode.TrimEnd(',');
                demoupdate.NoOfStudent = Convert.ToInt32(NoOfStudent.ToString());
                demoupdate.BatchCode = BatchCode;
                demoupdate.TrainerName = TrainerName;
                demoupdate.LabCode = LabName;
                demoupdate.StartDate = Convert.ToDateTime(StartDate.ToString());
                demoupdate.EndDate = Convert.ToDateTime(StartDate.ToString());
                demoupdate.StartTime = Convert.ToDateTime(StartTime.ToString());
                demoupdate.EndTime = Convert.ToDateTime(EndTime.ToString());
                demoupdate.StatusId = Convert.ToInt32(StatusId.ToString());
                demoupdate.ScheduledAddedByStaffCode = ScheduledAddedByStaffCode;
                demoupdate.StaffCode = Session["StaffCode"].ToString();
                await objbal.UpdateDemoKKAsync(demoupdate);
                return RedirectToAction("ListDemoKKAsync");
            }
        }


        //-----------------Kirti Demo  End -------------------------------------------------------------------------------//
        //-----------------Priyanka Event Start---------------------------------------------------------------------------//
        /// <summary>
        /// Method to Show Total EventList.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> ListEventPCAsync()    //Default event list
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                try
                {
                    Coordinator eve = new Coordinator();
                    eve.BranchCode = Session["BranchCode"].ToString();
                    DataSet ds = await objbal.EventListPCAsync(eve);
                    List<Coordinator> EventList = new List<Coordinator>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Coordinator d1 = new Coordinator();
                        d1.EventId = Convert.ToInt32(ds.Tables[0].Rows[i]["EventId"].ToString());
                        d1.EventCode = ds.Tables[0].Rows[i]["EventCode"].ToString();
                        d1.EventName = ds.Tables[0].Rows[i]["EventName"].ToString();
                        d1.StaffName = ds.Tables[0].Rows[i]["EventOrganizerName"].ToString();
                        DateTime Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"].ToString());
                        d1.SDate = Date.ToString("dd-MM-yyyy");
                        DateTime Date1 = Convert.ToDateTime(ds.Tables[0].Rows[i]["EndDate"].ToString());
                        d1.EDate = Date1.ToString("dd-MM-yyyy");
                        d1.StartTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartTime"].ToString());
                        d1.EndTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["EndTime"].ToString());
                        d1.NoOfParticipants = Convert.ToInt32(ds.Tables[0].Rows[i]["NoOfParticipants"].ToString());
                        d1.Status = ds.Tables[0].Rows[i]["Status"].ToString();
                        TempData["d1"] = "All data fetched";
                        EventList.Add(d1);
                    }
                    eve.eventlist = EventList;
                    List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                    new BreadcrumbItem { Name = "CoordinatorDashboard", Url =Url.Action("CoordinatorDashboardPRAsync","Coordinator") },
                    new BreadcrumbItem { Name = "AllEvent", Url =  Url.Action("ViewEventListPCAsync", "Coordinator") }
                };
                    ViewBag.Breadcrumbs = breadcrumbs;
                    return PartialView("ListEventPCAsync", eve);
                }
                catch (Exception ex)
                {
                    TempData["d1"] = "An error occurred:Data not Fetch Properly " + ex.Message;
                    return PartialView();
                }
            }
        }
        /// <summary>
        /// Method to Show Coducted Events.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> ListConductedListPCAsync()
        {
            Coordinator eve = new Coordinator();
            eve.BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await objbal.ConductedEventListPCAsync(eve);
            List<Coordinator> ConductedEventlist = new List<Coordinator>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Coordinator d = new Coordinator();
                d.EventId = Convert.ToInt32(ds.Tables[0].Rows[i]["EventId"].ToString());
                d.EventName = ds.Tables[0].Rows[i]["EventName"].ToString();
                d.StaffName = ds.Tables[0].Rows[i]["EventOrganizerName"].ToString();
                DateTime Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"].ToString());
                d.SDate = Date.ToString("dd-MM-yyyy");
                DateTime Date1 = Convert.ToDateTime(ds.Tables[0].Rows[i]["EndDate"].ToString());
                d.EDate = Date1.ToString("dd-MM-yyyy");
                d.StartTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartTime"].ToString());
                d.EndTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["EndTime"].ToString());
                d.NoOfParticipants = Convert.ToInt32(ds.Tables[0].Rows[i]["NoOfParticipants"].ToString());
                d.Status = ds.Tables[0].Rows[i]["Status"].ToString();
                TempData["d"] = "All data fetched";
                ConductedEventlist.Add(d);
            }
            eve.Conductedeventlist = ConductedEventlist;
            List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                    new BreadcrumbItem { Name = "CoordinatorDashboard", Url =Url.Action("CoordinatorDashboardPRAsync","Coordinator") },
                    new BreadcrumbItem { Name = "ConductedEvent", Url =  Url.Action("ListConductedListPCAsync", "Coordinator") }
                };

            ViewBag.Breadcrumbs = breadcrumbs;
            return PartialView("ListConductedListPCAsync", eve);
        }
        /// <summary>
        /// Method to Show Coducted Events.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> ListArrangedEventPCAsync()
        {
            Coordinator eve = new Coordinator();
            eve.BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await objbal.ArrangedEventListPCAsync(eve);
            List<Coordinator> ArrangedEventlist = new List<Coordinator>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Coordinator d2 = new Coordinator();
                d2.EventId = Convert.ToInt32(ds.Tables[0].Rows[i]["EventId"].ToString());
                d2.EventName = ds.Tables[0].Rows[i]["EventName"].ToString();
                d2.StaffName = ds.Tables[0].Rows[i]["EventOrganizerName"].ToString();
                DateTime Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"].ToString());
                d2.SDate = Date.ToString("dd-MM-yyyy");
                DateTime Date1 = Convert.ToDateTime(ds.Tables[0].Rows[i]["EndDate"].ToString());
                d2.EDate = Date1.ToString("dd-MM-yyyy");
                d2.StartTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartTime"].ToString());
                d2.EndTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["EndTime"].ToString());
                d2.NoOfParticipants = Convert.ToInt32(ds.Tables[0].Rows[i]["NoOfParticipants"].ToString());
                d2.Status = ds.Tables[0].Rows[i]["Status"].ToString();
                TempData["d2"] = "All data fetched";
                ArrangedEventlist.Add(d2);
            }
            eve.Arrangedeventlist = ArrangedEventlist;
            List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                    new BreadcrumbItem { Name = "CoordinatorDashboard", Url =Url.Action("CoordinatorDashboardPRAsync","Coordinator") },
                    new BreadcrumbItem { Name = "ArrangedEvent", Url =  Url.Action("ListArrangedEventPCAsync", "Coordinator") }
                };

            ViewBag.Breadcrumbs = breadcrumbs;
            return PartialView("ListArrangedEventPCAsync", eve);
        }
        /// <summary>
        /// Method to Show Pending Events.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> ListPendingEventPCAsync()
        {
            Coordinator eve = new Coordinator();
            eve.BranchCode = Session["BranchCode"].ToString();
            DataSet ds3 = await objbal.PendingEventListPCAsync(eve);
            List<Coordinator> PendingEventlist = new List<Coordinator>();
            for (int i = 0; i < ds3.Tables[0].Rows.Count; i++)
            {
                Coordinator d3 = new Coordinator();
                d3.EventId = Convert.ToInt32(ds3.Tables[0].Rows[i]["EventId"].ToString());
                d3.EventName = ds3.Tables[0].Rows[i]["EventName"].ToString();
                d3.StaffName = ds3.Tables[0].Rows[i]["EventOrganizerName"].ToString();
                DateTime Date = Convert.ToDateTime(ds3.Tables[0].Rows[i]["StartDate"].ToString());
                d3.SDate = Date.ToString("dd-MM-yyyy");
                DateTime Date1 = Convert.ToDateTime(ds3.Tables[0].Rows[i]["EndDate"].ToString());
                d3.EDate = Date1.ToString("dd-MM-yyyy");
                d3.StartTime = Convert.ToDateTime(ds3.Tables[0].Rows[i]["StartTime"].ToString());
                d3.EndTime = Convert.ToDateTime(ds3.Tables[0].Rows[i]["EndTime"].ToString());
                d3.NoOfParticipants = Convert.ToInt32(ds3.Tables[0].Rows[i]["NoOfParticipants"].ToString());
                d3.Status = ds3.Tables[0].Rows[i]["Status"].ToString();
                TempData["d3"] = "All data fetched";
                PendingEventlist.Add(d3);
            }
            eve.Pendingeventlist = PendingEventlist;
            List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                    new BreadcrumbItem { Name = "CoordinatorDashboard", Url =Url.Action("CoordinatorDashboardPRAsync","Coordinator") },
                    new BreadcrumbItem { Name = "PendingEvent", Url =  Url.Action("ListPendingEventPCAsync", "Coordinator") }
                };

            ViewBag.Breadcrumbs = breadcrumbs;
            return PartialView("ListPendingEventPCAsync", eve);
        }
        /// <summary>
        /// Method to bind Event-Organizer List to Dropdown.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task EventOrganizerListBindPCAsync()
        {
            Coordinator eve = new Coordinator();
            eve.BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await objbal.EventOrganizerBindPCAsync(eve);
            List<SelectListItem> eventOrgBindlist = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                eventOrgBindlist.Add(new SelectListItem { Text = dr["StaffName"].ToString(), Value = dr["StaffCode"].ToString() });
            }
            ViewBag.EventOrgnaizerCode = eventOrgBindlist;
        }
        /// <summary>
        /// Method to Display Statuswise Event list.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> ListAllEventPCAsync()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
                try
                {
                    List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                    new BreadcrumbItem { Name = "CoordinatorDashboard", Url =Url.Action("CoordinatorDashboardPRAsync","Coordinator") },
                    new BreadcrumbItem { Name = "Event", Url =  Url.Action("ListAllEventPCAsync", "Coordinator") }
                };

                    ViewBag.Breadcrumbs = breadcrumbs;
                    return await Task.Run(() => View());
                }
                catch (Exception ex)
                {
                    // Handle exceptions appropriately
                    TempData["d1"] = "An error occurred:Data not Fetch Properly " + ex.Message;
                    return View();
                }
        }
        /// <summary>
        /// Method to Bind Status for Event.
        /// </summary>
        /// <returns></returns>
        public async Task EventStatusBindPCAsync()
        {
            DataSet ds5 = await objbal.GetStatusPCAsync();
            List<SelectListItem> EventStatusName = new List<SelectListItem>();
            foreach (DataRow dr3 in ds5.Tables[0].Rows)
            {
                EventStatusName.Add(new SelectListItem
                {
                    Text = dr3["Status"].ToString(),
                    Value = dr3["StatusId"].ToString()
                });
            }
            ViewBag.eventStatusName = EventStatusName;
        }
        /// <summary>
        /// Method to Save Event in EventList.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> AddEventPCAsync()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
                try
                {
                    await EventOrganizerListBindPCAsync();
                    await EventStatusBindPCAsync();
                    await BatchBindPCAsync();
                    await StaffBindPCAsync();
                    List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                    new BreadcrumbItem { Name = "CoordinatorDashboard", Url =Url.Action("CoordinatorDashboardPRAsync","Coordinator") },
                    new BreadcrumbItem { Name = "AddEvent", Url =  Url.Action("AddEventPCAsync", "Coordinator") }
                };

                    ViewBag.Breadcrumbs = breadcrumbs;
                    return View();
                }
                catch (Exception ex)
                {
                    // Handle exceptions appropriately
                    TempData["d1"] = "An error occurred:Data not Fetch Properly " + ex.Message;
                    return View();
                }
        }
        [HttpPost]
        public async Task<JsonResult> AddEventPCAsync(Coordinator obj)
        {
            BALCoordinator objbal = new BALCoordinator();

            try
            {
                obj.StatusId = 25;
                obj.StaffCode = Session["StaffCode"].ToString();
                await objbal.RegisterEventPCAsync(obj);

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return await Task.Run(() => Json(new { success = true, message = "Event has been Added successfully!" }));
        }
        public async Task<ActionResult> BatchBindPCAsync()
        {
            Coordinator eve = new Coordinator();
            eve.BranchCode = Session["BranchCode"].ToString();
            ViewBag.Batch = await objbal.batchbindPCAsync(eve);
            return await Task.Run(() => View());
        }

        /// <summary>
        /// Method to Get StaffName for send invitation after selection.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> StaffBindPCAsync()
        {
            Coordinator eve = new Coordinator();
            eve.BranchCode = Session["BranchCode"].ToString();
            ViewBag.Staff = await objbal.GetEventOrganizerBindPCAsync(eve);
            return await Task.Run(() => View());
        }
        /// <summary>
        /// Method for Calender to show Event.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ViewCalenderPCAsync()
        {
            string branchCode = Session["BranchCode"].ToString(); // Assuming BranchCode is stored in Session
            Coordinator coordinator = new Coordinator { BranchCode = branchCode };

            DataSet ds = await objbal.CalenderListPCAsync(coordinator);
            Coordinator e = new Coordinator();
            List<Coordinator> EventList = new List<Coordinator>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                Coordinator d1 = new Coordinator();
                //d1.EventId = Convert.ToInt32(row["EventId"].ToString());
                //d1.EventCode = row["EventCode"].ToString();
                d1.EventName = row["EventName"].ToString();

                d1.SDate = Convert.ToDateTime(row["StartDate"]).ToString("dd-MM-yyyy");
                d1.EDate = Convert.ToDateTime(row["EndDate"]).ToString("dd-MM-yyyy");

                TempData["d1"] = "All data fetched";
                EventList.Add(d1);
            }
            e.events = EventList;
            ViewBag.EventsJson = JsonConvert.SerializeObject(EventList);
            // Ensure events are assigned to the Model
            return View(e);
        }
        /// <summary>
        /// Method to show Event-Details.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> DetailsEventListPCAsync(string id)
        {
            Coordinator objshow = new Coordinator();
            objshow.BranchCode = Session["BranchCode"].ToString();
            int ids = Convert.ToInt32(id);
            objshow.EventId = ids;
            SqlDataReader dr;
            dr = await objbal.EventDetailPCAsync(objshow);

            if (dr.Read())
            {
                objshow.EventName = dr["EventName"].ToString();
                objshow.EventCode = dr["EventCode"].ToString();
                objshow.NoOfParticipants = Convert.ToInt32(dr["NoOfParticipants"]);
                objshow.StartDate = Convert.ToDateTime(dr["StartDate"]);
                objshow.EndDate = Convert.ToDateTime(dr["EndDate"]);
                objshow.StartTime = Convert.ToDateTime(dr["StartTime"]);
                objshow.EndTime = Convert.ToDateTime(dr["EndTime"]);
                objshow.Location = dr["Location"].ToString();
                objshow.EventArrangedBy = dr["EventArrangedBy"].ToString();
                objshow.Description = dr["Description"].ToString();
                objshow.Status = dr["Status"].ToString();
                objshow.EventType = Convert.ToInt32(dr["EventType"].ToString());
                //objshow.InvitationToSend = dr["InvitationToSend"].ToString();
            }
            return PartialView("DetailsEventListPCAsync", objshow); // Assuming "DetailsEventListPC" is the name of your partial view
        }
        /// <summary>
        /// Method to Delete Event from EventList.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> RemoveEventListPCAsync(int id)
        {
            try
            {
                Coordinator objedit = new Coordinator();
                objedit.EventId = id;
                //objbal.id = 1;
                await objbal.EventDeletePCAsync(objedit);
                return await Task.Run(() => Json(new { success = true, message = "Event Delete Successfully" },
                JsonRequestBehavior.AllowGet));
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = "An error occurred while deleting the event." });
            }
        }

        //-----------------Priyanka Event End--------------------------------------------------------------------------//

        //-----------------Priyanka Event End--------------------------------------------------------------------------//
        //-----------------Rohit Fees Follow Up Start --------------------------------------------------------------------//
        /// <summary>
        /// Action method to list fee follow-up asynchronously.
        /// </summary>
        /// <returns>An asynchronous action result representing the view.</returns>
        [HttpGet]
        public async Task<ActionResult> ListFeeFollowupAsyncRs()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Coordinator objC = new Coordinator();
                string BranchCode = Session["BranchCode"].ToString();
                DataSet ds = await objbal.ListFeeFollowupAsyncRS(BranchCode);
                List<Coordinator> ListFee = new List<Coordinator>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Coordinator objC1 = new Coordinator();
                    objC1.StudentId = Convert.ToInt32(row["StudentId"].ToString());
                    objC1.StudentCode = row["CandidateCode"].ToString();
                    objC1.FullName = row["FullName"].ToString();
                    //objC1.StudentCode = row["CandidateCode"].ToString();
                    objC1.ContactNumber = row["ContactNumber"].ToString();
                    objC1.Emailid = row["EmailId"].ToString();
                    objC1.CourseName = row["CourseName"].ToString();
                    objC1.CourseFee = Convert.ToDecimal(row["CourseFee"]).ToString("N0") + " " + "Rs.";
                    objC1.RemainingFee = Convert.ToDecimal(row["RemainingAmount"]).ToString("#,0") + " " + "Rs.";
                    DateTime Date = Convert.ToDateTime(row["NextInstallmentDate"].ToString());
                    objC1.Duration = Date.ToString("dd-MM-yyyy");
                    objC1.InstallmentAmount = Math.Round(Convert.ToDecimal(row["NextInstallmentAmount"])).ToString("#,0") + " " + "Rs.";


                    ListFee.Add(objC1);
                }
                objC.lstfeefollowup = ListFee;
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                   new BreadcrumbItem { Name = "CoordinatorDashboard", Url =Url.Action("CoordinatorDashboardPRAsync","Coordinator")},
                   new BreadcrumbItem { Name = "AllFeeFollowUpTaken", Url =  Url.Action("ListFeeFollowupAsyncRs")}

                };

                ViewBag.Breadcrumbs = breadcrumbs;
                return await Task.Run(() => View("ListFeeFollowupAsyncRs", objC));
            }
        }
        /// <summary>
        /// Action method to view fee follow-up details asynchronously for a specific student.
        /// </summary>
        /// <param name="StudentId">The ID of the student for whom fee follow-up details are to be viewed.</param>
        /// <returns>An asynchronous action result representing the partial view containing fee follow-up details.</returns>
        [HttpGet]
        public async Task<ActionResult> ViewFeeFollowupDetailsAsyncRs(int StudentId)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Coordinator objC = new Coordinator();
                objC.StudentId = StudentId;
                objC.BranchCode = Session["BranchCode"].ToString();
                DataSet ds = await objbal.GetFeeFollowupDetailsAsyncRS(objC);

                Coordinator objC1 = new Coordinator();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    objC1.StudentId = Convert.ToInt32(ds.Tables[0].Rows[i]["StudentId"].ToString());
                    objC1.StudentCode = ds.Tables[0].Rows[i]["CandidateCode"].ToString();
                    objC1.FullName = ds.Tables[0].Rows[i]["FullName"].ToString();
                    objC1.ContactNumber = ds.Tables[0].Rows[i]["ContactNumber"].ToString();
                    objC1.Emailid = ds.Tables[0].Rows[i]["EmailId"].ToString();
                    objC1.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objC1.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    objC1.CourseFee = Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalFee"]).ToString("#,0") + " " + "Rs.";
                    objC1.Amount = Convert.ToDecimal(ds.Tables[0].Rows[i]["RegistrationFee"]).ToString("#,0") + " " + "Rs.";
                    //objC1.Discount = Convert.ToInt32(ds.Tables[0].Rows[i]["Discount"].ToString()) + "%";
                    //objC1.Discount = Convert.ToInt32(ds.Tables[0].Rows[i]["Discount"].ToString() + "%");

                    objC1.Discount = Convert.ToInt32(ds.Tables[0].Rows[i]["Discount"].ToString());
                    objC1.RemainingFee = Convert.ToDecimal(ds.Tables[0].Rows[i]["RegistrationFee"]).ToString("#,0") + " " + "Rs.";
                    objC1.PaidAmount = Convert.ToDecimal(ds.Tables[0].Rows[i]["PaidAmount"]).ToString("#,0") + " " + "Rs.";
                    objC1.RemainingFee = Convert.ToDecimal(ds.Tables[0].Rows[i]["RemainingAmount"]).ToString("#,0") + " " + "Rs.";
                    objC1.DiscountedCourseFee = Convert.ToDecimal(ds.Tables[0].Rows[i]["CourseFee"]).ToString("#,0") + " " + "Rs.";
                    objC1.InstallmentAmount = Math.Round(Convert.ToDecimal(ds.Tables[0].Rows[i]["NextInstallmentAmount"])).ToString("#,0") + " " + "Rs.";

                    DateTime Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["NextInstallmentDate"].ToString());
                    objC1.nextdate = Date.ToString("dd-MM-yyyy");

                    DateTime Date1 = Convert.ToDateTime(ds.Tables[0].Rows[i]["LastInstallmentDate"].ToString());
                    objC1.LastInstallmentDate = Date1.ToString("dd-MM-yyyy");
                }

                return PartialView("ViewFeeFollowupDetailsAsyncRs", objC1);
            }
        }
        /// <summary>
        /// Action method to add fee follow-up details asynchronously for a specific student.
        /// </summary>
        /// <param name="StudentId">The ID of the student for whom fee follow-up details are to be added.</param>
        /// <returns>An asynchronous action result representing the partial view containing fee follow-up details.</returns>
        [HttpGet]
        public async Task<ActionResult> AddFeeFollowUpDetailsAsyncRS(int StudentId)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Coordinator objC = new Coordinator();
                await GetStaffAsyncRS();
                await GetStatusAsyncRS();
                objC.StudentId = StudentId;
                objC.BranchCode = Session["BranchCode"].ToString();
                DataSet ds = await objbal.GetFeeFollowupDetailsAsyncRS(objC);
                Coordinator objC1 = new Coordinator();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    objC1.StudentId = Convert.ToInt32(ds.Tables[0].Rows[i]["StudentId"].ToString());
                    objC1.FullName = ds.Tables[0].Rows[i]["FullName"].ToString();
                    objC1.StudentCode = ds.Tables[0].Rows[i]["CandidateCode"].ToString();
                    objC1.Amount = Convert.ToDecimal(ds.Tables[0].Rows[i]["NextInstallmentAmount"]).ToString("#,0") + " Rs.";
                    objC1.RemainingFee = ds.Tables[0].Rows[i]["RemainingAmount"].ToString() + " " + "Rs.";
                }
                return PartialView("AddFeeFollowUpDetailsAsyncRS", objC1);
            }
        }
        /// <summary>
        /// Action method to asynchronously add fee follow-up details for a coordinator.
        /// </summary>
        /// <param name="ObjCo">The Coordinator object containing the fee follow-up details to be added.</param>
        /// <returns>An asynchronous action result redirecting to the list of added fee follow-up details.</returns>
        [HttpPost]
        public async Task<ActionResult> AddFeeFollowUpDetailsAsyncRS(Coordinator ObjCo)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await Task.Run(() => objbal.AddFeeFollowUpAsyncRS(ObjCo));
                return RedirectToAction("ListAddedFeeFollowupAsyncRs");
            }
        }
        /// <summary>
        /// Asynchronously retrieves and populates the staff list for a specific branch.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task GetStaffAsyncRS()
        {
            Coordinator objC = new Coordinator();
            objC.BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await objbal.GetStaffAsyncRS(objC);
            List<SelectListItem> StaffList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                StaffList.Add(new SelectListItem { Text = dr["StaffName"].ToString(), Value = dr["StaffCode"].ToString() });
            }
            ViewBag.StaffList = StaffList;
        }
        /// <summary>
        /// Asynchronously retrieves and populates the status list for a specific branch.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task GetStatusAsyncRS()
        {
            Coordinator objC = new Coordinator();
            objC.BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await objbal.GetStatusAsyncRS(objC);
            List<SelectListItem> StatusList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                StatusList.Add(new SelectListItem { Text = dr["Status"].ToString(), Value = dr["StatusId"].ToString() });
            }
            ViewBag.StatusList = StatusList;
        }
        /// <summary>
        /// Action method to asynchronously list added fee follow-up details.
        /// </summary>
        /// <returns>An asynchronous action result representing the view containing the list of added fee follow-up details.</returns>
        [HttpGet]
        public async Task<ActionResult> ListAddedFeeFollowupAsyncRs()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Coordinator objC = new Coordinator();
                string BranchCode = Session["BranchCode"].ToString();
                DataSet ds = await objbal.ListAddedFeeFollowupAsyncRS(BranchCode);
                List<Coordinator> ListFee = new List<Coordinator>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Coordinator objC1 = new Coordinator();
                    objC1.StudentId = Convert.ToInt32(row["StudentId"].ToString());
                    objC1.StudentCode = row["CandidateCode"].ToString();
                    DateTime Date = Convert.ToDateTime(row["FollowUpTakenDate"].ToString());
                    objC1.followupDate = Date.ToString("dd-MM-yyyy");
                    objC1.FullName = row["FullName"].ToString();
                    objC1.BatchName = row["BatchName"].ToString();
                    objC1.ContactNumber = row["ContactNumber"].ToString();
                    objC1.Emailid = row["EmailId"].ToString();
                    DateTime Date1 = Convert.ToDateTime(row["Date_join_installment"].ToString());
                    objC1.nextdate = Date1.ToString("dd-MM-yyyy");
                    DateTime Date2 = Convert.ToDateTime(row["NextFollowUpDate"].ToString());
                    objC1.NextFollowup = Date2.ToString("dd-MM-yyyy");


                    objC1.FollowUpNote = row["FollowUpNote"].ToString();
                    objC1.Status = row["Followupstatus"].ToString();
                    ListFee.Add(objC1);
                }
                objC.lstfeefollowup = ListFee;
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                  new BreadcrumbItem { Name = "CoordinatorDashboard", Url =Url.Action("CoordinatorDashboardPRAsync","Coordinator")},
                   new BreadcrumbItem { Name = "AllFeeFollowUpTaken", Url =  Url.Action("ListFeeFollowupAsyncRs","Coordinator")},
                   new BreadcrumbItem { Name = "AllFeeFollowUpDoneList", Url =  Url.Action("ListAddedFeeFollowupAsyncRs")}

                };

                ViewBag.Breadcrumbs = breadcrumbs;
                return await Task.Run(() => View("ListAddedFeeFollowupAsyncRs", objC));
            }
        }
        /// <summary>
        /// Action method to asynchronously view the follow-up history for a specific student.
        /// </summary>
        /// <param name="StudentId">The ID of the student for whom the follow-up history is to be viewed.</param>
        /// <returns>An asynchronous action result representing the partial view containing the follow-up history.</returns>
        public async Task<ActionResult> ViewFollowUpHistoryAsyncRS(int StudentId)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                List<Coordinator> followup = new List<Coordinator>();
                DataSet ds = await objbal.ViewFollowUpHistoryRS(StudentId);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Coordinator objc = new Coordinator();
                    objc.StudentId = Convert.ToInt32(ds.Tables[0].Rows[i]["StudentId"].ToString());
                    objc.StudentCode = ds.Tables[0].Rows[i]["CandidateCode"].ToString();
                    objc.FullName = ds.Tables[0].Rows[i]["FullName"].ToString();
                    objc.ContactNumber = ds.Tables[0].Rows[i]["ContactNumber"].ToString();
                    objc.Emailid = ds.Tables[0].Rows[i]["EmailId"].ToString();
                    objc.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    DateTime Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date_join_installment"].ToString());
                    objc.nextdate = Date.ToString("dd-MM-yyyy");
                    DateTime Date1 = Convert.ToDateTime(ds.Tables[0].Rows[i]["FollowUpTakenDate"].ToString());
                    objc.followupDate = Date1.ToString("dd-MM-yyyy");
                    objc.FollowUpNote = ds.Tables[0].Rows[i]["FollowUpNote"].ToString();
                    objc.Count = Convert.ToInt32(ds.Tables[0].Rows[i]["TotalFollowupCount"].ToString());
                    objc.FollowUpTakenBy = ds.Tables[0].Rows[i]["StaffName"].ToString();
                    objc.Status = ds.Tables[0].Rows[i]["Followupstatus"].ToString();
                    followup.Add(objc);
                }
                return PartialView("ViewFollowUpHistoryAsyncRS", followup);
            }
        }

        //-----------------Rohit Fees Follow Up End ----------------------------------------------------------------------//
        //-----------------Kirti Feedback Start --------------------------------------------------------------------------//
        /// <summary>
        /// This view is for the tab lists showing.
        /// </summary>
        /// <returns>It returns the all list that i want to shown in tabs. </returns>
        public async Task<ActionResult> ListAllFeedbackForTrainerKKAsync()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                  new BreadcrumbItem { Name = "CoordinatorDashboard", Url =Url.Action("CoordinatorDashboardPRAsync","Coordinator")},
                   new BreadcrumbItem { Name = "Feedback Lists", Url = Url.Action("ListAllFeedbackForTrainerkKKAsync")}
                };

                ViewBag.Breadcrumbs = breadcrumbs;
                return await Task.Run(() => View());
            }
        }
        /// <summary>
        /// This method is for the feedback give to trainer list which came from the batch.
        /// </summary>
        /// <returns>It returns the feedback trainer list.</returns>
        [HttpGet]
        public async Task<ActionResult> ListNewFeedbackToTrainerKKAsync()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Coordinator objfed = new Coordinator();
                objfed.BranchCode = Session["BranchCode"].ToString();
                DataSet ds = await objbal.NewFeedbackListFromStudentKKAsync(objfed);
                Coordinator objfeedback = new Coordinator();
                List<Coordinator> lstfeedbck = new List<Coordinator>();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Coordinator objf = new Coordinator();
                    objf.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objf.TypeName = ds.Tables[0].Rows[i]["TypeName"].ToString();
                    objf.FeedbackFrom = ds.Tables[0].Rows[i]["Feedbackfrom"].ToString();
                    objf.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    objf.FeedbackFor = ds.Tables[0].Rows[i]["Feedbackfor"].ToString();
                    DateTime FeedbackSendDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["FeedbackSendDate"].ToString());
                    objf.feedbcksendDate = FeedbackSendDate.ToString("dd-MM-yyyy");
                    DateTime FeedbackTillDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["FeedbackTillDate"].ToString());
                    objf.feedbcktillDate = FeedbackTillDate.ToString("dd-MM-yyyy");
                    objf.FollowUpTakenBy = ds.Tables[0].Rows[i]["FeedbackSendBy"].ToString();
                    objf.Description = ds.Tables[0].Rows[i]["Descriptions"].ToString();
                    lstfeedbck.Add(objf);
                }
                objfeedback.lsttrainerfeed = lstfeedbck;
                return PartialView("ListNewFeedbackToTrainerKKAsync", objfeedback);
            }
        }
        /// <summary>
        /// In this method shows the list of feedback given student to the trainer.
        /// </summary>
        /// <returns>It returns the feedback list of particular trainer student give.</returns>
        [HttpGet]
        public async Task<ActionResult> ListFeedbackGivenToTrainerKKAsync()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Coordinator objfed = new Coordinator();
                objfed.BranchCode = Session["BranchCode"].ToString();
                DataSet ds = await objbal.FeedbackFromStudentKKAsync(objfed);
                Coordinator objfeedback = new Coordinator();
                List<Coordinator> lstfeedbckgiven = new List<Coordinator>();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Coordinator objf = new Coordinator();
                    objf.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objf.TypeName = ds.Tables[0].Rows[i]["TypeName"].ToString();
                    objf.FeedbackFrom = ds.Tables[0].Rows[i]["Feedbackfrom"].ToString();
                    objf.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    objf.FeedbackFor = ds.Tables[0].Rows[i]["Feedbackfor"].ToString();
                    DateTime FeedbackSendDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["FeedbackSendDate"].ToString());
                    objf.feedbcksendDate = FeedbackSendDate.ToString("dd-MM-yyyy");
                    DateTime FeedbackTillDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["FeedbackTillDate"].ToString());
                    objf.feedbcktillDate = FeedbackTillDate.ToString("dd-MM-yyyy");
                    objf.FollowUpTakenBy = ds.Tables[0].Rows[i]["FeedbackSendBy"].ToString();
                    objf.Rating = Convert.ToInt32(ds.Tables[0].Rows[i]["Rating"].ToString());
                    objf.Comment = ds.Tables[0].Rows[i]["Comment"].ToString();
                    objf.Description = ds.Tables[0].Rows[i]["Descriptions"].ToString();
                    lstfeedbckgiven.Add(objf);
                }
                objfeedback.lsttrainerfeedgiven = lstfeedbckgiven;
                return PartialView("ListFeedbackGivenToTrainerKKAsync", objfeedback);
            }
        }
        /// <summary>
        /// This method is for the save new student feedback form.
        /// </summary>
        /// <returns>It returns the data of saving.</returns>
        [HttpGet]
        public async Task<ActionResult> AddStudentFeedbackFormKKAsync()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await CourseBindKKAsync();
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                   new BreadcrumbItem { Name = "CoordinatorDashboard", Url =Url.Action("CoordinatorDashboardPRAsync","Coordinator")},
                   new BreadcrumbItem { Name = "Feedback Lists", Url = Url.Action("ListAllFeedbackForTrainerKKAsync")},
                   new BreadcrumbItem { Name = "Student Feedback Form", Url = Url.Action("AddStudentFeedbackFormKKAsync")}
                };

                ViewBag.Breadcrumbs = breadcrumbs;
                return PartialView("AddStudentFeedbackFormKKAsync");
            }
        }

        /// <summary>
        /// This method is for the showing batches of selected course.
        /// </summary>
        /// <param name="Coursecode">Using this parameter shows the batch list.</param>
        /// <returns>Returns the course batch list.</returns>
        public async Task<JsonResult> CourseTrainerNameKKAsync(string Coursecode)
        {
            DataSet ds = await objbal.StaffTrainerNameKKAsync(Coursecode);
            List<SelectListItem> trainerlist = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                trainerlist.Add(new SelectListItem { Text = dr["StaffName"].ToString(), Value = dr["StaffCode"].ToString() });
            }
            return Json(trainerlist, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// This is showing the course list related to trainer.
        /// </summary>
        /// <param name="StaffPositionId">This parameter is use for the to getting only trainer staff.</param>
        /// <returns>It returns the courses of selected trainer.</returns>
        public async Task<JsonResult> TrainerCourseKKAsync()
        {
            Coordinator objco = new Coordinator();
            objco.BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await objbal.StaffCourseNameKKAsync(objco);
            List<SelectListItem> clist = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                clist.Add(new SelectListItem { Text = dr["CourseName"].ToString(), Value = dr["CourseCode"].ToString() });
            }
            return Json(clist, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// This method is use for the trainer batches.
        /// </summary>
        /// <param name="TrainerCode">This parameter is for the selected particular trainer.</param>
        /// <returns>It returns the batch list.</returns>
        public async Task<JsonResult> TrainerBatchesKKAsync(string TrainerCode)
        {
            Coordinator objb = new Coordinator();
            objb.StaffCode = TrainerCode;
            objb.BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await objbal.TrainerBatchNameKKAsync(objb);
            List<SelectListItem> batchlist = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                batchlist.Add(new SelectListItem { Text = dr["BatchName"].ToString(), Value = dr["BatchCode"].ToString() });
            }
            return Json(batchlist, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// This method is for the showing the demo trainer batch names.
        /// </summary>
        /// <param name="TrainerCode">This code is for select particular trainer.</param>
        /// <returns>It returns the demo batch list of particular trainer.</returns>
        public async Task<JsonResult> DemoTrainerBatchesKKAsync(string TrainerCode)
        {
            Coordinator objdebatch = new Coordinator();
            objdebatch.StaffCode = TrainerCode;
            objdebatch.BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await objbal.DemoTrainerBatchNameKKAsync(objdebatch);
            List<SelectListItem> dbatchlist = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                dbatchlist.Add(new SelectListItem { Text = dr["BatchName"].ToString(), Value = dr["BatchCode"].ToString() });
            }
            return Json(dbatchlist, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// This method is for the demo related course list.
        /// </summary>
        /// <param name="Typeid">Typeid use for the selected type.</param>
        /// <returns>It returns the demo course list.</returns>
        public async Task<JsonResult> DemoCourseNameListKKAsync(int Typeid = 4)
        {
            Coordinator dcourse = new Coordinator();
            dcourse.TypeId = Typeid;
            dcourse.BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await objbal.DemoCourseNameKKAsync(dcourse);
            List<SelectListItem> dclist = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                dclist.Add(new SelectListItem { Text = dr["CourseName"].ToString(), Value = dr["CourseCode"].ToString() });
            }
            return Json(dclist, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// This method is for the all event list of level1.
        /// </summary>
        /// <returns>It returns the event list.</returns>
        public async Task<JsonResult> ListEventKKAsync()
        {
            DataSet ds = await objbal.EventListKKAsync();
            List<SelectListItem> Event = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Event.Add(new SelectListItem { Text = dr["EventName"].ToString(), Value = dr["EventCode"].ToString() });
            }
            return Json(Event, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// This method is for the showing all the active batches.
        /// </summary>
        /// <returns>It returns the list of all active batches.</returns>
        public async Task<JsonResult> EventBatchListKKAsync()
        {
            DataSet ds = await objbal.EventBatchKKAsync();
            List<SelectListItem> ebatches = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ebatches.Add(new SelectListItem { Text = dr["BatchName"].ToString(), Value = dr["BatchCode"].ToString() });
            }
            return Json(ebatches, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// This method shows the demo batches.
        /// </summary>
        /// <param name="selectedCourseCode">This parameter use for the showing batch of selected course.</param>
        /// <returns>It returns the demobatch list.</returns>
        public async Task<JsonResult> DemoFeedbackBatchKKAsync(string selectedCourseCode)
        {
            Coordinator objdfbth = new Coordinator();
            objdfbth.CourseCode = selectedCourseCode;
            objdfbth.BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await objbal.DemoFeedbackListKKAsync(objdfbth);
            List<SelectListItem> dbatches = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                dbatches.Add(new SelectListItem { Text = dr["BatchName"].ToString(), Value = dr["BatchCode"].ToString() });
            }
            return Json(dbatches, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// This method is showing the active batch student list.
        /// </summary>
        /// <param name="BatchCode">The parameter is use for the selected particular batch student names. </param>
        /// <returns>It returns the student list.</returns>
        public async Task<JsonResult> BatchStudentsListKKAsync(string BatchCode)
        {
            Coordinator bthstu = new Coordinator();
            bthstu.BatchCode = BatchCode;
            bthstu.BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await objbal.BatchStudentListKKAsync(bthstu);
            List<SelectListItem> stlist = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                stlist.Add(new SelectListItem { Text = dr["FullName"].ToString(), Value = dr["CandidateCode"].ToString() });
            }
            return Json(stlist, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// This method is showing the demo batch student list.
        /// </summary>
        /// <param name="BatchCode">The parameter is use for the selected particular batch student names.</param>
        /// <returns>It returns the student list.</returns>
        public async Task<JsonResult> DemoBatchStudentsListKKAsync(string BatchCode)
        {
            Coordinator objdstud = new Coordinator();
            objdstud.BatchCode = BatchCode;
            objdstud.BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await objbal.DemoBatchStudentListKKAsync(objdstud);
            List<SelectListItem> dstlist = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                dstlist.Add(new SelectListItem { Text = dr["FullName"].ToString(), Value = dr["CandidateCode"].ToString() });
            }
            return Json(dstlist, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// This method is for the save the data of student feedback.
        /// </summary>
        /// <param name="objfeed">This object is use for the access the properties.</param>
        /// <returns>It returns the view of the list.</returns>
        [HttpPost]
        public async Task<ActionResult> AddStudentFeedbackFormKKAsync(List<Coordinator> records)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                if (records != null && (records).Any())
                {
                    string staffCode = Session["StaffCode"].ToString();
                    Coordinator objC = new Coordinator();
                    try
                    {
                        // Loop through each record and save it to the database
                        foreach (var record in records)
                        {
                            objC.StudentCode = record.StudentCode;
                            objC.TypeId = record.FeedbackType;
                            objC.CourseCode = record.CourseCode;
                            objC.FeedbackFor = record.FeedbackFor;
                            objC.BatchCode = record.BatchCode;
                            objC.feedbcksendDate = record.feedbcksendDate;
                            objC.feedbcktillDate = record.Date.ToString();
                            objC.Description = record.Description;
                            // Perform the necessary database operations to save the record
                            BALCoordinator objde = new BALCoordinator();
                            await objde.SaveStudentFeedbackKKAsync(record, staffCode);

                        }
                        return RedirectToAction("ListFeedbackGivenToTrainerKKAsync");
                    }
                    catch (Exception ex)
                    {
                        // Handle any exceptions that may occur during data saving
                        return Json(new { success = false, message = "Error saving records: " + ex.Message });
                    }
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    return await Task.Run(() => Json(new { success = false, message = "Validation error", errors = errors }));
                }
            }
        }
        [HttpGet]
        /// <summary>
        /// This view is for the tab lists showing.
        /// </summary>
        /// <returns>It returns the all list that i want to shown in tabs. </returns>
        public async Task<ActionResult> ListAllFeedbackForStudentKKAsync()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                   new BreadcrumbItem { Name = "CoordinatorDashboard", Url =Url.Action("CoordinatorDashboardPRAsync","Coordinator")},
                   new BreadcrumbItem { Name = "Feedbacks List ", Url = Url.Action("ListAllFeedbackForStudentKKAsync")},
                };
                ViewBag.Breadcrumbs = breadcrumbs;
                return await Task.Run(() => View());
            }
        }
        /// <summary>
        /// This method is showing the feedback list of student which trainer give.
        /// </summary>
        /// <returns>It retunns the list.</returns>    
        [HttpGet]
        public async Task<ActionResult> ListFeedbackForStudentKKAsync()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Coordinator objfs = new Coordinator();
                objfs.BranchCode = Session["BranchCode"].ToString();
                DataSet ds = await objbal.FeedbackFromTrainerKKAsync(objfs);
                Coordinator feed = new Coordinator();
                List<Coordinator> lstfeedback = new List<Coordinator>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Coordinator objfb = new Coordinator();
                    objfb.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objfb.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    objfb.FeedbackFor = ds.Tables[0].Rows[i]["FeedbackFor"].ToString();
                    objfb.FeedbackFrom = ds.Tables[0].Rows[i]["Feedbackfrom"].ToString();
                    DateTime FeedbackSendDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["FeedbackSendDate"].ToString());
                    objfb.feedbcksendDate = FeedbackSendDate.ToString("dd-MM-yyyy");
                    DateTime FeedbackAddedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["FeedbackAddedDate"].ToString());
                    objfb.feedbckaddedDate = FeedbackAddedDate.ToString("dd-MM-yyyy");
                    DateTime FeedbackTillDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["FeedbackTillDate"].ToString());
                    objfb.feedbcktillDate = FeedbackTillDate.ToString("dd-MM-yyyy");
                    objfb.Rating = Convert.ToInt32(ds.Tables[0].Rows[i]["Rating"].ToString());
                    objfb.Comment = ds.Tables[0].Rows[i]["Comment"].ToString();
                    objfb.FollowUpTakenBy = ds.Tables[0].Rows[i]["StaffName"].ToString();
                    lstfeedback.Add(objfb);
                }
                feed.lststudentfeed = lstfeedback;
                return PartialView("ListFeedbackForStudentKKAsync", feed);
            }
        }
        /// <summary>
        /// This method is for showing the list of trainer feedback for student.
        /// </summary>
        /// <returns>It returns the list.</returns>
        [HttpGet]
        public async Task<ActionResult> ListNewFeedbackForStudentKKAsync()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Coordinator objfs = new Coordinator();
                objfs.BranchCode = Session["BranchCode"].ToString();
                DataSet ds = await objbal.NewFeedbackListFromTrainerKKAsync(objfs);
                Coordinator feed = new Coordinator();
                List<Coordinator> lstfeedback = new List<Coordinator>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Coordinator objfb = new Coordinator();
                    objfb.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objfb.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    objfb.FeedbackFor = ds.Tables[0].Rows[i]["FeedbackFor"].ToString();
                    objfb.FeedbackFrom = ds.Tables[0].Rows[i]["Feedbackfrom"].ToString();
                    DateTime FeedbackTillDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["FeedbackTillDate"].ToString());
                    objfb.feedbcktillDate = FeedbackTillDate.ToString("dd-MM-yyyy");
                    DateTime FeedbackSendDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["FeedbackSendDate"].ToString());
                    objfb.feedbcksendDate = FeedbackSendDate.ToString("dd-MM-yyyy");
                    objfb.Description = ds.Tables[0].Rows[i]["Descriptions"].ToString();
                    objfb.FollowUpTakenBy = ds.Tables[0].Rows[i]["StaffName"].ToString();
                    //objfb.Rating = Convert.ToInt32(ds.Tables[0].Rows[i]["Rating"].ToString());
                    lstfeedback.Add(objfb);
                }
                feed.lststudentfeed = lstfeedback;
                return PartialView("ListNewFeedbackForStudentKKAsync", feed);
            }
        }
        /// <summary>
        /// This method is for only to pass partial views list.
        /// </summary>
        /// <returns>It returns the new and already given feedback lists.</returns>
        [HttpGet]
        public async Task<ActionResult> AddTrainerFeedbackKKAsync()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await CourseBindKKAsync();
                //await TrainerBindKKAsync();
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                   new BreadcrumbItem { Name = "CoordinatorDashboard", Url =Url.Action("CoordinatorDashboardPRAsync","Coordinator")},
                   new BreadcrumbItem { Name = "Feedback Lists", Url = Url.Action("ListAllFeedbackForStudentKKAsync")},
                   new BreadcrumbItem { Name = "Trainer Feedback", Url = Url.Action("AddTrainerFeedbackKKAsync")}

                };
                ViewBag.Breadcrumbs = breadcrumbs;
                return PartialView("AddTrainerFeedbackKKAsync");
            }
        }
        /// <summary>
        /// This method is for the showing the batches of course.
        /// </summary>
        /// <param name="CourseCode">This parameter is use for the showing batch of selected course.</param>
        /// <returns>It returns the batch list.</returns>
        public async Task<JsonResult> CourseBatchListKKAsync(string CourseCode)
        {
            Coordinator objcbatch = new Coordinator();
            objcbatch.CourseCode = CourseCode;
            objcbatch.BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await objbal.CourseBatchKKAsync(objcbatch);
            List<SelectListItem> cbatches = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                cbatches.Add(new SelectListItem { Text = dr["BatchName"].ToString(), Value = dr["BatchCode"].ToString() });
            }
            return Json(cbatches, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// This method is for to save the trainer feedback data.
        /// </summary>
        /// <param name="records">It is use for pass the records to the view.</param>
        /// <returns>It returns the saved data</returns>
        [HttpPost]
        public async Task<ActionResult> AddTrainerFeedbackKKAsync(List<Coordinator> records)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                if (records != null && (records).Any())
                {
                    string staffCode = Session["StaffCode"].ToString();
                    Coordinator objC = new Coordinator();
                    //objC.StaffCode = Session["StaffCode"].ToString();
                    try
                    {
                        // Loop through each record and save it to the database
                        foreach (var record in records)
                        {
                            objC.StudentCode = record.StudentCode;
                            objC.CourseCode = record.CourseCode;
                            objC.FeedbackFrom = record.TrainerName;
                            objC.feedbcktillDate = record.Date.ToString();
                            objC.Description = record.Description;
                            // Perform the necessary database operations to save the record
                            BALCoordinator objde = new BALCoordinator();
                            await objde.SaveTrainerFeedbackKKAsync(record, staffCode);

                        }
                        return RedirectToAction("ListAllFeedbackForStudentKKAsync");
                        //return Json(new { success = true, message = "Records saved successfully" });
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false, message = "Error saving records: " + ex.Message });
                    }
                }
                else
                {
                    // If the model state is not valid, return validation errors
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    return await Task.Run(() => Json(new { success = false, message = "Validation error", errors = errors }));
                }
            }
        }


        //-----------------Kirti Feedback  End ---------------------------------------------------------------------------//











    }
}