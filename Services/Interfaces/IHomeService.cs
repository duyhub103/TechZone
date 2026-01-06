using MyWeb.ViewModels;

namespace MyWeb.Services.Interfaces
{
    public interface IHomeService
    {
        Task<HomeViewModel> GetHomeViewModelAsync();
    }
}
