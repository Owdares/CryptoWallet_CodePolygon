namespace Blaved.Models.Info
{
    public class InfoForWithdrawModel
    {
        public string Asset { get; set; }
        public string Network { get; set; }
        public decimal MinimalSum { get; set; }
        public decimal MaximalSum { get; set; }

        public decimal WithdrawFee { get; set; }
        public decimal WithdrawInternalFee { get; set; }
        public decimal WithdrawCombineFee { get; set; }
    }
}
