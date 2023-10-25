using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustDefender.Core.Enums;

using System;
using System.Collections.Generic;

namespace StardustDefender.Core.Animation
{
    /// <summary>
    /// A helper class for managing and coordinating various sprites and textures to create practical and easy-to-control animations.
    /// </summary>
    public sealed class SAnimation
    {
        /// <summary>
        /// Get the texture used by the animation.
        /// </summary>
        public Texture2D Texture => this.texture;

        /// <summary>
        /// Get the coordinates and dimensions of the current frame in the animation.
        /// </summary>
        public Rectangle Frame => this.frame;

        /// <summary>
        /// Get the scale of the sprite based on the current frame of the animation.
        /// </summary>
        public float SpriteScale
        {
            get
            {
                float width = this.Frame.Width;
                float height = this.Frame.Height;

                return width == height ? width : (width + height) / 2;
            }
        }

        /// <summary>
        /// Get the current mode in which the animation is currently performing.
        /// </summary>
        public SAnimationMode Mode => this.mode;

        // ======================================= //

        private Texture2D texture;
        private Rectangle frame;

        private SAnimationMode mode;

        private float animationDelay = 1f;
        private float animationCurrentDelay = 0f;

        private readonly List<Rectangle> animationFrames = new();
        private int animationCurrentFrame = 0;

        // ======================================= //
        // [ EVENTS ]

        /// <summary>
        /// Event is triggered whenever the current frame reaches its maximum and returns to the initial frame of the animation.
        /// </summary>
        public event AnimationFinished OnAnimationFinished;
        public delegate void AnimationFinished();

        /// <summary>
        /// Initializes the initial components of the animation.
        /// </summary>
        public void Initialize()
        {
            if (this.animationFrames.Count > 0)
            {
                this.frame = this.animationFrames[0];
            }
        }

        /// <summary>
        /// Updates the animation frame based on the current mode and timing.
        /// </summary>
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
                    this.OnAnimationFinished?.Invoke();

                    if (this.mode == SAnimationMode.Once)
                    {
                        this.mode = SAnimationMode.Disable;
                        return;
                    }
                }
            }

            this.frame = this.animationFrames[this.animationCurrentFrame];
        }

        /// <summary>
        /// Resets all animation components to their initial state.
        /// </summary>
        public void Reset()
        {
            this.mode = SAnimationMode.Disable;

            this.animationDelay = 1f;
            this.animationCurrentDelay = 0f;

            this.animationCurrentFrame = 0;
        }

        /// <summary>
        /// Clears all registered frames in the animation.
        /// </summary>
        public void ClearFrames()
        {
            this.animationFrames.Clear();
        }

        /// <summary>
        /// Sets the texture for the animation.
        /// </summary>
        /// <param name="texture">The texture to be used for the animation.</param>
        public void SetTexture(Texture2D texture)
        {
            this.texture = texture;
        }

        /// <summary>
        /// Sets the animation mode.
        /// </summary>
        /// <param name="mode">The animation playback mode.</param>
        public void SetMode(SAnimationMode mode)
        {
            this.mode = mode;
        }

        /// <summary>
        /// Sets the duration of each frame in the animation.
        /// </summary>
        /// <param name="delay">The delay (in seconds) between frames.</param>
        public void SetDuration(float delay)
        {
            this.animationDelay = delay;
        }

        /// <summary>
        /// Sets the current frame to be displayed in the animation.
        /// </summary>
        /// <param name="frame">The index of the frame to be set as the current frame.</param>
        public void SetCurrentFrame(int frame)
        {
            this.animationCurrentFrame = Math.Clamp(frame, 0, this.animationFrames.Count - 1);
        }

        /// <summary>
        /// Adds a new frame to the animation.
        /// </summary>
        /// <param name="rect">The rectangle defining the frame within the texture.</param>
        public void AddFrame(Rectangle rect)
        {
            this.animationFrames.Add(rect);
        }

        /// <summary>
        /// Checks if the animation is empty (has no texture or rectangle).
        /// </summary>
        /// <returns>True if the animation is empty, false otherwise.</returns>
        internal bool IsEmpty()
        {
            return this.Texture == null || this.frame.IsEmpty;
        }
    }
}
