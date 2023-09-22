using StardustDefender.Engine;

namespace StardustDefender.Effects.Common
{
    internal sealed class SImpactEffect : SEffectTemplate
    {
        protected override void OnBuild()
        {
            Animation.SetTexture(STextures.GetTexture("EFFECTS_Impact"));
            Animation.AddSprite(STextures.GetSprite(64, 0, 0));
            Animation.AddSprite(STextures.GetSprite(64, 1, 0));
            Animation.AddSprite(STextures.GetSprite(64, 2, 0));
            Animation.AddSprite(STextures.GetSprite(64, 3, 0));
            Animation.AddSprite(STextures.GetSprite(64, 4, 0));
            Animation.AddSprite(STextures.GetSprite(64, 5, 0));
            Animation.SetDuration(0.2f);
        }
    }
}
