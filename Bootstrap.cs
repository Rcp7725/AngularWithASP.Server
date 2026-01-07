using AngularWithASP.Server.Controllers;
using AngularWithASP.Server.Core;

namespace AngularWithASP.Server
{
    public interface IBootstrap
    {
        Task StartAsync();
    }
    public class Bootstrap: IBootstrap
    {
        private IDataManager dataManager;
        readonly ILogger<Bootstrap> logger;
        readonly IConfiguration configuration;
        readonly IWebHostEnvironment environment;
        readonly IServiceProvider serviceProvider;
        readonly IHttpContextAccessor httpContextAccessor;
        readonly IHostApplicationLifetime hostApplicationLifetime;
        readonly IHostEnvironment hostEnvironment;
        readonly TreeController treeController;

        public Bootstrap()//IDataManager _dataManager)
        {
            //this.dataManager = _dataManager;
            this.logger = serviceProvider.GetRequiredService<ILogger<Bootstrap>>();
            this.configuration = serviceProvider.GetRequiredService<IConfiguration>();
            this.environment = serviceProvider.GetRequiredService<IWebHostEnvironment>();
            this.serviceProvider = serviceProvider;
            this.httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            this.hostApplicationLifetime = serviceProvider.GetRequiredService<IHostApplicationLifetime>();
            this.hostEnvironment = serviceProvider.GetRequiredService<IHostEnvironment>();
            this.treeController = serviceProvider.GetRequiredService<TreeController>();
        }
        public async Task StartAsync()
        {
            this.dataManager = DataManager.Instance;
            //inizializzazioni varie
            await Task.CompletedTask;
        }
    }
}
