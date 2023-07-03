using System.Text;
using Cefalo.EchoOfThoughts.AppCore.Dtos.Story;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace Cefalo.EchoOfThoughts.AppCore.Helpers {
    public class CustomOutputFormatter : TextOutputFormatter {
        public CustomOutputFormatter() {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/plain"));
            SupportedEncodings.Add(Encoding.UTF8);
        }

        protected override bool CanWriteType(Type? type)
            => typeof(StoryDto).IsAssignableFrom(type)
               || typeof(IEnumerable<StoryDto>).IsAssignableFrom(type);

        public override async Task WriteResponseBodyAsync(
            OutputFormatterWriteContext context, Encoding selectedEncoding) {
            var httpContext = context.HttpContext;
            var buffer = new StringBuilder();

            if (context.Object is IEnumerable<StoryDto> stories) {
                foreach (var story in stories) {
                    Format(buffer, story);
                }
            } else {
                Format(buffer, (StoryDto) context.Object!);
            }

            await httpContext.Response.WriteAsync(buffer.ToString(), selectedEncoding);
        }

        private static void Format(
            StringBuilder buffer, StoryDto story) {
            buffer.AppendLine($"Title: {story.Title}");
            buffer.AppendLine($"Body:  {story.Body}");
            buffer.AppendLine($"Date: {story.PublishedDate}");
            if (story.Author != null) {
                buffer.AppendLine($"Author:  {story.Author.Username}");
                buffer.AppendLine($"Author Image:  {story.Author.ProfilePicture}");
            }
            buffer.AppendLine("\n");
        }


    }
}
