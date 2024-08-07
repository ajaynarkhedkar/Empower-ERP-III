using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Helper;

namespace GSTEducationERPLibrary.Placement
{
	public class BALPlacement
	{
        MSSQL DBHelper = new MSSQL();
        /// <summary>
        /// method for Display Total Count of Candidate on Dashboard.
        /// </summary>
        /// <returns></returns>
        public async Task<SqlDataReader> CountTotalCandidatePCAsync(Placement obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "CountTotalCandidate");
            Param.Add("@BranchCode", obj.BranchCode);
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTPlacement", Param);
            return dr;
        }
        /// <summary>
        /// method for Get Count for Placed Candidate to show on Dashboard.
        /// </summary>
        /// <returns></returns>
        public async Task<SqlDataReader> CountPlacedCandidatePCAsync(Placement obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "CountPlacedCandidate");
            Param.Add("@BranchCode", obj.BranchCode);
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTPlacement", Param);
            return dr;
        }
        /// <summary>
        /// method for Get count of Active Candidate to show on Dashboard.
        /// </summary>
        /// <returns></returns>
        public async Task<SqlDataReader> CountActiveCandidatePCAsync(Placement obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "CountActiveCandidate");
            Param.Add("@BranchCode", obj.BranchCode);
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTPlacement", Param);
            return dr;
        }
        /// <summary>
        /// method for Get Total Count of Companies for Dashboard.
        /// </summary>
        /// <returns></returns>
        public async Task<SqlDataReader> CountCompaniesPCAsync(Placement objP)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "CountTotalCompanies");
            Param.Add("@BranchCode", objP.BranchCode);
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTPlacement", Param);
            return dr;
        }
        /// <summary>
        /// method for Get Total Count of Total Companies for Dashboard.
        /// </summary>
        /// <returns></returns>
        public async Task<SqlDataReader> CountOpeningPCAsync(Placement objP)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "CountTotalOpenings");
            Param.Add("@BranchCode", objP.BranchCode);
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTPlacement", Param);
            return dr;
        }
        /// <summary>
        /// method for Get Total Count of DesignationName for Dashboard.
        /// </summary>
        /// <returns></returns>
        public async Task<SqlDataReader> CountDesignationPCAsync(Placement obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "CountDesignation");
            Param.Add("@BranchCode", obj.BranchCode);
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTPlacement", Param);
            return dr;
        }
        /// <summary>
        /// method for show graphical presentation for Company Vs Close and Open Job.
        /// </summary>
        /// <returns></returns>
        public async Task<DataSet> GraphCompanyPCAsync(Placement objP)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "GraphCompanyJobCount");
            Param.Add("@BranchCode", objP.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
            return ds;
        }
        /// <summary>
        /// method for show graphical presentation for Candidate Count with respective Status.
        /// </summary>
        /// <returns></returns>
        public async Task<DataSet> GraphCandidateCountPCAsync(Placement objP)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "GraphCandidateStatusCount");
            Param.Add("@BranchCode", objP.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
            return ds;
        }
        /// <summary>
        /// method for show graphical presentation for Designationwise Open & Close Count.
        /// </summary>
        /// <returns></returns>
        public async Task<DataSet> GraphPositionOpenClosePCAsync(Placement objP)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "GraphPositionVsOpenCloseJob");
            Param.Add("@BranchCode", objP.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
            return ds;
        }
        /// <summary>
        /// method for Get Courselist for Dropdown Selection for Count.
        /// </summary>
        /// <returns></returns>
        public async Task<DataSet> GetCoursePCAsync(Placement objP)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "SelectCourseName");
            Param.Add("@BranchCode", objP.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
            return ds;
        }
        /// <summary>
        /// method for fetch Batchlist on selection of CourseList for Count.
        /// </summary>
        /// <param name="coursecode"></param>
        /// <returns></returns>
        public async Task<DataSet> BatchListPCAsync(Placement objP)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "SelectBatchName");
            Param.Add("@CourseCode", objP.CourseCode);
            Param.Add("@BranchCode", objP.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
            return ds;
        }
        /// <summary>
        /// method for Update fetched Count of Batchlist on selection of BatchName.
        /// </summary>
        /// <param name="batchname"></param>
        /// <returns></returns>
        public async Task<SqlDataReader> DashUpdateCountPCAsync(Placement obj)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@Flag", "DashboardCountUpdate");
                Param.Add("@BatchCode", obj.BatchName);
                Param.Add("@BranchCode", obj.BranchCode);
                SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTPlacement", Param);
                return dr;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's requirement
                Console.WriteLine("An error occurred in DashUpdateCountPCAsync: " + ex.Message);
                throw; // Re-throw the exception to the calling method
            }
        }
		///----------------------------------------------Snehal Exam Management------------------------------------------------//
		/// <summary>
		/// Used to get internal students exams list.
		/// </summary>
		/// <param name="StatusId"> Status id is used to get status wise data of all exams. </param>
		/// <returns> It returns status wise data like assigned, pending and conducted..</returns>
		public async Task<DataSet> ListInternalStudentsExamSNAsync(int StatusId, string branchCode, string courseCode, DateTime startDate, DateTime endDate)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ListInternalStudentsExamSN");
			Param.Add("@StatusId", StatusId.ToString());
			Param.Add("@BranchCode", branchCode);
			Param.Add("@StartDate", startDate.ToString("yyyy-MM-dd HH:mm:ss.sss"));
			Param.Add("@EndDate", endDate.ToString("yyyy-MM-dd HH:mm:ss.sss"));
			Param.Add("@CourseCode", courseCode);
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// Used to get external students exams list.
		/// </summary>
		/// <param name="StatusId"> Status id is used to get status wise data of all exams. </param>
		/// <returns> It returns status wise data like assigned, pending and conducted.</returns>
		public async Task<DataSet> ListExternalStudentsExamSNAsync(Placement objp)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ListExternalStudentsExamSN");
			Param.Add("@StatusId", objp.StatusId.ToString());
			Param.Add("@BranchCode", objp.BranchCode);
			Param.Add("@StartDate", objp.StartDate.ToString("yyyy-MM-dd HH:mm:ss.sss"));
			Param.Add("@EndDate", objp.EndDate.ToString("yyyy-MM-dd HH:mm:ss.sss"));
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		public async Task<DataSet> ListConductedExamExtStudentSN(Placement objp)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ListConductedExamExtStudentSN");
			Param.Add("@StatusId", objp.StatusId.ToString());
			Param.Add("@BranchCode", objp.BranchCode);
			Param.Add("@StartDate", objp.StartDate.ToString("yyyy-MM-dd HH:mm:ss.sss"));
			Param.Add("@EndDate", objp.EndDate.ToString("yyyy-MM-dd HH:mm:ss.sss"));
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// It used to get all exams list 
		/// </summary>
		/// <returns> It rerurns all exams list for placement process</returns>
		public async Task<DataSet> ListAllExamSNAsync(Placement ObjP)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ListAllExamSN");
			Param.Add("@BranchCode", ObjP.BranchCode);
			Param.Add("@StartDate", ObjP.StartDate.ToString("yyyy-MM-dd HH:mm:ss.sss"));
			Param.Add("@EndDate", ObjP.EndDate.ToString("yyyy-MM-dd HH:mm:ss.sss"));
			Param.Add("@CourseCode", ObjP.CourseCode);
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// Used to get coursewise exams.
		/// </summary>
		/// <param name="CourseCode">Passed to get exams of specific course.</param>
		/// <returns>It returns exam list of that course.</returns>
		public async Task<DataSet> ListCourseWiseExamSN(string CourseCode)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ListCourseWiseExamSN");
			Param.Add("@CourseCode", CourseCode);
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// It used to get all courses list
		/// </summary>
		/// <returns> It returns all courses available in backend</returns>
		public async Task<DataSet> ListAllCourseSN()
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ListAllCourseSN");
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		public async Task<DataSet> ListAllOpeningSN()
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ListOpenings");
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		public async Task<DataSet> ListAllTechnologySN()
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ListTechnology");
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// It used to add new exam to database.
		/// </summary>
		/// <param name="objP">Object is passed to save new exam.</param>
		public async Task AddNewExamSNAsync(Placement objP)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "AddNewExamSN");
			Param.Add("@TestName", objP.ExamName);
			Param.Add("@CourseCode", objP.CourseCode);
			string technologyIds = string.Join(",", objP.Technology);
			Param.Add("@TechnologyIds", technologyIds.ToString());
			Param.Add("@RegisterDate", DateTime.Today.ToString("yyyy-MM-dd"));
			string formattedDuration = objP.SelectedDuration.ToString(@"hh\:mm\:ss");
			Param.Add("@Duration", formattedDuration.ToString());
			Param.Add("@TotalMarks", objP.TotalMarks.ToString());
			Param.Add("@PassingMarks", objP.PassingMarks.ToString());
			Param.Add("@TestPaperFile", objP.FilePth);
			Param.Add("@StaffCode", objP.StaffCode);
			Param.Add("@TotalNoOfQuestion", objP.TotalNoOfQuestion.ToString());
			Param.Add("@StatusId", objP.StatusId.ToString());
			await DBHelper.ExecuteStoreProcedure("GSTPlacement", Param);
		}
		/// <summary>
		/// Used to get released batches
		/// </summary>
		/// <param name="CourseCode">Used to that batches of that course</param>
		/// <returns>It returns the released batches by using coursecode</returns>
		public async Task<DataSet> ListReleasedBatchSN(string CourseCode)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ListReleasedBatchSN");
			Param.Add("@CourseCode", CourseCode);
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// It used to get Student list of one batch
		/// </summary>
		/// <param name="BatchCode">Batchcode is used to get that batch students</param>
		/// <returns>It returns the student list of that batch</returns>
		public async Task<DataSet> ListStudentSN(string BatchCode)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ListStudentSN");
			Param.Add("@BatchCode", BatchCode);
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// It used to get trainerlist
		/// </summary>
		/// <returns>It returns the list of trainer</returns>
		public async Task<DataSet> ListTrainerSN()
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ListTrainerSN");
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// It used to get centerlist
		/// </summary>
		/// <returns>It returns the list of Center</returns>
		public async Task<DataSet> ListCenterSN()
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ListCenterSN");
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// It used to get lab list
		/// </summary>
		/// <param name="CenterCode">It used to get lab of that center</param>
		/// <returns>It returns the lab list of selected center</returns>
		public async Task<DataSet> ListLabSN(string BranchCode)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ListLabSN");
			Param.Add("@BranchCode", BranchCode);
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// Used to save assign exam data for internal students
		/// </summary>
		/// <param name="objP">Object is used to save specific data</param>
		/// <returns>It saves the data of assign exam</returns>
		public async Task AssignExamIntStudentSN(Placement objP)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "AssignExamSN");
			Param.Add("@BatchCode", objP.BatchName);
			Param.Add("@TestId", objP.ExamName);
			Param.Add("@StudentCodes", objP.SelectedStudents);
			Param.Add("@TrainerCodeAssignedByCode", objP.StaffCode);
			Param.Add("@AssignDate", DateTime.Today.ToString("yyyy-MM-dd"));
			Param.Add("@TestDate", objP.ExamDate.ToString("yyyy-MM-dd HH:mm:ss.sss"));
			Param.Add("@TestTime", objP.ExamTime.ToString("yyyy-MM-dd HH:mm:ss.sss"));
			Param.Add("@TrainerCodeSupervisorCode", objP.SupervisorName);
			Param.Add("@LabCode", objP.LabCode);
			Param.Add("@ArrangedDateSystemDate", DateTime.Today.ToString("yyyy-MM-dd HH:mm:ss.sss"));
			Param.Add("@StaffCodeArrangedBy", objP.StaffCode);
			Param.Add("@StatusId", "25");
			Param.Add("@StartDate", objP.ExamDate.ToString("yyyy-MM-dd"));
			Param.Add("@EndDate", objP.ExamDate.ToString("yyyy-MM-dd"));
			Param.Add("@StartT", objP.ExamTime.ToString("HH:mm:ss.fffffff"));
			Param.Add("@EndT", objP.EndTime.ToString("HH:mm:ss.fffffff"));
			await DBHelper.ExecuteStoreProcedure("GSTPlacement", Param);
		}
		/// <summary>
		/// Used to save assign exam data for external students
		/// </summary>
		/// <param name="objP">Object is used to save specific data</param>
		/// <returns>It saves the data of assign exam</returns>

		public async Task AssignExamExtStudentSN(Placement objP)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "AssignExamSN");
			Param.Add("@TestId", objP.ExamName);
			Param.Add("@StudentCodes", objP.StudentCode);
			Param.Add("@TrainerCodeAssignedByCode", objP.StaffCode);
			Param.Add("@AssignDate", DateTime.Today.ToString("yyyy-MM-dd"));
			Param.Add("@TestDate", objP.ExamDate.ToString("yyyy-MM-dd HH:mm:ss.sss"));
			Param.Add("@TestTime", objP.ExamTime.ToString("yyyy-MM-dd HH:mm:ss.sss"));
			Param.Add("@TrainerCodeSupervisorCode", objP.SupervisorName);
			Param.Add("@LabCode", objP.LabCode);
			Param.Add("@ArrangedDateSystemDate", DateTime.Today.ToString("yyyy-MM-dd HH:mm:ss.sss"));
			Param.Add("@StaffCodeArrangedBy", objP.StaffCode);
			Param.Add("@StatusId", "25");
			await DBHelper.ExecuteStoreProcedure("GSTPlacement", Param);
		}
		/// <summary>
		/// used to get external students list
		/// </summary>
		/// <param name="CourseCode">It used to get student of that course</param>
		/// <returns>Returns the student list of that course</returns>
		public async Task<DataSet> ListExternalStudentsSN(string BranchCode)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ListExternalStudentsSN");
			Param.Add("@BranchCode", BranchCode);
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// It is used to get details of exam.
		/// </summary>
		/// <param name="id">Parameter id is used to get details from that id.</param>
		/// <returns>It returns the details of exam of that particular examid.</returns>
		public async Task<DataSet> DetailsExamSN(int id)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "DetailsExamSN");
			Param.Add("@TestId", id.ToString());
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// This function is used to update exam details.
		/// </summary>
		/// <param name="objP">Parameter is passed to get data.</param>
		/// <returns>It updates the exam details of exam for that particular id.</returns>
		public async Task UpdateExamSN(Placement objP)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "UpdateExamSN");
			Param.Add("@TestId", objP.ExamId.ToString());
			Param.Add("@Duration", objP.SelectedDuration.ToString());
			Param.Add("@TotalMarks", objP.TotalMarks.ToString());
			Param.Add("@PassingMarks", objP.PassingMarks.ToString());
			Param.Add("@TestPaperFile", objP.FilePth);
			Param.Add("@TotalNoOfQuestion", objP.TotalNoOfQuestion.ToString());
			await DBHelper.ExecuteStoreProcedure("GSTPlacement", Param);
		}
		/// <summary>
		/// This action is used to details of assigned exam.
		/// </summary>
		/// <param name="id">Parameter is passed to get that id related data.</param>
		/// <returns>It returns that assigntestid related all data.</returns>
		public async Task<DataSet> DetailsAssignExamExternalSN(int id)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "DetailsAssignExamExternalSN");
			Param.Add("@AssignTestId", id.ToString());
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// This function is used updated changes of assigned exam.
		/// </summary>
		/// <param name="objP">Object is used to pass data.</param>
		/// <returns>It Updates the changes to database which is done by user.</returns>
		public async Task UpdateAssignExamSN(Placement objP)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "UpdateAssignExamSN");
			Param.Add("@AssignTestId", objP.AssignExamId.ToString());
			Param.Add("@TestDate", objP.Date.ToString("yyyy-MM-dd"));
			Param.Add("@TestTime", objP.StartTime.ToString("yyyy-MM-dd HH:mm:ss.sss"));
			Param.Add("@TrainerCodeSupervisorCode", objP.StaffCode);
			Param.Add("@LabCode", objP.LabName);
			Param.Add("@StatusId", objP.StatusId.ToString());
			Param.Add("@StartT", objP.StartTime.ToString("HH:mm:ss.fffffff"));
			Param.Add("@EndT", objP.Time.ToString("HH:mm:ss.fffffff"));
			await DBHelper.ExecuteStoreProcedure("GSTPlacement", Param);
		}
		/// <summary>
		/// This function is used to get status list.
		/// </summary>
		/// <returns>It returns status list which is required for exam management.</returns>
		public async Task<DataSet> ListStatusSN()
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ListStatusSN");
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// This function is used to get assigned exam data which was conducted.
		/// </summary>
		/// <param name="id">Parameter is used to get that id related data.</param>
		/// <returns>It returns the data of conducted exam. </returns>
		public async Task<DataSet> DetailConductedExamExtSN(int id)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "DetailConductedExamExtSN");
			Param.Add("@AssignTestId", id.ToString());
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// It is used to get result of external student.
		/// </summary>
		/// <param name="id"> id is used to get result of that assigned test id.</param>
		/// <returns>It returns result for that test.</returns>
		public async Task<DataSet> ViewResultExternalStudentSN(int id)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ViewResultExternalStudentSN");
			Param.Add("@AssignTestId", id.ToString());
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// This function is used to add result of exam.
		/// </summary>
		/// <param name="objP">Object is passed to add result of exam.</param>
		/// <returns>It saves the result of coducted exams.</returns>
		public async Task AddResultSN(Placement objP)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "AddResultSN");
			Param.Add("@TestAssignedId", objP.AssignExamId.ToString());
			Param.Add("@StudentCodes", objP.StudentCode);
			Param.Add("@ObtainedMarks", objP.ObtainedMarks.ToString());
			Param.Add("@AttendanceStatusId", objP.AttendanceStatusId.ToString());
			Param.Add("@ResultStatus", objP.ResultStatus);
			Param.Add("@TrainerCodeResultAddedBy", objP.StaffCode);
			Param.Add("@ResultAddedDate", DateTime.Today.ToString("yyyy-MM-dd HH:mm:ss.sss"));
			await DBHelper.ExecuteStoreProcedure("GSTPlacement", Param);
		}
		/// <summary>
		/// This function is used to get details of conducted exam for internal students.
		/// </summary>
		/// <param name="id">id is used to get that id's means conducted tests details.</param>
		/// <returns>It returns the details of conducted exam.</returns>
		public async Task<DataSet> DetailConductedExamIntSN(int id)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "DetailsConductedExamIntSN");
			Param.Add("@AssignTestId", id.ToString());
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// This function is used to get list of students for that id means that assignedtestid.
		/// </summary>
		/// <param name="id">id is used to get particular data from id.</param>
		/// <returns>It returns the list of external students from that id.</returns>
		public async Task<DataSet> ListInternalStudentCondExamSN(int id)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ListInternalStudentCondExamSN");
			Param.Add("@AssignTestId", id.ToString());
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// This function is used to view result of internal students.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<DataSet> ViewResultInternalStudentsSN(int id)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ViewResultInternalStudentsSN");
			Param.Add("@TestAssignedId", id.ToString());
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		public async Task<DataSet> TechnologyNameSN(int id)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "GetTechnologyNamesSN");
			Param.Add("@TestId", id.ToString());
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// This function is used to get exam list for external students from there skills.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<DataSet> ListExamExternalStudentsSN(string StudentCode)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ListExamsFromPrefferedSkillExtStu");
			Param.Add("@StudCode", StudentCode);
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		public async Task<DataSet> GetExamDurationSN(int id)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "GetExamDuration");
			Param.Add("@TestId", id.ToString());
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		public async Task<bool> IsExamAvailableSN(string ExamName, string CourseCode)
		{
			try
			{
				Dictionary<string, string> Param = new Dictionary<string, string>();
				Param.Add("@flag", "IsExamAvailableSN");
				Param.Add("@TestName", ExamName);
				Param.Add("@CourseCode", CourseCode);
				DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
				return ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && Convert.ToInt32(ds.Tables[0].Rows[0]["IsAvailable"]) == 1;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error in IsExamAvailableSN: " + ex.Message);
				return false; // Return false in case of an error
			}
		}

		public async Task<bool> IsExamAssignedToBatchSN(int ExamId, string BatchCode)
		{
			try
			{
				Dictionary<string, string> Param = new Dictionary<string, string>();
				Param.Add("@flag", "IsExamAssignedToBatchSN");
				Param.Add("@TestId", ExamId.ToString());
				Param.Add("@BatchCode", BatchCode);
				DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
				return ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && Convert.ToInt32(ds.Tables[0].Rows[0]["IsAssigned"]) == 1;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error in IsExamAssignedToBatch: " + ex.Message);
				return false; // Return false in case of an error
			}
		}
		/// <summary>
		/// This function is used to get available labs which is free to use means not assigned anywhere.
		/// </summary>
		/// <param name="objP">necessary details to get free labs means branchcode to get that specific batch related data,
		/// batchcode to check apacity of free lab from that particular batch count,
		/// Startdate and enddate to get details of that particular day on which user want to use that lab,both dates are same in this case because exam will be held on one day only,
		/// starttime and end time is used to get details for that particular time.
		/// </param>
		/// <returns>It returns available labs from date and time.</returns>
		public async Task<DataSet> ReadAvailableLabs(Placement objP)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ReadAvailableLabs");
			Param.Add("@BranchCode", objP.BranchCode);
			Param.Add("@BatchCode", objP.BatchCode);
			Param.Add("@StartDate", objP.StartDate.ToString("yyyy-MM-dd"));
			Param.Add("@EndDate", objP.StartDate.ToString("yyyy-MM-dd"));
			Param.Add("@StartT", objP.StartTime.ToString("HH:mm:ss.fffffff"));
			Param.Add("@EndT", objP.EndTime.ToString("HH:mm:ss.fffffff"));
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// This function is used to details of available labs.
		/// </summary>
		/// <param name="objP">Object is used to pass necessory details.</param>
		/// <returns>It returns available labs.</returns>
		public async Task<DataSet> ReadAvailableLabForExternalStu(Placement objP)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ReadAvailableLabsWithoutCapacity");
			Param.Add("@BranchCode", objP.BranchCode);
			Param.Add("@StartDate", objP.StartDate.ToString("yyyy-MM-dd"));
			Param.Add("@EndDate", objP.StartDate.ToString("yyyy-MM-dd"));
			Param.Add("@StartT", objP.StartTime.ToString("HH:mm:ss.fffffff"));
			Param.Add("@EndT", objP.EndTime.ToString("HH:mm:ss.fffffff"));
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		//-----------------------------------------Snehal Interview-----------------------------//
		public async Task<DataSet> ListShortlistedStudentsSN(Placement objP)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ListShortlistedStudentsInterviewSN");
			Param.Add("@BranchCode", objP.BranchCode);
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		public async Task<DataSet> ListScheduledInterviewSN(Placement objP)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ListScheduledInterviewSN");
			Param.Add("@BranchCode", objP.BranchCode);
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		public async Task<DataSet> DetailScheduleInterviewSN(int id)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "DetailsShortlistedStudentSN");
			Param.Add("@PlacementId", id.ToString());
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		public async Task<DataSet> ListLocationSN()
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ListLocationSN");
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		public async Task<DataSet> ListInterviewPerformanceSN(string BranchCode)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ListInterviewPerformanceSN");
			Param.Add("@BranchCode", BranchCode);
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		public async Task<DataSet> DetailsAllRoundSingleCandidateSN(string CandidateCode, string BranchCode)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@Flag", "DetailsAllRoundSingleCandidateSN");
			Param.Add("@StudCode", CandidateCode);
			Param.Add("@BranchCode", BranchCode);
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}

		public async Task<DataSet> ListPlacedStudentsSN(string BranchCode)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ListPlacedStudentsSN");
			Param.Add("@BranchCode", BranchCode);
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}

		public async Task ScheduleInterviewRound1SN(Placement objP)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ScheduleInterviewRound1SN");
			Param.Add("@PlacementId", objP.PlacementId.ToString());
			Param.Add("@InterviewDate", objP.InterviewDate.ToString("yyyy-MM-dd"));
			Param.Add("@Time", objP.Time.ToString("HH:mm:ss"));
			Param.Add("@InterviewMode", objP.InterviewMode.ToString());
			Param.Add("@InterviewRound", objP.InterviewRound.ToString());
			await DBHelper.ExecuteStoreProcedure("GSTPlacement", Param);
		}
		public async Task<DataSet> DetailsScheduledInterviewSN(int id)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "DetailsScheduledInterviewSN");
			Param.Add("@PlacementId", id.ToString());
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		public async Task<DataSet> ListInterviewStatusSN()
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ListInterviewStatusSN");
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		public async Task AddInterviewPerformanceSN(Placement objP)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "AddInterviewPerformanceSN");
			Param.Add("@PlacementId", objP.PlacementId.ToString());
			Param.Add("@FeedbackStatus", objP.StatusId.ToString());
			Param.Add("@Description", objP.Description.ToString());
			await DBHelper.ExecuteStoreProcedure("GSTPlacement", Param);
		}
		public async Task ScheduleInterviewSN(Placement objP)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ScheduleInterviewSN");
			Param.Add("@OpeningCode", objP.OpeningCode.ToString());
			Param.Add("@StudCode", objP.StudentCode.ToString());
			Param.Add("@InterviewDate", objP.InterviewDate.ToString("yyyy-MM-dd"));
			Param.Add("@Time", objP.Time.ToString("HH:mm:ss"));
			Param.Add("@InterviewRound", objP.InterviewRound.ToString());
			Param.Add("@InterviewMode", objP.InterviewMode.ToString());
			await DBHelper.ExecuteStoreProcedure("GSTPlacement", Param);
		}
		public async Task AcceptORRejectOfferSN(Placement objP)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "AcceptORRejectOfferSN");
			Param.Add("@PlacementId", objP.PlacementId.ToString());
			Param.Add("@CTC", objP.CTC_LPA.ToString() + " LPA");
			Param.Add("@OfferAddedDate", DateTime.Today.ToString("yyyy-MM-dd"));
			Param.Add("@FeedbackStatus", objP.FeedbackStatus.ToString());
			Param.Add("@OfferLetter", objP.FilePth.ToString());
			Param.Add("@Description", objP.Description.ToString());
			await DBHelper.ExecuteStoreProcedure("GSTPlacement", Param);
		}
		public async Task JoinCompanySN(Placement objP)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "JoinCompanySN");
			Param.Add("@PlacementId", objP.PlacementId.ToString());
			Param.Add("@CTC", objP.CTC_LPA.ToString() + " LPA");
			Param.Add("@FeedbackStatus", objP.StatusId.ToString());
			Param.Add("@OfferLetter", objP.FilePth.ToString());
			Param.Add("@Description", objP.Description.ToString());
			await DBHelper.ExecuteStoreProcedure("GSTPlacement", Param);
		}
		public async Task<DataSet> DetailSingleInterviewRoundSN(Placement objP)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "DetailSingleInterviewRoundSN");
			Param.Add("@StudCode", objP.StudentCode.ToString());
			Param.Add("@OpeningCode", objP.OpeningCode.ToString());
			Param.Add("@InterviewRound", objP.InterviewRound.ToString());
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		public async Task<DataSet> DetailsPlacedStudentSN(Placement objp)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@Flag", "DetailsPlacedStudentSN");
			Param.Add("@StudCode", objp.StudentCode);
			Param.Add("@BranchCode", objp.BranchCode);
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		public async Task<DataSet> DetailsOfferSN(int id)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "DetailsOfferSN");
			Param.Add("@PlacementId", id.ToString());
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		//--------------------------------------Punam Company------------------------------------//
		/// <summary>
		/// THIS METHOD IS USE TO COMPANY REGISTERTION.
		/// </summary>
		/// <param name="objplacement"></param>
		/// THIS OBJECT  TO PASS VALUES AND SAVE COMPANY DETAILS IN GSTTBLCOMPANY.
		public async Task RegisterCompany(Placement objplacement)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "RgisterCompany");
			Param.Add("@CompanyName", objplacement.CompanyName);
			Param.Add("@CompanyRegiDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			Param.Add("@InduastryId", Convert.ToInt32(objplacement.InduastryId).ToString());
			Param.Add("@CompanyEmail", objplacement.CompanyEmail);
			Param.Add("@Address", objplacement.Address);
			Param.Add("@CityId", objplacement.City);
			Param.Add("@HR1Name", objplacement.HR1Name);
			Param.Add("@hR1Mail", objplacement.HR1Mail);
			Param.Add("@HR1Contact", objplacement.HR1Contact);
			Param.Add("@HR2Name", objplacement.HR2Name);
			Param.Add("@hR2Mail", objplacement.HR2Mail);
			Param.Add("@HR2Contact", objplacement.HR2Contact);
			Param.Add("@HR3Name", objplacement.HR3Name);
			Param.Add("@hR3Mail", objplacement.HR3Mail);
			Param.Add("@HR3Contact", objplacement.HR3Contact);
			Param.Add("@CompanyDescription", objplacement.CompanyDescription);
			Param.Add("@BatchCode", objplacement.BranchCode);
			Param.Add("@StatusId", 21.ToString());
			Param.Add("@IsDeleted", 0.ToString());
			await DBHelper.ExecuteStoreProcedure("GSTPlacement", Param);
		}
		/// <summary>
		/// THIS METHOD IS USE TO GET INDUASTRY LIST.
		/// </summary>
		/// <returns> INDUASTRY LIST.</returns>
		public async Task<DataSet> GetInduastryPB()
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@Flag", "GetIndustryPB");
			dynamic ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// THIS METHOD IS USE TO GET COUNTRY.
		/// </summary>
		/// <returns> GET COUNTRY.</returns>
		public async Task<DataSet> GetCountry()
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@Flag", "GetCountry");
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// THIS METHOD IS USE TO GET STATE.
		/// </summary>
		/// <param name="CountryId"></param>
		/// THIS OBJECT IS USED TO GET STATE .
		/// <returns> GET STATE.</returns>
		public async Task<dynamic> GetState(int CountryId)
		{
			dynamic dynamicDS = null;
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@Flag", "GetState");
			Param.Add("@CountryId", CountryId.ToString());
			dynamicDS = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return dynamicDS;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="StateId"></param>
		/// <returns></returns>
		public async Task<DataSet> GetCity(int StateId)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@Flag", "GetCity");
			Param.Add("@StateId", StateId.ToString());
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// THIS METHOD IS USE TO GET COMPANYNAME FOR VALIDATION.
		/// </summary>
		/// <param name="Company"></param>
		/// THIS FUNCTION IS USE TO GET COMPANY NAME.
		/// <returns> GET COMPANY NAME .</returns>
		public async Task<DataSet> GetCompanyName(string Company)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@Flag", "GetCompanyName");
			Param.Add("@CompanyName", Company);
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// THIS METHOD IS USE TO GET COMPANY DETAILS.
		/// </summary>
		/// <returns> GET ALL DETAILS.</returns>
		public async Task<DataSet> GetCompanyDetailsPB(Placement objplacement)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@Flag", "GetAllDataOfCompanyPB");
			Param.Add("@BranchCode", objplacement.BranchCode);
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// THIS METHOD IS USE TO GET SPECIFY COMPANY .
		/// </summary>
		/// <param name="CompanyId"></param>
		/// THIS OBJECT IS USE TO GET DATA.
		/// <returns> GET COMPANY INFORMATION.</returns>
		public async Task<DataSet> GetCompany(string CompanyCode)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@Flag", "EditCompany");
			Param.Add("@CompanyCode", CompanyCode);
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// THID METHOD IS USE TO UPDATE COMPANY DETAILS.
		/// </summary>
		/// <param name="objplacement"></param>
		/// THIS OBJECT IS USED FOR UPDATE DETAILS .
		/// <returns> UPDATE COMANY DETAILS IN GSTTBLCOMPANY.</returns>
		public async Task UpdateCompanyDetailsAsyncPB(Placement objplacement)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "UpdateCompany");
			Param.Add("@CompanyName", objplacement.CompanyName);
			Param.Add("@InduastryId", Convert.ToInt32(objplacement.InduastryId).ToString());
			Param.Add("@CompanyEmail", objplacement.CompanyEmail);
			Param.Add("@Address", objplacement.Address);
			Param.Add("@CityId", objplacement.City);
			Param.Add("@HR1Name", objplacement.HR1Name);
			Param.Add("@hR1Mail", objplacement.HR1Mail);
			Param.Add("@HR1Contact", objplacement.HR1Contact);
			Param.Add("@HR2Name", objplacement.HR2Name);
			Param.Add("@hR2Mail", objplacement.HR2Mail);
			Param.Add("@HR2Contact", objplacement.HR2Contact);
			Param.Add("@HR3Name", objplacement.HR3Name);
			Param.Add("@hR3Mail", objplacement.HR3Mail);
			Param.Add("@HR3Contact", objplacement.HR3Contact);
			Param.Add("@CompanyDescription", objplacement.CompanyDescription);
			Param.Add("@CompanyCode", objplacement.CompanyCode.ToString());
			await DBHelper.ExecuteStoreProcedure("GSTPlacement", Param);
		}
		/// <summary>
		/// THIS METHOD IS USE TO GET COMPANY DETAILS.
		/// </summary>
		/// <param name="CompanyId"></param>
		/// THIS OBHECT IS USE TO GET COMPANY INFORMATION.
		/// <returns> GET COMPANY DETAILS. </returns>
		public async Task<dynamic> GetAboutCompanyDetails(string companycode)
		{
			dynamic State = null;
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@Flag", "DetailsCompany");
			Param.Add("@CompanyCode", companycode);
			State = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return State;
		}
		/// <summary>
		/// THIS METHOD IS USE TO SAVE INDUASTRY NAME.
		/// </summary>
		/// <param name="InduastryName"></param>
		/// THIS OBJECT IS USE TO PASS VALLUE.
		public async Task SaveIndustry(string InduastryName)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@Flag", "SaveIndustry");
			Param.Add("@InduastryName", InduastryName);
			await DBHelper.ExecuteStoreProcedure("GSTPlacement", Param);
		}
		/// <summary>
		/// THIS METHOD IS USE TO GET FILTER DATA ON DATE .
		/// </summary>
		/// <param name="objplace"></param>
		/// THIS OBJECT IS USE TO GET SELECTED DATE.
		/// <returns> FILTER DATA.</returns>
		public async Task<DataSet> DateFilterPB(DateTime startDate, DateTime endDate, object BranchCode)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@Flag", "DateFilter");
			Param.Add("@FromDate", startDate.ToString());
			Param.Add("@ToDate", endDate.ToString());
			Param.Add("@BranchCode", BranchCode.ToString());
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// THIS METHOD IS USE TO GET INDUSTRY NAME FOR VALIDATION.
		/// </summary>
		/// <returns>GET SANE INDUSTRY NAME.</returns>
		public async Task<DataSet> validationIndustry(string Industryname)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@Flag", "ValidationIndustry");
			Param.Add("@InduastryName", Industryname);
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// THIS METHOD IS USE TO GET COMPANY NAME FOR FILTER BY INDUASTRY ID.
		/// </summary>
		/// <param name="objplacement"></param>
		/// THIS OBJECT IS USE TO GET INDUASTRY ID.
		/// <returns> GET COMANAY NAME BR INDUASTRY ID.</returns>
		public async Task<DataSet> GetCompanyNameOnIndastryPB(int Induastry, Placement objplacement)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@Flag", "GetCompany");
			Param.Add("@InduastryId", Induastry.ToString());
			Param.Add("@BranchCode", objplacement.BranchCode);
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// THIS METHOD IS USE TO GET FILTER DATA BY INDUASTRY .
		/// </summary>
		/// <returns> GET FILYER DATA IN GSTTBLCOMPANY TABLE.</returns>
		public async Task<DataSet> FilterDtaOnInduastryPB(int Induastry, Placement objplacement)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@Flag", "GetfilterDataOnInduastry");
			Param.Add("@InduastryId", Induastry.ToString());
			Param.Add("@BranchCode", objplacement.BranchCode);
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// THIS METHOS IS USE TO GET FILTER DATA ON COAMPANY CODE .
		/// </summary>
		/// <returns> FILTER DATA ON COAMAPANY CODE.</returns>
		public async Task<DataSet> GetFilterDataCompanyId(string CompanyCode, Placement objplacement)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@Flag", "GetFilterDataCompany");
			Param.Add("@CompanyCode", CompanyCode);
			Param.Add("@BranchCode", objplacement.BranchCode);
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		//------------------------------------------------Shubhangi Mock-----------------------------------------------//
		public async Task AddMock(Placement Obj)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ScheduleMockSH");
			Param.Add("@BatchCode", Obj.BatchCode);
			Param.Add("@SkillId", Obj.selectedTechnologyId.ToString());
			Param.Add("@ExperienceId", Obj.ExperienceId.ToString());
			Param.Add("@QualificationId", Obj.QualificationId.ToString());
			Param.Add("@StudCode", Obj.SelectedStudentCodes);
			Param.Add("@MockName", Obj.MockId.ToString());
			Param.Add("@MockDate", Obj.MockDate.ToString("yyyy-MM-dd HH:mm:ss"));
			Param.Add("@LabCode", Obj.LabCode);
			Param.Add("@StartTime", Obj.StartTime.ToString("HH:mm:ss"));
			Param.Add("@Duration", Obj.SelectedDuration.ToString("t"));
			Param.Add("@InterviewerStffcode", Obj.StaffCode);
			Param.Add("@StatusId", "35");
			Param.Add("@AssignDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			await DBHelper.ExecuteStoreProcedure("GSTPlacement", Param);
		}
		public async Task AddIntMock(Placement Obj)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ScheduleIntMockSH");
			Param.Add("@BatchCode", Obj.BatchCode);
			Param.Add("@CndCodes", Obj.SelectedStudentCodes);
			Param.Add("@MockNameId", Obj.MockId.ToString());
			Param.Add("@MockDate", Obj.MockDate.ToString("yyyy-MM-dd HH:mm:ss"));
			Param.Add("@LabCode", Obj.LabCode);
			Param.Add("@StartTime", Obj.StartTime.ToString("yyyy-MM-dd HH:mm:ss"));
			Param.Add("@Duration", Obj.SelectedDuration.ToString("t"));
			Param.Add("@InterviewerStffcode", Obj.StaffCode);
			Param.Add("@StatusId", "35");
			Param.Add("@AssignDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			Param.Add("@LabStartDate", Obj.MockDate.ToString("yyyy-MM-dd"));
			Param.Add("@LabEndDate", Obj.MockDate.ToString("yyyy-MM-dd"));
			Param.Add("@StartT", Obj.StartTime.ToString("HH:mm:ss.fffffff"));
			Param.Add("@EndT", Obj.EndTime.ToString("HH:mm:ss.fffffff"));
			await DBHelper.ExecuteStoreProcedure("GSTPlacement", Param);
		}
		/// <summary>
		/// List of internal student mock.
		/// </summary>
		/// <param name="StatusId">Get the internal student list.</param>
		/// <returns>List of internal student mock.</returns>
		public async Task<DataSet> InternalMockSHAsync(int StatusId, string branchcode)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "InternalMockSH");
			Param.Add("@StatusId", StatusId.ToString());
			Param.Add("@BranchCode", branchcode);
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// List of external student mock.
		/// </summary>
		/// <param name="StatusId">Get the external student list.</param>
		/// <returns>List of external student mock.</returns>
		public async Task<DataSet> ExternalMockSHAsync(int StatusId, string branchcode)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ExternalMockSH");
			Param.Add("@StatusId", StatusId.ToString());
			Param.Add("@BranchCode", branchcode);
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// Get course list.
		/// </summary>
		/// <returns>List of course.</returns>
		public async Task<DataSet> CourseListSHAsync(string branchcode)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "CourseListSH");
			Param.Add("@BranchCode", branchcode);
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// Get skill list.
		/// </summary>
		/// <returns>List of skill.</returns>
		public async Task<DataSet> SkillListSHAsync()
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "SkillListSH");
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// Get experience year list.
		/// </summary>
		/// <returns>List of experience year.</returns>
		public async Task<DataSet> ExperianceYearListSH()
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ExperianceYearListSH");
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// Get mock name list.
		/// </summary>
		/// <returns>List of mock name.</returns>
		public async Task<DataSet> MockNameListSH()
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "MockNameListSH");
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// Get qualification list.
		/// </summary>
		/// <returns>List of qualification.</returns>
		public async Task<DataSet> QualificationListSHAsync()
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "QualificationListSH");
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// Get batch list.
		/// </summary>
		/// <param name="coursecode"> Get batchlist based on coursecode.</param>
		/// <returns>List of batches.</returns>
		public async Task<DataSet> BatchListSHAsync(string coursecode, string branchcode)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "BatchListSH");
			Param.Add("@Coursecode", coursecode);
			Param.Add("@BranchCode", branchcode);
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// Get student list.
		/// </summary>
		/// <param name="batchcode">Get internal student list based on batchcode.</param>
		/// <returns> List of internal students for mock.</returns>
		public async Task<DataSet> IntStudentListMockSHAsync(string batchcode, int MockId, string branchcode)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "IntStudentListMockSH");
			Param.Add("@BatchCode", batchcode);
			Param.Add("@MockName", MockId.ToString());
			Param.Add("@BranchCode", branchcode);
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// Get student list.
		/// </summary>
		/// <param name="batchcode">Get internal student list based on batchcode.</param>
		/// <returns> List of internal students for mock.</returns>
		public async Task<DataSet> ExtStudentListMockSHAsync(int MockId, string Branchcode)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ExtStudentListMockSH");
			Param.Add("@MockName", MockId.ToString());
			Param.Add("@BranchCode", Branchcode);
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}

		/// <summary>
		/// Get staff list.
		/// </summary>
		/// <returns>List of the staff </returns>
		public async Task<DataSet> ExtStudentDetailsforMockSH(string StudentCode)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ExtStudentDetailsforMockSH");
			Param.Add("@StudCode", StudentCode);
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		public async Task<DataSet> StaffListSHAsync(string BranchCode)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "StaffListSH");
			Param.Add("@BranchCode", BranchCode);
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		public async Task<DataSet> LabListSHAsync(string BranchCode)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "LabListSH");
			Param.Add("@BranchCode", BranchCode);
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// Mock details for edit mock.
		/// </summary>
		/// <param name="MockId">Fetch the details of scheduled mock by MockId.</param>
		/// <returns>Details of mock for edit.</returns>
		public async Task<DataSet> ScheduledExtMockDetailsSHAsync(int MockId, string branchcode)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ScheduledExtMockDetailsSH");
			Param.Add("@MockId", MockId.ToString());
			Param.Add("@BranchCode", branchcode);
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		public async Task<DataSet> ScheduledIntMockDetailsSHAsync(int MockId, string branchcode)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ScheduledIntMockDetailsSH");
			Param.Add("@MockId", MockId.ToString());
			Param.Add("@BranchCode", branchcode);
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// Edit scheduled mock.
		/// </summary>
		/// <param name="Obj">Object contain updating mock detalis.</param>
		public async Task UpdateScheduledMockSHAsync(Placement Obj)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "EditScheduledMockSH");
			Param.Add("@MockId", Obj.MockId.ToString());
			Param.Add("@MockDate", Obj.MockDate.ToString("yyyy-MM-dd HH:mm:ss"));
			Param.Add("@LabCode", Obj.LabCode);
			Param.Add("@StartTime", Obj.StartTime.ToString("yyyy-MM-dd HH:mm:ss"));
			Param.Add("@InterviewerStffcode", Obj.StaffCode);
			Param.Add("@StatusId", Obj.StatusId.ToString());
			await DBHelper.ExecuteStoreProcedure("GSTPlacement", Param);

		}
		/// <summary>
		/// Get the list of student for add mock performance.
		/// </summary>
		/// <returns>It returns list of student.</returns> 
		public async Task<DataSet> StudentListSHAsync(int mockid)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "StudentListForPerformanceSH");
			Param.Add("@MockId", mockid.ToString());
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// Get the list of status foe change the status.
		/// </summary>
		/// <returns>It returns list of status.</returns> 
		public async Task<DataSet> StatusListSHAsync()
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "GetStatusSH");
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// It show the mock performance of student.
		/// </summary>
		/// <param name="mockid"> this object returns performance data.</param>
		/// <returns> It returnce performance of mock.</returns>
		public async Task<DataSet> ViewMockPerformanceSH(int mockid)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ViewExtMockPerformanceSH");
			Param.Add("@MockId", mockid.ToString());
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// It show the mock performance of internal student.
		/// </summary>
		/// <param name="mockid"> this object returns performance data.</param>
		/// <returns> It returnce performance of mock.</returns>
		public async Task<DataSet> ViewIntMockPerformanceSH(int mockid)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "ViewIntMockPerformanceSH");
			Param.Add("@MockId", mockid.ToString());
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// Add mock performance of student.
		/// </summary>
		/// <param name="Obj">Object contains mock details.</param>
		public async Task AddMockPerformanceSH(Placement Obj)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "AddMockPerformanceSH");
			Param.Add("@MockId", Obj.MockId.ToString());
			Param.Add("@StudCode", Obj.StudentCode);
			Param.Add("@Attendance", Obj.AttendanceId.ToString());
			Param.Add("@PerformanceStatus", Obj.PerformanceStatus);
			Param.Add("@performanceAddDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			Param.Add("@StaffCode", Obj.StaffCode);
			await DBHelper.ExecuteStoreProcedure("GSTPlacement", Param);
		}
		//--------------------------------------Testimonial Prem---------------------------//
		/// <summary>
		/// Retrieves testimonial data asynchronously.
		/// </summary>
		/// <param name="objP">The Placement object containing parameters, if any, for the retrieval.</param>
		/// <returns>Returns a DataSet containing testimonial data.</returns>
		public async Task<DataSet> GetTestimonialPDAsync(Placement objP)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "GetTestimonialPD");
			Param.Add("@BranchCode", objP.BranchCode.ToString());
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// Retrieves course data asynchronously.
		/// </summary>
		/// <returns>Returns a DataSet containing course data.</returns>
		public async Task<DataSet> GetCoursePDAsync(Placement objP)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@Flag", "GetCoursePD");
			Param.Add("@BranchCode", objP.BranchCode.ToString());
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		/// <summary>
		/// Fetches batch names asynchronously based on the provided course code.
		/// </summary>
		/// <param name="Coursecode">The course code for which batch names are to be fetched.</param>
		/// <returns>Returns a DataSet containing batch names.</returns>
		public async Task<DataSet> FetchBatchName(Placement objP)
		{
			try
			{
				Dictionary<string, string> Param = new Dictionary<string, string>();
				Param.Add("@flag", "FetchBatchName");
				Param.Add("@Typeid", 5.ToString());
				Param.Add("@CourseCode", objP.CourseCode.ToString());
				Param.Add("@BranchCode", objP.BranchCode.ToString());
				DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
				return ds;
			}
			catch (Exception ex)
			{
				throw new Exception("An error occurred while fetching course names. Details: " + ex.Message);
			}
		}
		/// <summary>
		/// Retrieves student names asynchronously based on the provided course code.
		/// </summary>
		/// <param name="Coursecode">The course code used to retrieve student names.</param>
		/// <returns>Returns a DataSet containing student names.</returns>
		public async Task<DataSet> GetStudentNamePDAsync(Placement objP)
		{
			try
			{
				Dictionary<string, string> Param = new Dictionary<string, string>();
				Param.Add("@flag", "GetStudentNamePD");
				Param.Add("@CourseCode", objP.CourseCode);
				Param.Add("@BranchCode", objP.BranchCode.ToString());
				DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
				return ds;
			}
			catch (Exception ex)
			{
				throw new Exception("An error occurred while fetching course names. Details: " + ex.Message);
			}
		}
		/// <summary>
		/// Retrieves student data asynchronously based on the provided candidate code.
		/// </summary>
		/// <param name="CandidateCode">The candidate code used to retrieve student data.</param>
		/// <returns>Returns a SqlDataReader containing student data.</returns>
		public async Task<SqlDataReader> GetStudentDataPDAsync(Placement objP)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "GetStudentdata");
			Param.Add("@BranchCode", objP.BranchCode.ToString());
			Param.Add("@Candidatecode", objP.CandidateCode);
			SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTPlacement", Param);
			return dr;
		}
		/// <summary>
		/// Saves a testimonial asynchronously.
		/// </summary>
		/// <param name="ObjP">The Placement object containing testimonial details to be saved.</param>
		/// <returns>Task representing the asynchronous operation.</returns>
		public async Task SaveTestimonialAsyncPD(Placement ObjP)
		{
			try
			{
				Dictionary<string, string> Param = new Dictionary<string, string>();
				Param.Add("@flag", "SaveTestimonial");
				Param.Add("@Candidatecode", ObjP.StudentName);
				Param.Add("@TodayDate", DateTime.Today.ToString("yyyy-MM-dd"));
				Param.Add("@Comments", ObjP.Comment);
				Param.Add("@UploadVideo", ObjP.VideoPath);
				Param.Add("@UploadPDF", ObjP.PdfPath);
				Param.Add("@UploadAudio", ObjP.AudioPath);
				await DBHelper.ExecuteStoreProcedure("GSTPlacement", Param);
			}
			catch (Exception ex)
			{
				throw new Exception("An error occurred while registering the assigned project. Details: " + ex.Message);
			}
		}
		/// <summary>
		/// Retrieves testimonial data asynchronously for editing based on the provided Placement object.
		/// </summary>
		/// <param name="objP">The Placement object containing parameters for retrieving testimonial data.</param>
		/// <returns>Returns a SqlDataReader containing testimonial data for editing.</returns>
		public async Task<SqlDataReader> EditTestimonialPDAsync(Placement objP)
		{
			try
			{
				Dictionary<string, string> Param = new Dictionary<string, string>();
				Param.Add("@flag", "GetDataOnEditTestimonial");
				Param.Add("@TestimonialId", objP.TestimonialId.ToString());
				Param.Add("@CandidateCode", objP.CandidateCode);
				SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTPlacement", Param);
				return dr;
			}
			catch (Exception ex)
			{
				throw new Exception("An error occurred while fetching assigned project details. Details: " + ex.Message);
			}
		}
		/// <summary>
		/// Retrieves testimonial data asynchronously based on the provided Placement object.
		/// </summary>
		/// <param name="objP">The Placement object containing parameters for retrieving testimonial data.</param>
		/// <returns>Returns a SqlDataReader containing testimonial data.</returns>
		public async Task<SqlDataReader> ViewTestimonialPDAsync(Placement objP)
		{
			try
			{
				Dictionary<string, string> Param = new Dictionary<string, string>();
				Param.Add("@flag", "GetDataOnEditTestimonial");
				Param.Add("@TestimonialId", objP.TestimonialId.ToString());
				Param.Add("@CandidateCode", objP.CandidateCode);
				SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTPlacement", Param);
				return dr;
			}
			catch (Exception ex)
			{
				throw new Exception("An error occurred while fetching assigned project details. Details: " + ex.Message);
			}
		}
		/// <summary>
		/// Updates testimonial data asynchronously.
		/// </summary>
		/// <param name="ObjP">The Placement object containing updated testimonial data.</param>
		/// <returns>No return value.</returns>
		public async Task UpdateTestimonialAsyncPD(Placement ObjP)
		{
			try
			{
				Dictionary<string, string> Param = new Dictionary<string, string>();
				Param.Add("@flag", "UpdateTestimonial");
				Param.Add("@TestimonialId", ObjP.TestimonialId.ToString());
				Param.Add("@Comments", ObjP.Comment);
				Param.Add("@UploadVideo", ObjP.UploadVideo);
				Param.Add("@UploadPDF", ObjP.UploadPDF);
				Param.Add("@UploadAudio", ObjP.UploadAudio);
				await DBHelper.ExecuteStoreProcedure("GSTPlacement", Param);
			}
			catch (Exception ex)
			{
				throw new Exception("An error occurred while updating the assigned project. Details: " + ex.Message);
			}
		}
		public async Task<DataSet> GetFeesCompletionAsyncPD(Placement objP)
		{
			Dictionary<string, string> Param = new Dictionary<string, string>();
			Param.Add("@flag", "GetFeesData");
			Param.Add("@BranchCode", objP.BranchCode.ToString());
			DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			return ds;
		}
		//----------------------------------------------------Pratiksha (StudentList)-----------------------------------------------

		/// <summary>
		/// Retrieves a list of internally released students asynchronously.
		/// </summary>
		/// <param name="objpla">Placement object containing relevant information.</param>
		/// <returns>A DataSet containing the list of internally released students.</returns>
		public async Task<DataSet> ListInternalRelesedStudentsPRAsync(Placement objpla)
		{
			try
			{
				Dictionary<string, string> Param = new Dictionary<string, string>();
				Param.Add("@Flag", "InternalRelesedStudentsList");
				Param.Add("@BranchCode", objpla.BranchCode);
				return await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred in InternalRelesedStudentsListPRAsync: {ex.Message}");
				return null;
			}
		}

		/// <summary>
		/// Retrieves a list of internally on-hold students asynchronously.
		/// </summary>
		/// <param name="objpla">Placement object containing relevant information.</param>
		/// <returns>A DataSet containing the list of internally on-hold students.</returns>
		public async Task<DataSet> ListInternalOnHoldStudentsPRAsync(Placement objpla)
		{
			try
			{
				Dictionary<string, string> Param = new Dictionary<string, string>();
				Param.Add("@Flag", "InternalOnHoldStudentsList");
				Param.Add("@BranchCode", objpla.BranchCode);
				return await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred in InternalOnHoldStudentsListPRAsync: {ex.Message}");
				return null;
			}
		}

		/// <summary>
		/// Retrieves a list of externally released students asynchronously.
		/// </summary>
		/// <param name="objpla">Placement object containing relevant information.</param>
		/// <returns>A DataSet containing the list of externally released students.</returns>
		public async Task<DataSet> ListExternalRelesedStudentsPRAsync(Placement objpla)
		{
			try
			{
				Dictionary<string, string> Param = new Dictionary<string, string>();
				Param.Add("@Flag", "ExternalRelesedStudentsList");
				Param.Add("@BranchCode", objpla.BranchCode);
				return await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred in ExternalRelesedStudentsListPRAsync: {ex.Message}");
				return null;
			}
		}

		/// <summary>
		/// Retrieves a list of externally on-hold students asynchronously.
		/// </summary>
		/// <param name="objpla">Placement object containing relevant information.</param>
		/// <returns>A DataSet containing the list of externally on-hold students.</returns>
		public async Task<DataSet> ListExternalOnHoldStudentsPRAsync(Placement objpla)
		{
			try
			{
				Dictionary<string, string> Param = new Dictionary<string, string>();
				Param.Add("@Flag", "ExternalOnHoldStudentsList");
				Param.Add("@BranchCode", objpla.BranchCode);
				return await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred in ExternalOnHoldStudentsListPRAsync: {ex.Message}");
				return null;
			}
		}

		/// <summary>
		/// Retrieves data for editing the list of internally released students asynchronously.
		/// </summary>
		/// <param name="CandidateCode">Candidate code for the student to be edited.</param>
		/// <returns>A SqlDataReader containing the data for editing the internally released student.</returns>
		public async Task<SqlDataReader> InternalRelesedStudentsListEditPRAsync(string CandidateCode)
		{
			try
			{
				Dictionary<string, string> Param = new Dictionary<string, string>();
				Param.Add("@Flag", "InternalRelesedStudentsListEdit");
				Param.Add("@CandidateCode", CandidateCode);
				return await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTPlacement", Param);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred in InternalRelesedStudentsListEditPRAsync: {ex.Message}");
				return null;
			}
		}

		/// <summary>
		/// Fetches courses asynchronously.
		/// </summary>
		/// <param name="objpla">Placement object containing relevant information.</param>
		/// <returns>A DataSet containing the list of courses.</returns>
		public async Task<DataSet> FetchCoursePRAsync(Placement objpla)
		{
			try
			{
				Dictionary<string, string> Param = new Dictionary<string, string>();
				Param.Add("@Flag", "FetchCourse");
				Param.Add("@BranchCode", objpla.BranchCode);
				return await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred in FetchCoursePRAsync: {ex.Message}");
				return null;
			}
		}

		/// <summary>
		/// Fetches batches asynchronously.
		/// </summary>
		/// <param name="objpla">Placement object containing relevant information.</param>
		/// <returns>A DataSet containing the list of batches.</returns>
		public async Task<DataSet> FetchBatchPRAsync(Placement objpla)
		{
			try
			{
				Dictionary<string, string> Param = new Dictionary<string, string>();
				Param.Add("@Flag", "FetchBatch");
				Param.Add("@CourseCode", objpla.CourseCode);
				Param.Add("@BranchCode", objpla.BranchCode);
				return await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred in FetchBatchPRAsync: {ex.Message}");
				return null;
			}
		}

		/// <summary>
		/// Fetches students asynchronously.
		/// </summary>
		/// <param name="obj">Placement object containing relevant information.</param>
		/// <returns>A DataSet containing the list of students.</returns>
		public async Task<DataSet> FetchStudentPRAsync(Placement obj)
		{
			try
			{
				Dictionary<string, string> Param = new Dictionary<string, string>();
				Param.Add("@Flag", "FetchStudent");
				Param.Add("@BranchCode", obj.BranchCode);
				Param.Add("@BatchCode", obj.BatchCode);
				return await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred in FetchStudentPRAsync: {ex.Message}");
				return null;
			}
		}

		/// <summary>
		/// Shows the details of a student for registration asynchronously.
		/// </summary>
		/// <param name="obj">Placement object containing relevant information.</param>
		/// <returns>A SqlDataReader containing the details of the student for registration.</returns>
		public async Task<SqlDataReader> RegistrationDetailsPRAsync(Placement obj)
		{
			try
			{
				Dictionary<string, string> Param = new Dictionary<string, string>();
				Param.Add("@Flag", "RegistrationDetails");
				Param.Add("@CandidateCode", obj.CandidateCode);
				Param.Add("@BranchCode", obj.BranchCode);
				return await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTPlacement", Param);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred in RegistrationDetailsPRAsync: {ex.Message}");
				return null;
			}
		}

		/// <summary>
		/// Fetches skills asynchronously.
		/// </summary>
		/// <returns>A DataSet containing the list of skills.</returns>
		public async Task<DataSet> FetchSkillPRAsync()
		{
			try
			{
				Dictionary<string, string> Param = new Dictionary<string, string>();
				Param.Add("@Flag", "FetchSkill");
				return await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred in FetchSkillPRAsync: {ex.Message}");
				return null;
			}
		}

		/// <summary>
		/// Fetches projects asynchronously.
		/// </summary>
		/// <returns>A DataSet containing the list of projects.</returns>
		public async Task<DataSet> FetchProjectPRAsync()
		{
			try
			{
				Dictionary<string, string> Param = new Dictionary<string, string>();
				Param.Add("@Flag", "FetchProject");
				return await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred in FetchProjectPRAsync: {ex.Message}");
				return null;
			}
		}

		/// <summary>
		/// Updates the resume file path asynchronously.
		/// </summary>
		/// <param name="obj">Placement object containing relevant information.</param>
		public async Task UpdateResumePRAsync(Placement obj)
		{
			try
			{
				Dictionary<string, string> Param = new Dictionary<string, string>();
				Param.Add("@Flag", "UpdateResume");
				Param.Add("@CandidateCode", obj.CandidateCode);
				Param.Add("@ResumeFilePath", obj.ResumeFilePath?.ToString() ?? string.Empty);
				await DBHelper.ExecuteStoreProcedure("GSTPlacement", Param);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred in UpdateResumePRAsync: {ex.Message}");
			}
		}

		/// <summary>
		/// Fetches companies asynchronously.
		/// </summary>
		/// <returns>A DataSet containing the list of companies.</returns>
		public async Task<DataSet> FetchCompanyPRAsync()
		{
			try
			{
				// Create parameters for the stored procedure
				Dictionary<string, string> Param = new Dictionary<string, string>();
				Param.Add("@Flag", "FetchCompany");

				// Execute the stored procedure asynchronously and return the DataSet
				return await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			}
			catch (Exception ex)
			{
				// Log or handle the exception appropriately
				Console.WriteLine($"An error occurred in FetchCompanyPRAsync: {ex.Message}");

				// Return null or handle the error case based on your application's requirements
				return null;
			}
		}

		/// <summary>
		/// Fetches industries asynchronously.
		/// </summary>
		/// <returns>A DataSet containing the list of industries.</returns>
		public async Task<DataSet> FetchInduastryPRAsync()
		{
			try
			{
				Dictionary<string, string> Param = new Dictionary<string, string>();
				Param.Add("@Flag", "FetchInduastry");
				return await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred in FetchInduastryPRAsync: {ex.Message}");
				return null;
			}
		}

		/// <summary>
		/// Fetches designations asynchronously.
		/// </summary>
		/// <returns>A DataSet containing the list of designations.</returns>
		public async Task<DataSet> FetchDesignationPRAsync()
		{
			try
			{
				Dictionary<string, string> Param = new Dictionary<string, string>();
				Param.Add("@Flag", "FetchDesignation");
				return await DBHelper.ExecuteStoreProcedureReturnDS("GSTPlacement", Param);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred in FetchDesignationPRAsync: {ex.Message}");
				return null;
			}
		}

		/// <summary>
		/// Saves assigned skills asynchronously.
		/// </summary>
		/// <param name="obj">Placement object containing relevant information.</param>
		public async Task SaveAssignSkillPRAsync(Placement obj)
		{
			try
			{
				Dictionary<string, string> Param = new Dictionary<string, string>();
				Param.Add("@Flag", "SaveAssignSkill");
				Param.Add("@CandidateCode", obj.CandidateCode);
				Param.Add("@AllSkillId", obj.Skill?.ToString() ?? string.Empty);
				Param.Add("@PreferedSkillid", obj.Technologies?.ToString() ?? string.Empty);
				await DBHelper.ExecuteStoreProcedure("GSTPlacement", Param);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred in SaveAssignSkillPRAsync: {ex.Message}");
			}
		}

		/// <summary>
		/// Saves assigned projects asynchronously.
		/// </summary>
		/// <param name="obj">Placement object containing relevant information.</param>
		public async Task SaveAssignProjectPRAsync(Placement obj)
		{
			try
			{
				Dictionary<string, string> Param = new Dictionary<string, string>();
				Param.Add("@Flag", "SaveAssignProject");
				Param.Add("@CandidateCode", obj.CandidateCode);
				Param.Add("@ProjectCode", obj.Projects);
				await DBHelper.ExecuteStoreProcedure("GSTPlacement", Param);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred in SaveAssignProjectPRAsync: {ex.Message}");
			}
		}

		/// <summary>
		/// Adds experience asynchronously for a candidate.
		/// </summary>
		/// <param name="obj">Placement object containing experience details.</param>
		public async Task AddExperiencePRAsync(Placement obj)
		{
			try
			{
				Dictionary<string, string> Param = new Dictionary<string, string>();
				Param.Add("@Flag", "AddExperience");
				Param.Add("@CandidateCode", obj.CandidateCode);
				Param.Add("@IndustryId", obj.InduastryId.ToString());
				Param.Add("@DesignationId", obj.DesignationId.ToString());
				Param.Add("@CTC", obj.CTC);
				Param.Add("@Experience", obj.Experience);
				Param.Add("@JobType", obj.JobType);
				Param.Add("@CompanyName", obj.CompanyName);
				await DBHelper.ExecuteStoreProcedure("GSTPlacement", Param);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred in AddExperiencePRAsync: {ex.Message}");
			}
		}

		/// <summary>
		/// Updates the placement status of a student asynchronously.
		/// </summary>
		/// <param name="obj">Placement object containing student and status details.</param>
		public async Task UpdatePlacementStudentStatusPRAsync(Placement obj)
		{
			try
			{
				Dictionary<string, string> Param = new Dictionary<string, string>();
				Param.Add("@Flag", "UpdatePlacementStudentStatus");
				Param.Add("@CandidateCode", obj.CandidateCode);
				Param.Add("@StatusId", obj.StatusId.ToString());
				await DBHelper.ExecuteStoreProcedure("GSTPlacement", Param);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred in UpdatePlacementStudentStatusPRAsync: {ex.Message}");
			}
		}

		//---------------------------------------------------- END Pratiksha (StudentList)-----------------------------------------------

	}
}
