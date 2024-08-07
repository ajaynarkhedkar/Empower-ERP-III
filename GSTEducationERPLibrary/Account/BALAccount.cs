using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Helper;


namespace GSTEducationERPLibrary.Account
{
    public class BALAccount
    {
        MSSQL DBHelper = new MSSQL();

        public async Task<SqlDataReader> Login(Account objA)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "Login");
            Param.Add("@OfficialEmailId", objA.EmailId);
            Param.Add("@Password", objA.Password);
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTAccount", Param);
            return dr;
        }

        /// <summary>
        /// This method get Scheduled Batch Count.
        /// </summary>
        public async Task<DataSet> GetScheduledBatchCountAsyncST(String StaffCode)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "GetScheduledBatchCountAsyncST");
            Param.Add("@TrainerCodeStaffCode", StaffCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTTrainer", Param);
            return ds;
        }
    }
}