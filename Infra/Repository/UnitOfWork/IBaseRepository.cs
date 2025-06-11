using System.Linq.Expressions;

namespace Infra.Repository.UnitOfWork {
    public interface IBaseRepository<T> where T : class {

        // O expression seria, por exemplo, um predicate
        // Isso permite fazer o GET no banco, baseado em uma expressão lambda
        Task<T?> Get(Expression<Func<T, bool>> expression); // x => x.id == {parâmetro passado}
        IEnumerable<T> GetAll();
        Task<T> Create(T command);
        Task<T> Update(T commandUpdate);
        Task Delete(Guid id);
    }
}
