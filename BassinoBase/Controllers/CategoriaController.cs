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
    public class CategoriaController : AbmController<Categoria, CategoriaViewModel, CategoriaViewModel>
    {
        private readonly ICategoriaDao _categoriasDao;

        public CategoriaController(ICategoriaViewModelMapper categoriaViewModelMapper, ICategoriaDao categoriaDao, IUnitOfWorkHelper unitOfWorkHelper, IControllerBehabior abmControllerBahavior, ICategoriaDao categoriasDao)
			: base(abmControllerBahavior, categoriaDao, categoriaViewModelMapper, unitOfWorkHelper)
        {
            _categoriasDao = categoriasDao;
        }
        public override void BeforeDelete(Categoria model)
        {
            model.DeleteUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
            model.DateDelete = DateTime.Now;
            model.IsDelete = true;
            base.BeforeDelete(model);
        }
        private int GetLastSecuence()
        {
            var model = _categoriasDao.GetAll().Where(r => !r.IsDelete).OrderByDescending(r => r.DateCreate).FirstOrDefault();
            if (model != null)
            {
                return (int)(model.Secuencia + 1);

            }
            return 1;
        }

        public override void BeforeSave(Categoria model, CategoriaViewModel viewModel, bool isNew)
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
        public JsonResult GetCategorias()
        {

            var ubications = _categoriasDao.GetAll();
            return Json(
                      ubications.Select(x => new
                      {
                          Name = x.Descripcion,
                          id = x.Id
                      }), JsonRequestBehavior.AllowGet);
        }
	}
}