using BassinoBase.Models.ViewModelMapper.Interface;
using Interfaz.Controllers;
using Persistence.Dao.Interfaces;

namespace BassinoBase.Controllers
{
    public interface IControllerBehabior
    {

        void Index<TModel, TViewModel>(IAbmController<TModel, TViewModel> controller);

        TViewModel Details<TModel, TViewModel, TCommonViewModel>(object id, IAbmController<TModel, TViewModel> controller, IDao<TModel> dao,
            IViewModelMapper<TModel, TViewModel, TCommonViewModel> viewModelMapper);

        TViewModel Details<TModel, TViewModel, TCommonViewModel>(object[] id, IAbmController<TModel, TViewModel> controller, IDao<TModel> dao,
            IViewModelMapper<TModel, TViewModel, TCommonViewModel> viewModelMapper);

        TModel CreateModel<TModel, TViewModel, TCommonViewModel>(IAbmController<TModel, TViewModel> controller, TViewModel model, IDao<TModel> dao,
            IViewModelMapper<TModel, TViewModel, TCommonViewModel> modelMapper);

        TViewModel Edit<TModel, TViewModel, TCommonViewModel>(object id, IDao<TModel> dao,
            IViewModelMapper<TModel, TViewModel, TCommonViewModel> viewModelMapper);

        TViewModel Edit<TModel, TViewModel, TCommonViewModel>(object[] id, IDao<TModel> dao,
            IViewModelMapper<TModel, TViewModel, TCommonViewModel> viewModelMapper);

        void EditModel<TModel, TViewModel, TCommonViewModel>(IAbmController<TModel, TViewModel> controller, TViewModel model, IDao<TModel> dao,
            IViewModelMapper<TModel, TViewModel, TCommonViewModel> modelMapper);

        void Delete<TModel, TViewModel>(IAbmController<TModel, TViewModel> controller, IDao<TModel> dao, object id);

        string GetModuloSeguridad<TModel, TViewModel>(IAbmController<TModel, TViewModel> controller);
        object GetModelId<TViewModel>(TViewModel viewModel);
        object GetModelSecuencia<TViewModel>(TViewModel viewModel);
        string MensajeOperacionExitosa(string mensaje);
        string MensajeOperacionExitosaLogin(string mensaje);
        string MensajeOperacionExitosaModal(string mensaje);
        string MensajeOperacionErronea(string mensaje);
        string MensajeOperacionExitosaModalRefresAgenda(string mensaje);
        string MensajeErrorEnForeignKey();
        string MensajeErrorValidacionReglaDeNegocioAlBorrar();
        string MostrarNotificacionAlert(string mensaje, int type);
        string MensajeOperacionExitosaModalShowData(string mensaje);
        bool AccesoValido<TModel, TViewModel>(IAbmController<TModel, TViewModel> controller, AccionesEnum accion);
    }
}
