using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using Domain.Commodity;
using BassinoLibrary.ViewModels;
using BassinoBase.Models.ViewModelMapper;
using BassinoBase.Models.ViewModelMapper.Interface;
using Microsoft.Ajax.Utilities;
using Persistence;
using Persistence.Dao.Interfaces;


namespace BassinoBase.Models.ViewModelMapper
{
    public class InboundViewModelMapper : ViewModelMapper<Inbound, InboundViewModel, InboundViewModel>, IInboundViewModelMapper
    {
	 private IApplicationDbContext _dbContext;
        private readonly IClientDao _clientDao;
        private readonly IBillDao _billDao;
        private readonly IBillTypeDao _billtypeDao;
        private readonly IUserdDao _userExtendDao;
        private readonly IPackageTypeDao _packageTypeDao;
        private readonly IProductDao _productDao;
        private readonly IMeasureUnitDao _measureDao;

        public InboundViewModelMapper(IApplicationDbContext dbContext, IClientDao clientDao, IBillDao billDao, IBillTypeDao billtypeDao, IUserdDao userExtendDao, IPackageTypeDao packageTypeDao, IProductDao productDao, IMeasureUnitDao measureDao)
        {
            _clientDao = clientDao;
            _billDao = billDao;
            _billtypeDao = billtypeDao;
            _userExtendDao = userExtendDao;
            _packageTypeDao = packageTypeDao;
            _productDao = productDao;
            _measureDao = measureDao;
            Mapper.CreateMap<Inbound, InboundViewModel>()
                .ForMember(model => model.IsUsed, opt => opt.Ignore())
                .ForMember(model => model.Status, opt => opt.Ignore())
                .ForMember(model => model.PriceDecimal, opt => opt.Ignore())
                .ForMember(model => model.Quantity, opt => opt.Ignore())
                .ForMember(model => model.Kilos, opt => opt.Ignore())
                ;
            Mapper.CreateMap<InboundViewModel, Inbound>()
                .ForMember(model => model.DateTicket, opt => opt.Ignore())
                .ForMember(model => model.PriceDecimal, opt => opt.Ignore())
                .ForMember(model => model.Quantity, opt => opt.Ignore())
                .ForMember(model => model.Kilos, opt => opt.Ignore())
                .ForMember(model => model.UsoFisico, opt => opt.Ignore())
                .ForMember(model => model.IsUsed, opt => opt.Ignore())
                .ForMember(model => model.DateIn, opt => opt.Ignore())
                .ForMember(model => model.ClientFrom, opt => opt.Ignore())
                .ForMember(model => model.ClientOrigen, opt => opt.Ignore())
                .ForMember(model => model.ClientTo, opt => opt.Ignore())
                .ForMember(model => model.Bill, opt => opt.Ignore())
                .ForMember(model => model.BillType, opt => opt.Ignore())
                .ForMember(model => model.UserControl, opt => opt.Ignore())
                .ForMember(model => model.PackageType, opt => opt.Ignore())
                .ForMember(model => model.Product, opt => opt.Ignore())
                .ForMember(model => model.MeasureUnit, opt => opt.Ignore())
                ;
        }
		  public override InboundViewModel Map(Inbound model)
        {
            var viewModel = base.Map(model);
		      viewModel.IsUsed = model.IsUsed;
		      viewModel.Secuencia = model.Secuencia;
		      viewModel.Kilos = model.Kilos;
		      viewModel.PriceDecimal = model.PriceDecimal.ToString();
		      viewModel.Quantity = model.Quantity.ToString();
		      viewModel.Status = model.Status();
		      viewModel.Description = model.Description ?? "S/C";

            if (model.MeasureUnit != null)
            {
                viewModel.MeasureUnitId = model.MeasureUnit.Id;
                viewModel.MeasureUnit = model.MeasureUnit.Description;
            }

            if (model.ClientFrom != null)
            {
                viewModel.ClientFrom = model.ClientFrom.Alias;
                viewModel.ClientFromId = model.ClientFrom.Id;
            }
            if (model.ClientTo != null)
            {
                viewModel.ClientTo = model.ClientTo.Alias;
                viewModel.ClientToId = model.ClientTo.Id;
            }
            if (model.ClientOrigen != null)
            {
                viewModel.ClientOrigen = model.ClientOrigen.Alias;
                viewModel.ClientOrigenId = model.ClientOrigen.Id;
            }
            if (model.Bill != null)
            {
                viewModel.Bill = model.Bill.Description;
                viewModel.BillId = model.Bill.Id;
            }
            if (model.BillType != null)
            {
                viewModel.BillType = model.BillType.Description;
                viewModel.BillTypeId = model.BillType.Id;
            }
            if (model.UserControl != null)
            {
                viewModel.UserControl = model.UserControl.FullName;
                viewModel.UserControlId = model.UserControl.Id;
            }
            if (model.PackageType != null)
            {
                viewModel.PackageType = model.PackageType.Description;
                viewModel.PackageTypeId = model.PackageType.Id;
            }
            if (model.Product != null)
            {
                viewModel.Product = model.Product.Description;
                viewModel.ProductId = model.Product.Id;
            }
		      viewModel.UbicationDescription = model.UbicationDescription();
			return viewModel;
        }

        public override void Map(InboundViewModel viewModel, Inbound model)
        {
            if (viewModel.Id == 0)
                viewModel.DateIn = DateTime.Now.ToString();
            else
                viewModel.DateIn = model.DateIn.ToString();
            model.Secuencia = model.Secuencia;
            model.IsUsed = model.IsUsed;
            base.Map(viewModel, model);
            if (model.Description.IsNullOrWhiteSpace())
                model.Description = "S/C";
            model.Kilos = CustomParse(viewModel.Kilos).ToString();
            model.PriceDecimal = CustomParse(viewModel.PriceDecimal);
            model.Quantity = CustomParse(viewModel.Quantity);
            var usoFisico = string.IsNullOrEmpty(viewModel.UsoFisico) ? "0" : viewModel.UsoFisico;
            model.UsoFisico = CustomParse(usoFisico);

            model.DateIn = Convert.ToDateTime(viewModel.DateIn);
            model.DateTicket = Convert.ToDateTime(viewModel.DateTicket);

            this.Set(hospital => model.ClientFrom  = hospital, viewModel.ClientFromId , _clientDao);
            this.Set(hospital => model.ClientOrigen  = hospital, viewModel.ClientOrigenId , _clientDao);
            this.Set(hospital => model.ClientTo = hospital, viewModel.ClientToId, _clientDao);
            this.Set(hospital => model.Bill = hospital, viewModel.BillId, _billDao);
            this.Set(hospital => model.BillType = hospital, viewModel.BillTypeId,_billtypeDao);
            this.Set(hospital => model.UserControl = hospital, viewModel.UserControlId, _userExtendDao);
            this.Set(hospital => model.PackageType = hospital, viewModel.PackageTypeId, _packageTypeDao);
            this.Set(hospital => model.Product = hospital, viewModel.ProductId, _productDao);
            this.Set(hospital => model.MeasureUnit = hospital, viewModel.MeasureUnitId,_measureDao);
        }

        public override IEnumerable<InboundViewModel> Map(IEnumerable<Inbound> models)
        {
            return models.Select(Map);
        }
    }
}