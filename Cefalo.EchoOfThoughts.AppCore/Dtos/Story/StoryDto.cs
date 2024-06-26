﻿namespace Cefalo.EchoOfThoughts.AppCore.Dtos.Story {
    public class StoryDto {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime PublishedDate { get; set; }
        public DateTime UpdatedAt { get; set; }
        public AuthorDto Author { get; set; }
    }
}
