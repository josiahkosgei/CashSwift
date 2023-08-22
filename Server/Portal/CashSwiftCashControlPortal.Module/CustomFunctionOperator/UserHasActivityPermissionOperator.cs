
//CustomFunctionOperator.UserHasActivityPermissionOperator



using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CashSwiftCashControlPortal.Module.CustomFunctionOperator
{
    public class UserHasActivityPermissionOperator :
      ICustomFunctionOperator,
      ICustomFunctionOperatorFormattable,
      ICustomFunctionOperatorBrowsable
    {
        public string Name => "UserHasActivityPermission";

        public int MinOperandCount => 1;

        public int MaxOperandCount => 1;

        public string Description => "Checks if the user has a permission to do an Activity";

        public FunctionCategory Category => FunctionCategory.All;

        static UserHasActivityPermissionOperator()
        {
            UserHasActivityPermissionOperator customFunction = new UserHasActivityPermissionOperator();
            if (CriteriaOperator.GetCustomFunction(customFunction.Name) != null)
                return;
            CriteriaOperator.RegisterCustomFunction(customFunction);
        }

        public static void Register()
        {
        }

        public object Evaluate(params object[] operands)
        {
            try
            {
                if (operands.Count() != 1)
                    return false;
                return operands[0] is string ? IsAllowed((string)operands[0]) : (object)false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool IsAllowed(string activityName)
        {
            if (!(SecuritySystem.CurrentUser is ApplicationUser currentUser))
                throw new ArgumentNullException("No user logged in");
            SecuritySystem.LogonObjectSpace.ReloadObject(currentUser);
            if ((bool)new UserIsAdministrativeOperator().Evaluate(Array.Empty<object>()))
                return true;
            currentUser.user_group.GetAllUsers();
            return currentUser.DeviceRole.Permissions.FirstOrDefault(x => x.Activity.name.Equals(activityName, StringComparison.OrdinalIgnoreCase)) != null;
        }

        public Type ResultType(params Type[] operands) => typeof(bool);

        public string Format(Type providerType, params string[] operands) => operands[0] != null ? IsAllowed(operands[0]).ToString() : "1=0";

        public bool IsValidOperandCount(int count) => count >= MinOperandCount && count <= MaxOperandCount;

        public bool IsValidOperandType(int operandIndex, int operandCount, Type type) => object.Equals(type, typeof(Guid));
    }
}
