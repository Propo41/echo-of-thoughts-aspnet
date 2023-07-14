using System.Runtime.InteropServices.ComTypes;
using Cefalo.EchoOfThoughts.AppCore.Dtos.Story;
using Cefalo.EchoOfThoughts.AppCore.Helpers;
using Cefalo.EchoOfThoughts.AppCore.Helpers.Exceptions;
using Cefalo.EchoOfThoughts.AppCore.Services.Interfaces;
using Cefalo.EchoOfThoughts.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cefalo.EchoOfThoughts.WebApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class StoriesController : ControllerBase {
        private readonly IStoryService _storyService;
        private readonly ILogger<StoriesController> _logger;
        public StoriesController(IStoryService storyService, ILogger<StoriesController> logger) {
            _storyService = storyService;
            _logger = logger;
        }

        // GET api/stories?pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<StoriesDto> GetAllAsync([FromQuery] PaginationFilter filter) {
            _logger.LogInformation("fetching all stories");
            return await _storyService.GetAllAsync(filter.PageNumber, filter.PageSize);
        }

        // GET api/stories/{id}
        [HttpGet("{id:int}")]
        public async Task<StoryDto> GetAsync(int id) {
            _logger.LogInformation("fetching a single story with {id}", id);
            var story = await _storyService.FindByIdAsync(id);
            return story;
        }

        // POST api/stories
        [HttpPost]
        [Authorize]
        public async Task<StoryDto> PostAsync([FromBody] StoryCreateDto story) {
            _logger.LogInformation("creating a new story: {story}", story);
            var authorId = HttpContext.User.FindFirst("Id")?.Value;
            return await _storyService.CreateAsync(int.Parse(authorId!), story);
        }

        // PUT api/stories/{id}
        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<StoryDto> UpdateAsync(int id, [FromBody] StoryUpdateDto updateDto) {
            _logger.LogInformation("updating story with {id} and contents: {story}", id, updateDto);
            if (updateDto == null) {
                throw new BadRequestException("No body provided for update");
            }
            var userId = HttpContext.User.FindFirst("Id")?.Value;
            return await _storyService.UpdateAsync(int.Parse(userId!), id, updateDto);
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<Payload> DeleteAsync(int id) {
            _logger.LogInformation("deleting story with {id}", id);
            var userId = HttpContext.User.FindFirst("Id")?.Value;
            return await _storyService.DeleteByIdAsync(id, int.Parse(userId!));
        }

    }
}
