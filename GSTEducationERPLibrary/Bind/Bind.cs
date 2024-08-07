using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSTEducationERPLibrary.Bind
{
    public class Bind
    {
        #region Variable Declare
        //----------sayali Co-ordinator Trainer  Batch Management start ---------//
        public string BatchCode { get; set; }
        [DisplayName("Batch Name")]
        public string BatchName { get; set; }
        [DisplayName("Course Name")]
        public string CourseName { get; set; }
        public string StudentCode { get; set; }
        public string CourseCode { get; set; }
        [DisplayName("No Of Student")]
        public int NoOfStudent { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }
        public int StatusId { get; set; }
        public int TypeId { get; set; }
        [DisplayName("Staff Name")]
        public string StaffCode { get; set; }
        public DateTime ReleseDate { get; set; }

        public int ScheduleId { get; set; }
        public string BatchTime { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BatchScheduleDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime RescheduleDate { get; set; }
        public string Duration { get; set; }
        [DisplayName("End Date")]
        public DateTime EndDate { get; set; }
        [DisplayName("Start Time")]
        public DateTime StartTime { get; set; }
        [DisplayName("End Time")]
        public DateTime EndTime { get; set; }


        //------------------Sayali---------------------//
        public string Status { get; set; }
        public string BranchCode { get; set; }
        [DisplayName("Staff Name")]
        public string StaffName { get; set; }
        [DisplayName("Student Name")]
        public string StudentName { get; set; }
        [DisplayName("Contact No")]
        public string ContactNo { get; set; }
        public string EMail { get; set; }
        public string SelectBatch { get; set; }
        public string AddBatch { get; set; }
        public string SelectStudent { get; set; }
        public string AddStudent { get; set; }
        [DisplayName("Lab Name")]
        public string LabName { get; set; }
        public string AddedStaffCode { get; set; }

        public string AddStudentCode { get; set; }
        public int AddNoOfStudent { get; set; }
        [DisplayName("Lab Capacity")]
        public int LabCapacity { get; set; }
        [DisplayName("Course Duration")]
        public string CourseDuration { get; set; }
        public string SelectStudentCode { get; set; }
        public int SelectNoOfStudent { get; set; }
        public List<Bind> lstBatchData { get; set; }
        public List<Bind> lstAdmittedStudent { get; set; }
        public List<Bind> lstSelectedStudent { get; set; }
        public List<Bind> lstBatchStudentList { get; set; }

        #endregion


        //-----------sayali Co-ordinator Trainer  Batch Management end ---------//
        //--------        shubhangi start         ----//
        //public string StudentName { get; set; }
        [DisplayName("Contact No")]
        public string ContactNumber { get; set; }
        [DisplayName("Email")]
        public string EmailId { get; set; }
        [DisplayName("Company Name")]
        public string CompanyName { get; set; }
        public string Designation { get; set; }
        public List<Bind> StudentList { get; set; }
        public string MDate { get; set; }
        public DateTime Date { get; set; }

        //------- shuhnagi end ----------//
        //-----vaihav staff profile -----------------------------------------------------------------//
        public string StaffPosition { get; set; }
        public string CourseType { get; set; }
        public string DOB { get; set; }
        public string PhoneNo { get; set; }
        public string CurrentAddress { get; set; }
        public string currentCity { get; set; }
        public string CurrentPinCode { get; set; }
        public string PermanentAddress { get; set; }
        public string PermanentCity { get; set; }
        public string PermanentPinCode { get; set; }
        public string MaritalStatus { get; set; }
        public string Nationality { get; set; }
        public string BloodGroup { get; set; }
        public string Gender { get; set; }
        public string AadharCardNo { get; set; }
        public string PanCardNo { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactNo { get; set; }
        public string EmergencyContactAddress { get; set; }
        public string FatherName { get; set; }
        public string FatherContactNo { get; set; }
        public string MotherName { get; set; }
        public string MotherContactNo { get; set; }
        public string BankName { get; set; }
        public string AccountHolderName { get; set; }
        public string AccountNumber { get; set; }
        public string BankBranchName { get; set; }
        public string IFSCCode { get; set; }
        public string MICRCode { get; set; }
        public string JoiningDate { get; set; }
        public string OfficialEmailId { get; set; }
        public string Password { get; set; }
        public string SSCYear { get; set; }
        public string HSCYear { get; set; }
        public string DiplomaYear { get; set; }
        public string GraduationName { get; set; }
        public string GraduationYear { get; set; }
        public string PostGraduationName { get; set; }
        public string PostGraduationYear { get; set; }
        public string IndustryName { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        public string CTC { get; set; }
        public string Experience { get; set; }
        public string TotalExperienceOfDesignation { get; set; }
        public string JobType { get; set; }
        public string Photograph { get; set; }
        public string DepartmentNameCurrent { get; set; }
        public string DesignationCurrent { get; set; }
        public string SkillNames { get; set; }
        public string BranchName { get; set; }
        public string PanCard { get; set; }
        public string AadharCard { get; set; }
        public string SSC { get; set; }
        public string HSC { get; set; }
        public string Diploma { get; set; }
        public string Graduation { get; set; }
        public string PostGraduation { get; set; }
        public string RelievingLetter { get; set; }
        public string SalarySlip { get; set; }
        public string SalaryStructure { get; set; }
        public string JoiningLetter { get; set; }
        public string ExperienceLetter { get; set; }
        public string MedicalCertificate { get; set; }
        public string Resume { get; set; }
        //---------------------------------------------------------------------

        //----------pratibha task maagemet ---------------------------------
        //=========================================Task Management===========================================//
        public string TaskAddedStaff { get; set; }
        public string assignStaffcode { get; set; }
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string TaskCode { get; set; }
        public string AssignByStaffCode { get; set; }
        public string AssignToStaffCode { get; set; }
        public string Priority { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime TaskStartDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime TaskEndDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:hh:mm:ss tt}", ApplyFormatInEditMode = true)]
        public DateTime TaskStartTime { get; set; }
        [DisplayFormat(DataFormatString = "{0:hh:mm:ss tt}", ApplyFormatInEditMode = true)]
        public DateTime TaskEndTime { get; set; }
        public string TaskDescription { get; set; }
        public string StatusName { get; set; }
        public List<Bind> lstTaskManagement { get; set; }
        public int StaffPositionid { get; set; }
        //--------------------------------------------------
        public List<Bind> AdminList { get; set; }
        public bool AllowUpdate { get; set; }

        //----------pratiha task maagemet ---------------------------------
        //--------------------------------------Punam Contact---------------------------------------//
        [DisplayName("Contact")]
        public string To { get; set; }
        public string compose { get; set; }
        [DisplayName("Full Name")]
        public string FullName { get; set; }
        [DisplayName(" Address")]
        public string Address { get; set; }
        public string FileUploader { get; set; }
        public int Id { get; set; }
        public string City { get; set; }
        public string DeleteId { get; set; }
        public string Type { get; set; }
        public string Subject { get; set; }
        //------------------------------------------Punam Contact End-------------------------------//
    }
}
