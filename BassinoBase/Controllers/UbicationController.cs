using System;
using System.Linq;
using System.Web.Mvc;
using BassinoBase.Models.ViewModelMapper.Interface;
using Domain.Misc;
using BassinoLibrary.ViewModels;
using BassinoLibrary.ViewModels;
using Microsoft.AspNet.Identity;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;

namespace BassinoBase.Controllers
{ 
    public class UbicationController : AbmController<Ubication, UbicationViewModel, UbicationViewModel>
    {

        private readonly IUbicationDao _ubicationDao;
        private readonly IUbicationViewModelMapper _ubicationViewModelMapper;

		public UbicationController(IUbicationViewModelMapper ubicationViewModelMapper, IUbicationDao ubicationDao, IUnitOfWorkHelper unitOfWorkHelper, IControllerBehabior abmControllerBahavior, IUbicationDao ubicationDao1, IUbicationViewModelMapper ubicationViewModelMapper1)
			: base(abmControllerBahavior, ubicationDao, ubicationViewModelMapper, unitOfWorkHelper)
		{
		    _ubicationDao = ubicationDao1;
		    _ubicationViewModelMapper = ubicationViewModelMapper1;
		}

        public override void BeforeDelete(Ubication model)
        {
            model.DeleteUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
            model.DateDelete = DateTime.Now;
            model.IsDelete = true;
            base.BeforeDelete(model);
        }
        public override void BeforeSave(Ubication model, UbicationViewModel viewModel, bool isNew)
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

        public JsonResult GetProducts()
        {

            var ubications = _ubicationDao.GetAll();
            return Json(
                      ubications.Select(x => new
                      {
                          Name = x.DescripcionCompleta(),
                          id = x.Id
                      }), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAutoComplete(string texto)
        {
            var result = _ubicationDao.GetAutoComplete(texto).Take(10);

            var viewModels = _ubicationViewModelMapper.Map(result);

            return Json(viewModels, JsonRequestBehavior.AllowGet);
        }
        public override ActionResult Index()
        {
            ViewBag.Title = "Ubicacion";
            return PartialView();
        }
	}
}