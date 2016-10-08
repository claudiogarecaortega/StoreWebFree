using System;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace BassinoLibrary.Helpers
{
    public class ColorPickerBuilder<TModel> : UIBuilder
    {
        protected Expression<Func<TModel, object>> Expression;
        protected bool HasLabel;
        protected HtmlHelper<TModel> Helper;

        public ColorPickerBuilder(HtmlHelper<TModel> helper, Expression<Func<TModel, object>> ex)
        {
            Helper = helper;
            Expression = ex;
        }

        public virtual ColorPickerBuilder<TModel> Label()
        {
            HasLabel = true;
            return this;
        }

        public override string ToHtmlString()
        {
            var stringBuilder = new StringBuilder();

            if (HasLabel)
                stringBuilder.Append(Helper.LabelFor(Expression));

            var valor = Helper.ViewData.Model != null
                ? Expression.Compile().Invoke(Helper.ViewData.Model).ToString()
                : "";

            stringBuilder.AppendFormat(
                "<input type=\"text\" value=\"{0}\" name=\"Color\" class=\"pick-a-color form-control\">", valor);

            stringBuilder.Append(GetJs());

            return stringBuilder.ToString();
        }

        protected virtual string GetJs()
        {
            return string.Format(@"<script>
                                    $("".pick-a-color"").pickAColor({{
                                    showSavedColors: false,
                                    showAdvanced: false
                                    }});
                                </script>");
        }
    }
}