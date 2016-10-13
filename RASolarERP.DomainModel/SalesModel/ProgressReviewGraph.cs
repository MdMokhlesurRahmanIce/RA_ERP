using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarERP.DomainModel.SalesModel
{
   public class ProgressReviewGraph
    {
       private string _RepName;//SMSBased
       private string _DateString;//DailyEntry
       private decimal _TotalSales;
       private decimal _RepSales;//ERPPosting

       public string RepName 
       { 
           get { return _RepName; } 
           set { _RepName=value;} 
       }
       public string DateString 
       {
           get { return _DateString; }
           set { _DateString=value;}
       }
       public decimal TotalSales 
       { 
           get { return _TotalSales;} 
           set { _TotalSales=value;} 
       }
       public decimal RepSales 
       { 
           get{return _RepSales;} 
           set{_RepSales=value;} 
       }
    }
}
