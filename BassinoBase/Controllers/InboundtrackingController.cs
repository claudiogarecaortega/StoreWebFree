using System.Web.Mvc;
using BassinoBase.Models.ViewModelMapper.Interface;
using Domain.Commodity;
using BassinoLibrary.ViewModels;
using BassinoLibrary.ViewModels;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;

namespace BassinoBase.Controllers
{ 
    public class InboundtrackingController : AbmController<InboundTracking, InboundtrackingViewModel, InboundtrackingViewModel>
    {
		public InboundtrackingController(IInboundtrackingViewModelMapper inboundtrackingViewModelMapper, IInboundtrackingDao inboundtrackingDao, IUnitOfWorkHelper unitOfWorkHelper, IControllerBehabior abmControllerBahavior)
			: base(abmControllerBahavior, inboundtrackingDao, inboundtrackingViewModelMapper, unitOfWorkHelper)
        {
        }
        public override ActionResult Index()
        {
            return PartialView();
        }
	}
}