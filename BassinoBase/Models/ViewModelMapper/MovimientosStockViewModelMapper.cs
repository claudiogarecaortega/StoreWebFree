using System;
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
    public class MovimientosStockViewModelMapper : ViewModelMapper<MovimientosStock, MovimientosStockViewModel, MovimientosStockViewModel>, IMovimientosStockViewModelMapper
    {
	 private IApplicationDbContext _dbContext;
        private readonly IStockDao _iStockDao;
        private readonly IProviderDao _provedorDao;
        private readonly IUserdDao _userDao;

        public MovimientosStockViewModelMapper(IApplicationDbContext dbContext, IStockDao iStockDao, IProviderDao provedorDao, IUserdDao userDao)
        {
            _iStockDao = iStockDao;
            _provedorDao = provedorDao;
            _userDao = userDao;
            Mapper.CreateMap<MovimientosStock, MovimientosStockViewModel>();
            Mapper.CreateMap<MovimientosStockViewModel, MovimientosStock>()
                .ForMember(k=>k.Proveedor,opt=>opt.Ignore())
                .ForMember(k=>k.Stock,opt=>opt.Ignore());
        }
		  public override MovimientosStockViewModel Map(MovimientosStock model)
        {
            var viewModel = base.Map(model);
		      if (model.Stock != null)
		      {
		          viewModel.Stock = model.Stock.Descripcion;
		          viewModel.StockProducto = model.Stock.Producto.Nombre;
		          viewModel.StockId = model.Stock.Id;
              } 
              if (model.Proveedor != null)
              {
                  viewModel.Proveedor = model.Proveedor.Name;
                  viewModel.ProveedorId = model.Proveedor.Id;
                  
              }
		      viewModel.DateCreate = model.DateCreate.ToString();
		      viewModel.UserCreate = _userDao.Get(Convert.ToInt32(model.CreateUser)).FullName;
			return viewModel;
        }

        public override void Map(MovimientosStockViewModel viewModel, MovimientosStock model)
        {
            base.Map(viewModel, model);

            this.Set(hospital => model.Stock = hospital, viewModel.StockId, _iStockDao);
            this.Set(hospital => model.Proveedor = hospital, viewModel.ProveedorId, _provedorDao);
        }

        public override IEnumerable<MovimientosStockViewModel> Map(IEnumerable<MovimientosStock> models)
        {
            return models.Select(Map);
        }
    }
}