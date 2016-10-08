using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using BassinoBase.Models.ViewModelMapper.Interface;
using Domain.Users;

namespace BassinoBase.Models.ViewModelMapper
{
    public class UserViewModelMapper:
        ViewModelMapper<UserExtended, UserExtendedViewModel, UserExtendedViewModel>, IUserViewModelMapper
    {
        public UserViewModelMapper()
        {
            Mapper.CreateMap<UserExtended, UserExtendedViewModel>()
                .ForMember(model => model.userId, opt => opt.Ignore());
            //.ForMember(model => model.ContactName, opt => opt.Ignore());
            //.ForMember(model => model., opt => opt.Ignore());

            Mapper.CreateMap<UserExtendedViewModel, UserExtended>()
                .ForMember(model => model.PersonUser, opt => opt.Ignore())
               
                ;
        }

        public override UserExtendedViewModel Map(UserExtended model)
        {
            var viewModel = base.Map(model);
            viewModel.NombreApellido = model.PersonUser.FullName();
            viewModel.FullName = model.PersonUser.FullName();
            viewModel.Id = model.Id;
            viewModel.userId = model.Id;

            return viewModel;
        }

        public override void Map(UserExtendedViewModel viewModel, UserExtended model)
        {
            base.Map(viewModel, model);

            //  this.Set(hospital => model.Hospital = hospital, viewModel.IdHospital, _hospitalDAO);
        }

        public override IEnumerable<UserExtendedViewModel> Map(IEnumerable<UserExtended> codigoAtcs)
        {
            return codigoAtcs.Select(Map);
        }
    }
}