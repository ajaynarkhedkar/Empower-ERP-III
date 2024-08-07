using Helper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Helper;


namespace GSTEducationERPLibrary.Bind
{
    public class BALBind
    {
        MSSQL DBHelper = new MSSQL();

        public async Task<DataSet> AllCourseBind(Bind ObjCo)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "AllCourseBind");
            Param.Add("@BranchCode", ObjCo.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }

        public async Task<SqlDataReader> StaffProfileAsync(Bind obj)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "StaffProfile");
            Param.Add("@StaffCode", obj.StaffCode);
            Param.Add("@BranchCode", obj.BranchCode);
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTHR", Param);
            return dr;
        }
        public async Task<SqlDataReader> StaffDocumetAsync(Bind obj)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "StaffDocument");
            Param.Add("@StaffCode", obj.StaffCode);
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTHR", Param);
            return dr;
        }

        //-----------sayali Co-ordinator Trainer  Batch Management start ---------//
        //-----------sayali Co-ordinator Trainer  Batch Management start ---------//
        //----------------------------------------BatchManagement New Batch Create Task ST--------------------------------
        /// <summary>
        /// This method get all batches data  on list.
        /// </summary>
        /// <returns> Batch List</returns>
        public async Task<DataSet> GetDataBatchAsyncST(Bind ObjCo)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "GetDataBatchAsyncST");
            Param.Add("@BranchCode", ObjCo.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTBind", Param);
            return ds;
        }
        public async Task<DataSet> GetDataBatchCourseWiseAsyncST(Bind ObjCo)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "GetDataBatchCourseWiseAsyncST");
            Param.Add("@BranchCode", ObjCo.BranchCode);
            Param.Add("@CourceCode", ObjCo.CourseCode);

            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTBind", Param);
            return ds;
        }
        /// <summary>
        /// This method use for auto increment batch code on Register Batch View.
        /// </summary>
        /// <returns> auto increment batch code </returns>
        public async Task<SqlDataReader> BatchAutoCodeAsyncST()
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "BatchAutoCodeAsyncST");
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTBind", Param);
            return dr;
        }

        /// <summary>
        /// This method get all Cource data  on list.
        /// </summary>
        /// <returns> Cource list</returns>
        public async Task<DataSet> GetCourceAsyncST(Bind ObjCo)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "GetCourceAsyncST");
            Param.Add("@BranchCode", ObjCo.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTBind", Param);
            return ds;
        }

        /// <summary>
        /// This method get all Admitted Student  on list.
        /// </summary>
        /// <returns> Admitted Status Student List</returns>
        public async Task<DataSet> GetAdmittedStudentAsyncST(Bind ObjCo)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "GetAdmittedStudentAsyncST");
            Param.Add("@CourseCode", ObjCo.CourseCode);
            Param.Add("@BranchCode", ObjCo.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTBind", Param);
            return ds;
        }

        /// <summary>
        ///  This method use to save new created batch.
        /// </summary>
        public async Task AddBatchAsyncST(Bind ObjCo)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "AddBatchAsyncST");
            Param.Add("@BatchCode", ObjCo.BatchCode);
            Param.Add("@BatchName", ObjCo.BatchName);
            Param.Add("@StudentCode", ObjCo.StudentCode);
            Param.Add("@CourseCode", ObjCo.CourseCode);
            Param.Add("@NoOfStudent", ObjCo.NoOfStudent.ToString());
            Param.Add("@CreateDate", ObjCo.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"));
            Param.Add("@Statusid", ObjCo.StatusId.ToString());
            Param.Add("@TypeId", ObjCo.TypeId.ToString());
            Param.Add("@CreatedByStaffCode", ObjCo.StaffCode);
            await DBHelper.ExecuteStoreProcedure("GSTBind", Param);
        }
        /// <summary>
        ///  This method use to Validation For new created batch Name.
        /// </summary>
        public async Task<bool> IsBatchAvilableAsyncST(string BatchName)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@flag", "IsBatchAvilableAsyncST");
                Param.Add("@BatchName", BatchName);
                DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTBind", Param);

                // Check if the DataSet has any rows and if the value in the 'IsAvailable' column is 1
                return ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && Convert.ToInt32(ds.Tables[0].Rows[0]["IsAvailable"]) == 1;
            }
            catch (Exception ex)
            {
                // Handle exceptions if needed
                Console.WriteLine("Error in IsBatchAvilableAsyncST: " + ex.Message);
                return false; // Return false in case of an error
            }
        }
        /// <summary>
        ///This method use to get batch data for update.
        /// </summary>
        /// <returns> string BatchCode </returns>
        public async Task<SqlDataReader> GetUpdateBatchAsyncST(Bind ObjCo)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "GetUpdateBatchAsyncST");
            Param.Add("@BatchCode", ObjCo.BatchCode);
            Param.Add("@BranchCode", ObjCo.BranchCode);
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTBind", Param);
            return dr;
        }

        /// <summary>
        /// This method use to save Updated Batch Data.
        /// </summary>
        /// <returns> </returns>
        public async Task UpdateBatchDataAsyncST(Bind ObjCo)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "UpdateBatchDataAsyncST");
            Param.Add("@BatchCode", ObjCo.BatchCode);
            Param.Add("@BatchName", ObjCo.BatchName);
            Param.Add("@StudentCode", ObjCo.StudentCode);
            Param.Add("@CourseCode", ObjCo.CourseCode);
            Param.Add("@NoOfStudent", ObjCo.NoOfStudent.ToString());
            Param.Add("@CreatedByStaffCode", ObjCo.StaffCode);
            await DBHelper.ExecuteStoreProcedure("GSTBind", Param);
        }

        /// <summary>
        ///This method use to get batch data for View Only.
        /// </summary>
        /// <returns> Batch Data</returns>
        public async Task<SqlDataReader> DetailsBatchAsyncST(Bind ObjCo)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "DetailsBatchAsyncST");
            Param.Add("@BatchCode", ObjCo.BatchCode);
            Param.Add("@BranchCode", ObjCo.BranchCode);
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTBind", Param);
            return dr;
        }

        /// <summary>
        ///Show batch student info for click on Number Of Student Count.
        /// </summary>
        /// <returns> student info </returns>
        public async Task<DataSet> GetBatchStudNameAsyncST(Bind ObjCo)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "GetBatchStudNameAsyncST");
            Param.Add("@BatchCode", ObjCo.BatchCode);
            Param.Add("@BranchCode", ObjCo.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTBind", Param);
            return ds;
        }

        //------------------------------------Marge Batch Task ST----------------------------------------
        /// <summary>
        ///This method use to save select marge batch student.
        /// </summary>
        public async Task MergeBatchStudentAsyncST(Bind ObjCo)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "MergeBatchStudentAsyncST");
            Param.Add("@BatchCode", ObjCo.SelectBatch);
            Param.Add("@StudentCode", ObjCo.SelectStudentCode);
            Param.Add("@NoofStudent", ObjCo.SelectNoOfStudent.ToString());
            await DBHelper.ExecuteStoreProcedure("GSTBind", Param);
        }

        /// <summary>
        ///This method use to save add marge batch student.
        /// </summary>
        public async Task MergeBatchStudent2AsyncST(Bind ObjCo)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "MergeBatchStudentAsyncST");
            Param.Add("@BatchCode", ObjCo.AddBatch);
            Param.Add("@StudentCode", ObjCo.AddStudentCode);
            Param.Add("@NoofStudent", ObjCo.AddNoOfStudent.ToString());
            await DBHelper.ExecuteStoreProcedure("GSTBind", Param);
        }

        /// <summary>
        ///This method use to get batch names To selected batches on marge student view. 
        /// </summary>
        /// <returns> Batch List</returns>
        public async Task<DataSet> GetSelectedBatchAsyncST()
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "GetSelectedBatchAsyncST");
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTBind", Param);
            return ds;
        }

        /// <summary>
        ///This method use to batch select validation  on marge student view.
        /// </summary>
        /// <returns> </returns>
        public async Task<DataSet> ValidationBatchAsyncST(Bind ObjCo)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "ValidationBatchAsyncST");
            Param.Add("@SelectBatchCode", ObjCo.SelectBatch);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTBind", Param);
            return ds;
        }

        /// <summary>
        /// This method use to get selected batch student names on marge student view.
        /// </summary>
        /// <returns> Student Name </returns>
        public async Task<DataSet> SelectBatchStudNameAsyncST(Bind ObjCo1)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "SelectBatchStudNameAsyncST");
            Param.Add("@BatchCode", ObjCo1.SelectBatch);
            Param.Add("@BranchCode", ObjCo1.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTBind", Param);
            return ds;
        }

        /// <summary>
        /// This method use to get Add batch student names on marge student view.
        /// </summary>
        /// <returns> Batch Name </returns>
        public async Task<DataSet> AddBatchStudNameAsyncST(Bind ObjCo1)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "SelectBatchStudNameAsyncST");
            Param.Add("@BatchCode", ObjCo1.AddBatch);
            Param.Add("@BranchCode", ObjCo1.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTBind", Param);
            return ds;
        }

        /// <summary>
        ///This method use to save marge batch to batch Student.
        /// </summary>
        public async Task MergeBatchToBatchStudentAsyncST(Bind ObjCo)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "MergeBatchToBatchStudentAsyncST");
            Param.Add("@AddBatchCode", ObjCo.AddBatch);
            Param.Add("@SelectBatchCode", ObjCo.SelectBatch);
            await DBHelper.ExecuteStoreProcedure("GSTBind", Param);
        }

        /// <summary>
        /// This method get Validation Batch student Count Lab Capacity .
        /// </summary>
        /// <returns> Batch list</returns>
        public async Task<DataSet> ValidationStudCountLabCapacityAsyncST(Bind ObjCo)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "ValidationStudCountLabCapacityAsyncST");
            Param.Add("@BatchCode", ObjCo.BatchCode);
            Param.Add("@BranchCode", ObjCo.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTBind", Param);
            return ds;
        }

        //-----------sayali Co-ordinator Trainer  Batch Management end ---------//
        //---------- shuhangi patil -----------//

        public async Task<DataSet> StudentListSHAsync(int StatusId, string Branchcode, string CourseCode)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "StudentListSH");
            Param.Add("@StatusId", StatusId.ToString());
            Param.Add("@BranchCode", Branchcode);
            Param.Add("@CourseCode", CourseCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTBind", Param);
            return ds;
        }
        /// <summary>
        /// Get the placed student list.
        /// </summary>
        /// <param name="StatusId">Object for the get placed student list.</param>
        /// <returns> List of placed students.</returns>
        public async Task<DataSet> PlacedStudentListSHAsync(int StatusId, string Branchcode, string CourseCode)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "ShowPlacedStudentSH");
            Param.Add("@StatusId", StatusId.ToString());
            Param.Add("@BranchCode", Branchcode);
            Param.Add("@CourseCode", CourseCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTBind", Param);
            return ds;
        }

        /// <summary>
        /// Get the data for the change the status.
        /// </summary>
        /// <param name="studcode"> Studcode for get specific student data. </param>
        /// <returns> It returns student details.</returns>
        public async Task<DataSet> StatusChangeDataSHAsync(string studcode)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "StatusChangeDataSH");
            Param.Add("@StudentCode", studcode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTBind", Param);
            return ds;
        }
        /// <summary>
        /// Update the staus of student.
        /// </summary>
        /// <param name="obj"> Object for update the student.</param>
        public async Task ChangeStatusSHAsync(Bind obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "ChangeStatusSH");
            Param.Add("@Statusid", obj.StatusId.ToString());
            Param.Add("@StudentCode", obj.StudentCode);
            await DBHelper.ExecuteStoreProcedure("GSTBind", Param);
        }
        /// <summary>
        /// Get the list of status foe change the status.
        /// </summary>
        /// <returns>It returns list of student.</returns> 
        public async Task<DataSet> StatusListSHAsync()
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "GetStatusSH");
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTBind", Param);
            return ds;
        }
        /// <summary>
        /// Get the data for the release student.
        /// </summary>
        /// <param name="studcode"> Studcode for get specific student data. </param>
        /// <returns> It returns student details for relese student.</returns>
        public async Task<DataSet> ReleaseStudentDetailsSHAsync(string studcode, string Branchcode)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "ReleaseStudentDetailsSH");
            Param.Add("@StudentCode", studcode);
            Param.Add("@BranchCode", Branchcode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTBind", Param);
            return ds;
        }
        public async Task ReleaseStudentSHAsync(Bind obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "ReleaseStudentSH");
            Param.Add("@StudentCode", obj.StudentCode);
            await DBHelper.ExecuteStoreProcedure("GSTBind", Param);
        }
        //---------- Suhangi patil end student list -----------------------------------------------------//

        //-------------pratiha   ----------------
        //==============================================================================================================================================================//
        //================================================== Task Management ====================================================//
        /// <summary>
        /// Retrieves a DataSet containing information about task management asynchronously.
        /// </summary>
        /// <returns>An asynchronous task representing the operation that yields a DataSet with task management information.</returns>
        /// <exception cref="Exception">Thrown if an error occurs during the retrieval process.</exception>
        public async Task<DataSet> ListTaskManagementAsyncPG(Bind objT, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@flag", "ListTaskManagementPG");
                Param.Add("@TaskAddedStaff", objT.StaffCode);
                Param.Add("@BranchCode", objT.BranchCode);
                // Add fromDate and toDate parameters if provided
                if (fromDate != null)
                    Param.Add("@FromDate", fromDate.Value.ToString("yyyy-MM-dd")); // Assuming your stored procedure accepts date in yyyy-MM-dd format
                if (toDate != null)
                    Param.Add("@ToDate", toDate.Value.ToString("yyyy-MM-dd")); // Assuming your stored procedure accepts date in yyyy-MM-dd format

                DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTBind", Param);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving assigned projects. Details: " + ex.Message);
            }
        }

        /// <summary>
        /// Registers a task for task management asynchronously.
        /// </summary>
        /// <param name="ObjT">The Trainer object containing task details for registration.</param>
        /// <returns>An asynchronous task representing the registration process.</returns>
        /// <exception cref="Exception">Thrown if an error occurs during the registration process.</exception>
        public async Task RegisterTaskManagementAsyncPG(Bind ObjT)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@flag", "RegisterTask");
                Param.Add("@TaskName", ObjT.TaskName);
                Param.Add("@AssignByStaffCode", ObjT.StaffCode);
                Param.Add("@AssignToStaffCode", ObjT.AssignToStaffCode);
                Param.Add("@Priority", ObjT.Priority);
                Param.Add("@TaskStartDate", ObjT.TaskStartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                Param.Add("@TaskEndDate", ObjT.TaskEndDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                Param.Add("@TaskStartTime", ObjT.TaskStartTime.ToString("HH:mm:ss"));
                Param.Add("@TaskEndTime", ObjT.TaskEndTime.ToString("HH:mm:ss"));
                Param.Add("@StaffPositionId", 3.ToString());
                Param.Add("@Description", ObjT.TaskDescription);
                Param.Add("@StatusId", 19.ToString());
                Param.Add("@TaskAddedStaff", ObjT.StaffCode);
                Param.Add("@TaskAddedDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                await DBHelper.ExecuteStoreProcedure("GSTBind", Param);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while registering the task. Details: " + ex.Message);
            }
        }
        /// <summary>
        /// Updates task management details asynchronously for a given Trainer object.
        /// </summary>
        /// <param name="ObjT">The Trainer object containing the updated task management details.</param>
        /// <exception cref="Exception">Thrown when an error occurs during the update process.</exception>
        public async Task UpdateTaskManagementAsyncPG(Bind ObjT)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@flag", "UpdateTaskManagement");
                Param.Add("@TaskId", ObjT.TaskId.ToString());
                Param.Add("@TaskCode", ObjT.TaskCode);
                Param.Add("@TaskName", ObjT.TaskName);
                if (ObjT.AssignByStaffCode == null)
                {
                    Param.Add("@AssignByStaffCode", ObjT.StaffCode);
                    Param.Add("@AssignToStaffCode", ObjT.AssignToStaffCode);
                }
                else
                {
                    Param.Add("@AssignByStaffCode", ObjT.AssignByStaffCode);
                    Param.Add("@AssignToStaffCode", ObjT.StaffCode);
                }
                Param.Add("@Priority", ObjT.Priority);
                Param.Add("@TaskStartDate", ObjT.TaskStartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                Param.Add("@TaskEndDate", ObjT.TaskEndDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                Param.Add("@TaskStartTime", ObjT.TaskStartTime.ToString("HH:mm:ss"));
                Param.Add("@TaskEndTime", ObjT.TaskEndTime.ToString("HH:mm:ss"));
                Param.Add("@StaffPositionId", 2.ToString());
                Param.Add("@StatusId", ObjT.StatusId.ToString());
                Param.Add("@Description", ObjT.TaskDescription);

                await DBHelper.ExecuteStoreProcedure("GSTBind", Param);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the assigned project. Details: " + ex.Message);
            }
        }
        /// <summary>
        /// Updates the status for an assigned task for a trainer.
        /// </summary>
        /// <param name="ObjT">The Trainer object containing information about the task to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during the update process.</exception>
        public async Task UpdateStatusforAssignTask(Bind ObjT)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@flag", "UpdateStatusforAssignTask");
                Param.Add("@TaskId", ObjT.TaskId.ToString());
                Param.Add("@StatusId", ObjT.StatusId.ToString());
                await DBHelper.ExecuteStoreProcedure("GSTBind", Param);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the assigned project. Details: " + ex.Message);
            }
        }
        /// <summary>
        /// Fetches a dataset containing the reporting staff names for task assignment based on the assigning staff code and branch code.
        /// </summary>
        /// <param name="objT">An instance of the Trainer class containing information about the assigning staff and branch.</param>
        /// <returns>Returns a dataset containing the reporting staff names.</returns>
        /// <exception cref="Exception">Thrown when an error occurs while fetching the reporting staff names.</exception>
        public async Task<DataSet> AssignTaskRepotingStaffName(Bind objT)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@flag", "AssignTaskReportingStaffName");
                Param.Add("@ReportingToStaff", objT.StaffCode);
                Param.Add("@BranchCode", objT.BranchCode.ToString());
                DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTBind", Param);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching course names. Details: " + ex.Message);
            }
        }
        /// <summary>
        /// Retrieves staff names for a task based on the provided staff code.
        /// </summary>
        /// <param name="objT">An instance of the Trainer class containing the staff code.</param>
        /// <returns>Returns a dataset containing staff names.</returns>
        /// <exception cref="Exception">Thrown when an error occurs while retrieving staff names.</exception>
        public async Task<DataSet> StaffNameforTask(Bind objT)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@flag", "StaffName");
                Param.Add("@StaffCode", objT.StaffCode);
                Param.Add("@BranchCode", objT.BranchCode.ToString());
                DataSet ds1 = await DBHelper.ExecuteStoreProcedureReturnDS("GSTBind", Param);
                return ds1;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving assigned projects. Details: " + ex.Message);
            }
        }
        /// <summary>
        /// Fetches task management details asynchronously for a given Trainer object.
        /// </summary>
        /// <param name="objT">The Trainer object containing the TaskId for which details are to be fetched.</param>
        /// <returns>A SqlDataReader containing the fetched task management details.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during the data fetching process.</exception>
        public async Task<SqlDataReader> FetchTaskManagementAsyncPG(Bind objT)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@flag", "FetchTaskManagementDetails");
                Param.Add("@TaskId", objT.TaskId.ToString());
                Param.Add("@BranchCode", objT.BranchCode.ToString());
                SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTBind", Param);
                return dr;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching assigned project details. Details: " + ex.Message);
            }
        }
        /// <summary>
        /// Removes task management details asynchronously for a given Trainer object.
        /// </summary>
        /// <param name="objT">The Trainer object containing the TaskId of the task to be removed.</param>
        /// <exception cref="Exception">Thrown when an error occurs during the removal process.</exception>
        public async Task RemoveDataTaskManagement(Bind objT)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@flag", "RemoveDataTaskManagement");
                Param.Add("@TaskId", objT.TaskId.ToString());
                await DBHelper.ExecuteStoreProcedure("GSTBind", Param);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while removing task. Details: " + ex.Message);
            }
        }
        /// <summary>
        /// Retrieves the status of a task assigned to a trainer.
        /// </summary>
        /// <param name="objT">The Trainer object containing information to identify the task.</param>
        /// <returns>A DataSet containing the status information for the task.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during the retrieval process.</exception>
        public async Task<DataSet> FetchStatusForTask(Bind objT)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@flag", "FetchStatusNameforTask");
                Param.Add("@BranchCode", objT.BranchCode.ToString());
                DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTBind", Param);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving assigned projects. Details: " + ex.Message);
            }
        }

        /// <summary>
        /// Retrieves the status of a task assigned to a trainer.
        /// </summary>
        /// <param name="objT">The Trainer object containing information to identify the task.</param>
        /// <returns>A DataSet containing the status information for the task.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during the retrieval process.</exception>
        public async Task<DataSet> FetchStatusForUpdateTask(Bind objT)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@flag", "FetchStatusNameforUpdateTask");
                Param.Add("@BranchCode", objT.BranchCode.ToString());
                DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTBind", Param);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving assigned projects. Details: " + ex.Message);
            }
        }

        //-------------pratiha end   ----------------
        //-------------------------------------------Punam Contact,SMS,Email-------------------------------
        /// <summary>
        /// THIS METHOD IS USE TO GET COURCE NAME.
        /// </summary>
        /// <returns>GET COURCE NAME.</returns>
        public async Task<DataSet> GetCourceNameAsyncPB(Bind objTrainer)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "GetCourse");
            Param.Add("@BranchCode", objTrainer.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTBind", Param);
            return ds;
        }
        /// <summary>
        /// THIS FUNCTION IS USE TO GET CONTACT LIST.
        /// </summary>
        /// <returns> GSTTBLCONTACT TABLE DATA.</returns>
        public async Task<DataSet> GetContactListPB()
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "GetContactDetailsPB");
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTBind", Param);
            return ds;
        }
        public async Task<DataSet> GetFilterTypePB()
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "GetContactTypePB");
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTBind", Param);
            return ds;
        }
        /// <summary>
        /// THIS METHOD IS USE TO SAVE CONTACT DETAILS.
        /// </summary>
        /// <returns>SAVE CONTACT DETAILS IN GSTTBLCONTACT.</returns>
        public async Task SaveContactAsyncPB(Bind objTrainer)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "SaveContactPB");
            Param.Add("@Typeid", objTrainer.TypeId.ToString());
            Param.Add("@Name", objTrainer.FullName);
            Param.Add("@Contact", objTrainer.ContactNumber);
            Param.Add("@EmailId", objTrainer.EmailId);
            Param.Add("@PresentAddress", objTrainer.Address);
            Param.Add("@PresentCityid", objTrainer.City);
            Param.Add("@IsDeleted", 0.ToString());
            await DBHelper.ExecuteStoreProcedure("GSTBind", Param);
        }
        /// <summary>
        /// THIS METHIS IS USE TO GET CONTACT DETAILS USING FILTER TYPE.
        /// </summary>
        /// <returns> FILTER DATA ,</returns>
        public async Task<DataSet> FilterTypeAsyncPB(int TypeId)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "GetFilterDataPB");
            Param.Add("@Typeid", TypeId.ToString());
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTBind", Param);
            return ds;
        }
        /// <summary>
        /// THIS METHOD IS USE TO ADD CONTACT TYPE.
        /// </summary>
        /// <returns>ADD TYPE IN GSTTBLCONTACTTYPE.</returns>
        public async Task SaveAddedTypeAsyncPB(string AddedType)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "SaveContactType");
            Param.Add("@Type", AddedType);
            await DBHelper.ExecuteStoreProcedure("GSTBind", Param);
        }
        /// <summary>
        /// THIS METHOD IS USE TO GET CONTACT TYPE FOR VALIDATION.
        /// </summary>
        /// <param name="AddType"></param>
        /// THIS OBJECT IS USE TO GET TYPE.
        /// <returns>GET CONTACT TYPE NAME.</returns>
        public async Task<DataSet> GetContactTypeForValidaAsyncPB(string AddType)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "GetContactTypeForValidation");
            Param.Add("@Type", AddType);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTBind", Param);
            return ds;
        }
        /// <summary>
        /// THIS METHOD IS USE TO GET CONTAT FOR EDIT.
        /// </summary>
        /// <param name="objTrainer"></param>
        /// THIS OBJECT IS USE TO PASS THE VALUE.
        /// <returns>GET CONTACT</returns>
        public async Task<DataSet> GetContactForEditAsyncPB(Bind objTrainer)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "GetContactForEdit");
            Param.Add("@Id", objTrainer.Id.ToString());
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTBind", Param);
            return ds;
        }
        /// <summary>
        /// THIS METHOD IS USE TO UPDATE CONTACT.
        /// </summary>
        /// <returns>UPDATE CONTACT.</returns>
        public async Task UpdateContactAsyncPB(Bind objTrainer)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "UpdateContact");
            Param.Add("@Typeid", objTrainer.TypeId.ToString());
            Param.Add("@Name", objTrainer.FullName);
            Param.Add("@Contact", objTrainer.ContactNumber);
            Param.Add("@EmailId", objTrainer.EmailId);
            Param.Add("@PresentCityid", objTrainer.City);
            Param.Add("@PresentAddress", objTrainer.Address);
            Param.Add("@Id", objTrainer.Id.ToString());
            await DBHelper.ExecuteStoreProcedure("GSTBind", Param);
        }
        /// <summary>
        /// THIS METHOD IS USE TO GET CONTACT DETAILS.
        /// </summary>
        /// <param name="id"></param>
        /// THIS OBJECT IS USE TO GET ID.
        /// <returns>GET CONTACT DETAILS.</returns>
        public async Task<DataSet> GetDetailsAsyncPB(int id)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "DetailsContact");
            Param.Add("@Id", id.ToString());
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTBind", Param);
            return ds;
        }
        /// <summary>
        /// THIS METHOD IS USE TO DELETE CONTACT DETAILS.
        /// </summary>
        /// <param name="Id"></param>
        /// THIS OBJECT IS USE TO GET CONTACT ID AND DELETE CONTACT.
        /// <returns>DELETE CONTACT.</returns>
        public async Task DeleteContactAsyncPB(Bind objTrainer)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "DeleteContact");
            Param.Add("@Id", objTrainer.DeleteId);
            Param.Add("@IsDeleted", 1.ToString());
            await DBHelper.ExecuteStoreProcedure("GSTBind", Param);
        }
        /// <summary>
        /// THIS METHOD IS USE TO SEND MAIL.
        /// </summary>
        /// <param name="SenderId"></param>
        /// <param name="Subject"></param>
        /// <param name="Msg"></param>
        /// <param name="FileUploader"></param>
        /// THIS OBJECT IS USED TO GET VALUE ON VIEW AND PASS THE VALUE.
        /// <returns> PASS VALUE TO HELPER CLASS.</returns>
        public async Task SendMailPB(string SenderId, string Subject, string Msg, HttpPostedFileBase FileUploader)
        {
            DBHelper.SendMailPB(SenderId, Subject, Msg, FileUploader);
        }
        /// <summary>
        /// THIS METHOD IS USE TO GET EMAIL TEMPLATE NAME FOR SEND EMAILS.
        /// </summary>
        /// <returns> EMAIL TEMPALTE NAME AND MAILID.</returns>
        public async Task<DataSet> GetEmailTemplatePB()
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "GetEmailTempleName");
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTBind", Param);
            return ds;
        }
        /// <summary>
        /// THIS METHOD IS USE TO GET ALL DETAILS FOR FETCH DATA.
        /// </summary>
        /// <returns> GET ALL DETAIS OF TEMPLATE.</returns>
        public async Task<DataSet> GetAllDetailsofTemplatePB(string TemplateId)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "GetTemplateData");
            Param.Add("@TemplateId", TemplateId);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTBind", Param);
            return ds;
        }
        /// <summary>
        /// THIS METHOD IS USE FOR SEARCH BY BATCH NAME.
        /// </summary>
        /// <param name="search"></param>
        /// <returns>RETURN FILTER GET DATA.</returns>
        public async Task<DataSet> GetFilterDataContactPB(string search, string brabchcode)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "BatchFilter");
            Param.Add("@Search", search);
            Param.Add("@BranchCode", brabchcode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTBind", Param);
            return ds;
        }
        public async Task<DataSet> GetAllBatchNameAsyncPB(string CourceCode)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "GetAllBatchName");
            Param.Add("@CourceCode", CourceCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTBind", Param);
            return ds;
        }
        /// <summary>
        /// THIS FUNCTION IS USE TO GET TRAINER DATA CONTACT AND EMAIL, ADDRESS.
        /// </summary>
        /// <param name="BranchCode"></param>
        /// THIS ONJECT IS USE TO GET BRANCH CODE.
        /// <returns>THIS METHOD S USE TO GET TRAINER DATA IN LIST.</returns>

        public async Task<DataSet> GetAllBatchesPB(Bind objTrainer)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "GetAllBatchName");
            Param.Add("@BranchCode", objTrainer.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTBind", Param);
            return ds;
        }
        /// <summary>
        /// THIS METHOD IS USE TO CREATE QUERY FOR GET EMAILID IN GSTTBLBATCH AND ENQUIRY .
        /// </summary>
        /// <param name="batch"></param>
        /// THIS OBJECT IS USE TO SELECTED BATCH NAME GET AND GET EMAILIS STUDENTS.
        public async Task<DataSet> GetBachesEmailPB(string[] batch, string BranchCode)
        {
            string batchValues = string.Join(",", batch.Select(b => $"'{b.Trim()}'"));
            Dictionary<string, string> Param = new Dictionary<string, string>();

            string Query = "SELECT EmailId FROM GSTtblBatch B INNER JOIN GSTtblEnquiry E ON CHARINDEX(',' + E.CandidateCode + ',', ',' + B.CandidateCode + ',') > 0 where BranchCode = '" + BranchCode + "' and BatchCode in (" + batchValues + ") and B.Statusid = '" + 4 + "'";

            DataSet ds = DBHelper.ExecuteQueryReturnDs(Query, Param);
            return await Task.Run(() => ds);
        }
        /// <summary>
        /// THIS METHOD ID USE TO GET STAFF EMAIL ID IN GSTTBLSTAFF,AND IN THIS METHOD CREATE QUERY AND EXECUTEQUERY.
        /// </summary>
        /// <param name="GroupId"></param>
        /// THIS OBJECT IS USE TO GET POSITIONID AND POSITION ID WISE GET EMAILID.
        /// <returns>GET STAFF EMAIL.</returns>
        public DataSet GetEmailIdSelectedGroupPB(string GroupId, string BranchCode)
        {
            string Query = "SELECT OfficialEmailId as EmailId FROM GSTtblStaff WHERE BranchCode = '" + BranchCode + "' and StaffPositionId IN (" + GroupId + ")";
            Dictionary<string, string> Param = new Dictionary<string, string>();
            DataSet ds = DBHelper.ExecuteQueryReturnDs(Query, Param);
            return ds;
        }
        /// <summary>
        /// THIS METHOD IS USE TO GET STUDENT EMAILID CREATING QUERY AND USING EXECUTEQUERY.
        /// </summary>
        /// <param name="BranchCode"></param>
        /// THIS OBJECT IS USE TO GET BRANCHCODE AND BRANCHCOE WISE GET STUDENT EMAILID.
        /// <returns>GET EMAILID.</returns>
        public DataSet GetGroupStudentSelectedPB(string BranchCode)
        {
            string Query = "SELECT EmailId FROM GSTtblBatch B INNER JOIN GSTtblEnquiry E ON CHARINDEX(',' + E.CandidateCode + ',', ',' + B.CandidateCode + ',') > 0 where BranchCode = '" + BranchCode + "'and B.Statusid ='" + 4 + "'";
            Dictionary<string, string> Param = new Dictionary<string, string>();
            DataSet ds = DBHelper.ExecuteQueryReturnDs(Query, Param);
            return ds;
        }
        /// <summary>
        /// THIS METHOD IS USE TO GET BIRTHDAY AND GET EMAIL. 
        /// </summary>
        /// <param name="Todaysdate"></param>
        /// <param name="BranchCode"></param>
        /// THIS OBJECT IS USE TO GET BRANCHCODE NAD TODAYS DATE.
        /// <returns>GET EMAIL AND BIRTHDAY.</returns>
        public async Task<DataSet> GetBirthdaydate(string Todaysdate, string BranchCode)
        {
            string Query = "select StaffName,CONVERT(varchar,DOB,23) as DOB,Email from GSTtblStaff where BranchCode = '" + BranchCode + "' and DOB like '%" + Todaysdate + "%' ";
            Dictionary<string, string> Param = new Dictionary<string, string>();
            DataSet ds = DBHelper.ExecuteQueryReturnDs(Query, Param);
            return await Task.Run(() => ds);
        }
        /// <summary>
        /// THIS METHOD IS USE TO SAVE SEND EMAIL .
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Branchcode"></param>
        /// <param name="StaffCode"></param>
        /// THIS OBJECT IS USE TO GET VALUE .
        /// <returns>SAVE SEND EMAIL.</returns>
        public async Task SaveSendEmailPB(int Email, string IndividualTo, string StaffCode, string Branchcode)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "InsertSendEmail");
            Param.Add("@SendingGroupCount", Email.ToString());
            //Param.Add("@SendingEmailStudent", Email);
            Param.Add("@EmailId", IndividualTo);
            Param.Add("@StaffCode", StaffCode);
            Param.Add("@BatchCode", Branchcode);
            await DBHelper.ExecuteStoreProcedure("GSTBind", Param);
        }
        /// <summary>
        /// THIS METHOD IS USE TO GET GROUP WISE STAFF PHONE NUMBER.
        /// </summary>
        /// <param name="GroupId"></param>
        /// <param name="BranchCode"></param>
        /// THIS ONJECT IS USE TO GET SELECTED STAFF ID AND BRANCHCODE.
        /// <returns>GET PHONE NUMBERS.</returns>
        public DataSet GetGroupSmsAsyncPB(string GroupId, string BranchCode)
        {
            string Query = "SELECT PhoneNo FROM GSTtblStaff WHERE BranchCode = '" + BranchCode + "' and StaffPositionId IN (" + GroupId + ")";
            Dictionary<string, string> Param = new Dictionary<string, string>();
            DataSet ds = DBHelper.ExecuteQueryReturnDs(Query, Param);
            return ds;
        }
        /// <summary>
        /// THIS OBJECT IS USE TO GET GROUP STUDENT PHONE NUMBERS.
        /// </summary>
        /// <param name="BranchCode"></param>
        /// THIS OBJECT IS USE TO GET BRANCH CODE.
        /// <returns>GET ALL STUDENT PHONE NUMBER.</returns>
        public DataSet GetGroupStudentAsyncPB(string BranchCode)
        {
            string Query = "SELECT ContactNumber as PhoneNo FROM GSTtblBatch B INNER JOIN GSTtblEnquiry E ON CHARINDEX(',' + E.CandidateCode + ',', ',' + B.CandidateCode + ',') > 0 where BranchCode = '" + BranchCode + "'and B.Statusid ='" + 4 + "'";
            Dictionary<string, string> Param = new Dictionary<string, string>();
            DataSet ds = DBHelper.ExecuteQueryReturnDs(Query, Param);
            return ds;
        }
        /// <summary>
        /// THIS METHOD IS USE TO SELECTED BATCHES WISE PHONE NUMBER.
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="BranchCode"></param>
        /// THIS OBJECT IS USE TO GET BRANCH CODE AND BATCHECODE.
        /// <returns>GET BATCHES STUDENTS CONTACT NUMBERS.</returns>
        public async Task<DataSet> GetBatchesAsyncPB(string[] batch, string BranchCode)
        {
            string batchValues = string.Join(",", batch.Select(b => $"'{b.Trim()}'"));
            Dictionary<string, string> Param = new Dictionary<string, string>();

            string Query = "SELECT EmailId FROM GSTtblBatch B INNER JOIN GSTtblEnquiry E ON CHARINDEX(',' + E.CandidateCode + ',', ',' + B.CandidateCode + ',') > 0 where BranchCode = '" + BranchCode + "'and b.statusid ='" + 4 + "' and BatchCode in(" + batchValues + ")";
            DataSet ds = DBHelper.ExecuteQueryReturnDs(Query, Param);
            return await Task.Run(() => ds);
        }
        /// <summary>
        /// THIS METHOD IS USE TO GET CONTACT NAME AND NUMER.
        /// </summary>
        /// <param name="Todaysdate"></param>
        /// <param name="BranchCode"></param>
        /// THIS OBJECT IS USE TO GET VALUE.
        /// <returns>GET CONTACT NUMBER AND NAME.</returns>
        public async Task<DataSet> GetonNumberBirthdauAsyncPB(string Todaysdate, string BranchCode)
        {
            string Query = "select StaffName,CONVERT(varchar,DOB,23) as DOB,Email from GSTtblStaff where BranchCode = '" + BranchCode + "' and DOB like '%" + Todaysdate + "%' ";
            Dictionary<string, string> Param = new Dictionary<string, string>();
            DataSet ds = DBHelper.ExecuteQueryReturnDs(Query, Param);
            return await Task.Run(() => ds);
        }
        /// <summary>
        /// THIS METHOD IS USE TO GET COUNTRY.
        /// </summary>
        /// <returns> GET COUNTRY.</returns>
        public async Task<DataSet> GetCountry()
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "GetCountry");
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTBind", Param);
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
            dynamicDS = await DBHelper.ExecuteStoreProcedureReturnDS("GSTBind", Param);
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
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTBind", Param);
            return ds;
        }
        /// <summary>
        /// THIS METHOD IS USE TO GET BIRTHDAY AND GET Email. 
        /// </summary>
        /// <param name="Todaysdate"></param>
        /// <param name="BranchCode"></param>
        /// THIS OBJECT IS USE TO GET BRANCHCODE NAD TODAYS DATE.
        /// <returns>GET NUMBER.</returns>
        public async Task<DataSet> GetBirthdaydateAndNoPB(string Todaysdate, string BranchCode)
        {
            string Query = "select StaffName,CONVERT(varchar,DOB,23) as DOB,PhoneNo from GSTtblStaff where BranchCode = '" + BranchCode + "' and DOB like '%" + Todaysdate + "%' ";
            Dictionary<string, string> Param = new Dictionary<string, string>();
            DataSet ds = DBHelper.ExecuteQueryReturnDs(Query, Param);
            return await Task.Run(() => ds);
        }
    }
}
