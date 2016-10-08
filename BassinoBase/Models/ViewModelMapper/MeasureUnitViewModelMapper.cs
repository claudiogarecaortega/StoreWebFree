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
    public class MeasureUnitViewModelMapper : ViewModelMapper<MeasureUnit, MeasureUnitViewModel, MeasureUnitViewModel>, IMeasureUnitViewModelMapper
    {
	 private IApplicationDbContext _dbContext;

        public MeasureUnitViewModelMapper(IApplicationDbContext dbContext)
        {
            Mapper.CreateMap<MeasureUnit, MeasureUnitViewModel>();
            Mapper.CreateMap<MeasureUnitViewModel, MeasureUnit>();
        }
		  public override MeasureUnitViewModel Map(MeasureUnit model)
        {
            var viewModel = base.Map(model);
			return viewModel;
        }

        public override void Map(MeasureUnitViewModel viewModel, MeasureUnit model)
        {
            base.Map(viewModel, model);

            //this.Set(hospital => model.UserContact = hospital, viewModel.ContactId, UserExtendedDao);
        }

        public override IEnumerable<MeasureUnitViewModel> Map(IEnumerable<MeasureUnit> models)
        {
            return models.Select(Map);
        }
    }
}