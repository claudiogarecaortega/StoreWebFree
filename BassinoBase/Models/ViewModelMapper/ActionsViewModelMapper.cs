using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using Domain.Security;
using BassinoLibrary.ViewModels;
using BassinoBase.Models.ViewModelMapper;
using BassinoBase.Models.ViewModelMapper.Interface;
using Persistence;


namespace BassinoBase.Models.ViewModelMapper
{
    public class ActionsViewModelMapper : ViewModelMapper<Actions, ActionsViewModel, ActionsViewModel>, IActionsViewModelMapper
    {
	 private IApplicationDbContext _dbContext;

        public ActionsViewModelMapper(IApplicationDbContext dbContext)
        {
            Mapper.CreateMap<Actions, ActionsViewModel>();
            Mapper.CreateMap<ActionsViewModel, Actions>();
        }
		  public override ActionsViewModel Map(Actions model)
        {
            var viewModel = base.Map(model);
			return viewModel;
        }

        public override void Map(ActionsViewModel viewModel, Actions model)
        {
            base.Map(viewModel, model);

            //this.Set(hospital => model.UserContact = hospital, viewModel.ContactId, UserExtendedDao);
        }

        public override IEnumerable<ActionsViewModel> Map(IEnumerable<Actions> models)
        {
            return models.Select(Map);
        }
    }
}