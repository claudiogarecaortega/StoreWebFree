using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BassinoBase.Hubs;
using BassinoBase.Models.ViewModelMapper.Interface;
using Domain.Clients;
using BassinoLibrary.ViewModels;
using BassinoLibrary.ViewModels;
using Domain.Misc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;

namespace BassinoBase.Controllers
{ 
    public class ClientController : AbmController<Client, ClientViewModel, ClientViewModel>
    {
        private readonly ITaxConditionDao _taxConditioDao;
        private readonly IUserdDao _usersDao;
        private readonly IClientDao _iclientDao;
        private readonly IClientDao _clientDao;
        private readonly INotificationDao _notificacionDao;
        private readonly IContractTemplateDao _templateDao;
        private readonly IContractDao _contractDao;
        private readonly IUbicationDao _ubicationDao;
        private readonly IUbicationViewModelMapper _ubicatioViewModelMapper;
        private readonly IClientViewModelMapper _clientViewModelMapper;

        public ActionResult GetAutoComplete(string texto)
        {
            var result = _clientDao.GetAutoComplete(texto).Take(10);

            var viewModels = _clientViewModelMapper.Map(result);

            return Json(viewModels, JsonRequestBehavior.AllowGet);
        }
        public override void AfterSave(Client model, ClientViewModel viewModel, bool isNew)
        {
            var id=UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
            var idNotificatio=viewModel.Id==0?CreateNotificationClient(model): CreateNotificationClient(viewModel);
          Hubs.NoficationsHub j = new NoficationsHub();
            j.SendMessage(idNotificatio,id);
          
            base.AfterSave(model, viewModel, isNew);

        }

        public int GetLastSecuece(int type)
        {
            var secuence = 0;
            //if (type == 1)
            //{
            //    var model = _clientDao.GetAllClients().OrderByDescending(r => r.DateCreate).FirstOrDefault();
            //    if (model != null)
            //    {
            //        return secuence +  (int)model.Secuencia +1;
            //    }
            //    return 1;
            //}
            //if (type == 2)
            //{
            //    var model = _clientDao.GetAllOrigen("").OrderByDescending(r => r.DateCreate).First();
            //    if (model != null)
            //    {
            //        return secuence + (int)model.Secuencia + 1;
            //    }
            //    return 1;
            //}
            //if (type == 3)
            //{
                var model = _clientDao.GetAll().OrderByDescending(r => r.DateCreate).FirstOrDefault();
                if (model != null)
                {
                    return secuence + (int)model.Secuencia + 1;
                }
                return 1;
            //}
            //return 0;

        }
        public override void BeforeDelete(Client model)
        {
            model.DeleteUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
            model.DateDelete = DateTime.Now;
            model.IsDelete = true;
            base.BeforeDelete(model);
        }
        public static void SendMessage(string msg)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<NoficationsHub>();
            hubContext.Clients.All.nofifyClientAction(msg);
        }

        private int CreateNotificationClient(ClientViewModel  clientViewModel)
        {
             var model = _notificacionDao.Create();
            model.IdProduct = clientViewModel.Id;
            var user = UserManager.FindById(User.Identity.GetUserId());
            //
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            model.Url = "/"+controllerName + "/Details/" + model.IdProduct;
            model.IsForAllUSers = false;
            model.Importance = "3";
            var action = "";
            if (actionName.Contains("Create"))
                action = "Se ha creado la Cuenta por el usuario "+user.UserInfromation.FullName;
            if (actionName.Contains("Edit"))
                action = "Se ha Editado la Cuenta por el usuario " + user.UserInfromation.FullName;
            if (actionName.Contains("Delete"))
                action = "Se ha eliminado la Cuenta por el usuario " + user.UserInfromation.FullName;

            model.Message = action;
            var listauser = _usersDao.GetAll().Where(d => d.UserRol.Description == "Admin");
            var litaUSers = new List<UserNotification>();
            foreach (var item in listauser)
            {
                var notificationuser = new UserNotification() {User = item};
               litaUSers.Add(notificationuser);
            }
            model.UserToNotifiy = litaUSers;
            _notificacionDao.Add(model);
            _notificacionDao.Save();
            return model.Id;

        }
        private int CreateNotificationClient(Client clientViewModel)
        {
            var model = _notificacionDao.Create();
            model.IdProduct = clientViewModel.Id;
            var user = UserManager.FindById(User.Identity.GetUserId());
            //
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            model.Url = "/" + controllerName + "/Details/" + model.IdProduct;
            model.IsForAllUSers = false;
            model.Importance = "2";
            var action = "";
            if (actionName.Contains("Create"))
                action = "Se ha creado la Cuenta por el usuario " + user.UserInfromation.FullName;
            if (actionName.Contains("Edit"))
                action = "Se ha Editado la Cuenta por el usuario " + user.UserInfromation.FullName;
            if (actionName.Contains("Delete"))
                action = "Se ha eliminado la Cuenta por el usuario " + user.UserInfromation.FullName;

            model.Message = action;
            var listauser = _usersDao.GetAll().Where(d => d.UserRol.Description == "Admin");
            var litaUSers = new List<UserNotification>();
            foreach (var item in listauser)
            {
                var notificationuser = new UserNotification() { User = item };
                litaUSers.Add(notificationuser);
            }
            model.UserToNotifiy = litaUSers;
            _notificacionDao.Add(model);
            _notificacionDao.Save();
            return model.Id;

        }
        public ClientController(IClientViewModelMapper clientViewModelMapper, IClientDao clientDao, IUnitOfWorkHelper unitOfWorkHelper, IControllerBehabior abmControllerBahavior, ITaxConditionDao taxConditioDao, IUserdDao usersDao, IClientDao iclientDao, IClientDao clientDao1, INotificationDao notificacionDao, IContractTemplateDao templateDao, IContractDao contractDao, IUbicationDao ubicationDao, IUbicationViewModelMapper ubicatioViewModelMapper, IClientViewModelMapper clientViewModelMapper1)
			: base(abmControllerBahavior, clientDao, clientViewModelMapper, unitOfWorkHelper)
        {
            _taxConditioDao = taxConditioDao;
            _usersDao = usersDao;
            _iclientDao = iclientDao;
            _clientDao = clientDao1;
            _notificacionDao = notificacionDao;
            _templateDao = templateDao;
            _contractDao = contractDao;
            _ubicationDao = ubicationDao;
            _ubicatioViewModelMapper = ubicatioViewModelMapper;
            _clientViewModelMapper = clientViewModelMapper1;
        }
        
        public override void BeforeSave(Client model, ClientViewModel viewModel, bool isNew)
        {
            var type = 0;
            //if (model.IsClientDestination)
            //    type = 3;

            //if (model.IsClientOrigen)
            //    type = 2;

            //if (!model.IsClientDestination && !model.IsClientOrigen)
            //    type = 1;

            if (isNew)
            {
                model.Secuencia = GetLastSecuece(type);
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

        public ActionResult ReadContract(int id)
        {
            var flag = false;
            var model = _clientDao.Get(id);
            if (model.Contract != null)
                flag = true;
            return Json(new { result = flag });
            
            
        }
        [HttpPost]
        public ActionResult EditContract(ClientViewModel viewModel)
        {
            var model = _clientDao.Get(viewModel.Id);
            model.Contract.Contrato = viewModel.ContratoDoc;
            _clientDao.Save();
            return JavaScript(ABMControllerBehavior.MostrarNotificacionAlert(MensajeOperacionExitosa, 4));
        }
        public ActionResult EditContract(int id)
        {
            return base.Edit(id);
        }

        public ActionResult SyncContract(int id)
        {
            ViewBag.contratos = _templateDao.GetAll();
         return base.Edit(id);
        }
        [HttpPost]
        public ActionResult SyncContract(ClientViewModel viewModel)
        {
            var model = _clientDao.Get(viewModel.Id);
            var template = _templateDao.Get(viewModel.ContratoId);
            var contrato = _contractDao.Create();
            contrato.Contrato = template.Contrato;
            contrato.Description = template.Description;
            contrato.Client = model;
            _contractDao.Add(contrato);
            _contractDao.Save();
            model.Contract = contrato;
           _clientDao.Save();
           return JavaScript(ABMControllerBehavior.MostrarNotificacionAlert(MensajeOperacionExitosa, 4));
        }

        public override ActionResult Create(ClientViewModel viewModel)
        {
           
            if (!ModelState.IsValid)
            {
                ViewBag.Providers = _taxConditioDao.GetAll();
                ViewBag.Users = _usersDao.GetAll();
                return PartialView(viewModel);
            }
            return base.Create(viewModel);
        }
        [HttpPost]
        public ActionResult EditDestino(ClientViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Ubications = _ubicatioViewModelMapper.Map(_ubicationDao.GetAll());
                ViewBag.Action = "EditDestino";
                ViewBag.IsDestination = true;
                ViewBag.title = "Destino";


                ViewBag.Providers = _taxConditioDao.GetAll();
                ViewBag.Users = _usersDao.GetAll();
                return PartialView(viewModel);
            }


            viewModel.IsClientDestination = true;
            return base.Edit(viewModel);
        }
        [HttpPost]
        public ActionResult EditOrigen(ClientViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Providers = _taxConditioDao.GetAll();
                ViewBag.Users = _usersDao.GetAll();
                return PartialView(viewModel);
            }


            viewModel.IsClientOrigen = true;
            return base.Edit(viewModel);
        }
        [HttpPost]
        public ActionResult CreateDestino(ClientViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Ubications = _ubicatioViewModelMapper.Map(_ubicationDao.GetAll());
                ViewBag.Providers = _taxConditioDao.GetAll();
                ViewBag.Users = _usersDao.GetAll();
                
                return PartialView(viewModel);
            }
                

            viewModel.IsClientDestination = true;
            return base.Create(viewModel);
        }
        [HttpPost]
        public ActionResult CreateOrigen(ClientViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Providers = _taxConditioDao.GetAll();
                ViewBag.Users = _usersDao.GetAll();
                return PartialView(viewModel);
            }

            viewModel.IsClientOrigen = true;
            return base.Create(viewModel);
        }
        public override ActionResult GridInfo([DataSourceRequest] DataSourceRequest request, string filtro)
        {
            var result = _iclientDao.GetAllClients(filtro).ToDataSourceResult(request, ViewModelMapper.Map);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public virtual ActionResult GridInfoDestino([DataSourceRequest] DataSourceRequest request, string filtro)
        {
            var result = _iclientDao.GetAllDestino(filtro).ToDataSourceResult(request, ViewModelMapper.Map);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public virtual ActionResult GridInfoOrigen([DataSourceRequest] DataSourceRequest request, string filtro)
        {
            var result = _iclientDao.GetAllOrigen(filtro).ToDataSourceResult(request, ViewModelMapper.Map);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public override ActionResult Create()
        {
            

            ViewBag.Providers = _taxConditioDao.GetAll();
            ViewBag.Users = _usersDao.GetAll();
            return base.Create();
        }
        public ActionResult CreateDestino()
        {

            ViewBag.Ubications = _ubicatioViewModelMapper.Map(_ubicationDao.GetAll());
            ViewBag.Providers = _taxConditioDao.GetAll();
            ViewBag.Users = _usersDao.GetAll();
            return PartialView();
        }
        public ActionResult CreateOrigen()
        {
            

            ViewBag.Providers = _taxConditioDao.GetAll();
            ViewBag.Users = _usersDao.GetAll();
            return PartialView();
        }

     public override ActionResult Edit(int id)
     {
         var mode = _clientDao.Get(id);
        
        
             ViewBag.Action = "Edit";
             ViewBag.title = "Cuenta";
        
         ViewBag.Providers = _taxConditioDao.GetAll();
            ViewBag.Users = _usersDao.GetAll();

            return base.Edit(id);
        }
     public  ActionResult EditOrigen(int id)
     {
         var mode = _clientDao.Get(id);
        
             ViewBag.Action = "EditOrigen";
             ViewBag.IsOrigen = true;
             ViewBag.title = "Origen";
        
         ViewBag.Providers = _taxConditioDao.GetAll();
         ViewBag.Users = _usersDao.GetAll();
      
         return base.Edit(id);
     }

        public override ActionResult Edit(ClientViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Providers = _taxConditioDao.GetAll();
                ViewBag.Users = _usersDao.GetAll();
                return PartialView(viewModel);
            }

            return base.Edit(viewModel);
        }

        public  ActionResult EditDestino(int id)
     {
         var mode = _clientDao.Get(id);
         ViewBag.Ubications = _ubicatioViewModelMapper.Map(_ubicationDao.GetAll());
             ViewBag.Action = "EditDestino";
             ViewBag.IsDestination = true;
             ViewBag.title = "Destino";
         
        
         ViewBag.Providers = _taxConditioDao.GetAll();
         ViewBag.Users = _usersDao.GetAll();

         return base.Edit(id);
     }
        public override ActionResult Index()
        {
            
            if (User.Identity.IsAuthenticated)
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
                var module = user.UserInfromation.UserRol.ListModulesActions.FirstOrDefault(v => v.Module.Name == "Cuentas");
                if (module!=null)
                {
                   ViewBag.editar=module.Actions.Any(s=>s.Description=="Editar");
                   ViewBag.borrar=module.Actions.Any(s=>s.Description=="Borrar");
                   ViewBag.crear=module.Actions.Any(s=>s.Description=="Crear");
                   ViewBag.ver=module.Actions.Any(s=>s.Description=="Ver");
                   ViewBag.contrato = module.Actions.Any(s => s.Description == "ContratoCliente");
                   ViewBag.imprimir=module.Actions.Any(s=>s.Description=="Imprimir");
                }
                else
                {
                    ViewBag.editar = false;
                    ViewBag.borrar = false;
                    ViewBag.crear = false;
                    ViewBag.ver = false;
                    ViewBag.contrato = false;
                    ViewBag.imprimir = false;
                }
            }
            ViewBag.Title = "Cuentas";
            return PartialView();
        }
        public ActionResult _Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
                var module = user.UserInfromation.UserRol.ListModulesActions.FirstOrDefault(v => v.Module.Name == "Destino");
                if (module != null)
                {
                    ViewBag.editar = module.Actions.Any(s => s.Description == "Editar");
                    ViewBag.borrar = module.Actions.Any(s => s.Description == "Borrar");
                    ViewBag.crear = module.Actions.Any(s => s.Description == "Crear");
                    ViewBag.ver = module.Actions.Any(s => s.Description == "Ver");
                    ViewBag.imprimir = module.Actions.Any(s => s.Description == "Imprimir");
                }
                else
                {
                    ViewBag.editar = false;
                    ViewBag.borrar = false;
                    ViewBag.crear = false;
                    ViewBag.ver = false;
                    ViewBag.imprimir = false;
                }
            }
            ViewBag.Title = "Cuentas Destino";
            return PartialView();
        }
        public ActionResult _IndexOrigen()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
                var module = user.UserInfromation.UserRol.ListModulesActions.FirstOrDefault(v => v.Module.Name == "Origen");
                if (module != null)
                {
                    ViewBag.editar = module.Actions.Any(s => s.Description == "Editar");
                    ViewBag.borrar = module.Actions.Any(s => s.Description == "Borrar");
                    ViewBag.ver = module.Actions.Any(s => s.Description == "Ver");
                    ViewBag.crear = module.Actions.Any(s => s.Description == "Crear");
                    ViewBag.imprimir = module.Actions.Any(s => s.Description == "Imprimir");
                }
                else
                {
                    ViewBag.editar = false;
                    ViewBag.borrar = false;
                    ViewBag.crear = false;
                    ViewBag.ver = false;
                    ViewBag.imprimir = false;
                }
            }
            ViewBag.Title = "Cuentas Origen";
            return PartialView();
        }
	}
}