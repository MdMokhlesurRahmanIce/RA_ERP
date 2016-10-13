using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web;

namespace RASolarHelper
{
   public class IPNetworking
    {
       public static string GetIP4Address()
       {
           string IP4Address = String.Empty;

           foreach (IPAddress IPA in Dns.GetHostAddresses(HttpContext.Current.Request.UserHostAddress))
           {
               if (IPA.AddressFamily.ToString() == "InterNetwork")
               {
                   IP4Address = IPA.ToString();
                   break;


               }
           }

           if (IP4Address != String.Empty)
           {
               return IP4Address;
           }

           foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
           {
               if (IPA.AddressFamily.ToString() == "InterNetwork")
               {
                   IP4Address = IPA.ToString();
                   break;
               }
           }

           return IP4Address;
       }
   }
}
