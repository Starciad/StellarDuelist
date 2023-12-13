using StellarDuelist.Core.Enums;
using System;

namespace StellarDuelist.Core.Entities
{
    public abstract class SEntityDefinition
    {
        /// <summary>
        /// Gets the type of the associated entity.
        /// </summary>
        internal Type EntityTargetType { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the entity can spawn.
        /// </summary>
        internal bool CanSpawn => canSpawn.Invoke();

        /// <summary>
        /// Gets or sets the classification of the entity header.
        /// </summary>
        public SEntityClassification Classification => classification;

        protected Func<bool> canSpawn;
        protected SEntityClassification classification;

        internal void Build(Type entityType)
        {
            EntityTargetType = entityType;
            OnBuild();
        }

        protected abstract void OnBuild();
    }
}