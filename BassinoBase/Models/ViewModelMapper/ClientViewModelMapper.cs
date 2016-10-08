using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using Domain.Clients;
using BassinoLibrary.ViewModels;
using BassinoBase.Models.ViewModelMapper;
using BassinoBase.Models.ViewModelMapper.Interface;
using Domain.Misc;
using Persistence;
using Persistence.Dao.Interfaces;


namespace BassinoBase.Models.ViewModelMapper
{
    public class ClientViewModelMapper : ViewModelMapper<Client, ClientViewModel, ClientViewModel>, IClientViewModelMapper
    {
        private IApplicationDbContext _dbContext;
        private readonly IUserdDao _sellerDao;
        private readonly ITaxConditionDao _taxConditionDao;
        private readonly IUbicationDao _ubicationDao;
        private readonly IServicesDao _servicesDao;
        private readonly IContractDao _contratoDao;

        public ClientViewModelMapper(IApplicationDbContext dbContext, IUserdDao sellerDao, ITaxConditionDao taxConditionDao, IUbicationDao ubicationDao, IServicesDao servicesDao, IContractDao contratoDao)
        {
            _sellerDao = sellerDao;
            _taxConditionDao = taxConditionDao;
            _ubicationDao = ubicationDao;
            _servicesDao = servicesDao;
            _contratoDao = contratoDao;
            Mapper.CreateMap<Client, ClientViewModel>()
                  .ForMember(model => model.Secuencia, opt => opt.Ignore())
                  //.ForMember(model => model.Seller, opt => opt.Ignore())
                //.ForMember(model => model.Serviceses, opt => opt.Ignore())
                 .ForMember(model => model.Ubication, opt => opt.Ignore())
                 //.ForMember(model => model.TaxCondition, opt => opt.Ignore())
               ;
            Mapper.CreateMap<ClientViewModel, Client>()
                .ForMember(model => model.Secuencia, opt => opt.Ignore())
                //.ForMember(model => model.ServicePrice, opt => opt.Ignore())
                //.ForMember(model => model.Seller, opt => opt.Ignore())
                //.ForMember(model => model.Serviceses, opt => opt.Ignore())
                .ForMember(model => model.Contract, opt => opt.Ignore())
                 .ForMember(model => model.Ubication, opt => opt.Ignore())
                // .ForMember(model => model.TaxCondition, opt => opt.Ignore())
                 ;
        }
        public override ClientViewModel Map(Client model)
        {
            var viewModel = base.Map(model);
            viewModel.Secuencia = model.Secuencia;
            viewModel.NombreCompleto = model.GetCompleteName();
            if (model.Ubication != null)
            {
                viewModel.Ubication = model.Ubication.DescripcionCompleta();
                viewModel.UbicationId = model.Ubication.Id;
            }

            //if (model.TaxCondition != null)
            //{
            //    viewModel.TaxCondition = model.TaxCondition.Description;
            //    viewModel.TaxConditionId = model.TaxCondition.Id;
            //}
            //if (model.Seller != null)
            //{
            //    viewModel.Seller = model.Seller.FullName;
            //    viewModel.SellerId = model.Seller.Id;
            //}
            if (model.Contract != null)
            {
                viewModel.Contrato = model.Contract.Description;
                viewModel.ContratoDoc = model.Contract.Contrato;
                viewModel.ContratoId = model.Contract.Id;
            }
            //if (model.Serviceses.Any())
            //{
            //    var detalle = model.Serviceses.Select(d => d.Description).ToArray();
            //    var items = detalle.Aggregate("", (current, item) => current + item);
            //    viewModel.ServicesesDescription = items;
            //    viewModel.Serviceses = model.Serviceses.Select(d => d.Id).ToArray();
            //}
            return viewModel;
        }

        public override void Map(ClientViewModel viewModel, Client model)
        {
            base.Map(viewModel, model);
            model.Secuencia = model.Secuencia;
           // model.ServicePrice = CustomParseDouble(viewModel.ServicePrice);
            //this.Set(seller => model.Seller = seller, viewModel.SellerId, _sellerDao);
            //this.Set(taxCondition => model.TaxCondition = taxCondition, viewModel.TaxConditionId, _taxConditionDao);
            this.Set(ubication => model.Ubication = ubication, viewModel.UbicationId, _ubicationDao);
            this.Set(contrato => model.Contract = contrato, viewModel.ContratoId, _contratoDao);
            //if (model.Serviceses == null)
            //    model.Serviceses = new List<Services>();

            //foreach (var item in viewModel.UbicationRoute )
            //{
            //    model.UbicationRoute.Add(_ubicationDao.Get(item));
            //}
           // this.SetCollectionFromMultiSelect(viewModel.Serviceses, _servicesDao, model.Serviceses);
        }

        public override IEnumerable<ClientViewModel> Map(IEnumerable<Client> models)
        {
            return models.Select(Map);
        }
    }
}