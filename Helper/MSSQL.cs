using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Mail;
using System.Web;

namespace Helper
{
    public class MSSQL
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["GSTEduERP"].ConnectionString;

        //***************************************************************************************
        // EXECUTE STORED PROCEDURE AND GET RESULT IN DATA SET
        // SPName - STORED PROCEDURE NAME
        // DICTIONARY FIRST PARAMERET IS FILED NAME IN DATABASE AND ANOTHER PARAMETER IS VALUE
        //****************************************************************************************

        public async Task<DataSet> ExecuteStoreProcedureReturnDS(String SPName, Dictionary<string, string> InPara)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.Parameters.Clear();
                cmd.CommandText = SPName;
                cmd.Connection = con;
                cmd.CommandTimeout = 60;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                foreach (KeyValuePair<string, string> para in InPara)
                {
                    cmd.Parameters.AddWithValue(para.Key, para.Value);
                }
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                await Task.Run(() => da.Fill(ds));

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        //****************************************************************************************
        // EXECUTE STORED PROCEDURE AND RETURN dr
        //****************************************************************************************

        public async Task<SqlDataReader> ExecuteStoreProcedureReturnDataReader(String SPName, Dictionary<string, string> InPara)
        {
            SqlDataReader dr = null;
            try
            {
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.Parameters.Clear();
                cmd.CommandText = SPName;
                cmd.Connection = con;
                cmd.CommandTimeout = 60;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                foreach (KeyValuePair<string, string> para in InPara)
                {
                    cmd.Parameters.AddWithValue(para.Key, para.Value);
                }

                await con.OpenAsync();
                dr = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dr;
        }


        //****************************************************************************************
        // EXECUTE STORED PROCEDURE AND RETURN OBJECT
        //****************************************************************************************
        public async Task<Object> ExecuteStoreProcedureReturnObj(String SPName, Dictionary<string, string> InPara)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Clear();
            cmd.CommandText = SPName;
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            foreach (KeyValuePair<string, string> para in InPara)
            {
                cmd.Parameters.AddWithValue(para.Key, para.Value);
            }

            Object Obj = new Object();
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            Obj = await cmd.ExecuteScalarAsync();
            con.Close();
            return Obj;
        }

        //****************************************************************************************
        // EXECUTE STORED PROCEDURE AND RETURN NOTHING
        //****************************************************************************************
        public async Task ExecuteStoreProcedure(String SPName, Dictionary<string, string> InPara)
        {
            try
            {
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.Parameters.Clear();
                cmd.CommandText = SPName;
                cmd.Connection = con;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                foreach (KeyValuePair<string, string> para in InPara)
                {
                    cmd.Parameters.AddWithValue(para.Key, para.Value);
                }
                con.Open();
                await cmd.ExecuteScalarAsync();
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet ExecuteQueryReturnDs(string sqlQuery, Dictionary<string, string> parameters)
        {
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.CommandTimeout = 60;
                    cmd.CommandType = CommandType.Text;

                    foreach (KeyValuePair<string, string> para in parameters)
                    {
                        // Assuming that parameters are in the format "@ParameterName"
                        cmd.Parameters.AddWithValue(para.Key, para.Value);
                    }

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds);
                    }
                }
            }

            return ds;
        }
        /// <summary>
        /// THIS METHOD IS USE TO SEND EMAIL AND ATTACHMENTS.
        /// </summary>
        /// <param name="SenderID"></param>
        /// <param name="Subject"></param>
        /// <param name="Msg"></param>
        /// <param name="FileUploader"></param>
        /// THIS OBEJCTS IS USE TO GET VALUE AND SEND EMAIL AND ATTACHMENTS.
        public void SendMailPB(string SenderID, string Subject, String Msg, HttpPostedFileBase FileUploader)
        {
            string senderEmail = "hreducationerp@gmail.com";
            string senderPassword = "zapg chik ymwk biza";// pass : hjav gexz qbvh alpw hjav gexz qbvh alpw
            //string smtpHost = "smtp.office365.com";
            string smtpHost = "smtp.gmail.com";
            int smtpPort = 587; // Create a new email message
            MailMessage mailMessage = new MailMessage(senderEmail, SenderID, Subject, Msg); // Configure the SMTP client

            if (FileUploader != null)
            {
                string fileName = Path.GetFileName(FileUploader.FileName);
                mailMessage.Attachments.Add(new Attachment(FileUploader.InputStream, fileName));
            }

            SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential(senderEmail, senderPassword);
            smtpClient.EnableSsl = true; // Send the email
            smtpClient.Send(mailMessage);

        }
    }
}
