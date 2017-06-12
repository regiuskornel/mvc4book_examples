using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MvcApplication1.Controllers.Validator;

namespace MvcApplication1.Models
{

    #region Alap modell
    public class ValidationMaxModel
    {
        [HiddenInput]
        public int Id { get; set; }

        [Display(Name = "FullNameLabel", ResourceType = typeof(Resources.UILabels))]
        [Required(ErrorMessage = "A név megadása kötelező (1)!")]
        public string FullName { get; set; }

        [Display(Name = "Vásárló címe")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Display(Name = "Vásárló email")]
        [DataType(DataType.EmailAddress)]
        //Dotnet 4.5: [System.ComponentModel.DataAnnotations.EmailAddressAttribute]
        //Mvc4.0 + Futures [Microsoft.Web.Mvc.EmailAddressAttribute]
        public string Email { get; set; }

        [Display(Name = "Utolsó vásárlás")]
        //[DataType(DataType.Date)]
        [Required]
        [Range(typeof(DateTime), "2012.01.01", "2013.12.31")]
        public DateTime LastPurchaseDate { get; set; }

        [Required]
        public int RequiredInt { get; set; }

        [Required]
        public bool RequiredBool { get; set; }

        public static ValidationMaxModel GetModell(int id)
        {
            if (datalist == null) datalist = new Dictionary<int, ValidationMaxModel>();

            if (!datalist.ContainsKey(id))
            {
                datalist.Add(id, new ValidationMaxModel()
                    {
                        Id = id,
                        FullName = "Tanuló " + id,
                        Address = string.Format("Budapest {0}. kerület", id + 1),
                        Email = "proba@proba.hu",
                        LastPurchaseDate = DateTime.Now.AddDays(-2 * id)
                    });
            }
            return datalist[id];
        }

        private static Dictionary<int, ValidationMaxModel> datalist;
    }
    #endregion

    #region IValidatableObject modellek
    //Első variáció
    public class ValidationMaxIVOModel1 : ValidationMaxModel, IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            if (this.LastPurchaseDate > DateTime.Now || this.LastPurchaseDate < DateTime.Today.AddYears(-2))
            {
                results.Add(new ValidationResult("Az Utolsó vásárlás dátuma nem lehet a jövőben vagy mielőtt a bolt megnyílt!",
                    new[] { "LastPurchaseDate", "FullName" }));
            }
            return results;
        }

        public static ValidationMaxIVOModel GetModell(int id)
        {
            return new ValidationMaxIVOModel()
                {
                    Id = id,
                    FullName = "Tanuló " + id,
                    Address = string.Format("Budapest {0}. kerület", id + 1),
                    Email = "proba@proba.hu",
                    LastPurchaseDate = DateTime.Now.AddDays(-2 * id)
                };
        }
    }

//Második variáció
public class ValidationMaxIVOModel2 : ValidationMaxModel, IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();
        if(!Validator.TryValidateValue(this.LastPurchaseDate, validationContext, results, new[]
        {
            new RangeAttribute(typeof(DateTime),
                DateTime.Today.AddYears(-1).ToString("d"),
                DateTime.Today.AddYears(1).ToString("d"))
        }))
        {
            var badresults = new List<ValidationResult>();
            foreach(var validationResult in results)
                badresults.Add(new ValidationResult(validationResult.ErrorMessage, new[] { "LastPurchaseDate" }));

            return badresults;
        }

        return results;
    }

    public static new ValidationMaxIVOModel GetModell(int id)
    {
        return new ValidationMaxIVOModel()
        {
            Id = id,
            FullName = "Tanuló " + id,
            Address = string.Format("Budapest {0}. kerület", id + 1),
            Email = "proba@proba.hu",
            LastPurchaseDate = DateTime.Now.AddDays(-2 * id)
        };
    }
}

    //Harmadik variáció, (kettőt fizet hármat kap)
    [MetadataType(typeof(ValidationMaxIVOModelMetaData))]
    public class ValidationMaxIVOModel : ValidationMaxModel
    {
        public static new ValidationMaxIVOModel GetModell(int id)
        {
            return new ValidationMaxIVOModel()
            {
                Id = id,
                FullName = "Tanuló " + id,
                Address = string.Format("Budapest {0}. kerület", id + 1),
                Email = "proba@proba.hu",
                LastPurchaseDate = DateTime.Now.AddDays(-2 * id)
            };
        }
    }

    public class ValidationMaxIVOModelMetaData
    {
        //A dátumintervalumok fel lettek cserélve a könyvben szereplő példához képest
        //Így könnyen ellenőrizhető, hogy melyik Range validátor jut érvényre. Egy 2011-es dáutomot az alábbi validátor átenged, az ősosztályon definiált nem. A validáció érvényes lesz.
        [Range(typeof(DateTime), "2010.01.01", "9999.12.31")]
        public DateTime LastPurchaseDate { get; set; }
    }
    #endregion

    #region Realative Date Validator modellek

    public class ValidationMaxRelativeModel : ValidationMaxModel
    {
        [Display(Name = "Utazási nap")]
        //A szerveroldali validációban a "C" nélküli attribútumverzió is jó: RelativeDateValidator
        [RelativeDateValidatorC(RelativeDateValidatorAttribute.RelativeDate.ElozoHonap)]
        public DateTime TravelDate { get; set; }

        public static new ValidationMaxRelativeModel GetModell(int id)
        {
            return new ValidationMaxRelativeModel
            {
                Id = id,
                FullName = "Tanuló " + id,
                Address = string.Format("Budapest {0}. kerület", id + 1),
                Email = "proba@proba.hu",
                LastPurchaseDate = DateTime.Now.AddDays(-2 * id),
                TravelDate = DateTime.Today
            };
        }
    }


    #endregion

    #region Remote validation modell

    [MetadataType(typeof(ValidationMaxRemoteModelMetaData))]
    public class ValidationMaxRemoteModel : ValidationMaxModel
    {
        public static bool IsNameReserved(string newname)
        {
            return datalist.Any(d => d.Value.FullName == newname);
        }

        public static new ValidationMaxRemoteModel GetModell(int id)
        {
            if (datalist == null) datalist = new Dictionary<int, ValidationMaxRemoteModel>();

            if (!datalist.ContainsKey(id))
            {
                datalist.Add(id, new ValidationMaxRemoteModel()
                {
                    Id = id,
                    FullName = "Tanuló " + id,
                    Address = string.Format("Budapest {0}. kerület", id + 1),
                    Email = "proba@proba.hu",
                    LastPurchaseDate = DateTime.Now.AddDays(-2 * id)
                });
            }
            return datalist[id];
        }

        private static Dictionary<int, ValidationMaxRemoteModel> datalist;
    }

    public class ValidationMaxRemoteModelMetaData
    {
        [Remote("RemoteNameValidator", "Validations", ErrorMessage = "Ez már foglalt, próbálj másikat (attribute message)",
            HttpMethod = "Post", AdditionalFields = "Address,Id")]
        public string FullName { get; set; }
    }

    #endregion
}