using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using Domain.Providers;
using BassinoLibrary.ViewModels;
using BassinoBase.Models.ViewModelMapper;
using BassinoBase.Models.ViewModelMapper.Interface;
using Persistence;
using Persistence.Dao.Interfaces;


namespace BassinoBase.Models.ViewModelMapper
{
    public class ProviderViewModelMapper : ViewModelMapper<Provider, ProviderViewModel, ProviderViewModel>, IProviderViewModelMapper
    {
	 private IApplicationDbContext _dbContext;
        private readonly ITaxConditionDao _taxConditionDao;
        private readonly IUbicationDao _ubicationDao;

        public ProviderViewModelMapper(IApplicationDbContext dbContext, ITaxConditionDao taxConditionDao, IUbicationDao ubicationDao)
        {
            _taxConditionDao = taxConditionDao;
            _ubicationDao = ubicationDao;
            Mapper.CreateMap<Provider, ProviderViewModel>();
            Mapper.CreateMap<ProviderViewModel, Provider>()
             .ForMember(model => model.TaxCondition, opt => opt.Ignore())
             .ForMember(model => model.Ubication, opt => opt.Ignore())
             ;
        }
		  public override ProviderViewModel Map(Provider model)
        {
            var viewModel = base.Map(model);
            if (model.Ubication != null)
            {
                viewModel.UbicationId = model.Ubication.Id;
                viewModel.Ubication = model.Ubication.DescripcionCompleta();
            }

            if (model.TaxCondition != null)
            {
                viewModel.TaxCondition = model.TaxCondition.Description;
                viewModel.TaxConditionId = model.TaxCondition.Id;
            }

			return viewModel;
        }

        public override void Map(ProviderViewModel viewModel, Provider model)
        {
            base.Map(viewModel, model);

            this.Set(provider => model.TaxCondition = provider, viewModel.TaxConditionId, _taxConditionDao);
            this.Set(provider => model.Ubication = provider, viewModel.UbicationId, _ubicationDao);
        }

        public override IEnumerable<ProviderViewModel> Map(IEnumerable<Provider> models)
        {
            return models.Select(Map);
        }
    }
}