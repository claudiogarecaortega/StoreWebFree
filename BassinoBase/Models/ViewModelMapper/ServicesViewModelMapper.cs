using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using Domain.Misc;
using BassinoLibrary.ViewModels;
using BassinoBase.Models.ViewModelMapper;
using BassinoBase.Models.ViewModelMapper.Interface;
using Persistence;


namespace BassinoBase.Models.ViewModelMapper
{
    public class ServicesViewModelMapper : ViewModelMapper<Services, ServicesViewModel, ServicesViewModel>, IServicesViewModelMapper
    {
	 private IApplicationDbContext _dbContext;

        public ServicesViewModelMapper(IApplicationDbContext dbContext)
        {
            Mapper.CreateMap<Services, ServicesViewModel>();
            Mapper.CreateMap<ServicesViewModel, Services>();
        }
		  public override ServicesViewModel Map(Services model)
        {
            var viewModel = base.Map(model);
			return viewModel;
        }

        public override void Map(ServicesViewModel viewModel, Services model)
        {
            base.Map(viewModel, model);

            //this.Set(hospital => model.UserContact = hospital, viewModel.ContactId, UserExtendedDao);
        }

        public override IEnumerable<ServicesViewModel> Map(IEnumerable<Services> models)
        {
            return models.Select(Map);
        }
    }
}