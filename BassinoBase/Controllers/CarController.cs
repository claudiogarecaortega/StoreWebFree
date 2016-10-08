using System;
using System.Linq;
using System.Web.Mvc;
using BassinoBase.Models.ViewModelMapper.Interface;
using Domain.Misc;
using BassinoLibrary.ViewModels;
using BassinoLibrary.ViewModels;
using Domain.Providers;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;

namespace BassinoBase.Controllers
{ 
    public class CarController : AbmController<Car, CarViewModel, CarViewModel>
    {
        private readonly ICarDao _carDao;

        public CarController(ICarViewModelMapper carViewModelMapper, ICarDao carDao, IUnitOfWorkHelper unitOfWorkHelper, IControllerBehabior abmControllerBahavior, ICarDao carDao1)
			: base(abmControllerBahavior, carDao, carViewModelMapper, unitOfWorkHelper)
        {
            _carDao = carDao1;
        }
        public override ActionResult GridInfo([DataSourceRequest] DataSourceRequest request, string filtro)
        {
            var result = _carDao.GetAllQ(filtro).ToDataSourceResult(request, ViewModelMapper.Map);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public override void BeforeDelete(Car model)
        {
            model.DeleteUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
            model.DateDelete = DateTime.Now;
            model.IsDelete = true;
            base.BeforeDelete(model);
        }
        private int GetLastSecuence()
        {
            var model = _carDao.GetAll().Where(r => !r.IsDelete).OrderByDescending(r => r.DateCreate).FirstOrDefault();
            if (model != null)
            {
                return (int)(model.Secuencia + 1);

            }
            return 1;
        }

        public override void BeforeSave(Car model, CarViewModel viewModel, bool isNew)
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
        public JsonResult GetCarInfo(string text)
        {
            var cargas = _carDao.Get(Convert.ToInt32(text));
            if (cargas != null)
            {
                return Json(
                             (new
                             {
                                 id = cargas.Id,
                                 name = cargas.Nombre,
                                 usoFisico = cargas.UsoFisico,
                                description = cargas.Description,
                                patente = cargas.Patente,
                                kilos = cargas.KiloMaximo,
                                palet = cargas.PalletMaximo,

                             }), JsonRequestBehavior.AllowGet);
            }


            return Json(cargas, JsonRequestBehavior.AllowGet);

        }
        public override ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
                var module = user.UserInfromation.UserRol.ListModulesActions.FirstOrDefault(v => v.Module.Name == "Car");
                if (module != null)
                {
                    ViewBag.editar = module.Actions.Any(s => s.Description == "Editar");
                    ViewBag.borrar = module.Actions.Any(s => s.Description == "Borrar");
                    ViewBag.crear = module.Actions.Any(s => s.Description == "Crear");
                    ViewBag.Ver = module.Actions.Any(s => s.Description == "Ver");
                }
                else
                {
                    ViewBag.editar = false;
                    ViewBag.borrar = false;
                    ViewBag.crear = false;
                    ViewBag.Ver = false;
                }
            }
            return PartialView();
        }
    }
}