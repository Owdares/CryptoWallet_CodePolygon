using Binance.Net;
using Blaved.Data.DataBase;
using Blaved.Objects.Models.Configurations;
using CryptoExchange.Net.Authentication;
using Telegram.Bot;
using Bleved.Service.Backgroung;
using Microsoft.EntityFrameworkCore;
using Blaved.Controllers.Bot;
using Blaved.Interfaces.Repository;
using Blaved.Data.Repository;
using Blaved.Interfaces;
using Blaved.Interfaces.Services.Binance;
using BlevedCrypto.Service.Binance;
using Blaved.Interfaces.Services.BlockChain;
using Blaved.Services.BlockChain.Transfers.EthereumSimilar;
using Blaved.Services.BlockChain.Scanners.EthereumSimilar;
using Blaved.Services.BlockChain;
using Blaved.Services.BlockChain.Transfers;
using Blaved.Services.BlockChain.Scanners;
using Blaved.Interfaces.Services.Bot;
using Blaved.Services.Bot;
using Bleved.Services;
using Bleved.Interfaces.Services;
using Bleved.Service;
using Blaved.Interfaces.Services;
using Blaved.Controllers.Bot.RequestHandler;
using Blaved.Interfaces.Views;
using Blaved.Views.Bot;
using Serilog.AspNetCore;
using Serilog;
using Microsoft.Extensions.Logging;
using Nethereum.JsonRpc.Client;
using Blaved.Utility;
using System.Net;
using Microsoft.Identity.Client;

namespace Blaved
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _env = env;
            Configuration = configuration;
        }
        private readonly IWebHostEnvironment _env;
        public IConfiguration Configuration { get; set; }
        public void ConfigureServices(IServiceCollection services)
        {

            if (_env.IsDevelopment())
            {
                Configuration = new ConfigurationBuilder()
                .SetBasePath(_env.ContentRootPath)
                .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.private.Development.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables().Build();
            }
            else
            {
                Configuration = new ConfigurationBuilder()
                    .SetBasePath(_env.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile("appsettings.private.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables().Build();
            }
            services.Configure<AppConfig>(Configuration.GetSection("AppConfig"));

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration.GetSection("AppConfig"))
                .Enrich.FromLogContext()
                .Enrich.With(new RemovePropertiesEnricher())
                .CreateLogger();

            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddSerilog();
            });

            services.AddDbContext<MyDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetSection("AppConfig:ConnectionStrings:DefaultConnection").Get<string>(),
                    options => options.EnableRetryOnFailure());
            }, ServiceLifetime.Scoped);

            services.AddBinance(options =>
            {
                var binanceConf = Configuration.GetSection("AppConfig").Get<AppConfig>()!.BinanceConfiguration;
                options.ApiCredentials = new ApiCredentials(binanceConf.BinanceAPIKey, binanceConf.BinanceSecretKey, ApiCredentialsType.Hmac);
                options.Environment = BinanceEnvironment.Live;


            });
            services.AddHttpClient("telegram_bot_client")
                .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                {
                    var botConfig = Configuration.GetSection("AppConfig").Get<AppConfig>()!.BotConfiguration;
                    TelegramBotClientOptions options = new TelegramBotClientOptions(botConfig.BotToken);

                    return new TelegramBotClient(options, httpClient);
                });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBlavedPayIDRepository, BlavedPayIDRepository>();
            services.AddScoped<ICheckActivatedRepository, CheckActivatedRepository>();
            services.AddScoped<ICheckRepository, CheckRepository>();
            services.AddScoped<IDepositRepository, DepositRepository>();
            services.AddScoped<IExchangeRepository, ExchangeRepository>();
            services.AddScoped<IInfoForBlockChainRepository, InfoForBlockChainRepository>();
            services.AddScoped<ITransferToHotRepository, TransferToHotRepository>();
            services.AddScoped<IWithdrawRepository, WithdrawRepository>();
            services.AddScoped<IMessagesBlavedPayIDRepository, MessagesBlavedPayIDRepository>();
            services.AddScoped<IMessagesCheckRepository, MessagesCheckRepository>();
            services.AddScoped<IMessagesExchangeRepository, MessagesExchangeRepository>();
            services.AddScoped<IMessagesWithdrawRepository, MessagesWithdrawRepository>();
            services.AddScoped<IBalanceRepository, BalanceRepository>();
            services.AddScoped<IBonusBalanceRepository, BonusBalanceRepository>();
            services.AddScoped<IBlockChainWalletRepository, BlockChainWalletRepository>();
            services.AddScoped<IWithdrawOrderRepository, WithdrawOrderRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IBinanceService, BinanceService>();
            services.AddScoped<IBlockChainAccountService, BlockChainAccountService>();
            services.AddScoped<IEthereumSimilarHotTransferService, EthereumSimilarHotTransferService>();
            services.AddScoped<IEthereumSimilarScanUsersService, EthereumSimilarScanUsersService>();

            services.AddScoped<EthScanner>();
            services.AddScoped<BscScanner>();
            services.AddScoped<MaticScanner>();

            services.AddScoped<EthTransfer>();
            services.AddScoped<BscTransfer>();
            services.AddScoped<MaticTransfer>();

            services.AddScoped<IBlockChainScannerFacade, BlockChainScannersFacade>();
            services.AddScoped<IBlockChainTransferFacade, BlockChainTransfersFacade>();

            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<IBlavedPayService, BlavedPayService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICheckService, CheckService>();
            services.AddScoped<IExchangeService, ExchangeService>();

            services.AddScoped<IBlavedPayMenu, BlavedPayMenu>();
            services.AddScoped<ICheckMenu, CheckMenu>();
            services.AddScoped<IExchangeMenu, ExchangeMenu>();
            services.AddScoped<IHelpMenu, HelpMenu>();
            services.AddScoped<IMainMenu, MainMenu>();
            services.AddScoped<ISettingsMenu, SettingsMenu>();
            services.AddScoped<IWalletMenu, WalletMenu>();
            services.AddScoped<IBotMenu, BotMenu>();

            services.AddScoped<IInfoService, InfoService>();
            services.AddScoped<IInterfaceTranslatorService, InterfaceTranslatorService>();

            services.AddScoped<UpdateHandler>();
            services.AddScoped<CallbackRequestHandler>();
            services.AddScoped<MessageRequestHandler>();
            services.AddScoped<InlineRequestHandler>();

            services.AddRazorPages();

            services.AddMvc()
                .AddNewtonsoftJson();
            services.AddControllersWithViews();


            services.AddHostedService<ConfigureWebhook>();
            services.AddHostedService<MyBackgroundService>();
        }
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseStaticFiles();
            app.UseEndpoints(endpoints =>
            {
                var botConfiguration = Configuration.GetSection("AppConfig").Get<AppConfig>()!.BotConfiguration;

                endpoints.MapControllerRoute(
                    name: "bot_webhook",
                    pattern: botConfiguration.BotRoute,
                    defaults: new { controller = "Bot", action = "Post" });

                endpoints.MapControllers();
            });
        }
    }
}
