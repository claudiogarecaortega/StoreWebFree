using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using Domain.Products;
using BassinoLibrary.ViewModels;
using BassinoBase.Models.ViewModelMapper;
using BassinoBase.Models.ViewModelMapper.Interface;
using Domain.Misc;
using Persistence;
using Persistence.Dao.Interfaces;


namespace BassinoBase.Models.ViewModelMapper
{
    public class ProductViewModelMapper : ViewModelMapper<Product, ProductViewModel, ProductViewModel>, IProductViewModelMapper
    {
	 private IApplicationDbContext _dbContext;
        private readonly IProviderDao _providerDao;
        private readonly IMeasureUnitDao _measureUnitDao;
        private readonly ICategoriaDao _categoriaDao;
        private readonly IEtiquetasDao _etiqeutasDao;

        public ProductViewModelMapper(IApplicationDbContext dbContext, IProviderDao providerDao, IMeasureUnitDao measureUnitDao, ICategoriaDao categoriaDao, IEtiquetasDao etiqeutasDao)
        {
            _providerDao = providerDao;
            _measureUnitDao = measureUnitDao;
            _categoriaDao = categoriaDao;
            _etiqeutasDao = etiqeutasDao;
            Mapper.CreateMap<Product, ProductViewModel>();
            Mapper.CreateMap<ProductViewModel, Product>()
                 .ForMember(model => model.Categorias, opt => opt.Ignore())
                 .ForMember(model => model.Etiquetas, opt => opt.Ignore())
                // .ForMember(model => model.Provider, opt => opt.Ignore())
                 ;
        }
		  public override ProductViewModel Map(Product model)
        {
            var viewModel = base.Map(model);

              //if (model.MeasureUnit != null)
              //{
              //    viewModel.MeasureUnitId = model.MeasureUnit.Id;
              //    viewModel.MeasureUnit = model.MeasureUnit.Description;
              //}

              //if (model.Provider != null)
              //{
              //    viewModel.Provider = model.Provider.Description;
              //    viewModel.ProviderId = model.Provider.Id;
              //}
		      viewModel.Etiquetas = model.GetEtiquetasString();
		      viewModel.Categorias = model.GetCategoriasString();
		      if (model.Categorias != null) 
                  viewModel.CategoriasId = model.Categorias.Select(r => r.Id).ToArray();
		      if (model.Etiquetas != null)
                  viewModel.EtiquetasId = model.Etiquetas.Select(r => r.Id).ToArray();
		      viewModel.DescriptionCold = model.IsCold ? "Si" : "No";
		      viewModel.Secuencia = model.Secuencia;
		      return viewModel;
        }

        public override void Map(ProductViewModel viewModel, Product model)
        {
            base.Map(viewModel, model);
            model.Secuencia = model.Secuencia;
            if (model.Categorias == null)
                model.Categorias=new List<Categoria>();

                this.SetCollectionFromMultiSelect(viewModel.CategoriasId,_categoriaDao,model.Categorias);

            if (model.Etiquetas == null)
                model.Etiquetas=new List<Etiquetas>();

                this.SetCollectionFromMultiSelect(viewModel.EtiquetasId, _etiqeutasDao, model.Etiquetas);
            // this.Set(hospital => model.MeasureUnit = hospital, viewModel.MeasureUnitId, _measureUnitDao);
            //this.Set(hospital => model.UserContact = hospital, viewModel.ContactId, UserExtendedDao);
        }

        public override IEnumerable<ProductViewModel> Map(IEnumerable<Product> models)
        {
            return models.Select(Map);
        }
    }
}