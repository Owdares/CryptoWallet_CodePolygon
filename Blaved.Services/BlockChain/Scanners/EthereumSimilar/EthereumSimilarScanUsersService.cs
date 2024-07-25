using Blaved.Interfaces;
using Blaved.Interfaces.Services.BlockChain;
using Blaved.Objects.Models;
using Blaved.Objects.Models.Configurations;
using Bleved.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nethereum.Web3;
using Newtonsoft.Json;
using Serilog.Context;

namespace Blaved.Services.BlockChain.Scanners.EthereumSimilar
{
    public class EthereumSimilarScanUsersService : EthereumSimilarBaseScanner, IEthereumSimilarScanUsersService
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly IBlockChainTransferFacade _blockChainTransferFacade;
        public readonly IInfoService _infoService;
        public readonly AppConfig _appConfig;
        private readonly ILogger<EthereumSimilarScanUsersService> _logger;
        public EthereumSimilarScanUsersService(IUnitOfWork unitOfWork, IBlockChainTransferFacade blockChainTransferFacade,
            IInfoService infoService, IOptions<AppConfig> appConfig, ILogger<EthereumSimilarScanUsersService> logger)
        {
            _logger = logger;
            _appConfig = appConfig.Value;
            _infoService = infoService;
            _unitOfWork = unitOfWork;
            _blockChainTransferFacade = blockChainTransferFacade;
        }
        public async Task<List<TransactionDTO>> ScanUsersDeposit(HashSet<string> addresses, HashSet<string> transfersHash, string Asset, string network, bool isToken)
        {
            LogContext.PushProperty("IsToken", isToken);

            _logger.LogInformation("Blockchain scanning started");

            var web3 = new Web3(_appConfig.BlockChainConfiguration.NetworkNodesUrl[network]);
            var lastBlock = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
            var infoForBlockChaine = await _unitOfWork.InfoForBlockChainRepository.GetInfoForBlockChaine(Asset, network);
            var infoForDeposit = await _infoService.GetInfoForDeposit(Asset, network);
            var coinDecimal = _appConfig.AssetConfiguration.CoinDecimalByNetwork[network][Asset];
            var contract = _appConfig.AssetConfiguration.CoinContractByNetwork[network].GetValueOrDefault(Asset);

            List<TransactionDTO> transactions = await ScanTransaction(web3, addresses, transfersHash, infoForBlockChaine?.LastScanBlock ?? lastBlock.Value, lastBlock.Value, isToken, coinDecimal, infoForDeposit.MinAmount, contract);
            
            await _unitOfWork.InfoForBlockChainRepository.UpdateLastScanBlock(Asset, network, (long)lastBlock.Value);
            await _unitOfWork.SaveChanges();

            _logger.LogInformation("Blockchain scan completed");
            _logger.LogDebug("Blockchain scan completed Detailed - {@Response}", JsonConvert.SerializeObject(transactions, Formatting.Indented));

            return transactions;
        }
    }
}
