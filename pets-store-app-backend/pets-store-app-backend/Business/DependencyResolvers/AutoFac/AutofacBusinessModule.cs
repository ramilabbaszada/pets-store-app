using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Caching;
using Core.CrossCuttingConcerns.Caching.Microsoft;
using Core.Utilities.Interceptors;
using Core.Utilities.Security.Jwt;
using DataAccesss.Abstract;
using DataAccesss.Concrete;


namespace Business.DependencyResolvers.AutoFac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthManager>().As<IAuthService>();
            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<EmailManager>().As<IEmailService>();

            builder.RegisterType<EfOperationClaimDal>().As<IOperationClaimDal>();
            builder.RegisterType<EfUserOperationClaimDal>().As<IUserOperationClaim>();
            builder.RegisterType<EfUserDal>().As<IUserDal>();

            builder.RegisterType<MemoryCacheManager>().As<ICacheManager>();

            builder.RegisterType<JwtHelper>().As<ITokenHelper>();

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();

        }
    }
}
