using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CZManageSystem.Data
{
    public class ViewModelValidator
    {
        public ViewModelValidator(object objectToValidate)
        {
            objectBeingValidated = objectToValidate;
        }

        private static object objectBeingValidated { get; set; }

        public List<ValidationResult> ValidationErrors { get; private set; }
        public string ValidationErrorsToString
        {
            get
            {
                if (ValidationErrors.Count > 0)
                    return string.Join("，", ValidationErrors.Select(s => s.ErrorMessage));
                return "";
            }
            private set { }
        }

        public bool IsValid()
        {
            ValidationErrors = new List<ValidationResult>();
            if (objectBeingValidated == null)
            {
                ValidationErrors.Add(new ValidationResult("实体对象为空"));
                return false;
            }
            var context = new ValidationContext(objectBeingValidated,
                                                null,
                                                null);

            bool isValid = Validator.TryValidateObject(objectBeingValidated,
                                                       context,
                                                       ValidationErrors);

            if (!isValid) 
                return false; 
            return true; 
        }
    }
}
