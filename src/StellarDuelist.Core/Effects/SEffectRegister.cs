using StellarDuelist.Core.Animation;

namespace StellarDuelist.Core.Effects
{
    /// <summary>
    /// Base model for registering visual effects.
    /// </summary>
    /// <remarks>
    /// <see cref="SEffectRegister"/> is used to define new effects automatically. These effects are loaded during the game's initialization and are subsequently used to invoke effects on the screen.
    /// </remarks>
    public abstract class SEffectRegister
    {
        /// <summary>
        /// The animation used by instances of the effect.
        /// </summary>
        public SAnimation Animation { get; private set; } = new();

        /// <summary>
        /// Initializes the process of constructing the effect's registration model.
        /// </summary>
        internal void Initialize()
        {
            OnBuild();
        }

        /// <summary>
        /// Builds all the necessary components for the execution of the current effect.
        /// </summary>
        protected virtual void OnBuild() { return; }
    }
}
