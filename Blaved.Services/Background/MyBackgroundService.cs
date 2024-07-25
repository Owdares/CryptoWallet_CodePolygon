using Blaved.Interfaces.Services.Binance;
using Blaved.Interfaces.Services.Bot;
using Blaved.Objects.Models.Configurations;
using Blaved.Utility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace Bleved.Service.Backgroung
{
    public class MyBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<MyBackgroundService> _logger;
        private readonly AppConfig _appConfig;
        public MyBackgroundService(IServiceScopeFactory serviceScopeFactory, ILogger<MyBackgroundService> logger,
            IOptions<AppConfig> appConfig)
        {
            _appConfig = appConfig.Value;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //try
            //{
            //    Task[] tasks =
            //    [
            //        Task.Run(async () =>
            //        {
            //            while (!stoppingToken.IsCancellationRequested)
            //            {
            //                try
            //                {
            //                    using (var scope = _serviceScopeFactory.CreateScope())
            //                    {
            //                        var binanceService = scope.ServiceProvider.GetRequiredService<IBinanceService>();

            //                        var convertInfo = await binanceService.GetCoinConvertInfo();
            //                        var coinInfo = await binanceService.GetCoinInfo();
            //                        var coinPriceInfo = await binanceService.GetCoinPriceUSD();

            //                        if (!coinInfo.Status
            //                        || !coinPriceInfo.Status
            //                        || !convertInfo.Status
            //                        || coinInfo.Data == null
            //                        || coinPriceInfo.Data == null
            //                        || convertInfo.Data == null)
            //                        {
            //                            throw new Exception();
            //                        }
            //                        await JsonFileManager.PutToJsonAsync(_appConfig.PathConfiguration.InfoForWithdraw, coinInfo.Data);
            //                        await JsonFileManager.PutToJsonAsync(_appConfig.PathConfiguration.InfoForConvert, convertInfo.Data);
            //                        await JsonFileManager.PutToJsonAsync(_appConfig.PathConfiguration.InfoForPriceCoin, coinPriceInfo.Data);

            //                    }
            //                    await Task.Delay(1000000, stoppingToken);
            //                }
            //                catch (Exception ex)
            //                {
            //                    _logger.LogError(ex, "Task Exeption");
            //                    await Task.Delay(1000000, stoppingToken);
            //                }
            //            }
            //        }),
            //        Task.Run(async () =>
            //        {
            //            while (!stoppingToken.IsCancellationRequested)
            //            {
            //                try
            //                {
            //                    using (var scope = _serviceScopeFactory.CreateScope())
            //                    {
            //                        var withdrawService = scope.ServiceProvider.GetRequiredService<IWalletService>();

            //                        await withdrawService.WithdrawValidate();
            //                    }
            //                    await Task.Delay(20000, stoppingToken);

            //                }
            //                catch (Exception ex)
            //                {
            //                    _logger.LogError(ex, "Task Exeption");
            //                    await Task.Delay(100000, stoppingToken);
            //                }
            //            }
            //        }),
            //        Task.Run(async () =>
            //        {
            //            while (!stoppingToken.IsCancellationRequested)
            //            {
            //                try
            //                {
            //                    using (var scope = _serviceScopeFactory.CreateScope())
            //                    {
            //                        var bot = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

            //                        var botInfo = await bot.GetWebhookInfoAsync();

            //                        _logger.LogInformation("WeebHook info: {@Result}", new { botInfo });
            //                    }
            //                    await Task.Delay(20000, stoppingToken);

            //                }
            //                catch (Exception ex)
            //                {
            //                    _logger.LogError(ex, "Task Exeption");
            //                    await Task.Delay(100000, stoppingToken);
            //                }
            //            }
            //        }),
            //        Task.Run(async () =>
            //        {
            //            while (!stoppingToken.IsCancellationRequested)
            //            {
            //                try
            //                {
            //                    using (var scope = _serviceScopeFactory.CreateScope())
            //                    {
            //                        var depositeService = scope.ServiceProvider.GetRequiredService<IWalletService>();

            //                        await depositeService.DepositScanTransaction();
            //                    }
            //                    await Task.Delay(10000, stoppingToken);

            //                }
            //                catch (Exception ex)
            //                {
            //                    _logger.LogError(ex, "Task Exeption");
            //                    await Task.Delay(100000, stoppingToken);
            //                }
            //            }
            //        }),
            //    ];
            //    await Task.WhenAll(tasks);
            //    var exceptions = tasks.Where(t => t.Exception != null).SelectMany(t => t.Exception!.InnerExceptions);
            //    if (exceptions.Any())
            //    {
            //        foreach (var exception in exceptions)
            //        {
            //            _logger.LogError(exception, "Task Exeption");
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "MyBackgroundService have error");
            //}
        }
    }
}
