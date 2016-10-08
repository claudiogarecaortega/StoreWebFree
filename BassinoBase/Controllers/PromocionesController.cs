using BassinoBase.Models.ViewModelMapper.Interfaces;
using Domain.Products;
using BassinoLibrary.ViewModels;
using BassinoLibrary.ViewModels;

using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;

namespace BassinoBase.Controllers
{ 
    public class PromocionesController : AbmController<Promociones, PromocionesViewModel, PromocionesViewModel>
    {
		public PromocionesController(IPromocionesViewModelMapper promocionesViewModelMapper, IPromocionesDao promocionesDao, IUnitOfWorkHelper unitOfWorkHelper, IControllerBehabior abmControllerBahavior)
			: base(abmControllerBahavior, promocionesDao, promocionesViewModelMapper, unitOfWorkHelper)
        {
        }
	}
}