using System;
using System.Web.Mvc;
using BassinoBase.Models.ViewModelMapper.Interface;
using Domain.Security;
using BassinoLibrary.ViewModels;
using BassinoLibrary.ViewModels;
using Microsoft.AspNet.Identity;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;

namespace BassinoBase.Controllers
{ 
    public class ActionsController : AbmController<Actions, ActionsViewModel, ActionsViewModel>
    {
		public ActionsController(IActionsViewModelMapper actionsViewModelMapper, IActionsDao actionsDao, IUnitOfWorkHelper unitOfWorkHelper, IControllerBehabior abmControllerBahavior)
			: base(abmControllerBahavior, actionsDao, actionsViewModelMapper, unitOfWorkHelper)
        {
            
        }

        public override void BeforeDelete(Actions model)
        {
            model.DeleteUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
            model.DateDelete = DateTime.Now;
            model.IsDelete = true;
            base.BeforeDelete(model);
        }

        public override void BeforeSave(Actions model, ActionsViewModel viewModel, bool isNew)
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
    }
}