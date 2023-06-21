using Cefalo.EchoOfThoughts.AppCore.Services.Interfaces;
using Cefalo.EchoOfThoughts.Domain.Entities;
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
        public async Task<IEnumerable<Story>> GetAllAsync() {
            return await _storyService.GetAll();
        }

        // GET api/stories/{id}
        [HttpGet("{id}")]
        public async Task<Story> Get(int id) {
            var story = await _storyService.FindById(id);
            return story;
        }

        // POST api/stories
        [HttpPost]
        public async Task<Story> PostAsync([FromBody] Story story) {
            return await _storyService.Create(story);
        }

        // PUT api/stories/{id}
        [HttpPut("{id}")]
        public async Task<Story> UpdateAsync(int id, [FromBody] Story story) {
            return await _storyService.Update(id, story);
        }

    }
}
