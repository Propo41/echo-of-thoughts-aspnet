using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cefalo.EchoOfThoughts.AppCore.Dtos.Story {
    public class StoriesDto {
        public IEnumerable<StoryDto> Stories { get; set; }
        public int TotalCount { get; set; }
    }
}
