using AutoMapper;
using ManageMySpace.ActivityService.API.Models;
using ManageMySpace.Common.EF.Models;
using System.Linq;
using ManageMySpace.Common.Commands.ActivityCommands;

namespace ManageMySpace.ActivityService.Infrastructure.AutoMapper
{
    public class ActivityProfile : Profile
    {
        public ActivityProfile()
        {
            CreateMap<CreateRoomRequest, CreateRoom>();
            CreateMap<CreateRoom, CreateRoomRequest>();

            CreateMap<Activity, ActivityResponseModel>()
                .ForMember(a => a.VisitorsNumber, am => am.MapFrom(m => m.Visitors.Count))
                .ForMember(a => a.MaxCapacity, am => am.MapFrom(m => m.Reservations.FirstOrDefault().Room.Capacity))
                .ForMember(a => a.RoomNumber, am => am.MapFrom(m => m.Reservations.FirstOrDefault().Room.RoomNumber))
                .ForMember(a => a.Duration, am => am.MapFrom(m => m.Reservations.FirstOrDefault().DurationInMinutes))
                .ForMember(a => a.OrganizatorName, am => am.MapFrom(m => $"{m.Organizators.FirstOrDefault().User.Name} {m.Organizators.FirstOrDefault().User.LastName}"));

            CreateMap<ActivityResponseModel, Activity>();
        }
    }
}
