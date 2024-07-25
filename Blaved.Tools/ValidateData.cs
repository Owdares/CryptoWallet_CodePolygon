﻿using Blaved.Models.Info;
using Blaved.Models;

namespace Blaved.Utility
{
    public class ValidateData
    {
        public static bool ValidateBlavedPayIDTransferCreate_Balance(UserModel user, InfoForBlavedPayIDModel infoForBlavedPayID)
        {
            if (user.BalanceModel.GetBalance(infoForBlavedPayID.Asset) >= infoForBlavedPayID.MinAmount)
            {
                return true;
            }
            else { return false; }
        }
        public static bool ValidateBlavedPayIDTransferCreate_Amount(UserModel user, decimal amount, InfoForBlavedPayIDModel infoForBlavedPayID)
        {
            if (user.BalanceModel.GetBalance(infoForBlavedPayID.Asset) >= amount && amount >= infoForBlavedPayID.MinAmount)
            {
                return true;
            }
            else { return false; }
        }
        public static bool ValidateCheckCreate_Balance(UserModel user, InfoForCheckModel infoForCheck)
        {
            if (user.BalanceModel.GetBalance(infoForCheck.Asset) >= infoForCheck.MinAmount)
            {
                return true;
            }
            else { return false; }
        }
        public static bool ValidateCheckCreate_Amount(UserModel user, decimal amount, InfoForCheckModel infoForCheck)
        {
            if (user.BalanceModel.GetBalance(infoForCheck.Asset) >= amount && amount >= infoForCheck.MinAmount)
            {
                return true;
            }
            else { return false; }
        }
        public static bool ValidateCheckCreate_Count(UserModel user, int count, InfoForCheckModel infoForCheck)
        {
            decimal needBalance = (count * user.MessagesCheckModel.Amount).AmountRound();

            if (user.BalanceModel.GetBalance(infoForCheck.Asset) >= needBalance)
            {
                return true;
            }
            else { return false; }
        }
        public static bool ValidateCheckUpdate_Password(string password)
        {
            if (password.Length <= 100)
            {
                return true;
            }
            else { return false; }
        }
        public static bool ValidateCheckActivated_Password(CheckModel checkModel, string password)
        {
            if (checkModel.Password == password)
            {
                return true;
            }
            else { return false; }
        }
        public static bool ValidateExchangeCreate_Balance(UserModel user, InfoForExchangeModel infoForExchangeModel)
        {
            decimal userBalance = user.BalanceModel.GetBalance(infoForExchangeModel.FromAsset);
            decimal needBalance = (infoForExchangeModel.MinAmount + infoForExchangeModel.ExchangeInternalFee).AmountRound();

            if (userBalance >= needBalance)
            {
                return true;
            }
            else { return false; }
        }
        public static bool ValidateExchangeCreate_Amount(UserModel user, decimal amount, InfoForExchangeModel infoForExchangeModel)
        {
            decimal userBalance = user.BalanceModel.GetBalance(infoForExchangeModel.FromAsset);
            decimal needBalance = (amount + infoForExchangeModel.ExchangeInternalFee).AmountRound();

            if (userBalance >= needBalance && amount >= infoForExchangeModel.MinAmount)
            {
                return true;
            }
            else { return false; }
        }
        public static bool ValidateWithdrawCreate_Balance(UserModel user, InfoForWithdrawModel infoForWithdrawModel)
        {
            decimal userBalance = user.BalanceModel.GetBalance(infoForWithdrawModel.Asset);
            decimal needBalance = (infoForWithdrawModel.MinimalSum + infoForWithdrawModel.WithdrawCombineFee).AmountRound();

            if (userBalance >= needBalance)
            {
                return true;
            }
            else { return false; }
        }
        public static bool ValidateWithdrawCreate_Amount(UserModel user, decimal amount, InfoForWithdrawModel infoForWithdrawModel)
        {
            decimal userBalance = user.BalanceModel.GetBalance(infoForWithdrawModel.Asset);
            decimal needBalance = (amount + infoForWithdrawModel.WithdrawCombineFee).AmountRound();

            if (userBalance >= needBalance && amount >= infoForWithdrawModel.MinimalSum)
            {
                return true;
            }
            else { return false; }
        }
        public static bool ValidateWithdrawCreate_Address(string address, string network)
        {
            if (network == "BSC" || network == "MATIC" || network == "ETH")
            {
                if (address.StartsWith("0x") && address.Length == 42)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}