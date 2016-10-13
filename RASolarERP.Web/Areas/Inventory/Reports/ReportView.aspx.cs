using System;
using System.IO;
using System.Web;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Microsoft.Reporting.WebForms;

using RASolarHelper;
using RASolarERP.Model;
using RASolarAMS.Model;
using RASolarERP.Web.Models;
using RASolarERP.DomainModel.InventoryModel;
using RASolarERP.Web.Areas.Financial.Models;

namespace RASolarERP.Web.Areas.Inventory.Reports
{
    public partial class ReportView : ReportBasePage
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
            try
            {
                ReportViewerRSFReports.Height = Unit.Parse("100%");
                ReportViewerRSFReports.Width = Unit.Parse("100%");
                ReportViewerRSFReports.CssClass = "table";

                this.ReportViewerRSFReports.LocalReport.DataSources.Clear();

                ReportViewerRSFReports.ProcessingMode = ProcessingMode.Local;
                var rptPath = Server.MapPath(@"./Report/" + reportData.ReportName + ".rdlc");

                if (!File.Exists(rptPath))
                    return;

                this.ReportViewerRSFReports.LocalReport.ReportPath = rptPath;
                var rpPms = ReportViewerRSFReports.LocalReport.GetParameters();

                RASolarERPData dal = new RASolarERPData();

                if (reportData.ReportName == "ClosingInventory")
                {
                    List<ClosingInventoryValuation> objClosingInventory = new List<ClosingInventoryValuation>();
                    InventoryInTransitBalance objInventoryInTransitBalance = new InventoryInTransitBalance();

                    objClosingInventory = dal.ClosingInventoryReport(this.ReportDataObj.DataParameters[0].Value);
                    objInventoryInTransitBalance = dal.ReadInventoryInTransitBalance("SINGLEBALANCE", this.ReportDataObj.DataParameters[0].Value);

                    ReportParameter[] parms = new ReportParameter[7];
                    parms[0] = new ReportParameter("TransitBalanceNEW", objInventoryInTransitBalance.TransitBalanceNEW.ToString("0"));
                    parms[1] = new ReportParameter("TransitBalanceOLD", objInventoryInTransitBalance.TransitBalanceOLD.ToString("0"));
                    parms[2] = new ReportParameter("TransitBalanceNEWRD", objInventoryInTransitBalance.TransitBalanceNEWRD.ToString("0"));
                    parms[3] = new ReportParameter("TransitBalanceOLDRD", objInventoryInTransitBalance.TransitBalanceOLDRD.ToString("0"));
                    parms[4] = new ReportParameter("TotalTransitBalance", objInventoryInTransitBalance.TotalTransitBalance.ToString("0"));
                    parms[5] = new ReportParameter("YearMonth", Helper.DateTo(this.ReportDataObj.DataParameters[0].Value).ToString("dd-MMM-yyyy"));

                    //ReportParameter TransitBalanceNEW = new ReportParameter("TransitBalanceNEW", "0");
                    //ReportViewerRSFReports.LocalReport.SetParameters(TransitBalanceNEW);

                    //ReportParameter TransitBalanceOLD = new ReportParameter("TransitBalanceOLD", "0");
                    //ReportViewerRSFReports.LocalReport.SetParameters(TransitBalanceOLD);

                    //ReportParameter TransitBalanceNEWRD = new ReportParameter("TransitBalanceNEWRD", "0");
                    //ReportViewerRSFReports.LocalReport.SetParameters(TransitBalanceNEWRD);

                    //ReportParameter TransitBalanceOLDRD = new ReportParameter("TransitBalanceOLDRD", "0");
                    //ReportViewerRSFReports.LocalReport.SetParameters(TransitBalanceOLDRD);

                    //ReportParameter TotalTransitBalance = new ReportParameter("TotalTransitBalance", "0");
                    //ReportViewerRSFReports.LocalReport.SetParameters(TotalTransitBalance);                    

                    Decimal totalvalue = 0;
                    totalvalue = Convert.ToDecimal((objClosingInventory.Sum(i => i.ValueForNew)) + (objClosingInventory.Sum(i => i.ValueForSR)) + (objClosingInventory.Sum(i => i.ValueForRDNew)) + (objClosingInventory.Sum(i => i.ValueForInvAtVendor)) + (objClosingInventory.Sum(i => i.ValueForRDSR)));
                    parms[6] = new ReportParameter("TotalValue", Convert.ToString(totalvalue));

                    this.ReportViewerRSFReports.LocalReport.SetParameters(parms);

                    //ReportParameter YearMonth1 = new ReportParameter("YearMonth", this.ReportDataObj.DataParameters[0].Value);
                    //ReportViewerRSFReports.LocalReport.SetParameters(YearMonth1);

                    //foreach (var rpm in rpPms)
                    //{
                    //    var p = reportData.ReportParameters.SingleOrDefault(o => o.ParameterName.ToLower() == rpm.Name.ToLower());
                    //    if (p != null)
                    //    {
                    //        ReportParameter rp = new ReportParameter(rpm.Name, p.Value);
                    //        ReportViewerRSFReports.LocalReport.SetParameters(rp);
                    //    }
                    //}

                    var dsmems = ReportViewerRSFReports.LocalReport.GetDataSourceNames();
                    ReportViewerRSFReports.LocalReport.DataSources.Add(new ReportDataSource(dsmems[0], objClosingInventory));
                }
                else if (reportData.ReportName == "DailyProgressReport")
                {
                    //List<tbl_DailyProgressReport> objDailyProgressReport = new List<tbl_DailyProgressReport>();
                    //objDailyProgressReport = dal.ReadDailyProgressReport();

                    //var dsmems = ReportViewerRSFReports.LocalReport.GetDataSourceNames();
                    //ReportViewerRSFReports.LocalReport.DataSources.Add(new ReportDataSource(dsmems[0], objDailyProgressReport));
                }

                else if (reportData.ReportName == "ViewDeliveryNoteChallanSHSReport")
                {

                    List<SHSDelivaryNoteChallan> objSHSDelivaryNoteChallan = new List<SHSDelivaryNoteChallan>();

                    objSHSDelivaryNoteChallan = dal.ViewDeliveryNoteChallanSHSReport(this.ReportDataObj.DataParameters[0].Value);

                    var dsmems = ReportViewerRSFReports.LocalReport.GetDataSourceNames();
                    ReportViewerRSFReports.LocalReport.DataSources.Add(new ReportDataSource(dsmems[0], objSHSDelivaryNoteChallan));

                }


                // Refresh the ReportViewer.
                ReportViewerRSFReports.LocalReport.Refresh();
            }

            catch (Exception ex)
            {

                ReportViewerRSFReports.DocumentMapCollapsed = true;
                ReportViewerRSFReports.LocalReport.Refresh();

            }
        }
    }
}