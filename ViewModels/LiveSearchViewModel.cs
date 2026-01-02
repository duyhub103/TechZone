using MyWeb.Models;

namespace MyWeb.ViewModels
{
    public class LiveSearchViewModel
    {
        public List<string> Suggestions { get; set; } = new List<string>(); // key gợi ý "Laptop Dell", "Asus Gaming"
        public List<Product> Products { get; set; } = new List<Product>(); // Danh sách sản phẩm theo key
    }
}
