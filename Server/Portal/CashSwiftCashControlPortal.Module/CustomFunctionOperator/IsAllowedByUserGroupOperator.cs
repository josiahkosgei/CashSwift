
//CustomFunctionOperator.IsAllowedByUserGroupOperator


using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CashSwiftCashControlPortal.Module.CustomFunctionOperator
{
    internal class IsAllowedByUserGroupOperator :
      ICustomFunctionOperator,
      ICustomFunctionOperatorFormattable,
      ICustomFunctionOperatorBrowsable
    {
        public string Name => "IsAllowedByUserGroup";

        public int MinOperandCount => 1;

        public int MaxOperandCount => 1;

        public string Description => "Checks if the user is in the same group or parent group";

        public FunctionCategory Category => FunctionCategory.All;

        static IsAllowedByUserGroupOperator()
        {
            IsAllowedByUserGroupOperator customFunction = new IsAllowedByUserGroupOperator();
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
                if (operands.Count() == 1)
                    return IsAllowed((Guid)operands[0]);
                operands.Count();
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool IsAllowed(Guid user)
        {
            if (!(SecuritySystem.CurrentUser is ApplicationUser currentUser))
                throw new ArgumentNullException("No user logged in");
            SecuritySystem.LogonObjectSpace.ReloadObject(currentUser);
            return currentUser.user_group.GetAllUsers().FirstOrDefault(x => x.id == user) != null;
        }

        public Type ResultType(params Type[] operands) => typeof(bool);

        public string Format(Type providerType, params string[] operands)
        {
            Guid result;
            return Guid.TryParse(operands[0], out result) ? IsAllowed(result).ToString() : "1=1";
        }

        public bool IsValidOperandCount(int count) => count >= MinOperandCount && count <= MaxOperandCount;

        public bool IsValidOperandType(int operandIndex, int operandCount, Type type) => object.Equals(type, typeof(Guid));
    }
}
