using System;
using System.Linq;
using System.Web.Mvc;
using BassinoBase.Models.ViewModelMapper.Interfaces;
using Domain.Almacen;
using BassinoLibrary.ViewModels;
using BassinoLibrary.ViewModels;
using Domain.Misc;
using Microsoft.AspNet.Identity;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;

namespace BassinoBase.Controllers
{ 
    public class StockController : AbmController<Stock, StockViewModel, StockViewModel>
    {
        private readonly IProductDao _productoDao;
        private readonly IAlmacenDao _almacenDao;
        private readonly IStockDao _stockDao;

        public StockController(IStockViewModelMapper stockViewModelMapper, IStockDao stockDao, IUnitOfWorkHelper unitOfWorkHelper, IControllerBehabior abmControllerBahavior, IProductDao productoDao, IAlmacenDao almacenDao, IStockDao stockDao1)
			: base(abmControllerBahavior, stockDao, stockViewModelMapper, unitOfWorkHelper)
        {
            _productoDao = productoDao;
            _almacenDao = almacenDao;
            _stockDao = stockDao1;
        }
        public override void BeforeDelete(Stock model)
        {
            model.DeleteUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
            model.DateDelete = DateTime.Now;
            model.IsDelete = true;
            base.BeforeDelete(model);
        }
        private int GetLastSecuence()
        {
            var model = _stockDao.GetAll().Where(r => !r.IsDelete).OrderByDescending(r => r.DateCreate).FirstOrDefault();
            if (model != null)
            {
                return (int)(model.Secuencia + 1);

            }
            return 1;
        }

        public override void BeforeSave(Stock model, StockViewModel viewModel, bool isNew)
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
            ViewBag.Productos = _productoDao.GetAll();
            ViewBag.Almacenes = _almacenDao.GetAll();
            return base.Create();
        }

        public override ActionResult Create(StockViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.Productos = _productoDao.GetAll();
                ViewBag.Almacenes = _almacenDao.GetAll();
                return PartialView(viewModel);
            }
            return base.Create(viewModel);
        }

        public override ActionResult Edit(StockViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Productos = _productoDao.GetAll();
                ViewBag.Almacenes = _almacenDao.GetAll();
                return PartialView(viewModel);
            }
            return base.Edit(viewModel);
        }

        public override ActionResult Edit(int id)
        {
            ViewBag.Productos = _productoDao.GetAll();
            ViewBag.Almacenes = _almacenDao.GetAll();
            return base.Edit(id);
        }

        public override ActionResult Index()
        {
            return PartialView();
        }
    }
}