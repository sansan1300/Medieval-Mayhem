using System;

namespace MedievalMayhem.Economy
{
    public class GoldWallet
    {
        public int Balance { get; private set; }

        public GoldWallet(int startingGold)
        {
            if (startingGold < 0) throw new ArgumentOutOfRangeException(nameof(startingGold));
            Balance = startingGold;
        }

        public bool CanAfford(int cost)
        {
            return cost > 0 && Balance >= cost;
        }

        public bool TrySpend(int cost)
        {
            if (!CanAfford(cost)) return false;
            Balance -= cost;
            return true;
        }
    }
}
