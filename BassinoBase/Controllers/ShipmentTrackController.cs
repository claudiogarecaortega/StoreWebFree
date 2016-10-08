using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;
using BassinoBase.Hubs;
using BassinoBase.Models.ViewModelMapper.Interface;
using Domain.Products;
using BassinoLibrary.ViewModels;
using BassinoLibrary.ViewModels;
using Domain.Misc;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;

namespace BassinoBase.Controllers
{
    public class ShipmentTrackController : AbmController<ShipmentTrack, ShipmentTrackViewModel, ShipmentTrackViewModel>
    {
        private readonly IShipmentDao _shipmentDao;
        private readonly IShipmentViewModelMapper _shipViewModelMapper;
        private readonly IShipmentTrackDao _dao;
        private readonly IShipmentTrackViewModelMapper _modelMapper;
        private readonly IInboundDao _inboudDao;
        private readonly INotificationDao _notificacionDao;
        private readonly IUserdDao _usersDao;
        private readonly IShipmentTrackDao _shiptrackDao;
        private readonly IShipmentTrackViewModelMapper _shiTrackViewModelMapper;

        public ShipmentTrackController(IShipmentTrackViewModelMapper shipmenttrackViewModelMapper, IShipmentTrackDao shipmenttrackDao, IUnitOfWorkHelper unitOfWorkHelper, IControllerBehabior abmControllerBahavior, IShipmentDao shipmentDao, IShipmentViewModelMapper shipViewModelMapper, IShipmentTrackDao dao, IShipmentTrackViewModelMapper modelMapper, IInboundDao inboudDao, INotificationDao notificacionDao, IUserdDao usersDao, IShipmentTrackDao shiptrackDao, IShipmentTrackViewModelMapper shiTrackViewModelMapper)
            : base(abmControllerBahavior, shipmenttrackDao, shipmenttrackViewModelMapper, unitOfWorkHelper)
        {
            _shipmentDao = shipmentDao;
            _shipViewModelMapper = shipViewModelMapper;
            _dao = dao;
            _modelMapper = modelMapper;
            _inboudDao = inboudDao;
            _notificacionDao = notificacionDao;
            _usersDao = usersDao;
            _shiptrackDao = shiptrackDao;
            _shiTrackViewModelMapper = shiTrackViewModelMapper;
        }

        public ActionResult ShipTrackView(int id)
        {
            var model = _shipmentDao.Get(id);
            var viewModel = _shipViewModelMapper.Map(model);
            return PartialView(viewModel);

        }

        public override void BeforeSave(ShipmentTrack model, ShipmentTrackViewModel viewModel, bool isNew)
        {
            if (isNew)
            {
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

        public JsonResult CheckDestination(int id, int idUbication)
        {
            var model = _shipmentDao.Get(id);
            var ubication = model.UbicationRoute.FirstOrDefault(x => x.Id == idUbication);
            var inboub = model.Cargars;
            if (ubication != null)
            {
                var carga = inboub.FirstOrDefault(x => x.ClientTo.Ubication.Id == ubication.Id && !x.IsDelivered);
                return Json(
 (new
 {
     descriptionUbication = ubication.DescripcionCompleta(),
     descriptionCarga = carga.Description,
     cargaId = carga.Id,
     cargaFinal = false,
     state = true
 }), JsonRequestBehavior.AllowGet);
            }
            else if (model.UbicationTo.Id == idUbication)
            {
                var carga = inboub.FirstOrDefault(x => x.ClientTo.Ubication.Id == idUbication && !x.IsDelivered);
                return Json(
(new
{
    descriptionUbication = model.UbicationTo.DescripcionCompleta(),
    descriptionCarga = carga.Description,
    cargaId = carga.Id,
    cargaFinal = true,
    state = true
}), JsonRequestBehavior.AllowGet);
            }

            return Json(
 (new
 {
     descriptionUbication = "",
     descriptionCarga = "",
     state = false
 }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Receive(int id)
        {
            var mode = _shipmentDao.Get(id);
            ViewBag.Cargas = mode.Cargars.Where(s=>!s.IsDelivered);
            if (!CheckStatus(id))
                ViewBag.mesage = "El item se encuentra en bodega por lo tanto no se puede recibir nuevamente";

            if (!CheckSPendinClose(id))
                ViewBag.mesage = "El item no tienen paquetes para ser enviados esta pendiente para ser cerrado";
            if (CheckFinish(id))
                ViewBag.mesage = "El item se encuentra finalizado no puede enviarlo";

            ViewBag.shipId = id;
            return PartialView();
        }

        [HttpPost]
        public ActionResult Send(ShipmentTrackViewModel viewModel)
        {
            GenericCreate(viewModel, true);
            viewModel.IsSended = true;
            var model=_shiptrackDao.Create();
            _shiTrackViewModelMapper.Map(viewModel,model);
            _shiptrackDao.Add(model);
            _shiptrackDao.Save();
            var idNotificatio = CreateNotificacionSendFinish(model, true, false);
            var id = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
            Hubs.NoficationsHub j = new NoficationsHub();
            j.SendMessage(idNotificatio, id);

            return JavaScript(ABMControllerBehavior.MensajeOperacionExitosaModal(MensajeOperacionExitosa));
        }
        public ActionResult Send(int id)
        {

            if (CheckStatus(id))
                ViewBag.mesage = "El item se encuentra en viaje por lo tanto no se puede enviar nuevamente";

            if (!CheckSPendinClose(id))
                ViewBag.mesage = "El item no tienen paquetes para ser enviados esta pendiente para ser cerrado";
            if (CheckFinish(id))
                ViewBag.mesage = "El item se encuentra finalizado no puede enviarlo";

            ViewBag.shipId = id;
            return PartialView();
        }

        [HttpPost]
        public ActionResult Receive(ShipmentTrackViewModel viewModel)
        {
            string name = Request.Params["CargasRecibidas"];
           // var s = collection["Cargas"];
            var ve = name.Split('-');
            GenericCreate(viewModel, false);
            viewModel.IsReceived = true;
            var model = _shiptrackDao.Create();
            _shiTrackViewModelMapper.Map(viewModel, model);
            _shiptrackDao.Add(model);
            _shiptrackDao.Save();
            var mode=model.Shipment.Cargars.FirstOrDefault(u => u.ClientTo.Ubication.Id == viewModel.UbicationId);
            if (mode != null)
            {
                if (ve.Contains(mode.Id.ToString()))
                {
                    var car = _inboudDao.Get(mode.Id);
                    car.IsDelivered = true;
                    _inboudDao.Save();
                }
            }
            var idNotificatio = CreateNotificacionSendFinish(model, false, true);
            var id = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
            Hubs.NoficationsHub j = new NoficationsHub();
            j.SendMessage(idNotificatio, id);
            return JavaScript(ABMControllerBehavior.MensajeOperacionExitosaModal(MensajeOperacionExitosa));
           // return base.Create(viewModel);
        }

        public override void AfterSave(ShipmentTrack model, ShipmentTrackViewModel viewModel, bool isNew)
        {
            if (viewModel.CargaId > 0)
            {
                var modelCarga = _inboudDao.Get(viewModel.CargaId);
                modelCarga.IsDelivered = true;
                _inboudDao.Save();
            }

        }

        public void GenericCreate(ShipmentTrackViewModel viewModel, bool send)
        {
            var modelo = _shipmentDao.Get(viewModel.ShipmentId);
            if (send)
                modelo.IsTraveling = true;
            else
            {
                modelo.IsTraveling = false;

            }
            _dao.Save();

        }

        public bool CheckStatus(int id)
        {
            var modelo = _shipmentDao.Get(id);
            return modelo.IsTraveling;
        }
        public bool CheckFinish(int id)
        {
            var modelo = _shipmentDao.Get(id);
            return modelo.IsFinishig;
        }
        public bool CheckSPendinClose(int id)
        {
            var modelo = _shipmentDao.Get(id);
            return modelo.Cargars.Any(x => x.IsDelivered == false);
        }
        public override ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
                var module = user.UserInfromation.UserRol.ListModulesActions.FirstOrDefault(v => v.Module.Name == "CargasStatus");
                if (module != null)
                {
                    ViewBag.editar = module.Actions.Any(s => s.Description == "Editar");
                    ViewBag.borrar = module.Actions.Any(s => s.Description == "Borrar");
                    ViewBag.crear = module.Actions.Any(s => s.Description == "Crear");
                }
                else
                {
                    ViewBag.editar = false;
                    ViewBag.borrar = false;
                    ViewBag.crear = false;
                }
            }
            ViewBag.Title = "Estado de la Mercaderia";
            return PartialView();
        }
        private int CreateNotificacionSendFinish(ShipmentTrack Model, bool send, bool finish)
        {
            var model = _notificacionDao.Create();
            model.IdProduct = Model.Shipment.Id;
            var user = UserManager.FindById(User.Identity.GetUserId());
            //
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            model.Url = "/" + controllerName + "/ShipTrackView/" + model.IdProduct;
            model.IsForAllUSers = false;
            model.Importance = "4";
            var action = "";

            if (send)
                action = "El viaje numero " + Model.Shipment.Id + " ha llegado a : " + Model.Ubication.Description + "  y fue recibido por : " + user.UserInfromation.FullName;
            if (finish)
                action = "El viaje numero " + Model.Shipment.Id + " ha salido de : " + Model.Ubication.Description + "  y fue enviado por : " + user.UserInfromation.FullName;

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
        public bool ValidDate(string date, string id)
        {
            var dateTrack = Convert.ToDateTime(date + " " + DateTime.Now.ToLongTimeString());
            var shipmentTracks = _shipmentDao.Get(Convert.ToInt32(id)).Tracks;
            if (shipmentTracks != null)
            {
                var firstOrDefault = shipmentTracks.OrderByDescending(x => x.DateTrack).FirstOrDefault();
                if (firstOrDefault != null)
                {
                    var datetracks = firstOrDefault.DateTrack;
                    if (datetracks <= dateTrack)
                        return true;

                    return false;
                }

                return true;
            }

            return true;
        }
    }
}