using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using Domain.Misc;
using BassinoLibrary.ViewModels;
using BassinoBase.Models.ViewModelMapper;
using BassinoBase.Models.ViewModelMapper.Interface;
using Persistence;
using Persistence.Dao.Interfaces;


namespace BassinoBase.Models.ViewModelMapper
{
    public class UbicationViewModelMapper : ViewModelMapper<Ubication, UbicationViewModel, UbicationViewModel>, IUbicationViewModelMapper
    {
	 private IApplicationDbContext _dbContext;
        private readonly IUbicationDao _ubicationDao;

        public UbicationViewModelMapper(IApplicationDbContext dbContext, IUbicationDao ubicationDao)
        {
            _ubicationDao = ubicationDao;
            Mapper.CreateMap<Ubication, UbicationViewModel>();
            Mapper.CreateMap<UbicationViewModel, Ubication>();
        }
		  public override UbicationViewModel Map(Ubication model)
        {
            var viewModel = base.Map(model);
            viewModel.Description = model.Description;
		      viewModel.DescriptionCompleta = model.DescripcionCompleta();
            if (model.UbicationParent != null)
            {
                viewModel.UbicacionPadre = model.UbicationParent.DescripcionCompleta();
                viewModel.UbicacionPadreId = model.UbicationParent.Id;
            }
			return viewModel;
        }

        public override void Map(UbicationViewModel viewModel, Ubication model)
        {
            base.Map(viewModel, model);

            this.Set(ubication => model.UbicationParent = ubication, viewModel.UbicacionPadreId, _ubicationDao);
        }

        public override IEnumerable<UbicationViewModel> Map(IEnumerable<Ubication> models)
        {
            return models.Select(Map);
        }
    }
}