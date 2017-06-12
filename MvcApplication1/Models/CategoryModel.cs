using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace MvcApplication1.Models
{
public interface ICategoryFullNameUpdateModel
{
    string FullName { get; set; }
}

    //[CategoryModelBinderAttribute]
    public class CategoryModel : ICategoryFullNameUpdateModel
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public DateTime CreatedDate { get; set; }

        [Required]
        public string WillNeverValid { get; set; }

        //Lehetőleg ne használjukn Action és Controller nevű tulajdonságokat
        //public string Action { get; set; }
        //public string Controller { get; set; }


        [Display(Name = "Alkategóriák")]
        public List<CategoryModel> SubCategories { get; set; }

        [Display(Name = "Kategória pár")]
        public CategoryModel JoinedCategory { get; set; }

        #region FrontEnd Rules
        /// <summary>
        /// Minnél mélyebben vagyunk a hierarchiában, annál sötétebb háttérszín stílust ad vissza
        /// Használja: EditorTemplates és DisplayTemplates: CategoryModel.cshtml
        /// </summary>
        /// <param name="depth"></param>
        /// <returns></returns>
        public static string GetColorOfDepth(int depth)
        {
            return string.Format("background-color: #{0:X};", 0xdddddd - 0x181810 * depth); ;
        }
        #endregion

        #region In-memory perzisztencia

        private static int tid;
        static readonly Random Rand = new Random();

        /// <summary>
        /// Hierarchikus listát generál
        /// </summary>
        /// <param name="recreate"></param>
        /// <param name="itemNumber"></param>
        /// <param name="deep"></param>
        /// <returns></returns>
        public static List<CategoryModel> GetList(bool recreate = false, int itemNumber = 2, int deep = 2)
        {
            if (recreate || datalist == null)
            {
                tid = 0;
                datalist = CreateListInner(itemNumber, deep);
            }
            return datalist;
        }

        public static CategoryModel GetCategory(int id)
        {
            if (datalist == null) datalist = GetList();
            return FindById(id, datalist);
        }

        /// <summary>
        /// A modell megkeresése a fában
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modellist"></param>
        /// <returns></returns>
        private static CategoryModel FindById(int id, IList<CategoryModel> modellist)
        {
            var result = modellist.FirstOrDefault(m => m.Id == id);
            if (result != null) return result;
            foreach (var item in modellist.Where(item => item.SubCategories != null))
            {
                result = FindById(id, item.SubCategories);
                if (result != null)
                    return result;
            }
            return null;
        }

        /// <summary>
        /// Fa struktúra generálása
        /// </summary>
        /// <param name="itemNumber"></param>
        /// <param name="deep"></param>
        /// <returns></returns>
        private static List<CategoryModel> CreateListInner(int itemNumber, int deep)
        {
            if (itemNumber <= 0) return new List<CategoryModel>();
            var result = new List<CategoryModel>(itemNumber);
            for (int i = 1; i < itemNumber + 1; i++)
            {
                var id = tid++;
                var cm = new CategoryModel
                {
                    Id = id,
                    FullName = string.Format("Kategória {0}-{1}", deep, id),
                    CreatedDate = DateTime.Now.AddDays(deep * Rand.Next(20, 100) - 200),
                };
                if (deep != 0)
                    cm.SubCategories = CreateListInner(itemNumber, deep - 1);
                result.Add(cm);
            }

            return result;
        }

        private static List<CategoryModel> datalist;
        #endregion

    }

    public class CategoryDictionaryModel
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public DateTime CreatedDate { get; set; }


        [Display(Name = "Alkategóriák")]
        public IDictionary<string, CategoryDictionaryModel> SubCategories { get; set; }

        #region FrontEnd Rules
        /// <summary>
        /// Minnél mélyebben vagyunk a hierarchiában, annál sötétebb háttérszín stílust ad vissza
        /// Használja: EditorTemplates és DisplayTemplates: CategoryModel.cshtml
        /// </summary>
        /// <param name="depth"></param>
        /// <returns></returns>
        public static string GetColorOfDepth(int depth)
        {
            return string.Format("background-color: #{0:X};", 0xdddddd - 0x181810 * depth); ;
        }
        #endregion

        #region In-memory perzisztencia

        private static int tid;
        static readonly Random Rand = new Random();

        /// <summary>
        /// Hierarchikus listát generál
        /// </summary>
        /// <param name="recreate"></param>
        /// <param name="itemcount"></param>
        /// <param name="deep"></param>
        /// <returns></returns>
        public static Dictionary<string, CategoryDictionaryModel> GetList(bool recreate = false, int itemcount = 4, int deep = 3)
        {
            if (recreate || datalist == null)
            {
                tid = 0;
                datalist = CreateListInner(itemcount, deep);
            }
            return datalist;
        }

        public static CategoryDictionaryModel GetCategory(string key)
        {
            if (datalist == null) datalist = GetList();
            return FindByKey(key, datalist);
        }

        /// <summary>
        /// A modell megkeresése a fában
        /// </summary>
        private static CategoryDictionaryModel FindByKey(string key, IDictionary<string, CategoryDictionaryModel> modellist)
        {
            CategoryDictionaryModel result;
            if (modellist.TryGetValue(key, out result))
                return result;
            foreach (var item in modellist.Values.Where(item => item.SubCategories != null))
            {
                result = FindByKey(key, item.SubCategories);
                if (result != null)
                    return result;
            }
            return null;
        }

        /// <summary>
        /// Fa struktúra generálása
        /// </summary>
        /// <param name="itemcount"></param>
        /// <param name="deep"></param>
        /// <returns></returns>
        private static Dictionary<string, CategoryDictionaryModel> CreateListInner(int itemcount, int deep)
        {
            if (itemcount <= 0) return new Dictionary<string, CategoryDictionaryModel>();
            var result = new Dictionary<string, CategoryDictionaryModel>(itemcount);
            for (int i = 1; i < itemcount + 1; i++)
            {
                var id = tid++;
                var cm = new CategoryDictionaryModel
                {
                    Id = id,
                    FullName = string.Format("Kategória {0}-{1}", deep, id),
                    CreatedDate = DateTime.Now.AddDays(deep * Rand.Next(20, 100) - 200),
                };
                if (deep != 0)
                    cm.SubCategories = CreateListInner(itemcount, deep - 1);
                result.Add("Di" + id, cm);
            }

            return result;
        }

        private static Dictionary<string, CategoryDictionaryModel> datalist;
        #endregion

    }


}