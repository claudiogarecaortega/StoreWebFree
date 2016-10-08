using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BassinoBase.Models.ViewModelMapper.Interfaces;
using Domain.Almacen;
using BassinoLibrary.ViewModels;
using BassinoLibrary.ViewModels;
using Domain.Misc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;

namespace BassinoBase.Controllers
{ 
    public class MovimientosStockController : AbmController<MovimientosStock, MovimientosStockViewModel, MovimientosStockViewModel>
    {
        private readonly IStockDao _stockDao;
        private readonly IStockViewModelMapper _stockViewModelMapper;
        private readonly IProductDao _productDao;
        private readonly IMovimientosStockDao _stockMovimientoDao;
        private readonly IProviderDao _provedoresDao;

        public MovimientosStockController(IMovimientosStockViewModelMapper movimientosstockViewModelMapper, IMovimientosStockDao movimientosstockDao, IUnitOfWorkHelper unitOfWorkHelper, IControllerBehabior abmControllerBahavior, IStockDao stockDao, IStockViewModelMapper stockViewModelMapper, IProductDao productDao, IMovimientosStockDao stockMovimientoDao, IProviderDao provedoresDao)
			: base(abmControllerBahavior, movimientosstockDao, movimientosstockViewModelMapper, unitOfWorkHelper)
        {
            _stockDao = stockDao;
            _stockViewModelMapper = stockViewModelMapper;
            _productDao = productDao;
            _stockMovimientoDao = stockMovimientoDao;
            _provedoresDao = provedoresDao;
        }
        public override void BeforeDelete(MovimientosStock model)
        {
            model.DeleteUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
            model.DateDelete = DateTime.Now;
            model.IsDelete = true;
            base.BeforeDelete(model);
        }
        private int GetLastSecuence()
        {
            var model = _stockMovimientoDao.GetAll().Where(r => !r.IsDelete).OrderByDescending(r => r.DateCreate).FirstOrDefault();
            if (model != null)
            {
                return (int)(model.Secuencia + 1);

            }
            return 1;
        }

        public override void BeforeSave(MovimientosStock model, MovimientosStockViewModel viewModel, bool isNew)
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
        private IList<SelectListItem> GetItems()
        {
            var list = new List<SelectListItem>() { new SelectListItem() { Value = "0", Text = "Selecione una Cuenta" } };
            var lista = _productDao.GetAll().Where(d => !d.IsDelete);
            foreach (var item in lista)
            {
                var items = new SelectListItem() { Value = item.Id.ToString(), Text = item.Nombre };
                list.Add(items);

            }
            return list;
        }
        public override ActionResult Index()
        {
            ViewBag.ListaItems = GetItems();
            return PartialView();
        }
        public ActionResult GridInfoShipProduccion([DataSourceRequest] DataSourceRequest request, string cuenta, string fechas, bool infecha = false)
        {
            var start = new DateTime();
            var end = new DateTime();

            if (infecha)
            {
                var dates = fechas.Split('-');
                start = Convert.ToDateTime(dates[0].Trim());
                end = Convert.ToDateTime(dates[1].Trim());
            }
            var result = _stockMovimientoDao.GetAllAccount(Convert.ToInt32(cuenta), infecha, start, end).ToDataSourceResult(request, ViewModelMapper.Map);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddStock(int id)
        {
            var model = _stockDao.Get(id);
            ViewBag.Provedores = _provedoresDao.GetAll();
            var viewModel = new MovimientosStockViewModel
            {
                Stock = model.Descripcion,
                StockId = model.Id,
                StockProducto = model.Producto.Nombre
            };
            return PartialView(viewModel);

        }
        public ActionResult RemoveStock(int id)
        {
            var model = _stockDao.Get(id);
            ViewBag.Provedores = _provedoresDao.GetAll();
            var viewModel = new MovimientosStockViewModel
            {
                Stock = model.Descripcion,
                StockId = model.Id,
                StockProducto = model.Producto.Nombre
            };
            return PartialView(viewModel);

        }
        [HttpPost]
        public ActionResult AddStock(MovimientosStockViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Provedores = _provedoresDao.GetAll();
                return PartialView(viewModel);
            }
            var model = _stockDao.Get(viewModel.StockId);
            model.Cantidad = model.Cantidad + viewModel.Cantidad;
            viewModel.EsIngreso = true;
            _stockDao.Save();
            return base.Create(viewModel);

        }
        [HttpPost]
        public ActionResult RemoveStock(MovimientosStockViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Provedores = _provedoresDao.GetAll();
                return PartialView(viewModel);
            }
            var model = _stockDao.Get(viewModel.StockId);
            model.Cantidad = model.Cantidad - viewModel.Cantidad;
            viewModel.EsIngreso = true;
            _stockDao.Save();
            viewModel.EsIngreso = false;
            return base.Create(viewModel);
        }

        public ActionResult MovimientoStock(int id)
        {
            var model = _stockDao.Get(id);
            var viewModel =_stockViewModelMapper.Map(model);
            return PartialView(viewModel);
        }
    }
}