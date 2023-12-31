﻿using StellarDuelist.Core.Effects;
using StellarDuelist.Core.Engine;

namespace StellarDuelist.Game.Effects
{
    internal sealed class ExplosionEffect : SEffectRegister
    {
        protected override void OnBuild()
        {
            // Animation
            this.Animation.SetTexture(STextures.GetTexture("EFFECTS_Explosion"));
            this.Animation.AddFrame(STextures.GetSprite(64, 0, 0));
            this.Animation.AddFrame(STextures.GetSprite(64, 1, 0));
            this.Animation.AddFrame(STextures.GetSprite(64, 2, 0));
            this.Animation.AddFrame(STextures.GetSprite(64, 3, 0));
            this.Animation.AddFrame(STextures.GetSprite(64, 4, 0));
            this.Animation.AddFrame(STextures.GetSprite(64, 5, 0));
            this.Animation.AddFrame(STextures.GetSprite(64, 6, 0));
            this.Animation.AddFrame(STextures.GetSprite(64, 7, 0));
            this.Animation.AddFrame(STextures.GetSprite(64, 8, 0));
            this.Animation.AddFrame(STextures.GetSprite(64, 9, 0));
            this.Animation.SetDuration(0.2f);
        }
    }
}
