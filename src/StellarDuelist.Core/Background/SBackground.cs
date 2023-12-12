using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StellarDuelist.Core.Background.Layers;
using StellarDuelist.Core.Controllers;

using System.Collections.Generic;

namespace StellarDuelist.Core.Background
{
    /// <summary>
    /// Base class for building backgrounds.
    /// </summary>
    public abstract class SBackground
    {
        private readonly List<SBackgroundLayer> layers = new();
        private Texture2D texture;

        /// <summary>
        /// Initializes the initial processing of fundamental information for constructing background components.
        /// </summary>
        internal void Initialize()
        {
            OnProcess();
        }

        /// <summary>
        /// Updates all layers individually in the current background.
        /// </summary>
        internal void Update()
        {
            this.layers.ForEach(x => x.Update());
        }

        /// <summary>
        /// Renders all layers individually and in order in the current background.
        /// </summary>
        internal void Draw()
        {
            this.layers.ForEach(x => x.Draw());
        }

        /// <summary>
        /// Sets the default texture to be used by the current background.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         The texture must be a logical SpriteSheet containing all the parts that will be used and divided into <see cref="SBackgroundLayer"/> in the current background.
        ///     </para>
        ///     <para>
        ///         By default, each piece of the background texture must have dimensions of 640x360 pixels in width and height, for the parallax effects to be applied correctly.
        ///     </para>
        ///     <para>
        ///         The parallax effect/factor applied to the layers of the current background is strictly vertical.
        ///     </para>
        /// </remarks>
        /// <param name="texture">The texture to set as the default for the current background.</param>
        protected void SetTexture(Texture2D texture)
        {
            this.texture = texture;
        }

        /// <summary>
        /// Adds a layer to the current background.
        /// </summary>
        /// <remarks>
        /// A layer refers to the texture defined in the current background. It configures the anchor position and the reference dimensions that divide the current texture into that specific piece.
        /// </remarks>
        /// <param name="textureRectangle">The size and position of the piece of the texture that will be used by this layer.</param>
        /// <param name="parallaxFactor">The parallax factor that this layer will have.</param>
        protected void AddLayer(Rectangle textureRectangle, float parallaxFactor)
        {
            this.layers.Add(new(this.texture, textureRectangle, parallaxFactor));
        }

        /// <summary>
        /// Should process and build the initial components of the current background.
        /// </summary>
        /// <remarks>
        /// It is invoked as soon as the current background enters the construction queue and is called for instantiation in <see cref="SBackgroundController"/>.
        /// </remarks>
        protected abstract void OnProcess();
    }
}
