using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarERP.DomainModel.AMSModel
{
   public class TrialBalanceReport
    {
       private string _AccNo;
       private string _AccName;
       private byte _AccLevel;
       private int _Leaf;
       private string _ParentNo;
       private string _SortCol;
       private decimal _OpeningB;
       private decimal _Debit;
       private decimal _Credit;
       private decimal _ClosingB;
       private string _ParCapSub;

       public string AccNo
       {
           get { return _AccNo; }
           set { _AccNo = value; }
       }

       public string AccName 
       {
           get 
           { 
               return _AccName; 
           }  
           set 
           {
               _AccName = value; 
           }
       }
       public byte AccLevel 
       {   
           get 
           { 
               return _AccLevel; 
           } 
           set 
           { 
               _AccLevel = value; 
           }
       }
       public int Leaf 
       { 
           get 
           {
               return _Leaf; 
           } 
           set 
           {
               _Leaf = value; 
           }
       }
       public string ParentNo { 
           get 
           { 
               return _ParentNo; 
           } 
           set 
           { 
               _ParentNo = value;
           } 
       }
       public string SortCol 
       { 
           get 
           {
               return _SortCol; 
           } 
           set 
           { 
               _SortCol = value;
           } 
       }
       public decimal OpeningB 
       { 
           get 
           { 
               return _OpeningB; 
           } 
           set 
           {
               _OpeningB = value;
           } 
       }
       public decimal Debit 
       { 
           get 
           { 
               return _Debit; 
           }
           set 
           { 
               _Debit = value; 
           }
       }
       public decimal Credit 
       {
           get 
           {
               return _Credit; 
           } 
           set 
           {
               _Credit = value;
           }
       }
       public decimal ClosingB 
       {
           get 
           {
               return _ClosingB; 
           } 
           set 
           { 
               _ClosingB = value; 
           } 
       }
       public string ParCapSub 
       {
           get 
           { 
               return _ParCapSub; 
           } 
           set 
           {
               _ParCapSub = value; 
           } 
       }
    }
}
