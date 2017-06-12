using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FirstMVCApp.Models
{

public class CardRegister
{
    public CardRegister()
    {
        PhoneNumbers = new List<PhoneNumber>();
    }

    public int Id { get; set; }

    [Display(Name = "Teljes név")]
    [StringLength(100)]
    [Required]
    public string FullName { get; set; }

    [Display(Name = "Megszólítás")]
    [StringLength(10)]
    public string Title { get; set; }

    [Display(Name = "Cégnév")]
    [StringLength(200)]
    public string Company { get; set; }

    [Display(Name = "Beosztás")]
    [StringLength(150)]
    public string Position { get; set; }

    //Navigation property
    public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; }
}

public class PhoneNumber
{
    public int Id { get; set; }

    [Required]
    [StringLength(34)]
    [Display(Name = "Telefonszám")]
    public string Number { get; set; }

    //Backreference Id
    [Display(Name = "Névjegykártya")]
    public int CardRegisterId { get; set; }

    //Backreference
    public CardRegister CardRegister { get; set; }
}
}