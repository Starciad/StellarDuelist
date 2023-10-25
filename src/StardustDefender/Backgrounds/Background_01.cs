using StardustDefender.Core.Background;
using StardustDefender.Core.Components;

namespace StardustDefender.Backgrounds
{
    internal sealed class Background_01 : SBackground
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
