using Microsoft.Extensions.DependencyInjection;

namespace CLS.Module.Learner.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddLearnerApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        
        // Setup mapster mapping configs if necessary
        
        return services;
    }
}
