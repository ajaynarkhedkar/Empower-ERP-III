using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GSTEducationERPLibrary.Placement;
using System.Data;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace GSTEducationERP.Controllers
{

    public class PlacementController : Controller
    {
        private readonly BALPlacement objbal;//Achieves Encapsulation

        public PlacementController()
        {
            objbal = new BALPlacement();  //Achieves Abstraction
        }
        // GET: Coordinator
        public class BreadcrumbItem 
        {
            public string Name { get; set; }
            public string Url { get; set; }

        }

        Placement objPlacement = new Placement();

        //----------------------------------------Priyanka Dashboard---------------------------------//

        // GET: Placement
        /// <summary>
        /// method for to display Count for Dashboard.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> PlcementDashboardPCAsync()
        {
            Placement objnew = new Placement();
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                objnew.BranchCode = Session["BranchCode"].ToString();
                await CourseBindPCAsync(objnew);
                BALPlacement objbal = new BALPlacement();
                SqlDataReader dr;

                dr = await objbal.CountPlacedCandidatePCAsync(objnew);
                dr.Read();
                objnew.Studentid = Convert.ToInt32(dr["PlacedCandidate"].ToString());
                dr.Close();
                ViewBag.PlacedStudent = objnew.Studentid;

                dr = await objbal.CountActiveCandidatePCAsync(objnew);
                dr.Read();
                objnew.Studentid = Convert.ToInt32(dr["ActiveCandidate"].ToString());
                dr.Close();
                ViewBag.ActiveStudent = objnew.Studentid;

                dr = await objbal.CountCompaniesPCAsync(objnew);
                dr.Read();
                objnew.CompanyId = Convert.ToInt32(dr["Companies"].ToString());
                dr.Close();
                ViewBag.Companies = objnew.CompanyId;

                dr = await objbal.CountOpeningPCAsync(objnew);
                dr.Read();
                objnew.OpenningId = Convert.ToInt32(dr["Openings"].ToString());
                dr.Close();
                ViewBag.Openings = objnew.OpenningId;

                dr = await objbal.CountTotalCandidatePCAsync(objnew);
                dr.Read();
                objnew.Studentid = Convert.ToInt32(dr["Candidate"].ToString());
                dr.Close();
                ViewBag.Studentid = objnew.Studentid;

                // code for Doughnut chart
                DataSet D = await objbal.GraphPositionOpenClosePCAsync(objnew);
                List<object> dataPoints2 = new List<object>();
                foreach (DataRow row in D.Tables[0].Rows)
                {
                    string designationName = row["DesignationName"].ToString();
                    int openJobs = Convert.ToInt32(row["OpenJobs"]);
                    int closedJobs = Convert.ToInt32(row["ClosedJobs"]);

                    // Construct data point object for each designation
                    var dataPoint = new
                    {
                        designation = designationName,
                        open = openJobs,
                        closed = closedJobs
                    };
                    dataPoints2.Add(dataPoint);
                }
                // Serialize the list of data points to JSON and store it in ViewBag
                ViewBag.DataPoints2 = Newtonsoft.Json.JsonConvert.SerializeObject(dataPoints2);

                // Retrieve data for Open & Close Job with respective Companies
                DataSet Dsa = await objbal.GraphCompanyPCAsync(objnew);
                List<object> dataPoints = new List<object>();
                // Loop through the retrieved dataset and construct data points
                foreach (DataRow row in Dsa.Tables[0].Rows)
                {
                    string companyName = row["CompanyName"].ToString();
                    int openJobs = Convert.ToInt32(row["OpenJobs"]);
                    int closedJobs = Convert.ToInt32(row["ClosedJobs"]);
                    dataPoints.Add(new { label = companyName, open = openJobs, closed = closedJobs });
                }
                ViewBag.DataPoints = Newtonsoft.Json.JsonConvert.SerializeObject(dataPoints);

                // Retrieve data for Candidate Count as per Status
                DataSet Ds = await objbal.GraphCandidateCountPCAsync(objnew);
                DataTable dataTable = Ds.Tables[0];
                List<object> dataPoints1 = new List<object>();
                foreach (DataRow row in dataTable.Rows)
                {
                    string status = row["Status"].ToString();
                    // Fetching counts for each status
                    int placed = Convert.ToInt32(row["Placed"]);
                    int scheduled = Convert.ToInt32(row["Scheduled"]);
                    int shortlisted = Convert.ToInt32(row["Shortlisted"]);
                    int rejected = Convert.ToInt32(row["Rejected"]);
                    int rescheduled = Convert.ToInt32(row["Rescheduled"]);
                    // Adding each status and its count to the dataPoints1 list
                    dataPoints1.Add(new { label = status, value = placed });
                    dataPoints1.Add(new { label = status, value = scheduled });
                    dataPoints1.Add(new { label = status, value = shortlisted });
                    dataPoints1.Add(new { label = status, value = rejected });
                    dataPoints1.Add(new { label = status, value = rescheduled });
                }
                ViewBag.Ds1 = Newtonsoft.Json.JsonConvert.SerializeObject(dataPoints1);
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                    new BreadcrumbItem { Name = "PlacementDashboard",Url =Url.Action("PlcementDashboardPCAsync","Placement") },
                };
                ViewBag.Breadcrumbs = breadcrumbs;
                return View();
            }
        }
        /// <summary>
        /// method to view graphical presentation for Company and jobs in PieChart.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> PlcementDashCandiChartPCAsync()
        {
            //BALPlacement objbal = new BALPlacement();
            Placement obj = new Placement();
            SqlDataReader dr;
            obj.BranchCode = Session["BranchCode"].ToString();
            dr = await objbal.CountCompaniesPCAsync(obj);
            dr.Read();
            obj.CompanyId = Convert.ToInt32(dr["Companies"].ToString());
            dr.Close();
            ViewBag.Companies = obj.CompanyId;

            dr = await objbal.CountOpeningPCAsync(obj);
            dr.Read();
            obj.OpenningId = Convert.ToInt32(dr["Openings"].ToString());
            dr.Close();
            ViewBag.Openings = obj.OpenningId;
            return View();
        }
        /// <summary>
        /// method to bind course to dropdown for Dashboard Data.
        /// </summary>
        /// <param name="Courseid"></param>
        /// <returns></returns>
        public async Task<JsonResult> CourseBindPCAsync(Placement obj)
        {
            // BALPlacement objbal = new BALPlacement();
            obj.BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await objbal.GetCoursePCAsync(obj);
            List<SelectListItem> Courselist = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Courselist.Add(new SelectListItem { Text = dr["CourseName"].ToString(), Value = dr["CourseCode"].ToString() });
            }
            ViewBag.Course = Courselist;
            return await Task.Run(() => Json(Courselist, JsonRequestBehavior.AllowGet));
        }
        /// <summary>
        /// method to Get the list of batches based on course.
        /// </summary>
        /// <param name="coursecode">Course code for batches.</param>
        /// <returns>List of batches.</returns>
        public async Task<JsonResult> BatchBindPCAsync(Placement objp)
        {
            objp.BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await objbal.BatchListPCAsync(objp);
            List<SelectListItem> Batchlist = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Batchlist.Add(new SelectListItem { Text = dr["BatchName"].ToString(), Value = dr["BatchCode"].ToString() });
            }
            ViewBag.BatchName = Batchlist;
            return await Task.Run(() => Json(Batchlist, JsonRequestBehavior.AllowGet));
        }
        /// <summary>
        /// Method to get the updated count on the selection of Batch-Dropdown.
        /// </summary>
        /// <param name="BatchName"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetBatchCountsPCAsync(Placement obj)
        {
            obj.BranchCode = Session["BranchCode"].ToString();
            obj.BatchName = obj.BatchName;
            SqlDataReader dr = await objbal.DashUpdateCountPCAsync(obj);
            try
            {
                while (await dr.ReadAsync())
                {
                    obj.NoOfStudent = Convert.ToInt32(dr["NoOfStudent"]);
                    obj.Placed = Convert.ToInt32(dr["PlacedCandidates"]);
                    obj.Active = Convert.ToInt32(dr["ActiveCandidates"]);
                }
                ViewBag.StudentCount = obj.NoOfStudent;
                ViewBag.PlacedCount = obj.Placed;
                ViewBag.ActiveCount = obj.Active;
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                dr.Close();
            }
        }
        //----------------------------------Snehal Exam Management-------------------------------------------------//
        /// <summary>
        /// This action is used to show all types of exams (Assigned,Pending,Conducted).
        /// </summary>
        /// <returns>It show list of exams.</returns>
        public async Task<ActionResult> ListAllAssignedExamSNAsync()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                    new BreadcrumbItem { Name = "Dashboard", Url =Url.Action("PlcementDashboardPCAsync", "Placement") },
                    new BreadcrumbItem { Name = "Exam", Url = Url.Action("ListAllAssignedExamSNAsync", "Placement")}
                };

                ViewBag.Breadcrumbs = breadcrumbs;
                await ListAllCourseAsync();
                return await Task.Run(() => View());
            }
        }
        /// <summary>
        /// This action is used to get assigned exam list for internal students.
        /// </summary>
        /// <param name="statusId"> Statusid for assigned exams.</param>
        /// <returns> Returns partial view of assigned exam list for internal students.</returns>
        public async Task<ActionResult> ListAssignedExamIntSNAsync(int id = 0, string courseCode = "", DateTime? startDate = null, DateTime? endDate = null)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await ListAllCourseSNAsync();
                await ListTrainerSNAsync();
                List<Placement> model = await ListInternalStudentExamSNAsync(id, courseCode, startDate ?? DateTime.MinValue, endDate ?? DateTime.MaxValue);


                Placement ObjList = new Placement();
                ObjList.BranchCode = Session["BranchCode"].ToString();
                ObjList.lstExams = model;
                ObjList.ExamDate = DateTime.Today;
                return PartialView("_ListAssignedExamIntSNAsync", ObjList);
            }
        }
        /// <summary>
        /// This action is used to get pending exam list for internal students.
        /// </summary>
        /// <param name="statusId"> Statusid for pending exams.</param>
        /// <returns> Returns partial view of pending exam list for internal students.</returns>
        public async Task<ActionResult> ListPendingExamIntSNAsync(int id = 0, string courseCode = "", DateTime? startDate = null, DateTime? endDate = null)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                List<Placement> model = await ListInternalStudentExamSNAsync(id, courseCode, startDate ?? DateTime.MinValue, endDate ?? DateTime.MaxValue);
                Placement ObjList = new Placement();
                ObjList.lstPendingExams = model;
                return PartialView("_ListPendingExamIntSNAsync", ObjList);
            }
        }
        /// <summary>
        /// This action is used to get Conducted exam list for internal students.
        /// </summary>
        /// <param name="statusId"> Statusid for conducted exams.</param>
        /// <returns> Returns partial view of conducted exam list for internal students.</returns>
        public async Task<ActionResult> ListConductedExamIntSNAsync(int id = 0, string courseCode = "", DateTime? startDate = null, DateTime? endDate = null)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                List<Placement> model = await ListInternalStudentExamSNAsync(id, courseCode, startDate ?? DateTime.MinValue, endDate ?? DateTime.MaxValue);
                Placement ObjList = new Placement();
                ObjList.lstConductedExams = model;
                return PartialView("_ListConductedExamIntSNAsync", ObjList);
            }
        }
        /// <summary>
        /// This list is used to get statuswise Exam list for internal students.
        /// </summary>
        /// <param name="statusId"> Statusid is used to get statuswise data like asssigned, pending and conducted. </param>
        /// <returns> It returns status wise exam list for internal students.</returns>
        private async Task<List<Placement>> ListInternalStudentExamSNAsync(int statusId, string courseCode, DateTime startDate, DateTime endDate)
        {
            string BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await objbal.ListInternalStudentsExamSNAsync(statusId, BranchCode, courseCode, startDate, endDate);
            List<Placement> lstConductedExams = new List<Placement>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Placement objP = new Placement();
                    objP.AssignExamId = Convert.ToInt32(ds.Tables[0].Rows[i]["AssignTestId"].ToString());
                    objP.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objP.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    objP.ExamName = ds.Tables[0].Rows[i]["ExamName"].ToString();
                    objP.LabName = ds.Tables[0].Rows[i]["LabName"].ToString();
                    objP.SupervisorName = ds.Tables[0].Rows[i]["StaffName"].ToString();
                    DateTime Time = Convert.ToDateTime(ds.Tables[0].Rows[i]["TestTime"].ToString());
                    objP.TimeString = Time.ToString("t");
                    DateTime TestDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["TestDate"].ToString());
                    objP.DateString = TestDate.ToString("dd-MM-yyyy");
                    objP.SDuration = ds.Tables[0].Rows[i]["Duration"].ToString();
                    objP.TotalMarks = Convert.ToInt32(ds.Tables[0].Rows[i]["TotalMarks"].ToString());
                    objP.Status = ds.Tables[0].Rows[i]["IsResultAdded"].ToString();
                    lstConductedExams.Add(objP);
                }
            }
            return lstConductedExams;
        }
        /// <summary>
        /// This action is used to get Assigned exams for external students.
        /// </summary>
        /// <param name="id"> This parameters is used to pass status id to get exam list of that particular status.</param>
        /// <returns>It returns the assigned exams for external students from statusid.</returns>
        public async Task<ActionResult> ListAssignedExamExtSNAsync(int id = 0, string courseCode = "", DateTime? startDate = null, DateTime? endDate = null)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {

                List<Placement> model = await ListExternalStudentExamSNAsync(id, startDate ?? DateTime.MinValue, endDate ?? DateTime.MaxValue);
                Placement ObjList = new Placement();
                ObjList.BranchCode = Session["BranchCode"].ToString();
                await ListExternalStudentSNAsync();
                await ListLabsSNAsync(ObjList.BranchCode);
                await ListTrainerSNAsync();
                ObjList.lstExams = model;
                ObjList.ExamDate = DateTime.Today;
                return PartialView("_ListAssignedExamExtSNAsync", ObjList);
            }

        }
        /// <summary>
        /// This action is used to get pending exams for external students.
        /// </summary>
        /// <param name="id"> this parameters is used to pass status id.</param>
        /// <returns>It returns the partial view of pending exams for external students from statusid.</returns>
        public async Task<ActionResult> ListPendingExamExtSNAsync(int id = 0, string courseCode = "", DateTime? startDate = null, DateTime? endDate = null)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                List<Placement> model = await ListExternalStudentExamSNAsync(id, startDate ?? DateTime.MinValue, endDate ?? DateTime.MaxValue);
                Placement ObjList = new Placement();
                ObjList.lstPendingExams = model;
                return PartialView("_ListPendingExamExtSNAsync", ObjList);
            }
        }
        /// <summary>
        /// This action is used to get Conducted exams for external students.
        /// </summary>
        /// <param name="id"> This parameter is used to pass status id to get list of that particular status.</param>
        /// <returns>It returns the partial view of conducted exams for external students from statusid.</returns>
        public async Task<ActionResult> ListConductedExamExtSNAsync(int id = 0, string courseCode = "", DateTime? startDate = null, DateTime? endDate = null)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {

                List<Placement> model = await ListExternalStudentConductedExamSNAsync(id, startDate ?? DateTime.MinValue, endDate ?? DateTime.MaxValue);
                Placement ObjList = new Placement();
                ObjList.BranchCode = Session["BranchCode"].ToString();
                ObjList.lstConductedExams = model;


                return PartialView("_ListConductedExamExtSNAsync", ObjList);
            }
        }
        /// <summary>
        /// This list is used to get statuswise list of exams for external students.
        /// </summary>
        /// <param name="statusId">Status id is passed to get specific exam list from database.</param>
        /// <returns>It returns the exam list for particular status.  </returns>
        private async Task<List<Placement>> ListExternalStudentExamSNAsync(int statusId, DateTime startDate, DateTime endDate)
        {
            Placement objp = new Placement();
            objp.StatusId = statusId;
            objp.StartDate = startDate;
            objp.EndDate = endDate;
            objp.BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await objbal.ListExternalStudentsExamSNAsync(objp);
            List<Placement> lstAssignedExam = new List<Placement>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Placement objP = new Placement();
                objP.AssignExamId = Convert.ToInt32(ds.Tables[0].Rows[i]["AssignTestId"].ToString());
                objP.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                objP.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                objP.ExamName = ds.Tables[0].Rows[i]["ExamName"].ToString();
                objP.SupervisorName = ds.Tables[0].Rows[i]["StaffName"].ToString();
                objP.LabName = ds.Tables[0].Rows[i]["LabName"].ToString();
                DateTime Time = Convert.ToDateTime(ds.Tables[0].Rows[i]["TestTime"].ToString());
                objP.TimeString = Time.ToString("t");
                DateTime TestDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["TestDate"].ToString());
                objP.DateString = TestDate.ToString("dd-MM-yyyy");
                objP.SDuration = ds.Tables[0].Rows[i]["Duration"].ToString();
                objP.TotalMarks = Convert.ToInt32(ds.Tables[0].Rows[i]["TotalMarks"].ToString());
                lstAssignedExam.Add(objP);
            }
            return lstAssignedExam;
        }
        /// <summary>
        /// This action is used to get conducted exam of external students.
        /// </summary>
        /// <param name="statusId">statusId is for conducted exam is passed.</param>
        /// <returns>It returns conducted exams of external students.</returns>
        private async Task<List<Placement>> ListExternalStudentConductedExamSNAsync(int statusId, DateTime startDate, DateTime endDate)
        {
            Placement objp = new Placement();
            objp.StatusId = statusId;
            objp.StartDate = startDate;
            objp.EndDate = endDate;
            objp.BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await objbal.ListConductedExamExtStudentSN(objp);
            List<Placement> lstAssignedExam = new List<Placement>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Placement objP = new Placement();
                objP.AssignExamId = Convert.ToInt32(ds.Tables[0].Rows[i]["AssignTestId"].ToString());
                objP.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                objP.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                objP.ExamName = ds.Tables[0].Rows[i]["ExamName"].ToString();
                objP.SupervisorName = ds.Tables[0].Rows[i]["StaffName"].ToString();
                objP.LabName = ds.Tables[0].Rows[i]["LabName"].ToString();
                DateTime Time = Convert.ToDateTime(ds.Tables[0].Rows[i]["TestTime"].ToString());
                objP.TimeString = Time.ToString("t");
                DateTime TestDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["TestDate"].ToString());
                objP.DateString = TestDate.ToString("dd-MM-yyyy");
                objP.SDuration = ds.Tables[0].Rows[i]["Duration"].ToString();
                objP.TotalMarks = Convert.ToInt32(ds.Tables[0].Rows[i]["TotalMarks"].ToString());
                var obtainedMarksString = ds.Tables[0].Rows[i]["ObtainedMarks"].ToString();
                objP.ResultStatus = ds.Tables[0].Rows[i]["ResultStatus"].ToString();
                objP.ObtainedMarks = string.IsNullOrEmpty(obtainedMarksString) ? 0 : Convert.ToSingle(obtainedMarksString);
                lstAssignedExam.Add(objP);
            }
            return lstAssignedExam;
        }
        /// <summary>
        /// This Actionmethod is used to get all exams list which is added by trainer or coordinator.
        /// </summary>
        /// <returns> It returns the all exams list.</returns>
        public async Task<ActionResult> ListAllExamsSNAsync(string courseCode, DateTime startDate, DateTime endDate)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {

                await ListTechologySNAsync();
                await ListAllCourseSNAsync();
                List<Placement> model = await ListAllExamSNAsync(courseCode, startDate, endDate);

                Placement ObjList = new Placement();
                ObjList.lstAllExams = model;
                return PartialView(ObjList);
            }
        }

        /// <summary>
        /// This function is used to get list of all exams.
        /// </summary>
        /// <returns>Returns the list of all exams of all courses. </returns>
        private async Task<List<Placement>> ListAllExamSNAsync(string courseCode, DateTime startDate, DateTime endDate)
        {
            Placement model = new Placement();
            model.BranchCode = Session["BranchCode"].ToString();
            model.CourseCode = courseCode;
            model.StartDate = startDate;
            model.EndDate = endDate;
            DataSet ds = await objbal.ListAllExamSNAsync(model);
            List<Placement> lstAllExams = new List<Placement>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Placement objP = new Placement();
                objP.ExamId = Convert.ToInt32(ds.Tables[0].Rows[i]["TestId"].ToString());
                objP.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                objP.ExamName = ds.Tables[0].Rows[i]["ExamName"].ToString();
                objP.TotalMarks = Convert.ToInt32(ds.Tables[0].Rows[i]["TotalMarks"].ToString());
                objP.SDuration = ds.Tables[0].Rows[i]["Duration"].ToString();
                objP.TechnologyName = ds.Tables[0].Rows[i]["TechnologyNames"].ToString();
                objP.FilePth = ds.Tables[0].Rows[i]["ExamFile"].ToString();
                lstAllExams.Add(objP);
            }
            return lstAllExams;
        }
        /// <summary>
        /// this method is uesd to get view for add new exam.
        /// </summary>
        /// <returns>It returns the view to add new exams.</returns>

        /// <summary>
        /// This method is used to get all courses list.
        /// </summary>
        /// <returns>It returns list of all courses.</returns>
        public async Task ListAllCourseSNAsync()
        {
            DataSet ds = await objbal.ListAllCourseSN();
            List<SelectListItem> CourseList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                CourseList.Add(new SelectListItem { Text = dr["CourseName"].ToString(), Value = dr["Coursecode"].ToString() });
            }
            ViewBag.Course = CourseList;
        }
        public async Task ListAllCourseAsync()
        {
            DataSet ds = await objbal.ListAllCourseSN();
            List<SelectListItem> CourseList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                CourseList.Add(new SelectListItem { Text = dr["CourseName"].ToString(), Value = dr["Coursecode"].ToString() });
            }
            ViewBag.Courses = CourseList;
        }
        public async Task ListAllOpeningAsync()
        {
            DataSet ds = await objbal.ListAllOpeningSN();
            List<SelectListItem> OpeningList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                OpeningList.Add(new SelectListItem { Text = dr["CompanyName"].ToString(), Value = dr["OpeningCode"].ToString() });
            }
            ViewBag.Opening = OpeningList;
        }
        public async Task ListTechologySNAsync()
        {
            DataSet ds = await objbal.ListAllTechnologySN();
            List<SelectListItem> TechnologyList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TechnologyList.Add(new SelectListItem { Text = dr["SkillName"].ToString(), Value = dr["SkillId"].ToString() });
            }

            ViewBag.Technology = new MultiSelectList(TechnologyList, "Value", "Text");
        }

        /// <summary>
        /// This method is uesd to save new exam.
        /// </summary>
        /// <param name="objP"> Object of model class is passed to save new exam.</param>
        /// <returns>It adds new exam record to database.</returns>
        [HttpPost]
        public async Task<JsonResult> AddNewExamSNAsync(Placement objP)
        {

            try
            {
                await ListAllCourseSNAsync();

                if (objP.TestPaperFile != null && objP.TestPaperFile.ContentLength > 0)
                {


                    var fileName = Path.GetFileNameWithoutExtension(objP.TestPaperFile.FileName);
                    var extension = Path.GetExtension(objP.TestPaperFile.FileName);
                    fileName = fileName + extension;
                    var filePath = "~/Content/Placement/docs/TestFiles/" + fileName; // Relative path
                    objP.FilePth = filePath;
                    fileName = Path.Combine(Server.MapPath("~/Content/Placement/docs/TestFiles/"), fileName);
                    objP.TestPaperFile.SaveAs(fileName);
                    objP.StaffCode = Session["StaffCode"].ToString();
                    objP.StatusId = 44;
                    await objbal.AddNewExamSNAsync(objP);
                    return Json(new { success = true, message = "Exam Added Successfully..." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ModelState.AddModelError("TestPaperFile", "Please choose a file");
                    return Json(new { success = false, message = "Please choose a file" }, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {

                ViewBag.ErrorMessage = ex.Message;
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// This Method is used to assign exam for internal students.
        /// </summary>
        /// <returns> It returns the view to assign exam for internal students.</returns>
        [HttpGet]
        public async Task<ActionResult> AssignExamInternalStudentSNAsync()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await ListAllCourseSNAsync();
                await ListTrainerSNAsync();
                Placement model = new Placement();
                model.BranchCode = Session["BranchCode"].ToString();
                await ListLabsSNAsync(model.BranchCode);
                model.ExamDate = DateTime.Now;
                return PartialView(model);
            }
        }
        /// <summary>
        /// This action is used to save assigned exam data for Internal students.
        /// </summary>
        /// <param name="ObjP">This object is used to pass value for assign exam.</param>
        /// <returns> It saves the assigned exam data.</returns>
        [HttpPost]
        public async Task<ActionResult> AssignExamInternalStudentSNAsync(Placement ObjP)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                try
                {
                    ObjP.StaffCode = Session["StaffCode"].ToString();
                    //BALPlacement AssignExam = new BALPlacement();
                    await objbal.AssignExamIntStudentSN(ObjP);
                    return RedirectToAction("ListAllAssignedExamSNAsync");
                }
                catch (Exception ex)
                {
                    // Log the exception and return an error view or redirect to an error page
                    // Alternatively, you can return a specific error message to the user
                    return View("Error");
                }
            }
        }
        /// <summary>
        /// This Method is used to assign exam for External students.
        /// </summary>
        /// <returns> It returns the view to assign exam for External students.</returns>
        [HttpGet]
        public async Task<ActionResult> AssignExamExternalStudentSNAsync()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await ListExternalStudentSNAsync();
                await ListTrainerSNAsync();
                Placement model = new Placement();
                model.BranchCode = Session["BranchCode"].ToString();
                await ListLabsSNAsync(model.BranchCode);
                model.ExamDate = DateTime.Now;
                return View(model);
            }
        }
        /// <summary>
        /// This action is used to save assigned exam data for External students.
        /// </summary>
        /// <param name="ObjP">This object is used to pass value for assign exam.</param>
        /// <returns> It saves the assigned exam data.</returns>
        [HttpPost]
        public async Task<ActionResult> AssignExamExternalStudentSNAsync(Placement ObjP)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                try
                {
                    ObjP.StaffCode = Session["StaffCode"].ToString();
                    // BALPlacement AssignExam = new BALPlacement();
                    await objbal.AssignExamExtStudentSN(ObjP);
                    return RedirectToAction("ListAllAssignedExamSNAsync");
                }
                catch (Exception ex)
                {
                    // Log the exception and return an error view or redirect to an error page
                    // Alternatively, you can return a specific error message to the user
                    return View("Error");
                }
            }
        }
        /// <summary>
        /// This action is used to get list of released batch.
        /// </summary>
        /// <param name="CourseCode">Coursecode is passed to get coursewise batches.</param>
        /// <returns>It returns the coursewise batches.</returns>
        public async Task<JsonResult> ListALLBatchSNAsync(string CourseCode)
        {
            DataSet ds = await objbal.ListReleasedBatchSN(CourseCode);

            List<SelectListItem> BatchList = new List<SelectListItem>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                BatchList.Add(new SelectListItem { Text = dr["BatchName"].ToString(), Value = dr["BatchCode"].ToString() });
            }

            return Json(BatchList, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// This action is used to get list of students.  
        /// </summary>
        /// <param name="BatchCode">BatchCode is passed to get single batch students.</param>
        /// <returns>It returns the students of selected batch.</returns>
        public async Task<JsonResult> ListStudentSNAsync(string BatchCode)
        {
            DataSet ds = await objbal.ListStudentSN(BatchCode);

            List<SelectListItem> StudentList = new List<SelectListItem>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                StudentList.Add(new SelectListItem { Text = dr["FullName"].ToString(), Value = dr["CandidateCode"].ToString() });
            }

            return Json(StudentList, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// This action is used to get exam list.
        /// </summary>
        /// <param name="CourseCode">Coursecode is used to get coursewise list.</param>
        /// <returns> It returns Coursewise list of exams.</returns>
        public async Task<JsonResult> ListExamSNAsync(string CourseCode)
        {
            DataSet ds = await objbal.ListCourseWiseExamSN(CourseCode);
            List<SelectListItem> ExamList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ExamList.Add(new SelectListItem { Text = dr["TestName"].ToString(), Value = dr["TestId"].ToString() });
            }
            return Json(ExamList, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// This action is used to get exam list.
        /// </summary>
        /// <param name="CourseCode">Coursecode is used to get coursewise list.</param>
        /// <returns> It returns Coursewise list of exams.</returns> 
        public async Task ListExamsSNAsync(string CourseCode)
        {
            DataSet ds = await objbal.ListCourseWiseExamSN(CourseCode);

            List<SelectListItem> ExamList = new List<SelectListItem>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ExamList.Add(new SelectListItem { Text = dr["TestName"].ToString(), Value = dr["TestId"].ToString() });
            }

            ViewBag.Exam = ExamList;
        }
        /// <summary>
        /// This action is used to bind trainer list.
        /// </summary>
        /// <returns>It returns viewbag for trainer list.</returns>
        public async Task ListTrainerSNAsync()
        {
            DataSet ds = await objbal.ListTrainerSN();

            List<SelectListItem> TrainerList = new List<SelectListItem>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TrainerList.Add(new SelectListItem { Text = dr["StaffName"].ToString(), Value = dr["StaffCode"].ToString() });
            }

            ViewBag.Trainer = TrainerList;
        }
        /// <summary>
        /// This action is used to bind center list.
        /// </summary>
        /// <returns>It returns viewbag for center list.</returns>
        public async Task ListCenterSNAsync()
        {
            DataSet ds = await objbal.ListCenterSN();
            List<SelectListItem> CenterList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                CenterList.Add(new SelectListItem { Text = dr["CenterName"].ToString(), Value = dr["CenterCode"].ToString() });
            }
            ViewBag.Center = CenterList;

        }
        /// <summary>
        /// This action is used to get external student list .
        /// </summary>
        /// <param name="CourseCode">Coursecode is passed to get coursewise students.</param>
        /// <returns>Returns the coursewise external student list. </returns>
        public async Task ListExternalStudentSNAsync()
        {
            Placement objP = new Placement();
            objP.BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await objbal.ListExternalStudentsSN(objP.BranchCode);

            List<object> StudentList = new List<object>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                var student = new
                {
                    Text = dr["FullName"].ToString(),
                    Value = dr["CandidateCode"].ToString(),
                    CandidateSkill = dr["SkillNames"].ToString()
                };

                StudentList.Add(student);
            }

            ViewBag.Student = StudentList;
        }

        /// <summary>
        /// This action is used to get external student list .
        /// </summary>
        /// <param name="CourseCode">Coursecode is passed to get coursewise students.</param>
        /// <returns>Returns the coursewise external student list. </returns>
        public async Task ListExternalStudentsSNAsync(string CourseCode)
        {
            DataSet ds = await objbal.ListExternalStudentsSN(CourseCode);
            List<SelectListItem> StudentList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                StudentList.Add(new SelectListItem { Text = dr["FullName"].ToString(), Value = dr["CandidateCode"].ToString() });
            }
            ViewBag.Student = StudentList;
        }
        /// <summary>
        /// This action is used to get Lab List.
        /// </summary>
        /// <param name="CenterCode">It used to get centerwise labs.</param>
        /// <returns>It returns the Centerwise Lablist.</returns>
        public async Task<JsonResult> ListLabSNAsync(string CenterCode)
        {
            DataSet ds = await objbal.ListLabSN(CenterCode);
            List<SelectListItem> LabList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                LabList.Add(new SelectListItem { Text = dr["LabName"].ToString(), Value = dr["LabCode"].ToString() });
            }
            return Json(LabList, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// This action is used to get Lab List.
        /// </summary>
        /// <param name="CenterCode">It used to get centerwise labs.</param>
        /// <returns>It returns the Centerwise Lablist.</returns>
        public async Task ListLabsSNAsync(string BranchCode)
        {
            DataSet ds = await objbal.ListLabSN(BranchCode);

            List<SelectListItem> LabList = new List<SelectListItem>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                LabList.Add(new SelectListItem { Text = dr["LabName"].ToString(), Value = dr["LabCode"].ToString() });
            }

            ViewBag.Lab = LabList;
        }
        /// <summary>
        /// It used to get data of exam which having changes.
        /// </summary>
        /// <param name="id">Id is passed to get details of that exam.</param>
        /// <returns>It returns the view to change details of exam from that id.</returns>
        [HttpGet]
        public async Task<ActionResult> UpdateExamAsyncSN(int id)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await ListAllCourseSNAsync();
                await ListTechologySNAsync();
                DataSet ds = await objbal.DetailsExamSN(id);

                Placement objP = new Placement();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    objP.ExamId = Convert.ToInt32(ds.Tables[0].Rows[i]["TestId"].ToString());
                    objP.ExamName = ds.Tables[0].Rows[i]["TestName"].ToString();
                    objP.CourseCode = ds.Tables[0].Rows[i]["CourseCode"].ToString();
                    objP.SelectedDuration = TimeSpan.Parse(ds.Tables[0].Rows[i]["Duration"].ToString());
                    objP.TotalMarks = Convert.ToInt32(ds.Tables[0].Rows[i]["TotalMarks"].ToString());
                    objP.PassingMarks = Convert.ToInt32(ds.Tables[0].Rows[i]["PassingMarks"].ToString());
                    objP.FilePth = ds.Tables[0].Rows[i]["TestPaperFile"].ToString();
                    objP.TechnologyName = ds.Tables[0].Rows[i]["SkillId"].ToString();
                    //var selectedTechnologies = ds.Tables[0].Rows[i]["SkillId"].ToString();
                    //ViewBag.SelectedTechnologies = new SelectList(selectedTechnologies);
                    objP.StaffCode = ds.Tables[0].Rows[i]["TrainerCodeTestAddedBy"].ToString();
                    objP.TotalNoOfQuestion = Convert.ToInt32(ds.Tables[0].Rows[i]["TotalNoOfQuestion"].ToString());
                    // Split the comma-separated string of technology IDs into an array
                    var selectedTechnologies = ds.Tables[0].Rows[i]["SkillId"].ToString().Split(',');
                    // Convert the array of strings to an array of integers
                    var selectedTechIds = Array.ConvertAll(selectedTechnologies, int.Parse);
                    // Populate ViewBag with the array of selected technology IDs
                    // ViewBag.SelectedTechnologies = new MultiSelectList(await GetTechnologyList(), "TechnologyId", "TechnologyName", selectedTechIds);
                }

                return PartialView(objP);
            }
        }
        /// <summary>
        /// This action is used to update data of exam.
        /// </summary>
        /// <param name="objP">This parameter is used to pass details.</param>
        /// <returns>It Updates the details of exam for selected or required exam from exam id.</returns>
        [HttpPost]
        public async Task<ActionResult> UpdateExamAsyncSN(Placement objP)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {

                await ListAllCourseSNAsync();
                HttpPostedFileBase file = objP.TestPaperFile as HttpPostedFileBase;
                if (file != null && file.ContentLength > 0)
                {
                    try
                    {
                        string fileName = Path.GetFileName(objP.TestPaperFile.FileName);
                        string folderPath = Server.MapPath("~/Content/Placement/docs");
                        string filePath = Path.Combine(folderPath, fileName);

                        // Ensure the folder exists
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        // Save the new file to the specified path
                        objP.TestPaperFile.SaveAs(filePath);

                        // Update other properties in your Placement object
                        objP.FilePth = filePath; // Update the file path


                        // Call the method to update the exam (assuming it updates the details in a database)
                        //  BALPlacement objB = new BALPlacement();
                        await objbal.UpdateExamSN(objP);
                    }
                    catch (Exception ex)
                    {
                        // Handle the exception (log, show user-friendly message, etc.)
                        ModelState.AddModelError(string.Empty, "An error occurred during file update: " + ex.Message);
                        return View(objP);
                    }
                }
                else
                {



                    // Call the method to update the exam (assuming it updates the details in a database)
                    //BALPlacement objB = new BALPlacement();
                    await objbal.UpdateExamSN(objP);
                }

                return RedirectToAction("ListAllExamsSNAsync");
            }
        }
        /// <summary>
        /// This action is used to get status names.
        /// </summary>
        /// <returns>It returns status list.</returns>
        public async Task ListStatusAsyncSN()
        {
            //  BALPlacement Getstatus = new BALPlacement();
            DataSet ds = await objbal.ListStatusSN();

            List<SelectListItem> StatusList = new List<SelectListItem>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                StatusList.Add(new SelectListItem { Text = dr["Status"].ToString(), Value = dr["StatusId"].ToString() });
            }

            ViewBag.Status = StatusList;
        }
        /// <summary>
        /// Thiis action is used to get details of which is having changes.
        /// </summary>
        /// <param name="id">id is passed to get details of that id.</param>
        /// <returns>It returns the details of id and show on view.</returns>
        [HttpGet]
        public async Task<ActionResult> UpdateAssignExamExternalAsyncSN(int id)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {


                await ListTrainerSNAsync();
                await ListStatusAsyncSN();
                //  BALPlacement objExam = new BALPlacement();
                DataSet ds = await objbal.DetailsAssignExamExternalSN(id);
                Placement objP = new Placement();
                objP.BranchCode = Session["BranchCode"].ToString();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    objP.AssignExamId = Convert.ToInt32(ds.Tables[0].Rows[i]["AssignTestId"].ToString());
                    //objP.CourseCode = ds.Tables[0].Rows[i]["CourseCode"].ToString();
                    objP.StudentCode = ds.Tables[0].Rows[i]["CandidateCode"].ToString();
                    objP.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                    objP.ExamName = ds.Tables[0].Rows[i]["TestName"].ToString();
                    objP.StaffCode = ds.Tables[0].Rows[i]["TrainerCodeSupervisorCode"].ToString();
                    objP.LabName = ds.Tables[0].Rows[i]["LabCode"].ToString();
                    objP.StartTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["TestTime"].ToString());
                    objP.Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["TestDate"].ToString());
                    objP.Duration = ds.Tables[0].Rows[i]["DurationInHrs"].ToString();
                    objP.TotalMarks = Convert.ToInt32(ds.Tables[0].Rows[i]["TotalMarks"].ToString());
                    objP.StatusId = Convert.ToInt32(ds.Tables[0].Rows[i]["StatusId"].ToString());
                }
                await ListExternalStudentsSNAsync(objP.CourseCode);
                await ListExamsSNAsync(objP.CourseCode);
                await ListLabsSNAsync(objP.BranchCode);
                return await Task.Run(() => PartialView(objP));
            }
        }
        /// <summary>
        /// It is used to save changes.
        /// </summary>
        /// <param name="objP">Object is passed to get updated changes.</param>
        /// <returns>It updates the changes of that assigned exam.</returns>
        [HttpPost]
        public async Task<ActionResult> UpdateAssignExamExternalAsyncSN(Placement objP)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {

                //BALPlacement objAssignExam = new BALPlacement();
                await objbal.UpdateAssignExamSN(objP);
                return RedirectToAction("ListAllAssignedExamSNAsync");
            }
        }
        /// <summary>
        /// This action is used to get details of conducted id.
        /// </summary>
        /// <param name="id">This parameter is used to get that id related data.</param>
        /// <returns>It returns the partial view of add result to add conducted exams result. </returns>
        [HttpGet]
        public async Task<ActionResult> DetailConductedExamExtStuAsyncSN(int id)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {

                DataSet ds = await objbal.DetailConductedExamExtSN(id);
                Placement objP = new Placement();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    objP.AssignExamId = Convert.ToInt32(ds.Tables[0].Rows[i]["AssignTestId"].ToString());
                    objP.StudentCode = ds.Tables[0].Rows[i]["CandidateCode"].ToString();
                    objP.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                    objP.CourseCode = ds.Tables[0].Rows[i]["CourseCode"].ToString();
                    objP.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objP.ExamName = ds.Tables[0].Rows[i]["TestName"].ToString();
                    objP.ExamDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["TestDate"].ToString());
                    objP.TotalMarks = float.Parse(ds.Tables[0].Rows[i]["TotalMarks"].ToString());
                    objP.PassingMarks = float.Parse(ds.Tables[0].Rows[i]["PassingMarks"].ToString());
                    objP.StaffCode = "STF009";
                }
                return PartialView("_AddResultExternalStudentAsyncSN", objP);
            }
        }
        /// <summary>
        /// This action is used to add result of external student.
        /// </summary>
        /// <param name="ObjP">Object is passed to get data related result.</param>
        /// <returns>It adds result of that particular student.</returns>
        [HttpPost]
        public async Task<ActionResult> AddResultExternalStudentAsyncSN(Placement ObjP)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {

                await objbal.AddResultSN(ObjP);
                return RedirectToAction("ListAllAssignedExamSNAsync");
            }
        }
        /// <summary>
        /// This action is used to view result of student.
        /// </summary>
        /// <param name="id">This parameter is used to get result details of its related id.</param>
        /// <returns>It returns and shows the result of that student which is passed through id.</returns>
        [HttpGet]
        public async Task<ActionResult> ViewResultExternalStudentAsyncSN(int id)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {

                DataSet ds = await objbal.ViewResultExternalStudentSN(id);
                Placement objP = new Placement();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    objP.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                    objP.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objP.ExamName = ds.Tables[0].Rows[i]["TestName"].ToString();
                    objP.ExamDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["TestDate"].ToString());
                    objP.TotalMarks = float.Parse(ds.Tables[0].Rows[i]["TotalMarks"].ToString());
                    objP.ObtainedMarks = float.Parse(ds.Tables[0].Rows[i]["ObtainedMarks"].ToString());
                    objP.Status = ds.Tables[0].Rows[i]["Status"].ToString();
                    objP.ResultStatus = ds.Tables[0].Rows[i]["ResultStatus"].ToString();
                }
                return PartialView("_ViewResultExternalStudentAsyncSN", objP);
            }
        }
        /// <summary>
        /// This action is used to open file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async Task<ActionResult> ViewFileAsyncSN(string filePath)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {

                return await Task.Run(() => View((object)filePath));
            }
        }
        /// <summary>
        /// This action is used to get details of conducted exams.
        /// </summary>
        /// <param name="id">id is used to get that particular exams details.</param>
        /// <returns>It returns to view to display conducted exam details.</returns>
        public async Task<ActionResult> DetailConductedExamIntStuAsyncSN(int id)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {

                DataSet ds = await objbal.DetailConductedExamIntSN(id);
                Placement objP = new Placement();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    objP.AssignExamId = Convert.ToInt32(ds.Tables[0].Rows[i]["AssignTestId"].ToString());
                    objP.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    objP.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objP.ExamName = ds.Tables[0].Rows[i]["TestName"].ToString();
                    objP.ExamDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["TestDate"].ToString());
                    objP.ExamTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["TestTime"].ToString());
                    objP.TotalMarks = float.Parse(ds.Tables[0].Rows[i]["TotalMarks"].ToString());
                    objP.PassingMarks = float.Parse(ds.Tables[0].Rows[i]["PassingMarks"].ToString());
                }
                //  BALPlacement GetStudents = new BALPlacement();
                DataSet ds1 = await objbal.ListInternalStudentCondExamSN(id);
                List<SelectListItem> StudentList = new List<SelectListItem>();
                foreach (DataRow dr in ds1.Tables[0].Rows)
                {
                    StudentList.Add(new SelectListItem { Text = dr["FullName"].ToString(), Value = dr["CandidateCode"].ToString() });
                }
                ViewBag.Student = StudentList;
                return PartialView(objP);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resultList"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> SaveResultInternalStudentAsyncSN(List<Placement> studentDataArray)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {

                try
                {
                    // BALPlacement objP = new BALPlacement();
                    foreach (var studentData in studentDataArray)
                    {
                        // Call your asynchronous method to add results to the database
                        await objbal.AddResultSN(new Placement
                        {
                            AssignExamId = studentData.AssignExamId,
                            StudentCode = studentData.StudentCode,
                            ObtainedMarks = studentData.ObtainedMarks,
                            AttendanceStatusId = studentData.AttendanceStatusId,
                            ResultStatus = studentData.ResultStatus,
                            StaffCode = Session["StaffCode"].ToString()
                        });
                    }
                    // Return a success response if needed
                    return Json(new { success = true, message = "Results saved successfully." });
                }
                catch (Exception ex)
                {
                    // Log the exception details
                    Console.WriteLine(ex.ToString());

                    // Return an error response with a more detailed message
                    return Json(new { success = false, message = "An error occurred while saving results. Details: " + ex.Message });
                }
            }
        }
        public async Task<ActionResult> ViewResultInternalStudentsAsyncSN(int id)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {

                DataSet ds = await objbal.ViewResultInternalStudentsSN(id);
                List<Placement> lstResult = new List<Placement>();
                Placement objP = new Placement();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Placement objResult = new Placement();
                        objP.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                        objP.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                        objP.ExamName = ds.Tables[0].Rows[i]["TestName"].ToString();
                        objP.ExamDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["TestDate"].ToString());
                        objP.ExamTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["TestTime"].ToString());
                        objP.TotalMarks = Convert.ToInt32(ds.Tables[0].Rows[i]["TotalMarks"].ToString());
                        objResult.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                        objResult.ObtainedMarks = Convert.ToInt32(ds.Tables[0].Rows[i]["ObtainedMarks"].ToString());
                        objResult.Attendance = ds.Tables[0].Rows[i]["Status"].ToString();
                        objResult.ResultStatus = ds.Tables[0].Rows[i]["ResultStatus"].ToString();
                        lstResult.Add(objResult);
                    }
                }
                objP.lstConductedExams = lstResult;
                return await Task.Run(() => PartialView(objP));
            }
        }
        [HttpGet]
        public async Task<ActionResult> UpdateAssignedExamIntStuAsyncSN(int id)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {

                await ListStatusAsyncSN();
                DataSet ds = await objbal.DetailConductedExamIntSN(id);
                Placement objP = new Placement();
                objP.BranchCode = Session["BranchCode"].ToString();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    objP.AssignExamId = Convert.ToInt32(ds.Tables[0].Rows[i]["AssignTestId"].ToString());
                    objP.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    objP.BatchCode = ds.Tables[0].Rows[i]["BatchCode"].ToString();
                    objP.CourseCode = ds.Tables[0].Rows[i]["Coursecode"].ToString();
                    objP.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objP.ExamId = Convert.ToInt32(ds.Tables[0].Rows[i]["TestId"].ToString());
                    objP.ExamName = ds.Tables[0].Rows[i]["TestName"].ToString();
                    objP.Duration = ds.Tables[0].Rows[i]["Duration"].ToString();
                    objP.Description = ds.Tables[0].Rows[i]["Duration"].ToString();
                    objP.Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["TestDate"].ToString());
                    objP.StartTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["TestTime"].ToString());
                    objP.StaffCode = ds.Tables[0].Rows[i]["TrainerCodeSupervisorCode"].ToString();
                    objP.LabName = ds.Tables[0].Rows[i]["LabCode"].ToString();
                    objP.StatusId = Convert.ToInt32(ds.Tables[0].Rows[i]["StatusId"].ToString());

                }
                //  BALPlacement GetStudents = new BALPlacement();
                DataSet ds1 = await objbal.ListInternalStudentCondExamSN(id);
                List<Placement> StudentList = new List<Placement>();
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    Placement objstu = new Placement();
                    objstu.StudentName = ds1.Tables[0].Rows[i]["FullName"].ToString();
                    StudentList.Add(objstu);
                }
                objP.lstExams = StudentList;
                await ListTrainerSNAsync();
                //await ListExamsSNAsync(objP.CourseCode);
                await ListLabsSNAsync(objP.BranchCode);
                return PartialView(objP);
            }
        }
        [HttpPost]
        public async Task<ActionResult> UpdateAssignedExamIntStuAsyncSN(Placement objP)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {

                // BALPlacement objAssignExam = new BALPlacement();
                await objbal.UpdateAssignExamSN(objP);
                return RedirectToAction("ListAllAssignedExamSNAsync");
            }
        }
        [HttpPost]
        public async Task<JsonResult> IsExamAssignedToBatchSN(int ExamId, string BatchCode)
        {
            bool isAssigned = await objbal.IsExamAssignedToBatchSN(ExamId, BatchCode);
            return Json(new { isAssigned });      // Return a JsonResult indicating whether the exam is assigned to that batch.
        }
        [HttpGet]
        public async Task<JsonResult> GetTechnologyNameAsyncSN(int ExamId)
        {
            DataSet ds = await objbal.TechnologyNameSN(ExamId);
            Placement objp = new Placement();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                objp.TechnologyName = ds.Tables[0].Rows[i]["TechnologyNames"].ToString();
            }
            string technology = objp.TechnologyName;
            return Json(technology, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public async Task<JsonResult> GetExamDurationAsyncSN(int ExamId)
        {
            DataSet ds = await objbal.GetExamDurationSN(ExamId);
            Placement objp = new Placement();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                objp.Duration = ds.Tables[0].Rows[i]["Duration"].ToString();
            }
            string Duration = objp.Duration;
            return Json(Duration, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public async Task<JsonResult> ListExamExternalStudentsSN(string StudentCode)
        {
            Placement objP = new Placement();

            DataSet ds = await objbal.ListExamExternalStudentsSN(StudentCode);

            List<object> ExamList = new List<object>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                var exam = new
                {
                    Text = dr["TestName"].ToString(),
                    Value = dr["TestId"].ToString(),
                    ExamSkill = dr["SkillNames"].ToString(),
                    Duration = dr["Duration"].ToString()
                };

                ExamList.Add(exam);
            }
            return Json(ExamList, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public async Task<JsonResult> IsExamAvailableAsyncSN(string examName, string courseCode)
        {
            bool isAvailable = await objbal.IsExamAvailableSN(examName, courseCode);
            return Json(new { isAvailable });      // Return a JsonResult indicating whether the exam is available
        }
        [HttpPost]
        public async Task<JsonResult> ReadAvailableLabBatch_Bind(string Batchcode, DateTime startDate, DateTime StartTime, DateTime EndTime)
        {

            Placement obj = new Placement();
            obj.BranchCode = Session["BranchCode"].ToString();
            obj.BatchCode = Batchcode;
            obj.StartDate = startDate;
            obj.StartTime = StartTime;
            //obj.EndTime = EndTime;
            obj.EndTime = EndTime.AddMinutes(-15);
            DataTable dt = new DataTable();
            DataSet ds1 = new DataSet();
            if (Batchcode != null)
            {
                ds1 = await objbal.ReadAvailableLabs(obj);
            }
            else
            {
                ds1 = await objbal.ReadAvailableLabForExternalStu(obj);
            }
            dt = ds1.Tables[0];
            var Jsondata = JsonConvert.SerializeObject(dt);
            return Json(Jsondata);

        }
        /// <summary>
        /// This action is used to get main view of interview to load list of various list related interview.
        /// </summary>
        /// <returns>It returns main view and load shotlisted students partial view.</returns>
        public async Task<ActionResult> ListAllInterviewAsyncSN()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                    new BreadcrumbItem { Name = "Dashboard", Url =Url.Action("PlcementDashboardPCAsync", "Placement") },
                    new BreadcrumbItem { Name = "Interview", Url = Url.Action("ListAllInterviewAsyncSN", "Placement")}
                };
                await ListAllOpeningAsync();
                ViewBag.Breadcrumbs = breadcrumbs;
                return await Task.Run(() => View());
            }
        }
        /// <summary>
        /// It used to get partial view to load shortlisted students for interview.
        /// </summary>
        /// <returns>It returns partial view for shortlisted students.</returns>
        public async Task<ActionResult> ListShortlistedStudentsAsyncSN()
        {

            Placement objPla = new Placement();
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                objPla.BranchCode = Session["BranchCode"].ToString();
                // BALPlacement getShortlistedStudents = new BALPlacement();
                DataSet ds = await objbal.ListShortlistedStudentsSN(objPla);
                List<Placement> lstStudents = new List<Placement>();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Placement objP = new Placement();
                        objP.PlacementId = Convert.ToInt32(ds.Tables[0].Rows[i]["PlacementId"].ToString());
                        objP.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                        objP.ContactNo = ds.Tables[0].Rows[i]["ContactNumber"].ToString();
                        objP.Email = ds.Tables[0].Rows[i]["EmailId"].ToString();
                        objP.HighestQualification = ds.Tables[0].Rows[i]["HighestQualification"].ToString();
                        objP.CompanyName = ds.Tables[0].Rows[i]["CompanyName"].ToString();
                        objP.DesignationName = ds.Tables[0].Rows[i]["DesignationName"].ToString();
                        objP.Experience = ds.Tables[0].Rows[i]["Experience"].ToString();
                        lstStudents.Add(objP);
                    }
                }
                objPla.lstShortlistedCandidates = lstStudents;
                return await Task.Run(() => PartialView("_ListShortlistedStudents", objPla));
            }
        }
        /// <summary>
        /// It used to get partial view to load Scheduled interview list.
        /// </summary>
        /// <returns>It returns partial view for Scheduled interview.</returns>
        public async Task<ActionResult> ListScheduledInterviewAsyncSN()
        {
            Placement objPla = new Placement();
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                objPla.BranchCode = Session["BranchCode"].ToString();
                //BALPlacement ScheduledInterview = new BALPlacement();
                DataSet ds1 = await objbal.ListScheduledInterviewSN(objPla);
                List<Placement> lstScheduledInterviewStudents = new List<Placement>();
                if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        Placement ObjP = new Placement();
                        ObjP.PlacementId = Convert.ToInt32(ds1.Tables[0].Rows[i]["PlacementId"].ToString());
                        ObjP.StudentName = ds1.Tables[0].Rows[i]["FullName"].ToString();
                        ObjP.ContactNo = ds1.Tables[0].Rows[i]["ContactNumber"].ToString();
                        ObjP.Email = ds1.Tables[0].Rows[i]["EmailId"].ToString();
                        ObjP.CompanyName = ds1.Tables[0].Rows[i]["CompanyName"].ToString();
                        ObjP.DesignationName = ds1.Tables[0].Rows[i]["DesignationName"].ToString();
                        ObjP.Experience = ds1.Tables[0].Rows[i]["Experience"].ToString();
                        ObjP.Location = ds1.Tables[0].Rows[i]["Location"].ToString();
                        ObjP.InterviewDate = Convert.ToDateTime(ds1.Tables[0].Rows[i]["InterviewDate"].ToString());
                        ObjP.Time = Convert.ToDateTime(ds1.Tables[0].Rows[i]["Time"].ToString());
                        ObjP.InterviewMode = ds1.Tables[0].Rows[i]["InterviewMode"].ToString();
                        ObjP.InterviewRound = ds1.Tables[0].Rows[i]["InterviewRound"].ToString();
                        lstScheduledInterviewStudents.Add(ObjP);
                    }
                }
                objPla.lstScheduledInterview = lstScheduledInterviewStudents;
                return await Task.Run(() => PartialView("_ListScheduledInterviewAsyncSN", objPla));
            }
        }
        /// <summary>
        /// This action is used to schedule interview of candidate.
        /// </summary>
        /// <param name="id">This parameter is used to pass placement id to get opnening details of related interview.</param>
        /// <param name="round">This parameter used to pass round of that interview.</param>
        /// <returns>It returns the view for schedule interview with information related that interview like company name,designation.</returns>
        [HttpGet]
        public async Task<ActionResult> ScheduleInterviewAsyncSN(int id, string round)
        {
            Placement objPla = new Placement();
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {

                objPla.BranchCode = Session["BranchCode"].ToString();
                // BALPlacement ScheduledInterview = new BALPlacement();
                DataSet ds1 = await objbal.DetailScheduleInterviewSN(id);
                if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        objPla.PlacementId = Convert.ToInt32(ds1.Tables[0].Rows[i]["PlacementId"].ToString());
                        objPla.OpeningCode = ds1.Tables[0].Rows[i]["OpeningCode"].ToString();
                        objPla.StudentCode = ds1.Tables[0].Rows[i]["CandidateCode"].ToString();
                        objPla.StudentName = ds1.Tables[0].Rows[i]["FullName"].ToString();
                        objPla.CompanyName = ds1.Tables[0].Rows[i]["CompanyName"].ToString();
                        objPla.DesignationName = ds1.Tables[0].Rows[i]["DesignationName"].ToString();
                        objPla.InterviewDate = DateTime.Today;
                    }
                }
                objPla.InterviewRound = round;
                return await Task.Run(() => PartialView("_ScheduleInterviewAsyncSN", objPla));
            }
        }
        /// <summary>
        /// This action is used to schedule interview of student,save interview details.
        /// </summary>
        /// <param name="objP">Object is passed to save details of interview.</param>
        /// <returns>It schedule interview with necessory details.</returns>
        [HttpPost]
        public async Task<ActionResult> ScheduleInterviewAsyncSN(Placement objP)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                if (objP.InterviewRound == "First")
                {
                    // BALPlacement objBAL = new BALPlacement();
                    await objbal.ScheduleInterviewRound1SN(objP);
                    return RedirectToAction("ListAllInterviewAsyncSN");
                }
                else
                {
                    // BALPlacement objBAL = new BALPlacement();
                    await objbal.ScheduleInterviewSN(objP);
                    return RedirectToAction("ListAllInterviewAsyncSN");
                }
            }
        }
        //          public async Task<JsonResult> ListLocationAsyncSN()
        //{
        //          BALPlacement GetLocations = new BALPlacement();
        //          DataSet ds = await GetLocations.ListLocationSN();
        //          List<SelectListItem> LocationList = new List<SelectListItem>();
        //          foreach (DataRow dr in ds.Tables[0].Rows)
        //          {
        //              LocationList.Add(new SelectListItem { Text = dr["CityName"].ToString(), Value = dr["CityId"].ToString() });
        //          }
        //          return Json(LocationList, JsonRequestBehavior.AllowGet);
        //      }

        /// <summary>
        /// This action is used to get interview perfomance of candidates.
        /// </summary>
        /// <returns>It returns the partial view for interview performance.</returns>
        public async Task<ActionResult> ListInterviewPerformanceAsyncSN()
        {
            Placement objPla = new Placement();
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                objPla.BranchCode = Session["BranchCode"].ToString();
                // BALPlacement objBAL = new BALPlacement();
                DataSet ds = await objbal.ListInterviewPerformanceSN(objPla.BranchCode);
                List<Placement> lstInterviewPerformance = new List<Placement>();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Placement ObjP = new Placement();
                        ObjP.StudentCode = ds.Tables[0].Rows[i]["CandidateCode"].ToString();
                        ObjP.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                        ObjP.HR1Name = ds.Tables[0].Rows[i]["HR1Name"].ToString();
                        ObjP.CompanyName = ds.Tables[0].Rows[i]["CompanyName"].ToString();
                        ObjP.DesignationName = ds.Tables[0].Rows[i]["DesignationName"].ToString();
                        ObjP.Experience = ds.Tables[0].Rows[i]["Experience"].ToString();
                        ObjP.Location = ds.Tables[0].Rows[i]["CityName"].ToString();
                        ObjP.Status = ds.Tables[0].Rows[i]["Status"].ToString();
                        lstInterviewPerformance.Add(ObjP);
                    }
                }
                objPla.lstInterviewPerformance = lstInterviewPerformance;
                return await Task.Run(() => PartialView("_ListInterviewPerformanceAsyncSN", objPla));
            }
        }
        /// <summary>
        /// This action is used to get all interview list of single candidate.
        /// </summary>
        /// <param name="id"> id is used to get that candidates all interview rounds. </param>
        /// <returns>It returns all interview round list with status and related details of that rounds. </returns>
        public async Task<ActionResult> ListAllInterviewRoundAsyncSN(string studentCode)
        {
            Placement objPla = new Placement();
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                objPla.BranchCode = Session["BranchCode"].ToString();
                //  BALPlacement objBAL = new BALPlacement();
                DataSet ds = await objbal.DetailsAllRoundSingleCandidateSN(studentCode, objPla.BranchCode);
                List<Placement> lstAllRoundDetails = new List<Placement>();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Placement ObjP = new Placement();
                        ObjP.PlacementId = Convert.ToInt32(ds.Tables[0].Rows[i]["LastRound_PlacementId"].ToString());
                        ObjP.OpeningCode = ds.Tables[0].Rows[i]["OpeningCode"].ToString();
                        objPla.StudentCode = ds.Tables[0].Rows[i]["CandidateCode"].ToString();
                        objPla.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                        objPla.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                        ObjP.HR1Name = ds.Tables[0].Rows[i]["HR1Name"].ToString();
                        ObjP.CompanyName = ds.Tables[0].Rows[i]["CompanyName"].ToString();
                        ObjP.CTC = ds.Tables[0].Rows[i]["CTC"].ToString();
                        ObjP.DesignationName = ds.Tables[0].Rows[i]["DesignationName"].ToString();
                        ObjP.Location = ds.Tables[0].Rows[i]["CityName"].ToString();
                        ObjP.Round1 = ds.Tables[0].Rows[i]["Round1"].ToString();
                        ObjP.Round2 = ds.Tables[0].Rows[i]["Round2"].ToString();
                        ObjP.Round3 = ds.Tables[0].Rows[i]["Round3"].ToString();
                        ObjP.InterviewRound = ds.Tables[0].Rows[i]["LastRound_Status"].ToString();
                        ObjP.FilePth = ds.Tables[0].Rows[i]["OfferLetter"].ToString();
                        lstAllRoundDetails.Add(ObjP);
                    }
                }
                objPla.lstAllRoundDetails = lstAllRoundDetails;
                return await Task.Run(() => PartialView("_ListAllInterviewRoundAsyncSN", objPla));
            }
        }
        /// <summary>
        /// This action is used to get placed students list .
        /// </summary>
        /// <returns>It returns partial view for placed students.</returns>
        public async Task<ActionResult> ListPlacedStudentsAsyncSN()
        {
            Placement objPla = new Placement();
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                objPla.BranchCode = Session["BranchCode"].ToString();
                //BALPlacement objBAL = new BALPlacement();
                DataSet ds = await objbal.ListPlacedStudentsSN(objPla.BranchCode);
                List<Placement> lstInterviewPerformance = new List<Placement>();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Placement ObjP = new Placement();
                        ObjP.StudentCode = ds.Tables[0].Rows[i]["CandidateCode"].ToString();
                        ObjP.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                        ObjP.CompanyName = ds.Tables[0].Rows[i]["CompanyName"].ToString();
                        ObjP.DesignationName = ds.Tables[0].Rows[i]["DesignationName"].ToString();
                        ObjP.Experience = ds.Tables[0].Rows[i]["Experience"].ToString();
                        ObjP.Location = ds.Tables[0].Rows[i]["CityName"].ToString();
                        ObjP.CTC = ds.Tables[0].Rows[i]["CTC"].ToString();
                        ObjP.Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["OfferAddDate"].ToString());
                        lstInterviewPerformance.Add(ObjP);
                    }
                }
                objPla.lstInterviewPerformance = lstInterviewPerformance;
                return await Task.Run(() => PartialView("_ListPlacedStudentsAsyncSN", objPla));
            }
        }
        /// <summary>
        /// This action is used to update scheduled interview of candidates.
        /// </summary>
        /// <param name="id">id is used to get data of interview from particular placementid.</param>
        /// <returns>It display the view to change interview details with necessory details.</returns>
        public async Task<ActionResult> UpdateScheduledInterviewAsyncSN(int id)
        {
            Placement ObjP = new Placement();
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                // BALPlacement objBAL = new BALPlacement();
                DataSet ds = await objbal.DetailsScheduledInterviewSN(id);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ObjP.PlacementId = Convert.ToInt32(ds.Tables[0].Rows[i]["PlacementId"].ToString());
                        ObjP.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                        ObjP.CompanyName = ds.Tables[0].Rows[i]["CompanyName"].ToString();
                        ObjP.DesignationName = ds.Tables[0].Rows[i]["DesignationName"].ToString();
                        ObjP.InterviewDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["InterviewDate"].ToString());
                        ObjP.Time = Convert.ToDateTime(ds.Tables[0].Rows[i]["Time"].ToString());
                        ObjP.InterviewRound = ds.Tables[0].Rows[i]["InterviewRound"].ToString();
                        ObjP.InterviewMode = ds.Tables[0].Rows[i]["InterviewMode"].ToString();
                    }
                }
                return await Task.Run(() => PartialView("_UpdateScheduledInterviewAsyncSN", ObjP));
            }
        }

        public async Task<ActionResult> RescheduleInterviewAsyncSN(string studentCode, string openingCode, string round)
        {
            Placement ObjP = new Placement();
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                // BALPlacement objBAL = new BALPlacement();
                ObjP.StudentCode = studentCode;
                ObjP.OpeningCode = openingCode;
                ObjP.InterviewRound = round;
                DataSet ds = await objbal.DetailSingleInterviewRoundSN(ObjP);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ObjP.PlacementId = Convert.ToInt32(ds.Tables[0].Rows[i]["PlacementId"].ToString());
                        ObjP.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                        ObjP.CompanyName = ds.Tables[0].Rows[i]["CompanyName"].ToString();
                        ObjP.DesignationName = ds.Tables[0].Rows[i]["DesignationName"].ToString();
                        ObjP.InterviewDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["InterviewDate"].ToString());
                        ObjP.Time = Convert.ToDateTime(ds.Tables[0].Rows[i]["Time"].ToString());
                        ObjP.InterviewRound = ds.Tables[0].Rows[i]["InterviewRound"].ToString();
                        ObjP.InterviewMode = ds.Tables[0].Rows[i]["InterviewMode"].ToString();
                    }
                }
                return await Task.Run(() => PartialView("_UpdateScheduledInterviewAsyncSN", ObjP));
            }
        }
        /// <summary>
        /// This action is used to scheduled interview details.
        /// </summary>
        /// <param name="objP">Object is passed to update related information of that interview.</param>
        /// <returns>It updates scheduled interview details.</returns>
        [HttpPost]
        public async Task<ActionResult> UpdateScheduledInterviewAsyncSN(Placement objP)
        {
            //BALPlacement objBAL = new BALPlacement();
            await objbal.ScheduleInterviewRound1SN(objP);
            return RedirectToAction("ListAllInterviewAsyncSN");
        }
        /// <summary>
        /// This action is used to add interview performance of candidate.
        /// </summary>
        /// <param name="id">id is used get interview performance of candidate from that placementid.</param>
        /// <returns>It returns the view for add interview performance.</returns>
        [HttpGet]
        public async Task<ActionResult> AddInterviewPerformanceAsyncSN(int id)
        {
            Placement ObjP = new Placement();
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await ListInterviewStatusAsyncSN();
                //BALPlacement objBAL = new BALPlacement();
                DataSet ds = await objbal.DetailsScheduledInterviewSN(id);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ObjP.PlacementId = Convert.ToInt32(ds.Tables[0].Rows[i]["PlacementId"].ToString());
                        ObjP.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                        ObjP.CompanyName = ds.Tables[0].Rows[i]["CompanyName"].ToString();
                        ObjP.DesignationName = ds.Tables[0].Rows[i]["DesignationName"].ToString();
                        ObjP.InterviewDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["InterviewDate"].ToString());
                        ObjP.HR1Name = ds.Tables[0].Rows[i]["HR1Name"].ToString();
                        ObjP.Location = ds.Tables[0].Rows[i]["CityName"].ToString();
                        ObjP.Time = Convert.ToDateTime(ds.Tables[0].Rows[i]["Time"].ToString());
                        ObjP.InterviewRound = ds.Tables[0].Rows[i]["InterviewRound"].ToString();
                    }
                }
                return await Task.Run(() => PartialView("_AddInterviewPerformanceAsyncSN", ObjP));
            }
        }
        /// <summary>
        /// This action is used to get status related interview.
        /// </summary>
        /// <returns>It returns interview status list.</returns>
        public async Task ListInterviewStatusAsyncSN()
        {
            // BALPlacement Getstatus = new BALPlacement();
            DataSet ds = await objbal.ListInterviewStatusSN();
            List<SelectListItem> StatusList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                StatusList.Add(new SelectListItem { Text = dr["Status"].ToString(), Value = dr["StatusId"].ToString() });
            }
            ViewBag.InterviewStatus = StatusList;
        }
        /// <summary>
        /// This action is used to add interview performance of candidates from placementid.
        /// </summary>
        /// <param name="ObjP">Object is used to pass details of interview and there performance.</param>
        /// <returns>It add interview performance of candidates.</returns>
        [HttpPost]
        public async Task<ActionResult> AddInterviewPerformanceAsyncSN(Placement ObjP)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                //BALPlacement objBAL = new BALPlacement();
                await objbal.AddInterviewPerformanceSN(ObjP);
                return RedirectToAction("ListAllInterviewAsyncSN");
            }
        }
        /// <summary>
        /// This action is used to accept or reject offer of candidates.
        /// </summary>
        /// <param name="id">id is used to get related data from placement id.</param>
        /// <returns>It returns view to accept or reject offer.</returns>
        [HttpGet]
        public async Task<ActionResult> AcceptORRejectOfferAsyncSN(int id)
        {
            Placement ObjP = new Placement();
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {

                //  BALPlacement objBAL = new BALPlacement();
                DataSet ds = await objbal.DetailsScheduledInterviewSN(id);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ObjP.PlacementId = Convert.ToInt32(ds.Tables[0].Rows[i]["PlacementId"].ToString());
                        ObjP.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                        ObjP.CompanyName = ds.Tables[0].Rows[i]["CompanyName"].ToString();
                        ObjP.DesignationName = ds.Tables[0].Rows[i]["DesignationName"].ToString();
                    }
                }
                return await Task.Run(() => PartialView("_AcceptORRejectOfferAsyncSN", ObjP));
            }
        }
        /// <summary>
        /// This actton is used to save after accepted or rejected offer letter by user.
        /// </summary>
        /// <param name="ObjP">Object is used to pass data.</param>
        /// <returns>It saves the data after any one action from the user.</returns>
        [HttpPost]
        public async Task<ActionResult> AcceptORRejectOfferAsyncSN(Placement ObjP)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                // BALPlacement objBAL = new BALPlacement();

                if (ObjP.OfferLetter != null && ObjP.OfferLetter.ContentLength > 0)
                {
                    var fileName = Path.GetFileNameWithoutExtension(ObjP.OfferLetter.FileName);
                    var extension = Path.GetExtension(ObjP.OfferLetter.FileName);
                    fileName = fileName + extension;
                    var filePath = "~/Content/Placement/docs/OfferLetter/" + fileName; // Relative path
                    ObjP.FilePth = filePath;
                    fileName = Path.Combine(Server.MapPath("~/Content/Placement/docs/OfferLetter/"), fileName);
                    ObjP.OfferLetter.SaveAs(fileName);
                    await objbal.AcceptORRejectOfferSN(ObjP);
                    return RedirectToAction("ListAllInterviewAsyncSN");
                }
                else
                {
                    return RedirectToAction("Error", "Home"); // Handle the case where no offer letter is present
                }
            }
        }
        [HttpGet]
        public async Task<ActionResult> ViewOfferAndAcceptOfferAsyncSN(int placementId)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Placement ObjP = new Placement();

                DataSet ds = await objbal.DetailsOfferSN(placementId);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ObjP.PlacementId = Convert.ToInt32(ds.Tables[0].Rows[i]["PlacementId"].ToString());
                        ObjP.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                        ObjP.CompanyName = ds.Tables[0].Rows[i]["CompanyName"].ToString();
                        ObjP.DesignationName = ds.Tables[0].Rows[i]["DesignationName"].ToString();
                        ObjP.CTC = ds.Tables[0].Rows[i]["NumericCTC"].ToString();
                        ObjP.CTC_LPA = (decimal)float.Parse(ds.Tables[0].Rows[i]["NumericCTC"].ToString());
                        ObjP.FilePth = ds.Tables[0].Rows[i]["OfferLetter"].ToString();
                        ObjP.Description = ds.Tables[0].Rows[i]["Description"].ToString();
                        ObjP.JoiningDate = DateTime.Today;
                    }
                }
                return PartialView("_ViewOfferAndAcceptOfferAsyncSN", ObjP);
            }
        }

        /// <summary>
        /// This action is used to details of candidate for which company that candidate has been joined.
        /// </summary>
        /// <param name="id">This id is used to pass candidate placement for which that candidate has neen joined.</param>
        /// <param name="feedbackStatus">Feedback status is used to pass status for join.</param>
        /// <returns>It update the status of that particular candidate from placement id.</returns>
        [HttpPost]
        public async Task<ActionResult> ViewOfferAndAcceptOfferAsyncSN(Placement objP)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                if (objP.OfferLetter != null && objP.OfferLetter.ContentLength > 0)
                {
                    var fileName = Path.GetFileNameWithoutExtension(objP.OfferLetter.FileName);
                    var extension = Path.GetExtension(objP.OfferLetter.FileName);
                    fileName = fileName + extension;
                    var filePath = "~/Content/Placement/docs/OfferLetter/" + fileName; // Relative path
                    objP.FilePth = filePath;
                    fileName = Path.Combine(Server.MapPath("~/Content/Placement/docs/OfferLetter/"), fileName);
                    objP.OfferLetter.SaveAs(fileName);
                    objP.StatusId = 12;
                    await objbal.JoinCompanySN(objP);
                    return RedirectToAction("ListAllInterviewAsyncSN");

                    //Placement ObjP = new Placement();
                    //await objbal.JoinCompanySN(ObjP);
                    //return RedirectToAction("ListAllInterviewAsyncSN");
                }
                else if (objP.FilePth != null)
                {
                    var filePath = "~/Content/Placement/docs/OfferLetter/" + objP.FilePth; // Relative path
                    objP.FilePth = filePath;
                    objP.StatusId = 12;
                    await objbal.JoinCompanySN(objP);
                    return RedirectToAction("ListAllInterviewAsyncSN");
                }
                else
                {
                    return RedirectToAction("Error", "Home");
                }

            }
        }
        /// <summary>
        /// This action is used to get details of interview round of candidate.
        /// </summary>
        /// <param name="studentCode">Student code is used to get candidate details.</param>
        /// <param name="openingCode">openingCode is used to get details of company,designation for which candidate has applied.</param>
        /// <param name="round">round is used to get for which interview rounds information user wants.</param>
        /// <returns>From studentcode,openingcode and user get information about whick company candidate are going to face interview and for which round.</returns>
        [HttpGet]
        public async Task<ActionResult> DetailSingleInterviewRoundAsyncSN(string studentCode, string openingCode, string round)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Placement ObjP = new Placement();
                ObjP.StudentCode = studentCode;
                ObjP.OpeningCode = openingCode;
                ObjP.InterviewRound = round;
                //BALPlacement objBAL = new BALPlacement();
                DataSet ds = await objbal.DetailSingleInterviewRoundSN(ObjP);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ObjP.PlacementId = Convert.ToInt32(ds.Tables[0].Rows[i]["PlacementId"].ToString());
                        ObjP.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                        ObjP.CompanyName = ds.Tables[0].Rows[i]["CompanyName"].ToString();
                        ObjP.DesignationName = ds.Tables[0].Rows[i]["DesignationName"].ToString();
                        ObjP.InterviewDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["InterviewDate"].ToString());
                        ObjP.HR1Name = ds.Tables[0].Rows[i]["HR1Name"].ToString();
                        ObjP.Location = ds.Tables[0].Rows[i]["CityName"].ToString();
                        ObjP.Time = Convert.ToDateTime(ds.Tables[0].Rows[i]["Time"].ToString());
                        ObjP.InterviewRound = ds.Tables[0].Rows[i]["InterviewRound"].ToString();
                        ObjP.StatusId = Convert.ToInt32(ds.Tables[0].Rows[i]["FeedbackStatus"].ToString());
                        ObjP.Description = ds.Tables[0].Rows[i]["Description"].ToString();
                    }
                }
                await ListInterviewStatusAsyncSN();
                return PartialView("_AddInterviewPerformanceAsyncSN", ObjP);
            }
        }
        /// <summary>
        /// This action is used to details of candiate who is already placed in a company.
        /// </summary>
        /// <param name="studentCode">studentcode is passed to get that particulat candidate information.</param>
        /// <returns>It returns the information of student where placed.</returns>
        [HttpGet]
        public async Task<ActionResult> DetailsPlacedStudentsAsyncSN(string studentCode)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Placement ObjP = new Placement();
                ObjP.StudentCode = studentCode;
                ObjP.BranchCode = Session["BranchCode"].ToString();
                // BALPlacement objBAL = new BALPlacement();
                DataSet ds = await objbal.DetailsPlacedStudentSN(ObjP);
                List<Placement> lstPlacedStudents = new List<Placement>();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Placement objpla = new Placement();
                        ObjP.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                        objpla.CompanyName = ds.Tables[0].Rows[i]["CompanyName"].ToString();
                        objpla.DesignationName = ds.Tables[0].Rows[i]["DesignationName"].ToString();
                        objpla.CTC = ds.Tables[0].Rows[i]["CTC"].ToString();
                        objpla.Status = ds.Tables[0].Rows[i]["Status"].ToString();
                        objpla.FilePth = ds.Tables[0].Rows[i]["OfferLetter"].ToString();
                        lstPlacedStudents.Add(objpla);
                    }
                }
                ObjP.lstAllRoundDetails = lstPlacedStudents;
                return PartialView("_DetailsPlacedStudentsAsyncSN", ObjP);
            }
        }

        //------------------------------------------Punam Company---------------------------------------//
        /// <summary>
        /// THIS FUNCTION IS USE TO COMPANY REGISTERATION.
        /// </summary>
        /// <returns> THIS FUNCTION RETURNS VIEW .</returns>
        [HttpGet]
        public async Task<ActionResult> RegisterCompanyAsyncPB()
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                try
                {
                    objPlacement.BranchCode = Session["BranchCode"].ToString();
                    objPlacement.StaffCode = Session["StaffCode"].ToString();
                    ViewBag.Induastry = await objbal.GetInduastryPB();
                    ViewBag.Country = await objbal.GetCountry();
                    List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                     {
                    new BreadcrumbItem { Name = "Dashboard", Url = "PlcementDashboardPCAsync" },
                   new BreadcrumbItem { Name = "AllCompany", Url = "ListAllCompanyAsyncPB" },
                    new BreadcrumbItem { Name = "RegisterCompany", Url = "RegisterCompanyAsyncPB" }
                     };
                    ViewBag.Breadcrumbs = breadcrumbs;
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
                return await Task.Run(() => View());
            }
        }
        /// <summary>
        /// THIS FUNCTION IS USE TO COMPANY INFORMATION SAVE.
        /// </summary>
        /// <param name="objp"> THIS OBJECT IS USE TO THE THE  VALUES.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> RegisterCompanyPB(Placement objPlacement)
        {
            try
            {
                objPlacement.BranchCode = Session["BranchCode"].ToString();
                await objbal.RegisterCompany(objPlacement);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return await Task.Run(() => Json(new { success = true, message = "Company has been Created successfully!" }));
        }
        /// <summary>
        /// THIS FUNCTION IS USE TO STATE BIND USING COUNTRYID .
        /// </summary>
        /// <param name="CountryId"></param>
        /// THIS OBJECT IS USE FETCH DATA ON COUNTRYID.
        /// <returns> GET STATE. </returns>
        [HttpPost]
        public async Task<JsonResult> GetStatePB(int CountryId)
        {
            try
            {
                DataSet ds = await objbal.GetState(CountryId);
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                var jsondata = JsonConvert.SerializeObject(dt);
                return await Task.Run(() => Json(jsondata));
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// THIS METHOD IS USED FOR TO CITY BIND.
        /// </summary>
        /// <param name="StateId"></param>
        /// THIS OBJECT IS USE TO GET CITY ON STATEID.
        /// <returns> GET CITY.</returns>
        [HttpPost]
        public async Task<JsonResult> GetCity(int StateId)
        {
            try
            {
                DataSet ds = await objbal.GetCity(StateId);
                DataTable dt = ds.Tables[0];
                var jsondata = JsonConvert.SerializeObject(dt);
                return await Task.Run(() => Json(jsondata));
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// THIS METHOD IS USE TO GET COMPANY NAME FOR VALIDATION.
        /// </summary>
        /// <param name="Company"></param>
        /// <returns> FETCH COMPANY NAME.</returns>
        [HttpPost]
        public async Task<JsonResult> GetCompanyNamePB(string Company)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = await objbal.GetCompanyName(Company);
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                var jsondata = JsonConvert.SerializeObject(dt);
                return await Task.Run(() => Json(jsondata));
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// THIS METHOD IS USE TO VALIDATION INDUSTRY NAME FOR NOT ALLOW TO SAME INDUSTRY NAME.
        /// </summary>
        /// <param name="IndustryName"></param>
        /// <returns> SAME NAME NOT SAVE THAT'S WHY GET INDUSTRY NAME.</returns>
        [HttpPost]
        public async Task<JsonResult> ValidationIndustryNamePB(string IndustryName)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = await objbal.validationIndustry(IndustryName);
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                var jsondata = JsonConvert.SerializeObject(dt);
                return await Task.Run(() => Json(jsondata));
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// THIS METHOD IS USE TO GET ALL COMPANY DETAILS.
        /// </summary>
        /// <returns>GET COMANY LIST.</returns>
        [HttpGet]
        public async Task<ActionResult> ListAllCompanyAsyncPB()
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                try
                {
                    objPlacement.BranchCode = Session["BranchCode"].ToString();
                    objPlacement.StaffCode = Session["StaffCode"].ToString();
                    ViewBag.Induastry = await objbal.GetInduastryPB();
                    ViewBag.Company = await objbal.GetCompanyDetailsPB(objPlacement);
                    List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                     {
                    new BreadcrumbItem { Name = "Dashboard", Url = "PlcementDashboardPCAsync" },
                    new BreadcrumbItem { Name = "AllCompany", Url = "ListAllCompanyAsyncPB" },
                     };
                    ViewBag.Breadcrumbs = breadcrumbs;
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
                return await Task.Run(() => View());
            }
        }
        /// <summary>
        /// THIS FITER IS USE TO GET DATE WISE FILTER DATA.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// THIS OBJECT IS USE TO GET START DATE AND END DATE GET.
        /// <returns> THIS METHOS RETUN FILTER LIST.</returns>
        [HttpPost]
        public async Task<JsonResult> FilterDateCompanyPB(DateTime startDate, DateTime endDate)
        {
            try
            {
                objPlacement.BranchCode = Session["BranchCode"].ToString();
                DataSet ds = await objbal.DateFilterPB(startDate, endDate, objPlacement.BranchCode);
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                var jsondata = JsonConvert.SerializeObject(dt);
                return await Task.Run(() => Json(jsondata));
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// THIS METHOD IS USE TO GET UPDATE VIEW.
        /// </summary>
        /// <param name="CompanyId"></param>
        /// THIS OBJECT IS USE TO GET COMPANY INFORMATION.
        /// <returns>UPDATE COMPANY VIEW.</returns>
        [HttpGet]
        public async Task<ActionResult> UpdateCompanyAsyncPB(string CompanyCode)
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                DataSet ds = await objbal.GetCompany(CompanyCode);
                Placement ObjPlacement = new Placement();
                try
                {
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = ds.Tables[0].Rows[0];
                        ObjPlacement.CompanyCode = row["CompanyCode"].ToString();
                        ObjPlacement.CompanyName = row["CompanyName"].ToString();
                        ObjPlacement.CompanyEmail = row["CompanyEmail"].ToString();
                        ObjPlacement.Address = row["Address"].ToString();
                        ObjPlacement.CountryId = Convert.ToInt32(row["CountryId"].ToString());
                        ObjPlacement.StateId = Convert.ToInt32(row["StateId"].ToString());
                        ObjPlacement.City = row["CityName"].ToString();
                        ObjPlacement.HR1Name = row["HR1Name"].ToString();
                        ObjPlacement.HR1Mail = row["HR1Mail"].ToString();
                        ObjPlacement.HR1Contact = row["HR1Contact"].ToString();
                        ObjPlacement.HR2Name = row["HR2Name"].ToString();
                        ObjPlacement.HR2Mail = row["HR2Mail"].ToString();
                        ObjPlacement.HR2Contact = row["HR2Contact"].ToString();
                        ObjPlacement.HR3Name = row["HR3Name"].ToString();
                        ObjPlacement.HR3Mail = row["HR3Mail"].ToString();
                        ObjPlacement.HR3Contact = row["HR3Contact"].ToString();
                        ObjPlacement.CompanyDescription = row["CompanyDescription"].ToString();
                    }
                    ViewBag.Update = await objbal.GetCompany(CompanyCode);
                    ViewBag.Induastry = await objbal.GetInduastryPB();

                    DataSet ds1 = await objbal.GetCountry();
                    List<SelectListItem> countrylst = new List<SelectListItem>();
                    foreach (DataRow dr1 in ds1.Tables[0].Rows)
                    {
                        countrylst.Add(new SelectListItem { Text = dr1["CountryName"].ToString(), Value = dr1["CountryId"].ToString() });
                    }
                    ViewBag.Country = countrylst;

                    BALPlacement objBALState = new BALPlacement();
                    DataSet ds2 = await objBALState.GetState(ObjPlacement.CountryId);
                    List<SelectListItem> StateList = new List<SelectListItem>();
                    foreach (DataRow dr2 in ds2.Tables[0].Rows)
                    {
                        StateList.Add(new SelectListItem { Text = dr2["StateName"].ToString(), Value = dr2["StateId"].ToString() });
                    }
                    ViewBag.State = StateList;

                    BALPlacement objCity = new BALPlacement();
                    DataSet ds3 = new DataSet();
                    DataTable dt = new DataTable();
                    ds3 = await objCity.GetCity(ObjPlacement.StateId);
                    List<SelectListItem> Citylist = new List<SelectListItem>();
                    foreach (DataRow dr3 in ds3.Tables[0].Rows)
                    {
                        Citylist.Add(new SelectListItem { Text = dr3["CityName"].ToString(), Value = dr3["CityId"].ToString() });
                    }

                    dt = ds3.Tables[0];
                    var jsonData = JsonConvert.SerializeObject(dt);
                    ViewBag.City = Citylist;
                }
                catch (Exception ex)
                {

                    throw (ex);
                }
                return await Task.Run(() => PartialView("UpdateCompanyAsyncPB", ObjPlacement));
            }
        }
        /// <summary>
        /// THIS METHOD IS USE TO UPDATE COMPANY DETAILS.
        /// </summary>
        /// <param name="objp"></param>
        /// THIS OBJECT IS USE TO GET VALUES ON OBJECT.
        /// <returns> UPDATE COMPNAY DETAILS IN TBL GSTTBLCOMPANY.</returns>
        [HttpPost]
        public async Task<JsonResult> UpdatCompanyAsyncPB(Placement objp)
        {
            try
            {
                await objbal.UpdateCompanyDetailsAsyncPB(objp);
                //return Json(new { success = true, message = "Batch has been updated successfully!" });
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return await Task.Run(() => Json(new { success = true, message = "Company has been updated successfully!" }));
        }
        /// <summary>
        /// THIS METHOD IS USE TO GET DETAILS VIEW.
        /// </summary>
        /// <returns> GET VIEW.</returns>
        [HttpGet]
        public async Task<ActionResult> DetailsCompanyAsyncPB(string CompanyCode)
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                try
                {
                    ViewBag.Detalis = await objbal.GetAboutCompanyDetails(CompanyCode);

                }
                catch (Exception ex)
                {
                    throw (ex);
                }
                return await Task.Run(() => PartialView("DetailsCompanyAsyncPB"));
            }
        }
        /// <summary>
        /// THIS METHOD IS USE TO GET REGISTER INDUASTRY VIEW.
        /// </summary>
        /// <returns>GET INDUASTRY VIEW.</returns>
        [HttpGet]
        public async Task<ActionResult> RegisterInduastryAsyncPB()
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                try
                {
                    return await Task.Run(() => PartialView("RegisterInduastryAsyncPB"));
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
        }
        /// <summary>
        /// THIS METHOD IS USE TO SAVE INDUASTRY.
        /// </summary>
        /// <param name="objp"></param>
        /// THIS OBJECT IS USE TO SAVE NAME.
        /// <returns> SAVE INDUASTRY NAME.</returns>
        [HttpPost]
        public async Task<ActionResult> SaveInduastryAsyncPB(string InduastryName)
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                try
                {
                    await objbal.SaveIndustry(InduastryName);
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
                return await Task.Run(() => RedirectToAction("RegisterCompanyAsyncPB"));
            }
        }
        /// <summary>
        /// THIS OBJECT IS USET TO GET INDUASTRY ID AND FILTER DATA OF COMPANY.
        /// </summary>
        /// <returns>FILTER DATA OF GSTTBLCOMPANY .</returns>
        [HttpPost]
        public async Task<JsonResult> GetInduastryIdforFilter(int Induastry)
        {
            try
            {
                objPlacement.BranchCode = Session["BranchCode"].ToString();
                DataSet ds = await objbal.GetCompanyNameOnIndastryPB(Induastry, objPlacement);
                DataTable dt = ds.Tables[0];
                var jsondata = JsonConvert.SerializeObject(dt);
                return await Task.Run(() => Json(jsondata));
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// THIS METHOD IS USE TO GET FILTER DATA OF COMPANY LIST ON INDUASTRY ID.
        /// </summary>
        /// <returns>T COMPANY DETAILS LIST.</returns>
        [HttpPost]
        public async Task<JsonResult> FilterDataCompanyOnInduastryIdPB(int Induastry)
        {
            try
            {
                objPlacement.BranchCode = Session["BranchCode"].ToString();
                DataSet ds = await objbal.FilterDtaOnInduastryPB(Induastry, objPlacement);
                DataTable dt = ds.Tables[0];
                var jsondata = JsonConvert.SerializeObject(dt);
                return await Task.Run(() => Json(jsondata));
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// THIS METHOD IS USE TO GET FILTER DATA ON COMPANY CODE.
        /// </summary>
        /// <param name="Induastry"></param>
        /// THIS OBJECT IS USE TO GET COMPANY CODE.
        /// <returns> FILTER DATA OF GSTTBLCOMPANY .</returns>
        [HttpPost]
        public async Task<JsonResult> FilterDataCompanyCodePB(string CompanyCode)
        {
            try
            {
                objPlacement.BranchCode = Session["BranchCode"].ToString();
                DataSet ds = await objbal.GetFilterDataCompanyId(CompanyCode, objPlacement);
                DataTable dt = ds.Tables[0];
                var jsondata = JsonConvert.SerializeObject(dt);
                return await Task.Run(() => Json(jsondata));
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        //---------------------------------------------------------Shubhangi Mock-----------------------------------------//
        public async Task<ActionResult> ListAllMock()
        {
            List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                    new BreadcrumbItem { Name = "Dashboard", Url =Url.Action("PlcementDashboardPCAsync", "Placement") },
                     new BreadcrumbItem { Name = "Mock", Url = Url.Action("ListAllMock", "Placement")}
                };

            ViewBag.Breadcrumbs = breadcrumbs;
            return await Task.Run(() => View());
        }
        /// <summary>
        ///Get the list of internal student mocks.
        /// </summary>
        /// <param name="Statusid">Status ID for mocks.</param>
        /// <returns>List of internal student mocks.</returns>
        [HttpGet]
        private async Task<List<Placement>> ListIntMockShAsync(int Statusid)
        {
            string BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await objbal.InternalMockSHAsync(Statusid, BranchCode);
            List<Placement> lstInternalMock = new List<Placement>();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Placement objmck = new Placement();
                    objmck.MockId = Convert.ToInt32(row["MockId"]);
                    objmck.CourseName = row["CourseName"].ToString();
                    objmck.BatchName = row["BatchName"].ToString();
                    objmck.MockName = row["MockName"].ToString();
                    objmck.LabName = row["LabName"].ToString();
                    objmck.MDate = Convert.ToDateTime(row["MockDate"]).ToString("dd-MM-yyyy");
                    objmck.StartTime = Convert.ToDateTime(row["StartTime"]);
                    objmck.InterviewrName = row["StaffName"].ToString();
                    objmck.StatusId = Convert.ToInt32(row["StatusId"]);
                    objmck.Status = row["Action"] == DBNull.Value ? null : row["Action"].ToString();
                    lstInternalMock.Add(objmck);
                }
            }
            return lstInternalMock;
        }

        /// <summary>
        /// Display a list of internal student mocks.
        /// </summary>
        /// <param name="id">Status ID for  mocks.</param>
        /// <returns>List of internal student mocks.</returns>
        [HttpGet]
        public async Task<ActionResult> ListInternalMockSHAsync(int id)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                List<Placement> model = await ListIntMockShAsync(id);
                Placement objlist = new Placement();
                objlist.lstInternalMock = model;
                return PartialView("ListInternalMockSHAsync", objlist);
            }
        }
        /// <summary>
        /// Display a list of internal student for pending mocks.
        /// </summary>
        /// <param name="id">Status ID for  mocks.</param>
        /// <returns>List of internal student mocks.</returns>
        [HttpGet]
        public async Task<ActionResult> ListPendingInternalMockSHAsync(int id)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                List<Placement> model = await ListIntMockShAsync(id);
                Placement objlist = new Placement();
                objlist.lstInternalMock = model;
                return PartialView("ListInternalMockSHAsync", objlist);
            }
        }
        /// <summary>
        /// Display a list of internal student for conducted mocks.
        /// </summary>
        /// <param name="id">Status ID for  mocks.</param>
        /// <returns>List of internal student mocks.</returns>
        [HttpGet]
        public async Task<ActionResult> ListConductedInternalMockSHAsync(int id)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                List<Placement> model = await ListIntMockShAsync(id);
                Placement objlist = new Placement();
                objlist.lstInternalMock = model;
                return PartialView("ListInternalMockSHAsync", objlist);
            }
        }
        /// <summary>
        /// Get the list of external student mocks.
        /// </summary>
        /// <param name="Statusid">Status ID for mocks.</param>
        /// <returns>List of external student mocks.</returns>
        [HttpGet]
        private async Task<List<Placement>> ListExtMockShAsync(int Statusid)
        {
            string BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await objbal.ExternalMockSHAsync(Statusid, BranchCode);
            List<Placement> lstExternalMock = new List<Placement>();

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Placement objmck = new Placement();
                    objmck.MockId = Convert.ToInt32(row["MockId"]);
                    objmck.SkillName = row["SkillName"].ToString();
                    objmck.StudentName = row["FullName"].ToString();
                    objmck.MockName = row["MockName"].ToString();
                    objmck.LabName = row["LabName"].ToString();
                    objmck.MDate = Convert.ToDateTime(row["MockDate"]).ToString("dd-MM-yyyy");
                    objmck.StartTime = Convert.ToDateTime(row["StartTime"]);
                    objmck.InterviewrName = row["StaffName"].ToString();
                    objmck.StatusId = Convert.ToInt32(row["StatusId"]);
                    objmck.MockPerformanceId = (row["PerformnceId"] == DBNull.Value ? (int?)null : (int?)Convert.ToInt32(row["PerformnceId"]));
                    lstExternalMock.Add(objmck);
                }
            }

            return lstExternalMock;
        }

        /// <summary>
        /// Display a list of external student mocks.
        /// </summary>
        /// <param name="id">Status ID for mocks.</param>
        /// <returns>List of external student mocks.</returns> 
        [HttpGet]
        public async Task<ActionResult> ListExternalMockShAsync(int id = 0)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                List<Placement> model = await ListExtMockShAsync(id);
                Placement objlist = new Placement();
                objlist.lstExternalMock = model;
                return PartialView("ListExternalMockShAsync", objlist);
            }
        }
        /// <summary>
        /// Display a list of external student pending mocks.
        /// </summary>
        /// <param name="id">Status ID for mocks.</param>
        /// <returns>List of external student mocks.</returns>    
        [HttpGet]
        public async Task<ActionResult> ListPendingExternalMockShAsync(int id = 0)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                List<Placement> model = await ListExtMockShAsync(id);
                Placement objlist = new Placement();
                objlist.lstExternalMock = model;
                return PartialView("ListExternalMockShAsync", objlist);
            }
        }

        /// <summary>
        /// Display a list of external students conducted mocks.
        /// </summary>
        /// <param name="id">Status ID for mocks.</param>
        /// <returns>List of external student mocks.</returns>
        [HttpGet]
        public async Task<ActionResult> ListConductedExternalMockShAsync(int id = 0)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                List<Placement> model = await ListExtMockShAsync(id);
                Placement objlist = new Placement();
                objlist.lstExternalMock = model;
                return PartialView("ListExternalMockShAsync", objlist);
            }
        }


        /// <summary>
        /// scheduling internal student mocks.
        /// </summary>
        /// <returns>Scheduling internal student mocks.</returns>
        [HttpGet]
        public async Task<ActionResult> ScheduleInternalMockSHAsync()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                // await LabBindSHAsync();
                await CourseBindSHAsync();
                await StaffBindSHAsync();
                await StatusBindSHAsync();
                await MockNameListSHAsync();
                Placement objP = new Placement();
                objP.SelectedDuration = TimeSpan.Parse("00:00");
                return PartialView(objP);
            }
        }
        /// <summary>
        /// Schedule internal student mock.
        /// </summary>
        /// <param name="Obj">Object containing mock details.</param>
        /// <returns>List of internal mocks.</returns>
        [HttpPost]
        public async Task<ActionResult> ScheduleInternalMockSHAsync(Placement Obj)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await Task.Run(() => objbal.AddIntMock(Obj));
                return View("ListAllMock");
            }
        }
        /// <summary>
        /// Scheduling external student mocks.
        /// </summary>
        /// <returns>Scheduling external student mocks.</returns>
        [HttpGet]
        public async Task<ActionResult> ScheduleExternalMockSHAsync()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await LabBindSHAsync();
                await StaffBindSHAsync();
                await StatusBindSHAsync();
                await MockNameListSHAsync();
                Placement objP = new Placement();
                objP.SelectedDuration = TimeSpan.Parse("00:00");
                return PartialView(objP);
            }
        }
        /// <summary>
        /// Schedule external student mock.
        /// </summary>
        /// <param name="Obj">Object containing mock details.</param>
        /// <returns>List of internal mocks.</returns>
        [HttpPost]
        public async Task<ActionResult> ScheduleExternalMockSHAsync(Placement Obj)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await Task.Run(() => objbal.AddMock(Obj));
                return View("ListAllMock");
            }
        }
        /// <summary>
        /// Get the list of courses.
        /// </summary>
        [HttpGet]
        public async Task CourseBindSHAsync()
        {
            string BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await objbal.CourseListSHAsync(BranchCode);
            List<SelectListItem> Courselist = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Courselist.Add(new SelectListItem { Text = dr["CourseName"].ToString(), Value = dr["Coursecode"].ToString() });
            }
            ViewBag.CourseList = Courselist;
        }
        /// <summary>
        /// Get the list of technology.
        /// </summary>
        /// <returns> List of technology.</returns>
        [HttpGet]
        public async Task<JsonResult> TechnologyListSHAsync()
        {
            DataSet ds = await objbal.SkillListSHAsync();
            List<SelectListItem> skilllist = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                skilllist.Add(new SelectListItem { Text = dr["SkillName"].ToString(), Value = dr["SkillId"].ToString() });
            }
            return Json(skilllist, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Get the list of mock name.
        /// </summary>
        /// <returns>list of mock name.</returns>
        [HttpGet]
        public async Task MockNameListSHAsync()
        {
            DataSet ds = await objbal.MockNameListSH();
            List<SelectListItem> mocklist = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                mocklist.Add(new SelectListItem { Text = dr["MockName"].ToString(), Value = dr["MockNameId"].ToString() });
            }
            ViewBag.MockName = mocklist;
        }
        /// <summary>
        /// Get the list of batches based on course.
        /// </summary>
        /// <param name="coursecode">Course code for batches.</param>
        /// <returns>List of batches.</returns>
        [HttpGet]
        public async Task<JsonResult> BatchBindSHAsync(string coursecode)
        {
            string BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await objbal.BatchListSHAsync(coursecode, BranchCode);
            List<SelectListItem> batchlist = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                batchlist.Add(new SelectListItem { Text = dr["BatchName"].ToString(), Value = dr["BatchCode"].ToString() });
            }
            return Json(batchlist, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Get the list of internal student mocks based on batch.
        /// </summary>
        /// <param name="batchcode">Batch code for internal student mocks.</param>
        /// <returns>List of internal student mocks.</returns>
        [HttpGet]
        public async Task<JsonResult> IntrnalStudentBindSHAsync(string batchcode, int MockId)
        {
            string BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await objbal.IntStudentListMockSHAsync(batchcode, MockId, BranchCode);
            List<SelectListItem> intstudentlist = new List<SelectListItem>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                intstudentlist.Add(new SelectListItem { Text = dr["FullName"].ToString(), Value = dr["CandidateCode"].ToString() });
            }

            return Json(intstudentlist, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Get the list of external student mocks based on course.
        /// </summary>
        /// <param name="coursecode">Course code for external student mocks.</param>
        /// <returns>List of external student mocks.</returns>
        [HttpGet]
        public async Task<JsonResult> ExtStudentBindSHAsync(int MockId)
        {
            string BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await objbal.ExtStudentListMockSHAsync(MockId, BranchCode);
            List<SelectListItem> extstudentlist = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                extstudentlist.Add(new SelectListItem { Text = dr["FullName"].ToString(), Value = dr["CandidateCode"].ToString() });
            }
            return Json(extstudentlist, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> GetStudentDetailsAsync(string studentCode)
        {
            DataSet ds = await objbal.ExtStudentDetailsforMockSH(studentCode);
            List<Placement> studentDetails = new List<Placement>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                studentDetails.Add(new Placement
                {
                    CompanyName = dr["CompanyName"].ToString(),
                    DesignationName = dr["DesignationName"].ToString(),
                    Experience = dr["Experience"].ToString()
                });
            }

            return Json(studentDetails, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Get the list of staff members.
        /// </summary>
        [HttpGet]
        public async Task StaffBindSHAsync()
        {
            string BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await objbal.StaffListSHAsync(BranchCode);
            List<SelectListItem> AllStaff = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                AllStaff.Add(new SelectListItem { Text = dr["StaffName"].ToString(), Value = dr["StaffCode"].ToString() });
            }
            ViewBag.StaffList = AllStaff;
        }
        public async Task LabBindSHAsync()
        {
            string BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await objbal.LabListSHAsync(BranchCode);
            List<SelectListItem> AllLab = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                AllLab.Add(new SelectListItem { Text = dr["LabName"].ToString(), Value = dr["LabCode"].ToString() });
            }
            ViewBag.Lab = AllLab;
        }

        /// <summary>
        /// Get the list of status.
        /// </summary>
        /// <returns>Return the list of status.</returns>
        [HttpGet]
        public async Task StatusBindSHAsync()
        {
            DataSet ds1 = await objbal.StatusListSHAsync();
            List<SelectListItem> AllStatus = new List<SelectListItem>();
            foreach (DataRow dr1 in ds1.Tables[0].Rows)
            {
                AllStatus.Add(new SelectListItem { Text = dr1["Status"].ToString(), Value = dr1["StatusId"].ToString() });
            }
            ViewBag.StatusList = AllStatus;
        }
        /// <summary>
        /// Edit the details of a scheduled mock
        /// .
        /// </summary>
        /// <param name="id">Mock ID for editing scheduled mock details.</param>
        /// <returns>Editing scheduled mock details.</returns>      
        [HttpGet]
        public async Task<ActionResult> UpdateExtMockSHAsync(int MockId)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await LabBindSHAsync();
                await StaffBindSHAsync();
                await StatusBindSHAsync();
                Placement objP = new Placement();
                objP.MockId = MockId;
                DataSet ds;
                string BranchCode = Session["BranchCode"].ToString();
                ds = await objbal.ScheduledExtMockDetailsSHAsync(MockId, BranchCode);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    //Placement objP = new Placement();
                    objP.MockId = Convert.ToInt32(ds.Tables[0].Rows[i]["MockId"].ToString());
                    objP.StudentCode = ds.Tables[0].Rows[i]["CandidateCode"].ToString();
                    objP.SkillName = ds.Tables[0].Rows[i]["SkillName"].ToString();
                    objP.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                    objP.MockName = ds.Tables[0].Rows[i]["MockName"].ToString();
                    //DateTime Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["MockDate"].ToString());
                    objP.MockDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["MockDate"].ToString());
                    objP.LabCode = ds.Tables[0].Rows[i]["LabCode"].ToString();
                    objP.StaffCode = ds.Tables[0].Rows[i]["InterviewerStffcode"].ToString();
                    objP.StartTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartTime"].ToString());
                    objP.SelectedDuration = TimeSpan.Parse(ds.Tables[0].Rows[i]["Duration"].ToString());
                    objP.StatusId = Convert.ToInt32(ds.Tables[0].Rows[i]["Statusid"].ToString());
                }
                return PartialView("UpdateExtMockSHAsync", objP);
            }
        }
        /// <summary>
        /// Edit scheduled mock of external student.
        /// </summary>
        /// <param name="obju">Update the scheduled mock </param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> UpdateExtMockSHAsync(Placement obju)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await Task.Run(() => objbal.UpdateScheduledMockSHAsync(obju));
                return View("ListAllMock");
            }
        }
        /// <summary>
        /// Edit scheduled mock of internal student.
        /// </summary>
        /// <param name="id">Get scheduled mock details of particular mockid.</param>
        /// <returns>Return details of scheduled mock.</returns>
        [HttpGet]
        public async Task<ActionResult> UpdateIntMockSHAsync(int MockId)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await LabBindSHAsync();
                await StaffBindSHAsync();
                await StatusBindSHAsync();
                Placement objP = new Placement();
                objP.MockId = MockId;
                DataSet ds;
                string BranchCode = Session["BranchCode"].ToString();
                ds = await objbal.ScheduledIntMockDetailsSHAsync(MockId, BranchCode);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    //Placement objP = new Placement();
                    objP.MockId = Convert.ToInt32(ds.Tables[0].Rows[i]["MockId"].ToString());
                    objP.CourseCode = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objP.BatchCode = ds.Tables[0].Rows[i]["BatchCode"].ToString();
                    objP.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    //objP.StudentCode = ds.Tables[0].Rows[i]["FullName"].ToString();
                    objP.MockName = ds.Tables[0].Rows[i]["MockName"].ToString();
                    objP.MockDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["MockDate"].ToString());
                    objP.LabCode = ds.Tables[0].Rows[i]["LabCode"].ToString();
                    objP.StaffCode = ds.Tables[0].Rows[i]["InterviewerStffcode"].ToString();
                    objP.StartTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartTime"].ToString());
                    objP.SelectedDuration = TimeSpan.Parse(ds.Tables[0].Rows[i]["Duration"].ToString());
                    objP.StatusId = Convert.ToInt32(ds.Tables[0].Rows[i]["StatusId"].ToString());
                }
                List<string> studentNames = new List<string>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string studentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                    studentNames.Add(studentName);
                }
                objP.StudentList = studentNames;
                return PartialView("UpdateIntMockSHAsync", objP);
            }
        }
        /// <summary>
        /// Update the internal students scheduled mock.
        /// </summary>
        /// <param name="obju">It update the selected mock.</param>
        /// <returns>Update mock.</returns>
        [HttpPost]
        public async Task<ActionResult> UpdateIntMockSHAsync(Placement obju)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await Task.Run(() => objbal.UpdateScheduledMockSHAsync(obju));
                return RedirectToAction("ListAllMock");
            }
        }
        [HttpGet]
        /// <summary>
        /// Fetch and display mock performance details.
        /// </summary>
        /// <param name="mockid">This object retrive performance of the mock placement</param>
        /// <returns>It containing mock performance details.</returns>
        public async Task<ActionResult> ViewMockPerformanceSHAsync(int mockid)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Placement obj = new Placement();
                obj.MockId = mockid;
                DataSet ds = await objbal.ViewMockPerformanceSH(mockid);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    obj.MockId = Convert.ToInt32(ds.Tables[0].Rows[i]["MockId"].ToString());
                    obj.CourseName = ds.Tables[0].Rows[i]["SkillName"].ToString();
                    obj.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                    obj.MockName = ds.Tables[0].Rows[i]["MockName"].ToString();
                    obj.Attendance = ds.Tables[0].Rows[i]["Status"].ToString();
                    DateTime Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["MockDate"].ToString());
                    obj.MDate = Date.ToString("dd-MM-yyyy");
                    obj.PerformanceStatus = ds.Tables[0].Rows[i]["PerformanceStatus"].ToString();
                }
                return PartialView("ViewMockPerformanceSHAsync", obj);
            }
        }/// <summary>
         /// View internal students mock performance
         /// </summary>
         /// <param name="MockId">Mockid use to show particular maock performance.</param>
         /// <returns>Show mock performance.</returns>
        [HttpGet]
        public async Task<ActionResult> ViewIntMockPerformanceSHAsync(int MockId)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                List<Placement> placements = new List<Placement>();
                DataSet ds = await objbal.ViewIntMockPerformanceSH(MockId);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Placement obj = new Placement();
                    obj.MockId = Convert.ToInt32(ds.Tables[0].Rows[i]["MockId"].ToString());
                    obj.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    obj.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    obj.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                    obj.MockName = ds.Tables[0].Rows[i]["MockName"].ToString();
                    obj.Attendance = ds.Tables[0].Rows[i]["Status"].ToString();
                    DateTime Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["MockDate"].ToString());
                    obj.MDate = Date.ToString("dd-MM-yyyy");
                    obj.PerformanceStatus = ds.Tables[0].Rows[i]["PerformanceStatus"].ToString();
                    placements.Add(obj);
                }
                return PartialView("ViewIntMockPerformanceSHAsync", placements);
            }
        }
        /// <summary>
        /// Get details for add mock performance of external students.
        /// </summary>
        /// <param name="MockId">Get detilas of conducted mock.</param>
        /// <returns>Details of scheduled mock.</returns>
        [HttpGet]
        public async Task<ActionResult> AddMockExtPerformanceSHAsync(int MockId)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Placement objP = new Placement();
                objP.MockId = MockId;
                DataSet ds;
                string BranchCode = Session["BranchCode"].ToString();
                ds = await objbal.ScheduledExtMockDetailsSHAsync(MockId, BranchCode);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    objP.MockId = Convert.ToInt32(ds.Tables[0].Rows[i]["MockId"].ToString());
                    objP.StudentCode = ds.Tables[0].Rows[i]["CandidateCode"].ToString();
                    objP.SkillName = ds.Tables[0].Rows[i]["SkillName"].ToString();
                    objP.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                    objP.MockName = ds.Tables[0].Rows[i]["MockName"].ToString();
                    DateTime Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["MockDate"].ToString());
                    objP.MDate = Date.ToString("dd-MM-yyyy");
                }
                return PartialView("AddMockExtPerformanceSHAsync", objP);
            }
        }
        /// <summary>
        /// Add mock perforamnce of External students.
        /// </summary>
        /// <param name="Obj">Add mock performanmce of external student.</param>
        /// <returns>Add movk performance.</returns>
        [HttpPost]
        public async Task<ActionResult> AddMockExtPerformanceSHAsync(Placement Obj)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Obj.StaffCode = Session["StaffCode"].ToString();
                int averageRating = (Obj.CommunicationRating + Obj.TechnicalRating + Obj.ConfidenceRating + Obj.ProjectKnowledgeRating) / 4;
                Obj.PerformanceRating = averageRating;
                Obj.PerformanceStatus = GetPerformanceCategory(averageRating);
                await Task.Run(() => objbal.AddMockPerformanceSH(Obj));
                return View("Mock");
            }
        }
        /// <summary>
        /// Get details for add mock performance of internal students.
        /// </summary>
        /// <param name="MockId">Get detilas of conducted mock.</param>
        /// <returns>Details of scheduled mock.</returns>
        [HttpGet]
        public async Task<ActionResult> AddInternalMockPerformanceSHAsync(int MockId)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Placement objP = new Placement();
                objP.MockId = MockId;
                DataSet ds;
                string BranchCode = Session["BranchCode"].ToString();
                ds = await objbal.ScheduledIntMockDetailsSHAsync(MockId, BranchCode);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    objP.MockId = Convert.ToInt32(ds.Tables[0].Rows[i]["MockId"].ToString());
                    objP.StudentCode = ds.Tables[0].Rows[i]["CandidateCode"].ToString();
                    objP.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    objP.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objP.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                    objP.MockName = ds.Tables[0].Rows[i]["MockName"].ToString();
                    DateTime Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["MockDate"].ToString());
                    objP.MDate = Date.ToString("dd-MM-yyyy");
                }
                DataSet ds3 = await objbal.StudentListSHAsync(objP.MockId);
                List<SelectListItem> studentlist = new List<SelectListItem>();
                foreach (DataRow dr3 in ds3.Tables[0].Rows)
                {
                    studentlist.Add(new SelectListItem { Text = dr3["FullName"].ToString(), Value = dr3["CandidateCode"].ToString() });
                }
                ViewBag.StudentList1 = studentlist;

                return PartialView("AddInternalMockPerformanceSHAsync", objP);
            }
        }
        /// <summary>
        /// Add mock perforamnce of internal students.
        /// </summary>
        /// <param name="Obj">Add mock performanmce of external student.</param>
        /// <returns>Add movk performance.</returns>
        [HttpPost]
        public async Task<ActionResult> AddInternalMockPerformanceSHAsync(List<Placement> studentDataArray)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                try
                {
                    // BALPlacement objP = new BALPlacement();
                    foreach (var studentData in studentDataArray)
                    {
                        // Call your asynchronous method to add results to the database
                        await objbal.AddMockPerformanceSH(new Placement
                        {
                            MockId = studentData.MockId,
                            StudentCode = studentData.StudentCode,
                            PerformanceStatus = studentData.PerformanceStatus,
                            AttendanceId = studentData.AttendanceId,
                            StaffCode = Session["StaffCode"].ToString()
                        });
                    }
                    // Return a success response if needed
                    return Json(new { success = true, message = "Results saved successfully." });
                }
                catch (Exception ex)
                {
                    // Log the exception details
                    Console.WriteLine(ex.ToString());

                    // Return an error response with a more detailed message
                    return Json(new { success = false, message = "An error occurred while saving results. Details: " + ex.Message });
                }
            }
        }

        /// <summary>
        /// Calculate performance.
        /// </summary>
        /// <param name="rating"></param>
        /// <returns>Returns calculated performance.</returns>
        private string GetPerformanceCategory(int rating)
        {
            if (rating >= 1 && rating < 3)
            {
                return "Low";
            }
            else if (rating >= 3 && rating < 5)
            {
                return "Average";
            }
            else if (rating >= 5 && rating < 7)
            {
                return "Good";
            }
            else if (rating >= 7 && rating < 9)
            {
                return "Better";
            }
            else if (rating == 10)
            {
                return "Best";
            }
            else
            {
                return "-";
            }
        }
        //----------------------------------------Testimonial Prem--------------------------------//
        /// <summary>
        /// Retrieves a list of testimonials asynchronously.
        /// </summary>
        /// <returns>Returns a View containing a list of testimonials.</returns>
        [HttpGet]
        public async Task<ActionResult> ListTestimonialPD()
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                Placement objP = new Placement();
                objP.BranchCode = Session["BranchCode"].ToString();
                DataSet ds = await objbal.GetTestimonialPDAsync(objP);
                List<Placement> ListTestimonial = new List<Placement>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Placement ObjP1 = new Placement();
                    ObjP1.TestimonialId = Convert.ToInt32(row["TestimonialId"].ToString());
                    ObjP1.StudentName = row["FullName"].ToString();
                    ObjP1.Comment = row["Comments"].ToString();
                    ObjP1.Audio = row["UploadAudio"].ToString();
                    ObjP1.Video = row["UploadVideo"].ToString();
                    ObjP1.Pdf = row["UploadPDF"].ToString();
                    ListTestimonial.Add(ObjP1);
                }
                objP.lstTestimonial = ListTestimonial;
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                    new BreadcrumbItem { Name = "Dashboard", Url =Url.Action("PlcementDashboardPCAsync","Placement")},
                    new BreadcrumbItem { Name = "Testimonial", Url =  Url.Action("ListTestimonialPD","Placement")}
                };

                ViewBag.Breadcrumbs = breadcrumbs;
                return await Task.Run(() => View(objP));
            }

        }
        /// <summary>
        /// Retrieves and binds course data asynchronously for use in views.
        /// </summary>
        /// <returns>Returns a JsonResult containing a list of SelectListItem objects representing courses.</returns>
        public async Task<JsonResult> Course_BindPDAsync()
        {
            Placement objP = new Placement();
            objP.BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await objbal.GetCoursePDAsync(objP);
            List<SelectListItem> CourseList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                CourseList.Add(new SelectListItem { Text = dr["CourseName"].ToString(), Value = dr["Coursecode"].ToString() });
            }
            ViewBag.Course = CourseList;
            return await Task.Run(() => Json(CourseList, JsonRequestBehavior.AllowGet));
        }
        /// <summary>
        /// Prepares the view for adding a testimonial asynchronously.
        /// </summary>
        /// <returns>Returns a View for adding a testimonial with necessary data populated asynchronously.</returns>
        public async Task<ActionResult> AddTestimonialPDAsync()
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                await Course_BindPDAsync();
                Placement objP = new Placement();
                objP.BranchCode = Session["BranchCode"].ToString();
                BALPlacement objbalpla = new BALPlacement();
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                    new BreadcrumbItem { Name = "Dashboard", Url =Url.Action("PlcementDashboardPCAsync", "Placement") },
                    new BreadcrumbItem { Name = "Testimonial", Url =  Url.Action("ListTestimonialPD","Placement")},
                    new BreadcrumbItem { Name = "Add Testimonial", Url =  Url.Action("AddTestimonialPDAsync","Placement")}
                };
                ViewBag.Breadcrumbs = breadcrumbs;
                return View(objP);
            }
        }
        /// <summary>
        /// Retrieves student names asynchronously based on the provided course code.
        /// </summary>
        /// <param name="Coursecode">The course code used to retrieve student names.</param>
        /// <returns>
        /// Returns a JsonResult containing a list of SelectListItem objects, where each SelectListItem represents a student name with their corresponding candidate code.
        /// </returns>
        [HttpGet]
        public async Task<JsonResult> StudentName_Bind(string Coursecode)
        {
            Placement objP = new Placement();
            objP.CourseCode = Coursecode;
            objP.BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await objbal.GetStudentNamePDAsync(objP);
            List<SelectListItem> BatchNameList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                BatchNameList.Add(new SelectListItem { Text = dr["StudentName"].ToString(), Value = dr["CandidateCode"].ToString() });
            }
            return Json(BatchNameList, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Retrieves student data asynchronously based on the provided candidate code.
        /// </summary>
        /// <param name="CandidateCode">The candidate code used to retrieve student data.</param>
        /// <returns>
        /// Returns a JsonResult containing student data such as candidate code, student name, qualification, company name, designation name, CTC, and location.
        /// </returns>
        [HttpGet]
        public async Task<JsonResult> StudentData(string CandidateCode)
        {
            Placement objP = new Placement();
            objP.CandidateCode = CandidateCode;
            objP.BranchCode = Session["BranchCode"].ToString();
            SqlDataReader dr = await objbal.GetStudentDataPDAsync(objP);
            if (dr.Read())
            {
                objP.CandidateCode = dr["CandidateCode"].ToString();
                objP.StudentName = dr["FullName"].ToString();
                objP.Qualification = dr["HighestQualification"].ToString();
                objP.CompanyName = dr["CompanyName"].ToString();
                objP.DesignationName = dr["DesignationName"].ToString();
                objP.CTC = dr["Salary"].ToString();
                objP.Location = dr["Location"].ToString();
                objP.JoiningDatePM = dr["JoiningDate"].ToString();
            }
            return Json(objP, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Saves a testimonial asynchronously.
        /// </summary>
        /// <param name="objP">The Placement object containing the testimonial details to be saved.</param>
        /// <returns>
        /// Saves the testimonial details provided in the Placement object asynchronously.
        /// and redirects to the "ListTestimonialPD" action.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult> SaveTestimonial(Placement objP)
        {
            if (Session["StaffCode"] == null || Session["BranchCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                if (objP.PDFFILe != null && objP.PDFFILe.ContentLength > 0)
                {
                    var fileName = Path.GetFileNameWithoutExtension(objP.PDFFILe.FileName);
                    var extension = Path.GetExtension(objP.PDFFILe.FileName);
                    fileName = fileName + extension;
                    var filePath = "~/Content/Placement/Testimonial/PDF/" + fileName;
                    objP.PdfPath = filePath;
                    fileName = Path.Combine(Server.MapPath("~/Content/Placement/Testimonial/PDF/"), fileName);
                    objP.PDFFILe.SaveAs(fileName);
                }
                if (objP.AudioFIle != null && objP.AudioFIle.ContentLength > 0)
                {
                    var newFileName = Path.GetFileNameWithoutExtension(objP.AudioFIle.FileName);
                    var extension = Path.GetExtension(objP.AudioFIle.FileName);
                    newFileName = newFileName + extension;
                    var filePath = "~/Content/Placement/Testimonial/Audio/" + newFileName;
                    objP.AudioPath = filePath;
                    filePath = Path.Combine(Server.MapPath("~/Content/Placement/Testimonial/Audio/"), newFileName);
                    objP.AudioFIle.SaveAs(filePath);
                }
                if (objP.VideoFILe != null && objP.VideoFILe.ContentLength > 0)
                {
                    var newFileName = Path.GetFileNameWithoutExtension(objP.VideoFILe.FileName);
                    var extension = Path.GetExtension(objP.VideoFILe.FileName);
                    newFileName = newFileName + extension;
                    var filePath = "~/Content/Placement/Testimonial/Video/" + newFileName;
                    objP.VideoPath = filePath;
                    filePath = Path.Combine(Server.MapPath("~/Content/Placement/Testimonial/Video/"), newFileName);
                    objP.VideoFILe.SaveAs(filePath);
                }
                objP.StaffCode = Session["StaffCode"].ToString();
                objP.BranchCode = Session["BranchCode"].ToString();
                await objbal.SaveTestimonialAsyncPD(objP);
                return RedirectToAction("ListTestimonialPD");

            }

        }
        /// <summary>
        /// Retrieves testimonial details for editing asynchronously.
        /// </summary>
        /// <param name="id">The ID of the testimonial to be edited.</param>
        /// <returns>Returns a View containing the details of the testimonial identified by the given ID for editing.</returns>
        [HttpGet]
        public async Task<ActionResult> UpdateTestimonialPD(int id)
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                Placement objP = new Placement();
                objP.TestimonialId = id;

                SqlDataReader dr;
                dr = await objbal.EditTestimonialPDAsync(objP);
                while (dr.Read())
                {
                    objP.TestimonialId = Convert.ToInt32(dr["TestimonialId"].ToString());
                    objP.CandidateCode = dr["CandidateCode"].ToString();
                    objP.StudentName = dr["FullName"].ToString();
                    objP.Date = Convert.ToDateTime(dr["TodayDate"].ToString());
                    objP.Qualification = dr["HighestQualification"].ToString();
                    objP.CompanyName = dr["CompanyName"].ToString();
                    objP.DesignationName = dr["DesignationName"].ToString();
                    objP.CTC = dr["Salary"].ToString();
                    objP.Location = dr["Location"].ToString();
                    objP.Comment = dr["Comments"].ToString();
                    objP.Video = dr["UploadVideo"].ToString();
                    objP.Audio = dr["UploadAudio"].ToString();
                    objP.Pdf = dr["UploadPDF"].ToString();
                }
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                    new BreadcrumbItem { Name = "Dashboard", Url =Url.Action("PlcementDashboardPCAsync", "Placement") },
                    new BreadcrumbItem { Name = "ALLTestimonial", Url =  Url.Action("ListTestimonialPD","Placement")},
                    new BreadcrumbItem { Name = "EditTestimonial", Url =  Url.Action("UpdateTestimonialPD","Placement")}
                };
                ViewBag.Breadcrumbs = breadcrumbs;
                return View("UpdateTestimonialPD", objP);
            }
        }
        /// <summary>
        /// Updates the testimonial details asynchronously.
        /// </summary>
        /// <param name="objP">The Placement object containing the updated testimonial details.</param>
        /// <returns>Returns a redirection to the "ListTestimonialPD" action if the update is successful; otherwise, redirects to the "Login" action of the "Account" controller.</returns>
        [HttpPost]
        public async Task<ActionResult> UpdateTestimonialPD(Placement objP)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                if (objP.PDFFILe != null)
                {
                    var fileName = Path.GetFileNameWithoutExtension(objP.PDFFILe.FileName);
                    var extension = Path.GetExtension(objP.PDFFILe.FileName);
                    fileName = fileName + extension;
                    var filePath = "~/Content/Placement/Testimonial/PDF/" + fileName;
                    objP.UploadPDF = filePath;
                    fileName = Path.Combine(Server.MapPath("~/Content/Placement/Testimonial/PDF/"), fileName);
                    objP.PDFFILe.SaveAs(fileName);
                }
                else
                {
                    objP.UploadPDF = objP.Pdf;
                }
                if (objP.AudioFIle != null)
                {
                    var fileName = Path.GetFileNameWithoutExtension(objP.AudioFIle.FileName);
                    var extension = Path.GetExtension(objP.AudioFIle.FileName);
                    fileName = fileName + extension;
                    var filePath = "~/Content/Placement/Testimonial/Audio/" + fileName;
                    objP.UploadAudio = filePath;
                    fileName = Path.Combine(Server.MapPath("~/Content/Placement/Testimonial/Audio/"), fileName);
                    objP.AudioFIle.SaveAs(fileName);
                }
                else
                {
                    objP.UploadAudio = objP.Audio;
                }
                if (objP.VideoFILe != null)
                {
                    var fileName = Path.GetFileNameWithoutExtension(objP.VideoFILe.FileName);
                    var extension = Path.GetExtension(objP.VideoFILe.FileName);
                    fileName = fileName + extension;
                    var filePath = "~/Content/Placement/Testimonial/Video/" + fileName;
                    objP.UploadVideo = filePath;
                    fileName = Path.Combine(Server.MapPath("~/Content/Placement/Testimonial/Video/"), fileName);
                    objP.VideoFILe.SaveAs(fileName);
                }
                else
                {
                    objP.UploadVideo = objP.Video;
                }

                await objbal.UpdateTestimonialAsyncPD(objP);
                return RedirectToAction("ListTestimonialPD");
            }
        }
        /// <summary>
        /// Retrieves details of a testimonial for viewing asynchronously.
        /// </summary>
        /// <param name="id">The ID of the testimonial to be viewed.</param>
        /// <returns>Returns a View containing the details of the testimonial identified by the given ID.</returns>
        [HttpGet]
        public async Task<ActionResult> ViewTestimonialPD(int id)
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                Placement objP = new Placement();
                try
                {
                    objP.TestimonialId = id;
                    SqlDataReader dr;
                    dr = await objbal.ViewTestimonialPDAsync(objP);
                    while (dr.Read())
                    {
                        objP.TestimonialId = Convert.ToInt32(dr["TestimonialId"].ToString());
                        objP.CandidateCode = dr["CandidateCode"].ToString();
                        objP.StudentName = dr["FullName"].ToString();
                        objP.TodayDate = dr["TodayDate"].ToString();
                        objP.Qualification = dr["HighestQualification"].ToString();
                        objP.CompanyName = dr["CompanyName"].ToString();
                        objP.DesignationName = dr["DesignationName"].ToString();
                        objP.CTC = dr["Salary"].ToString();
                        objP.JoiningDatePM = dr["JoiningDate"].ToString();
                        objP.Location = dr["Location"].ToString();
                        objP.Comment = dr["Comments"].ToString();
                        objP.Video = dr["UploadVideo"].ToString();
                        objP.Audio = dr["UploadAudio"].ToString();
                        objP.Pdf = dr["UploadPDF"].ToString();
                    }
                    List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                    new BreadcrumbItem { Name = "Dashboard", Url =Url.Action("PlcementDashboardPCAsync", "Placement") },
                    new BreadcrumbItem { Name = "Testimonial", Url =  Url.Action("ListTestimonialPD","Placement")},
                    new BreadcrumbItem { Name = "View Testimonial", Url =  Url.Action("ViewTestimonialPD","Placement")}
                };
                    ViewBag.Breadcrumbs = breadcrumbs;
                    return View("ViewTestimonialPD", objP);
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
                    return View("Error"); // You can create an Error view to display the error message
                }
            }
        }
        [HttpGet]
        public async Task<ActionResult> ListFeesCompletionPD()
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                Placement objP = new Placement();
                objP.BranchCode = Session["BranchCode"].ToString();
                DataSet ds = await objbal.GetFeesCompletionAsyncPD(objP);
                List<Placement> ListFeesCompletion = new List<Placement>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Placement ObjP1 = new Placement();
                    ObjP1.CandidateCode = row["CandidateCode"].ToString();
                    ObjP1.StudentName = row["FullName"].ToString();
                    ObjP1.ContactNumber = row["ContactNumber"].ToString();
                    ObjP1.EmailId = row["EmailId"].ToString();
                    ObjP1.BatchName = row["BatchName"].ToString();
                    ObjP1.CourseName = row["CourseName"].ToString();
                    ObjP1.CTC = row["CTC"].ToString();
                    ObjP1.TotalFees = float.Parse(row["ThreeMonthCTC"].ToString());
                    ObjP1.PaidAmount = float.Parse(row["PaidAmount"].ToString());
                    ObjP1.PendingAmount = float.Parse(row["PendingAmount"].ToString());
                    ListFeesCompletion.Add(ObjP1);
                }
                objP.lstFees = ListFeesCompletion;
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                    new BreadcrumbItem { Name = "Dashboard", Url =Url.Action("PlcementDashboardPCAsync","Placement")},
                    new BreadcrumbItem { Name = "Fees Completion", Url =  Url.Action("ListFeesCompletionPD","Placement")}
                };

                ViewBag.Breadcrumbs = breadcrumbs;
                return await Task.Run(() => View(objP));
            }
        }


        //------------------------------------------------Pratiksha(StudentList)------------------------------------------------
        /// <summary>
        /// This is ListInternalRelesedStudentsPRAsync method for show the list of internal relesed students.
        /// </summary>
        /// <returns>This list return the internal relesed students list.</returns>
        public async Task<ActionResult> ListInternalRelesedStudentsPRAsync()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                try
                {
                    DataSet ds = new DataSet();
                    Placement objpla = new Placement();
                    objpla.BranchCode = Session["BranchCode"].ToString();
                    ds = await objbal.ListInternalRelesedStudentsPRAsync(objpla);
                    List<Placement> lstRelesedStudentList = new List<Placement>();

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Placement objP = new Placement();
                        objP.CandidateCode = ds.Tables[0].Rows[i]["CandidateCode"].ToString();
                        objP.StudentName = ds.Tables[0].Rows[i]["StudentName"].ToString();
                        objP.MobileNo = ds.Tables[0].Rows[i]["MobileNo"].ToString();
                        objP.EmailId = ds.Tables[0].Rows[i]["EmailId"].ToString();
                        objP.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                        objP.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                        objP.Qualification = ds.Tables[0].Rows[i]["Qualification"].ToString();
                        objP.YearOfPassing = ds.Tables[0].Rows[i]["YearOfPassing"].ToString();
                        objP.Experience = ds.Tables[0].Rows[i]["Experience"].ToString();
                        objP.ResumeFilePath = ds.Tables[0].Rows[i]["Resume"].ToString();
                        lstRelesedStudentList.Add(objP);
                    }
                    objpla.lstRelesedStudent = lstRelesedStudentList;
                    List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Name = "Dashboard",Url =Url.Action("Dashboard","Placement") },
                new BreadcrumbItem { Name = "RelesedStudentsList", Url = Url.Action("ListInternalRelesedStudentsPRAsync", "Placement") },
            };
                    ViewBag.Breadcrumbs = breadcrumbs;
                    return View(objpla);
                }
                catch (Exception)
                {
                    ViewBag.ErrorMessage = "An error occurred while processing the request.";
                    return View("Error");
                }
            }
        }

        /// <summary>
        /// This is ListInternalOnHoldStudentsPRAsync method for show the list of internal on hold students.
        /// </summary>
        /// <returns>This list return the internal on hold students list.</returns>
        public async Task<ActionResult> ListInternalOnHoldStudentsPRAsync()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                try
                {
                    DataSet ds = new DataSet();
                    Placement objpla = new Placement();
                    objpla.BranchCode = Session["BranchCode"].ToString();
                    ds = await objbal.ListInternalOnHoldStudentsPRAsync(objpla);
                    List<Placement> lstOnHoldStudentList = new List<Placement>();

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Placement objP = new Placement();
                        objP.StudentName = ds.Tables[0].Rows[i]["StudentName"].ToString();
                        objP.MobileNo = ds.Tables[0].Rows[i]["MobileNo"].ToString();
                        objP.EmailId = ds.Tables[0].Rows[i]["EmailId"].ToString();
                        objP.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                        objP.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                        objP.Qualification = ds.Tables[0].Rows[i]["Qualification"].ToString();
                        objP.YearOfPassing = ds.Tables[0].Rows[i]["YearOfPassing"].ToString();
                        objP.Experience = ds.Tables[0].Rows[i]["Experience"].ToString();
                        objP.ResumeFilePath = ds.Tables[0].Rows[i]["Resume"].ToString();
                        lstOnHoldStudentList.Add(objP);
                    }
                    objpla.lstOnHoldStudent = lstOnHoldStudentList;
                    List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Name = "Dashboard",Url =Url.Action("Dashboard","Placement") },
                new BreadcrumbItem { Name = "OnHoldStudentList", Url = Url.Action("ListInternalOnHoldStudentsPRAsync", "Placement") },
            };
                    ViewBag.Breadcrumbs = breadcrumbs;
                    return View(objpla);
                }
                catch (Exception)
                {
                    ViewBag.ErrorMessage = "An error occurred while processing the request.";
                    return View("Error");
                }
            }
        }

        /// <summary>
        /// Retrieves and displays the list of externally released students for placement.
        /// </summary>
        /// <returns>An ActionResult representing the view with the list of released students.</returns>
        public async Task<ActionResult> ListExternalRelesedStudentsPRAsync()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                try
                {
                    DataSet ds = new DataSet();
                    Placement objpla = new Placement();
                    objpla.BranchCode = Session["BranchCode"].ToString();
                    ds = await objbal.ListExternalRelesedStudentsPRAsync(objpla);
                    List<Placement> lstExtRelesedStudentList = new List<Placement>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Placement objP = new Placement();
                        objP.StudentName = ds.Tables[0].Rows[i]["StudentName"].ToString();
                        objP.MobileNo = ds.Tables[0].Rows[i]["MobileNo"].ToString();
                        objP.EmailId = ds.Tables[0].Rows[i]["EmailId"].ToString();
                        objP.Qualification = ds.Tables[0].Rows[i]["Qualification"].ToString();
                        objP.YearOfPassing = ds.Tables[0].Rows[i]["YearOfPassing"].ToString();
                        objP.Experience = ds.Tables[0].Rows[i]["Experience"].ToString();
                        objP.ResumeFilePath = ds.Tables[0].Rows[i]["Resume"].ToString();
                        lstExtRelesedStudentList.Add(objP);
                    }
                    objpla.lstExtRelesedStudent = lstExtRelesedStudentList;

                    List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
        {
            new BreadcrumbItem { Name = "Dashboard", Url = Url.Action("Dashboard", "Placement") },
            new BreadcrumbItem { Name = "ExternalRelesedStudentsList", Url = Url.Action("ListExternalRelesedStudentsPRAsync", "Placement") },
        };
                    ViewBag.Breadcrumbs = breadcrumbs;
                    return View(objpla);
                }
                catch (Exception)
                {
                    ViewBag.ErrorMessage = "An error occurred while processing the request.";
                    return View("Error");
                }
            }
        }

        /// <summary>
        /// Retrieves and displays the list of externally on-hold students for placement.
        /// </summary>
        /// <returns>An ActionResult representing the view with the list of on-hold students.</returns>
        public async Task<ActionResult> ListExternalOnHoldStudentsPRAsync()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                try
                {
                    DataSet ds = new DataSet();
                    Placement objpla = new Placement();
                    objpla.BranchCode = Session["BranchCode"].ToString();
                    ds = await objbal.ListExternalOnHoldStudentsPRAsync(objpla);
                    List<Placement> lstExtOnHoldStudentList = new List<Placement>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Placement objP = new Placement();
                        objP.StudentName = ds.Tables[0].Rows[i]["StudentName"].ToString();
                        objP.MobileNo = ds.Tables[0].Rows[i]["MobileNo"].ToString();
                        objP.EmailId = ds.Tables[0].Rows[i]["EmailId"].ToString();
                        objP.Qualification = ds.Tables[0].Rows[i]["Qualification"].ToString();
                        objP.YearOfPassing = ds.Tables[0].Rows[i]["YearOfPassing"].ToString();
                        objP.Experience = ds.Tables[0].Rows[i]["Experience"].ToString();
                        objP.ResumeFilePath = ds.Tables[0].Rows[i]["Resume"].ToString();
                        lstExtOnHoldStudentList.Add(objP);
                    }

                    objpla.lstExtOnHoldStudent = lstExtOnHoldStudentList;

                    List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
        {
            new BreadcrumbItem { Name = "Dashboard", Url = Url.Action("Dashboard", "Placement") },
            new BreadcrumbItem { Name = "ExternalOnHoldStudentsList", Url = Url.Action("ListExternalOnHoldStudentsPRAsync", "Placement") },
        };
                    ViewBag.Breadcrumbs = breadcrumbs;

                    return View(objpla);
                }
                catch (Exception)
                {
                    ViewBag.ErrorMessage = "An error occurred while processing the request.";
                    return View("Error");
                }
            }
        }

        /// <summary>
        /// Asynchronously retrieves the list of courses for placement and returns it as a JsonResult.
        /// </summary>
        /// <returns>A JsonResult containing the list of courses.</returns>
        public async Task<JsonResult> Course_BindPRAsync()
        {
            try
            {
                Placement objpla = new Placement();
                objpla.BranchCode = Session["BranchCode"].ToString();
                DataSet ds = await objbal.FetchCoursePRAsync(objpla);
                List<SelectListItem> CourseList = new List<SelectListItem>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    CourseList.Add(new SelectListItem { Text = dr["CourseName"].ToString(), Value = dr["Coursecode"].ToString() });
                }
                ViewBag.Course = CourseList;
                return await Task.Run(() => Json(CourseList, JsonRequestBehavior.AllowGet));
            }
            catch (Exception)
            {
                return Json(new { error = "An error occurred while fetching the course list." }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// This is Batch_BindPRAsync method for bind the batches.
        /// </summary>
        /// <param name="CourseCode"></param>
        /// <returns>This method return the batches.</returns>
        public async Task<JsonResult> Batch_BindPRAsync(Placement obj)
        {
            try
            {
                obj.BranchCode = Session["BranchCode"].ToString();
                DataSet ds = await objbal.FetchBatchPRAsync(obj);
                List<SelectListItem> BatchList = new List<SelectListItem>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BatchList.Add(new SelectListItem { Text = dr["BatchName"].ToString(), Value = dr["BatchCode"].ToString() });
                }

                ViewBag.Batch = BatchList;
                return await Task.Run(() => Json(BatchList, JsonRequestBehavior.AllowGet));
            }
            catch (Exception)
            {
                return Json(new { error = "An error occurred while fetching batch data." }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Asynchronously retrieves the list of students for a given batch code and returns it as a JsonResult.
        /// </summary>
        /// <param name="BatchCode">The batch code for which to retrieve the students.</param>
        /// <returns>A JsonResult containing the list of students.</returns>
        public async Task<JsonResult> Student_BindPRAsync(Placement obj)
        {
            try
            {
                obj.BranchCode = Session["BranchCode"].ToString();
                DataSet ds = await objbal.FetchStudentPRAsync(obj);
                List<SelectListItem> StudentList = new List<SelectListItem>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    StudentList.Add(new SelectListItem { Text = dr["StudentName"].ToString(), Value = dr["CandidateCode"].ToString() });
                }
                ViewBag.StudentName = StudentList;
                return await Task.Run(() => Json(StudentList, JsonRequestBehavior.AllowGet));
            }
            catch (Exception)
            {
                return Json(new { error = "An error occurred while fetching the student list." }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// This is RegistrationDetailsPRAsync method for showing the dropdown.
        /// </summary>
        /// <returns>This returns the student information.</returns>
        [HttpGet]
        public async Task<ActionResult> RegistrationDetailsPRAsync(Placement obj, string CandidateCode)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                try
                {
                    obj.CandidateCode = CandidateCode;
                    await Course_BindPRAsync();
                    await Skill_BindPRAsync();
                    await Project_BindPRAsync();
                    await Company_BindPRAsync();
                    await Induastry_BindPRAsync();
                    await Designation_BindPRAsync();

                    List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
        {
            new BreadcrumbItem { Name = "Dashboard", Url = Url.Action("Dashboard", "Placement") },
            new BreadcrumbItem { Name = "RelesedStudentsList", Url = Url.Action("ListInternalRelesedStudentsPRAsync", "Placement") },
            new BreadcrumbItem { Name = "Registration", Url = Url.Action("RegistrationDetailsPRAsync", "Placement") },
        };

                    ViewBag.Breadcrumbs = breadcrumbs;
                    return View("RegistrationDetailsPRAsync", obj);
                }
                catch (Exception)
                {
                    ViewBag.ErrorMessage = "An error occurred while processing the registration details.";
                    return View("Error");
                }
            }
        }

        /// <summary>
        /// Updates registration details, assigns skills and projects, saves resume files,
        /// and updates the placement student status asynchronously.
        /// </summary>
        /// <param name="obj">The Placement object containing updated registration details.</param>
        /// <returns>The updated RegistrationDetailsPRAsync view with the Placement object.</returns>
        [HttpPost]
        public async Task<ActionResult> RegistrationDetailsPRAsync(Placement obj)
        {
            try
            {
                obj.StatusId = 49;

                if (obj.SkillId1 != null && obj.SkillId1.Length > 0)
                {
                    obj.Skill = string.Join(",", obj.SkillId1);
                }

                if (obj.ProjectCode1 != null && obj.ProjectCode1.Length > 0)
                {
                    obj.Projects = string.Join(",", obj.ProjectCode1);
                }

                if (obj.TechnolgyId1 != null && obj.TechnolgyId1.Length > 0)
                {
                    obj.Technologies = string.Join(",", obj.TechnolgyId1);
                }

                List<string> ResumeFilePath = new List<string>();

                foreach (var file in obj.Resume)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        try
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            var folderPath = Server.MapPath("~/Content/Placement/docs");
                            var path = Path.Combine(folderPath, fileName);

                            file.SaveAs(path);
                            ResumeFilePath.Add(path);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("Error saving file: " + ex.Message);
                        }
                    }
                }

                obj.ResumeFilePath = string.Join(",", ResumeFilePath);

                await objbal.UpdateResumePRAsync(obj);
                await objbal.SaveAssignSkillPRAsync(obj);
                await objbal.SaveAssignProjectPRAsync(obj);
                await objbal.UpdatePlacementStudentStatusPRAsync(obj);
                await Course_BindPRAsync();
                await Skill_BindPRAsync();
                await Project_BindPRAsync();
                await Company_BindPRAsync();
                await Induastry_BindPRAsync();
                await Designation_BindPRAsync();
                return View("RegistrationDetailsPRAsync", obj);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("An error occurred: " + ex.Message);
                return View("Error");
            }
        }

        /// <summary>
        /// Retrieves and displays registration details for a candidate asynchronously,
        /// returning the data as a JSON result.
        /// </summary>
        /// <param name="obj">The Placement object to store registration details.</param>
        /// <param name="CandidateCode">The candidate code for which details are retrieved.</param>
        /// <returns>A JSON result containing the registration details or an error message.</returns>
        [HttpGet]
        public async Task<JsonResult> RegistrationDetailsShowPRAsync(Placement obj, string CandidateCode)
        {
            try
            {
                obj.CandidateCode = CandidateCode;
                obj.BranchCode = Session["BranchCode"].ToString();
                SqlDataReader dr;
                dr = await objbal.RegistrationDetailsPRAsync(obj);
                while (dr.Read())
                {
                    obj.CandidateCode = dr["CandidateCode"].ToString();
                    obj.FirstName = dr["FirstName"].ToString();
                    obj.MiddleName = dr["MiddleName"].ToString();
                    obj.LastName = dr["LastName"].ToString();
                    obj.MobileNo = dr["MobileNo"].ToString();
                    obj.AlternateContact = dr["AlternateContact"].ToString();
                    obj.EmailId = dr["EmailId"].ToString();
                    obj.Gender = dr["Gender"].ToString();
                    obj.RegistrationDate = Convert.ToDateTime(dr["RegistrationDate"].ToString());
                    obj.FatherName = dr["FatherName"].ToString();
                    obj.ContactNo = dr["ContactNo"].ToString();
                    obj.MotherName = dr["MotherName"].ToString();
                    obj.MContactNo = dr["MContactNo"].ToString();
                    obj.LocalAddr = dr["LocalAddr"].ToString();
                    obj.PermanantAddress = dr["PermanantAddress"].ToString();
                    obj.CityName = dr["CityName"].ToString();
                    obj.Pin = dr["Pin"].ToString();
                    obj.SSCYear = dr["SSCYear"].ToString();
                    obj.HSCYear = dr["HSCYear"].ToString();
                    obj.DiplomaYear = dr["DiplomaYear"].ToString();
                    obj.GraduationYear = dr["GraduationYear"].ToString();
                    obj.PostGraduationYear = dr["PostGraduationYear"].ToString();
                    obj.CourseName = dr["CourseName"].ToString();
                    obj.Photograph = dr["Photograph"].ToString();
                    obj.AadharCard = dr["AadharCard"].ToString();
                    obj.PanCard = dr["PanCard"].ToString();

                }
                if (dr != null && !dr.IsClosed)
                {
                    dr.Close();
                }
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { error = "An error occurred while fetching registration details." }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Adds placement experiences for a list of students asynchronously.
        /// </summary>
        /// <param name="studentDetailsModels">List of Placement objects containing student details.</param>
        /// <returns>A JSON result indicating success or an error message.</returns>
        [HttpPost]
        public async Task<JsonResult> RegistrationDetailsShowPRAsync(List<Placement> studentDetailsModels)
        {
            try
            {
                foreach (var StudentDetails in studentDetailsModels)
                {
                    if (StudentDetails != null)
                    {
                        await objbal.AddExperiencePRAsync(new Placement
                        {
                            CandidateCode = StudentDetails.CandidateCode,
                            CompanyName = StudentDetails.CompanyName,
                            InduastryId = StudentDetails.InduastryId,
                            DesignationId = StudentDetails.DesignationId,
                            CTC = StudentDetails.CTC,
                            Experience = StudentDetails.Experience,
                            JobType = StudentDetails.JobType
                        });

                    }
                }
                return await Task.Run(() => Json(new { success = true, message = "Results saved successfully." }));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Fetches skills asynchronously and binds them to a dropdown.
        /// </summary>
        /// <returns>A JSON result containing the list of skills.</returns>
        public async Task<JsonResult> Skill_BindPRAsync()
        {
            try
            {
                DataSet ds = await objbal.FetchSkillPRAsync();
                List<SelectListItem> SkillList = new List<SelectListItem>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    SkillList.Add(new SelectListItem { Text = dr["SkillName"].ToString(), Value = dr["SkillId"].ToString() });
                }

                ViewBag.Skill = SkillList;

                return await Task.Run(() => Json(SkillList, JsonRequestBehavior.AllowGet));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in Skill_BindPRAsync: {ex.Message}");
                return Json(new { error = "An error occurred while fetching and binding skills." }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Fetches projects asynchronously and binds them to a dropdown.
        /// </summary>
        /// <returns>A JSON result containing the list of projects.</returns>
        public async Task<JsonResult> Project_BindPRAsync()
        {
            try
            {
                DataSet ds = await objbal.FetchProjectPRAsync();
                List<SelectListItem> ProjectList = new List<SelectListItem>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ProjectList.Add(new SelectListItem { Text = dr["ProjectName"].ToString(), Value = dr["ProjectCode"].ToString() });
                }
                ViewBag.Project = ProjectList;
                return await Task.Run(() => Json(ProjectList, JsonRequestBehavior.AllowGet));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in Project_BindPRAsync: {ex.Message}");
                return Json(new { error = "An error occurred while fetching and binding projects." }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Fetches companies asynchronously and binds them to a dropdown.
        /// </summary>
        /// <returns>A JSON result containing the list of companies.</returns>
        public async Task<JsonResult> Company_BindPRAsync()
        {
            try
            {
                DataSet ds = await objbal.FetchCompanyPRAsync();
                List<SelectListItem> CompanyList = new List<SelectListItem>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    CompanyList.Add(new SelectListItem { Text = dr["CompanyName"].ToString(), Value = dr["CompanyId"].ToString() });
                }
                ViewBag.Company = CompanyList;
                return await Task.Run(() => Json(CompanyList, JsonRequestBehavior.AllowGet));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in Company_BindPRAsync: {ex.Message}");
                return Json(new { error = "An error occurred while fetching and binding companies." }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Fetches industries asynchronously and binds them to a dropdown.
        /// </summary>
        /// <returns>A JSON result containing the list of industries.</returns>
        public async Task<JsonResult> Induastry_BindPRAsync()
        {
            try
            {
                DataSet ds = await objbal.FetchInduastryPRAsync();
                List<SelectListItem> InduastryList = new List<SelectListItem>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    InduastryList.Add(new SelectListItem { Text = dr["InduastryName"].ToString(), Value = dr["InduastryId"].ToString() });
                }
                ViewBag.Induastry = InduastryList;
                return await Task.Run(() => Json(InduastryList, JsonRequestBehavior.AllowGet));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in Induastry_BindPRAsync: {ex.Message}");
                return Json(new { error = "An error occurred while fetching and binding industries." }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Fetches designations asynchronously and binds them to a dropdown.
        /// </summary>
        /// <returns>A JSON result containing the list of designations.</returns>
        public async Task<JsonResult> Designation_BindPRAsync()
        {
            try
            {
                DataSet ds = await objbal.FetchDesignationPRAsync();
                List<SelectListItem> DesignationList = new List<SelectListItem>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    DesignationList.Add(new SelectListItem { Text = dr["DesignationName"].ToString(), Value = dr["DesignationId"].ToString() });
                }
                ViewBag.Designation = DesignationList;
                return await Task.Run(() => Json(DesignationList, JsonRequestBehavior.AllowGet));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in Designation_BindPRAsync: {ex.Message}");
                return Json(new { error = "An error occurred while fetching and binding designations." }, JsonRequestBehavior.AllowGet);
            }
        }

        //------------------------------------------------END Pratiksha(StudentList)------------------------------------------------

    }
}