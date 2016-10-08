using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using BassinoBase.Models.ViewModelMapper.Interface;
using Interfaz.Controllers;
using Microsoft.AspNet.Identity.Owin;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;

namespace BassinoBase.Controllers
{
    public abstract class AbmController<TModel, TViewModel, TCommonModel> : GeneralAbmController<TModel, TViewModel, TCommonModel>
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        protected AbmController(IControllerBehabior abmControllerBehavior, IDao<TModel> dao, IViewModelMapper<TModel, TViewModel, TCommonModel> viewModelMapper, IUnitOfWorkHelper unitOfWorkHelper)
            : base(abmControllerBehavior, dao, viewModelMapper, unitOfWorkHelper)
        {
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
                return PartialView(viewModel);

            ABMControllerBehavior.EditModel(this, viewModel, DAO, ViewModelMapper);

            return JavaScript(ABMControllerBehavior.MostrarNotificacionAlert(MensajeOperacionExitosa, 4));
        }

        public virtual ActionResult Delete(int id)
        {
            var modelo = DAO.Get(id);

            return PartialView("Delete", ViewModelMapper.Map(modelo));
        }
        //public string GetDisplayName<TModel, TProperty>(TModel model, Expression<Func<TModel, TProperty>> expression)
        //{

        //    Type type = typeof(TModel);

        //    MemberExpression memberExpression = (MemberExpression)expression.Body;
        //    string propertyName = ((memberExpression.Member is PropertyInfo) ? memberExpression.Member.Name : null);

        //    // First look into attributes on a type and it's parents
        //    DisplayAttribute attr;
        //    attr = (DisplayAttribute)type.GetProperty(propertyName).GetCustomAttributes(typeof(DisplayAttribute), true).SingleOrDefault();

        //    // Look for [MetadataType] attribute in type hierarchy
        //    // http://stackoverflow.com/questions/1910532/attribute-isdefined-doesnt-see-attributes-applied-with-metadatatype-class
        //    if (attr == null)
        //    {
        //        MetadataTypeAttribute metadataType = (MetadataTypeAttribute)type.GetCustomAttributes(typeof(MetadataTypeAttribute), true).FirstOrDefault();
        //        if (metadataType != null)
        //        {
        //            var property = metadataType.MetadataClassType.GetProperty(propertyName);
        //            if (property != null)
        //            {
        //                attr = (DisplayAttribute)property.GetCustomAttributes(typeof(DisplayNameAttribute), true).SingleOrDefault();
        //            }
        //        }
        //    }
        //    return (attr != null) ? attr.Name : String.Empty;


        //}
        [HttpPost]
        //public virtual ActionResult Delete(TViewModel viewModel)
        public virtual ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
               // viewModel.IsDelete = true;
                this.ABMControllerBehavior.Delete(this, DAO, id);

                return JavaScript(ABMControllerBehavior.MostrarNotificacionAlert(MensajeOperacionExitosa,4));
            }
            // catch (ViolacionReglaDeNegocioException)
            catch (Exception es)
            {
                return JavaScript(ABMControllerBehavior.MostrarNotificacionAlert("Error el elemento contiene otros relacionados",1));
            }
            //catch (DbUpdateException ex)
            //{
            //    switch (this.DatabaseExceptionManager.GetErrorCode(ex))
            //    {
            //        case ErrorCode.ForeignKey:
            //            return JavaScript(ABMControllerBehavior.MensajeErrorEnForeignKey());
            //    }

            //    throw;
            //}
        }
    }
}