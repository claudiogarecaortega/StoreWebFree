using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace BassinoLibrary.Helpers
{
    public class ButtonBuilder<TModel> : UIBuilder
    {
        protected string ActionName;
        protected string ButtonBlock = "";
        protected string ButtonLabel;
        protected string Color = "btn-default";
        protected string ControllerName;
        protected string FunctionOnClick = "function(){}";
        protected string FunctionOnComplete = "function(){}";
        protected HtmlHelper<TModel> Helper;
        protected string IconName;
        protected string IdHtml;
        protected string ModalId;
        protected object Routes;
        protected string Size = "";

        public ButtonBuilder(HtmlHelper<TModel> helper, string idHtml)
        {
            Helper = helper;
            IdHtml = idHtml;
        }

        public virtual ButtonBuilder<TModel> OnClick(string functionJS)
        {
            FunctionOnClick = functionJS;
            return this;
        }

        public virtual ButtonBuilder<TModel> Label(string label)
        {
            ButtonLabel = label;
            return this;
        }

        public virtual ButtonBuilder<TModel> Block()
        {
            Size = "btn-block";
            return this;
        }

        public virtual ButtonBuilder<TModel> Icon(string iconName)
        {
            IconName = iconName;
            return this;
        }

        public virtual ButtonBuilder<TModel> OpenModal(string modalId, string actionName, string controllerName,
            object routes)
        {
            ModalId = modalId;
            ActionName = actionName;
            ControllerName = controllerName;
            Routes = routes;
            return this;
        }

        public override string ToHtmlString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("<button type=\"button\" class=\"btn {2} {3} {4}\" {0} {5}>{6} {1}</button>",
                SetId(), ButtonLabel, Color, Size, ButtonBlock, ModalOpener(), SetIcon());

            stringBuilder.Append(GetJs());

            return stringBuilder.ToString();
        }

        private string SetIcon()
        {
            if (string.IsNullOrEmpty(IconName))
                return string.Empty;

            return string.Format("<span class=\"glyphicon glyphicon-{0}\"></span>", IconName);
        }

        private string SetId()
        {
            if (string.IsNullOrEmpty(IdHtml))
                return string.Empty;

            return string.Format("id='{0}'", IdHtml);
        }

        private string ModalOpener()
        {
            if (string.IsNullOrEmpty(ModalId))
                return string.Empty;

            return string.Format("data-target='#{0}' data-toggle='modal' href='{1}'", ModalId, GetUrl());
        }

        private string GetUrl()
        {
            return UrlHelper.GenerateUrl(null, ActionName, ControllerName, null, null, null,
                new RouteValueDictionary(Routes), Helper.RouteCollection, Helper.ViewContext.RequestContext, true);
        }

        protected virtual string GetJs()
        {
            if (string.IsNullOrEmpty(FunctionOnClick))
                return string.Empty;

            return string.Format(@"<script>
										$(""#{0}"").click({1});
									</script>", IdHtml, FunctionOnClick);
        }

        #region Color

        public virtual ButtonBuilder<TModel> Primary()
        {
            Color = "btn-primary";
            return this;
        }

        public virtual ButtonBuilder<TModel> Success()
        {
            Color = "btn-success";
            return this;
        }

        public virtual ButtonBuilder<TModel> Info()
        {
            Color = "btn-info";
            return this;
        }

        public virtual ButtonBuilder<TModel> Warning()
        {
            Color = "btn-warning";
            return this;
        }

        public virtual ButtonBuilder<TModel> Danger()
        {
            Color = "btn-danger";
            return this;
        }

        #endregion

        #region Size

        public virtual ButtonBuilder<TModel> Large()
        {
            Size = "btn-lg";
            return this;
        }

        public virtual ButtonBuilder<TModel> Small()
        {
            Size = "btn-sm";
            return this;
        }

        public virtual ButtonBuilder<TModel> ExtraSmall()
        {
            Size = "btn-xs";
            return this;
        }

        #endregion
    }
}