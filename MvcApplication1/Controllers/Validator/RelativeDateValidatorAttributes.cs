using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Controllers.Validator
{
    #region szerver oldali relatív dátum validátor
    //Csak propertyn lehet használni.
    [AttributeUsage(AttributeTargets.Property)]
    public class RelativeDateValidatorAttribute : ValidationAttribute
    {
        public enum RelativeDate
        {
            ElozoHonap, Ma, KovetkezoHonap
        }

        private readonly RequiredAttribute innerRequired = new RequiredAttribute();
        protected readonly RelativeDate rdate;

        public RelativeDateValidatorAttribute(RelativeDate relDate)
        {
            this.rdate = relDate;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || !innerRequired.IsValid(value))
                return new ValidationResult("A dátum kitöltendő!");
            DateTime datum = (DateTime)value;
            switch (this.rdate)
            {
                case RelativeDate.ElozoHonap:
                    if (StartOfMonth(datum) != MonthStart(DateTime.Today, -1))
                        return new ValidationResult("A dátum csak múlt hónapi lehet!");
                    break;
                case RelativeDate.Ma:
                    if (datum.Date != DateTime.Today)
                        return new ValidationResult("A dátum csak mai nap lehet!");
                    break;
                case RelativeDate.KovetkezoHonap:
                    if (StartOfMonth(datum) != MonthStart(DateTime.Today, +1))
                        return new ValidationResult("A dátum csak a következő hónapi lehet!");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return ValidationResult.Success;
        }

        private static DateTime StartOfMonth(DateTime d)
        {
            return d.AddDays(-d.Day + 1);
        }

        private static DateTime MonthStart(DateTime d, int monthRel)
        {
            return StartOfMonth(d.AddMonths(monthRel));
        }

    }
    #endregion

    #region Szerver és kliens oldali relatív dátum validátor

    //Osztályon és propertyn is lehet használni.
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class RelativeDateValidatorCAttribute : RelativeDateValidatorAttribute, IClientValidatable
    {
        //A validálandó property neve
        public string PropertyName { get; private set; }

        //Az egyébként belsőleg tárolt szabály kivezetése
        public RelativeDate RelativeDate
        {
            get { return this.rdate; }
        }

        public RelativeDateValidatorCAttribute(RelativeDate relDate)
            : base(relDate)
        {
        }

        public RelativeDateValidatorCAttribute(RelativeDate relDate, string propertyName)
            : base(relDate)
        {
            this.PropertyName = propertyName;
        }

        public IEnumerable<ModelClientValidationRule>
            GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new RelativeDateClientValidationRule(rdate);
        }
    }

    public class RelativeDateClientValidationRule : ModelClientValidationRule
    {
        public RelativeDateClientValidationRule(
            RelativeDateValidatorAttribute.RelativeDate relativeDate)
        {
            switch (relativeDate)
            {
                case RelativeDateValidatorAttribute.RelativeDate.ElozoHonap:
                    ErrorMessage = "A dátum csak múlt hónapi lehet (kliens)!";
                    break;
                case RelativeDateValidatorAttribute.RelativeDate.Ma:
                    ErrorMessage = "A dátum csak mai nap lehet (kliens)!";
                    break;
                case RelativeDateValidatorAttribute.RelativeDate.KovetkezoHonap:
                    ErrorMessage = "A dátum csak a következő hónapi lehet (kliens)!";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("relativeDate");
            }

            ValidationType = "daterelative";
            ValidationParameters.Add("reldate", relativeDate.ToString().ToLower());
        }
    }

    //Global.asax-ba -> DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(RelativeDateValidatorCAttribute), typeof(RelativeDateModelValidator));
    /*
    public class RelativeDateModelValidator : DataAnnotationsModelValidator<RelativeDateValidatorCAttribute>
    {
        public RelativeDateModelValidator(ModelMetadata metadata, 
            ControllerContext context, 
            RelativeDateValidatorCAttribute attribute)
            : base(metadata, context, attribute) {}

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            return new[] {new RelativeDateClientValidationRule(Attribute.FormatErrorMessage(Metadata.GetDisplayName()))};
        }

        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            var field = Metadata.ContainerType.GetProperty(Attribute.PropertyName);
            if (field == null) yield break;

            var value = field.GetValue(container, null);
            if (value is DateTime && !Attribute.IsValid(value))
            {
                yield return new ModelValidationResult { Message = ErrorMessage };
            }
        }
    }*/
    #endregion
}