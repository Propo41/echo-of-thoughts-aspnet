using AutoMapper;
using Cefalo.EchoOfThoughts.AppCore.Dtos.Story;
using Cefalo.EchoOfThoughts.Domain.Entities;

namespace Cefalo.EchoOfThoughts.AppCore.MappingProfiles {
    public class StoryMappingProfile : Profile {
        public StoryMappingProfile() {
            // Map from Story Object to UpdateStoryDto Object
            CreateMap<Story, StoryUpdateDto>()
               .ForSourceMember(x => x.Id, y => y.DoNotValidate())
               .ForSourceMember(x => x.PublishedDate, y => y.DoNotValidate())
               .ReverseMap();

            CreateMap<Story, StoryDto>()
                .ReverseMap();
        }
    }
}
