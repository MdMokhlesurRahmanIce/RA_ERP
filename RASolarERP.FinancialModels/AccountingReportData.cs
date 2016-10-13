using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using RASolarERP.Model;
using RASolarAMS.Model;
using RASolarHelper;
using System.Collections;
using RASolarERP.DomainModel.AMSModel;

namespace RASolarERP.Web.Areas.Financial.Models
{
    public class AccountingReportData : BaseData
    {
        public List<AccountingDataEntryStatus> ReadAccountingDataEntryStatus(string reportType, string locationCode, string yearMonth)
        {
            return AMSService.ReadAccountingDataEntryStatus(reportType, locationCode, yearMonth);
        }

        public List<TrialBalanceReport> ReadTrialBalance(string reportType, string locationCode, string startDate, string endDate, string projectCode)
        {
            return AMSService.ReadTrialBalance(reportType, locationCode, startDate, endDate, projectCode);
        }

        public RASolarAMS.Model.Common_ProjectInfo ReadProject(string projectCode)
        {
            return AMSService.ReadProject(projectCode);
        }

        public List<ProjectInfo> ReadProject()
        {
            return AMSService.ReadProject();
        }

        public ArrayList ChartOfAccountLevel()
        {
            return AMSService.ChartOfAccountLevel();
        }

        public List<AccountBalanceSheet> ReadAccountBalanceSheet(string onDate)
        {
            return AMSService.ReadAccountBalanceSheet(onDate);
        }

        public List<AccIncomeStatementForMonthlyMIS> ReadAccIncomeStatementForMonthlyMIS(string fromDate, string toDate)
        {
            return AMSService.ReadAccIncomeStatementForMonthlyMIS(fromDate, toDate);
        }

        //public tbl_UnitWiseEntryStatus UpdateAccountEntryStatus(tbl_UnitWiseEntryStatus objUnitWiseEntryStatus)
        //{
        //    return erpService.Update(objUnitWiseEntryStatus);
        //}

        public List<AccShowAccountWiseBreakdown> ReadAccShowAccountWiseBreakdown(string locationCode, string yearMonth, string transType)
        {
            return AMSService.ReadAccShowAccountWiseBreakdown(locationCode, yearMonth, transType);
        }

        public MonthlyAccountingSummaryStatement ReadMonthlyAccountingSummaryStatement(string locationCode, string fromDate, string toDate)
        {
            return AMSService.ReadMonthlyAccountingSummaryStatement(locationCode, fromDate, toDate);
        }

        public Acc_MonthlyCashReceiptPaymentTransaction Create(Acc_MonthlyCashReceiptPaymentTransaction objMonthlyCashReceiptPaymentTransaction)
        {
            return AMSService.Create(objMonthlyCashReceiptPaymentTransaction);
        }

        public List<GLAccountLedger> ReadGLAccountLedger(string locationCode, string fromDate, string toDate, string accountNo, string projectCode)
        {
            return AMSService.ReadGLAccountLedger(locationCode, fromDate, toDate, accountNo, projectCode);
        }
        public List<ChartOfAccounts> ReadChartOfAccounts()
        {
            return AMSService.ReadChartOfAccounts();
        }
        public List<AccGetMonthlyExpensesInDetail> ReadAccGetMonthlyExpensesInDetail(string locationCode, string projectCode)
        {
            return AMSService.ReadAccGetMonthlyExpensesInDetail(locationCode, projectCode);
        }

        public List<AccTrialBalanceInDetailByAccount> ReadTrialBalanceInDetailByAccount(DateTime fromDate, DateTime toDate, string projectCode, string accountNo)
        {
            return AMSService.ReadTrialBalanceInDetailByAccount(fromDate, toDate, projectCode, accountNo);
        }

        public List<VoucherTransaction> GetFinalTransactionDetailListByTransactionNo(string transactionNo, string locationCode)
        {
            return AMSService.GetFinalTransactionDetailListByTransactionNo(transactionNo, locationCode);
        }

        public List<AccountIncomeStatement> ReadIncomeStatement(byte chartNo, string reportId, byte reportSetNo, DateTime fromDate, DateTime toDate, string projectCode)
        {
            return AMSService.ReadIncomeStatement(chartNo, reportId, reportSetNo, fromDate, toDate, projectCode);
        }

        public List<ChartOfAccounts> ReadChartOfAccountsForGLLedger(string locationCode, string projectCode, string isValidforLocation, string accountParCapSub)
        {
            return AMSService.ReadChartOfAccountsForGLLedger(locationCode, projectCode, isValidforLocation, accountParCapSub);
        }

        public List<EmployeeWiseSalaryPayment> ReadEmployeeWiseSalaryPaymentPosting(string queryOption, string locationCode)
        {

            List<EmployeeWiseSalaryPayment> lstSalaryPayment = new List<EmployeeWiseSalaryPayment>();
            lstSalaryPayment = AMSService.ReadEmployeeWiseSalaryPaymentPosting(queryOption, locationCode);

            foreach (EmployeeWiseSalaryPayment sp in lstSalaryPayment)
            {
                sp.YearMonth = Helper.YearMonthNumberToYearMonthText(sp.YearMonth);
            }

            return lstSalaryPayment;
        }

        public List<ChartOfAccounts> GetLocationWiseChartOfAccount(string locationCode)
        {
            return AMSService.GetLocationWiseChartOfAccount(locationCode);
        }

    }
}