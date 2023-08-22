using CashSwiftDataAccess.Entities;
using System.Collections.Generic;

namespace CashSwiftDeposit.Models.Forms
{
    public class Validation
    {
        public ValidationType Type { get; set; }

        public List<ValidationValue> Values { get; set; }

        public string ErrorMessage { get; set; }

        public string SuccessMessage { get; set; }
    }
}
