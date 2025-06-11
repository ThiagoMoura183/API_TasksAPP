using Domain.Entity;
using Infra.Persistency;
using Infra.Repository.IRepositories;
using Infra.Repository.UnitOfWork;

namespace Infra.Repository.Repositories {
    public class UserRepository(TasksDbContext context) : BaseRepository<User>(context), IUserRepository {
    }
}
