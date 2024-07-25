﻿// <auto-generated />
using System;
using Blaved.Data.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Blaved.Data.Migrations
{
    [DbContext(typeof(MyDbContext))]
    partial class MyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Blaved.Models.BalanceModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<decimal>("BalanceADA")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<decimal>("BalanceAPE")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<decimal>("BalanceBNB")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<decimal>("BalanceBUSD")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<decimal>("BalanceDOGE")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<decimal>("BalanceETH")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<decimal>("BalanceLINK")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<decimal>("BalanceMATIC")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<decimal>("BalanceSAND")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<decimal>("BalanceSHIB")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<decimal>("BalanceUSDC")
                        .HasColumnType("decimal(24, 8)");

                    b.Property<decimal>("BalanceUSDT")
                        .HasColumnType("decimal(24, 8)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Balances");
                });

            modelBuilder.Entity("Blaved.Models.BlavedPayIDTransferModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<string>("Asset")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("ToUserId")
                        .HasColumnType("bigint");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("BlavedPayIDTransfers");
                });

            modelBuilder.Entity("Blaved.Models.BlockChainWalletModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("AddressBSC")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AddressETH")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AddressMATIC")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("PrivatKeyBSC")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PrivatKeyETH")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PrivatKeyMATIC")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("BlockChainWallets");
                });

            modelBuilder.Entity("Blaved.Models.BonusBalanceModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<decimal>("BonusBalanceADA")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<decimal>("BonusBalanceAPE")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<decimal>("BonusBalanceBNB")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<decimal>("BonusBalanceBUSD")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<decimal>("BonusBalanceDOGE")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<decimal>("BonusBalanceETH")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<decimal>("BonusBalanceLINK")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<decimal>("BonusBalanceMATIC")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<decimal>("BonusBalanceSAND")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<decimal>("BonusBalanceSHIB")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<decimal>("BonusBalanceUSDC")
                        .HasColumnType("decimal(24, 8)");

                    b.Property<decimal>("BonusBalanceUSDT")
                        .HasColumnType("decimal(24, 8)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("BonusBalances");
                });

            modelBuilder.Entity("Blaved.Models.CheckActivatedModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("CheckId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CheckId");

                    b.HasIndex("UserId");

                    b.ToTable("CheckActivateds");
                });

            modelBuilder.Entity("Blaved.Models.CheckModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<string>("Asset")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Checks");
                });

            modelBuilder.Entity("Blaved.Models.DepositModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("AddressFrom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AddressTo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<string>("Asset")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Fee")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<string>("Hash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsInside")
                        .HasColumnType("bit");

                    b.Property<string>("Network")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Deposits");
                });

            modelBuilder.Entity("Blaved.Models.HotTransferModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<string>("Asset")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Fee")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<string>("FromAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Hash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Network")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Status")
                        .HasColumnType("bigint");

                    b.Property<string>("ToAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("HotTransfers");
                });

            modelBuilder.Entity("Blaved.Models.Info.InfoForBlockChainModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Asset")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("LastScanBlock")
                        .HasColumnType("bigint");

                    b.Property<string>("Network")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.ToTable("InfoForBlockChaines");
                });

            modelBuilder.Entity("Blaved.Models.MessagesBlavedPayIDModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<string>("Asset")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("ToUserId")
                        .HasColumnType("bigint");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("MessagesBlavedPayIDs");
                });

            modelBuilder.Entity("Blaved.Models.MessagesCheckModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<string>("Asset")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("MessagesChecks");
                });

            modelBuilder.Entity("Blaved.Models.MessagesExchangeModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("FromAsset")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ToAsset")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("MessagesExchanges");
                });

            modelBuilder.Entity("Blaved.Models.MessagesWithdrawModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<string>("Asset")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Network")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("MessagesWithdraws");
                });

            modelBuilder.Entity("Blaved.Models.UserModel", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<bool>("AcceptedTermsOfUse")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("EnabledNotificationsBlavedPay")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsBanned")
                        .HasColumnType("bit");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MessageId")
                        .HasColumnType("int");

                    b.Property<int>("RateReferralExchange")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("WhereMenu")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("WhoseReferral")
                        .HasColumnType("bigint");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Blaved.Models.WithdrawModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("AddressFrom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AddressTo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<string>("Asset")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("ChargeToCapital")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Fee")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<string>("Hash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Network")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrderId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Withdraws");
                });

            modelBuilder.Entity("Blaved.Models.WithdrawOrderModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("AddressTo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<string>("Asset")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("ChargeToCapital")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Fee")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<string>("IdOrder")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Network")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("WithdrawOrders");
                });

            modelBuilder.Entity("Blaved.Objects.Models.ExchangeModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<decimal>("ChargeToCapital")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<decimal>("ChargeToReferral")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ExchangeId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ExchangeMethod")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Fee")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<decimal>("FromAmount")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<string>("FromAsset")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("HiddenFee")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<decimal>("ToAmount")
                        .HasColumnType("decimal(36, 8)");

                    b.Property<string>("ToAsset")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Exchanges");
                });

            modelBuilder.Entity("Blaved.Models.BalanceModel", b =>
                {
                    b.HasOne("Blaved.Models.UserModel", "User")
                        .WithOne("BalanceModel")
                        .HasForeignKey("Blaved.Models.BalanceModel", "UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Blaved.Models.BlavedPayIDTransferModel", b =>
                {
                    b.HasOne("Blaved.Models.UserModel", "User")
                        .WithMany("BlavedPayIDTransferModels")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Blaved.Models.BlockChainWalletModel", b =>
                {
                    b.HasOne("Blaved.Models.UserModel", "User")
                        .WithOne("BlockChainWalletModel")
                        .HasForeignKey("Blaved.Models.BlockChainWalletModel", "UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Blaved.Models.BonusBalanceModel", b =>
                {
                    b.HasOne("Blaved.Models.UserModel", "User")
                        .WithOne("BonusBalanceModel")
                        .HasForeignKey("Blaved.Models.BonusBalanceModel", "UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Blaved.Models.CheckActivatedModel", b =>
                {
                    b.HasOne("Blaved.Models.CheckModel", "Check")
                        .WithMany("CheckActivatedModels")
                        .HasForeignKey("CheckId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Blaved.Models.UserModel", "User")
                        .WithMany("CheckActivatedModels")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Check");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Blaved.Models.CheckModel", b =>
                {
                    b.HasOne("Blaved.Models.UserModel", "User")
                        .WithMany("CheckModels")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Blaved.Models.DepositModel", b =>
                {
                    b.HasOne("Blaved.Models.UserModel", "User")
                        .WithMany("DepositModels")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Blaved.Models.MessagesBlavedPayIDModel", b =>
                {
                    b.HasOne("Blaved.Models.UserModel", "User")
                        .WithOne("MessagesBlavedPayIDModel")
                        .HasForeignKey("Blaved.Models.MessagesBlavedPayIDModel", "UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Blaved.Models.MessagesCheckModel", b =>
                {
                    b.HasOne("Blaved.Models.UserModel", "User")
                        .WithOne("MessagesCheckModel")
                        .HasForeignKey("Blaved.Models.MessagesCheckModel", "UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Blaved.Models.MessagesExchangeModel", b =>
                {
                    b.HasOne("Blaved.Models.UserModel", "User")
                        .WithOne("MessagesExchangeModel")
                        .HasForeignKey("Blaved.Models.MessagesExchangeModel", "UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Blaved.Models.MessagesWithdrawModel", b =>
                {
                    b.HasOne("Blaved.Models.UserModel", "User")
                        .WithOne("MessagesWithdrawModel")
                        .HasForeignKey("Blaved.Models.MessagesWithdrawModel", "UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Blaved.Models.WithdrawModel", b =>
                {
                    b.HasOne("Blaved.Models.UserModel", "User")
                        .WithMany("WithdrawModels")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Blaved.Models.WithdrawOrderModel", b =>
                {
                    b.HasOne("Blaved.Models.UserModel", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Blaved.Objects.Models.ExchangeModel", b =>
                {
                    b.HasOne("Blaved.Models.UserModel", "User")
                        .WithMany("ExchangeModels")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Blaved.Models.CheckModel", b =>
                {
                    b.Navigation("CheckActivatedModels");
                });

            modelBuilder.Entity("Blaved.Models.UserModel", b =>
                {
                    b.Navigation("BalanceModel")
                        .IsRequired();

                    b.Navigation("BlavedPayIDTransferModels");

                    b.Navigation("BlockChainWalletModel")
                        .IsRequired();

                    b.Navigation("BonusBalanceModel")
                        .IsRequired();

                    b.Navigation("CheckActivatedModels");

                    b.Navigation("CheckModels");

                    b.Navigation("DepositModels");

                    b.Navigation("ExchangeModels");

                    b.Navigation("MessagesBlavedPayIDModel")
                        .IsRequired();

                    b.Navigation("MessagesCheckModel")
                        .IsRequired();

                    b.Navigation("MessagesExchangeModel")
                        .IsRequired();

                    b.Navigation("MessagesWithdrawModel")
                        .IsRequired();

                    b.Navigation("WithdrawModels");
                });
#pragma warning restore 612, 618
        }
    }
}
