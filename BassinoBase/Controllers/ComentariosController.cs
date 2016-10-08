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
    public class ComentariosController : AbmController<Comentarios, ComentariosViewModel, ComentariosViewModel>
    {
        private readonly IComentariosDao _comentarioDao;

        public ComentariosController(IComentariosViewModelMapper comentariosViewModelMapper, IComentariosDao comentariosDao, IUnitOfWorkHelper unitOfWorkHelper, IControllerBehabior abmControllerBahavior, IComentariosDao comentarioDao)
			: base(abmControllerBahavior, comentariosDao, comentariosViewModelMapper, unitOfWorkHelper)
        {
            _comentarioDao = comentarioDao;
        }

        public override void BeforeDelete(Comentarios model)
        {
            model.DeleteUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
            model.DateDelete = DateTime.Now;
            model.IsDelete = true;
            base.BeforeDelete(model);
        }
        private int GetLastSecuence()
        {
            var model = _comentarioDao.GetAll().Where(r => !r.IsDelete).OrderByDescending(r => r.DateCreate).FirstOrDefault();
            if (model != null)
            {
                return (int)(model.Secuencia + 1);

            }
            return 1;
        }

        public override void BeforeSave(Comentarios model, ComentariosViewModel viewModel, bool isNew)
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