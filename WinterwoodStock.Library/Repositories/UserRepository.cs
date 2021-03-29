using WinterwoodStock.Library.Entities;
using WinterwoodStock.Library.Interfaces.Repositories;

namespace WinterwoodStock.Library.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(WinterwoodDbContext context) : base(context)
        {
        }
    }
}
