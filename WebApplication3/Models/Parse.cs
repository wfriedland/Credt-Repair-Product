using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net.Http;
using System.Xml.XPath;
using HtmlAgilityPack;

namespace WebApplication3.Models
{
    public class Parse
    {
        string url = "http://johloyd.com/CreditReport_1_Full.html";

        private void Parser()
        {
            
            var getHtmlWeb = new HtmlWeb();
            var document = getHtmlWeb.Load(url);
            var divs = document.DocumentNode.SelectNodes("//div[@class='accountBox']"); //Gets all account box divs
            var divTransunion = document.DocumentNode.SelectNodes("//div[@class='transunion']"); //Gets div for credit score
            var divCreditHeading = document.DocumentNode.SelectNodes("//div[@class='creditHeading']"); //Gets div for report date

            /*--- Credit Summary Report <td> tags ---*/
            var creditSummaryTableTD = divs[4].Descendants("td").ToList();

            /*--- Credit Summary Report Data ---*/
            var totalAccounts = CreditSummary(12);
            var openAccounts = CreditSummary(13);
            var closedAccounts = CreditSummary(14);
            var totalBalance = Convert.ToInt32(creditSummaryTableTD[15].InnerText.Trim().Substring(1));
            var monthlyPayments = Convert.ToInt32(creditSummaryTableTD[16].InnerText.Trim().Substring(1));
            var unsatisfactoryAccounts = CreditSummary(17);
            var derogatoryAccounts = CreditSummary(18);
            var inquiresLast2Years = CreditSummary(19);
            var publicRecords = CreditSummary(20);

            /*--- Credit Score ---*/
            var divCreditScore = divTransunion.Descendants("h1").ToList();
            var creditScore = Convert.ToInt32(divCreditScore[0].InnerText);
            //resultLabel.Text += "Credit Score: " + creditScore + "<br />";

            /*--- Report Date ---*/
            var divReportDate = divCreditHeading.Descendants("p").ToList();
            var reportDateString = divReportDate[0].InnerText;
            string[] seperators = { " " };
            string[] reportDateArray;
            reportDateArray = reportDateString.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
            var reportDate = Convert.ToDateTime(reportDateArray[5]);
            //resultLabel.Text += reportDate + "<br /> <br />";

            /*--- Account History Tables ---*/
            AccountHistory(12);
            AccountHistory(15);
            AccountHistory(18);
            AccountHistory(21);
            AccountHistory(24);
            AccountHistory(27);

        }

        private int CreditSummary(int divIndex)
        {
            var getHtmlWeb = new HtmlWeb();
            var document = getHtmlWeb.Load(url);
            var divs = document.DocumentNode.SelectNodes("//div[@class='accountBox']");
            var creditSummaryTableTD = divs[4].Descendants("td").ToList();

            return Convert.ToInt32(creditSummaryTableTD[divIndex].InnerText);
        }

        private void AccountHistory(int divIndex)
        {
            var getHtmlWeb = new HtmlWeb();
            var document = getHtmlWeb.Load(url);
            var divs = document.DocumentNode.SelectNodes("//div[@class='accountBox']");

            /*--- Account History Table ---*/
            var accountHistoryPTags = divs[divIndex].Descendants("p").ToList(); //12 15 +3
            var accountNumber = accountHistoryPTags[12].InnerText;
            var accountType = accountHistoryPTags[13].InnerText;
            var accountStatus = accountHistoryPTags[14].InnerText;
            var dateOpen = Convert.ToDateTime(accountHistoryPTags[15].InnerText);
            var datelastReported = Convert.ToDateTime(accountHistoryPTags[16].InnerText);
            var creditLimitString = accountHistoryPTags[17].InnerText.Trim();
            var monthlyPaymentString = accountHistoryPTags[18].InnerText.Trim();
            var balance = Convert.ToInt32(accountHistoryPTags[19].InnerText.Trim().Substring(1));
            var pastDueBalanceString = accountHistoryPTags[20].InnerText.Trim();

            var creditLimit = -1;
            var monthlyPayment = -1;
            var pastDueBalance = -1;

            if (creditLimitString.Contains("-") || creditLimitString == "0")
            {
                creditLimitString = "0";
                creditLimit = int.Parse(creditLimitString);
            }
            else
            {
                creditLimit = Convert.ToInt32(accountHistoryPTags[17].InnerText.Trim().Substring(1));
            }

            if (monthlyPaymentString.Contains("-") || monthlyPaymentString == "0")
            {
                monthlyPaymentString = "0";
                monthlyPayment = int.Parse(monthlyPaymentString);
            }
            else
            {
                monthlyPayment = Convert.ToInt32(accountHistoryPTags[18].InnerText.Trim().Substring(1));
            }

            if (pastDueBalanceString.Contains("-") || pastDueBalanceString == "0")
            {
                pastDueBalanceString = "0";
                pastDueBalance = int.Parse(pastDueBalanceString);
            }
            else
            {
                pastDueBalance = Convert.ToInt32(accountHistoryPTags[20].InnerText.Trim().Substring(1));
            }

        }

    }
}