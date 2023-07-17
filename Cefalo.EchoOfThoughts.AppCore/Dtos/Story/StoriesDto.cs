namespace Cefalo.EchoOfThoughts.AppCore.Dtos.Story {
    public class StoriesDto {
        public IEnumerable<StoryDto> Stories { get; set; }
        public int TotalCount { get; set; }
    }
}
