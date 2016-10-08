using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using Domain.Products;
using BassinoLibrary.ViewModels;
using BassinoBase.Models.ViewModelMapper.Interfaces;
using BassinoBase.Models.ViewModelMapper;
using Persistence;


namespace BassinoBase.Models.ViewModelMapper
{
    public class PromocionesViewModelMapper : ViewModelMapper<Promociones, PromocionesViewModel, PromocionesViewModel>, IPromocionesViewModelMapper
    {
	 private IApplicationDbContext _dbContext;

        public PromocionesViewModelMapper(IApplicationDbContext dbContext)
        {
            Mapper.CreateMap<Promociones, PromocionesViewModel>();
            Mapper.CreateMap<PromocionesViewModel, Promociones>();
        }
		  public override PromocionesViewModel Map(Promociones model)
        {
            var viewModel = base.Map(model);
			return viewModel;
        }

        public override void Map(PromocionesViewModel viewModel, Promociones model)
        {
            base.Map(viewModel, model);

            //this.Set(hospital => model.UserContact = hospital, viewModel.ContactId, UserExtendedDao);
        }

        public override IEnumerable<PromocionesViewModel> Map(IEnumerable<Promociones> models)
        {
            return models.Select(Map);
        }
    }
}