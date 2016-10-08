using System;
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
    public class CreditoViewModelMapper : ViewModelMapper<Credito, CreditoViewModel, CreditoViewModel>, ICreditoViewModelMapper
    {
	 private IApplicationDbContext _dbContext;
        private readonly IClientDao _clientDao;
        private IProductDao _productDao;
        private readonly ICreditoDao _creditoDao;

        public CreditoViewModelMapper(IApplicationDbContext dbContext, IClientDao clientDao, IProductDao productDao, ICreditoDao creditoDao)
        {
            _clientDao = clientDao;
            _productDao = productDao;
            _creditoDao = creditoDao;
            Mapper.CreateMap<Credito, CreditoViewModel>()
			.ForMember(model => model.Descripcion, opt => opt.Ignore())
			;
            Mapper.CreateMap<CreditoViewModel, Credito>()
    .ForMember(model => model.Descripcion, opt => opt.Ignore())
                .ForMember(model => model.Cuotas, opt => opt.Ignore())
			.ForMember(model => model.Producto, opt => opt.Ignore())
			.ForMember(model => model.Cliente, opt => opt.Ignore())
			;
        }
		  public override CreditoViewModel Map(Credito model)
        {
            var viewModel = base.Map(model);
		      if (model.Cliente != null)
		      {
		          viewModel.Cliente = model.Cliente.Name;
		          viewModel.ClienteId = model.Cliente.Id;
		      }
		      viewModel.Descripcion = TruncateLongString(model.Descripcion,50);
		      viewModel.ProductosId = model.Producto.Select(f => f.Id).ToArray();
              viewModel.ProductoDescripcion = model.Producto.Aggregate("", (current, item) => current + item.Description + " - ");
			return viewModel;
        }

        public override void Map(CreditoViewModel viewModel, Credito model)
        {
            base.Map(viewModel, model);
            if(model.Cuotas==null)
                model.Cuotas=new List<Cuotas>();
            model.Descripcion = model.Descripcion+" -- Observacion fecha: " + DateTime.Now + " " + viewModel.Descripcion;

            if (viewModel.Id == 0)
            {
                model.Cuotas = GenerateCoutas(viewModel.NumeroPagos, 0, viewModel.Monto,viewModel.UserId.ToString());
            }
            else
            {
                var pagas = model.Cuotas.Count(t => t.Pagada);
                var pendientes = model.Cuotas.Count(t => !t.Pagada);
                model.Cuotas = ReGenerateCoutas(viewModel.NumeroPagos, 0, viewModel.Monto, pagas, pendientes, pagas + pendientes, model.Cuotas, viewModel.Id, viewModel.UserId.ToString());
            }
            this.Set(hospital => model.Cliente = hospital, viewModel.ClienteId, _clientDao);
            if(model.Producto==null)
                model.Producto=new List<Product>();

            this.SetCollectionFromMultiSelect(viewModel.ProductosId, _productDao,model.Producto);
        }
        private IList<Cuotas> GenerateCoutas(int cantidad, decimal interes, decimal monto,string userId)
        {
            var cuotas = new List<Cuotas>();
            // interes = interes/100;
            var fecha = DateTime.Now.Date;
            double Monto = Convert.ToDouble(monto);
            int Plazos = cantidad;
            double taza = Convert.ToDouble(interes);


            //A = 1-(1+taza)^-plazos
            int p = Plazos * -1;
            double b = (1 + taza);
            double A = (1 - Math.Pow(b, p)) / taza;

            //Cuota Fija = Monto / A;

            double Cuota_F = Monto / cantidad;
            for (int i = 0; i < cantidad; i++)
            {
                fecha = fecha.AddMonths(1);
                var cuota = new Cuotas();
                cuota.Valor = Convert.ToDecimal(Cuota_F);
                cuota.Vencimiento = fecha;
                cuota.CreateUser = userId;
                cuota.DateCreate = DateTime.Now;
                cuotas.Add(cuota);
            }
            return cuotas;

        }
        private IList<Cuotas> ReGenerateCoutas(int cantidad, decimal interes, decimal monto,int pagas, int restantes,int cantidadanterior,IList<Cuotas> cuotasActuales,int creditoId ,string userId)
        {
            foreach (var cuota in cuotasActuales)
            {
                if (!cuota.Pagada)
                {
                    cuota.IsDelete = true;
                }
            }
            var cuotas = cuotasActuales;
            // interes = interes/100;
            var fecha = DateTime.Now.Date;
            double Monto = Convert.ToDouble(monto);
            int Plazos = cantidad;
            double taza = Convert.ToDouble(interes);


            //A = 1-(1+taza)^-plazos
            int p = Plazos * -1;
            double b = (1 + taza);
            double A = (1 - Math.Pow(b, p)) / taza;

            //Cuota Fija = Monto / A;

            double Cuota_F = Monto / cantidad;
            for (int i = 0; i < cantidad; i++)
            {
                fecha = fecha.AddMonths(1);
                var cuota = new Cuotas();
                cuota.Credito = _creditoDao.Get(creditoId);
                cuota.Valor = Convert.ToDecimal(Cuota_F);
                cuota.Vencimiento = fecha;
                cuota.UpdateUser = userId;
                cuota.DateUpdate = DateTime.Now;

                cuotas.Add(cuota);
            }
            return cuotas;

        }
        public override IEnumerable<CreditoViewModel> Map(IEnumerable<Credito> models)
        {
            return models.Select(Map);
        }
    }
}