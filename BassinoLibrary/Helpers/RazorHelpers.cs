using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.UI.HtmlControls;
using BassinoLibrary.Attributes;

namespace BassinoLibrary.Helpers
{
    public static class RazorHelpers
    {

        public static MvcHtmlString PersonalSEditorCuitFor<TModel, TProperty>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> ex, string value = null)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("<div class='form-group'>");

            stringBuilder.Append(html.LabelFor(ex).ToHtmlString());

            if (EsRequerido(ex))
                stringBuilder.Append(GetRequiredSpan());

            var placeHolder = GetPlaceHolder(ex);

            if (string.IsNullOrEmpty(value))
                stringBuilder.Append(
                    html.TextBoxFor(ex, new { @class = "form-control ", @placeholder = placeHolder, autocomplete = "off" })
                        .ToHtmlString());
            else
                stringBuilder.Append(
                    html.TextBoxFor(ex,
                        new { @class = "form-control", @placeholder = placeHolder, autocomplete = "off", @Value = value })
                        .ToHtmlString());

            stringBuilder.Append(html.ValidationMessageFor(ex, null, new { @class = "help-block" }));

            stringBuilder.Append("</div>");

            return new MvcHtmlString(stringBuilder.ToString());
        }
        public static MvcHtmlString PersonalSEditorFor<TModel, TProperty>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> ex, string value = null)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("<div class='form-group'>");

            stringBuilder.Append(html.LabelFor(ex).ToHtmlString());

            if (EsRequerido(ex))
                stringBuilder.Append(GetRequiredSpan());

            var placeHolder = GetPlaceHolder(ex);

            if (string.IsNullOrEmpty(value))
                stringBuilder.Append(
                    html.TextBoxFor(ex, new {@class = "form-control", @placeholder = placeHolder, autocomplete = "off"})
                        .ToHtmlString());
            else
                stringBuilder.Append(
                    html.TextBoxFor(ex,
                        new {@class = "form-control", @placeholder = placeHolder, autocomplete = "off", @Value = value})
                        .ToHtmlString());

            stringBuilder.Append(html.ValidationMessageFor(ex, null, new {@class = "help-block"}));

            stringBuilder.Append("</div>");

            return new MvcHtmlString(stringBuilder.ToString());
        }
        public static MvcHtmlString PersonalSEditorNoGroupFor<TModel, TProperty>(this HtmlHelper<TModel> html,
           Expression<Func<TModel, TProperty>> ex, string value = null)
        {
            var stringBuilder = new StringBuilder();
            //stringBuilder.Append("<div class='form-group'>");

            //stringBuilder.Append(html.LabelFor(ex).ToHtmlString());

            if (EsRequerido(ex))
                stringBuilder.Append(GetRequiredSpan());

            var placeHolder = GetPlaceHolder(ex);

            if (string.IsNullOrEmpty(value))
                stringBuilder.Append(
                    html.TextBoxFor(ex, new { @class = "form-control", @placeholder = placeHolder, autocomplete = "off" })
                        .ToHtmlString());
            else
                stringBuilder.Append(
                    html.TextBoxFor(ex,
                        new { @class = "form-control", @placeholder = placeHolder, autocomplete = "off", @Value = value })
                        .ToHtmlString());

            stringBuilder.Append(html.ValidationMessageFor(ex, null, new { @class = "help-block" }));

           // stringBuilder.Append("</div>");

            return new MvcHtmlString(stringBuilder.ToString());
        }

        public static MvcHtmlString PersonalSEditorForFull<TModel, TProperty>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> ex, string placeHolder)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("<div class='form-group'>");

            stringBuilder.Append(html.LabelFor(ex).ToHtmlString());

            if (EsRequerido(ex))
                stringBuilder.Append(GetRequiredSpan());


            stringBuilder.Append(
                html.TextBoxFor(ex,
                    new
                    {
                        @style = "max-width:100%;",
                        @class = "form-control input-lg",
                        @placeholder = placeHolder,
                        autocomplete = "off"
                    }).ToHtmlString());
            stringBuilder.Append(html.ValidationMessageFor(ex, null, new {@class = "help-block"}));

            stringBuilder.Append("</div>");

            return new MvcHtmlString(stringBuilder.ToString());
        }

        public static MvcHtmlString PersonalSEditorForExtra<TModel, TProperty>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> ex, string placeHolder)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("<div class='form-group'>");

            stringBuilder.Append(html.LabelFor(ex).ToHtmlString());
            stringBuilder.Append("<span class='input-group-addon'>@</span>");
            if (EsRequerido(ex))
                stringBuilder.Append(GetRequiredSpan());


            stringBuilder.Append(
                html.TextBoxFor(ex,
                    new
                    {
                        @style = "max-width:100%;",
                        @class = "form-control input-lg",
                        @placeholder = placeHolder,
                        autocomplete = "off"
                    }).ToHtmlString());
            stringBuilder.Append(html.ValidationMessageFor(ex, null, new {@class = "help-block"}));

            stringBuilder.Append("</div>");

            return new MvcHtmlString(stringBuilder.ToString());
        }

        public static MvcHtmlString PersonalSEditorForFullPassword<TModel, TProperty>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> ex, string placeHolder)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("<div class='form-group'>");

            stringBuilder.Append(html.LabelFor(ex).ToHtmlString());

            if (EsRequerido(ex))
                stringBuilder.Append(GetRequiredSpan());


            stringBuilder.Append(
                html.PasswordFor(ex,
                    new
                    {
                        @style = "max-width:100%;",
                        @class = "form-control input-lg",
                        @placeholder = placeHolder,
                        autocomplete = "off"
                    }).ToHtmlString());
            stringBuilder.Append(html.ValidationMessageFor(ex, null, new {@class = "help-block"}));

            stringBuilder.Append("</div>");

            return new MvcHtmlString(stringBuilder.ToString());
        }

        public static MvcHtmlString PersonalTextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> ex)
        {
            return html.TextAreaFor(ex, 3);
        }

        public static MvcHtmlString PersonalTextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> ex, string placeholder, int rows, bool maxWidth = false)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("<div class='form-group'>");

            stringBuilder.Append(html.LabelFor(ex).ToHtmlString());

            if (EsRequerido(ex))
                stringBuilder.Append(GetRequiredSpan());

            var placeHolder = GetPlaceHolder(ex);
            if (maxWidth)
                stringBuilder.Append(
                    html.TextAreaFor(ex, new {@style = "max-width:100%;", @class = "form-control", placeholder, rows})
                        .ToHtmlString());
            else
                stringBuilder.Append(
                    html.TextAreaFor(ex, new {@class = "form-control", placeholder, rows}).ToHtmlString());


            stringBuilder.Append(html.ValidationMessageFor(ex, null, new {@class = "help-block"}));

            stringBuilder.Append("</div>");

            return new MvcHtmlString(stringBuilder.ToString());
        }

        public static MvcHtmlString PersonalDisplayFor<TModel, TProperty>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> ex)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("<div class='form-group'>");

            stringBuilder.Append(html.LabelFor(ex).ToHtmlString());
            stringBuilder.Append("<div><em>");
            stringBuilder.Append(html.DisplayFor(ex).ToHtmlString());
            stringBuilder.Append("</em></div>");

            stringBuilder.Append("</div>");

            return new MvcHtmlString(stringBuilder.ToString());
        }
        public static object GetValue(MemberExpression member)
        {
            var objectMember = Expression.Convert(member, typeof(object));

            var getterLambda = Expression.Lambda<Func<object>>(objectMember);

            var getter = getterLambda.Compile();

            return getter();
        }
        public static MvcHtmlString PersonalDisplayBoolFor<TModel, TProperty>(this HtmlHelper<TModel> html,
           Expression<Func<TModel, TProperty>> ex)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("<div class='form-group'>");
            var member =GetValue( (MemberExpression)ex.Body);

            stringBuilder.Append(html.LabelFor(ex).ToHtmlString());
            stringBuilder.Append("<div><em>");
            stringBuilder.Append(member.ToString());
            stringBuilder.Append("</em></div>");

            stringBuilder.Append("</div>");

            return new MvcHtmlString(stringBuilder.ToString());
        }
        public static MvcHtmlString PersonalDisplayTelefono<TModel>(this HtmlHelper<TModel> html, string property)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("<div class='form-group'>");

            stringBuilder.Append("<i class='glyphicon glyphicon-phone'></i> ");
            stringBuilder.Append(html.Label(property).ToHtmlString());
            stringBuilder.Append("<div><em>");
            stringBuilder.Append(html.Display(property).ToHtmlString());
            stringBuilder.Append("</em></div>");

            stringBuilder.Append("</div>");

            return new MvcHtmlString(stringBuilder.ToString());
        }

        public static MvcHtmlString PersonalDisplayUbicacion<TModel>(this HtmlHelper<TModel> html)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("<div class='form-group'>");

            stringBuilder.Append("<i class='glyphicon glyphicon-home'></i> ");
            stringBuilder.Append(html.Label("Ubicacion").ToHtmlString());
            stringBuilder.Append("<div><em>");
            stringBuilder.Append(html.Display("Domicilio").ToHtmlString());
            stringBuilder.Append("<br/>");
            stringBuilder.Append(html.Display("CodigoPostal").ToHtmlString());
            stringBuilder.Append("<br/>");
            stringBuilder.Append(html.Display("Ubicacion").ToHtmlString());
            stringBuilder.Append("</em></div>");

            stringBuilder.Append("</div>");

            return new MvcHtmlString(stringBuilder.ToString());
        }

        public static MvcHtmlString PersonalPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> ex)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("<div class='form-group'>");

            stringBuilder.Append(html.LabelFor(ex).ToHtmlString());

            if (EsRequerido(ex))
                stringBuilder.Append(GetRequiredSpan());

            var placeHolder = GetPlaceHolder(ex);

            stringBuilder.Append(
                html.PasswordFor(ex, new {@class = "form-control", @placeholder = placeHolder}).ToHtmlString());
            stringBuilder.Append(html.ValidationMessageFor(ex, null, new {@class = "help-block"}));

            stringBuilder.Append("</div>");

            return new MvcHtmlString(stringBuilder.ToString());
        }

        private static string GetRequiredSpan()
        {
            return "<span style=\"Color: red\">*</span>";
        }

        private static bool EsRequerido<TModel, TProperty>(Expression<Func<TModel, TProperty>> ex)
        {
            var property = GetLastPropertyInfoFromExpression(ex);

            var attributes = property.GetCustomAttributes(true).OfType<RequireHttpsAttribute>();

            return attributes.Any();
        }

        private static string GetPlaceHolder<TModel, TProperty>(Expression<Func<TModel, TProperty>> ex)
        {
            var property = GetLastPropertyInfoFromExpression(ex);

            var attributes = property.GetCustomAttributes(true).OfType<PlaceHolderAttribute>();

            if (attributes.Any())
                return attributes.First().Name;

            return "";
        }

        private static PropertyInfo GetLastPropertyInfoFromExpression<TModel, TProperty>(
            Expression<Func<TModel, TProperty>> ex)
        {
            PropertyInfo property = null;

            var htmlFieldName = ExpressionHelper.GetExpressionText(ex);
            var modelMetadata = typeof (TModel);

            var propertiesList = htmlFieldName.Split('.');
            for (var index = 0; index < propertiesList.Length; index++)
            {
                var propertyName = propertiesList[index];
                property = modelMetadata.GetProperties().First(x => x.Name == propertyName);

                if (index + 1 < propertiesList.Length)
                {
                    modelMetadata = property.PropertyType;
                }
            }

            return property;
        }
    }
}