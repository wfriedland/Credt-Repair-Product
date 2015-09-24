//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication3.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserCreditSummary
    {
        public int summaryID { get; set; }
        public Nullable<int> totalAccounts { get; set; }
        public Nullable<int> openAccounts { get; set; }
        public Nullable<int> closedAccounts { get; set; }
        public Nullable<decimal> totalBalance { get; set; }
        public Nullable<decimal> monthlyPayments { get; set; }
        public Nullable<int> unsatisfactoryAccounts { get; set; }
        public Nullable<int> derogatoryAccounts { get; set; }
        public Nullable<int> inquiresLast2Years { get; set; }
        public Nullable<int> publicRecords { get; set; }
        public string userID { get; set; }
        public Nullable<System.DateTime> reportDate { get; set; }
        public string creditScore { get; set; }
    }
}