using System;
using System.IO;
using System.Web;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Microsoft.Reporting.WebForms;

using RASolarHelper;
using RASolarAMS.Model;
using RASolarERP.Model;
using RASolarERP.Web.Models;
using RASolarERP.Web.Areas.Financial.Models;

namespace RASolarERP.Web.Areas.Financial.Reports
{
    public partial class ReportViewerAccountFinance : ReportBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RenderReportModels(this.ReportDataObj);
            }
        }
        private void RenderReportModels(ReportData reportData)
        {
            AccountingReportData accountDal = new AccountingReportData();

            // Reset report properties.
            ReportViewerFinancial.Height = Unit.Parse("100%");
            ReportViewerFinancial.Width = Unit.Parse("100%");
            ReportViewerFinancial.CssClass = "table";

            // Clear out any previous datasources.
            this.ReportViewerFinancial.LocalReport.DataSources.Clear();

            // Set report mode for local processing.
            ReportViewerFinancial.ProcessingMode = ProcessingMode.Local;

            // Validate report source.
            var rptPath = Server.MapPath(@"./Report/" + reportData.ReportName + ".rdlc");

            // @"E:\RSFERP_SourceCode\RASolarERP\RASolarERP\Areas\Financial\Reports\Report\StatementOfFinancilaPosition.rdlc";
            //Server.MapPath(@"./Report/" + reportData.ReportName + ".rdlc")

            if (!File.Exists(rptPath))
                return;

            if (reportData.ReportName == "StatementOfFinancilaPosition")
            {
                List<AccountBalanceSheet> objAccountBalanceSheet = new List<AccountBalanceSheet>();
                //Set data paramater for report SP execution
                objAccountBalanceSheet = accountDal.ReadAccountBalanceSheet(Helper.DateFormatMMDDYYYY(this.ReportDataObj.DataParameters[0].Value));

                // Set report path.
                this.ReportViewerFinancial.LocalReport.ReportPath = rptPath;

                // Set report parameters.
                var rpPms = ReportViewerFinancial.LocalReport.GetParameters();


                //decimal TotalCurrentAssestsCurrent = Convert.ToDecimal(objAccountBalanceSheet[3].CurrentYearBalance) +
                //                                    Convert.ToDecimal(objAccountBalanceSheet[4].CurrentYearBalance) +
                //                                    Convert.ToDecimal(objAccountBalanceSheet[5].CurrentYearBalance) +
                //                                    Convert.ToDecimal(objAccountBalanceSheet[6].CurrentYearBalance) +
                //                                    Convert.ToDecimal(objAccountBalanceSheet[7].CurrentYearBalance) +
                //                                    Convert.ToDecimal(objAccountBalanceSheet[8].CurrentYearBalance) +
                //                                    Convert.ToDecimal(objAccountBalanceSheet[9].CurrentYearBalance) +
                //                                    Convert.ToDecimal(objAccountBalanceSheet[19].CurrentYearBalance);

                //decimal TotalCurrentAssestsPrevious = Convert.ToDecimal(objAccountBalanceSheet[3].LastYearBalance) +
                //                                    Convert.ToDecimal(objAccountBalanceSheet[4].LastYearBalance) +
                //                                    Convert.ToDecimal(objAccountBalanceSheet[5].LastYearBalance) +
                //                                    Convert.ToDecimal(objAccountBalanceSheet[6].LastYearBalance) +
                //                                    Convert.ToDecimal(objAccountBalanceSheet[7].LastYearBalance) +
                //                                    Convert.ToDecimal(objAccountBalanceSheet[8].LastYearBalance) +
                //                                    Convert.ToDecimal(objAccountBalanceSheet[9].LastYearBalance) +
                //                                    Convert.ToDecimal(objAccountBalanceSheet[19].LastYearBalance);


                //decimal CurrentLiabilitiesCurrent = Convert.ToDecimal(objAccountBalanceSheet[10].CurrentYearBalance) +
                //                                    Convert.ToDecimal(objAccountBalanceSheet[11].CurrentYearBalance) +
                //                                    Convert.ToDecimal(objAccountBalanceSheet[12].CurrentYearBalance) +
                //                                    Convert.ToDecimal(objAccountBalanceSheet[13].CurrentYearBalance) +
                //                                    Convert.ToDecimal(objAccountBalanceSheet[14].CurrentYearBalance);


                //decimal CurrentLiabilitiesPrevious = Convert.ToDecimal(objAccountBalanceSheet[10].LastYearBalance) +
                //                                    Convert.ToDecimal(objAccountBalanceSheet[11].LastYearBalance) +
                //                                    Convert.ToDecimal(objAccountBalanceSheet[12].LastYearBalance) +
                //                                    Convert.ToDecimal(objAccountBalanceSheet[13].LastYearBalance) +
                //                                    Convert.ToDecimal(objAccountBalanceSheet[14].LastYearBalance);

                //decimal NetCurrentAssetCurrent = TotalCurrentAssestsCurrent - (CurrentLiabilitiesCurrent < 0 ? CurrentLiabilitiesCurrent * -1 : CurrentLiabilitiesCurrent);
                //decimal NetCurrentAssetPrevious = TotalCurrentAssestsPrevious - (CurrentLiabilitiesPrevious < 0 ? CurrentLiabilitiesPrevious * -1 : CurrentLiabilitiesPrevious);

                //decimal PropertyPlanequipmentCurrent = Convert.ToDecimal(objAccountBalanceSheet[0].CurrentYearBalance + objAccountBalanceSheet[1].CurrentYearBalance);
                //decimal PropertyPlanequipmentPrevious = Convert.ToDecimal(objAccountBalanceSheet[0].LastYearBalance + objAccountBalanceSheet[1].LastYearBalance);
                //decimal IntangibleAssetsCurrent = Convert.ToDecimal(objAccountBalanceSheet[2].CurrentYearBalance);
                //decimal IntangibleAssetsPrevious = Convert.ToDecimal(objAccountBalanceSheet[2].LastYearBalance);

                //decimal TotalAssetCurrent = NetCurrentAssetCurrent + PropertyPlanequipmentCurrent + IntangibleAssetsCurrent;
                //decimal TotalAssetPrevious = NetCurrentAssetPrevious + PropertyPlanequipmentPrevious + IntangibleAssetsPrevious;

                //decimal SourceOfFundCurrent = Convert.ToDecimal(objAccountBalanceSheet[15].CurrentYearBalance) +
                //                                   Convert.ToDecimal(objAccountBalanceSheet[17].CurrentYearBalance);
                //decimal SourceOfFundPrevious = Convert.ToDecimal(objAccountBalanceSheet[15].LastYearBalance) +
                //                                    Convert.ToDecimal(objAccountBalanceSheet[17].LastYearBalance);

                //decimal TotalFundLiabilitiesCurrent = SourceOfFundCurrent + objAccountBalanceSheet[18].CurrentYearBalance;
                //decimal TotalFundLiabilitiesPrevious = SourceOfFundPrevious + objAccountBalanceSheet[18].LastYearBalance;

                //ReportParameter ReportYear = new ReportParameter("ReportYear", Helper.DateFormat(Convert.ToDateTime(Helper.DateFormatMMDDYYYY(this.ReportDataObj.DataParameters[0].Value))));
                //ReportViewerFinancial.LocalReport.SetParameters(ReportYear);

                //ReportParameter AmountCurrentYear = new ReportParameter("AmountCurrentYear", "2011-12");
                //ReportViewerFinancial.LocalReport.SetParameters(AmountCurrentYear);

                //ReportParameter AmountPreviousYear = new ReportParameter("AmountPreviousYear", "2010-11");
                //ReportViewerFinancial.LocalReport.SetParameters(AmountPreviousYear);

                //ReportParameter AccumulatedDepriceationCurrentYear = new ReportParameter("AccumulatedDepriceationCurrentYear", Convert.ToString(objAccountBalanceSheet[0].CurrentYearBalance + objAccountBalanceSheet[1].CurrentYearBalance));
                //ReportViewerFinancial.LocalReport.SetParameters(AccumulatedDepriceationCurrentYear);

                //ReportParameter AccumulatedDepriceationPreviousYear = new ReportParameter("AccumulatedDepriceationPreviousYear", Convert.ToString(objAccountBalanceSheet[0].LastYearBalance + objAccountBalanceSheet[1].LastYearBalance));
                //ReportViewerFinancial.LocalReport.SetParameters(AccumulatedDepriceationPreviousYear);

                //ReportParameter IntangibleAssestAmountCurrentYear = new ReportParameter("IntangibleAssestAmountCurrentYear", Convert.ToString(objAccountBalanceSheet[2].CurrentYearBalance));
                //ReportViewerFinancial.LocalReport.SetParameters(IntangibleAssestAmountCurrentYear);

                //ReportParameter IntangibleAssestAmountPreviousYear = new ReportParameter("IntangibleAssestAmountPreviousYear", Convert.ToString(objAccountBalanceSheet[2].LastYearBalance));
                //ReportViewerFinancial.LocalReport.SetParameters(IntangibleAssestAmountPreviousYear);

                //ReportParameter InventoryCurrent = new ReportParameter("InventoryCurrent", Convert.ToString(objAccountBalanceSheet[3].CurrentYearBalance));
                //ReportViewerFinancial.LocalReport.SetParameters(InventoryCurrent);

                //ReportParameter InventoryPrevious = new ReportParameter("InventoryPrevious", Convert.ToString(objAccountBalanceSheet[3].LastYearBalance));
                //ReportViewerFinancial.LocalReport.SetParameters(InventoryPrevious);

                //ReportParameter AdjustmentAccountCurrent = new ReportParameter("AdjustmentAccountCurrent", Convert.ToString(objAccountBalanceSheet[19].CurrentYearBalance));
                //ReportViewerFinancial.LocalReport.SetParameters(AdjustmentAccountCurrent);

                //ReportParameter AdjustmentAccountPrevious = new ReportParameter("AdjustmentAccountPrevious", Convert.ToString(objAccountBalanceSheet[19].LastYearBalance));
                //ReportViewerFinancial.LocalReport.SetParameters(AdjustmentAccountPrevious);

                //ReportParameter AccountReceivableCurrent = new ReportParameter("AccountReceivableCurrent", Convert.ToString(objAccountBalanceSheet[4].CurrentYearBalance));
                //ReportViewerFinancial.LocalReport.SetParameters(AccountReceivableCurrent);

                //ReportParameter AccountReceivablePrevious = new ReportParameter("AccountReceivablePrevious", Convert.ToString(objAccountBalanceSheet[4].LastYearBalance));
                //ReportViewerFinancial.LocalReport.SetParameters(AccountReceivablePrevious);

                //ReportParameter AdvanceDepositPaymentCurrent = new ReportParameter("AdvanceDepositPaymentCurrent", Convert.ToString(objAccountBalanceSheet[5].CurrentYearBalance));
                //ReportViewerFinancial.LocalReport.SetParameters(AdvanceDepositPaymentCurrent);

                //ReportParameter AdvanceDepositPaymentPrevious = new ReportParameter("AdvanceDepositPaymentPrevious", Convert.ToString(objAccountBalanceSheet[5].LastYearBalance));
                //ReportViewerFinancial.LocalReport.SetParameters(AdvanceDepositPaymentPrevious);

                //ReportParameter PersonalLoanMotorCycleCurrent = new ReportParameter("PersonalLoanMotorCycleCurrent", Convert.ToString(objAccountBalanceSheet[6].CurrentYearBalance));
                //ReportViewerFinancial.LocalReport.SetParameters(PersonalLoanMotorCycleCurrent);

                //ReportParameter PersonalLoanMotorCyclePrevious = new ReportParameter("PersonalLoanMotorCyclePrevious", Convert.ToString(objAccountBalanceSheet[6].LastYearBalance));
                //ReportViewerFinancial.LocalReport.SetParameters(PersonalLoanMotorCyclePrevious);

                //ReportParameter AccruedInterestIncomeFDRCurrent = new ReportParameter("AccruedInterestIncomeFDRCurrent", Convert.ToString(objAccountBalanceSheet[7].CurrentYearBalance));
                //ReportViewerFinancial.LocalReport.SetParameters(AccruedInterestIncomeFDRCurrent);

                //ReportParameter AccruedInterestIncomeFDRPrevious = new ReportParameter("AccruedInterestIncomeFDRPrevious", Convert.ToString(objAccountBalanceSheet[7].LastYearBalance));
                //ReportViewerFinancial.LocalReport.SetParameters(AccruedInterestIncomeFDRPrevious);

                //ReportParameter AdvanceIncomeTaxCurrent = new ReportParameter("AdvanceIncomeTaxCurrent", Convert.ToString(objAccountBalanceSheet[8].CurrentYearBalance));
                //ReportViewerFinancial.LocalReport.SetParameters(AdvanceIncomeTaxCurrent);

                //ReportParameter AdvanceIncomeTaxPrevious = new ReportParameter("AdvanceIncomeTaxPrevious", Convert.ToString(objAccountBalanceSheet[8].LastYearBalance));
                //ReportViewerFinancial.LocalReport.SetParameters(AdvanceIncomeTaxPrevious);

                //ReportParameter CashEquipmentCurrent = new ReportParameter("CashEquipmentCurrent", Convert.ToString(objAccountBalanceSheet[9].CurrentYearBalance));
                //ReportViewerFinancial.LocalReport.SetParameters(CashEquipmentCurrent);

                //ReportParameter CashEquipmentPrevious = new ReportParameter("CashEquipmentPrevious", Convert.ToString(objAccountBalanceSheet[9].LastYearBalance));
                //ReportViewerFinancial.LocalReport.SetParameters(CashEquipmentPrevious);

                //ReportParameter AccountPayableCurrent = new ReportParameter("AccountPayableCurrent", Convert.ToString(objAccountBalanceSheet[10].CurrentYearBalance));
                //ReportViewerFinancial.LocalReport.SetParameters(AccountPayableCurrent);

                //ReportParameter AccountPayablePrevious = new ReportParameter("AccountPayablePrevious", Convert.ToString(objAccountBalanceSheet[10].LastYearBalance));
                //ReportViewerFinancial.LocalReport.SetParameters(AccountPayablePrevious);

                //ReportParameter LiabilitiesExpensesCurrent = new ReportParameter("LiabilitiesExpensesCurrent", Convert.ToString(objAccountBalanceSheet[11].CurrentYearBalance));
                //ReportViewerFinancial.LocalReport.SetParameters(LiabilitiesExpensesCurrent);

                //ReportParameter LiabilitiesExpensesPrevious = new ReportParameter("LiabilitiesExpensesPrevious", Convert.ToString(objAccountBalanceSheet[11].LastYearBalance));
                //ReportViewerFinancial.LocalReport.SetParameters(LiabilitiesExpensesPrevious);

                //ReportParameter ProvisionBadDebtsCurrent = new ReportParameter("ProvisionBadDebtsCurrent", Convert.ToString(objAccountBalanceSheet[12].CurrentYearBalance));
                //ReportViewerFinancial.LocalReport.SetParameters(ProvisionBadDebtsCurrent);

                //ReportParameter ProvisionBadDebtsPrevious = new ReportParameter("ProvisionBadDebtsPrevious", Convert.ToString(objAccountBalanceSheet[12].LastYearBalance));
                //ReportViewerFinancial.LocalReport.SetParameters(ProvisionBadDebtsPrevious);

                //ReportParameter ShortTermLoanCurrent = new ReportParameter("ShortTermLoanCurrent", Convert.ToString(objAccountBalanceSheet[13].CurrentYearBalance));
                //ReportViewerFinancial.LocalReport.SetParameters(ShortTermLoanCurrent);

                //ReportParameter ShortTermLoanPrevious = new ReportParameter("ShortTermLoanPrevious", Convert.ToString(objAccountBalanceSheet[13].LastYearBalance));
                //ReportViewerFinancial.LocalReport.SetParameters(ShortTermLoanPrevious);

                //ReportParameter ProvisionInterestCurrent = new ReportParameter("ProvisionInterestCurrent", Convert.ToString(objAccountBalanceSheet[14].CurrentYearBalance));
                //ReportViewerFinancial.LocalReport.SetParameters(ProvisionInterestCurrent);

                //ReportParameter ProvisionInterestPrevious = new ReportParameter("ProvisionInterestPrevious", Convert.ToString(objAccountBalanceSheet[14].LastYearBalance));
                //ReportViewerFinancial.LocalReport.SetParameters(ProvisionInterestPrevious);

                //ReportParameter AssestsTotalCurrent = new ReportParameter("AssestsTotalCurrent", Convert.ToString(TotalCurrentAssestsCurrent));
                //ReportViewerFinancial.LocalReport.SetParameters(AssestsTotalCurrent);

                //ReportParameter AssestsTotalPrevious = new ReportParameter("AssestsTotalPrevious", Convert.ToString(TotalCurrentAssestsPrevious));
                //ReportViewerFinancial.LocalReport.SetParameters(AssestsTotalPrevious);

                //ReportParameter LiabilitiesTotalCurrent = new ReportParameter("LiabilitiesTotalCurrent", Convert.ToString(CurrentLiabilitiesCurrent));
                //ReportViewerFinancial.LocalReport.SetParameters(LiabilitiesTotalCurrent);

                //ReportParameter LiabilitiesTotalPrevious = new ReportParameter("LiabilitiesTotalPrevious", Convert.ToString(CurrentLiabilitiesPrevious));
                //ReportViewerFinancial.LocalReport.SetParameters(LiabilitiesTotalPrevious);

                //ReportParameter NetAssestsCurrent = new ReportParameter("NetAssestsCurrent", Convert.ToString(NetCurrentAssetCurrent));
                //ReportViewerFinancial.LocalReport.SetParameters(NetAssestsCurrent);

                //ReportParameter NetAssestsPrevious = new ReportParameter("NetAssestsPrevious", Convert.ToString(NetCurrentAssetPrevious));
                //ReportViewerFinancial.LocalReport.SetParameters(NetAssestsPrevious);

                //ReportParameter TotalAssestsCurrent = new ReportParameter("TotalAssestsCurrent", Convert.ToString(TotalAssetCurrent));
                //ReportViewerFinancial.LocalReport.SetParameters(TotalAssestsCurrent);

                //ReportParameter TotalAssestsPrevious = new ReportParameter("TotalAssestsPrevious", Convert.ToString(TotalAssetPrevious));
                //ReportViewerFinancial.LocalReport.SetParameters(TotalAssestsPrevious);

                //ReportParameter CapitalFundCurrent = new ReportParameter("CapitalFundCurrent", Convert.ToString(objAccountBalanceSheet[15].CurrentYearBalance));
                //ReportViewerFinancial.LocalReport.SetParameters(CapitalFundCurrent);

                //ReportParameter CapitalFundPrevious = new ReportParameter("CapitalFundPrevious", Convert.ToString(objAccountBalanceSheet[15].LastYearBalance));
                //ReportViewerFinancial.LocalReport.SetParameters(CapitalFundPrevious);

                //ReportParameter RetainedEarningCurrent = new ReportParameter("RetainedEarningCurrent", Convert.ToString(objAccountBalanceSheet[17].CurrentYearBalance));
                //ReportViewerFinancial.LocalReport.SetParameters(RetainedEarningCurrent);

                //ReportParameter RetainedEarningPrevious = new ReportParameter("RetainedEarningPrevious", Convert.ToString(objAccountBalanceSheet[17].LastYearBalance));
                //ReportViewerFinancial.LocalReport.SetParameters(RetainedEarningPrevious);

                //ReportParameter TermLoanLongPortionCurrent = new ReportParameter("TermLoanLongPortionCurrent", Convert.ToString(objAccountBalanceSheet[18].CurrentYearBalance));
                //ReportViewerFinancial.LocalReport.SetParameters(TermLoanLongPortionCurrent);

                //ReportParameter TermLoanLongPortionPrevious = new ReportParameter("TermLoanLongPortionPrevious", Convert.ToString(objAccountBalanceSheet[18].LastYearBalance));
                //ReportViewerFinancial.LocalReport.SetParameters(TermLoanLongPortionPrevious);

                //ReportParameter SourceOfFundTotalCurrent = new ReportParameter("SourceOfFundTotalCurrent", Convert.ToString(SourceOfFundCurrent));
                //ReportViewerFinancial.LocalReport.SetParameters(SourceOfFundTotalCurrent);

                //ReportParameter SourceOfFundTotalPrevious = new ReportParameter("SourceOfFundTotalPrevious", Convert.ToString(SourceOfFundPrevious));
                //ReportViewerFinancial.LocalReport.SetParameters(SourceOfFundTotalPrevious);

                //ReportParameter TotalFundAndLiabilitiesCurrent = new ReportParameter("TotalFundAndLiabilitiesCurrent", Convert.ToString(TotalFundLiabilitiesCurrent));
                //ReportViewerFinancial.LocalReport.SetParameters(TotalFundAndLiabilitiesCurrent);

                //ReportParameter TotalFundAndLiabilitiesPrevious = new ReportParameter("TotalFundAndLiabilitiesPrevious", Convert.ToString(TotalFundLiabilitiesPrevious));
                //ReportViewerFinancial.LocalReport.SetParameters(TotalFundAndLiabilitiesPrevious);

                // Load the dataSource.
                var dsmems = ReportViewerFinancial.LocalReport.GetDataSourceNames();
                ReportViewerFinancial.LocalReport.DataSources.Add(new ReportDataSource(dsmems[0], objAccountBalanceSheet));
            }
            else if (reportData.ReportName == "IncomeStatement")
            {
                string months = Convert.ToDateTime(Helper.DateFormatMMDDYYYY(this.ReportDataObj.DataParameters[0].Value)).ToString("MMM-yyyy");

                List<AccIncomeStatementForMonthlyMIS> objMIS = new List<AccIncomeStatementForMonthlyMIS>();
                objMIS = accountDal.ReadAccIncomeStatementForMonthlyMIS(Helper.DateFormatMMDDYYYY(this.ReportDataObj.DataParameters[0].Value), Helper.DateFormatMMDDYYYY(this.ReportDataObj.DataParameters[1].Value));

                this.ReportViewerFinancial.LocalReport.ReportPath = rptPath;
                var rpPms = ReportViewerFinancial.LocalReport.GetParameters();

                //decimal TotalRevenueTotal = objMIS[0].Amount + objMIS[1].Amount + objMIS[2].Amount + objMIS[3].Amount;
                //decimal TotalOperatingExpensesTotal = objMIS[5].Amount + objMIS[6].Amount + objMIS[7].Amount + objMIS[8].Amount;
                //decimal TotalInterestExpenseTotal = objMIS[9].Amount;

                //ReportParameter ReportYear = new ReportParameter("ReportYear", months);
                //ReportViewerFinancial.LocalReport.SetParameters(ReportYear);

                //ReportParameter ParticularsYear = new ReportParameter("ParticularsYear", months);
                //ReportViewerFinancial.LocalReport.SetParameters(ParticularsYear);

                //ReportParameter RevenueFromSales = new ReportParameter("RevenueFromSales", Convert.ToString(objMIS[0].Amount));
                //ReportViewerFinancial.LocalReport.SetParameters(RevenueFromSales);

                //ReportParameter RevenueFromServiceCharge = new ReportParameter("RevenueFromServiceCharge", Convert.ToString(objMIS[1].Amount));
                //ReportViewerFinancial.LocalReport.SetParameters(RevenueFromServiceCharge);

                //ReportParameter Grant = new ReportParameter("Grant", Convert.ToString(objMIS[2].Amount));
                //ReportViewerFinancial.LocalReport.SetParameters(Grant);

                //ReportParameter RevenueOthers = new ReportParameter("RevenueOthers", Convert.ToString(objMIS[3].Amount));
                //ReportViewerFinancial.LocalReport.SetParameters(RevenueOthers);

                //ReportParameter TotalRevenue = new ReportParameter("TotalRevenue", Convert.ToString(TotalRevenueTotal));
                //ReportViewerFinancial.LocalReport.SetParameters(TotalRevenue);

                //ReportParameter LessCostOfGoodsSold = new ReportParameter("LessCostOfGoodsSold", Convert.ToString(objMIS[4].Amount));
                //ReportViewerFinancial.LocalReport.SetParameters(LessCostOfGoodsSold);

                //ReportParameter EmployeeExpenses = new ReportParameter("EmployeeExpenses", Convert.ToString(objMIS[5].Amount));
                //ReportViewerFinancial.LocalReport.SetParameters(EmployeeExpenses);

                //ReportParameter AdministrativeExpenses = new ReportParameter("AdministrativeExpenses", Convert.ToString(objMIS[6].Amount));
                //ReportViewerFinancial.LocalReport.SetParameters(AdministrativeExpenses);

                //ReportParameter SellingDistributionExpenses = new ReportParameter("SellingDistributionExpenses", Convert.ToString(objMIS[7].Amount));
                //ReportViewerFinancial.LocalReport.SetParameters(SellingDistributionExpenses);

                //ReportParameter MarketingPromotionalExpenses = new ReportParameter("MarketingPromotionalExpenses", Convert.ToString(objMIS[8].Amount));
                //ReportViewerFinancial.LocalReport.SetParameters(MarketingPromotionalExpenses);

                //ReportParameter TotalOperatingExpenses = new ReportParameter("TotalOperatingExpenses", Convert.ToString(TotalOperatingExpensesTotal));
                //ReportViewerFinancial.LocalReport.SetParameters(TotalOperatingExpenses);

                //ReportParameter Idcol = new ReportParameter("Idcol", Convert.ToString(objMIS[9].Amount));
                //ReportViewerFinancial.LocalReport.SetParameters(Idcol);

                //ReportParameter TotalInterestExpense = new ReportParameter("TotalInterestExpense", Convert.ToString(TotalInterestExpenseTotal));
                //ReportViewerFinancial.LocalReport.SetParameters(TotalInterestExpense);

                //ReportParameter SocialDevelopmentExpenses = new ReportParameter("SocialDevelopmentExpenses", Convert.ToString(objMIS[10].Amount));
                //ReportViewerFinancial.LocalReport.SetParameters(SocialDevelopmentExpenses);

                var dsmems = ReportViewerFinancial.LocalReport.GetDataSourceNames();
                ReportViewerFinancial.LocalReport.DataSources.Add(new ReportDataSource(dsmems[0], objMIS));
            }
            else if (reportData.ReportName == "CustomerTrainingSummary")
            {

                DateTime fromDate = Convert.ToDateTime(Helper.DateFormatMMDDYYYY(this.ReportDataObj.DataParameters[0].Value));
                DateTime toDate = Convert.ToDateTime(Helper.DateFormatMMDDYYYY(this.ReportDataObj.DataParameters[1].Value));

                RASolarERPData erpDal = new RASolarERPData();

                List<CustomerTrainingSummary> objMIS = new List<CustomerTrainingSummary>();
                objMIS = erpDal.ReadCustomerTraining(fromDate, toDate);

                this.ReportViewerFinancial.LocalReport.ReportPath = rptPath;
                var rpPms = ReportViewerFinancial.LocalReport.GetParameters();


                var dsmems = ReportViewerFinancial.LocalReport.GetDataSourceNames();
                ReportViewerFinancial.LocalReport.DataSources.Add(new ReportDataSource(dsmems[0], objMIS));
            }

            else if (reportData.ReportName == "CustomerTrainingDetails")
            {

                string fromDate = Helper.DateFormatDDMMMYYYY(this.ReportDataObj.DataParameters[0].Value);
                string toDate = Helper.DateFormatDDMMMYYYY(this.ReportDataObj.DataParameters[1].Value);

                RASolarERPData erpDal = new RASolarERPData();

                List<CustomerTrainingDetails> objMIS = new List<CustomerTrainingDetails>();
                objMIS = erpDal.ReadCustomerTrainingDetails(fromDate, toDate);

                this.ReportViewerFinancial.LocalReport.ReportPath = rptPath;
                var rpPms = ReportViewerFinancial.LocalReport.GetParameters();

                var dsmems = ReportViewerFinancial.LocalReport.GetDataSourceNames();
                ReportViewerFinancial.LocalReport.DataSources.Add(new ReportDataSource(dsmems[0], objMIS));
            }

            // Refresh the ReportViewer.
            ReportViewerFinancial.LocalReport.Refresh();
        }

    }
}