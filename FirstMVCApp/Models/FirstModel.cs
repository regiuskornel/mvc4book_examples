using System;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FirstMVCApp.Models
{
    
    public class FirstModel
    {
        public int Id { get; set; }

        [Display(Name = "Teljes név")]
        public string FullName { get; set; }

        [Display(Name = "Szállítási cím")]
        public string Address { get; set; }
    }



}