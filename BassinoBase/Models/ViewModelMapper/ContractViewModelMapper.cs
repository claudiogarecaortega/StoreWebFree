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
    public class ContractViewModelMapper : ViewModelMapper<Contract, ContractViewModel, ContractViewModel>, IContractViewModelMapper
    {
	 private IApplicationDbContext _dbContext;

        public ContractViewModelMapper(IApplicationDbContext dbContext)
        {
            Mapper.CreateMap<Contract, ContractViewModel>();
            Mapper.CreateMap<ContractViewModel, Contract>();
        }
		  public override ContractViewModel Map(Contract model)
        {
            var viewModel = base.Map(model);
			return viewModel;
        }

        public override void Map(ContractViewModel viewModel, Contract model)
        {
            base.Map(viewModel, model);

            //this.Set(hospital => model.UserContact = hospital, viewModel.ContactId, UserExtendedDao);
        }

        public override IEnumerable<ContractViewModel> Map(IEnumerable<Contract> models)
        {
            return models.Select(Map);
        }
    }
}