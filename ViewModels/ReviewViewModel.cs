namespace MyWeb.ViewModels
{
    public class ReviewViewModel
    {
        public int ProductId { get; set; }

        public int Rating { get; set; } // 1–5

        public string? Comment { get; set; }
    }
}
