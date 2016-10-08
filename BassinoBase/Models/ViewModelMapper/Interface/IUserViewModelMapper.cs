using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Users;

namespace BassinoBase.Models.ViewModelMapper.Interface
{
    public interface IUserViewModelMapper :
        IViewModelMapper<UserExtended, UserExtendedViewModel, UserExtendedViewModel>
    {
    }
}