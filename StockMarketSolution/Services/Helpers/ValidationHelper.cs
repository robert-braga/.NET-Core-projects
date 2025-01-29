using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public class ValidationHelper
    {
        internal static void ModelValidation(object obj)
        {
            ValidationContext validationContext = new ValidationContext(obj, null, null);

            List<ValidationResult> results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, results, true);

            if (!isValid)
            {
                StringBuilder sb = new StringBuilder();
                foreach (ValidationResult validationResult in results)
                {
                    sb.AppendLine(validationResult.ErrorMessage);
                }
                throw new ArgumentException(sb.ToString());
            }
        }
    }
}