using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RASolarHelper;

namespace RASolarSecurity.Model
{
    public interface IRASolarSecurityReposritory : IDisposable
    {
        UserInformation GetUserInformation(string userId, string userPassword, byte userStatus);
        ListOfPagesWithAccessRightsForThisUser ReadListOfPagesWithAccessRightsForAnUser(string userID, string pageOption, string pageID);
        List<ListOfPagesWithAccessRightsForThisUser> ReadListOfPagesWithAccessRightsForUser(string userID, string pageOption, string pageID);
        PageAccessRightHelper ReadPageAccessRight(string moduleId, string pageNameToLink, string roleOrGroupID, byte accessRightStatus);

    }
}
