// ViewModels.IPermissionRequired


namespace CashSwiftDeposit.ViewModels
{
    public interface IPermissionRequired
    {
        void HandleAuthorisationResult(PermissionRequiredResult result);
    }
}
