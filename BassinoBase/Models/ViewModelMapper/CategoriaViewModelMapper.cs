using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using Domain.Misc;
using BassinoLibrary.ViewModels;
using BassinoBase.Models.ViewModelMapper.Interfaces;
using BassinoBase.Models.ViewModelMapper;
using Persistence;


namespace BassinoBase.Models.ViewModelMapper
{
    public class CategoriaViewModelMapper : ViewModelMapper<Categoria, CategoriaViewModel, CategoriaViewModel>, ICategoriaViewModelMapper
    {
	 private IApplicationDbContext _dbContext;

        public CategoriaViewModelMapper(IApplicationDbContext dbContext)
        {
            Mapper.CreateMap<Categoria, CategoriaViewModel>();
            Mapper.CreateMap<CategoriaViewModel, Categoria>();
        }
		  public override CategoriaViewModel Map(Categoria model)
        {
            var viewModel = base.Map(model);
			return viewModel;
        }

        public override void Map(CategoriaViewModel viewModel, Categoria model)
        {
            base.Map(viewModel, model);

            //this.Set(hospital => model.UserContact = hospital, viewModel.ContactId, UserExtendedDao);
        }

        public override IEnumerable<CategoriaViewModel> Map(IEnumerable<Categoria> models)
        {
            return models.Select(Map);
        }
    }
}