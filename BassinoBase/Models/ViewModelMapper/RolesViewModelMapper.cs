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
    public class RolesViewModelMapper : ViewModelMapper<Roles, RolesViewModel, RolesViewModel>, IRolesViewModelMapper
    {
	 private IApplicationDbContext _dbContext;

        public RolesViewModelMapper(IApplicationDbContext dbContext)
        {
            Mapper.CreateMap<Roles, RolesViewModel>();
            Mapper.CreateMap<RolesViewModel, Roles>();
        }
		  public override RolesViewModel Map(Roles model)
        {
            var viewModel = base.Map(model);
			return viewModel;
        }

        public override void Map(RolesViewModel viewModel, Roles model)
        {
            base.Map(viewModel, model);

            //this.Set(hospital => model.UserContact = hospital, viewModel.ContactId, UserExtendedDao);
        }

        public override IEnumerable<RolesViewModel> Map(IEnumerable<Roles> models)
        {
            return models.Select(Map);
        }
    }
}