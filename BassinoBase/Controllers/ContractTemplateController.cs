using System.Linq;
using System.Web.Mvc;
using BassinoBase.Models.ViewModelMapper.Interface;
using Domain.Misc;
using BassinoLibrary.ViewModels;
using BassinoLibrary.ViewModels;
using Microsoft.AspNet.Identity;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;

namespace BassinoBase.Controllers
{ 
    public class ContractTemplateController : AbmController<ContractTemplate, ContractTemplateViewModel, ContractTemplateViewModel>
    {
		public ContractTemplateController(IContractTemplateViewModelMapper contracttemplateViewModelMapper, IContractTemplateDao contracttemplateDao, IUnitOfWorkHelper unitOfWorkHelper, IControllerBehabior abmControllerBahavior)
			: base(abmControllerBahavior, contracttemplateDao, contracttemplateViewModelMapper, unitOfWorkHelper)
        {
        }

        public override ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                //ViewBag.ListaItems = GetItems();
                var user = UserManager.FindById(User.Identity.GetUserId());
                var module = user.UserInfromation.UserRol.ListModulesActions.FirstOrDefault(v => v.Module.Name == "Plantillas");
                if (module != null)
                {
                    ViewBag.editar = module.Actions.Any(s => s.Description == "Editar");
                    ViewBag.borrar = module.Actions.Any(s => s.Description == "Borrar");
                    ViewBag.crear = module.Actions.Any(s => s.Description == "Crear");
                    ViewBag.ver = module.Actions.Any(s => s.Description == "Ver");
                    
                }
                else
                {
                    ViewBag.editar = false;
                    ViewBag.borrar = false;
                    ViewBag.crear = false;
                    ViewBag.ver = false;
                }
            }
            ViewBag.Title = "Recepción de Mercadería";
            return PartialView();
        }
    }
}