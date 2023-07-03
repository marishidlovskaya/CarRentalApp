using AutoMapper;
using CarRental.Domain.Core.DTO.Users;
using CarRental.Domain.Core.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.Core.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserInfo> ()
              .ForMember(
                  dest => dest.Id,
                  opt => opt.MapFrom(src => $"{src.Id}")
              )
              .ForMember(
                  dest => dest.FirstName,
                  opt => opt.MapFrom(src => $"{src.FirstName}")
              )
              .ForMember(
                  dest => dest.LastName,
                  opt => opt.MapFrom(src => $"{src.LastName}")
              )
              .ForMember(
                  dest => dest.Email,
                  opt => opt.MapFrom(src => $"{src.Email}")
              )
              .ForMember(
                  dest => dest.RegistrationDate,
                  opt => opt.MapFrom(src => $"{src.RegistrationDate}")
              );


            CreateMap<UserInfo, User>()
           .ForMember(
               dest => dest.Id,
               opt => opt.MapFrom(src => $"{src.Id}")
           )
           .ForMember(
               dest => dest.FirstName,
               opt => opt.MapFrom(src => $"{src.FirstName}")
           )
           .ForMember(
               dest => dest.LastName,
               opt => opt.MapFrom(src => $"{src.LastName}")
           )
           .ForMember(
               dest => dest.Email,
               opt => opt.MapFrom(src => $"{src.Email}")
           )
           .ForMember(
               dest => dest.RegistrationDate,
               opt => opt.MapFrom(src => $"{src.RegistrationDate}")
           );
        }
    }
}
