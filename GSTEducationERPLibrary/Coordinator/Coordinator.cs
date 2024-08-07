using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GSTEducationERPLibrary.Coordinator
{
    public class Coordinator
    {
        #region Variable Declare
        public int LabId { get; set; }
        public string LabCode { get; set; }
        [DisplayName("Lab Name")]
        public string LabName { get; set; }
        public string CenterCode { get; set; }
        [DisplayName("Lab Capacity")]
        public int LabCapacity { get; set; }
        [DisplayName("Available System")]
        public int AvailableSystem { get; set; }
        [DisplayName("Lab Created Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime LabCreatedDate { get; set; }
        public int BatchId { get; set; }
        public string BatchCode { get; set; }
        [DisplayName("Batch Name")]
        public string BatchName { get; set; }
        public string StudentCode { get; set; }
        public string CourseCode { get; set; }
        [DisplayName("No of Student")]
        public int NoOfStudent { get; set; }
        public DateTime CreateDate { get; set; }
        [DisplayName("Status")]
        public int StatusId { get; set; }
        [DisplayName("Type")]
        public int TypeId { get; set; }
        [DisplayName("Staff Name")]
        public string StaffCode { get; set; }
        public int ScheduleId { get; set; }
        [DisplayName("Batch Time")]
        public string BatchTime { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BatchScheduleDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime RescheduleDate { get; set; }
        public string Duration { get; set; }
        public int FollowUpId { get; set; }
        [DisplayName("Follow Up Note")]
        public string FollowUpNote { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Next Follow Up Date")]
        public DateTime NextFollowUpDate { get; set; }
        [DisplayName("Follow up Taken Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FollowUpTakenDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Date")]
        public DateTime Date { get; set; }
        [DisplayName("Event")]
        public int EventId { get; set; }
        [DisplayName("Event Name")]
        public string EventName { get; set; }
        [DisplayName("Event Organizer Code")]
        public string EventOrgnaizerCode { get; set; }
        [DisplayName("No Of Participants")]
        public int NoOfParticipants { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("End Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        [DisplayName("Start Time")]
        [DataType(DataType.Time)]
        public DateTime StartTime { get; set; }
        [DisplayName("End Time")]
        [DataType(DataType.Time)]
        public DateTime EndTime { get; set; }
        public string Location { get; set; }
        [DisplayName("Event Arranged By")]
        public string EventArrangedBy { get; set; }
        [DisplayName("Event Type")]
        public int EventType { get; set; }
        [DisplayName("Event Added By")]
        public string EventAddedBy { get; set; }
        public string Description { get; set; }
        [DisplayName("Invitation Send To")]
        public string InvitationToSend { get; set; }
        public int FeedbackId { get; set; }
        [DisplayName("Feedback For")]
        public string FeedbackFor { get; set; }
        [DisplayName("Feedback From")]
        public string FeedbackFrom { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Feedback Send Date")]
        public DateTime FeedbackSendDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Feedback Till Date")]
        public DateTime FeedbackTillDate { get; set; }
        [DisplayName("Feedback Added Date")]
        public DateTime FeedbackAddedDate { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public int AssignTestId { get; set; }
        [DisplayName("Test")]
        public int TestId { get; set; }
        [DisplayName("Test Date")]
        public DateTime TestDate { get; set; }
        [DisplayName("Test Time")]
        public DateTime TestTime { get; set; }
        [DisplayName("Supervisor")]
        public string SupervisorCode { get; set; }
        [DisplayName("Arranged Date")]
        public DateTime ArrangedDate { get; set; }

        #endregion

        //---------- vaibhav pawar Start -------------//
        #region public list properties  starts

        public List<Coordinator> lstAssignTestGrid { get; set; }
        public List<Coordinator> lstArrangeTestGrid { get; set; }
        public List<Coordinator> lstPendingTestGrid { get; set; }
        public List<Coordinator> lstCouductedTestGrid { get; set; }
        [DisplayName("Course Name")]
        public string CourseName { get; set; }
        [DisplayName("Test Name")]
        public string TestName { get; set; }
        [DisplayName("Complete Till Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string CompleteTillDate { get; set; }
        [DisplayName("Total Marks")]
        public int TotalMarks { get; set; }
        [DisplayName("Staff Name")]
        public string StaffName { get; set; }
        [DisplayName("Test Time")]
        public string TestTimeMAnage { get; set; }
        [DisplayName("Test Date")]
        public string TestDateVP { get; set; }
        [DisplayName("Branch Name")]
        public string BranchCode { get; set; }
        //-----------------------------vaibhav pawar End ----------------------------

        //-----------------------------pratiksha dashboard start  -----------------------------

        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int ActiveBatches { get; set; }
        public int ReleasedBatches { get; set; }
        public int FeeCollectionId { get; set; }

        //-----------------------------pratiksha dashboard end  -----------------------------

        //--------------------Sayli Batch Schedule start -----------------------------------------
        //------------------Sayali---------------------//
        public string Status { get; set; }
        //public string BranchCode { get; set; }
        //public string StaffName { get; set; }
        [DisplayName("Student Name")]
        public string StudentName { get; set; }
        [DisplayName("Contach No")]
        public string ContactNo { get; set; }
        public string EMail { get; set; }
        [DisplayName("Selected Batch")]
        public string SelectBatch { get; set; }
        [DisplayName("Add Batch")]
        public string AddBatch { get; set; }
        public string SelectStudent { get; set; }
        public string AddStudent { get; set; }
        //public string LabName { get; set; }
        public string AddedStaffCode { get; set; }
        //public string SelectStudentCode { get; set; }
        public string AddStudentCode { get; set; }
        public int AddNoOfStudent { get; set; }
        //public int LabCapacity { get; set; }
        public string CourseDuration { get; set; }
        public string SelectStudentCode { get; set; }
        public int SelectNoOfStudent { get; set; }
        public List<Coordinator> lstBatchData { get; set; }
        public List<Coordinator> lstAdmittedStudent { get; set; }
        public List<Coordinator> lstSelectedStudent { get; set; }
        public List<Coordinator> lstBatchStudentList { get; set; }
        public int LabScheduleId { get; set; }

        public string StartTime1 { get; set; }
        public string EndTime1 { get; set; }
        //public string LabCode { get; set; }


        //--------------------Sayli Batch Schedule End -----------------------------------------
        //--------------------lab sedule start vp --------------------------------------------
        public List<Coordinator> lstlabseduleGridvp { get; set; }


        public string LabSchedule { get; set; }

        //public Dictionary<string, string> Schedule { get; set; } = new Dictionary<string, string>();

        //--------------------lab sedule End  vp --------------------------------------------//
        //-----------------------kirti Attendanse Follow Up ------------------------------------------//
        [DisplayName("Trainer Name")]
        public string TrainerName { get; set; }
        [DisplayName("Contact Number")]
        public string ContactNumber { get; set; }
        [DisplayName("Email")]
        public string Emailid { get; set; }
        [DisplayName("Alternate Number")]
        public string AlternateNumber { get; set; }

        [DisplayName("Status")]
        public string StatusName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Date Of Join")]
        public DateTime DateofJoin { get; set; }
        public string FollowUpTaken { get; set; }
        [DisplayName("Followup Taken By")]
        public string FollowUpTakenBy { get; set; }
        public List<Coordinator> lstFollowup { get; set; }
        public List<Coordinator> lstAllFollowup { get; set; }
        public List<Coordinator> lstPending { get; set; }

        [DisplayName("Next FollowUp Date")]
        public string NextFoDate { get; set; }
        [DisplayName("FollowUp Taken Date")]
        public string FTakenDate { get; set; }

        [DisplayName("Date Of Join")]
        public string DofJoin { get; set; }

        //[DisplayName("Demo Name")]
        //public string FollowupTakenCode { get; set; }
        public List<Coordinator> lstHistory { get; set; }
        [DisplayName("Follow Up Count")]
        public int FollowupCount { get; set; }
        [DisplayName("Absent Days")]
        public int AbsentDays { get; set; }



        //-----------------------kirti Attendanse Follow Up ------------------------------------------//
        //---------------------------Kirti Coordinator Feedack  Start -------------//


        //---------------------------Kirti Coordinator FeedBack  End --------------//
        //----------------------- Rohit Fees  Follow Up ------------------------------------------//
        //-----Rohit-----//

        public List<Coordinator> lstfeefollowup { get; set; }
        //[DisplayName("Enrollment No")]
        //public int StudentId { get; set; }
        [DisplayName("Student Name")]
        public string FullName { get; set; }
        //[DisplayName("Phone No")]
        //public string ContactNumber { get; set; }
        //[DisplayName("Email Id")]
        //public string EmailId { get; set; }

        //[DisplayName("Course Name")]
        //public string CourseName { get; set; }
        [DisplayName("Course Fee")]
        public string CourseFee { get; set; }
        [DisplayName("Discounted Course Fee")]
        public string DiscountedCourseFee { get; set; }
        public int Discount { get; set; }
        [DisplayName("Remaining Fee")]
        public string RemainingFee { get; set; }
        [DisplayName("Installment Amount")]
        public string InstallmentAmount { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Next Installment Date")]
        public DateTime NextInstallmentDate { get; set; }

        //public string BranchCode { get; set; }
        [DisplayName("Registration Fee")]
        public string Amount { get; set; }
        //public string FollowUpTakenBy { get; set; }

        [DisplayName("Paid Amount")]
        public string PaidAmount { get; set; }
        [DisplayName("Follow Up Date")]
        public string followupDate { get; set; }
        [DisplayName("Next Date")]
        public string nextdate { get; set; }
        [DisplayName("Last Installment Date")]
        public string LastInstallmentDate { get; set; }
        //public string Status { get; set; }
        public int Count { get; set; }
        [DisplayName("Next Follow Up")]
        public string NextFollowup { get; set; }

        //-----------------------Rohit Fees  Follow Up ------------------------------------------//


        //-----------------Kirti Feedback Start --------------------------------------------------------------------------//
        [DisplayName("Feedback Till Date")]
        public string feedbcktillDate { get; set; }
        public List<Coordinator> lststudentfeed { get; set; }
        public string feedbcksendDate { get; set; }
        public string feedbckaddedDate { get; set; }
        [DisplayName("Feedback Type")]
        public int FeedbackType { get; set; }
        public string ScheduledAddedByStaffCode { get; set; }
        [DisplayName("Type")]
        public string TypeName { get; set; }
        public List<Coordinator> lsttrainerfeed { get; set; }
        public List<Coordinator> lsttrainerfeedgiven { get; set; }
        public List<Coordinator> lstNoofstudent { get; set; }
        public string StudentList { get; set; }

        //-----------------Kirti Feedback  End ---------------------------------------------------------------------------//
        //-----------------Vedant Lab Management Start -------------------------------------------------------------------//
        public string LabScheduledeatils { get; set; }
        public List<Coordinator> DetailsActiveLabAsyncVJ { get; set; }
        [DisplayName("Release Date")]
        public DateTime? ReleseDate { get; set; }
        public List<Coordinator> ListAcitveLabAsyncVJ { get; set; }
        public List<Coordinator> ListLabAsyncVJ { get; set; }
        public string CenterName { get; set; }
        public string FillLabCode { get; set; }
        //-----------------Vedant Lab Management End ----------------------------------------------------------------------//
        //-----------------Kirti Demo Start ------------------------------------------------------------------------------//
        public string SelectedStudentCodes { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime ExpectedDate { get; set; }
        public List<Coordinator> lstDemo { get; set; }
        [DisplayName("Demo Name")]
        public string DemoName { get; set; }
        [DisplayName("Demo ArrangedBy ")]
        public string DemoArrangedBy { get; set; }
        [DisplayName("Start Date")]
        public string strtDate { get; set; }
        [DisplayName("Expected Date")]
        public string ExpecDate { get; set; }
        //-----------------Kirti Demo  End -------------------------------------------------------------------------------//
        //-----------------------Priyanka Event------------------------------------------------------//
        public List<Coordinator> eventlist { get; set; }
        public List<Coordinator> events { get; set; }
        public List<Coordinator> Conductedeventlist { get; set; }
        public List<Coordinator> Arrangedeventlist { get; set; }
        public List<Coordinator> Pendingeventlist { get; set; }
        [DisplayName("Event")]
        public string EventCode { get; set; }
        public string SDate { get; set; }
        public string EDate { get; set; }
        //------------------------------------------Priyanka end-----------------------------------//
        #endregion public list properties end
    }
}
