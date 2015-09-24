using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WebApplication3.Models
{
    /*
     * The creditReportReader takes an html file and parses out credit report information.
     * 
     * There are currently two database tables that store credit reprort information:
     * 1) creditSummary - Stored information that is reported once per credit report
     * 2) UserDebtDetail - Information that represents Account History Reports. There could be any
     * number of account history reports in a credit report.
     * 
     * Why not use html agility pack ??? - Because this html file is structured in way that was to
     * traverse in a generic way. Most information in this report is contained in <div class="AccountBox"> sections.
     * The table parameters are defined in the <div class="AccountBoxLeft"> section while the values are
     * defined in the <div class="AccountBoxRight"> section. The reader must interpret the parameters to 
     * figure out which table is being parsed.
     * 
     * The solution to this problem was for the creditReportReader to use a state machine to keep track of 
     * the information being reported.
     */
    public class creditReportReader
    {
        public enum creditTables { acctSummary, acctHistory, none}
        public enum htmlState { seek, readLabel, readValue, comment, creditScore, reportDate, paymentHistory1, paymentHistory2 }
        const string tableLabels = "<div class=\"accountboxleft";
        const string tableValues = "<div class=\"accountboxright";
        const string para = "<p>";
        const string h2 = "<h2>";
        const string li = "<li>";
        const string creditComment = "<div class=\"experian-plus-";
        const string creditorPhone = "<p class=\"account_creditor_phone";
        const string creditorAddr ="<p class=\"account_creditor_address";
        const string creditorName = "<div class=\"mark";
        const string endDiv = "</div";
        const string creditScore = "<div class=\"credit-score-main";
        const string reportDate = "<div class=\"creditheading";
        const string paymentHistory = "<h3>2 Years Payment History";
        
        protected StreamReader cr;
        protected UserCreditSummary creditSum;
        protected List<UserDebtDetail> debtDetails;
        protected UserDebtDetail newdebtDetails = null; 
        protected htmlState state;
        protected creditTables tableType;
        protected string line, tmpCreditorName;
        protected int paraCnt , lineNum;
        protected bool tagContinuation;
        protected string strippedLine { get; set; }
        public creditReportReader()
        { }
            
        /*
         * The init method takes the full path of a html file and opens it.
         * The return value is true if the file can be opened sucessfully.
         * The return value is false if the file can not be opened.
         */
        public bool init (string file)
        {
            try
            {
                cr = new StreamReader(file);

                if (cr == null)
                {
                  //  Console.WriteLine("Error Opening file "+file);
                    return false;
                }
                /*
                 * We are now ready to parse a html credit report
                 */
                state = htmlState.seek;
                tableType = creditTables.none;
                paraCnt = 0;
                lineNum = 0;
                creditSum = new UserCreditSummary();
                debtDetails = new List<UserDebtDetail>();
                tagContinuation = false;
                return true;
            }
            catch (Exception e)
            {
                // Console.WriteLine(e.Message);
                return false;
            }
        }
        /*
         * Method stripLine strips away the html tags from a line and returns the
         * text within. The class strippedLine line is alway updated with the stripped
         * contents.
         * 
         * Note: The character '$' is filtered out because it prevents the decoding of UserDebtDetail
         * money values.
         */
        protected string stripLine()
        {
            bool skip = false;
            strippedLine = "";
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '<') skip= true;
                if (skip == false && line[i] != '$'){
                    strippedLine += line[i];
                } else if (line[i] == '>') skip = false;
            }
            return strippedLine;
        }

        /*
         * Method isPtag() is necessay to parse <p> elements where the content
         * is on a seperate line than the tag
         * 
         */
        // isPtag currently does not support multible lines of text only. This function will
        // be improved in the next version.
        protected bool isPtag()
        {
            stripLine();
            if (line.StartsWith(para))
            {
                if (strippedLine == "")
                    tagContinuation = true;
                return true;
            } else if (tagContinuation == true)
            {
                tagContinuation = false;
                return true;
            }
            return false;
        }
        /*
         * Method parseAcctHistory loads the newdeptDetails structure. After the last
         * element has been loaded, the structure is added to the list of UserDebtDetails
         */
        protected void parseAcctHistory()
        {
            decimal num;
            if (strippedLine == "") return;
            switch (paraCnt)
            {
                case 9: 
                    newdebtDetails.accountNumber = strippedLine;
                    break;
                case 8:
                    newdebtDetails.accountType = strippedLine;
                    break;
                case 7:
                    newdebtDetails.accountStatus = strippedLine;
                    break;
                case 6:
                    newdebtDetails.dateOpened = Convert.ToDateTime(strippedLine).Date;
                    break;
                case 5:
                    newdebtDetails.dateLastReported = Convert.ToDateTime(strippedLine).Date;
                    break;
                case 4:
                    if (decimal.TryParse(strippedLine, out num) == true)
                        newdebtDetails.creditLimit = num;
                    else
                        newdebtDetails.creditLimit = 0.0m;
                    break;
                case 3:
                    if (decimal.TryParse(strippedLine, out num) == true)
                        newdebtDetails.monthlyPayment = num;
                    else
                        newdebtDetails.monthlyPayment = 0.0m;
                    break;
                case 2:
                    if (decimal.TryParse(strippedLine, out num) == true)
                        newdebtDetails.balance = num;
                    else
                        newdebtDetails.balance = 0.0m;
                    break;
                case 1:
                    newdebtDetails.pastDueBalance = strippedLine;
                    newdebtDetails.reportDate = creditSum.reportDate;
                    debtDetails.Add(newdebtDetails);
                    state = htmlState.seek;
                    break;
            }
            paraCnt--;
        }

        /*
         * Method parseAcctSummary loads the creditSummary structure
         * 
         */
        protected void parseAcctSummary()
        {
            decimal num;
            int number;
            switch (paraCnt)
            {
                case 9:
                    if (int.TryParse(strippedLine, out number) == true)
                        creditSum.totalAccounts = number;
                    else
                        creditSum.totalAccounts = 0;
                    break;
                case 8:
                    if (int.TryParse(strippedLine, out number) == true)
                        creditSum.openAccounts = number;
                    else
                        creditSum.openAccounts = 0;
                    break;
                case 7:
                    if (int.TryParse(strippedLine, out number) == true)
                        creditSum.closedAccounts = number;
                    else
                        creditSum.closedAccounts = 0;
                    break;
                case 6:
                    if (decimal.TryParse(strippedLine, out num) == true)
                        creditSum.totalBalance = num;
                    else
                        creditSum.totalBalance = 0.0m;
                    break;
                case 5:
                    if (decimal.TryParse(strippedLine, out num) == true)
                        creditSum.monthlyPayments = num;
                    else
                        creditSum.monthlyPayments = 0.0m;
                    break;
                case 4:
                    if (int.TryParse(strippedLine, out number) == true)
                        creditSum.unsatisfactoryAccounts = number;
                    else
                        creditSum.unsatisfactoryAccounts = 0;
                    break;
                case 3:
                    if (int.TryParse(strippedLine, out number) == true)
                        creditSum.derogatoryAccounts = number;
                    else
                        creditSum.derogatoryAccounts = 0;
                    break;
                case 2:
                    if (int.TryParse(strippedLine, out number) == true)
                        creditSum.inquiresLast2Years = number;
                    else
                        creditSum.inquiresLast2Years = 0;
                    break;
                case 1:
                    if (int.TryParse(strippedLine, out number) == true)
                        creditSum.publicRecords = number;
                    else
                        creditSum.publicRecords = 0;
                    state = htmlState.seek;
                    break;
            }
            paraCnt--;
        }
        /*
         * Lender address is given to us in a single string and we
         * must parse it into street address, town, state, and zip.
         */
        protected void parseLenderAddress()
        {
            int blankCnt = 0, idx=0;
            char[] zip = { ' ', ' ', ' ', ' ', ' ' };
            char[] state = { ' ', ' ' };
            StringBuilder street = new StringBuilder();
            stripLine();
            while (blankCnt < 3)
            {
                street.Append(strippedLine[idx]);
                if (strippedLine[idx] == ' ') blankCnt++;
                idx++;
            }
            newdebtDetails.debtCompanyStreeAddress = street.ToString().ToUpper();
            street.Clear();
            while (strippedLine[idx] == ' ') idx++;
            blankCnt = 0;
            while (blankCnt < 3)
            {
                street.Append(strippedLine[idx]);
                if (strippedLine[idx] == ' ') blankCnt++;
                idx++;
            }
            newdebtDetails.debtCompanyCtiy = street.ToString().ToUpper();
            var zipStart = strippedLine.Length - 5;
            var stateStart = strippedLine.Length - 8;
            strippedLine.CopyTo(stateStart, state, 0, 2);
            var stateStr = new string(state);
            newdebtDetails.debtCompanyState = stateStr.ToUpper();
            strippedLine.CopyTo(zipStart, zip, 0, 5);
            var zipStr = new string(zip);
            newdebtDetails.debtCompanyZip = zipStr; 


    
        }

        /*
         * Credit Report State Machine.
         * 
         * Read the html credit report one line at time. Based on <div class= and
         * other html landmarks, navigate through the credit report and store its
         * contents in the creditSummary and UserDebtDetail classes.
         * 
         */
        public void parse()
        {
            while ((line = cr.ReadLine()) != null)
            {
                line = line.Trim().ToLower();
                lineNum++;
                switch (state)
                {
                    case htmlState.seek:
                        {
                            if (line.StartsWith(tableLabels)) {
                                state = htmlState.readLabel;
                                paraCnt = 0;
                            } else if (line.StartsWith(creditComment)) {
                                state = htmlState.comment;
                            }
                            else if (line.StartsWith(creditScore))
                            {
                                state = htmlState.creditScore;
                            } else if (line.StartsWith(reportDate))
                            {
                                state = htmlState.reportDate;
                            }

                            break;
                        }
                    case htmlState.readLabel:
                        {
                            if (line.StartsWith(creditorName))
                            {
                                tmpCreditorName = stripLine();
                            }
                            else if (line.StartsWith(creditorPhone))
                            {
                                newdebtDetails = new UserDebtDetail();
                                newdebtDetails.debtCompanyName = tmpCreditorName;
                                newdebtDetails.debtCompanyPhone = stripLine();
//                                Console.WriteLine("Creditor Phone: " + line());
                            }
                            else if (line.StartsWith(creditorAddr))
                            {
                                parseLenderAddress();
                          //      newdebtDetails.debtCompanyAddress = stripLine();
//                                Console.WriteLine("Creditor Address: " + line);
                            }
                            else if (line.StartsWith(para))
                            {
                                paraCnt++;
                                if (paraCnt == 1)
                                {
                                    if (stripLine() == "account #")
                                    {
                                        tableType = creditTables.acctHistory;

                                    }
                                    else if (strippedLine == "total accounts")
                                    {
                                        tableType = creditTables.acctSummary;

                                    } else {
                                        state = htmlState.seek;
                                    }

                                }
                             //   Console.WriteLine(lineNum.ToString()+" Read label: "+paraCnt.ToString()+" "+stripLine());
                            }
                            else if (line.StartsWith(tableValues))
                            {
                                state = htmlState.readValue;
                            } 
                            break;
                        }
                    case htmlState.readValue:
                        {
                            if (isPtag() == true)
                            {
                                if (tableType == creditTables.acctHistory) parseAcctHistory();
                                else if (tableType == creditTables.acctSummary) parseAcctSummary();
                            }
                            break;
                        }
                    case htmlState.comment:
                        {
                            /*
                             * Currently there is no place defined for this information.
                             * I am leaving it here because it might be used in the future
                             */
                            if (line.StartsWith(h2) || line.StartsWith(li))
                            {
//                                Console.WriteLine(lineNum.ToString() + " Comment: " + stripLine());
                            }
                            else if (line.StartsWith(endDiv)) state = htmlState.seek;
                            break;
                        } 
                    case htmlState.creditScore:
                        {
                            if (line.StartsWith("<h1"))
                            {
                                creditSum.creditScore = stripLine();
                                state = htmlState.seek;
                            }
                            break;
                        }
                    case htmlState.reportDate:
                        {
                            if (line.StartsWith(para))
                            {
                                // This is really Ugly. Brute strength is the name of the game;
                                stripLine();
                                string reportD = "";
                                for (int i = 0; i < strippedLine.Length; i++)
                                {
                                    if ((strippedLine[i] >= '0' &&
                                         strippedLine[i] <= '9') ||
                                         strippedLine[i] == '/')
                                    {
                                        reportD += strippedLine[i];
                                    }
                                }
                                creditSum.reportDate = Convert.ToDateTime(reportD).Date;
                                state = htmlState.seek;
                            }
                        }
                        break;
                     /*
                      * The following two cases are designed to caputure the 2 year payment history information.
                      * This feature will be provided in a future version of this program.
                      */
                    case htmlState.paymentHistory1:
                        {
                            if (line.StartsWith(paymentHistory))
                                state = htmlState.paymentHistory2;
                        }
                        break;
                    case htmlState.paymentHistory2:
                        {
                            if (line.StartsWith(paymentHistory))
                                state = htmlState.paymentHistory2;
                        }
                        break;
                    default:
                        {
                        }
                        break;
                }
            }
        }
        
        /*
         * Functions to report database tables after parse is complete
         */
        public UserCreditSummary getCreditSummary()
        {
            return creditSum;
        }
        public List<UserDebtDetail> getCreditDetails()
        {
            return debtDetails;
        }
    }
}
