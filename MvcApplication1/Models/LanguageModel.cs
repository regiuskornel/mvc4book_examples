using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Models
{
    public class LanguageModel
    {
        public const string DefaultLanguage = "en";
        public static string[] AppLanguages = new string[] { "hu", "en", "de" };

        public static string GetAvailableOrFallback(string inlang)
        {
            return AppLanguages.Contains(inlang) ? inlang : DefaultLanguage;
        }

        public static string GetLanguageFlagFileName(string lang)
        {
            return string.Format("~/Content/Flags/{0}.png", lang);
        }
    }

}