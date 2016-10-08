using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Domain.Contable;
using BassinoLibrary.ViewModels;
using BassinoLibrary.ViewModels;
using BassinoBase.Models.ViewModelMapper.Interface;
using Persistence.Dao.Interfaces;
using BassinoBase.Models.ViewModelMapper.Interfaces;
using Microsoft.AspNet.Identity;
using Persistence.UnitsOfWork;

namespace BassinoBase.Controllers
{ 
    public class CreditoController : AbmController<Credito, CreditoViewModel, CreditoViewModel>
    {
        private readonly IProductDao _productsDao;
        private readonly ICreditoDao _creditDao;
        private readonly IBancariaDao _bancariaDao;
        private readonly ICuentasCobrarDao _cuentaCobrarDao;

        public CreditoController(ICreditoViewModelMapper creditoViewModelMapper, ICreditoDao creditoDao, IUnitOfWorkHelper unitOfWorkHelper, IControllerBehabior abmControllerBahavior, IProductDao productsDao, ICreditoDao creditDao, IBancariaDao bancariaDao, ICuentasCobrarDao cuentaCobrarDao)
			: base(abmControllerBahavior, creditoDao, creditoViewModelMapper, unitOfWorkHelper)
        {
            _productsDao = productsDao;
            _creditDao = creditDao;
            _bancariaDao = bancariaDao;
            _cuentaCobrarDao = cuentaCobrarDao;
        }
        public override void BeforeDelete(Credito model)
        {
            model.DeleteUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
            model.DateDelete = DateTime.Now;
            model.IsDelete = true;
            base.BeforeDelete(model);
        }
        private int GetLastSecuence()
        {
            var model = _creditDao.GetAll().Where(r => !r.IsDelete).OrderByDescending(r => r.DateCreate).FirstOrDefault();
            if (model != null && model.Secuencia != null)
            {
                return (int)(model.Secuencia + 1);

            }
            return 1;
        }

        public override void BeforeSave(Credito model, CreditoViewModel viewModel, bool isNew)
        {
            if (isNew)
            {
                model.Secuencia = GetLastSecuence();
                model.CreateUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
                model.DateCreate = DateTime.Now;
                var cuentasCobrar = new CuentasCobrar();
                cuentasCobrar.Monto = model.Monto;
                cuentasCobrar.CuentaDeposito = _bancariaDao.GetCuentaIgresos();
                cuentasCobrar.FechaVencimiento = model.Cuotas.FirstOrDefault().Vencimiento;
                model.CuentasCobrar = cuentasCobrar;
                
            }
            else
            {
                model.UpdateUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
                model.DateUpdate = DateTime.Now;

                model.CuentasCobrar.Monto = model.Monto;
                model.CuentasCobrar.CuentaDeposito = _bancariaDao.GetCuentaIgresos();
                model.CuentasCobrar.FechaVencimiento = model.Cuotas.FirstOrDefault().Vencimiento;
                

            }
            base.BeforeSave(model, viewModel, isNew);
        }
        [HttpPost]
        public override ActionResult Create(CreditoViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Productos = _productsDao.GetAll();
                return PartialView(viewModel);
            }

            return base.Create(viewModel);
        }

        public override ActionResult Edit(int id)
        {
            ViewBag.CreateUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
            var model = _creditDao.Get(id);
            var js = model.Producto.Select(f => f.Id).ToArray();
            var str = js.Aggregate("", (current, item) => current + item + "-");
            ViewBag.productIds = str;


            return base.Edit(id);
        }
        public override ActionResult Create()
        {
            ViewBag.CreateUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
            ViewBag.Productos = _productsDao.GetAll();
            return base.Create();
        }

        //public override void BeforeSave(Credito model, CreditoViewModel viewModel, bool isNew)
        //{
        //    model.Cuotas = GenerateCoutas(viewModel.NumeroPagos,viewModel.Interes,viewModel.Monto);
        //    base.BeforeSave(model, viewModel, isNew);
        //}
        public JsonResult SimularCuotas(int cantidad, decimal interes, decimal monto)
        {
            var cargas = GenerateCoutas(cantidad,interes,monto);
            if (cargas != null)
            {
                return Json(
                            cargas.Select(x => new
                            {
                                cuota = x.Valor.ToString().Replace('.',','),
                                fecha = x.Vencimiento.ToShortDateString()
                            }), JsonRequestBehavior.AllowGet);
            }
            return Json(cargas, JsonRequestBehavior.AllowGet);

        }
        public JsonResult SimularCuotasAmortizar(int cantidad, decimal interes, decimal monto,decimal montoamortizar)
        {
            var cargas = GenerateCoutas(cantidad, interes, monto-montoamortizar);
            if (cargas != null)
            {
                return Json(
                            cargas.Select(x => new
                            {
                                cuota = x.Valor.ToString().Replace('.', ','),
                                fecha = x.Vencimiento.ToShortDateString()
                            }), JsonRequestBehavior.AllowGet);
            }
            return Json(cargas, JsonRequestBehavior.AllowGet);

        }
        public ActionResult CloseCredit(int id)
        {
            ViewBag.CreateUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
            return base.Edit(id);
        }
        public ActionResult Amortizar(int id)
        {
            ViewBag.CreateUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
            return base.Edit(id);
        }
        [HttpPost]
        public ActionResult CloseCredit(CreditoViewModel viewModel)
        {
            var model = _creditDao.Get(viewModel.Id);
            var montoPgar = model.Monto;
            var user= UserManager.FindById(User.Identity.GetUserId()).UserInfromation;
            model.Pagada = true;
            model.Descripcion = model.Descripcion + " --- Se cerro el credito en fecha" + DateTime.Now +
                                "  por el usuario : " + user.FullName;
            foreach (var cuota in model.Cuotas)
            {
                if (!cuota.Pagada)
                {
                    cuota.Pagada = true;
                    cuota.Descripcion = "El credito se cerro por cancelacion del usuario";
                    cuota.UpdateUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
                    cuota.DateUpdate = DateTime.Now;

                }
            }
            model.Monto = 0;
            model.CuentasCobrar.Monto = model.CuentasCobrar.Monto - viewModel.MontoAmortizar;
            model.CuentasCobrar.Comentarios = model.CuentasCobrar.Comentarios + "Se ha realizado un pago de :" + viewModel.MontoAmortizar + "en fecha: " + DateTime.Now;
            _creditDao.Save();
           
            var bancaria = _bancariaDao.GetCuentaIgresos();
            bancaria.Saldo = bancaria.Saldo + montoPgar;
            _bancariaDao.Save();
            return JavaScript(ABMControllerBehavior.MostrarNotificacionAlert(MensajeOperacionExitosa, 4));
        }
         [HttpPost]
        public ActionResult Amortizar(CreditoViewModel viewModel)
        {
            var user = UserManager.FindById(User.Identity.GetUserId()).UserInfromation;
            var model = _creditDao.Get(viewModel.Id);
            model.Monto = model.Monto - viewModel.MontoAmortizar;
            decimal Cuota_F = model.Monto / model.NumeroPagos;
            model.Descripcion = model.Descripcion + " --- Se hizo una amortuzacion de "+viewModel.MontoAmortizar+" sobre el total en fecha" + DateTime.Now +
                               "  por el usuario : " + user.FullName;
            foreach (var cuota in model.Cuotas)
            {
                cuota.Valor = Cuota_F;

            }
            model.CuentasCobrar.Monto = model.CuentasCobrar.Monto - viewModel.MontoAmortizar;
             model.CuentasCobrar.Comentarios = model.CuentasCobrar.Comentarios + "Se ha realizado un pago de :"+viewModel.MontoAmortizar +"en fecha: "+ DateTime.Now;
            _creditDao.Save();
            var bancaria = _bancariaDao.GetCuentaIgresos();
             
            bancaria.Saldo = bancaria.Saldo + viewModel.MontoAmortizar;
            _bancariaDao.Save();
            return JavaScript(ABMControllerBehavior.MostrarNotificacionAlert(MensajeOperacionExitosa, 4));
        }

        public override void AfterSave(Credito model, CreditoViewModel viewModel, bool isNew)
        {
           
            base.AfterSave(model, viewModel, isNew);
        }

        private IList<Cuotas> GenerateCoutas(int cantidad,decimal interes,decimal monto)
        {
            var cuotas=new List<Cuotas>();
           // interes = interes/100;
            var fecha = DateTime.Now.Date;
            double Monto =Convert.ToDouble(monto);
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
               fecha= fecha.AddMonths(1);
              var cuota=new Cuotas();
                cuota.Valor = Convert.ToDecimal(Math.Round(Cuota_F,2));
                cuota.Vencimiento = fecha;
                cuotas.Add(cuota);
            }
            return cuotas;

        }

    }
}