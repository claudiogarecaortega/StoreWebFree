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
    public class CuotasViewModelMapper : ViewModelMapper<Cuotas, CuotasViewModel, CuotasViewModel>, ICuotasViewModelMapper
    {
	 private IApplicationDbContext _dbContext;

        public CuotasViewModelMapper(IApplicationDbContext dbContext)
        {
            Mapper.CreateMap<Cuotas, CuotasViewModel>()
			//.ForMember(model => model.Categorias, opt => opt.Ignore())
			;
            Mapper.CreateMap<CuotasViewModel, Cuotas>()
			.ForMember(model => model.Credito, opt => opt.Ignore())
			;
        }
		  public override CuotasViewModel Map(Cuotas model)
        {
            var viewModel = base.Map(model);
            viewModel.Status = model.Status();
		      if (model.Credito != null)
		      {
		          viewModel.Credito ="Numero de Credito:  "+ model.Credito.Id.ToString();
		          viewModel.CreditoId = model.Credito.Id;
		          viewModel.CreditoCliente = model.Credito.Cliente.Name;
		          viewModel.MontoTotal = model.Credito.MontoTotal.ToString();
		          viewModel.MontoPendiente = model.Credito.Monto.ToString();
		      }
              
			return viewModel;
        }

        public override void Map(CuotasViewModel viewModel, Cuotas model)
        {
            base.Map(viewModel, model);

            //this.Set(hospital => model.UserContact = hospital, viewModel.ContactId, UserExtendedDao);
        }

        public override IEnumerable<CuotasViewModel> Map(IEnumerable<Cuotas> models)
        {
            return models.Select(Map);
        }
    }
}