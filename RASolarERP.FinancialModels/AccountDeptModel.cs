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
    public class AccountDeptModel : BaseData
    {
        public ArrayList GetBankAccountInformation(byte specialAccountType, string locationCode, string projectCode, string isValidforLocation)
        {
            return AMSService.GetBankAccountInformation(specialAccountType, locationCode, projectCode, isValidforLocation);
        }

        public List<ChartOfAccounts> ReadChartOfAccounts(string locationCode, string projectCode, string isValidforLocation, string accountParCapSub)
        {
            return AMSService.ReadChartOfAccounts(locationCode, projectCode, isValidforLocation, accountParCapSub);
        }

        public List<ChartOfAccounts> ReadCashBankAccount(string locationCode, string projectCode, string isValidforLocation, string accountParCapSub)
        {
            return AMSService.ReadCashBankAccount(locationCode, projectCode, isValidforLocation, accountParCapSub);
        }

        public List<ChartOfAccounts> ReadCashBankAccountWithDimension(string locationCode, string projectCode, string isValidforLocation, string accountParCapSub)
        {
            return AMSService.ReadCashBankAccountWithDimension(locationCode, projectCode, isValidforLocation, accountParCapSub);
        }

        public List<ChartOfAccounts> ReadOtherAccount(string locationCode, string projectCode, string isValidforLocation, string accountParCapSub)
        {
            return AMSService.ReadOtherAccount(locationCode, projectCode, isValidforLocation, accountParCapSub);
        }

        public ArrayList ReadVoucherTransNoMax(string locationCode, string yearMonthDate)
        {
            return AMSService.ReadVoucherTransNoMax(locationCode, yearMonthDate);
        }  

        public Acc_PrePostTransMaster CreateVoucher(Acc_TransNoCount objAccTransNocount,Acc_PrePostTransMaster objPrePostTransMaster, List<Acc_PrePostTransDetail> lstPrePostTransDetail, Aud_AuditAdjustmentRelatedAccountingTransaction objAuditAdjustmentRelatedAccountingTransaction)
        {
            return AMSService.Create(objAccTransNocount,objPrePostTransMaster, lstPrePostTransDetail, objAuditAdjustmentRelatedAccountingTransaction);
        }

        public Acc_PrePostTransMaster CreateVoucher(Acc_TransNoCount objAccTransNocount, Acc_PrePostTransMaster objPrePostTransMaster, List<Acc_PrePostTransDetail> lstPrePostTransDetail, List<Acc_PrePostTransDetailByDimension> lstPrePostTransDetailByDimension)
        {
            return AMSService.Create(objAccTransNocount,objPrePostTransMaster, lstPrePostTransDetail, lstPrePostTransDetailByDimension);
        }

        public Acc_PrePostTransMaster CreateVoucherForSaveEmployeeWiseSalaryPayment(Acc_TransNoCount objAccTransNocount, List<EmployeeWiseSalaryPayment> lstEmployeeWiseSalary, List<Acc_PrePostTransDetail> lstPrePostTransDetail, List<Acc_PrePostTransDetailByDimension> lstPrePostTransDetailByDimension, Acc_PrePostTransMaster objPrePostTransMaster, string supportMethod)
        {
            return AMSService.Create(objAccTransNocount, lstEmployeeWiseSalary, lstPrePostTransDetail, lstPrePostTransDetailByDimension, objPrePostTransMaster, supportMethod);
        }

        public Acc_PrePostTransMaster CreateVoucher(Acc_PrePostTransMaster objPrePostTransMaster, List<EmployeeWiseSalaryPayment> lstEmployeeWiseSalary)
        {
            return AMSService.Create(objPrePostTransMaster, lstEmployeeWiseSalary);
        }

        public bool IsTheDimensionMandatoryExistOrNot(string accountNumber)
        {
            return AMSService.IsTheDimensionMandatoryExistOrNot(accountNumber);
        }

        public Acc_PrePostTransMaster CreateVoucher(Acc_TransNoCount objAccTransNocount, Acc_PrePostTransMaster objPrePostTransMaster, List<Acc_PrePostTransDetail> lstPrePostTransDetail, Aud_AuditAdjustmentRelatedAccountingTransaction objAuditAdjustmentRelatedAccountingTransaction, List<Acc_PrePostTransDetailByDimension> lstPrePostTransDetailByDimension)
        {
            return AMSService.Create(objAccTransNocount, objPrePostTransMaster, lstPrePostTransDetail, objAuditAdjustmentRelatedAccountingTransaction, lstPrePostTransDetailByDimension);
        }

        public List<SubLedgerHeadDetails> GetSubLedgerHeadDetails(string dimensionCode, string locationCode)
        {
            return AMSService.GetSubLedgerHeadDetails(dimensionCode, locationCode);
        }

      
               
    }
}