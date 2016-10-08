using System;
using System.Linq;
using System.Web.Mvc;
using Domain.Ventas;
using BassinoLibrary.ViewModels;
using BassinoLibrary.ViewModels;
using BassinoBase.Models.ViewModelMapper.Interface;
using BassinoBase.Models.ViewModelMapper.Interfaces;
using Domain.Contable;
using Microsoft.AspNet.Identity;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;

namespace BassinoBase.Controllers
{ 
    public class VentaController : AbmController<Venta, VentaViewModel, VentaViewModel>
    {
        private readonly IVentaDao _ventaDao;
        private readonly IBancariaDao _bancariaDao;

        public VentaController(IVentaViewModelMapper ventaViewModelMapper, IVentaDao ventaDao, IUnitOfWorkHelper unitOfWorkHelper, IControllerBehabior abmControllerBahavior, IVentaDao ventaDao1, IBancariaDao bancariaDao)
			: base(abmControllerBahavior, ventaDao, ventaViewModelMapper, unitOfWorkHelper)
        {
            _ventaDao = ventaDao1;
            _bancariaDao = bancariaDao;
        }

        public override void BeforeDelete(Venta model)
        {
            model.DeleteUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
            model.DateDelete = DateTime.Now;
            model.IsDelete = true;
            base.BeforeDelete(model);
        }
        private int GetLastSecuence()
        {
            var model = _ventaDao.GetAll().Where(r => !r.IsDelete).OrderByDescending(r => r.DateCreate).FirstOrDefault();
            if (model != null && model.Secuencia != null)
            {
                return (int)(model.Secuencia + 1);

            }
            return 1;
        }

        public override void BeforeSave(Venta model, VentaViewModel viewModel, bool isNew)
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
        public override ActionResult Edit(int id)
        {
            ViewBag.CreateUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
            var model = _ventaDao.Get(id);
            var js = model.Producto.Select(f => f.Id).ToArray();
            var str = js.Aggregate("", (current, item) => current + item + "-");
            ViewBag.productIds = str;


            return base.Edit(id);
        }
        public override ActionResult Create()
        {
            ViewBag.CreateUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
            ViewBag.Productos = _ventaDao.GetAll();
            return base.Create();
        }

        public override void AfterSave(Venta model, VentaViewModel viewModel, bool isNew)
        {
            var bancaria = _bancariaDao.GetCuentaIgresos();
            bancaria.Saldo = bancaria.Saldo + viewModel.Monto;
            _bancariaDao.Save();
            base.AfterSave(model, viewModel, isNew);
        }
    }
}