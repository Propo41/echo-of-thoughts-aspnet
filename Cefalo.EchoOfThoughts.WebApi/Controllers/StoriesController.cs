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

        [HttpPost]
        public Story PostAsync(Story story) {
            Console.WriteLine(story.ToString());
            return story;
        }


    }
}
