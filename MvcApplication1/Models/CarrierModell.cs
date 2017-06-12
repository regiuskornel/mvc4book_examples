using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models
{
public enum CustomerTypeEnum
{
    Normal,
    Supplier,
    VIP
}

public class CarrierModell
{
    public CustomerTypeEnum CustomerType { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime? TransportDate { get; set; }

    public List<string> Arranges { get; set; }

    //Számított értékek
    public bool DeliveryDateAvailable
    {
        get { return TransportDate.HasValue; }
    }

    public string WarningMessage
    {
        get
        {
            return !TransportDate.HasValue && OrderDate.Date < DateTime.Today.AddDays(-2)
                ? "Késedelmes szállítás, azonnal intézkedj!" : String.Empty;
        }
    }

    public string CustomerNameCSS
    {
        get
        {
            return CustomerType == CustomerTypeEnum.VIP ?
                "vipcustomer" : "normalcustomer";
        }
    }
}
}