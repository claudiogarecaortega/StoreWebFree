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
    public class PackageTypeViewModelMapper : ViewModelMapper<PackageType, PackageTypeViewModel, PackageTypeViewModel>, IPackageTypeViewModelMapper
    {
	 private IApplicationDbContext _dbContext;

        public PackageTypeViewModelMapper(IApplicationDbContext dbContext)
        {
            Mapper.CreateMap<PackageType, PackageTypeViewModel>();
            Mapper.CreateMap<PackageTypeViewModel, PackageType>();
        }
		  public override PackageTypeViewModel Map(PackageType model)
        {
            var viewModel = base.Map(model);
			return viewModel;
        }

        public override void Map(PackageTypeViewModel viewModel, PackageType model)
        {
            base.Map(viewModel, model);

            //this.Set(hospital => model.UserContact = hospital, viewModel.ContactId, UserExtendedDao);
        }

        public override IEnumerable<PackageTypeViewModel> Map(IEnumerable<PackageType> models)
        {
            return models.Select(Map);
        }
    }
}