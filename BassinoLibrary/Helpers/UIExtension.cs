using System;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using BassinoLibrary.Resource;

namespace BassinoLibrary.Helpers
{
    public static class UIExtension
    {
        public static KendoWidgetFactory<T> KendoGrid<T>(this HtmlHelper<T> helper)
        {
            return new KendoWidgetFactory<T>(helper);
        }

        public static KendoWidgetFactory<T> KendoMultiSelect<T>(this HtmlHelper<T> helper)
        {
            return new KendoWidgetFactory<T>(helper);
        }

        public static ComboBuilder<TModel, TProperty> ComboFor<TModel, TProperty>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> ex)
        {
            return new ComboBuilder<TModel, TProperty>(helper, ex);
        }

        public static MultiSelectBuilder<TModel, TProperty> MultiSelectFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> ex)
        {
            return new MultiSelectBuilder<TModel, TProperty>(helper, ex);
        }

        public static AutocompleteBuilder<TModel, TId> AutocompleteFor<TModel, TId>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, object>> ex)
        {
            return new AutocompleteBuilder<TModel, TId>(helper, ex);
        }

        public static ColorPickerBuilder<TModel> ColorPicker<TModel>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, object>> ex)
        {
            return new ColorPickerBuilder<TModel>(helper, ex);
        }

        public static DateTimePickerBuilder<TModel> DateTimePicker<TModel>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, object>> ex)
        {
            return new DateTimePickerBuilder<TModel>(helper, ex);
        }

        public static DateTimePickerBuilder<TModel> DateTimePicker<TModel>(this HtmlHelper<TModel> helper, string name)
        {
            return new DateTimePickerBuilder<TModel>(helper, name);
        }

        public static ButtonBuilder<TModel> ButtonPersonal<TModel>(this HtmlHelper<TModel> helper, string idHtml)
        {
            return new ButtonBuilder<TModel>(helper, idHtml);
        }

        public static ButtonAjaxBuilder<TModel> ButtonAjax<TModel>(this HtmlHelper<TModel> helper, string idHtml)
        {
            return new ButtonAjaxBuilder<TModel>(helper, idHtml);
        }

        public static CheckboxBuilder<TModel> CheckBoxPersonal<TModel>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, bool>> ex)
        {
            return new CheckboxBuilder<TModel>(helper, ex);
        }

        public static RadioButtonBuilder<TModel> RadioButtonPersonal<TModel>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, string>> ex)
        {
            return new RadioButtonBuilder<TModel>(helper, ex);
        }

        public static CheckboxListBuilder<TModel, TProperty> CkechBoxListPersonal<TModel, TProperty>(
            this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> ex)
        {
            return new CheckboxListBuilder<TModel, TProperty>(helper, ex);
        }

        public static MvcHtmlString BotonAperturaModalCreate<TModel>(this HtmlHelper<TModel> helper, string actionName,
            Type controllerType, string modalId, object routeValues)
        {
            var html = string.Empty;


            html = helper.ButtonPersonal("")
                .Label(Resources.New)
                .OpenModal(modalId, actionName, controllerType.Name.Replace("Controller", ""), routeValues)
                .ToHtmlString();


            return new MvcHtmlString(html);
        }

        public static MvcHtmlString BotonAperturaModalCreateNombre<TModel>(this HtmlHelper<TModel> helper,
            string actionName, Type controllerType, string modalId, object routeValues, string texto)
        {
            var html = string.Empty;


            html = helper.ButtonPersonal("")
                .Label(texto)
                .OpenModal(modalId, actionName, controllerType.Name.Replace("Controller", ""), routeValues)
                .ToHtmlString();


            return new MvcHtmlString(html);
        }

        public static MvcHtmlString SeccionNotificacion(this HtmlHelper helper)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("<div id=\"areaNotificacion\" class=\"notifiaction-container\">");
            stringBuilder.Append("<div id=\"mensajeNotificacion\" class=\"notification\">");
            stringBuilder.Append("</div>");
            stringBuilder.Append("</div>");

            stringBuilder.Append("<div id=\"areaError\" class=\"notifiaction-container\">");
            stringBuilder.Append("<div id=\"mensajeError\" class=\"notificacionError\">");
            stringBuilder.Append("</div>");
            stringBuilder.Append("</div>");


            stringBuilder.Append("<script>");
            stringBuilder.Append("function mostrarNotificacion(mensaje) {");

            stringBuilder.Append("$(\"#modal\").modal('hide');");
            stringBuilder.Append("$(\"#modalValidations\").modal('hide');");
            stringBuilder.Append("$(\"#modalValidationsErrors\").modal('hide');");
            stringBuilder.Append(
                "var alert = '<div class=\"alert alert-success fade in\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-hidden=\"true\">×</button><strong>Ok!</strong> ' + mensaje + '.</div>';");
            stringBuilder.Append("$(\"#mensajeNotificacion\").html(alert);");
            stringBuilder.Append("$(\"#areaNotificacion\").show(0).delay(3000).hide(0);");
            stringBuilder.Append("$(\".alert\").alert();");
            stringBuilder.Append("}");
            stringBuilder.Append("function mostrarNotificacionReload(mensaje) {");

            stringBuilder.Append("$(\"#modal\").modal('hide');");
            stringBuilder.Append("$(\"#modalValidations\").modal('hide');");
            stringBuilder.Append("$(\"#modalValidationsErrors\").modal('hide');");
            stringBuilder.Append("SaveLogin();");
            stringBuilder.Append(
                "var alert = '<div class=\"alert alert-success fade in\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-hidden=\"true\">×</button><strong>Ok!</strong> ' + mensaje + '.</div>';");
            stringBuilder.Append("$(\"#alertDiv\").html(alert);");
            stringBuilder.Append("$(\"#alertDiv\").show(0).delay(3000).hide(0);");
            stringBuilder.Append("$(\".alert\").alert();");
            stringBuilder.Append(" location.reload();");

            stringBuilder.Append("}");

            stringBuilder.Append("function mostrarNotificacionRegistroUsuario(mensaje) {");

            stringBuilder.Append("$(\"#modal\").modal('hide');");
            stringBuilder.Append("$(\"#modalValidations\").modal('hide');");
            stringBuilder.Append("$(\"#modalValidationsErrors\").modal('hide');");
            stringBuilder.Append(
                "var alert = '<div class=\"alert alert-success fade in\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-hidden=\"true\">×</button><strong>Ok!</strong> ' + mensaje + '.</div>';");
            stringBuilder.Append("$(\"#mensajeNotificacion\").html(alert);");
            stringBuilder.Append("$(\"#areaNotificacion\").show(0).delay(30000).hide(0);");
            stringBuilder.Append("$(\".alert\").alert();");
            stringBuilder.Append("}");

            stringBuilder.Append("function mostrarError(mensaje) {");
            stringBuilder.Append("$(\"#modal\").modal('hide');");
            stringBuilder.Append(
                "var alert = '<div class=\"alert alert-danger fade in\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-hidden=\"true\">×</button><strong>Error!</strong> ' + mensaje + '.</div>';");
            stringBuilder.Append("$(\"#mensajeError\").html(alert);");
            stringBuilder.Append("$(\"#areaError\").show(0).delay(3000).hide(0);");
            stringBuilder.Append("$(\".alert\").alert();");
            stringBuilder.Append("}");

            stringBuilder.Append("</script>");

            return new MvcHtmlString(stringBuilder.ToString());
        }

        public static MvcHtmlString LoginAlamr()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("<div id=\"areaNotificacionLogin\" class=\"notifiaction-container\">");
            stringBuilder.Append("<div id=\"mensajeNotificacionLogin\" class=\"notification\">");
            stringBuilder.Append("</div>");
            stringBuilder.Append("</div>");

            return new MvcHtmlString(stringBuilder.ToString());
        }

        public static MvcHtmlString SeccionNotificacionLogin(this HtmlHelper helper)
        {
            var stringBuilder = new StringBuilder();


            stringBuilder.Append("<script>");
            stringBuilder.Append("function mostrarNotificacionLogin(mensaje) {");
            stringBuilder.Append(
                "var alert = '<div class=\"alert alert-success fade in\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-hidden=\"true\">×</button><strong>Ok!</strong> ' + mensaje + '.</div>';");
            stringBuilder.Append("$(\"#mensajeNotificacionLogin\").html(alert);");
            stringBuilder.Append("$(\"#areaNotificacionLogin\").show(0).delay(3000).hide(0);");
            stringBuilder.Append("$(\".alert\").alert();");
            stringBuilder.Append("}");

            stringBuilder.Append("function mostrarErrorLogin(mensaje) {");
            stringBuilder.Append(
                "var alert = '<div class=\"alert alert-danger fade in\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-hidden=\"true\">×</button><strong>Error!</strong> ' + mensaje + '.</div>';");
            stringBuilder.Append("$(\"#mensajeErrorLogin\").html(alert);");
            stringBuilder.Append("$(\"#areaErrorLogin\").show(0).delay(3000).hide(0);");
            stringBuilder.Append("$(\".alert\").alert();");
            stringBuilder.Append("}");


            stringBuilder.Append("</script>");

            return new MvcHtmlString(stringBuilder.ToString());
        }

        public static MvcHtmlString SeccionNotificacionIndex(this HtmlHelper helper)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("<div id=\"areaNotificacion\" class=\"notifiaction-container\">");
            stringBuilder.Append("<div id=\"mensajeNotificacion\" class=\"notification\">");
            stringBuilder.Append("</div>");
            stringBuilder.Append("</div>");

            stringBuilder.Append("<div id=\"areaError\" class=\"notifiaction-container\">");
            stringBuilder.Append("<div id=\"mensajeError\" class=\"notificacionError\">");
            stringBuilder.Append("</div>");
            stringBuilder.Append("</div>");


            stringBuilder.Append("<script>");
            stringBuilder.Append("function mostrarNotificacion(mensaje) {");

            stringBuilder.Append("$(\"#modal\").modal('hide');");
            stringBuilder.Append(
                "var alert = '<div class=\"alert alert-success fade in\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-hidden=\"true\">×</button><strong>Ok!</strong> ' + mensaje + '.</div>';");
            stringBuilder.Append("$(\"#mensajeNotificacion\").html(alert);");
            stringBuilder.Append("$(\"#areaNotificacion\").show(0).delay(3000).hide(0);");
            stringBuilder.Append("$(\".alert\").alert();");
            stringBuilder.Append("}");

            stringBuilder.Append("function mostrarError(mensaje) {");
            stringBuilder.Append("$(\"#modal\").modal('hide');");
            stringBuilder.Append(
                "var alert = '<div class=\"alert alert-danger fade in\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-hidden=\"true\">×</button><strong>Error!</strong> ' + mensaje + '.</div>';");
            stringBuilder.Append("$(\"#mensajeError\").html(alert);");
            stringBuilder.Append("$(\"#areaError\").show(0).delay(3000).hide(0);");
            stringBuilder.Append("$(\".alert\").alert();");
            stringBuilder.Append("}");

            stringBuilder.Append("</script>");

            return new MvcHtmlString(stringBuilder.ToString());
        }
    }
}