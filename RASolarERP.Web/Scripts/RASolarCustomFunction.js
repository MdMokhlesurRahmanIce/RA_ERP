
function Helper() {

}

Helper.GetParameterByName = function (name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.href);
    if (results == null) return "";
    else return decodeURIComponent(results[1].replace(/\+/g, " "));
}

Helper.CustomerIdWithoutHiPen = function (customerCode) {
    customerCode = customerCode.replace(/ /g, '');
    customerCode = $.trim(customerCode.replace(/\-/g, ""));

    return customerCode;
}

Helper.WhiteSpaceRemove = function (removeFor) {
    removeFor = removeFor.replace(/ /g, '');
    return removeFor;
}

Helper.IsNumeric = function (input) {
    return (input - 0) == input && input.length > 0;
}

Helper.IsInt = function (n) {
    var er = /^[0-9]+$/;
    return (er.test(n)) ? true : false;
}

Helper.IsDecimal = function (n) {
    var er = /^\d*\.?\d*$/;
    return (er.test(n)) ? true : false;
}

Helper.IsDecimal2FloatPoint = function (n) {
    var er = /^\d*\.?\d{0,2}$/;
    return (er.test(n)) ? true : false;
}

Helper.IsLeadingZeroContains = function (n) {
    var er = /^0+/;
    return (er.test(n)) ? true : false;
}

Helper.DateCange = function (n) {

    var splitDate = n.split('-');
    splitDate[1] = MonthNameToDigit(splitDate[1]);

    return new Date(splitDate[1] + "/" + splitDate[0] + "/" + splitDate[2]);

}

Helper.StartDate = function (n) {

    var splitDate = n.split('-');
    splitDate[1] = MonthNameToDigit(splitDate[1]);
    var firstDay = 1;

    return new Date(splitDate[1] + "/" + firstDay + "/" + splitDate[2]);
}

Helper.EndDate = function (n) {

    var splitDate = n.split('-');

    var lastDay = MonthWiseLastDate(splitDate[1]);
    splitDate[1] = MonthNameToDigit(splitDate[1]);

    return new Date(splitDate[1] + "/" + lastDay + "/" + splitDate[2]);
}

Helper.DateFormat = function (myDate) {
    var d = new Date(myDate);
    return d.toDateString();
}

Helper.SpecialCharacterCheck = function (myData) {
    var er = /[!@#$%^&*()_']+/;
    // /^[@!#\$\^%&*()+=\-\[\]\\\';,\.\/\{\}\|\":<>\? ]+$/; 
    // /[$\\@\\\#%\^\&\*\(\)\[\]\+\_\{\}\`\~\=\|]/;
    return (er.test(myData)) ? true : false;
}

Helper.MonthNameToMonthDigit = function (date) {
    var splitDate = date.split('-');
    var monthDigit = MonthNameToDigit(splitDate[1]);

    return monthDigit;
}

function MonthNameToDigit(month) {

    month = month.toLowerCase();

    if (month == 'jan') {
        return 1;
    }
    else if (month == 'feb') {
        return 2;
    }
    else if (month == 'mar') {
        return 3;
    }
    else if (month == 'apr') {
        return 4;
    }
    else if (month == 'may') {
        return 5;
    }
    else if (month == 'jun') {
        return 6;
    }
    else if (month == 'jul') {
        return 7;
    }
    else if (month == 'aug') {
        return 8;
    }
    else if (month == 'sep') {
        return 9;
    }
    else if (month == 'oct') {
        return 10;
    }
    else if (month == 'nov') {
        return 11;
    }
    else if (month == 'dec') {
        return 12;
    }
    else { return 0; }
}

function MonthWiseLastDate(month) {

    month = month.toLowerCase();

    if (month == 'jan' || month == 'mar' || month == 'may' || month == 'jul' || month == 'aug') {
        return 31;
    }
    else if (month == 'feb') {
        return 28;
    }
    else if (month == 'apr' || month == 'jun' || month == 'sep' || month == 'nov' || month == 'oct' || month == 'dec') {
        return 30;
    }
    else { return 0; }
}

Helper.Menu = function (root, goFor) {

    var url = "";
    var modulePrefix = "";
    var pages = "";

    modulePrefix = goFor.substring(0, 3);

    //alert(modulePrefix);

    pages = goFor.substring(4);

    //alert(pages);

    if (modulePrefix == "SAL") {
        url = SalesMenu(pages);
    }
    else if (modulePrefix == "INV") {
        url = InventroyMenu(pages);
    }
    else if (modulePrefix == "ACC") {
        url = AccountMenu(pages);
    }
    else if (modulePrefix == "HRM") {
        url = HRMMenu(pages);
    }

    url = root + url;

    return url;
}

function SalesMenu(goFor) {

    var url = "";

    if (goFor == "SalesMonitoringEntry") {
        url = "Sales/SalesMonitoring/SalesMonitoringEntry";
    }
    else if (goFor == "SummaryForTheDayClosing") {
        url = "Sales/SalesMonitoring/SummaryForTheDayClosing";
    }
    else if (goFor == "EmployeeWiseDailyTargetEntry") {
        url = "Sales/SalesMonitoring/EmployeeWiseDailySalesTargetSetup";
    }
    else if (goFor == "EmployeeWiseMonthlyTargetView") {
        url = "Sales/SalesMonitoring/EmployeeWiseMonthlyTargetView";
    }
    else if (goFor == "DailyProgressReviewDataEntryStatus") {
        url = "Sales/SalesMonitoring/DailyProgressReviewDataEntryStatus";
    }
    else if (goFor == "LocationNEmployeeWiseSalesNCollectionEntry") {
        url = "Sales/SalesMonitoring/LocationNEmployeeWiseSalesNCollectionEntry"
    }
    else if (goFor == "SalesReSalesAgreement") {
        url = "Sales/SalesDept/SalesReSalesAgreement";
    }
    else if (goFor == "SalesSummary") {
        url = "Sales/SalesDept/SalesSummary";
    }
    else if (goFor == "SpareSales") {
        url = "Sales/SalesDept/SpareSales";
    }
    else if (goFor == "SpareSalesByItemSet") {
        url = "Sales/SalesDept/SpareSalesByItemSet";
    }
    else if (goFor == "CollectionEfficiencySummary") {
        url = "Sales/SalesReport/CollectionEfficiencySummary";
    }
    else if (goFor == "LocationNEmployeeWiseWeeklySalesNCollectionReport") {
        url = "Sales/SalesMonitoring/LocationNEmployeeWiseWeeklySalesNCollectionReport";
    }
    else if (goFor == "SalesSummaryDetailView") {
        url = "Sales/SalesReport/SalesSummaryDetailView";
    }
    else if (goFor == "OverdueCollectionTargetVsAchievementByUnitOfficeReport") {
        url = "Sales/SalesReport/OverdueCollectionTargetVsAchievementByUnitOfficeReport";
    }
    else if (goFor == "SalesAuditAdjsutment") {
        url = "Sales/SalesAudit/SalesAuditAdjsutment";
    }
    else if (goFor == "WeeklySalesNCollectionEntryStatus") {
        url = "Sales/SalesMonitoring/WeeklySalesNCollectionEntryStatus";
    }
   
    //WeeklySalesNCollectionEntryStatusForUnitUser
    
    else if (goFor == "UnitWiseCustomerRegister") {
        url = "Sales/CustomerRegister/UnitWiseCustomerRegister";
    }
    else if (goFor == "UnitWiseCustomerReceive") {
        url = "Sales/CustomerRegister/UnitWiseCustomerReceive";
    }
    else if (goFor == "UOCollectionVsHOPhysicalCashMovementReport") {
        url = "Sales/SalesDept/UOCollectionVsHOPhysicalCashMovementReport";
    }
    else if (goFor == "CustomerCollectionEfficiencyReport") {
        url = "Sales/SalesReport/CustomerCollectionEfficiencyReport";
    }
    else if (goFor == "CustomerWiseFPREntry") {
        url = "Sales/CustomerRegister/CustomerWiseFPREntry";
    }
    else if (goFor == "EmployeeRegister") {
        url = "HRMS/EmployeeRegister/EmployeeRegister";
    }
    else if (goFor == "EmployeeAcceptReject") {
        url = "HRMS/EmployeeRegister/EmployeeAcceptReject";
    }
    else if (goFor == "ProgressReviewGraph") {
        url = "Sales/SalesMonitoring/ProgressReviewGraph";
    }
    else if (goFor == "CustomerTrainingEntry") {
        url = "Sales/CustomerRegister/CustomerTrainingEntry";
    }
    else if (goFor == "SummarySheetForRegionalSalesPosting") {
        url = "Sales/SalesReport/SummarySheetForRegionalSalesPosting";
    }
    else if (goFor == "CustomersOtherView") {
        url = "Sales/SystemReturns/CustomersOtherView?cs=&cm=";
    }
    else if (goFor == "OtherAmountCollectionFromCustomer") {
        url = "Sales/CustomerRegister/OtherAmountCollectionFromCustomer";
    }
    else if (goFor == "CashMemoInformation") {
        url = "Sales/CustomerRegister/CashMemoInformation";
    }
    else if (goFor == "CustomerRegisterForAuditor") {
        url = "Sales/SalesAudit/CustomerRegisterForAuditor";
    }
    else if (goFor == "ODRecoveryStatus") {
        url = "Sales/SalesMonitoring/ODRecoveryStatus";
    }
    else if (goFor == "SalesRecoveryCommitmentReview") {
        url = "Sales/SalesMonitoring/SalesRecoveryCommitmentReview";
    }
    else if (goFor == "SalesRecoveryStatusEntry") {
        url = "Sales/SalesMonitoring/SalesRecoveryStatusEntry";
    }
    else if (goFor == "OtherSalesReSalesAgreement") {
        url = "Sales/SalesDept/OtherSalesReSalesAgreement";
    }
    else if (goFor == "SalesAgreementForSpecialPackageSales") {
        url = "Sales/SalesDept/SalesAgreementForSpecialPackageSales";
    }
    else if (goFor == "CollectionSheetForCustomerFPR") {
        url = "Sales/SalesReport/CollectionSheetForCustomerFPR";
    }
    else if (goFor == "DailyPerformanceMonitoring") {
        url = "Sales/SalesMonitoring/DailyPerformanceMonitoring";
    }
    else if (goFor == "WeeklySalesNCollectionEntryStatus") {
        url = "Sales/SalesMonitoring/WeeklySalesNCollectionEntryStatus";
    }
    else if (goFor == "WeeklySalesNCollectionEntryStatusForUnitUser") {
        url = "Sales/SalesMonitoring/WeeklySalesNCollectionEntryStatusForUnitUser";
    }
    else if (goFor == "LoadRequestEntry") {
        url = "Sales/SalesMonitoring/LoadRequestEntry";
    }
    else if (goFor == "EmployeeVisitPlan") {
        url = "Sales/SalesMonitoring/EmployeeVisitPlan";
    } 

    else if (goFor == "EmployeeWiseTrainingAssessmentEntry") {
        url = "HRMS/EmployeeDept/EmployeeWiseTrainingAssessmentEntry";
    }
    return url;
}

function InventroyMenu(goFor) {

    var url = "";

    if (goFor == "InventoryDataEntryStatusReport") {
        url = "Inventory/InventoryDept/InventoryDataEntryStatusReport";
    }
    else if (goFor == "StockPositionViewNUpdate") {
        url = "Inventory/InventoryDailyTransaction/StockPositionViewNUpdate";
    }
    else if (goFor == "StockPositionViewNUpdateAuditObservation") {
        url = "Inventory/InventoryDailyTransaction/StockPositionViewNUpdateAuditObservation"
    }
    else if (goFor == "ItemIssue") {
        url = "Inventory/InventoryDailyTransaction/ItemIssue";
    }
    else if (goFor == "ChallanWithMultipleLocation") {
        url = "Inventory/InventoryDailyTransaction/ChallanWithMultipleLocation";
    }
    else if (goFor == "ItemReceiveWithChallan") {
        url = "Inventory/InventoryDailyTransaction/ItemReceiveWithChallan";
    }
    else if (goFor == "ItemReceiveWithoutChallan") {
        url = "Inventory/InventoryDailyTransaction/ItemReceiveWithoutChallan";
    }
    else if (goFor == "InventorySummaryToDetailView") {
        url = "Inventory/InventoryReport/InventorySummaryToDetailView?ym=";
    }
    else if (goFor == "ReportViewer") {
        url = "Inventory/InventoryReport/ReportViewer";
    }
    else if (goFor == "AuditAdjustmentNewItem") {
        url = "Inventory/InventoryAudit/AuditAdjustmentNewItem";
    }
    else if (goFor == "AuditAdjustmentCustomerSupportItem") {
        url = "Inventory/InventoryAudit/AuditAdjustmentCustomerSupportItem";
    }
    else if (goFor == "AuditAdjustmentSystemReturnItem") {
        url = "Inventory/InventoryAudit/AuditAdjustmentSystemReturnItem";
    }
    else if (goFor == "InventoryLedger") {
        url = "Inventory/InventoryDailyTransaction/InventoryLedger"
    }
    else if (goFor == "InventoryInTransitReport") {
        url = "Inventory/InventoryReport/InventoryInTransitReport";
    }
    else if (goFor == "ItemSummary") {
        url = "Inventory/InventoryDept/ItemSummary";
    }
    else if (goFor == "ERPVersusPhysicalBalance") {
        url = "Inventory/InventoryDept/ERPVersusPhysicalBalance";
    }
    else if (goFor == "ItemIssueForAuditAdjustment") {
        url = "Inventory/InventoryAudit/ItemIssueForAuditAdjustment";
    }
    else if (goFor == "ItemReceiveForAuditAdjustment") {
        url = "Inventory/InventoryAudit/ItemReceiveForAuditAdjustment";
    }
    else if (goFor == "FixedAssetRegister") {
        url = "Inventory/InventoryManagement/FixedAssetRegister";
    }
    else if (goFor == "VendorChallanVMrrVerification") {
        url = "Inventory/InventoryDept/VendorChallanVMrrVerification";
    }
    else if (goFor == "SHSDistributionPlan") {
        url = "Inventory/InventoryDept/SHSDistributionPlan";
    }
    //else if (goFor == "EmployeeVisitPlan") {
    //    url = "Inventory/InventoryDept/EmployeeVisitPlan";
        //}
    else if (goFor == "SHSDistributionScheduleOrRouteTransfer") {
        url = "Inventory/InventoryDept/SHSDistributionScheduleOrRouteTransfer";
    }
    else if (goFor == "EditSHSDistributionPlan") {
        url = "Inventory/InventoryDept/EditSHSDistributionPlan";
    }
    else if (goFor == "DelevaryNoteChallanSHS") {
        url = "Inventory/InventoryReport/DelevaryNoteChallanSHS";
    }
    else if (goFor == "ChallanInformationAtGlance") {
        url = "Inventory/InventoryAudit/ChallanInformationAtGlance";
    }
    else if (goFor == "MRRInformationAtGlance") {
        url = "Inventory/InventoryAudit/MRRInformationAtGlance";
    }
    else if (goFor == "StockInTransitAtGlance") {
        url = "Inventory/InventoryAudit/StockInTransitAtGlance";
    }
    else if (goFor == "CashMemoBookAllocation") {
        url = "Inventory/InventoryManagement/CashMemoBookAllocation";
    }
    else if (goFor == "WarrantyClaimNSettlement") {
        url = "Inventory/InventoryManagement/WarrantyClaimNSettlement";
    }
    else if (goFor == "ItemSerialCorrection") {
        url = "Inventory/InventoryAudit/ItemSerialCorrection";
    }
    return url;
}

function AccountMenu(goFor) {

    var url = "";
    
    if (goFor == "AccountingSummary") {
        url = "Financial/AccountingReport/AccountingSummary";
    }
    else if (goFor == "MonthlyReceiptPaymentSummery") {
        url = "Financial/AccountingReport/MonthlyReceiptPaymentSummery";
    }
    else if (goFor == "TrialBalance") {
        url = "Financial/AccountingReport/TrialBalance";
    }
    else if (goFor == "GeneralLedger") {
        url = "Financial/AccountingReport/GeneralLedger";
    }
    else if (goFor == "ReportViewerAccountFinance") {
        url = "Financial/ReportAccountFinance/ReportViewerAccountFinance";
    }
    else if (goFor == "ReportIncomeStatement") {
        url = "Financial/ReportAccountFinance/ReportIncomeStatement";
    }
    else if (goFor == "CustomerTrainingSummaryReport") {
        url = "Financial/AccountingReport/CustomerTrainingSummaryReport";
    }
    else if (goFor == "CustomerTrainingDetailsReport") {
        url = "Financial/AccountingReport/CustomerTrainingDetailsReport";
    }
    else if (goFor == "MonthlyAccountSummaryStatement") {
        url = "Financial/AccountingReport/MonthlyAccountSummaryStatement";
    }
    else if (goFor == "MonthlyCollectionPaymentSummaryStatement") {
        url = "Financial/AccountingReport/MonthlyCollectionPaymentSummaryStatement";
    }
    else if (goFor == "CustomerTrainingModule") {
        url = "Financial/AccountDept/CustomerTrainingModule";
    }
    else if (goFor == "AccountModule") {
        url = "Financial/AccountDept/AccountModule";
    }
    else if (goFor == "AccountingModuleForZO") {
        url = "Financial/AccountDept/AccountingModuleForZO";
    }
    else if (goFor == "AccountingModuleForHeadOffice") {
        url = "Financial/AccountDept/AccountingModuleForHeadOffice";
    }
    else if (goFor == "IncomeStatement") {
        url = "Financial/AccountingReport/IncomeStatement";
    }
    else if (goFor == "ContraVoucherForAuditor") {
        url = "Financial/AccountVoucher/ContraVoucherForAuditor";
    }
    else if (goFor == "PaymentVoucherForAuditor") {
        url = "Financial/AccountVoucher/PaymentVoucherForAuditor";
    }
    else if (goFor == "ReceiveVoucherForAuditor") {
        url = "Financial/AccountVoucher/ReceiveVoucherForAuditor";
    }
    else if (goFor == "NonCashVoucherForAuditor") {
        url = "Financial/AccountVoucher/NonCashVoucherForAuditor";
    }
    else if (goFor == "EmployeeWiseSalaryPayment") {
        url = "Financial/AccountDept/EmployeeWiseSalaryPayment";
    }
    else if (goFor == "MonthlyReceiptPaymentSummeryForHoafadmin") {
        url = "Financial/AccountingReport/MonthlyReceiptPaymentSummeryForHoafadmin";
    }
    else if (goFor == "PaymentVoucher") {
        url = "Financial/AccountVoucher/PaymentVoucher";
    }
    else if (goFor == "ReceiveVoucher") {
        url = "Financial/AccountVoucher/ReceiveVoucher";
    }
    else if (goFor == "ContraVoucher") {
        url = "Financial/AccountVoucher/ContraVoucher";
    }
    else if (goFor == "NonCashVoucher") {
        url = "Financial/AccountVoucher/NonCashVoucher";
    }
    return url;
}

function HRMMenu(goFor) {
    var url = "";

    if (goFor == "EmployeeSearch") {
        url = "HRMS/EmployeeDept/EmployeeSearch";
    }
    else if (goFor == "EmployeeTransfer") {
        url = "HRMS/EmployeeRegister/EmployeeTransfer";
    }
    else if (goFor == "SalaryAdviceForBank") {
        url = "HRMS/HrmReport/SalaryAdviceForBank";
    }
    else if (goFor == "Auditor") {
        url = "Auditor/AuditSetup";
    }
    else if (goFor == "EmployeeDetails") {
        url = "HRMS/Employee/EmployeeDetails";
    }
    else if (goFor == "AddEditEmployee") {
        url = "HRMS/Employee/AddEditEmployee";
    }
    else if (goFor == "EmployeeCV") {
        url = "HRMS/Employee/EmployeeCV";
    }
    else if (goFor == "DailyTADAEntry") {
        url = "HRMS/EmployeeDept/DailyTADAEntry";
    }
    else if (goFor == "TADAReviewNApproval") {
        url = "HRMS/EmployeeDept/TADAReviewNApproval";
    }

    return url;
}


Helper.AlertMessage = function (messageTitle, message) {

    $("#AlertMessageBox").html(message);
    $("#AlertMessageBox").dialog({
        width: "auto",
        height:"auto",       
        buttons: [{
            text: "Ok",
            click: function () { $(this).dialog("close"); }
        }],
        modal: true,
        closeOnEscape: true,
        resizable: false,
        title: messageTitle,
        show: "highlight"
    });
}
