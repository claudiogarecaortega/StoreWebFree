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
    public class BillViewModelMapper : ViewModelMapper<Bill, BillViewModel, BillViewModel>, IBillViewModelMapper
    {
	 private IApplicationDbContext _dbContext;

        public BillViewModelMapper(IApplicationDbContext dbContext)
        {
            Mapper.CreateMap<Bill, BillViewModel>();
            Mapper.CreateMap<BillViewModel, Bill>();
        }
		  public override BillViewModel Map(Bill model)
        {
            var viewModel = base.Map(model);
			return viewModel;
        }

        public override void Map(BillViewModel viewModel, Bill model)
        {
            base.Map(viewModel, model);

            //this.Set(hospital => model.UserContact = hospital, viewModel.ContactId, UserExtendedDao);
        }

        public override IEnumerable<BillViewModel> Map(IEnumerable<Bill> models)
        {
            return models.Select(Map);
        }
    }
}