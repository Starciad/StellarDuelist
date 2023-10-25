using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustDefender.Core.Enums;

using System;
using System.Collections.Generic;

namespace StardustDefender.Core.Animation
{
    public sealed class SAnimation
    {
        internal Texture2D Texture => this.texture;
        internal Rectangle TextureRectangle => this.textureRectangle;

        internal float SpriteScale
        {
            get
            {
                float width = this.TextureRectangle.Width;
                float height = this.TextureRectangle.Height;

                return width == height ? width : (width + height) / 2;
            }
        }

        internal SAnimationMode Mode => this.mode;

        private Texture2D texture;
        private Rectangle textureRectangle;

        private SAnimationMode mode;

        private float animationDelay = 1f;
        private float animationCurrentDelay = 0f;

        private readonly List<Rectangle> animationFrames = new();
        private int animationCurrentFrame = 0;

        internal event AnimationFinished OnAnimationFinished;
        internal delegate void AnimationFinished();

        internal void Initialize()
        {
            if (this.animationFrames.Count > 0)
            {
                this.textureRectangle = this.animationFrames[0];
            }
        }

        public void Update()
        {
            if (this.mode == SAnimationMode.Disable)
            {
                return;
            }

            if (this.animationCurrentDelay < this.animationDelay)
            {
                this.animationCurrentDelay += 0.1f;
            }
            else
            {
                this.animationCurrentDelay = 0;

                if (this.animationCurrentFrame < this.animationFrames.Count - 1)
                {
                    this.animationCurrentFrame++;
                }
                else
                {
                    this.animationCurrentFrame = 0;
                    if (this.mode == SAnimationMode.Once)
                    {
                        this.mode = SAnimationMode.Disable;
                        OnAnimationFinished?.Invoke();
                        return;
                    }
                }
            }

            this.textureRectangle = this.animationFrames[this.animationCurrentFrame];
        }
        public void Reset()
        {
            this.mode = SAnimationMode.Disable;

            this.animationDelay = 1f;
            this.animationCurrentDelay = 0f;

            this.animationCurrentFrame = 0;
        }
        public void Clear()
        {
            this.animationFrames.Clear();
        }

        public void SetTexture(Texture2D texture)
        {
            this.texture = texture;
        }
        public void SetMode(SAnimationMode mode)
        {
            this.mode = mode;
        }
        public void SetDuration(float delay)
        {
            this.animationDelay = delay;
        }
        public void SetCurrentFrame(int frame)
        {
            this.animationCurrentFrame = Math.Clamp(frame, 0, this.animationFrames.Count - 1);
        }
        public void AddSprite(Rectangle rect)
        {
            this.animationFrames.Add(rect);
        }

        internal bool IsEmpty()
        {
            return this.Texture == null || this.TextureRectangle.IsEmpty;
        }
    }
}
