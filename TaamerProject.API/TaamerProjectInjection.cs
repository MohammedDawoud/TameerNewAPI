//using TaamerProject.Service.Interfaces.UsersF;
//using TaamerProject.Service.Services.UsersF;
using Microsoft.Extensions.DependencyInjection;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Repository.Repositories;
using TaamerProject.Service.Interfaces;
//using Bayanatech.TameerPro.BusinessServices;
using TaamerProject.Service.Services;

namespace TaamerProject.API
{
    public static class TaamerProjectInjection
    {
        public static void InjectServiceandRepository(this IServiceCollection service)
        {
            var services = new ServiceCollection();

            services.AddDbContext<TaamerProjectContext>();
            #region Services
            //services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IAcc_CategoriesService, Acc_CategoriesService>();
            services.AddScoped<IAcc_CategorTypeService, Acc_CategorTypeService>();
            services.AddScoped<IAcc_ClausesService, Acc_ClausesService>();

            #endregion
            #region Repository
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IAcc_CategoriesRepository, Acc_CategoriesRepository>();
            services.AddScoped<IAcc_CategorTypeRepository, Acc_CategorTypeRepository>();
            services.AddScoped<IAcc_ClausesRepository, Acc_ClausesRepository>();

            #endregion
            services.AddHttpContextAccessor();

            var serviceProvider = services.BuildServiceProvider();

            //var usersService = serviceProvider.GetService<IUsersService>();
            var usersRepository = serviceProvider.GetService<IUsersRepository>();

            if (serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
