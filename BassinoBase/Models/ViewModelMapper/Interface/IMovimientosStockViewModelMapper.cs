using BassinoBase.Models.ViewModelMapper.Interface;
using Domain.Almacen;
using BassinoLibrary.ViewModels;
using BassinoBase.Models.ViewModelMapper.Interfaces;

namespace BassinoBase.Models.ViewModelMapper.Interfaces
{
    public interface IMovimientosStockViewModelMapper : IViewModelMapper<MovimientosStock, MovimientosStockViewModel, MovimientosStockViewModel>
    {
    }
}