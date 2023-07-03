using AutoMapper;
using CarRental.Domain.Core.DTO.Cars;
using CarRental.Domain.Core.DTO.Users;
using CarRental.Domain.Core.Models.Cars;
using CarRental.Domain.Core.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.Core.Profiles
{
    public class CarProfile : Profile
    {
        public CarProfile()
        {
            CreateMap<Car, CarInfo>()
              .ForMember(
                  dest => dest.Id,
                  opt => opt.MapFrom(src => $"{src.Id}")
              )
              .ForMember(
                  dest => dest.Model,
                  opt => opt.MapFrom(src => $"{src.Model}")
              )
              .ForMember(
                  dest => dest.Price,
                  opt => opt.MapFrom(src => $"{src.Price}")
              );
              
              
            CreateMap<CarInfo, Car>()
           .ForMember(
               dest => dest.Id,
               opt => opt.MapFrom(src => $"{src.Id}")
           )
           .ForMember(
               dest => dest.Model,
               opt => opt.MapFrom(src => $"{src.Model}")
           )
           .ForMember(
               dest => dest.Price,
               opt => opt.MapFrom(src => $"{src.Price}")
           );
        }
    }
}
