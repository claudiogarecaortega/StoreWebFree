using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using Domain.Misc;
using BassinoLibrary.ViewModels;
using BassinoBase.Models.ViewModelMapper;
using BassinoBase.Models.ViewModelMapper.Interface;
using Domain.Users;
using Persistence;
using Persistence.Dao.Interfaces;


namespace BassinoBase.Models.ViewModelMapper
{
    public class MessagingViewModelMapper : ViewModelMapper<Messaging, MessagingViewModel, MessagingViewModel>, IMessagingViewModelMapper
    {
	 private IApplicationDbContext _dbContext;
        private readonly IUserdDao _userExtendedDao;
        private readonly IMessagingDao _messagingDao;

        public MessagingViewModelMapper(IApplicationDbContext dbContext, IUserdDao userExtendedDao, IMessagingDao messagingDao)
        {
            _userExtendedDao = userExtendedDao;
            _messagingDao = messagingDao;
            Mapper.CreateMap<Messaging, MessagingViewModel>();
            Mapper.CreateMap<MessagingViewModel, Messaging>()
                 .ForMember(model => model.MessagingParent, opt => opt.Ignore())
                 .ForMember(model => model.UserReciver, opt => opt.Ignore())
                 .ForMember(model => model.UserSend, opt => opt.Ignore())
                ;
        }
		  public override MessagingViewModel Map(Messaging model)
        {
            var viewModel = base.Map(model);
		      if (model.MessagingParent != null)
		      {
		          viewModel.MessagingParent = model.MessagingParent.Message;
		          viewModel.MessagingParentId = model.MessagingParent.Id;
		      }
		      if (model.UserReciver!=null)
		      {
		          //viewModel.UserReciver = model.UserReciver;
		          viewModel.UserReciverId = model.UserReciver.Select(c=>c.Id).ToArray();

		      }
              if (model.UserSend != null)
              {
                  viewModel.UserSend = model.UserSend.FullName;
                  viewModel.UserSendId = model.UserSend.Id;

              }
		      return viewModel;
        }

        public override void Map(MessagingViewModel viewModel, Messaging model)
        {
            base.Map(viewModel, model);
            model.DateSend = DateTime.Now;
            //this.SetCollectionFromMultiSelect(hospital => model.UserReciver = hospital, viewModel.UserReciverId, _userExtendedDao);

            if (model.UserReciver == null)
                model.UserReciver = new List<UserExtended>();

            //foreach (var item in viewModel.UbicationRoute )
            //{
            //    model.UbicationRoute.Add(_ubicationDao.Get(item));
            //}
            this.SetCollectionFromMultiSelect(viewModel.UserReciverId, _userExtendedDao, model.UserReciver);

            this.Set(hospital => model.UserSend = hospital, viewModel.UserSendId, _userExtendedDao);
            this.Set(hospital => model.MessagingParent = hospital, viewModel.MessagingParentId, _messagingDao);
        }

        public override IEnumerable<MessagingViewModel> Map(IEnumerable<Messaging> models)
        {
            return models.Select(Map);
        }
    }
}