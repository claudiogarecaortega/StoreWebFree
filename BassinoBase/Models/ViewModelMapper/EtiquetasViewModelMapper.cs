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
    public class EtiquetasViewModelMapper : ViewModelMapper<Etiquetas, EtiquetasViewModel, EtiquetasViewModel>, IEtiquetasViewModelMapper
    {
	 private IApplicationDbContext _dbContext;

        public EtiquetasViewModelMapper(IApplicationDbContext dbContext)
        {
            Mapper.CreateMap<Etiquetas, EtiquetasViewModel>();
            Mapper.CreateMap<EtiquetasViewModel, Etiquetas>();
        }
		  public override EtiquetasViewModel Map(Etiquetas model)
        {
            var viewModel = base.Map(model);
			return viewModel;
        }

        public override void Map(EtiquetasViewModel viewModel, Etiquetas model)
        {
            base.Map(viewModel, model);

            //this.Set(hospital => model.UserContact = hospital, viewModel.ContactId, UserExtendedDao);
        }

        public override IEnumerable<EtiquetasViewModel> Map(IEnumerable<Etiquetas> models)
        {
            return models.Select(Map);
        }
    }
}