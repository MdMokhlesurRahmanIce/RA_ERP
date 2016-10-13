using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using RASolarHelper;
using RASolarERP.Model;
using RASolarAMS.Model;
using RASolarERP.Web.Models;
using RASolarERP.DomainModel.AMSModel;
using RASolarERP.Web.Areas.Financial.Models;

namespace RASolarERP.Web.Areas.Financial.Models
{
    public class VoucherProcess : BaseData
    {
        //private HRMSData hrmDal = new HRMSData();
        private AccountDeptModel accountDeptDal = new AccountDeptModel();

        public Acc_TransNoCount TransNoCount(VoucherTransaction objCashBankVoucher, string locationCode)
        {

            Acc_TransNoCount objtransNoCount = new Acc_TransNoCount();

            objtransNoCount.LocationCode = locationCode;
            objtransNoCount.TransDate = DateTime.Now; //Convert.ToDateTime(objCashBankVoucher.TransactionDate);
            //objtransNoCount.ProjectCode = objCashBankVoucher.ProjectCode;
            objtransNoCount.TransNo = objCashBankVoucher.TransactionNo;
            //objtransNoCount.Add(objtransNoCount);
            return objtransNoCount;
        }

        public Acc_PrePostTransMaster CashBankVoucherProcess(VoucherTransaction objCashBankVoucher, string voucherType, LoginHelper objLoginHelper, string entrySource)
        {
            Acc_PrePostTransMaster objPrePostTransMaster = new Acc_PrePostTransMaster();

            objPrePostTransMaster.LocationCode = objLoginHelper.LocationCode;
            objPrePostTransMaster.TransDate = objCashBankVoucher.TransactionDate;
            objPrePostTransMaster.ProjectCode = objCashBankVoucher.ProjectCode;
            objPrePostTransMaster.TransNo = objCashBankVoucher.TransactionNo;
            objPrePostTransMaster.GenParticulars = objCashBankVoucher.GeneralParticulars;
            objPrePostTransMaster.CBVoucher = Helper.VoucherSelection(voucherType);
            objPrePostTransMaster.CBVType = Helper.VoucherTypeSelection(voucherType);
            objPrePostTransMaster.IsAutoEntry = true;
            objPrePostTransMaster.EntrySource = entrySource;
            objPrePostTransMaster.SenderOrReceiverFlag = null;
            objPrePostTransMaster.SenderOrReceiverLocationCode = null;
            objPrePostTransMaster.UserID = objLoginHelper.LogInID;
            objPrePostTransMaster.EntryTime = DateTime.Now;

            return objPrePostTransMaster;

        }

        public List<Acc_PrePostTransDetail> OtherBankVoucherProcess(List<VoucherTransaction> lstOtherVoucher, string voucherType, LoginHelper objLoginHelper, string EntrySource, string transactionNo)
        {
            List<Acc_PrePostTransDetail> lstPrePostTransDetail = new List<Acc_PrePostTransDetail>();
            Acc_PrePostTransDetail objPrePostTransDetail;
            int serialNo = 0;

            foreach (VoucherTransaction vtrans in lstOtherVoucher)
            {
                objPrePostTransDetail = new Acc_PrePostTransDetail();

                objPrePostTransDetail.LocationCode = objLoginHelper.LocationCode;
                objPrePostTransDetail.TransDate = vtrans.TransactionDate;
                objPrePostTransDetail.ProjectCode = vtrans.ProjectCode;
                objPrePostTransDetail.TransNo = transactionNo;
                objPrePostTransDetail.SerialNo = ++serialNo;
                objPrePostTransDetail.AccountNo = vtrans.AccountNo;
                objPrePostTransDetail.Particulars = vtrans.Particulars;
                objPrePostTransDetail.RefEntrySource = EntrySource;

                objPrePostTransDetail.Amount = DebitOrCreditConversion(vtrans.TransactionType, vtrans.Amount);

                objPrePostTransDetail.TransProjectCode = null;
                objPrePostTransDetail.TransLocationCode = null;
                objPrePostTransDetail.RefEntrySource = null;

                lstPrePostTransDetail.Add(objPrePostTransDetail);
            }

            return lstPrePostTransDetail;
        }

        public Aud_AuditAdjustmentRelatedAccountingTransaction AuditAdjustmentVoucherOrAccount(VoucherTransactionAudit objVoucherTransactionAudit, LoginHelper objLoginHelper)
        {
            RASolarERPData erpDal = new RASolarERPData();

            Aud_AuditAdjustmentRelatedAccountingTransaction objAuditAdjustmentRelatedAccountingTransaction = new Aud_AuditAdjustmentRelatedAccountingTransaction();

            string auditSequenceNumber = string.Empty;
            auditSequenceNumber = erpDal.ReadAuditSeqNumberAfterCheckFinishedDate(objLoginHelper.LocationCode);

            objAuditAdjustmentRelatedAccountingTransaction.LocationCode = objLoginHelper.LocationCode;
            objAuditAdjustmentRelatedAccountingTransaction.TransDate = objVoucherTransactionAudit.TransDate;
            objAuditAdjustmentRelatedAccountingTransaction.ProjectCode = objVoucherTransactionAudit.ProjectCode;
            objAuditAdjustmentRelatedAccountingTransaction.TransNo = objVoucherTransactionAudit.TransNo;
            objAuditAdjustmentRelatedAccountingTransaction.AuditSeqNo = auditSequenceNumber;
            objAuditAdjustmentRelatedAccountingTransaction.ReasonCode = objVoucherTransactionAudit.ReasonCode;
            objAuditAdjustmentRelatedAccountingTransaction.RefVoucherDate = objVoucherTransactionAudit.RefVoucherDate;
            objAuditAdjustmentRelatedAccountingTransaction.ResponsibleEmployeeID = objVoucherTransactionAudit.ResponsibleEmployeeID;
            objAuditAdjustmentRelatedAccountingTransaction.Remarks = objVoucherTransactionAudit.Remarks;

            return objAuditAdjustmentRelatedAccountingTransaction;
        }

        public List<Acc_PrePostTransDetailByDimension> PrePostTransDetailByDimensionProcess(VoucherTransaction objCashBankVoucher, List<Acc_PrePostTransDetail> lstPrePostTransDetail, List<SubLedgerHeadDetails> lstSubLedgerHeadDetails, string locationCode)
        {
            List<Acc_PrePostTransDetailByDimension> lstPrePostTransDetailByDimensionProcess = new List<Acc_PrePostTransDetailByDimension>();
            Acc_PrePostTransDetailByDimension objPrePostTransDetailByDimension;
            int serialNo = 0;
            string ssp = "a";

            foreach (SubLedgerHeadDetails subLedger in lstSubLedgerHeadDetails)
            {
               if (ssp != subLedger.AccountNo)
                {
                    serialNo = (from ss in lstPrePostTransDetail where ss.AccountNo == subLedger.AccountNo select ss.SerialNo).FirstOrDefault();
                    ssp = (from ss in lstPrePostTransDetail where ss.AccountNo == subLedger.AccountNo select ss.AccountNo).FirstOrDefault();
                }

                bool isTheDimensionMandatory = accountDeptDal.IsTheDimensionMandatoryExistOrNot(subLedger.AccountNo);
                if (isTheDimensionMandatory == true)
                {

                    objPrePostTransDetailByDimension = new Acc_PrePostTransDetailByDimension();
                    objPrePostTransDetailByDimension.LocationCode = locationCode;
                    objPrePostTransDetailByDimension.TransDate = objCashBankVoucher.TransactionDate;
                    objPrePostTransDetailByDimension.ProjectCode = objCashBankVoucher.ProjectCode;
                    objPrePostTransDetailByDimension.TransNo = objCashBankVoucher.TransactionNo;
                    objPrePostTransDetailByDimension.SerialNo = serialNo;
                    objPrePostTransDetailByDimension.DimensionCode = subLedger.DimensionCode;
                    objPrePostTransDetailByDimension.DimensionValueID = subLedger.DimensionValueID;

                    objPrePostTransDetailByDimension.Amount = DebitOrCreditConversion(subLedger.TransactionType, Convert.ToDecimal(subLedger.DimensionAmount));

                    objPrePostTransDetailByDimension.RefAccountNo = subLedger.AccountNo;

                    lstPrePostTransDetailByDimensionProcess.Add(objPrePostTransDetailByDimension);
                }
            }

            return lstPrePostTransDetailByDimensionProcess;
        }

        public List<Acc_PrePostTransDetailByDimension> PrePostTransDetailByEmployeeWiseSalaryDimensionProcess(VoucherTransaction objCashBankVoucher, List<Acc_PrePostTransDetail> lstPrePostTransDetail, List<EmployeeWiseSalaryPayment> lstEmployeeWiseSalary, string locationCode)
        {
            List<Acc_PrePostTransDetailByDimension> lstPrePostTransDetailByDimensionProcess = new List<Acc_PrePostTransDetailByDimension>();
            Acc_PrePostTransDetailByDimension objPrePostDimensionForSalary;

            foreach (Acc_PrePostTransDetail vtrans in lstPrePostTransDetail)
            {
                if (vtrans.AccountNo == "204010150" || vtrans.AccountNo == "204010175" || vtrans.AccountNo == "204010155")
                {
                    foreach (EmployeeWiseSalaryPayment emp in lstEmployeeWiseSalary)
                    {
                        if (vtrans.Amount > 0)
                        {
                            if (emp.SalaryOtherThanTADAAmount != 0 || emp.TADAAmount != 0 || emp.BonusAmount != 0)
                            {
                                objPrePostDimensionForSalary = new Acc_PrePostTransDetailByDimension();

                                objPrePostDimensionForSalary.LocationCode = locationCode;
                                objPrePostDimensionForSalary.TransDate = Convert.ToDateTime(objCashBankVoucher.TransactionDate);
                                objPrePostDimensionForSalary.ProjectCode = objCashBankVoucher.ProjectCode;
                                objPrePostDimensionForSalary.TransNo = objCashBankVoucher.TransactionNo;
                                objPrePostDimensionForSalary.SerialNo = vtrans.SerialNo;
                                objPrePostDimensionForSalary.DimensionCode = "EMPLOYEEID";
                                objPrePostDimensionForSalary.DimensionValueID = emp.EmployeeID;

                                if (vtrans.AccountNo == "204010150")
                                    objPrePostDimensionForSalary.Amount = emp.SalaryOtherThanTADAAmount;
                                else if (vtrans.AccountNo == "204010175")
                                    objPrePostDimensionForSalary.Amount = emp.TADAAmount;
                                else if (vtrans.AccountNo == "204010155")
                                    objPrePostDimensionForSalary.Amount = emp.BonusAmount;

                                objPrePostDimensionForSalary.RefAccountNo = vtrans.AccountNo;
                                lstPrePostTransDetailByDimensionProcess.Add(objPrePostDimensionForSalary);
                            }
                        }
                    }
                }
            }

            return lstPrePostTransDetailByDimensionProcess;
        }

        public List<SubLedgerHeadDetails> PrepareEmployeeWiseSalaryToSubLedgerDimension(List<EmployeeWiseSalaryPayment> lstEmployeeWiseSalary, string locationCode, string transactionNo)
        {
            List<SubLedgerHeadDetails> lstSubledgerHeads = new List<SubLedgerHeadDetails>();
            SubLedgerHeadDetails objSubledgerHeads;

            foreach (EmployeeWiseSalaryPayment empSalary in lstEmployeeWiseSalary)
            {
                var lstSubledgerHeadsDimensionValueIDCheck = from D in lstSubledgerHeads
                                                             where D.DimensionValueID == empSalary.EmployeeID
                                                             select D;

                if (lstSubledgerHeadsDimensionValueIDCheck.Count() > 0)
                {
                    // Salary
                    if (empSalary.SalaryOtherThanTADAAmount != 0)
                    {
                        (from modify in lstSubledgerHeads
                         where modify.DimensionValueID == empSalary.EmployeeID && modify.AccountNo == "204010150"
                         select modify).ToList().ForEach(modify => modify.DimensionAmount += empSalary.SalaryOtherThanTADAAmount);
                    }

                    // TA/DA
                    if (empSalary.TADAAmount != 0)
                    {
                        (from modify in lstSubledgerHeads
                         where modify.DimensionValueID == empSalary.EmployeeID && modify.AccountNo == "204010175"
                         select modify).ToList().ForEach(modify => modify.DimensionAmount += empSalary.TADAAmount);
                    }


                    // Bonus
                    if (empSalary.BonusAmount != 0)
                    {

                        (from modify in lstSubledgerHeads
                         where modify.DimensionValueID == empSalary.EmployeeID && modify.AccountNo == "204010155"
                         select modify).ToList().ForEach(modify => modify.DimensionAmount += empSalary.BonusAmount);
                    }

                }
                else
                {

                    // Salary
                    if (empSalary.SalaryOtherThanTADAAmount != 0)
                    {
                        objSubledgerHeads = new SubLedgerHeadDetails();
                        objSubledgerHeads.DimensionValueID = empSalary.EmployeeID;
                        objSubledgerHeads.DimensionValueDesc = empSalary.EmployeeName;
                        objSubledgerHeads.DimensionAmount = empSalary.SalaryOtherThanTADAAmount;
                        objSubledgerHeads.AccountNo = "204010150";
                        objSubledgerHeads.DimensionCode = "EMPLOYEEID";
                        objSubledgerHeads.TransactionType = "DR";
                        lstSubledgerHeads.Add(objSubledgerHeads);
                    }

                    // TA/DA
                    if (empSalary.TADAAmount != 0)
                    {
                        objSubledgerHeads = new SubLedgerHeadDetails();
                        objSubledgerHeads.DimensionValueID = empSalary.EmployeeID;
                        objSubledgerHeads.DimensionValueDesc = empSalary.EmployeeName;
                        objSubledgerHeads.DimensionAmount = empSalary.TADAAmount;
                        objSubledgerHeads.AccountNo = "204010175";
                        objSubledgerHeads.DimensionCode = "EMPLOYEEID";
                        objSubledgerHeads.TransactionType = "DR";
                        lstSubledgerHeads.Add(objSubledgerHeads);
                    }

                    // Bonus
                    if (empSalary.BonusAmount != 0)
                    {
                        objSubledgerHeads = new SubLedgerHeadDetails();
                        objSubledgerHeads.DimensionValueID = empSalary.EmployeeID;
                        objSubledgerHeads.DimensionValueDesc = empSalary.EmployeeName;
                        objSubledgerHeads.DimensionAmount = empSalary.BonusAmount;
                        objSubledgerHeads.AccountNo = "204010155";
                        objSubledgerHeads.DimensionCode = "EMPLOYEEID";
                        objSubledgerHeads.TransactionType = "DR";
                        lstSubledgerHeads.Add(objSubledgerHeads);
                    }

                    empSalary.LocationCode = locationCode;
                    empSalary.TransactionNo = transactionNo;
                }
            }

            return lstSubledgerHeads;
        }

        private decimal DebitOrCreditConversion(string drOrCr, decimal drCrValue)
        {
            decimal debitOrCreditValue = 0;

            if (drOrCr == "DR")
            {
                debitOrCreditValue = drCrValue;
            }
            else if (drOrCr == "CR")
            {
                debitOrCreditValue = drCrValue * -1;
            }

            return debitOrCreditValue;
        }
    }
}