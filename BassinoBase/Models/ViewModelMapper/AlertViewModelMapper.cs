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
    public class AlertViewModelMapper : ViewModelMapper<Alert, AlertViewModel, AlertViewModel>, IAlertViewModelMapper
    {
        private readonly IUserdDao _userDao;
        //private iuser _userAlertDao;

        public AlertViewModelMapper(IUserdDao userDao)
        {
            _userDao = userDao;
            Mapper.CreateMap<Alert, AlertViewModel>()
                .ForMember(model => model.UsersAlert, opt => opt.Ignore())
                ;
            Mapper.CreateMap<AlertViewModel, Alert>()
                .ForMember(model => model.UsersAlert, opt => opt.Ignore())
                ;
        }
        public override AlertViewModel Map(Alert model)
        {
            var viewModel = base.Map(model);
            if (model.UsersAlert.Any())
                viewModel.UsersAlert = model.UsersAlert.Select(d => d.Id).ToArray();
            return viewModel;
        }

        public override void Map(AlertViewModel viewModel, Alert model)
        {
            base.Map(viewModel, model);


            if (model.UsersAlert == null)
                model.UsersAlert = new List<UserAlert>();

            if(viewModel.IsForAll)
            {
                viewModel.UsersAlert = _userDao.GetAll().Select(d => d.Id).ToArray();
            }

            model.UsersAlert=new List<UserAlert>();
            for (int i = 0; i < viewModel.UsersAlert.Length; i++)
            {
                var useraler = new UserAlert();
                useraler.User = _userDao.Get(viewModel.UsersAlert[i]);
                model.UsersAlert.Add(useraler);
                
            }
            //this.SetCollectionFromMultiSelect(viewModel.UsersAlert, _userDao, model.UsersAlert);
       
            //this.Set(hospital => model.UserContact = hospital, viewModel.ContactId, UserExtendedDao);
        }

        public override IEnumerable<AlertViewModel> Map(IEnumerable<Alert> models)
        {
            return models.Select(Map);
        }
    }
}