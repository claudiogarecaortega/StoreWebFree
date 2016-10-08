using Domain.Ventas;
using BassinoLibrary.ViewModels;

using BassinoBase.Models.ViewModelMapper.Interface;
namespace BassinoBase.Models.ViewModelMapper.Interfaces
{
    public interface IVentaViewModelMapper : IViewModelMapper<Venta, VentaViewModel, VentaViewModel>
    {
    }
}