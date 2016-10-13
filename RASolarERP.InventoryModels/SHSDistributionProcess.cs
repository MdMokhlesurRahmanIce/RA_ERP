using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RASolarERP.DomainModel.InventoryModel;
using RASolarERP.Model;
using RASolarHelper;

namespace RASolarERP.Web.Areas.Inventory.Models
{
   public class SHSDistributionProcess
    {


       public List<SHSDistributionPlan_RootWiseLocation> SHSDistributionRootWiseLocation(List<SHSDistributionPlan> lstUnitWisePackageAmount, string scheduleDate)
       {
           List<SHSDistributionPlan_RootWiseLocation> lstSHSDistributionPlan_RootWiseLocation = new List<SHSDistributionPlan_RootWiseLocation>();
           SHSDistributionPlan_RootWiseLocation objSHSDistributionPlan_RootWiseLocation;

           foreach (SHSDistributionPlan shsDistributionPlan in lstUnitWisePackageAmount)
           {

               var lstSHSDistributionPlan_RootWiseLocationDuplicateCheck = from D in lstSHSDistributionPlan_RootWiseLocation
                                        where D.RouteNo == shsDistributionPlan.RootNo && D.LocationCode==shsDistributionPlan.LocationCode
                                       select D;
               if (lstSHSDistributionPlan_RootWiseLocationDuplicateCheck.Count() == 0)
               {


                   objSHSDistributionPlan_RootWiseLocation = new SHSDistributionPlan_RootWiseLocation();
                   objSHSDistributionPlan_RootWiseLocation.DistribScheduleNo = Helper.DateFormatYYMMDD(Convert.ToDateTime(scheduleDate.ToString()));
                   objSHSDistributionPlan_RootWiseLocation.RouteNo = shsDistributionPlan.RootNo;
                   objSHSDistributionPlan_RootWiseLocation.LocationCode = shsDistributionPlan.LocationCode;

                   lstSHSDistributionPlan_RootWiseLocation.Add(objSHSDistributionPlan_RootWiseLocation);
               }
           }

           return lstSHSDistributionPlan_RootWiseLocation;
       }

       public List<SHSDistributionPlan_RootWiseLocationNPackage> UnitWisePackageAmountSHSDistributionProcess(List<SHSDistributionPlan> lstUnitWisePackageAmount, string scheduleDate)
       {
           List<SHSDistributionPlan_RootWiseLocationNPackage> lstSHSDistributionPlan_RootWiseLocationNPackage = new List<SHSDistributionPlan_RootWiseLocationNPackage>();
           SHSDistributionPlan_RootWiseLocationNPackage objSHSDistributionPlan_RootWiseLocationNPackage;
           Int32 pkgSequenceNo = 1;
           foreach (SHSDistributionPlan shsPackageUnitAmount in lstUnitWisePackageAmount)
           {
               if (shsPackageUnitAmount.PackageOrItemSelection == "Package")
               {

                   //var lstSHSDistributionPlan_RootWiseLocationCheckPackage = from D in lstSHSDistributionPlan_RootWiseLocationNPackage
                   //                                                          where D.PackageCode == shsPackageUnitAmount.PackageCode 
                   //                                                            select D;
                   //if (lstSHSDistributionPlan_RootWiseLocationCheckPackage.Count() == 0)
                     

                   objSHSDistributionPlan_RootWiseLocationNPackage = new SHSDistributionPlan_RootWiseLocationNPackage();

                   objSHSDistributionPlan_RootWiseLocationNPackage.DistribScheduleNo = Helper.DateFormatYYMMDD(Convert.ToDateTime(scheduleDate.ToString()));
                   objSHSDistributionPlan_RootWiseLocationNPackage.RouteNo = shsPackageUnitAmount.RootNo;
                   objSHSDistributionPlan_RootWiseLocationNPackage.LocationCode = shsPackageUnitAmount.LocationCode;
                   objSHSDistributionPlan_RootWiseLocationNPackage.PackageCode = shsPackageUnitAmount.PackageCode;
                   objSHSDistributionPlan_RootWiseLocationNPackage.PkgSequenceNo = pkgSequenceNo++;
                   objSHSDistributionPlan_RootWiseLocationNPackage.PanelModel = shsPackageUnitAmount.PanelCode;
                   objSHSDistributionPlan_RootWiseLocationNPackage.BatteryModel = shsPackageUnitAmount.BatteryCode;
                   objSHSDistributionPlan_RootWiseLocationNPackage.PackageQuantity = shsPackageUnitAmount.PackageQuantity;

                   lstSHSDistributionPlan_RootWiseLocationNPackage.Add(objSHSDistributionPlan_RootWiseLocationNPackage);


               }
           }

           return lstSHSDistributionPlan_RootWiseLocationNPackage;
       }

       public List<SHSDistributionPlan_IndividualItem> UnitWiseItemAmountSHSDistributionProcess(List<SHSDistributionPlan> lstUnitWisePackageAmount, string scheduleDate)
       {
           List<SHSDistributionPlan_IndividualItem> lstSHSDistributionPlan_IndividualItem = new List<SHSDistributionPlan_IndividualItem>();
           SHSDistributionPlan_IndividualItem objSHSDistributionPlan_IndividualItem;
           foreach (SHSDistributionPlan shsItemUnitAmount in lstUnitWisePackageAmount)
           {
               if (shsItemUnitAmount.PackageOrItemSelection == "Item")
               {

                   objSHSDistributionPlan_IndividualItem = new SHSDistributionPlan_IndividualItem();

                   objSHSDistributionPlan_IndividualItem.DistribScheduleNo = Helper.DateFormatYYMMDD(Convert.ToDateTime(scheduleDate.ToString()));
                   objSHSDistributionPlan_IndividualItem.RouteNo = shsItemUnitAmount.RootNo;
                   objSHSDistributionPlan_IndividualItem.LocationCode = shsItemUnitAmount.LocationCode;
                   objSHSDistributionPlan_IndividualItem.ItemCode = shsItemUnitAmount.PackageCode;
                   objSHSDistributionPlan_IndividualItem.ItemQuantity = shsItemUnitAmount.PackageQuantity;

                   lstSHSDistributionPlan_IndividualItem.Add(objSHSDistributionPlan_IndividualItem);


               }
           }

           return lstSHSDistributionPlan_IndividualItem;
       }
    }
}
