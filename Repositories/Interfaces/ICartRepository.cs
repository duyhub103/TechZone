using MyWeb.Models;

public interface ICartRepository
{
    Task<Cart?> GetCartByUserIdAsync(string userId);
    Task<Cart> CreateCartAsync(string userId);

    Task AddToCartAsync(int cartId, int productId, int quantity);
    Task RemoveItemAsync(int cartId, int productId);
    Task UpdateQuantityAsync(int cartId, int productId, int newQuantity);

    Task ClearCartAsync(int cartId);
    Task SaveAsync();
}
