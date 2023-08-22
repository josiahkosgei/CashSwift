// ViewModels.PermissionRequiredResult


using CashSwiftDataAccess.Entities;

namespace CashSwiftDeposit.ViewModels
{
    public class PermissionRequiredResult
    {
        public bool LoginSuccessful { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
