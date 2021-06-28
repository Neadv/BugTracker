using AutoMapper;
using BugTracker.API.Models;
using BugTracker.API.Models.Responses;
using System.Linq;

namespace BugTracker.API.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserResponse>()
                .ForMember(u => u.Roles, opts => opts.MapFrom(u => u.Roles.Select(r => r.Name)));
        }
    }
}
