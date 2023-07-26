using IBetting.Services.DataSavingService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IBetting.Services.BackgroundServices
{
    public class DataUploaderBackgroudService : BackgroundService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        public DataUploaderBackgroudService(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = serviceScopeFactory.CreateScope())
                    {
                        var dataSavingService = scope.ServiceProvider.GetRequiredService<IDataSavingService>();
                        dataSavingService.Save();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Background service error: " + e.Message);
                }

                await Task.Delay(60000, stoppingToken);
            }
        }
    }
}
