using AutoMapper;
using Cefalo.EchoOfThoughts.AppCore.Dtos.Story;
using Cefalo.EchoOfThoughts.AppCore.Helpers;
using Cefalo.EchoOfThoughts.AppCore.Helpers.Exceptions;
using Cefalo.EchoOfThoughts.AppCore.Helpers.Interfaces;
using Cefalo.EchoOfThoughts.AppCore.MappingProfiles;
using Cefalo.EchoOfThoughts.AppCore.Services;
using Cefalo.EchoOfThoughts.Domain.Entities;
using Cefalo.EchoOfThoughts.Domain.Repositories.Interfaces;
using FakeItEasy;
using Microsoft.Extensions.Configuration;

namespace Cefalo.EchoOfThoughts.AppCore.UnitTests {
    public class AuthServiceTests {
        private readonly IUserRepository _userRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly AuthService _sut;
        private readonly IConfiguration _configuration;


        public AuthServiceTests() {
            _userRepository = A.Fake<IUserRepository>();
            _dateTimeProvider = A.Fake<IDateTimeProvider>();
            _configuration = A.Fake<IConfiguration>();
            // mapper config
            var mapperConfiguration = new MapperConfiguration(cfg => {
                cfg.AddProfile<StoryMappingProfile>();
                cfg.AddProfile<UserMappingProfile>();
            });
            var mapper = mapperConfiguration.CreateMapper();
            _sut = new AuthService(_userRepository, mapper, _configuration, _dateTimeProvider);
        }

        [Fact]
        public async void CreateAsync_WithCorrectFields_ReturnsCreatedStory() {
            // arrange
            var storyDto = new StoryDto {
                Title = "hello dog",
                Body = "this is a dog story"
            };
            const int authorId = 1;
            var storyEntity = new Story {
                Id = 12,
                Body = storyDto.Body,
                Title = storyDto.Title,
                AuthorId = authorId
            };
            var publishedTime = DateTime.Today;
            storyEntity.Id = 12;
            storyEntity.PublishedDate = publishedTime;

            A.CallTo(() => _storyRepository.AddAsync(A<Story>._)).Returns(storyEntity);
            A.CallTo(() => _dateTimeProvider.GetCurrentTime()).Returns(publishedTime);

            // act
            var result = await _sut.CreateAsync(authorId, storyDto);

            // assert
            A.CallTo(() => _storyRepository.AddAsync(A<Story>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _dateTimeProvider.GetCurrentTime()).MustHaveHappenedOnceExactly();
            Assert.IsAssignableFrom<StoryDto>(result);
            Assert.Equal(result.PublishedDate, publishedTime);
            Assert.Equal(result.Title, storyDto.Title);
            Assert.Equal(result.Body, storyDto.Body);
        }

    }
}