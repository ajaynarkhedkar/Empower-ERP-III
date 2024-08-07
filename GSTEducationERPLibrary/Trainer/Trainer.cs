using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace GSTEducationERPLibrary.Trainer
{
    public class Trainer
    {
        #region public variable declare
        /// <summary>
        /// 
        /// </summary>
        public int SectionId { get; set; }
        [DisplayName("Course Name")]
        public string CourseCode { get; set; }
        [DisplayName("Section Name")]
        public string SectionName { get; set; }
        public int AssignmentId { get; set; }
        public string AssignmentFile { get; set; }
        public int TopicId { get; set; }
        [DisplayName("Topic Name")]
        public string TopicName { get; set; }
        [DisplayName("No of Sessions")]

        public int NoOfSessions { get; set; }
        public string Duration { get; set; }
        [DisplayName("Trainer Name")]
        public string TrainerCode { get; set; }
        [DisplayName("Topic Added Date")]
        public DateTime TopicAddedDate { get; set; }
        public int TopicStatusId { get; set; }
        public int AttendanceId { get; set; }
        [DisplayName("Batch Name")]
        public string BatchCode { get; set; }
        public int AssignScheduleId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        [DataType(DataType.Time)]
        public DateTime Time { get; set; }
        [DisplayName("Student Name")]
        public string StudentCode { get; set; }
        public int AttendanceStatusId { get; set; }
        [DisplayName("Reason For Absence")]
        public string ReasonForAbsence { get; set; }
        [DisplayName("Status")]
        public int StatusId { get; set; }
        public int BatchAssignedScheduledId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Start Date")]
        [Required(ErrorMessage = "Start Date is required.")]
        public DateTime StartDate { get; set; }
        public DateTime ScheduleAssignedDate { get; set; }
        public int TestId { get; set; }
        [DisplayName("Test Name")]
        public string TestName { get; set; }
        [DisplayName("Register Date")]
        public DateTime RegisterDate { get; set; }
        [DisplayName("Total Marks")]
        public float TotalMarks { get; set; }
        [DisplayName("Passing Marks")]
        public float PassingMarks { get; set; }
        [DisplayName("Test Paper")]
        public string TestPaperFile { get; set; }
        [DisplayName("Total No Of Questions")]
        public int TotalNoOfQuestion { get; set; }
        public int AssignTestId { get; set; }
        [DisplayName("Complete Till Date")]
        public DateTime CompleteTillDate { get; set; }
        public DateTime AssignDate { get; set; }
        public int ResultId { get; set; }
        public int TestAssignedId { get; set; }
        [DisplayName("Obtained Marks")]
        public float ObtainedMarks { get; set; }
        public string ResultStatus { get; set; }
        public DateTime ResultAddedDate { get; set; }
        public int AssignProjectId { get; set; }
        public string TeamLeaderCode { get; set; }
        public string ProjectCode { get; set; }
        public DateTime AssignedDate { get; set; }

        public string StaffCodeProjectAssignedBy { get; set; }


        ////----vaibhav-------// prop 
        public string StaffCode { get; set; }
        [DisplayName("Course Name")]
        public string CourseName { get; set; }
        [DisplayName("Batch Name")]
        public string BatchName { get; set; }
        public string STARTDATEA { get; set; }
        public string ENDDATEA { get; set; }
        [DisplayName("Lab Name")]
        public string LabName { get; set; }
        public int BatchId { get; set; }
        public string BranchCode { get; set; }
        public string Status { get; set; }
        #endregion
        //---------- vaibhav pawar Start -------------//


        #region public list properties  starts
        //-- batch sedule - create sedule - view girdview 
        public List<Trainer> lstSectionGRid { get; set; }
        public List<Trainer> lstTopicGRid { get; set; }
        public List<Trainer> lstassinggrid { get; set; }
        public List<Trainer> lstAssingseduleGRid { get; set; }
        public List<HttpPostedFileBase> Files { get; set; }

        public string TopicDescription { get; set; }

        //---------- vaibhav pawar End -------------//
        #endregion public list properties end



        //---------------------------pratibha------------------------------------------------------------------------//
        [DisplayName("Student Name")]
        public string StudentName { get; set; }
        [DisplayName("Course Name")]
        public string CourceName { get; set; }
        [DisplayName("Project Name")]
        public string ProjectName { get; set; }
        public List<Trainer> lstAssignedProject { get; set; }

        public List<Trainer> lstStudentList { get; set; }
        public string AssignTrainer { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Tentative End Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime TentetiveEndDate { get; set; }
        public string MergeBatch { get; set; }
        public string TeamMember { get; }
        public string TotalMemberOnProject { get; set; }
        [DisplayName("Team Leader")]
        public string TeamLeaderName { get; set; }
        public int id { get; set; }
        public int SRNO { get; set; }
        public string Technology { get; set; }
        public string Methodology { get; set; }
        [DisplayName("Staff Name")]
        public string StaffName { get; set; }
        [DisplayName("Project Owner")]
        public string ProjectOwner { get; set; }
        [DisplayName("Status")]
        public string StatusName { get; set; }
        public List<Trainer> lstAllProjects { get; set; }
        public int Projectid { get; set; }
        [Display(Name = "Selected Students")]
        public string SelectedStudentsString { get; set; }
        public string MergeStudentsString { get; set; }
        //--------pratibha end --------------------------------------------------------------------//
        // YASH COURSE start------------------------------------------------------------------------//

        // YASH COURSE End-----------------------------------------------------------------------------
        //tushar dashboard start ----------------------------------------------------------------------
        public int TotalBatchCount { get; set; }
        public int TotalStudentCount { get; set; }
        public int ActiveBatchCount { get; set; }
        public int ReleaseBatchCount { get; set; }
        public int UpcomingBatchCount { get; set; }
        public int ActiveStudent { get; set; }
        public int ReleaseStudent { get; set; }
        public int PlacedStudent { get; set; }
        public int NoOfStudent { get; set; }
        public List<Trainer> GraphData { get; set; }

        //tushar dashboard End ----------------------------------------------------------------------
        //---sayali notification start -------------------------------------------------------------

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BatchScheduleDate { get; set; }

        public int ScheduleId { get; set; }

        public List<Trainer> lstBatchData { get; set; }


        //---sayali notification End  -------------------------------------------------------------
        //-------------------kirti FeedBack Start-----------------------------------------------------------------//
        public int Rating { get; set; }
        public string DemoName { get; set; }
        public string DemoArrangedBy { get; set; }
        public List<Trainer> lstArrangeDemo { get; set; }
        public string strtDate { get; set; }
        [DisplayName("Contact Number")]
        public string ContactNumber { get; set; }
        [DisplayName("Email")]
        public string Emailid { get; set; }
        public List<Trainer> lstNoofstudent { get; set; }
        public DateTime StartTime { get; set; }
        public string strttime { get; set; }
        public DateTime EndTime { get; set; }
        public List<Trainer> lstNewStudFeedback { get; set; }
        [DisplayName("Feedback For")]
        public string FeedbackFor { get; set; }
        [DisplayName("Feedback From")]
        public string FeedbackFrom { get; set; }
        [DisplayName("Feedback Send Date")]
        public DateTime FeedbackSendDate { get; set; }
        [DisplayName("Feedback Till Date")]
        public DateTime FeedbackTillDate { get; set; }
        [DisplayName("Feedback Send Date")]
        public string feedbcksendDate { get; set; }
        [DisplayName("Feedback Till Date")]
        public string feedbcktillDate { get; set; }
        public int FeedbackId { get; set; }
        public string Comment { get; set; }
        public DateTime FeedbackAddedDate { get; set; }

        //-------------------kirti FeedBack END-----------------------------------------------------------------//
        //-------------------Tushar  Test Maagement  Start -----------------------------------------------------------------//
        public string TrainerCodeAssignedByCode { get; set; }
        public List<Trainer> AllTestList { get; set; }
        public bool TestResultExists { get; set; }
        public List<Trainer> AddTestList { get; set; }
        public List<Trainer> ConductedTestList { get; set; }
        public List<Trainer> PendingTestList { get; set; }
        public List<Trainer> ArrengeTestList { get; set; }
        public List<Trainer> AssignTestList { get; set; }
        public List<Trainer> ResultList { get; set; }
        [DisplayName("Test Date")]
        [DataType(DataType.Date)]
        public DateTime TestDate { get; set; }
        [DisplayName("Test Time")]
        [DataType(DataType.Time)]
        public DateTime TestTime { get; set; }
        public TimeSpan SDuration { get; set; }
        public string Attendance { get; set; }
        public string Result { get; set; }

        //-------------------Tushar  Test Maagement END-----------------------------------------------------------------//
        //-----------------Kirti Demo Start ------------------------------------------------------------------------------//
        //-----------------Kirti Demo  End -------------------------------------------------------------------------------//
        //------------------YASH Attendance Start --------------------------------------------------------------//
        public List<Trainer> LstAttendanceTopicSL { get; set; }
        public int ApplyLeaveId { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("End Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        public List<Trainer> lstcountproject { get; set; }
        [DisplayName("Total Days")]
        public int TotalDays { get; set; }
        public string Reason { get; set; }
        [DisplayName("Total Present Days")]
        public int TotalPresentProject { get; set; }
        [DisplayName("Total Absent Days")]
        public int TotalAbsentProject { get; set; }
        public List<Trainer> lstAttendanceTopic { get; set; }
        [DisplayName("Batch Name")]
        public string BatchTime { get; set; }
        public List<Trainer> lstCourseContent { get; set; }
        [DisplayName("Syllabus File")]
        public string SyllabusFile { get; set; }
        public string Description { get; set; }
        public List<Trainer> lstAttendanceProject { get; set; }
        [DisplayName("Total Students")]
        public int TotalStudents { get; set; }
        public string EnqCode { get; set; }
        public List<Trainer> lstAttendanceProjectSL { get; set; }
        public List<Trainer> lstAttendanceTopicSS { get; set; }
        [DisplayName("Attendance")]
        public string Attenddance { get; set; }
        public List<Trainer> lstViewAttendanceTopic { get; set; }
        public List<Trainer> lstViewSessions { get; set; }
        [DisplayName("Present Student")]
        public int PresentStudent { get; set; }
        [DisplayName("Absent Student")]
        public int AbsentStudent { get; set; }
        [DisplayName("No Of Topics")]
        public int TopicCount { get; set; }
        [DisplayName("Total Present Sessions")]
        public int TotalPresentSessions { get; set; }
        [DisplayName("Total Absent Sessions")]
        public int TotalAbsentSessions { get; set; }
        public List<Trainer> lstviewMarkedatt { get; set; }
        public List<Trainer> lststudentleave { get; set; }
        public bool HasSyllabus { get; set; }
        public List<Trainer> lstmarkatt { get; set; }
        [DisplayName("Applied Leave Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime ApplyLeaveDate { get; set; }
        [DisplayName("No Of Days")]
        public int NoOfDays { get; set; }

        //------------------YASH Attendance Start --------------------------------------------------------------//

    }
}
