using System;
using System.Linq;
using System.Web.Mvc;
using BassinoBase.Models.ViewModelMapper.Interface;
using Domain.Security;
using BassinoLibrary.ViewModels;
using BassinoLibrary.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;

namespace BassinoBase.Controllers
{ 
    public class ModuleController : AbmController<Module, ModuleViewModel, ModuleViewModel>
    {
        private readonly IModuleDao _moduleDao;
        private readonly IModuleViewModelMapper _moduleViewModelMapper;

        public ModuleController(IModuleViewModelMapper moduleViewModelMapper, IModuleDao moduleDao, IUnitOfWorkHelper unitOfWorkHelper, IControllerBehabior abmControllerBahavior, IModuleDao moduleDao1, IModuleViewModelMapper moduleViewModelMapper1)
			: base(abmControllerBahavior, moduleDao, moduleViewModelMapper, unitOfWorkHelper)
        {
            _moduleDao = moduleDao1;
            _moduleViewModelMapper = moduleViewModelMapper1;
        }
        public override void BeforeDelete(Module model)
        {
            model.IsDelete = true;
            base.BeforeDelete(model);
        }
        public override void BeforeSave(Module model, ModuleViewModel viewModel, bool isNew)
        {
            if (isNew)
            {
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

        public override ActionResult GridInfo([DataSourceRequest] DataSourceRequest request, string filtro)
        {
            var result = DAO.GetAllQ(filtro).ToDataSourceResult(request, ViewModelMapper.Map);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAutoComplete(string texto)
        {
            var result = _moduleDao.GetAutoComplete(texto).Take(10);

            var viewModels = _moduleViewModelMapper.Map(result);

            return Json(viewModels, JsonRequestBehavior.AllowGet);
        }
        public override ActionResult Index()
        {
            return PartialView();
        }
    }
}