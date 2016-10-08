using System;
using System.Linq;
using System.Web.Mvc;
using BassinoBase.Models.ViewModelMapper.Interface;
using Domain.Misc;
using BassinoLibrary.ViewModels;
using BassinoLibrary.ViewModels;
using Domain.Products;
using Microsoft.AspNet.Identity;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;

namespace BassinoBase.Controllers
{ 
    public class ServicesController : AbmController<Services, ServicesViewModel, ServicesViewModel>
    {
        private readonly IServicesDao _servicesDao;

        public ServicesController(IServicesViewModelMapper servicesViewModelMapper, IServicesDao servicesDao, IUnitOfWorkHelper unitOfWorkHelper, IControllerBehabior abmControllerBahavior, IServicesDao servicesDao1)
			: base(abmControllerBahavior, servicesDao, servicesViewModelMapper, unitOfWorkHelper)
        {
            _servicesDao = servicesDao1;
        }
        public override void BeforeDelete(Services model)
        {
            model.DeleteUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
            model.DateDelete = DateTime.Now;
            model.IsDelete = true;
            base.BeforeDelete(model);
        }
        public override void BeforeSave(Services model, ServicesViewModel viewModel, bool isNew)
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
        public override ActionResult Index()
        {
            return PartialView();
        }

        public JsonResult GetServices()
        {

            var ubications = _servicesDao.GetAll();
            return Json(
                      ubications.Select(x => new
                      {
                          Name = x.Description,
                          id = x.Id
                      }), JsonRequestBehavior.AllowGet);
         }
	}
}