using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web;


namespace GSTEducationERPLibrary.Placement
{
	public class Placement
	{
		#region variable declare
		public string StudentCode { get; set; }
		public int SkillId { get; set; }
		[DisplayName("Skill Name")]
		public string SkillName { get; set; }
		public int TechnolgyId { get; set; }
		[DisplayName("Technology Name")]
		public string TechnologyName { get; set; }
		public int ExamId { get; set; }
		[DisplayName("Exam Name")]
		public string ExamName { get; set; }
		public string CourseCode { get; set; }
		public DateTime RegisterDate { get; set; }
		public string Duration { get; set; }
		[Range(1, 999, ErrorMessage = "Total Marks must be a number between 1 and 999.")]
		[RegularExpression(@"^[1-9]\d{0,2}(\.\d{1,2})?$", ErrorMessage = "Total Marks must be a number between 1 and 999 with up to two decimal places.")]
		[DisplayName("Total Marks")]
		public float TotalMarks { get; set; }
		[DisplayName("Passing Marks")]
		public float PassingMarks { get; set; }
		public string StaffCode { get; set; }
		[DisplayName("Total No Of Questions")]
		public int TotalNoOfQuestion { get; set; }
		[DisplayName("Status")]
		public int StatusId { get; set; }
		public int DesignationId { get; set; }
		[DisplayName("Designation")]
		public string DesignationName { get; set; }
		public int InduastryId { get; set; }
		[DisplayName("Industry Name")]
		public string InduastryName { get; set; }
		public int CompanyId { get; set; }
		public string CompanyCode { get; set; }
		[DisplayName("Company")]
		public string CompanyName { get; set; }
		public DateTime CompanyRegisterDate { get; set; }
		public string CompanyEmail { get; set; }
		public string Address { get; set; }
		public int CityId { get; set; }
		[DisplayName("HR Name")]
		public string HR1Name { get; set; }
		public string HR1Mail { get; set; }
		public string HR1Contact { get; set; }
		public string HR2Name { get; set; }
		public string HR2Mail { get; set; }
		public string HR2Contact { get; set; }
		public string HR3Name { get; set; }
		public string HR3Mail { get; set; }
		public string HR3Contact { get; set; }
		public string CompanyDescription { get; set; }
		public int AssignExamId { get; set; }
		public string BatchCode { get; set; }
		[DisplayName("Batch Name")]
		public string BatchName { get; set; }
		public DateTime CompleteTillDate { get; set; }
		public DateTime AssignDate { get; set; }
		[DataType(DataType.Date)]
		[DisplayName("Exam Date")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime ExamDate { get; set; }
		[DataType(DataType.Time)]
		[DisplayName("Exam Time")]
		public DateTime ExamTime { get; set; }
		public string LabCode { get; set; }
		[DisplayName("Lab Name")]
		public string LabName { get; set; }
		public DateTime ArrangedDate { get; set; }
		public int ResultId { get; set; }
		[DisplayName("Obtained Marks")]
		public float ObtainedMarks { get; set; }
		public int AttendanceStatusId { get; set; }
		public string ResultStatus { get; set; }
		public DateTime ResultAddedDate { get; set; }
		public int MockId { get; set; }
		public string MockCode { get; set; }
		[DisplayName("Mock Name")]
		public string MockName { get; set; }
		public int MockSectiontId { get; set; }
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime MockDate { get; set; }
		[DataType(DataType.Time)]
		public DateTime StartTime { get; set; }
		public int? MockPerformanceId { get; set; }
		public string Attendance { get; set; }
		public string PerformanceStatus { get; set; }
		public int OpenningId { get; set; }
		public string OpeningCode { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string Experience { get; set; }
		public int NoOfOpening { get; set; }
		public string CTC { get; set; }
		public int SourceId { get; set; }
		public string JobDescription { get; set; }
		public string JobDescriptionFile { get; set; }
		public DateTime OpeningAddDate { get; set; }
		[DisplayName("Source Name")]
		public string SourceName { get; set; }
		public int PlacementId { get; set; }
		public int CandidateStatusId { get; set; }
		public string Location { get; set; }
		[DataType(DataType.Date)]
		[DisplayName("Interview Date")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime InterviewDate { get; set; }
		[DataType(DataType.Time)]
		public DateTime Time { get; set; }
		[DisplayName("Interview Round")]
		public string InterviewRound { get; set; }
		[DisplayName("Interview Mode")]
		public string InterviewMode { get; set; }
		public string FeedbackStatus { get; set; }
		public int TestimonialId { get; set; }
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime Date { get; set; }
		public string Comments { get; set; }
		public string UploadVideo { get; set; }
		public string UploadPDF { get; set; }
		public string UploadAudio { get; set; }

		//------------Priyanka-----------//
		public List<Placement> CourseList { get; set; }
		public string BranchCode { get; set; }
		[DisplayName("Course Name")]
		public string CourseName { get; set; }
		public int NoOfStudent { get; set; }
		public int Placed { get; set; }
		public int Active { get; set; }
		public int Studentid { get; set; }

		//-------------------Snehal----------------------//
		public List<Placement> lstExams { get; set; }

		[DisplayName("Exam Date")]
		public string DateString { get; set; }
		[DisplayName("Exam Time")]
		public string TimeString { get; set; }
		[DisplayName("Student Name")]
		public string StudentName { get; set; }
		[DisplayName("Supervisor Name")]
		public string SupervisorName { get; set; }
		public List<Placement> lstAllExams { get; set; }
		[DisplayName("Duration")]
		public TimeSpan SelectedDuration { get; set; }
		[DisplayName("Exam Paper File")]
		public HttpPostedFileBase TestPaperFile { get; set; }
		public String FilePth { get; set; }
		public List<Placement> lstPendingExams { get; set; }
		public List<Placement> lstConductedExams { get; set; }
		public string Center { get; set; }
		public string SelectedStudents { get; set; }
		public string Status { get; set; }

		public string CenterCode { get; set; }
		private TimeSpan _duration;

		[DisplayName("Duration")]
		public string SDuration
		{
			get { return _duration.ToString(); }
			set
			{
				if (TimeSpan.TryParse(value, out TimeSpan parsedDuration))
				{
					_duration = parsedDuration;
				}
				// Handle parsing error if needed
			}
		}
		[DisplayName("Duration")]
		public string FormattedDuration
		{
			get { return _duration.ToString(@"hh\:mm"); }
			set
			{
				if (TimeSpan.TryParse(value, out TimeSpan formattedDuration))
				{
					_duration = formattedDuration;
				}
				// Handle parsing error if needed
			}
		}
		[DisplayName("Contact No")]
		public string ContactNo { get; set; }
		public string Email { get; set; }
		public List<Placement> lstShortlistedCandidates { get; set; }
		public List<Placement> lstScheduledInterview { get; set; }
		public List<Placement> lstInterviewPerformance { get; set; }
		public List<Placement> lstAllRoundDetails { get; set; }
		[DisplayName("Higher Qualification")]
		public string HighestQualification { get; set; }
		public string Round1 { get; set; }
		public string Round2 { get; set; }
		public string Round3 { get; set; }
		public string Description { get; set; }
		[DisplayName("CTC")]
		public decimal CTC_LPA { get; set; }
		[DisplayName("Offer Letter")]
		public HttpPostedFileBase OfferLetter { get; set; }
		public IEnumerable<string> Technology { get; set; }
		[DataType(DataType.Date)]
		[DisplayName("Joining Date")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime JoiningDate { get; set; }
		public DateTime EndTime { get; set; }
		//-----------------------------------------------End------------------------------------------------//
		//-------------------------------------------Punam Company-------------------------------------------//
		public string DateS { get; set; }
		public int CountryId { get; set; }
		public int StateId { get; set; }
		public string City { get; set; }
		public List<Placement> ListCompany { get; set; }
		//-----------------------------------------------End--------------------------------------------------//
		//---------------------------------------------Shubhangi Mock---------------------------------------//
		[DisplayName("Interviewr Name")]
		public string InterviewrName { get; set; }
		public string SelectedStudentCodes { get; set; }
		public int selectedTechnologyId { get; set; }
		public List<Placement> lstInternalMock { get; set; }
		public List<Placement> lstExternalMock { get; set; }
		[DisplayName("Communication")]
		public int CommunicationRating { get; set; }
		[DisplayName("Technical Knowledge")]
		public int TechnicalRating { get; set; }
		[DisplayName("Confiedence")]
		public int ConfidenceRating { get; set; }
		[DisplayName("Project Knowledge")]
		public int ProjectKnowledgeRating { get; set; }
		[DisplayName("Performance")]
		public int PerformanceRating { get; set; }
		[DisplayName("Date")]
		public string MDate { get; set; }
		public List<string> StudentList { get; set; }
		public int QualificationId { get; set; }
		public string Qualification { get; set; }
		public int ExperienceId { get; set; }
		public int AttendanceId { get; set; }
		//----------------------------------------------Testimonial Prem -----------------------------
		public string Comment { get; set; }
		public string Audio { get; set; }
		public string Video { get; set; }
		public string Pdf { get; set; }
		public List<Placement> lstTestimonial { get; set; }
		public string CandidateCode { get; set; }
		public string ContactNumber { get; set; }
		public string EmailId { get; set; }

		[DataType(DataType.Date)]
		[DisplayName("Joining Date")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public string JoiningDatePM { get; set; }
		public string TodayDate { get; set; }
		[DisplayName("Total Fees")]
		public float TotalFees { get; set; }
		[DisplayName("Paid Amount")]
		public float PaidAmount { get; set; }
		[DisplayName("Pending Amount")]
		public float PendingAmount { get; set; }
		public List<Placement> lstFees { get; set; }
		public HttpPostedFileBase PDFFILe { get; set; }
		public HttpPostedFileBase AudioFIle { get; set; }
		public HttpPostedFileBase VideoFILe { get; set; }
		public string PdfPath { get; set; }
		public string AudioPath { get; set; }
		public string VideoPath { get; set; }
		//--------------------------------------Pratiksha(StudentList)--------------------------------------------


		[DisplayName("Mobile No")]
		public string MobileNo { get; set; }
		[DisplayName("Year Of Passing")]
		public string YearOfPassing { get; set; }
		public string StudentType { get; set; }
		public List<Placement> lstRelesedStudent { get; set; }
		public List<Placement> lstOnHoldStudent { get; set; }
		public List<Placement> lstExtRelesedStudent { get; set; }
		public List<Placement> lstExtOnHoldStudent { get; set; }

		[DisplayName("First Name")]
		public string FirstName { get; set; }

		[DisplayName("Middle Name")]
		public string MiddleName { get; set; }

		[DisplayName("Last Name")]
		public string LastName { get; set; }

		[DisplayName("Alternate Number")]
		public string AlternateContact { get; set; }
		public string Gender { get; set; }

		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
		public DateTime RegistrationDate { get; set; }
		[DisplayName("Father Name")]
		public string FatherName { get; set; }
		[DisplayName("Contact No")]
		public string MContactNo { get; set; }
		[DisplayName("Mother Name")]
		public string MotherName { get; set; }
		[DisplayName("Local Addr")]
		public string LocalAddr { get; set; }
		[DisplayName("Permant Addr")]
		public string PermanantAddress { get; set; }
		[DisplayName("City Name")]
		public string CityName { get; set; }
		public string Pin { get; set; }
		[DisplayName("SSC Year")]
		public string SSCYear { get; set; }
		[DisplayName("SSC File")]
		public string SSCFile { get; set; }
		[DisplayName("HSC Year")]
		public string HSCYear { get; set; }
		[DisplayName("HSC File")]
		public string HSCFile { get; set; }
		[DisplayName("Diploma Year")]
		public string DiplomaYear { get; set; }
		[DisplayName("Diploma File")]
		public string DiplomaFile { get; set; }
		[DisplayName("Graduation Year")]
		public string GraduationYear { get; set; }
		[DisplayName("Graduation File")]
		public string GraduationFile { get; set; }
		[DisplayName("PostGraduation Year")]
		public string PostGraduationYear { get; set; }
		[DisplayName("PostGraduation File")]
		public string PostGraduationFile { get; set; }
		public string Photograph { get; set; }
		[DisplayName("Aadhar Card")]
		public string AadharCard { get; set; }
		[DisplayName("Pan Card")]
		public string PanCard { get; set; }
		[DisplayName("Project Name")]
		public string ProjectName { get; set; }
		public List<string> ProjectCodes { get; set; }
		public List<int> TechnologyIds { get; set; }
		public List<Placement> lstRegistrationList { get; set; }
		public List<Placement> SkillList { get; set; }
		public string JobType { get; set; }
		public int DepartmentId { get; set; }
		public string[] SkillId1 { get; set; }
		public string Skill { get; set; }
		public string[] TechnolgyId1 { get; set; }
		public string Technologies { get; set; }
		public string ProjectCode { get; set; }
		[DisplayName("Projects")]
		public string[] ProjectCode1 { get; set; }
		public string Projects { get; set; }
		public IEnumerable<HttpPostedFileBase> Resume { get; set; }
		public string ResumeFilePath { get; set; }
		public string IndustryName { get; set; }
		public string TotalExperienceOfDesignation { get; set; }
		public List<Placement> studentDetailsModels { get; set; }

		//-------------------------------------- END Pratiksha(StudentList)--------------------------------------------

		#endregion
	}
}
