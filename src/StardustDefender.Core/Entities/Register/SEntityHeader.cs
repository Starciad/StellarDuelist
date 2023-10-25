using StardustDefender.Core.Enums;

using System;

namespace StardustDefender.Core.Entities.Register
{
    /// <summary>
    /// Represents a base class for entity headers.
    /// </summary>
    public abstract class SEntityHeader
    {
        /// <summary>
        /// Gets the type of the associated entity.
        /// </summary>
        internal Type EntityType { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the entity can spawn.
        /// </summary>
        internal bool CanSpawn => OnSpawningCondition();

        /// <summary>
        /// Gets or sets the classification of the entity header.
        /// </summary>
        public SEntityClassification Classification { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SEntityHeader"/> class.
        /// </summary>
        public SEntityHeader()
        {
            OnProcess();
        }

        /// <summary>
        /// Builds the entity header with the specified entity type.
        /// </summary>
        /// <param name="entityType">The type of the associated entity.</param>
        internal void Build(Type entityType)
        {
            this.EntityType = entityType;
        }

        /// <summary>
        /// Method called when processing the entity header.
        /// </summary>
        protected abstract void OnProcess();

        /// <summary>
        /// Determines whether the entity can spawn under certain conditions.
        /// </summary>
        /// <returns><c>true</c> if the entity can spawn; otherwise, <c>false</c>.</returns>
        protected virtual bool OnSpawningCondition() { return true; }
    }
}
