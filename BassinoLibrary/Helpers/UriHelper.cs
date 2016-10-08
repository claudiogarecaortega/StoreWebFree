using System.Web;
using System.Web.Routing;

namespace BassinoLibrary.Helpers
{
    public class UriHelper
    {
        private static UriHelper _instance;

        public static UriHelper Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UriHelper();

                return _instance;
            }
        }

        public string GetUrlAbsoluta()
        {
            return VirtualPathUtility.ToAbsolute("~/");
        }

        public string GetUbicacionesAutocompleteURL()
        {
            var ruta = (Route) RouteTable.Routes["UbicacionAutocomplete"];
            return GetUrlAbsoluta() + ruta.Url;
        }

        public string GetSijpZonasAutocompleteURL()
        {
            var ruta = (Route) RouteTable.Routes["SijpZonaAutocomplete"];
            return GetUrlAbsoluta() + ruta.Url;
        }
    }
}