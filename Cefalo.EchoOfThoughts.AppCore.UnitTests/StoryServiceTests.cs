using AutoMapper;
using Cefalo.EchoOfThoughts.AppCore.Dtos.Story;
using Cefalo.EchoOfThoughts.AppCore.Helpers;
using Cefalo.EchoOfThoughts.AppCore.Helpers.Exceptions;
using Cefalo.EchoOfThoughts.AppCore.Helpers.Interfaces;
using Cefalo.EchoOfThoughts.AppCore.MappingProfiles;
using Cefalo.EchoOfThoughts.AppCore.Services;
using Cefalo.EchoOfThoughts.AppCore.Services.Interfaces;
using Cefalo.EchoOfThoughts.Domain.Entities;
using Cefalo.EchoOfThoughts.Domain.Repositories.Interfaces;
using FakeItEasy;

namespace Cefalo.EchoOfThoughts.AppCore.UnitTests {
    public class StoryServiceTests {
        private readonly IStoryRepository _storyRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IStoryService _sut;

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

        private static Story CreateFakeStory(int id = 1, string body = "empty", string title = "empty", int authorId = 2) {
            return new Story {
                Id = id,
                Body = body,
                Title = title,
                AuthorId = authorId
            };
        }
        private static StoryDto CreateFakeStoryDto(string title = "empty", string body = "empty") {
            return new StoryDto {
                Title = title,
                Body = body
            };
        }

        [Fact]
        public async void CreateAsync_WithCorrectFields_ReturnsCreatedStory() {
            // arrange
            var storyDto = new StoryCreateDto {
                Title = "title",
                Body = "body"
            };
            const int authorId = 1;
            var storyEntity = CreateFakeStory(0, storyDto.Body, storyDto.Title, authorId);

            var publishedTime = DateTime.Today;
            storyEntity.PublishedDate = publishedTime;
            storyEntity.UpdatedAt = publishedTime;

            A.CallTo(() => _dateTimeProvider.GetCurrentTime()).Returns(publishedTime);
            A.CallTo(() => _storyRepository.AddAsync(A<Story>._)).Returns(storyEntity);

            // act
            var result = await _sut.CreateAsync(authorId, storyDto);

            // assert
            A.CallTo(() => _storyRepository.AddAsync(A<Story>.That.Matches(x => x.Title == storyDto.Title)))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _dateTimeProvider.GetCurrentTime()).MustHaveHappenedOnceExactly();

            Assert.IsAssignableFrom<StoryDto>(result);
            Assert.Equal(result.PublishedDate, publishedTime);
            Assert.Equal(result.Title, storyDto.Title);
            Assert.Equal(result.Body, storyDto.Body);
        }

        [Fact]
        public async void FindByIdAsync_WithStoryIdThatExists_ReturnsStory() {
            // arrange
            const int id = 1;
            var story = CreateFakeStory(id);
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
            const bool includeAuthor = false;

            var stories = new List<Story> {
                CreateFakeStory(1),
                CreateFakeStory(2)
            };

            A.CallTo(() => _storyRepository.FindAllAsync(A<int>.Ignored, pageSize, includeAuthor)).Returns((totalCount, stories));

            // act
            var result = await _sut.GetAllAsync(pageNumber, pageSize);

            // assert
            Assert.IsType<StoriesDto>(result);
            Assert.Equal(result.TotalCount, totalCount);
            Assert.Equal(result.Stories.Count(), stories.Count);
            A.CallTo(() => _storyRepository.FindAllAsync(
                A<int>.That.Matches(x => true),
                pageSize,
                includeAuthor)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void UpdateAsync_WithNonExistentStoryId_ThrowsNotFoundException() {
            // arrange
            const int blogId = 1;
            const int userId = 21;
            Story? story = null;
            var storyToUpdate = new StoryUpdateDto {
                Body = "new body",
                Title = "new title"
            };
            A.CallTo(() => _storyRepository.FindByIdAsync(blogId, A<bool>._)).Returns(story);

            // act
            var exception = await Record.ExceptionAsync(() => _sut.UpdateAsync(userId, blogId, storyToUpdate));

            // assert
            Assert.IsType<NotFoundException>(exception);
            A.CallTo(() => _storyRepository.FindByIdAsync(blogId, A<bool>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void UpdateAsync_WithDifferentAuthorsId_ThrowsUnauthorizedException() {
            // arrange
            const int blogId = 1;
            const int userId = 1;
            var story = CreateFakeStory(authorId: 10);
            var storyToUpdate = new StoryUpdateDto {
                Body = "new body",
                Title = "new title"
            };
            A.CallTo(() => _storyRepository.FindByIdAsync(blogId, A<bool>._)).Returns(story);

            // act
            var exception = await Record.ExceptionAsync(() => _sut.UpdateAsync(userId, blogId, storyToUpdate));

            // assert
            Assert.IsType<UnauthorizedException>(exception);
            A.CallTo(() => _storyRepository.FindByIdAsync(blogId, A<bool>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void UpdateAsync_ValidParameters_ReturnsUpdatedStory() {
            // arrange
            const int blogId = 1;
            const int userId = 1;
            var existingStory = CreateFakeStory(authorId: 1);

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
            A.CallTo(() => _storyRepository.FindByIdAsync(blogId, A<bool>.That.Matches(x => true))).MustHaveHappenedOnceExactly();
            A.CallTo(() => _storyRepository.UpdateAsync(existingStory)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _dateTimeProvider.GetCurrentTime()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void DeleteAsync_WithNonExistentStoryId_ThrowsNotFoundException() {
            // arrange
            const int blogId = 1;
            const int userId = 1;
            Story? story = null;
            A.CallTo(() => _storyRepository.FindByIdAsync(blogId, A<bool>._)).Returns(story);

            // act
            var exception = await Record.ExceptionAsync(() => _sut.DeleteByIdAsync(blogId, userId));

            // assert
            Assert.IsType<NotFoundException>(exception);
            A.CallTo(() => _storyRepository.FindByIdAsync(blogId, A<bool>.That.Matches(x => true))).MustHaveHappenedOnceExactly();
            A.CallTo(() => _storyRepository.DeleteAsync(A<Story>.That.Matches(x => true))).MustNotHaveHappened();
        }

        [Fact]
        public async void DeleteAsync_WithExistingStoryId_ReturnsPayload() {
            // arrange
            const int blogId = 1;
            const int userId = 1;
            var story = CreateFakeStory(1, authorId: userId);
            A.CallTo(() => _storyRepository.FindByIdAsync(blogId, A<bool>._)).Returns(story);

            // act
            var result = await _sut.DeleteByIdAsync(blogId, userId);

            // assert
            Assert.IsType<Payload>(result);
            A.CallTo(() => _storyRepository.FindByIdAsync(blogId, A<bool>.That.Matches(x => true))).MustHaveHappenedOnceExactly();
            A.CallTo(() => _storyRepository.DeleteAsync(A<Story>.That.Matches(x => true))).MustHaveHappenedOnceExactly();
        }
    }
}