using StardustDefender.Core.Animation;

namespace StardustDefender.Core.Effects
{
    public abstract class SEffectRegister
    {
        public SAnimation Animation { get; private set; } = new();

        internal void Initialize()
        {
            OnBuild();
        }
        protected virtual void OnBuild() { return; }
    }
}
