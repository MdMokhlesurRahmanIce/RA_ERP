using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarHelper
{
    public class UserGroup
    {
        //ADMINTRTOR	Administrator	0	2012-04-17 15:29:15.077
        //HOAUDITDPT	Head Office Audit	0	2012-05-31 16:19:10.747
        //HOFINANACC	Head Office Finance & Accounts	0	2012-04-17 15:30:19.827
        //HOINVENTRY	Head Office Inventory	0	2012-05-14 19:35:46.043
        //HOITDEPART	Head Office IT	0	2012-04-17 15:31:08.187
        //HOSALESDPT	Head Office Sales	0	2012-05-15 11:57:43.450
        //REGIONMNGR	Regional Manager	0	2012-04-15 20:24:29.050
        //UNITAUDITR	Unit Office Audit	0	2012-05-16 13:25:22.763
        //UNITMANAGR	Unit Manager	0	2012-04-15 20:21:59.553
        //ZONALITUSR	Zonal IT User	0	2012-04-15 20:26:48.453
        //ZONALMANGR	Zonal Manager	0	2012-04-15 20:25:22.910

        public const string Administrator = "ADMINTRTOR";	            //Administrator	
        public const string HeadOfficeAudit = "HOAUDITDPT";	            //Head Office Audit	
        public const string HeadOfficeFinanceAccount = "HOFINANACC";	//Head Office Finance & Accounts
        public const string HeadOfficeInventory = "HOINVENTRY"; 	    //Head Office Inventory	
        public const string HeadOfficeIT = "HOITDEPART";	            //Head Office IT	
        public const string HeadOfficeSales = "HOSALESDPT";	            //Head Office Sales
        public const string RegionManager = "REGIONMNGR";                         //Regional Manager	
        public const string UnitAuditor = "UNITAUDITR";	                         //Unit Office Audit
        public const string UnitManager = "UNITMANAGR";	                         //Unit Manager
        public const string ZonalITUser = "ZONALITUSR";	                         //Zonal IT User
        public const string ZonalManager = "ZONALMANGR";	                       //Zonal Manager
        public const string RacoReviewer = "RACOREVWER";	                        //Raco Reviewer
        public const string RREReviewer = "RRERVIEWER";                             //RRE Reviewer 
        public const string ITReviewer = "ITREVIEWER";                             //IT Reviewer 
        public const string UnitOfficeUser = "UNITOFUSER";                          //Unit Office User
        public const string ZonalOperationUser = "ZONALOPSUR";                      //Zonal Opearation User
        public const string RSFExecutiveCommittee = "RSFECMVIEW";                   //RSF Executive Committee Member's View
        public const string HeadOfficeHRNAdmin = "HOHRADMDPT";                      //Head Office HR & Admin
        public const string HeadOfficeAccountAdvanceUsers = "ACCADVUSER";           // Advance Users (For ACC) Home 
        public const string HeadOfficeAccountEntryUsers = "ACCENTUSER";             // Entry Users (For ACC) Home
        public const string InventroyAccountEntryUsers = "INVENTUSER";           // Entry Users (For INV) Home
        public const string InventroyAdvanceUsers = "INVADVUSER";             // Advance Users (For INV) Home
    }
}
