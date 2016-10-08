using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using Domain.Products;
using BassinoLibrary.ViewModels;
using BassinoBase.Models.ViewModelMapper.Interfaces;
using BassinoBase.Models.ViewModelMapper;
using Persistence;


namespace BassinoBase.Models.ViewModelMapper
{
    public class ComentariosViewModelMapper : ViewModelMapper<Comentarios, ComentariosViewModel, ComentariosViewModel>, IComentariosViewModelMapper
    {
	 private IApplicationDbContext _dbContext;

        public ComentariosViewModelMapper(IApplicationDbContext dbContext)
        {
            Mapper.CreateMap<Comentarios, ComentariosViewModel>();
            Mapper.CreateMap<ComentariosViewModel, Comentarios>();
        }
		  public override ComentariosViewModel Map(Comentarios model)
        {
            var viewModel = base.Map(model);
			return viewModel;
        }

        public override void Map(ComentariosViewModel viewModel, Comentarios model)
        {
            base.Map(viewModel, model);

            //this.Set(hospital => model.UserContact = hospital, viewModel.ContactId, UserExtendedDao);
        }

        public override IEnumerable<ComentariosViewModel> Map(IEnumerable<Comentarios> models)
        {
            return models.Select(Map);
        }
    }
}