﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using StellarDuelist.Core.Camera;
using StellarDuelist.Core.Controllers;
using StellarDuelist.Core.Engine;
using StellarDuelist.Core.Enums;
using StellarDuelist.Core.Extensions;
using StellarDuelist.Core.GUI;

namespace StellarDuelist.Game.GUI
{
    internal class GUI_Pause : SGUI
    {
        // Textures
        private Texture2D backgroundTexture;
        private Texture2D pausedTexture;

        // Transform
        private Vector2 backgroundTextureOrigin;
        private Vector2 pausedTextureOrigin;

        protected override bool ConditionToBeDrawn()
        {
            return SGameController.State == SGameState.Paused;
        }

        protected override void OnEnable()
        {
            SSongs.Volume = 0.2f;
        }
        protected override void OnDisable()
        {
            SSongs.Volume = 0.5f;
        }

        protected override void OnInitialize()
        {
            this.backgroundTexture = STextures.GetTexture("UI_SolidBackground");
            this.pausedTexture = STextures.GetTexture("TEXTS_Paused");

            this.pausedTextureOrigin = this.pausedTexture.GetOriginPosition();
            this.backgroundTextureOrigin = this.backgroundTexture.GetOriginPosition();
        }
        protected override void OnUpdate()
        {
            if (SInput.Started(Keys.Space) || SInput.Started(Keys.Escape))
            {
                SGameController.SetGameState(SGameState.Running);
                Disable();
            }
        }
        protected override void OnDraw()
        {
            SGraphics.SpriteBatch.Draw(this.backgroundTexture, new Vector2(SCamera.Center.X, SCamera.Center.Y), null, new Color(1, 11, 25, 180), 0f, this.backgroundTextureOrigin, new Vector2(1.5f), SpriteEffects.None, 0f);
            SGraphics.SpriteBatch.Draw(this.pausedTexture, new Vector2(SCamera.Center.X, SCamera.Center.Y), null, Color.White, 0f, this.pausedTextureOrigin, new Vector2(1f), SpriteEffects.None, 0f);
        }
    }
}
