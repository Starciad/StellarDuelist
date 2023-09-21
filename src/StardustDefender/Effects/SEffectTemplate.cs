using StardustDefender.Animation;
using StardustDefender.Enums;

namespace StardustDefender.Effects
{
    internal abstract class SEffectTemplate
    {
        internal SAnimation Animation { get; private set; } = new();

        internal void Initialize()
        {
            OnBuild();
        }
        protected virtual void OnBuild() { return; }
    }
}
