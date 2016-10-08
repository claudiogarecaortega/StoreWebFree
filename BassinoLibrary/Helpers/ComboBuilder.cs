using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using BassinoLibrary.Utils;

namespace BassinoLibrary.Helpers
{
    public class ComboBuilder<TModel, TProperty> : UIBuilder
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
        protected object Selected;
        protected object SelectedId;
        protected string SelectedProperty;
        protected string TextProperty;
        protected string ValueProperty;

        public ComboBuilder(HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> ex)
        {
            Helper = helper;
            Expression = ex;
        }

        public virtual ComboBuilder<TModel, TProperty> Name(string name)
        {
            ImputName = name;
            return this;
        }

        public ComboBuilder<TModel, TProperty> Values(List<SelectListItem> values)
        {
            var items = new List<SelectListItem>();
            items.Add(new SelectListItem {Value = null, Text = ""});

            Data = items.Union(values);
            return this;
        }

        public ComboBuilder<TModel, TProperty> Values<TValue>(IEnumerable<TValue> values)
        {
            var items = new List<SelectListItem> {new SelectListItem {Value = null, Text = ""}};

            var valueProp = typeof (TValue).GetProperty(ValueProperty);
            var textProp = typeof (TValue).GetProperty(TextProperty);

            Data = items.Union(values.Select(item => new SelectListItem
            {
                Selected = (Convert.ToString(SelectedId) == Convert.ToString(valueProp.GetValue(item))),
                Text = Convert.ToString(textProp.GetValue(item)),
                Value = Convert.ToString(valueProp.GetValue(item))
            }));
            return this;
        }

        public ComboBuilder<TModel, TProperty> Values(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            var valores = Enum.GetValues(enumType);

            var items = new List<SelectListItem>();
            items.Add(new SelectListItem {Value = null, Text = ""});

            foreach (var valor in valores)
            {
                var descripcion = ((Enum) valor).GetDescriptions();
                var value = ((Enum) valor).ToString("d");

                items.Add(new SelectListItem {Text = descripcion, Value = value});
            }

            Data = items;
            return this;
        }

        public virtual ComboBuilder<TModel, TProperty> Source(string action, string controller)
        {
            Action = action;
            Controller = controller;
            return this;
        }

        public virtual ComboBuilder<TModel, TProperty> Bind(string bindId)
        {
            BindId = bindId;
            return this;
        }

        public virtual ComboBuilder<TModel, TProperty> SelectedValue(object value)
        {
            Selected = value;
            return this;
        }

        public virtual ComboBuilder<TModel, TProperty> SelectedValueId(int value)
        {
            SelectedId = value;
            return this;
        }

        public virtual ComboBuilder<TModel, TProperty> ValueField<T>(Expression<Func<T, object>> ex)
        {
            ValueProperty = GetPropertyName(ex);
            return this;
        }

        public virtual ComboBuilder<TModel, TProperty> ValueField(string value)
        {
            ValueProperty = value;
            return this;
        }

        public virtual ComboBuilder<TModel, TProperty> TextField<T>(Expression<Func<T, object>> ex)
        {
            
            if (ex.Body is MemberExpression)
            {
                TextProperty=((MemberExpression)ex.Body).Member.Name;
            }
            else if(ex.Body  is UnaryExpression)
            {
                var op = ((UnaryExpression)ex.Body).Operand;
                TextProperty = ((UnaryExpression)op).Method.Name;
            }         
            else if (ex.Body is MethodCallExpression)
            {
                TextProperty = ((MethodCallExpression)ex.Body).Method.Name;
               // TextProperty = ((MemberExpression)op).Member.Name;
            }
           // TextProperty = ReflectionUtils.Instance.GetPropertyName(ex);
            //TextProperty = ((MemberExpression) ex.Body).Member.Name;
            return this;
        }

        public virtual ComboBuilder<TModel, TProperty> TextField(string text)
        {
            TextProperty = text;
            return this;
        }

        public virtual ComboBuilder<TModel, TProperty> Label()
        {
            HasLabel = true;
            return this;
        }

        public virtual ComboBuilder<TModel, TProperty> ValidationMessage()
        {
            HasValidationMessage = true;
            return this;
        }

        public virtual ComboBuilder<TModel, TProperty> AditionalParameters(string functionName)
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

            var combo = Helper.DropDownListFor(Expression, new SelectList(data, "Value", "Text", SelectedId),
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
            var expressionInfo = ModelMetadata.FromLambdaExpression(Expression, Helper.ViewData);
            var script = string.Format(@"<script>
                                            $(document).ready(function (){{
                                                $(""#{0}"").on('change',function (){{
                                                    if($(""#{0}"").val()?1:0){{
                                                        {1}
                                                    }}
                                                    else{{
                                                        $('#{2}').empty();
                                                    }}
                                                }});
                                            }});
                                        </script>", BindId, GetAjaxScript(), expressionInfo.PropertyName);
            return script;
        }

        protected virtual string GetAjax()
        {
            return string.Format("<script>{0}</script>", GetAjaxScript());
        }

        protected virtual string GetAjaxScript()
        {
            var expressionInfo = ModelMetadata.FromLambdaExpression(Expression, Helper.ViewData);

            return string.Format(@"
                            
                                $.ajax({{
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
                                }})    
                            ;", UriHelper.Instance.GetUrlAbsoluta(), Controller, Action, AditionalParametersFunction,
                expressionInfo.PropertyName, GetValue(), GetText());
        }

        protected virtual string GetPropertyName<TValue>(Expression<Func<TModel, TValue>> ex)
        {
            var expressionInfo = ModelMetadata.FromLambdaExpression(ex, Helper.ViewData);
            return expressionInfo.PropertyName;
        }
    }
}