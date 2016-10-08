using System;
using System.Linq;
using System.Web.Mvc;
using BassinoBase.Models.ViewModelMapper.Interface;
using Domain.Providers;
using BassinoLibrary.ViewModels;
using BassinoLibrary.ViewModels;
using Microsoft.AspNet.Identity;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;

namespace BassinoBase.Controllers
{ 
    public class ProviderController : AbmController<Provider, ProviderViewModel, ProviderViewModel>
    {
        private readonly IProviderDao _providerDao;
        private readonly ITaxConditionDao _taxConditioDao;

        public ProviderController(IProviderViewModelMapper providerViewModelMapper, IProviderDao providerDao, IUnitOfWorkHelper unitOfWorkHelper, IControllerBehabior abmControllerBahavior, IProviderDao providerDao1, ITaxConditionDao taxConditioDao)
			: base(abmControllerBahavior, providerDao, providerViewModelMapper, unitOfWorkHelper)
        {
            _providerDao = providerDao1;
            _taxConditioDao = taxConditioDao;
        }

        private int GetLastSecuence()
        {
            var model = _providerDao.GetAll().Where(r => !r.IsDelete).OrderByDescending(r => r.DateCreate).FirstOrDefault();
            if (model != null)
            {
                return (int)(model.Secuencia + 1);

            }
            return 1;
        }
        public override void BeforeDelete(Provider model)
        {
            model.DeleteUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
            model.DateDelete = DateTime.Now;
            model.IsDelete = true;
            base.BeforeDelete(model);
        }

        public override void BeforeSave(Provider model, ProviderViewModel viewModel, bool isNew)
        {
            if (isNew)
            {
                model.Secuencia = GetLastSecuence();
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

        public override ActionResult Create()
        {
            ViewBag.Providers = _taxConditioDao.GetAll();
            return base.Create();
        }

        public override ActionResult Edit(int id)
        {
            ViewBag.Providers = _taxConditioDao.GetAll();
            return base.Edit(id);
        }
        public override ActionResult Create(ProviderViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Providers = _taxConditioDao.GetAll();
                return PartialView(viewModel);
            }
            return base.Create(viewModel);
        }

        public override ActionResult Edit(ProviderViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Providers = _taxConditioDao.GetAll();
                return PartialView(viewModel);
            }
            
            return base.Edit(viewModel);
        }

        public override ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
                var module = user.UserInfromation.UserRol.ListModulesActions.FirstOrDefault(v => v.Module.Name == "Proveedores");
                if (module != null)
                {
                    ViewBag.editar = module.Actions.Any(s => s.Description == "Editar");
                    ViewBag.borrar = module.Actions.Any(s => s.Description == "Borrar");
                    ViewBag.crear = module.Actions.Any(s => s.Description == "Crear");
                    ViewBag.imprimir = module.Actions.Any(s => s.Description == "Imprimir");
                }
                else
                {
                    ViewBag.editar = false;
                    ViewBag.borrar = false;
                    ViewBag.crear = false;
                    ViewBag.imprimir = false;
                }
            }
            ViewBag.Title = "Proveedor";
            return PartialView();
        }
    }
}