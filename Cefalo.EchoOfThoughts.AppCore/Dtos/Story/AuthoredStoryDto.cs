﻿namespace Cefalo.EchoOfThoughts.AppCore.Dtos.Story {
    public class AuthoredStoryDto {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime PublishedDate { get; set; }
    }
}
