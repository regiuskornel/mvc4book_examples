using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using System.ComponentModel;


namespace MvcApplication1.Models
{
    [CustomValidation(typeof(ValidationDemoModel), "ValidateDemoModel")]
    public class ValidationDemoModel
    {
        [HiddenInput(DisplayValue = false)]
        //[HiddenInput(DisplayValue = true)]
        public int Id { get; set; }

        [Display(Name = "FullNameLabel", ResourceType = typeof(Resources.UILabels))]
        [Required(ErrorMessage = "A név megadása kötelező (1)!")]
        //[Required(ErrorMessageResourceName = "UserNameRule",
        //    ErrorMessageResourceType = typeof(Resources.Validations))]
        //[StringLength(10, MinimumLength = 9)]
        //[DataType(DataType.Password)]
        //[DataType(DataType.Url)]
        [CustomValidation(typeof(ValidationDemoModel), "ValidateFullName")]
        //[RegularExpression(@"^[a-zA-Z'\s]{1,20}$",
        //     ErrorMessage = "Kötelezően csak az angol ABC betűi lehetnek, maximálisan 20 karakter hosszúságban!")]
        public string FullName { get; set; }

        [Display(Name = "Vásárló címe")]
        [DataType(DataType.MultilineText)]
        //[Required(ErrorMessageResourceName = "AddressRule",
        //    ErrorMessageResourceType = typeof(Resources.Validations))]
        public string Address { get; set; }

        [Display(Name = "Vásárló email")]
        [DataType(DataType.EmailAddress)]
        //Dotnet 4.5: [EmailAddressAttribute]
        public string Email { get; set; }

        [Display(Name = "Vásárlások összértéke")]
        [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        //[Range(100.1, 200.1)]
        public decimal TotalSum { get; set; }

        [Display(Name = "Utolsó vásárlás")]
        [DataType(DataType.Date)]
        [Range(typeof(DateTime), "2010.01.01", "9999.12.31")]
        //[DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime LastPurchaseDate { get; set; }

        [Display(Name = "Vásárló típus")]
        [EnumDataType(typeof(CustomerTypeEnum))]
        public CustomerTypeEnum CustomerType { get; set; }

        public static ValidationDemoModel GetModell(int id)
        {
            if (datalist == null) datalist = new Dictionary<int, ValidationDemoModel>();

            if (!datalist.ContainsKey(id))
            {
                datalist.Add(id, new ValidationDemoModel()
                    {
                        Id = id,
                        FullName = "Tanuló " + id,
                        Address = string.Format("Budapest {0}. kerület", id + 1),
                        Email = "proba@proba.hu",
                        TotalSum = id * 2345.45m,
                        LastPurchaseDate = DateTime.Now.AddDays(-2 * id)
                    });
            }
            return datalist[id];
        }

        private static Dictionary<int, ValidationDemoModel> datalist;

        public static ValidationResult ValidateFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return new ValidationResult("A nevet meg kell adni!");
            if (fullName.IndexOfAny("0123456789".ToCharArray()) >= 0)
                return new ValidationResult("A név nem tartalmazhat számot!");
            return ValidationResult.Success;
        }

        public static ValidationResult ValidateDemoModel(ValidationDemoModel tovalidate)
        {
            if (string.IsNullOrEmpty(tovalidate.Address) && string.IsNullOrEmpty(tovalidate.Email))
                return new ValidationResult("A címet vagy az email címet meg kell adni!", new[] { "Address" });
            return ValidationResult.Success;
        }

        public enum CustomerTypeEnum
        {
            [Description("Nem ismert")]
            Unknown,
            [Description("Magán személy")]
            Person,
            [Description("Kiskereskedő")]
            Retailer,
            [Description("Nagykereskedő")]
            Supplier
        }
    }
}