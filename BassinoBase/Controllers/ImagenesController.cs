using System;
using System.Linq;
using BassinoBase.Models.ViewModelMapper.Interfaces;
using Domain.Products;
using BassinoLibrary.ViewModels;
using BassinoLibrary.ViewModels;
using Domain.Misc;
using Microsoft.AspNet.Identity;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;

namespace BassinoBase.Controllers
{ 
    public class ImagenesController : AbmController<Imagenes, ImagenesViewModel, ImagenesViewModel>
    {
        private readonly IImagenesDao _imagenesDao;

        public ImagenesController(IImagenesViewModelMapper imagenesViewModelMapper, IImagenesDao imagenesDao, IUnitOfWorkHelper unitOfWorkHelper, IControllerBehabior abmControllerBahavior, IImagenesDao imagenesDao1)
			: base(abmControllerBahavior, imagenesDao, imagenesViewModelMapper, unitOfWorkHelper)
        {
            _imagenesDao = imagenesDao1;
        }

        public override void BeforeDelete(Imagenes model)
        {
            model.DeleteUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
            model.DateDelete = DateTime.Now;
            model.IsDelete = true;
            base.BeforeDelete(model);
        }
        private int GetLastSecuence()
        {
            var model = _imagenesDao.GetAll().Where(r => !r.IsDelete).OrderByDescending(r => r.DateCreate).FirstOrDefault();
            if (model != null)
            {
                return (int)(model.Secuencia + 1);

            }
            return 1;
        }

        public override void BeforeSave(Imagenes model, ImagenesViewModel viewModel, bool isNew)
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
	}
}