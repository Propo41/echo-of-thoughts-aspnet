using AutoMapper;
using Cefalo.EchoOfThoughts.AppCore.Dtos.User;
using Cefalo.EchoOfThoughts.Domain.Entities;

namespace Cefalo.EchoOfThoughts.AppCore.MappingProfiles {
    public class UserMappingProfile : Profile {
        public UserMappingProfile() {
            CreateMap<User, UserUpdateDto>()
               .ForSourceMember(x => x.Id, y => y.DoNotValidate())
               .ForSourceMember(x => x.CreatedAt, y => y.DoNotValidate())
               .ForSourceMember(x => x.IsDisabled, y => y.DoNotValidate())
               .ForSourceMember(x => x.PasswordHash, y => y.DoNotValidate())
               .ForSourceMember(x => x.PasswordUpdatedAt, y => y.DoNotValidate())
               .ForSourceMember(x => x.Role, y => y.DoNotValidate())
               .ForSourceMember(x => x.UserName, y => y.DoNotValidate())
               .ForSourceMember(x => x.Stories, y => y.DoNotValidate())
               .ReverseMap();

            CreateMap<User, UserDto>()
                .ReverseMap();

            CreateMap<User, UserSignUpDto>()
                .ReverseMap();
        }
    }
}