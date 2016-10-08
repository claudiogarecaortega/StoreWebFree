using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Kendo.Mvc.Extensions;

namespace BassinoLibrary.Helpers
{
    public class CheckboxListBuilder<TModel, TProperty> : UIBuilder
    {
        protected string cositaloca;
        protected IEnumerable Data;
        protected Expression<Func<TModel, TProperty>> Expression;
        protected HtmlHelper<TModel> Helper;
        protected int[] SelectedValues;
        protected bool ShowAvaliables;
        protected string TextProperty;
        protected string ValueProperty;

        public CheckboxListBuilder(HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> ex)
        {
            Helper = helper;
            Expression = ex;
        }

        public CheckboxListBuilder<TModel, TProperty> Values<TValue>(IEnumerable<TValue> values)
        {
            Data = values;
            return this;
        }

        public CheckboxListBuilder<TModel, TProperty> SelectedItems(int[] selectevalues)
        {
            SelectedValues = selectevalues;
            return this;
        }

        public CheckboxListBuilder<TModel, TProperty> ShowAvaliable(bool avalible)
        {
            ShowAvaliables = avalible;
            return this;
        }

        public CheckboxListBuilder<TModel, TProperty> Idmodelo(string id)
        {
            cositaloca = "_" + id;
            return this;
        }

        public virtual CheckboxListBuilder<TModel, TProperty> ValueField<T>(Expression<Func<T, object>> ex)
        {
            ValueProperty = GetPropertyName(ex);
            return this;
        }

        public virtual CheckboxListBuilder<TModel, TProperty> TextField<T>(Expression<Func<T, object>> ex)
        {
            TextProperty = ((MemberExpression) ex.Body).Member.Name;
            return this;
        }

        public override string ToHtmlString()
        {
            var stringBuilder = new StringBuilder();
            var contador = 0;
            var cantidad = Data.AsQueryable().Count()/2;
            stringBuilder.Append(@"<div class='row'><div class='col-md-12'><div class='row'><div class='col-md-6'>");
            foreach (var dato in Data)
            {
                if (contador == cantidad)
                {
                    stringBuilder.Append(@"</div>");
                    stringBuilder.Append(@"<div class='col-md-6'>");
                    contador = 0;
                }


                var valueProp = dato.GetType().GetProperty(ValueProperty);
                var textProp = dato.GetType().GetProperty(TextProperty);

                var idvalor = Convert.ToString(valueProp.GetValue(dato));
                var text = Convert.ToString(textProp.GetValue(dato));


                if (SelectedValues.Any(p => p == Convert.ToInt32(idvalor)))
                {
                    stringBuilder.Append(@"<div  class=""checkbox"">");
                    stringBuilder.Append(@"<label>");
                    var ckeckBox = Helper.CheckBox("" + cositaloca + "", new
                    {
                        value = idvalor,
                        @checked = "true"
                    });
                    stringBuilder.Append(ckeckBox);
                    stringBuilder.Append(Helper.Label(text));
                    stringBuilder.Append(@"</label>");
                    stringBuilder.Append(@"</div>");
                }
                else
                {
                    if (ShowAvaliables)
                    {
                        stringBuilder.Append(@"<div  class=""checkbox"">");
                        stringBuilder.Append(@"<label>");


                        var ckeckBox = Helper.CheckBox("" + cositaloca + "", new
                        {
                            value = idvalor
                        });
                        stringBuilder.Append(ckeckBox);
                        stringBuilder.Append(Helper.Label(text));
                        stringBuilder.Append(@"</label>");
                        stringBuilder.Append(@"</div>");
                    }
                }

                contador++;
            }
            stringBuilder.Append(@"</div>");

            return stringBuilder.ToString();
        }
    }
}