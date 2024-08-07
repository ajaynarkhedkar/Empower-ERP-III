using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Helper;

namespace GSTEducationERPLibrary.Coordinator
{
    public class BALCoordinator
    {
        MSSQL DBHelper = new MSSQL();

        public async Task<DataSet> AllCourseBind(Coordinator ObjCo)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "AllCourseBind");
            Param.Add("@BranchCode", ObjCo.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        //-----------------------------------vaibhav Coordinator Test Management --------------------------------
        /// <summary>
        /// used for shoing list of assign sedule 
        /// </summary>
        /// <returns></returns>
        public async Task<DataSet> ArrangeTestList(Coordinator obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ArrangeTestList");
            Param.Add("@CourceCode", obj.CourseCode);
            Param.Add("@BranchCode", obj.BranchCode);
            Param.Add("@StartDate", obj.StartDate.ToString("yyyy-MM-dd"));
            Param.Add("@EndDate", obj.EndDate.ToString("yyyy-MM-dd"));
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// used for shoing list of conduct sedule 
        /// </summary>
        /// <returns></returns>
        public async Task<DataSet> ConductedTestList(Coordinator obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ConductedTestList");
            Param.Add("@CourceCode", obj.CourseCode);
            Param.Add("@BranchCode", obj.BranchCode);
            Param.Add("@StartDate", obj.StartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            Param.Add("@EndDate", obj.EndDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// used for shoing list of pending  sedule 
        /// </summary>
        /// <returns></returns>
        public async Task<DataSet> PendingTestList(Coordinator obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "PendingTestList");
            Param.Add("@CourceCode", obj.CourseCode);
            Param.Add("@BranchCode", obj.BranchCode);
            Param.Add("@StartDate", obj.StartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            Param.Add("@EndDate", obj.EndDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// used for shoing list of assign sedule list
        /// </summary>
        /// <returns></returns>
        public async Task<DataSet> AssignTestList(Coordinator obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "AssignTestList");
            Param.Add("@CourceCode", obj.CourseCode);
            Param.Add("@BranchCode", obj.BranchCode);
            Param.Add("@StartDate", obj.StartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            Param.Add("@EndDate", obj.EndDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// read arrange test details 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<SqlDataReader> ReadArrageTestVp(Coordinator obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ReadArrageTestVp");
            Param.Add("@AssignTestId ", obj.AssignTestId.ToString());
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTCoordinator", Param);
            return dr;

        }
        /// <summary>
        /// used for save arrange test 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task SaveArrangeTest(Coordinator obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ArrageTestSave");
            Param.Add("@StartDate", obj.StartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            Param.Add("@StartT", obj.StartTime.ToString("HH:mm:ss.fffffff"));
            Param.Add("@TestTime", obj.StartTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            Param.Add("@TrainerCodeSupervisorCode", obj.SupervisorCode);
            Param.Add("@LabCode", obj.LabCode.ToString());
            Param.Add("@ArrangedDateSystemDate", obj.ArrangedDate.ToString("yyyy-MM-dd"));
            Param.Add("@StaffCodeArrangedBy", obj.StaffCode);
            Param.Add("@AssignTestId", obj.AssignTestId.ToString());
            Param.Add("@EndDate", obj.StartDate.ToString("yyyy-MM-dd"));
            Param.Add("@EndT", obj.EndTime.ToString("HH:mm:ss.fffffff"));

            await DBHelper.ExecuteStoreProcedure("GSTCoordinator", Param);
        }
        /// <summary>
        /// Read all supervisor code 
        /// </summary>
        /// <returns></returns>
        public async Task<DataSet> Readsupervisers(Coordinator obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "Readsupervisers");
            Param.Add("@BranchCode", obj.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// Read all supervisor code 
        /// </summary>
        /// <returns></returns>
        public async Task<DataSet> AllTestStausVp(Coordinator obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "AllTestStausVp");
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// Read available labs details 
        /// </summary>
        /// <returns></returns>
        public async Task<DataSet> ReadAvailableLabs(Coordinator obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ReadAvailableLabs");
            Param.Add("@StartDate", obj.StartDate.ToString("yyyy-MM-dd"));
            Param.Add("@EndDate", obj.StartDate.ToString("yyyy-MM-dd"));
            Param.Add("@StartT", obj.StartTime.ToString("HH:mm:ss.fffffff"));
            Param.Add("@EndT", obj.EndTime.ToString("HH:mm:ss.fffffff"));
            Param.Add("@BatchCode", obj.BatchCode);
            Param.Add("@BranchCode", obj.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// use for read aarange test details 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<SqlDataReader> ReadArrangeTestDetails(Coordinator obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ReadArrangeTestDetails");
            Param.Add("@AssignTestId", obj.AssignTestId.ToString());
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTCoordinator", Param);
            return dr;
        }
        /// <summary>
        /// use for arrange test update
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task ArrageTestupdate(Coordinator obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ArrageTestupdate");
            Param.Add("@TestDate", obj.StartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            Param.Add("@TestTime", obj.StartTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            Param.Add("@TrainerCodeSupervisorCode", obj.SupervisorCode);
            Param.Add("@Statusid", obj.StatusId.ToString());
            Param.Add("@LabCode", obj.LabCode.ToString());
            Param.Add("@AssignTestId", obj.AssignTestId.ToString());
            Param.Add("@StartDate", obj.StartDate.ToString("yyyy-MM-dd"));
            Param.Add("@EndDate", obj.StartDate.ToString("yyyy-MM-dd"));
            Param.Add("@StartT", obj.StartTime.ToString("HH:mm:ss.fffffff"));
            Param.Add("@EndT", obj.EndTime.ToString("HH:mm:ss.fffffff"));



            await DBHelper.ExecuteStoreProcedure("GSTCoordinator", Param);
        }
        public async Task ArrageTestStatusupdate(Coordinator obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ArrageTestStatusupdate");
            Param.Add("@TrainerCodeSupervisorCode", obj.SupervisorCode);
            Param.Add("@Statusid", obj.StatusId.ToString());
            Param.Add("@AssignTestId", obj.AssignTestId.ToString());
            await DBHelper.ExecuteStoreProcedure("GSTCoordinator", Param);
        }

        //-------------------------------------------------vaibhav Coordinator TEST managemrnt End  ------------ 
        //----------------------------------------------------Pratiksha (Dashboard)-----------------------------------------------
        /// <summary>
        /// This is CountTotalStudent function used for count the total students.
        /// </summary>
        /// <returns>This function returns the count of total student.</returns>
        public async Task<SqlDataReader> CountTotalStudentPRAsync(Coordinator objB)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "CountTotalStudent");
            Param.Add("@BranchCode", objB.BranchCode);
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTCoordinator", Param);
            return dr;
        }
        /// <summary>
        ///  This is CountTotalBatches function used for count the total batches.
        /// </summary>
        /// <returns>This function returns the count of total batches.</returns>
        public async Task<SqlDataReader> CountTotalBatchesPRAsync(Coordinator objB)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "CountTotalBatches");
            Param.Add("@BranchCode", objB.BranchCode);
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTCoordinator", Param);
            return dr;
        }
        /// <summary>
        /// This is CountActiveBatches function used for count the active batches.
        /// </summary>
        /// <returns>This function returns the count of active batches.</returns>
        public async Task<SqlDataReader> CountActiveBatchesPRAsync(Coordinator objB)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "CountActiveBatches");
            Param.Add("@BranchCode", objB.BranchCode);
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTCoordinator", Param);
            return dr;
        }
        /// <summary>
        /// This is CountReleasedBatches function used for count the released batches.
        /// </summary>
        /// <returns>This function returns the count of released batches.</returns>
        public async Task<SqlDataReader> CountReleasedBatchesPRAsync(Coordinator objB)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "CountReleasedBatches");
            Param.Add("@BranchCode", objB.BranchCode);
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTCoordinator", Param);
            return dr;
        }
        /// <summary>
        /// This is CountTotalCourses function used for count the total courses.
        /// </summary>
        /// <returns>This function returns the count of total courses.</returns>
        public async Task<SqlDataReader> CountTotalCoursesPRAsync(Coordinator objB)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "CountTotalCourses");
            Param.Add("@BranchCode", objB.BranchCode);
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTCoordinator", Param);
            return dr;
        }
        /// <summary>
        /// This is CountTotalLab function used for count the total lab.
        /// </summary>
        /// <returns>This function returns the count of total lab.</returns>
        public async Task<SqlDataReader> CountTotalLabPRAsync(Coordinator objB)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "CountTotalLab");
            Param.Add("@BranchCode", objB.BranchCode);
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTCoordinator", Param);
            return dr;
        }

        /// <summary>
        /// This is CountEvents function used for count the events.
        /// </summary>
        /// <returns>This function returns the count of events.</returns>
        public async Task<SqlDataReader> CountEventsPRAsync(Coordinator objB)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "CountEvents");
            Param.Add("@BranchCode", objB.BranchCode);
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTCoordinator", Param);
            return dr;
        }

        /// <summary>
        /// This is GraphActiveBatch function used for show the active batches student count in graph format.
        /// </summary>
        /// <returns>This function returns the count of active batches student.</returns>
        public async Task<DataSet> GraphActiveBatchPRAsync(Coordinator objB)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "GraphActiveBatch");
            Param.Add("@BranchCode", objB.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        ///  This is CountFeesPRAsync function used for count of the paid,unpaid and partial fees of student.
        /// </summary>
        /// <returns>This function returns the count of the fees of student.</returns>
        public async Task<SqlDataReader> CountFeesPRAsync(Coordinator objB)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "CountFees");
            Param.Add("@BranchCode", objB.BranchCode);
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTCoordinator", Param);
            return dr;
        }


        //-----------------------pratiksha dashboard end --------------------------------------------------
        //--------------------Sayli Batch Schedule start -----------------------------------------
        //----------------------------------------BatchScheduled Task ST--------------------------------
        //--------------------Sayli Batch Schedule start -----------------------------------------
        //----------------------------------------BatchScheduled Task ST--------------------------------
        /// <summary>
        /// This method get all Cource data  on list.
        /// </summary>
        /// <returns> Cource list</returns>
        public async Task<DataSet> GetCourceAsyncST(Coordinator ObjCo)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "GetCourceAsyncST");
            Param.Add("@BranchCode", ObjCo.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTBind", Param);
            return ds;
        }
        /// <summary>
        /// This method get all Scheduled batches data  on list.
        /// </summary>
        /// <returns> Batch List</returns>
        public async Task<DataSet> ListScheduledBatchAsyncST(Coordinator ObjCo)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "ListScheduledBatchAsyncST");
            Param.Add("@BranchCode", ObjCo.BranchCode);
            Param.Add("@CourseCode", ObjCo.CourseCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }

        /// <summary>
        ///  This method use to save new created batch Schedule.
        /// </summary>
        public async Task BatchScheduleAsyncST(Coordinator ObjCo)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            //------------------BatchSchedule Parameters---------------------------
            Param.Add("@Flag", "AddBatchScheduleAsyncST");
            Param.Add("@BatchCode", ObjCo.BatchCode);
            Param.Add("@TrainerCodeStaffCode", ObjCo.StaffName);
            Param.Add("@LabCode", ObjCo.LabName);
            Param.Add("@ScheduleStartDate", ObjCo.StartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            Param.Add("@BatchScheduleDate", ObjCo.BatchScheduleDate.ToString("yyyy-MM-dd HH:mm:ss"));
            Param.Add("@ScheduledAddedByStaffCode", ObjCo.StaffCode);
            //------------------LabSchedule Parameters---------------------------
            Param.Add("@StartTime", ObjCo.StartTime.ToString("HH:mm:ss"));
            Param.Add("@EndTime", ObjCo.EndTime.ToString("HH:mm:ss"));
            Param.Add("@StartDate", ObjCo.StartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            Param.Add("@EndDate", ObjCo.EndDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));

            await DBHelper.ExecuteStoreProcedure("GSTCoordinator", Param);
        }
        /// <summary>
        /// This method get New Created Batch.
        /// </summary>
        /// <returns> Batch list</returns>
        public async Task<DataSet> GetBatchAsyncST()
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "GetBatchAsyncST");
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// This method get CourseDuration On Creted new batch Schedule.
        /// </summary>
        /// <returns> Batch list</returns>
        public async Task<DataSet> GetCourceDurationST(Coordinator ObjCo)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "GetCourceDurationST");
            Param.Add("@BatchCode", ObjCo.BatchCode);
            Param.Add("@BranchCode", ObjCo.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// This method get all Pendding batches data  on list.
        /// </summary>
        /// <returns> Batch List</returns>
        public async Task<DataSet> ListPenddingBatchAsyncST(Coordinator ObjCo)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "ListPenddingBatchAsyncST");
            Param.Add("@BranchCode", ObjCo.BranchCode);
            Param.Add("@CourseCode", ObjCo.CourseCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// This method get all Assign batches data  on list.
        /// </summary>
        /// <returns> Batch List</returns>
        public async Task<DataSet> ListAssignBatchAsyncST(Coordinator ObjCo)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "ListAssignBatchAsyncST");
            Param.Add("@BranchCode", ObjCo.BranchCode);
            Param.Add("@CourseCode", ObjCo.CourseCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        ///This method use to get batch Schedule data for View Only and Update.
        /// </summary>
        /// <returns> Batch Schedule Data</returns>
        public async Task<SqlDataReader> DetailsBatchScheduleAsyncST(Coordinator ObjCo)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "DetailsBatchScheduleAsyncST");
            Param.Add("@ScheduleId", ObjCo.ScheduleId.ToString());
            Param.Add("@BranchCode", ObjCo.BranchCode);
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTCoordinator", Param);
            return dr;
        }

        public async Task<SqlDataReader> GetBatchScheduleDetailsUpdateAsyncST(Coordinator ObjCo)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "GetBatchScheduleDetailsUpdateAsyncST");
            Param.Add("@ScheduleId", ObjCo.ScheduleId.ToString());
            Param.Add("@BranchCode", ObjCo.BranchCode);
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTCoordinator", Param);
            return dr;
        }
        /// <summary>
        /// This view use to save Updated Batch Schedule Data.
        /// </summary>
        /// <returns> </returns>
        public async Task UpdateBatchScheduleAsyncST(Coordinator ObjCo)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "UpdateBatchScheduleAsyncST");
            Param.Add("@ScheduleId", ObjCo.ScheduleId.ToString());
            Param.Add("@LabScheduleId", ObjCo.LabScheduleId.ToString());
            Param.Add("@BatchCode", ObjCo.BatchCode);
            Param.Add("@TrainerCodeStaffCode", ObjCo.StaffCode);
            Param.Add("@LabCode", ObjCo.LabName);
            Param.Add("@BatchScheduleDate", ObjCo.BatchScheduleDate.ToString("yyyy-MM-dd HH:mm:ss"));
            Param.Add("@ScheduledAddedByStaffCode", ObjCo.AddedStaffCode);
            Param.Add("@Statusid", ObjCo.StatusId.ToString());
            Param.Add("@StartTime", ObjCo.StartTime.ToString("HH:mm:ss"));
            Param.Add("@EndTime", ObjCo.EndTime.ToString("HH:mm:ss"));
            Param.Add("@ScheduleStartDate", ObjCo.StartDate.ToString("yyyy-MM-dd HH:mm:ss"));
            Param.Add("@EndDate", ObjCo.EndDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            await DBHelper.ExecuteStoreProcedure("GSTCoordinator", Param);
        }

        /// <summary>
        /// This method get Status List.
        /// </summary>
        /// <returns> Status list</returns>
        public async Task<DataSet> GetStatusAsyncST()
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "GetStatusAsyncST");
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        ///// <summary>
        ///// This method Read Available Labs List.
        ///// </summary>
        ///// <returns> Status list</returns>
        public async Task<DataSet> ReadAvailableLabsST(Coordinator ObjCo)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ReadAvailableLabsST");
            Param.Add("@StartDate", ObjCo.StartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            Param.Add("@EndDate", ObjCo.EndDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            Param.Add("@StartTime", ObjCo.StartTime.ToString("HH:mm:ss"));
            Param.Add("@EndTime", ObjCo.EndTime.ToString("HH:mm:ss"));
            Param.Add("@BatchCode", ObjCo.BatchCode.ToString());
            Param.Add("@BranchCode", ObjCo.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }

        /// <summary>
        /// This method get Lab List.
        /// </summary>
        /// <returns> Lab list</returns>
        public async Task<DataSet> GetLabAsyncST(Coordinator ObjCo)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "GetLabAsyncST");
            Param.Add("@BatchTime", ObjCo.BatchTime);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }

        /// <summary>
        /// This method get Trainer List.
        /// </summary>
        /// <returns> Trainer list</returns>
        public async Task<DataSet> GetTrainerAsyncST(Coordinator ObjCo)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "GetTrainerAsyncST");
            Param.Add("@CourceCode", ObjCo.CourseCode);
            Param.Add("@BranchCode", ObjCo.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// This view use to save Reschedule Batch  Data.
        /// </summary>
        /// <returns> </returns>
        public async Task BatchRescheduleAsyncST(Coordinator ObjCo)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "BatchRescheduleAsyncST");
            Param.Add("@ScheduleId", ObjCo.ScheduleId.ToString());
            Param.Add("@LabScheduleId", ObjCo.LabScheduleId.ToString());
            Param.Add("@BatchCode", ObjCo.BatchCode);
            Param.Add("@TrainerCodeStaffCode", ObjCo.StaffName);
            Param.Add("@LabCode", ObjCo.LabName);
            Param.Add("@BatchScheduleDate", ObjCo.BatchScheduleDate.ToString("yyyy-MM-dd HH:mm:ss"));
            Param.Add("@ScheduledAddedByStaffCode", ObjCo.StaffCode);
            Param.Add("@Statusid", ObjCo.StatusId.ToString());
            Param.Add("@StartTime", ObjCo.StartTime.ToString("HH:mm:ss"));
            Param.Add("@EndTime", ObjCo.EndTime.ToString("HH:mm:ss"));
            Param.Add("@ScheduleStartDate", ObjCo.StartDate.ToString("yyyy-MM-dd HH:mm:ss"));
            Param.Add("@EndDate", ObjCo.EndDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            Param.Add("@RescheduleDate", ObjCo.RescheduleDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            await DBHelper.ExecuteStoreProcedure("GSTCoordinator", Param);
        }

        //--------------------Sayli Batch Schedule End -----------------------------------------


        //--------------------Sayli Batch Schedule End -----------------------------------------

        ////       ----------------lab sedule vp -------------------------------------------------
        //public async Task<DataSet> LabscheduleAsyncVP(Coordinator obj )
        //{
        //    Dictionary<String, String> Param = new Dictionary<String, String>();
        //    Param.Add("@Flag", "LabSeduleData");
        //    Param.Add("@BranchCode", obj.BranchCode);
        //    DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
        //    return ds;
        //}
        ////       ----------------lab sedule vp -------------------------------------------------


        //---------------------------Kirti Coordinator Attendanse Follow Up Start  -------------
        /// <summary>
        /// This method is showing all present attendance to take followup.
        /// </summary>
        /// <returns>It Returns All Absent Student list who are absent more than 2 Days</returns>
        public async Task<DataSet> GetAttendanceFollowupKKAsync(Coordinator objd)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "ShowAttendanceFollowUpKK");
            Param.Add("@BranchCode", objd.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// This method is showing addattendancefollowup.
        /// </summary>
        /// <param name="id">id is use for the read the data from followup.</param>
        /// <returns>It returns the details of studentdata which is from followup form.</returns>
        public async Task<DataSet> AddAttendanceFollowupKKAsync(Coordinator objatt)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "AddFollowUpKK");
            Param.Add("@CandidateCode", objatt.StudentCode);
            Param.Add("@BranchCode", objatt.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// This function is for Save all the value of followup.
        /// </summary>
        /// <param name="objsave">Use for access the property from property class.</param>
        public async Task SaveFollowUpDataKKAsync(Coordinator objsave)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "SaveAddFollowUpDataKK");
            Param.Add("@StudentCode", objsave.StudentCode);
            Param.Add("@TypeId", "1");
            Param.Add("@FollowUpNote", objsave.FollowUpNote);
            Param.Add("@FollowUpTakenDateSystem", DateTime.Now.ToString("yyyy-MM-dd"));
            Param.Add("@NextFollowUpDate", objsave.NextFollowUpDate.ToString("yyyy-MM-dd HH:mm:ss"));
            Param.Add("@Statusid", objsave.StatusName);
            Param.Add("@DatejoinORinstallment", objsave.DateofJoin.ToString("yyyy-MM-dd HH:mm:ss"));
            Param.Add("@FollowUpTakenByStaffCode", objsave.FollowUpTaken);
            await DBHelper.ExecuteStoreProcedure("GSTCoordinator", Param);
        }
        /// <summary>
        /// This function is showing the statusnames which i used for followup.
        /// </summary>
        /// <returns>Returns the statusnames list</returns>
        public async Task<DataSet> ShowStatusKKAsync()
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "ShowStatusKK");
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// This method is showing staffnames.
        /// </summary>
        /// <returns>returns the staffname list</returns>
        public async Task<DataSet> ShowStaffKKAsync(Coordinator objs)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "ShowStaffKK");
            Param.Add("@BranchCode", objs.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// This method is for showing all pending and completed followup.
        /// </summary>
        /// <returns>returns the pending and completed followup list.</returns>
        public async Task<DataSet> ShowAllFollowupKKAsync(Coordinator objb)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "ViewAllFollowUpKK");
            Param.Add("@BranchCode", objb.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// This function is shows all Not reachable students followup.
        /// </summary>
        /// <returns>Returns the pending followup list.</returns>
        public async Task<DataSet> ShowPendingFollowupKKAsync(Coordinator objb)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "AllPendingFollowupKK");
            Param.Add("@BranchCode", objb.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// This method is for add student next followup.
        /// </summary>
        /// <param name="objfid">This is use for getting the all values from the addfollowupform.</param>
        /// <returns>Returns all the column value.</returns>
        public async Task<DataSet> AddViewAttendanceFollowupKKAsync(Coordinator objfid)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "EditDataAddFollowupKK");
            Param.Add("@FollowUpId", objfid.FollowUpId.ToString());
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// This function is for update the followup information which we taken.
        /// </summary>
        /// <param name="objup">This is use for the update the data.</param>
        public async Task UpdateFollowupKKAsync(Coordinator objup)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "UpdateFollowupDataKK");
            Param.Add("@FollowUpId", objup.FollowUpId.ToString());
            Param.Add("@FollowUpNote", objup.FollowUpNote);
            Param.Add("@Statusid", objup.StatusName);
            Param.Add("@FollowUpTakenDateSystem", DateTime.Now.ToString("yyyy-MM-dd"));
            Param.Add("@NextFollowUpDate", objup.NextFollowUpDate.ToString("yyyy-MM-dd HH:mm:ss"));
            Param.Add("@DatejoinORinstallment", objup.DateofJoin.ToString("yyyy-MM-dd HH:mm:ss"));
            Param.Add("@FollowUpTakenByStaffCode", objup.FollowUpTaken);
            await DBHelper.ExecuteStoreProcedure("GSTCoordinator", Param);
        }
        /// <summary>
		/// This method is for the view the followup history of the particular student.
		/// </summary>
		/// <param name="objstc">This object of student code use for the access the property from the class.</param>
		/// <returns>Returns the selected student code followup history.</returns>
		public async Task<DataSet> ViewHistoryDetailsKKAsync(Coordinator obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "ViewHistoryDataKK");
            Param.Add("@StudentCode", obj.StudentCode);
            Param.Add("@BranchCode", obj.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
		/// In this method shows the all coursenames.
		/// </summary>
		/// <returns>Returns the courselist.</returns>
		public async Task<DataSet> ShowCourseKKAsync(Coordinator objst)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "ShowCourseNameKK");
            Param.Add("@BranchCode", objst.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }

        //---------------------------Kirti Coordinator Attendanse Follow Up End -------------
        //---------------------------Kirti Coordinator Feedack  Start -------------
        /// <summary>
		/// This method is for showing the feedback list of trainer which student want to give.
		/// </summary>
		/// <returns>Returns the feedback trainer list.</returns>
		public async Task<DataSet> NewFeedbackListFromStudentKKAsync(Coordinator objf)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "NewListFeedbackForTrainerKK");
            Param.Add("@BranchCode", objf.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// This is list of feedback which is already given student to trainer.
        /// </summary>
        /// <param name="objf">Use for access parameter.</param>
        /// <returns>It returns feedback given list.</returns>
        public async Task<DataSet> FeedbackFromStudentKKAsync(Coordinator objf)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "FeedbackForTrainerKK");
            Param.Add("@BranchCode", objf.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// This method is for the trainer names related course.
        /// </summary>
        /// <param name="StaffPositionId">This parameter use for getting only trainer from the staff table.</param>
        /// <returns>It returns the trainer list related to their course.</returns>
        public async Task<DataSet> StaffCourseNameKKAsync(Coordinator objcourse)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "GetTrainerCourseKK");
            Param.Add("@BranchCode", objcourse.BranchCode);
            //Param.Add("@StaffPositionId", StaffPositionId.ToString());
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// This method is for the getting staff name of selected course.
        /// </summary>
        /// <param name="Coursecode">Course code use for the selected coursedata.</param>
        /// <returns>It returns the coursetrainer list.</returns>
        public async Task<DataSet> StaffTrainerNameKKAsync(string Coursecode)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "GetCourseTrainerNameKK");
            Param.Add("@CourseCode", Coursecode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// This method is for the batch names related to trainer.
        /// </summary>
        /// <param name="TrainerCode">This parameter use for the to get particular trainer.</param>
        /// <returns>It returns the trainer batch list.</returns>
        public async Task<DataSet> TrainerBatchNameKKAsync(Coordinator objbatch)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "GetTrainerBatchKK");
            Param.Add("@TrainerCodeStaffCode", objbatch.StaffCode);
            Param.Add("@BranchCode", objbatch.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// This method is for the only demo course names.
        /// </summary>
        /// <returns>It returns the demo course name list.</returns>
        public async Task<DataSet> DemoCourseNameKKAsync(Coordinator objdco)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "DemoCoursesKK");
            Param.Add("@Typeid", objdco.TypeId.ToString());
            Param.Add("@BranchCode", objdco.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// This method is for the demo trainer batch names.
        /// </summary>
        /// <param name="TrainerCode">This parameter is use for the shows selected trainer batch.</param>
        /// <returns>It returns the batch names list of selected trainer.</returns>
        public async Task<DataSet> DemoTrainerBatchNameKKAsync(Coordinator objdb)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "GetDemoTrainerBatchKK");
            Param.Add("@TrainerCodeStaffCode", objdb.StaffCode);
            Param.Add("@BranchCode", objdb.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// This method is for the showing all the event list.
        /// </summary>
        /// <returns>It returns the level1 event lists.</returns>
        public async Task<DataSet> EventListKKAsync()
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "GetEventNameKK");
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// This method is for the showing all active batch list.
        /// </summary>
        /// <returns>It returns the active batch list.</returns>
        public async Task<DataSet> EventBatchKKAsync()
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "GetEventBatchKK");
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }

        /// <summary>
        /// This method is for the to save feedback for trainer form details.
        /// </summary>
        /// <param name="objfed">This parameter is use for the access property from the class.</param>
        /// <returns>It returns the student feedback form details.</returns>
        public async Task SaveStudentFeedbackKKAsync(Coordinator objfed, string staffCode)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "SaveFeedbackKK");
            Param.Add("@CourseCode", objfed.CourseCode);
            Param.Add("@TypeId", objfed.FeedbackType.ToString());
            Param.Add("@FeedbackFor", objfed.FeedbackFor);
            Param.Add("@FeedbackFrom", objfed.StudentCode);
            Param.Add("@FeedbackSendDate", DateTime.Now.ToString("yyyy-MM-dd"));
            Param.Add("@FeedbackTillDate", objfed.Date.ToString("yyyy - MM - dd HH: mm:ss"));
            Param.Add("@Descriptions", objfed.Description);
            Param.Add("@FeedbackSendBy", staffCode);
            await DBHelper.ExecuteStoreProcedure("GSTCoordinator", Param);
        }
        /// <summary>
        /// This method is showing the feedback list which trainer give to student.
        /// </summary>
        /// <returns>It returns the list of trainer feedback list.</returns>
        public async Task<DataSet> FeedbackFromTrainerKKAsync(Coordinator objtr)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "FeedbackForStudentKK");
            Param.Add("@BranchCode", objtr.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// This method is for the new feedback list from trainer give to students.
        /// </summary>
        /// <param name="objtr">Use for the access the property from the class.</param>
        /// <returns>It returns the new list of trainer feedback give for students from batch. </returns>
        public async Task<DataSet> NewFeedbackListFromTrainerKKAsync(Coordinator objtr)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "NewListFeedbackForStudentKK");
            Param.Add("@BranchCode", objtr.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// This method is showing the demo courses.
        /// </summary>
        /// <param name="Coursecode">Course code is use for the select course.</param>
        /// <returns>It returns the list.</returns>
        public async Task<DataSet> DemoFeedbackListKKAsync(Coordinator objdbth)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "GetDemoFeedbackKK");
            Param.Add("@CourseCode", objdbth.CourseCode);
            Param.Add("@BranchCode", objdbth.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// This method is for the getting student list of batch.
        /// </summary>
        /// <param name="BatchCode">This parameter is for the seelcted batchname.</param>
        /// <returns>It returns the active batch student list.</returns>
        public async Task<DataSet> BatchStudentListKKAsync(Coordinator objstudents)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "GetBatchStudentKK");
            Param.Add("@BatchCode", objstudents.BatchCode);
            Param.Add("@BranchCode", objstudents.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// This method is showing the demo batch student list.
        /// </summary>
        /// <param name="BatchCode">Batchcode is use for the showing selected batch list.</param>
        /// <returns>It returns the demo batch student list.</returns>
        public async Task<DataSet> DemoBatchStudentListKKAsync(Coordinator dbthstud)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "GetDemoStudentKK");
            Param.Add("@BatchCode", dbthstud.BatchCode);
            Param.Add("@BranchCode", dbthstud.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// This method is for the getting batch list from course.
        /// </summary>
        /// <param name="CourseCode">This parameter is use for selected particular course.</param>
        /// <returns>It returns the active batch list of selected course.</returns>
        public async Task<DataSet> CourseBatchKKAsync(Coordinator objcbth)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "GetCourseBatchKK");
            Param.Add("@CourseCode", objcbth.CourseCode);
            Param.Add("@BranchCode", objcbth.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// This method is for the save feedback for student.
        /// </summary>
        /// <param name="objfed">This parameter use for access the property from property class.</param>
        /// <returns>It returns the saved data.</returns>
        public async Task SaveTrainerFeedbackKKAsync(Coordinator objfed, string staffCode)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "SaveFeedbackKK");
            Param.Add("@CourseCode", objfed.CourseCode);
            Param.Add("@TypeId", "6");
            Param.Add("@FeedbackFor", objfed.StudentCode);
            Param.Add("@FeedbackFrom", objfed.TrainerName);
            Param.Add("@FeedbackSendDate", DateTime.Now.ToString("yyyy-MM-dd"));
            Param.Add("@FeedbackTillDate", objfed.Date.ToString("yyyy - MM - dd HH: mm:ss"));
            Param.Add("@Descriptions", objfed.Description);
            Param.Add("@FeedbackSendBy", staffCode);
            await DBHelper.ExecuteStoreProcedure("GSTCoordinator", Param);
        }

        //---------------------------Kirti Coordinator FeedBack  End -------------
        //----------------------- Rohit Fees  Follow Up ------------------------------------------//
        /// <summary>
		/// Asynchronously retrieves a dataset containing fee follow-up details for a specific branch.
		/// </summary>
		/// <param name="branchcode">The branch code for which fee follow-up details are to be retrieved.</param>
		/// <returns>A task representing the asynchronous operation, returning the dataset with fee follow-up details.</returns>
		public async Task<DataSet> ListFeeFollowupAsyncRS(string branchcode)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ListFeeFollowup");
            Param.Add("@BranchCode", branchcode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// Asynchronously retrieves a dataset containing fee follow-up details for a specific student.
        /// </summary>
        /// <param name="StudentId">The ID of the student for whom fee follow-up details are to be retrieved.</param>
        /// <returns>A task representing the asynchronous operation, returning the dataset with fee follow-up details.</returns>
        public async Task<DataSet> ViewFeeFollowupDetailsAsyncRS(int StudentId)
        {

            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ViewFeeFollowupDetails");
            Param.Add("@StudentId", StudentId.ToString());
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// Asynchronously retrieves a dataset containing fee follow-up details for a coordinator.
        /// </summary>
        /// <param name="objc">The Coordinator object containing branch code and student ID for which fee follow-up details are to be retrieved.</param>
        /// <returns>A task representing the asynchronous operation, returning the dataset with fee follow-up details.</returns>
        public async Task<DataSet> GetFeeFollowupDetailsAsyncRS(Coordinator objc)
        {

            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "GetFeeFollowupDetails");
            Param.Add("@BranchCode", objc.BranchCode);
            Param.Add("@StudentId", objc.StudentId.ToString());
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// Asynchronously adds fee follow-up details for a coordinator.
        /// </summary>
        /// <param name="Obj">The Coordinator object containing the fee follow-up details to be added.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task AddFeeFollowUpAsyncRS(Coordinator Obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "AddFeeFollowUpDetails");
            Param.Add("@CandidateCode", Obj.StudentCode.ToString());
            Param.Add("@FollowUpNote", Obj.FollowUpNote.ToString());
            Param.Add("@NextFollowUpDate", Obj.NextFollowUpDate.ToString("yyyy-MM-dd HH:mm:ss"));
            Param.Add("@Statusid", Obj.StatusId.ToString());
            Param.Add("@DatejoinORinstallment", Obj.NextInstallmentDate.ToString("yyyy-MM-dd HH:mm:ss"));
            Param.Add("@FollowUpTakenByStaffCode", Obj.StaffCode);
            Param.Add("@FollowUpTakenDateSystem", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            await DBHelper.ExecuteStoreProcedure("GSTCoordinator", Param);
        }
        /// <summary>
        /// Asynchronously retrieves staff data from the database for the specified coordinator.
        /// </summary>
        /// <param name="objc">The coordinator object containing necessary information.</param>
        /// <returns>A DataSet containing the retrieved staff data.</returns>
        public async Task<DataSet> GetStaffAsyncRS(Coordinator objc)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "GetStaff");
            Param.Add("@BranchCode", objc.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// Asynchronously retrieves status data from the database for the specified coordinator.
        /// </summary>
        /// <param name="objc">The coordinator object containing necessary information.</param>
        /// <returns>A DataSet containing the retrieved status data.</returns>
        public async Task<DataSet> GetStatusAsyncRS(Coordinator objc)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "GetStatus");
            Param.Add("@BranchCode", objc.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// Asynchronously lists added fee follow-up data from the database for the specified branch.
        /// </summary>
        /// <param name="branchcode">The branch code for which the data is retrieved.</param>
        /// <returns>A DataSet containing the retrieved added fee follow-up data.</returns>
        public async Task<DataSet> ListAddedFeeFollowupAsyncRS(string branchcode)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "AddedFeeFollowUp");
            Param.Add("@BranchCode", branchcode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// Asynchronously retrieves follow-up history data from the database for the specified student.
        /// </summary>
        /// <param name="StudentId">The ID of the student for which the data is retrieved.</param>
        /// <returns>A DataSet containing the retrieved follow-up history data.</returns>
        public async Task<DataSet> ViewFollowUpHistoryRS(int StudentId)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ViewFollowUpHistoryRS");
            Param.Add("@StudentId", StudentId.ToString());
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }

        //-----------------------Rohit Fees  Follow Up ------------------------------------------//
        //-------------------vedant lab Managemet Start  -----------------------------------------------------------------//
        /// <summary>
        /// This Methode is used to View All Lab List.
        /// </summary>
        /// <returns> Lab List. </returns>
        public async Task<DataSet> ViewLabList(Coordinator objC)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ViewLabList");
            Param.Add("@BranchCode", objC.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        public BALCoordinator()
        {
        }
        /// <summary>
        /// This Methode is used to Create New Lab
        /// </summary>
        /// <returns> Create New Lab </returns>
        public async Task CreateNewLab(Coordinator objC)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "CreateNewLab");
            Param.Add("@LabName", objC.LabName);
            Param.Add("@BranchCode", objC.BranchCode);
            Param.Add("@LabCapacity", objC.LabCapacity.ToString());
            Param.Add("@AvailableSystem", objC.AvailableSystem.ToString());
            Param.Add("@LabCreatedDate", DateTime.Today.ToString("yyyy-MM-dd"));
            await DBHelper.ExecuteStoreProcedure("GSTCoordinator", Param);
        }


        /// <summary>
        /// This Methode is used to Fetch Center Name.
        /// </summary>
        /// <returns> Center Name. </returns>
        public async Task<DataSet> FetchCenterName()
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "GetCenterName");
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// This Methode is used to Get Lab Details.
        /// </summary>
        /// <param name="objC"> This Object is used to Get Lab Details.</param>
        /// <returns></returns>
        public async Task<SqlDataReader> LabDetails(Coordinator objC)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "LabDetails");
            Param.Add("@LabId", objC.LabId.ToString());
            Param.Add("@BranchCode", objC.BranchCode);
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTCoordinator", Param);
            return dr;
        }
        /// <summary>
        /// This Methode is used to Update Lab Details 
        /// </summary>
        /// <param name="objC"> This Object is used to Update Lab Details</param>
        /// <returns> Update Lab Details </returns>
        public async Task UpdateLab(Coordinator objC)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "UpdateLab");
            Param.Add("@LabId", objC.LabId.ToString());
            Param.Add("@LabCapacity", objC.LabCapacity.ToString());
            Param.Add("@AvailableSystem", objC.AvailableSystem.ToString());
            await DBHelper.ExecuteStoreProcedure("GSTCoordinator", Param);
        }
        /// <summary>
        /// This Methode is used to Delete Lab.
        /// </summary>
        /// <param name="objU"> This Object is used to Delete Lab.</param>
        /// <returns></returns>
        public async Task DeleteLab(Coordinator objU)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "DeleteLab");
            Param.Add("@LabId", objU.LabId.ToString());
            Param.Add("@BranchCode", objU.BranchCode);
            await DBHelper.ExecuteStoreProcedure("GSTCoordinator", Param);
        }
        /// <summary>
        /// This Methode is used to View All Active Lab List.
        /// </summary>
        /// <returns> Active Lab List. </returns>
        public async Task<DataSet> ViewActiveLabList(Coordinator objC)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ActiveLabList");
            Param.Add("@BranchCode", objC.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// This Methode is used to Schedule  Active Lab. 
        /// </summary>
        /// <returns> Schedule Active Lab. </returns>
        public async Task<DataSet> ScheduleLab(string LabCode, string BranchCode)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ViewLabSchedule");
            Param.Add("@LabCode", LabCode);
            Param.Add("@BranchCode", BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        public async Task<DataSet> LabscheduleAsyncVP(Coordinator obj)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "LabSeduleData");
            Param.Add("@BranchCode", obj.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }

        public async Task<bool> IsLabAvilableAsyncVJ(string LabName)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@flag", "IsLabAvilableAsyncVJ");
                Param.Add("@LabName", LabName);
                DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);

                // Check if the DataSet has any rows and if the value in the 'IsAvailable' column is 1
                return ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && Convert.ToInt32(ds.Tables[0].Rows[0]["IsAvailable"]) == 1;
            }
            catch (Exception ex)
            {
                // Handle exceptions if needed
                Console.WriteLine("Error in IsLabAvilableAsyncVJ: " + ex.Message);
                return false; // Return false in case of an error
            }
        }

        //-------------------vedant lab Managemet End -----------------------------------------------------------------//
        //-----------------Kirti Demo Start ------------------------------------------------------------------------------//
        /// <summary>
        /// This method is for to show the all demo list. 
        /// </summary>
        /// <returns>It returns the list of all demo</returns>
        public async Task<DataSet> ViewDemoListKKAsync(Coordinator obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "ViewAllDemoListKK");
            Param.Add("@Statusid", obj.StatusId.ToString());
            Param.Add("@BranchCode", obj.BranchCode);
            Param.Add("@ScheduledAddedByStaffCode", obj.ScheduledAddedByStaffCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }  /// <summary>
           /// This method is for shows the all trainer staff.
           /// </summary>
           /// <returns>Returns all trainer list.</returns>
        public async Task<DataSet> ShowTrainerStaffKKAsync(Coordinator objt)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "ShowTrainerStaffKK");
            Param.Add("@BranchCode", objt.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// This method is for the showing all the demo status which is required for to new demo.
        /// </summary>
        /// <returns>Returns the status list.</returns>
        public async Task<DataSet> ShowDemoStatusKKAsync()
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "ShowDemoStatusKK");
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// This method is for showing the all labs.
        /// </summary>
        /// <returns>Returns the all labs list.</returns>
        public async Task<DataSet> AllLabListKKAsync(Coordinator objl)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "ShowLabNameKK");
            Param.Add("@StartDate", objl.StartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            Param.Add("@EndDate", objl.EndDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            Param.Add("@StartTime", objl.StartTime.ToString("HH:mm:ss"));
            Param.Add("@EndTime", objl.EndTime.ToString("HH:mm:ss"));
            Param.Add("@NoofStudent", objl.NoOfStudent.ToString());
            Param.Add("@BranchCode", objl.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// This method is for showing the lab capacity.
        /// </summary>
        /// <param name="labid">This parameter is use for the shows the capacity of selected lab.</param>
        /// <returns>Returns the capacity value of which lab is select.</returns>
        public async Task<DataSet> AllLabCapacityKKAsync(Coordinator objlab)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "ShowLabCapacityKK");
            Param.Add("@LabCode", objlab.LabCode);
            Param.Add("@BranchCode", objlab.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// This function is for showing the student list selected course who want a demo.
        /// </summary>
        /// <param name="courseid">Using courseid shows the student list related course.</param>
        /// <returns>Returns the list.</returns>
        public async Task<DataSet> DemoStudentsListKKAsync(Coordinator objdw)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "DemoWantStudentListKK");
            Param.Add("@CourseCode", objdw.CourseCode);
            Param.Add("@BranchCode", objdw.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// This function is for to save the demo details.
        /// </summary>
        /// <param name="objd">objd is for access the property from the coordinator class.</param>
        /// <returns></returns>
        public async Task SaveDemoDataKKAsync(Coordinator objd)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "SaveDemoKK");
            Param.Add("@BatchName", objd.BatchName);
            Param.Add("@StudentCode", objd.SelectedStudentCodes);
            Param.Add("@CourseCode", objd.CourseName);
            Param.Add("@NoofStudent", objd.NoOfStudent.ToString());
            Param.Add("@CreateDate", DateTime.Now.ToString("yyyy-MM-dd"));
            Param.Add("@Statusid", "44");
            Param.Add("@TypeId", "4");
            Param.Add("@CreatedByStaffCode", objd.StaffCode);
            await DBHelper.ExecuteStoreProcedure("GSTCoordinator", Param);
        }
        /// <summary>
        /// This method is for save demo data and also lab schedule.
        /// </summary>
        /// <param name="objdemo">objdemo object use for the access the properties.</param>
        /// <returns></returns>
        public async Task SaveDemoScheduleKKAsync(Coordinator objdemo)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            //------------------BatchSchedule Parameters---------------------------
            Param.Add("@flag", "SaveDemoAndLabScheduleKK");
            Param.Add("@TrainerCodeStaffCode", objdemo.TrainerName);
            Param.Add("@LabCode", objdemo.LabCode);
            Param.Add("@StartDate", objdemo.ExpectedDate.ToString("yyyy-MM-dd HH:mm:ss"));
            Param.Add("@BatchScheduleDate", DateTime.Now.ToString("yyyy-MM-dd"));
            Param.Add("@ScheduledAddedByStaffCode", objdemo.StaffCode);
            Param.Add("@Statusid", "25");
            //------------------LabSchedule Parameters---------------------------
            Param.Add("@StartTime", objdemo.StartTime.ToString("HH: mm:ss"));
            Param.Add("@EndTime", objdemo.EndTime.ToString("HH:mm:ss"));
            Param.Add("@EndDate", objdemo.EndDate.ToString("yyyy-MM-dd HH:mm:ss"));
            await DBHelper.ExecuteStoreProcedure("GSTCoordinator", Param);
        }
        /// <summary>
        /// This method is for the showing the details of demo which is conducted.
        /// </summary>
        /// <param name="Scheduleid">The schedule id use for the selected id data showing.</param>
        /// <returns>Retruns the selected id data.</returns>
        public async Task<DataSet> DemoDetailsKKAsync(Coordinator objdd)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "DemoDetailsKK");
            Param.Add("@ScheduleId", objdd.ScheduleId.ToString());
            Param.Add("@BranchCode", objdd.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// This method is for showing the feedback list of trainer which student want to give.
        /// </summary>
        /// <returns>Returns the feedback trainer list.</returns>	/// <summary>
        /// This method is for reschedule demo data.
        /// </summary>
        /// <param name="">This is use for getting the all values from the rejecteddemo.</param>
        /// <returns>Returns all the value.</returns>
        public async Task<DataSet> RescheduleDemoKKAsync(Coordinator objrd)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "EditRejectedDemoKK");
            Param.Add("@ScheduleId", objrd.ScheduleId.ToString());
            Param.Add("@BranchCode", objrd.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// Following method is for the reshedule the demo.
        /// </summary>
        /// <param name="objUdemo">This parameter use for the access the property.</param>
        /// <returns>It returns the updated value batch schedule and lab schedule.</returns>
        public async Task UpdateDemoScheduleKKAsync(Coordinator objUdemo)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            //------------------BatchSchedule Parameters---------------------------
            Param.Add("@flag", "UpdateRescheduleDemoKK");
            Param.Add("@ScheduleId", objUdemo.ScheduleId.ToString());
            Param.Add("@TrainerCodeStaffCode", objUdemo.TrainerName);
            Param.Add("@LabCode", objUdemo.LabCode);
            Param.Add("@StartDate", objUdemo.StartDate.ToString("yyyy-MM-dd HH:mm:ss"));
            Param.Add("@RescheduleDate", objUdemo.RescheduleDate.ToString("yyyy-MM-dd HH:mm:ss"));
            Param.Add("@ScheduledAddedByStaffCode", objUdemo.ScheduledAddedByStaffCode);
            Param.Add("@Statusid", "25");
            //------------------LabSchedule Parameters---------------------------
            Param.Add("@LabScheduleId", objUdemo.LabScheduleId.ToString());
            Param.Add("@StartTime", objUdemo.StartTime.ToString("HH: mm:ss"));
            Param.Add("@EndTime", objUdemo.EndTime.ToString("HH:mm:ss"));
            Param.Add("@ScheduleStartDate", objUdemo.StartDate.ToString("yyyy-MM-dd HH:mm:ss"));
            Param.Add("@EndDate", objUdemo.StartDate.ToString("yyyy-MM-dd HH:mm:ss"));
            await DBHelper.ExecuteStoreProcedure("GSTCoordinator", Param);
        }
        /// <summary>
        /// This method is for the edit demo which is arrange for the trainer.
        /// </summary>
        /// <param name="demoedit">This parameter using the access properties.</param>
        /// <returns>It returns the which scheduleid selected the data of particular id showing.</returns>
        public async Task<DataSet> DemoEditKKAsync(Coordinator demoedit)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "EditDemoDataKK");
            Param.Add("@ScheduleId", demoedit.ScheduleId.ToString());
            Param.Add("@BranchCode", demoedit.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        public async Task UpdateDemoKKAsync(Coordinator Updated)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            //------------------Batch Parameters---------------------------
            Param.Add("@flag", "UpdateDemoKK");
            Param.Add("@BatchId", Updated.BatchId.ToString());
            Param.Add("@StudentCode", Updated.StudentCode);
            Param.Add("@NoofStudent", Updated.NoOfStudent.ToString());
            Param.Add("@CreatedByStaffCode", Updated.StaffCode);
            //------------------BatchSchedule Parameters---------------------------
            Param.Add("@ScheduleId", Updated.ScheduleId.ToString());
            Param.Add("@TrainerCodeStaffCode", Updated.TrainerName);
            Param.Add("@LabCode", Updated.LabCode);
            Param.Add("@StartDate", Updated.StartDate.ToString("yyyy-MM-dd HH:mm:ss"));
            Param.Add("@ScheduledAddedByStaffCode", Updated.ScheduledAddedByStaffCode);
            Param.Add("@Statusid", Updated.StatusId.ToString());
            //------------------LabSchedule Parameters---------------------------
            Param.Add("@LabScheduleId", Updated.LabScheduleId.ToString());
            Param.Add("@StartTime", Updated.StartTime.ToString("HH: mm:ss"));
            Param.Add("@EndTime", Updated.EndTime.ToString("HH:mm:ss"));
            Param.Add("@EndDate", Updated.StartDate.ToString("yyyy-MM-dd HH:mm:ss"));
            await DBHelper.ExecuteStoreProcedure("GSTCoordinator", Param);
        }
        public async Task<DataSet> GetDemoStatusKKAsync()
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "GetStatusForDemoKK");
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        public async Task<bool> IsBatchAvilableAsyncST(string BatchName)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@flag", "IsBatchAvilableAsyncST");
                Param.Add("@BatchName", BatchName);
                DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
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
		/// This method is shows the Noofstudent list.
		/// </summary>
		/// <param name="Batchcode">Using batch code showing the student list.</param>
		/// <returns>It returns the list of student.</returns>
		public async Task<DataSet> NoOfStudentListKKAsync(Coordinator objstu)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@flag", "NoofStudentListKK");
            Param.Add("@BatchCode", objstu.BatchCode);
            Param.Add("@BranchCode", objstu.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }

        //-----------------Kirti Demo  End -------------------------------------------------------------------------------//
        //-----------------------------------------Priyanka Event-------------------------------------//
        /// <summary>
        /// method for view Total Event list.
        /// </summary>
        /// <returns>ShowList</returns>
        public async Task<DataSet> EventListPCAsync(Coordinator obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ShowEventList");
            Param.Add("@BranchCode", obj.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// Method to Show Event in Calender Datewise.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<DataSet> CalenderListPCAsync(Coordinator obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "Calenderlistshow");
            Param.Add("@BranchCode", obj.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// Method for view Conducted Event List.
        /// </summary>
        /// <returns></returns>
        public async Task<DataSet> ConductedEventListPCAsync(Coordinator obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ShowConductedEventList");
            Param.Add("@BranchCode", obj.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// Method for view Arranged Event List.
        /// </summary>
        /// <returns></returns>
        public async Task<DataSet> ArrangedEventListPCAsync(Coordinator obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ShowArrangedEventList");
            Param.Add("@BranchCode", obj.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// Method for view Pending Event List.
        /// </summary>
        /// <returns></returns>
        public async Task<DataSet> PendingEventListPCAsync(Coordinator obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ShowPendingEventList");
            Param.Add("@BranchCode", obj.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// Method for Get Event list at Dropdown.
        /// </summary>
        /// <returns></returns>
        public async Task<DataSet> GetEventListPCAsync()
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "EventListBind");
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// Method for Get Event-Organizer list at Dropdown.
        /// </summary>
        /// <returns></returns>
        public async Task<DataSet> EventOrganizerBindPCAsync(Coordinator obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "OrganizerListBind");
            Param.Add("@BranchCode", obj.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// Method for get selected event organizer at Dropdown list.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<DataSet> EventOrganizerstaffBindPCAsync(Coordinator obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "OrganizerstaffListBind");
            Param.Add("@StaffCodeArrangedBy", obj.EventOrgnaizerCode);
            Param.Add("@BranchCode", obj.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// Method for Get Event-Organizer list at Dropdown.
        /// </summary>
        /// <returns></returns>
        public async Task<DataSet> GetEventOrganizerBindPCAsync(Coordinator obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "EventOrganizerListBind");
            Param.Add("@BranchCode", obj.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// Method for Status-list to Dropdown list.
        /// </summary>
        /// <returns></returns>
        public async Task<DataSet> GetStatusPCAsync()
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "SelectStatus");
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// Method to get Batch list to select and send invitation.
        /// </summary>
        /// <returns></returns>
        public async Task<DataSet> batchbindPCAsync(Coordinator obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "SelectBatchList");
            Param.Add("@BranchCode", obj.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTCoordinator", Param);
            return ds;
        }
        /// <summary>
        /// Method to Add event in event Management.
        /// </summary>
        /// <returns></returns>
        public async Task RegisterEventPCAsync(Coordinator obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "NewEventAdd");
            Param.Add("@EventName", obj.EventName);
            Param.Add("@EventOrgnaizerCode", obj.EventOrgnaizerCode);
            Param.Add("@NoOfParticipants", obj.NoOfParticipants.ToString());
            Param.Add("@StartDate", obj.StartDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            Param.Add("@EndDate", obj.EndDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            Param.Add("@StartTime", obj.StartTime.ToString("HH:mm:ss"));
            Param.Add("@EndTime", obj.EndTime.ToString("HH:mm:ss"));
            Param.Add("@Location", obj.Location);
            Param.Add("@EventArrangedByStaffCode", obj.EventArrangedBy);
            Param.Add("@EventType", obj.EventType.ToString());
            Param.Add("@EventAddedByStaffCode", obj.StaffCode);
            Param.Add("@Description", obj.Description);
            Param.Add("@Statusid", obj.StatusId.ToString());
            Param.Add("@InvitationToSend", obj.InvitationToSend);
            Param.Add("@IsDeleted", 0.ToString());
            await DBHelper.ExecuteStoreProcedure("GSTCoordinator", Param);
        }

        /// <summary>
        /// Method to View Detail of Particular Event.
        /// </summary>
        /// <param name="eventid"></param>
        /// <returns></returns>
        public async Task<SqlDataReader> EventDetailPCAsync(Coordinator obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ViewEventDetailslist");
            Param.Add("@EventId", obj.EventId.ToString());
            Param.Add("@BranchCode", obj.BranchCode);
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTCoordinator", Param);
            return dr;
        }
        /// <summary>
        /// Method to Delete Event from List.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<SqlDataReader> EventDeletePCAsync(Coordinator obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "eventlistDelete");
            Param.Add("@EventId", obj.EventId.ToString());
            Param.Add("@BranchCode", obj.BranchCode);
            Param.Add("@IsDeleted", 1.ToString());
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTCoordinator", Param);
            return dr;
        }


        //----------------------------------------------------Priyanka End-----------------------//
    }
}
