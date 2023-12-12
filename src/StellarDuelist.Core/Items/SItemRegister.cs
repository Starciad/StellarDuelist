using StellarDuelist.Core.Animation;
using StellarDuelist.Core.Controllers;
using StellarDuelist.Core.Entities.Templates;

namespace StellarDuelist.Core.Items
{
    /// <summary>
    /// Represents a base class for registering game items.
    /// </summary>
    public abstract class SItemRegister
    {
        /// <summary>
        /// Gets the animation associated with the item.
        /// </summary>
        public SAnimation Animation { get; private set; } = new();

        /// <summary>
        /// Gets a value indicating whether the item can spawn based on certain conditions.
        /// </summary>
        internal bool CanSpawn => SpawnCondition(SLevelController.Player);

        // Const values
        /// <summary>
        /// The sprite scale for the item.
        /// </summary>
        public const byte SPRITE_SCALE = 16;

        /// <summary>
        /// Initializes the item and its animation.
        /// </summary>
        internal void Initialize()
        {
            OnInitialize();
            this.Animation.Initialize();
        }

        /// <summary>
        /// Applies the effect of the item.
        /// </summary>
        internal void ApplyEffect()
        {
            OnEffect(SLevelController.Player);
        }

        /// <summary>
        /// Called when initializing the item. Override this method to set up the item's properties.
        /// </summary>
        protected abstract void OnInitialize();

        /// <summary>
        /// Called to apply the item's effect to the player.
        /// </summary>
        /// <param name="player">The player entity that the effect is applied to.</param>
        protected abstract void OnEffect(SPlayerEntity player);

        /// <summary>
        /// Determines whether the item can spawn based on certain conditions. Override this method to implement custom spawn conditions.
        /// </summary>
        /// <param name="player">The player entity.</param>
        /// <returns><c>true</c> if the item can spawn; otherwise, <c>false</c>.</returns>
        protected virtual bool SpawnCondition(SPlayerEntity player) { return true; }
    }
}
