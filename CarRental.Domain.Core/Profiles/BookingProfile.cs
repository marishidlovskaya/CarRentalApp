using AutoMapper;
using CarRental.Domain.Core.DTO.Bookings;
using CarRental.Domain.Core.DTO.Cars;
using CarRental.Domain.Core.Models.Bookings;
using CarRental.Domain.Core.Models.Cars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.Core.Profiles
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<Booking, BookingInfo>()
              .ForMember(
                  dest => dest.Id,
                  opt => opt.MapFrom(src => $"{src.Id}")
              )
              .ForMember(
                  dest => dest.UserId,
                  opt => opt.MapFrom(src => $"{src.UserId}")
              )
               .ForMember(
                  dest => dest.CarId,
                  opt => opt.MapFrom(src => $"{src.CarId}")
              )
                .ForMember(
                  dest => dest.StartDate,
                  opt => opt.MapFrom(src => $"{src.StartDate}")
              )
                 .ForMember(
                  dest => dest.EndDate,
                  opt => opt.MapFrom(src => $"{src.EndDate}")
              )
                  .ForMember(
                  dest => dest.Price,
                  opt => opt.MapFrom(src => $"{src.Price}")
              )
              .ForMember(
                  dest => dest.Total,
                  opt => opt.MapFrom(src => $"{src.Total}")
              );


            CreateMap<BookingInfo, Booking>()
           .ForMember(
               dest => dest.Id,
               opt => opt.MapFrom(src => $"{src.Id}")
           )
           .ForMember(
               dest => dest.UserId,
               opt => opt.MapFrom(src => $"{src.UserId}")
           )
           .ForMember(
               dest => dest.CarId,
               opt => opt.MapFrom(src => $"{src.CarId}")
           )
            .ForMember(
               dest => dest.StartDate,
               opt => opt.MapFrom(src => $"{src.StartDate}")
           )
             .ForMember(
               dest => dest.EndDate,
               opt => opt.MapFrom(src => $"{src.EndDate}")
           )
              .ForMember(
               dest => dest.Price,
               opt => opt.MapFrom(src => $"{src.Price}")
           )
               .ForMember(
               dest => dest.Total,
               opt => opt.MapFrom(src => $"{src.Total}")
           );
        }
    }
}
