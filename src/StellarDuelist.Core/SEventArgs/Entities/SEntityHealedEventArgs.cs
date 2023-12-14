using System;

namespace StellarDuelist.Core.SEventArgs.Entities
{
    public sealed class SEntityHealedEventArgs : EventArgs
    {
        public int HealingValue { get; }

        public SEntityHealedEventArgs(int healingValue)
        {
            this.HealingValue = healingValue;
        }
    }
}
