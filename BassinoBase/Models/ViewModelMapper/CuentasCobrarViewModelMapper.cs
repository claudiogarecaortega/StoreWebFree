using Domain.Contable;
using BassinoLibrary.ViewModels;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using Domain.Products;
using BassinoLibrary.ViewModels;
using BassinoBase.Models.ViewModelMapper;
using BassinoBase.Models.ViewModelMapper.Interface;
using Domain.Misc;
using Persistence;
using Persistence.Dao.Interfaces;
using BassinoBase.Models.ViewModelMapper.Interfaces;


namespace BassinoBase.Models.ViewModelMapper
{
    public class CuentasCobrarViewModelMapper : ViewModelMapper<CuentasCobrar, CuentasCobrarViewModel, CuentasCobrarViewModel>, ICuentasCobrarViewModelMapper
    {
	 private IApplicationDbContext _dbContext;
        private readonly IBancariaDao _bancariaDao;
        private readonly ICreditoDao _creditoDao;

        public CuentasCobrarViewModelMapper(IApplicationDbContext dbContext, IBancariaDao bancariaDao, ICreditoDao creditoDao)
        {
            _bancariaDao = bancariaDao;
            _creditoDao = creditoDao;
            Mapper.CreateMap<CuentasCobrar, CuentasCobrarViewModel>()
			//.ForMember(model => model, opt => opt.Ignore())
			;
            Mapper.CreateMap<CuentasCobrarViewModel, CuentasCobrar>()
			.ForMember(model => model.Credito, opt => opt.Ignore())
			.ForMember(model => model.CuentaDeposito, opt => opt.Ignore())
			;
        }
		  public override CuentasCobrarViewModel Map(CuentasCobrar model)
        {
            var viewModel = base.Map(model);
		      if (model.Credito != null)
		      {
		          viewModel.Credito = model.Credito.Descripcion;
		          viewModel.CreditoId = model.Credito.Id;
		      }
		      if (model.CuentaDeposito != null)
		      {
		          viewModel.CuentaDeposito = model.CuentaDeposito.Alias;
		          viewModel.CuentaDepositoId = model.CuentaDeposito.Id;
		      }
		      return viewModel;
        }

        public override void Map(CuentasCobrarViewModel viewModel, CuentasCobrar model)
        {
            base.Map(viewModel, model);

            this.Set(hospital => model.CuentaDeposito = hospital, viewModel.CuentaDepositoId, _bancariaDao);
            this.Set(hospital => model.Credito = hospital, viewModel.CreditoId, _creditoDao);
        }

        public override IEnumerable<CuentasCobrarViewModel> Map(IEnumerable<CuentasCobrar> models)
        {
            return models.Select(Map);
        }
    }
}