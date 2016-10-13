using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using RASolarERP.DomainModel.AMSModel;

namespace RASolarAMS.Model
{
    public class RASolarAMSService : IRASolarAMSService
    {
        #region Properties And Constructor

        private IRASolarAMSRepository raSolarAMSRepository;

        public RASolarAMSService()
        {
            this.raSolarAMSRepository = new RASolarAMSRepository(new RASolarAMSEntities());
        }

        public RASolarAMSService(IRASolarAMSRepository raSolarRepository)
        {
            this.raSolarAMSRepository = raSolarRepository;
        }

        public ArrayList GetBankAccountForSalaryBoucher(string locationCode)
        {
            return raSolarAMSRepository.GetBankAccountForSalaryBoucher(locationCode);
        }

        public ArrayList GetBankAccountForSalaryBoucher()
        {
            return raSolarAMSRepository.GetBankAccountForSalaryBoucher();
        }

        #endregion

        #region Read Method

        public List<Acc_ChartOfAccounts> ReadChartOfAccount()
        {
            return raSolarAMSRepository.ReadChartOfAccount().ToList();
        }

        public ArrayList ChartOfAccountLevel()
        {
            ArrayList lstChartOfAccountLevel = new ArrayList();

            int maxChartLevel = Convert.ToInt32(raSolarAMSRepository.ChartOfAccountLevelMax());

            for (int i = 0; i <= maxChartLevel; i++)
            {
                lstChartOfAccountLevel.Add(new { Value = i, Display = i });
            }

            return lstChartOfAccountLevel;
        }

        public Common_ProjectInfo ReadProject(string projectCode)
        {
            return raSolarAMSRepository.ReadProject(projectCode);
        }

        public List<ProjectInfo> ReadProject()
        {
            return raSolarAMSRepository.ReadProject();
        }

        public List<TrialBalanceReport> ReadTrialBalance(string reportType, string locationCode, string startDate, string endDate, string projectCode)
        {
            return raSolarAMSRepository.ReadTrialBalance(reportType, locationCode, startDate, endDate, projectCode);
        }

        public List<AccountBalanceSheet> ReadAccountBalanceSheet(string onDate)
        {
            return raSolarAMSRepository.ReadAccountBalanceSheet(onDate);
        }

        public List<AccIncomeStatementForMonthlyMIS> ReadAccIncomeStatementForMonthlyMIS(string fromDate, string toDate)
        {
            return raSolarAMSRepository.ReadAccIncomeStatementForMonthlyMIS(fromDate, toDate);
        }

        public List<AccountingDataEntryStatus> ReadAccountingDataEntryStatus(string reportType, string locationCode, string yearMonth)
        {
            return raSolarAMSRepository.ReadAccountingDataEntryStatus(reportType, locationCode, yearMonth);
        }

        public List<AccShowAccountWiseBreakdown> ReadAccShowAccountWiseBreakdown(string locationCode, string yearMonth, string transType)
        {
            return raSolarAMSRepository.ReadAccShowAccountWiseBreakdown(locationCode, yearMonth, transType);
        }

        public MonthlyAccountingSummaryStatement ReadMonthlyAccountingSummaryStatement(string locationCode, string fromDate, string toDate)
        {
            return raSolarAMSRepository.ReadMonthlyAccountingSummaryStatement(locationCode, fromDate, toDate);
        }

        public List<GLAccountLedger> ReadGLAccountLedger(string locationCode, string fromDate, string toDate, string accountNo, string projectCode)
        {
            return raSolarAMSRepository.ReadGLAccountLedger(locationCode, fromDate, toDate, accountNo, projectCode);
        }

        public List<ChartOfAccounts> ReadChartOfAccounts()
        {
            return raSolarAMSRepository.ReadChartOfAccounts();
        }

        public List<ChartOfAccounts> ReadChartOfAccounts(string locationCode, string projectCode, string isValidforLocation, string accountParCapSub)
        {
            return raSolarAMSRepository.ReadChartOfAccounts(locationCode, projectCode, isValidforLocation, accountParCapSub);
        }

        public List<ChartOfAccounts> ReadCashBankAccount(string locationCode, string projectCode, string isValidforLocation, string accountParCapSub)
        {
            return raSolarAMSRepository.ReadCashBankAccount(locationCode, projectCode, isValidforLocation, accountParCapSub);
        }

        public List<ChartOfAccounts> ReadOtherAccount(string locationCode, string projectCode, string isValidforLocation, string accountParCapSub)
        {
            return raSolarAMSRepository.ReadOtherAccount(locationCode, projectCode, isValidforLocation, accountParCapSub);
        }

        public List<ChartOfAccounts> ReadCashBankAccountWithDimension(string locationCode, string projectCode, string isValidforLocation, string accountParCapSub)
        {
            return raSolarAMSRepository.ReadCashBankAccountWithDimension(locationCode, projectCode, isValidforLocation, accountParCapSub);
        }

        public List<ChartOfAccounts> ReadChartOfAccountsForGLLedger(string locationCode, string projectCode, string isValidforLocation, string accountParCapSub)
        {
            return raSolarAMSRepository.ReadChartOfAccountsForGLLedger(locationCode, projectCode, isValidforLocation, accountParCapSub);
        }

        public List<AccGetMonthlyExpensesInDetail> ReadAccGetMonthlyExpensesInDetail(string locationCode, string projectCode)
        {
            return raSolarAMSRepository.ReadAccGetMonthlyExpensesInDetail(locationCode, projectCode);
        }

        public List<AccTrialBalanceInDetailByAccount> ReadTrialBalanceInDetailByAccount(DateTime fromDate, DateTime toDate, string projectCode, string accountNo)
        {
            return raSolarAMSRepository.ReadTrialBalanceInDetailByAccount(fromDate, toDate, projectCode, accountNo);
        }

        public List<VoucherTransaction> GetFinalTransactionDetailListByTransactionNo(string transactionNo, string locationCode)
        {
            return raSolarAMSRepository.GetFinalTransactionDetailListByTransactionNo(transactionNo, locationCode);
        }

        public List<AccountIncomeStatement> ReadIncomeStatement(byte chartNo, string reportId, byte reportSetNo, DateTime fromDate, DateTime toDate, string projectCode)
        {
            return raSolarAMSRepository.ReadIncomeStatement(chartNo, reportId, reportSetNo, fromDate, toDate, projectCode);
        }

        public ArrayList GetBankAccountInformation(byte specialAccountType, string locationCode, string projectCode, string isValidforLocation)
        {
            return raSolarAMSRepository.GetBankAccountInformation(specialAccountType, locationCode, projectCode, isValidforLocation);
        }

        public ArrayList ReadVoucherTransNoMax(string locationCode, string yearMonthDate)
        {
            return raSolarAMSRepository.ReadVoucherTransNoMax(locationCode, yearMonthDate);
        }

        public decimal GetProjectWiseAccountBalance(string locationCode, string projectCode, string accountNumber, bool isIncludePrePostVoucher)
        {
            return raSolarAMSRepository.GetProjectWiseAccountBalance(locationCode, projectCode, accountNumber, isIncludePrePostVoucher);
        }

        public List<BankInformation> ReadBankInformation(string locationCode)
        {
            return raSolarAMSRepository.ReadBankInformation(locationCode);
        }
        public List<BankInformation> ReadBankInformation(string locationCode, string bankId, string bankBranchID)
        {
            return raSolarAMSRepository.ReadBankInformation(locationCode, bankId, bankBranchID);
        }
        public List<BankBranchInfo> ReadBankBranchInformation(string locationCode, string bankId)
        {
            return raSolarAMSRepository.ReadBankBranchInformation(locationCode, bankId);
        }

        public List<BankAccountType> ReadBankAccountType()
        {
            return raSolarAMSRepository.ReadBankAccountType();
        }

        public List<EmployeeWiseSalaryPayment> ReadEmployeeWiseSalaryPaymentPosting(string queryOption, string locationCode)
        {
            return raSolarAMSRepository.ReadEmployeeWiseSalaryPaymentPosting(queryOption, locationCode);
        }

        public List<ChartOfAccounts> GetLocationWiseChartOfAccount(string locationCode)
        {
            return raSolarAMSRepository.GetLocationWiseChartOfAccount(locationCode);
        }

        public List<SubLedgerHeadDetails> GetSubLedgerHeadDetails(string dimensionCode, string locationCode)
        {
            return raSolarAMSRepository.GetSubLedgerHeadDetails(dimensionCode, locationCode);
        }

        public bool IsTheDimensionMandatoryExistOrNot(string accountNumber)
        {
            return raSolarAMSRepository.IsTheDimensionMandatoryExistOrNot(accountNumber);
        }

        #endregion

        #region Create Methods

        public Acc_PrePostTransMaster Create(Acc_TransNoCount objAccTransNocount, Acc_PrePostTransMaster objPrePostTransMaster, List<Acc_PrePostTransDetail> lstPrePostTransDetail, Aud_AuditAdjustmentRelatedAccountingTransaction objAuditAdjustmentRelatedAccountingTransaction)
        {
            return raSolarAMSRepository.Create(objAccTransNocount, objPrePostTransMaster, lstPrePostTransDetail, objAuditAdjustmentRelatedAccountingTransaction);
        }
        
        public Acc_PrePostTransMaster Create(Acc_TransNoCount objAccTransNocount,Acc_PrePostTransMaster objPrePostTransMaster, List<Acc_PrePostTransDetail> lstPrePostTransDetail, List<Acc_PrePostTransDetailByDimension> lstPrePostTransDetailByDimension)
        {
            return raSolarAMSRepository.Create(objAccTransNocount,objPrePostTransMaster, lstPrePostTransDetail, lstPrePostTransDetailByDimension);
        }

        public Acc_PrePostTransMaster Create(Acc_TransNoCount objAccTransNocount, List<EmployeeWiseSalaryPayment> lstEmployeeWiseSalary, List<Acc_PrePostTransDetail> lstPrePostTransDetail, List<Acc_PrePostTransDetailByDimension> lstPrePostTransDetailByDimension, Acc_PrePostTransMaster objPrePostTransMaster, string supportMethod)
        {
            return raSolarAMSRepository.Create(objAccTransNocount, lstEmployeeWiseSalary, lstPrePostTransDetail, lstPrePostTransDetailByDimension, objPrePostTransMaster, supportMethod); 
         
        } 

        public Acc_PrePostTransMaster Create(Acc_PrePostTransMaster objPrePostTransMaster, List<EmployeeWiseSalaryPayment> lstEmployeeWiseSalary)
        {
            return raSolarAMSRepository.Create(objPrePostTransMaster, lstEmployeeWiseSalary);
        }
        
        public Acc_PrePostTransMaster Create(Acc_TransNoCount objAccTransNocount, Acc_PrePostTransMaster objPrePostTransMaster, List<Acc_PrePostTransDetail> lstPrePostTransDetail, Aud_AuditAdjustmentRelatedAccountingTransaction objAuditAdjustmentRelatedAccountingTransaction, List<Acc_PrePostTransDetailByDimension> lstPrePostTransDetailByDimension)
        {
            return raSolarAMSRepository.Create(objAccTransNocount, objPrePostTransMaster, lstPrePostTransDetail, objAuditAdjustmentRelatedAccountingTransaction, lstPrePostTransDetailByDimension);
        }


        public Acc_MonthlyCashReceiptPaymentTransaction Create(Acc_MonthlyCashReceiptPaymentTransaction objAccMonthlyCashReceiptPaymentTransaction)
        {
            return raSolarAMSRepository.Create(objAccMonthlyCashReceiptPaymentTransaction);
        }
        
        #endregion
    }
}

