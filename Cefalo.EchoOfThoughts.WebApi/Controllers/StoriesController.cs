using System.Runtime.InteropServices.ComTypes;
using Cefalo.EchoOfThoughts.AppCore.Dtos.Story;
using Cefalo.EchoOfThoughts.AppCore.Helpers;
using Cefalo.EchoOfThoughts.AppCore.Helpers.Exceptions;
using Cefalo.EchoOfThoughts.AppCore.Services.Interfaces;
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
        public async Task<IEnumerable<StoryDto>> GetAllAsync([FromQuery] PaginationFilter filter) {
            return await _storyService.GetAll(filter.PageNumber, filter.PageSize);
        }

        // GET api/stories/{id}
        [HttpGet("{id:int}")]
        public async Task<StoryDto> Get(int id) {
            var story = await _storyService.FindById(id);
            return story;
        }

        // POST api/stories
        [HttpPost]
        [Authorize]
        public async Task<StoryDto> PostAsync([FromBody] StoryDto story) {
            var authorId = HttpContext.User.FindFirst("Id")?.Value;
            return await _storyService.Create(int.Parse(authorId!), story);
        }

        // PUT api/stories/{id}
        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<StoryDto> UpdateAsync(int id, [FromBody] StoryUpdateDto updateDto) {
            if (updateDto == null) {
                throw new BadRequestException("No body provided for update");
            }
            var userId = HttpContext.User.FindFirst("Id")?.Value;
            return await _storyService.Update(int.Parse(userId!), id, updateDto);
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<Payload> DeleteAsync(int id) {
            var userId = HttpContext.User.FindFirst("Id")?.Value;
            return await _storyService.DeleteById(id, int.Parse(userId!));
        }

    }
}
