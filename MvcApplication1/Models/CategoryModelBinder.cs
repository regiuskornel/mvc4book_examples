using System;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Models
{
    public class CategoryModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext,
                                ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;
            int id;
            string fullName;
            DateTime createdDate;
            
            id = Convert.ToInt32(request.Form.Get("Id"));
            fullName = request.Form.Get("FullName");
            createdDate = Convert.ToDateTime(request.Form.Get("CreatedDate"));

            id = (int)bindingContext.ValueProvider.GetValue("Id").ConvertTo(typeof(int));
            fullName = bindingContext.ValueProvider.GetValue("FullName").AttemptedValue;
            createdDate = (DateTime)bindingContext.ValueProvider.GetValue("CreatedDate").ConvertTo(typeof(DateTime));

            //Value provider dolgozik a route kollekción
            string action = bindingContext.ValueProvider.GetValue("action").AttemptedValue;
            string controller = bindingContext.ValueProvider.GetValue("controller").AttemptedValue;

            //Value provider dolgozik a 
            string neverValid = bindingContext.ValueProvider.GetValue("WillNeverValid").AttemptedValue;

            var model = CategoryModel.GetCategory(id);
            model.FullName = fullName;
            model.CreatedDate = createdDate;
            return model;
        }
    }

    public class CategoryModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(Type modelType)
        {
            if (!typeof(CategoryModel).IsAssignableFrom(modelType))
                return null;

            return new CategoryModelBinder();
        }
    }

    public class CategoryModelBinderAttribute : CustomModelBinderAttribute
    {
        public override IModelBinder GetBinder()
        {
            return new CategoryModelBinder();
        }
    }
}