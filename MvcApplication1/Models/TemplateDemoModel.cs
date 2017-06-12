using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MvcApplication1.Models
{
[AdditionalMetadata("modell metainfo", "További metainformációk")]
public class TemplateDemoModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "FullNameLabel", ResourceType = typeof(Resources.UILabels))]
        [DataType(DataType.Text)]
        [Required]

        [UIHint("AaaaTemplate")]
        [ReadOnly(true)]
        [DisplayFormat(DataFormatString = "g",
            ApplyFormatInEditMode = true,
            NullDisplayText = "Üres szöveg esetén")]
        [AdditionalMetadata("placeholder","Mi a neved?")]
        public string FullName { get; set; }

        //[AllowHtml]
        [Display(Name = "Vásárló címe")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Display(Name = "Vásárló email")]
        //[DataType(DataType.EmailAddress)] 
        //Dotnet 4.5: [EmailAddressAttribute]
        [DataType(DataType.Text)]
        public string Email { get; set; }

        [Display(Name = "Vásárlások összértéke")]
        //[UIHint("Int32")]
        //[UIHint("KEuro")]
        //[UIHint("RangeEuro")]
        public decimal TotalSum { get; set; }

        [Display(Name = "Utolsó vásárlás")]
        [DataType(DataType.Date)]
        public DateTime LastPurchaseDate { get; set; }

        [Display(Name = "Vásárlások listája")]
        public IList<TemplateDemoProductModel> PurchasesList { get; set; }

        [Display(Name = "Kiemelt várárlás")]
        public TemplateDemoProductModel KeyPurchase { get; set; }

        [Display(Name = "Fontos ügyfél")]
        //        [UIHint("Text")]
        public bool? VIP { get; set; }

        [DataType(DataType.Currency)]
        public int Duration { get; set; }

    #region In memory perzisztencia
        public static TemplateDemoModel GetModell(int id)
        {
            if (datalist == null) datalist = new Dictionary<int, TemplateDemoModel>();

            if (!datalist.ContainsKey(id))
            {
                var product = TemplateDemoProductModel.CreateProduct(id);
                datalist.Add(id, new TemplateDemoModel
                    {
                        Id = id,
                        FullName = "Vásárló " + id,
                        Address = string.Format("Budapest {0}. kerület", (id % 22) + 1),
                        Email = "proba@proba.hu",
                        TotalSum = id * 345.45m,
                        LastPurchaseDate = DateTime.Now.AddDays(-2 * id),
                        PurchasesList = product,
                        KeyPurchase = product[2]
                    });
            }
            return datalist[id];
        }

        public static IList<TemplateDemoModel> GetList(int count)
        {
            if (datalist == null)
            {
                datalist = new Dictionary<int, TemplateDemoModel>();
                for (int i = 1; i < count + 1; i++)
                    GetModell(i);
            }

            return datalist.Select(dl => dl.Value).ToList();
        }

        //Ajaxdemo-hoz
        public static IEnumerable<TemplateDemoModel> GetList()
        {
            return datalist.Select(dl => dl.Value);
        }

        private static Dictionary<int, TemplateDemoModel> datalist;
        #endregion

    }

    public class TemplateDemoProductModel
    {
        static readonly Random rand = new Random();

        [Display(Name = "Azonosító")]
        public int Id { get; set; }

        [Display(Name = "Cikkszám")]
        [DataType(DataType.Text)]
        public string ItemNo { get; set; }

        [Display(Name = "Termék név")]
        [DataType(DataType.Text)]
        public string ProductName { get; set; }

        [Display(Name = "Mennyiség")]
        public int Quantity { get; set; }

        #region Listafeltöltés
        private static int tid; //next id
        public static IList<TemplateDemoProductModel> CreateProduct(int parentid)
        {
            int count = rand.Next(5, 10);
            var result = new List<TemplateDemoProductModel>(count);
            for (int i = 0; i < count; i++)
            {
                result.Add(new TemplateDemoProductModel
                    {
                        Id = ++tid,
                        ItemNo = string.Format("szam-{0}/{1}-k{2,-3}", parentid, i, DateTime.Today.Day),
                        Quantity = rand.Next(1, 1000),
                        ProductName = string.Format("{0}{1}",
                            ProductNames[rand.Next(ProductNames.Length)], tid * 1001)
                    });
            }
            return result;
        }
        private static readonly string[] ProductNames =
            new[] { "Szék", "Ágy", "Asztal", "Párna", "Tükör", "Polc" };
        #endregion
    }

}