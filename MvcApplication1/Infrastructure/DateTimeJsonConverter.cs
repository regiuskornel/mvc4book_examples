using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace MvcApplication1.Infrastructure
{
public class DateTimeJsonConverter : JavaScriptConverter
{
    public override IEnumerable<Type> SupportedTypes
    {
        get {return new[] { typeof(DateTime) };}
    }

    public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
    {
        if (!(obj is DateTime)) return null;

        DateTime datet = (DateTime)obj;
        var result = new Dictionary<string, object>();

        //A JS objektum tulajdonságai
        result["dateTime"] = datet.ToString();
        result["date"] = datet.ToShortDateString();
        result["long"] = datet.ToString("D");
        return result;
    }

    public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
    {
        string dateString;
        if (dictionary.ContainsKey("dateTime"))
        {
            dateString = dictionary["dateTime"].ToString();
        }
        else if (dictionary.ContainsKey("date"))
        {
            dateString = dictionary["date"].ToString();
        }
        else if (dictionary.ContainsKey("long"))
        {
            dateString = dictionary["long"].ToString();
        }
        else return null;

        DateTime datet;
        if (DateTime.TryParse(dateString, out datet))
            return datet;
        return null;
    }

}
}