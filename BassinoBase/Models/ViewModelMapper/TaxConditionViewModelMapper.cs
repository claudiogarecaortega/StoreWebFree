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
    public class TaxConditionViewModelMapper : ViewModelMapper<TaxCondition, TaxConditionViewModel, TaxConditionViewModel>, ITaxConditionViewModelMapper
    {
	 private IApplicationDbContext _dbContext;

        public TaxConditionViewModelMapper(IApplicationDbContext dbContext)
        {
            Mapper.CreateMap<TaxCondition, TaxConditionViewModel>();
            Mapper.CreateMap<TaxConditionViewModel, TaxCondition>();
        }
		  public override TaxConditionViewModel Map(TaxCondition model)
        {
            var viewModel = base.Map(model);
			return viewModel;
        }

        public override void Map(TaxConditionViewModel viewModel, TaxCondition model)
        {
            base.Map(viewModel, model);

            //this.Set(hospital => model.UserContact = hospital, viewModel.ContactId, UserExtendedDao);
        }

        public override IEnumerable<TaxConditionViewModel> Map(IEnumerable<TaxCondition> models)
        {
            return models.Select(Map);
        }
    }
}