using CarPartsStore.Repositories.Interfaces;
using CarPartsStore.Repositories.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace CarPartsStore.Repositories.Injections
{
    public static class Injector
    {
        public static void AddRepositoryInjections(this IServiceCollection services)
        {
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IContactRepository, ContactRepository>();
            services.AddTransient<IHeadSettingRepository, HeadSettingRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<ITimelineRepository, TimelineRepository>();
            services.AddTransient<ITeamMemberRepository, TeamMemberRepository>();
            services.AddTransient<IAdminRepository, AdminRepository>();
            services.AddTransient<IAdminCandidateRepository, AdminCandidateRepository>();
        }
    }
}
