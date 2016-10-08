using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BassinoBase.Models.ViewModelMapper.Interface;
using Domain.Misc;
using Domain.Users;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Persistence.Dao.Interfaces;

namespace BassinoBase.Controllers
{
    public class RolesViewModelIndex
    {
        public string Module { get; set; }
        public string[] SubStrings { get; set; }
    }

    public class HomeController : Controller
    {
          private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
         private readonly IUserdDao _userDao;
         private readonly IUserViewModelMapper _userViewModelMapper;
        private readonly IClientDao _clientdao;
        private readonly IShipmentDao _shipmentDao;
        private readonly IMessagingDao _messagingDao;
        private readonly INotificationDao _notificationDao;
        private readonly IAlertDao _alertDao;
        private readonly IPedidosDao _pedidosDao;
        private readonly IVentaDao _ventasDao;
        private readonly IUserdDao _usuarioDao;
        private readonly ICreditoDao _creditosDao;
        private readonly ICuentasPagarDao _pagosDao;
        private readonly ICuentasCobrarDao _cobrosDao;
        private readonly IProductDao _productosDao;
        private readonly IPedidosProductoDao _pedidoProductoDao;


        public HomeController(ApplicationSignInManager _SignInManager, ApplicationUserManager _UserManager, IUserdDao userDao, IUserViewModelMapper userViewModelMapper, IClientDao clientdao, IShipmentDao shipmentDao, IMessagingDao messagingDao, INotificationDao notificationDao, IAlertDao alertDao, IPedidosDao pedidosDao, IVentaDao ventasDao, IUserdDao usuarioDao, ICreditoDao creditosDao, ICuentasPagarDao pagosDao, ICuentasCobrarDao cobrosDao, IProductDao productosDao, IPedidosProductoDao pedidoProductoDao)
        {
           
            _signInManager = _SignInManager;// HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            _userManager = _UserManager;// HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
             _userDao = userDao;
             _userViewModelMapper = userViewModelMapper;
            _clientdao = clientdao;
            _shipmentDao = shipmentDao;
            _messagingDao = messagingDao;
            _notificationDao = notificationDao;
            _alertDao = alertDao;
            _pedidosDao = pedidosDao;
            _ventasDao = ventasDao;
            _usuarioDao = usuarioDao;
            _creditosDao = creditosDao;
            _pagosDao = pagosDao;
            _cobrosDao = cobrosDao;
            _productosDao = productosDao;
            _pedidoProductoDao = pedidoProductoDao;
        }

        public ActionResult Notification()
        {
            return PartialView();
        }
        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }


        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }

        public ActionResult Content()
        {
            if (User.Identity.IsAuthenticated)
            {

                var user = UserManager.FindById(User.Identity.GetUserId());
                ViewBag.UsersBirthday = _userDao.GetBornDates();
                ViewBag.Notifications = _notificationDao.GetNotificationByUser(user.UserInfromation.Id);
                var alertas= _alertDao.GetAlertsByUser(user.UserInfromation.Id);
                var lista = new List<Alert>();
                foreach (var alert in alertas)
                {
                    if(alert.UsersAlert.Any(f=>!f.IsRead && f.User.Id==user.UserInfromation.Id))
                        lista.Add(alert);

                }
                ViewBag.Alerts =lista;
                ViewBag.Pedidos=_pedidosDao.NuevosPedidos();
                ViewBag.Ventas = _ventasDao.VentasSemana();
                ViewBag.Usuarios = _usuarioDao.GetAll().Count();
                ViewBag.Creditos = _creditosDao.GetCreditos();
                ViewBag.Pagos = _pagosDao.CuentasPagarCount();
                ViewBag.Cobros = _cobrosDao.CuentasCobrarCount();
                ViewBag.Productos = _productosDao.GetBajoStock();
                ViewBag.Ordenes = _pedidoProductoDao.Pedidos();


            }
            else
            {
                ViewBag.UsersBirthday = new List<UserExtended>();
                ViewBag.Alerts = new List<Alert>();
                ViewBag.Notifications = new List<Notification>();
                ViewBag.Pedidos = 0;
                ViewBag.Ventas = 0;
                ViewBag.Usuarios = 0;
                ViewBag.Creditos = 0;
                ViewBag.Pagos = 0;
                ViewBag.Cobros = 0;
                ViewBag.Productos = 0;
                ViewBag.Ordenes = 0;
               
            }
            return PartialView();
        }

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
               
                var user=UserManager.FindById(User.Identity.GetUserId());
                 var alertas = _alertDao.GetAlertsByUser(user.UserInfromation.Id);
                var lista = new List<Alert>();
                foreach (var alert in alertas)
                {
                    if (alert.UsersAlert.Any(f => !f.IsRead && f.User.Id == user.UserInfromation.Id))
                        lista.Add(alert);

                }
                var Notificationes = _notificationDao.GetNotificationByUser(user.UserInfromation.Id);
                var listaNotifications = new List<Notification>();
                foreach (var alert in Notificationes)
                {
                    if (alert.UserToNotifiy.Any(f => !f.IsRead && f.User.Id == user.UserInfromation.Id))
                        listaNotifications.Add(alert);

                }
                var notificaciones = _notificationDao.GetNotificationByUser(user.UserInfromation.Id);
                var alerts = lista;
                var cumple = _userDao.GetBornDates();
                ViewBag.UsersBirthday = cumple;
                ViewBag.Notifications = notificaciones;
                ViewBag.Notificaciones = listaNotifications;
                ViewBag.NotiCount = listaNotifications.Count;
                ViewBag.Alerts = alerts;
                ViewBag.FullName = user.UserInfromation.FullName;
                ViewBag.CountAlert = cumple.Count() + alerts.Count();
                ViewBag.userId = user.UserInfromation.Id;
                ViewBag.Position = user.UserInfromation.Position;
                ViewBag.DateResgister = String.Format("{0:ddd, MMM d, yyyy}", user.UserInfromation.DateCreate);
                ViewBag.Imagen = user.UserInfromation.Imagen ?? "/content/images/avatar5.png";
                ViewBag.UsersCount = UserManager.Users.Count();
                ViewBag.CompleteSend = _shipmentDao.GetAll().Count(x => x.IsFinishig);
                ViewBag.Neworders = _shipmentDao.GetAll().Count(x => x.IsSent); ;
                ViewBag.ClientCount = _clientdao.GetAll().Count();
                ViewBag.messages = _messagingDao.GetAllUserUnread(user.UserInfromation.Id);
                ViewBag.messagesCount = _messagingDao.GetAllUserUnread(user.UserInfromation.Id).Count();
                var listaRoles = new List<RolesViewModelIndex>();
                var module = user.UserInfromation.UserRol.Name.Contains("Admin");
                if (module)
                {
                    ViewBag.admin = true;
        

                }
                else
                {
                    ViewBag.admin = false;
                 
                }
                var mroles = user.UserInfromation.UserRol.ListModulesActions.Where(s=>s.Module.ModuleParent==null);
                if (mroles!=null)
                {
                    foreach (var moduleaction in mroles)
                    {
                        var modelidex = new  RolesViewModelIndex();
                        modelidex.Module = moduleaction.Module.Name;
                        modelidex.SubStrings =
                            user.UserInfromation.UserRol.ListModulesActions.Where(
                                d => d.Module.ModuleParent!=null && d.Module.ModuleParent.Id == moduleaction.Module.Id)
                                .Select(m => m.Module.Name)
                                .ToArray();
                        listaRoles.Add(modelidex);
                    }
                    
                }
                ViewBag.roles = listaRoles;
            }
            else
            {
                ViewBag.admin = false;
                ViewBag.UsersBirthday = new List<UserExtended>();
                ViewBag.Alerts = new List<Alert>();
                ViewBag.Notificaciones = new List<Notification>();
                ViewBag.CountAlert = 0;
                ViewBag.NotiCount = 0;
                ViewBag.roles = new List<RolesViewModelIndex>();
                ViewBag.messages = new List<Messaging>();
                ViewBag.messagesCount = 0;
                ViewBag.Imagen = "/content/images/avatar5.png";
            }
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
