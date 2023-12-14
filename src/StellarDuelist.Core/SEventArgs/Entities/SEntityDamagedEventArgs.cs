using System;

namespace StellarDuelist.Core.SEventArgs.Entities
{
    public sealed class SEntityDamagedEventArgs : EventArgs
    {
        public int DamageAmount { get; }

        public SEntityDamagedEventArgs(int damageAmount)
        {
            this.DamageAmount = damageAmount;
        }
    }
}
