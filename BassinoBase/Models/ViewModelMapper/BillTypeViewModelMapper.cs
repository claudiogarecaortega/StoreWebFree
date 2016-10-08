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
    public class BillTypeViewModelMapper : ViewModelMapper<BillType, BillTypeViewModel, BillTypeViewModel>, IBillTypeViewModelMapper
    {
	 private IApplicationDbContext _dbContext;

        public BillTypeViewModelMapper(IApplicationDbContext dbContext)
        {
            Mapper.CreateMap<BillType, BillTypeViewModel>();
            Mapper.CreateMap<BillTypeViewModel, BillType>();
        }
		  public override BillTypeViewModel Map(BillType model)
        {
            var viewModel = base.Map(model);
			return viewModel;
        }

        public override void Map(BillTypeViewModel viewModel, BillType model)
        {
            base.Map(viewModel, model);

            //this.Set(hospital => model.UserContact = hospital, viewModel.ContactId, UserExtendedDao);
        }

        public override IEnumerable<BillTypeViewModel> Map(IEnumerable<BillType> models)
        {
            return models.Select(Map);
        }
    }
}