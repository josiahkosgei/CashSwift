﻿
using CashSwiftDataAccess.Data;
using CashSwiftDataAccess.Entities;

namespace CashSwiftDeposit.ViewModels
{
    public static class AuthenticationAndAuthorisation
    {
        public static bool Authenticate(
          ApplicationViewModel applicationViewModel,
          ApplicationUser user,
          string activityString,
          bool isAuthorising)
        {
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                if (applicationViewModel.UserPermissionAllowed(user, activityString, isAuthorising))
                {
                    ApplicationViewModel.SaveToDatabase(DBContext);
                    return true;
                }
                ApplicationViewModel.SaveToDatabase(DBContext);
                return false;
            }
        }
    }
}
