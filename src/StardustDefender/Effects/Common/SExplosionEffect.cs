using StardustDefender.Core;

namespace StardustDefender.Effects.Common
{
    internal sealed class SExplosionEffect : SEffectTemplate
    {
        protected override void OnBuild()
        {
            // Animation
            Animation.SetTexture(STextures.GetTexture("EFFECTS_Explosion"));
            Animation.AddSprite(STextures.GetSprite(64, 0, 0));
            Animation.AddSprite(STextures.GetSprite(64, 1, 0));
            Animation.AddSprite(STextures.GetSprite(64, 2, 0));
            Animation.AddSprite(STextures.GetSprite(64, 3, 0));
            Animation.AddSprite(STextures.GetSprite(64, 4, 0));
            Animation.AddSprite(STextures.GetSprite(64, 5, 0));
            Animation.AddSprite(STextures.GetSprite(64, 6, 0));
            Animation.AddSprite(STextures.GetSprite(64, 7, 0));
            Animation.AddSprite(STextures.GetSprite(64, 8, 0));
            Animation.AddSprite(STextures.GetSprite(64, 9, 0));
            Animation.SetDuration(0.2f);
        }
    }
}
