using System;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace BassinoLibrary.Helpers
{
    public class RadioButtonBuilder<TModel> : UIBuilder
    {
        protected string EventOnchange;
        protected Expression<Func<TModel, string>> Expression;
        protected HtmlHelper<TModel> Helper;
        protected string IdLabel;
        protected string NameLabel;
        protected string NameRadio;
        protected string ValueRadio;

        public RadioButtonBuilder(HtmlHelper<TModel> helper, Expression<Func<TModel, string>> ex)
        {
            Helper = helper;
            Expression = ex;
        }

        public RadioButtonBuilder<TModel> Nombre(string nameClass)
        {
            NameRadio = nameClass;
            return this;
        }

        public RadioButtonBuilder<TModel> NombreLabel(string nameLabel)
        {
            NameLabel = nameLabel;
            return this;
        }

        public RadioButtonBuilder<TModel> IddelLabel(string idLabel)
        {
            IdLabel = idLabel;
            return this;
        }

        public RadioButtonBuilder<TModel> Value(string valueRadio)
        {
            ValueRadio = valueRadio;
            return this;
        }

        public RadioButtonBuilder<TModel> OnChange(string eventOnchange)
        {
            EventOnchange = eventOnchange;
            return this;
        }

        public override string ToHtmlString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(@"<div class=""radio"">");
            stringBuilder.Append(@"<label>");

            var ckeckBox = Helper.RadioButtonFor(Expression, ValueRadio,
                new {Name = NameRadio, onchange = EventOnchange});
            stringBuilder.Append(ckeckBox);

            stringBuilder.Append(Helper.Label(NameLabel, new {id = IdLabel}));

            stringBuilder.Append(@"</label>");
            stringBuilder.Append(@"</div>");

            return stringBuilder.ToString();
        }
    }
}