using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarePoint.Extensions
{
    public class ContainsDate : ValidationAttribute, IClientValidatable
    {
        private string _day;
        private string _month;
        private string _year;

        public ContainsDate(string Day,string Month,string Year)
        {
            if(!string.IsNullOrEmpty(Month) && !string.IsNullOrEmpty(Year))
            {
                _day = Day;
                _month = Month;
                _year = Year;
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dayProperty = validationContext.ObjectType.GetProperty(_day);
            var monthProperty = validationContext.ObjectType.GetProperty(_month);
            var yearProperty = validationContext.ObjectType.GetProperty(_year);
            if (dayProperty == null)
                return new ValidationResult(string.Format("Property '{0}' is undefined.", _day));
            if (monthProperty == null)
                return new ValidationResult(string.Format("Property '{0}' is undefined.", _month));
            if (yearProperty == null)
                return new ValidationResult(string.Format("Property '{0}' is undefined.", _year));
            var dayValue = monthProperty.GetValue(validationContext.ObjectInstance, null);
            var monthValue = monthProperty.GetValue(validationContext.ObjectInstance, null);
            var yearValue = yearProperty.GetValue(validationContext.ObjectInstance, null);

            String DateString = String.Format("{0}/{1}/{2}", dayValue, monthValue, yearValue);

            DateTime dateTime;
            if (!DateTime.TryParse(DateString, out dateTime))
            {
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            }
            return null;
        }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule
            {
                ErrorMessage = this.ErrorMessage,
                ValidationType = "Date"
            };
        }
    }
}