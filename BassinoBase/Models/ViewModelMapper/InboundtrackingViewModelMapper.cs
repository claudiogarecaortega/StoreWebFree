using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using Domain.Commodity;
using BassinoLibrary.ViewModels;
using BassinoBase.Models.ViewModelMapper;
using BassinoBase.Models.ViewModelMapper.Interface;
using Persistence;


namespace BassinoBase.Models.ViewModelMapper
{
    public class InboundtrackingViewModelMapper : ViewModelMapper<InboundTracking, InboundtrackingViewModel, InboundtrackingViewModel>, IInboundtrackingViewModelMapper
    {
	 private IApplicationDbContext _dbContext;

        public InboundtrackingViewModelMapper(IApplicationDbContext dbContext)
        {
            Mapper.CreateMap<InboundTracking, InboundtrackingViewModel>();
            Mapper.CreateMap<InboundtrackingViewModel, InboundTracking>();
        }
		  public override InboundtrackingViewModel Map(InboundTracking model)
        {
            var viewModel = base.Map(model);
			return viewModel;
        }

        public override void Map(InboundtrackingViewModel viewModel, InboundTracking model)
        {
            base.Map(viewModel, model);

            //this.Set(hospital => model.UserContact = hospital, viewModel.ContactId, UserExtendedDao);
        }

        public override IEnumerable<InboundtrackingViewModel> Map(IEnumerable<InboundTracking> models)
        {
            return models.Select(Map);
        }
    }
}