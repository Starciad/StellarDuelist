using StardustDefender.Core.Components;
using StardustDefender.Core.Effects;

namespace StardustDefender.Game.Effects
{
    internal sealed class ImpactEffect : SEffectRegister
    {
        protected override void OnBuild()
        {
            this.Animation.SetTexture(STextures.GetTexture("EFFECTS_Impact"));
            this.Animation.AddSprite(STextures.GetSprite(64, 0, 0));
            this.Animation.AddSprite(STextures.GetSprite(64, 1, 0));
            this.Animation.AddSprite(STextures.GetSprite(64, 2, 0));
            this.Animation.AddSprite(STextures.GetSprite(64, 3, 0));
            this.Animation.AddSprite(STextures.GetSprite(64, 4, 0));
            this.Animation.AddSprite(STextures.GetSprite(64, 5, 0));
            this.Animation.SetDuration(0.2f);
        }
    }
}
