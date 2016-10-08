using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Interfaz.Controllers
{
    public interface IAbmController<TModel, TViewModel>
    {
        IPrincipal User { get; }
        dynamic ViewBag { get; }

        void AfterSave(TModel model, TViewModel viewModel, bool isNew);
        void BeforeSave(TModel model, TViewModel viewModel, bool isNew);
        void AfterDelete(TModel model);
        void BeforeDelete(TModel model);
    }
}
