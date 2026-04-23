using CLS.Module.Learner.Application.Interfaces;
using CLS.Module.Learner.Infrastructure.Persistence;
using CLS.Module.Learner.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CLS.Module.Learner.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddLearnerInfrastructure(this IServiceCollection services, string connectionString)
    {
        // For MVP & demonstration, using In-Memory Database before mapping actual SQL Server
        services.AddDbContext<LearnerDbContext>(options =>
            // options.UseSqlServer(connectionString)
            options.UseInMemoryDatabase("LearnerDb")
        );
        
        services.AddScoped<ILearnerRepository, LearnerRepository>();

        return services;
    }
}
