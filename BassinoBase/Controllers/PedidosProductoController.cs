using System;
using System.Linq;
using System.Web.Mvc;
using Domain.Providers;
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
    public class PedidosProductoController : AbmController<PedidosProducto, PedidosProductoViewModel, PedidosProductoViewModel>
    {
        private readonly IProviderDao _provedoresDao;
        private readonly IPedidosProductoDao _pedidoProductoDao;

        public PedidosProductoController(IPedidosProductoViewModelMapper pedidosproductoViewModelMapper, IPedidosProductoDao pedidosproductoDao, IUnitOfWorkHelper unitOfWorkHelper, IControllerBehabior abmControllerBahavior, IProviderDao provedoresDao, IPedidosProductoDao pedidoProductoDao)
			: base(abmControllerBahavior, pedidosproductoDao, pedidosproductoViewModelMapper, unitOfWorkHelper)
        {
            _provedoresDao = provedoresDao;
            _pedidoProductoDao = pedidoProductoDao;
        }

        public override void BeforeDelete(PedidosProducto model)
        {
            model.DeleteUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
            model.DateDelete = DateTime.Now;
            model.IsDelete = true;
            base.BeforeDelete(model);
        }
        private int GetLastSecuence()
        {
            var model = _pedidoProductoDao.GetAll().Where(r => !r.IsDelete).OrderByDescending(r => r.DateCreate).FirstOrDefault();
            if (model != null && model.Secuencia != null)
            {
                return (int)(model.Secuencia + 1);

            }
            return 1;
        }

        public override void BeforeSave(PedidosProducto model, PedidosProductoViewModel viewModel, bool isNew)
        {
            if (isNew)
            {
                model.Secuencia = GetLastSecuence();
                model.CreateUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
                model.DateCreate = DateTime.Now;
                var cuentaPagar= new CuentasPagar();
                cuentaPagar.Monto = viewModel.Precio - viewModel.Adelantado;
                cuentaPagar.Comentarios = cuentaPagar.Comentarios +
                                          " se ha creado la cuenta por pagar del pedido por el monto :" +
                                          viewModel.Precio + " con el adelanto de :" + viewModel.Adelantado+" en fecha: "+DateTime.Now;
                cuentaPagar.FechaVencimiento = Convert.ToDateTime(viewModel.FechaDePago);
                model.CuentasPagar = cuentaPagar;


            }
            else
            {
                model.UpdateUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
                model.DateUpdate = DateTime.Now;
                model.CuentasPagar.Monto = viewModel.Precio - viewModel.Adelantado;
                model.CuentasPagar.Comentarios = model.CuentasPagar.Comentarios +
                                          " se ha Modificado la cuenta por pagar del pedido por el monto :" +
                                          viewModel.Precio + " con el adelanto de :" + viewModel.Adelantado + " en fecha: " + DateTime.Now;
                model.CuentasPagar.FechaVencimiento = Convert.ToDateTime(viewModel.FechaDePago);

            }
            base.BeforeSave(model, viewModel, isNew);
        }
        public override ActionResult Create()
        {
            ViewBag.Provedores = _provedoresDao.GetAll();
            return base.Create();
        }

        public override ActionResult Create(PedidosProductoViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Provedores = _provedoresDao.GetAll();
                return PartialView(viewModel);
            }
            return base.Create(viewModel);
        }

        public override ActionResult Edit(PedidosProductoViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Provedores = _provedoresDao.GetAll();
                return PartialView(viewModel);
            }
            return base.Edit(viewModel);
        }

        public override ActionResult Edit(int id)
        {
            ViewBag.Provedores = _provedoresDao.GetAll();
            return base.Edit(id);
        }

        public override void AfterSave(PedidosProducto model, PedidosProductoViewModel viewModel, bool isNew)
        {
            
            base.AfterSave(model, viewModel, isNew);
        }
    }
}