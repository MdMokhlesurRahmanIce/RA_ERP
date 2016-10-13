using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;

namespace RASolarHelper
{
    public static class ExceptionHelper
    {
        public static ArrayList ExceptionMessage(Exception ex)
        {
            ArrayList messageArray = new ArrayList();
            string validationMessage = string.Empty;

            if ((ex) is DbEntityValidationException)
            {
                validationMessage = EntityValidationErrorProcess(ex);
            }

            if ((ex) is DbUpdateException)
            {
                validationMessage = EntityDBUpdateExceptionProcess(ex);
            }

            if (!string.IsNullOrEmpty(validationMessage))
            {
                if (ex.InnerException == null)
                    messageArray.Add(new { messageCode = "error", message = "Save Is Not Succeed. Please Try Again." + "\n\nError:  " + ex.Message + "\n\nError Details: " + validationMessage + "\n\nTarget: " + ex.TargetSite.ToString() });
                else
                    messageArray.Add(new { messageCode = "error", message = "Save Is Not Succeed. Please Try Again." + "\n\nError:  " + ex.InnerException.Message + "\n\nError Details: " + validationMessage + "\n\nTarget: " + ex.TargetSite.ToString() });
            }
            else
            {
                if (ex.InnerException == null)
                    messageArray.Add(new { messageCode = "error", message = "Save Is Not Succeed. Please Try Again." + "\n\nError:  " + ex.Message + "\n\nTarget: " + ex.TargetSite.ToString() });
                else if (ex.Message.Substring(0,8) == "Employee")
                    messageArray.Add(new { messageCode = "error", message = ex.Message + " Please Try Again." + "\n\nError:  " + ex.InnerException.Message + "\n\nTarget: " + ex.TargetSite.ToString() });
                else
                    messageArray.Add(new { messageCode = "error", message = "Save Is Not Succeed. Please Try Again." + "\n\nError:  " + ex.InnerException.Message + "\n\nTarget: " + ex.TargetSite.ToString() });
            }

            return messageArray;
        }

        public static ArrayList ExceptionMessage(Exception ex, string errorMessage)
        {
            ArrayList messageArray = new ArrayList();

            string validationMessage = string.Empty;

            if ((ex) is DbEntityValidationException)
            {
                validationMessage = EntityValidationErrorProcess(ex);
            }

            if (!string.IsNullOrEmpty(validationMessage))
            {
                if (ex.InnerException == null)
                    messageArray.Add(new { messageCode = "error", message = errorMessage + "\n\nError:  " + ex.Message + "\n\nError Details: " + validationMessage + "\n\nTarget: " + ex.TargetSite.ToString() });
                else
                    messageArray.Add(new { messageCode = "error", message = errorMessage + "\n\nError:  " + ex.InnerException.Message + "\n\nError Details: " + validationMessage + "\n\nTarget: " + ex.TargetSite.ToString() });
            }
            else
            {
                if (ex.InnerException == null)
                    messageArray.Add(new { messageCode = "error", message = errorMessage + "\n\nError:  " + ex.Message + "\n\nTarget: " + ex.TargetSite.ToString() });
                else
                    messageArray.Add(new { messageCode = "error", message = errorMessage + "\n\nError:  " + ex.InnerException.Message + "\n\nTarget: " + ex.TargetSite.ToString() });
            }

            return messageArray;
        }

        public static string ExceptionMessageOnly(Exception ex)
        {
            if (ex.InnerException == null)
                return ex.Message;
            else
                return ex.InnerException.Message;
        }

        private static string EntityValidationErrorProcess(Exception ex)
        {
            StringBuilder sb = new StringBuilder();

            DbEntityValidationException dbException = (DbEntityValidationException)ex;

            foreach (var failure in dbException.EntityValidationErrors)
            {
                sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                foreach (var error in failure.ValidationErrors)
                {
                    sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }

        private static string EntityDBUpdateExceptionProcess(Exception ex)
        {
            StringBuilder sb = new StringBuilder();

            DbUpdateException dbException = (DbUpdateException)ex;

            if (dbException.InnerException.InnerException != null)
                sb.AppendFormat("{0} Inner Exception:\n", dbException.InnerException.InnerException.Message);

            return sb.ToString();
        }


        /// <summary>
        /// If assign the parameter value empty, then it returns only " Add Is Succeed".
        /// If parameter assign some value then it returns value + "Is Add Succesfully".
        /// 
        /// Ex.- ex=string.empty then "Add Is Succeed"
        ///      ex= "Agreement" then "Agreement Is Add Succesfully"
        /// </summary>
        /// <param name="ex">string</param>
        /// <returns>A arraylist with Message Code and the Message</returns>
        public static ArrayList ExceptionMessageAdd(string ex)
        {
            ArrayList messageArray = new ArrayList();

            if (string.IsNullOrEmpty(ex))
                messageArray.Add(new { messageCode = "successAdd", message = "Save Is Succeed" });
            else
                messageArray.Add(new { messageCode = "successAdd", message = ex });

            return messageArray;
        }


        /// <summary>
        /// If assign the parameter value empty, then it returns only "Save Is Succeed".
        /// If parameter assign some value then it returns value + "Is Save Succesfully".
        /// 
        /// Ex.- ex=string.empty then "Save Is Succeed"
        ///      ex= "Agreement" then "Agreement Is Save Succesfully"
        /// </summary>
        /// <param name="ex">string</param>
        /// <returns>A arraylist with Message Code and the Message</returns>
        public static ArrayList ExceptionMessage(string ex)
        {
            ArrayList messageArray = new ArrayList();

            if (string.IsNullOrEmpty(ex))
                messageArray.Add(new { messageCode = "success", message = "Save Is Succeed" });
            else
                messageArray.Add(new { messageCode = "success", message = ex });

            return messageArray;
        }


        public static ArrayList ExceptionMessage(string ex, string messageCode, string id)
        {
            ArrayList messageArray = new ArrayList();

            if (string.IsNullOrEmpty(ex))
                messageArray.Add(new { messageCode = messageCode, id = id, message = "Save Is Succeed" });
            else
                messageArray.Add(new { messageCode = messageCode, id = id, message = ex });

            return messageArray;
        }

        public static ArrayList ExceptionCustomErrorMessage(string ex)
        {
            ArrayList messageArray = new ArrayList();

            if (string.IsNullOrEmpty(ex))
                messageArray.Add(new { messageCode = "error", message = "Save Is Not Succeed" });
            else
                messageArray.Add(new { messageCode = "error", message = ex });

            return messageArray;
        }

        public static ArrayList ExceptionMessage(string ex, string id)
        {
            ArrayList messageArray = new ArrayList();

            if (string.IsNullOrEmpty(ex))
                messageArray.Add(new { messageCode = "success", message = " Save Is Succeed", id = id });
            else
                messageArray.Add(new { messageCode = "success", message = ex, id = id });

            return messageArray;
        }
    }
}
