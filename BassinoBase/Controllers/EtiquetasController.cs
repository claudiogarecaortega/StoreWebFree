using System;
using System.Linq;
using System.Web.Mvc;
using BassinoBase.Models.ViewModelMapper.Interfaces;
using Domain.Misc;
using BassinoLibrary.ViewModels;
using BassinoLibrary.ViewModels;
using Microsoft.AspNet.Identity;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;

namespace BassinoBase.Controllers
{ 
    public class EtiquetasController : AbmController<Etiquetas, EtiquetasViewModel, EtiquetasViewModel>
    {
        private readonly IEtiquetasDao _etiquetasDao;

        public EtiquetasController(IEtiquetasViewModelMapper etiquetasViewModelMapper, IEtiquetasDao etiquetasDao, IUnitOfWorkHelper unitOfWorkHelper, IControllerBehabior abmControllerBahavior, IEtiquetasDao etiquetasDao1)
			: base(abmControllerBahavior, etiquetasDao, etiquetasViewModelMapper, unitOfWorkHelper)
        {
            _etiquetasDao = etiquetasDao1;
        }
        public override void BeforeDelete(Etiquetas model)
        {
            model.DeleteUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
            model.DateDelete = DateTime.Now;
            model.IsDelete = true;
            base.BeforeDelete(model);
        }
        private int GetLastSecuence()
        {
            var model = _etiquetasDao.GetAll().Where(r => !r.IsDelete).OrderByDescending(r => r.DateCreate).FirstOrDefault();
            if (model != null)
            {
                return (int)(model.Secuencia + 1);

            }
            return 1;
        }

        public override void BeforeSave(Etiquetas model, EtiquetasViewModel viewModel, bool isNew)
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
        public JsonResult GetEtiquetas()
        {

            var ubications = _etiquetasDao.GetAll();
            return Json(
                      ubications.Select(x => new
                      {
                          Name = x.Descripcion,
                          id = x.Id
                      }), JsonRequestBehavior.AllowGet);
        }
	}
}