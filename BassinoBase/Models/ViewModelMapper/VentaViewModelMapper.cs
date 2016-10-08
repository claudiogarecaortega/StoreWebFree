using System;
using Domain.Ventas;
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
    public class VentaViewModelMapper : ViewModelMapper<Venta, VentaViewModel, VentaViewModel>, IVentaViewModelMapper
    {
	 private IApplicationDbContext _dbContext;
        private readonly IClientDao _clienteDao;
        private readonly IProductDao _productDao;

        public VentaViewModelMapper(IApplicationDbContext dbContext, IClientDao clienteDao, IProductDao productDao)
        {
            _clienteDao = clienteDao;
            _productDao = productDao;
            Mapper.CreateMap<Venta, VentaViewModel>()
            .ForMember(model => model.Descripcion, opt => opt.Ignore())
            .ForMember(model => model.Descripcion, opt => opt.Ignore())
                //.ForMember(model => model.Categorias, opt => opt.Ignore())
			;
            Mapper.CreateMap<VentaViewModel, Venta>()
			.ForMember(model => model.Producto, opt => opt.Ignore())

			.ForMember(model => model.Cliente, opt => opt.Ignore())
			;
        }
		  public override VentaViewModel Map(Venta model)
        {
            var viewModel = base.Map(model);
            if (model.Cliente != null)
            {
                viewModel.Cliente = model.Cliente.Name;
                viewModel.ClienteId = model.Cliente.Id;
            }
            viewModel.Descripcion = TruncateLongString(model.Descripcion, 50);
            viewModel.ProductosId = model.Producto.Select(f => f.Id).ToArray();
            viewModel.ProductoDescripcion = model.Producto.Aggregate("", (current, item) => current + item.Description + " - ");
			
			return viewModel;
        }

        public override void Map(VentaViewModel viewModel, Venta model)
        {
            base.Map(viewModel, model);
            model.Descripcion = model.Descripcion + " -- Observacion fecha: " + DateTime.Now + " " + viewModel.Descripcion;

            this.Set(hospital => model.Cliente = hospital, viewModel.ClienteId, _clienteDao);
            if (model.Producto == null)
                model.Producto = new List<Product>();

            this.SetCollectionFromMultiSelect(viewModel.ProductosId, _productDao, model.Producto);
        }

        public override IEnumerable<VentaViewModel> Map(IEnumerable<Venta> models)
        {
            return models.Select(Map);
        }
    }
}