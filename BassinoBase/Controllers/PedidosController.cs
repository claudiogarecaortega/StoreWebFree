using System;
using System.Linq;
using System.Web.Mvc;
using Domain.Ventas;
using BassinoLibrary.ViewModels;
using BassinoLibrary.ViewModels;
using BassinoBase.Models.ViewModelMapper.Interface;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using BassinoBase.Models.ViewModelMapper.Interfaces;
using Domain.Products;
using Microsoft.AspNet.Identity;

namespace BassinoBase.Controllers
{ 
    public class PedidosController : AbmController<Pedidos, PedidosViewModel, PedidosViewModel>
    {
        private readonly IPedidosDao _pedidosDao;
        private readonly IBancariaDao _bancariasDao;

        public PedidosController(IPedidosViewModelMapper pedidosViewModelMapper, IPedidosDao pedidosDao, IUnitOfWorkHelper unitOfWorkHelper, IControllerBehabior abmControllerBahavior, IPedidosDao pedidosDao1, IBancariaDao bancariasDao)
			: base(abmControllerBahavior, pedidosDao, pedidosViewModelMapper, unitOfWorkHelper)
        {
            _pedidosDao = pedidosDao1;
            _bancariasDao = bancariasDao;
        }

        public override void BeforeDelete(Pedidos model)
        {
            model.DeleteUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
            model.DateDelete = DateTime.Now;
            model.IsDelete = true;
            base.BeforeDelete(model);
        }
        private int GetLastSecuence()
        {
            var model = _pedidosDao.GetAll().Where(r => !r.IsDelete).OrderByDescending(r => r.DateCreate).FirstOrDefault();
            if (model != null && model.Secuencia != null)
            {
                return (int)(model.Secuencia + 1);

            }
            return 1;
        }

        public override void BeforeSave(Pedidos model, PedidosViewModel viewModel, bool isNew)
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
            ViewBag.Cuentas = _bancariasDao.GetCuentaIgresos();
            ViewBag.usuarioId = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id;
            return base.Create();
        }

        public override ActionResult Edit(int id)
        {
            return base.Edit(id);
        }
    }
}