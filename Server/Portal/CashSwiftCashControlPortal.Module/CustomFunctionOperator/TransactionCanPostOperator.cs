
//CustomFunctionOperator.TransactionCanPostOperator


using DevExpress.Data.Filtering;
using DevExpress.Xpo.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CashSwiftCashControlPortal.Module.CustomFunctionOperator
{
    public class TransactionCanPostOperator :
      ICustomFunctionOperator,
      ICustomFunctionOperatorFormattable,
      ICustomFunctionOperatorBrowsable
    {
        public string Name => "TransactionCanPost";

        public int MinOperandCount => 1;

        public int MaxOperandCount => 1;

        public string Description => "Returns whether the entered transaction has posted to core banking";

        public FunctionCategory Category => FunctionCategory.Logical;

        static TransactionCanPostOperator()
        {
            TransactionCanPostOperator customFunction = new TransactionCanPostOperator();
            if (CriteriaOperator.GetCustomFunction(customFunction.Name) != null)
                return;
            CriteriaOperator.RegisterCustomFunction(customFunction);
        }

        public static void Register()
        {
        }

        public object Evaluate(params object[] operands)
        {
            operands.Count();
            return false;
        }

        public Type ResultType(params Type[] operands) => typeof(bool);

        public string Format(Type providerType, params string[] operands) => providerType == typeof(MSSqlConnectionProvider) &&  operands.Count() == 1 ? "funTransactionCanPost '" + operands[0] + "'" : "";

        public bool IsValidOperandCount(int count) => count >= MinOperandCount && count <= MaxOperandCount;

        public bool IsValidOperandType(int operandIndex, int operandCount, Type type) => object.Equals(type, typeof(Guid));
    }
}
