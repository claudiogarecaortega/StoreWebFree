using Domain.Ventas;
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
    public class PedidosViewModelMapper : ViewModelMapper<Pedidos, PedidosViewModel, PedidosViewModel>, IPedidosViewModelMapper
    {
	 private IApplicationDbContext _dbContext;
        private readonly IUserdDao _userDao;

        public PedidosViewModelMapper(IApplicationDbContext dbContext, IUserdDao userDao)
        {
            _userDao = userDao;
            Mapper.CreateMap<Pedidos, PedidosViewModel>()
			//.ForMember(model => model.Categorias, opt => opt.Ignore())
			;
            Mapper.CreateMap<PedidosViewModel, Pedidos>()
			.ForMember(model => model.Usuario, opt => opt.Ignore())
			.ForMember(model => model.Imagenes, opt => opt.Ignore())
			;
        }
		  public override PedidosViewModel Map(Pedidos model)
        {
            var viewModel = base.Map(model);
		      if (model.Usuario != null)
		      {
		          viewModel.Usuario = model.Usuario.PersonUser.FullName();
		          viewModel.UsuarioId = model.Usuario.Id;
		      }
			return viewModel;
        }

        public override void Map(PedidosViewModel viewModel, Pedidos model)
        {
            base.Map(viewModel, model);

            this.Set(hospital => model.Usuario = hospital, viewModel.UsuarioId, _userDao);
        }

        public override IEnumerable<PedidosViewModel> Map(IEnumerable<Pedidos> models)
        {
            return models.Select(Map);
        }
    }
}