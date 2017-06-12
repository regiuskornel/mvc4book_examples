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
    public class CacheDemoModel
    {

        public CacheDemoModel()
        {
            this.CreateTime = DateTime.Now;
        }

        [HiddenInput]
        public int Id { get; set; }

        [Display(Name = "FullNameLabel", ResourceType = typeof(Resources.UILabels))]
        [Required(ErrorMessage = "A név megadása kötelező (1)!")]
        public string FullName { get; set; }

        [Display(Name = "Lekérdezési időpont")]
        [ReadOnly(true)]
        public DateTime SelectTime { get; private set; }

        [Display(Name = "Létrehozási időpont")]
        [ReadOnly(true)]
        public DateTime CreateTime { get; private set; }

        public static CacheDemoModel GetModell(int id)
        {
            if (datalist == null) datalist = new Dictionary<int, CacheDemoModel>();
            if (!datalist.ContainsKey(id))
            {
                datalist.Add(id, new CacheDemoModel()
                    {
                        Id = id,
                        FullName = "Tanuló " + id,
                    });
            }

            var dl = datalist[id];
            dl.SetSelectTime();
            return dl;
        }


        public void SetSelectTime()
        {
            this.SelectTime = DateTime.Now;
        }

        private static Dictionary<int, CacheDemoModel> datalist;
    }
}