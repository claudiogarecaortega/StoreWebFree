using BassinoLibrary.ViewModels;
using Domain.Products;

namespace BassinoBase.Models.ViewModelMapper.Interface
{
    public interface IProductViewModelMapper : IViewModelMapper<Product, ProductViewModel, ProductViewModel>
    {
    }
}