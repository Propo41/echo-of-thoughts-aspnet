using System.ComponentModel.DataAnnotations;

namespace Cefalo.EchoOfThoughts.AppCore.Dtos.Story {
    public class StoryCreateDto {
        [Required]
        [MinLength(10)]
        [MaxLength(600)]
        public string Title { get; set; }
        [Required]
        [MinLength(200)]
        [MaxLength(3000)]
        public string Body { get; set; }
    }
}
