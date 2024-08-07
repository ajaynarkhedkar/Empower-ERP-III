using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using GSTEducationERPLibrary.Trainer;
using Newtonsoft.Json;
using static System.Collections.Specialized.BitVector32;
using static GSTEducationERP.Controllers.CoordinatorController;


namespace GSTEducationERP.Controllers
{
    public class TrainerController : Controller
    {

       

        private readonly BALTrainer objbal;

        public TrainerController()
        {
            objbal = new BALTrainer();
        }

        public class BreadcrumbItem
        {
            public string Name { get; set; }
            public string Url { get; set; }
        }

        #region public method declare

        //-------------------------- Tushar DashBoard Start ---------------------------------------------------------------//
        [HttpGet]
        public async Task<ActionResult> TrainerDashboardAsyncTS(string BranchCode)
        {
            try
            {

                if (Session["BranchCode"] == null || Session["CourseCode"] == null || Session["StaffCode"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {

                    string CourseCode = Session["CourseCode"].ToString(); // Retrieve course code from session
                    string StaffCode = Session["StaffCode"].ToString();

                    Trainer objT = new Trainer();
                    objT.BranchCode = Session["BranchCode"].ToString();
                    objT.TrainerCode= Session["StaffCode"].ToString();
                    int activeBatchCount = await objbal.ActiveBatch(CourseCode, objT.BranchCode);
                    int totalBatchCount = await objbal.GetAllBatchCount(CourseCode, objT.BranchCode);
                    int totalStudentCount = await objbal.GetStudentCount(CourseCode, objT.BranchCode);
                    int releasestudentcount = await objbal.ReleaseBatch(CourseCode, objT.BranchCode);
                    int upcomingbatch = await objbal.UpcomingBatch(CourseCode, objT.BranchCode);
                    int activestudentcount = await objbal.ActiveStudent(CourseCode, objT.BranchCode);
                    int ReleaseStudent = await objbal.RealeseStudent(CourseCode, objT.BranchCode);
                    int placedstudent = await objbal.PlacedStudent(CourseCode, objT.BranchCode);
                    List<Trainer> trainers = await objbal.GetBatchVsNoOfStudentGraphData(CourseCode, StaffCode, objT.BranchCode);

                    var viewModel = new Trainer
                    {
                        TotalBatchCount = totalBatchCount,
                        TotalStudentCount = totalStudentCount,
                        ActiveBatchCount = activeBatchCount,
                        ReleaseBatchCount = releasestudentcount,
                        UpcomingBatchCount = upcomingbatch,
                        ActiveStudent = activestudentcount,
                        ReleaseStudent = ReleaseStudent,
                        PlacedStudent = placedstudent,
                        GraphData = trainers
                    };
                    DataSet ds = await objbal.DemoReNotificationKKAsync(objT);
                    Trainer count = new Trainer();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        count.Rating = Convert.ToInt32(ds.Tables[0].Rows[i]["TotalCount"].ToString());
                    }
                    ViewBag.DemoNotification = count.Rating;

                    List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                   {
                    new BreadcrumbItem { Name = "TrainerDashboard", Url = "TrainerDashboardAsyncTS/Trainer" },
                   };

                    ViewBag.Breadcrumbs = breadcrumbs;
                    // Return PartialView
                    return PartialView(viewModel);

                }
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or rethrow if necessary
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Redirect to an error page or display a user-friendly error message
                return RedirectToAction("Error", "TrainerDashboard");
            }
        }
   


//-------------------------- Tushar DashBoard End -----------------------------------------------------------------//


//-------------------------- Sayali Batch Management Start ----------------------//
[HttpGet]
        public async Task<ActionResult> ListScheduledBatchRequestAsyncST(Trainer ObjTr)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                ObjTr.TrainerCode = Session["StaffCode"].ToString();
                ObjTr.BranchCode = Session["BranchCode"].ToString();

                DataSet ds = new DataSet();
                ds = await objbal.ListScheduledBatchRequestAsyncST(ObjTr);
                List<Trainer> lstBatchData1 = new List<Trainer>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Trainer ObjTr1 = new Trainer();
                    ObjTr1.ScheduleId = Convert.ToInt32(ds.Tables[0].Rows[i]["ScheduleId"].ToString());
                    ObjTr1.BatchCode = ds.Tables[0].Rows[i]["BatchCode"].ToString();
                    ObjTr1.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    ObjTr1.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    ObjTr1.StaffName = ds.Tables[0].Rows[i]["StaffName"].ToString();
                    ObjTr1.LabName = ds.Tables[0].Rows[i]["LabName"].ToString();
                    ObjTr1.NoOfStudent = Convert.ToInt32(ds.Tables[0].Rows[i]["NoOfStudent"].ToString());
                    ObjTr1.BatchScheduleDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["BatchScheduleDate"].ToString());
                    ObjTr1.StartDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"].ToString());
                    ObjTr1.Status = ds.Tables[0].Rows[i]["Status"].ToString();

                    lstBatchData1.Add(ObjTr1);
                }
                ObjTr.lstBatchData = lstBatchData1;
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
               {
                   new BreadcrumbItem { Name = "TrainerDashboard", Url =Url.Action("TrainerDashboardAsyncTS","Trainer")},
                   new BreadcrumbItem { Name = "NewSchedule", Url =  Url.Action("ListScheduledBatchRequestAsyncST")}
               };

                ViewBag.Breadcrumbs = breadcrumbs;
                return PartialView("ListScheduledBatchRequestAsyncST", ObjTr);
            }
        }


        /// <summary>
        ///This view use to get batch Schedule data for View Only.
        /// </summary>
        /// <returns> Batch Schedule Data</returns>
        [HttpGet]
        public async Task<ActionResult> DetailsNotificationRequestAsyncST(int ScheduleId)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Trainer ObjTr = new Trainer();
                ObjTr.ScheduleId = ScheduleId;
                ObjTr.BranchCode = Session["BranchCode"].ToString();

                SqlDataReader dr;
                dr = await objbal.DetailsBatchScheduleAsyncST(ObjTr);
                while (dr.Read())
                {
                    ObjTr.ScheduleId = Convert.ToInt32(dr["ScheduleId"].ToString());
                    ObjTr.BatchCode = dr["BatchCode"].ToString();
                    ObjTr.BatchName = dr["BatchName"].ToString();
                    ObjTr.CourseName = dr["CourseName"].ToString();
                    ObjTr.StaffName = dr["StaffName"].ToString();
                    ObjTr.LabName = dr["LabName"].ToString();
                    ObjTr.NoOfStudent = Convert.ToInt32(dr["NoOfStudent"].ToString());
                    ObjTr.BatchScheduleDate = Convert.ToDateTime(dr["BatchScheduleDate"].ToString());
                    ObjTr.StartDate = Convert.ToDateTime(dr["StartDate"].ToString());
                    ObjTr.Status = dr["Status"].ToString();
                }
                dr.Close();
                return PartialView("DetailsNotificationRequestAsyncST", ObjTr);
            }

        }

        [HttpPost]
        public async Task<ActionResult> NotificationRequestAcceptAsyncST(Trainer ObjTr)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await objbal.NotificationRequestAcceptAsyncST(ObjTr);
                return Json(new { success = true, message = "Batch has been updated successfully!" });
            }

        }

        [HttpPost]
        public async Task<ActionResult> NotificationRequestRejectAsyncST(Trainer ObjTr)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await objbal.NotificationRequestRejectAsyncST(ObjTr);
                return Json(new { success = true, message = "Batch has been updated successfully!" });
            }
        }
        //-------------------------- Sayali Batch Management End ----------------------//

        //-------------------------- Vaibhav Batch Schedule Start ----------------------//
        /// <summary>
        /// This method is used for to show grid view of section list to trainer in batch sedule.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> DetailsSectionAsyncVP()
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                Trainer objtrainer = new Trainer();
                objtrainer.StaffCode = Session["StaffCode"].ToString();
                objtrainer.BranchCode = Session["BranchCode"].ToString();
                DataSet ds = await objbal.ShowSectionToTrainerAsync(objtrainer);
                Trainer objdetail = new Trainer();
                List<Trainer> LstSectionGRid = new List<Trainer>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Trainer objurser = new Trainer();
                    objurser.SectionId = Convert.ToInt32(ds.Tables[0].Rows[i]["SectionId"].ToString());
                    objurser.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objurser.SectionName = ds.Tables[0].Rows[i]["SectionName"].ToString();
                    LstSectionGRid.Add(objurser);
                }
                objdetail.lstSectionGRid = LstSectionGRid;
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                    new BreadcrumbItem { Name = "TrainerDashboard", Url = Url.Action("TrainerDashboardAsyncTS","Trainer") },
                    new BreadcrumbItem { Name = "Section", Url = Url.Action("DetailsSectionAsyncVP") }
                };

                ViewBag.Breadcrumbs = breadcrumbs;
                return await Task.Run(() => View(objdetail));
            }
        }
        /// <summary>
        /// This method is used for to show grid view of Topic list to trainer.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> DetailsTopicAsyncVP(int id)
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                Trainer obja = new Trainer();
                obja.SectionId = id;
                //BALTrainer objbal = new BALTrainer();
                DataSet ds = await objbal.ShowTopicToTrainerAsync(obja.SectionId);
                Trainer objTopic = new Trainer();
                List<Trainer> LstTopicGRid = new List<Trainer>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Trainer objurser = new Trainer();

                    objurser.TopicId = Convert.ToInt32(ds.Tables[0].Rows[i]["TopicId"].ToString());
                    objurser.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objurser.SectionName = ds.Tables[0].Rows[i]["SectionName"].ToString();
                    objurser.TopicName = ds.Tables[0].Rows[i]["TopicName"].ToString();
                    objurser.NoOfSessions = Convert.ToInt32(ds.Tables[0].Rows[i]["NoOfSessions"].ToString());
                    objurser.Duration = ds.Tables[0].Rows[i]["Duration"].ToString();
                    objurser.TopicDescription = ds.Tables[0].Rows[i]["Topicdescription"].ToString();
                    objurser.Status = ds.Tables[0].Rows[i]["Status"].ToString();
                    LstTopicGRid.Add(objurser);
                }
                objTopic.lstTopicGRid = LstTopicGRid;
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                    new BreadcrumbItem { Name = "TrainerDashboard", Url = Url.Action("TrainerDashboardAsyncTS","Trainer") },
                    new BreadcrumbItem { Name = "Section", Url = Url.Action("DetailsSectionAsyncVP","Trainer") },
                    new BreadcrumbItem { Name = "Topic", Url = Url.Action("DetailsTopicAsyncVP") }
                };

                ViewBag.Breadcrumbs = breadcrumbs;
                return await Task.Run(() => View(objTopic));
            }
        }
        /// <summary>
        /// this method is used to show grid view if assingment to trainer in batch sedule .
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> DetailsAssignmentsAsyncVP(int id)
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                Trainer obja = new Trainer();
                obja.TopicId = id;
                //BALTrainer objbal = new BALTrainer();
                DataSet ds = await objbal.TrainerViewAssingmentAsync(obja);
                Trainer objassing = new Trainer();
                List<Trainer> LstassingGRid = new List<Trainer>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Trainer objurser = new Trainer();
                    objurser.AssignmentId = Convert.ToInt32(ds.Tables[0].Rows[i]["AssignmentId"].ToString());
                    objurser.TopicName = ds.Tables[0].Rows[i]["TopicName"].ToString();
                    objurser.AssignmentFile = ds.Tables[0].Rows[i]["AssignmentFile"].ToString();
                    LstassingGRid.Add(objurser);
                }
                objassing.lstassinggrid = LstassingGRid;
                return await Task.Run(() => PartialView(objassing));
            }
        }
        /// <summary>
        /// this method is used to add new section of perticular course in batch sedule trainer module.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> RegisterSectionAsyncVP()
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                Trainer objtrainer = new Trainer();
                objtrainer.StaffCode = Session["StaffCode"].ToString();
                SqlDataReader dr;
                dr = await objbal.TrainerreadTrainerCouse(objtrainer);
                while (dr.Read())
                {
                    objtrainer.CourseCode = dr["Coursecode"].ToString();
                    objtrainer.CourseName = dr["CourseName"].ToString();
                }
                dr.Close();

                return PartialView("RegisterSectionAsyncVP", objtrainer);
            }
        }
        /// <summary>
        /// this method is used to saved all section of course in batch sedule . 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> RegisterSectionAsyncVP(Trainer obj)
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                bool success = await objbal.TrainerAddSectionAsync(obj);

                if (success)
                {
                    return Json(new { success = true, message = "Section added successfully" });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to add section" });
                }
            }
        }
        /// <summary>
        /// this methid is showing view of add new topic in batch sedule .
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> RegisterTopicAsyncVP()
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                await Section_Bind();
                await Status_Bind();
                return await Task.Run(() => PartialView());
            }
        }
        /// <summary>
        /// this method is used to saved topic in batch sedule .
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> RegisterTopicAsyncVP(Trainer obj)
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                obj.StaffCode = Session["StaffCode"].ToString();
                obj.TopicAddedDate = DateTime.Now;
                //BALTrainer objba = new BALTrainer();
                await objbal.TrainerAddTopicAndAssingAsync(obj);
                foreach (var file in obj.Files)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var newFileName = Path.GetFileNameWithoutExtension(file.FileName);
                        var extention = Path.GetExtension(file.FileName);
                        newFileName = newFileName + extention;
                        var files = "~/Content/Trainer/docs/" + newFileName;
                        obj.AssignmentFile = files;
                        newFileName = Path.Combine(Server.MapPath("~/Content/Trainer/docs/"), newFileName);
                        file.SaveAs(newFileName);
                        await objbal.TrainerSaveAssingmentAsync(obj);
                    }
                }

                return Json(new { success = true, message = "Topic has been saved successfully!" });
            }
        }
        /// <summary>
        /// this method to bind section data to dropdown 
        /// </summary>
        public async Task Section_Bind()
        {
            Trainer obj = new Trainer();
            obj.CourseCode = Session["CourseCode"].ToString();
            DataSet ds1 = new DataSet();
            ds1 = await objbal.ShowSectionDropdownAsync(obj);
            List<SelectListItem> Sectionlist = new List<SelectListItem>();
            if (ds1.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds1.Tables[0].Rows)
                {
                    Sectionlist.Add(new SelectListItem
                    {
                        Text = dr["SectionName"].ToString(),
                        Value = dr["SectionId"].ToString()
                    });
                }
            }
            ViewBag.Section = Sectionlist;
        }
        /// <summary>
        /// this method is used to bind data to dropdown in batch sedule .
        /// </summary>
        public async Task Status_Bind()
        {
            //BALTrainer objbal = new BALTrainer();
            DataSet ds1 = new DataSet();
            ds1 = await objbal.TopicShowStatusAsync();
            List<SelectListItem> Statuslist = new List<SelectListItem>();
            if (ds1.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds1.Tables[0].Rows)
                {
                    Statuslist.Add(new SelectListItem
                    {
                        Text = dr["Status"].ToString(),
                        Value = dr["StatusId"].ToString()
                    });
                }
            }
            ViewBag.Status = Statuslist;
        }
        /// <summary>
        /// this method is used to delete section batch sedule .
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Trainerdeletesection(int id)
        {
            Trainer obtassingment = new Trainer();
            obtassingment.SectionId = id;
            SqlDataReader dr;
            dr = await objbal.TrainerDeleteSectionCheckAsync(obtassingment);
            if (dr.Read())
            {
                TempData["SweetAlertMessage"] = "This content is related and cannot be deleted.";
            }
            else
            {
                //BALTrainer objbal1 = new BALTrainer();
                await objbal.TrainerDeleteSectionAsync(obtassingment);
                return Json(new { success = true, message = "Section deleted successfully" }, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("DetailsSectionAsyncVP");
        }
        /// <summary>
        /// this method is used to delete topic of session in batch sedule module .
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> TrainerdeleteTopic(int id)
        {
            Trainer obtassingment = new Trainer();
            obtassingment.TopicId = id;
            //BALTrainer objbal = new BALTrainer();
            SqlDataReader dr;
            dr = await objbal.TrainerDeleteTopicCheckAsync(obtassingment);
            if (dr.Read())
            {
                TempData["SweetAlertMessage"] = "This content is related and cannot be deleted.";
            }
            else
            {
                BALTrainer objbal1 = new BALTrainer();
                await objbal1.TrainerDeleteTopicAsync(obtassingment);
                return Json(new { success = true, message = "Topic deleted successfully" }, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("DetailsTopicAsyncVP", new { id = obtassingment.SectionId });

        }
        /// <summary>
        /// this method is used to delete assingment of topic in batch sedule module .
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> TrainerAssingDelete(int id)
        {
            Trainer obj = new Trainer();
            obj.AssignmentId = id;
            //BALTrainer objsave = new BALTrainer();
            await objbal.TrainerDeleteAssingmentAsync(obj);
            return Json(new { success = true, message = "Assignment deleted successfully" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> TrainerAssingDeletemulti(List<int> ids)
        {
            //BALTrainer objsave = new BALTrainer();

            foreach (var id in ids)
            {
                Trainer obj = new Trainer();
                obj.AssignmentId = id;
                await objbal.TrainerDeleteAssingmentAsync(obj);
            }

            return Json(new { success = true, message = "Selected assignments deleted successfully" }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// this method used to show ing edit topic view and freaching data of topic 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> UpdateTopicAsyncVP(int id)
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                await Section_Bind();
                await Status_Bind();
                Trainer objedit = new Trainer();
                objedit.TopicId = id;
                SqlDataReader dr;
                dr = await objbal.TrainerreadTopicAsync(objedit);
                while (dr.Read())
                {
                    objedit.TopicId = Convert.ToInt32(dr["TopicId"].ToString());
                    objedit.SectionId = Convert.ToInt32(dr["SectionId"].ToString());
                    objedit.TopicName = dr["TopicName"].ToString();
                    objedit.NoOfSessions = Convert.ToInt32(dr["NoOfSessions"].ToString());
                    objedit.Duration = dr["Duration"].ToString();
                    objedit.TopicDescription = dr["Topicdescription"].ToString();
                    objedit.StatusId = Convert.ToInt32(dr["TopicStatusId"].ToString());
                }
                dr.Close();
                ViewBag.Duration = objedit.Duration;
                return await Task.Run(() => PartialView(objedit));
            }
        }
        /// <summary>
        /// this is used to saved edit detail of topic in batch sedule .
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> UpdateTopicAsyncVP(Trainer obj)
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                await objbal.TrainerUpdateTopicAsync(obj);
                if (obj.Files != null)
                {
                    foreach (var file in obj.Files)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            var newFileName = Path.GetFileNameWithoutExtension(file.FileName);
                            var extention = Path.GetExtension(file.FileName);
                            newFileName = newFileName + extention;
                            var files = "~/Content/Trainer/docs/" + newFileName;
                            obj.AssignmentFile = files;
                            newFileName = Path.Combine(Server.MapPath("~/Content/Trainer/docs/"), newFileName);
                            file.SaveAs(newFileName);
                            await objbal.TrainerUpdateAssingmentAsync(obj);
                        }
                    }
                }
                return Json(new { success = true, message = "Topic has been updated successfully." });
            }
        }
        /// <summary>
        /// this is method used to show view of adit section in batch sedule .
        /// </summary>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> UpdateSectionAsyncVP(int sectionId)
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                Trainer objedit = new Trainer();
                objedit.SectionId = sectionId;
                SqlDataReader dr;
                dr = await objbal.TrainerreadsectionAsync(objedit);
                while (dr.Read())
                {
                    objedit.SectionId = Convert.ToInt32(dr["SectionId"].ToString());
                    //objedit.CourseCode = dr["TopicName"].ToString();
                    objedit.CourseName = dr["CourseName"].ToString();
                    objedit.SectionName = dr["SectionName"].ToString();
                }
                dr.Close();
                return PartialView("UpdateSectionAsyncVP", objedit);
            }
        }
        /// <summary>
        /// this method is used to saved deatil of session of batch sedule 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> UpdateSectionAsyncVP(Trainer obj)
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                await objbal.TrainerUpdateSectionAsync(obj);
                return Json(new { success = true, message = "Section Updated successfully" });
            }
        }
        //----------------------------Assing Sedule-----------------------//
        /// <summary>
        /// this method show assing sedule in grid view in assing sedule .
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> DetailsAssignScheduleAsyncVP()
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                Trainer objtrainer = new Trainer();
                objtrainer.StaffCode = Session["StaffCode"].ToString();
                objtrainer.BranchCode = Session["BranchCode"].ToString();
                DataSet ds = await objbal.ShowTrainerAssingSeduleAsync(objtrainer);
                Trainer objdetail = new Trainer();
                List<Trainer> LstAssingGRid = new List<Trainer>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Trainer obju = new Trainer();
                    obju.AssignScheduleId = Convert.ToInt32(ds.Tables[0].Rows[i]["AssignScheduleId"].ToString());
                    obju.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    obju.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    obju.SectionName = ds.Tables[0].Rows[i]["SectionName"].ToString();
                    obju.TopicName = ds.Tables[0].Rows[i]["TopicName"].ToString();
                    obju.LabName = ds.Tables[0].Rows[i]["LabName"].ToString();
                    obju.Duration = ds.Tables[0].Rows[i]["Duration"].ToString();
                    obju.STARTDATEA = ds.Tables[0].Rows[i]["StartDate"].ToString();
                    obju.ENDDATEA = ds.Tables[0].Rows[i]["enddate"].ToString();
                    obju.Status = ds.Tables[0].Rows[i]["Status"].ToString();
                    LstAssingGRid.Add(obju);
                }
                objdetail.lstAssingseduleGRid = LstAssingGRid;
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                    new BreadcrumbItem { Name = "TrainerDashboard", Url = Url.Action("TrainerDashboardAsyncTS","Trainer") },
                    new BreadcrumbItem { Name = "Assign Schedule", Url = Url.Action("DetailsAssignScheduleAsyncVP","Trainer") }

                };

                ViewBag.Breadcrumbs = breadcrumbs;
                return await Task.Run(() => View(objdetail));
            }
        }
        /// <summary>
        ///  this method to used for add assing sedule batch dropdown .
        /// </summary>
        public async Task AssingBatch_Bind()
        {

            Trainer objtrainer = new Trainer();
            objtrainer.StaffCode = Session["StaffCode"].ToString();
            objtrainer.BranchCode = Session["BranchCode"].ToString();
            DataSet ds1 = new DataSet();
            ds1 = await objbal.ShowASsingSectionBatchAsync(objtrainer);
            List<SelectListItem> Batchlist = new List<SelectListItem>();
            if (ds1.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds1.Tables[0].Rows)
                {
                    Batchlist.Add(new SelectListItem
                    {
                        Text = dr["BatchName"].ToString(),
                        Value = dr["ScheduleId"].ToString()
                    });
                }
            }
            ViewBag.AssingBatch = Batchlist;
        }
        /// <summary>
        /// this method to used for add assing sedule topic dropdown .
        /// </summary>
        /// <param name="sectionid"></param>
        /// <returns></returns>
        public async Task<JsonResult> SEctionTopic_Bind(int sectionid)
        {
            Trainer objt = new Trainer();
            objt.SectionId = sectionid;
            //BALTrainer objbal = new BALTrainer();
            DataSet ds1 = new DataSet();
            ds1 = await objbal.ShowTopicDropdownAsync(objt);
            List<SelectListItem> Topiclist = new List<SelectListItem>();
            if (ds1.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds1.Tables[0].Rows)
                {
                    Topiclist.Add(new SelectListItem
                    {
                        Text = dr["TopicName"].ToString(),
                        Value = dr["TopicId"].ToString()
                    });
                }
            }
            ViewBag.SectionTopic = Topiclist;
            return Json(Topiclist, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// this method to used for showing view of add assing sedule .
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> RegisterAssignScheduleAsyncVP()
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                await AssingBatch_Bind();
                await Section_Bind();
                return await Task.Run(() => PartialView());
            }
        }
        /// <summary>
        /// this method to used for save add assing sedule .
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> RegisterAssignScheduleAsyncVP(Trainer obj)
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {

                obj.StaffCode = Session["StaffCode"].ToString();
                obj.ScheduleAssignedDate = DateTime.Now;
                obj.StatusId = 19;
                //BALTrainer objbal = new BALTrainer();
                await objbal.TrainerAddAssingSeduleAsync(obj);
                return Json(new { success = true });
            }
            //return RedirectToAction("DetailsAssignScheduleAsyncVP");
        }
        [HttpGet]
        public async Task<ActionResult> UpdateAssingSeduleAsyncVP(int id)
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                await AssingSeduleStatus_Bind();
                Trainer objedit = new Trainer();
                objedit.AssignScheduleId = id;
                //BALTrainer objbal = new BALTrainer();
                SqlDataReader dr;
                dr = await objbal.TrainerReadAssingSeduleAsync(objedit);
                while (dr.Read())
                {
                    objedit.AssignScheduleId = Convert.ToInt32(dr["AssignScheduleId"].ToString());
                    objedit.BatchName = dr["BatchName"].ToString();
                    objedit.SectionName = dr["SectionName"].ToString();
                    objedit.TopicName = dr["TopicName"].ToString();
                    objedit.NoOfSessions = Convert.ToInt32(dr["NoOfSessions"].ToString());
                    objedit.Duration = dr["Duration"].ToString();
                    objedit.StartDate = dr["StartDate"].ToString().AsDateTime();
                    objedit.StatusId = Convert.ToInt32(dr["StatusId"].ToString());
                }
                dr.Close();
                return await Task.Run(() => PartialView(objedit));
            }

        }
        [HttpPost]
        public async Task<ActionResult> UpdateAssingSeduleAsyncVP(Trainer objt)
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                await objbal.TrainerUpdateAssignSchedule(objt);
                return Json(new { success = true, message = "Assign Sedule has been updated successfully." });
            }
            //return RedirectToAction("DetailsAssignScheduleAsyncVP");

        }
        /// <summary>
        /// this is used for feaching detail od topic in view 
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetTopicDetails(int topicId)
        {
            Trainer objread = new Trainer();
            objread.TopicId = topicId;
            //BALTrainer objbal = new BALTrainer();
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            ds = await objbal.TrainerreadTopicdeatilsAsync(objread);
            dt = ds.Tables[0];
            var JsonData = JsonConvert.SerializeObject(dt);
            return Json(JsonData);
        }
        public async Task AssingSeduleStatus_Bind()
        {
            //BALTrainer objbal = new BALTrainer();
            DataSet ds1 = new DataSet();
            ds1 = await objbal.ShowAssignScheduleStatus();
            List<SelectListItem> Statuslist = new List<SelectListItem>();
            if (ds1.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds1.Tables[0].Rows)
                {
                    Statuslist.Add(new SelectListItem
                    {
                        Text = dr["Status"].ToString(),
                        Value = dr["StatusId"].ToString()
                    });
                }
            }
            ViewBag.AssingStatus = Statuslist;
        }
        public async Task<ActionResult> TrainerAssingSeduleDelete(int id)
        {
            if (Session["StaffCode"] == null)
            {
                return await Task.Run(() => RedirectToAction("Login", "Account"));
            }
            else
            {
                Trainer obj = new Trainer();
                obj.AssignScheduleId = id;
                //BALTrainer objsave = new BALTrainer();
                await objbal.TrainerDeleteAssignSeduleAsync(obj);
                return Json(new { success = true, message = "Assign Sedule deleted successfully" }, JsonRequestBehavior.AllowGet);
            }
        }

        //-------------------------- Vaibhav Batch Schedule End ----------------------//

        //-------------------------- Kirti Demo Start ----------------------//
        /// <summary>
        /// This method is showing the all demo request count comes from coordinator.
        /// </summary>
        /// <param name="objdemo">This parameter is use for the session pass.</param>
        /// <returns>It returns the demo request counts of particular trainer.</returns>
        //public async Task<ActionResult> TrainerDashboard(Trainer objdemo)
        //{
        //    if (Session["StaffCode"] == null)
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }
        //    else
        //    {
        //        objdemo.BranchCode = Session["BranchCode"].ToString();
        //        objdemo.TrainerCode = Session["StaffCode"].ToString();
        //        DataSet ds = await objbal.DemoReNotificationKKAsync(objdemo);
        //        Trainer count = new Trainer();
        //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //        {
        //            count.Rating = Convert.ToInt32(ds.Tables[0].Rows[i]["TotalCount"].ToString());
        //        }
        //        ViewBag.DemoNotification = count.Rating;

        //        List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
        //        {
        //            new BreadcrumbItem { Name = "Home", Url = Url.Action( "TrainerDashboard")},
        //        };
        //        ViewBag.Breadcrumbs = breadcrumbs;

        //        return View();
        //    }
        //}
        [HttpGet]
        /// <summary>
        /// This method is a showing on tab the arranged status demo list.
        /// </summary>
        /// <returns>Returns the list.</returns>
        public async Task<ActionResult> ListDemoArrangedKKAsync()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                    new BreadcrumbItem { Name = "Home", Url = Url.Action("TrainerDashboard")},
                   new BreadcrumbItem { Name = "Demo Requests", Url = Url.Action("ListDemoArrangedKKAsync")}
                };

                ViewBag.Breadcrumbs = breadcrumbs;
                return await Task.Run(() => View());
            }
        }
        [HttpGet]
        /// <summary>
        /// This method is for the all arrnaged demos list.
        /// </summary>
        /// <param name="id">To get the id from the list.</param>
        /// <returns>Returns the list.</returns>
        public async Task<ActionResult> ListAcceptedDemoKKAsync(int id = 0)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                List<Trainer> model = await ArrangedDemoList(id);
                Trainer objlist = new Trainer();
                objlist.lstArrangeDemo = model;
                return PartialView("ListAcceptedDemoKKAsync", objlist);
            }
        }
        /// <summary>
        /// This method is for the showing the list of demo which is arrange to particular trainer.
        /// </summary>
        /// <param name="statusid">This parameter use for the select the status id data.</param>
        /// <returns>it returns the status wise list.</returns>
        [HttpGet]
        private async Task<List<Trainer>> ArrangedDemoList(int statusid)
        {

            Trainer objde = new Trainer();
            objde.StatusId = statusid;
            objde.BranchCode = Session["BranchCode"].ToString();
            objde.TrainerCode = Session["StaffCode"].ToString();
            DataSet ds = await objbal.ViewArrangedDemoListKKAsync(objde);
            List<Trainer> lstd = new List<Trainer>();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Trainer objt = new Trainer();
                    objt.ScheduleId = Convert.ToInt32(ds.Tables[0].Rows[i]["ScheduleId"].ToString());
                    objt.BatchCode = ds.Tables[0].Rows[i]["BatchCode"].ToString();
                    objt.DemoName = ds.Tables[0].Rows[i]["DemoName"].ToString();
                    objt.DemoArrangedBy = ds.Tables[0].Rows[i]["DemoArrangedBy"].ToString();
                    objt.LabName = ds.Tables[0].Rows[i]["LabName"].ToString();
                    DateTime StartDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString());
                    objt.strtDate = StartDate.ToString("dd-MM-yyyy");
                    objt.NoOfStudent = Convert.ToInt32(ds.Tables[0].Rows[i]["NoOfStudent"].ToString());
                    objt.StatusName = ds.Tables[0].Rows[i]["Status"].ToString();
                    objt.StatusId = Convert.ToInt32(ds.Tables[0].Rows[i]["Statusid"].ToString());
                    objt.StartTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartTime"].ToString());
                    objt.EndTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["EndTime"].ToString());
                    lstd.Add(objt);
                }
            }
            return lstd;

        }
        /// <summary>
        /// This method is shows the noofstudent list.
        /// </summary>
        /// <param name="batchcode">Using this parameter shows the details of single student.</param>
        /// <returns>It returns the list of the student.</returns>
        [HttpGet]
        public async Task<ActionResult> ListDemoStudentKKAsync(string batchcode)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Trainer studs = new Trainer();
                studs.BatchCode = batchcode;
                studs.BranchCode = Session["BranchCode"].ToString();
                DataSet ds;
                ds = await objbal.DemoNoOfStudentListKKAsync(studs);
                Trainer nooflist = new Trainer();
                nooflist.BatchCode = batchcode;
                List<Trainer> lstnoofstud = new List<Trainer>();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Trainer objnoof = new Trainer();
                    objnoof.StudentCode = ds.Tables[0].Rows[i]["value"].ToString();
                    objnoof.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                    objnoof.ContactNumber = ds.Tables[0].Rows[i]["ContactNumber"].ToString();
                    objnoof.Emailid = ds.Tables[0].Rows[i]["EmailId"].ToString();
                    lstnoofstud.Add(objnoof);
                }
                nooflist.lstNoofstudent = lstnoofstud;
                return PartialView("ListDemoStudentKKAsync", nooflist);
            }

        }
        /// <summary>
        /// This method is for the accepting the demo requests.
        /// </summary>
        /// <param name="Scheduleid">Scheduleid use for the get particular list.</param>
        /// <returns>Returns the list of accepting demo.</returns>
        [HttpGet]
        public async Task<ActionResult> DemoAcceptedKKAsync(int Scheduleid)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Trainer arrdemo = new Trainer();
                arrdemo.ScheduleId = Scheduleid;
                arrdemo.BranchCode = Session["BranchCode"].ToString();
                DataSet ds;
                ds = await objbal.DemoAccecptedKKAsync(arrdemo);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    arrdemo.ScheduleId = Convert.ToInt32(ds.Tables[0].Rows[i]["ScheduleId"].ToString());
                    arrdemo.DemoName = ds.Tables[0].Rows[i]["DemoName"].ToString();
                    arrdemo.NoOfStudent = Convert.ToInt32(ds.Tables[0].Rows[i]["NoOfStudent"].ToString());
                    arrdemo.LabName = ds.Tables[0].Rows[i]["LabName"].ToString();
                    DateTime StartDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString());
                    arrdemo.strtDate = StartDate.ToString("dd-MM-yyyy");
                    arrdemo.StartTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartTime"].ToString());
                    arrdemo.EndTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["EndTime"].ToString());
                }

                return PartialView("DemoAcceptedKKAsync", arrdemo);
            }

        }
        /// <summary>
        /// This method is for the update the demo status .
        /// </summary>
        /// <param name="obj">This object use for the access the property from the property class.</param>
        /// <returns>It returns the status.</returns>
        [HttpPost]
        public async Task<ActionResult> DemoAcceptedKKAsync(Trainer obj)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await objbal.ChangeAcceptStatusKKAsync(obj);
                return RedirectToAction("ListArrangedDemoKKAsync");
            }
        }
        [HttpPost]
        public async Task<ActionResult> DemoRejectedKKAsync(Trainer obj)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await objbal.ChangeDemoAcceptStatusKKAsync(obj);
                return RedirectToAction("ListArrangedDemoKKAsync");
            }
        }
        /// <summary>
        /// This method is for the the showing the details of demo.
        /// </summary>
        /// <param name="ScheduleId">This parameter is showing the detials of demo which is assigned.</param>
        /// <returns>It returns the demo details data.</returns>
        public async Task<ActionResult> DetailsDemoAcceptedKKAsync(int ScheduleId)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Trainer detail = new Trainer();
                detail.ScheduleId = ScheduleId;
                detail.BranchCode = Session["BranchCode"].ToString();
                DataSet ds;
                ds = await objbal.DemoAccpetedDetailsKKAsync(detail);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    detail.ScheduleId = Convert.ToInt32(ds.Tables[0].Rows[i]["ScheduleId"].ToString());
                    detail.DemoName = ds.Tables[0].Rows[i]["DemoName"].ToString();
                    DateTime StartDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString());
                    detail.strtDate = StartDate.ToString("dd-MM-yyyy");
                    detail.StartTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartTime"].ToString());
                    detail.EndTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["EndTime"].ToString());
                    detail.LabName = ds.Tables[0].Rows[i]["LabName"].ToString();
                    detail.NoOfStudent = Convert.ToInt32(ds.Tables[0].Rows[i]["NoOfStudent"].ToString());
                    detail.StaffName = ds.Tables[0].Rows[i]["StaffName"].ToString();
                    detail.StatusName = ds.Tables[0].Rows[i]["Status"].ToString();
                }
                return PartialView("DetailsDemoAcceptedKKAsync", detail);
            }
        }
        /// <summary>
        /// This method is for the showing the student list from the batch.
        /// </summary>
        /// <param name="batchcode">The batch code is use for the showing selected batch students.</param>
        /// <returns>It returns the student lists.</returns>
        [HttpGet]
        public async Task<ActionResult> ListDemoAttendanceStudentKKAsync(string batchCode, string startDate, string startTime)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Trainer att = new Trainer();
                att.BatchCode = batchCode;
                att.StartDate = Convert.ToDateTime(startDate);
                att.StartTime = Convert.ToDateTime(startTime);
                att.BranchCode = Session["BranchCode"].ToString();
                DataSet ds;
                ds = await objbal.DemoStudentAttendanceListKKAsync(att);
                List<Trainer> lstnoofstud = new List<Trainer>();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Trainer student = new Trainer();
                    student.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                    student.StudentCode = ds.Tables[0].Rows[i]["stud"].ToString();
                    lstnoofstud.Add(student);
                }
                att.lstNoofstudent = lstnoofstud;
                return PartialView("ListDemoAttendanceStudentKKAsync", att);
            }
        }
        /// <summary>
        /// This is the method for the add attendance of demo students.
        /// </summary>
        /// <param name="attendanceData">This paramter use for the use multiple data save like studentcode and attendance id.</param>
        /// <param name="objo">This use for the access the properties from the property class.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> ListDemoAttendanceStudentKKAsync(List<Trainer> attendanceData, Trainer objo)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                foreach (var data in attendanceData)
                {
                    string staffCode = Session["StaffCode"].ToString();
                    Trainer obj = new Trainer();
                    obj.TrainerCode = staffCode;
                    obj.BatchCode = objo.BatchCode;
                    obj.AttendanceStatusId = data.AttendanceStatusId;
                    obj.StudentCode = data.StudentCode;
                    obj.StartDate = objo.StartDate;
                    obj.StartTime = objo.StartTime;
                    await objbal.SaveDemoAttendanceKKAsync(obj);


                }
            }
            return RedirectToAction("ListDemoArrangedKKAsync");
        }
        //-------------------------- Kirti Demo End ---------------------------------------------------------------------------------------------//

        //-------------------------- Yash Attendance Start --------------------------------------------------------------------------------------//
        //-------------------------- Yash Attendance End -----------------------------------------------------------------------------------------//

        //-------------------------- Yash Course Content Start ----------------------//
        /// <summary>
        /// This view is used to show the gridview of course content
        /// </summary>
        /// <returns> Course content list</returns>
        public async Task<ActionResult> ListCourseContentAsyncYT()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {

                Trainer Obj = new Trainer();
                Obj.StaffCode = Session["StaffCode"].ToString();
                DataSet ds = await objbal.ListCourseContentYT();
                Trainer objlist = new Trainer();
                List<Trainer> lstCourseContent = new List<Trainer>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Trainer objC = new Trainer();
                    objC.CourseCode = ds.Tables[0].Rows[i]["Coursecode"].ToString();
                    objC.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objC.SyllabusFile = ds.Tables[0].Rows[i]["SyllabusFile"].ToString();
                    objC.Description = ds.Tables[0].Rows[i]["Description"].ToString();
                    lstCourseContent.Add(objC);
                }
                objlist.lstCourseContent = lstCourseContent;
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                new BreadcrumbItem { Name = "TrainerDashboard", Url =Url.Action("TrainerDashboardAsyncTS","Trainer")},
                new BreadcrumbItem { Name = "Course Content",Url= Url.Action("ListCourseContentAsyncYT","Trainer")},

                };
                ViewBag.Breadcrumbs = breadcrumbs;
                return View(objlist);
            }
        }
        //-------------------------- Yash Course Content End ----------------------//

        //-------------------------- Tushar Test Management Start ----------------------//
        public async Task ListCourse_Bind(string staffCode)
        {

            BALTrainer obc = new BALTrainer();
            Trainer objT = new Trainer();
            objT.StaffCode = staffCode;
            DataSet ds = await objbal.ListCourse(objT);
            List<SelectListItem> courselist = new List<SelectListItem>();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    courselist.Add(new SelectListItem { Text = dr["CourseName"].ToString(), Value = dr["CourseCode"].ToString() });
                }
            }
            ViewBag.Course6 = courselist;
        }
        public async Task Branch_Bind()
        {


            DataSet ds = await objbal.Branch();
            List<SelectListItem> branchlist = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                branchlist.Add(new SelectListItem { Text = dr["BranchName"].ToString(), Value = dr["BranchCode"].ToString() });
            }
            ViewBag.Branch = branchlist;
        } /// <summary>
          /// Section list  BAL method-GetSection(string CourseCode) 
          /// </summary> 
        public async Task<JsonResult> Section_BindTS(string CourseCode)
        {

            Trainer objT = new Trainer();
            objT.CourseCode = CourseCode;
            DataSet ds = await objbal.GetSection(objT);
            List<SelectListItem> sectionlist = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sectionlist.Add(new SelectListItem { Text = dr["SectionName"].ToString(), Value = dr["SectionId"].ToString() });
            }
            ViewBag.sectionlist = sectionlist;
            return Json(sectionlist, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// Batch list(Course Related)
        /// </summary>
        /// <param name="CourseCode"></param>
        /// <returns></returns>
        public async Task<JsonResult> Batch_Bind(string CourseCode)
        {

            Trainer objT = new Trainer();
            objT.CourseCode = CourseCode;
            DataSet ds = await objbal.ListBatch(objT);
            List<SelectListItem> Batchlist = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Batchlist.Add(new SelectListItem { Text = dr["BatchName"].ToString(), Value = dr["BatchCode"].ToString() });
            }
            ViewBag.Batchlist = Batchlist;
            return Json(Batchlist, JsonRequestBehavior.AllowGet);

        }
        public async Task<JsonResult> BranchrelatedCourse_Bind(string BranchCode)
        {
            BALTrainer objBC = new BALTrainer();
            DataSet ds = await objBC.GetBranchrelatedCourse(BranchCode);
            List<SelectListItem> BranchCourselist = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                BranchCourselist.Add(new SelectListItem { Text = dr["CourseName"].ToString(), Value = dr["CourseCode"].ToString() });
            }
            ViewBag.BranchCourselist = BranchCourselist;
            return Json(BranchCourselist, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// Topic list  BaL method -GetTopic(int SectionId)
        /// </summary>
        public async Task<JsonResult> Topic_Bind(int SectionId)
        {

            Trainer objT = new Trainer();
            objT.SectionId = SectionId;
            DataSet ds = await objbal.GetTopic(objT);
            List<SelectListItem> topiclist = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                topiclist.Add(new SelectListItem { Text = dr["TopicName"].ToString(), Value = dr["TopicId"].ToString() });
            }
            return Json(topiclist, JsonRequestBehavior.AllowGet);

        }

        public async Task<JsonResult> Test_Bind(int TopicId)
        {
            BALTrainer objTest = new BALTrainer();
            Trainer objT = new Trainer();
            objT.TopicId = TopicId;
            DataSet ds = await objTest.GetTestName(objT.TopicId);
            List<SelectListItem> testlist = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                testlist.Add(new SelectListItem { Text = dr["TestName"].ToString(), Value = dr["TestId"].ToString() });
            }
            return Json(testlist, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> EditTest_Bind(int TopicId)
        {

            Trainer objT = new Trainer();
            objT.TopicId = TopicId;
            DataSet ds = await objbal.GetTestNameforAssign(objT);
            List<SelectListItem> testlist = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                testlist.Add(new SelectListItem { Text = dr["TestName"].ToString(), Value = dr["TestId"].ToString() });
            }
            return Json(testlist, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Add(Register) new Test get view
        /// </summary>
        /// <returns></returns>
        [HttpGet]

        public async Task<ActionResult> RegisterTestAsycTS(string StaffCode)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Trainer objT = new Trainer();
                objT.BranchCode = Session["BranchCode"].ToString();
                objT.SDuration = TimeSpan.FromHours(1);
                objT.StaffCode = Session["StaffCode"].ToString(); // Pass the staff code to the model
                await ListCourse_Bind(objT.StaffCode); // Call ListCourse_Bind with the StaffCode
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
            {
                //new BreadcrumbItem { Name = "Home", Url = "/" },
                 new BreadcrumbItem { Name = "TrainerDashboard", Url = "TrainerDashboardAsyncTS/Trainer" },
               new BreadcrumbItem { Name = "Test Managment", Url = "ListTestManagementAsynchTS/Trainer" },
               new BreadcrumbItem { Name = "  Add New Test", Url = "RegisterTestAsycTS/Trainer" }, // Adjust URL as needed
            };

                ViewBag.Breadcrumbs = breadcrumbs;
                return View(objT);
            }
        }




        [HttpPost]
        public async Task<ActionResult> RegisterTestAsycTS(Trainer objT)
        {
            try
            {
                if (Session["BranchCode"] == null || Session["CourseCode"] == null || Session["StaffCode"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    // Retrieve staff code from session
                    string staffCode = Session["StaffCode"].ToString();

                    // Call ListCourse_Bind method with staff code parameter
                    await ListCourse_Bind(staffCode);

                    BALTrainer objTrainer = new BALTrainer();
                    objT.BranchCode = Session["BranchCode"].ToString();
                    objT.RegisterDate = DateTime.Now;

                    foreach (var file in objT.Files ?? Enumerable.Empty<HttpPostedFileBase>())
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            var path = Path.Combine(Server.MapPath("~/Content/Trainer/docs/"), fileName);
                            file.SaveAs(path);
                            Trainer obj1 = new Trainer();
                            obj1.TestName = objT.TestName;
                            obj1.CourseName = objT.CourseName;
                            obj1.TestPaperFile = path;

                            // Pass staff code retrieved from session to AddTest method


                            obj1.SDuration = objT.SDuration;
                            obj1.TotalMarks = objT.TotalMarks;
                            obj1.PassingMarks = objT.PassingMarks;
                            obj1.TotalNoOfQuestion = objT.TotalNoOfQuestion;
                            obj1.TopicName = objT.TopicName;
                            BALTrainer objbal1 = new BALTrainer();
                            await objbal1.AddNewTestAsycTS(obj1, staffCode);
                        }
                    }
                    TempData["AlertMessage"] = "Employee Registration Successfully";
                    return RedirectToAction("ListTestManagementAsynchTS", new { BranchCode = objT.BranchCode, CourseCode = objT.CourseCode });
                }
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or rethrow if necessary
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Redirect to an error page or display a user-friendly error message
                return RedirectToAction("Error", "Home");
            }
        }


        /// <summary>
        /// Add(register) new test BAL Method-AddTest(Trainer objT)
        /// </summary>
        /// <param name="objT"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> UpdateAddNewTestAsnchTS(int id)
        {
            // Ensure that session variables for BranchCode, CourseCode, and StaffCode are available
            if (Session["BranchCode"] == null || Session["CourseCode"] == null || Session["StaffCode"] == null)
            {
                // Redirect to the login page if any of the session variables are missing
                return RedirectToAction("Login", "Account");
            }
            else
            {
                // Retrieve branch code, course code, and staff code from session
                string branchCode = Session["BranchCode"].ToString();
                string courseCode = Session["CourseCode"].ToString();
                string staffCode = Session["StaffCode"].ToString();

                Trainer objT = new Trainer();
                objT.TestId = id;



                // Call FechAddNewTest method with branch code, staff code, and test id parameters
                // SqlDataReader dr = await objy.FechAddNewTest(branchCode, staffCode, objT.TestId);
                Trainer fetchedTrainer = await objbal.FechAddNewTest(branchCode, staffCode, objT.TestId);


                if (fetchedTrainer != null)
                {
                    objT.TestId = fetchedTrainer.TestId;
                    objT.TestName = fetchedTrainer.TestName;
                    objT.CourseName = fetchedTrainer.CourseName;
                    objT.CourseCode = fetchedTrainer.CourseCode;
                    objT.SectionName = fetchedTrainer.SectionName;
                    objT.SectionId = fetchedTrainer.SectionId;
                    objT.TopicName = fetchedTrainer.TopicName;
                    objT.TotalMarks = fetchedTrainer.TotalMarks;
                    objT.PassingMarks = fetchedTrainer.PassingMarks;
                    objT.TopicId = fetchedTrainer.TopicId;
                    objT.RegisterDate = fetchedTrainer.RegisterDate;
                    objT.SDuration = fetchedTrainer.SDuration;
                    objT.TestPaperFile = fetchedTrainer.TestPaperFile;
                }

                BALTrainer obc = new BALTrainer();


                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
            {
                //new BreadcrumbItem { Name = "Home", Url = "/" },
                 new BreadcrumbItem { Name = "TrainerDashboard", Url = "TrainerDashboardAsyncTS/Trainer" },
               new BreadcrumbItem { Name = "Test Managment", Url = "ListTestManagementAsynchTS/Trainer" },
                new BreadcrumbItem { Name = " ALL Test Paper List", Url = "ListAddTestAsyncTs/Trainer" },
               new BreadcrumbItem { Name = " Edit Add Test", Url = "EditAddNewTestAsnchTS/Trainer" }, // Adjust URL as needed
            };

                ViewBag.Breadcrumbs = breadcrumbs;

                // return View(objT);
                return PartialView("UpdateAddNewTestAsnchTS", objT);
            }
        }
        /// <summary>
        /// post method of Edit Add new test 
        /// </summary>
        /// <param name="objT"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<ActionResult> UpdateAddNewTestAsnchTS(Trainer objT, HttpPostedFileBase NewFile, bool? DeleteFile)

        {

            if (Session["BranchCode"] == null || Session["CourseCode"] == null || Session["StaffCode"] == null)
            {

                return RedirectToAction("Login", "Account");
            }


            string staffCode = Session["StaffCode"].ToString();

            await ListCourse_Bind(staffCode);

            objT.RegisterDate = DateTime.Now;

            // Check if the user wants to delete the current file
            if (DeleteFile.HasValue && DeleteFile.Value && !string.IsNullOrEmpty(objT.TestPaperFile))
            {
                // Delete the current file
                var filePath = Path.Combine(Server.MapPath("~/Content/Trainer/docs/"), objT.TestPaperFile);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                objT.TestPaperFile = null; // Clear the file name
            }

            // Check if a new file is uploaded
            if (NewFile != null && NewFile.ContentLength > 0)
            {
                var fileName = Path.GetFileName(NewFile.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/Trainer/docs/"), fileName);

                // Save the new file to the server
                NewFile.SaveAs(path);

                // Update the file path in the Trainer object
                objT.TestPaperFile = fileName; // Save only the file name, not the full path
            }

            // Update the test details

            await objbal.UpdateAddNewTestAsyncTS(objT, staffCode);

            TempData["AlertMessage"] = "Test Updated Successfully";

            return PartialView("ListTestManagementAsynchTS", objT);
            // Redirect to the index or another action

        }



        public async Task<ActionResult> DeleteTest(int id)
        {
            // Create a Trainer object with the specified TestId
            Trainer objT = new Trainer();
            objT.TestId = id;

            // Create an instance of your BAL class


            // Call the deleteAddTest method from your BAL
            await objbal.deleteAddTest(objT);


            TempData["AlertMessage"] = "Test Deleted Successfully";

            // Redirect to a different action, if needed
            return RedirectToAction("ListAddTestAsyncTs", "Trainer");

        }


        /// <summary>
        /// this is list view of Add new tests
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public async Task<ActionResult> ListAddTestAsyncTs()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account"); // Redirect to login page if staff code is not found in session
            }
            else
            {
                string staffCode = Session["StaffCode"].ToString(); // Retrieve staff code from session
                string branchCode = Session["BranchCode"].ToString(); // Retrieve branch code from session


                DataSet ds = new DataSet();

                // Pass staff code and branch code to the method for fetching tests
                ds = await objbal.ListAddTestAsyncTs(branchCode, staffCode);

                Trainer objtr1 = new Trainer();
                List<Trainer> AddTestList1 = new List<Trainer>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Trainer objtn = new Trainer();

                    objtn.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objtn.TestId = Convert.ToInt32(ds.Tables[0].Rows[i]["TestId"].ToString());
                    objtn.TestName = ds.Tables[0].Rows[i]["TestName"].ToString();
                    objtn.TopicName = ds.Tables[0].Rows[i]["TopicName"].ToString();

                    try
                    {
                        objtn.RegisterDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["RegisterDate"].ToString());
                    }
                    catch (FormatException ex)
                    {
                        // Handle format exception
                    }

                    string totalMarksStr = ds.Tables[0].Rows[i]["TotalMarks"].ToString();
                    float totalMarks;
                    if (float.TryParse(totalMarksStr, out totalMarks))
                    {
                        objtn.TotalMarks = totalMarks;
                    }
                    else
                    {
                        // Handle the case where the conversion fails
                    }

                    AddTestList1.Add(objtn);
                }
                objtr1.AddTestList = AddTestList1;
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
            {
                //new BreadcrumbItem { Name = "Home", Url = "/" },
               new BreadcrumbItem { Name = " ALL Test Paper List", Url = "ListAddTestAsyncTs/Trainer" }, // Adjust URL as needed
            };

                ViewBag.Breadcrumbs = breadcrumbs;
                TempData["AlertMessage"] = "Add Test Successfully";
                return PartialView("ListAddTestAsyncTs", objtr1);
            }
        }


        /// <summary>
        /// All Test List conducted,pending Assign ,Arrenge BAL Method-AllTestList()
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> ListAllTestAsyncTS()
        {
            BALTrainer objTr = new BALTrainer();
            DataSet ds = new DataSet();

            ds = await objTr.AllTestList();
            Trainer objtr2 = new Trainer();
            List<Trainer> AllTestList1 = new List<Trainer>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Trainer objtm = new Trainer();
                objtm.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                objtm.TestName = ds.Tables[0].Rows[i]["TestName"].ToString();
                objtm.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();

                try
                {
                    objtm.TestDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["TestDate"].ToString());
                    objtm.TestTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["TestTime"].ToString());
                }
                catch (FormatException ex)
                {
                    // Handle the case where date or time conversion fails
                }

                objtm.Duration = ds.Tables[0].Rows[i]["Duration"].ToString();

                // Parsing TotalMarks safely using float.TryParse
                float totalMarks;
                if (float.TryParse(ds.Tables[0].Rows[i]["TotalMarks"].ToString(), out totalMarks))
                {
                    objtm.TotalMarks = totalMarks;
                }
                else
                {
                    // Handle the case where TotalMarks conversion fails
                    // For example, assign a default value or log the error
                }

                objtm.Status = ds.Tables[0].Rows[i]["Status"].ToString();
                AllTestList1.Add(objtm);
            }
            objtr2.AllTestList = AllTestList1;
            return PartialView("ListAllTestAsyncTS", objtr2);
        }
        /// <summary>
        /// List view of Test Conducted
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> ListConductedTestAsyncTS()
        {
            if (Session["StaffCode"] == null || Session["BranchCode"] == null)
            {
                return RedirectToAction("Login", "Account"); // Redirect to login page if staff code or branch code is not found in session
            }
            else
            {
                string staffCode = Session["StaffCode"].ToString(); // Retrieve staff code from session
                string branchCode = Session["BranchCode"].ToString(); // Retrieve branch code from session


                DataSet ds = new DataSet();

                // Pass both branch code and staff code to the method for fetching conducted tests
                ds = await objbal.ConductedList(branchCode, staffCode);

                Trainer objtr3 = new Trainer();
                List<Trainer> ConductedTestList1 = new List<Trainer>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Trainer objtm = new Trainer();

                    objtm.AssignTestId = Convert.ToInt32(row["AssignTestId"]);
                    objtm.CourseName = row["CourseName"].ToString();
                    objtm.TopicName = row["TopicName"].ToString();
                    objtm.TestName = row["TestName"].ToString();
                    objtm.BatchName = row["BatchName"].ToString();
                    objtm.BatchCode = row["BatchCode"].ToString();
                    objtm.LabName = row["LabName"].ToString();
                    objtm.StaffName = row["StaffName"].ToString();
                    objtm.TestDate = Convert.ToDateTime(row["TestDate"]);
                    objtm.TestTime = Convert.ToDateTime(row["TestTime"]);
                    objtm.Duration = row["Duration"].ToString();
                    objtm.TotalMarks = float.Parse(row["TotalMarks"].ToString());
                    objtm.Status = row["Status"].ToString();

                    // Check if TestResultExists
                    bool isResultAdded = Convert.ToInt32(row["IsResultAdded"]) == 1;
                    objtm.TestResultExists = !isResultAdded;

                    ConductedTestList1.Add(objtm);
                }
                objtr3.ConductedTestList = ConductedTestList1;
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
            {
                //new BreadcrumbItem { Name = "Home", Url = "/" },
               new BreadcrumbItem { Name = " Conducted Test List", Url = "ListConductedTestAsyncTS/Trainer" }, // Adjust URL as needed
            };

                ViewBag.Breadcrumbs = breadcrumbs;

                return PartialView("ListConductedTestAsyncTS", objtr3);
            }
        }
        /// <summary>
        /// get Action method of Add result of studends whose test is conducted
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet]
        public async Task<ActionResult> AddRResutltTSAsynch(int id)
        {
            try
            {
                // Ensure that session variables for BranchCode and StaffCode are available
                if (Session["BranchCode"] == null || Session["StaffCode"] == null)
                {
                    // Redirect to the login page if any of the session variables are missing
                    return RedirectToAction("Login", "Account");
                }

                // Retrieve branch code and staff code from session
                string branchCode = Session["BranchCode"].ToString();
                string staffCode = Session["StaffCode"].ToString();

                // Call resultDetails method with branch code, staff code, and test id parameters
                DataSet ds = await objbal.ResultDetailsAsyncTS(branchCode, staffCode, id);
                Trainer objP = new Trainer();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    objP.AssignTestId = Convert.ToInt32(ds.Tables[0].Rows[i]["AssignTestId"].ToString());
                    objP.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    objP.BatchCode = ds.Tables[0].Rows[i]["BatchCode"].ToString();
                    objP.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objP.TestName = ds.Tables[0].Rows[i]["TestName"].ToString();
                    objP.TestDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["TestDate"].ToString());
                    objP.TestTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["TestTime"].ToString());
                    objP.TotalMarks = float.Parse(ds.Tables[0].Rows[i]["TotalMarks"].ToString());
                    objP.PassingMarks = float.Parse(ds.Tables[0].Rows[i]["PassingMarks"].ToString());
                }

                // Call GetstudentResult method to get student results
                DataSet ds1 = await objbal.GetstudentResult(objP.BatchCode);
                List<SelectListItem> StudentList = new List<SelectListItem>();

                foreach (DataRow dr in ds1.Tables[0].Rows)
                {
                    StudentList.Add(new SelectListItem { Text = dr["FullName"].ToString(), Value = dr["CandidateCode"].ToString() });
                }

                ViewBag.Student = StudentList;
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
            {
                //new BreadcrumbItem { Name = "Home", Url = "/" },
                 new BreadcrumbItem { Name = "TrainerDashboard", Url = "TrainerDashboardAsyncTS/Trainer" },
               new BreadcrumbItem { Name = "Test Managment", Url = "ListTestManagementAsynchTS/Trainer" },
                new BreadcrumbItem { Name = " Conducted Test List", Url = "ListConductedTestAsyncTS/Trainer" },
               new BreadcrumbItem { Name = " Add Test Result", Url = "AddRResutltTSAsynch/Trainer" }, // Adjust URL as needed
            };

                ViewBag.Breadcrumbs = breadcrumbs;

                return View("SaveResultStudentTSAsynch", objP);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// post Action result of Add Result of Student
        /// </summary>
        /// <param name="studentDataArray"></param>
        /// <returns></returns>
        [HttpPost]

        public async Task<ActionResult> AddRResultTSAsynch(List<Trainer> studentDataArray)
        {
            try
            {
                // Check if studentDataArray is null
                if (studentDataArray == null)
                {
                    return Json(new { success = false, message = "No student data provided." });
                }

                // Ensure that session variables for BranchCode and StaffCode are available
                if (Session["BranchCode"] == null || Session["StaffCode"] == null)
                {
                    // Redirect to the login page if any of the session variables are missing
                    return Json(new { success = false, message = "Session variables missing. Please log in again." });
                }

                // Retrieve branch code and staff code from session
                string branchCode = Session["BranchCode"].ToString();
                string staffCode = Session["StaffCode"].ToString();



                // Loop through each studentData in studentDataArray
                foreach (var studentData in studentDataArray)
                {
                    // Call AddResult method for each studentData
                    await objbal.AddResult(new Trainer
                    {
                        AssignTestId = studentData.AssignTestId,
                        StudentCode = studentData.StudentCode,
                        ObtainedMarks = studentData.ObtainedMarks,
                        AttendanceStatusId = studentData.AttendanceStatusId,
                        Status = studentData.Status
                    }, staffCode);
                }

                // Return a success response if results are saved successfully
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

        /// <summary>
        /// display the result of student whose result added
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet]
        public async Task<ActionResult> ViewRResutltTSAsynch(int id)
        {
            try
            {
                // Ensure that session variables for BranchCode and StaffCode are available
                if (Session["BranchCode"] == null || Session["StaffCode"] == null)
                {
                    // Redirect to the login page if any of the session variables are missing
                    return RedirectToAction("Login", "Account");
                }

                // Retrieve branch code and staff code from session
                string branchCode = Session["BranchCode"].ToString();
                string staffCode = Session["StaffCode"].ToString();



                // Create a Trainer object to hold test details and student results
                Trainer objResult = new Trainer();
                objResult.AssignTestId = id;

                // Retrieve test details using DataSet
                DataSet testDetailsDataSet = await objbal.ResultDetailsAsyncTS(branchCode, staffCode, id);

                // Check if DataSet contains any tables
                if (testDetailsDataSet != null && testDetailsDataSet.Tables.Count > 0)
                {
                    DataTable testDetailsTable = testDetailsDataSet.Tables[0];

                    // Populate Trainer object with test details
                    if (testDetailsTable.Rows.Count > 0)
                    {
                        DataRow testDetailsRow = testDetailsTable.Rows[0];

                        objResult.CourseName = testDetailsRow["CourseName"].ToString();
                        objResult.TopicName = testDetailsRow["TopicName"].ToString();
                        objResult.TopicId = Convert.ToInt32(testDetailsRow["TopicId"]);
                        objResult.TestName = testDetailsRow["TestName"].ToString();


                        float totalMarks;
                        if (float.TryParse(testDetailsRow["TotalMarks"].ToString(), out totalMarks))
                        {
                            objResult.TotalMarks = totalMarks;
                        }

                        objResult.BatchName = testDetailsRow["BatchName"].ToString();
                        objResult.BatchCode = testDetailsRow["BatchCode"].ToString();

                        DateTime testDate;
                        if (DateTime.TryParse(testDetailsRow["TestDate"].ToString(), out testDate))
                        {
                            objResult.TestDate = testDate;
                        }

                        DateTime testTime;
                        if (DateTime.TryParse(testDetailsRow["TestTime"].ToString(), out testTime))
                        {
                            objResult.TestTime = testTime;
                        }
                    }
                }

                // Retrieve student results
                DataSet studentResultsDataSet = await objbal.ShowResultStudent(objResult);
                List<Trainer> ResultList1 = new List<Trainer>();

                // Iterate through the dataset to populate the student results
                foreach (DataRow row in studentResultsDataSet.Tables[0].Rows)
                {
                    Trainer objtb = new Trainer();
                    objtb.StudentName = row["FullName"].ToString();
                    objtb.Attendance = row["Status"].ToString();
                    objtb.ObtainedMarks = float.Parse(row["ObtainedMarks"].ToString());
                    objtb.Result = row["ResultStatus"].ToString();
                    ResultList1.Add(objtb);
                }

                // Add the student results to the Trainer object
                objResult.ResultList = ResultList1;

                TempData["AlertMessage"] = "Test details and student results retrieved successfully.";

                // Pass the Trainer object to the view
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
            {
                //new BreadcrumbItem { Name = "Home", Url = "/" },
                 new BreadcrumbItem { Name = "TrainerDashboard", Url = "TrainerDashboardAsyncTS/Trainer" },
               new BreadcrumbItem { Name = "Test Managment", Url = "ListTestManagementAsynchTS/Trainer" },
                new BreadcrumbItem { Name = " Conducted Test List", Url = "ListConductedTestAsyncTS/Trainer" },
               new BreadcrumbItem { Name = " View Test List", Url = "ViewRResutltTSAsynch/Trainer" }, // Adjust URL as needed
            };

                ViewBag.Breadcrumbs = breadcrumbs;
                return PartialView("ViewRResutltTSAsynch", objResult);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                TempData["ErrorMessage"] = "An error occurred while retrieving data.";
                return RedirectToAction("Error", "Home"); // Redirect to an error page
            }
        }



        /// <summary>
        /// Pending test List BAL method-PendingList()
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> ListPendingTestAsyncTs()
        {
            if (Session["StaffCode"] == null || Session["BranchCode"] == null)
            {
                return RedirectToAction("Login", "Account"); // Redirect to login page if staff code or branch code is not found in session
            }
            else
            {
                string staffCode = Session["StaffCode"].ToString(); // Retrieve staff code from session
                string branchCode = Session["BranchCode"].ToString(); // Retrieve branch code from session


                DataSet ds = new DataSet();

                // Pass both branch code and staff code to the method for fetching pending tests
                ds = await objbal.ListPendingTestAsync(branchCode, staffCode);

                Trainer objtr4 = new Trainer();
                List<Trainer> PendingTestList1 = new List<Trainer>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Trainer objts = new Trainer();

                    objts.AssignTestId = Convert.ToInt32(ds.Tables[0].Rows[i]["AssignTestId"].ToString());
                    objts.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objts.TestName = ds.Tables[0].Rows[i]["TestName"].ToString();
                    objts.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    objts.LabName = ds.Tables[0].Rows[i]["LabName"].ToString();
                    objts.StaffName = ds.Tables[0].Rows[i]["StaffName"].ToString();
                    objts.TestDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["TestDate"].ToString());

                    objts.Duration = ds.Tables[0].Rows[i]["Duration"].ToString();
                    objts.TotalMarks = float.Parse(ds.Tables[0].Rows[i]["TotalMarks"].ToString());
                    objts.Status = ds.Tables[0].Rows[i]["Status"].ToString();
                    PendingTestList1.Add(objts);
                }
                objtr4.PendingTestList = PendingTestList1;
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
            {
                //new BreadcrumbItem { Name = "Home", Url = "/" },
               new BreadcrumbItem { Name = " Pending Test List", Url = "ListPendingTestAsyncTs/Trainer" }, // Adjust URL as needed
            };

                ViewBag.Breadcrumbs = breadcrumbs;

                return PartialView("ListPendingTestAsyncTs", objtr4);
            }
        }

        /// <summary>
        /// details of pednding test from AssignTestId
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpGet]
        public async Task<ActionResult> ViewPendingTSAsynch(int id)
        {
            try
            {
                // Ensure that session variables for BranchCode and StaffCode are available
                if (Session["BranchCode"] == null || Session["StaffCode"] == null)
                {
                    // Redirect to the login page if any of the session variables are missing
                    return RedirectToAction("Login", "Account");
                }

                // Retrieve branch code and staff code from session
                string branchCode = Session["BranchCode"].ToString();
                string staffCode = Session["StaffCode"].ToString();

                Trainer objp = new Trainer();
                objp.AssignTestId = id;

                // Call the viewPendingList method with branch code, staff code, and test id parameters
                SqlDataReader dr = await objbal.DetailsPendingListAsycTS(branchCode, staffCode, id);

                while (dr.Read())
                {
                    objp.AssignTestId = Convert.ToInt32(dr["AssignTestId"].ToString());
                    objp.CourseName = dr["CourseName"].ToString();
                    objp.TestName = dr["TestName"].ToString();
                    objp.BatchName = dr["BatchName"].ToString();
                    objp.LabName = dr["LabName"].ToString();
                    objp.StaffName = dr["StaffName"].ToString();
                    objp.Duration = dr["Duration"].ToString();
                    objp.TotalMarks = float.Parse(dr["TotalMarks"].ToString());
                    objp.TestDate = DateTime.Parse(dr["TestDate"].ToString());

                    objp.TestTime = DateTime.Parse(dr["TestTime"].ToString());


                }

                // Close the SqlDataReader
                dr.Close();
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
            {
                //new BreadcrumbItem { Name = "Home", Url = "/" },
                 new BreadcrumbItem { Name = "TrainerDashboard", Url = "TrainerDashboardAsyncTS/Trainer" },
               new BreadcrumbItem { Name = "Test Managment", Url = "ListTestManagementAsynchTS/Trainer" },
                new BreadcrumbItem { Name = " Pending Test List", Url = "ListPendingTestAsyncTs/Trainer" },
               new BreadcrumbItem { Name = "Details PendingTest List", Url = "ViewPendingTSAsynch/Trainer" }, // Adjust URL as needed
            };

                ViewBag.Breadcrumbs = breadcrumbs;
                return PartialView("ViewPendingTSAsynch", objp);
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                return View("ErrorView"); // You can return a specific error view or handle the exception accordingly
            }
        }
        /// <summary>
        /// Arrenge test List BAL Method -ArrengedList()
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> ListArrengeAsynchTS()
        {
            if (Session["StaffCode"] == null || Session["BranchCode"] == null)
            {
                return RedirectToAction("Login", "Account"); // Redirect to login page if staff code or branch code is not found in session
            }
            else
            {
                string staffCode = Session["StaffCode"].ToString(); // Retrieve staff code from session
                string branchCode = Session["BranchCode"].ToString(); // Retrieve branch code from session


                DataSet ds = new DataSet();

                // Pass both branch code and staff code to the method for fetching arranged tests
                ds = await objbal.ArrengedList(branchCode, staffCode);

                Trainer objtr5 = new Trainer();
                List<Trainer> ArrengeTestList1 = new List<Trainer>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Trainer objts = new Trainer();

                    objts.AssignTestId = Convert.ToInt32(ds.Tables[0].Rows[i]["AssignTestId"].ToString());
                    objts.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objts.TestName = ds.Tables[0].Rows[i]["TestName"].ToString();
                    objts.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    objts.LabName = ds.Tables[0].Rows[i]["LabName"].ToString();
                    objts.StaffName = ds.Tables[0].Rows[i]["StaffName"].ToString();
                    objts.TotalMarks = float.Parse(ds.Tables[0].Rows[i]["TotalMarks"].ToString());
                    objts.TestDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["TestDate"].ToString());
                    objts.TestTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["TestTime"].ToString());
                    //try
                    //{
                    //    objts.TestDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["TestDate"].ToString());
                    //    objts.TestTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["TestTime"].ToString());
                    //}
                    //catch (FormatException ex)
                    //{
                    //    // Handle format exception
                    //}
                    objts.Duration = ds.Tables[0].Rows[i]["Duration"].ToString();


                    //string totalMarksStr = ds.Tables[0].Rows[i]["TotalMarks"].ToString();
                    //float totalMarks;
                    //if (float.TryParse(totalMarksStr, out totalMarks))
                    //{
                    //    objts.TotalMarks = totalMarks;
                    //}
                    //else
                    //{
                    //    // Handle the case where the conversion fails
                    //}

                    objts.Status = ds.Tables[0].Rows[i]["Status"].ToString();
                    ArrengeTestList1.Add(objts);
                }
                objtr5.ArrengeTestList = ArrengeTestList1;
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
            {
                //new BreadcrumbItem { Name = "Home", Url = "/" },
                 new BreadcrumbItem { Name = "TrainerDashboard", Url = "TrainerDashboardAsyncTS/Trainer" },
               new BreadcrumbItem { Name = "Test Managment", Url = "ListTestManagementAsynchTS/Trainer" },
               new BreadcrumbItem { Name = "Arrange", Url = "ListArrengeAsynchTS/Trainer" }, // Adjust URL as needed
            };

                ViewBag.Breadcrumbs = breadcrumbs;

                return PartialView("ListArrengeAsynchTS", objtr5);
            }
        }
        /// <summary>
        /// view details of Arrange test list from AssignTestId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> ViewArrangeTSAsynch(int id)
        {
            try
            {
                // Ensure that session variables for BranchCode and StaffCode are available
                if (Session["BranchCode"] == null || Session["StaffCode"] == null)
                {
                    // Redirect to the login page if any of the session variables are missing
                    return RedirectToAction("Login", "Account");
                }

                // Retrieve branch code and staff code from session
                string branchCode = Session["BranchCode"].ToString();
                string staffCode = Session["StaffCode"].ToString();

                Trainer objp = new Trainer();
                objp.AssignTestId = id;

                // Call the viewArregeList method with branch code, staff code, and test id parameters
                SqlDataReader dr = await objbal.DetailsArregeListAsncTS(branchCode, staffCode, id);

                while (dr.Read())
                {
                    objp.AssignTestId = Convert.ToInt32(dr["AssignTestId"].ToString());
                    objp.CourseName = dr["CourseName"].ToString();
                    objp.TestName = dr["TestName"].ToString();
                    objp.BatchName = dr["BatchName"].ToString();
                    objp.LabName = dr["LabName"].ToString();
                    objp.StaffName = dr["StaffName"].ToString();
                    objp.Duration = dr["Duration"].ToString();
                    objp.TotalMarks = float.Parse(dr["TotalMarks"].ToString());
                    objp.TestDate = DateTime.Parse(dr["TestDate"].ToString());

                    objp.TestTime = DateTime.Parse(dr["TestTime"].ToString());

                    //DateTime testDate;
                    //if (DateTime.TryParse(dr["TestDate"].ToString(), out testDate))
                    //{
                    //    objp.TestDate = testDate;
                    //}

                    //DateTime testTime;
                    //if (DateTime.TryParse(dr["TestTime"].ToString(), out testTime))
                    //{
                    //    objp.TestTime = testTime;
                    //}
                }

                // Close the SqlDataReader
                dr.Close();
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
            {
                //new BreadcrumbItem { Name = "Home", Url = "/" },
                 new BreadcrumbItem { Name = "TrainerDashboard", Url = "TrainerDashboardAsyncTS/Trainer" },
               new BreadcrumbItem { Name = "Test Managment", Url = "ListTestManagementAsynchTS/Trainer" },
                new BreadcrumbItem { Name = "Arrange", Url = "ListArrengeAsynchTS/Trainer" },
               new BreadcrumbItem { Name = "Details Arrange Test List", Url = "ViewArrangeTSAsynch/Trainer" }, // Adjust URL as needed
            };

                ViewBag.Breadcrumbs = breadcrumbs;
                return PartialView("ViewArrangeTSAsynch", objp);
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                return View("ErrorView"); // You can return a specific error view or handle the exception accordingly
            }
        }








        //[HttpGet]
        //public ActionResult ListallTestManagementAsynchTS()
        //{
        //    return PartialView();
        //}

        [HttpGet]
        public ActionResult ListTestManagementAsynchTS(string BranchCode)
        {
            try
            {
                if (Session["BranchCode"] == null || Session["CourseCode"] == null || Session["StaffCode"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    string CourseCode = Session["CourseCode"].ToString(); // Retrieve course code from session
                    string StaffCode = Session["StaffCode"].ToString();

                    List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
            {
                //new BreadcrumbItem { Name = "Home", Url = "/" },
                new BreadcrumbItem { Name = "TrainerDashboard", Url = "TrainerDashboardAsyncTS/Trainer" },
               new BreadcrumbItem { Name = "Test Managment", Url = "ListTestManagementAsynchTS/Trainer" }, // Adjust URL as needed
            };

                    ViewBag.Breadcrumbs = breadcrumbs;
                    return View();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or rethrow if necessary
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Redirect to an error page or display a user-friendly error message
                return RedirectToAction("Error", "Home");
            }
        }





        /// <summary>
        /// add (register)form result 
        /// </summary>
        /// <returns></returns>
        /// 


        [HttpGet]
        public ActionResult SaveResultStudentTSAsynch()
        {
            return View();

        }

        [HttpGet]
        public async Task<ActionResult> RegisterAssignTestTS()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                string staffCode = Session["StaffCode"].ToString();
                await ListCourse_Bind(staffCode);

                Trainer objT = new Trainer();
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
            {
                //new BreadcrumbItem { Name = "Home", Url = "/" },
                 new BreadcrumbItem { Name = "TrainerDashboard", Url = "TrainerDashboardAsyncTS/Trainer" },
               new BreadcrumbItem { Name = "Test Managment", Url = "ListTestManagementAsynchTS/Trainer" },
               new BreadcrumbItem { Name = "  Register Assign Test", Url = "RegisterAssignTestTS/Trainer" }, // Adjust URL as needed
            };

                ViewBag.Breadcrumbs = breadcrumbs;
                return View(objT);
            }
        }
        [HttpPost]
        public async Task<ActionResult> RegisterAssignTestTS(Trainer objT)
        {
            try
            {
                if (Session["BranchCode"] == null || Session["CourseCode"] == null || Session["StaffCode"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {

                    // Retrieve the staff code from the session
                    string staffCode = Session["StaffCode"].ToString();
                    objT.TrainerCodeAssignedByCode = Session["StaffCode"].ToString();
                    // Call the ResisterAssignTtest method
                    await objbal.ResisterAssignTtest(objT, staffCode);

                    // Optionally, redirect to a success page or return a success response
                    return RedirectToAction("RegisterAssignTestTS");
                }
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or rethrow if necessary
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Redirect to an error page or display a user-friendly error message
                return RedirectToAction("Error", "Home");
            }
        }
        /// <summary>
        /// List of Assign Test 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> ListAssignAsynchTS()
        {
            if (Session["StaffCode"] == null || Session["BranchCode"] == null)
            {
                return RedirectToAction("Login", "Account"); // Redirect to login page if staff code or branch code is not found in session
            }
            else
            {
                string staffCode = Session["StaffCode"].ToString(); // Retrieve staff code from session
                string branchCode = Session["BranchCode"].ToString(); // Retrieve branch code from session


                DataSet ds = new DataSet();

                // Pass both branch code and staff code to the method for fetching assigned tests
                ds = await objbal.ListAssignTestAsycTS(branchCode, staffCode);

                Trainer objtr6 = new Trainer();
                List<Trainer> AssignTestList1 = new List<Trainer>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Trainer objts = new Trainer();

                    objts.AssignTestId = Convert.ToInt32(ds.Tables[0].Rows[i]["AssignTestId"].ToString());
                    objts.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objts.TestName = ds.Tables[0].Rows[i]["TestName"].ToString();
                    objts.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    objts.TopicName = ds.Tables[0].Rows[i]["TopicName"].ToString();
                    objts.Duration = ds.Tables[0].Rows[i]["Duration"].ToString();
                    objts.TotalMarks = float.Parse(ds.Tables[0].Rows[i]["TotalMarks"].ToString());


                    AssignTestList1.Add(objts);
                }
                objtr6.AssignTestList = AssignTestList1;
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
            {
                //new BreadcrumbItem { Name = "Home", Url = "/" },
               new BreadcrumbItem { Name = "Assign Test List", Url = "ListAssignAsynchTS/Trainer" }, // Adjust URL as needed
            };

                ViewBag.Breadcrumbs = breadcrumbs;

                return PartialView("ListAssignAsynchTS", objtr6);
            }
        }
        /// <summary>
        /// Detail view of Assig test list for Edit purpose from   AssignTestId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet]
        public async Task<ActionResult> UpdateAssignTestAsnchTS(int id)
        {
            // Ensure that session variables for BranchCode and StaffCode are available
            if (Session["BranchCode"] == null || Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account"); // Redirect to login page if staff code or branch code is not found in session
                                                             // Handle the case where session variables are missing
                                                             // Redirect or return an appropriate view
            }
            else
            {
                // Retrieve branch code and staff code from session
                string branchCode = Session["BranchCode"].ToString();
                string staffCode = Session["StaffCode"].ToString();

                Trainer objT = new Trainer();
                objT.AssignTestId = id;



                DataSet ds = await objbal.fetchAssignedTest(branchCode, staffCode, objT.AssignTestId);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0]; // Assuming only one row is returned

                    objT.AssignTestId = Convert.ToInt32(dr["AssignTestId"]);
                    objT.TestName = dr["TestName"].ToString();
                    objT.TestId = Convert.ToInt32(dr["TestId"]);
                    objT.CourseName = dr["CourseName"].ToString();
                    objT.CourseCode = dr["CourseCode"].ToString();
                    objT.SectionName = dr["SectionName"].ToString();
                    objT.SectionId = Convert.ToInt32(dr["SectionId"]);
                    objT.TopicName = dr["TopicName"].ToString();
                    objT.TopicId = Convert.ToInt32(dr["TopicId"]);
                    objT.BatchName = dr["BatchName"].ToString();
                    // objT.BatchCode = dr["BatchCode"].ToString();

                    // objT.RegisterDate = DateTime.Parse(dr["RegisterDate"].ToString());
                }


                BALTrainer obc = new BALTrainer();

                // Call ListCourse_Bind method passing staffCode
                await ListCourse_Bind(staffCode);



                DataSet dsBatch = await objbal.ListBatch(objT);
                List<SelectListItem> Batchlist = new List<SelectListItem>();
                foreach (DataRow dr9 in dsBatch.Tables[0].Rows)
                {
                    Batchlist.Add(new SelectListItem { Text = dr9["BatchName"].ToString(), Value = dr9["BatchCode"].ToString() });
                }
                ViewBag.Batchlist1 = Batchlist;


                DataSet dsSection = await objbal.GetSection(objT);
                List<SelectListItem> sectionlist = new List<SelectListItem>();
                foreach (DataRow dr2 in dsSection.Tables[0].Rows)
                {
                    sectionlist.Add(new SelectListItem { Text = dr2["SectionName"].ToString(), Value = dr2["SectionId"].ToString() });
                }
                ViewBag.sectionlist3 = sectionlist;

                // Check if objT.SectionId needs to be set here


                DataSet dsTopic = await objbal.GetTopic(objT);  // Check if objT.SectionId needs to be used
                List<SelectListItem> topiclist = new List<SelectListItem>();

                foreach (DataRow dr3 in dsTopic.Tables[0].Rows)
                {
                    topiclist.Add(new SelectListItem { Text = dr3["TopicName"].ToString(), Value = dr3["TopicId"].ToString() });
                }

                ViewBag.topic = topiclist;


                DataSet dsTest = await objbal.GetTestNameforAssign(objT);
                List<SelectListItem> testlist = new List<SelectListItem>();
                foreach (DataRow dr7 in dsTest.Tables[0].Rows)
                {
                    testlist.Add(new SelectListItem { Text = dr7["TestName"].ToString(), Value = dr7["TestId"].ToString() });
                }
                ViewBag.test1 = testlist;
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
            {
                //new BreadcrumbItem { Name = "Home", Url = "/" },
                  new BreadcrumbItem { Name = "TrainerDashboard", Url = "TrainerDashboardAsyncTS/Trainer" },
               new BreadcrumbItem { Name = "Test Managment", Url = "ListTestManagementAsynchTS/Trainer" },
                new BreadcrumbItem { Name = "Assign Test List", Url = "ListAssignAsynchTS/Trainer" },
               new BreadcrumbItem { Name = "  Edit Assign Test", Url = "EditAssignTestAsnchTS/Trainer" }, // Adjust URL as needed
            };

                ViewBag.Breadcrumbs = breadcrumbs;

                return PartialView("UpdateAssignTestAsnchTS", objT);
            }
        }

        /// <summary>
        /// Update and Save data of Assign test
        /// </summary>
        /// <param name="objT"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> UpdateAssignTestAsnchTS(Trainer objT)
        {
            // Ensure that session variables for StaffCode is available
            if (Session["StaffCode"] == null)
            {
                // Handle the case where session variables are missing
                // Redirect or return an appropriate view
            }

            // Retrieve staff code from session
            string staffCode = Session["StaffCode"].ToString();

            await ListCourse_Bind(staffCode); // Assuming you have a Course_Bind method similar to the Register action



            // objT.RegisterDate = DateTime.Now;

            await objbal.UpdateAssignTest(objT);

            TempData["AlertMessage"] = "Test Updated Successfully";

            return RedirectToAction("ListTestManagementAsynchTS", "Trainer");
        }


        [HttpGet]
        public async Task<JsonResult> GetstudentResult(string BatchCode)
        {


            DataSet ds = await objbal.GetstudentResult(BatchCode);
            List<SelectListItem> Studentresult = new List<SelectListItem>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Studentresult.Add(new SelectListItem { Text = dr["FullName"].ToString(), Value = dr["CandidateCode"].ToString() });
            }
            return Json(Studentresult, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// from conducted list click result action
        /// </summary>
        /// <param name="objT"></param>
        /// <returns></returns>

        //-------------------------- Tushar Test Management End ----------------------//

        //-------------------------- Pratibha Project Management Start ----------------------//
        //-------------------------- Pratibha Project Management Start ----------------------//

        public class ErrorViewModel
        {
            public string ErrorMessage { get; set; }
        }
        /// <summary>
        /// Retrieves a list of assigned projects and displays them in a gridview.
        /// </summary>
        /// <returns>
        /// Returns a gridviwview containing the list of assigned projects, including details such as
        /// project ID, course name, batch name, project name, assigned date, duration, and status.
        /// </returns>
        [HttpGet]
        public async Task<ActionResult> ListAllAssignedProjectAsyncPG(string courseName = null)
        {
            // Trainer objT = new Trainer();
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                try
                {
                    await CourceName_Bind();
                    Trainer objT = new Trainer();
                    objT.BranchCode = Session["BranchCode"].ToString();
                    objT.StaffCode = Session["StaffCode"].ToString();
                    DataSet ds = await objbal.ListAllAssignedProjectAsyncPG(objT, courseName);
                    List<Trainer> LstAssignedProject = new List<Trainer>();

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        Trainer objT1 = new Trainer();
                        objT1.AssignProjectId = Convert.ToInt32(row["AssignProjectId"].ToString());
                        objT1.CourceName = row["CourseName"].ToString();
                        objT1.BatchName = row["BatchName"].ToString();
                        objT1.ProjectName = row["ProjectName"].ToString();
                        objT1.AssignDate = Convert.ToDateTime(row["AssignedDate"].ToString());
                        objT1.Duration = row["Duration"].ToString();
                        objT1.StatusName = row["Status"].ToString();
                        LstAssignedProject.Add(objT1);
                    }
                    objT.lstAssignedProject = LstAssignedProject;
                    List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                    new BreadcrumbItem { Name = "TrainerDashboard", Url =Url.Action("TrainerDashboardAsyncTS","Trainer") },
                    new BreadcrumbItem { Name = "AssignProject", Url =  Url.Action("ListAllAssignedProjectAsyncPG", "Trainer") }
                };

                    ViewBag.Breadcrumbs = breadcrumbs;


                    return await Task.Run(() => PartialView(objT));
                }
                catch (Exception ex)
                {
                    return View("Error", new ErrorViewModel { ErrorMessage = ex.Message });
                }
            }
        }

        /// <summary>
        /// Retrieves and displays a list of students assigned to a specific project.
        /// </summary>
        /// <param name="AssignProjectId">The unique identifier of the assigned project.it is used to return students </param>
        /// <returns>
        /// Returns a partial view containing the list of students for the specified project, 
        /// including details such as student name, assigned project ID, batch name, and project name.
        /// </returns>
        [HttpGet]
        public async Task<ActionResult> ListAssignProjectStudentAsyncPG(int AssignProjectId)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                try
                {
                    Trainer objT = new Trainer();
                    objT.AssignProjectId = AssignProjectId;
                    objT.StaffCode = Session["StaffCode"].ToString();
                    objT.BranchCode = Session["BranchCode"].ToString();
                    List<Trainer> LstAssignedProject = new List<Trainer>();
                    DataSet ds = new DataSet();
                    ds = await objbal.ListAssignProjectStudentAsyncPG(objT);

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Trainer objT1 = new Trainer();
                        objT1.AssignProjectId = Convert.ToInt32(ds.Tables[0].Rows[i]["AssignProjectId"].ToString());
                        objT1.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                        objT1.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                        objT1.ProjectName = ds.Tables[0].Rows[i]["ProjectName"].ToString();
                        LstAssignedProject.Add(objT1);
                    }
                    objT.lstStudentList = LstAssignedProject;
                    return await Task.Run(() => PartialView(objT));
                }
                catch (Exception ex)
                {
                    return View("Error", new ErrorViewModel { ErrorMessage = ex.Message });
                }
            }
        }
        /// <summary>
        /// Retrieves and displays a list of all projects.
        /// </summary>
        /// <returns>
        /// Returns a view containing the list of all projects, including details such as project ID,
        /// project name, date, duration, technology, methodology, project owner, and staff name.
        /// </returns>
        [HttpGet]
        public async Task<ActionResult> ListAllProjectsAsyncPG()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                try
                {
                    Trainer objT = new Trainer();
                    objT.StaffCode = Session["StaffCode"].ToString();
                    objT.BranchCode = Session["BranchCode"].ToString();
                    DataSet ds = await objbal.ListAllProjectsAsyncPG(objT);
                    List<Trainer> LstAllProjects = new List<Trainer>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Trainer objT1 = new Trainer();
                        objT1.Projectid = Convert.ToInt32(ds.Tables[0].Rows[i]["ProjectId"].ToString());
                        objT1.ProjectName = ds.Tables[0].Rows[i]["ProjectName"].ToString();
                        objT1.Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString());
                        objT1.Duration = ds.Tables[0].Rows[i]["Duration"].ToString();
                        objT1.Technology = ds.Tables[0].Rows[i]["TechnologyName"].ToString();
                        objT1.Methodology = ds.Tables[0].Rows[i]["MethodologyName"].ToString();
                        objT1.ProjectOwner = ds.Tables[0].Rows[i]["ClientName"].ToString();
                        objT1.StaffName = ds.Tables[0].Rows[i]["StaffName"].ToString();
                        LstAllProjects.Add(objT1);
                    }
                    objT.lstAllProjects = LstAllProjects;
                    List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                   new BreadcrumbItem { Name = "TrainerDashboard", Url =Url.Action("TrainerDashboardAsyncTS","Trainer") },
                     new BreadcrumbItem { Name = "AssignProject",Url =  Url.Action("ListAllAssignedProjectAsyncPG","Trainer")},
                     new BreadcrumbItem { Name = "AllProjects",Url =  Url.Action("ListAllProjectsAsyncPG","Trainer") }
                };
                    ViewBag.Breadcrumbs = breadcrumbs;
                    return await Task.Run(() => View(objT));
                }
                catch (Exception ex)
                {
                    return View("Error", new ErrorViewModel { ErrorMessage = ex.Message });
                }
            }
        }
        /// <summary>
        /// Binds course names from the database to the ViewBag for use in the dropdown.
        /// </summary>
        [HttpGet]
        public async Task CourceName_Bind()
        {
            Trainer objT = new Trainer();
            objT.BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await objbal.FetchCourceName(objT);
            List<SelectListItem> courceList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                courceList.Add(new SelectListItem { Text = dr["CourseName"].ToString(), Value = dr["Coursecode"].ToString() });
            }
            ViewBag.CourceName = courceList;
        }
        /// <summary>
        /// Fetches batch names for use in assigning projects and binds them to the ViewBag for use in the dropdown.
        /// </summary>
        [HttpGet]
        public async Task<JsonResult> BatchName_Bind(string Coursecode)
        {
            Trainer objT = new Trainer();
            objT.CourseCode = Coursecode;
            objT.BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await objbal.FetchBatchName(objT);
            List<SelectListItem> BatchNameList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                BatchNameList.Add(new SelectListItem { Text = dr["BatchName"].ToString(), Value = dr["BatchCode"].ToString() });
            }
            // ViewBag.BatchName = BatchNameList;
            return Json(BatchNameList, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Binds trainer names from the database to the ViewBag for use in the dropdown.
        /// </summary>
        [HttpGet]
        public async Task AssignTrainerName_Bind()
        {
            Trainer objT = new Trainer();
            objT.BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await objbal.FetchAssignTrainer(objT);
            List<SelectListItem> TrainerList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TrainerList.Add(new SelectListItem { Text = dr["StaffName"].ToString(), Value = dr["StaffCode"].ToString() });
            }
            ViewBag.AssignTrainer = TrainerList;
        }
        /// <summary>
        /// Binds project names from the database to the ViewBag for use in the dropdown.
        /// </summary>
        [HttpGet]
        public async Task ProjectName_Bind()
        {
            Trainer objT = new Trainer();
            objT.BranchCode = Session["BranchCode"].ToString();
            DataSet ds = await objbal.FetchProjectName(objT);
            List<SelectListItem> ProjectNameList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ProjectNameList.Add(new SelectListItem { Text = dr["ProjectName"].ToString(), Value = dr["ProjectCode"].ToString() });
            }
            ViewBag.ProjectName = ProjectNameList;
        }
        [HttpGet]
        public async Task<JsonResult> ProjectDuration(string ProjectCode)
        {
            Trainer objT = new Trainer();
            objT.BranchCode = Session["BranchCode"].ToString();
            objT.ProjectCode = ProjectCode;
            SqlDataReader dr = await objbal.ProjectDuration(objT);
            if (dr.Read())
            {
                objT.Duration = dr["Duration"].ToString();
            }
            return Json(new { Duration = objT.Duration }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Retrieves and returns the list of students for a Add batch as a JsonResult.
        /// </summary>
        /// <param name="SelectStudent">The selected student for batch addition.</param>
        /// <returns>
        /// Returns a JsonResult containing the updated list of students after adding the selected student to the batch.
        /// </returns>
        [HttpGet]
        public async Task<JsonResult> AddBatchStudent(string SelectStudent)
        {
            Trainer objT = new Trainer();
            objT.BatchCode = SelectStudent;
            objT.BranchCode = Session["BranchCode"].ToString();
            DataSet ds = new DataSet();
            ds = await objbal.AddBatchStudent(objT);
            List<SelectListItem> StudentList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                StudentList.Add(new SelectListItem
                {
                    Text = dr["FullName"].ToString(),
                    Value = dr["CandidateCode"].ToString()
                });
            }
            ViewBag.AddStudent = StudentList;
            return Json(StudentList, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Retrieves and returns the list of students for a selected batch as a JsonResult.
        /// </summary>
        /// <param name="selectedBatch">The code of the selected batch.</param>
        /// <returns>
        /// Returns a JsonResult containing the list of students for the selected batch, 
        /// including details such as student name and code.
        /// </returns>
        [HttpGet]
        public async Task<JsonResult> SelectBatchStudent(string AddStudent)
        {
            Trainer objT = new Trainer();
            objT.BranchCode = Session["BranchCode"].ToString();
            objT.BatchCode = AddStudent;
            DataSet ds = new DataSet();
            ds = await objbal.AddBatchStudent(objT);
            List<SelectListItem> StudentList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                StudentList.Add(new SelectListItem
                {
                    Text = dr["FullName"].ToString(),
                    Value = dr["CandidateCode"].ToString()
                });
            }
            ViewBag.AddStudent = StudentList;
            return Json(StudentList, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// This action method handles HTTP GET requests for registering and assigning a project asynchronously.
        /// </summary>
        /// <returns>Returns an ActionResult representing the view for the registration and assignment of projects.</returns>
        [HttpGet]
        public async Task<ActionResult> RegisterAssignProjectAsyncPG()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await CourceName_Bind();
                await AssignTrainerName_Bind();
                await ProjectName_Bind();
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                   new BreadcrumbItem { Name = "TrainerDashboard", Url =Url.Action("TrainerDashboardAsyncTS","Trainer") },
                     new BreadcrumbItem { Name = "AssignProject",Url =  Url.Action("ListAllAssignedProjectAsyncPG","Trainer")},
                     new BreadcrumbItem { Name = "RegisterAssignProject",Url =  Url.Action("RegisterAssignProjectAsyncPG","Trainer") }
                };

                ViewBag.Breadcrumbs = breadcrumbs;
                return View();
            }
        }
        /// <summary>
        /// This action method handles HTTP POST requests for registering and assigning a project asynchronously.
        /// </summary>
        /// <param name="objT">An instance of the Trainer class representing the data submitted through the POST request.</param>
        /// <returns>Returns an ActionResult, typically redirecting to the "ListAllAssignedProjectAsyncPG" action.</returns>
        [HttpPost]
        public async Task<ActionResult> RegisterAssignProjectAsyncPG(Trainer objT)
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
                    objT.BranchCode = Session["BranchCode"].ToString();
                    await objbal.RegisterAssignProjectAsyncPG(objT);
                    return Json(new { success = true, message = "Data saved successfully" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "An error occurred while saving data: " + ex.Message });
                }
            }
        }

        /// <summary>
        /// This action method handles HTTP GET requests for updating an assigned project asynchronously.
        /// </summary>
        /// <param name="id">The identifier of the assigned project to be updated.</param>
        /// <returns>Returns an ActionResult representing the view for updating the assigned project.</returns>
        [HttpGet]
        public async Task<ActionResult> UpdateAssignProjectAsyncPG(int id)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await AssignTrainerName_Bind();
                await ProjectName_Bind();
                Trainer objT = new Trainer();
                objT.AssignProjectId = id;
                objT.StaffCode = Session["StaffCode"].ToString();
                objT.BranchCode = Session["BranchCode"].ToString();
                SqlDataReader dr;
                dr = await objbal.FetchAssignProjectAsyncPG(objT);
                while (dr.Read())
                {
                    objT.AssignProjectId = Convert.ToInt32(dr["AssignProjectId"].ToString());
                    objT.CourseCode = dr["Coursecode"].ToString();
                    objT.CourceName = dr["CourseName"].ToString();
                    objT.TrainerCode = dr["TrainerCode"].ToString();
                    objT.ProjectCode = dr["ProjectCode"].ToString();
                    objT.StartDate = Convert.ToDateTime(dr["StartDate"].ToString());
                    objT.BatchCode = dr["BatchCode"].ToString();
                    objT.BatchName = dr["BatchName"].ToString();
                    objT.SelectedStudentsString = dr["StudentFullName"].ToString();
                    objT.TeamLeaderCode = dr["TeamLeaderCode"].ToString();
                    objT.TeamLeaderName = dr["TeamLeaderFullName"].ToString();
                    objT.Duration = dr["Duration"].ToString();
                }
                // string CourseCode1 = objT.CourseCode;

                DataSet ds1 = await objbal.FetchBatchName(objT);
                List<SelectListItem> BatchNameList = new List<SelectListItem>();
                foreach (DataRow dr2 in ds1.Tables[0].Rows)
                {
                    BatchNameList.Add(new SelectListItem { Text = dr2["BatchName"].ToString(), Value = dr2["BatchCode"].ToString() });
                }
                ViewBag.BatchName1 = BatchNameList;

                List<SelectListItem> studentsList = await GetStudentsList(objT.AssignProjectId);
                ViewBag.StudentsList = studentsList;

                ViewBag.TLName = objT.TeamLeaderName;
                ViewBag.TLCode = objT.TeamLeaderCode;
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                   new BreadcrumbItem { Name = "TrainerDashboard", Url =Url.Action("TrainerDashboardAsyncTS","Trainer") },
                     new BreadcrumbItem { Name = "AssignProject", Url = Url.Action("ListAllAssignedProjectAsyncPG", "Trainer")},
                     new BreadcrumbItem { Name = "UpdateAssignProject", Url =Url.Action("UpdateAssignProjectAsyncPG","Trainer") }
                };
                ViewBag.Breadcrumbs = breadcrumbs;

                return View("UpdateAssignProjectAsyncPG", objT);
            }
        }
        /// <summary>
        /// This action method handles HTTP POST requests for updating an assigned project asynchronously.
        /// </summary>
        /// <param name="objT">An instance of the Trainer class representing the updated data for the assigned project.</param>
        /// <returns>Returns an ActionResult, typically redirecting to the "ListAllAssignedProjectAsyncPG" action.</returns>
        [HttpPost]
        public async Task<ActionResult> UpdateAssignProjectAsyncPG(Trainer objT)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                try
                {
                    await AssignTrainerName_Bind();
                    await ProjectName_Bind();
                    objT.StaffCode = Session["StaffCode"].ToString();
                    // objT.BranchCode = Session["BranchCode"].ToString();
                    await objbal.UpdateAssignProjectAsyncPG(objT);
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "An error occurred while updating data: " + ex.Message });
                }
            }
        }
        [HttpGet]
        public async Task<ActionResult> DetailsAssignProjectAsyncPG(int id)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Trainer objT = new Trainer();
                objT.AssignProjectId = id;
                objT.StaffCode = Session["StaffCode"].ToString();
                objT.BranchCode = Session["BranchCode"].ToString();
                SqlDataReader dr;
                dr = await objbal.DetailsAssignProjectAsyncPG(objT);
                while (dr.Read())
                {
                    objT.AssignProjectId = Convert.ToInt32(dr["AssignProjectId"].ToString());
                    // objT.Duration = dr["Duration"].ToString();
                    objT.CourceName = dr["CourseName"].ToString();
                    objT.AssignTrainer = dr["StaffName"].ToString();
                    objT.ProjectName = dr["ProjectName"].ToString();
                    objT.StartDate = Convert.ToDateTime(dr["StartDate"].ToString());
                    //objT.BatchCode = dr["BatchCode"].ToString();
                    objT.BatchName = dr["BatchName"].ToString();
                    //objT.SelectedStudentsString = dr["StudentFullName"].ToString();
                    // objT.TeamLeaderCode = dr["TeamLeaderCode"].ToString();
                    objT.TeamLeaderName = dr["TeamLeaderFullName"].ToString();
                    objT.Duration = dr["Duration"].ToString();
                    objT.Status = dr["Status"].ToString();
                }
                return PartialView("DetailsAssignProjectAsyncPG", objT);
            }
        }
        /// <summary>
        /// This private asynchronous method fetches a list of students based on the given AssignProjectId.
        /// </summary>
        /// <param name="AssignProjectId">The identifier of the assigned project for which students are to be fetched.</param>
        /// <returns>Returns a Task<List<SelectListItem>> representing the list of students as SelectListItem objects.</returns>
        private async Task<List<SelectListItem>> GetStudentsList(int AssignProjectId)
        {
            Trainer objT = new Trainer();
            objT.BranchCode = Session["BranchCode"].ToString();
            objT.AssignProjectId = AssignProjectId;
            DataSet ds = await objbal.FetchStudents(objT);
            List<SelectListItem> studentsList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                studentsList.Add(new SelectListItem
                {
                    Text = dr["StudentFullName"].ToString(),
                    Value = dr["CandidateCode"].ToString()
                });
            }
            return studentsList;
        }
        /// <summary>
        /// This action method handles HTTP GET requests for viewing added sprints of an assigned project asynchronously.
        /// </summary>
        /// <param name="assignProjectId">The identifier of the assigned project for which sprints are to be viewed.</param>
        /// <returns>Returns an ActionResult representing the view for viewing added sprints.</returns>
        [HttpGet]
        public async Task<ActionResult> AddedSprintsAsyncPG(int assignProjectId)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Trainer objT = new Trainer();
                ViewBag.AssignProjectId = assignProjectId;
                objT.AssignProjectId = assignProjectId;
                objT.StaffCode = Session["StaffCode"].ToString();
                objT.BranchCode = Session["BranchCode"].ToString();
                SqlDataReader dr;
                dr = await objbal.GetStatusId(objT);
                while (dr.Read())
                {
                    objT.StatusId = Convert.ToInt32(dr["StatusId"].ToString());
                    objT.ProjectName = dr["ProjectName"].ToString();
                    objT.ProjectOwner = dr["ClientName"].ToString();
                    objT.StaffCodeProjectAssignedBy = dr["StaffName"].ToString();
                    objT.StartDate = Convert.ToDateTime(dr["StartDate"].ToString());
                    objT.SelectedStudentsString = dr["CandidateCode"].ToString();
                }
                ViewBag.StatusId = objT.StatusId;
                ViewBag.ProjectName = objT.ProjectName;
                ViewBag.ProjectOwnerName = objT.ProjectOwner;
                ViewBag.ScrumMaster = objT.StaffCodeProjectAssignedBy;
                ViewBag.StartDate = objT.StartDate.ToShortDateString();
                ViewBag.CandidateCode = objT.SelectedStudentsString;
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                   new BreadcrumbItem { Name = "TrainerDashboard", Url =Url.Action("TrainerDashboardAsyncTS","Trainer") },
                     new BreadcrumbItem { Name = "AssignProject", Url = Url.Action("ListAllAssignedProjectAsyncPG", "Trainer") }
                     //new BreadcrumbItem { Name = "CompleteSprint",  Url = Url.Action("AddedSprintsAsyncPG","Trainer") }
                };
                ViewBag.Breadcrumbs = breadcrumbs;
                return await Task.Run(() => View(objT));
            }
        }
        /// <summary>
        /// This action method handles HTTP POST requests for updating the status of a sprint asynchronously.
        /// </summary>
        /// <param name="assignProjectId">The identifier of the assigned project for which the sprint belongs.</param>
        /// <param name="sprintNumber">The sprint number to be updated.</param>
        /// <returns>Returns a JSON result indicating the success of the sprint status update.</returns>
        [HttpPost]
        public async Task<ActionResult> UpdateSprintStatus(int assignProjectId, int sprintNumber, string CandidateCode)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Trainer objT = new Trainer();
                objT.StaffCode = Session["StaffCode"].ToString();
                objT.BranchCode = Session["BranchCode"].ToString();
                await objbal.UpdateSprintStatus(sprintNumber, assignProjectId, CandidateCode, objT);
                return Json(new { success = true, message = "Sprint status updated successfully." });
            }
        }
        //-------------------------- Pratibha Project Management End -----------------------------------------------------------------------------------------//

        //-------------------------- Pratibha Project Management End -----------------------------------------------------------------------------------------//
        //-------------------------- Pratibha Project Management End -----------------------------------------------------------------------------------------//
        //-------------------kirti FeedBack Start------------------------------------------------------------------------------------------------------------------//
        /// <summary>
        /// This methos showing the new feedback list for trainer.
        /// </summary>
        /// <returns>It returns the list.</returns>
        [HttpGet]
        public async Task<ActionResult> ListNewFeedbackToStudentKKAsync()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Trainer objfs = new Trainer();
                objfs.BranchCode = Session["BranchCode"].ToString();
                objfs.TrainerCode = Session["StaffCode"].ToString();
                DataSet ds = await objbal.ShowNewStudentsFeedbackKKAsync(objfs);
                Trainer feed = new Trainer();
                List<Trainer> lstfeedback = new List<Trainer>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Trainer objfb = new Trainer();
                    objfb.FeedbackId = Convert.ToInt32(ds.Tables[0].Rows[i]["FeedbackId"].ToString());
                    objfb.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objfb.FeedbackFor = ds.Tables[0].Rows[i]["Feedbackfor"].ToString();
                    objfb.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    objfb.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                    objfb.FeedbackFrom = ds.Tables[0].Rows[i]["Feedbackfrom"].ToString();
                    DateTime FeedbackSendDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["FeedbackSendDate"].ToString());
                    objfb.feedbcksendDate = FeedbackSendDate.ToString("dd-MM-yyyy");
                    DateTime FeedbackTillDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["FeedbackTillDate"].ToString());
                    objfb.feedbcktillDate = FeedbackTillDate.ToString("dd-MM-yyyy");
                    //objfb.Rating = Convert.ToInt32(ds.Tables[0].Rows[i]["Rating"].ToString());
                    //objfb.Comment = ds.Tables[0].Rows[i]["Comment"].ToString();
                    lstfeedback.Add(objfb);
                }
                feed.lstNewStudFeedback = lstfeedback;
                return View(feed);
            }
        }
        /// <summary>
        /// This method for add student feedback.
        /// </summary>
        /// <param name="FeedbackId">Use for select single feedback.</param>
        /// <returns>It returns the list of students of feedback from batch.</returns>
        [HttpGet]
        public async Task<ActionResult> AddStudentFeedbackKKAsyc(int FeedbackId)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Trainer detail = new Trainer();
                detail.FeedbackId = FeedbackId;
                detail.BranchCode = Session["BranchCode"].ToString();
                DataSet ds;
                ds = await objbal.AddedNewStudentsFeedbackKKAsync(detail);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    detail.FeedbackId = Convert.ToInt32(ds.Tables[0].Rows[i]["FeedbackId"].ToString());
                    detail.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    detail.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                    detail.Description = ds.Tables[0].Rows[i]["Descriptions"].ToString();
                    //detail.Rating = Convert.ToInt32(ds.Tables[0].Rows[i]["Rating"].ToString());
                    //detail.Comment = ds.Tables[0].Rows[i]["Comment"].ToString();
                }
                return PartialView("AddStudentFeedbackKKAsyc", detail);
            }
        }
        /// <summary>
        /// In this method add the feedback for student.
        /// </summary>
        /// <param name="feedup">This object use for the access the properties from the class.</param>
        /// <returns>It returns the added feedback data.</returns>
        [HttpPost]
        public async Task<ActionResult> AddStudentFeedbackKKAsyc(Trainer feedup)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                await Task.Run(() => objbal.FeedbackUpdateKKAsync(feedup));
                return RedirectToAction("ListNewFeedbackToStudentKKAsync", feedup);

            }

        }

        //-------------------kirti FeedBack END-----------------------------------------------------------------//
        //------------------YASH Attendance Start --------------------------------------------------------------//
        /// <summary>
        /// This view is used to show the main view of attendance 
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ListAttendanceAsyncYT(string successMessage)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                new BreadcrumbItem { Name = "Home", Url = Url.Action("Trainer")},
                new BreadcrumbItem { Name = "Attendance", Url = Url.Action("ListAttendanceAsyncYT")},
                };
                ViewBag.Breadcrumbs = breadcrumbs;
                return await Task.Run(() => View());
            }
        }
        /// <summary>
        /// This view is used to show the gridview of topicwise attendance
        /// </summary>
        /// <returns> Topicwise attendance list </returns>
        public async Task<ActionResult> ListAttendanceTopicAsyncYT()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Trainer Obj = new Trainer();
                Obj.StaffCode = Session["StaffCode"].ToString();
                BALTrainer objtopic = new BALTrainer();
                DataSet ds = await objtopic.ListAttendanceTopicYT(Obj.StaffCode);
                Trainer objshowlist = new Trainer();
                List<Trainer> lstAttendanceTopic = new List<Trainer>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Trainer objT = new Trainer();
                    objT.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objT.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    objT.BatchTime = ds.Tables[0].Rows[i]["StartTime"].ToString();
                    objT.TotalStudents = Convert.ToInt32(ds.Tables[0].Rows[i]["NoOfStudent"].ToString());
                    objT.BatchCode = ds.Tables[0].Rows[i]["BatchCode"].ToString();
                    lstAttendanceTopic.Add(objT);
                }
                objshowlist.lstAttendanceTopic = lstAttendanceTopic;
                return PartialView("_ListAttendanceTopicAsyncYT", objshowlist);
            }
        }
        /// <summary>
        /// This view is used to show the gridview of student list topicwise attendance
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> ListAttendanceTopicStudentAsyncYT(string id)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Trainer Obj = new Trainer();
                Obj.StaffCode = Session["StaffCode"].ToString();
                BALTrainer objtopicSL = new BALTrainer();
                DataSet ds = await objtopicSL.AttendanceTopicStudentListYT(id);
                List<Trainer> lstAttendanceTopicSL = new List<Trainer>();
                Trainer objlable = new Trainer();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Trainer objTSL = new Trainer();
                    objTSL.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                    objTSL.StudentCode = ds.Tables[0].Rows[i]["CandidateCode"].ToString();
                    lstAttendanceTopicSL.Add(objTSL);
                    objlable.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objlable.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    objlable.TotalStudents = Convert.ToInt32(ds.Tables[0].Rows[i]["NoOfStudent"].ToString());
                }
                objlable.LstAttendanceTopicSL = lstAttendanceTopicSL;
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                new BreadcrumbItem { Name = "Home", Url = Url.Action("Trainer")},
                new BreadcrumbItem { Name = "Attendance", Url = Url.Action("ListAttendanceAsyncYT","Trainer")},
                new BreadcrumbItem { Name = "Topicwise Student List", Url = Url.Action("ListAttendanceTopicStudentAsyncYT")}
                };
                ViewBag.Breadcrumbs = breadcrumbs;
                return View("ListAttendanceTopicStudentAsyncYT", objlable);
            }
        }
        /// <summary>
        /// This view is used to show the gridview of single student list topicwise attendance and gridview of count
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> ListAttendanceTopicSingleStudentAsyncYT(string id)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Trainer obj = new Trainer();
                obj.StaffCode = Session["StaffCode"].ToString();
                BALTrainer objtopicSS = new BALTrainer();
                DataSet ds = await objtopicSS.AttendanceTopicSingleStudentYT(id);
                Trainer objshowlist = new Trainer();
                List<Trainer> lstAttendanceTopicSS = new List<Trainer>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Trainer objatss = new Trainer();
                    objatss.SectionName = ds.Tables[0].Rows[i]["SectionName"].ToString();
                    objatss.TopicName = ds.Tables[0].Rows[i]["TopicName"].ToString();
                    objatss.Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString());
                    objatss.Time = Convert.ToDateTime(ds.Tables[0].Rows[i]["Time"].ToString());
                    objatss.Attenddance = ds.Tables[0].Rows[i]["Status"].ToString();
                    lstAttendanceTopicSS.Add(objatss);
                    objshowlist.StudentCode = ds.Tables[0].Rows[i]["CandidateCode"].ToString();
                    objshowlist.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                    objshowlist.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                }
                BALTrainer objCount = new BALTrainer();
                DataSet ds1 = await objCount.ViewCountTopicSingleStudentYT(id);
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    objshowlist.TopicCount = Convert.ToInt32(ds1.Tables[0].Rows[i]["NumberOfTopics"].ToString());
                    objshowlist.NoOfSessions = Convert.ToInt32(ds1.Tables[0].Rows[i]["NumberOfAssignSchedule"].ToString());
                    objshowlist.TotalPresentSessions = Convert.ToInt32(ds1.Tables[0].Rows[i]["PresentCount"].ToString());
                    objshowlist.TotalAbsentSessions = Convert.ToInt32(ds1.Tables[0].Rows[i]["AbsentCount"].ToString());
                }
                objshowlist.lstAttendanceTopicSS = lstAttendanceTopicSS;
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                new BreadcrumbItem { Name = "Home", Url = Url.Action("Trainer")},
                new BreadcrumbItem { Name = "Attendance", Url = Url.Action("ListAttendanceAsyncYT","Trainer")},
                new BreadcrumbItem { Name = "Topicwise Student List", Url = Url.Action("ListAttendanceTopicStudentAsyncYT")},
                new BreadcrumbItem { Name = "Topicwise Single Student", Url = Url.Action("ListAttendanceTopicSingleStudentAsyncYT")}
                };
                ViewBag.Breadcrumbs = breadcrumbs;
                return View("ListAttendanceTopicSingleStudentAsyncYT", objshowlist);
            }
        }
        /// <summary>
        /// This view is used to show the gridview of topicwise attendance   
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> ListAttendanceTopicwiseAsyncYT(string id)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Trainer obj = new Trainer();
                obj.StaffCode = Session["StaffCode"].ToString();
                BALTrainer objviewAT = new BALTrainer();
                DataSet ds = await objviewAT.ViewAttendanceTopicYT(id);
                Trainer objshowlist = new Trainer();
                List<Trainer> lstViewAttendanceTopic = new List<Trainer>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Trainer objVAT = new Trainer();
                    objVAT.SectionName = ds.Tables[0].Rows[i]["SectionName"].ToString();
                    objVAT.TopicName = ds.Tables[0].Rows[i]["TopicName"].ToString();
                    objVAT.StartDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"].ToString());
                    objVAT.EndDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["EndDate"].ToString());
                    objVAT.NoOfSessions = Convert.ToInt32(ds.Tables[0].Rows[i]["NoOfSessions"].ToString());
                    objVAT.AssignScheduleId = Convert.ToInt32(ds.Tables[0].Rows[i]["AssignScheduleId"].ToString());
                    lstViewAttendanceTopic.Add(objVAT);
                    objshowlist.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objshowlist.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    objshowlist.TotalStudents = Convert.ToInt32(ds.Tables[0].Rows[i]["NoOfStudent"].ToString());
                }
                objshowlist.lstViewAttendanceTopic = lstViewAttendanceTopic;
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                new BreadcrumbItem { Name = "Home", Url = Url.Action("Trainer")},
                new BreadcrumbItem { Name = "Attendance", Url = Url.Action("ListAttendanceAsyncYT")},
                new BreadcrumbItem { Name = "Sectionwise Attendance", Url = Url.Action("ListAttendanceTopicwiseAsyncYT")}
                };
                ViewBag.Breadcrumbs = breadcrumbs;
                return View("ListAttendanceTopicwiseAsyncYT", objshowlist);
            }
        }
        /// <summary>
        /// This view is used to show the session wise gridview 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> ListSessionsTopicAsyncYT(int id)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Trainer obj = new Trainer();
                obj.StaffCode = Session["StaffCode"].ToString();
                BALTrainer objviewsessions = new BALTrainer();
                DataSet ds = await objviewsessions.ViewSessionsTopicYT(id);
                Trainer objsowlist = new Trainer();
                List<Trainer> lstViewSession = new List<Trainer>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Trainer objVST = new Trainer();
                    objVST.AssignScheduleId = Convert.ToInt32(ds.Tables[0].Rows[i]["AssignScheduleId"].ToString());
                    objVST.Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString());
                    objVST.Time = Convert.ToDateTime(ds.Tables[0].Rows[i]["Time"].ToString());
                    objVST.PresentStudent = Convert.ToInt32(ds.Tables[0].Rows[i]["PresentStudent"].ToString());
                    objVST.AbsentStudent = Convert.ToInt32(ds.Tables[0].Rows[i]["AbsentStudent"].ToString());
                    lstViewSession.Add(objVST);
                    objsowlist.TopicName = ds.Tables[0].Rows[i]["TopicName"].ToString();
                    objsowlist.NoOfSessions = Convert.ToInt32(ds.Tables[0].Rows[i]["NoOfSessions"].ToString());
                }
                objsowlist.lstViewSessions = lstViewSession;
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                new BreadcrumbItem { Name = "Home", Url = Url.Action("Trainer")},
                new BreadcrumbItem { Name = "Attendance", Url = Url.Action("ListAttendanceAsyncYT")},
                new BreadcrumbItem { Name = "Sectionwise Attendance", Url = Url.Action("ListAttendanceTopicwiseAsyncYT")},
                new BreadcrumbItem { Name = "Sessionwise Attendance", Url = Url.Action("ListSessionsTopicAsyncYT")}
                };
                ViewBag.Breadcrumbs = breadcrumbs;
                return View("ListSessionsTopicAsyncYT", objsowlist);
            }
        }
        /// <summary>
        /// This view is used to show the gridview of  marked attendance topic wise
        /// </summary>
        /// <param name="id"></param>
        /// <param name="date"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public async Task<ActionResult> ListMarkedAttendanceTopicAsyncYT(int id, DateTime date, DateTime time)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Trainer obj = new Trainer();
                obj.StaffCode = Session["StaffCode"].ToString();
                BALTrainer objViewmarkedatt = new BALTrainer();
                DataSet ds = await objViewmarkedatt.ViewMarkedAttendanceTopicYT(id, date, time);
                Trainer OBJVMA = new Trainer();
                List<Trainer> lstviewMarkedatt = new List<Trainer>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Trainer objvma = new Trainer();
                    OBJVMA.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    OBJVMA.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    OBJVMA.SectionName = ds.Tables[0].Rows[i]["SectionName"].ToString();
                    OBJVMA.TopicName = ds.Tables[0].Rows[i]["TopicName"].ToString();
                    OBJVMA.Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString());
                    OBJVMA.Time = Convert.ToDateTime(ds.Tables[0].Rows[i]["Time"].ToString());
                    OBJVMA.TotalStudents = Convert.ToInt32(ds.Tables[0].Rows[i]["TotalStudents"].ToString());
                    OBJVMA.NoOfSessions = Convert.ToInt32(ds.Tables[0].Rows[i]["NoOfSessions"].ToString());
                    OBJVMA.PresentStudent = Convert.ToInt32(ds.Tables[0].Rows[i]["PresentStudents"].ToString());
                    OBJVMA.NoOfSessions = Convert.ToInt32(ds.Tables[0].Rows[i]["NoOfSessions"].ToString());
                    objvma.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                    objvma.Attenddance = ds.Tables[0].Rows[i]["Status"].ToString();
                    objvma.ReasonForAbsence = ds.Tables[0].Rows[i]["ReasonForAbsence"].ToString();
                    lstviewMarkedatt.Add(objvma);
                }
                OBJVMA.lstviewMarkedatt = lstviewMarkedatt;
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                new BreadcrumbItem { Name = "Home", Url = Url.Action("Trainer")},
                new BreadcrumbItem { Name = "Attendance", Url = Url.Action("ListAttendanceAsyncYT")},
                new BreadcrumbItem { Name = "Sectionwise Attendance", Url = Url.Action("ListAttendanceTopicwiseAsyncYT")},
                new BreadcrumbItem { Name = "Sessionwise Attendance", Url = Url.Action("ListSessionsTopicAsyncYT")},
                new BreadcrumbItem { Name = "Marked Attendance", Url = Url.Action("ListMarkedAttendanceTopicAsyncYT")}
                };
                ViewBag.Breadcrumbs = breadcrumbs;
                return View("ListMarkedAttendanceTopicAsyncYT", OBJVMA);
            }
        }
        /// <summary>
        /// This view is used to show the gridview of new attendance
        /// </summary>
        /// <param name="id"></param>
        /// <param name="date"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> AddAttendanceAsyncYT()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Trainer objT = new Trainer();
                objT.StaffCode = Session["StaffCode"].ToString();
                objT.BranchCode = Session["BranchCode"].ToString();
                DataSet ds = await objbal.ListAssignedBatchYT(objT);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    objT.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                }
                List<Trainer> AllBatch = new List<Trainer>();
                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {
                    AllBatch.Add(new Trainer
                    {
                        BatchName = dr1["BatchName"].ToString(),
                        BatchAssignedScheduledId = Convert.ToInt32(dr1["ScheduleId"].ToString()),
                        BatchCode = dr1["BatchCode"].ToString(),
                        TotalStudents = Convert.ToInt32(dr1["NoOfStudent"].ToString())
                    });
                }
                ViewBag.BatchList = AllBatch;
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                new BreadcrumbItem { Name = "Home", Url = Url.Action("Trainer")},
                new BreadcrumbItem { Name = "Attendance", Url = Url.Action("ListAttendanceAsyncYT")},
                new BreadcrumbItem { Name = "Mark Attendance", Url = Url.Action("AddAttendanceAsyncYT")}
                };
                ViewBag.Breadcrumbs = breadcrumbs;
                return View(objT);
            }
        }
        /// <summary>
        /// This view is used to add attendance
        /// </summary>
        /// <param name="studentAttendance"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AddAttendanceAsyncYT(List<Trainer> studentAttendance)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                try
                {
                    foreach (var studentData in studentAttendance)
                    {
                        await objbal.AddAttendanceYT(new Trainer
                        {
                            BatchCode = studentData.BatchCode,
                            AssignProjectId = studentData.AssignProjectId,
                            AssignScheduleId = studentData.AssignScheduleId,
                            Date = studentData.Date,
                            Time = studentData.Time,
                            StudentCode = studentData.StudentCode,
                            AttendanceStatusId = studentData.AttendanceStatusId,
                            StaffCode = Session["StaffCode"].ToString()
                        });
                    }
                    return Json(new { success = true, message = "Attendance saved successfully." });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return Json(new { success = false, message = "An error occurred while saving results. Details: " + ex.Message });
                }
            }
        }
        /// <summary>
        /// This view is used to show the list of sections
        /// </summary>
        /// <param name="ScheduleId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> ListSection(int ScheduleId)
        {
            DataSet ds = await objbal.ListSection(ScheduleId);
            List<SelectListItem> SectionList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                SectionList.Add(new SelectListItem { Text = dr["SectionName"].ToString(), Value = dr["SectionId"].ToString() });
            }
            return Json(SectionList, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// This view is used to show the list of topics
        /// </summary>
        /// <param name="SectionId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> ListTopic(int SectionId, int ScheduleId)
        {
            DataSet ds = await objbal.ListTopic(SectionId, ScheduleId);
            List<SelectListItem> TopicList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TopicList.Add(new SelectListItem { Text = dr["TopicName"].ToString(), Value = dr["AssignScheduleId"].ToString() });
            }
            return Json(TopicList, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// This view is used to show the topic start date
        /// </summary>
        /// <param name="TopicId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetTopicStartDate(int TopicId)
        {
            Trainer OBJvma = new Trainer();
            SqlDataReader dr = await objbal.GetTopicStartDate(TopicId);
            while(dr.Read())
            {
                OBJvma.StartDate = Convert.ToDateTime(dr["StartDate"].ToString());
            }
            return Json(OBJvma.StartDate.ToShortDateString(), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// This view is used to show list of students
        /// </summary>
        /// <param name="ScheduleId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> ListStudentYT(int ScheduleId)
        {
            DataSet ds = await objbal.ListStudentYT(ScheduleId);
            List<SelectListItem> StudentList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                StudentList.Add(new SelectListItem { Text = dr["FullName"].ToString(), Value = dr["CandidateCode"].ToString() });
            }
            return Json(StudentList, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// This view is used to show the gridview of projectwise attendance
        /// </summary>
        /// <returns> Projectwise attendance list </returns>
        public async Task<ActionResult> ListAttendanceProjectAsyncYT()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Trainer Obj = new Trainer();
                Obj.StaffCode = Session["StaffCode"].ToString();
                BALTrainer objproject = new BALTrainer();
                DataSet ds = await objproject.ListAttendanceProjectYT(Obj.StaffCode);
                Trainer objshowlst = new Trainer();
                List<Trainer> lstAttendanceProject = new List<Trainer>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Trainer objP = new Trainer();
                    objP.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    objP.AssignProjectId = Convert.ToInt32(ds.Tables[0].Rows[i]["AssignProjectId"].ToString());
                    objP.ProjectName = ds.Tables[0].Rows[i]["ProjectName"].ToString();
                    objP.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    objP.BatchTime = ds.Tables[0].Rows[i]["StartTime"].ToString();
                    objP.TotalStudents = Convert.ToInt32(ds.Tables[0].Rows[i]["NoOfStudent"].ToString());
                    lstAttendanceProject.Add(objP);
                }
                objshowlst.lstAttendanceProject = lstAttendanceProject;
                return PartialView("_ListAttendanceProjectAsyncYT", objshowlst);
            }
        }
        /// <summary>
        /// This view is used to show the gridview of student list projectwise attendance
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> ListAttendanceProjectStudentAsyncYT(string id)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Trainer Obj = new Trainer();
                Obj.StaffCode = Session["StaffCode"].ToString();
                BALTrainer objprojectSL = new BALTrainer();
                DataSet ds = await objprojectSL.AttendanceProjectStudentListYT(id);
                Trainer objshwlis = new Trainer();
                List<Trainer> lstAttendanceProjectSL = new List<Trainer>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Trainer objPSL = new Trainer();
                    objPSL.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                    objPSL.StudentCode = ds.Tables[0].Rows[i]["CandidateCode"].ToString();
                    lstAttendanceProjectSL.Add(objPSL);
                }
                objshwlis.lstAttendanceProjectSL = lstAttendanceProjectSL;
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                new BreadcrumbItem { Name = "Home", Url = Url.Action("Trainer")},
                new BreadcrumbItem { Name = "Attendance", Url = Url.Action("ListAttendanceAsyncYT")},
                new BreadcrumbItem { Name = "Projectwise Student List", Url = Url.Action("ListAttendanceProjectStudentAsyncYT")}
                };
                ViewBag.Breadcrumbs = breadcrumbs;
                return View("ListAttendanceProjectStudentAsyncYT", objshwlis);
            }
        }
        /// <summary>
        /// This view is used to show the gridview count of single student in project 
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> DetailsCountProjectSingleStudentAsyncYT(string id)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Trainer obj = new Trainer();
                obj.StaffCode = Session["StaffCode"].ToString();
                BALTrainer objviewcount = new BALTrainer();
                DataSet ds = await objviewcount.ViewCountProjectSingleStudentYT(id);
                Trainer objshowlist = new Trainer();
                List<Trainer> lstcountproject = new List<Trainer>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    objshowlist.TotalPresentProject = Convert.ToInt32(ds.Tables[0].Rows[i]["PresentCount"].ToString());
                    objshowlist.TotalAbsentProject = Convert.ToInt32(ds.Tables[0].Rows[i]["AbsentCount"].ToString());
                    objshowlist.TotalDays = Convert.ToInt32(ds.Tables[0].Rows[i]["TotalDays"].ToString());
                    lstcountproject.Add(objshowlist);
                    objshowlist.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                    objshowlist.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                }
                objshowlist.lstcountproject = lstcountproject;
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                new BreadcrumbItem { Name = "Home", Url = Url.Action("Trainer")},
                new BreadcrumbItem { Name = "Attendance", Url = Url.Action("ListAttendanceAsyncYT")},
                new BreadcrumbItem { Name = "Projectwise Student List", Url = Url.Action("ListAttendanceProjectStudentAsyncYT")},
                new BreadcrumbItem { Name = "Projectwise Single Student", Url = Url.Action("DetailsCountProjectSingleStudentAsyncYT")}
                };
                ViewBag.Breadcrumbs = breadcrumbs;
                return View("DetailsCountProjectSingleStudentAsyncYT", objshowlist);
            }
        }
        /// <summary>
        /// This view is used to add attendance of project
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> AddAttendanceProjectAsyncYT()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Trainer objT = new Trainer();
                objT.StaffCode = Session["StaffCode"].ToString();
                objT.BranchCode = Session["BranchCode"].ToString();
                DataSet ds = await objbal.ListAssignedBatchProjectYT(objT);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    objT.CourseName = ds.Tables[0].Rows[i]["CourseName"].ToString();

                }
                List<Trainer> AllBatch = new List<Trainer>();
                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {
                    AllBatch.Add(new Trainer
                    {
                        BatchName = dr1["BatchName"].ToString(),
                        AssignScheduleId = Convert.ToInt32(dr1["AssignProjectId"].ToString()),
                        BatchCode = dr1["BatchCode"].ToString(),
                        ProjectName = dr1["ProjectName"].ToString(),
                        TotalStudents = Convert.ToInt32(dr1["NoOfStudent"].ToString())
                    });
                }
                ViewBag.BatchList = AllBatch;
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                new BreadcrumbItem { Name = "Home", Url = Url.Action("Trainer")},
                new BreadcrumbItem { Name = "Attendance", Url = Url.Action("ListAttendanceAsyncYT")},
                new BreadcrumbItem { Name = "Mark Attendance", Url = Url.Action("AddAttendanceProjectAsyncYT")}
                };
                ViewBag.Breadcrumbs = breadcrumbs;
                return View(objT);
            }
        }
        /// <summary>
        /// This view is used to show start date of project
        /// </summary>
        /// <param name="BatchCode"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetProjectStartDate(string BatchCode)
        {
            Trainer OBJvma = new Trainer();
            DataSet ds = await objbal.GetProjectStartDate(BatchCode);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                OBJvma.StartDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"].ToString());
            }
            return Json(OBJvma.StartDate.ToShortDateString(), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// This view is used to list of students in project
        /// </summary>
        /// <param name="BatchCode"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> ListStudentProjectYT(string BatchCode)
        {
            DataSet ds = await objbal.ListStudentProjectYT(BatchCode);
            List<SelectListItem> StudentList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                StudentList.Add(new SelectListItem { Text = dr["FullName"].ToString(), Value = dr["CandidateCode"].ToString() });
            }
            return Json(StudentList, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// This view is used to view list of project attendance
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ListViewAttendanceProjectYT()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Trainer Obj = new Trainer();
                Obj.StaffCode = Session["StaffCode"].ToString();
                DataSet ds = await objbal.ListViewAttendanceProjectYT();
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                new BreadcrumbItem { Name = "Home", Url = Url.Action("Trainer")},
                new BreadcrumbItem { Name = "Attendance", Url = Url.Action("ListAttendanceAsyncYT","Trainer")},
                new BreadcrumbItem { Name = "Monthly Attendance", Url = Url.Action("ListViewAttendanceProjectYT")}
                };
                ViewBag.Breadcrumbs = breadcrumbs;
                return View("ListViewAttendanceProjectYT");
            }
        }
        /// <summary>
        /// This view is used to show the gridview of student leave
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> ListStudentLeaveAsyncYT()
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Trainer obj = new Trainer();
                obj.StaffCode = Session["StaffCode"].ToString();
                BALTrainer objviewleave = new BALTrainer();
                DataSet ds = await objviewleave.ViewStudentLeaveYT(obj.StaffCode);
                Trainer objView = new Trainer();
                List<Trainer> lststudentleave = new List<Trainer>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Trainer objVL = new Trainer();
                    objVL.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                    objVL.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    objVL.StartDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"].ToString());
                    objVL.EndDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["EndDate"].ToString());
                    objVL.ApplyLeaveDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["ApplyLeaveDate"].ToString());
                    objVL.NoOfDays = int.Parse(ds.Tables[0].Rows[i]["NoOfDays"].ToString());
                    objVL.Reason = ds.Tables[0].Rows[i]["Reason"].ToString();
                    objVL.Status = ds.Tables[0].Rows[i]["Status"].ToString();
                    objVL.StudentCode = ds.Tables[0].Rows[i]["CandidateCode"].ToString();
                    lststudentleave.Add(objVL);
                }
                objView.lststudentleave = lststudentleave;
                List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>
                {
                new BreadcrumbItem { Name = "Home", Url = Url.Action("Trainer")},
                new BreadcrumbItem { Name = "Attendance", Url = Url.Action("ListAttendanceAsyncYT")}
                };
                ViewBag.Breadcrumbs = breadcrumbs;
                return PartialView("ListStudentLeaveAsyncYT", objView);
            }
        }
        /// <summary>
        /// This view is used to show the details of student leave
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> DetailStudentLeaveAsyncYT(string id)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                Trainer obj = new Trainer();
                obj.StaffCode = Session["StaffCode"].ToString();
                BALTrainer objviewleave = new BALTrainer();
                DataSet ds = await objviewleave.DetailStudentLeaveYT(id);
                Trainer objVL = new Trainer();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    objVL.ApplyLeaveId = Convert.ToInt32(ds.Tables[0].Rows[i]["ApplyLeaveId"].ToString());
                    objVL.StudentName = ds.Tables[0].Rows[i]["FullName"].ToString();
                    objVL.BatchName = ds.Tables[0].Rows[i]["BatchName"].ToString();
                    objVL.StartDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"].ToString());
                    objVL.EndDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["EndDate"].ToString());
                    objVL.Reason = ds.Tables[0].Rows[i]["Reason"].ToString();
                    objVL.StatusId = Convert.ToInt32(ds.Tables[0].Rows[i]["StatusId"].ToString());
                    objVL.StudentCode = ds.Tables[0].Rows[i]["CandidateCode"].ToString();
                }
                await ListStatusAsyncYT();
                return PartialView("DetailStudentLeaveAsyncYT", objVL);
            }
        }
        /// <summary>
        /// This view is used to show list of status in project
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task ListStatusAsyncYT()
        {
            DataSet ds = await objbal.ListStatusYT();
            List<SelectListItem> StatusList = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                StatusList.Add(new SelectListItem { Text = dr["Status"].ToString(), Value = dr["StatusId"].ToString() });
            }
            ViewBag.Status = StatusList;
        }
        /// <summary>
        /// This view is used to edit student leave
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> EditStudentLeaveAsyncYT(Trainer obj)
        {
            if (Session["StaffCode"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                obj.StaffCode = Session["StaffCode"].ToString();
                await objbal.EditStudentLeaveYT(obj);
                return RedirectToAction("ListAttendanceAsyncYT");
            }
        }

        //------------------YASH Attendance Start --------------------------------------------------------------//


    }
    #endregion

}