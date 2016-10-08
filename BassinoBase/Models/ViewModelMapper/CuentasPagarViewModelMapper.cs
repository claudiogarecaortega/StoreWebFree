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
    public class CuentasPagarViewModelMapper : ViewModelMapper<CuentasPagar, CuentasPagarViewModel, CuentasPagarViewModel>, ICuentasPagarViewModelMapper
    {
        private IApplicationDbContext _dbContext;
        private readonly IBancariaDao _bancariaDao;
        private readonly IPedidosProductoDao _pedidoproductoDao;

        public CuentasPagarViewModelMapper(IApplicationDbContext dbContext, IBancariaDao bancariaDao, IPedidosProductoDao pedidoproductoDao)
        {
            _bancariaDao = bancariaDao;
            _pedidoproductoDao = pedidoproductoDao;
            Mapper.CreateMap<CuentasPagar, CuentasPagarViewModel>()
                //.ForMember(model => model.Categorias, opt => opt.Ignore())
            ;
            Mapper.CreateMap<CuentasPagarViewModel, CuentasPagar>()
            .ForMember(model => model.Credito, opt => opt.Ignore())
            .ForMember(model => model.CuentaExtracto, opt => opt.Ignore())
            ;
        }
        public override CuentasPagarViewModel Map(CuentasPagar model)
        {
            var viewModel = base.Map(model);
            if (model.Credito != null)
            {
                viewModel.Credito = model.Credito.Descripcion;
                viewModel.CreditoId = model.Credito.Id;
            }
            if (model.CuentaExtracto != null)
            {
                viewModel.CuentaExtracto = model.CuentaExtracto.Alias;
                viewModel.CuentaExtractoId = model.CuentaExtracto.Id;
            }

            return viewModel;
        }

        public override void Map(CuentasPagarViewModel viewModel, CuentasPagar model)
        {
            base.Map(viewModel, model);

            this.Set(hospital => model.CuentaExtracto = hospital, viewModel.CuentaExtractoId, _bancariaDao);
            this.Set(hospital => model.Credito = hospital, viewModel.CreditoId, _pedidoproductoDao);
        }

        public override IEnumerable<CuentasPagarViewModel> Map(IEnumerable<CuentasPagar> models)
        {
            return models.Select(Map);
        }
    }
}