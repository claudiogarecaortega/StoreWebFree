using System;
using AutoMapper;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Domain.Products;
using BassinoLibrary.ViewModels;
using BassinoBase.Models.ViewModelMapper;
using BassinoBase.Models.ViewModelMapper.Interface;
using Domain.Commodity;
using Domain.Misc;
using Persistence;
using Persistence.Dao.Interfaces;


namespace BassinoBase.Models.ViewModelMapper
{
    public class ShipmentViewModelMapper : ViewModelMapper<Shipment, ShipmentViewModel, ShipmentViewModel>, IShipmentViewModelMapper
    {
	 private IApplicationDbContext _dbContext;
        private readonly IUbicationDao _ubicationDao;
        private readonly IInboundDao _inboundDao;
        private readonly IShipmentTrackDao _shipmentTrackDao;
        private readonly IShipmentTrackViewModelMapper _shiptrackViewModelMapper;
        private readonly ICarDao _carDao;

        public ShipmentViewModelMapper(IApplicationDbContext dbContext, IUbicationDao ubicationDao, IInboundDao inboundDao, IShipmentTrackDao shipmentTrackDao, IShipmentTrackViewModelMapper shiptrackViewModelMapper, ICarDao carDao)
        {
            _ubicationDao = ubicationDao;
            _inboundDao = inboundDao;
            _shipmentTrackDao = shipmentTrackDao;
            _shiptrackViewModelMapper = shiptrackViewModelMapper;
            _carDao = carDao;
            Mapper.CreateMap<Shipment, ShipmentViewModel>()
                .ForMember(model => model.Statuts, opt => opt.Ignore())
                .ForMember(model => model.TotalKilos, opt => opt.Ignore())
                .ForMember(model => model.Car, opt => opt.Ignore())
                .ForMember(model => model.CarId, opt => opt.Ignore())
                .ForMember(model => model.TotalPakages, opt => opt.Ignore())
                .ForMember(model => model.Cargars, opt => opt.Ignore())
                .ForMember(model => model.UbicationRoute, opt => opt.Ignore())
                .ForMember(model => model.Tracks, opt => opt.Ignore())
                .ForMember(model => model.IsFinishig, opt => opt.Ignore())
                .ForMember(model => model.IsSent, opt => opt.Ignore())
                .ForMember(model => model.IsTraveling, opt => opt.Ignore())
                ;
            Mapper.CreateMap<ShipmentViewModel, Shipment>()
                .ForMember(model => model.UbicationFrom, opt => opt.Ignore())
                .ForMember(model => model.Truck, opt => opt.Ignore())
                .ForMember(model => model.TotalKilos, opt => opt.Ignore())
                .ForMember(model => model.UbicationTo, opt => opt.Ignore())
                .ForMember(model => model.Tracks, opt => opt.Ignore())
                .ForMember(model => model.Cargars, opt => opt.Ignore())
                .ForMember(model => model.UbicationRoute, opt => opt.Ignore())
                .ForMember(model => model.IsFinishig, opt => opt.Ignore())
                .ForMember(model => model.IsSent, opt => opt.Ignore())
                .ForMember(model => model.IsTraveling, opt => opt.Ignore())
                ;
        }
		  public override ShipmentViewModel Map(Shipment model)
        {
           
            var viewModel = base.Map(model);

		      viewModel.Statuts = model.Status();
              viewModel.IsTraveling = model.IsTraveling;
              viewModel.IsSent = model.IsSent;

              viewModel.IsFinishig = model.IsFinishig;
            if (model.UbicationTo != null)
            {
                viewModel.UbicationToId = model.UbicationTo.Id;
                viewModel.UbicationTo = model.UbicationTo.DescripcionCompleta();
            }
            if (model.Truck != null)
            {
                viewModel.CarId = model.Truck.Id;
                viewModel.Car = model.Truck.Nombre;
            }

            if (model.UbicationFrom != null)
            {
                viewModel.UbicationFrom = model.UbicationFrom.DescripcionCompleta();
                viewModel.UbicationFromId = model.UbicationFrom.Id;
            }
            if (model.Cargars != null)
            {
                viewModel.Cargars = model.Cargars.Select(x=>x.Id).ToArray();
                
            }
            if (model.Tracks != null)
            {
                viewModel.Tracks = _shiptrackViewModelMapper.Map(model.Tracks);

            }
            if (model.UbicationRoute != null)
            {
                viewModel.UbicationRoute = model.UbicationRoute.Select(x => x.Id).ToArray();

            }
		      viewModel.TotalKilos = model.TotalKilos.ToString();
		      viewModel.TotalPakages = model.TotalPakages.ToString();
			return viewModel;
        }

        public override void Map(ShipmentViewModel viewModel, Shipment model)
        {
            base.Map(viewModel, model);
            model.IsTraveling = model.IsTraveling;
            model.IsSent = model.IsSent;

            model.TotalKilos = (decimal) CustomParse(viewModel.TotalKilos);
            //model.TotalKilos =Convert.ToDecimal( viewModel.TotalKilos.Replace('.',','));
            model.Secuencia = model.Secuencia;
            model.IsFinishig = model.IsFinishig;
            this.Set(hospital => model.UbicationTo = hospital, viewModel.UbicationToId, _ubicationDao);
            this.Set(hospital => model.Truck = hospital, viewModel.CarId, _carDao);
            this.Set(hospital => model.UbicationFrom = hospital, viewModel.UbicationFromId, _ubicationDao);
            if(model.Cargars==null)
                model.Cargars=new List<Inbound>();

            this.SetCollectionFromMultiSelect(viewModel.Cargars,_inboundDao,model.Cargars);
            if (model.Tracks == null)
                model.Tracks = new List<ShipmentTrack>();

            //this.SetCollectionFromMultiSelect(viewModel.Tracks,_shipmentTrackDao,model.Tracks);

            if (model.UbicationRoute == null)
                model.UbicationRoute = new List<Ubication>();
            
            //foreach (var item in viewModel.UbicationRoute )
            //{
            //    model.UbicationRoute.Add(_ubicationDao.Get(item));
            //}
            this.SetCollectionFromMultiSelect(viewModel.UbicationRoute, _ubicationDao, model.UbicationRoute);
        }
        
        public override IEnumerable<ShipmentViewModel> Map(IEnumerable<Shipment> models)
        {
            return models.Select(this.Map);
        }
    }
}