using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Helper;
using System.Runtime.Remoting.Messaging;
using System.Web.UI.WebControls.WebParts;
using System.Text.RegularExpressions;

namespace GSTEducationERPLibrary.Accountant
{
	public class BALAccountant
	{
        MSSQL DBHelper = new MSSQL();
        Dictionary<string, string> Param = new Dictionary<string, string>();

        public async Task AddVoucherAsyncSGS(Accountant ObjT)
        {
            try
            {
                Param.Add("@flag", "AddVoucher");
                Param.Add("@VoucherCode", ObjT.VoucherCode.ToString());
                Param.Add("@VendorName", ObjT.VendorName.ToString());
                Param.Add("@Amount", ObjT.Amount.ToString());
                Param.Add("@AmountPaidTo", ObjT.AmountPaidTo.ToString());
                Param.Add("@Description", ObjT.Description.ToString());
                Param.Add("@PaymentMode", ObjT.PaymentMode.ToString());
                Param.Add("@StaffCode", ObjT.StaffCode.ToString());
                Param.Add("@BankId", ObjT.BankId.ToString());
                Param.Add("@ReceiverBankAccountNumber", ObjT.ReceiverBankAccountNumber.ToString());
                Param.Add("@ReceiverBankAccountHolderName", ObjT.ReceiverBankAccountHolderName.ToString());
                Param.Add("@ReceiverBankIFSCCode", ObjT.ReceiverBankIFSCCode.ToString());
                Param.Add("@ReceiverBankName", ObjT.ReceiverBankName.ToString());
                Param.Add("@Balance", ObjT.Balance.ToString());
                Param.Add("@Currency", ObjT.Currency.ToString());
                Param.Add("@TransactionId", ObjT.TransactionId.ToString());
                Param.Add("@VoucherType", ObjT.VoucherType.ToString());
                Param.Add("@VoucherDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                Param.Add("@StatusId", 6.ToString());
                await DBHelper.ExecuteStoreProcedure("GSTAccountant", Param);
            } catch (Exception ex)
            {
                throw new Exception("An error occurred while registering the assigned project. Details: " + ex.Message);
            }
        }
        //public async Task<List<Accountant>> GetData(Accountant objU)
        //{
        //    List<Accountant> items = new List<Accountant>();
        //    Param.Clear();
        //    Param.Add("@flag", "ViewUser");

        //    DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTAccountant", Param);
        //    foreach (DataRow sdr in ds.Tables[0].Rows)
        //    {
        //        items.Add(new Accountant
        //        {
        //            VoucherId = Convert.ToInt32(sdr["VoucherId"]),
        //            VoucherCode = sdr["VoucherCode"].ToString(),
        //            VendorName = sdr["VendorName"].ToString(),
        //            Amount = float.Parse(sdr["Amount"].ToString()),
        //            AmountPaidTo = sdr["AmountPaidTo"].ToString(),
        //            Description = sdr["Description"].ToString(),
        //            PaymentMode = sdr["PaymentMode"].ToString(),
        //            BankId = Convert.ToInt32(sdr["BankId"]),
        //            ReceiverBankAccountNumber = Convert.ToInt64(sdr["ReceiverBankAccountNumber"]),
        //            ReceiverBankAccountHolderName = sdr["ReceiverBankAccountHolderName"].ToString(),
        //            ReceiverBankIFSCCode = sdr["ReceiverBankIFSCCode"].ToString(),
        //            ReceiverBankName = sdr["ReceiverBankName"].ToString(),
        //            Balance = float.Parse(sdr["Balance"].ToString()),
        //            Currency = sdr["Currency"].ToString(),
        //            TransactionId = sdr["TransactionId"].ToString(),
        //            VoucherType = sdr["VoucherType"].ToString(),
        //            VoucherDate = DateTime.Parse(sdr["VoucherDate"].ToString()),
        //            StaffCode = sdr["StaffCode"].ToString(),
        //            StatusId = Convert.ToInt32(sdr["StatusId"])
        //        });
        //    }
        //    return items;
        //}
        public async Task<DataSet> GetVoucher(Accountant objT)
        {
            try
            {
                Dictionary<string, string> Param = new Dictionary<string, string>();
                Param.Add("@flag", "GetVoucher");
                DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTAccountant", Param);
                return ds;
            } catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching course names. Details: " + ex.Message);
            }
        }
        public async Task<DataSet> ListVoucherAsyncSGS()
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ListVoucher");
            //Param.Add("@BranchCode", branchCode); // Pass branch code parameter
            //Param.Add("@StaffCode", staffCode); // Pass staff code parameter
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTAccountant", Param);
            return ds;
        }
        public async Task<DataSet> ListPendingVoucherAsyncSGS()
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ListPendingVoucher");
            //Param.Add("@BranchCode", branchCode); // Pass branch code parameter
            //Param.Add("@StaffCode", staffCode); // Pass staff code parameter
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTAccountant", Param);
            return ds;
        }
        public async Task<DataSet> ListCompletedVoucherAsyncSGS()
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "ListCompletedVoucher");
            //Param.Add("@BranchCode", branchCode); // Pass branch code parameter
            //Param.Add("@StaffCode", staffCode); // Pass staff code parameter
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTAccountant", Param);
            return ds;
        }
        #region //this is vishlas region for the purchase module
        /// <summary>
        /// making the methode for the incremental purchase codes
        /// </summary>
        /// <param name="PurchaseCode"></param>
        /// <returns>NewPurchaseCode</returns>
        public async Task<string> GetTaskPurchaseCode(Accountant obj)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "GetMaxPurCodeAsncVP");
            SqlDataReader ds = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTAccountant", Param);
            string LastTransactionCode = "";
            while (ds.Read())
            {
                LastTransactionCode = ds["TransactionCode"].ToString();
            }
            string newPurchaseCode = IncrementPurchaseCode(LastTransactionCode);
            return newPurchaseCode;
        }
        public static string IncrementPurchaseCode(string lastPurchaseCode)
        {
            // Define a regular expression to extract the numeric part
            Regex regex = new Regex(@"(\D+)(\d+)");
            Match match = regex.Match(lastPurchaseCode);

            if (match.Success)
            {
                string prefix = match.Groups[1].Value; // The non-numeric prefix (e.g., "PUR")
                string numberPart = match.Groups[2].Value; // The numeric part (e.g., "017")

                // Parse the numeric part to an integer
                int number = int.Parse(numberPart);

                // Increment the number
                number++;

                // Determine the length of the original numeric part to maintain leading zeros
                int lengthOfNumberPart = numberPart.Length;

                // Format the new number with leading zeros
                string newNumberPart = number.ToString().PadLeft(lengthOfNumberPart, '0');

                // Reassemble the new purchase code
                string newPurchaseCode = prefix + newNumberPart;

                return newPurchaseCode;
            }
            else
            {
                throw new ArgumentException("Invalid purchase code format.");
            }
        }
        /// <summary>
        /// fetching the all the purchase details here for purchase dashboard
        /// </summary>
        /// <param name="ObjA"></param>
        /// <returns></returns>
        public async Task<DataSet> ListPurchasesAsyncVP(Accountant ObjA)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "ListPurchasesAsyncVP");
            Param.Add("@BranchCode", ObjA.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTAccountant", Param);
            return ds;
        }
        /// <summary>
        /// this method is wrritten for the fetching the details for the edit purchase in purchase dashboard
        /// </summary>
        /// <param name="ObjA"></param>
        /// <returns>Sql datareader object dr </returns>
        public async Task<SqlDataReader> ListPurchasesDetailsAsyncVP(Accountant ObjA)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "ListPurchaseDetailsAsyncVP");
            Param.Add("@PurchaseCode", ObjA.PurchaseCode);
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTAccountant", Param);
            return dr;
        }
        /// <summary>
        /// this methode is for fetching the all items for purchase here for purchase 
        /// </summary>
        /// <param name="ObjA"></param>
        /// <returns>dataset with the purchased item list</returns>
        public async Task<DataSet> ListPurchasedItemsAsyncVP(Accountant ObjA)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "ListPurchasedItemsAsyncVP");
            Param.Add("@PurchaseCode", ObjA.PurchaseCode);
            //Param.Add("@BranchCode", ObjA.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTAccountant", Param);
            return ds;
        }

        /// <summary>
        /// this methode is written for the fetching the hsncode and category defination for dropdown in add purchase
        /// </summary>
        /// <returns>List of Hsn Code </returns>
        public async Task<DataSet> ListHSNCategoryAsyncVP()
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "ListHSNCategoryAsyncVP");
            //Param.Add("@BranchCode", ObjA.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTAccountant", Param);
            return ds;
        }
        /// <summary>
        /// this methode is written for the fetching the tax for dropdown in add purchase
        /// </summary>
        /// <returns>List of Hsn Code </returns>
        public async Task<DataSet> ListtTaxAsyncVP()
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "ListTaxAsyncVP");
            //Param.Add("@BranchCode", ObjA.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTAccountant", Param);
            return ds;
        }

        /// <summary>
        /// fetching the status (66-setteled,6-pending) for add purchase pages and purchase module
        /// </summary>
        /// <param name=""></param>
        /// <returns>status settled- 66 and pending-6</returns>
        public async Task<DataSet> ListStatusAsyncVP()
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "ListStatusForPurchaseAsyncVP");
            //Param.Add("@BranchCode", ObjA.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTAccountant", Param);
            return ds;
        }
        ///<summery>
        ///this methode is wrritten for the validation of save purchase
        /// </summery>
        /// <param name="dr"></param>
        /// <returns>this methode returns the datareader for validating the save purchase</returns>
        public async Task<SqlDataReader> ValidatePurchaseAsyncVP(Accountant ObjA)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "ValidatePurchaseAsyncVP");
            Param.Add("@PurchaseCode", ObjA.PurchaseCode);
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTAccountant", Param);
            return dr;
        }
        /// <summary>
        /// Saving the add purchase details to database into the tbltransaction  
        /// </summary>
        /// <param name="ObjA"></param>
        /// <returns></returns>
        public async Task SavePurchaseAsyncVP(Accountant ObjA)
        {
            long bamt = (long)Math.Floor(decimal.Parse(ObjA.BalanceAmount.ToString()));
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "SavePurchaseAsyncVP");
            Param.Add("@TransactionCode", ObjA.TransactionCode);
            Param.Add("@VendorName", ObjA.VendorName);
            Param.Add("@TransactionDate", ObjA.TransactionDate.ToString("yyyy-MM-dd"));
            Param.Add("@TransactionAmount", 0.ToString());
            Param.Add("@BalanceAmount", bamt.ToString());
            Param.Add("@LogInStaffCode", ObjA.StaffCode);
            Param.Add("@StatusId", ObjA.StatusId.ToString());//completed-66 or pending-6
            Param.Add("@Description", ObjA.Description);
            Param.Add("@TransactionType", "68");//68 is the status for debit from account
            await DBHelper.ExecuteStoreProcedure("GSTAccountant", Param);

        }
        public async Task SavePurchasePaymentAsyncVP(Accountant ObjA)
        {
            long bamt = (long)Math.Floor(decimal.Parse(ObjA.BalanceAmount.ToString()));
            long Tamount = (long)Math.Floor(decimal.Parse(ObjA.TransactionAmount.ToString()));
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "SavePaymentDtlAsyncVP");
            Param.Add("@TransactionCode", ObjA.TransactionCode);
            Param.Add("@TransactionAmount", Tamount.ToString());
            Param.Add("@BalanceAmount", bamt.ToString());
            Param.Add("@PaymentMode", ObjA.PaymentMode);
            if (ObjA.PaymentMode == "CASH")
            {
                Param.Add("@TranId_CheqNo", null);
                Param.Add("@ChequeDate", null);

            }
            else if (ObjA.PaymentMode == "CHEQUE")
            {
                Param.Add("@TranId_CheqNo", ObjA.TransactionId);
                Param.Add("@ChequeDate", ObjA.ChequeDate.ToString("yyyy-MM-dd"));
            }
            else if (ObjA.PaymentMode == "BANK")
            {
                Param.Add("@TranId_CheqNo", ObjA.TransactionId);
                Param.Add("@ChequeDate", null);
            }
            Param.Add("@StatusId", ObjA.StatusId.ToString());//completed-66 or pending-6
            Param.Add("@Description", ObjA.Description);
            Param.Add("@TransactionType", "68");//68 is the status for debit from account
            await DBHelper.ExecuteStoreProcedure("GSTAccountant", Param);
        }
        /// <summary>
        /// Saving the purchased items details to database into the tblPurchasedItem  
        /// </summary>
        /// <param name="ListPurchasedItem"></param>
        /// <returns></returns>
        public async Task SavePurchasedItemsAsyncVP(Accountant ObjA)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "SaveItmsAsyncVP");
            Param.Add("@TransactionCode", ObjA.TransactionCode);
            Param.Add("@ItemName", ObjA.ItemName);
            Param.Add("@Quantity", ObjA.Quantity.ToString());
            Param.Add("@UnitPrice", ObjA.UnitPrice.ToString());
            Param.Add("@Discount", ObjA.Discount.ToString());
            Param.Add("@HSNCode", ObjA.HSNCode);
            Param.Add("@AppliedTax", ObjA.AppliedTax);
            await DBHelper.ExecuteStoreProcedure("GSTAccountant", Param);

        }
        /// <summary>
        /// this methode is wrriten for saving the purchase and the voucher agaist the purchase 
        /// </summary>
        /// <param name="ObjA"></param>
        /// <returns></returns>
        public async Task SaveVoucherPurchaseAsyncVP(Accountant ObjA)
        {
            long Tamount = (long)Math.Floor(decimal.Parse(ObjA.TransactionAmount.ToString()));
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "SaveVoucherLinkAsyncVP");
            Param.Add("@PurchaseCode", ObjA.TransactionCode);
            Param.Add("@VoucherCode", ObjA.VoucherCode);
            Param.Add("@TransactionAmount", Tamount.ToString());
            Param.Add("@TransactionDate", DateTime.Now.ToString("yyyy-MM-dd"));
            Param.Add("@Description", ObjA.Description);
            await DBHelper.ExecuteStoreProcedure("GSTAccountant", Param);
        }
        /// <summary>
        /// this method is wrriten for the updating the purchase in transaction table
        /// </summary>
        /// <param name="ObjA"></param>
        /// <returns></returns>
        public async Task UpdatePurchaseAsyncVP(Accountant ObjA)
        {
            long bamt = (long)Math.Floor(decimal.Parse(ObjA.BalanceAmount.ToString()));
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "UpdatePurchaseAsyncVP");
            Param.Add("@PurchaseCode", ObjA.TransactionCode);
            Param.Add("@VendorName", ObjA.VendorName);
            Param.Add("@TransactionDate", ObjA.TransactionDate.ToString("yyyy-MM-dd"));
            Param.Add("@TransactionAmount", "0");
            Param.Add("@BalanceAmount", bamt.ToString());
            Param.Add("@LogInStaffCode", ObjA.StaffCode);
            Param.Add("@StatusId", ObjA.Status);//completed-66 or pending-6
            Param.Add("@Description", ObjA.Description);
            await DBHelper.ExecuteStoreProcedure("GSTAccountant", Param);

        }
        /// <summary>
        /// this methode is wrriten for the updating the purchased items details in update purchase
        /// </summary>
        /// <param name="ObjA"></param>
        /// <returns>AsyncVP</returns>
        public async Task UpdatePurchasedItemsAsyncVP(Accountant ObjA)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "UpdatePurchasedItemAsyncVP");
            Param.Add("@ItemId", ObjA.ItemId.ToString());
            Param.Add("@ItemName", ObjA.ItemName);
            Param.Add("@Quantity", ObjA.Quantity.ToString());
            Param.Add("@UnitPrice", ObjA.UnitPrice.ToString());
            Param.Add("@Discount", ObjA.Discount.ToString());
            Param.Add("@HSNCode", ObjA.HSNCode);
            Param.Add("@AppliedTax", ObjA.AppliedTax);
            await DBHelper.ExecuteStoreProcedure("GSTAccountant", Param);

        }
        ///<summary>
        ///fetching the vouchers for matching with the Purchase 
        /// </summary>
        /// <param name="ListVouchers"></param>
        /// <return>list of the vouchers</return>
        public async Task<DataSet> ListVouchersAsyncVP(Accountant ObjA)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "ListVoucherAsyncVP");
            Param.Add("@VendorName", ObjA.VendorName);
            //Param.Add("@BranchCode", ObjA.PaymentMode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTAccountant", Param);
            return ds;
        }
        /// <summary>
        /// this methode is written for the deleting the purchase items details from the table /n so we can insert the updated entires
        /// </summary>
        /// <param name="ObjA"></param>
        /// <returns>Nothins</returns>
        public async Task DeletePurchasedItemAsyncVP(Accountant ObjA)
        {
            Dictionary<String, String> Param = new Dictionary<String, String>();
            Param.Add("@Flag", "DeletePurchasedItemAsyncVP");
            Param.Add("@ItemId", ObjA.ItemId.ToString());
            await DBHelper.ExecuteStoreProcedure("GSTAccountant", Param);
        }
        #region//not using vishals methodes
        ///// <summary>
        ///// fetching the banks for add purchase pages and purchase module
        ///// </summary>
        ///// <param name="ObjA"></param>
        ///// <returns> the all the banks name registerd in database</returns>
        //public async Task<DataSet> ListBankAsyncVP(Accountant ObjA)
        //{
        //    Dictionary<String, String> Param = new Dictionary<String, String>();
        //    Param.Add("@Flag", "ListBanksAsyncVP");
        //    Param.Add("@BranchCode", ObjA.BranchCode);
        //    DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTAccountant", Param);
        //    return ds;
        //}
        ///// <summary>
        ///// fetching the bank holder name for select bank 
        ///// </summary>
        ///// <param name="bank name"></param>
        ///// <returns>returns the account holder name with last four digits for the selected bank</returns>
        //public async Task<DataSet> ListBankHolderNameAsyncVP(Accountant ObjA)
        //{
        //    Dictionary<String, String> Param = new Dictionary<String, String>();
        //    Param.Add("@Flag", "ListBankACHolderAsyncVP");
        //    Param.Add("@BankName", ObjA.BankName);
        //    Param.Add("@BranchCode", ObjA.BranchCode);
        //    DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTAccountant", Param);
        //    return ds;
        //}
        #endregion
        #endregion //vishals region ends here



        #region  // Cash and bank - Ajay Narkhedkar

        /// <summary>
		/// Show Total Bank Accounts for that Perticular Branch
		/// </summary>
		/// <returns>List of Bank Accounts with Details</returns>
		/// 
		public async Task<DataSet> BankAccountsListAN(Accountant obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "BankAccountsListAN");
            Param.Add("@BranchCode", obj.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTAccountant", Param);
            return ds;
        }

        /// <summary>
        /// Add New Bank Account to the System
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Add New Bank Account</returns>
        public async Task AddNewBankAccountAsyncAN(Accountant obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "AddNewBankAccountAN");
            Param.Add("@BankName", obj.BankName);
            Param.Add("@BankBranch", obj.BankBrach);
            Param.Add("@AccountHolderName", obj.AccountHolderName);
            Param.Add("@AccountNumber", obj.BankAccountNumber.ToString());
            Param.Add("@IFSCCode", obj.IFSCCode);
            Param.Add("@MICRCode", obj.MICRCode);
            Param.Add("@AccountType", obj.AccountType);
            Param.Add("@openingDate", obj.BankAccountOpeningDate.ToString("yyyy-MM-dd"));
            Param.Add("@OpeningBalance", obj.BankAccountOpeningBalance.ToString());
            Param.Add("@MinimumBalance", obj.BankAccountMinimumBalance.ToString());
            Param.Add("@StaffCode", obj.StaffCode);
            await DBHelper.ExecuteStoreProcedure("GSTAccountant", Param);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Showing the bank Details</returns>
        public async Task<SqlDataReader> BankAccountTransactionDetailsAsync(Accountant obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "BankAccountTransactionDetailsAsyncAN");
            Param.Add("@BranchCode", obj.BranchCode);
            Param.Add("@BankId", obj.BankId.ToString());
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTAccountant", Param);
            return dr;
        }

        public async Task<DataSet> CashReceiptListAsyncAN(Accountant obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "CashReceiptListAsyncAN");
            Param.Add("@BranchCode", obj.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTAccountant", Param);
            return ds;
        }

        public async Task<DataSet> CashExpenceListAsyncAN(Accountant obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "CashExpenceListAsyncAN");
            Param.Add("@BranchCode", obj.BranchCode);
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTAccountant", Param);
            return ds;
        }

        public async Task<DataSet> FilterDateCashTransactions(DateTime startDate, DateTime endDate, object BranchCode)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "FilterDateCashTransactions");
            Param.Add("@FromDate", startDate.ToString());
            Param.Add("@ToDate", endDate.ToString());
            Param.Add("@BranchCode", BranchCode.ToString());
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTAccountant", Param);
            return ds;
        }


        public async Task<SqlDataReader> CashRecievedTransferViewAN(Accountant obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "CashRecievedTransferViewAN");
            Param.Add("@TransactionCode", obj.TransactionCode);
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTAccountant", Param);
            return dr;
        }
        
        public async Task<SqlDataReader> CashDeductionTransferViewAN(Accountant obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "CashGoneTransferViewAN");
            Param.Add("@TransactionCode", obj.TransactionCode);
            SqlDataReader dr = await DBHelper.ExecuteStoreProcedureReturnDataReader("GSTAccountant", Param);
            return dr;
        }

        public async Task<DataSet> BankTransactionHistory(Accountant obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "BankAccountTransactionHistory");
            Param.Add("@BankId", obj.BankId.ToString());
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTAccountant", Param);
            return ds;
        }

        public async Task<DataSet> DeleteBankAccountAN(Accountant obj)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();
            Param.Add("@Flag", "DeleteBankAccountAN");
            Param.Add("@BankId", obj.BankId.ToString());
            DataSet ds = await DBHelper.ExecuteStoreProcedureReturnDS("GSTAccountant", Param);
            return ds;
        }

        #endregion
    }
}
