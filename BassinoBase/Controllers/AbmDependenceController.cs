using System.Web;
using System.Web.Mvc;
using BassinoBase.Models.ViewModelMapper.Interface;
using Domain.IdentificableObject;
using Interfaz.Controllers;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity.Owin;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;

namespace BassinoBase.Controllers
{
    public abstract class AbmDependenceController<TModel, TViewModel, TCommonViewModel> : Controller, IAbmController<TModel, TViewModel>
      where TViewModel : IIdentifiableObject

    {
        protected readonly IControllerBehabior ABMControllerBehavior;
        protected IViewModelMapper<TModel, TViewModel, TCommonViewModel> ViewModelMapper;
        protected IDaoDependence<TModel> DAO;
        protected IUnitOfWorkHelper UnitOfWorkHelper;
        protected string MensajeOperacionExitosa = "ok";
        protected string MensajeOperacionErronea = "no";
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        protected AbmDependenceController(IControllerBehabior abmControllerBehavior, IDaoDependence<TModel> dao, IViewModelMapper<TModel, TViewModel, TCommonViewModel> viewModelMapper, IUnitOfWorkHelper unitOfWorkHelper)
        {
            DAO = dao;
            ViewModelMapper = viewModelMapper;
            UnitOfWorkHelper = unitOfWorkHelper;
            ABMControllerBehavior = abmControllerBehavior;
        }
        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }
        public virtual ActionResult Index(int idDependiente)
        {
            ABMControllerBehavior.Index(this);

            this.ViewBag.IdDependiente = idDependiente;

            return PartialView();
        }

        public virtual ActionResult GridInfo([DataSourceRequest] DataSourceRequest request, int idDependiente, string filtro)
        {
            var result = DAO.GetAllQ(idDependiente, filtro).ToDataSourceResult(request, ViewModelMapper.MapCommon);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult Create(int idDependiente)
        {
            this.ViewBag.IdDependiente = idDependiente;
            return PartialView();
        }

        public virtual ActionResult Details(int id)
        {
            var viewModel = ABMControllerBehavior.Details(id, this, DAO, ViewModelMapper);

            return PartialView(viewModel);
        }

        public virtual ActionResult Edit(int id)
        {
            var viewModel = ABMControllerBehavior.Edit(id, DAO, ViewModelMapper);

            return PartialView(viewModel);
        }

        [HttpPost]
        public virtual ActionResult Edit(TViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Edit");

            ABMControllerBehavior.EditModel(this, viewModel, DAO, ViewModelMapper);

            return JavaScript(ABMControllerBehavior.MensajeOperacionExitosaModal(MensajeOperacionExitosa));
        }

        public virtual ActionResult Delete(int id)
        {
            var modelo = this.DAO.Get(id);

            return PartialView(ViewModelMapper.Map(modelo));
        }

        //[HttpPost]
        //public virtual ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        ABMControllerBehavior.Delete(this, DAO, id);

        //        return JavaScript(ABMControllerBehavior.MensajeOperacionExitosaModal(MensajeOperacionExitosa));
        //    }
        //    catch (Exception es)
        //    {
        //        return JavaScript(ABMControllerBehavior.MensajeErrorValidacionReglaDeNegocioAlBorrar());
        //    }
        //    //catch (DbUpdateException ex)
        //    //{
        //    //    switch (DatabaseExceptionManager.GetErrorCode(ex))
        //    //    {
        //    //        case ErrorCode.ForeignKey:
        //    //            return JavaScript(ABMControllerBehavior.MensajeErrorEnForeignKey());
        //    //    }

        //    //    throw;
        //    //}
        //}

        public virtual void AfterSave(TModel model, TViewModel viewModel, bool isNew) { }
        public virtual void BeforeSave(TModel model, TViewModel viewModel, bool isNew) { }
        public virtual void AfterDelete(TModel model) { }
        public virtual void BeforeDelete(TModel model) { }
    }
}