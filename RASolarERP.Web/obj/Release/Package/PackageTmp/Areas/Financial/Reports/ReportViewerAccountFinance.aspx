<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportViewerAccountFinance.aspx.cs" Inherits="RASolarERP.Web.Areas.Financial.Reports.ReportViewerAccountFinance" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <rsweb:ReportViewer ID="ReportViewerFinancial" runat="server" AsyncRendering="false"></rsweb:ReportViewer>
    </form>
</body>
</html>
