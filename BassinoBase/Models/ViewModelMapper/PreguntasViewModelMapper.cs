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
    public class PreguntasViewModelMapper : ViewModelMapper<Preguntas, PreguntasViewModel, PreguntasViewModel>, IPreguntasViewModelMapper
    {
	 private IApplicationDbContext _dbContext;

        public PreguntasViewModelMapper(IApplicationDbContext dbContext)
        {
            Mapper.CreateMap<Preguntas, PreguntasViewModel>();
            Mapper.CreateMap<PreguntasViewModel, Preguntas>();
        }
		  public override PreguntasViewModel Map(Preguntas model)
        {
            var viewModel = base.Map(model);
			return viewModel;
        }

        public override void Map(PreguntasViewModel viewModel, Preguntas model)
        {
            base.Map(viewModel, model);

            //this.Set(hospital => model.UserContact = hospital, viewModel.ContactId, UserExtendedDao);
        }

        public override IEnumerable<PreguntasViewModel> Map(IEnumerable<Preguntas> models)
        {
            return models.Select(Map);
        }
    }
}