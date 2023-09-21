using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustDefender.Enums;

namespace StardustDefender.Animation
{
    internal sealed class SAnimation
    {
        internal Texture2D Texture => texture;
        internal Rectangle TextureRectangle => textureRectangle;

        internal float SpriteScale
        {
            get
            {
                float width = TextureRectangle.Width;
                float height = TextureRectangle.Height;

                if (width == height)
                {
                    return width;
                }

                return (width + height) / 2;
            }
        }

        internal AnimationMode Mode => mode;

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
            textureRectangle = animationFrames[0];
        }
        internal void Reset()
        {
            mode = AnimationMode.Disable;

            animationDelay = 1f;
            animationCurrentDelay = 0f;

            animationFrames.Clear();
            animationCurrentFrame = 0;
        }
        internal void Update()
        {
            if (mode == AnimationMode.Disable)
                return;

            if (animationCurrentDelay < animationDelay)
            {
                animationCurrentDelay += 0.1f;
            }
            else
            {
                animationCurrentDelay = 0;

                if (animationCurrentFrame < animationFrames.Count - 1)
                {
                    animationCurrentFrame++;
                }
                else
                {
                    animationCurrentFrame = 0;
                    if (mode == AnimationMode.Once)
                    {
                        mode = AnimationMode.Disable;
                        OnAnimationFinished?.Invoke();
                        return;
                    }
                }
            }

            textureRectangle = animationFrames[animationCurrentFrame];
        }

        internal void SetTexture(Texture2D texture)
        {
            this.texture = texture;
        }
        internal void AddSprite(Rectangle rect)
        {
            animationFrames.Add(rect);
        }

        internal void SetMode(AnimationMode mode)
        {
            this.mode = mode;
        }
        internal void SetDuration(float delay)
        {
            animationDelay = delay;
        }
    }
}
