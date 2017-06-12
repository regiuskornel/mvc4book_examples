using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.Models;

namespace MvcApplication1.Controllers
{
    public class BinderDemoController : Controller
    {
        public BinderDemoController()
        {
            var prov = ModelBinderProviders.BinderProviders;
        }

        public ActionResult Index()
        {
            return View(CategoryModel.GetList(false, 5, 1));
        }

        #region Egyszerű típusok
        public ActionResult Edit(int id)
        {
            return View(CategoryModel.GetCategory(id));
        }

        [HttpPost]
        public ActionResult Edit()
        {
            int id;
            DateTime createdDate;

            if (!Int32.TryParse(Request["Id"], out id))
                return Content("Id nem áll rendelkezésre");
            var model = CategoryModel.GetCategory(id);

            string nev = Request["FullName"];
            if (string.IsNullOrEmpty(nev))
                return Content("A nevet meg kell adni!");
            model.FullName = nev;

            if (DateTime.TryParse(Request.Form["CreatedDate"], out createdDate))
                model.CreatedDate = createdDate;
            else
                return Content("'Létrehozva' nem dátum");

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit1(int id, FormCollection coll)
        {
            try
            {
                var model = CategoryModel.GetCategory(id);
                ValueProviderResult fullname = coll.GetValue("FullName");
                ValueProviderResult createdDate = coll.GetValue("CreatedDate");
                model.FullName = (string)fullname.ConvertTo(typeof(string));
                model.CreatedDate = (DateTime)createdDate.ConvertTo(typeof(DateTime));
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit2(int id, string FuLNAme, string Fullname, string fullname, DateTime createdDate, List<CategoryModel> subCategories)
        {
            var model = CategoryModel.GetCategory(id);
            model.FullName = FuLNAme;
            model.CreatedDate = createdDate;
            model.SubCategories = subCategories;
            return RedirectToAction("Index");
        }

        //public ActionResult Edit3(CategoryModel inputmodel, string fullname, string createdDate, DateTime CreatedDate)
        //{
        //}

        [HttpPost]
        public ActionResult Edit3(CategoryModel inputmodel, string fullname, string createdDate)
        {
            var model = CategoryModel.GetCategory(inputmodel.Id);
            model.FullName = inputmodel.FullName;
            model.CreatedDate = inputmodel.CreatedDate;
            model.SubCategories = inputmodel.SubCategories;
            return RedirectToAction("Index");
        }

        /// <summary>
        /// manuális model update
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit4(int id)
        {
            try
            {
                var model = CategoryModel.GetCategory(id);
                this.UpdateModel(model);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// manuális model update TryUpdate
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit5(int id)
        {
            var model = CategoryModel.GetCategory(id);
            if (this.TryUpdateModel(model))
            {
                //Minden Ok, mehet a mentés
            }
            else
            {
                //Minden Ok, mehet a mentés
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        ///  manuális model update szűréssel
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit6(int id)
        {
            var model = CategoryModel.GetCategory(id);
            //model.WillNeverValid = "Csak legyen valami";
            //1. Interface
            if (this.TryUpdateModel<ICategoryFullNameUpdateModel>(model))
            {
                //MindenOk.        
                bool isValid = this.ModelState.IsValid;
            }

            //2. white list
            if (this.TryUpdateModel(model, string.Empty, new[] { "FullName" }))
            {
                //MindenOk.        
                bool isValid = this.ModelState.IsValid;
            }

            //3. black list
            if (this.TryUpdateModel(model, string.Empty, null, new[] { "CreatedDate", "WillNeverValid" }))
            {
                //MindenOk.        
                bool isValid = this.ModelState.IsValid;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateOnlyFormFieldsAttribute]
        public ActionResult Edit7([Bind(Include = "FullName", Exclude = "CreatedDate,WillNeverValid")] CategoryModel inputmodel)
        {
            var model = CategoryModel.GetCategory(inputmodel.Id);
            model.FullName = inputmodel.FullName;

            //this.ModelState["WillNeverValid"].Errors.Clear();
            bool isValid = this.ModelState.IsValid;

            return RedirectToAction("Index");
        }
        #endregion

        #region Listák és szótárak
        public ActionResult ListTree()
        {
            ViewData["depth"] = 1;
            return View(CategoryModel.GetList());
        }

        public ActionResult RecreateTree()
        {
            //7,4;5,5
            CategoryModel.GetList(true);
            return RedirectToAction("ListTree");
        }

        public ActionResult EditTree(int id)
        {
            ViewData["depth"] = 1;
            return View(CategoryModel.GetCategory(id));
        }


        [HttpPost]
        [ActionName("EditTree")]
        public ActionResult EditTreePost(int id)
        {
            var model = CategoryModel.GetCategory(id);
            this.TryUpdateModel(model);
            return RedirectToAction("ListTree");
        }



        public ActionResult DictionaryTree()
        {
            ViewData["depth"] = 1;
            return View(CategoryDictionaryModel.GetList());
        }

        public ActionResult RecreateDTree()
        {
            //7,4;5,5
            CategoryDictionaryModel.GetList(true);
            return RedirectToAction("DictionaryTree");
        }

        public ActionResult EditDictionaryTree(string key)
        {
            ViewData["depth"] = 1;
            return View(CategoryDictionaryModel.GetCategory(key));
        }


        [HttpPost]
        [ActionName("EditDictionaryTree")]
        public ActionResult EditDictionaryTreePost(string key, CategoryDictionaryModel inputmodel)
        {

            var model = CategoryDictionaryModel.GetCategory(key);


            this.TryUpdateModel(model);
            return RedirectToAction("DictionaryTree");
        }
        #endregion

        #region Saját modelbinder

        public ActionResult EditCategModelBinder(int id)
        {
            return View(CategoryModel.GetCategory(id));
        }

        //A használatához a CategoryModel-en el kell helyezni a CategoryModelBinderAttribute-ot
        //vagy a Global.asax-ban a Application_Start()-ban regisztrálni kell a CategoryModelBinder-t vagy a CategoryModelBinderProvider-t
        [HttpPost]
        public ActionResult EditCategModelBinder(CategoryModel inputmodel)
        {
            //TODO: menteni az adatbázisba...
            return RedirectToAction("Index");
        }


        #endregion

        #region ValueProvider kiválazstása

        public ActionResult FixValueProvider()
        {
            return View();
        }

        [HttpPost]
        [ActionName("FixValueProvider")]
        public ActionResult FixValueProviderPost()
        {
            //Value provider csak az URL paraméterrel dolgozik
            var querystringValues = new QueryStringValueProvider(this.ControllerContext);
            var routeValues = new RouteDataValueProvider(this.ControllerContext);

            ValueProviderResult action = querystringValues.GetValue("action"); //action=null
            ValueProviderResult controller = querystringValues.GetValue("controller"); //controller=null

            ValueProviderResult idResult = querystringValues.GetValue("Id"); //idResult=null

            int id = (int)routeValues.GetValue("Id").ConvertTo(typeof(int)); //idResult=99999
            string EzNemValid = querystringValues.GetValue(key: "WillNeverValid").AttemptedValue;

            var model = CategoryModel.GetCategory(1);
            //A model.WillNeverValid értéke "Hát ez honnan jött?" lesz:
            this.TryUpdateModel<CategoryModel>(model, string.Empty, querystringValues);

            //Input mezők 
            var formValues = new FormValueProvider(this.ControllerContext);
            bool szerepel = formValues.ContainsPrefix("prefix.WillNeverValid");
            this.TryUpdateModel<CategoryModel>(model, "prefix", formValues);

            return RedirectToAction("FixValueProvider");
        }

        #endregion
    }

    public class ValidateOnlyFormFieldsAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext fc)
        {
            var modelState = fc.Controller.ViewData.ModelState;

            var keysWithNoIncomingValue =
                modelState.Keys.Where(x => !fc.Controller.ValueProvider.ContainsPrefix(x));

            foreach (var key in keysWithNoIncomingValue)
                modelState[key].Errors.Clear();
        }
    }
}
