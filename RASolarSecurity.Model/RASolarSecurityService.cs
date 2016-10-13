using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RASolarHelper;

namespace RASolarSecurity.Model
{
    public class RASolarSecurityService
    {
        private IRASolarSecurityReposritory securityReposritory;

        public RASolarSecurityService()
        {
            this.securityReposritory = new RASolarSecurityReposritory(new RASolarERP_SecurityAdminEntities());
        }

        #region Read Methods

        public UserInformation GetUserInformation(string userId, string userPassword, byte userStatus)
        {
            return securityReposritory.GetUserInformation(userId, userPassword, userStatus);
        }

        public ListOfPagesWithAccessRightsForThisUser ReadListOfPagesWithAccessRightsForAnUser(string userID, string pageOption, string pageID)
        {
            return securityReposritory.ReadListOfPagesWithAccessRightsForAnUser(userID, pageOption, pageID);
        }

        public List<ListOfPagesWithAccessRightsForThisUser> ReadListOfPagesWithAccessRightsForUser(string userID, string pageOption, string pageID)
        {
            return securityReposritory.ReadListOfPagesWithAccessRightsForUser(userID, pageOption, pageID);
        }

        public PageAccessRightHelper ReadPageAccessRight(string moduleId, string pageNameToLink, string roleOrGroupID, byte accessRightStatus)
        {
            return securityReposritory.ReadPageAccessRight(moduleId, pageNameToLink, roleOrGroupID, accessRightStatus);
        }

        #endregion

        #region Dispose Repository

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    securityReposritory.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
