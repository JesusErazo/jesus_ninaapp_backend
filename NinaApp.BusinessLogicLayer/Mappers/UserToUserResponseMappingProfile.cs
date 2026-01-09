using AutoMapper;
using NinaApp.Core.DTO;
using NinaApp.Core.Entities;

namespace NinaApp.Core.Mappers
{
  internal class UserToUserResponseMappingProfile : Profile
  {
    public UserToUserResponseMappingProfile()
    {
      CreateMap<User, UserResponse>();
      CreateMap<UserCreation, User>();

      CreateMap<UserUpdation, User>()
        .ForAllMembers(opts =>
          opts.Condition((_, _, srcMember) => srcMember != null)
        );
    }
  }
}
