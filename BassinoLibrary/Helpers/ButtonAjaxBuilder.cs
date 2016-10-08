using System;
using System.Text;
using System.Web.Mvc;

namespace BassinoLibrary.Helpers
{
    public class ButtonAjaxBuilder<TModel> : UIBuilder
    {
        protected string FunctionAlways = "";
        protected string FunctionBeforeCall = "";
        protected string FunctionGetParametersFrom = "";
        protected string FunctionOnDone = "";
        protected string FunctionOnFail = "";
        protected string IdHtml;
        protected string Label;
        protected TypeIcon typeIcon = Helpers.TypeIcon.Search;
        protected string Url;

        public ButtonAjaxBuilder(HtmlHelper<TModel> helper, string idHtml)
        {
            IdHtml = idHtml;
        }

        public virtual ButtonAjaxBuilder<TModel> ParametersFrom(string functionJS)
        {
            FunctionGetParametersFrom = functionJS;
            return this;
        }

        public virtual ButtonAjaxBuilder<TModel> SetLabel(string label)
        {
            Label = label;
            return this;
        }

        public virtual ButtonAjaxBuilder<TModel> SetUrl(string url)
        {
            Url = url;
            return this;
        }

        public virtual ButtonAjaxBuilder<TModel> BeforeCall(string functionJS)
        {
            FunctionBeforeCall = functionJS;
            return this;
        }

        public virtual ButtonAjaxBuilder<TModel> OnDone(string functionJS)
        {
            FunctionOnDone = functionJS;
            return this;
        }

        public virtual ButtonAjaxBuilder<TModel> OnFail(string functionJS)
        {
            FunctionOnFail = functionJS;
            return this;
        }

        public virtual ButtonAjaxBuilder<TModel> AlwaysDo(string functionJS)
        {
            FunctionAlways = functionJS;
            return this;
        }

        public virtual ButtonAjaxBuilder<TModel> TypeIcon(TypeIcon type)
        {
            typeIcon = type;
            return this;
        }

        public override string ToHtmlString()
        {
            var stringBuilder = new StringBuilder();

            //stringBuilder.AppendFormat("<input type=\"button\" id=\"" + IdHtml + "\" class=\"btn btn-info\" value=\"{0}\" style=\"width:150px;\"/>", Label);
            stringBuilder.AppendFormat(
                "<button type=\"button\" class=\"btn btn-info\" id=\"" + IdHtml +
                "\"><span class=\"glyphicon glyphicon-search\"></span> {0}</button>", Label);

            stringBuilder.Append(GetJs());

            return stringBuilder.ToString();
        }

        protected virtual string GetJs()
        {
            if (string.IsNullOrEmpty(FunctionOnDone))
                throw new Exception("Se debe definir la funcion OnDone");
            if (string.IsNullOrEmpty(Url))
                throw new Exception("Se debe definir la Url del Ajax");


            return string.Format(@"<script>
										$(""#{0}"").click(function(event){{ 
											$(this).html(""Buscando..."");
											$(this).attr(""disabled"", ""disabled"");
											
											{1}
											$.ajax({{
												type: ""POST"",
												url: ""{2}"",
												{3}											
												dataType: 'json'
											}})
											.done(function(data){{
												{4}(data);					
											}})
											.fail(function(jqXHR, textStatus, errorThrown){{
												{5}
											}})
											.always(function(){{
												$(""#{0}"").html(""<span class='glyphicon glyphicon-search'></span> {7}"");
												$(""#{0}"").removeAttr(""disabled"");
												{6}
											}})
											
										}});
									</script>", IdHtml,
                (!string.IsNullOrEmpty(FunctionBeforeCall) ? FunctionBeforeCall + "();" : ""),
                Url,
                (!string.IsNullOrEmpty(FunctionGetParametersFrom) ? "data:" + FunctionGetParametersFrom + "()," : ""),
                FunctionOnDone,
                (!string.IsNullOrEmpty(FunctionOnFail) ? FunctionOnFail + "();" : ""),
                (!string.IsNullOrEmpty(FunctionAlways) ? FunctionAlways + "();" : ""),
                Label);
        }
    }

    public enum TypeIcon
    {
        Search,
        Add,
        Delete
    }
}