using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using BassinoBase.Models.ViewModelMapper.Interface;
using BassinoLibrary.ViewModels;
using Interfaz.Controllers;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;

namespace BassinoBase.Controllers
{
    public abstract class GeneralAbmController<TModel, TViewModel, TCommonModel> : Controller, IAbmController<TModel, TViewModel>
    {
        protected IDao<TModel> DAO;
        protected IViewModelMapper<TModel, TViewModel, TCommonModel> ViewModelMapper;
        protected IUnitOfWorkHelper UnitOfWorkHelper;
        protected readonly IControllerBehabior ABMControllerBehavior;
        protected string MensajeOperacionExitosa = "Se ha creado el item numero : ";
        protected string MensajeOperacionErronea = "No";
       // protected readonly INotificationDao _notificacionDao;
        public readonly IUserdDao UserDao;
        

        protected GeneralAbmController(IControllerBehabior abmControllerBehavior, IDao<TModel> dao, IViewModelMapper<TModel, TViewModel, TCommonModel> viewModelMapper, IUnitOfWorkHelper unitOfWorkHelper,  IUserdDao userDao)
        {
            DAO = dao;
            ViewModelMapper = viewModelMapper;
            UnitOfWorkHelper = unitOfWorkHelper;
         //   _notificacionDao = notificacionDao;
            UserDao = userDao;
            ABMControllerBehavior = abmControllerBehavior;
        }
        protected GeneralAbmController(IControllerBehabior abmControllerBehavior, IDao<TModel> dao, IViewModelMapper<TModel, TViewModel, TCommonModel> viewModelMapper, IUnitOfWorkHelper unitOfWorkHelper)
        {
            DAO = dao;
            ViewModelMapper = viewModelMapper;
            UnitOfWorkHelper = unitOfWorkHelper;
            

            ABMControllerBehavior = abmControllerBehavior;
        }
        public virtual ActionResult Index()
        {
            ABMControllerBehavior.Index(this);
            return PartialView();
        }

        public virtual ActionResult GridInfo([DataSourceRequest] DataSourceRequest request, string filtro)
        {
            var result = DAO.GetAllQ(filtro).ToDataSourceResult(request, ViewModelMapper.Map);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //[ImportModelStateFromTempData]
        public virtual ActionResult Create()
        {
            return PartialView();
        }

        public virtual string GetDisplayName(TViewModel viewModel,string prop)
        {
            MemberInfo property = typeof(TViewModel).GetProperty(prop);
            var dd = property.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
            var name2 = "";
            if (dd != null)
            {
             
               name2 = dd.GetName();
             
            }
            return name2;
        }

        public string GetItem(string controller)
        {
            switch (controller)
            {
                case "Client":
                    return "Cuenta";
                case "Inbound":
                    return "Ingreso de Mercaderia";
                case "Shipment":
                    return "Gestion de Mercaderia";
                case"ShipmentTrack":
                    return "Recepcion de mercaria";
            }
            return "";
        }

        //public virtual void CreateNotification(TViewModel viewModel)
        //{
        //    var model = _notificacionDao.Create();
        //    model.IdProduct=Convert.ToInt32( ABMControllerBehavior.GetModelId(viewModel));
        //    //
        //    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    model.Url = "/"+controllerName + "/" + actionName + "/" + model.IdProduct;
        //    model.IsForAllUSers = false;
        //    model.Importance = "3";
        //    var action = "";
        //    if (actionName.Contains("Create"))
        //        action = "Creado";
        //    else if (actionName.Contains("Edit"))
        //        action = "Editado";
        //    else if (actionName.Contains("Delete"))
        //        action = "Eliminado";

        //    model.Message = "se ha "+action+ "el siguiente item de tipo  :";
        //    model.UserToNotifiy = UserDao.GetAll().Where(d=>d.UserRol.Description=="Admin").ToList();
        //    _notificacionDao.Add(model);
        //    _notificacionDao.Save();

        //}
        //public virtual void CreateNotificationShippmentTrack(TViewModel viewModel)
        //{
        //    var model = _notificacionDao.Create();
        //    model.IdProduct = Convert.ToInt32(ABMControllerBehavior.GetModelId(viewModel));
        //    //
        //    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    model.Url = "/"+controllerName + "/Details/" + model.IdProduct;
        //    model.IsForAllUSers = false;
        //    model.Importance = "3";
        //   model.Message = "El envio numero :"+model.IdProduct+" ha llegado a un nuevo destino";
        //    model.UserToNotifiy = UserDao.GetAll().Where(d => d.UserRol.Description == "Admin").ToList();
        //    _notificacionDao.Add(model);
        //    _notificacionDao.Save();

        //}
        [HttpPost]
        public virtual ActionResult Create(TViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return PartialView(viewModel);

           var model= this.CreateModel(viewModel);

           return JavaScript(ABMControllerBehavior.MensajeOperacionExitosaModalShowData(MensajeOperacionExitosa + " " + ABMControllerBehavior.GetModelSecuencia(model)));
        }

        protected virtual TModel CreateModel(TViewModel viewModel)
        {
            return ABMControllerBehavior.CreateModel(this, viewModel, DAO, ViewModelMapper);
        }

        public virtual void AfterSave(TModel model, TViewModel viewModel, bool isNew)
        {
         
        }
        public virtual void BeforeSave(TModel model, TViewModel viewModel, bool isNew) { }
        public virtual void AfterDelete(TModel model) { }
        public virtual void BeforeDelete(TModel model) { }
    }
}