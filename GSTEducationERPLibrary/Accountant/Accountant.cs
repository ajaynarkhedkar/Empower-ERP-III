using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GSTEducationERPLibrary.Accountant
{
	public class Accountant
	{
        //------------------SHREYAYAS Voucher Start --------------------------------------------------------------//
        public int VoucherId { get; set; }
        public string VoucherCode { get; set; }
        public string VendorName { get; set; }
        public float Amount { get; set; }
        public string AmountPaidTo { get; set; }
        public string Description { get; set; }
        public string PaymentMode { get; set; }
        public int BankId { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Cheque Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ChequeDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Cheque Clearence Date")]
        public DateTime ChequeClearenceDate { get; set; }
        public long ReceiverBankAccountNumber { get; set; }
        public string ReceiverBankAccountHolderName { get; set; }
        public string ReceiverBankIFSCCode { get; set; }
        public string ReceiverBankName { get; set; }
        public float Balance { get; set; }
        public string Currency { get; set; }
        public string TransactionId { get; set; }
        public string VoucherType { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Voucher Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime VoucherDate { get; set; }
        public string StaffCode { get; set; }
        public int StatusId { get; set; }
        public List<Accountant> lstVoucher { get; set; }
	public List<Accountant> lstPendingVoucher { get; set; }
        //------------------SHREYAYAS Voucher Start --------------------------------------------------------------//
        //----------------------------------------vishla's properties here-----------------------------------------------------------------------------------
        #region
        /// <summary>
        /// vishals properties starts from here 
        /// </summary>
        public string BranchCode { get; set; }

        /// <summary>
        /// purchased Items for purchased items view
        /// </summary>
        [DisplayName("Bill Number")]
        public string BillNumber { get; set; }
        [DisplayName("Purchase Code")]
        public string PurchaseCode { get; set; }
        public int? ItemId { get; set; }
        [DisplayName("Item Name")]
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        [DisplayName("Unit Price")]
        public decimal UnitPrice { get; set; }
        public double Discount { get; set; }
        [DisplayName("HSN Code")]
        public string HSNCode { get; set; }
        [DisplayName("Applied Tax")]
        public string AppliedTax { get; set; }
        public string Tax { get; set; }
        /// <summary>
        /// properties for the purchase  table here
        /// </summary>
        public string TransactionCode { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Transaction Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime TransactionDate { get; set; }
        [DisplayName("Paid amount")]
        public double TransactionAmount { get; set; }
        [DisplayName("Balance Amount")]
        public double BalanceAmount { get; set; }
        [DisplayName("Bank Type")]
        public string BankType { get; set; }
        public string Status { get; set; }
        [DisplayName("TransactionId")]
        public string TranId_CheqNo { get; set; }

        public List<Accountant> lstPurchaseVP = new List<Accountant>();
        public List<Accountant> lstPurchaseItemVP = new List<Accountant>();
        public List<Accountant> lstTransactionVP = new List<Accountant>();
        #endregion
        //---------------------------vishals properties ends here-----------------------------------------------------------------------------------------------



        #region Variable Declare -- Cash and Bank -- Ajay Narkhedkar

        [RegularExpression(@"^[A-Za-z][A-Za-z0-9 ]{0,249}$", ErrorMessage = "Bank name must start with a letter and can only contain letters, numbers, and spaces. Maximum length is 250 characters.")]
        [Required(ErrorMessage = "Bank Name is required.")]
        public string BankName { get; set; }


        [RegularExpression(@"^[A-Za-z ]{1,250}$", ErrorMessage = "Account holder name can only be characters with max 250 characters.")]
        [Required(ErrorMessage = "Bank Account Holder Name is Requered")]
        public string AccountHolderName { get; set; }

        [RegularExpression(@"^[0-9]{9,18}$", ErrorMessage = "Bank account number must be numeric with 9-18 digits.")]
        [Required(ErrorMessage = "Account Number is Requered")]
        public Int64 BankAccountNumber { get; set; }

        [RegularExpression(@"^[A-Za-z ]{1,250}$", ErrorMessage = "Bank branch can only be characters with max 250 characters.")]
        [Required(ErrorMessage = "Bank Branch is Requered")]
        public string BankBranch { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Bank amount must be non-negative.")]
        public float BankAmount { get; set; }

        [Required(ErrorMessage = "Bank Type is Requered")]
        public string AccountType { get; set; }

        public List<Accountant> lstBankAccounts { get; set; }

        [RegularExpression(@"^[A-Za-z]{4}[A-Za-z0-9]{7}$", ErrorMessage = "IFSC code must start with 4 characters followed by alphanumeric characters.")]
        [Required(ErrorMessage = "Bank IFSC Code is required.")]
        public string IFSCCode { get; set; }


        [RegularExpression(@"^[0-9]{9}$", ErrorMessage = "MICR code must be numeric with exactly 9 digits.")]
        [Required(ErrorMessage = "MICR Code is Requered")]
        public string MICRCode { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Account Opening Date is Requered")]
        public DateTime BankAccountOpeningDate { get; set; }

        [Required(ErrorMessage = "Account Opening Balace is Requered")]
        [Range(0, float.MaxValue, ErrorMessage = "Opening balance must be non-negative.")]
        public float BankAccountOpeningBalance { get; set; }

        [Required(ErrorMessage = "Account Minimum Balace is Requered")]
        [Range(0, float.MaxValue, ErrorMessage = "Minimum balance must be non-negative.")]
        public float BankAccountMinimumBalance { get; set; }
        public string BankBrach { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public string StudentName { get; set; }
        public List<Accountant> lstCashList { get; set; }
        public string FeeCollectionCode { get; set; }
        public List<Accountant> lstBankTransactions { get; set; }
        public string StaffName { get; set; }
        public int TransactionCount { get; set; }
        public string EmailId { get; set; }
        public string TransactionType { get; set; }
        public string Rupees { get; set; }
        public string ExpenseCategory { get; set; }

        #endregion

    }
}
