using System;
using AutoMapper;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Domain.Misc;
using BassinoLibrary.ViewModels;
using BassinoBase.Models.ViewModelMapper;
using BassinoBase.Models.ViewModelMapper.Interface;
using Persistence;


namespace BassinoBase.Models.ViewModelMapper
{
    public class CarViewModelMapper : ViewModelMapper<Car, CarViewModel, CarViewModel>, ICarViewModelMapper
    {
	 private IApplicationDbContext _dbContext;

        public CarViewModelMapper(IApplicationDbContext dbContext)
        {
            Mapper.CreateMap<Car, CarViewModel>()
                .ForMember(model => model.DueDate, opt => opt.Ignore())
                .ForMember(model => model.KiloMaximo, opt => opt.Ignore())
                .ForMember(model => model.PalletMaximo, opt => opt.Ignore())
                .ForMember(model => model.UsoFisico, opt => opt.Ignore())
                .ForMember(model => model.Secuencia, opt => opt.Ignore())
                .ForMember(model => model.DueDateItv, opt => opt.Ignore()); 
            Mapper.CreateMap<CarViewModel, Car>()
                .ForMember(model => model.DueDate, opt => opt.Ignore())
                .ForMember(model => model.KiloMaximo, opt => opt.Ignore())
                .ForMember(model => model.PalletMaximo, opt => opt.Ignore())
                .ForMember(model => model.UsoFisico, opt => opt.Ignore())
                .ForMember(model => model.Secuencia, opt => opt.Ignore())
                .ForMember(model => model.DueDateItv, opt => opt.Ignore());
        }
		  public override CarViewModel Map(Car model)
        {
            var viewModel = base.Map(model);
            viewModel.Secuencia = model.Secuencia;
		      viewModel.DueDate = model.DueDate.ToLongDateString();
		      viewModel.UsoFisico = model.UsoFisico.ToString();
		      viewModel.KiloMaximo = model.KiloMaximo.ToString();
		      viewModel.PalletMaximo = model.PalletMaximo.ToString();
              viewModel.DueDateItv = model.DueDateItv.ToLongDateString();
			return viewModel;
        }

        public override void Map(CarViewModel viewModel, Car model)
        {
            base.Map(viewModel, model);
            model.Secuencia = model.Secuencia;  
            model.UsoFisico = CustomParse(viewModel.UsoFisico.ToString());
            model.KiloMaximo = CustomParse(viewModel.KiloMaximo.ToString());
            model.PalletMaximo =CustomParse(viewModel.PalletMaximo.ToString());
            model.DueDate = Convert.ToDateTime(viewModel.DueDate);
            model.DueDateItv = Convert.ToDateTime(viewModel.DueDateItv);

            //this.Set(hospital => model.UserContact = hospital, viewModel.ContactId, UserExtendedDao);
        }

        public override IEnumerable<CarViewModel> Map(IEnumerable<Car> models)
        {
            return models.Select(Map);
        }
    }
}