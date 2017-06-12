using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using MvcApplication1.Controllers;

namespace MvcApplication1.Infrastructure.Helpers
{
    public static class Mvc4HtmlExtensions
    {

        #region Html5 textbox

        //1. verzió
        public static object TextBoxV1For<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            return System.Web.Mvc.Html.InputExtensions.TextBoxFor(htmlHelper, expression,
                htmlAttributes: new RouteValueDictionary() { { "placeholder", metadata.DisplayName } });
        }

        //2. verzió
        public static object TextBoxV2For<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string phtext;
            object phobject;
            if (metadata.AdditionalValues.TryGetValue("placeholder", out phobject))
                phtext = phobject.ToString();
            else
                phtext = metadata.DisplayName;
            return System.Web.Mvc.Html.InputExtensions.TextBoxFor(htmlHelper, expression,
                htmlAttributes: new RouteValueDictionary() { { "placeholder", phtext } });
        }

        //3. verzió
        public static object TextBoxV3For<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression)
        {
            return TextBoxV3For(htmlHelper, expression, null, null);
        }

        public static MvcHtmlString TextBoxV3For<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            string format, IDictionary<string, object> htmlAttributes)
        {
            //Metaadatok megszerzése
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            //Property path
            string name = ExpressionHelper.GetExpressionText(expression);
            string fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);

            //HTML tag építése
            TagBuilder tagBuilder = new TagBuilder("input");
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("type", HtmlHelper.GetInputTypeString(InputType.Text));
            tagBuilder.MergeAttribute("name", fullName, true);

            //Szöveg formázása
            string valueParameter = htmlHelper.FormatValue(metadata.Model, format);
            string attemptedValue = null;

            //post request adat
            ModelState modelState;
            if (htmlHelper.ViewData.ModelState.TryGetValue(fullName, out modelState)
                    && modelState.Value != null)
                attemptedValue = modelState.Value.ToString();

            //A textbox tartalmának beállítása
            tagBuilder.MergeAttribute("value", attemptedValue ?? valueParameter, true);
            //placeholder attribútum <- a főcél
            tagBuilder.MergeAttribute("placeholder", metadata.DisplayName);
            //id attribútum
            tagBuilder.GenerateId(fullName);

            //Validációs hibák stílusa
            if (modelState != null && modelState.Errors.Count > 0)
                tagBuilder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
            //Validációs attribútumok
            tagBuilder.MergeAttributes(htmlHelper.GetUnobtrusiveValidationAttributes(name, metadata));

            //HTML generálása
            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }
        #endregion

        /*
        #region modell property elérhető az egész útvonalon?
        
        public static bool IsAvailable<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> e)
        {
            TModel model = helper.ViewData.Model;
            var expressions = GetMemberExpressions(e.Body);
            
            //Expression path bejárása balról jobbra
            foreach (MemberExpression a in expressions.Reverse()) 
            {
                var propertyValue = GetPropertyValue<TModel>(model, a);
                //Ha a property érték nulla, akkor nem keresünk tovább a path-ban.
                if (propertyValue == null) return false; 
            }
            return true;
        }

        private static IEnumerable<MemberExpression> GetMemberExpressions(Expression e)
        {
            MemberExpression memberExpression;
            while ((memberExpression = e as MemberExpression) != null)
            {
                yield return memberExpression;
                e = memberExpression.Expression;
            }
        }

        private static object GetPropertyValue<T>(T instance, MemberExpression me)
        {
            object tobject = me.Expression.NodeType == ExpressionType.Parameter ?
                instance : GetPropertyValue<T>(instance, me.Expression as MemberExpression);
            return tobject.GetType().GetProperty(me.Member.Name).GetValue(tobject, null);
        }

        #endregion
        */

        #region FileUpload

public static MvcHtmlString FileUploadFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
    Expression<Func<TModel, TProperty>> expression)
{
    //Metaadatok megszerzése
    ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
    //Property path
    string name = ExpressionHelper.GetExpressionText(expression);
    string fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);

    //HTML tag építése
    TagBuilder tagBuilder = new TagBuilder("input");
    tagBuilder.MergeAttribute("type", "file");
    tagBuilder.MergeAttribute("name", fullName, true);

    //Saját FileUpload attribútum megszerzése
    object validationAttribute;
    if(metadata.AdditionalValues.TryGetValue(FileUploadValidationAttribute.FileUploadValidationName,out validationAttribute))
    {
        var fileAttribute = validationAttribute as FileUploadValidationAttribute;
        if(fileAttribute != null)
        {
            tagBuilder.MergeAttribute("accept", fileAttribute.UsableFileTypes);
            if(fileAttribute.Multiple)
                tagBuilder.MergeAttribute("multiple", "multiple");
        }
    }
    //HTML generálása
    return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.SelfClosing));
}
        #endregion

    }
}