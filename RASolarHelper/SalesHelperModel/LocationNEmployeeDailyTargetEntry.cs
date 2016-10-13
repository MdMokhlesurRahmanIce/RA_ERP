using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarHelper.SalesHelperModel
{
   public  class LocationNEmployeeDailyTargetEntry
    {   

           private string _locationCode;
           private string _locationSalesCurrentMonthTarget;
           private decimal _locationSalesAchievementUptoLastDay;
           private decimal _locationSalesAchievementToday;
           private string _locationSalesAchievementStatus;
           private decimal _locationRegColcAchievementUptoLastDay;
           private string _locationRegColcAchievementStatus;
           private byte _locationOverdueColcCurrentMonthTarget;
           private decimal _locationOverdueColcAchievementToday;
           private decimal _lcationOverdueColcAchievementUptoLastDay;
           private string _locationOverdueColcAchievementStatus;
           private decimal _locationSystemReturn;
           private decimal _locationSystemReSale;
           private decimal _locationCustomerTraining;
           private decimal _locationBioGasSales;
           private decimal _locationRSFBatterySales;
           private string _EMP_ID;
           private string _EMP_SalesCurrentMonthTarget;
           private string _EMP_SalesAchievementUptoLastDay;
           private string _EMP_SalesAchievementToday;
           private decimal _EMP_SalesAchievementStatus;
           private decimal _EMP_RegColcCurrentMonthTarget;
           private decimal _EMP_RegColcAchievementUptoLastDay;
           private decimal _EMP_RegColcAchievementToday;
           private decimal _EMP_RegColcAchievementStatus;
           private decimal _EMP_OverdueColcCurrentMonthTarget;
           private decimal _EMP_OverdueColcAchievementUptoLastDay;
           private decimal _EMP_OverdueColcAchievementToday;
           private decimal _EMP_OverdueColcAchievementStatus;
           private string _EMP_TA;
           private string _EMP_ReasonForTA;
           private string _EMP_SystemInstallation;
           private decimal _EMP_InstallmentCollection;
           private string _EMP_CustomerVisitForMarketing;
           private string _EMP_MaintenanceServicing;
           private string _EMP_BankOperation;
           private string _EMP_OfficialVisitToOtherUnit;
           private string _EMP_Meeting;
           private string _EMP_WasInLeave;
           private string _EMP_DeskWorks;
           private string _EMP_Remarks;

           public string LocationCode 
           {
               set { _locationCode = value; }
               get { return _locationCode; }
           }
           public string LocationSalesCurrentMonthTarget 
           {
               set { _locationSalesCurrentMonthTarget = value; }
               get { return _locationSalesCurrentMonthTarget; }
           }
           public decimal LocationSalesAchievementUptoLastDay 
           {
               set { _locationSalesAchievementUptoLastDay = value; }
               get { return _locationSalesAchievementUptoLastDay; }
           }
           public decimal LocationSalesAchievementToday 
           {
               set { _locationSalesAchievementToday = value; }
               get { return _locationSalesAchievementToday; }
           }
           public string LocationSalesAchievementStatus 
           {
               set { _locationSalesAchievementStatus = value; }
               get { return _locationSalesAchievementStatus; }
           }
           public decimal LocationRegColcAchievementUptoLastDay 
           {
               set { _locationRegColcAchievementUptoLastDay = value; }
               get { return _locationRegColcAchievementUptoLastDay; }
           }
           public string LocationRegColcAchievementStatus 
           {
               set { _locationRegColcAchievementStatus = value; }
               get { return _locationRegColcAchievementStatus; }
           }
           public byte LocationOverdueColcCurrentMonthTarget 
           {
               set { _locationOverdueColcCurrentMonthTarget = value; }
               get { return _locationOverdueColcCurrentMonthTarget; }
           }
           public decimal LocationOverdueColcAchievementToday 
           {
               set { _locationOverdueColcAchievementToday = value; }
               get { return _locationOverdueColcAchievementToday; }
           }
           public decimal LcationOverdueColcAchievementUptoLastDay 
           {
               set { _lcationOverdueColcAchievementUptoLastDay = value; }
               get { return _lcationOverdueColcAchievementUptoLastDay; }
           }
           public string LocationOverdueColcAchievementStatus 
           {
               set { _locationOverdueColcAchievementStatus = value; }
               get { return _locationOverdueColcAchievementStatus; }
           }
           public decimal LocationSystemReturn 
           {
               set { _locationSystemReturn = value; }
               get { return _locationSystemReturn; }
           }
           public decimal LocationSystemReSale 
           {
               set { _locationSystemReSale=value;}
               get { return _locationSystemReSale; }
           }
           public decimal LocationCustomerTraining 
           {
               set { _locationCustomerTraining = value; }
               get { return _locationCustomerTraining; }
           }
           public decimal LocationBioGasSales 
           {
               set { _locationBioGasSales = value; }
               get { return _locationBioGasSales; }
           }
           public decimal LocationRSFBatterySales 
           {
               set { _locationRSFBatterySales = value; }
               get {return _locationRSFBatterySales; }
           }
           public string EMP_ID 
           {
               set { _EMP_ID = value; }
               get { return EMP_ID; }
           }
           public string EMP_SalesCurrentMonthTarget 
           {
               set { _EMP_SalesCurrentMonthTarget = value; }
               get { return EMP_SalesCurrentMonthTarget; }
           }
           public string EMP_SalesAchievementUptoLastDay 
           {
               set { _EMP_SalesAchievementUptoLastDay = value; }
               get { return _EMP_SalesAchievementUptoLastDay; }
           }
           public string EMP_SalesAchievementToday 
           {
               set { _EMP_SalesAchievementToday = value; }
               get { return _EMP_SalesAchievementToday; }
           }
           public decimal EMP_SalesAchievementStatus 
           {
               set { _EMP_SalesAchievementStatus = value; }
               get { return _EMP_SalesAchievementStatus; }
           }
           public decimal EMP_RegColcCurrentMonthTarget 
           {
               set { _EMP_RegColcCurrentMonthTarget = value; }
               get { return _EMP_RegColcCurrentMonthTarget; }
           }
           
           public decimal EMP_RegColcAchievementUptoLastDay 
           {
               set { EMP_RegColcAchievementUptoLastDay = value; }
               get {return EMP_RegColcAchievementUptoLastDay; }
           }
           public decimal EMP_RegColcAchievementToday 
           {
               set { _EMP_RegColcAchievementToday = value; }
               get { return _EMP_RegColcAchievementToday; }
           }
           public decimal EMP_RegColcAchievementStatus 
           {
               set { _EMP_RegColcAchievementStatus = value; }
               get { return _EMP_RegColcAchievementStatus; }
           }
           public decimal EMP_OverdueColcCurrentMonthTarget 
           {
               set { _EMP_OverdueColcCurrentMonthTarget = value; }
               get { return _EMP_OverdueColcCurrentMonthTarget;}
           }
           public decimal EMP_OverdueColcAchievementUptoLastDay 
           {
               set { _EMP_OverdueColcAchievementUptoLastDay = value; }
               get { return _EMP_OverdueColcAchievementUptoLastDay; }
           }
           public decimal EMP_OverdueColcAchievementToday 
           {
               set { _EMP_OverdueColcAchievementToday = value; }
               get { return _EMP_OverdueColcAchievementToday; }
           }
           public decimal EMP_OverdueColcAchievementStatus 
           {
               set { _EMP_OverdueColcAchievementStatus = value; }
               get {return  _EMP_OverdueColcAchievementStatus; }
           }
           public string EMP_TA 
           {
               set { _EMP_TA = value; }
               get { return EMP_TA; }
           }
           public string EMP_ReasonForTA 
           {
               set { _EMP_ReasonForTA = value; }
               get { return EMP_ReasonForTA; }
           }
           public string EMP_SystemInstallation 
           {
               set { _EMP_SystemInstallation = value; }
               get { return _EMP_SystemInstallation; }
           }
           public decimal EMP_InstallmentCollection 
           {
               set { _EMP_InstallmentCollection = value; }
               get { return _EMP_InstallmentCollection; }
           }
           public string EMP_CustomerVisitForMarketing 
           {
               set { _EMP_CustomerVisitForMarketing = value; }
               get { return _EMP_CustomerVisitForMarketing; }
           }
           public string EMP_MaintenanceServicing 
           {
               set { _EMP_MaintenanceServicing = value; }
               get { return _EMP_MaintenanceServicing; }
           }
           public string EMP_BankOperation 
           {
               set { _EMP_BankOperation = value; }
               get { return EMP_BankOperation; }
           }
           public string EMP_OfficialVisitToOtherUnit 
           {
               set { _EMP_OfficialVisitToOtherUnit = value; }
               get { return _EMP_OfficialVisitToOtherUnit; }
           }
           public string EMP_Meeting 
           {
               set { _EMP_Meeting = value; }
               get { return EMP_Meeting; }
           }
           public string EMP_WasInLeave 
           {
               set { _EMP_WasInLeave = value; }
               get { return _EMP_WasInLeave; }
           }
           public string EMP_DeskWorks 
           {
               set { _EMP_DeskWorks = value; }
               get { return EMP_DeskWorks; }
           }
           public string EMP_Remarks 
           {
               set { _EMP_Remarks = value; }
               get { return EMP_Remarks; }
           }


    }
}
