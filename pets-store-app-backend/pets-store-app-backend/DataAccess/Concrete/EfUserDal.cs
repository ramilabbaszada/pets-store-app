using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccesss.Abstract;
using DataAccesss.Concrete.Contexts;
using Microsoft.EntityFrameworkCore;


namespace DataAccesss.Concrete
{
    public class EfUserDal : EfEntityRepositoryBase<User, TestProjectContext>, IUserDal
    {
        public async Task< List<OperationClaim>> GetClaims(User user)
        {
            using (var context = new TestProjectContext())
            {
                var result = from operationClaim in context.OperationClaims
                             join userOperationClaim in context.UserOperationClaims
                                 on operationClaim.Id equals userOperationClaim.OperationClaimId
                             where userOperationClaim.UserId == user.Id
                             select new OperationClaim { Id = operationClaim.Id, Name = operationClaim.Name };
                return await result.ToListAsync();
            }
        }
    }
}
