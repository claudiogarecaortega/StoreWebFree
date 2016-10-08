using System;
using Domain.Providers;
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
    public class PedidosProductoViewModelMapper : ViewModelMapper<PedidosProducto, PedidosProductoViewModel, PedidosProductoViewModel>, IPedidosProductoViewModelMapper
    {
	 private IApplicationDbContext _dbContext;
        private readonly IProviderDao _provedorDao;
        private readonly IProductDao _productoDao;

        public PedidosProductoViewModelMapper(IApplicationDbContext dbContext, IProviderDao provedorDao, IProductDao productoDao)
        {
            _provedorDao = provedorDao;
            _productoDao = productoDao;
            Mapper.CreateMap<PedidosProducto, PedidosProductoViewModel>()
			.ForMember(model => model.Productos, opt => opt.Ignore())
			.ForMember(model => model.FechaDePago, opt => opt.Ignore())
			;
            Mapper.CreateMap<PedidosProductoViewModel, PedidosProducto>()
			.ForMember(model => model.Productos, opt => opt.Ignore())
			.ForMember(model => model.Imagenes, opt => opt.Ignore())
			.ForMember(model => model.Provedor, opt => opt.Ignore())
			;
        }
		  public override PedidosProductoViewModel Map(PedidosProducto model)
        {
            var viewModel = base.Map(model);
		      if (model.Provedor != null)
		      {
		          viewModel.ProvedorId = model.Provedor.Id;
		          viewModel.Provedor = model.Provedor.Description;
		      }
		      if (model.Productos.Any())
		      {
		          viewModel.Productos = model.Productos.Select(e => e.Id).ToArray();
		          viewModel.Productosdescripcion = model.Productos.Aggregate("",
		              (current, item) => current + " - " + item.Description);
		      }
		      return viewModel;
        }

        public override void Map(PedidosProductoViewModel viewModel, PedidosProducto model)
        {

            base.Map(viewModel, model);
            model.FechaDePago = Convert.ToDateTime(viewModel.FechaDePago);
            this.Set(hospital => model.Provedor = hospital, viewModel.ProvedorId, _provedorDao);

            if(model.Productos==null)
                model.Productos=new List<Product>();
           
            SetCollectionFromMultiSelect(viewModel.Productos,_productoDao,model.Productos);
        }

        public override IEnumerable<PedidosProductoViewModel> Map(IEnumerable<PedidosProducto> models)
        {
            return models.Select(Map);
        }
    }
}