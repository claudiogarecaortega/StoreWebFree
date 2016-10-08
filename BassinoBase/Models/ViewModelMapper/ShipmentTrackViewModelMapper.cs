using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using Domain.Products;
using BassinoLibrary.ViewModels;
using BassinoBase.Models.ViewModelMapper;
using BassinoBase.Models.ViewModelMapper.Interface;
using Persistence;
using Persistence.Dao.Interfaces;


namespace BassinoBase.Models.ViewModelMapper
{
    public class ShipmentTrackViewModelMapper : ViewModelMapper<ShipmentTrack, ShipmentTrackViewModel, ShipmentTrackViewModel>, IShipmentTrackViewModelMapper
    {
	 private IApplicationDbContext _dbContext;
        private readonly IUbicationDao _ubicationDao;
        private readonly IShipmentDao _shipmentDao;

        public ShipmentTrackViewModelMapper(IApplicationDbContext dbContext, IUbicationDao ubicationDao, IShipmentDao shipmentDao)
        {
            _ubicationDao = ubicationDao;
            _shipmentDao = shipmentDao;
            Mapper.CreateMap<ShipmentTrack, ShipmentTrackViewModel>();
            Mapper.CreateMap<ShipmentTrackViewModel, ShipmentTrack>()
                .ForMember(model => model.Ubication, opt => opt.Ignore())
                .ForMember(model => model.DateTrack, opt => opt.Ignore())
                ;
        }
		  public override ShipmentTrackViewModel Map(ShipmentTrack model)
        {
            var viewModel = base.Map(model);
		      if (model.Ubication!=null)
		      {
		          viewModel.Ubication = model.Ubication.DescripcionCompleta();
		          viewModel.UbicationId = model.Ubication.Id;
		      }
			return viewModel;
        }

        public override void Map(ShipmentTrackViewModel viewModel, ShipmentTrack model)
        {
            base.Map(viewModel, model);
            var da = viewModel.DateTrack.Split('/');
            var d = da[0] + "/" + da[1] + "/" + da[2];
            model.DateTrack = Convert.ToDateTime(d+" "+DateTime.Now.ToLongTimeString());
            this.Set(hospital => model.Ubication = hospital, viewModel.UbicationId, _ubicationDao);
            this.Set(hospital => model.Shipment = hospital, viewModel.ShipmentId, _shipmentDao);
        }

        public override IEnumerable<ShipmentTrackViewModel> Map(IEnumerable<ShipmentTrack> models)
        {
            return models.Select(Map);
        }
    }
}