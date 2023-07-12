using AutoMapper;
using Cefalo.EchoOfThoughts.AppCore.Dtos.Story;
using Cefalo.EchoOfThoughts.AppCore.Dtos.User;
using Cefalo.EchoOfThoughts.AppCore.Helpers;
using Cefalo.EchoOfThoughts.AppCore.Helpers.Exceptions;
using Cefalo.EchoOfThoughts.AppCore.MappingProfiles;
using Cefalo.EchoOfThoughts.AppCore.Services;
using Cefalo.EchoOfThoughts.Domain.Entities;
using Cefalo.EchoOfThoughts.Domain.Repositories;
using Cefalo.EchoOfThoughts.Domain.Repositories.Interfaces;
using FakeItEasy;

namespace Cefalo.EchoOfThoughts.AppCore.UnitTests {
    public class StoryServiceTests {
        private readonly IStoryRepository _storyRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly StoryService _sut;

        public StoryServiceTests() {
            _storyRepository = A.Fake<IStoryRepository>();
            _dateTimeProvider = A.Fake<IDateTimeProvider>();
            // mapper config
            var mapperConfiguration = new MapperConfiguration(cfg => {
                cfg.AddProfile<StoryMappingProfile>();
                cfg.AddProfile<UserMappingProfile>();
            });
            var mapper = mapperConfiguration.CreateMapper();
            _sut = new StoryService(_storyRepository, mapper, _dateTimeProvider);

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


        [Fact]
        public async void FindByIdAsync_WithValidId_ReturnsStory() {
            // arrange
            const int id = 1;
            var story = new Story {
                Id = id,
            };
            A.CallTo(() => _storyRepository.FindByIdAsync(id, A<bool>._)).Returns(story);

            // act
            var result = await _sut.FindByIdAsync(id);

            // assert
            A.CallTo(() => _storyRepository.FindByIdAsync(id, A<bool>._)).MustHaveHappenedOnceExactly();
            Assert.IsType<StoryDto>(result);
            Assert.Equal(id, result.Id);
        }

        [Fact]
        public async void GetAllAsync_WithPagination_ReturnsList() {
            // arrange
            const int pageNumber = 1;
            const int pageSize = 10;
            const int totalCount = 30;

            var stories = new List<Story> {
                new() { Title = "ali" },
                new() { Title = "ahnaf" }
            };

            A.CallTo(() => _storyRepository.FindAllAsync(A<int>._, pageSize, A<bool>._)).Returns((totalCount, stories));

            // act
            var result = await _sut.GetAllAsync(pageNumber, pageSize);

            // assert
            Assert.IsType<StoriesDto>(result);
            Assert.Equal(result.TotalCount, totalCount);
            Assert.Equal(result.Stories.Count(), stories.Count);
            A.CallTo(() => _storyRepository.FindAllAsync(A<int>._, pageSize, A<bool>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void UpdateAsync_InvalidId_ThrowsNotFoundException() {
            // arrange
            const int blogId = 1;
            const int userId = 21;
            Story? story = null;
            var storyToUpdate = new StoryUpdateDto {
                Body = "new body",
                Title = "new title"
            };
            A.CallTo(() => _storyRepository.FindByIdAsync(blogId, A<bool>._)).Returns(story);

            // act & assert
            await Assert.ThrowsAsync<NotFoundException>(
                async () => await _sut.UpdateAsync(userId, blogId, storyToUpdate));
            A.CallTo(() => _storyRepository.FindByIdAsync(blogId, A<bool>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void UpdateAsync_InvalidAuthorId_ThrowsUnauthorizedException() {
            // arrange
            const int blogId = 1;
            const int userId = 1;
            var story = new Story {
                AuthorId = 10
            };
            var storyToUpdate = new StoryUpdateDto {
                Body = "new body",
                Title = "new title"
            };
            A.CallTo(() => _storyRepository.FindByIdAsync(blogId, A<bool>._)).Returns(story);

            // act & assert
            await Assert.ThrowsAsync<UnauthorizedException>(
                async () => await _sut.UpdateAsync(userId, blogId, storyToUpdate));
            A.CallTo(() => _storyRepository.FindByIdAsync(blogId, A<bool>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void UpdateAsync_ValidParameters_ReturnsUpdatedStory() {
            // arrange
            const int blogId = 1;
            const int userId = 1;
            var existingStory = new Story {
                AuthorId = 1,
                Title = "old title",
                Body = "old body",
            };
            var storyToUpdate = new StoryUpdateDto {
                Body = "new body",
                Title = "new title"
            };
            var updatedTime = DateTime.Today;
            var updatedStory = new Story {
                AuthorId = existingStory.AuthorId,
                Title = storyToUpdate.Title,
                Body = storyToUpdate.Body,
                UpdatedAt = updatedTime
            };

            A.CallTo(() => _storyRepository.FindByIdAsync(blogId, A<bool>._)).Returns(existingStory);
            A.CallTo(() => _storyRepository.UpdateAsync(existingStory)).Returns(updatedStory);
            A.CallTo(() => _dateTimeProvider.GetCurrentTime()).Returns(updatedTime);

            // act
            var result = await _sut.UpdateAsync(userId, blogId, storyToUpdate);

            // assert
            Assert.Equal(result.UpdatedAt, updatedTime);
            Assert.Equal(result.Title, storyToUpdate.Title);
            Assert.Equal(result.Body, storyToUpdate.Body);
            A.CallTo(() => _storyRepository.FindByIdAsync(blogId, A<bool>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _storyRepository.UpdateAsync(existingStory)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _dateTimeProvider.GetCurrentTime()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void DeleteAsync_InvalidStoryId_ThrowsNotFoundException() {
            // arrange
            const int blogId = 1;
            const int userId = 1;
            Story? story = null;
            A.CallTo(() => _storyRepository.FindByIdAsync(blogId, A<bool>._)).Returns(story);

            // act & assert
            await Assert.ThrowsAsync<NotFoundException>(
                async () => await _sut.DeleteByIdAsync(blogId, userId));
            A.CallTo(() => _storyRepository.FindByIdAsync(blogId, A<bool>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _storyRepository.DeleteAsync(A<Story>._)).MustNotHaveHappened();
        }

        [Fact]
        public async void DeleteAsync_CorrectStoryId_ReturnsPayload() {
            // arrange
            const int blogId = 1;
            const int userId = 1;
            var story = new Story {
                Id = 1,
                AuthorId = userId
            };
            A.CallTo(() => _storyRepository.FindByIdAsync(blogId, A<bool>._)).Returns(story);

            // act
            var result = await _sut.DeleteByIdAsync(blogId, userId);

            // assert
            Assert.IsType<Payload>(result);
            A.CallTo(() => _storyRepository.FindByIdAsync(blogId, A<bool>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _storyRepository.DeleteAsync(A<Story>._)).MustHaveHappenedOnceExactly();
        }
    }
}