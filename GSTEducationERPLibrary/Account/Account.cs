using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace GSTEducationERPLibrary.Account
{
    public class Account
    {
        #region variable declare

        [Required]
        [EmailAddress]
        [StringLength(150)]
        [Display(Name = "Email Id ")]

        public string EmailId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(150, MinimumLength = 8)]
        [Display(Name = "Password ")]
        public string Password { get; set; }

        public String Count { get; set; }

        #endregion
    }
}