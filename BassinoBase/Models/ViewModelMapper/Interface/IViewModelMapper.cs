using System.Collections.Generic;

namespace BassinoBase.Models.ViewModelMapper.Interface
{
    public interface IViewModelMapper<TModel, TViewModel, TCommonModel>
    {
        void Map(TViewModel viewModel, TModel model);
        TViewModel Map(TModel model);
        IEnumerable<TViewModel> Map(IEnumerable<TModel> models);
        void MapCommon(TCommonModel viewModel, TModel model);
        TCommonModel MapCommon(TModel model);
        IEnumerable<TCommonModel> MapCommon(IEnumerable<TModel> models);
    }
}