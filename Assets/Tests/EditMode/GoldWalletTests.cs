using System;
using MedievalMayhem.Economy;
using NUnit.Framework;

public class GoldWalletTests
{
    [Test]
    public void ExactCostPurchaseSpendsLastGoldAndNextPurchaseIsRejected()
    {
        var wallet = new GoldWallet(25);
        Assert.That(wallet.TrySpend(25), Is.True);
        Assert.That(wallet.Balance, Is.Zero);
        Assert.That(wallet.TrySpend(25), Is.False);
        Assert.That(wallet.Balance, Is.Zero);
    }

    [Test]
    public void InsufficientGoldIsPreserved()
    {
        var wallet = new GoldWallet(24);
        Assert.That(wallet.TrySpend(25), Is.False);
        Assert.That(wallet.Balance, Is.EqualTo(24));
    }

    [Test]
    public void StartingBudgetAllowsFourSoldiersAndNoOverdraft()
    {
        var wallet = new GoldWallet(100);
        for (int purchase = 0; purchase < 4; purchase++)
            Assert.That(wallet.TrySpend(25), Is.True);
        Assert.That(wallet.TrySpend(25), Is.False);
        Assert.That(wallet.Balance, Is.Zero);
    }

    [TestCase(0)]
    [TestCase(-25)]
    [TestCase(int.MinValue)]
    public void InvalidCostsCannotCreateFreeUnitsOrIncreaseBalance(int cost)
    {
        var wallet = new GoldWallet(100);
        Assert.That(wallet.TrySpend(cost), Is.False);
        Assert.That(wallet.Balance, Is.EqualTo(100));
    }

    [Test]
    public void TeamWalletsAreIndependent()
    {
        var player1 = new GoldWallet(100);
        var player2 = new GoldWallet(100);
        player1.TrySpend(25);
        Assert.That(player1.Balance, Is.EqualTo(75));
        Assert.That(player2.Balance, Is.EqualTo(100));
    }

    [Test]
    public void NegativeStartingGoldIsRejected()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new GoldWallet(-1));
    }

    [Test]
    public void AffordabilityCheckDoesNotSpendGold()
    {
        var wallet = new GoldWallet(25);
        Assert.That(wallet.CanAfford(25), Is.True);
        Assert.That(wallet.Balance, Is.EqualTo(25));
    }
}
