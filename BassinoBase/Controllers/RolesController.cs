using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BassinoBase.Models.ViewModelMapper.Interface;
using Domain.Security;
using BassinoLibrary.ViewModels;
using BassinoLibrary.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;

namespace BassinoBase.Controllers
{ 
    public class RolesController : AbmController<Roles, RolesViewModel, RolesViewModel>
    {
        private readonly IRolesDao _rolesDaoDao;
        private readonly IModuleViewModelMapper _moduleViewModelMapper;
        private readonly IActionsDao _actionsDao;
        private readonly IModuleDao _moduleDao;

        public RolesController(IRolesViewModelMapper rolesViewModelMapper, IRolesDao rolesDao, IUnitOfWorkHelper unitOfWorkHelper, IControllerBehabior abmControllerBahavior, IRolesDao rolesDaoDao, IModuleViewModelMapper moduleViewModelMapper, IActionsDao actionsDao, IModuleDao moduleDao)
			: base(abmControllerBahavior, rolesDao, rolesViewModelMapper, unitOfWorkHelper)
        {
            _rolesDaoDao = rolesDaoDao;
            _moduleViewModelMapper = moduleViewModelMapper;
            _actionsDao = actionsDao;
            _moduleDao = moduleDao;
        }
        public override void BeforeDelete(Roles model)
        {
            model.DeleteUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
            model.DateDelete = DateTime.Now;
            model.IsDelete = true;
            base.BeforeDelete(model);
        }
        public override void BeforeSave(Roles model, RolesViewModel viewModel, bool isNew)
        {
            if (isNew)
            {
                model.CreateUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
                model.DateCreate = DateTime.Now;
            }
            else
            {
                model.UpdateUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
                model.DateUpdate = DateTime.Now;

            }
            base.BeforeSave(model, viewModel, isNew);
        }

        public override ActionResult Index()
        {
            return PartialView();
        }
        public ActionResult GridInfoRoles([DataSourceRequest] DataSourceRequest request, string id)
        {
            var result = _rolesDaoDao.GetModulesRole(Convert.ToInt32(id)).ToDataSourceResult(request, _moduleViewModelMapper.Map);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddRol(int idRol,int idModule)
        {
            var viewmodel = new RolesModuleViewModel();
            var rol = _rolesDaoDao.Get(idRol);
            var module = _moduleDao.Get(idModule);
            viewmodel.ModuleNameId = module.Id;
            viewmodel.ModuleName = module.ModuleName;
            viewmodel.RolName = rol.Name;
            viewmodel.RolNameId = rol.Id;

            var firstOrDefault = rol.ListModulesActions.FirstOrDefault(d => d.Module.Id == idModule);
            if (firstOrDefault != null)
                viewmodel.Actions =
                    firstOrDefault
                        .Actions.Select(z => z.Description)
                        .ToArray();

            ViewBag.Actions = _actionsDao.GetAll();

            return PartialView(viewmodel);
        }
        [HttpPost]
        public ActionResult AddRol(RolesModuleViewModel viewModel)
        {
            var keys = Request.Params.AllKeys;
            var model = _rolesDaoDao.Get(viewModel.RolNameId);
            var moduleActions = model.ListModulesActions ?? new List<ModuleActions>();
            if (moduleActions.Any(s => s.Module.Id == viewModel.ModuleNameId))
            {
                var before = model.ListModulesActions.FirstOrDefault(s => s.Module.Id == viewModel.ModuleNameId).Actions;

                  var toupdate= GetActions(keys).ToList();
                    var nuevaAdd = new List<Actions>();
                    var nuevaRemove = new List<Actions>();
                    nuevaRemove = before.Where(item => !toupdate.Contains(item)).ToList();
                    nuevaAdd = toupdate.Where(item => !before.Contains(item)).ToList();
                    foreach (var add in nuevaAdd)
                {
                    model.ListModulesActions.FirstOrDefault(s => s.Module.Id == viewModel.ModuleNameId).Actions.Add(add);
                }
                    foreach (var add in nuevaRemove)
                    {
                        model.ListModulesActions.FirstOrDefault(s => s.Module.Id == viewModel.ModuleNameId).Actions.Remove(add);
                    }

            }
            else
            {
                var module = _moduleDao.Get(viewModel.ModuleNameId);
                var moduleactio = new ModuleActions();
                moduleactio.Module = module;
                moduleactio.Actions = GetActions(keys).ToList();
                moduleactio.Role = model;
                model.ListModulesActions.Add(moduleactio);

            }
            _rolesDaoDao.Save();

            return JavaScript(ABMControllerBehavior.MostrarNotificacionAlert(MensajeOperacionExitosa, 4));
        }

      
        private IEnumerable<Actions> GetActions(string[] keysStrings)
        {
            var actions = _actionsDao.GetAll();
            var actionsResult = new List<Actions>();
            foreach (var item in keysStrings)
            {
                var result = actions.FirstOrDefault(c => c.Description == item);
                if(result!=null)
                    actionsResult.Add(result);
                
            }
            return actionsResult;

        }

        public ActionResult EditModules(int id)
        {
            ViewBag.Roleid = id;
            return PartialView();
        }
    }
}