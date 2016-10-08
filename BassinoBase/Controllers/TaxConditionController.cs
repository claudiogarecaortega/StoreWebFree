using System;
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
    public class TaxConditionController : AbmController<TaxCondition, TaxConditionViewModel, TaxConditionViewModel>
    {
		public TaxConditionController(ITaxConditionViewModelMapper taxconditionViewModelMapper, ITaxConditionDao taxconditionDao, IUnitOfWorkHelper unitOfWorkHelper, IControllerBehabior abmControllerBahavior)
			: base(abmControllerBahavior, taxconditionDao, taxconditionViewModelMapper, unitOfWorkHelper)
        {
        }
        public override void BeforeDelete(TaxCondition model)
        {
            model.DeleteUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
            model.DateDelete = DateTime.Now;
            model.IsDelete = true;
            base.BeforeDelete(model);
        }
        public override void BeforeSave(TaxCondition model, TaxConditionViewModel viewModel, bool isNew)
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
            ViewBag.Title = "Condicion Iva";
            return PartialView();
        }
	}
}