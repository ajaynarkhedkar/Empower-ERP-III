using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using Helper;

namespace GSTEducationERPLibrary.Trainer
{
    public class BALTrainer
    {
        MSSQL DBHelper = new MSSQL();

        //---------- vaibhav pawar Start -------------//
        /// <summary>
        /// this is method is used to showing  list of section  in batch sedule.
        /// </summary>
        /// <returns> show list </returns>
        public async Task<DataSet> ShowSectionToTrainerAsync(Trainer obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ShowSectionTOTrainer");
            Param.Add("@StaffCode", obj.StaffCode);
            Param.Add("@BranchCode", obj.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        ///  this method used to show list of perticular topic of section in batch sedule.
        /// </summary>
        /// <param name="sectionid"></param>
        /// <returns> topic list</returns>
        public async Task<DataSet> ShowTopicToTrainerAsync(int sectionid)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ShowTopicToTrainer");
            Param.Add("@SectionId", sectionid.ToString());
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// this method is used to save new section in batch sedule.
        /// </summary>
        /// <param name="obj"></param>
        public async Task<bool> TrainerAddSectionAsync(Trainer obj)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@Flag", "TrainerAddSection");
                Param.Add("@CourseCode", obj.CourseCode);
                Param.Add("@SectionName", obj.SectionName);
                await DBHelper.ExecuteStoreProcedure("GSTTrainer", Param);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// this method is used to add new section related topic in batch sedule.
        /// </summary>
        /// <param name="obj"></param>
        public async Task TrainerAddTopicAndAssingAsync(Trainer obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "TrainerAddTopicdetails");
            Param.Add("@SectionId", obj.SectionId.ToString());
            Param.Add("@TopicName", obj.TopicName);
            Param.Add("@NoOfSessions", obj.NoOfSessions.ToString());
            Param.Add("@Duration", obj.Duration);
            Param.Add("@description", obj.TopicDescription);
            Param.Add("@StaffCode", obj.StaffCode);
            Param.Add("@TopicAddedDate", obj.TopicAddedDate.ToString("yyyy-MM-dd HH:mm:ss"));
            Param.Add("@StatusId", obj.StatusId.ToString());
            await DBHelper.ExecuteStoreProcedure("GSTTrainer", Param);
        }
        /// <summary>
        /// this method is used to save new topic related assingment in batch sedule .
        /// </summary>
        /// <param name="obj"></param>
        public async Task TrainerSaveAssingmentAsync(Trainer obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "TrainerSaveAssingment");
            Param.Add("@AssignmentFile", obj.AssignmentFile);
            await DBHelper.ExecuteStoreProcedure("GSTTrainer", Param);
        }
        /// <summary>
        /// this method used to show all section in dropdown .
        /// </summary>
        /// <returns></returns>
        public async Task<DataSet> ShowSectionDropdownAsync(Trainer obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ShowSectionDropdown");
            Param.Add("@CourseCode", obj.CourseCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// this method is used to show all status of topic in batch sedule .
        /// </summary>
        /// <returns>show status</returns>
        public async Task<DataSet> TopicShowStatusAsync()
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "TopicShowStatus");
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// this method is used to show all topic related assingment in list in batch sedule .
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>show listview</returns>
        public async Task<DataSet> TrainerViewAssingmentAsync(Trainer obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "TrainerViewAssingment");
            Param.Add("@TopicId", obj.TopicId.ToString());
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// this method is used to delete section in batch sedule.
        /// </summary>
        /// <param name="obj"></param>
        public async Task TrainerDeleteSectionAsync(Trainer obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "TrainerDeleteSection");
            Param.Add("@SectionId", obj.SectionId.ToString());
            await DBHelper.ExecuteStoreProcedure("GSTTrainer", Param);
        }
        /// <summary>
        /// this method is used to delete topic in batch sedule.
        /// </summary>
        /// <param name="obj"></param>
        public async Task TrainerDeleteTopicAsync(Trainer obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "TrainerDeleteTopic");
            Param.Add("@TopicId", obj.TopicId.ToString());
            await DBHelper.ExecuteStoreProcedure("GSTTrainer", Param);
        }
        /// <summary>
        /// this method is used to delete assingment in batch sedule.
        /// </summary>
        /// <param name="obj"></param>
        public async Task TrainerDeleteAssingmentAsync(Trainer obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "TrainerDeleteAssingment");
            Param.Add("@AssignmentId", obj.AssignmentId.ToString());
            await DBHelper.ExecuteStoreProcedure("GSTTrainer", Param);
        }
        /// <summary>
        /// this method is used to delete perticular session in batch sedule .
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<SqlDataReader> TrainerDeleteSectionCheckAsync(Trainer obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "TrainerDeleteSectionCheck");
            Param.Add("@SectionId", obj.SectionId.ToString());
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTTrainer", Param);
            return dr;
        }
        /// <summary>
        /// this method is used to delete perticular topic of session in batch sedule .
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<SqlDataReader> TrainerDeleteTopicCheckAsync(Trainer obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "TrainerDeleteTopicCheck");
            Param.Add("@TopicId", obj.TopicId.ToString());
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTTrainer", Param);
            return dr;
        }
        /// <summary>
        /// this method is used to update perticular topic of session in batch sedule.
        /// </summary>
        /// <param name="obj"></param>
        public async Task TrainerUpdateTopicAsync(Trainer obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "TrainerUpdateTopic");
            Param.Add("@SectionId", obj.SectionId.ToString());
            Param.Add("@TopicName", obj.TopicName);
            Param.Add("@NoOfSessions", obj.NoOfSessions.ToString());
            Param.Add("@Duration", obj.Duration);
            Param.Add("@description", obj.TopicDescription);
            Param.Add("@StatusId", obj.StatusId.ToString());
            Param.Add("@TopicId", obj.TopicId.ToString());
            await DBHelper.ExecuteStoreProcedure("GSTTrainer", Param);
        }
        /// <summary>
        /// this method is used to update assingment of topic in batch sedule .
        /// </summary>
        /// <param name="obj"></param>
        public async Task TrainerUpdateAssingmentAsync(Trainer obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "TrainerUpdateAssingment");
            Param.Add("@AssignmentFile", obj.AssignmentFile);
            Param.Add("@TopicId", obj.TopicId.ToString());
            await DBHelper.ExecuteStoreProcedure("GSTTrainer", Param);
        }
        /// <summary>
        /// this method is used to read topic of session in batch sedule .
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<SqlDataReader> TrainerreadTopicAsync(Trainer obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "TrainerreadTopic");
            Param.Add("@TopicId", obj.TopicId.ToString());
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTTrainer", Param);
            return dr;
        }
        /// <summary>
        /// this  method is used to read section of perticular course.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<SqlDataReader> TrainerreadsectionAsync(Trainer obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "Trainerreadsection");
            Param.Add("@SectionId", obj.SectionId.ToString());
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTTrainer", Param);
            return dr;
        }
        /// <summary>
        ///  method is used to read couse of perticular staff.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<SqlDataReader> TrainerreadTrainerCouse(Trainer obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "TrainerreadTrainerCouse");
            Param.Add("@StaffCode", obj.StaffCode);
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTTrainer", Param);
            return dr;
        }
        /// <summary>
        /// this method is used to update section in batch sedule in trainer module .
        /// </summary>
        /// <param name="obj"></param>
        public async Task TrainerUpdateSectionAsync(Trainer obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "TrainerUpdateSection");
            Param.Add("@SectionName", obj.SectionName);
            Param.Add("@SectionId", obj.SectionId.ToString());
            await DBHelper.ExecuteStoreProcedure("GSTTrainer", Param);
        }
        //----------- TRainer ASSing Sedule //---------------
        /// <summary>
        /// it feaching assing sedule grid view data to trainer in batch sedule .
        /// </summary>
        /// <returns>grid view data </returns>
        public async Task<DataSet> ShowTrainerAssingSeduleAsync(Trainer obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ShowTrainerAssignSchedule");
            Param.Add("@StaffCode", obj.StaffCode);
            Param.Add("@BranchCode", obj.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// this is add new assing sedule  in batch sedule .
        /// </summary>
        /// <param name="obj">save new sedule</param>
        public async Task TrainerAddAssingSeduleAsync(Trainer obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "TrainerAddAssignSchedule");
            Param.Add("@BatchAssingScheduleId", obj.BatchId.ToString());
            Param.Add("@StaffCode", obj.StaffCode);
            Param.Add("@TopicId", obj.TopicId.ToString());
            Param.Add("@StartDate", obj.StartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            Param.Add("@ScheduleAssignedDate", obj.ScheduleAssignedDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            Param.Add("@StatusId", obj.StatusId.ToString());
            await DBHelper.ExecuteStoreProcedure("GSTTrainer", Param);
        }
        /// <summary>
        /// this method is used to Read Trainer Assing Sedule in Batch Sedule Module. 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<SqlDataReader> TrainerReadAssingSeduleAsync(Trainer obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "TrainerReadAssignSchedule");
            Param.Add("@AssignScheduleId", obj.AssignScheduleId.ToString());
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTTrainer", Param);
            return dr;
        }


        /// <summary>
        /// this method is showing assing section batches in assing sedule .
        /// </summary>
        /// <param name="objt"></param>
        /// <returns></returns>
        public async Task<DataSet> ShowASsingSectionBatchAsync(Trainer objt)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "TrainerAssignScheduleBatch");
            Param.Add("@StaffCode", objt.StaffCode);
            Param.Add("@BranchCode", objt.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// this method is used to session relsted topic in dropdown in add assing sedule .
        /// </summary>
        /// <param name="objt"></param>
        /// <returns></returns>
        public async Task<DataSet> ShowTopicDropdownAsync(Trainer objt)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ShowTopicDropdown");
            Param.Add("@SectionId", objt.SectionId.ToString());
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// this method is used to showing perticular topic details like duration and no od session in batch sedule .
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<DataSet> TrainerreadTopicdeatilsAsync(Trainer obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ShowTopicDetailsOnAssignSchedule");
            Param.Add("@TopicId", obj.TopicId.ToString());
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// used for show assighn sedule status 
        /// </summary>
        /// <returns></returns>
        public async Task<DataSet> ShowAssignScheduleStatus()
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ShowAssignScheduleStatus");
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;

        }
        /// <summary>
        /// use for delete assign sedule 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task TrainerDeleteAssignSeduleAsync(Trainer obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "DeleteAssignSchedule");
            Param.Add("@AssignScheduleId", obj.AssignScheduleId.ToString());
            await DBHelper.ExecuteStoreProcedure("GSTTrainer", Param);

        }
        /// <summary>
        /// use for update assing sedule 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task TrainerUpdateAssignSchedule(Trainer obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "TrainerUpdateAssignSchedule");
            Param.Add("@StartDate", obj.StartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            Param.Add("@StatusId", obj.StatusId.ToString());
            Param.Add("@AssignScheduleId", obj.AssignScheduleId.ToString());
            await DBHelper.ExecuteStoreProcedure("GSTTrainer", Param);

        }
        //---------- vaibhav pawar End -------------//


        //-----------pratibha start  project management ----------------------------------------------------------------------------//
        //-----------pratibha start  project management ----------------------------------------------------------------------------//
        /// <summary>
        /// Retrieves a dataset containing the list of assigned projects.
        /// </summary>
        /// <returns>A DataSet containing assigned project information.</returns>
        public async Task<DataSet> ListAllAssignedProjectAsyncPG(Trainer objT, String courseName = null)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@flag", "ShowAssignedProject");
                Param.Add("@BranchCode", objT.BranchCode);
                if (courseName != null)
                {
                    Param.Add("@CourseCode", courseName);
                }

                //Param.Add("@TaskAddedStaff", objT.StaffCode);
                DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving assigned projects. Details: " + ex.Message);
            }
        }
        /// <summary>
        /// Retrieves a dataset containing the list of students assigned to a specific project.
        /// </summary>
        /// <param name="AssignProjectId"></param>
        /// <returns>A DataSet containing Full Name about students assigned to the specified project.</returns>
        public async Task<DataSet> ListAssignProjectStudentAsyncPG(Trainer objT)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@flag", "ViewStudentList");
                Param.Add("@AssignProjectId", objT.AssignProjectId.ToString());
                Param.Add("@BranchCode", objT.BranchCode.ToString());
                //Param.Add("@AssignProjectId", objT.StaffCode.ToString());
                DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the list of assigned students. Details: " + ex.Message);
            }
        }
        /// <summary>
        /// Retrieves a DataSet containing information about all projects.
        /// </summary>
        /// <returns>A DataSet containing details of all projects retrieved from the database.</returns>
        public async Task<DataSet> ListAllProjectsAsyncPG(Trainer objT)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@flag", "ShowAllProjects");
                Param.Add("@BranchCode", objT.BranchCode.ToString());
                DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the list of all projects. Details: " + ex.Message);
            }
        }
        /// <summary>
        /// Retrieves a DataSet containing a list of course names.
        /// </summary>
        /// <returns>A DataSet containing course names retrieved from the database.</returns>
        public async Task<DataSet> FetchCourceName(Trainer objT)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@flag", "FetchCourceName");
                Param.Add("@BranchCode", objT.BranchCode.ToString());
                DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching course names. Details: " + ex.Message);
            }
        }
        /// <summary>
        /// Retrieves a DataSet containing information about assigned trainers with a specific staff position.
        /// </summary>
        /// <returns>A DataSet containing Name of trainers assigned to a specific staff position.</returns>
        public async Task<DataSet> FetchAssignTrainer(Trainer objT)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@flag", "FetchAssignTrainer");
                Param.Add("@BranchCode", objT.BranchCode.ToString());
                Param.Add("@StaffPositionId", 3.ToString());
                DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching course names. Details: " + ex.Message);
            }
        }
        /// <summary>
        /// Retrieves a DataSet containing a list of project names.
        /// </summary>
        /// <returns>A DataSet containing project names retrieved from the database.</returns>
        public async Task<DataSet> FetchProjectName(Trainer objT)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@flag", "FetchProjectName");
                Param.Add("@BranchCode", objT.BranchCode.ToString());
                DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching project names. Details: " + ex.Message);
            }
        }
        /// <summary>
        /// Retrieves a DataSet containing a list of batch names with a specific type.
        /// </summary>
        /// <returns>A DataSet containing batch names retrieved from the database.</returns>
        public async Task<DataSet> FetchBatchName(Trainer objT)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@flag", "FetchBatchName");
                Param.Add("@Typeid", 5.ToString());
                Param.Add("@CourseCode", objT.CourseCode);
                Param.Add("@BranchCode", objT.BranchCode.ToString());
                DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching course names. Details: " + ex.Message);
            }
        }
        /// <summary>
        /// Retrieves a DataSet containing the list of students for a specific batch.
        /// </summary>
        /// <param name="BatchCode">The code of the batch for which student information is retrieved.</param>
        /// <returns>A DataSet containing details of students for the specified batch.</returns>
        public async Task<DataSet> ViewBatchStudentList(string BatchCode)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@flag", "ViewBatchStudentList");
                Param.Add("@BatchCode", BatchCode);
                DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while viewing the batch student list. Details: " + ex.Message);
            }
        }
        /// <summary>
        /// Adds a batch student based on the provided Trainer object.
        /// </summary>
        /// <param name="objT">The Trainer object containing information about the batch student to be added.</param>
        /// <returns>A DataSet containing the result of the operation.</returns>
        public async Task<DataSet> AddBatchStudent(Trainer objT)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@flag", "AddBatchStudent");
                Param.Add("@BatchCode", objT.BatchCode);
                Param.Add("@BranchCode", objT.BranchCode.ToString());
                DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching course names. Details: " + ex.Message);
            }
        }
        /// <summary>
        /// Fetches the project duration based on the provided project code.
        /// </summary>
        /// <param name="ProjectCode">The code of the project for which to fetch the duration.</param>
        /// <returns>An asynchronous task representing the operation that yields a SqlDataReader.</returns>
        public async Task<SqlDataReader> ProjectDuration(Trainer objT)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@flag", "FetchProjectDuration");
                Param.Add("@ProjectCode", objT.ProjectCode);
                Param.Add("@BranchCode", objT.BranchCode.ToString());
                SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTTrainer", Param);
                return dr;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching project duration. Details: " + ex.Message);
            }
        }
        /// <summary>
        /// Registers the assignment of a project for a Trainer asynchronously.
        /// </summary>
        /// <param name="ObjT">The Trainer object containing assignment details.</param>
        /// <returns>An asynchronous task representing the registration process.</returns>
        /// <exception cref="Exception">Thrown if an error occurs during the registration process.</exception>
        public async Task RegisterAssignProjectAsyncPG(Trainer ObjT)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@flag", "TrainerAssignProject");
                // Param.Add("@BranchCode", ObjT.BranchCode.ToString());
                Param.Add("@BatchCode", ObjT.BatchName);
                Param.Add("@MergeBatchCode", ObjT.MergeBatch);
                Param.Add("@TeamLeaderCode", ObjT.TeamLeaderName);
                Param.Add("@TrainerCodeStaffCode", ObjT.StaffCode.ToString());
                Param.Add("@ProjectCode", ObjT.ProjectName);
                Param.Add("@StartDate", ObjT.StartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                Param.Add("@CandidateCodes", ObjT.SelectedStudentsString);
                Param.Add("@StudentCode", ObjT.MergeStudentsString);
                Param.Add("@AssignedDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                Param.Add("@TrainerCodeProjectAssignedBy", ObjT.AssignTrainer);
                Param.Add("@StatusId", 50.ToString());
                Param.Add("@Status", 50.ToString());
                await DBHelper.ExecuteStoreProcedure("GSTTrainer", Param);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while registering the assigned project. Details: " + ex.Message);
            }
        }
        /// <summary>
        /// Fetches assigned project details for a Trainer asynchronously based on the provided AssignProjectId.
        /// </summary>
        /// <param name="objT">The Trainer object containing the AssignProjectId to fetch details for.</param>
        /// <returns>An asynchronous task representing the operation that yields a SqlDataReader with assigned project details.</returns>
        /// <exception cref="Exception">Thrown if an error occurs during the fetching process.</exception>
        public async Task<SqlDataReader> FetchAssignProjectAsyncPG(Trainer objT)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@flag", "ShowAssignProjectDetails");
                Param.Add("@BranchCode", objT.BranchCode.ToString());
                Param.Add("@AssignProjectId", objT.AssignProjectId.ToString());
                SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTTrainer", Param);
                return dr;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching assigned project details. Details: " + ex.Message);
            }
        }

        public async Task<SqlDataReader> DetailsAssignProjectAsyncPG(Trainer objT)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@flag", "ShowDetailsProjects");
                Param.Add("@BranchCode", objT.BranchCode.ToString());
                Param.Add("@AssignProjectId", objT.AssignProjectId.ToString());
                SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTTrainer", Param);
                return dr;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching assigned project details. Details: " + ex.Message);
            }
        }
        /// <summary>
        /// Updates the details of an assigned project for a Trainer asynchronously.
        /// </summary>
        /// <param name="ObjT">The Trainer object containing updated assignment details.</param>
        /// <returns>An asynchronous task representing the update process.</returns>
        /// <exception cref="Exception">Thrown if an error occurs during the update process.</exception>
        public async Task UpdateAssignProjectAsyncPG(Trainer ObjT)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@flag", "UpdateAssignProject");
                Param.Add("@AssignProjectId", ObjT.AssignProjectId.ToString());
                // Param.Add("@BranchCode", ObjT.BranchCode.ToString());
                Param.Add("@BatchCode", ObjT.BatchCode);
                Param.Add("@MergeBatchCode", ObjT.MergeBatch);
                Param.Add("@TrainerCodeStaffCode", ObjT.StaffCode.ToString());
                Param.Add("@TeamLeaderCode", ObjT.TeamLeaderName);
                Param.Add("@AssignByStaffCode", ObjT.TrainerCode);
                Param.Add("@ProjectCode", ObjT.ProjectCode);
                Param.Add("@StartDate", ObjT.StartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                Param.Add("@CandidateCodes", ObjT.SelectedStudentsString);
                Param.Add("@StudentCode", ObjT.MergeStudentsString);
                Param.Add("@AssignedDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                //Param.Add("@AssignByStaffCode", "STF006");
                Param.Add("@Status", 50.ToString());
                await DBHelper.ExecuteStoreProcedure("GSTTrainer", Param);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the assigned project. Details: " + ex.Message);
            }
        }
        /// <summary>
        /// Fetches a DataSet containing information about students assigned to a specific project for a Trainer asynchronously.
        /// </summary>
        /// <param name="objT">The Trainer object containing the AssignProjectId to fetch students for.</param>
        /// <returns>An asynchronous task representing the operation that yields a DataSet with student information.</returns>
        /// <exception cref="Exception">Thrown if an error occurs during the fetching process.</exception>
        public async Task<DataSet> FetchStudents(Trainer objT)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@flag", "FetchStudents");
                Param.Add("@AssignProjectId", objT.AssignProjectId.ToString());
                Param.Add("@BranchCode", objT.BranchCode.ToString());
                DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching course names. Details: " + ex.Message);
            }
        }
        /// <summary>
        /// Updates the status of a specific sprint for a project assigned to a Trainer asynchronously.
        /// </summary>
        /// <param name="sprintNumber">The number of the sprint to update the status for.</param>
        /// <param name="assignProjectId">The AssignProjectId of the assigned project.</param>
        /// <param name="ObjT">The Trainer object associated with the assigned project.</param>
        /// <returns>An asynchronous task representing the update process.</returns>
        public async Task UpdateSprintStatus(int sprintNumber, int assignProjectId, string CandidateCode, Trainer ObjT)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@flag", "UpdateStatusForProject");
                Param.Add("@AssignProjectId", assignProjectId.ToString());
                //  Param.Add("@BranchCode", ObjT.BranchCode.ToString());
                Param.Add("@TrainerCodeStaffCode", ObjT.StaffCode.ToString());
                int sprintStatus = GetSprintStatus(sprintNumber);
                Param.Add("@StatusId", sprintStatus.ToString());
                if (sprintStatus == 54)
                {
                    await UpdateStudentStatus(CandidateCode);
                }

                await DBHelper.ExecuteStoreProcedure("GSTTrainer", Param);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating sprint status. Details: " + ex.Message);
            }
        }
        /// <summary>
        /// Updates the status of a student with the given candidate code.
        /// </summary>
        /// <param name="candidateCode">The candidate code of the student whose status needs to be updated.</param>
        /// <exception cref="Exception">Thrown when an error occurs while updating the student status.</exception>
        public async Task UpdateStudentStatus(string candidateCode)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@flag", "UpdateStudentStatus");
                Param.Add("@CandidateCodes", candidateCode);
                await DBHelper.ExecuteStoreProcedure("GSTTrainer", Param);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating student status. Details: " + ex.Message);
            }
        }
        /// <summary>
        /// Retrieves the status code for a given sprint number.
        /// </summary>
        /// <param name="sprintNumber">The number of the sprint.</param>
        /// <returns>The status code corresponding to the given sprint number.</returns>
        private int GetSprintStatus(int sprintNumber)
        {
            int sprintStatus;

            switch (sprintNumber)
            {
                case 1:
                    sprintStatus = 52;
                    break;
                case 2:
                    sprintStatus = 53;
                    break;
                case 3:
                    sprintStatus = 54;
                    break;

                default:
                    sprintStatus = 6;
                    break;
            }
            return sprintStatus;
        }
        /// <summary>
        /// Retrieves the status ID for a specific assigned project asynchronously.
        /// </summary>
        /// <param name="objT">The Trainer object containing the AssignProjectId to fetch the status ID for.</param>
        /// <returns>An asynchronous task representing the operation that yields a SqlDataReader with the status ID.</returns>
        public async Task<SqlDataReader> GetStatusId(Trainer objT)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@flag", "GetStatusId");
                Param.Add("@BranchCode", objT.BranchCode.ToString());
                // Param.Add("@AssignProjectId", objT.StaffCode.ToString());
                Param.Add("@AssignProjectId", objT.AssignProjectId.ToString());

                SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTTrainer", Param);
                return dr;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching status ID. Details: " + ex.Message);
            }
        }

        //----------pratibha end project management ------------------------------------------------------------------//

        //----------pratibha end project management ------------------------------------------------------------------//

        // YASH Course start  
        /// <summary>
        /// This method is used to show the gridview of course content
        /// </summary>
        /// <returns> Course content list </returns>
        public async Task<DataSet> ListCourseContentYT()
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ListCourseContentYT");
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        // YASH Course ed 
        //--------------------tushar dashboard start ----------------------------------------------------------------------
        public async Task<int> GetStudentCount(string CourseCode, string BranchCode)
        {

            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@Flag", "AllStudentCount");
                Param.Add("@CourseCode", CourseCode);
                Param.Add("@BranchCode", BranchCode);
                int totalBatchCount = (int)await DBHelper.ExecuteStoreProcedureReturnObj("GSTTrainer", Param);

                return totalBatchCount;
            }
            catch (Exception)
            {
                throw;
            }

        }


        public async Task<int> GetAllBatchCount(string CourseCode, string BranchCode)
        {

            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@Flag", "Allbatchcount");
                Param.Add("@CourseCode", CourseCode);
                Param.Add("@BranchCode", BranchCode);
                int totalBatchCount = (int)await DBHelper.ExecuteStoreProcedureReturnObj("GSTTrainer", Param);

                return totalBatchCount;
            }
            catch (Exception ex)
            {
                throw;
            }
        }




        public async Task<int> ActiveBatch(string CourseCode, string BranchCode)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@Flag", "ActiveBatches");
                Param.Add("@CourseCode", CourseCode);
                Param.Add("@BranchCode", BranchCode);

                int ActiveBatchCount = (int)await DBHelper.ExecuteStoreProcedureReturnObj("GSTTrainer", Param);

                return ActiveBatchCount;
            }
            catch (Exception ex)
            {



                throw;
            }
        }


        public async Task<int> ReleaseBatch(string CourseCode, string BranchCode)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@Flag", "ReleaseBatches");
                Param.Add("@CourseCode", CourseCode);
                Param.Add("@BranchCode", BranchCode);

                int ReleaseBatchCount = (int)await DBHelper.ExecuteStoreProcedureReturnObj("GSTTrainer", Param);

                return ReleaseBatchCount;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<int> UpcomingBatch(string CourseCode, string BranchCode)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@Flag", "UpcomingBatches");
                Param.Add("@CourseCode", CourseCode);
                Param.Add("@BranchCode", BranchCode);

                int UpcomingBatchCount = (int)await DBHelper.ExecuteStoreProcedureReturnObj("GSTTrainer", Param);

                return UpcomingBatchCount;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<int> ActiveStudent(string CourseCode, string BranchCode)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@Flag", "activestudet");
                Param.Add("@CourseCode", CourseCode);
                Param.Add("@BranchCode", BranchCode);
                int ActiveStudent = (int)await DBHelper.ExecuteStoreProcedureReturnObj("GSTTrainer", Param);

                return ActiveStudent;
            }
            catch (Exception ex)
            {
                // Log the exception
                // Example: logger.LogError(ex, "Error occurred in PlacedStudent method.");

                // You can either re-throw the exception or return a default value or handle it in any appropriate manner for your application.
                throw; // re-throw the exception
            }
        }


        public async Task<int> RealeseStudent(string CourseCode, string BranchCode)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@Flag", "Realesetudent");
                Param.Add("@CourseCode", CourseCode);
                Param.Add("@BranchCode", BranchCode);
                int ReleaseStudent = (int)await DBHelper.ExecuteStoreProcedureReturnObj("GSTTrainer", Param);

                return ReleaseStudent;
            }
            catch (Exception ex)
            {
                // Log the exception
                // Example: logger.LogError(ex, "Error occurred in PlacedStudent method.");

                // You can either re-throw the exception or return a default value or handle it in any appropriate manner for your application.
                throw; // re-throw the exception
            }
        }





        public async Task<int> PlacedStudent(string CourseCode, string BranchCode)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@Flag", "Placedstudent");
                Param.Add("@CourseCode", CourseCode);
                Param.Add("@BranchCode", BranchCode);

                int PlacedStudent = (int)await DBHelper.ExecuteStoreProcedureReturnObj("GSTTrainer", Param);

                return PlacedStudent;
            }
            catch (Exception ex)
            {
                // Log the exception
                // Example: logger.LogError(ex, "Error occurred in PlacedStudent method.");

                // You can either re-throw the exception or return a default value or handle it in any appropriate manner for your application.
                throw; // re-throw the exception
            }
        }


        public async Task<List<Trainer>> GetBatchVsNoOfStudentGraphData(string CourseCode, string TrainerCodeStaffCode, string BranchCode)
        {
            List<Trainer> trainers = new List<Trainer>();
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@Flag", "graphbatchvsnostudett");
                Param.Add("@CourseCode", CourseCode);
                Param.Add("@BranchCode", BranchCode);
                Param.Add("@TrainerCodeStaffCode", TrainerCodeStaffCode);
                using (DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param))
                {
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            Trainer trainer = new Trainer
                            {
                                BatchName = Convert.ToString(row["BatchName"]),
                                NoOfStudent = Convert.ToInt32(row["NoOfStudent"])
                            };

                            trainers.Add(trainer);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return trainers;
        }

        //--------------------tushar dashboard End ----------------------------------------------------------------------
        //---sayali notification start -------------------------------------------------------------
        //----------------------------------------BatchScheduled Notification Task ST--------------------------------


        /// <summary>
        /// This method get all Scheduled batches Request  on list.
        /// </summary>
        /// <returns> Batch List</returns>
        public async Task<DataSet> ListScheduledBatchRequestAsyncST(Trainer ObjTr)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "ListScheduledBatchRequestAsyncST");
            Param.Add("@TrainerCodeStaffCode", ObjTr.TrainerCode);
            Param.Add("@BranchCode", ObjTr.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }


        /// <summary>
        ///This method use to get batch Schedule data for View Only and Update.
        /// </summary>
        /// <returns> Batch Schedule Data</returns>
        public async Task<SqlDataReader> DetailsBatchScheduleAsyncST(Trainer ObjTr)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "DetailsBatchScheduleAsyncST");
            Param.Add("@ScheduleId", ObjTr.ScheduleId.ToString());
            Param.Add("@BranchCode", ObjTr.BranchCode);

            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTTrainer", Param);
            return dr;
        }


        /// <summary>
        /// This method use to  Updated Batch Status ,schedule status and also student status.
        /// </summary>
        /// <returns> </returns>
        public async Task NotificationRequestAcceptAsyncST(Trainer ObjTr)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "NotificationRequestAcceptAsyncST");
            Param.Add("@ScheduleId", ObjTr.ScheduleId.ToString());
            Param.Add("@BatchCode", ObjTr.BatchCode);
            await DBHelper.ExecuteStoreProcedure("GSTTrainer", Param);
        }
        /// <summary>
        /// This method use to  Updated Batch Status ,schedule status and also student status.
        /// </summary>
        /// <returns> </returns>
        public async Task NotificationRequestRejectAsyncST(Trainer ObjTr)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "NotificationRequestRejectAsyncST");
            Param.Add("@ScheduleId", ObjTr.ScheduleId.ToString());
            Param.Add("@BatchCode", ObjTr.BatchCode);
            await DBHelper.ExecuteStoreProcedure("GSTTrainer", Param);
        }



        //---sayali notification End  -------------------------------------------------------------

        //-------------------kirti FeedBack Start-----------------------------------------------------------------//
        /// <summary>
		/// This method is for the showing the list batch stduent list for add new feedback.
		/// </summary>
		/// <param name="objsf">This parameter use for the access the property.</param>
		/// <returns>It returns the batch student list of add new feedback.</returns>
		public async Task<DataSet> ShowNewStudentsFeedbackKKAsync(Trainer objsf)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "NewListFeedbackForStudentKK");
            Param.Add("@BranchCode", objsf.BranchCode);
            Param.Add("@StaffCode", objsf.TrainerCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// This method is for the showing single data which id is selected.
        /// </summary>
        /// <param name="objaddf">This parameter is for the showing selected data.</param>
        /// <returns>It returns the data which id is selected.</returns>
        public async Task<DataSet> AddedNewStudentsFeedbackKKAsync(Trainer objaddf)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "NewFeedbackForStudKK");
            Param.Add("@FeedbackId", objaddf.FeedbackId.ToString());
            Param.Add("@BranchCode", objaddf.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// This method is for the update the feedback of particular student.
        /// </summary>
        /// <param name="feedupdate">This object use for the select particular feedback id.</param>
        /// <returns>It returns the update the rating and the comment.</returns>
        public async Task<DataSet> FeedbackUpdateKKAsync(Trainer feedupdate)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "UpdateFeedbackForStudKK");
            Param.Add("@FeedbackId", feedupdate.FeedbackId.ToString());
            Param.Add("@FeedbackAddedDate", DateTime.Now.ToString("yyyy-MM-dd"));
            Param.Add("@Rating", feedupdate.Rating.ToString());
            Param.Add("@Comment", feedupdate.Comment);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }

        //-------------------kirti FeedBack END-----------------------------------------------------------------//
        //-------------------Tushar  Test Maagement  Start -----------------------------------------------------------------//
        public async Task AddNewTestAsycTS(Trainer objT, string staffCode)
        {

            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "AddNewTest");
            Param.Add("@TestName", objT.TestName);
            Param.Add("@CourseCode", objT.CourseName);
            Param.Add("@SectionName ", objT.SectionName);
            Param.Add("@TopicId", objT.TopicName);
            // Set RegisterDate to the current date and time
            // objT.RegisterDate = DateTime.Now;

            Param.Add("@RegisterDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            Param.Add("@Duration", objT.SDuration.ToString());
            Param.Add("@TotalMarks", objT.TotalMarks.ToString());
            Param.Add("@PassingMarks", objT.PassingMarks.ToString());
            Param.Add("@TestPaperFile", objT.TestPaperFile);
            Param.Add("@TrainerCodeTestAddedBy", staffCode);
            Param.Add("@TotalNoOfQuestion", objT.TotalNoOfQuestion.ToString());
            Param.Add("@StatusId", 44.ToString());
            await DBHelper.ExecuteStoreProcedure("GSTTrainer", Param);
        }
        public async Task<DataSet> ListAddTestAsyncTs(string branchCode, string staffCode)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ListNewAddTest");
            Param.Add("@BranchCode", branchCode); // Pass branch code parameter
            Param.Add("@StaffCode", staffCode); // Pass staff code parameter
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }


        /// <summary>
        /// fech add test details for edit perpose
        /// </summary>
        /// <param name="objT"></param>
        /// <returns></returns>
        public async Task<Trainer> FechAddNewTest(string branchCode, string staffCode, int testId)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "fechAllTestforEditaddnewTest");
            Param.Add("@TestId", testId.ToString());
            Param.Add("@BranchCode", branchCode); // Pass branch code parameter
            Param.Add("@StaffCode", staffCode); // Pass staff code parameter
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTTrainer", Param);
            Trainer objT = null; // Create Trainer object to hold fetched data

            if (dr.Read())
            {
                objT = new Trainer();
                objT.TestId = Convert.ToInt32(dr["TestId"]);
                objT.TestName = dr["TestName"].ToString();
                objT.CourseName = dr["CourseName"].ToString();
                objT.CourseCode = dr["Coursecode"].ToString();
                objT.SectionName = dr["SectionName"].ToString();
                objT.SectionId = Convert.ToInt32(dr["SectionId"].ToString());
                objT.TopicName = dr["TopicName"].ToString();
                objT.TotalMarks = Convert.ToInt32(dr["TotalMarks"].ToString());
                objT.PassingMarks = Convert.ToInt32(dr["PassingMarks"].ToString());
               // objT.TopicId = Convert.ToInt32(dr["TopicId"].ToString());
                objT.RegisterDate = DateTime.Parse(dr["RegisterDate"].ToString());
                objT.SDuration = TimeSpan.Parse(dr["Duration"].ToString());
                objT.TestPaperFile = dr["TestPaperFile"].ToString(); // Fetch file path
            }

            return objT;
        }
        public async Task deleteAddTest(Trainer objT)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "deleteTest");
            Param.Add("@TestId", objT.TestId.ToString());
            await DBHelper.ExecuteStoreProcedure("GSTTrainer", Param);

        }
        public async Task UpdateAddNewTestAsyncTS(Trainer objT, string staffCode)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "updateAddNewTest");
            Param.Add("@TestId", objT.TestId.ToString());
            Param.Add("@TestName", objT.TestName);
            // Param.Add("@CourseCode", objT.CourseName);
            // Param.Add("@SectionName", objT.SectionName);
            // Param.Add("@TopicName", objT.TopicName);
            //  Param.Add("@TopicId", objT.TopicId.ToString());
            Param.Add("@RegisterDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            Param.Add("@Duration", objT.SDuration.ToString());
            Param.Add("@TotalMarks", objT.TotalMarks.ToString());
            Param.Add("@PassingMarks", objT.PassingMarks.ToString());
            Param.Add("@TrainerCodeTestAddedBy", staffCode);
            Param.Add("@TestPaperFile", objT.TestPaperFile);
            // Add other parameters as needed for the update

            await DBHelper.ExecuteStoreProcedure("GSTTrainer", Param);
        }

        public async Task ResisterAssignTtest(Trainer objT, string staffCode)
        {
            // Retrieve the staff code from the session
            // string staffCode = objT.TrainerCodeAssignedByCode;

            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "RegisterAssignTest");
            Param.Add("@BatchCode", objT.BatchName);
            Param.Add("@TestId", objT.TestName);
            Param.Add("@StatusId", 19.ToString());
            Param.Add("@CompleteTillDate", objT.CompleteTillDate.ToString("yyyy-MM-dd HH:mm:ss"));
            Param.Add("@TrainerCodeAssignedByCode", staffCode); // Use staff code here
            Param.Add("@AssignDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            // Param.Add("@RetestForAssignTestId", objT.TestName);
            await DBHelper.ExecuteStoreProcedure("GSTTrainer", Param);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objT"></param>
        /// <returns></returns>
        public async Task ReResisterAssigntest(Trainer objT)
        {

            string staffCode = objT.TrainerCodeAssignedByCode;

            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ReAssignTest");
            Param.Add("@BatchCode", objT.BatchName);
            //  Param.Add("@BatchCode", objT.BatchCode);
            Param.Add("@TestId", objT.TestName);
            Param.Add("@StatusId", 19.ToString());
            // Param.Add(@CompleteTillDate", objT.CompleteTillDate);
            Param.Add("@CompleteTillDate", objT.CompleteTillDate.ToString("yyyy-MM-dd HH:mm:ss"));
            Param.Add("@TrainerCodeAssignedByCode", staffCode); // Use staff code here
            Param.Add("@AssignDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            Param.Add("@RetestForAssignTestId", objT.TestName);
            await DBHelper.ExecuteStoreProcedure("GSTTrainer", Param);
        }

        /// <summary>
        /// Get All Course
        /// </summary>
        /// <returns></returns>
        /// 
        public async Task<DataSet> Course()                         //--------------------------------------//
        {

            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ListBranch");
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        public async Task<DataSet> ListCourse(Trainer objT)                         //--------------------------------------//
        {

            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ListCourse");
            Param.Add("@StaffCode", objT.StaffCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        public async Task<DataSet> Branch()                         //--------------------------------------//
        {

            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ListBranch");
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// Get All Section-
        /// </summary>
        /// <param name="CourseCode"></param>
        /// <returns></returns>
        public async Task<DataSet> GetSection(Trainer objT)          //------------------------------------//
        {

            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "SectionList");
            Param.Add("@CourseCode", objT.CourseCode);

            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;

        }

        public async Task<DataSet> ListBatch(Trainer objT)          //------------------------------------//
        {

            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "listBatch");
            Param.Add("@CourseCode", objT.CourseCode);

            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;

        }
        public async Task<DataSet> GetBranchrelatedCourse(string BranchCode)          //------------------------------------//
        {

            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "getBranchrelatedCourse");
            Param.Add("@BranchCode", BranchCode);

            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;

        }

        public async Task<DataSet> fetchAssignedTest(string branchCode, string staffCode, int AssignTestId)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "fetchAssignedTest");

            Param.Add("@AssignTestId", AssignTestId.ToString());
            Param.Add("@BranchCode", branchCode); // Pass branch code parameter
            Param.Add("@StaffCode", staffCode); // Pass staff code parameter
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        public async Task UpdateAssignTest(Trainer objT)
        {
            try
            {
                string staffCode = objT.TrainerCodeAssignedByCode;

                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@Flag", "updateAssignTest");
                Param.Add("@AssignTestId", objT.AssignTestId.ToString());

                Param.Add("@TestId", objT.TestId.ToString());

                Param.Add("@CompleteTillDate", objT.CompleteTillDate.ToString("yyyy-MM-dd HH:mm:ss"));
                Param.Add("@TrainerCodeAssignedByCode", staffCode); // Use staff code here


                await DBHelper.ExecuteStoreProcedure("GSTTrainer", Param);
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine("An error occurred while updating the record: " + ex.Message);
                // You can log the exception to a file or a logging framework like Serilog or NLog
                // Example: logger.LogError(ex, "An error occurred while updating the record");
                throw; // Rethrow the exception to propagate it further if needed
            }
        }
        /// <summary>
        /// -Get All Section-
        /// </summary>
        /// <param name="SectionId"></param>
        /// <returns></returns>
        public async Task<DataSet> GetTopic(Trainer objT)
        {

            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "TopicNameList");
            Param.Add("@SectionId", objT.SectionId.ToString());
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        public async Task<DataSet> GetTestNameforAssign(Trainer objT)          //------------------------------------//
        {

            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "getTestName");
            Param.Add("@TopicId", objT.TopicId.ToString());
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }


        /// <summary>
        /// forAll List
        /// </summary>
        /// <returns></returns>
        public async Task<DataSet> AllTestList()
        {
            //con.Open();
            //SqlCommand cmd = new SqlCommand("GSTTrainer", con);
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@Flag", "getAllTestList");
            //// cmd.Parameters.AddWithValue("@TestId", " TestId");
            //SqlDataAdapter adpt = new SqlDataAdapter();
            //DataSet ds = new DataSet();
            //adpt.SelectCommand = cmd;
            //await Task.Run(() => adpt.Fill(ds));
            //return ds;
            //con.Close();
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "getAllTestList");
            //  Param.Add("@TestId",  TestId.);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// for conducted List
        /// </summary>
        /// <returns></returns>
        public async Task<DataSet> ConductedList(string branchCode, string staffCode)
        {

            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ListConductedTest");
            Param.Add("@BranchCode", branchCode); // Pass branch code parameter
            Param.Add("@StaffCode", staffCode); // Pass staff code parameter
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// for Pending List
        /// </summary>
        /// <returns></returns>
        public async Task<DataSet> ListPendingTestAsync(string branchCode, string staffCode)
        {

            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ListPendingTest");
            Param.Add("@BranchCode", branchCode); // Pass branch code parameter
            Param.Add("@StaffCode", staffCode); // Pass staff code parameter
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// for Arrenged List
        /// </summary>
        /// <returns></returns>
        /// 
        public async Task<SqlDataReader> DetailsPendingListAsycTS(string branchCode, string staffCode, int AssignTestId)
        {

            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "readpedingtest");
            Param.Add("@AssignTestId", AssignTestId.ToString());
            Param.Add("@BranchCode", branchCode); // Pass branch code parameter
            Param.Add("@StaffCode", staffCode); // Pass staff code parameter
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTTrainer", Param);
            return dr;
        }
        public async Task<SqlDataReader> DetailsArregeListAsncTS(string branchCode, string staffCode, int AssignTestId)
        {

            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ViewArrangetest");
            Param.Add("@AssignTestId", AssignTestId.ToString());
            Param.Add("@BranchCode", branchCode); // Pass branch code parameter
            Param.Add("@StaffCode", staffCode); // Pass staff code parameter
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTTrainer", Param);
            return dr;
        }
        public async Task<DataSet> ArrengedList(string branchCode, string staffCode)
        {

            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ListArrangedTest");
            Param.Add("@BranchCode", branchCode); // Pass branch code parameter
            Param.Add("@StaffCode", staffCode); // Pass staff code parameter
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// for Assign List
        /// </summary>
        /// <returns></returns>
        public async Task<DataSet> ListAssignTestAsycTS(string branchCode, string staffCode)
        {

            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ListAssignedTest");
            Param.Add("@BranchCode", branchCode); // Pass branch code parameter
           // Param.Add("@StaffCode", staffCode); // Pass staff code parameter
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        public async Task<DataSet> ResultDetailsAsyncTS(string branchCode, string staffCode, int id)
        {

            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "DetailsConductedTest");
            // Param.Add("@AssignTestId",objtrainer.AssignTestId.ToString());
            Param.Add("@AssignTestId", id.ToString());
            Param.Add("@BranchCode", branchCode); // Pass branch code parameter
            Param.Add("@StaffCode", staffCode); // Pass staff code parameter
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        public async Task<DataSet> ShowResultStudent(Trainer objT)
        {

            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ResultlistOfStudent");
            Param.Add("@AssignTestId", objT.AssignTestId.ToString());
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }


        public async Task<DataSet> GetTestName(int TopicId)          //------------------------------------//
        {

            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "getTestName");
            Param.Add("@TopicId", TopicId.ToString());
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        public async Task<DataSet> GetstudentResult(string BatchCode)          //------------------------------------//
        {

            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "StudentListforresult");
            Param.Add("@BatchCode", BatchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        public async Task AddResult(Trainer objT, string staffCode)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "AddResultOfStudent");
            Param.Add("@TestAssignedId", objT.AssignTestId.ToString());
            Param.Add("@StudentCode", objT.StudentCode);
            Param.Add("@ObtainedMarks", objT.ObtainedMarks.ToString());
            Param.Add("@AttendanceStatusId", objT.AttendanceStatusId.ToString());
            Param.Add("@ResultStatus", objT.Status);
            Param.Add("@TrainerCodeResultAddedBy", staffCode);
            Param.Add("@ResultAddedDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            await DBHelper.ExecuteStoreProcedure("GSTTrainer", Param);
        }
        private int? GetAttendanceStatusId(string attendanceStatus)
        {
            switch (attendanceStatus.ToLower())
            {
                case "present":
                    return 1; // Replace with the actual ID for "Present" in your database
                case "absent":
                    return 2; // Replace with the actual ID for "Absent" in your database
                default:
                    return null; // Allow null for other cases
            }
        }

        //-------------------Tushar  Test Maagement END-----------------------------------------------------------------//

        //-----------------Kirti Demo Start ------------------------------------------------------------------------------//
        /// <summary>
		/// This method is showing the demo request count as per trainer.
		/// </summary>
		/// <returns>Showing the list of demo request.</returns>
        public async Task<DataSet> DemoReNotificationKKAsync(Trainer count)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "DemoRequestKK");
            Param.Add("@TrainerCodeStaffCode", count.TrainerCode);
            Param.Add("@BranchCode", count.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// This method is for the all arranged demo list which comes from co-ordinator.
        /// </summary>
        /// <param name="obj">This parameter use for the access the property.</param>
        /// <returns>Returns the arranged demo lists.</returns>
        public async Task<DataSet> ViewArrangedDemoListKKAsync(Trainer obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "ArrangedDemoRequestKK");
            Param.Add("@StatusId", obj.StatusId.ToString());
            Param.Add("@BranchCode", obj.BranchCode);
            Param.Add("@TrainerCodeStaffCode", obj.TrainerCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// This method is shows the Noofstudent list.
        /// </summary>
        /// <param name="Batchcode">Using batch code showing the student list.</param>
        /// <returns>It returns the list of student.</returns>
        public async Task<DataSet> DemoNoOfStudentListKKAsync(Trainer objstu)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "DemoStudentsListKK");
            Param.Add("@BatchCode", objstu.BatchCode);
            Param.Add("@BranchCode", objstu.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// This sis method for the demo request accepted.
        /// </summary>
        /// <param name="objdr">This parameter is use for the access the properties.</param>
        /// <returns>Returns the request detail of particular trainer.</returns>
        public async Task<DataSet> DemoAccecptedKKAsync(Trainer objdr)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "ArrangedDemoAcceptKK");
            Param.Add("@ScheduleId", objdr.ScheduleId.ToString());
            Param.Add("@BranchCode", objdr.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// This method is for the change the status of the demo.
        /// </summary>
        /// <param name="obj">This parameter use for the access property from the class.</param>
        /// <returns>It returns the changed status.</returns>
        public async Task ChangeAcceptStatusKKAsync(Trainer obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "DemoAcceptedStatusKK");
            Param.Add("@ScheduleId", obj.ScheduleId.ToString());
            await DBHelper.ExecuteStoreProcedure("GSTTrainer", Param);
        }
        /// <summary>
        /// This method is for the change the status of the demo.
        /// </summary>
        /// <param name="obj">This parameter use for the access property from the class.</param>
        /// <returns>It returns the changed status.</returns>
        public async Task ChangeDemoAcceptStatusKKAsync(Trainer objs)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "DemoRejectedStatusKK");
            Param.Add("@ScheduleId", objs.ScheduleId.ToString());
            await DBHelper.ExecuteStoreProcedure("GSTTrainer", Param);
        }
        /// <summary>
        /// This method is for the showing the details of demo which is Assigned.
        /// </summary>
        /// <param name="Scheduleid">The schedule id use for the selected id data showing.</param>
        /// <returns>Retruns the selected id data.</returns>
        public async Task<DataSet> DemoAccpetedDetailsKKAsync(Trainer objdd)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "DemoAcceptDetailsKK");
            Param.Add("@ScheduleId", objdd.ScheduleId.ToString());
            Param.Add("@BranchCode", objdd.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// This method is for the showing student name of the batch.
        /// </summary>
        /// <param name="objatt">The obj is use for the access parameter.</param>
        /// <returns>It returns the student list.</returns>
        public async Task<DataSet> DemoStudentAttendanceListKKAsync(Trainer objatt)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "DemoStudentAttendanceKK");
            Param.Add("@BatchCode", objatt.BatchCode);
            Param.Add("@BranchCode", objatt.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// This method is for the save attendance of demo.
        /// </summary>
        /// <param name="objdatt">This parameter use for the access property from the class.</param>
        /// <returns>It returns the attendance data.</returns>
        public async Task SaveDemoAttendanceKKAsync(Trainer objdatt)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "SaveDemoStudentsAttendanceKK");
            Param.Add("@BatchCode", objdatt.BatchCode);
            Param.Add("@Date", objdatt.StartDate.ToString("yyyy-MM-dd HH:mm:ss"));
            Param.Add("@Time", objdatt.StartTime.ToString("yyyy-MM-dd HH: mm:ss"));
            Param.Add("@StudentCode", objdatt.StudentCode);
            Param.Add("@AttendanceStatusId", objdatt.AttendanceStatusId.ToString());
            Param.Add("@TrainerCodeStaffCode", objdatt.TrainerCode);
            Param.Add("@StatusId", "10");
            await DBHelper.ExecuteStoreProcedure("GSTTrainer", Param);
        }

        //-----------------Kirti Demo  End -------------------------------------------------------------------------------//
        //------------------YASH Attendance Start --------------------------------------------------------------//
        /// <summary>
        /// This method is used to show the gridview of topicwise attendance
        /// </summary>
        /// <param name="StaffCode"></param>
        /// <returns> Topicwise attendance list </returns>
        public async Task<DataSet> ListAttendanceTopicYT(string StaffCode)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ListAttendanceTopicYT");
            Param.Add("@StaffCode", StaffCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// This method is used to show the gridview of topicwise attendance student list as per batch
        /// </summary>
        /// <param name="BatchCode"></param>
        /// <returns></returns>
        public async Task<DataSet> AttendanceTopicStudentListYT(string BatchCode)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "AttendanceTopicStudentListYT");
            Param.Add("@BatchCode", BatchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// This method is used to show the gridview of count of topics in single student list
        /// </summary>
        /// <param name="studentCode"></param>
        /// <returns></returns>
        public async Task<DataSet> ViewCountTopicSingleStudentYT(string studentCode)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ViewCountTopicSingleStudentYT");
            Param.Add("@StudentCode", studentCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// This method is used to show the gridview of topic single student 
        /// </summary>
        /// <param name="StudentCode"></param>
        /// <returns></returns>
        public async Task<DataSet> AttendanceTopicSingleStudentYT(string StudentCode)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "AttendanceTopicSingleStudentYT");
            Param.Add("@StudentCode", StudentCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// This method is used to show the gridview topicwise attendance
        /// </summary>
        /// <param name="BatchCode"></param>
        /// <returns></returns>
        public async Task<DataSet> ViewAttendanceTopicYT(string BatchCode)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ViewAttendanceTopicYT");
            Param.Add("@BatchCode", BatchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// This method is used to show the gridview of view session page
        /// </summary>
        /// <param name="AssignScheduleId"></param>
        /// <returns></returns>
        public async Task<DataSet> ViewSessionsTopicYT(int AssignScheduleId)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ViewSessionsTopicYT");
            Param.Add("@AssignScheduleId", AssignScheduleId.ToString());
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// This method is used to show the gridview of view of marked attendance topic wise
        /// </summary>
        /// <param name="AssignScheduleId"></param>
        /// <param name="Date"></param>
        /// <param name="Time"></param>
        /// <returns></returns>
        public async Task<DataSet> ViewMarkedAttendanceTopicYT(int AssignScheduleId, DateTime Date, DateTime Time)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ViewMarkedAttendanceTopicYT");
            Param.Add("@AssignScheduleId", AssignScheduleId.ToString());
            Param.Add("@Date", Date.ToString("yyyy-MM-dd HH:mm:ss"));
            Param.Add("@Time", Time.ToString("yyyy-MM-dd HH:mm:ss"));
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// This method is used to show the assigned batch to the dropdown list
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<DataSet> ListAssignedBatchYT(Trainer obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "ListAssignedBatchYT");
            Param.Add("@StaffCode", obj.StaffCode);
            Param.Add("@BranchCode", obj.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// This method is used to show the list of section as per batch
        /// </summary>
        /// <param name="ScheduleId"></param>
        /// <returns></returns>
        public async Task<DataSet> ListSection(int ScheduleId)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ListSectionYT");
            Param.Add("@AssignScheduleId", ScheduleId.ToString());
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// This method is used to show the list of topic as per sections
        /// </summary>
        /// <param name="SectionId"></param>
        /// <returns></returns>
        public async Task<DataSet> ListTopic(int SectionId, int Scheduleid)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ListTopicYT");
            Param.Add("@SectionId", SectionId.ToString());
            Param.Add("@ScheduleId", Scheduleid.ToString());
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// This method is used to show the start date as per topic
        /// </summary>
        /// <param name="TopicId"></param>
        /// <returns></returns>
        public async Task<SqlDataReader> GetTopicStartDate(int TopicId)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "GetTopicStartDate");
            Param.Add("@TopicId", TopicId.ToString());
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTTrainer", Param);
            return dr;
        }
        /// <summary>
        /// This method is used to show the student list
        /// </summary>
        /// <param name="scheduleid"></param>
        /// <returns></returns>
        public async Task<DataSet> ListStudentYT(int scheduleid)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ListStudentYT");
            Param.Add("@ScheduleId", scheduleid.ToString());
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// This method is used to add attemdance of topic
        /// </summary>
        /// <param name="objT"></param>
        /// <returns></returns>
        public async Task AddAttendanceYT(Trainer objT)
        {
            string statusId = "";
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "AddAttendanceYT");
            Param.Add("@BatchCode", objT.BatchCode);
            if (objT.AssignScheduleId != 0)
            {
                statusId = "28";
                Param.Add("@ScheduleId", objT.AssignScheduleId.ToString());
            }
            if (objT.AssignProjectId != 0)
            {
                statusId = "29";
                Param.Add("@AssignProjectId", objT.AssignProjectId.ToString());
            }
            Param.Add("@Date", objT.Date.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            Param.Add("@Time", objT.Time.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            Param.Add("@StudentCode", objT.StudentCode);
            Param.Add("@AttendanceStatusId", objT.AttendanceStatusId.ToString());
            Param.Add("@StaffCode", objT.StaffCode);
            Param.Add("@StatusId", statusId);
            await DBHelper.ExecuteStoreProcedure("GSTTrainer", Param);
        }
        /// <summary>
        /// This method is used to show the gridview of projectwise attendance
        /// </summary>
        /// <returns></returns>
        public async Task<DataSet> ListAttendanceProjectYT(string StaffCode)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ListAttendanceProjectYT");
            Param.Add("@StaffCode", StaffCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// This method is used to show the gridview of projectwise attendance student list 
        /// </summary>
        /// <param name="AssignProjectId"></param>
        /// <returns></returns>
        public async Task<DataSet> AttendanceProjectStudentListYT(string AssignProjectId)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "AttendanceProjectStudentListYT");
            Param.Add("@AssignProjectId", AssignProjectId);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// This method is used to show the gridview of count of project in single student list
        /// </summary>
        /// <param name="studentCode"></param>
        /// <returns></returns>
        public async Task<DataSet> ViewCountProjectSingleStudentYT(string studentCode)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ViewCountProjectSingleStudentYT");
            Param.Add("@StudentCode", studentCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// This method is used to show the list of assigned batch in dropdown list
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<DataSet> ListAssignedBatchProjectYT(Trainer obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ListAssignedBatchProjectYT");
            Param.Add("@StaffCode", obj.StaffCode);
            Param.Add("@BranchCode", obj.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// This method is used to show the project start date
        /// </summary>
        /// <param name="BatchCode"></param>
        /// <returns></returns>
        public async Task<DataSet> GetProjectStartDate(string BatchCode)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "GetProjectStartDate");
            Param.Add("@BatchCode", BatchCode.ToString());
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// This method is used to show the list of students
        /// </summary>
        /// <param name="BatchCode"></param>
        /// <returns></returns>
        public async Task<DataSet> ListStudentProjectYT(string BatchCode)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ListStudentProjectYT");
            Param.Add("@BatchCode", BatchCode.ToString());
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// This method is used to show the list of monthly attendance
        /// </summary>
        /// <returns></returns>
        public async Task<DataSet> ListViewAttendanceProjectYT()
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ListViewAttendanceProjectYT");
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// This method is used to show the gridview of student leave
        /// </summary>
        /// <returns></returns>
        public async Task<DataSet> ViewStudentLeaveYT(string staffcode)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ViewStudentLeaveYT");
            Param.Add("@StaffCode", staffcode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// This method is used to edit the status of student leave
        /// </summary>
        /// <param name="staffcode"></param>
        /// <returns></returns>
        public async Task<DataSet> DetailStudentLeaveYT(string studentcode)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "DetailStudentLeaveYT");
            Param.Add("@StudentCode", studentcode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// This method is used to show list of status in dropdown
        /// </summary>
        /// <returns></returns>
        public async Task<DataSet> ListStatusYT()
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ListStatusYT");
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
        /// <summary>
        /// This method is used to edit student leave
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task EditStudentLeaveYT(Trainer obj)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "EditStudentLeaveYT");
            Param.Add("@ApplyLeaveId", obj.ApplyLeaveId.ToString());
            Param.Add("@StatusId", obj.StatusId.ToString());
            await DBHelper.ExecuteStoreProcedure("GSTTrainer", Param);
        }

        //------------------YASH Attendance Start --------------------------------------------------------------//

    }
}
