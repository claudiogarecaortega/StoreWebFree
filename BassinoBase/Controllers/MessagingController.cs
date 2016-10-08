using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BassinoBase.Models.ViewModelMapper.Interface;
using Domain.Misc;
using BassinoLibrary.ViewModels;
using BassinoLibrary.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;

namespace BassinoBase.Controllers
{ 
    public class MessagingController : AbmController<Messaging, MessagingViewModel, MessagingViewModel>
    {
        private readonly IMessagingDao _messagingDao;
        private readonly IMessagingViewModelMapper _messagingViewModelMapper;
        private readonly IUserdDao _userDao;

        public MessagingController(IMessagingViewModelMapper messagingViewModelMapper, IMessagingDao messagingDao, IUnitOfWorkHelper unitOfWorkHelper, IControllerBehabior abmControllerBahavior, IMessagingDao messagingDao1, IMessagingViewModelMapper messagingViewModelMapper1, IUserdDao userDao)
			: base(abmControllerBahavior, messagingDao, messagingViewModelMapper, unitOfWorkHelper)
        {
            _messagingDao = messagingDao1;
            _messagingViewModelMapper = messagingViewModelMapper1;
            _userDao = userDao;
        }

        public override void BeforeSave(Messaging model, MessagingViewModel viewModel, bool isNew)
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

        public override ActionResult Create(MessagingViewModel viewModel)
        {
            viewModel.UserSendId =UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id;
            return base.Create(viewModel);
        }

        public override ActionResult Index()
        {

            return PartialView();
        }
        public ActionResult IndexOut()
        {

            return PartialView();
        }
        private void MarkAsRead(int id)
        {
            var model = _messagingDao.Get(id);
            model.IsRead = true;
            _messagingDao.Save();
        }

        public override ActionResult Details(int id)
        {
            MarkAsRead(id);
            return base.Details(id);
        }

        public ActionResult Reply(int id)
        {
            MarkAsRead(id);
            var model = _messagingDao.Get(id);
            var nodesList = new List<Messaging> { model };
            var viewmodel = _messagingViewModelMapper.Map(model);
            var nodes = model.GetAllNodes();
            nodesList.AddRange(model.GetAllNodes());
                ViewBag.nodes = nodesList;
            
            viewmodel.UserReciver = model.UserSend.FullName;
           // viewmodel.UserReciverId = model.UserSend.Id;
            viewmodel.MessagingParentId = id;
            return PartialView(viewmodel);

        }
        [HttpPost]
        public ActionResult Reply(MessagingViewModel viewModel)
        {
            viewModel.UserSendId = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id;
            viewModel.Id = 0;
            return base.Create(viewModel);

        }
        public JsonResult GetNewMessages()
        {
            var ubications = _messagingDao.GetAllUserUnread(UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id).OrderByDescending(c=>c.DateSend);
            return Json(
                          ubications.Select(x => new
                          {
                              from = x.UserSend.FullName,
                              subject = x.Subject,
                              time=x.GetTime(),
                              IsUrgent = x.IsUrgent,
                              id=x.Id
                          }), JsonRequestBehavior.AllowGet);

        }
        public override ActionResult GridInfo(DataSourceRequest request, string id)
        {
            id = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();

            var result = _messagingDao.GetAllUSer(Convert.ToInt32(id)).ToDataSourceResult(request, ViewModelMapper.Map);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUsers()
        {

            var ubications = _userDao.GetAll();
            return Json(
  ubications.Select(x => new
  {
      Name = x.FullName,
      id = x.Id
  }), JsonRequestBehavior.AllowGet);
        }

        public  ActionResult GridInfoSendItems(DataSourceRequest request, string id)
        {
            id = UserManager.FindById(User.Identity.GetUserId()).UserInfromation.Id.ToString();

            var result = _messagingDao.GetAllUSent(Convert.ToInt32(id)).ToDataSourceResult(request, ViewModelMapper.Map);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}