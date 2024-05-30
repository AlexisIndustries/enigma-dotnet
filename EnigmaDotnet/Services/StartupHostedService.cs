using Microsoft.AspNetCore.Identity;

namespace EnigmaDotnet.Services;

public class StartupHostedService : IHostedService
{
    private IServiceProvider _serviceProvider;
    private RoleManager<IdentityRole> _roleManager;
    
    public StartupHostedService(IServiceProvider services){
        this._serviceProvider = services;
        var scope = _serviceProvider.CreateScope();
        _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!_roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult() && !_roleManager.RoleExistsAsync("Viewer").GetAwaiter().GetResult() && !_roleManager.RoleExistsAsync("Worker").GetAwaiter().GetResult())
        {
            await _roleManager.CreateAsync(new IdentityRole("Admin"));
            await _roleManager.CreateAsync(new IdentityRole("Worker"));
            await _roleManager.CreateAsync(new IdentityRole("Viewer"));
        }

        //return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}