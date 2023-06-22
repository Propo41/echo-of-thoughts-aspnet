using Cefalo.EchoOfThoughts.AppCore.Dtos.Story;
using Cefalo.EchoOfThoughts.AppCore.Helpers;
using Cefalo.EchoOfThoughts.AppCore.Helpers.Exceptions;
using Cefalo.EchoOfThoughts.AppCore.Services.Interfaces;
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

        // GET api/stories
        [HttpGet]
        public async Task<IEnumerable<StoryDto>> GetAllAsync() {
            return await _storyService.GetAll();
        }

        // GET api/stories/{id}
        [HttpGet("{id:int}")]
        public async Task<StoryDto> Get(int id) {
            var story = await _storyService.FindById(id);
            return story;
        }

        // POST api/stories
        [HttpPost]
        public async Task<StoryDto> PostAsync([FromBody] StoryDto story) {
            return await _storyService.Create(story);
        }

        // PUT api/stories/{id}
        [HttpPut("{id:int}")]
        public async Task<StoryDto> UpdateAsync(int id, [FromBody] StoryUpdateDto updateDto) {
            if (updateDto == null) {
                throw new BadRequestException("No body provided for update");
            }
            return await _storyService.Update(id, updateDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<Payload> DeleteAsync(int id) {
            return await _storyService.DeleteById(id);
        }

    }
}
