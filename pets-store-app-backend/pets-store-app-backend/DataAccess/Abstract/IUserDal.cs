using Core.DataAccess;
using Core.Entities.Concrete;

namespace DataAccesss.Abstract
{
    public interface IUserDal: IEntityRepository<User>
    {
        public Task<List<OperationClaim>> GetClaims(User user);
    }
}
