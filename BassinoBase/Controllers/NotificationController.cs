using System;
using System.Linq;
using System.Web.Mvc;
using BassinoBase.Models.ViewModelMapper.Interface;
using Domain.Misc;
using BassinoLibrary.ViewModels;
using BassinoLibrary.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;

namespace BassinoBase.Controllers
{ 
    public class NotificationController : AbmController<Notification, NotificationViewModel, NotificationViewModel>
    {
        private readonly INotificationDao _notificationDao;
        private readonly IUserdDao _iUserDao;

        public NotificationController(INotificationViewModelMapper notificationViewModelMapper, INotificationDao notificationDao, IUnitOfWorkHelper unitOfWorkHelper, IControllerBehabior abmControllerBahavior, INotificationDao notificationDao1, IUserdDao iUserDao)
			: base(abmControllerBahavior, notificationDao, notificationViewModelMapper, unitOfWorkHelper)
        {
            _notificationDao = notificationDao1;
            _iUserDao = iUserDao;
        }
        public override void BeforeDelete(Notification model)
        {
            model.DeleteUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
            model.DateDelete = DateTime.Now;
            model.IsDelete = true;
            base.BeforeDelete(model);
        }
        public override ActionResult GridInfo(DataSourceRequest request, string filtro)
        {
            var id = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id;
            var result = _notificationDao.GetNotificationByUser(id).ToDataSourceResult(request, ViewModelMapper.Map);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public override void BeforeSave(Notification model, NotificationViewModel viewModel, bool isNew)
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
        public JsonResult ReadAlert(int id)
        {
            var model = _iUserDao.Get(UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id);
            model.Notifications.FirstOrDefault(c => c.Alert.Id == id && c.User.Id == model.Id).IsRead = true;
            _iUserDao.Save();
            return Json(
(new
{
    cold = true
}), JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetNotification(string text)
        {
            var cargas = _notificationDao.Get(Convert.ToInt32(text));
            if (cargas != null)
            {
                return Json(
 (new
 {
     Id=cargas.Id,
     Message = cargas.Message,
     Url=cargas.Url,
     Importance = cargas.Importance,
     Users = cargas.UserToNotifiy.Select(s=>s.Id).ToArray()
 }), JsonRequestBehavior.AllowGet);
            }


            return Json(cargas, JsonRequestBehavior.AllowGet);

        }
        public override ActionResult Index()
        {
            return PartialView();
        }
	}
}