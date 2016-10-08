using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BassinoBase.Hubs;
using BassinoBase.Models.ViewModelMapper.Interface;
using Domain.Products;
using BassinoLibrary.ViewModels;
using Domain.Commodity;
using Domain.Misc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;

namespace BassinoBase.Controllers
{
    public class ShipmentController : AbmController<Shipment, ShipmentViewModel, ShipmentViewModel>
    {
        private readonly IInboundDao _cargasDao;
        private readonly IShipmentDao _shipDao;
        private readonly IUbicationDao _ubicationDao;
        private readonly IShipmentTrackDao _shipTrackDao;
        private readonly INotificationDao _notificacionDao;
        private readonly IUserdDao _usersDao;
        private readonly IClientDao _clientDao;
        private readonly IInboundDao _inboundDao;


        public ShipmentController(IShipmentViewModelMapper shipmentViewModelMapper, IShipmentDao shipmentDao, IUnitOfWorkHelper unitOfWorkHelper, IControllerBehabior abmControllerBahavior, IInboundDao cargasDao, IShipmentDao shipDao, IUbicationDao ubicationDao, IShipmentTrackDao shipTrackDao, INotificationDao notificacionDao, IUserdDao usersDao, IClientDao clientDao, IInboundDao inboundDao)
            : base(abmControllerBahavior, shipmentDao, shipmentViewModelMapper, unitOfWorkHelper)
        {
            _cargasDao = cargasDao;
            _shipDao = shipDao;
            _ubicationDao = ubicationDao;
            _shipTrackDao = shipTrackDao;
            _notificacionDao = notificacionDao;
            _usersDao = usersDao;
            _clientDao = clientDao;
            _inboundDao = inboundDao;
        }
        public override void BeforeDelete(Shipment model)
        {
            model.DeleteUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
            model.DateDelete = DateTime.Now;
            model.IsDelete = true;
            if (!model.IsSent)
            {
                var list = model.Cargars;
                foreach (var item in list)
                {
                    var carga = _cargasDao.Get(item.Id);
                    carga.IsDecline = false;
                    carga.IsDelivered = false;
                    carga.IsUsed = false;
                    _cargasDao.Save();
                }
                model.Cargars = new List<Inbound>();
            }
            base.BeforeDelete(model);
        }

        public ActionResult Anular(int id)
        {
            var action = true;
            var mesage = "Al anular un viaje se deja registro que el viaje se cancelo pero sera igualmente facturado, y a su ves las cargas seran devueltas al deposito.";
            var model = _shipDao.Get(id);
            if (model.IsFinishig)
            {
                mesage = "No es posible anular un viaje que ha finalizado si una de las cargas no fue entregado por favor recurra al modulo Recuperar mercaderia.";
                action =false ;
            }
            ViewBag.mesage = mesage;
            ViewBag.actiones = action;
            ViewBag.cargasList=model.Cargars;
            return base.Details(id);
        }

        [HttpPost]
        public ActionResult Anular(ShipmentViewModel viewModel)
        {
            var model = _shipDao.Get(viewModel.Id);
            var cargas=model.Cargars.Select(d=>d.Id).ToArray();
            foreach (var item in cargas)
            {
                var carga = _inboundDao.Get(item);
                if (!carga.IsDelivered)
                {
                    model.TotalKilos = model.TotalKilos - Convert.ToDecimal(carga.Kilos.Replace('.', ','));
                    model.TotalPakages = model.TotalPakages - 1;
                    carga.IsUsed = false;
                    model.Cargars.Remove(carga);
                    _inboundDao.Save();
                }
            }
            model.IsDecline = true;
            _shipDao.Save();
            return JavaScript(ABMControllerBehavior.MensajeOperacionExitosaModalShowData("El viaje Numero: " + model.Secuencia + " ha sido anulado y las cargas no entregadas han sido devuletas al Deposito"));
        }

        public override ActionResult GridInfo([DataSourceRequest] DataSourceRequest request, string filtro)
        {
            var result = DAO.GetAllQ(filtro).ToDataSourceResult(request, ViewModelMapper.Map);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GridInfoShipFacturacion([DataSourceRequest] DataSourceRequest request, string cuenta, string fechas, bool infecha=false)
        {
            var start =new DateTime();
            var end = new DateTime();
            
            if (infecha)
            {
                var dates = fechas.Split('-');
                 start = Convert.ToDateTime(dates[0].Trim());
                 end = Convert.ToDateTime(dates[1].Trim());
            }
            var result = _shipDao.GetAllAccount(Convert.ToInt32(cuenta),infecha,start,end).ToDataSourceResult(request, ViewModelMapper.Map);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GridInfoShip(DataSourceRequest request, string filtro, bool viaje, bool end, bool init)
        {
            DataSourceResult result = new DataSourceResult();
            if (!viaje && !end && !init)
            {
                 result = _shipDao.GetAllQ(filtro).ToDataSourceResult(request, ViewModelMapper.Map);

            }
            else
            {
                result = _shipDao.GetAllQFiltros(filtro, viaje, end, init).ToDataSourceResult(request, ViewModelMapper.Map);   
            }
             

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public List<SelectListItem> ConvertoListItems(IEnumerable<Inbound> items)
        {
            foreach (var item in items)
            {
            }
            return
                items.Select(item => new SelectListItem { Text = item.Description, Value = item.Id.ToString() })
                    .ToList();
        }
       
        private int CreateNotificationClient(Shipment Model)
        {
            var model = _notificacionDao.Create();
            model.IdProduct = Model.Id;
            var user = UserManager.FindById(User.Identity.GetUserId());
            //
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            model.Url = "/" + controllerName + "/Details/" + model.IdProduct;
            model.IsForAllUSers = false;
            model.Importance = "4";
            var action = "";
           
                if (actionName.Contains("Create"))
                    action = "El viaje numero "+Model.Id+ "ha sido creado por: " + user.UserInfromation.FullName;
                if (actionName.Contains("Edit"))
                    action = "El viaje numero " + Model.Id + "ha sido Editado por: " + user.UserInfromation.FullName;
                if (actionName.Contains("Delete"))
                    action = "El viaje numero " + Model.Id + "ha sido Emilinado por: " + user.UserInfromation.FullName;
            
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
        private int CreateNotificacionSendFinish(Shipment Model,bool send,bool finish)
        {
            var model = _notificacionDao.Create();
            model.IdProduct = Model.Id;
            var user = UserManager.FindById(User.Identity.GetUserId());
            //
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            model.Url = "/" + controllerName + "/Details/" + model.IdProduct;
            model.IsForAllUSers = false;
            model.Importance = "4";
            var action = "";

            if (send)
                action = "El viaje numero " + Model.Id + "ha sido Iniciado por: " + user.UserInfromation.FullName;
            if (finish)
                action = "El viaje numero " + Model.Id + "ha sido finalizado por: " + user.UserInfromation.FullName;
            
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

        public override void BeforeSave(Shipment model, ShipmentViewModel viewModel, bool isNew)
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
            var ship = _shipDao.Get(id);
            var ubications = "";
            foreach (var item in ship.UbicationRoute)
            {
                ubications = ubications + ","+item.Id;

            }
            ViewBag.ubicationroutes = ubications;
            if (ship.IsSent && ship.IsTraveling)
            {
                ViewBag.disable = "disabled";
                ViewBag.message =
                    "el envio se encuentra en transito y no puede ser editado hasta la llegada al siguiente destino";
            }

            else if (ship.IsSent)
            {
                ViewBag.blockEliminar = true;
                ViewBag.message =
                    "el envio se encuentra enviado solo pueden ser agregados mas items no elminar";
            }
            if (ship.IsFinishig)
            {
                ViewBag.disable = "disabled";
                ViewBag.message =
                    "el envio se encuentra en Finalizado y no puede ser editado hasta la llegada al siguiente destino";
            }

            ViewBag.Cargas = ship.Cargars.Select(x => x.Id).ToArray();
            return base.Edit(id);
        }


        public ActionResult SendShip(int id)
        {
            var ship = _shipDao.Get(id);

            if (ship.IsSent || ship.IsTraveling)
            {
                var sendMess = "";
                var mensaje = "El envio se encuentra en estado: ";
                var travMess = "";
                if (ship.IsSent)
                {
                    sendMess = "Enviado y no puede ser enviado nuevamente";
                    mensaje = mensaje + sendMess;
                }
                if (ship.IsTraveling)
                {
                    travMess = "Viaje y no puede ser enviado nuevamente";
                    mensaje = mensaje + " y " + travMess;
                }
                ViewBag.Message = mensaje;

                ViewBag.disable = "disabled";


            }
            if (ship.IsFinishig)
            {
                ViewBag.disable = "disabled";
                ViewBag.Message = "El envio ha sido fializado y no puede realizar mas acciones";


            }
            if(!ship.IsTraveling && !ship.IsSent && !ship.IsTraveling)
            {
                ViewBag.Message = "Recuerde que una ves que se envie no se puede modificar hasta que llege a destino";
            }



            return base.Edit(id);
        }
        public ActionResult FinishShip(int id)
        {

            var mensaje = "En envio se encuentra en estado";
            var ship = _shipDao.Get(id);

            if (!ship.IsSent)
            {
                ViewBag.Message = "No puede Finalizar un envio si no ha sido enviado aun";
                ViewBag.disable = "disabled";
                return base.Details(id);
            }
            else
            {
                if (ship.IsTraveling)
                {
                    ViewBag.Message = "No puede Finalizar un envio que esta en viaje";
                    ViewBag.disable = "disabled";
                    return base.Details(id);
                }
                else
                {
                    if (ship.IsFinishig)
                    {
                        ViewBag.Message = "No puede finalizar el envio por que ya fue finalizado";
                        ViewBag.disable = "disabled";
                        return base.Details(id);

                    }
                }
            }


                ViewBag.Message =
                    "va a finalizar un envio, recuerde que no puede finalizar si el envio no se encuentra en el destino";
                //  return base.Edit(id);
           








            return base.Edit(id);
        }

        public bool CheckFinish(int id)
        {
            var envio = _shipDao.Get(id);
            var ubicaTo = envio.UbicationTo.Id;
            var ubicatrack = envio.Tracks.OrderByDescending(x => x.DateTrack).FirstOrDefault();
            if (ubicatrack != null && ubicatrack.Ubication.Id == ubicaTo)
                return true;

            return false;

        }

        [HttpPost]
        public ActionResult SendShip(ShipmentViewModel viewModel)
        {
            
            var modelo = _shipDao.GetForEdit(viewModel.Id);

            modelo.IsSent = true;

            _shipDao.Save();
            var mode=_shipTrackDao.Create();
            mode.DateTrack = DateTime.Now;
            mode.IsReceived = true;
            mode.Observaciones = "El viaje ha iniciado";
            mode.Shipment = modelo;
            mode.Ubication = modelo.UbicationFrom;
            _shipTrackDao.Add(mode);
            _shipTrackDao.Save();

                var idNotificatio = CreateNotificacionSendFinish(modelo, true, false);
            var id = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
            Hubs.NoficationsHub j = new NoficationsHub();
            j.SendMessage(idNotificatio, id);

            return JavaScript(ABMControllerBehavior.MostrarNotificacionAlert(MensajeOperacionExitosa, 4));
        }
        [HttpPost]
        public ActionResult FinishShip(ShipmentViewModel viewModel)
        {

            var modelo = _shipDao.GetForEdit(viewModel.Id);

            modelo.IsFinishig = true;

            _shipDao.Save();
            var mode = _shipTrackDao.Create();
            mode.DateTrack = DateTime.Now;
            mode.IsReceived = true;
            mode.Observaciones = "El viaje ha Finalizado";
            mode.Shipment = modelo;
            mode.Ubication = modelo.UbicationFrom;
            _shipTrackDao.Add(mode);
            _shipTrackDao.Save();
            var idNotificatio = CreateNotificacionSendFinish(modelo, false, true);
            var id = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
            Hubs.NoficationsHub j = new NoficationsHub();
            j.SendMessage(idNotificatio, id);

            return JavaScript(ABMControllerBehavior.MostrarNotificacionAlert(MensajeOperacionExitosa, 4));
        }
        private int GetLastSecuence()
        {
            var model = _shipDao.GetAll().Where(r => !r.IsDelete).OrderByDescending(r => r.DateCreate).FirstOrDefault();
            if (model != null)
            {
                return (int)(model.Secuencia + 1);

            }
            return 1;
        }
        public override void AfterSave(Shipment model, ShipmentViewModel viewModel, bool isNew)
        {
            foreach (int t in viewModel.Cargars)
            {
                var pro = _cargasDao.Get(t);
                pro.IsUsed = true;
                _cargasDao.Save();
            }
            
            var idNotificatio = CreateNotificationClient(model);
            var id = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
            Hubs.NoficationsHub j = new NoficationsHub();
            j.SendMessage(idNotificatio, id);

            base.AfterSave(model, viewModel, isNew);
        }

        public override ActionResult Create(ShipmentViewModel viewModel)
        {

            string name = Request.Params["TotalKilos"];
            string name2 = Request.Form["TotalKilos"];
            var mode = base.CreateModel(viewModel);
            var mensaje = "se ha creado la orden Numero: " + mode.Secuencia.ToString();
            return JavaScript(ABMControllerBehavior.MensajeOperacionExitosaModalShowData(mensaje));
        }

        public override ActionResult Edit(ShipmentViewModel viewModel)
        {

            return base.Edit(viewModel);
        }
        public override ActionResult Create()
        {
            ViewBag.ubicationroutes = "";
            if (Request.Params.AllKeys.Contains("data"))
            {
                string name = Request.Params["data"];
                var ids = name.Split('-');
                var idcargas = new List<int>();
                for (int i = 0; i < ids.Count(); i++)
                {
                    if (ids[i] != "" && !idcargas.Contains(Convert.ToInt32(ids[i])))
                    {

                        idcargas.Add(Convert.ToInt32(ids[i]));

                    }

                }
                var idubication = new List<int>();
                for (int i = 0; i < ids.Count(); i++)
                {
                    if (ids[i] != "" && !idubication.Contains(Convert.ToInt32(ids[i])))
                    {
                        var mode = _cargasDao.Get(Convert.ToInt32(ids[i]));
                        if (!idubication.Contains(mode.ClientTo.Ubication.Id))
                            idubication.Add(mode.ClientTo.Ubication.Id);

                    }

                }
                ViewBag.ubications = idubication.ToArray();
                ViewBag.cargasLoad = idcargas.ToArray();
            }
            ViewBag.Cargas = ConvertoListItems(_cargasDao.GetAll());
            return base.Create();
        }

        public JsonResult GetUbications()
        {
            var ubications = _ubicationDao.GetAll();
            return Json(
  ubications.Select(x => new
  {
      UbicationName = x.DescripcionCompleta(),
      id = x.Id
  }), JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetProducts(string text, string ubications = null, bool edit = false)
        {
            var cargas = _cargasDao.GetAll().Where(d=>!d.IsDelete);
            if (!string.IsNullOrEmpty(text))
            {
                int[] ids = { (Convert.ToInt32(text)) };
                int[] ids2 = new int[0];
                if (!string.IsNullOrEmpty(ubications) && ubications != "," && ubications.Length > 0)
                {
                    if (ubications.StartsWith(","))
                    {
                        ubications = ubications.Substring(1);
                    }
                    int[] vector = ubications.Split(',').Select(int.Parse).ToArray();
                    ids2 = new int[vector.Length + 1];
                    for (int i = 0; i < vector.Length; i++)
                    {
                        ids2[i] = vector[i];

                    }
                    ids2[(vector.Length - 1) + 1] = Convert.ToInt32(text);

                }
                else
                    ids2 = ids;

                var carg = _cargasDao.GetAllFilter(ids2, edit);
                return Json(
   carg.Select(x => new
   {
       id = x.Id,
       name = x.UbicationDescription()
   }), JsonRequestBehavior.AllowGet);
            }



            //return Json(cargas, JsonRequestBehavior.AllowGet);
            return Json(
    cargas.Select(x => new
    {
        id = x.Id,
        name = x.UbicationDescription()
    }), JsonRequestBehavior.AllowGet);
        }

        public override ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
                var module = user.UserInfromation.UserRol.ListModulesActions.FirstOrDefault(v => v.Module.Name == "GestionCargas");
                if (module != null)
                {
                    ViewBag.editar = module.Actions.Any(s => s.Description == "Editar");
                    ViewBag.borrar = module.Actions.Any(s => s.Description == "Borrar");
                    ViewBag.crear = module.Actions.Any(s => s.Description == "Crear");
                    ViewBag.finalizar = module.Actions.Any(s => s.Description == "ver");
                    ViewBag.ver = module.Actions.Any(s => s.Description == "Finalizar");
                    ViewBag.iniciar = module.Actions.Any(s => s.Description == "Iniciar");
                    ViewBag.recibir = module.Actions.Any(s => s.Description == "Recibir");
                    ViewBag.enviar = module.Actions.Any(s => s.Description == "Enviar");
                    ViewBag.anular = module.Actions.Any(s => s.Description == "Anular");
                }
                else
                {
                    ViewBag.editar = false;
                    ViewBag.borrar = false;
                    ViewBag.crear = false;
                    ViewBag.finalizar = false;
                    ViewBag.iniciar = false;
                    ViewBag.recibir = false;
                    ViewBag.enviar = false;
                    ViewBag.anular = false;
                    ViewBag.ver = false;
                }
            }
            ViewBag.Title = "Recepción de Mercadería";
            return PartialView();
        }

        private IList<SelectListItem> GetItems()
        {
            var list=new List<SelectListItem>(){new SelectListItem(){Value = "0",Text="Selecione una Cuenta"}};
            var lista = _clientDao.GetAll().Where(d=>!d.IsDelete);
            foreach (var item in lista)
            {
                var items = new SelectListItem() { Value = item.Id.ToString(), Text = item.Name };
                list.Add(items);

            }
            return list;
        }

        public  ActionResult IndexFacturacion()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.ListaItems = GetItems();
                var user = UserManager.FindById(User.Identity.GetUserId());
                var module = user.UserInfromation.UserRol.ListModulesActions.FirstOrDefault(v => v.Module.Name == "Facturacion");
                if (module != null)
                {
                    ViewBag.editar = module.Actions.Any(s => s.Description == "Editar");
                    ViewBag.borrar = module.Actions.Any(s => s.Description == "Borrar");
                    ViewBag.crear = module.Actions.Any(s => s.Description == "Crear");
                    ViewBag.anular = module.Actions.Any(s => s.Description == "Restaruar");
                }
                else
                {
                    ViewBag.editar = false;
                    ViewBag.borrar = false;
                    ViewBag.crear = false;
                }
            }
            ViewBag.Title = "Recepción de Mercadería";
            return PartialView();
        }
    }
}