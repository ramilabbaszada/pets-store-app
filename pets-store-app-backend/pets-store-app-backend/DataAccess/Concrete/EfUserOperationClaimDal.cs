using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccesss.Abstract;
using DataAccesss.Concrete.Contexts;

namespace DataAccesss.Concrete
{
    public class EfUserOperationClaimDal: EfEntityRepositoryBase<UserOperationClaim, TestProjectContext>, IUserOperationClaim
    {
    }
}
