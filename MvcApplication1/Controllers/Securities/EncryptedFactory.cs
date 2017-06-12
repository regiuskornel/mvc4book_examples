using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;

namespace MvcApplication1.Controllers.Securities
{
    public abstract class EncryptedValueProviderFactoryBase : ValueProviderFactory
    {
        //Rekurzív
        internal static void AddToProvidedList(Dictionary<string, object> provided, string prefix, object value)
        {
            var d = value as IDictionary<string, object>;
            if (d != null)
            {
                foreach (KeyValuePair<string, object> entry in d)
                    AddToProvidedList(provided, MakePropertyKey(prefix, entry.Key), entry.Value);
                return;
            }

            var l = value as IList;
            if (l != null)
            {
                for (int i = 0; i < l.Count; i++)
                    AddToProvidedList(provided, MakeArrayKey(prefix, i), l[i]);
                return;
            }
            provided.Add(prefix, value);
        }

        private static string MakeArrayKey(string prefix, int index)
        {
            return prefix + "[" + index.ToString(System.Globalization.CultureInfo.InvariantCulture) + "]";
        }

        private static string MakePropertyKey(string prefix, string propertyName)
        {
            return (String.IsNullOrEmpty(prefix)) ? propertyName : prefix + "." + propertyName;
        }
    }

    public class EncryptedValueProviderFactory : EncryptedValueProviderFactoryBase
    {
        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            var provided = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            try
            {
                //Megkeressük a hobj- karakterekkel kezdodő input elemeket
                foreach (string inputname in
                    controllerContext.HttpContext.Request.Form.Keys.Cast<string>()
                        .Where(inputname =>
                            inputname.StartsWith(SecurityHtmlExtension.SecurityHiddenFieldNamePrefix)))
                {
                    //levágjuk az input név elejéről a hobj- -et
                    string rootinputname = inputname.Substring(SecurityHtmlExtension.SecurityHiddenFieldNamePrefix.Length);
                    //Visszaalakítjuk a szöveges adatot objektummá
                    var decoded = StringEncoderHelper.GetDecoded(controllerContext.HttpContext.Request.Form[inputname]);
                    //Rekurzívan bejárva az objektumot feltöltjük a szótárat azokkal
                    //a property path-okkal, amikhez tudunk értéket szolgáltatni
                    AddToProvidedList(provided, rootinputname, decoded);
                }

                if (provided.Count == 0) return null;
                return new DictionaryValueProvider<object>(provided, System.Globalization.CultureInfo.CurrentCulture);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Hibás adat", ex);
            }
        }
    }

    public class EncryptedRouteAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var routedata = filterContext.RouteData;
            var ritem = routedata.Values["id"];
            if (ritem == null) throw new InvalidOperationException("Hiányzó Id");
            routedata.Values.Remove("id"); //Lecseréljük

            var provided = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            var decoded = StringEncoderHelper.GetDecoded((string)ritem);
            string rootinputname = string.Empty;
            if (!(decoded is IEnumerable))
                rootinputname = "id";
            EncryptedValueProviderFactoryBase.AddToProvidedList(provided, rootinputname, decoded);
            foreach (KeyValuePair<string, object> keyValuePair in provided)
                routedata.Values.Add(keyValuePair.Key, keyValuePair.Value);
        }
    }

    public static class StringEncoderHelper
    {
        static readonly byte[] SecurityKey = new byte[] { 52, 41, 30, 29, 18, 7, 24, 24, 11, 19, 45, 89, 11, 4, 9, 51 };
        static readonly byte[] InitVector = new byte[] { 99, 30, 29, 18, 7, 24, 24, 11, 19, 45, 89, 11, 4, 9, 51, 41 };
        static readonly RijndaelManaged Rijndael;
        //.Net 4.5 private const string purpose = "hiddenfield demo v1";

        static StringEncoderHelper()
        {
            Rijndael = new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = 128,
                BlockSize = 128,
                Key = SecurityKey,
                IV = InitVector
            };
        }

        public static string GetEncodedString(object toEncode)
        {
            string timestamp = string.Format("{0:X16}", DateTime.Now.Ticks);
            string ser = timestamp + new JavaScriptSerializer().Serialize(toEncode);

            byte[] buffer = Encoding.UTF8.GetBytes(ser);
            //.Net 4.5 byte[] encoded = System.Web.Security.MachineKey.Protect(buffer, purpose);
            byte[] encoded = Rijndael.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length);
            return System.Web.HttpServerUtility.UrlTokenEncode(encoded);
        }

        public static object GetDecoded(string encodedString)
        {
            byte[] encoded = System.Web.HttpServerUtility.UrlTokenDecode(encodedString);
            //.Net 4.5 byte[] decoded = System.Web.Security.MachineKey.UnProtect(encoded, purpose);
            byte[] decoded = Rijndael.CreateDecryptor().TransformFinalBlock(encoded, 0, encoded.Length);
            string ser = Encoding.UTF8.GetString(decoded);


            string timestamptxt = ser.Substring(0, 16);
            long timestamp;
            if (long.TryParse(timestamptxt, NumberStyles.HexNumber, null, out timestamp))
            {
                if (new DateTime(timestamp).AddMinutes(1) > DateTime.Now)
                    return new JavaScriptSerializer().DeserializeObject(ser.Substring(16));
                throw new SecurityException("A token ideje lejárt");
            }
            throw new SecurityException("Hibás időbélyeg");
        }
    }

}