<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportView.aspx.cs" Inherits="RASolarERP.Web.Areas.HRMS.Reports.ReportView" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>    
</head>
<body>   
    <div id="EndLetterSequenceNoToShow" runat="server" style="display:none"></div>
    <form id="form1" runat="server">        
        <div style="clear: both; width: 998px; margin: 0 auto;">            
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <rsweb:ReportViewer ID="ReportViewerHRAReports" runat="server"  AsyncRendering="false" ShowPrintButton="true" ZoomMode="FullPage" SizeToReportContent="False" Width="1100px"></rsweb:ReportViewer>
        </div>
    </form>
</body>
</html>
