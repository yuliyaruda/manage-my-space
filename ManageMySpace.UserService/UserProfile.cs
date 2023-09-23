using System.Linq;
using AutoMapper;
using ManageMySpace.Common.EF.Models;
using ManageMySpace.UserService.API.Models;

namespace ManageMySpace.UserService
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserInfoResponse>()
                .ForMember(ur => ur.Role, u => u.MapFrom(um => um.UserRoles.FirstOrDefault().Role.Name));
        }
    }
}
