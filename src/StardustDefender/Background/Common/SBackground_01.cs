using StardustDefender.Engine;

namespace StardustDefender.Background.Common
{
    internal sealed class SBackground_01 : SBackground
    {
        protected override void OnProcess()
        {
            SetTexture(STextures.GetTexture("BACKGROUND_01"));
            AddLayer(STextures.GetSprite(SScreen.Width / 2, SScreen.Height / 2, 0, 0), 0f);
            AddLayer(STextures.GetSprite(SScreen.Width / 2, SScreen.Height / 2, 0, 1), 0.2f);
            AddLayer(STextures.GetSprite(SScreen.Width / 2, SScreen.Height / 2, 0, 2), 0.4f);
        }
    }
}
