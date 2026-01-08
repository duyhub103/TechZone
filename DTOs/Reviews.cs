using System.ComponentModel.DataAnnotations;

namespace MyWeb.DTOs.Reviews
{
    public class SubmitReviewRequest
    {
        [Required]
        public int ProductId { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [Required, StringLength(2000)]
        public string Comment { get; set; } = "";
    }
}
