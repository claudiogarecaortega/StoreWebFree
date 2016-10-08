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
    public class ImagenesViewModelMapper : ViewModelMapper<Imagenes, ImagenesViewModel, ImagenesViewModel>, IImagenesViewModelMapper
    {
	 private IApplicationDbContext _dbContext;

        public ImagenesViewModelMapper(IApplicationDbContext dbContext)
        {
            Mapper.CreateMap<Imagenes, ImagenesViewModel>();
            Mapper.CreateMap<ImagenesViewModel, Imagenes>();
        }
		  public override ImagenesViewModel Map(Imagenes model)
        {
            var viewModel = base.Map(model);
			return viewModel;
        }

        public override void Map(ImagenesViewModel viewModel, Imagenes model)
        {
            base.Map(viewModel, model);

            //this.Set(hospital => model.UserContact = hospital, viewModel.ContactId, UserExtendedDao);
        }

        public override IEnumerable<ImagenesViewModel> Map(IEnumerable<Imagenes> models)
        {
            return models.Select(Map);
        }
    }
}