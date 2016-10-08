using System.Web.Mvc;
using BassinoBase.Models.ViewModelMapper.Interfaces;
using Domain.Almacen;
using BassinoLibrary.ViewModels;
using BassinoLibrary.ViewModels;

using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;

namespace BassinoBase.Controllers
{ 
    public class AlmacenController : AbmController<Almacen, AlmacenViewModel, AlmacenViewModel>
    {
		public AlmacenController(IAlmacenViewModelMapper almacenViewModelMapper, IAlmacenDao almacenDao, IUnitOfWorkHelper unitOfWorkHelper, IControllerBehabior abmControllerBahavior)
			: base(abmControllerBahavior, almacenDao, almacenViewModelMapper, unitOfWorkHelper)
        {
        }

        public override ActionResult Create(AlmacenViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return PartialView(viewModel);

            return base.Create(viewModel);
        }

        public override ActionResult Index()
        {
            return PartialView();
        }
    }
}