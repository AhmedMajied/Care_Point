using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarePoint.Extensions
{
    public class CalendarDay : ValidationAttribute, IClientValidatable
    {
        private string _month;
        private string _year;

        public CalendarDay(string Month,string Year)
        {
            if(!string.IsNullOrEmpty(Month) && !string.IsNullOrEmpty(Year))
            {
                _month = Month;
                _year = Year;
            }
        }

        protected override ValidationResult IsValid(object dayValue, ValidationContext validationContext)
        {
            var monthProperty = validationContext.ObjectType.GetProperty(_month);
            var yearProperty = validationContext.ObjectType.GetProperty(_year);
            if (monthProperty == null)
                return new ValidationResult(string.Format("Property '{0}' is undefined.", _month));
            if (yearProperty == null)
                return new ValidationResult(string.Format("Property '{0}' is undefined.", _year));
            var monthValue = monthProperty.GetValue(validationContext.ObjectInstance, null);
            var yearValue = yearProperty.GetValue(validationContext.ObjectInstance, null);

            String DateString = String.Format("{0}/{1}/{2}", monthValue, dayValue, yearValue);

            DateTime dateTime;
            if (!DateTime.TryParse(DateString, out dateTime))
            {
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            }
            return null;
        }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = this.ErrorMessage,
                ValidationType = "datevalidator"
            };
            rule.ValidationParameters.Add("param", metadata.PropertyName+','+_month+','+_year);
            
            yield return rule;
        }
    }
}