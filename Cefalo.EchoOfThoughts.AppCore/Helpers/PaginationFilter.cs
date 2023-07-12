namespace Cefalo.EchoOfThoughts.AppCore.Helpers {
    public class PaginationFilter {
        private int _pageNumber;
        public int PageNumber {
            get => _pageNumber;
            set => _pageNumber = value <= 0 ? 1 : value;
        }

        public int PageSize { get; set; }
    }
}
