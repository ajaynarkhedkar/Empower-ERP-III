using GSTEducationERPLibrary.Bind;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Net.Http;



namespace GSTEducationERP.Controllers
{
    public class BindController : Controller
    {
        private readonly BALBind BALObj;

        public BindController()
        {
            BALObj = new BALBind();
        }
        public ActionResult Index()
        {
            return View();
        }
        public class BreadcrumbItem
        {
            public string Name { get; set; }
            public string Url { get; set; }
        }
        public async Task AllCourseBind()
        {
            Bind obj = new Bind();
            obj.BranchCode = Session["BranchCode"].ToString();
            DataSet ds1 = new DataSet();
            ds1 = await BALObj.AllCourseBind(obj);
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
        [HttpGet]
        public async Task<ActionResult> DetailStaffProfile()
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                Bind ObjCo = new Bind();
                ObjCo.BranchCode = Session["BranchCode"].ToString();
                ObjCo.StaffCode = Session["StaffCode"].ToString();
                SqlDataReader dr;
                dr = await BALObj.StaffProfileAsync(ObjCo);
                while (dr.Read())
                {
                    ObjCo.StaffName = dr["StaffName"].ToString();
                    ObjCo.Photograph = dr["Photograph"].ToString();
                    ObjCo.StaffPosition = dr["StaffPosition"].ToString();
                    ObjCo.DesignationCurrent = dr["DesignationCurrent"].ToString();
                    ObjCo.DepartmentNameCurrent = dr["DepartmentNameCurrent"].ToString();
                    ObjCo.JoiningDate = dr["JoiningDate"].ToString();
                    ObjCo.GraduationName = dr["GraduationName"].ToString();
                    ObjCo.PostGraduationName = dr["PostGraduationName"].ToString();
                    ObjCo.BranchName = dr["BranchName"].ToString();
                    ObjCo.currentCity = dr["currentCity"].ToString();
                    ObjCo.SkillNames = dr["SkillNames"].ToString();

                }
                dr.Close();
                string Staffpostion = Session["StaffPositionId"].ToString();
                if (Staffpostion == "3")
                {
                    List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                    {
                    new BreadcrumbItem { Name = "TrainerDashboard", Url =Url.Action("TrainerDashboardAsyncTS","Trainer")},
                    new BreadcrumbItem { Name = "Profile", Url =  Url.Action("DetailStaffProfile")}
                    };

                    ViewBag.Breadcrumb = breadcrumbs;
                }
                else
                {
                    List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                    {
                    new BreadcrumbItem { Name = "CoordinatorDashboard", Url = Url.Action("CoordinatorDashboardPRAsync","Coordinator")},
                    new BreadcrumbItem { Name = "Profile", Url =  Url.Action("DetailStaffProfile")}
                    };

                    ViewBag.Breadcrumb = breadcrumbs;

                }
                return await Task.Run(() => View("DetailStaffProfile", ObjCo));
            }
        }

        public async Task<ActionResult> DetailsStaffProfileAsync()
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));

            }
            else
            {
                Bind ObjCo = new Bind();
                ObjCo.BranchCode = Session["BranchCode"].ToString();
                ObjCo.StaffCode = Session["StaffCode"].ToString();
                SqlDataReader dr;
                dr = await BALObj.StaffProfileAsync(ObjCo);
                while (dr.Read())
                {
                    ObjCo.StaffName = dr["StaffName"].ToString();
                    ObjCo.StaffPosition = dr["StaffPosition"].ToString();
                    ObjCo.CourseName = dr["CourseName"].ToString();
                    ObjCo.CourseType = dr["CourseType"].ToString();
                    ObjCo.DOB = dr["DOB"].ToString();
                    ObjCo.PhoneNo = dr["PhoneNo"].ToString();
                    ObjCo.EMail = dr["Email"].ToString();
                    ObjCo.CurrentAddress = dr["CurrentAddress"].ToString();
                    ObjCo.currentCity = dr["currentCity"].ToString();
                    ObjCo.CurrentPinCode = dr["CurrentPinCode"].ToString();
                    ObjCo.PermanentAddress = dr["PermanentAddress"].ToString();
                    ObjCo.PermanentCity = dr["PermanentCity"].ToString();
                    ObjCo.PermanentPinCode = dr["PermanentPinCode"].ToString();
                    ObjCo.MaritalStatus = dr["MaritalStatus"].ToString();
                    ObjCo.Nationality = dr["Nationality"].ToString();
                    ObjCo.BloodGroup = dr["BloodGroup"].ToString();
                    ObjCo.Gender = dr["Gender"].ToString();
                    ObjCo.AadharCardNo = dr["AadharCardNo"].ToString();
                    ObjCo.PanCardNo = dr["PanCardNo"].ToString();
                    ObjCo.EmergencyContactName = dr["EmergencyContactName"].ToString();
                    ObjCo.EmergencyContactNo = dr["EmergencyContactNo"].ToString();
                    ObjCo.EmergencyContactAddress = dr["EmergencyContactAddress"].ToString();
                    ObjCo.FatherName = dr["FatherName"].ToString();
                    ObjCo.FatherContactNo = dr["FatherContactNo"].ToString();
                    ObjCo.MotherName = dr["MotherName"].ToString();
                    ObjCo.MotherContactNo = dr["MotherContactNo"].ToString();
                    ObjCo.BankName = dr["BankName"].ToString();
                    ObjCo.AccountHolderName = dr["AccountHolderName"].ToString();
                    ObjCo.AccountNumber = dr["AccountNumber"].ToString();
                    ObjCo.BankBranchName = dr["BankBranchName"].ToString();
                    ObjCo.IFSCCode = dr["IFSCCode"].ToString();
                    ObjCo.MICRCode = dr["MICRCode"].ToString();
                    ObjCo.JoiningDate = dr["JoiningDate"].ToString();
                    ObjCo.OfficialEmailId = dr["OfficialEmailId"].ToString();
                    ObjCo.Password = dr["Password"].ToString();
                    ObjCo.SSCYear = dr["SSCYear"].ToString();
                    ObjCo.HSCYear = dr["HSCYear"].ToString();
                    ObjCo.DiplomaYear = dr["DiplomaYear"].ToString();
                    ObjCo.GraduationName = dr["GraduationName"].ToString();
                    ObjCo.GraduationYear = dr["GraduationYear"].ToString();
                    ObjCo.PostGraduationName = dr["PostGraduationName"].ToString();
                    ObjCo.PostGraduationYear = dr["PostGraduationYear"].ToString();
                    ObjCo.IndustryName = dr["InduastryName"].ToString();
                    ObjCo.DesignationName = dr["DesignationName"].ToString();
                    ObjCo.DepartmentName = dr["DepartmentName"].ToString();
                    ObjCo.CTC = dr["CTC"].ToString();
                    ObjCo.Experience = dr["Experience"].ToString();
                    ObjCo.TotalExperienceOfDesignation = dr["TotalExperienceOfDesignation"].ToString();
                    ObjCo.JobType = dr["JobType"].ToString();
                    ObjCo.CompanyName = dr["CompanyName"].ToString();
                    ObjCo.Photograph = dr["Photograph"].ToString();
                    ObjCo.BranchName = dr["BranchName"].ToString();
                    ObjCo.DepartmentNameCurrent = dr["DepartmentNameCurrent"].ToString();
                    ObjCo.DesignationCurrent = dr["DesignationCurrent"].ToString();
                    ObjCo.SkillNames = dr["SkillNames"].ToString();


                }
                dr.Close();
                return await Task.Run(() => PartialView(ObjCo));
            }
        }
        public async Task<ActionResult> DetailsStaffDocumetAsync()
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));

            }
            else
            {
                Bind ObjCo = new Bind();
                ObjCo.StaffCode = Session["StaffCode"].ToString();
                SqlDataReader dr;
                dr = await BALObj.StaffDocumetAsync(ObjCo);
                while (dr.Read())
                {
                    ObjCo.PanCard = dr["PanCard"].ToString();
                    ObjCo.AadharCard = dr["AadharCard"].ToString();
                    ObjCo.SSC = dr["SSC"].ToString();
                    ObjCo.HSC = dr["HSC"].ToString();
                    ObjCo.Diploma = dr["Diploma"].ToString();
                    ObjCo.Graduation = dr["Graduation"].ToString();
                    ObjCo.PostGraduation = dr["PostGraduation"].ToString();
                    ObjCo.RelievingLetter = dr["RelievingLetter"].ToString();
                    ObjCo.SalarySlip = dr["SalarySlip"].ToString();
                    ObjCo.SalaryStructure = dr["SalaryStructure"].ToString();
                    ObjCo.JoiningLetter = dr["JoiningLetter"].ToString();
                    ObjCo.ExperienceLetter = dr["ExperienceLetter"].ToString();
                    ObjCo.MedicalCertificate = dr["MedicalCertificate"].ToString();
                    ObjCo.Photograph = dr["Photograph"].ToString();
                    ObjCo.Resume = dr["Resume"].ToString();

                }
                dr.Close();
                return await Task.Run(() => PartialView(ObjCo));
            }
        }



        public async Task<ActionResult> BatchScheduleMainViewAsyncST()
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                return await Task.Run(() => View());
            }
        }


        [HttpGet]
        public async Task<ActionResult> ListAllBatchAsyncST(string CourseCode)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await GetCourceAsyncST(); // Pass CourseCode to GetCourceAsyncST
                Bind ObjCo = new Bind();
                ObjCo.CourseCode = CourseCode;
                ObjCo.BranchCode = Session["BranchCode"].ToString();

                DataSet ds = new DataSet();
                //if (CourseCode != null)
                //{
                //    ds = await BALObj.GetDataBatchCourseWiseAsyncST(ObjCo); // Pass CourseCode to GetDataBatchCourseWiseAsyncST
                //}
                //else
                //{
                ds = await BALObj.GetDataBatchAsyncST(ObjCo); // Pass CourseCode to GetDataBatchAsyncST
                                                              //}

                List<Bind> lstBatchData1 = new List<Bind>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Bind coObj = new Bind();
                    coObj.BatchCode = ds.Tables[0].Rows[i]["BatchCode"].ToString();
                    coObj.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    coObj.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    coObj.NoOfStudent = Convert.ToInt32(ds.Tables[0].Rows[i]["NoOfStudent"].ToString());
                    coObj.CreateDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreateDate"].ToString());
                    coObj.Status = ds.Tables[0].Rows[i]["Status"].ToString();
                    coObj.StaffName = ds.Tables[0].Rows[i]["StaffName"].ToString();

                    lstBatchData1.Add(coObj);
                }
                ObjCo.lstBatchData = lstBatchData1;

                string Staffpostion = Session["StaffPositionId"].ToString();
                if (Staffpostion == "3")
                {
                    List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
     {
         new BreadcrumbItem { Name = "TrainerDashboard", Url =Url.Action("TrainerDashboardAsyncTS","Trainer")},
         new BreadcrumbItem { Name = "ALLBatch", Url =  Url.Action("ListAllBatchAsyncST")}
     };

                    ViewBag.Breadcrumb = breadcrumbs;
                }
                else
                {
                    List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
     {
         new BreadcrumbItem { Name = "CoordinatorDashboard", Url = Url.Action("CoordinatorDashboardPRAsync","Coordinator")},
         new BreadcrumbItem { Name = "ALLBatch", Url =  Url.Action("ListAllBatchAsyncST")}
     };

                    ViewBag.Breadcrumb = breadcrumbs;
                }
                return View(ObjCo);
            }
        }
        public async Task<string> BatchAutoCodeAsyncST()
        {
            Bind ObjCo = new Bind();
            SqlDataReader dr;
            dr = await BALObj.BatchAutoCodeAsyncST();
            int maxId = 0;
            while (dr.Read())
            {
                int currentId = int.Parse(dr["BatchId"].ToString());
                if (currentId > maxId)
                {
                    maxId = currentId;
                }
            }
            dr.Close();
            int nextId = maxId + 1;
            string batchcode = "BTH" + nextId.ToString("000");
            ViewBag.batchcode = batchcode;
            TempData["BatchCode"] = batchcode;
            return batchcode;
        }

        /// <summary>
        /// This JsonResult get all Cource data  on list.
        /// </summary>
        /// <returns> Cource list</returns>
        public async Task<JsonResult> GetCourceAsyncST()
        {
            Bind ObjCo = new Bind();
            DataSet ds = new DataSet();
            ObjCo.BranchCode = Session["BranchCode"].ToString();
            ds = await BALObj.GetCourceAsyncST(ObjCo);
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
        [HttpGet]
        /// <summary>
        /// This method get all Admitted Student  on list.
        /// </summary>
        /// <returns> Admitted Status Student List</returns>
        public async Task<ActionResult> GetAdmittedStudentAsyncST(String CourseCode)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Bind ObjCo = new Bind();
                ObjCo.CourseCode = CourseCode;
                ObjCo.BranchCode = Session["BranchCode"].ToString();
                DataSet ds = new DataSet();
                ds = await BALObj.GetAdmittedStudentAsyncST(ObjCo);
                List<SelectListItem> StudentList = new List<SelectListItem>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    StudentList.Add(new SelectListItem
                    {
                        Text = dr["FullName"].ToString(),
                        Value = dr["CandidateCode"].ToString()
                    });
                }
                ViewBag.AdmittedStudent = StudentList;
                return Json(StudentList, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///  This view use to save new created batch.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> RegisterBatchAsyncST()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await GetCourceAsyncST();
                await BatchAutoCodeAsyncST();
                return PartialView();
            }
        }
        [HttpPost]
        public async Task<ActionResult> RegisterBatchAsyncST(string BatchName, string CourseCode, string StudentCode, string NoOfStudent)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Bind ObjCo = new Bind();
                ObjCo.StaffCode = Session["StaffCode"].ToString();

                ObjCo.CreateDate = DateTime.Now;
                ObjCo.BatchName = BatchName;
                ObjCo.CourseCode = CourseCode;
                ObjCo.StudentCode = StudentCode.TrimEnd(',');
                ObjCo.NoOfStudent = Convert.ToInt32(NoOfStudent.ToString());
                ObjCo.StatusId = 44;
                ObjCo.TypeId = 5;
                if (TempData.ContainsKey("BatchCode"))
                    ObjCo.BatchCode = TempData["BatchCode"].ToString();
                await BALObj.AddBatchAsyncST(ObjCo);
                return Json(new { success = true, message = "Batch has been create successfully!" });

            }
        }
        /// <summary>
        ///  This method use to Validation For new created batch Name.
        /// </summary>
        [HttpPost]
        public async Task<JsonResult> IsBatchAvilableAsyncST(string BatchName)
        {
            bool isAvailable = await BALObj.IsBatchAvilableAsyncST(BatchName);
            return Json(new { isAvailable });      // Return a JsonResult indicating whether the exam is available
        }

        [HttpGet]
        public async Task<ActionResult> UpdateBatchAsyncST(String batchcode)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await GetCourceAsyncST();
                Bind ObjCo = new Bind();
                ObjCo.BatchCode = batchcode;
                ObjCo.BranchCode = Session["BranchCode"].ToString();
                SqlDataReader dr;
                dr = await BALObj.GetUpdateBatchAsyncST(ObjCo);
                while (dr.Read())
                {
                    ObjCo.BatchCode = dr["BatchCode"].ToString();
                    ObjCo.BatchName = dr["BatchName"].ToString();
                    ObjCo.CourseName = dr["CourseName"].ToString();
                    ObjCo.CourseCode = dr["Coursecode"].ToString();
                }
                dr.Close();
                return PartialView("UpdateBatchAsyncST", ObjCo);
            }
        }
        /// <summary>
        /// This view use to save Updated Batch Data.
        /// </summary>
        /// <returns> </returns>
        [HttpPost]
        public async Task<ActionResult> UpdateBatchAsyncST(string BatchCode, string BatchName, string StudentCode, string CourseCode, string NoOfStudent)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                if (StudentCode != null && NoOfStudent != null)
                {
                    Bind ObjCo = new Bind();
                    ObjCo.BatchCode = BatchCode;
                    ObjCo.BatchName = BatchName;
                    ObjCo.CourseCode = CourseCode;
                    ObjCo.StudentCode = StudentCode.TrimEnd(',');
                    ObjCo.NoOfStudent = Convert.ToInt32(NoOfStudent.ToString());
                    ObjCo.StaffCode = Session["StaffCode"].ToString();

                    await BALObj.UpdateBatchDataAsyncST(ObjCo);
                    return Json(new { success = true, message = "Batch has been Updated successfully!" });
                }
                else
                {
                    return RedirectToAction("ListAllBatchAsyncST", "Bind");
                }
            }
        }

        /// <summary>
        ///This view use to get batch data for View Only.
        /// </summary>
        /// <returns> Batch Data</returns>
        [HttpGet]
        public async Task<ActionResult> DetailsBatchAsyncST(String BatchCode)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Bind ObjCo = new Bind();
                ObjCo.BatchCode = BatchCode;
                ObjCo.BranchCode = Session["BranchCode"].ToString();
                SqlDataReader dr;
                dr = await BALObj.DetailsBatchAsyncST(ObjCo);
                while (dr.Read())
                {
                    ObjCo.BatchCode = dr["BatchCode"].ToString();
                    ObjCo.BatchName = dr["BatchName"].ToString();
                    ObjCo.CourseName = dr["CourseName"].ToString();
                    ObjCo.CreateDate = Convert.ToDateTime(dr["CreateDate"].ToString());
                    ObjCo.NoOfStudent = Convert.ToInt32(dr["NoOfStudent"].ToString());
                    ObjCo.StaffName = dr["StaffName"].ToString();
                    ObjCo.Status = dr["Status"].ToString();
                }
                dr.Close();
                return PartialView("DetailsBatchAsyncST", ObjCo);
            }
        }

        /// <summary>
        ///This view show batch student info for click on Number Of Student Count.
        /// </summary>
        /// <returns> student info </returns>
        [HttpGet]
        public async Task<ActionResult> BatchAllStudentDetailsAsyncST(string batchcode)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Bind studs = new Bind();
                studs.BatchCode = batchcode;
                studs.BranchCode = Session["BranchCode"].ToString();
                BALBind objnooflist = new BALBind();
                DataSet ds;
                ds = await objnooflist.GetBatchStudNameAsyncST(studs);
                Bind nooflist = new Bind();
                nooflist.BatchCode = batchcode;
                List<Bind> lstnoofstud = new List<Bind>();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Bind objnoof = new Bind();
                    objnoof.StudentCode = ds.Tables[0].Rows[i]["value"].ToString();
                    objnoof.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                    objnoof.ContactNo = ds.Tables[0].Rows[i]["ContactNumber"].ToString();
                    objnoof.EMail = ds.Tables[0].Rows[i]["EmailId"].ToString();
                    lstnoofstud.Add(objnoof);
                }
                nooflist.lstBatchStudentList = lstnoofstud;
                return PartialView("BatchAllStudentDetailsAsyncST", nooflist);
            }

        }
        //------------------------------------Marge Batch----------------------------------------
        /// <summary>
        ///This view use to save select marge batch student.
        ///This view use to save add marge batch student.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> MergeBatchStudentAsyncST()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await GetSelectedBatchAsyncST();
                string Staffpostion = Session["StaffPositionId"].ToString();
                if (Staffpostion == "3")
                {
                    List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
             {
             new BreadcrumbItem { Name = "TrainerDashboard", Url =Url.Action("TrainerDashboardAsyncTS","Trainer")},
             new BreadcrumbItem { Name = "ALLBatch", Url =  Url.Action("ListAllBatchAsyncST", "Bind")},
             new BreadcrumbItem { Name = "MergeBatch", Url =  Url.Action("MergeBatchStudentAsyncST")}
             };
                    ViewBag.Breadcrumb = breadcrumbs;
                }
                else
                {
                    List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
             {
              new BreadcrumbItem { Name = "CoordinatorDashboard", Url = Url.Action("CoordinatorDashboardPRAsync","Coordinator")},
             new BreadcrumbItem { Name = "ALLBatch", Url =  Url.Action("ListAllBatchAsyncST", "Bind")},
             new BreadcrumbItem { Name = "MergeBatch", Url =  Url.Action("MergeBatchStudentAsyncST")}
             };
                    ViewBag.Breadcrumb = breadcrumbs;

                }

                return View();
            }
        }

        [HttpPost]
        public async Task<ActionResult> MergeBatchStudentAsyncST(String SelectBatch, String SelectStudentCode, String SelectNoOfStudent, String AddBatch, String AddStudentCode, String AddNoOfStudent)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                if (!string.IsNullOrEmpty(SelectStudentCode) && !string.IsNullOrEmpty(SelectNoOfStudent) && !string.IsNullOrEmpty(AddStudentCode) && !string.IsNullOrEmpty(AddNoOfStudent))
                {
                    Bind ObjCo = new Bind();
                    ObjCo.SelectBatch = SelectBatch;
                    ObjCo.SelectStudentCode = SelectStudentCode.TrimEnd(',');

                    // Validate and convert SelectNoOfStudent
                    if (int.TryParse(SelectNoOfStudent, out int selectNo))
                    {
                        ObjCo.SelectNoOfStudent = selectNo;
                    }
                    else
                    {
                        return Json(new { success = false, message = "Invalid value for SelectNoOfStudent." });
                    }

                    ObjCo.AddBatch = AddBatch;
                    ObjCo.AddStudentCode = AddStudentCode.TrimEnd(',');

                    // Validate and convert AddNoOfStudent
                    if (int.TryParse(AddNoOfStudent, out int addNo))
                    {
                        ObjCo.AddNoOfStudent = addNo;
                    }
                    else
                    {
                        return Json(new { success = false, message = "Invalid value for AddNoOfStudent." });
                    }
                    await BALObj.MergeBatchStudentAsyncST(ObjCo);
                    await BALObj.MergeBatchStudent2AsyncST(ObjCo);
                    return Json(new { success = true, message = "Batch Student has been merged successfully!" });
                }
                else
                {
                    return RedirectToAction("MergeBatchStudentAsyncST", "Bind");
                }
            }
        }

        /// <summary>
        ///This JsonResult method use to get batch names To selected batches on marge student view. 
        /// </summary>
        /// <returns> Batch List</returns>
        [HttpGet]
        public async Task<JsonResult> GetSelectedBatchAsyncST()
        {
            DataSet ds = new DataSet();
            ds = await BALObj.GetSelectedBatchAsyncST();
            List<SelectListItem> BatchList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                BatchList.Add(new SelectListItem
                {
                    Text = dr["BatchName"].ToString(),
                    Value = dr["Batchcode"].ToString()
                });
            }
            ViewBag.Batch = BatchList;
            return Json(BatchList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///This JsonResult method use to batch select validation  on marge student view.
        /// </summary>
        /// <returns> </returns>
        [HttpPost]
        public async Task<JsonResult> ValidationBatchAsyncST(string SelectBatchCode)
        {
            Bind ObjCo = new Bind();
            ObjCo.SelectBatch = SelectBatchCode;
            DataSet ds = new DataSet();
            ds = await BALObj.ValidationBatchAsyncST(ObjCo);
            List<SelectListItem> BatchList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                BatchList.Add(new SelectListItem
                {
                    Text = dr["BatchName"].ToString(),
                    Value = dr["Batchcode"].ToString()
                });
            }
            return Json(BatchList, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// This JsonResult method use to get selected batch student names on marge student view.
        /// </summary>
        /// <returns> Student Name </returns>

        [HttpGet]
        public async Task<JsonResult> SelectBatchStudNameAsyncST(Bind ObjCo)
        {
            Bind ObjCo1 = new Bind();
            ObjCo1.SelectBatch = ObjCo.SelectBatch;
            ObjCo1.BranchCode = Session["BranchCode"].ToString();
            DataSet ds = new DataSet();

            ds = await BALObj.SelectBatchStudNameAsyncST(ObjCo1);
            List<SelectListItem> StudentList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                StudentList.Add(new SelectListItem
                {
                    Text = dr["FullName"].ToString(),
                    Value = dr["value"].ToString()
                });

            }
            ViewBag.SelectStudent = StudentList;

            return Json(StudentList, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// This JsonResult method use to get Add batch student names on marge student view.
        /// </summary>
        /// <returns> Batch Name </returns>
        [HttpGet]
        public async Task<JsonResult> AddBatchStudNameAsyncST(String AddBatch)
        {
            Bind ObjCo = new Bind();
            DataSet ds = new DataSet();
            ObjCo.AddBatch = AddBatch;
            ObjCo.BranchCode = Session["BranchCode"].ToString();
            ds = await BALObj.AddBatchStudNameAsyncST(ObjCo);
            List<SelectListItem> StudentList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                StudentList.Add(new SelectListItem
                {
                    Text = dr["FullName"].ToString(),
                    Value = dr["value"].ToString()
                });
            }
            ViewBag.AddStudent = StudentList;
            return Json(StudentList, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        ///This view use to save marge batch to batch Student.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> MergeBatchToBatchStudentAsyncST()
        {

            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await GetSelectedBatchAsyncST();
                return PartialView();
            }
        }
        [HttpPost]
        public async Task<ActionResult> MergeBatchToBatchStudentAsyncST(Bind ObjCo)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await BALObj.MergeBatchToBatchStudentAsyncST(ObjCo);
                return Json(new { success = true, message = "Batch has been Merge successfully!" });
            }
        }

        /// <summary>
        /// This method get CourseDuration On Creted new batch Schedule.
        /// </summary>
        /// <returns> Batch list</returns>
        [HttpGet]
        public async Task<JsonResult> ValidationStudCountLabCapacityAsyncST(string batchcode)
        {
            Bind ObjCo = new Bind();
            ObjCo.BatchCode = batchcode;
            ObjCo.BranchCode = Session["BranchCode"].ToString();
            DataSet ds = new DataSet();
            ds = await BALObj.ValidationStudCountLabCapacityAsyncST(ObjCo);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Bind coObj = new Bind();
                coObj.NoOfStudent = Convert.ToInt32(ds.Tables[0].Rows[i]["NoOfStudent"].ToString());
                coObj.LabCapacity = Convert.ToInt32(ds.Tables[0].Rows[i]["LabCapacity"].ToString());
                ObjCo = coObj;
            }
            return Json(ObjCo, JsonRequestBehavior.AllowGet);
        }


        // shuhangi -------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Show the list of Student.
        /// </summary>
        /// <param name="id"> This id for the show student list statuswise.</param>
        /// <returns>Show the list of students.</returns>
        [HttpGet]
        public async Task<ActionResult> ListStudent()
        {
            await AllCourseBind();
            string Staffpostion = Session["StaffPositionId"].ToString();
            if (Staffpostion == "3")
            {
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                new BreadcrumbItem { Name = "TrainerDashboard", Url =Url.Action("TrainerDashboardAsyncTS","Trainer")},
                new BreadcrumbItem { Name = "Student", Url = Url.Action("ListStudent")},
                };
                ViewBag.Breadcrumb = breadcrumbs;
            }
            else
            {
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                 new BreadcrumbItem { Name = "CoordinatorDashboard", Url = Url.Action("CoordinatorDashboardPRAsync","Coordinator")},
                new BreadcrumbItem { Name = "Student", Url = Url.Action("ListStudent")},
                };
                ViewBag.Breadcrumb = breadcrumbs;

            }


            return await Task.Run(() => View("ListStudent"));

        }
        /// <summary>
        /// Show the list of active student.
        /// </summary>
        /// <param name="id"> This id for the show student list statuswise.</param>
        /// <returns>Show the list of students.</returns>
        [HttpGet]
        public async Task<ActionResult> ListStudentSHAsync(int id, string CourseCode)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                List<Bind> model = await ListStudShAsync(id, CourseCode);
                Bind objlist = new Bind();
                objlist.StudentList = model;
                return PartialView("ListStudentSHAsync", objlist);
            }
        }
        /// <summary>
        /// Show the list of relese student.
        /// </summary>
        /// <param name="id"> This id for the show student list statuswise.</param>
        /// <returns>Show the list of students.</returns>
        [HttpGet]
        public async Task<ActionResult> ListReleseStudentSHAsync(string CourseCode)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                string Staffpostion = Session["StaffPositionId"].ToString();
                if (Staffpostion == "3")
                {
                    int id = 54;

                    List<Bind> model = await ListStudShAsync(id, CourseCode);
                    Bind objlist = new Bind();
                    objlist.StudentList = model;
                    return PartialView("ListStudentSHAsync", objlist);

                }
                else
                {
                    int id = 11;
                    List<Bind> model = await ListStudShAsync(id, CourseCode);
                    Bind objlist = new Bind();
                    objlist.StudentList = model;
                    return PartialView("ListStudentSHAsync", objlist);


                }


            }
        }
        /// <summary>
        /// Show the list of hold student.
        /// </summary>
        /// <param name="id"> This id for the show student list statuswise.</param>
        /// <returns>Show the list of students.</returns>
        [HttpGet]
        public async Task<ActionResult> ListHoldStudentSHAsync(int id, string CourseCode)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                List<Bind> model = await ListStudShAsync(id, CourseCode);
                Bind objlist = new Bind();
                objlist.StudentList = model;
                return PartialView("ListStudentSHAsync", objlist);
            }
        }
        [HttpGet]
        /// <summary>
        /// Show the list of dropout student.
        /// </summary>
        /// <param name="id"> This id for the show student list statuswise.</param>
        /// <returns>Show the list of students.</returns>
        public async Task<ActionResult> ListDropOutStudentSHAsync(int id, string CourseCode)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                List<Bind> model = await ListStudShAsync(id, CourseCode);
                Bind objlist = new Bind();
                objlist.StudentList = model;
                return PartialView("ListStudentSHAsync", objlist);
            }
        }
        [HttpGet]
        /// <summary>
        /// This get the list of student.
        /// </summary>
        /// <param name="Statusid">This object for the to get student list statuswise.</param>
        /// <returns>Returns list of student.</returns>
        private async Task<List<Bind>> ListStudShAsync(int Statusid, string CourseCode)
        {
            string BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await BALObj.StudentListSHAsync(Statusid, BranchCode, CourseCode);
            List<Bind> lstStudent = new List<Bind>();

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Bind objS = new Bind();
                    objS.StudentCode = ds.Tables[0].Rows[i]["CandidateCode"].ToString();
                    objS.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    objS.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                    objS.ContactNumber = ds.Tables[0].Rows[i]["ContactNumber"].ToString();
                    objS.EmailId = ds.Tables[0].Rows[i]["EmailId"].ToString();
                    objS.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    DateTime Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["AdmmissionDate"].ToString());
                    objS.MDate = Date.ToString("dd-MM-yyyy");
                    objS.StatusId = Convert.ToInt32(ds.Tables[0].Rows[i]["StatusId"].ToString());
                    lstStudent.Add(objS);
                }
            }
            return lstStudent;
        }
        [HttpGet]
        /// <summary>
        /// Show list placed student.
        /// </summary>
        /// <param name="id">Id to get student list.</param>
        /// <returns> List of placed students.</returns>
        public async Task<ActionResult> ListPlacedStudentSHAsync(int id, string CourseCode)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                List<Bind> model = await ListPlacedStudShAsync(id, CourseCode);
                Bind objlist = new Bind();
                objlist.StudentList = model;
                return PartialView("ListPlacedStudentSHAsync", objlist);
            }
        }
        /// <summary>
        /// This get the list of placed student.
        /// </summary>
        /// <param name="Statusid">This object for the to get student list of placed student.</param>
        /// <returns>Returns list of placed student.</returns>
        private async Task<List<Bind>> ListPlacedStudShAsync(int Statusid, string CourseCode)
        {
            string BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await BALObj.PlacedStudentListSHAsync(Statusid, BranchCode, CourseCode);
            List<Bind> lstStudent = new List<Bind>();

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Bind objS = new Bind();
                    objS.StudentCode = ds.Tables[0].Rows[i]["CandidateCode"].ToString();
                    objS.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                    objS.ContactNumber = ds.Tables[0].Rows[i]["ContactNumber"].ToString();
                    objS.EmailId = ds.Tables[0].Rows[i]["EmailId"].ToString();
                    objS.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objS.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    objS.CompanyName = ds.Tables[0].Rows[i]["CompanyName"].ToString();
                    objS.Designation = ds.Tables[0].Rows[i]["DesignationName"].ToString();
                    //objS.Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["AdmmissionDate"].ToString());
                    lstStudent.Add(objS);
                }
            }
            return lstStudent;
        }
        /// <summary>
        /// Get details for update the student status.
        /// </summary>
        /// <param name="StudentCode"> Object for get the details..</param>
        /// <returns>It returns the detils if student for update.</returns>
        [HttpGet]
        public async Task<ActionResult> UpdateStudStatusSHAsync(string StudentCode)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Bind obj = new Bind();
                obj.StudentCode = StudentCode;
                DataSet ds = await BALObj.StatusChangeDataSHAsync(StudentCode);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    obj.StudentCode = ds.Tables[0].Rows[i]["CandidateCode"].ToString();
                    obj.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    obj.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    obj.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                    obj.StatusId = Convert.ToInt32(ds.Tables[0].Rows[i]["Statusid"].ToString());
                }
                DataSet ds1 = await BALObj.StatusListSHAsync();
                List<SelectListItem> AllStatus = new List<SelectListItem>();
                foreach (DataRow dr1 in ds1.Tables[0].Rows)
                {

                    AllStatus.Add(new SelectListItem { Text = dr1["Status"].ToString(), Value = dr1["StatusId"].ToString() });

                }
                ViewBag.StatusList = AllStatus;

                return PartialView("UpdateStudStatusSHAsync", obj);
            }
        }

        /// <summary>
        /// Update the student details.
        /// </summary>
        /// <param name="obj"> Object for the update the student status.</param>
        /// <returns>It update the student status.</returns>
        [HttpPost]
        public async Task<ActionResult> UpdateStudStatusSHAsync(Bind obj)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await BALObj.ChangeStatusSHAsync(obj);
                return RedirectToAction("ListStudent");
            }
        }
        [HttpGet]
        public async Task<ActionResult> ListReleaseStudentSHAsync(string StudentCode)
        {
            Bind obj = new Bind();
            obj.StudentCode = StudentCode;
            string BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await BALObj.ReleaseStudentDetailsSHAsync(StudentCode, BranchCode);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                obj.StudentCode = ds.Tables[0].Rows[i]["CandidateCode"].ToString();
                obj.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                obj.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                obj.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                obj.StatusId = Convert.ToInt32(ds.Tables[0].Rows[i]["Performance"].ToString());
            }
            return PartialView("ListReleaseStudentSHAsync", obj);
        }
        [HttpPost]
        public async Task<ActionResult> ListReleaseStudentSHAsync(Bind obj)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await BALObj.ReleaseStudentSHAsync(obj);
                return RedirectToAction("ListStudent");
            }
        }

        //shubhangi student list end -----------------------------------------------------------------
        //--------- ---------- pratiha task ----------------------------------------
        //--------- ---------- pratiha task ----------------------------------------
        public class ErrorViewModel
        {
            public string ErrorMessage { get; set; }
        }
        //=================================================== Task Management =================================================//
        /// <summary>
        /// Retrieves a list of task management details asynchronously.
        /// </summary>
        /// <returns>An asynchronous ActionResult representing the list of task management details.</returns>
        [HttpGet]
        public async Task<ActionResult> ListTaskManagementAsyncPG(DateTime? fromDate = null, DateTime? toDate = null)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                try
                {
                    Bind objT = new Bind();
                    objT.StaffCode = Session["StaffCode"].ToString();
                    objT.BranchCode = Session["BranchCode"].ToString();
                    DataSet ds = await BALObj.ListTaskManagementAsyncPG(objT, fromDate, toDate);
                    List<Bind> LstTaskManagement = new List<Bind>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Bind objT1 = new Bind();
                        objT1.TaskId = Convert.ToInt32(dr["TaskId"].ToString());
                        objT1.TaskName = dr["TaskName"].ToString();
                        objT1.TaskCode = dr["TaskCode"].ToString();
                        objT1.AssignByStaffCode = dr["AssignByStaffName"].ToString();
                        objT1.AssignToStaffCode = dr["AssignToStaffName"].ToString();
                        objT1.Priority = dr["Priority"].ToString();
                        objT1.TaskStartDate = Convert.ToDateTime(dr["TaskStartDate"].ToString());
                        objT1.TaskEndDate = Convert.ToDateTime(dr["TaskEndDate"].ToString());
                        objT1.StatusName = dr["Status"].ToString();
                        objT1.TaskDescription = dr["Description"].ToString();
                        objT1.TaskAddedStaff = dr["TaskAddedStaff"].ToString();
                        objT1.assignStaffcode = Session["StaffCode"].ToString();
                        LstTaskManagement.Add(objT1);
                    }
                    objT.lstTaskManagement = LstTaskManagement;
                    string Staffpostion = Session["StaffPositionId"].ToString();
                    if (Staffpostion == "2")
                    {
                        List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                    {
                    new BreadcrumbItem { Name = "CoordinatorDashboard", Url = Url.Action("CoordinatorDashboardPRAsync","Coordinator")},
                    new BreadcrumbItem { Name = "Task Management", Url = Url.Action("ListTaskManagementAsyncPG", "Bind")  }
                    };
                        ViewBag.Breadcrumb = breadcrumbs;
                    }
                    else if (Staffpostion == "3")
                    {
                        List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                    {
                   new BreadcrumbItem { Name = "TrainerDashboard", Url =Url.Action("TrainerDashboardAsyncTS","Trainer")},
                    new BreadcrumbItem { Name = "Task Management", Url = Url.Action("ListTaskManagementAsyncPG", "Bind")  }
                    };
                        ViewBag.Breadcrumb = breadcrumbs;

                    }
                    else if (Staffpostion == "5")
                    {
                        List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                    {
                   new BreadcrumbItem { Name = "PlcementDashboard", Url =Url.Action("PlcementDashboardPCAsync","Placement")},
                    new BreadcrumbItem { Name = "Task Management", Url = Url.Action("ListTaskManagementAsyncPG", "Bind")  }
                    };
                        ViewBag.Breadcrumb = breadcrumbs;

                    }
                    return await Task.Run(() => PartialView("ListTaskManagementAsyncPG", objT));
                }
                catch (Exception ex)
                {
                    return View("Error", new ErrorViewModel { ErrorMessage = ex.Message });
                }
            }
        }

        /// <summary>
        /// Displays the page for registering task management asynchronously.
        /// </summary>
        /// <returns>Returns the task management registration page.</returns>
        [HttpGet]
        public async Task<ActionResult> RegisterTaskManagementAsyncPG()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Bind objT = new Bind();
                objT.StaffCode = Session["StaffCode"].ToString();
                objT.BranchCode = Session["BranchCode"].ToString();
                List<SelectListItem> combinedReportingList = new List<SelectListItem>();
                DataSet ds1 = await BALObj.StaffNameforTask(objT);
                List<SelectListItem> StaffList = new List<SelectListItem>();
                foreach (DataRow dr in ds1.Tables[0].Rows)
                {
                    StaffList.Add(new SelectListItem
                    {
                        Text = "Self",
                        Value = dr["StaffCode"].ToString()
                    }); ;
                }
                combinedReportingList.AddRange(StaffList);
                DataSet ds = await BALObj.AssignTaskRepotingStaffName(objT);
                List<SelectListItem> Repotingstafflist = new List<SelectListItem>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Repotingstafflist.Add(new SelectListItem
                    {
                        Text = dr["StaffName"].ToString(),
                        Value = dr["StaffCode"].ToString()
                    });
                }
                combinedReportingList.AddRange(Repotingstafflist);
                ViewBag.combinedReportingList = combinedReportingList;

                string Staffpostion = Session["StaffPositionId"].ToString();
                if (Staffpostion == "2")
                {
                    List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                    {
                    new BreadcrumbItem { Name = "CoordinatorDashboard", Url = Url.Action("CoordinatorDashboardPRAsync","Coordinator")},
                    new BreadcrumbItem { Name = "Task Management", Url = Url.Action("ListTaskManagementAsyncPG", "Bind")  },
                   new BreadcrumbItem { Name = "RegisterTaskManagement",Url = Url.Action("RegisterTaskManagementAsyncPG", "Bind") }
                    };
                    ViewBag.Breadcrumb = breadcrumbs;
                }
                else if (Staffpostion == "3")
                {
                    List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                    {
                   new BreadcrumbItem { Name = "TrainerDashboard", Url =Url.Action("TrainerDashboardAsyncTS","Trainer")},
                    new BreadcrumbItem { Name = "Task Management", Url = Url.Action("ListTaskManagementAsyncPG", "Bind")  },
                   new BreadcrumbItem { Name = "RegisterTaskManagement",Url = Url.Action("RegisterTaskManagementAsyncPG", "Bind") }
                    };
                    ViewBag.Breadcrumb = breadcrumbs;

                }
                else if (Staffpostion == "5")
                {
                    List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                    {
                   new BreadcrumbItem { Name = "PlcementDashboard", Url =Url.Action("PlcementDashboardPCAsync","Placement")},
                    new BreadcrumbItem { Name = "Task Management", Url = Url.Action("ListTaskManagementAsyncPG", "Bind")  },
                   new BreadcrumbItem { Name = "RegisterTaskManagement",Url = Url.Action("RegisterTaskManagementAsyncPG", "Bind") }
                    };
                    ViewBag.Breadcrumb = breadcrumbs;

                }
                // ViewBag.combinedReportingList = new List<SelectListItem>();
                return await Task.Run(() => PartialView("RegisterTaskManagementAsyncPG"));
            }
        }
        /// <summary>
        /// Registers task management details asynchronously.
        /// </summary>
        /// <param name="objT">The Trainer object containing the task management details to be registered.</param>
        /// <returns>An asynchronous ActionResult representing the result of the registration process.</returns>
        [HttpPost]
        public async Task<ActionResult> RegisterTaskManagementAsyncPG(Bind objT)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                try
                {
                    objT.StaffCode = Session["StaffCode"].ToString();
                    await BALObj.RegisterTaskManagementAsyncPG(objT);
                    return Json(new { success = true, message = "Data saved successfully" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "An error occurred while saving data: " + ex.Message });
                }
            }
        }
        /// <summary>
        /// Prepares data for updating task management asynchronously.
        /// </summary>
        /// <param name="id">The ID of the task management entry to be updated.</param>
        /// <returns>An asynchronous ActionResult representing the update task management view.</returns>
        [HttpGet]
        public async Task<ActionResult> UpdateTaskManagementAsyncPG(int id)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Bind objT = new Bind();
                objT.TaskId = id;
                objT.BranchCode = Session["BranchCode"].ToString();
                SqlDataReader dr;
                dr = await BALObj.FetchTaskManagementAsyncPG(objT);
                while (dr.Read())
                {
                    objT.TaskId = Convert.ToInt32(dr["TaskId"].ToString());
                    objT.TaskName = dr["TaskName"].ToString();
                    objT.TaskCode = dr["TaskCode"].ToString();
                    objT.AssignByStaffCode = dr["AssignByStaffCode"].ToString();
                    objT.AssignToStaffCode = dr["AssignToStaffCode"].ToString();
                    objT.Priority = dr["Priority"].ToString();
                    objT.TaskStartDate = Convert.ToDateTime(dr["TaskStartDate"].ToString());
                    objT.TaskEndDate = Convert.ToDateTime(dr["TaskEndDate"].ToString());
                    objT.TaskStartTime = Convert.ToDateTime(dr["TaskStartTime"].ToString());
                    objT.TaskEndTime = Convert.ToDateTime(dr["TaskEndTime"].ToString());
                    objT.TaskDescription = dr["Description"].ToString();
                    objT.StatusId = Convert.ToInt32(dr["StatusId"].ToString());
                    objT.StaffName = dr["AssignByStaffName"].ToString();
                }
                objT.StaffCode = Session["StaffCode"].ToString();

                if (objT.AssignByStaffCode == Session["StaffCode"].ToString())
                {
                    List<SelectListItem> combinedReportingList = new List<SelectListItem>();
                    DataSet ds2 = await BALObj.StaffNameforTask(objT);
                    List<SelectListItem> StaffList1 = new List<SelectListItem>();
                    foreach (DataRow dr1 in ds2.Tables[0].Rows)
                    {
                        StaffList1.Add(new SelectListItem
                        {
                            Text = "Self",
                            Value = dr1["StaffCode"].ToString()
                        }); ;
                    }
                    combinedReportingList.AddRange(StaffList1);
                    DataSet ds3 = await BALObj.AssignTaskRepotingStaffName(objT);
                    List<SelectListItem> Repotingstafflist = new List<SelectListItem>();
                    foreach (DataRow dr2 in ds3.Tables[0].Rows)
                    {
                        Repotingstafflist.Add(new SelectListItem
                        {
                            Text = dr2["StaffName"].ToString(),
                            Value = dr2["StaffCode"].ToString()
                        });
                    }
                    combinedReportingList.AddRange(Repotingstafflist);
                    ViewBag.combinedReportingList = combinedReportingList;
                    //--------------------------Apply Status-------------------------------------
                    DataSet ds5 = await BALObj.FetchStatusForTask(objT);
                    List<SelectListItem> TaskStatusName = new List<SelectListItem>();
                    foreach (DataRow dr3 in ds5.Tables[0].Rows)
                    {
                        TaskStatusName.Add(new SelectListItem
                        {
                            Text = dr3["Status"].ToString(),
                            Value = dr3["StatusId"].ToString()
                        });
                    }
                    ViewBag.TaskStatusName = TaskStatusName;
                    objT.AllowUpdate = true;
                }
                else
                {
                    DataSet ds5 = await BALObj.FetchStatusForUpdateTask(objT);
                    List<SelectListItem> TaskStatusName = new List<SelectListItem>();
                    foreach (DataRow dr3 in ds5.Tables[0].Rows)
                    {
                        TaskStatusName.Add(new SelectListItem
                        {
                            Text = dr3["Status"].ToString(),
                            Value = dr3["StatusId"].ToString()
                        });
                    }
                    ViewBag.TaskStatusName = TaskStatusName;
                    objT.AllowUpdate = false;
                }
                string Staffpostion = Session["StaffPositionId"].ToString();
                if (Staffpostion == "2")
                {
                    List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                    {
                    new BreadcrumbItem { Name = "CoordinatorDashboard", Url = Url.Action("CoordinatorDashboardPRAsync","Coordinator")},
                    new BreadcrumbItem { Name = "Task Management", Url = Url.Action("ListTaskManagementAsyncPG", "Bind")  },
                    new BreadcrumbItem { Name = "UpdateTaskManagement",Url = Url.Action("UpdateTaskManagementAsyncPG", "Bind") }
                    };
                    ViewBag.Breadcrumb = breadcrumbs;
                }
                else if (Staffpostion == "3")
                {
                    List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                    {
                   new BreadcrumbItem { Name = "TrainerDashboard", Url =Url.Action("TrainerDashboardAsyncTS","Trainer")},
                    new BreadcrumbItem { Name = "Task Management", Url = Url.Action("ListTaskManagementAsyncPG", "Bind")  },
                    new BreadcrumbItem { Name = "UpdateTaskManagement",Url = Url.Action("UpdateTaskManagementAsyncPG", "Bind") }
                    };
                    ViewBag.Breadcrumb = breadcrumbs;

                }
                else if (Staffpostion == "5")
                {
                    List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                    {
                   new BreadcrumbItem { Name = "PlcementDashboard", Url =Url.Action("PlcementDashboardPCAsync","Placement")},
                    new BreadcrumbItem { Name = "Task Management", Url = Url.Action("ListTaskManagementAsyncPG", "Bind")  },
                    new BreadcrumbItem { Name = "UpdateTaskManagement",Url = Url.Action("UpdateTaskManagementAsyncPG", "Bind") }
                    };
                    ViewBag.Breadcrumb = breadcrumbs;

                }
                return PartialView("UpdateTaskManagementAsyncPG", objT);
            }
        }
        /// <summary>
        /// Updates task management details asynchronously.
        /// </summary>
        /// <param name="objT">The Trainer object containing the updated task management details.</param>
        /// <returns>An asynchronous ActionResult representing the result of the update process.</returns>
        [HttpPost]
        public async Task<ActionResult> UpdateTaskManagementAsyncPG(Bind objT)
        {
            try
            {
                if (Session["StaffCode"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    objT.StaffCode = Session["StaffCode"].ToString();
                    await BALObj.UpdateTaskManagementAsyncPG(objT);
                    return Json(new { success = true, message = "Data saved successfully" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "An error occurred while saving data: " + ex.Message });
            }
        }
        /// <summary>
        /// Removes task management details asynchronously based on the provided task ID.
        /// </summary>
        /// <param name="id">The ID of the task management entry to be removed.</param>
        /// <returns>An asynchronous ActionResult representing the result of the removal process.</returns>
        [HttpPost]
        public async Task<ActionResult> DeleteTask(int taskId)
        {
            try
            {
                if (Session["StaffCode"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    Bind objT = new Bind();
                    objT.TaskId = taskId;
                    objT.BranchCode = Session["BranchCode"].ToString();
                    objT.StaffCode = Session["StaffCode"].ToString();
                    await BALObj.RemoveDataTaskManagement(objT);
                    return Json(new { success = true, message = "Task deleted successfully" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }


        //pratiha task ----------------------------------------
        //pratiha task ----------------------------------------
        //-------------------------------------------Punam Contact,Email,SMS--------------------
        /// <summary>
        /// THIS METHOD IS USE TO GET COURCE WISE BATCHES.
        /// </summary>
        /// <param name="CourceCode"></param>
        /// THIS OBJECT IS USE TO GET COUECE CODE.
        /// <returns>GET BATCHES.</returns>
        [HttpPost]
        public async Task<JsonResult> GetCourceWiseBatches(string CourceCode)
        {
            DataSet ds = await BALObj.GetAllBatchNameAsyncPB(CourceCode);
            DataTable dt = ds.Tables[0];
            var jsondata = JsonConvert.SerializeObject(dt);
            return await Task.Run(() => Json(jsondata));
        }
        /// <summary>
        /// THIS METHOD IS USE TO GET CONTACT LIST FOR TRAINER. 
        /// </summary>
        /// <returns>CONTACT LIST GET.</returns>
        [HttpGet]
        public async Task<ActionResult> ListContactAsyncPB()
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                try
                {
                    ViewBag.Contact = await BALObj.GetContactListPB();
                    ViewBag.Type = await BALObj.GetFilterTypePB();
                    List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                    new BreadcrumbItem { Name = "Dashboard", Url = "TrainerDashboardAsyncTS" },
                    new BreadcrumbItem { Name = "Contact", Url = "ListContactAsyncPB"}
                };
                    ViewBag.Breadcrumb = breadcrumbs;
                    return await Task.Run(() => View());
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
        }
        /// <summary>
        /// THIS METHOD IS USE TO GET FILTER DATA ON TYPE ID.
        /// </summary>
        /// <param name="typeid"></param>
        /// THIS OBJECT IS USE TO PASS TYPE ID VIEW TO CONTROLLER AND CONTROLLER TO BAL.
        /// <returns> FILTER DATA ON TYPE ID.</returns>
        [HttpPost]
        public async Task<JsonResult> FilterContactDataAsyncPB(int typeid)
        {
            try
            {
                DataSet ds = await BALObj.FilterTypeAsyncPB(typeid);
                DataTable dt = ds.Tables[0];
                var jsondata = JsonConvert.SerializeObject(dt);
                return Json(jsondata);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// THIS METHOD IS USE TO GET CREATE CONTACT VIEW AND GET COUNTRY FOR BINDING.
        /// TO TAKE THE NAME OF THE COUNTRY , THE CALL HAS BEEN MADE AS THE OBJECT OF BALPLACEMENT.
        /// </summary>
        /// <returns> GET CONTACT VIEW.</returns>
        [HttpGet]
        public async Task<ActionResult> CreateContactAsyncPB()
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                try
                {
                    ViewBag.Country = await BALObj.GetCountry();
                    ViewBag.Type = await BALObj.GetFilterTypePB();
                    List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                    new BreadcrumbItem { Name = "Dashboard", Url = "TrainerDashboardAsyncTS" },
                    new BreadcrumbItem { Name = "ContactList", Url = "ListContactAsyncPB"}
                };
                    ViewBag.Breadcrumb = breadcrumbs;
                    return await Task.Run(() => PartialView("CreateContactAsyncPB"));
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
        }
        [HttpGet]
        public async Task<ActionResult> CreateTypeAsyncPB()
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                try
                {
                    return await Task.Run(() => PartialView("CreateTypeAsyncPB"));
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
        }

        /// <summary>
        /// THIS METHOD IS USE TO SAVE NEW TYPE TYPE IN GSTTBLCONTACTYPE.
        /// </summary>
        /// <returns>SAVE NEW TYPE</returns>
        [HttpPost]
        public async Task<ActionResult> SaveTypeAsyncPB(Bind objteainer)
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                try
                {
                    await BALObj.SaveAddedTypeAsyncPB(objteainer.Type);
                    return await Task.Run(() => PartialView("CreateTypeAsyncPB"));
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
        }
        /// <summary>
        /// THIS METHOD IS USE TO SAVE NEW CONTACT.
        /// </summary>
        /// <returns> SAVE CONTACT IN GSTTBLCONTACT.</returns>
        [HttpPost]
        public async Task<JsonResult> SaveContactAsyncPB(Bind objteainer)
        {
            try
            {
                await BALObj.SaveContactAsyncPB(objteainer);
                return await Task.Run(() => Json(new { success = true, message = "Contact has been Created successfully!" }));
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// THIS METHOD IS USE TO GET DATABASE SAVED TYPE AND VALIDATION FOR NEW TYPE EX. DON'T SAVE SAME TYPE.
        /// </summary>
        /// <param name="Newtype"></param>
        /// THIS OBJECT IS USE TO GET VALUE.
        /// <returns>SANE TYPE NOT SAVE.</returns>
        [HttpPost]
        public async Task<JsonResult> GetAllReadySavedTypeAsyncPB(string Newtype)
        {
            try
            {
                DataSet ds = await BALObj.GetContactTypeForValidaAsyncPB(Newtype);
                DataTable dt = ds.Tables[0];
                var jsondata = JsonConvert.SerializeObject(dt);
                return Json(jsondata);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// THIS METHOD IS USE TO GET STATE NAME ON THE COUNTRY ID.
        /// TO TAKE THE NAME OF THE STATE , THE CALL HAS BEEN MADE AS THE OBJECT OF BALPLACEMENT.
        /// </summary>
        /// <param name="CountryId"></param>
        /// THIS OBJECT IS USE TO GET STATE NAME ON COUNTRY SPECEFIC ID.
        /// <returns>COUNTRY ID WISE STATE NAME .</returns>
        [HttpPost]
        public async Task<JsonResult> GetStateAsyncPB(int CountryId)
        {
            try
            {
                DataSet ds = await BALObj.GetState(CountryId);
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
        /// THIS METHOD IS USED FOR TO CITY BIND ON THE STATE ID.
        /// TO TAKE THE NAME OF THE CITY , THE CALL HAS BEEN MADE AS THE OBJECT OF BALPLACEMENT.
        /// </summary>
        /// <param name="StateId"></param>
        /// THIS OBJECT IS USE TO GET CITY ON STATEID.
        /// <returns> GET CITY.</returns>
        [HttpPost]
        public async Task<JsonResult> GetCityAsyncPB(int StateId)
        {
            try
            {
                DataSet ds = await BALObj.GetCity(StateId);
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
        /// THIS METHOD IS USE TO GET CONTACT FOR EDIT.
        /// </summary>
        /// <param name="objteainer"></param>
        /// THIS OBJECT IS USE TO GET ID.
        /// <returns>get contact.</returns>
        [HttpGet]
        public async Task<ActionResult> EditContactAsyncPB(Bind objteainer)
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                try
                {
                    ViewBag.ContacEdit = await BALObj.GetContactForEditAsyncPB(objteainer);
                    ViewBag.Type = await BALObj.GetFilterTypePB();
                    ViewBag.Country = await BALObj.GetCountry();
                    ViewBag.State = await BALObj.GetState(ViewBag.ContacEdit.Tables[0].Rows[0]["CountryId"]);
                    ViewBag.City = await BALObj.GetCity(ViewBag.ContacEdit.Tables[0].Rows[0]["StateId"]);

                    return await Task.Run(() => PartialView("EditContactAsyncPB"));
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
        }
        /// <summary>
        /// THIS METHOD IS USE UPDATE CONTACT DETAILS.
        /// </summary>
        /// <returns>UPDATE CONTACT DETAILS ON ID.</returns>
        [HttpPost]
        public async Task<JsonResult> UpdateContactAsyncPB(Bind objteainer)
        {
            try
            {
                await BALObj.UpdateContactAsyncPB(objteainer);
                return await Task.Run(() => Json(new { success = true, message = "Contact has been updated successfully!" }));
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// THIS METHOD IS USE TO GET CONTACT DETAILS.
        /// </summary>
        /// <param name="objteainer"></param>
        /// THIS ONJECT IS USE TO GET ID.
        /// <returns>ID WISE DETAILS GET.</returns>
        [HttpGet]
        public async Task<ActionResult> DetailsContactAsyncPB(int Id)
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                try
                {
                    ViewBag.ContacEdit = await BALObj.GetDetailsAsyncPB(Id);
                    return await Task.Run(() => PartialView("DetailsContactAsyncPB"));
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
        }
        /// <summary>
        ///THIS FUNCTION IS USE TO GET COMPOSE VIEW AND EMAIL TEMPLARE.
        /// </summary>
        /// <returns> GET COMPOSE VIEW.</returns>
        [HttpGet]
        public async Task<ActionResult> SendEmailAsyncPB()
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                try
                {
                    Bind objB = new Bind();
                    objB.BranchCode = Session["BranchCode"].ToString();
                    List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                   {
                    new BreadcrumbItem { Name = "Dashboard", Url = "TrainerDashboardAsyncTS" },
                    new BreadcrumbItem { Name = "Email", Url = "SendEmailAsyncPB" }

                    };

                    ViewBag.Breadcrumb = breadcrumbs;
                    ViewBag.TemplateName = await BALObj.GetEmailTemplatePB();
                    ViewBag.CourceName = await BALObj.GetCourceNameAsyncPB(objB);
                    return await Task.Run(() => View());
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
        }

        /// <summary>
        /// THIS METHIS IS USE TO GET ALL TEMPLATE DATA ON TAMPLATE NAME.
        /// </summary>
        /// <returns> IT'S RETUN ALL DATA.</returns>
        [HttpPost]
        public async Task<JsonResult> GetAllTemplateDetailsPB(string TemplateId)
        {
            try
            {
                DataSet ds = await BALObj.GetAllDetailsofTemplatePB(TemplateId);
                DataTable dt = ds.Tables[0];
                var jsondsta = JsonConvert.SerializeObject(dt);
                return Json(jsondsta);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// THIS METHOD IS USE TO DELETE CONTACT DETAILS.
        /// </summary>
        /// <param name="id"></param>
        /// THIS OBJECT IS USE TO GET ID.
        /// <returns>DELEYE CONTACT RECORDED.</returns>
        [HttpPost]
        public async Task<ActionResult> DeleteOneContactAsyncePB(int id)
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                try
                {
                    Bind obj = new Bind();
                    obj.DeleteId = id.ToString();
                    await BALObj.DeleteContactAsyncPB(obj);
                    return await Task.Run(() => Json(new { success = true, message = "Contact deleted successfully" }, JsonRequestBehavior.AllowGet));
                }
                catch (Exception ex)
                {
                    throw (ex);
                }

            }
        }
        /// <summary>
        /// THIS METHOD ID USE TO DELETE CONTACT DETAILS.
        /// </summary>
        /// <param name="Id"></param>
        /// THIS OBJECT IS USE TO GET COMPANY ID.
        /// <returns>DELETE CONTACT.</returns>
        [HttpPost]
        public async Task<ActionResult> DeleteContactDetailsAsyncPB(List<string> ids)
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                try
                {
                    foreach (var Delete in ids)
                    {
                        Bind obj = new Bind();
                        obj.DeleteId = Delete;
                        await BALObj.DeleteContactAsyncPB(obj);
                    }
                    return Json(new { success = true, message = "Selected Contacts deleted successfully" }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
        }
        [HttpPost]
        public async Task<JsonResult> GetEmailIdAndShowPB(string checkvalues, string selectedBatches)
        {
            try
            {
                //Session["count"] = count;
                Bind objtrainer = new Bind();
                var jsondata = "";
                NameValueCollection FormData = Request.Form;
                string BranchCode = Session["BranchCode"].ToString();
                dynamic SelectedGroup = !string.IsNullOrEmpty(checkvalues) ? checkvalues.TrimEnd(',') : null;
                var SelectedBatch = selectedBatches;
                if (SelectedGroup != null)
                {
                    DataSet StudentEmail = new DataSet();
                    DataTable dt = new DataTable();

                    if (SelectedGroup.Contains("0"))
                    {
                        StudentEmail = BALObj.GetGroupStudentSelectedPB(BranchCode).Copy(); // for student
                    }
                    DataSet OtherGroupId = BALObj.GetEmailIdSelectedGroupPB(SelectedGroup, BranchCode).Copy();
                    if (OtherGroupId.Tables.Count > 0 && OtherGroupId.Tables[0].Rows.Count > 0)
                    {
                        StudentEmail.Merge(OtherGroupId, false, MissingSchemaAction.Add); // merge two datasets
                    }
                    jsondata = JsonConvert.SerializeObject(StudentEmail.Tables[0]); // Serialize the DataTable directly
                }
                if (SelectedBatch != null && SelectedBatch != "")
                {
                    string[] batchArray = SelectedBatch.Split(',');
                    DataSet BatchEmail = await BALObj.GetBachesEmailPB(batchArray, BranchCode);
                    jsondata = JsonConvert.SerializeObject(BatchEmail.Tables[0]); // Serialize the DataTable directly
                }
                return await Task.Run(() => Json(jsondata));
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// THIS FUNCTION IS USE TO SEND EMAIL.
        /// </summary>
        /// <param name="objTrainer"></param>
        /// THIS FUNCTION IS USED USING OBJECT PASS VALUE AND SEND EMAIL.
        /// <returns> SEND EMAIL.</returns>
        /// 
        [HttpPost]
        public async Task<JsonResult> SendEmail(Bind objTrainer, HttpPostedFileBase FileUploader)
        {
            NameValueCollection FormData = Request.Form;
            try
            {
                string BranchCode = Session["BranchCode"].ToString();
                string StaffCode = Session["StaffCode"].ToString();
                var IndividualTo = FormData["to"];
                var TodaysBirthday = FormData["SelectedEmail"];
                var emails = FormData["remainingEmails"];
                int emailCount = 0;
                if (emails != null && emails != "")
                {
                    string[] RemaingEmails = emails.Split(',');
                    emailCount = RemaingEmails.Length;
                    for (int i = 0; i < RemaingEmails.Length; i++)
                    {
                        var test = RemaingEmails[i];
                        await BALObj.SendMailPB(RemaingEmails[i].ToString(), objTrainer.Subject.ToString(), objTrainer.compose.ToString(), FileUploader);

                    }
                    await BALObj.SaveSendEmailPB(emailCount, IndividualTo, StaffCode, BranchCode);
                }
                if (IndividualTo != null && IndividualTo != "")
                {
                    string email = IndividualTo;
                    await BALObj.SendMailPB(IndividualTo, objTrainer.Subject.ToString(), objTrainer.compose.ToString(), FileUploader);
                    await BALObj.SaveSendEmailPB(emailCount, IndividualTo, StaffCode, BranchCode);
                }
                if (TodaysBirthday != null)
                {
                    string[] SelectedDOB = TodaysBirthday.Split(',');
                    int selectedDOB = SelectedDOB.Length;
                    for (int i = 0; i < SelectedDOB.Length; i++)
                    {
                        var testEmail = SelectedDOB[i];
                        await BALObj.SendMailPB(SelectedDOB[i], objTrainer.Subject.ToString(), objTrainer.compose.ToString(), FileUploader);
                    }
                    await BALObj.SaveSendEmailPB(selectedDOB, IndividualTo, StaffCode, BranchCode);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return Json(new { success = true, message = "Email Send successfully..." }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// THIS METHOD IS USE TO GET MESSAGE VIEW.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> SendMessageAsyncPB()
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                try
                {
                    Bind objB = new Bind();
                    List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                    new BreadcrumbItem { Name = "Dashboard", Url = "TrainerDashboardAsyncTS" },
                    new BreadcrumbItem { Name = "Email", Url = "SendEmailAsyncPB" },

                };

                    ViewBag.Breadcrumb = breadcrumbs;
                    ViewBag.TemplateName = await BALObj.GetEmailTemplatePB();
                    ViewBag.CourceName = await BALObj.GetCourceNameAsyncPB(objB);
                    return await Task.Run(() => View());
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
        }

        /// <summary>
        /// THIS METHOD IS USE TO SEND MESSAGE.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> SendSMSAsyncPB(Bind objTrainer)
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                NameValueCollection FormData = Request.Form;
                try
                {
                    string mobile_number = objTrainer.To;
                    string Description = objTrainer.compose;
                    string Sms = "http://bhashsms.com/api/sendmsg.php?user=sadgurukrupa&pass=sadgururkupa&sender=MANDAL&phone=" + HttpUtility.UrlEncode(mobile_number) + "&text=" + HttpUtility.UrlEncode(Description) + "&priority=ndnd&stype=normal";
                    using (HttpClient _httpClient = new HttpClient())
                    {
                        HttpResponseMessage response = _httpClient.GetAsync(Sms).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            string result = response.Content.ReadAsStringAsync().Result;
                            Console.WriteLine("Response: " + result);
                        }
                        else
                        {
                            Console.WriteLine("Error: " + response.StatusCode);
                        }
                    }
                    return await Task.Run(() => View("SendMessageAsyncPB"));
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
        }
        /// <summary>
        /// THIS METHOD IS USE TO GET STAFF DOB.
        /// </summary>
        /// <returns> GET STAFF DOB. </returns>
        [HttpPost]
        public async Task<JsonResult> GetTodaysBirthdayAsyncPB()
        {
            string BranchCode = Session["BranchCode"].ToString();
            string today = DateTime.Now.ToString("MM-dd");
            today = "11-11";
            DataSet ds = await BALObj.GetBirthdaydate(today, BranchCode);
            DataTable dt = new DataTable();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            var jsondate = JsonConvert.SerializeObject(dt);
            return await Task.Run(() => Json(jsondate));
        }
        /// <summary>
        /// THIS METHOD IS USE TO GET SELECTED VALUE WISE GET CONTACT NUMBER.
        /// </summary>
        /// <param name="checkvalues"></param>
        /// <param name="selectedBatches"></param>
        /// THIS OBJECT IS USE TO GET SELECTED VALUES AND BATCHCODE.
        /// <returns>GET CONTACT NUMBER.</returns>
        [HttpPost]
        public async Task<JsonResult> GetSmsDataShowAsyncPB(string checkvalues, string selectedBatches)
        {
            try
            {
                Bind objtrainer = new Bind();
                var jsondata = "";
                NameValueCollection FormData = Request.Form;
                string BranchCode = Session["BranchCode"].ToString();
                //dynamic SelectedGroup = checkvalues.TrimEnd(',');
                dynamic SelectedGroup = !string.IsNullOrEmpty(checkvalues) ? checkvalues.TrimEnd(',') : null;
                var SelectedBatch = selectedBatches;
                if (SelectedGroup != null)
                {
                    DataSet dsContact = new DataSet();
                    DataTable dt = new DataTable();
                    if (SelectedGroup.Contains("0"))
                    {
                        dsContact = BALObj.GetGroupStudentAsyncPB(BranchCode).Copy(); // for student
                    }
                    DataSet OtherGroupId = BALObj.GetGroupSmsAsyncPB(SelectedGroup, BranchCode).Copy();
                    if (OtherGroupId.Tables.Count > 0 && OtherGroupId.Tables[0].Rows.Count > 0)
                    {
                        dsContact.Merge(OtherGroupId, false, MissingSchemaAction.Add); // merge two datasets
                    }
                    jsondata = JsonConvert.SerializeObject(dsContact.Tables[0]); // Serialize the DataTable directly
                }
                if (SelectedBatch != null && SelectedBatch != "")
                {
                    string[] batchArray = SelectedBatch.Split(',');
                    DataSet BatchEmail = await BALObj.GetBatchesAsyncPB(batchArray, BranchCode);
                    jsondata = JsonConvert.SerializeObject(BatchEmail.Tables[0]); // Serialize the DataTable directly
                }
                return await Task.Run(() => Json(jsondata));
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }
        /// <summary>
        /// THIS METHOD IS USE TO GET STAFF DOB AND NUMBER.
        /// </summary>
        /// <returns> GET STAFF DOB. </returns>
        [HttpPost]
        public async Task<JsonResult> GetStaffBirthdayAndNumberAsyncPB()
        {
            string BranchCode = Session["BranchCode"].ToString();
            string today = DateTime.Now.ToString("MM-dd");
            today = "11-11";
            DataSet ds = await BALObj.GetBirthdaydateAndNoPB(today, BranchCode);
            DataTable dt = new DataTable();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
            }
            var jsondate = JsonConvert.SerializeObject(dt);
            return await Task.Run(() => Json(jsondate));
        }
        //-----------------------------------------------Punam-------------------------------------
    }
}