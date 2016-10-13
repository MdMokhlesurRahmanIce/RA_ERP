using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using RASolarERP.DomainModel.AMSModel;
using System.Data.SqlClient;
using System.Collections;
using System.Transactions;

using RASolarHelper;

namespace RASolarAMS.Model
{
    public class RASolarAMSRepository : IRASolarAMSRepository
    {
        #region Properties And Constructor

        private RASolarAMSEntities AccountContext { get; set; }

        public RASolarAMSRepository(RASolarAMSEntities _AccountContext)
        {
            AccountContext = _AccountContext;
        }

        #endregion

        #region Read Method

        public IQueryable<Acc_ChartOfAccounts> ReadChartOfAccount()
        {
            return AccountContext.Acc_ChartOfAccounts;
        }

        public byte ChartOfAccountLevelMax()
        {
            return AccountContext.Acc_ChartOfAccounts.Max(m => m.AccountLevel);
        }

        public Common_ProjectInfo ReadProject(string projectCode)
        {
            return AccountContext.Common_ProjectInfo.Where(p => p.ProjectCode == projectCode).FirstOrDefault();
        }

        public List<ProjectInfo> ReadProject()
        {
            var projects = from pi in AccountContext.Common_ProjectInfo
                           where pi.Status == Helper.Active
                           select new ProjectInfo
                           {
                               ProjectCode = pi.ProjectCode,
                               ProjectName = pi.ProjectCode + " - " + pi.ProjectName
                           };

            return projects.ToList();
        }

        public List<TrialBalanceReport> ReadTrialBalance(string reportType, string locationCode, string startDate, string endDate, string projectCode)
        {
            string strTrialBalanceReport = string.Empty;

            if (string.IsNullOrEmpty(locationCode))
                locationCode = Helper.DBNullValue;
            else
                locationCode = "'" + locationCode + "'";

            if (string.IsNullOrEmpty(projectCode))
                projectCode = Helper.DBNullValue;
            else
                projectCode = "'" + projectCode + "'";

            List<TrialBalanceReport> lstTrialBalanceReport = new List<TrialBalanceReport>();
            strTrialBalanceReport = "EXEC REP_AccTrialBalanceV2 '" + reportType + "', " + locationCode + ",'" + startDate + "','" + endDate + "'," + projectCode;
            try
            {
                ((System.Data.Entity.Infrastructure.IObjectContextAdapter)AccountContext).ObjectContext.CommandTimeout = 300;
                var v = AccountContext.Database.SqlQuery<TrialBalanceReport>(strTrialBalanceReport);
                return v.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<AccountBalanceSheet> ReadAccountBalanceSheet(string onDate)
        {
            return AccountContext.AccountBalanceSheet(2, "BALANSHEET", 1, Convert.ToDateTime(onDate), "").OrderBy(ab => ab.ParCapSub).ToList(); //ReportHeadNo
        }

        public List<AccIncomeStatementForMonthlyMIS> ReadAccIncomeStatementForMonthlyMIS(string fromDate, string toDate)
        {
            return AccountContext.AccIncomeStatementForMonthlyMIS(fromDate, toDate).ToList();
        }
        public List<AccountingDataEntryStatus> ReadAccountingDataEntryStatus(string reportType, string locationCode, string yearMonth)
        {
            ((System.Data.Entity.Infrastructure.IObjectContextAdapter)AccountContext).ObjectContext.CommandTimeout = 300;
            return AccountContext.AccountingDataEntryStatus(reportType, locationCode, yearMonth).ToList();
        }
        public List<AccShowAccountWiseBreakdown> ReadAccShowAccountWiseBreakdown(string locationCode, string yearMonth, string transType)
        {
            return AccountContext.AccShowAccountWiseBreakdown(locationCode, yearMonth, transType).ToList();
        }
        public MonthlyAccountingSummaryStatement ReadMonthlyAccountingSummaryStatement(string locationCode, string fromDate, string toDate)
        {
            return AccountContext.MonthlyAccountingSummaryStatement(locationCode, fromDate, toDate).FirstOrDefault();
        }
        public List<GLAccountLedger> ReadGLAccountLedger(string LocationCode, string FromDate, string ToDate, string AccountNo, string ProjectCode)
        {
            string strGLAccountLedger = string.Empty;
            List<GLAccountLedger> lstGLAccountLedger = new List<GLAccountLedger>();
            strGLAccountLedger = "EXEC REP_GLAccountLedger '" + LocationCode + "','" + FromDate + "','" + ToDate + "','" + AccountNo + "','" + ProjectCode + "'";

            try
            {
                ((System.Data.Entity.Infrastructure.IObjectContextAdapter)AccountContext).ObjectContext.CommandTimeout = 500;
                return AccountContext.Database.SqlQuery<GLAccountLedger>(strGLAccountLedger).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<ChartOfAccounts> ReadChartOfAccounts()
        {
            string chartOfAccounts = string.Empty;
            List<ChartOfAccounts> lstChartOfAccounts = new List<ChartOfAccounts>();
            //chartOfAccounts = "SELECT  a.AccountNo  ,a.AccountName +'( '+ a.AccountNo +' )' AccountName " +
            //                  "FROM Acc_ChartOfAccounts a LEFT JOIN Acc_ChartOfAccounts b  ON a.ParentAccountNo=b.AccountNo " +
            //                  " WHERE  a.ParCapSub='S' "; [Comment by ZIR on 03-Sep-2013

            chartOfAccounts = "SELECT AccountNo,AccountName +' ('+ AccountNo +')' AccountName " +
                  "FROM Acc_ChartOfAccounts " +
                  "WHERE ParCapSub = 'S' AND ";


            try
            {
                return AccountContext.Database.SqlQuery<ChartOfAccounts>(chartOfAccounts).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public List<ChartOfAccounts> ReadChartOfAccounts(string locationCode, string projectCode, string isValidforLocation, string accountParCapSub)
        {
            string whereCondition = string.Empty;

            if (isValidforLocation == Helper.HeadOffice)
            {
                whereCondition = "AND IsValidForHeadOffice = 1";
            }
            else if (isValidforLocation == Helper.Zone)
            {
                whereCondition = "AND IsValidForZonalOffice = 1";
            }
            else if (isValidforLocation == Helper.Unit)
            {
                whereCondition = "AND IsValidForUnitOffice = 1";
            }

            string chartOfAccounts = " SELECT AccountNo, AccountName + ' ('+ AccountNo +')' AccountName " +
                                     " FROM Acc_ChartofAccounts " +
                                     " WHERE (OnlyForLocation IS NULL OR OnlyForLocation = '" + locationCode + "') AND " +
                                     " (OnlyForProject IS NULL OR OnlyForProject = '" + projectCode + "') AND " +
                                     " OnlyForAutoEntry = 0 AND " +
                                     " ParCapSub='" + accountParCapSub + "' " +
                                       whereCondition;
            try
            {
                return AccountContext.Database.SqlQuery<ChartOfAccounts>(chartOfAccounts).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<ChartOfAccounts> ReadCashBankAccount(string locationCode, string projectCode, string isValidforLocation, string accountParCapSub)
        {
            string andCondition = string.Empty;

            if (isValidforLocation == Helper.HeadOffice)
            {
                andCondition = " AND ca.IsValidForHeadOffice = 1 ";
            }
            else if (isValidforLocation == Helper.Zone)
            {
                andCondition = " AND ca.IsValidForZonalOffice = 1 ";
            }
            else if (isValidforLocation == Helper.Unit)
            {
                andCondition = " AND ca.IsValidForUnitOffice = 1 ";
            }

            //string chartOfAccounts = " SELECT ca.AccountNo, ca.AccountName + ' ('+ ca.AccountNo +')' AccountName " +
            //            " FROM Acc_ChartOfAccounts ca INNER JOIN  " +
            //            " (SELECT * FROM [Acc_SpecialAccount] WHERE SAIID IN (1,2)) sa  " +
            //            " ON sa.AccountNo = ca.AccountNo        " +
            //            " WHERE (ca.OnlyForLocation IS NULL OR ca.OnlyForLocation = '" + locationCode + "') " +
            //            " AND (ca.OnlyForProject IS NULL OR ca.OnlyForProject = '" + projectCode + "') " +
            //            " AND ca.ParCapSub = '" + accountParCapSub + "'" +
            //              whereCondition;

            string chartOfAccount = "SELECT ca.[AccountNo],ca.AccountName " +
                               " FROM Common_LocationWiseBankAccountInfo wi INNER JOIN Acc_ChartOfAccounts ca ON ca.AccountNo = wi.GLAccountNo " +
                               " WHERE wi.LocationCode = '" + locationCode + "' " +
                               " UNION ALL " +
                               " (SELECT ca.AccountNo,ca.AccountName FROM Acc_ChartOfAccounts ca INNER JOIN " +
                                       " (SELECT * FROM [Acc_SpecialAccount] WHERE SAIID IN (1)) sa ON sa.AccountNo = ca.AccountNo " +
                                " WHERE (ca.OnlyForLocation IS NULL OR ca.OnlyForLocation ='" + locationCode + "') " +
                                " AND (ca.OnlyForProject IS NULL OR ca.OnlyForProject ='" + projectCode + "' OR ca.OnlyForProject ='" + projectCode + "') " +
                                " " + andCondition + " " +
                                " AND ca.ParCapSub = '" + accountParCapSub + "' " +
                                " ) " +
                                " ORDER BY AccountName ";
            try
            {
                return AccountContext.Database.SqlQuery<ChartOfAccounts>(chartOfAccount).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<ChartOfAccounts> ReadCashBankAccountWithDimension(string locationCode, string projectCode, string isValidforLocation, string accountParCapSub)
        {
            string andCondition = string.Empty;

            if (isValidforLocation == Helper.HeadOffice)
            {
                andCondition = " AND ca.IsValidForHeadOffice = 1 ";
            }
            else if (isValidforLocation == Helper.Zone)
            {
                andCondition = " AND ca.IsValidForZonalOffice = 1 ";
            }
            else if (isValidforLocation == Helper.Unit)
            {
                andCondition = " AND ca.IsValidForUnitOffice = 1 ";
            }

            string chartOfAccount = string.Format("SELECT cainfo.*, glaccvd.DimensionCode  FROM " +
                                       "(SELECT DISTINCT ca.[AccountNo], ca.AccountName   " +
                                       "FROM Common_LocationWiseBankAccountInfo wi  " +
                                       "INNER JOIN Acc_ChartOfAccounts ca  " +
                                       "ON ca.AccountNo = wi.GLAccountNo   " +
                                       "WHERE wi.LocationCode = '{0}'   " +
                                       "UNION ALL   " +
                                       "	( " +
                                       "		SELECT ca.AccountNo,ca.AccountName FROM Acc_ChartOfAccounts ca  " +
                                       "		INNER JOIN  (SELECT * FROM [Acc_SpecialAccount] WHERE SAIID IN (1)) sa  " +
                                       "					 ON sa.AccountNo = ca.AccountNo   " +
                                       "					 WHERE (ca.OnlyForLocation IS NULL OR ca.OnlyForLocation ='{0}')  AND  " +
                                       "						   (ca.OnlyForProject IS NULL OR ca.OnlyForProject ='{1}' OR ca.OnlyForProject ='{1}') " +
                                       "						    {2}   AND ca.ParCapSub = '{3}'   " +
                                       "	)  " +
                                       ")cainfo " +
                                       "LEFT OUTER JOIN Acc_GLAccountVsDimension glaccvd " +
                                       "ON cainfo.AccountNo = glaccvd.AccountNo "
                , locationCode, projectCode, andCondition, accountParCapSub);

            try
            {
                return AccountContext.Database.SqlQuery<ChartOfAccounts>(chartOfAccount).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<ChartOfAccounts> ReadOtherAccount(string locationCode, string projectCode, string isValidforLocation, string accountParCapSub)
        {
            string andCondition = string.Empty;

            if (isValidforLocation == Helper.HeadOffice)
            {
                andCondition = " AND ca.IsValidForHeadOffice = 1 ";
            }
            else if (isValidforLocation == Helper.Zone)
            {
                andCondition = " AND ca.IsValidForZonalOffice = 1 ";
            }
            else if (isValidforLocation == Helper.Unit)
            {
                andCondition = " AND ca.IsValidForUnitOffice = 1 ";
            }

            //string chartOfAccounts = " SELECT * FROM Acc_ChartOfAccounts ca LEFT OUTER JOIN " +
            //                          " (SELECT * FROM [Acc_SpecialAccount] WHERE SAIID IN (1,2)) sa ON sa.AccountNo = ca.AccountNo " +
            //                          " WHERE (ca.OnlyForLocation IS NULL OR ca.OnlyForLocation = '" + locationCode + "') " +
            //                          " AND (ca.OnlyForProject IS NULL OR ca.OnlyForProject ='" + projectCode + "' OR ca.OnlyForProject = '" + projectCode + "') " +
            //                          " " + andCondition + " " +
            //                          " AND ca.ParCapSub = '" + accountParCapSub + "' " +
            //                          " AND sa.AccountNo IS NULL " +
            //                          " ORDER BY AccountName ";

            string chartOfAccounts = string.Format("SELECT ca.*,GLAccVsDim.DimensionCode FROM Acc_ChartOfAccounts ca  " +
                                                    " LEFT OUTER JOIN   " +
                                                    " (SELECT * FROM [Acc_SpecialAccount] WHERE SAIID IN (1,2) " +
                                                    "     ) sa ON sa.AccountNo = ca.AccountNo   " +
                                                    "     LEFT OUTER JOIN [Acc_GLAccountVsDimension] GLAccVsDim ON ca.AccountNo = GLAccVsDim.AccountNo " +
                                                    "  WHERE (ca.OnlyForLocation IS NULL OR ca.OnlyForLocation = '{0}')   " +
                                                    "    AND (ca.OnlyForProject IS NULL OR ca.OnlyForProject ='{1}')    " +
                                                    "     {2}    " +
                                                    "    AND ca.ParCapSub = '{3}'   " +
                                                    "    AND sa.AccountNo IS NULL   " +
                                                    "    ORDER BY AccountName "
                                                    , locationCode, projectCode, andCondition, accountParCapSub);

            try
            {
                return AccountContext.Database.SqlQuery<ChartOfAccounts>(chartOfAccounts).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<ChartOfAccounts> ReadChartOfAccountsForGLLedger(string locationCode, string projectCode, string isValidforLocation, string accountParCapSub)
        {
            string whereCondition = string.Empty;

            if (isValidforLocation == Helper.HeadOffice)
            {
                whereCondition = "AND IsValidForHeadOffice = 1";
            }
            else if (isValidforLocation == Helper.Zone)
            {
                whereCondition = "AND IsValidForZonalOffice = 1";
            }
            else if (isValidforLocation == Helper.Unit)
            {
                whereCondition = "AND IsValidForUnitOffice = 1";
            }

            // SELECT GLAccountNo [AccountNo],ca.AccountName [AccountName] FROM Common_LocationWiseBankAccountInfo wi
            //      INNER JOIN Acc_ChartOfAccounts ca ON ca.AccountNo = wi.GLAccountNo
            //      WHERE wi.LocationCode = 'ANA003'
            //UNION ALL
            // (SELECT DISTINCT AccountNo, AccountName + ' ('+ AccountNo +')' AccountName  
            //      FROM Acc_ChartofAccounts ca
            //      LEFT OUTER JOIN [RASolarERP].[dbo].[Common_LocationWiseBankAccountInfo] li ON li.GLAccountNo = ca.AccountNo       
            //      WHERE (OnlyForLocation IS NULL OR OnlyForLocation = 'ANA003') 
            //      AND (OnlyForProject IS NULL OR OnlyForProject = '100200' OR OnlyForProject = '100500')
            //      AND  ParCapSub='S' AND IsValidForUnitOffice = 1 
            //      AND li.GLAccountNo IS NULL 
            //      )
            //ORDER BY AccountName


            string chartOfAccounts = " SELECT DISTINCT ca.AccountNo, ca.AccountName + ' ('+ ca.AccountNo +')' AccountName " +
                                     " FROM Acc_ChartofAccounts ca INNER JOIN Common_LocationWiseBankAccountInfo lwbnkaci " +
                                     " ON ca.AccountNo = lwbnkaci.GLAccountNo" +
                                     " WHERE lwbnkaci.LocationCode = '" + locationCode + "'" +
                                     " UNION ALL " +
                                     " ( " +
                                         " SELECT DISTINCT ca.AccountNo, ca.AccountName + ' ('+ ca.AccountNo +')' AccountName " +
                                         " FROM Acc_ChartofAccounts ca " +
                                         " LEFT OUTER JOIN Common_LocationWiseBankAccountInfo lwbnkaci " +
                                         " ON lwbnkaci.GLAccountNo = ca.AccountNo " +
                                         " WHERE (OnlyForLocation IS NULL OR OnlyForLocation = '" + locationCode + "') AND " +
                                         " (OnlyForProject IS NULL OR OnlyForProject = '100500' OR OnlyForProject = '" + projectCode + "') AND " +
                                         " ParCapSub='" + accountParCapSub + "' " +
                                           whereCondition +
                                         " AND lwbnkaci.GLAccountNo IS NULL " +
                                     " ) " +
                                     " ORDER BY AccountName ";
            try
            {
                return AccountContext.Database.SqlQuery<ChartOfAccounts>(chartOfAccounts).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<AccGetMonthlyExpensesInDetail> ReadAccGetMonthlyExpensesInDetail(string locationCode, string projectCode)
        {
            string getMonthlyExpensesInDetail = string.Empty;
            List<AccGetMonthlyExpensesInDetail> lstGetMonthlyExpensesInDetail = new List<AccGetMonthlyExpensesInDetail>();
            getMonthlyExpensesInDetail = "EXEC REP_AccGetMonthlyExpensesInDetail '" + locationCode.ToString() + "','" + projectCode.ToString() + "'";

            try
            {
                return AccountContext.Database.SqlQuery<AccGetMonthlyExpensesInDetail>(getMonthlyExpensesInDetail).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<AccTrialBalanceInDetailByAccount> ReadTrialBalanceInDetailByAccount(DateTime fromDate, DateTime toDate, string projectCode, string accountNo)
        {
            return AccountContext.AccTrialBalanceInDetailByAccount(fromDate, toDate, projectCode, accountNo).ToList();
        }

        public List<VoucherTransaction> GetFinalTransactionDetailListByTransactionNo(string transactionNo, string locationCode)
        {
            string query = "SELECT Acc_FinalTransDetail.LocationCode " +
                            " ,Acc_FinalTransDetail.TransNo TransactionNo " +
                            " ,Acc_FinalTransDetail.AccountNo " +
                            " ,Acc_FinalTransDetail.Particulars " +
                            " ,Acc_FinalTransDetail.Amount " +
                            " ,Acc_FinalTransDetail.ProjectCode " +
                            " ,[AccountName] AccountName " +
                            " ,Acc_FinalTransMaster.TransDate TransactionDate " +
                          " FROM [Acc_FinalTransDetail] " +
                          " LEFT JOIN Acc_FinalTransMaster ON (Acc_FinalTransDetail.TransNo=Acc_FinalTransMaster.TransNo AND Acc_FinalTransDetail.LocationCode=Acc_FinalTransMaster.LocationCode) " +
                          " LEFT JOIN Acc_ChartOfAccounts ON Acc_ChartOfAccounts.AccountNo= Acc_FinalTransDetail.AccountNo " +
                         "WHERE Acc_FinalTransDetail.TransNo='" + transactionNo + "' " +
                         "AND  Acc_FinalTransDetail.LocationCode = '" + locationCode + "'";

            return AccountContext.Database.SqlQuery<VoucherTransaction>(query).ToList();

        }

        public List<AccountIncomeStatement> ReadIncomeStatement(byte chartNo, string reportId, byte reportSetNo, DateTime fromDate, DateTime toDate, string projectCode)
        {
            SqlParameter[] storeParam = new SqlParameter[6];
            storeParam[0] = new SqlParameter("prmChartNo", chartNo);
            storeParam[1] = new SqlParameter("prmReportID", reportId);
            storeParam[2] = new SqlParameter("prmReportSetNo", reportSetNo);
            storeParam[3] = new SqlParameter("prmFromDate", fromDate);
            storeParam[4] = new SqlParameter("prmToDate", toDate);
            storeParam[5] = new SqlParameter("prmProjectCode", projectCode);

            var resultList = AccountContext.Database.SqlQuery<AccountIncomeStatement>("Exec REP_AccIncomeStatement @prmChartNo, @prmReportID, @prmReportSetNo, @prmFromDate, @prmToDate, @prmProjectCode", storeParam);

            return resultList.ToList();
        }

        public ArrayList GetBankAccountForSalaryBoucher(string locationCode)
        {
            var v = from lwba in AccountContext.Common_LocationWiseBankAccountInfo
                    join bif in AccountContext.Common_BankInfo on
                    lwba.BankID equals bif.BankID
                    where lwba.LocationCode == locationCode
                    select new
                    {
                        lwba.BankID,
                        bif.BankName
                    };
            ArrayList arrList = new ArrayList(v.ToList());

            return arrList;
        }

        public ArrayList GetBankAccountForSalaryBoucher()
        {
            var v = from bif in AccountContext.Common_BankInfo
                    where bif.IsItAOnlineBank == true
                    select new
                    {
                        bif.BankID,
                        bif.BankName
                    };
            ArrayList arrList = new ArrayList(v.ToList());

            return arrList;
        }

        public ArrayList GetBankAccountInformation(byte specialAccountType, string locationCode, string projectCode, string isValidforLocation)
        {
            string whereCondition = string.Empty;

            if (isValidforLocation == Helper.HeadOffice)
            {
                whereCondition = "AND ca.IsValidForHeadOffice = 1";
            }
            else if (isValidforLocation == Helper.Zone)
            {
                whereCondition = "AND ca.IsValidForZonalOffice = 1";
            }
            else if (isValidforLocation == Helper.Unit)
            {
                whereCondition = "AND ca.IsValidForUnitOffice = 1";
            }

            string ss = " SELECT ca.AccountNo, ca.AccountName " +
                        " FROM Acc_ChartOfAccounts ca INNER JOIN [Acc_SpecialAccount] sa " +
                        " ON sa.AccountNo = ca.AccountNo " +
                        " WHERE sa.SAIID = " + specialAccountType + " AND " +
                        " (ca.OnlyForLocation IS NULL OR ca.OnlyForLocation = '" + locationCode + "') AND " +
                        " (ca.OnlyForProject IS NULL OR ca.OnlyForProject = '" + projectCode + "')  " +
                        whereCondition;

            var v = AccountContext.Database.SqlQuery<ChartOfAccounts>(ss);

            ArrayList accountList = new ArrayList(v.ToList());

            return accountList;
        }

        public ArrayList ReadVoucherTransNoMax(string locationCode, string yearMonthDate)
        {
            string voucherSequenceSql = string.Format("SELECT * FROM	" +
                                        "(" +
                                        "	( SELECT TOP(1) ISNULL(chm.TransNo,'') TransNo FROM  " +
                                        "	  Acc_PrePostTransMaster chm WHERE chm.LocationCode = '{0}' AND   " +
                                        "	  CONVERT(INT,SUBSTRING(chm.TransNo,1, 6)) = '{1}'  " +
                                        "	  ORDER BY  CONVERT(INT,SUBSTRING(chm.TransNo,1, 6)) DESC," +
                                        "	  CONVERT(INT,SUBSTRING(chm.TransNo,7, LEN(chm.TransNo))) DESC" +
                                        "	 )" +
                                        "  UNION ALL" +
                                        "	( SELECT TOP(1) ISNULL(chm.TransNo,'') TransNo FROM  " +
                                        "	  Acc_FinalTransMaster chm WHERE chm.LocationCode = '{0}' AND   " +
                                        "	  CONVERT(INT,SUBSTRING(chm.TransNo,1, 6)) = '{1}'  " +
                                        "	  ORDER BY  CONVERT(INT,SUBSTRING(chm.TransNo,1, 6)) DESC," +
                                        "	  CONVERT(INT,SUBSTRING(chm.TransNo,7, LEN(chm.TransNo))) DESC " +
                                        "	 )" +
                                        "   )voucherSequence ", locationCode, yearMonthDate);


            var voucherSequence = AccountContext.Database.SqlQuery<VoucherTransactionNumber>(voucherSequenceSql);

            ArrayList sequenceNumbers = new ArrayList();
            foreach (VoucherTransactionNumber tr in voucherSequence)
            {
                sequenceNumbers.Add(tr.TransNo);
            }

            return sequenceNumbers;
        }

        public decimal GetProjectWiseAccountBalance(string locationCode, string projectCode, string accountNumber, bool isIncludePrePostVoucher)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.AppendFormat(" SELECT dbo.[FUNC_GetAvailableBalance]('{0}','{1}','{2}', {3}) ", locationCode, projectCode, accountNumber, isIncludePrePostVoucher);

            var v = AccountContext.Database.SqlQuery<decimal>(strSQL.ToString()).FirstOrDefault();

            return v;
        }

        public List<BankInformation> ReadBankInformation(string locationCode)
        {
            var v = (from bif in AccountContext.Common_BankInfo
                     join lwba in AccountContext.Common_LocationWiseBankAccountInfo
                     on bif.BankID equals lwba.BankID
                     where lwba.LocationCode == locationCode && bif.Status == Helper.Active
                     select new BankInformation
                     {
                         BankID = lwba.BankID,
                         BankName = bif.BankName,
                         BankBranchID = lwba.BankBranchID,
                         BankAccountNumber = lwba.BankAccountNumber,
                         BankAccountTypeID = lwba.BankAccountType,
                         Address = lwba.Address

                     }).ToList();

            return v.GroupBy(g => g.BankID).Select(s => s.First()).ToList();
        }

        public List<BankInformation> ReadBankInformation(string locationCode, string bankId, string bankBranchID)
        {
            var v = (from bif in AccountContext.Common_BankInfo
                     join lwba in AccountContext.Common_LocationWiseBankAccountInfo
                     on bif.BankID equals lwba.BankID
                     where lwba.LocationCode == locationCode &&
                     lwba.BankID == bankId &&
                     lwba.BankBranchID == bankBranchID &&
                     lwba.Status == Helper.Active
                     select new BankInformation
                     {
                         BankID = lwba.BankID,
                         BankName = bif.BankName,
                         BankBranchID = lwba.BankBranchID,
                         BankAccountNumber = lwba.BankAccountNumber,
                         BankAccountTypeID = lwba.BankAccountType,
                         Address = lwba.Address,
                         SalaryDisbursementBankAccountNumber = lwba.BankAccountNumber + " [" + bif.BankName + "]"

                     }).ToList();

            return v.GroupBy(g => g.BankID).Select(s => s.First()).ToList();
        }

        public List<BankBranchInfo> ReadBankBranchInformation(string locationCode, string bankId)
        {
            var v = (from bbi in AccountContext.Common_BankBranchInfo
                     join lwba in AccountContext.Common_LocationWiseBankAccountInfo
                     on new { bbi.BankID, bbi.BankBranchID } equals new { lwba.BankID, lwba.BankBranchID }
                     where bbi.BankID == bankId && lwba.LocationCode == locationCode && bbi.Status == Helper.Active
                     select new BankBranchInfo
                     {
                         BankBranchID = bbi.BankBranchID,
                         BankBranchName = bbi.BankBranchName

                     }).ToList();

            return v.GroupBy(g => g.BankBranchID).Select(s => s.First()).ToList();
        }

        public List<BankAccountType> ReadBankAccountType()
        {
            var v = from bbi in AccountContext.Common_BankAccountType
                    where bbi.Status == Helper.Active
                    select new BankAccountType
                    {
                        BankAccountTypeID = bbi.BankAccountTypeID,
                        BankAccountTypeDescription = bbi.BATypeDescription
                    };

            return v.ToList();
        }

        public List<EmployeeWiseSalaryPayment> ReadEmployeeWiseSalaryPaymentPosting(string queryOption, string locationCode)
        {
            SqlParameter[] storeParam = new SqlParameter[2];
            storeParam[0] = new SqlParameter("prmOption", queryOption);
            storeParam[1] = new SqlParameter("prmSalaryLocationCode", locationCode);
            //storeParam[2] = new SqlParameter("prmSalaryDisbursementBankID", Helper.DBNullValue); 

            var resultList = AccountContext.Database.SqlQuery<EmployeeWiseSalaryPayment>("Exec REP_AccGetEmployeeListForSalaryPaymentPosting @prmOption, @prmSalaryLocationCode", storeParam);

            return resultList.ToList();
        }

        public List<ChartOfAccounts> GetLocationWiseChartOfAccount(string locationCode)
        {
            string query = "SELECT DISTINCT aca.AccountNo, aca.AccountName from Common_LocationWiseBankAccountInfo as clwb " +
                    " right join Acc_ChartOfAccounts  as aca " +
                    " on clwb.GLAccountNo = aca.AccountNo " +
                    " where clwb.LocationCode = '" + locationCode + "' ";

            return AccountContext.Database.SqlQuery<ChartOfAccounts>(query).ToList();
        }

        public List<SubLedgerHeadDetails> GetSubLedgerHeadDetails(string dimensionCode, string locationCode)
        {
            SqlParameter[] storeParam = new SqlParameter[2];
            storeParam[0] = new SqlParameter("prmDimensionCode", dimensionCode);
            storeParam[1] = new SqlParameter("prmLocationCode", locationCode);

            var queryResult = AccountContext.Database.SqlQuery<SubLedgerHeadDetails>("EXEC REP_ComGetDimensionValues @prmDimensionCode, @prmLocationCode", storeParam);

            return queryResult.ToList();
        }

        public bool IsTheDimensionMandatoryExistOrNot(string accountNumber)
        {
            bool isExistOrNot = false;

            try
            {
                Acc_GLAccountVsDimension objObservationIsTheDimensionMandatory = new Acc_GLAccountVsDimension();

                objObservationIsTheDimensionMandatory = AccountContext.Acc_GLAccountVsDimension.Where(a => a.AccountNo == accountNumber).FirstOrDefault();

                if (objObservationIsTheDimensionMandatory != null)
                {
                    isExistOrNot = true;
                }

                return isExistOrNot;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Create Methods

        public Acc_PrePostTransMaster Create(Acc_TransNoCount objAccTransNocount, Acc_PrePostTransMaster objPrePostTransMaster, List<Acc_PrePostTransDetail> lstPrePostTransDetail, Aud_AuditAdjustmentRelatedAccountingTransaction objAuditAdjustmentRelatedAccountingTransaction)
        {
            using (AccountContext)
            {
                using (var Ts = new TransactionScope())
                {
                    try
                    {
                        //string dbSequenceNumber = ReadPrePostTransMasterTransNoMax(objPrePostTransMaster.LocationCode, objPrePostTransMaster.TransNo.Substring(0, 6));
                        //string sequenceNumber = string.Empty;

                        //if (!string.IsNullOrEmpty(dbSequenceNumber))
                        //{
                        //    if (!Helper.ChallanSequenceNumberDuplicationCheck(objPrePostTransMaster.TransNo, dbSequenceNumber))
                        //    {
                        //        sequenceNumber = Helper.AccountSequenceNumberGeneration(dbSequenceNumber);
                        //        objPrePostTransMaster.TransNo = sequenceNumber;

                        //        for (int i = 0; i < lstPrePostTransDetail.Count; i++)
                        //        {
                        //            lstPrePostTransDetail[i].TransNo = sequenceNumber;
                        //        }

                        //        objAuditAdjustmentRelatedAccountingTransaction.TransNo = sequenceNumber;
                        //    }
                        //}

                        AccountContext.Acc_TransNoCount.Add(objAccTransNocount);

                        AccountContext.Acc_PrePostTransMaster.Add(objPrePostTransMaster);

                        foreach (Acc_PrePostTransDetail trd in lstPrePostTransDetail)
                        {
                            AccountContext.Acc_PrePostTransDetail.Add(trd);
                        }

                        AccountContext.Aud_AuditAdjustmentRelatedAccountingTransaction.Add(objAuditAdjustmentRelatedAccountingTransaction);

                        AccountContext.SaveChanges();

                        SqlParameter[] storeParam = new SqlParameter[5];
                        storeParam[0] = new SqlParameter("prmLocationCode", objPrePostTransMaster.LocationCode);
                        storeParam[1] = new SqlParameter("prmTransDate", objPrePostTransMaster.TransDate);
                        storeParam[2] = new SqlParameter("prmProjectCode", objPrePostTransMaster.ProjectCode);
                        storeParam[3] = new SqlParameter("prmTransNo", objPrePostTransMaster.TransNo);
                        storeParam[4] = new SqlParameter("prmDBTransType", Helper.Insert);

                        AccountContext.Database.ExecuteSqlCommand("Exec AE_Aud_AutoPostingOfAccountingTransactionV2 @prmLocationCode, @prmTransDate, @prmProjectCode, @prmTransNo, @prmDBTransType", storeParam);

                        Ts.Complete();

                        return objPrePostTransMaster;
                    }
                    catch (Exception ex)
                    {
                        Ts.Dispose();
                        throw;
                    }
                }
            }
        }

        public Acc_PrePostTransMaster Create(Acc_TransNoCount objAccTransNocount, Acc_PrePostTransMaster objPrePostTransMaster, List<Acc_PrePostTransDetail> lstPrePostTransDetail, Aud_AuditAdjustmentRelatedAccountingTransaction objAuditAdjustmentRelatedAccountingTransaction, List<Acc_PrePostTransDetailByDimension> lstPrePostTransDetailByDimension)
        {
            using (AccountContext)
            {
                using (var Ts = new TransactionScope())
                {
                    try
                    {
                        //string dbSequenceNumber = ReadPrePostTransMasterTransNoMax(objPrePostTransMaster.LocationCode, objPrePostTransMaster.TransNo.Substring(0, 6));
                        //string sequenceNumber = string.Empty;

                        //if (!string.IsNullOrEmpty(dbSequenceNumber))
                        //{
                        //    if (!Helper.ChallanSequenceNumberDuplicationCheck(objPrePostTransMaster.TransNo, dbSequenceNumber))
                        //    {
                        //        sequenceNumber = Helper.AccountSequenceNumberGeneration(dbSequenceNumber);
                        //        objPrePostTransMaster.TransNo = sequenceNumber;

                        //        for (int i = 0; i < lstPrePostTransDetail.Count; i++)
                        //        {
                        //            lstPrePostTransDetail[i].TransNo = sequenceNumber;
                        //        }

                        //        objAuditAdjustmentRelatedAccountingTransaction.TransNo = sequenceNumber;
                        //    }
                        //}

                        AccountContext.Acc_TransNoCount.Add(objAccTransNocount);

                        AccountContext.Acc_PrePostTransMaster.Add(objPrePostTransMaster);

                        foreach (Acc_PrePostTransDetail trd in lstPrePostTransDetail)
                        {
                            AccountContext.Acc_PrePostTransDetail.Add(trd);
                        }

                        foreach (Acc_PrePostTransDetailByDimension dimensionDetails in lstPrePostTransDetailByDimension)
                        {
                            AccountContext.Acc_PrePostTransDetailByDimension.Add(dimensionDetails);
                        }

                        AccountContext.Aud_AuditAdjustmentRelatedAccountingTransaction.Add(objAuditAdjustmentRelatedAccountingTransaction);

                        AccountContext.SaveChanges();

                        SqlParameter[] storeParam = new SqlParameter[5];
                        storeParam[0] = new SqlParameter("prmLocationCode", objPrePostTransMaster.LocationCode);
                        storeParam[1] = new SqlParameter("prmTransDate", objPrePostTransMaster.TransDate);
                        storeParam[2] = new SqlParameter("prmProjectCode", objPrePostTransMaster.ProjectCode);
                        storeParam[3] = new SqlParameter("prmTransNo", objPrePostTransMaster.TransNo);
                        storeParam[4] = new SqlParameter("prmDBTransType", Helper.Insert);

                        AccountContext.Database.ExecuteSqlCommand("Exec AE_Aud_AutoPostingOfAccountingTransactionV2 @prmLocationCode, @prmTransDate, @prmProjectCode, @prmTransNo, @prmDBTransType", storeParam);

                        Ts.Complete();

                        return objPrePostTransMaster;
                    }
                    catch (Exception ex)
                    {
                        Ts.Dispose();
                        throw;
                    }
                }
            }
        }

       // Acc_PrePostTransMaster CreateVoucherForSaveEmployeeWiseSalaryPayment(Acc_TransNoCount objAccTransNocount, Acc_PrePostTransMaster objPrePostTransMaster, List<Acc_PrePostTransDetail> lstPrePostTransDetail, List<Acc_PrePostTransDetailByDimension> lstPrePostTransDetailByDimension, List<EmployeeWiseSalaryPayment> lstEmployeeWiseSalary);
        public Acc_PrePostTransMaster Create(Acc_TransNoCount objAccTransNocount, List<EmployeeWiseSalaryPayment> lstEmployeeWiseSalary, List<Acc_PrePostTransDetail> lstPrePostTransDetail, List<Acc_PrePostTransDetailByDimension> lstPrePostTransDetailByDimension, Acc_PrePostTransMaster objPrePostTransMaster, string supportMethod)
        {
            using (AccountContext)
            {
                using (var Ts = new TransactionScope())
                {
                    try
                    {



                        //string dbSequenceNumber = ReadPrePostTransMasterTransNoMax(objPrePostTransMaster.LocationCode, objPrePostTransMaster.TransNo.Substring(0, 6));
                        //string sequenceNumber = string.Empty;

                        //if (!string.IsNullOrEmpty(dbSequenceNumber))
                        //{
                        //    if (!Helper.ChallanSequenceNumberDuplicationCheck(objPrePostTransMaster.TransNo, dbSequenceNumber))
                        //    {
                        //        sequenceNumber = Helper.AccountSequenceNumberGeneration(dbSequenceNumber);
                        //        objPrePostTransMaster.TransNo = sequenceNumber;

                        //        for (int i = 0; i < lstPrePostTransDetail.Count; i++)
                        //        {
                        //            lstPrePostTransDetail[i].TransNo = sequenceNumber;
                        //        }

                        //        objAuditAdjustmentRelatedAccountingTransaction.TransNo = sequenceNumber;
                        //    }
                        //}





                        //start Hrm_SalaryPayRegisterByMonthNEmployee

                        string message = "";
                        foreach (EmployeeWiseSalaryPayment trdd in lstEmployeeWiseSalary)
                        {
                            Hrm_SalaryPayRegisterByMonthNEmployee objHrm_SalaryPayRegisterByMonthNEmployee = new Hrm_SalaryPayRegisterByMonthNEmployee();

                            //  objHrm_SalaryPayRegisterByMonthNEmployee.EmployeeID = trdd.EmployeeID;
                            //   objHrm_SalaryPayRegisterByMonthNEmployee.YearMonth = trdd.YearMonth;



                            string YearMonth = Helper.YearMonth(trdd.YearMonth);

                            Hrm_SalaryPayRegisterByMonthNEmployee dbRecord = AccountContext.Hrm_SalaryPayRegisterByMonthNEmployee.Where(s => s.EmployeeID == trdd.EmployeeID && s.YearMonth == YearMonth && s.PaidInYearMonth == null).FirstOrDefault();

                            message = trdd.EmployeeID.ToString();


                            dbRecord.PaidSalaryOtherThanTADAAmount = trdd.SalaryOtherThanTADAAmount;
                            dbRecord.PaidTADAAmount = trdd.TADAAmount;
                            dbRecord.PaidBonusAmount = trdd.BonusAmount;
                            dbRecord.PaidTotalSalaryAmount = trdd.TotalSalaryAmount;
                            dbRecord.PaidInYearMonth = Helper.YearMonth(trdd.YearMonth);
                            dbRecord.PaidDate = trdd.TransactionDate;
                            dbRecord.RefAELocationCode = trdd.LocationCode;
                            dbRecord.RefAETransDate = trdd.TransactionDate;
                            dbRecord.RefAEProjectCode = Convert.ToString(trdd.ProjectCode);
                            dbRecord.RefAETransNo = trdd.TransactionNo;

                        }


                        //end Hrm_SalaryPayRegisterByMonthNEmployee




                        AccountContext.Acc_TransNoCount.Add(objAccTransNocount);

                        AccountContext.Acc_PrePostTransMaster.Add(objPrePostTransMaster);

                        foreach (Acc_PrePostTransDetail trd in lstPrePostTransDetail)
                        {
                            AccountContext.Acc_PrePostTransDetail.Add(trd);
                        }

                        if (lstPrePostTransDetailByDimension.Count > 0)
                        {
                            foreach (Acc_PrePostTransDetailByDimension trdd in lstPrePostTransDetailByDimension)
                            {
                                AccountContext.Acc_PrePostTransDetailByDimension.Add(trdd);
                            }
                        }

                        AccountContext.SaveChanges();


                        //start for check Query for valid data in  dimension table


                        string ss =     " SELECT pptd.*,ppdd.Amounts FROM (SELECT LocationCode,TransDate,ProjectCode,TransNo,SerialNo,(Amount) "
                                        +" FROM Acc_PrePostTransDetail) pptd INNER JOIN (SELECT LocationCode,TransDate,ProjectCode,TransNo,SerialNo,SUM(Amount) Amounts "
                                        +" FROM Acc_PrePostTransDetailByDimension  GROUP BY LocationCode,TransDate,ProjectCode,TransNo,SerialNo "
                                        +" ) ppdd ON pptd.LocationCode = ppdd.LocationCode AND pptd.TransDate = ppdd.TransDate AND pptd.ProjectCode = ppdd.ProjectCode "
                                        + " AND pptd.TransNo = ppdd.TransNo AND pptd.SerialNo = ppdd.SerialNo WHERE pptd.Amount <> ppdd.Amounts and pptd.LocationCode= '" + objAccTransNocount.LocationCode
                                        + "' and pptd.TransDate = '" + objAccTransNocount.TransDate + "' and pptd.TransNo ='" + objAccTransNocount.TransNo + "'";

                        var v = AccountContext.Database.SqlQuery<CheckDimensionData>(ss);


                        if (v.Count() <= 0)
                        {
                            Ts.Complete();
                            
                        }
                        // Ts.Complete();
                        else if (v.Count() > 0)
                        {
                             Ts.Dispose();
                             objPrePostTransMaster = null;
                        }
                        //end for check Query for valid data in  dimension table

                       // Ts.Complete();
                        return objPrePostTransMaster;
                    }
                    catch (Exception ex)
                    {
                        Ts.Dispose();
                        throw;
                    }
                }
            }
        }










        public Acc_PrePostTransMaster Create(Acc_TransNoCount objAccTransNocount,Acc_PrePostTransMaster objPrePostTransMaster, List<Acc_PrePostTransDetail> lstPrePostTransDetail, List<Acc_PrePostTransDetailByDimension> lstPrePostTransDetailByDimension)
        {
            using (AccountContext)
            {
                using (var Ts = new TransactionScope())
                {
                    try
                    {
                        //string dbSequenceNumber = ReadPrePostTransMasterTransNoMax(objPrePostTransMaster.LocationCode, objPrePostTransMaster.TransNo.Substring(0, 6));
                        //string sequenceNumber = string.Empty;

                        //if (!string.IsNullOrEmpty(dbSequenceNumber))
                        //{
                        //    if (!Helper.ChallanSequenceNumberDuplicationCheck(objPrePostTransMaster.TransNo, dbSequenceNumber))
                        //    {
                        //        sequenceNumber = Helper.AccountSequenceNumberGeneration(dbSequenceNumber);
                        //        objPrePostTransMaster.TransNo = sequenceNumber;

                        //        for (int i = 0; i < lstPrePostTransDetail.Count; i++)
                        //        {
                        //            lstPrePostTransDetail[i].TransNo = sequenceNumber;
                        //        }

                        //        objAuditAdjustmentRelatedAccountingTransaction.TransNo = sequenceNumber;
                        //    }
                        //}



                        AccountContext.Acc_TransNoCount.Add(objAccTransNocount);

                        AccountContext.Acc_PrePostTransMaster.Add(objPrePostTransMaster);

                        foreach (Acc_PrePostTransDetail trd in lstPrePostTransDetail)
                        {
                            AccountContext.Acc_PrePostTransDetail.Add(trd);
                        }

                        if (lstPrePostTransDetailByDimension.Count > 0)
                        {
                            foreach (Acc_PrePostTransDetailByDimension trdd in lstPrePostTransDetailByDimension)
                            {
                                AccountContext.Acc_PrePostTransDetailByDimension.Add(trdd);
                            }
                        }

                        AccountContext.SaveChanges();

                        Ts.Complete();

                        return objPrePostTransMaster;
                    }
                    catch (Exception ex)
                    {
                        Ts.Dispose();
                        throw;
                    }
                }
            }
        }

        public Acc_PrePostTransMaster Create(Acc_PrePostTransMaster objPrePostTransMaster, List<EmployeeWiseSalaryPayment> lstEmployeeWiseSalary)
        {
            //using (AccountContext)
            //{

            string message="";

                using (var Ts = new TransactionScope())
                {
                    try
                    {



                        foreach (EmployeeWiseSalaryPayment trdd in lstEmployeeWiseSalary)
                        {
                            Hrm_SalaryPayRegisterByMonthNEmployee objHrm_SalaryPayRegisterByMonthNEmployee = new Hrm_SalaryPayRegisterByMonthNEmployee();

                            //  objHrm_SalaryPayRegisterByMonthNEmployee.EmployeeID = trdd.EmployeeID;
                            //   objHrm_SalaryPayRegisterByMonthNEmployee.YearMonth = trdd.YearMonth;



                            string YearMonth = Helper.YearMonth(trdd.YearMonth);

                            Hrm_SalaryPayRegisterByMonthNEmployee dbRecord = AccountContext.Hrm_SalaryPayRegisterByMonthNEmployee.Where(s => s.EmployeeID == trdd.EmployeeID && s.YearMonth == YearMonth && s.PaidInYearMonth==null).FirstOrDefault();

                            message = trdd.EmployeeID.ToString();


                            dbRecord.PaidSalaryOtherThanTADAAmount = trdd.SalaryOtherThanTADAAmount;
                            dbRecord.PaidTADAAmount = trdd.TADAAmount;
                            dbRecord.PaidBonusAmount = trdd.BonusAmount;
                            dbRecord.PaidTotalSalaryAmount = trdd.TotalSalaryAmount;
                            dbRecord.PaidInYearMonth = Helper.YearMonth(trdd.YearMonth);
                            dbRecord.PaidDate = trdd.TransactionDate;
                            dbRecord.RefAELocationCode = trdd.LocationCode;
                            dbRecord.RefAETransDate = trdd.TransactionDate;
                            dbRecord.RefAEProjectCode = Convert.ToString(trdd.ProjectCode);
                            dbRecord.RefAETransNo = trdd.TransactionNo;

                        }



                        AccountContext.SaveChanges();

                        Ts.Complete();

                        return objPrePostTransMaster;
                    }
                    catch (Exception ex)
                    {
                        Ts.Dispose();

                        message = "Employee Id:"+message+" is Already Paid";

                        Exception e2 = (Exception)Activator.CreateInstance(ex.GetType(), message, ex);
                        throw e2;

                      
                        //throw;
                    }
                }
            //}
        }


        public Acc_MonthlyCashReceiptPaymentTransaction Create(Acc_MonthlyCashReceiptPaymentTransaction objAccMonthlyCashReceiptPaymentTransaction)
        {
            using (AccountContext)
            {
                using (var Ts = new TransactionScope())
                {
                    try
                    {


                        AccountContext.Acc_MonthlyCashReceiptPaymentTransaction.Add(objAccMonthlyCashReceiptPaymentTransaction);

                        AccountContext.SaveChanges();

                        Ts.Complete();

                        return objAccMonthlyCashReceiptPaymentTransaction;
                    }
                    catch (Exception ex)
                    {
                        Ts.Dispose();
                        throw;
                    }
                }
            }
        }

        #endregion
    }
}
