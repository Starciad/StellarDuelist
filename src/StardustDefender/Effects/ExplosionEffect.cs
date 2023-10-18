using StardustDefender.Core.Components;
using StardustDefender.Core.Effects;

namespace StardustDefender.Game.Effects
{
    internal sealed class ExplosionEffect : SEffectRegister
    {
        protected override void OnBuild()
        {
            // Animation
            this.Animation.SetTexture(STextures.GetTexture("EFFECTS_Explosion"));
            this.Animation.AddSprite(STextures.GetSprite(64, 0, 0));
            this.Animation.AddSprite(STextures.GetSprite(64, 1, 0));
            this.Animation.AddSprite(STextures.GetSprite(64, 2, 0));
            this.Animation.AddSprite(STextures.GetSprite(64, 3, 0));
            this.Animation.AddSprite(STextures.GetSprite(64, 4, 0));
            this.Animation.AddSprite(STextures.GetSprite(64, 5, 0));
            this.Animation.AddSprite(STextures.GetSprite(64, 6, 0));
            this.Animation.AddSprite(STextures.GetSprite(64, 7, 0));
            this.Animation.AddSprite(STextures.GetSprite(64, 8, 0));
            this.Animation.AddSprite(STextures.GetSprite(64, 9, 0));
            this.Animation.SetDuration(0.2f);
        }
    }
}
