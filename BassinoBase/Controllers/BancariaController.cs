using Domain.Contable;
using BassinoLibrary.ViewModels;
using BassinoLibrary.ViewModels;
using BassinoBase.Models.ViewModelMapper.Interface;
using Persistence.Dao.Interfaces;
using BassinoBase.Models.ViewModelMapper.Interfaces;
using Persistence.UnitsOfWork;

namespace BassinoBase.Controllers
{ 
    public class BancariaController : AbmController<Bancaria, BancariaViewModel, BancariaViewModel>
    {
		public BancariaController(IBancariaViewModelMapper bancariaViewModelMapper, IBancariaDao bancariaDao, IUnitOfWorkHelper unitOfWorkHelper, IControllerBehabior abmControllerBahavior)
			: base(abmControllerBahavior, bancariaDao, bancariaViewModelMapper, unitOfWorkHelper)
        {
        }
	}
}