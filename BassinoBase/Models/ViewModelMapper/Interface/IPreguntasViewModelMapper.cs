using BassinoBase.Models.ViewModelMapper.Interface;
using Domain.Products;
using BassinoLibrary.ViewModels;
using BassinoBase.Models.ViewModelMapper.Interfaces;

namespace BassinoBase.Models.ViewModelMapper.Interfaces
{
    public interface IPreguntasViewModelMapper : IViewModelMapper<Preguntas, PreguntasViewModel, PreguntasViewModel>
    {
    }
}