using Cefalo.EchoOfThoughts.AppCore.Services.Interfaces;
using Cefalo.EchoOfThoughts.Domain.Entities;
using Microsoft.AspNetCore.Http;
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

        [HttpGet]
        public async Task<IEnumerable<Story>> GetAllAsync() {
            return await _storyService.GetAll();
        }

        [HttpPost]
        public async Task<Story> PostAsync([FromBody] Story story) {
            return await _storyService.Create(story);
        }
    }
}
