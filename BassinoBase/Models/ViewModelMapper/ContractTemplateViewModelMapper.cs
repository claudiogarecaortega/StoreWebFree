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
    public class ContractTemplateViewModelMapper : ViewModelMapper<ContractTemplate, ContractTemplateViewModel, ContractTemplateViewModel>, IContractTemplateViewModelMapper
    {
	 private IApplicationDbContext _dbContext;

        public ContractTemplateViewModelMapper(IApplicationDbContext dbContext)
        {
            Mapper.CreateMap<ContractTemplate, ContractTemplateViewModel>();
            Mapper.CreateMap<ContractTemplateViewModel, ContractTemplate>();
        }
		  public override ContractTemplateViewModel Map(ContractTemplate model)
        {
            var viewModel = base.Map(model);
			return viewModel;
        }

        public override void Map(ContractTemplateViewModel viewModel, ContractTemplate model)
        {
            base.Map(viewModel, model);

            //this.Set(hospital => model.UserContact = hospital, viewModel.ContactId, UserExtendedDao);
        }

        public override IEnumerable<ContractTemplateViewModel> Map(IEnumerable<ContractTemplate> models)
        {
            return models.Select(Map);
        }
    }
}