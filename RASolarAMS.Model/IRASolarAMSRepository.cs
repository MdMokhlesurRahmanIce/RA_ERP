using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RASolarERP.DomainModel.AMSModel;
using System.Collections;

namespace RASolarAMS.Model
{
    public interface IRASolarAMSRepository
    {

        IQueryable<Acc_ChartOfAccounts> ReadChartOfAccount();
        byte ChartOfAccountLevelMax();

        Common_ProjectInfo ReadProject(string projectCode);
        List<ProjectInfo> ReadProject();

        List<TrialBalanceReport> ReadTrialBalance(string reportType, string locationCode, string startDate, string endDate, string projectCode);
        List<AccountBalanceSheet> ReadAccountBalanceSheet(string onDate);

        List<AccIncomeStatementForMonthlyMIS> ReadAccIncomeStatementForMonthlyMIS(string fromDate, string toDate);

        List<AccountingDataEntryStatus> ReadAccountingDataEntryStatus(string reportType, string locationCode, string yearMonth);

        List<AccShowAccountWiseBreakdown> ReadAccShowAccountWiseBreakdown(string locationCode, string yearMonth, string transType);

        MonthlyAccountingSummaryStatement ReadMonthlyAccountingSummaryStatement(string locationCode, string fromDate, string toDate);

        List<GLAccountLedger> ReadGLAccountLedger(string locationCode, string fromDate, string toDate, string accountNo, string projectCode);

        List<ChartOfAccounts> ReadChartOfAccounts();
        List<ChartOfAccounts> ReadChartOfAccounts(string locationCode, string projectCode, string isValidforLocation, string accountParCapSub);
        List<ChartOfAccounts> ReadCashBankAccount(string locationCode, string projectCode, string isValidforLocation, string accountParCapSub);
        List<ChartOfAccounts> ReadOtherAccount(string locationCode, string projectCode, string isValidforLocation, string accountParCapSub);

        List<ChartOfAccounts> ReadChartOfAccountsForGLLedger(string locationCode, string projectCode, string isValidforLocation, string accountParCapSub);

        List<AccGetMonthlyExpensesInDetail> ReadAccGetMonthlyExpensesInDetail(string locationCode, string projectCode);

        List<AccTrialBalanceInDetailByAccount> ReadTrialBalanceInDetailByAccount(DateTime fromDate, DateTime toDate, string projectCode, string accountNo);

        List<VoucherTransaction> GetFinalTransactionDetailListByTransactionNo(string transactionNo, string locationCode);

        List<AccountIncomeStatement> ReadIncomeStatement(byte chartNo, string reportId, byte reportSetNo, DateTime fromDate, DateTime toDate, string projectCode);

        ArrayList GetBankAccountForSalaryBoucher(string locationCode);
        ArrayList GetBankAccountForSalaryBoucher();

        ArrayList GetBankAccountInformation(byte specialAccountType, string locationCode, string projectCode, string isValidforLocation);

        ArrayList ReadVoucherTransNoMax(string locationCode, string yearMonthDate);
        List<ChartOfAccounts> ReadCashBankAccountWithDimension(string locationCode, string projectCode, string isValidforLocation, string accountParCapSub);


        decimal GetProjectWiseAccountBalance(string locationCode, string projectCode, string accountNumber, bool isIncludePrePostVoucher);

        Acc_PrePostTransMaster Create(Acc_TransNoCount objAccTransNocount, Acc_PrePostTransMaster objPrePostTransMaster, List<Acc_PrePostTransDetail> lstPrePostTransDetail, Aud_AuditAdjustmentRelatedAccountingTransaction objAuditAdjustmentRelatedAccountingTransaction);

        Acc_MonthlyCashReceiptPaymentTransaction Create(Acc_MonthlyCashReceiptPaymentTransaction objAccMonthlyCashReceiptPaymentTransaction);

        List<BankInformation> ReadBankInformation(string locationCode);
        List<BankInformation> ReadBankInformation(string locationCode, string bankId, string bankBranchID);
        List<BankBranchInfo> ReadBankBranchInformation(string locationCode, string bankId);
        List<BankAccountType> ReadBankAccountType();
        List<EmployeeWiseSalaryPayment> ReadEmployeeWiseSalaryPaymentPosting(string queryOption, string locationCode);
        List<ChartOfAccounts> GetLocationWiseChartOfAccount(string locationCode);

        List<SubLedgerHeadDetails> GetSubLedgerHeadDetails(string dimensionCode, string locationCode);
        Acc_PrePostTransMaster Create(Acc_TransNoCount objAccTransNocount,Acc_PrePostTransMaster objPrePostTransMaster, List<Acc_PrePostTransDetail> lstPrePostTransDetail, List<Acc_PrePostTransDetailByDimension> lstPrePostTransDetailByDimension);
        
        Acc_PrePostTransMaster Create(Acc_PrePostTransMaster objPrePostTransMaster, List<EmployeeWiseSalaryPayment> lstEmployeeWiseSalary);

        Acc_PrePostTransMaster Create(Acc_TransNoCount objAccTransNocount, List<EmployeeWiseSalaryPayment> lstEmployeeWiseSalary, List<Acc_PrePostTransDetail> lstPrePostTransDetail, List<Acc_PrePostTransDetailByDimension> lstPrePostTransDetailByDimension, Acc_PrePostTransMaster objPrePostTransMaster, string supportMethod);
     
        bool IsTheDimensionMandatoryExistOrNot(string accountNumber);
        Acc_PrePostTransMaster Create(Acc_TransNoCount objAccTransNocount, Acc_PrePostTransMaster objPrePostTransMaster, List<Acc_PrePostTransDetail> lstPrePostTransDetail, Aud_AuditAdjustmentRelatedAccountingTransaction objAuditAdjustmentRelatedAccountingTransaction, List<Acc_PrePostTransDetailByDimension> lstPrePostTransDetailByDimension);

    }
}
