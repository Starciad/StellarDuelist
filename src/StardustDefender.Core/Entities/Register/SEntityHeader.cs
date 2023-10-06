using StardustDefender.Core.Enums;

using System;

namespace StardustDefender.Core.Entities.Register
{
    public abstract class SEntityHeader
    {
        internal Type EntityType { get; private set; }
        internal bool CanSpawn => OnSpawningCondition();
        public SEntityClassification Classification { get; protected set; }

        public SEntityHeader()
        {
            OnProcess();
        }

        internal void Build(Type entityType)
        {
            this.EntityType = entityType;
        }

        protected abstract void OnProcess();
        protected virtual bool OnSpawningCondition() { return true; }
    }
}
