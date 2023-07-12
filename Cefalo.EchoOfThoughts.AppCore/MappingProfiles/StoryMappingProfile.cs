using AutoMapper;
using Cefalo.EchoOfThoughts.AppCore.Dtos.Story;
using Cefalo.EchoOfThoughts.Domain.Entities;

namespace Cefalo.EchoOfThoughts.AppCore.MappingProfiles {
    public class StoryMappingProfile : Profile {
        public StoryMappingProfile() {
            CreateMap<Story, StoryUpdateDto>()
               .ForSourceMember(x => x.Id, y => y.DoNotValidate())
               .ForSourceMember(x => x.PublishedDate, y => y.DoNotValidate())
               .ReverseMap();
            CreateMap<User, AuthorDto>();
            CreateMap<Story, StoryDto>()
                .ReverseMap();

            CreateMap<Story, AuthoredStoryDto>();
        }
    }
}
