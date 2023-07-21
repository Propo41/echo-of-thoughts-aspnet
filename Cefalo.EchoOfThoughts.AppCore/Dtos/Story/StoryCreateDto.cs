using System.ComponentModel.DataAnnotations;

namespace Cefalo.EchoOfThoughts.AppCore.Dtos.Story {
    public class StoryCreateDto {
        [Required]
        [StringLength(maximumLength: 600, MinimumLength = 10)]
        public string Title { get; set; }
        [Required]
        [StringLength(maximumLength: 3000, MinimumLength = 200)]
        public string Body { get; set; }
    }
}
