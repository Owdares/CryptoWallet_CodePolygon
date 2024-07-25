using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blaved.Objects.Models.Configurations
{
    public class AppConfig
    {
        public AssetConfiguration AssetConfiguration { get; set; }
        public BinanceConfiguration BinanceConfiguration { get; set; }
        public BlockChainConfiguration BlockChainConfiguration { get; set; }
        public BotConfiguration BotConfiguration { get; set; }
        public FunctionConfiguration FunctionConfiguration { get; set; }
        public PathConfiguration PathConfiguration { get; set; }
        public UrlConfiguration UrlConfiguration { get; set; }
        public CryptographyConfiguration CryptographyConfiguration { get; set; }
    }
}
