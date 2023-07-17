using System.ComponentModel.DataAnnotations;

namespace Cefalo.EchoOfThoughts.AppCore.Dtos.Story {
    public class StoryCreateDto {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Body { get; set; }
    }
}
