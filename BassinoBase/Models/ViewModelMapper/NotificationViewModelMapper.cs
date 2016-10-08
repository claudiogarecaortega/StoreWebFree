using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using Domain.Misc;
using BassinoLibrary.ViewModels;
using BassinoBase.Models.ViewModelMapper;
using BassinoBase.Models.ViewModelMapper.Interface;
using Persistence;


namespace BassinoBase.Models.ViewModelMapper
{
    public class NotificationViewModelMapper : ViewModelMapper<Notification, NotificationViewModel, NotificationViewModel>, INotificationViewModelMapper
    {
	 private IApplicationDbContext _dbContext;

        public NotificationViewModelMapper(IApplicationDbContext dbContext)
        {
            Mapper.CreateMap<Notification, NotificationViewModel>()
                .ForMember(model => model.Messages, opt => opt.Ignore());
            Mapper.CreateMap<NotificationViewModel, Notification>();
        }
		  public override NotificationViewModel Map(Notification model)
        {
            var viewModel = base.Map(model);
		      viewModel.Messages = model.Message;
			return viewModel;
        }

        public override void Map(NotificationViewModel viewModel, Notification model)
        {
            base.Map(viewModel, model);

            //this.Set(hospital => model.UserContact = hospital, viewModel.ContactId, UserExtendedDao);
        }

        public override IEnumerable<NotificationViewModel> Map(IEnumerable<Notification> models)
        {
            return models.Select(Map);
        }
    }
}