using Business.Abstract;
using Business.Constants;
using Core.Aspects.Autofac.Exception;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccesss.Abstract;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        IUserDal _userDal;

        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        [LogAspect(typeof(DatabaseLogger))]
        [ExceptionLogAspect(typeof(DatabaseLogger))]
        public async Task<IDataResult<List<OperationClaim>>> GetClaims(User user)
        {
            return  new SuccessDataResult<List<OperationClaim>>(await _userDal.GetClaims(user));
        }

        [LogAspect(typeof(DatabaseLogger))]
        [ExceptionLogAspect(typeof(DatabaseLogger))]
        public async Task<IResult> Add(User user)
        {
            await _userDal.Add(user);
            return new SuccessResult();
        }

        [LogAspect(typeof(DatabaseLogger))]
        [ExceptionLogAspect(typeof(DatabaseLogger))]
        public async Task<IDataResult<User>> GetByMail(string email)
        {
            return new SuccessDataResult<User>( await _userDal.Get(u => u.Email == email));
        }

        [LogAspect(typeof(DatabaseLogger))]
        [ExceptionLogAspect(typeof(DatabaseLogger))]
        public async Task<IResult> Update(User user)
        {
            await _userDal.Update(user);
            return new SuccessResult();
        }

        [LogAspect(typeof(DatabaseLogger))]
        [ExceptionLogAspect(typeof(DatabaseLogger))]
        public async Task<IDataResult<User>> GetByRefreshToken(string token)
        {
            User user = await _userDal.Get(user => user.RefreshToken == token);
            if (user == null)
                return new ErrorDataResult<User>(Messages.RefreshTokenContentError);

            return new SuccessDataResult<User>(user);
        }

        [LogAspect(typeof(DatabaseLogger))]
        [ExceptionLogAspect(typeof(DatabaseLogger))]
        public async Task<IDataResult<User>> GetById(int id)
        {
            User user = await _userDal.Get(user => user.Id == id);
            if (user == null)
                return new ErrorDataResult<User>(Messages.UserDoesNotExists);

            return new SuccessDataResult<User>(user);
        }
    }
    
}
