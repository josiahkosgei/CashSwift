
//CustomFunctionOperator.IsVisibleByUserGroupOperator



using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CashSwiftCashControlPortal.Module.CustomFunctionOperator
{
    internal class IsVisibleByUserGroupOperator :
      ICustomFunctionOperator,
      ICustomFunctionOperatorFormattable,
      ICustomFunctionOperatorBrowsable,
      ICustomFunctionOperatorQueryable
    {
        static IsVisibleByUserGroupOperator()
        {
            IsVisibleByUserGroupOperator customFunction = new IsVisibleByUserGroupOperator();
            if (CriteriaOperator.GetCustomFunction(customFunction.Name) != null)
                return;
            CriteriaOperator.RegisterCustomFunction(customFunction);
        }

        public static void Register()
        {
        }

        public string Name => "IsVisibleByUserGroup";

        public int MinOperandCount => 1;

        public int MaxOperandCount => 1;

        public string Description => "Checks if the logged in user has visibility to an object based on whether the usergroup of the item is a child of the user's usergroup";

        public FunctionCategory Category => FunctionCategory.All;

        public object Evaluate(params object[] operands)
        {
            if (operands.Count() != 1)
                throw new ArgumentOutOfRangeException(nameof(operands), string.Format("operands.Coount {0} != 1", operands.Count()));
            if (!(SecuritySystem.CurrentUser is ApplicationUser currentUser))
                throw new ArgumentNullException("No user logged in");
            SecuritySystem.LogonObjectSpace.ReloadObject(currentUser);
            return currentUser.user_group.GetAllNodes().FirstOrDefault(x => x.id == (int)operands[0]);
        }

        public string Format(Type providerType, params string[] operands)
        {
            if (operands.Count() != 1)
                throw new ArgumentOutOfRangeException(nameof(operands), string.Format("operands.Coount {0} != 1", operands.Count()));
            if (!(SecuritySystem.CurrentUser is ApplicationUser currentUser))
                throw new ArgumentNullException("No user logged in");
            SecuritySystem.LogonObjectSpace.ReloadObject(currentUser);
            return string.Format("{0} IN (SELECT * FROM dbo.funGetUserGroupHierarchyByID({1}))", operands[0], currentUser.user_group.id);
        }

        public Type ResultType(params Type[] operands) => typeof(bool);

        public bool IsValidOperandCount(int count) => count == 1;

        public bool IsValidOperandType(int operandIndex, int operandCount, Type type) => operandIndex == 0 && operandCount == 1 && type.Equals(typeof(int));

        MethodInfo ICustomFunctionOperatorQueryable.GetMethodInfo() => GetType().GetMethod("IsVisibleByUserGroup");
    }
}
