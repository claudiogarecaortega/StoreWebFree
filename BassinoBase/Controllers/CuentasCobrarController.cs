using System;
using System.Linq;
using System.Web.Mvc;
using Domain.Contable;
using BassinoLibrary.ViewModels;
using BassinoLibrary.ViewModels;
using BassinoBase.Models.ViewModelMapper.Interface;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using BassinoBase.Models.ViewModelMapper.Interfaces;
using Microsoft.AspNet.Identity;

namespace BassinoBase.Controllers
{ 
    public class CuentasCobrarController : AbmController<CuentasCobrar, CuentasCobrarViewModel, CuentasCobrarViewModel>
    {
        private readonly ICuentasCobrarDao _cuentasCobrarDao;
        private readonly IBancariaDao _bancariaDao;

        public CuentasCobrarController(ICuentasCobrarViewModelMapper cuentascobrarViewModelMapper, ICuentasCobrarDao cuentascobrarDao, IUnitOfWorkHelper unitOfWorkHelper, IControllerBehabior abmControllerBahavior, ICuentasCobrarDao cuentasCobrarDao, IBancariaDao bancariaDao)
			: base(abmControllerBahavior, cuentascobrarDao, cuentascobrarViewModelMapper, unitOfWorkHelper)
        {
            _cuentasCobrarDao = cuentasCobrarDao;
            _bancariaDao = bancariaDao;
        }

        public override void BeforeDelete(CuentasCobrar model)
        {
            model.DeleteUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
            model.DateDelete = DateTime.Now;
            model.IsDelete = true;
            base.BeforeDelete(model);
        }
        private int GetLastSecuence()
        {
            var model = _cuentasCobrarDao.GetAll().Where(r => !r.IsDelete).OrderByDescending(r => r.DateCreate).FirstOrDefault();
            if (model != null && model.Secuencia != null)
            {
                return (int)(model.Secuencia + 1);

            }
            return 1;
        }

        public override void BeforeSave(CuentasCobrar model, CuentasCobrarViewModel viewModel, bool isNew)
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
            ViewBag.Cuentas = _bancariaDao.GetAll();
            return base.Create();
        }

        public override ActionResult Edit(int id)
        {
            ViewBag.Cuentas = _bancariaDao.GetAll();
            return base.Edit(id);
        }

        public override ActionResult Create(CuentasCobrarViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Cuentas = _bancariaDao.GetAll();
                return PartialView(viewModel);
            }
            return base.Create(viewModel);
        }

        public override ActionResult Edit(CuentasCobrarViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Cuentas = _bancariaDao.GetAll();
                return PartialView(viewModel);
            }
            return base.Edit(viewModel);
        }
    }
}