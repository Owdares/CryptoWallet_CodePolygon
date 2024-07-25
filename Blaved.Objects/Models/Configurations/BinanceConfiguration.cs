namespace Blaved.Objects.Models.Configurations
{
    public class BinanceConfiguration
    {
        public string BinanceAPIKey { get; init; } = default!;
        public string BinanceSecretKey { get; init; } = default!;

        public Dictionary<string, string> DepositeAddress { get; init; } = default!;
    }
}
