using Domain.Contable;
using BassinoLibrary.ViewModels;
using BassinoLibrary.ViewModels;
using BassinoBase.Models.ViewModelMapper.Interface;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using BassinoBase.Models.ViewModelMapper.Interface;
using BassinoBase.Models.ViewModelMapper.Interfaces;

namespace BassinoBase.Controllers
{ 
    public class CuentasPagarController : AbmController<CuentasPagar, CuentasPagarViewModel, CuentasPagarViewModel>
    {
		public CuentasPagarController(ICuentasPagarViewModelMapper cuentaspagarViewModelMapper, ICuentasPagarDao cuentaspagarDao, IUnitOfWorkHelper unitOfWorkHelper, IControllerBehabior abmControllerBahavior)
			: base(abmControllerBahavior, cuentaspagarDao, cuentaspagarViewModelMapper, unitOfWorkHelper)
        {
        }
	}
}