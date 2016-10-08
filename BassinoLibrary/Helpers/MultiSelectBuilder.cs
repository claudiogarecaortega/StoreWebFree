using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace BassinoLibrary.Helpers
{
    public class MultiSelectBuilder<TModel, TProperty> : UIBuilder
    {
        protected string Action;
        protected string AditionalParametersFunction;
        protected string BindId;
        protected string Controller;
        protected IEnumerable Data;
        protected Expression<Func<TModel, TProperty>> Expression;
        protected bool HasLabel;
        protected bool HasValidationMessage;
        protected HtmlHelper<TModel> Helper;
        protected string ImputName;
        protected IEnumerable Selected;
        protected IEnumerable SelectedEdit;
        protected string TextProperty;
        protected string ValueProperty;

        public MultiSelectBuilder(HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> ex)
        {
            Helper = helper;
            Expression = ex;
        }

        public virtual MultiSelectBuilder<TModel, TProperty> Name(string name)
        {
            ImputName = name;
            return this;
        }

        public MultiSelectBuilder<TModel, TProperty> Values<TValue>(IEnumerable<TValue> values)
        {
            var items = new List<SelectListItem>();
            items.Add(new SelectListItem {Value = null, Text = "", Selected = true});
            var valueProp = typeof (TValue).GetProperty(ValueProperty);
            var textProp = typeof (TValue).GetProperty(TextProperty);
            Data =
                items.Union(
                    values.Select(
                        bla =>
                            new SelectListItem
                            {
                                Text = Convert.ToString(textProp.GetValue(bla)),
                                Value = Convert.ToString(valueProp.GetValue(bla))
                            }));

            return this;
        }

        public virtual MultiSelectBuilder<TModel, TProperty> Source(string action, string controller)
        {
            Action = action;
            Controller = controller;
            return this;
        }

        public virtual MultiSelectBuilder<TModel, TProperty> Bind(string bindId)
        {
            BindId = bindId;
            return this;
        }

        public virtual MultiSelectBuilder<TModel, TProperty> SelectedValue(IEnumerable value)
        {
            Selected = value;
            return this;
        }

        public virtual MultiSelectBuilder<TModel, TProperty> SelectedValueEdit<TValue>(IEnumerable<TValue> value)
        {
            SelectedEdit = value;
            return this;
        }

        public virtual MultiSelectBuilder<TModel, TProperty> ValueField<T>(Expression<Func<T, object>> ex)
        {
            ValueProperty = GetPropertyName(ex);
            return this;
        }

        public virtual MultiSelectBuilder<TModel, TProperty> ValueField(string value)
        {
            ValueProperty = value;
            return this;
        }

        public virtual MultiSelectBuilder<TModel, TProperty> TextField<T>(Expression<Func<T, object>> ex)
        {
            TextProperty = ((MemberExpression) ex.Body).Member.Name;
            return this;
        }

        public virtual MultiSelectBuilder<TModel, TProperty> TextField(string text)
        {
            TextProperty = text;
            return this;
        }

        public virtual MultiSelectBuilder<TModel, TProperty> Label()
        {
            HasLabel = true;
            return this;
        }

        public virtual MultiSelectBuilder<TModel, TProperty> ValidationMessage()
        {
            HasValidationMessage = true;
            return this;
        }

        public virtual MultiSelectBuilder<TModel, TProperty> AditionalParameters(string functionName)
        {
            AditionalParametersFunction = functionName + "()";
            return this;
        }

        public override string ToString()
        {
            return ToHtmlString();
        }

        public override string ToHtmlString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("<div class=\"form-group\">");

            if (HasLabel)
                stringBuilder.Append(Helper.LabelFor(Expression));

            var data = Data ?? new List<SelectListItem>();


            var combo = Helper.ListBoxFor(Expression, new MultiSelectList(data, "Value", "Text", Selected),
                new {@class = "form-control"});

            stringBuilder.Append(combo);

            if (Data == null && BindId == null)
                stringBuilder.Append(GetAjax());

            if (BindId != null)
                stringBuilder.Append(GetBind());

            if (HasValidationMessage)
                stringBuilder.Append(Helper.ValidationMessageFor(Expression, null, new {@class = "help-block"}));

            stringBuilder.Append("</div>");

            return stringBuilder.ToString();
        }

        protected virtual string GetText()
        {
            return TextProperty ?? "Descripcion";
        }

        protected virtual string GetValue()
        {
            return ValueProperty ?? "Id";
        }

        private string GetBind()
        {
            var script = string.Format(@"<script>
                                            $(document).ready(function (){{
                                                $(""#{0}"").change(function (){{
                                                    {1}
                                                }});
                                            }});
                                        </script>", BindId, GetAjaxScript());
            return script;
        }

        protected virtual string GetAjax()
        {
            return string.Format("<script>{0}</script>", GetAjaxScript());
        }

        protected virtual string GetAjaxScript()
        {
            var expressionInfo = ModelMetadata.FromLambdaExpression(Expression, Helper.ViewData);

            return string.Format(@"$.ajax({{
                            type: ""GET"",
                            url: ""{0}{1}/{2}"",
                            data: eval({3}),
                            dataType: 'json',
                            success: function (data) {{
                                var ddl = $('#{4}');
                                ddl.append(
                                    $('<option/>', {{
                                        value: null,
                                        html: """"
                                    }}));
                                $.each(data, function (index, value) {{
                                    ddl.append(
                                        $('<option/>', {{
                                            value: value.{5},
                                            html: value.{6}
                                        }}));
                                }});
                            }}
                        }});", UriHelper.Instance.GetUrlAbsoluta(), Controller, Action, AditionalParametersFunction,
                expressionInfo.PropertyName, GetValue(), GetText());
        }

        protected virtual string GetPropertyName<TValue>(Expression<Func<TModel, TValue>> ex)
        {
            var expressionInfo = ModelMetadata.FromLambdaExpression(ex, Helper.ViewData);
            return expressionInfo.PropertyName;
        }
    }
}