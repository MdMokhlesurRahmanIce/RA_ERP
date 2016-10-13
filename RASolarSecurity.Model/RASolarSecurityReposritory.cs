using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RASolarHelper;

namespace RASolarSecurity.Model
{
    public class RASolarSecurityReposritory : IRASolarSecurityReposritory
    {
        #region Properties And Constructor

        private RASolarERP_SecurityAdminEntities contextSecurity { get; set; }

        public RASolarSecurityReposritory()
        {
            this.contextSecurity = new RASolarERP_SecurityAdminEntities();
        }

        public RASolarSecurityReposritory(RASolarERP_SecurityAdminEntities securityAdminEntities)
        {
            this.contextSecurity = securityAdminEntities;
        }

        #endregion

        #region Read Methods

        public UserInformation GetUserInformation(string userId, string userPassword, byte userStatus)
        {
            UserInformation user = contextSecurity.UserInformation.Where(u => u.UserID.ToLower() == userId.ToLower() && u.UserPassword.ToLower() == userPassword.ToLower() && u.Status == userStatus).FirstOrDefault();
            return user;
        }

        public ListOfPagesWithAccessRightsForThisUser ReadListOfPagesWithAccessRightsForAnUser(string userID, string pageOption, string pageID)
        {
            return contextSecurity.ListOfPagesWithAccessRightsForThisUser(userID, pageOption, pageID).FirstOrDefault();
        }

        public List<ListOfPagesWithAccessRightsForThisUser> ReadListOfPagesWithAccessRightsForUser(string userID, string pageOption, string pageID)
        {
            return contextSecurity.ListOfPagesWithAccessRightsForThisUser(userID, pageOption, pageID).ToList();
        }

        public PageAccessRightHelper ReadPageAccessRight(string moduleId, string pageNameToLink, string roleOrGroupID, byte accessRightStatus)
        {
            var pageAccessright = from pages in contextSecurity.ListOfRASolarERPPages
                                  join permission in contextSecurity.UserRoleOrGroupWisePermission
                                  on new { pages.ModuleID, pages.PageID } equals new { permission.ModuleID, permission.PageID }
                                  into pagePermissionLeftJoin
                                  from pagePermission in pagePermissionLeftJoin.DefaultIfEmpty()
                                  where pages.ModuleID == moduleId && pages.PageNameToLink == pageNameToLink && pagePermission.RoleOrGroupID == roleOrGroupID && pages.Status == accessRightStatus
                                  select new PageAccessRightHelper
                                  {
                                      RoleOrGroupID = pagePermission.RoleOrGroupID,
                                      ModuleID = pages.ModuleID,
                                      PageID = pages.PageID,
                                      PageNameToLink = pages.PageNameToLink,
                                      IsGrantedForSelect = pagePermission.IsGrantedForSelect,
                                      IsGrantedForInsert = pagePermission.IsGrantedForInsert,
                                      IsGrantedForUpdate = pagePermission.IsGrantedForUpdate,
                                      IsGrantedForDelete = pagePermission.IsGrantedForDelete,
                                      MessageToShow = pages.MessageToShow,
                                      AccessStatus = pages.Status
                                  };

            return pageAccessright.FirstOrDefault();
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
                    contextSecurity.Dispose();
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
