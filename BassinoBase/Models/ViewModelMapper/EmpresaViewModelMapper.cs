using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using Domain.Misc;
using BassinoLibrary.ViewModels;
using BassinoBase.Models.ViewModelMapper.Interfaces;
using BassinoBase.Models.ViewModelMapper;
using Persistence;


namespace BassinoBase.Models.ViewModelMapper
{
    public class EmpresaViewModelMapper : ViewModelMapper<Empresa, EmpresaViewModel, EmpresaViewModel>, IEmpresaViewModelMapper
    {
	 private IApplicationDbContext _dbContext;

        public EmpresaViewModelMapper(IApplicationDbContext dbContext)
        {
            Mapper.CreateMap<Empresa, EmpresaViewModel>();
            Mapper.CreateMap<EmpresaViewModel, Empresa>();
        }
		  public override EmpresaViewModel Map(Empresa model)
        {
            var viewModel = base.Map(model);
			return viewModel;
        }

        public override void Map(EmpresaViewModel viewModel, Empresa model)
        {
            base.Map(viewModel, model);

            //this.Set(hospital => model.UserContact = hospital, viewModel.ContactId, UserExtendedDao);
        }

        public override IEnumerable<EmpresaViewModel> Map(IEnumerable<Empresa> models)
        {
            return models.Select(Map);
        }
    }
}