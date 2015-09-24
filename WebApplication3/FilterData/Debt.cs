using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3
{
    public class Debt
    {
        //public Guid userId { get; set; }
        //public bool autopay { get; set; }
        //public DateTime autopayDate { get; set; }
        //public string debtGoodOrBad { get; set; }
        public string debtCompanyName { get; set; }
        //public string debtCompanyAddress { get; set; }
        //public string debtCompanyPhone { get; set; }
        public string nickname { get; set; }
        public string accountNumber { get; set; }
        //public string accountType { get; set; }
        //public string accountStatus { get; set; }
        //public DateTime dateOpened { get; set; }
        //public DateTime dateLastReported { get; set; }
        //public double creditLimit { get; set; }
        //public double monthlyPayment { get; set; }
        public double balance { get; set; }
        public double pastDueBalance { get; set; }
        //public List<string> paymentHistory { get; set; }
        //public string comments { get; set; }
        //public string remarks { get; set; }
        //public DateTime reportDate { get; set; }
    }
}