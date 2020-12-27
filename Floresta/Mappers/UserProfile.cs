using AutoMapper;
using Floresta.Models;
using Floresta.ViewModels;

namespace Floresta.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, EditUserViewModel>()
                .ForMember(dest => 
                dest.Surname,
                opt => opt.MapFrom(src => src.UserSurname));
                
        }
    }
}
