using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MvcApplication1.Models
{
    public class ActionDemoModel
    {

        [HiddenInput(DisplayValue = true)]
        public int Id { get; set; }

        [Display(Name = "FullNameLabel", ResourceType = typeof(Resources.UILabels))]
        public string FullName { get; set; }

        //[AllowHtml]
        [Display(Name = "Vásárló címe")]
        //[DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Display(Name = "Vásárló email")]
        [DataType(DataType.EmailAddress)]
        //Dotnet 4.5: [EmailAddressAttribute]
        public string Email { get; set; }

        [Display(Name = "Vásárlások összértéke")]
        public decimal TotalSum { get; set; }

        [Display(Name = "Utolsó vásárlás")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime LastPurchaseDate { get; set; }

        [Display(Name = "Vásárlások listája")]
        public IList<ActionDemoProductModel> PurchasesList { get; set; }

        [Display(Name = "Kiemelt várárlás")]
        public ActionDemoProductModel KeyPurchase { get; set; }

        public int[] KeyPurchaseIds { get; set; }

        [Display(Name = "Fontos ügyfél")]
        public bool VIP { get; set; }


        #region In memory perzisztencia
        public static ActionDemoModel GetModell(int id)
        {
            if (datalist == null) datalist = new Dictionary<int, ActionDemoModel>();

            if (!datalist.ContainsKey(id))
            {
                var products=ActionDemoProductModel.CreateProducts();
                datalist.Add(id, new ActionDemoModel
                    {
                        Id = id,
                        FullName = "Tanuló " + id,
                        Address = string.Format("Budapest {0}. kerület", id + 1),
                        Email = "proba@proba.hu",
                        TotalSum = id * 345.45m,
                        LastPurchaseDate = DateTime.Now.AddDays(-2 * id),
                        PurchasesList = products,
                        KeyPurchase = products[2]
                    });
            }
            return datalist[id];
        }

        public static IList<ActionDemoModel> GetList()
        {
            return datalist.Select(dl => dl.Value).ToList();
        }

        public SelectList GetSelectList()
        {
            return new SelectList(this.PurchasesList, "Id", "ProductName",this.KeyPurchase.Id);
        }

        private static Dictionary<int, ActionDemoModel> datalist;
        #endregion

    }

    public class ActionDemoProductModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Cikkszám")]
        public string ItemNo { get; set; }

        [Display(Name = "Termék név")]
        public string ProductName { get; set; }

        [Display(Name = "Mennyiség")]
        public int Quantity { get; set; }

        #region Listafeltöltés
        private static int tid; //next id
        public static IList<ActionDemoProductModel> CreateProducts()
        {
            var rand = new Random();
            int count = rand.Next(5, 10);
            var result = new List<ActionDemoProductModel>(count);
            for (int i = 0; i < count; i++)
            {
                result.Add(new ActionDemoProductModel
                    {
                        Id = ++tid,
                        ItemNo = string.Format("szam{0}-k{1}", i, DateTime.Today.Day),
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

public class WrongModel
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string name { get; set; }

    public string NAME { get; set; }

    public bool Checked { get; set; }

    public bool Disabled { get; set; }

    public string Form { get; set; }

    public string Value { get; set; }
}

}