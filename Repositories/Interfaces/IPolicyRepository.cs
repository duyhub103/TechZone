using MyWeb.Models;

namespace MyWeb.Repositories.Interfaces
{
    public interface IPolicyRepository
    {
        List<Policy> GetAll();
    }
}
