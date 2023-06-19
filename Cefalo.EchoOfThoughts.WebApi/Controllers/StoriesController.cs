using Cefalo.EchoOfThoughts.AppCore.Services.Interfaces;
using Cefalo.EchoOfThoughts.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cefalo.EchoOfThoughts.WebApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class StoriesController : ControllerBase {
        private readonly IStoryService _storyService;

        public StoriesController(IStoryService storyService) {
            _storyService = storyService;
        }

        [HttpGet]
        public async Task<IEnumerable<Story>> GetAllAsync() {
            var res = await _storyService.GetAll();
            return res;
        }

        [HttpPost]
        public async Task<Story> PostAsync([FromBody] Story story) {
            var res = await _storyService.Create(story);
            return res;
        }
    }
}
