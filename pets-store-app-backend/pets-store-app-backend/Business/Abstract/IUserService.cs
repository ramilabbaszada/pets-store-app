using Core.Entities.Concrete;
using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IUserService
    {
        Task<IDataResult<List<OperationClaim>>> GetClaims(User user);
        Task<IResult> Add(User user);
        Task<IDataResult<User>> GetByMail(string email);
        Task<IResult> Update(User user);
        Task<IDataResult<User>> GetByRefreshToken(string token);
        Task<IDataResult<User>> GetById(int id);
    }
}
