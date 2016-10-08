using System;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using BassinoLibrary.Resource;

namespace BassinoLibrary.Helpers
{
    public class DateTimePickerBuilder<TModel> : UIBuilder
    {
        protected bool changeMonth;
        protected bool changeYear;
        protected Expression<Func<TModel, object>> Expression;
        protected bool HasLabel;
        protected HtmlHelper<TModel> Helper;
        protected bool Hidden;
        protected string IdDiv;
        protected string IdHTML;
        protected bool IsInputGroup;
        protected string Name;
        protected bool pickMonthYear;
        protected string PlaceHolderString;
        protected string Size = "";
        protected string Function;

        public DateTimePickerBuilder(HtmlHelper<TModel> helper, Expression<Func<TModel, object>> ex)
        {
            Helper = helper;
            Expression = ex;
        }

        public DateTimePickerBuilder(HtmlHelper<TModel> helper, string name)
        {
            Helper = helper;
            Name = name;
        }

        public virtual DateTimePickerBuilder<TModel> Label()
        {
            HasLabel = true;
            return this;
        }

        public virtual DateTimePickerBuilder<TModel> SetID(string Id)
        {
            IdHTML = Id;
            return this;
        }

        public virtual DateTimePickerBuilder<TModel> SetIDDiv(string Id)
        {
            IdDiv = Id;
            return this;
        }

        public virtual DateTimePickerBuilder<TModel> Hide(bool hidden)
        {
            Hidden = hidden;
            return this;
        }

        public virtual DateTimePickerBuilder<TModel> ChangeMonth()
        {
            changeMonth = true;
            return this;
        }

        public virtual DateTimePickerBuilder<TModel> ChangeYear()
        {
            changeYear = true;
            return this;
        }

        public virtual DateTimePickerBuilder<TModel> PickMonthYear()
        {
            pickMonthYear = true;
            ChangeMonth();
            ChangeYear();

            return this;
        }

        public virtual DateTimePickerBuilder<TModel> SetFunctionn(string function)
        {
            this.Function = function;
            return this;
        }
    
        public virtual DateTimePickerBuilder<TModel> Small()
        {
            Size = "input-sm";
            return this;
        }

        public virtual DateTimePickerBuilder<TModel> InputGroup()
        {
            IsInputGroup = true;
            return this;
        }

        public virtual DateTimePickerBuilder<TModel> PlaceHolder(string placeHolder)
        {
            PlaceHolderString = placeHolder;
            return this;
        }

        public override string ToHtmlString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("<div id=\"{0}\" {1} class=\"form-group\">",
                string.IsNullOrEmpty(IdDiv) ? "" : IdDiv, Hidden ? "hidden" : "");

            if (HasLabel)
                stringBuilder.Append(Helper.LabelFor(Expression));

            if (IsInputGroup)
                stringBuilder.Append("<div class=\"input-group\">");

            if (Expression != null)
                stringBuilder.Append(Helper.TextBoxFor(Expression,
                    new {@id = Id(), @class = "datepicker form-control " + Size, placeholder = PlaceHolderString}));
            else
                stringBuilder.Append(Helper.TextBox(Name, null,
                    new {@id = Id(), @class = "datepicker form-control " + Size, placeholder = PlaceHolderString}));

            if (IsInputGroup)
                stringBuilder.Append("<span class=\"input-group-addon glyphicon glyphicon-calendar\"></span></div>");

            stringBuilder.Append("</div>");

            stringBuilder.Append(GetJs());

            return stringBuilder.ToString();
        }

        protected virtual string GetJs()
        {
            var opciones = new StringBuilder();

            if (changeMonth) opciones.Append("changeMonth: true,");
            //if (changeMonth) opciones.Append("defaultDate: true,");
            //if (changeMonth) opciones.Append("defaultDate: true,");
            if (changeYear) opciones.Append("changeYear: true,");

            opciones.Append(pickMonthYear ? "dateFormat: \"mm/yy\"," : "dateFormat: \"dd/mm/yy\",");

            var ocultarDias = "";
            if (pickMonthYear)
            {
                opciones.Append(GetMonthYearOnCloseFunction());
                ocultarDias = OcultarDias();
            }
            if (this.Function != null)
            {
                return string.Format(@"	<script>
										$(""#{0}"").datepicker({{ yearRange: ""-150:+0"", {1} 
											{2}, {3},
                                           {5}
										}});
									</script>
									{4}",
                    Id(), opciones, GetDaysArrayOption(), GetMonthsArrayOption(), ocultarDias, OnSelect());
            }
            return string.Format(@"	<script>
										$(""#{0}"").datepicker({{ yearRange: ""-150:+0"", {1} 
											{2}, {3},
                                           
										}});
									</script>
									{4}",
                     Id(), opciones, GetDaysArrayOption(), GetMonthsArrayOption(), ocultarDias);
        }

        private string Id()
        {
            if (!string.IsNullOrEmpty(IdHTML))
                return IdHTML;

            if (Expression != null)
                return GetPropertyName(Expression);

            return Name;
        }

        private string GetMonthYearOnCloseFunction()
        {
            return @"onClose: function(dateText, inst) {  
									var month = $(""#ui-datepicker-div .ui-datepicker-month :selected"").val(); 
									var year = $(""#ui-datepicker-div .ui-datepicker-year :selected"").val(); 
									$(this).val($.datepicker.formatDate('mm/yy', new Date(year, month, 1)));
								  },";
        }

        private string OnSelect()
        {
            if(this.Function!=null)
            return string.Format(@"onSelect:function(){{"+this.Function+"()}}");

            return "";
        }

        private string OcultarDias()
        {
            return string.Format(@"<script>
											$(""#{0}"").focus(function () {{
												$("".ui-datepicker-calendar"").hide();
											}});
										  </script>", IdHTML);
        }

        private string GetDaysArrayOption()
        {
            return string.Format(@"dayNamesMin: [ ""{0}"", ""{1}"", ""{2}"", ""{3}"", ""{4}"", ""{5}"", ""{6}"" ]",
                Resources.SundatShort, Resources.MondayShort, Resources.TuesdayShort, Resources.WednesdayShort,
                Resources.ThursdayShort, Resources.FridayShort, Resources.SaturdayShort);
        }

        private string GetMonthsArrayOption()
        {
            //January, February, March, April, May, June, August, September, October, November, December
            return
                string.Format(
                    @"monthNames: [ ""{0}"", ""{1}"", ""{2}"", ""{3}"", ""{4}"", ""{5}"", ""{6}"", ""{7}"", ""{8}"", ""{9}"", ""{10}"", ""{11}"" ]",
                    Resources.January, Resources.February, Resources.March, Resources.April, Resources.May,
                    Resources.June, Resources.July, Resources.August, Resources.September, Resources.October,
                    Resources.November, Resources.December);
        }
    }
}