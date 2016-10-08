using Domain.Contable;
using BassinoLibrary.ViewModels;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using Domain.Products;
using BassinoLibrary.ViewModels;
using BassinoBase.Models.ViewModelMapper;
using BassinoBase.Models.ViewModelMapper.Interface;
using BassinoBase.Models.ViewModelMapper.Interfaces;
using Domain.Misc;
using Persistence;
using Persistence.Dao.Interfaces;



namespace BassinoBase.Models.ViewModelMapper
{
    public class BancariaViewModelMapper : ViewModelMapper<Bancaria, BancariaViewModel, BancariaViewModel>, IBancariaViewModelMapper
    {
	 private IApplicationDbContext _dbContext;

        public BancariaViewModelMapper(IApplicationDbContext dbContext)
        {
            Mapper.CreateMap<Bancaria, BancariaViewModel>()
			//.ForMember(model => model.Categorias, opt => opt.Ignore())
			;
            Mapper.CreateMap<BancariaViewModel, Bancaria>()
			//.ForMember(model => model.Categorias, opt => opt.Ignore())
			;
        }
		  public override BancariaViewModel Map(Bancaria model)
        {
            var viewModel = base.Map(model);
			return viewModel;
        }

        public override void Map(BancariaViewModel viewModel, Bancaria model)
        {
            base.Map(viewModel, model);

            //this.Set(hospital => model.UserContact = hospital, viewModel.ContactId, UserExtendedDao);
        }

        public override IEnumerable<BancariaViewModel> Map(IEnumerable<Bancaria> models)
        {
            return models.Select(Map);
        }
    }
}