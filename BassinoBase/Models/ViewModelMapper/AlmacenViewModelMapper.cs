using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using Domain.Almacen;
using BassinoLibrary.ViewModels;
using BassinoBase.Models.ViewModelMapper.Interfaces;
using BassinoBase.Models.ViewModelMapper;
using Persistence;
using Persistence.Dao.Interfaces;


namespace BassinoBase.Models.ViewModelMapper
{
    public class AlmacenViewModelMapper : ViewModelMapper<Almacen, AlmacenViewModel, AlmacenViewModel>, IAlmacenViewModelMapper
    {
	 private IApplicationDbContext _dbContext;
        private readonly IUbicationDao _ubicacionDao;

        public AlmacenViewModelMapper(IApplicationDbContext dbContext, IUbicationDao ubicacionDao)
        {
            _ubicacionDao = ubicacionDao;
            Mapper.CreateMap<Almacen, AlmacenViewModel>()
                ;//.ForMember(model => model., opt => opt.Ignore());
            Mapper.CreateMap<AlmacenViewModel, Almacen>()
                .ForMember(model => model.Ubicacion, opt => opt.Ignore());

        }
		  public override AlmacenViewModel Map(Almacen model)
        {
            var viewModel = base.Map(model);
		      if (model.Ubicacion != null)
		      {
		          viewModel.Ubicacion=model.Ubicacion.DescripcionCompleta();
		          viewModel.UbicacionId = model.Ubicacion.Id;
		      }
		      
		      return viewModel;
        }

        public override void Map(AlmacenViewModel viewModel, Almacen model)
        {
            base.Map(viewModel, model);

            this.Set(hospital => model.Ubicacion = hospital, viewModel.UbicacionId, _ubicacionDao);
        }

        public override IEnumerable<AlmacenViewModel> Map(IEnumerable<Almacen> models)
        {
            return models.Select(Map);
        }
    }
}