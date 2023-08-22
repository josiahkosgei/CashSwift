
//CustomFunctionOperator.UserIsAdministrativeOperator


using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CashSwiftCashControlPortal.Module.CustomFunctionOperator
{
    public class UserIsAdministrativeOperator :
      ICustomFunctionOperator,
      ICustomFunctionOperatorBrowsable
    {
        public string Name => "UserIsAdministrative";

        public int MinOperandCount => 0;

        public int MaxOperandCount => 0;

        public string Description => "Check whether the logged in user has the IsAdministrative privilege";

        public FunctionCategory Category => FunctionCategory.Logical;

        public object Evaluate(params object[] operands)
        {
            try
            {
                if (operands.Count() == 0)
                    return (SecuritySystem.CurrentUser as ApplicationUser).Roles.Any(x => x.IsAdministrative);
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        public bool IsValidOperandCount(int count) => count == 0;

        public bool IsValidOperandType(int operandIndex, int operandCount, Type type) => false;

        public Type ResultType(params Type[] operands) => typeof(bool);

        static UserIsAdministrativeOperator()
        {
            UserIsAdministrativeOperator customFunction = new UserIsAdministrativeOperator();
            if (CriteriaOperator.GetCustomFunction(customFunction.Name) != null)
                return;
            CriteriaOperator.RegisterCustomFunction(customFunction);
        }

        public static void Register()
        {
        }
    }
}
