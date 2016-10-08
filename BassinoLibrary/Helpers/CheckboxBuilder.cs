using System;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace BassinoLibrary.Helpers
{
    public class CheckboxBuilder<TModel> : UIBuilder
    {
        protected Expression<Func<TModel, bool>> Expression;
        protected HtmlHelper<TModel> Helper;

        public CheckboxBuilder(HtmlHelper<TModel> helper, Expression<Func<TModel, bool>> ex)
        {
            Helper = helper;
            Expression = ex;
        }

        public override string ToHtmlString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(@"<div class=""checkbox"">");
            stringBuilder.Append(@"<label>");

            var ckeckBox = Helper.CheckBoxFor(Expression);
            stringBuilder.Append(ckeckBox);

            stringBuilder.Append(Helper.LabelFor(Expression));

            stringBuilder.Append(@"</label>");
            stringBuilder.Append(@"</div>");

            return stringBuilder.ToString();
        }
    }
}