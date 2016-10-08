using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using Domain.Security;
using BassinoLibrary.ViewModels;
using BassinoBase.Models.ViewModelMapper;
using BassinoBase.Models.ViewModelMapper.Interface;
using Persistence;
using Persistence.Dao.Interfaces;


namespace BassinoBase.Models.ViewModelMapper
{
    public class ModuleViewModelMapper : ViewModelMapper<Module, ModuleViewModel, ModuleViewModel>, IModuleViewModelMapper
    {
	 private IApplicationDbContext _dbContext;
        private readonly IModuleDao _moduleDao;

        public ModuleViewModelMapper(IApplicationDbContext dbContext, IModuleDao moduleDao)
        {
            _moduleDao = moduleDao;
            Mapper.CreateMap<Module, ModuleViewModel>();
            Mapper.CreateMap<ModuleViewModel, Module>()
                .ForMember(model => model.ModuleParent, opt => opt.Ignore());
        }
		  public override ModuleViewModel Map(Module model)
        {
            var viewModel = base.Map(model);
		      viewModel.ModuleName = model.ModuleName;
            
            if (model.ModuleParent != null)
            {
                viewModel.ModuleParent = model.ModuleParent.ModuleName;
                viewModel.ModuleParentId = model.ModuleParent.Id;
            }
			return viewModel;
        }

        public override void Map(ModuleViewModel viewModel, Module model)
        {
            base.Map(viewModel, model);
            this.Set(ubication => model.ModuleParent = ubication, viewModel.ModuleParentId, _moduleDao);
            //this.Set(hospital => model.UserContact = hospital, viewModel.ContactId, UserExtendedDao);
        }

        public override IEnumerable<ModuleViewModel> Map(IEnumerable<Module> models)
        {
            return models.Select(Map);
        }
    }
}