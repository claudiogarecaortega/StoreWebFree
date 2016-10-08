using System;
using System.Linq;
using System.Web.Mvc;
using Domain.Contable;
using BassinoLibrary.ViewModels;
using BassinoLibrary.ViewModels;
using BassinoBase.Models.ViewModelMapper.Interface;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using BassinoBase.Models.ViewModelMapper.Interfaces;
using Microsoft.AspNet.Identity;

namespace BassinoBase.Controllers
{ 
    public class CuotasController : AbmController<Cuotas, CuotasViewModel, CuotasViewModel>
    {
        private readonly ICuotasDao _cuotasDao;
        private readonly ICreditoDao _creditoDao;
        private readonly IBancariaDao _bancariaDao;

        public CuotasController(ICuotasViewModelMapper cuotasViewModelMapper, ICuotasDao cuotasDao, IUnitOfWorkHelper unitOfWorkHelper, IControllerBehabior abmControllerBahavior, ICuotasDao cuotasDao1, ICreditoDao creditoDao, IBancariaDao bancariaDao)
			: base(abmControllerBahavior, cuotasDao, cuotasViewModelMapper, unitOfWorkHelper)
        {
            _cuotasDao = cuotasDao1;
            _creditoDao = creditoDao;
            _bancariaDao = bancariaDao;
        }
        public override void BeforeDelete(Cuotas model)
        {
            model.DeleteUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
            model.DateDelete = DateTime.Now;
            model.IsDelete = true;
            base.BeforeDelete(model);
        }
        private int GetLastSecuence()
        {
            var model = _cuotasDao.GetAll().Where(r => !r.IsDelete).OrderByDescending(r => r.DateCreate).FirstOrDefault();
            if (model != null && model.Secuencia != null)
            {
                return (int)(model.Secuencia + 1);

            }
            return 1;
        }

        public override void BeforeSave(Cuotas model, CuotasViewModel viewModel, bool isNew)
        {
            if (isNew)
            {
                model.Secuencia = GetLastSecuence();
                model.CreateUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
                model.DateCreate = DateTime.Now;
            }
            else
            {
                model.UpdateUser = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();
                model.DateUpdate = DateTime.Now;

            }
            base.BeforeSave(model, viewModel, isNew);
        }
        public override ActionResult Edit(CuotasViewModel viewModel)
        {
            var model = _creditoDao.Get(viewModel.CreditoId);
            model.Monto = model.Monto - viewModel.Valor;
            model.CuentasCobrar.Monto = model.CuentasCobrar.Monto - viewModel.Valor;
            model.CuentasCobrar.Comentarios = model.CuentasCobrar.Comentarios + "Se ha realizado un pago de :" + viewModel.Valor + "en fecha: " + DateTime.Now;
            _creditoDao.Save();
            viewModel.Pagada = true;
            return base.Edit(viewModel);
        }

        public override void AfterSave(Cuotas model, CuotasViewModel viewModel, bool isNew)
        {
            var bancaria = _bancariaDao.GetCuentaIgresos();
            bancaria.Saldo = bancaria.Saldo + viewModel.Valor;
            _bancariaDao.Save();
            base.AfterSave(model, viewModel, isNew);
        }
    }
}