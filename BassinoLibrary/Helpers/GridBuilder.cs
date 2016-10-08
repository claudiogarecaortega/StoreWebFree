using System;
using System.Text;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;

namespace BassinoLibrary.Helpers
{
    public class GridBuilder<T, TC> : GridBuilder<T> where T : class
    {
        private const string EventModeModal = "gridOnChangeModal";
        private const string EventModeView = "gridOnChange";
        private const string EventModeNone = "gridOnChangeNone";

        public GridBuilder(Grid<T> component, GridDetailsMode detailsMode = GridDetailsMode.Modal)
            : base(component)
        {
            Pageable();
            Scrollable(scr => scr.Height(780));
            Sortable();
            Selectable(c =>
            {
                c.Enabled(true);
               
                c.Type(GridSelectionType.Row);
            });
            HtmlAttributes(new {controller = ControllerName()});

            ConfigurarEvento(detailsMode);
        }

        public new GridBuilder<T, TC> Name(string name)
        {
            base.Name(name);

            return this;
        }

        public new GridBuilder<T, TC> HtmlAttributes(object attributes)
        {
            base.HtmlAttributes(attributes);

            return this;
        }

        public new GridBuilder<T, TC> Columns(Action<GridColumnFactory<T>> columns)
        {
            base.Columns(columns);

            return this;
        }

        public GridBuilder<T, TC> EditarBorrar(bool permisoEditar, bool permisoBorrar, bool registroSistema = false)
        {
            Columns(
                columns =>
                    columns.Template(T => "")
                        .HtmlAttributes(new {@class = "acciones"})
                        .ClientTemplate(LinksEditarBorrar(permisoEditar, permisoBorrar, registroSistema)));

            return this;
        }
        public GridBuilder<T, TC> ViewShip(bool permisoEditar,bool recibir,bool enviar)
        {
            Columns(
                columns =>
                    columns.Template(T => "")
                        .HtmlAttributes(new { @class = "acciones" })
                        .ClientTemplate(LinkView(permisoEditar,recibir,enviar)));

            return this;
        }
        public GridBuilder<T, TC> EditarBorrarEnviar(bool permisoEditar, bool permisoBorrar, bool enviar,bool finalizar, bool registroSistema = false)
        {
            Columns(
                columns =>
                    columns.Template(T => "")
                        .HtmlAttributes(new { @class = "acciones" })
                        .ClientTemplate(LinksEditarBorrarEnviar(permisoEditar, permisoBorrar,enviar,finalizar, registroSistema)));

            return this;
        }

        public GridBuilder<T, TC> EditarBorrarNooModal(bool permisoEditar, bool permisoBorrar,
            bool registroSistema = false)
        {
            Columns(
                columns =>
                    columns.Template(T => "")
                        .HtmlAttributes(new {@class = "acciones "})
                        .ClientTemplate(LinksEditarBorrarNoModal(permisoEditar, permisoBorrar, registroSistema)));

            return this;
        }

        public GridBuilder<T, TC> Editar(bool permisoEditar)
        {
            Columns(
                columns =>
                    columns.Template(T => "")
                        .HtmlAttributes(new {@class = "acciones "})
                        .ClientTemplate(LinkEditar(permisoEditar, false)));

            return this;
        }

        public GridBuilder<T, TC> Borrar(bool permisoBorrar)
        {
            Columns(
                columns =>
                    columns.Template(T => "")
                        .HtmlAttributes(new {@class = "acciones"})
                        .ClientTemplate(LinkBorrar(permisoBorrar)));

            return this;
        }

        private string LinksEditarBorrar(bool permisoEditar, bool permisoBorrar, bool registroSistema = false)
        {
            var stringResult = new StringBuilder("");
            if (registroSistema)
            {
                return string.Format("{0}", SistemaLinkEditarBorrar());
            }

            if (permisoEditar)
                stringResult.Append(string.Format("{0} ", LinkEditar(permisoEditar)));

            if (permisoBorrar)
                stringResult.Append(string.Format("{0} ", LinkBorrar(permisoBorrar)));

            return stringResult.ToString();
        }
        private string LinksviewShip(bool permisoVer)
        {
            var stringResult = new StringBuilder("");
            if (permisoVer)
            {
                return string.Format("{0}", SistemaLinkEditarBorrar());
            }

           return stringResult.ToString();
        }
        private string LinksEditarBorrarEnviar(bool permisoEditar, bool permisoBorrar,bool enviar,bool finalizar, bool registroSistema = false)
        {
            var stringResult = new StringBuilder("");
            if (registroSistema)
            {
                return string.Format("{0}", SistemaLinkEditarBorrar());
            }

            if (permisoEditar)
                stringResult.Append(string.Format("{0} ", LinkEditar(permisoEditar)));

            if (permisoBorrar)
                stringResult.Append(string.Format("{0} ", LinkBorrar(permisoBorrar)));

            if (enviar)
                stringResult.Append(string.Format("{0} ", LinkEnviar(enviar)));
            if (finalizar)
                stringResult.Append(string.Format("{0} ", LinkFinalizar(finalizar)));

            return stringResult.ToString();
        }

        private string LinksEditarBorrarNoModal(bool permisoEditar, bool permisoBorrar, bool registroSistema = false)
        {
            var stringResult = new StringBuilder("");
            if (registroSistema)
            {
                return string.Format("{0}", SistemaLinkEditarBorrar());
            }

            if (permisoEditar)
                stringResult.Append(string.Format("{0} ", LinkEditar(permisoEditar, false)));

            if (permisoBorrar)
                stringResult.Append(string.Format("{0} ", LinkBorrar(permisoBorrar)));

            return stringResult.ToString();
        }

        private string LinkEditar(bool permisoEditar, bool modal = true)
        {
            if (!permisoEditar)
                return "";
            if (modal)
                return string
                    .Format(
                        "<a href='{0}{1}/{2}/#= encodeURIComponent(Id) #'  data-target='\\#modal' data-toggle='modal'  data-toggle='tooltip' data-backdrop='static' data-keyboard='false' data-placement='top' title='Editar'><i class='glyphicon glyphicon-pencil'></i></a>",
                        UriHelper.Instance.GetUrlAbsoluta(), ControllerName(), "Edit");
            return string
                .Format(
                    "<a href=''  data-url='{1}/{2}/#= encodeURIComponent(Id) #' data-toggle='tooltip' data-backdrop='static' data-keyboard='false' data-placement='top' title='Editar'><i class='glyphicon glyphicon-pencil'></i></a>",
                    UriHelper.Instance.GetUrlAbsoluta(), ControllerName(), "Edit");
        }

        private string SistemaLinkEditarBorrar()
        {
            var stringResult = new StringBuilder("");
            stringResult.Append(string.Format("#if(PuedeEditarBorrar) {{ # {0} {1} # }}else{{ #  # }}#",
                LinkEditar(true), LinkBorrar(true)));

            return stringResult.ToString();
        }
       private string LinkBorrar(bool permisoBorrar)
        {
            if (!permisoBorrar)
                return "";
            return string.Format(
                "<a href='{0}{1}/{2}/#= encodeURIComponent(Id) #' data-target='\\#modal' data-toggle='modal' data-backdrop='static' data-keyboard='false' data-toggle='tooltip' data-placement='top' title='Borrar'><i class='glyphicon glyphicon-trash'></i></a>",
                UriHelper.Instance.GetUrlAbsoluta(), ControllerName(), "Delete");
        }
       private string LinkView(bool permisoBorrar)
       {
           if (!permisoBorrar)
               return "";
           
           return string
                 .Format(

                     "<a href='' onclick='LoadPartialId('{0}{1}/{2}/#= encodeURIComponent(Id) #', 'mainDiv', #= encodeURIComponent(Id) #) 'data-toggle='tooltip' data-placement='top' title='Ver Envio'><i class='fa fa-eye'></i></a>",
                     UriHelper.Instance.GetUrlAbsoluta(), ControllerName(), "ShipTrackView");
       }
       private string LinkView(bool permisoBorrar,bool recibir,bool enviar)
       {
           var stringResult = new StringBuilder("");
           if (!permisoBorrar)
               return "";
           stringResult.Append(string
                 .Format(

                     //"<a href='#' onclick='LoadPartialId('{0}{1}/{2}/', 'mainDiv',' #= encodeURIComponent(Id) #') ' data-toggle='tooltip' data-placement='top' title='Ver Envio'><i class='fa fa-eye'></i></a>",
                     "<a href='{0}{1}/{2}/#= encodeURIComponent(Id) #' data-target='\\#modal' data-toggle='modal' data-backdrop='static' data-keyboard='false'  data-toggle='tooltip' data-placement='top' title='Recibir'><i class='fa fa-eye'></i></a>",
                     UriHelper.Instance.GetUrlAbsoluta(), "ShipmentTrack", "ShipTrackView"));

           if (!recibir)
                return "";
            stringResult.Append(string.Format(
                "<a href='{0}{1}/{2}/#= encodeURIComponent(Id) #' data-target='\\#modal' data-toggle='modal' data-backdrop='static' data-keyboard='false' data-toggle='tooltip' data-placement='top' title='Recibir'><i class='fa fa-truck'></i></a>",
                UriHelper.Instance.GetUrlAbsoluta(), "ShipmentTrack", "Receive"));
           if (!enviar)
                return "";
            stringResult.Append(string.Format(
                "<a href='{0}{1}/{2}/#= encodeURIComponent(Id) #' data-target='\\#modal' data-toggle='modal' data-backdrop='static' data-keyboard='false' data-toggle='tooltip' data-placement='top' title='Enviar'><i class='fa fa-plane'></i></a>",
                UriHelper.Instance.GetUrlAbsoluta(), "ShipmentTrack", "Send"));
           return stringResult.ToString();
       }
        private string LinkEnviar(bool permisoBorrar)
        {
           
            if (!permisoBorrar)
                return "";
            return string.Format(
                "<a href='{0}{1}/{2}/#= encodeURIComponent(Id) #' data-target='\\#modal' data-toggle='modal' data-backdrop='static' data-keyboard='false' data-toggle='tooltip' data-placement='top' title='Enviar'><i class='fa fa-plane'></i></a>",
          UriHelper.Instance.GetUrlAbsoluta(), ControllerName(), "SendShip");
        }
        private string LinkFinalizar(bool permisoBorrar)
        {

            if (!permisoBorrar)
                return "";
            return string.Format(
                "<a href='{0}{1}/{2}/#= encodeURIComponent(Id) #' data-target='\\#modal' data-toggle='modal' data-backdrop='static' data-keyboard='false' data-toggle='tooltip' data-placement='top' title='Finish'><i class='fa fa-lock'></i></a>",
          UriHelper.Instance.GetUrlAbsoluta(), ControllerName(), "FinishShip");
        }
        private string ControllerName()
        {
            return typeof (TC).Name.Replace("Controller", "");
        }

        private void ConfigurarEvento(GridDetailsMode detailsMode)
        {
            switch (detailsMode)
            {
                case GridDetailsMode.Modal:
                    Events(e => e.Change(EventModeModal));
                    break;
                case GridDetailsMode.View:
                    Events(e => e.Change(EventModeView));
                    break;
                case GridDetailsMode.None:
                    Events(e => e.Change(EventModeNone));
                    break;
            }
        }
    }
}