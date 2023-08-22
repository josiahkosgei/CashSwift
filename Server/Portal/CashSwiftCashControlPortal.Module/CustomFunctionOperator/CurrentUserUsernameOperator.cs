
//CustomFunctionOperator.CurrentUserUsernameOperator


using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Xpo.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CashSwiftCashControlPortal.Module.CustomFunctionOperator
{
    public class CurrentUserUsernameOperator :
      ICustomFunctionOperator,
      ICustomFunctionOperatorFormattable,
      ICustomFunctionOperatorBrowsable
    {
        public string Name => "CurrentUserUsername";

        public int MinOperandCount => 0;

        public int MaxOperandCount => 1;

        public string Description => "Returns whether the entered string is the current username (case insensitive) or returns the current username used to login";

        public FunctionCategory Category => FunctionCategory.Text;

        static CurrentUserUsernameOperator()
        {
            CurrentUserUsernameOperator customFunction = new CurrentUserUsernameOperator();
            if (CriteriaOperator.GetCustomFunction(customFunction.Name) != null)
                return;
            CriteriaOperator.RegisterCustomFunction(customFunction);
        }

        public static void Register()
        {
        }

        public object Evaluate(params object[] operands) => operands.Count() == 1 ? string.Equals(operands[0] as string, SecuritySystem.CurrentUserName, StringComparison.CurrentCultureIgnoreCase) : (object)SecuritySystem.CurrentUserName;

        public Type ResultType(params Type[] operands) => operands.Count() == 1 ? typeof(bool) : typeof(string);

        public string Format(Type providerType, params string[] operands) => providerType == typeof(MSSqlConnectionProvider) &&  operands.Count() == 0 ? SecuritySystem.CurrentUserName : "";

        public bool IsValidOperandCount(int count) => count >= MinOperandCount && count <= MaxOperandCount;

        public bool IsValidOperandType(int operandIndex, int operandCount, Type type) => object.Equals(type, typeof(string));
    }
}
