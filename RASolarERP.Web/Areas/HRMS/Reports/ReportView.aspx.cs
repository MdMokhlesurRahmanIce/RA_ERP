using System;
using System.IO;
using System.Web;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Microsoft.Reporting.WebForms;

using RASolarHelper;
using RASolarERP.Web.Areas.HRMS.Models;
using RASolarERP.DomainModel.HRMSModel;

namespace RASolarERP.Web.Areas.HRMS.Reports
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
            HRMSData hraDal = new HRMSData();

            try
            {
                ReportViewerHRAReports.Height = Unit.Parse("100%");
                ReportViewerHRAReports.Width = Unit.Parse("100%");
                ReportViewerHRAReports.CssClass = "table";

                this.ReportViewerHRAReports.LocalReport.DataSources.Clear();

                ReportViewerHRAReports.ProcessingMode = ProcessingMode.Local;
                var rptPath = Server.MapPath(@"./Report/" + reportData.ReportName + ".rdlc"); //reportData.ReportName

                if (!File.Exists(rptPath))
                    return;

                this.ReportViewerHRAReports.LocalReport.ReportPath = rptPath;
                var rpPms = ReportViewerHRAReports.LocalReport.GetParameters();

                if (reportData.ReportName == "BankAdviceForSalary" || reportData.ReportName == "BankAdviceForSalaryWithBonus" || reportData.ReportName == "BankAdviceForBonus")
                {
                    string forMonth = string.Empty, reportType = string.Empty, generateType = string.Empty;
                    string locationCode = string.Empty, bankAccount = string.Empty;

                    forMonth = this.ReportDataObj.DataParameters[0].Value;
                    reportType = this.ReportDataObj.DataParameters[1].Value;
                    generateType = this.ReportDataObj.DataParameters[2].Value;
                    locationCode = this.ReportDataObj.DataParameters[3].Value;
                    bankAccount = this.ReportDataObj.DataParameters[4].Value;

                    List<BankAdviceForSalaryReport> lstBankAdviceForSalaryReport = new List<BankAdviceForSalaryReport>();
                    lstBankAdviceForSalaryReport = hraDal.ReadBankAdviceForSalaryReport(forMonth, reportType, generateType, locationCode, bankAccount);

                    if (lstBankAdviceForSalaryReport.Count == 0)
                    {
                        lstBankAdviceForSalaryReport.Add(new BankAdviceForSalaryReport());
                    }

                    int dds = 0;
                    if (lstBankAdviceForSalaryReport.Count > 0)
                    {
                        dds = lstBankAdviceForSalaryReport.Max(m => m.AdviceLetterReferenceSeqNo);
                    }

                    EndLetterSequenceNoToShow.InnerText = dds.ToString();

                    ReportParameter[] parms = new ReportParameter[2];
                    parms[0] = new ReportParameter("SalaryBankAdviceMonth", Helper.DateFrom(forMonth).ToString("MMMM, yyyy"));
                    parms[1] = new ReportParameter("SalaryBankAdviceDate", lstBankAdviceForSalaryReport[0].CreatedOn.ToString("MMMM dd, yyyy"));

                    this.ReportViewerHRAReports.LocalReport.SetParameters(parms);

                    var dsmems = ReportViewerHRAReports.LocalReport.GetDataSourceNames();
                    ReportViewerHRAReports.LocalReport.DataSources.Add(new ReportDataSource(dsmems[0], lstBankAdviceForSalaryReport));
                }
                //LatterToCustomer
                else if (reportData.ReportName == "LetterToCustomer")
                {
                    string OCT_End_CDODCUSTOMERS = "OCT_End_CDODCUSTOMERS";

                    List<BankAdviceForSalaryLatterReport> lstBankAdviceForSalaryLatterReport = new List<BankAdviceForSalaryLatterReport>();
                    lstBankAdviceForSalaryLatterReport = hraDal.ReadBankAdviceForSalaryLatterReport(OCT_End_CDODCUSTOMERS);

                    if (lstBankAdviceForSalaryLatterReport.Count == 0)
                    {
                        lstBankAdviceForSalaryLatterReport.Add(new BankAdviceForSalaryLatterReport());
                    }
                    var dsmems = ReportViewerHRAReports.LocalReport.GetDataSourceNames();
                    ReportViewerHRAReports.LocalReport.DataSources.Add(new ReportDataSource(dsmems[0], lstBankAdviceForSalaryLatterReport));
                }
                //
                else if (reportData.ReportName == "LetterToChairman")
                {
                    string OCT_End_CDODCUSTOMERS = "OCT_End_CDODCUSTOMERSUPC";

                    List<LatterToChairmanReport> lstLatterToChairmanReport = new List<LatterToChairmanReport>();
                    lstLatterToChairmanReport = hraDal.ReadLatterToChairmanReport(OCT_End_CDODCUSTOMERS);

                    if (lstLatterToChairmanReport.Count == 0)
                    {
                        lstLatterToChairmanReport.Add(new LatterToChairmanReport());
                    }
                    var dsmems = ReportViewerHRAReports.LocalReport.GetDataSourceNames();
                    ReportViewerHRAReports.LocalReport.DataSources.Add(new ReportDataSource(dsmems[0], lstLatterToChairmanReport));
                }

                else if (reportData.ReportName == "LetterToUNO")
                {
                    string OCT_End_CDODCUSTOMERS = "OCT_End_CDODCUSTOMERSUNO";

                    List<LatterToUpoZelaChairmanReport> lstLatterToUpoZelaChairmanReport = new List<LatterToUpoZelaChairmanReport>();
                    lstLatterToUpoZelaChairmanReport = hraDal.ReadLatterToUpoZelaChairmanReport(OCT_End_CDODCUSTOMERS);

                    if (lstLatterToUpoZelaChairmanReport.Count == 0)
                    {
                        lstLatterToUpoZelaChairmanReport.Add(new LatterToUpoZelaChairmanReport());
                    }
                    var dsmems = ReportViewerHRAReports.LocalReport.GetDataSourceNames();
                    ReportViewerHRAReports.LocalReport.DataSources.Add(new ReportDataSource(dsmems[0], lstLatterToUpoZelaChairmanReport));
                }

                else if (reportData.ReportName == "Envelope")
                {
                    string OCT_End_CDODCUSTOMERS = "OCT_End_CDODCUSTOMERS";

                    List<BankAdviceForSalaryLatterReport> lstBankAdviceForSalaryLatterReport = new List<BankAdviceForSalaryLatterReport>();
                    lstBankAdviceForSalaryLatterReport = hraDal.ReadBankAdviceForSalaryLatterReport(OCT_End_CDODCUSTOMERS);

                    if (lstBankAdviceForSalaryLatterReport.Count == 0)
                    {
                        lstBankAdviceForSalaryLatterReport.Add(new BankAdviceForSalaryLatterReport());
                    }
                    var dsmems = ReportViewerHRAReports.LocalReport.GetDataSourceNames();
                    ReportViewerHRAReports.LocalReport.DataSources.Add(new ReportDataSource(dsmems[0], lstBankAdviceForSalaryLatterReport));
                }//AttachmentToUnionReport

                else if (reportData.ReportName == "AttachmentToUnionReport")
                {
                    string OCT_End_CDODCUSTOMERS = "OCT_End_CDODCUSTOMERS";

                    List<BankAdviceForSalaryLatterReport> lstBankAdviceForSalaryLatterReport = new List<BankAdviceForSalaryLatterReport>();
                    lstBankAdviceForSalaryLatterReport = hraDal.ReadBankAdviceForSalaryLatterReport(OCT_End_CDODCUSTOMERS);

                    if (lstBankAdviceForSalaryLatterReport.Count == 0)
                    {
                        lstBankAdviceForSalaryLatterReport.Add(new BankAdviceForSalaryLatterReport());
                    }
                    var dsmems = ReportViewerHRAReports.LocalReport.GetDataSourceNames();
                    ReportViewerHRAReports.LocalReport.DataSources.Add(new ReportDataSource(dsmems[0], lstBankAdviceForSalaryLatterReport));
                }

                else if (reportData.ReportName == "AttachmentToUpazilaReport")
                {
                    string OCT_End_CDODCUSTOMERS = "OCT_End_CDODCUSTOMERS";

                    List<BankAdviceForSalaryLatterReport> lstBankAdviceForSalaryLatterReport = new List<BankAdviceForSalaryLatterReport>();
                    lstBankAdviceForSalaryLatterReport = hraDal.ReadBankAdviceForSalaryLatterReport(OCT_End_CDODCUSTOMERS);

                    if (lstBankAdviceForSalaryLatterReport.Count == 0)
                    {
                        lstBankAdviceForSalaryLatterReport.Add(new BankAdviceForSalaryLatterReport());
                    }
                    var dsmems = ReportViewerHRAReports.LocalReport.GetDataSourceNames();
                    ReportViewerHRAReports.LocalReport.DataSources.Add(new ReportDataSource(dsmems[0], lstBankAdviceForSalaryLatterReport));
                }

                ReportViewerHRAReports.LocalReport.Refresh();
            }
            catch (Exception ex)
            {
                ReportViewerHRAReports.DocumentMapCollapsed = true;
                ReportViewerHRAReports.LocalReport.Refresh();

                throw ex;
            }
        }
    }
}