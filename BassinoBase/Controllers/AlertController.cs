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
    public class AlertController : AbmController<Alert, AlertViewModel, AlertViewModel>
    {
        private readonly IUserdDao _iUserDao;
        private readonly IAlertDao _alerDao;

        public AlertController(IAlertViewModelMapper alertViewModelMapper, IAlertDao alertDao, IUnitOfWorkHelper unitOfWorkHelper, IControllerBehabior abmControllerBahavior, IUserdDao iUserDao, IAlertDao alerDao)
			: base(abmControllerBahavior, alertDao, alertViewModelMapper, unitOfWorkHelper)
        {
            _iUserDao = iUserDao;
            _alerDao = alerDao;
        }

        public override void BeforeDelete(Alert model)
        {
            model.DeleteUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
            model.DateDelete = DateTime.Now;
            model.IsDelete = true;
            base.BeforeDelete(model);
        }

        public override void BeforeSave(Alert model, AlertViewModel viewModel, bool isNew)
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
            model.Alerts.FirstOrDefault(c => c.Alert.Id == id && c.User.Id==model.Id).IsRead = true;
            _iUserDao.Save();
            return Json(
(new
{
    cold = true
}), JsonRequestBehavior.AllowGet);
            
        }

        public override ActionResult GridInfo(DataSourceRequest request, string filtro)
        {
            var id = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id;
            var result = _alerDao.GetAlertsByUser(id).ToDataSourceResult(request, ViewModelMapper.Map);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public override ActionResult Index()
        {
            return PartialView();
        }
    }
}