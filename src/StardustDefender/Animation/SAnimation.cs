using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustDefender.Enums;

using System;
using System.Collections.Generic;

namespace StardustDefender.Animation
{
    internal sealed class SAnimation
    {
        internal Texture2D Texture => this.texture;
        internal Rectangle TextureRectangle => this.textureRectangle;

        internal float SpriteScale
        {
            get
            {
                float width = TextureRectangle.Width;
                float height = TextureRectangle.Height;

                return width == height ? width : (width + height) / 2;
            }
        }

        internal AnimationMode Mode => this.mode;

        private Texture2D texture;
        private Rectangle textureRectangle;

        private AnimationMode mode;

        private float animationDelay = 1f;
        private float animationCurrentDelay = 0f;

        private readonly List<Rectangle> animationFrames = new();
        private int animationCurrentFrame = 0;

        internal event AnimationFinished OnAnimationFinished;
        internal delegate void AnimationFinished();

        internal void Initialize()
        {
            if (animationFrames.Count > 0)
            {
                this.textureRectangle = this.animationFrames[0];
            }
        }
        internal void Reset()
        {
            this.mode = AnimationMode.Disable;

            this.animationDelay = 1f;
            this.animationCurrentDelay = 0f;
            
            this.animationCurrentFrame = 0;
        }
        internal void Update()
        {
            if (this.mode == AnimationMode.Disable)
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
                    if (this.mode == AnimationMode.Once)
                    {
                        this.mode = AnimationMode.Disable;
                        OnAnimationFinished?.Invoke();
                        return;
                    }
                }
            }

            this.textureRectangle = this.animationFrames[this.animationCurrentFrame];
        }
        internal void Clear()
        {
            this.animationFrames.Clear();
        }

        internal void SetTexture(Texture2D texture)
        {
            this.texture = texture;
        }
        internal void SetMode(AnimationMode mode)
        {
            this.mode = mode;
        }
        internal void SetDuration(float delay)
        {
            this.animationDelay = delay;
        }
        internal void SetCurrentFrame(int frame)
        {
            animationCurrentFrame = Math.Clamp(frame, 0, animationFrames.Count - 1);
        }

        internal void AddSprite(Rectangle rect)
        {
            this.animationFrames.Add(rect);
        }

        internal bool IsEmpty()
        {
            if (Texture == null || TextureRectangle.IsEmpty)
                return true;

            return false;
        }
    }
}
