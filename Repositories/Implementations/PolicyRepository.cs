using MyWeb.Data;
using MyWeb.Repositories.Interfaces;
using MyWeb.Models;

namespace MyWeb.Repositories.Implementations
{
    public class PolicyRepository : IPolicyRepository
    {
        private readonly TechZoneDbContext _context;

        public PolicyRepository(TechZoneDbContext context)
        {
            _context = context;
        }

        public List<Policy> GetAll()
        {
            return _context.Policies.OrderBy(x => x.Id).ToList();
        }
    }
}
