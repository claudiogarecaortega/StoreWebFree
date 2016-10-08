using System.Data;
using BassinoBase.Models.ViewModelMapper.Interface;
using Domain.IdentificableObject;
using Interfaz.Controllers;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;

namespace BassinoBase.Controllers
{
    public enum AccionesEnum
    {
        Ver = 1,
        Editar = 2,
        Crear = 3,
        Borrar = 4,
        Ejecutar = 5
    }
    public class ControllerBehabior :IControllerBehabior
    {

        private readonly IUnitOfWorkHelper _unitOfWorkHelper;

        public ControllerBehabior(IUnitOfWorkHelper unitOfWorkHelper)
        {
            _unitOfWorkHelper = unitOfWorkHelper;
        }

        public virtual void Index<TModel, TViewModel>(IAbmController<TModel, TViewModel> controller)
        {
            controller.ViewBag.PermisoEditar = this.AccesoValido(controller, AccionesEnum.Editar);
            controller.ViewBag.PermisoCrear = this.AccesoValido(controller, AccionesEnum.Crear);
            controller.ViewBag.PermisoBorrar = this.AccesoValido(controller, AccionesEnum.Borrar);
        }

        public virtual bool AccesoValido<TModel, TViewModel>(IAbmController<TModel, TViewModel> controller, AccionesEnum accion)
        {
            return true;
        }

        public virtual TViewModel Details<TModel, TViewModel, TCommonViewModel>(object id, IAbmController<TModel, TViewModel> controller, IDao<TModel> dao, IViewModelMapper<TModel, TViewModel, TCommonViewModel> viewModelMapper)
        {
            controller.ViewBag.PermisoEditar = true;

            var modelo = dao.Get(id);

            var viewModel = viewModelMapper.Map(modelo);

            return viewModel;
        }

        public virtual TViewModel Details<TModel, TViewModel, TCommonViewModel>(object[] id, IAbmController<TModel, TViewModel> controller, IDao<TModel> dao, IViewModelMapper<TModel, TViewModel, TCommonViewModel> viewModelMapper)
        {
            controller.ViewBag.PermisoEditar = true;

            var modelo = dao.Get(id);

            var viewModel = viewModelMapper.Map(modelo);

            return viewModel;
        }

        public virtual TViewModel Edit<TModel, TViewModel, TCommonViewModel>(object id, IDao<TModel> dao, IViewModelMapper<TModel, TViewModel, TCommonViewModel> viewModelMapper)
        {
            var modelo = dao.Get(id);

            var viewModel = viewModelMapper.Map(modelo);

            return viewModel;
        }

        public virtual TViewModel Edit<TModel, TViewModel, TCommonViewModel>(object[] id, IDao<TModel> dao, IViewModelMapper<TModel, TViewModel, TCommonViewModel> viewModelMapper)
        {
            var modelo = dao.Get(id);

            var viewModel = viewModelMapper.Map(modelo);

            return viewModel;
        }

        public virtual TModel CreateModel<TModel, TViewModel, TCommonViewModel>(IAbmController<TModel, TViewModel> controller, TViewModel viewModel, IDao<TModel> dao, IViewModelMapper<TModel, TViewModel, TCommonViewModel> modelMapper)
        {
            var modelo = dao.Create();
            modelMapper.Map(viewModel, modelo);
            dao.Add(modelo);
            
            controller.BeforeSave(modelo, viewModel, true);

            _unitOfWorkHelper.SaveChanges();

            controller.AfterSave(modelo, viewModel, true);

            return modelo;
        }

        public virtual void EditModel<TModel, TViewModel, TCommonViewModel>(IAbmController<TModel, TViewModel> controller, TViewModel viewModel, IDao<TModel> dao, IViewModelMapper<TModel, TViewModel, TCommonViewModel> modelMapper)
        {
            var modelo = dao.GetForEdit(this.GetModelId(viewModel));

            modelMapper.Map(viewModel, modelo);

            controller.BeforeSave(modelo, viewModel, false);

            _unitOfWorkHelper.SaveChanges();

            controller.AfterSave(modelo, viewModel, false);

        }

        public virtual void Delete<TModel, TViewModel>(IAbmController<TModel, TViewModel> controller, IDao<TModel> dao, object id)
        {
            var modelo = dao.GetForDelete(id);

            if (!dao.SePuedeBorrar(modelo))
                throw new DataException();

            controller.BeforeDelete(modelo);
           
           // dao.Delete(modelo);

            controller.AfterDelete(modelo);

            _unitOfWorkHelper.SaveChanges();
        }

        public virtual void Delete<TModel, TViewModel>(IAbmController<TModel, TViewModel> controller, IDao<TModel> dao, object[] id)
        {
            var modelo = dao.GetForDelete(id);

            if (!dao.SePuedeBorrar(modelo))
                throw new DataException();

            controller.BeforeDelete(modelo);

            dao.Delete(modelo);

            controller.AfterDelete(modelo);

            _unitOfWorkHelper.SaveChanges();
        }

        public virtual string GetModuloSeguridad<TModel, TViewModel>(IAbmController<TModel, TViewModel> controller)
        {
            return controller.GetType().Name.Replace("Controller", "");
        }

        public virtual object GetModelId<TViewModel>(TViewModel viewModel)
        {
            var identificableObject = viewModel as IIdentifiableObject;

            if (identificableObject != null)
                return identificableObject.Id;

            var stringIdentificableObject = viewModel as IStringIdentificableObject;
            if (stringIdentificableObject != null)
                return stringIdentificableObject.Id;

            return null;
        }
        public virtual object GetModelSecuencia<TViewModel>(TViewModel viewModel)
        {
            var identificableObject = viewModel as IIdentifiableObject;

            if (identificableObject != null)
                return identificableObject.Secuencia;

            var stringIdentificableObject = viewModel as IStringIdentificableObject;
            if (stringIdentificableObject != null)
                return stringIdentificableObject.Id;

            return null;
        }
        public string MensajeOperacionExitosaLogin(string mensaje)
        {
            return string.Format("mostrarNotificacionLogin('{0}');", mensaje);
        }

        public string MensajeOperacionExitosaModal(string mensaje)
        {
            return string.Format("modalSave('{0}');", mensaje);
        }
        public string MensajeOperacionExitosaModalShowData(string mensaje)
        {
            return string.Format("modalSaveShowData('{0}');", mensaje);
        }
        public string MensajeOperacionExitosa(string mensaje)
        {
            return string.Format("mostrarNotificacion('{0}');", mensaje);
        }
        public string MostrarNotificacionAlert(string mensaje, int type)
        {
            switch (type)
            {
                case 1:
                    return string.Format("alertDanger('{0}');", mensaje);
                    break;
                case 2:
                    return string.Format("alertWarnig('{0}');", mensaje);
                case 3:
                    return string.Format("alertInfo('{0}');", mensaje);
                case 4:
                    return string.Format("alertSuccess('{0}');", mensaje);


            }
            return string.Format("mostrarNotificacionReload('{0}');", mensaje);
        }
        public string MensajeOperacionErronea(string mensaje)
        {
            return string.Format("mostrarError('{0}');", mensaje);
        }
        public string MensajeOperacionExitosaModalRefresAgenda(string mensaje)
        {
            return string.Format("modalSaveRefresAgenda('{0}');", mensaje);
        }
        public string MensajeErrorEnForeignKey()
        {
            return this.MensajeOperacionErronea("No se puede eliminar, tiene elementos relacionados");
        }

        public string MensajeErrorValidacionReglaDeNegocioAlBorrar()
        {
            return this.MensajeOperacionErronea("El elemento no puede ser eliminado");
        }
    }
}