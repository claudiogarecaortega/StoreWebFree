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
    public class StockViewModelMapper : ViewModelMapper<Stock, StockViewModel, StockViewModel>, IStockViewModelMapper
    {
	 private IApplicationDbContext _dbContext;
        private readonly IAlmacenDao _almacenDao;
        private readonly IProductDao _productoDao;
        private readonly IMovimientosStockViewModelMapper _stockMovimientosMapper;

        public StockViewModelMapper(IApplicationDbContext dbContext, IAlmacenDao almacenDao, IProductDao productoDao, IMovimientosStockViewModelMapper stockMovimientosMapper)
        {
            _almacenDao = almacenDao;
            _productoDao = productoDao;
            _stockMovimientosMapper = stockMovimientosMapper;
            Mapper.CreateMap<Stock, StockViewModel>();
            Mapper.CreateMap<StockViewModel, Stock>()
                 .ForMember(model => model.Almacen, opt => opt.Ignore())
                 .ForMember(model => model.Producto, opt => opt.Ignore());
        }
		  public override StockViewModel Map(Stock model)
        {
            var viewModel = base.Map(model);
		      if (model.Almacen!=null)
		      {
		          viewModel.AlmacenId = model.Almacen.Id;
		          viewModel.Almacen = model.Almacen.Descripcion;
		      }
              if (model.Producto != null)
              {
                  viewModel.ProductoId = model.Producto.Id;
                  viewModel.Producto = model.Producto.Nombre;
              }
              if (model.Movimientos != null)
              {
                  viewModel.Tracks =_stockMovimientosMapper.Map(model.Movimientos);

              }
		      return viewModel;
        }

        public override void Map(StockViewModel viewModel, Stock model)
        {
            base.Map(viewModel, model);

            this.Set(hospital => model.Almacen = hospital, viewModel.AlmacenId, _almacenDao);
            this.Set(hospital => model.Producto = hospital, viewModel.ProductoId, _productoDao);
        }

        public override IEnumerable<StockViewModel> Map(IEnumerable<Stock> models)
        {
            return models.Select(Map);
        }
    }
}